define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/SDForms/SDForm.LinkList', 'models/AssetForms/SdEntityList', 'dateTimeControl', 'models/SDForms/SDForm.User', 'fileControl'], function (ko, $, ajaxLib, fc, fhModule, tclib, linkListLib, callReferenceListLib, dtLib, userLib, fcLib) {
    var module = {
        ViewModel: function ($region, selectedObjects) {
            var self = this;
            self.$region = $region;
            self.selectedObjects = selectedObjects;
            self.LifeCycleStateOperationID = null;//context
            //
            self.mode = ko.observable();
            self.Founding = ko.observable('');
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
                $.when(userD).done(function (user) {
                    self.UserID(user.UserID);
                    self.InitializeUser();
                });
                //
                self.ResetAdaptersCbVisibility();
                self.ResetPeripheralsCbVisibility();
                //
                self.HasLogicalDevice(self.ObjectsHasLogicalDevice());
            };
            //
            self.ObjectsHasLogicalDevice = function () {
                for (var i = 0; i < self.selectedObjects.length; i++)
                    if (self.selectedObjects[i].IsLogical)
                        return true;
                return false;
            };
            self.HasLogicalDevice = ko.observable(false);
            //
            self.HasEnteredFields = function () {
                if (self.periodEndDateTime() || self.Founding() || self.FoundingNumberStr())
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
            self.ajaxControlWriteOff = new ajaxLib.control();
            self.AssetWriteOff = function () {
                if (!self.UserID())
                {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('MustSetUser'));
                    });
                    return;
                }
                //
                if (!self.periodEndDateTime()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('AssetOff_MustSetDate'));
                    });
                    return;
                }
                //
                var data =
                    {
                        'DeviceList': self.getDeviceList(),
                        'DateOff': dtLib.GetMillisecondsSince1970(self.periodEndDateTime()),
                        'Founding': self.Founding(),
                        'UserID': self.UserID(),
                        'ReasonNumber': self.FoundingNumberStr(),
                        'PrintReport': self.printReport(),
                        'LifeCycleStateOperationID': self.LifeCycleStateOperationID,
                        'WithAdapters': self.withAdapters(),
                        'withPeripherals': self.withPeripherals()
                    }
                self.ajaxControlWriteOff.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/assetApi/AssetWriteOff'
                    },
                    function (newVal) {
                        if (newVal) {
                            if (newVal.Result === 0) {
                                self.$isDone.resolve(true);
                                //
                                var message = getTextResource('AssetWriteOff_Succsess');

                                ko.utils.arrayForEach(self.selectedObjects, function (el) {
                                    if (el.ClassID === 223) {
                                        message = getTextResource('LicenseWriteOff_Succsess');
                                    }
                                    $(document).trigger('local_objectUpdated', [el.ClassID, el.ID]);
                                    
                                });
                                //
                                var succsess = true;
                                if (self.printReport()) {
                                    if (newVal.PrintReportResult === 2)//no report
                                    {
                                        succsess = false;
                                        require(['sweetAlert'], function () {
                                            swal(message, getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoReport'), 'info');
                                        });
                                    }
                                    else if (newVal.PrintReportResult === 3)//no ID parameter
                                    {
                                        succsess = false;
                                        require(['sweetAlert'], function () {
                                            swal(message, getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoParam'), 'info');
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
                                        swal(message);
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
            self.dateNow = Date.now();
            self.showOnlyDate = true;
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
                    if (self.HasLogicalDevice())
                        return;
                    //
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
                    if (self.HasLogicalDevice())
                        return;
                    //
                    var checkbox = $('.withPeripherals-checkbox')[0];
                    var checked = checkbox.checked;
                    checkbox.checked = !checked;
                    self.withPeripherals(checkbox.checked);
                };
            }
            //
            self.AfterRender = function () {
                self.InitDtp('.writeOff-date', self.periodEnd, self.periodEndDateTime, self.controlEnd);
                //
                $region.find('.writeOff-date').focus();
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
            {//user          
                self.CanEdit = ko.observable(true);
                self.IsReadOnly = ko.observable(false);
                //
                self.UserID = ko.observable(null);
                self.UserLoaded = ko.observable(false);
                //
                self.EditUser = function () {
                    showSpinner();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            fieldFriendlyName: getTextResource('WrittenOff_UserNameOff'),
                            oldValue: self.UserLoaded() ? { ID: self.User().ID(), ClassID: self.User().ClassID(), FullName: self.User().FullName() } : null,
                            object: ko.toJS(self.User()),
                            searcherName: 'WebUserSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [],
                            nosave: true,
                            onSave: function (objectInfo) {
                                self.UserLoaded(false);
                                self.User(new userLib.EmptyUser(self, userLib.UserTypes.writerOff, self.EditUser));
                                if (!objectInfo)
                                    self.UserID(null);
                                else
                                    self.UserID(objectInfo.ID);
                                //
                                self.InitializeUser();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.User = ko.observable(new userLib.EmptyUser(self, userLib.UserTypes.writerOff, self.EditUser));
                //
                self.InitializeUser = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.UserLoaded() == false) {
                            if (self.UserID()) {
                                var options = {
                                    UserID: self.UserID(),
                                    UserType: userLib.UserTypes.writerOff,
                                    UserName: null,
                                    EditAction: self.EditUser,
                                };
                                var user = new userLib.User(self, options);
                                self.User(user);
                                self.UserLoaded(true);
                            }
                        }
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
                    var d = vm.AssetWriteOff();
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
                    'region_assetOffForm',//form region prefix
                    'setting_assetOffForm',//location and size setting
                    operationName,//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    600, 510,//minSize
                    buttons,//form buttons
                    null,//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmAssetOff\', afterRender: AfterRender}"'//attributes of form region
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