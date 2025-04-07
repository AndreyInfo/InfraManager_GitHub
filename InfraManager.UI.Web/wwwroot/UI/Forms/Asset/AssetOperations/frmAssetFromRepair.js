define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/SDForms/SDForm.LinkList', 'models/AssetForms/SdEntityList', 'dateTimeControl', 'fileControl', 'jqueryStepper'], function (ko, $, ajaxLib, fc, fhModule, tclib, linkListLib, callReferenceListLib, dtLib, fcLib) {
    var module = {
        MaxPrice: 1000000000,
        ViewModel: function ($region, selectedObjects) {
            var self = this;
            self.$region = $region;
            self.selectedObjects = selectedObjects;
            self.LifeCycleStateOperationID = null;//context
            //
            self.mode = ko.observable();
            self.RepairType = ko.observable('');
            self.RepairQuality = ko.observable('');
            self.Agreement = ko.observable('');
            self.Cost = ko.observable(getFormattedMoneyString('0'));
            //
            self.$isDone = $.Deferred();//resolve, когда операция выполнена
            //
            {//tabs
                self.modes = {
                    main: 'main',
                    links: 'links'
                };
                //
                self.mode = ko.observable();
                self.mode.subscribe(function (newValue) {
                    if (newValue == self.modes.links)
                        self.linkList.CheckListData();
                });
                //
                self.mainClick = function () {
                    self.mode(self.modes.main);
                };
                self.linksClick = function () {
                    self.mode(self.modes.links);
                };
                //
                self.TabLinksCaption = ko.observable('');
                //
                self.LinkIDList = [];
                var canEdit = ko.observable(true);
                var isReadOnly = ko.observable(false);
                self.linkList = new linkListLib.LinkList(null, null, self.$region.find('.links__b .tabContent').selector, isReadOnly, canEdit, self.LinkIDList);
            }
            //
            self.Load = function () {
                self.mode(self.modes.main);
                //
                if (self.selectedObjects)
                    self.selectedObjects.forEach(function (el) {
                        self.LinkIDList.push(el.ID);
                    });
                //
                self.TabLinksCaption(getTextResource('AssetLinkHeaderChoosen') + ' (' + self.LinkIDList.length + ')');
                //
                self.InitializeStartLocation();
            };
            //
            self.GetDecimalFromMoneyString = function (moneyStr) {
                var retval = parseFloat(moneyStr.split(' ').join('').split(' ').join('').replace(',', '.'));//hack
                return retval;
            };
            //
            self.HasEnteredFields = function () {
                if (self.LocationID || self.periodEndDateTime() || self.RepairType() || self.RepairQuality() || self.Agreement() || self.GetDecimalFromMoneyString(self.Cost()) || self.FoundingNumberStr())
                    return true;
                return false;
            };
            //
            self.getDeviceList = function () {
                var retVal = [];
                self.selectedObjects.forEach(function (el) {
                    var item =
                    {
                        ID: el.ID,
                        ClassID: el.ClassID
                    };
                    //
                    retVal.push(item);
                });
                return retVal;
            };
            //
            self.ajaxControlRepair = new ajaxLib.control();
            self.AssetFromRepair = function () {
                if (!self.LocationID) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('AssetLocationNotSet'));
                    });
                    return;
                }
                //
                var data =
                {
                    'DeviceList': self.getDeviceList(),
                    'LocationID': self.LocationID,
                    'LocationClassID': self.LocationClassID,
                    'LocationFullName': self.LocationFullName(),
                    'RepairType': self.RepairType(),
                    'RepairQuality': self.RepairQuality(),
                    'Agreement': self.Agreement(),
                    'Cost': self.GetDecimalFromMoneyString(self.Cost()),
                    'DateEnd': dtLib.GetMillisecondsSince1970(self.periodEndDateTime()),
                    'ReasonNumber': self.FoundingNumberStr(),
                    'LifeCycleStateOperationID': self.LifeCycleStateOperationID,
                    'PrintReport': self.printReport()
                }
                self.ajaxControlRepair.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/assetApi/AssetFromRepair'
                    },
                    function (newVal) {
                        if (newVal) {
                            if (newVal.Result === 0) {
                                self.$isDone.resolve(true);
                                //
                                ko.utils.arrayForEach(self.selectedObjects, function (el) {
                                    $(document).trigger('local_objectUpdated', [el.ClassID, el.ID]);
                                });
                                //
                                var succsess = true;
                                if (self.printReport()) {
                                    if (newVal.PrintReportResult === 2)//no report
                                    {
                                        succsess = false;
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('AssetFromRepair_Success'), getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoReport'), 'info');
                                        });
                                    }
                                    else if (newVal.PrintReportResult === 3)//no ID parameter
                                    {
                                        succsess = false;
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('AssetFromRepair_Success'), getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoParam'), 'info');
                                        });
                                    }
                                    else {
                                        if (newVal.FileInfoList != null) {
                                            var reportControl = new fcLib.control();
                                            newVal.FileInfoList.forEach(function (el) {
                                                var item = new reportControl.CreateItem(el.ID, el.ObjectID, el.FileName, '', '', '', 'pdf');
                                                reportControl.DownloadFile(item);
                                            });
                                        }
                                    }
                                }
                                else if (newVal.Error) {
                                    succsess = false;
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), Error, 'error');
                                    });
                                }
                                //
                                if (succsess)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('AssetFromRepair_Success'));
                                    });
                            }
                            else {
                                self.$isDone.resolve(true);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AssetOperation_Error'), 'error');
                                });
                            }
                        }
                    });
                //
                return self.$isDone.promise();
            };
            //
            //
            self.SizeChanged = function () {

            };
            //
            self.controlEnd = ko.observable(null);
            //
            self.showOnlyDate = true;
            self.dateNow = Date.now();
            self.periodEnd = ko.observable(parseDate(self.dateNow, self.showOnlyDate));//always local string
            self.periodEnd.subscribe(function (newValue) {
                var dt = self.controlEnd().dtpControl.length > 0 ? self.controlEnd().dtpControl.datetimepicker('getValue') : null;
                //
                if (!newValue || newValue.length == 0)
                    self.periodEndDateTime(null);//clear field => reset value
                else if (dtLib.Date2String(dt, self.showOnlyDate) != newValue) {
                    self.periodEndDateTime(null);//value incorrect => reset value
                    self.periodEnd('');
                }
                else
                    self.periodEndDateTime(dt);
            });
            self.periodEndDateTime = ko.observable(new Date(parseInt(self.dateNow)));//always dateTime, auto convert serverUtcDateString to jsLocalTime
            //
            self.InitDtp = function (dtpClass, dateTimeStr, dateTime, control) {
                var dtp = self.$region.find(dtpClass);
                var ctrl = new dtLib.control();
                ctrl.init(dtp, {
                    StringObservable: dateTimeStr,
                    ValueDate: dateTime(),
                    OnSelectDateFunc: function (current_time, $input) {
                        dateTime(current_time);
                        dateTimeStr(dtLib.Date2String(current_time, self.showOnlyDate));
                    },
                    OnSelectTimeFunc: function (current_time, $input) {
                        dateTime(current_time);
                        dateTimeStr(dtLib.Date2String(current_time, self.showOnlyDate));
                    },
                    HeaderText: '',
                    OnlyDate: self.showOnlyDate
                });
                control(ctrl);
            };
            //
            {//report
                self.printReport = ko.observable(false);

                self.printReportClick = function (data, e) {
                    var checkbox = $('.reportPrint-checkbox')[0];
                    var checked = checkbox.checked;
                    checkbox.checked = !checked;
                    self.printReport(checkbox.checked);
                };
            }
            //
            self.AfterRender = function () {
                self.InitDtp('.repair-date-end', self.periodEnd, self.periodEndDateTime, self.controlEnd);
                //
                self.InitializeLocationSearcher();
                self.InitializeStepper();
                //
                $region.find('.repair-date-end').focus();
            };
            //
            self.InitializeStepper = function () {
                var $input = self.$region.find('.assetRepair-cause.cost');
                $input.stepper({
                    type: 'float',
                    floatPrecission: 2,
                    wheelStep: 1,
                    arrowStep: 1,
                    limit: [0, module.MaxPrice],
                    onStep: function (val, up) {
                        self.Cost(val);
                    }
                });
            };
            //
            self.ContextMenuVisible = ko.observable(false);
            //
            self.LinkSdObjectClick = function (data, e) {
                var isVisible = self.ContextMenuVisible();
                self.ContextMenuVisible(!isVisible);
                //
                e.stopPropagation();
            };
            //
            self.formClick = function () {
                self.ContextMenuVisible(false);
            };
            //
            self.FoundingNumberStr = ko.observable(null);
            //
            self.LinkCall = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSearcherLite([701], null, null, null, self.FoundingNumberStr);
                });
            };
            //
            self.LinkWorkorder = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSearcherLite([119], null, null, null, self.FoundingNumberStr);
                });
            };
            //
            self.LinkProblem = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSearcherLite([702], null, null, null, self.FoundingNumberStr);
                });
            };
            //
            {//location
                self.LocationClassID = null;
                self.LocationID = null;
                self.LocationFullName = ko.observable('');
                //
                self.locationSearcher = null;
                self.locationSearcherD = $.Deferred();
                self.InitializeLocationSearcher = function () {
                    var fh = new fhModule.formHelper();
                    var locationD = fh.SetTextSearcherToField(
                        $region.find('.assetReg-location'),
                        'AssetLocationSearcher',//not only room! rack and workplace can be
                        null,
                        ['true', 'false', 'false', 'false', null],//ShowRoom, ShowRack, ShowWorkplace, ShowDevice, storageID
                        function (objectInfo) {//select
                            self.LocationClassID = objectInfo.ClassID;
                            self.LocationID = objectInfo.ID;
                            self.LocationFullName(objectInfo.FullName);
                        },
                        function () {//reset
                            self.LocationClassID = null;
                            self.LocationID = null;
                            self.LocationFullName('');
                        });
                    $.when(locationD).done(function (ctrl) {
                        self.locationSearcher = ctrl;
                        self.locationSearcherD.resolve(ctrl);
                        ctrl.CurrentUserID = null;
                    });
                };
                //
                self.InitializeStartLocation = function () {
                    var firstObj = self.selectedObjects[0];
                    //
                    var ajaxControl = new ajaxLib.control();
                    var data = { 'ID': firstObj.ID, 'ClassID': firstObj.ClassID }
                    ajaxControl.Ajax(self.$region.find('.assetReg-locationControl'),
                        {
                            dataType: "json",
                            method: 'GET',
                            data: data,
                            url: '/sdApi/GetObjectLocation'
                        },
                        function (newVal) {
                            if (newVal) {
                                if (newVal.Result == 0) {//success
                                    self.LocationClassID = newVal.ClassID;
                                    self.LocationID = newVal.LocationID;
                                    //
                                    var params = { 'objectID': self.LocationID, 'objectClassID': self.LocationClassID }
                                    ajaxControl.Ajax(self.$region.find('.assetReg-locationControl'),
                                        {
                                            url: '/searchApi/getObjectFullName?' + $.param(params),
                                            method: 'GET'
                                        },
                                        function (objectFullName) {
                                            self.LocationFullName(objectFullName.result);
                                        },
                                        function () {//object not found => not exists
                                            self.LocationID = null;
                                            self.LocationClassID = null;
                                            self.LocationFullName('');
                                        });
                                }
                            }
                        },
                        function () {
                            self.LocationClassID = null;
                            self.LocationID = null;
                        });
                };
            }
        },
        ShowDialog: function (selectedObjects, operationName, lifeCycleStateOperationID, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            var $retval = $.Deferred();
            //
            $.when(userD).done(function (user) {
                var isReadOnly = false;
                var forceClose = false;
                //
                if (user.HasRoles == false)
                    isReadOnly = true;
                //
                var frm = undefined;
                var vm = undefined;
                //
                var buttons = {}
                buttons[operationName] = function () {
                    var d = vm.AssetFromRepair();
                    $.when(d).done(function (result) {
                        if (!result)
                            return;
                        //
                        forceClose = true;
                        frm.Close();
                    });
                };
                //
                frm = new fc.control(
                    'region_assetFromRepairForm',//form region prefix
                    'setting_assetFromRepairForm',//location and size setting
                    operationName,//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    600, 590,//minSize
                    buttons,//form buttons
                    null,//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmAssetFromRepair\', afterRender: AfterRender}"'//attributes of form region
                );
                //
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                //
                frm.BeforeClose = function () {
                    var retval = forceClose;
                    //
                    if (retval == false) {
                        if (!vm.HasEnteredFields())
                            retval = true;
                        else
                            require(['sweetAlert'], function () {
                                swal({
                                    title: getTextResource('FormClosingQuestion'),
                                    showCancelButton: true,
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                    function (value) {
                                        if (value == true) {
                                            forceClose = true;
                                            setTimeout(function () {
                                                frm.Close();
                                            }, 300);//TODO? close event of swal
                                        }
                                    });
                            });
                    }
                    //
                    return retval;
                };
                //
                var $region = $('#' + frm.GetRegionID());
                vm = new module.ViewModel($region, selectedObjects);
                vm.LifeCycleStateOperationID = lifeCycleStateOperationID;
                vm.$isDone = $retval;
                vm.Load();
                //
                frm.SizeChanged = function () {
                    var width = frm.GetInnerWidth();
                    var height = frm.GetInnerHeight();
                    //
                    vm.$region.css('width', width + 'px').css('height', height + 'px');
                    vm.SizeChanged();
                };
                //
                ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                $.when(frm.Show(), vm.LoadD).done(function (frmD, loadD) {
                    if (loadD == false) {//force close
                        frm.Close();
                    }
                    hideSpinner();
                });
            });
            //
            return $retval.promise();
        }
    }
    return module;
});