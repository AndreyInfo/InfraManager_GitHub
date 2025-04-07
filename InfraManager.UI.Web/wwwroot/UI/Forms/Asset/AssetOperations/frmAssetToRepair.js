define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/SDForms/SDForm.LinkList', 'models/AssetForms/SdEntityList', 'dateTimeControl', 'fileControl'], function (ko, $, ajaxLib, fc, fhModule, tclib, linkListLib, callReferenceListLib, dtLib, fcLib) {
    var module = {
        ViewModel: function ($region, selectedObjects) {
            var self = this;
            self.$region = $region;
            self.selectedObjects = selectedObjects;
            self.LifeCycleStateOperationID = null;//context
            //
            self.mode = ko.observable();
            self.Problems = ko.observable('');
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
                self.selectedObjects.forEach(function (el) {
                    self.LinkIDList.push(el.ID);
                });
                //
                self.TabLinksCaption(getTextResource('AssetLinkHeaderChoosen') + ' (' + self.LinkIDList.length + ')');
                //
                self.InitializeStartLocation();
                //
                self.ResetAdaptersCbVisibility();
                self.ResetPeripheralsCbVisibility();
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
            self.HasEnteredFields = function () {
                if (self.LocationID || self.periodStartDateTime() || self.periodEndDateTime() || self.Problem() || self.selectedServiceItem() || self.FoundingNumberStr())
                    return true;
                return false;
            };
            //
            self.ajaxControlRepair = new ajaxLib.control();
            self.AssetToRepair = function () {
                var dateStart = dtLib.GetMillisecondsSince1970(self.periodStartDateTime());
                var dateEnd = dtLib.GetMillisecondsSince1970(self.periodEndDateTime());
                //
                if (!self.periodStartDateTime()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('StartDateMustBeSet'));
                    });
                    return;
                }
                //
                if (dateStart > dateEnd) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('AssetToRepairCheckDate'));
                    });
                    return;
                }
                //
                if (self.LocationID == null) {
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
                    'Problems': self.Problems(),
                    'ServiceCenterID': self.selectedServiceItem() ? self.selectedServiceItem().ID : null,
                    'ServiceContractID': self.selectedContractItem() ? self.selectedContractItem().ID : null,
                    'DateStart': dtLib.GetMillisecondsSince1970(self.periodStartDateTime()),
                    'DateAnticipated': dtLib.GetMillisecondsSince1970(self.periodEndDateTime()),
                    'ReasonNumber': self.FoundingNumberStr(),
                    'LifeCycleStateOperationID': self.LifeCycleStateOperationID,
                    'PrintReport': self.printReport(),
                    'WithAdapters': self.withAdapters(),
                    'withPeripherals': self.withPeripherals()
                }
                self.ajaxControlRepair.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/assetApi/AssetToRepair'
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
                                            swal(getTextResource('AssetToRepair_Success'), getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoReport'), 'info');
                                        });
                                    }
                                    else if (newVal.PrintReportResult === 3)//no ID parameter
                                    {
                                        succsess = false;
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('AssetToRepair_Success'), getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoParam'), 'info');
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
                                //
                                if (succsess)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('AssetToRepair_Success'));
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
            self.controlStart = ko.observable(null);
            self.controlEnd = ko.observable(null);
            self.showOnlyDate = true;
            self.dateNow = Date.now();
            //
            self.periodStart = ko.observable(parseDate(self.dateNow, self.showOnlyDate));//always local string
            self.periodStart.subscribe(function (newValue) {
                var dt = self.controlStart().dtpControl.length > 0 ? self.controlStart().dtpControl.datetimepicker('getValue') : null;
                //
                if (!newValue || newValue.length == 0)
                    self.periodStartDateTime(null);//clear field => reset value
                else if (dtLib.Date2String(dt, self.showOnlyDate) != newValue) {
                    self.periodStartDateTime(null);//value incorrect => reset value
                    self.periodStart('');
                }
                else
                    self.periodStartDateTime(dt);
            });
            self.periodStartDateTime = ko.observable(new Date(parseInt(self.dateNow)));//always dateTime, auto convert serverUtcDateString to jsLocalTime
            //
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
            {//withAdapters
                self.withAdaptersVisible = ko.observable(false);
                self.withAdapters = ko.observable(false);
                //
                self.ResetAdaptersCbVisibility = function () {
                    self.selectedObjects.forEach(function (el) {
                        if (el.ClassID == 5 || el.ClassID == 6) {
                            self.withAdaptersVisible(true);
                            return;
                        }
                    });
                };
                //
                self.withAdaptersClick = function (data, e) {
                    var checkbox = $('.withAdapters-checkbox')[0];
                    var checked = checkbox.checked;
                    checkbox.checked = !checked;
                    self.withAdapters(checkbox.checked);
                };
            }
            //
            {//withPeripherals
                self.withPeripheralsVisible = ko.observable(false);
                self.withPeripherals = ko.observable(false);
                //
                self.ResetPeripheralsCbVisibility = function () {
                    self.selectedObjects.forEach(function (el) {
                        if (el.ClassID == 5 || el.ClassID == 6) {
                            self.withPeripheralsVisible(true);
                            return;
                        }
                    });
                };
                //
                self.withPeripheralsClick = function (data, e) {
                    var checkbox = $('.withPeripherals-checkbox')[0];
                    var checked = checkbox.checked;
                    checkbox.checked = !checked;
                    self.withPeripherals(checkbox.checked);
                };
            }
            //
            self.AfterRender = function () {
                self.InitDtp('.repair-date-start', self.periodStart, self.periodStartDateTime, self.controlStart);
                self.InitDtp('.repair-date-end', self.periodEnd, self.periodEndDateTime, self.controlEnd);
                //
                self.GetServiceCenterList();
                self.InitializeLocationSearcher();
                //
                $region.find('.assetRepair-cause').focus();
            };
            //
            self.selectedServiceItem = ko.observable(null);
            self.serviceCentrComboItems = ko.observableArray([]);
            self.getServiceCentrComboItems = function () {
                return {
                    data: self.serviceCentrComboItems(),
                    totalCount: self.serviceCentrComboItems().length
                };
            };
            //
            self.selectedServiceItem.subscribe(function (newValue) {
                self.GetServiceContractList();
            });
            //
            self.ajaxControlServiceCenter = new ajaxLib.control();
            self.GetServiceCenterList = function () {
                self.ajaxControlServiceCenter.Ajax($region.find('.service-center-combobox'),
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/api/suppliers?orderByProperty=Name'
                    },
                    function (result) {
                        if (result) {
                            var selEl = null;
                            result.forEach(function (el) {
                                self.serviceCentrComboItems().push(el);
                            });
                            self.serviceCentrComboItems.valueHasMutated();
                            self.selectedServiceItem(selEl);
                        }
                    });
            };
            //
            self.serviceContractEnabled = ko.computed(function () {
                return self.selectedServiceItem() != null;
            });
            self.selectedContractItem = ko.observable(null);
            self.serviceContractComboItems = ko.observableArray([]);
            self.getServiceContractComboItems = function () {
                return {
                    data: self.serviceContractComboItems(),
                    totalCount: self.serviceContractComboItems().length
                };
            };
            //
            self.ajaxControlServiceContract = new ajaxLib.control();
            self.GetServiceContractList = function () {
                var data =
                {
                    'ServiceCenterID': self.selectedServiceItem().ID
                };
                //
                self.ajaxControlServiceContract.Ajax($region.find('.service-contract-combobox'),
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/sdApi/GetServiceContractList'
                    },
                    function (result) {
                        if (result && result.List) {
                            var selEl = null;
                            self.serviceContractComboItems.removeAll();
                            //
                            result.List.forEach(function (el) {

                                self.serviceContractComboItems().push(el);
                            });
                            self.serviceContractComboItems.valueHasMutated();
                            self.selectedContractItem(selEl);
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
            self.formClick = function (data, e) {
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
                        ['true', 'false', 'false', 'false', null],//ShowRoom, ShowRack, ShowWorkplace, ShowDevice, StorageID
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
                    var data = { 'ID': firstObj.ID, 'ClassID': firstObj.ClassID, 'OperationType': 2 /*ToRepair*/ }
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
                buttons[getTextResource('AssetToRepair')] = function () {
                    var d = vm.AssetToRepair();
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
                    'region_assetToRepairForm',//form region prefix
                    'setting_assetToRepairForm',//location and size setting
                    operationName,//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    716, 590,//minSize
                    buttons,//form buttons
                    null,//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmAssetToRepair\', afterRender: AfterRender}"'//attributes of form region
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