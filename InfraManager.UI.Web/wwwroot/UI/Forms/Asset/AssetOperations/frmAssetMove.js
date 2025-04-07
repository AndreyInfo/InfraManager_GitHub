define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/SDForms/SDForm.LinkList', 'models/AssetForms/SdEntityList', 'dateTimeControl', 'models/SDForms/SDForm.User', 'fileControl', 'treeControl'], function (ko, $, ajaxLib, fc, fhModule, tclib, linkListLib, callReferenceListLib, dtLib, userLib, fcLib, treeLib) {
    var module = {
        ViewModel: function ($region, selectedObjects) {
            var self = this;
            self.$region = $region;
            self.forceFrmClose = null;
            self.selectedObjects = selectedObjects;
            self.LifeCycleStateOperationID = null;//context
            self.OperationType = null;//Move or FromStorage
            self.AvailableClassID = null;//FromStorage
            //
            self.$isDone = $.Deferred();//resolve, когда операция выполнена
            //
            self.mode = ko.observable();
            self.Founding = ko.observable('');
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
                if (self.OperationType != 7 && self.selectedObjects)
                    self.selectedObjects.forEach(function (el) {
                        self.LinkIDList.push(el.ID);
                    });
                //
                self.TabLinksCaption(getTextResource('AssetLinkHeaderChoosen') + ' (' + self.LinkIDList.length + ')');
                //
                if (self.OperationType != 8)//toStorage
                {
                    var asset = self.selectedObjects[0];
                    self.UtilizerID(asset.UtilizerID);
                    self.UtilizerClassID(asset.UtilizerClassID);
                }
                else {
                    self.UtilizerID(null);
                    self.UtilizerClassID(null);
                }
                //
                self.InitializeUtilizer();
                //
                self.InitializeStartLocation();
            };

            //
            self.HasEnteredFields = function () {
                if (self.LocationID || self.UtilizerID() || self.FoundingNumberStr())
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
            self.ajaxControlMove = new ajaxLib.control();
            self.AssetMove = function () {
                var data = {};
                if (!self.LocationID || !self.LocationClassID) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('AssetLocationNotSet'));
                    });
                    return;
                }
                //
                data =
                {
                    'DeviceList': self.getDeviceList(),
                    'LocationID': self.LocationID,
                    'LocationClassID': self.LocationClassID,
                    'UtilizerID': self.UtilizerID(),
                    'UtilizerClassID': self.UtilizerClassID(),
                    'UtilizerFullName': self.UtilizerFullName(),
                    'ReasonNumber': self.FoundingNumberStr(),
                    'PrintReport': self.printReport(),
                    'LifeCycleStateOperationID': self.LifeCycleStateOperationID,
                }
                //
                self.ajaxControlMove.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/assetApi/AssetMove'
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
                                            swal(getTextResource('AssetMove_Succsess'), getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoReport'), 'info');
                                        });
                                    }
                                    else if (newVal.PrintReportResult === 3)//no ID parameter
                                    {
                                        succsess = false;
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('AssetMove_Succsess'), getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoParam'), 'info');
                                        });
                                    }
                                    else if (newVal.PrintReportResult === 4)//DifferentAssetOperations
                                    {
                                        succsess = false;
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('AssetMove_Succsess'), getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_DifferentAssetOperations'), 'info');
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
                                        swal(getTextResource('AssetMove_Succsess'));
                                    });
                            }
                            else {
                                self.$isDone.resolve(true);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), newVal.Error, 'error');
                                });
                            }
                        }
                    });
                //
                return self.$isDone.promise();
            };
            //
            self.SizeChanged = function () {

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
                if (self.OperationType == 7) {//"Установить в"
                    self.SearchSubdeviceShow();
                }
                else {
                    self.InitializeLocationSearcher();
                }
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
            {//utilizer          
                self.CanEdit = ko.observable(true);
                self.IsReadOnly = ko.observable(false);
                //
                self.UtilizerID = ko.observable(null);
                self.UtilizerClassID = ko.observable(null);
                self.UtilizerFullName = ko.observable(null);
                self.UserLoaded = ko.observable(false);
                //
                self.EditUtilizer = function () {
                    showSpinner();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            fieldFriendlyName: getTextResource('AssetNumber_UtilizerName'),
                            oldValue: self.UserLoaded() ? { ID: self.User().ID(), ClassID: self.User().ClassID(), FullName: self.User().FullName() } : null,
                            object: ko.toJS(self.User()),
                            searcherName: 'UtilizerSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [],
                            nosave: true,
                            onSave: function (objectInfo) {
                                self.UserLoaded(false);
                                self.User(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditUtilizer));
                                if (!objectInfo) {
                                    self.UtilizerID(null);
                                    self.UtilizerClassID(null);
                                    self.UtilizerFullName(null);
                                }
                                else {
                                    self.UtilizerID(objectInfo.ID);
                                    self.UtilizerClassID(objectInfo ? objectInfo.ClassID : '');
                                    self.UtilizerFullName(objectInfo ? objectInfo.FullName : '');
                                }
                                //
                                self.InitializeUtilizer();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.User = ko.observable(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditUtilizer));
                //
                self.InitializeUtilizer = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.UserLoaded() == false && self.UtilizerID()) {
                            var type = null;
                            if (self.UtilizerClassID() == 9) {//IMSystem.Global.OBJ_USER
                                type = userLib.UserTypes.utilizer;
                            }
                            else if (self.UtilizerClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                                type = userLib.UserTypes.queueExecutor;
                            }
                            else if (self.UtilizerClassID() == 101) {//IMSystem.Global.OBJ_ORGANIZATION
                                type = userLib.UserTypes.organization;
                            }
                            else if (self.UtilizerClassID() == 102) {//IMSystem.Global.OBJ_DIVISION
                                type = userLib.UserTypes.subdivision;
                            }
                            //
                            var options = {
                                UserID: self.UtilizerID(),
                                UserType: type,
                                UserName: null,
                                EditAction: self.EditUtilizer,
                                ForceTypeName: getTextResource('AssetNumber_UtilizerName')
                            };
                            var user = new userLib.User(self, options);
                            self.User(user);
                            self.UserLoaded(true);
                        }
                    });
                };
            }
            //
            {//location
                self.LocationClassID = null;
                self.LocationID = null;
                self.LocationFullName = ko.observable('');
                //
                self.ObjectsHasOnlyAdapterOrPeripheral = ko.computed(function () {
                    for (var i = 0; i < self.selectedObjects.length; i++) {
                        var classID = self.selectedObjects[i].ClassID;
                        if (classID != 33 && classID != 34)//OBJ_Adapter, OBJ_Peripheral
                            return false;
                    }
                    return true;
                });
                self.ObjectsHasOnlyTerminalDevice = ko.computed(function () {
                    for (var i = 0; i < self.selectedObjects.length; i++)
                        if (self.selectedObjects[i].ClassID != 6)//OBJ_TerminalDevice
                            return false;
                    return true;
                });
                self.ObjectsHasOnlyNetworkDevice = ko.computed(function () {
                    for (var i = 0; i < self.selectedObjects.length; i++)
                        if (self.selectedObjects[i].ClassID != 5)//OBJ_NetworkDevice
                            return false;
                    return true;
                });
                //
                self.getSearcherParams = function () {
                    var retval = null;
                    if (self.OperationType == 1)//Move
                    {
                        if (self.ObjectsHasOnlyNetworkDevice())
                            retval = ['true', 'true', 'false', 'false', null];//ShowRoom, ShowRack, ShowWorkplace, ShowDevice, StorageID
                        else if (self.ObjectsHasOnlyTerminalDevice())
                            retval = ['true', 'false', 'true', 'false', null];
                        else if (self.ObjectsHasOnlyAdapterOrPeripheral())
                            retval = ['true', 'false', 'false', 'true', null];
                        else
                            retval = ['true', 'false', 'false', 'false', null];
                    }
                    else if (self.OperationType == 7)//fromStorage
                    {
                        retval = ['true', 'true', 'true', 'true', null];
                    }
                    else if (self.OperationType == 8)//toStorage
                    {
                        retval = ['true', 'false', 'false', 'false', null];
                    }
                    //
                    return retval;
                };
                //
                self.locationSearcher = null;
                self.locationSearcherD = $.Deferred();
                self.InitializeLocationSearcher = function () {
                    var fh = new fhModule.formHelper();
                    var locationD = fh.SetTextSearcherToField(
                        $region.find('.assetReg-location'),
                        'AssetLocationSearcher',//not only room! rack and workplace can be
                        null,
                        self.getSearcherParams(),
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
                        //
                        $region.find('.assetReg-location').focus();
                    });
                };
                //
                self.LocationReadOnly = ko.observable(false);
                self.InitializeStartLocation = function () {
                    var firstObj = self.selectedObjects[0];
                    if (self.OperationType == 7)//"Установить в"
                    {
                        self.LocationID = firstObj.ID;
                        self.LocationClassID = firstObj.ClassID;
                        self.LocationFullName(firstObj.Name);
                        self.LocationReadOnly(true);
                        self.selectedObjects = [];
                    }
                    else {
                        var ajaxControl = new ajaxLib.control();
                        var data = { 'ID': firstObj.ID, 'ClassID': firstObj.ClassID, 'OperationType': self.OperationType }
                        ajaxControl.Ajax(self.$region.find('.assetReg-navigatorControl'),
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
                                        ajaxControl.Ajax(self.$region.find('.assetReg-navigatorControl'),
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
            }
            //
            {//select device (для команды Установить В)
                self.SearchSubdeviceShow = function () {
                    showSpinner();
                    require(['ui_forms/Asset/frmSubdeviceSearch'], function (jsModule) {
                        jsModule.ShowDialog(function (selectedInfos) {//[{ClassID, ID}]                            
                            selectedInfos.forEach(function (el) {
                                self.selectedObjects.push(el);
                                self.LinkIDList.push(el.ID);
                            });
                            //
                            self.TabLinksCaption(getTextResource('AssetLinkHeaderChoosen') + ' (' + self.LinkIDList.length + ')');
                        },
                        self.forceFrmClose,
                        self.AvailableClassID,
                        true);
                    });
                };
            }

        },
        ShowDialog: function (selectedObjects, operationName, lifeCycleStateOperationID, isSpinnerActive, operationType, availableClassID) {
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
                    var d = vm.AssetMove();
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
                    'region_assetMoveForm',//form region prefix
                    'setting_assetMoveForm',//location and size setting
                    operationName,//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    600, 430,//minSize
                    buttons,//form buttons
                    null,//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmAssetMove\', afterRender: AfterRender}"'//attributes of form region
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
                vm.OperationType = operationType;
                vm.AvailableClassID = availableClassID;
                vm.$isDone = $retval;
                //
                var forceFrmClose = function () {
                    forceClose = true;
                    frm.Close();
                };
                vm.forceFrmClose = forceFrmClose;
                //
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
            return $retval.promise();
        }
    }
    return module;
});