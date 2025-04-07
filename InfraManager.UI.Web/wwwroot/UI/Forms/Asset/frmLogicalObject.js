define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl',
    'models/AssetForms/AssetFields', 'models/AssetForms/ReferenceControl',
    'models/AssetForms/AssetReferenceControl', 'ui_forms/Asset/Controls/AdapterList',
    'ui_forms/Asset/Controls/PeripheralList', 'ui_forms/Asset/Controls/PortList',
    'ui_forms/Asset/Controls/SlotList', 'ui_forms/Asset/Controls/SoftwareInstallationList',
    'models/AssetForms/AssetForm.History', 'parametersControl', './LogicObjectComponentsForTable'],
    function (ko, $, ajaxLib, fc, fhModule, tclib,
        assetFields, referenceControl,
        assetReferenceControl, adapterList, peripheralList,
        portList, slotList, installationList, assetHistory, pcLib, logicObjectComponents) {
    var module = {
        ViewModel: function (isReadOnly, $region) {
            var self = this;
            self.$region = $region;
            //
            $.when(userD).done(function (user) {
                self.CurrentUser = user;
            });
            //
            self.ClassID = ko.observable(null);
            self.LogicalObject = ko.observable(null);
            //
            self.TabHeight = ko.observable(0);
            self.TabWidth = ko.observable(0);
            self.TabSize = ko.computed(function () {
                return {
                    h: self.TabHeight(),
                    w: self.TabWidth()
                };
            });
            //
            self.SizeChanged = function () {
                if (!self.LogicalObject())
                    return;
                //
                var tabHeight = self.$region.height();//form height
                tabHeight -= 140;
                //
                var tabWidth = self.$region.width();//form width
                tabWidth -= self.$region.find('.b-requestDetail-right').outerWidth(true);
                //
                self.TabHeight(Math.max(0, tabHeight - 10) + 'px');
                self.TabWidth(Math.max(0, tabWidth - 5) + 'px');
            };
            //
            self.IsReadOnly = ko.observable(isReadOnly);
            self.CanEdit = ko.computed(function () {
                if (!self.LogicalObject())
                    return false;
                if (self.LogicalObject().LifeCycleStateName() === 'Списано')
                    return false;
                //
                return !self.IsReadOnly();
            });
            self.CanShow = ko.observable(self.CanEdit);
            //            
            self.AssetOperationControl = ko.observable(null);
            self.LoadAssetOperationControl = function () {
                if (!self.LogicalObject())
                    return;
                //
                require(['assetOperations'], function (wfLib) {
                    if (self.AssetOperationControl() == null) {
                        self.AssetOperationControl(new wfLib.control(self.$region, self.LogicalObject, self.Load));
                    }
                    self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                    self.AssetOperationControl().Initialize();
                });
            };            //
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                attachments: 'attachments',
                installationList: 'installationList',
                history: 'history',
                logicObjectComponents: 'logicObjectComponents'
            };
            //
            self.GetTabSize = function () {
                return {
                    h: parseInt(self.TabHeight().replace('px', '')),
                    w: parseInt(self.TabWidth().replace('px', ''))
                };
            };
            //
            self.assetHistory = new assetHistory.Tape(self.LogicalObject, self.ClassID, self.$region.find('.assetHistory .tabContent').selector);
            self.logicObjectComponents = new logicObjectComponents.List(self);
            //          
            //MAIN TAB BLOCK
            self.mode = ko.observable(self.modes.main);
            self.mode.subscribe(function (newValue) {
                if (newValue == self.modes.nothing)
                    return;
                //
                else if (newValue == self.modes.history) {
                    self.assetHistory.CheckData();
                }
                else if (newValue == self.modes.attachments) {
                    self.InitializeAttachmentControl();
                }
                else if (newValue == self.modes.installationList) {
                    self.InitializeInstallationControl();
                }

            });
            //
            //tab installationList
            {
                self.installation_Control = ko.observable(null);
                self.installationList = ko.observable(null);
                self.InitializeInstallationControl = function () {
                    if (self.installation_Control() != null)
                        return;
                    //
                    self.installationList(new installationList.LinkList(self.LogicalObject, self.$region.find('.softwareInstallationList').selector, self.CanEdit));
                    self.installation_Control(new assetReferenceControl.Control
                        (
                            self.LogicalObject,
                            [
                                { List: self.installationList(), TemplateName: '../UI/Forms/Asset/Controls/SoftwareInstallationList' },
                            ],
                            function () {
                                var retD = $.Deferred();
                                $.when(self.installationList().CheckData()).done(function () {
                                    retD.resolve();
                                });
                                return retD.promise();
                            },
                            '.nd-installation',
                            self.GetTabSize
                        ));
                    //
                    self.installation_Control().Initialize();
                };
            }
            //
            //tab installationList
            {
                self.installation_Control = ko.observable(null);
                self.installationList = ko.observable(null);
                self.InitializeInstallationControl = function () {
                    if (self.installation_Control() != null)
                        return;
                    //
                    self.installationList(new installationList.LinkList(self.LogicalObject, self.$region.find('.softwareInstallationList').selector, self.CanEdit));
                    self.installation_Control(new assetReferenceControl.Control
                        (
                            self.LogicalObject,
                            [
                                { List: self.installationList(), TemplateName: '../UI/Forms/Asset/Controls/SoftwareInstallationList' },
                            ],
                            function () {
                                var retD = $.Deferred();
                                $.when(self.installationList().CheckData()).done(function () {
                                    retD.resolve();
                                });
                                return retD.promise();
                            },
                            '.nd-installation',
                            self.GetTabSize
                        ));
                    //
                    self.installation_Control().Initialize();
                };
            }
            //
            self.MainClick = function () {
                self.mode(self.modes.main);
            };
            //
            self.HistoryClick = function () {
                self.mode(self.modes.history);
                self.SizeChanged();
            };
            //
            self.InstallationListClick = function () {
                self.mode(self.modes.installationList);
            };
            self.AttachmentsClick = function () {
                self.mode(self.modes.attachments);
            };
            //
            self.LogicObjectComponentsClick = function () {
                self.mode(self.modes.logicObjectComponents);
            };
            //
             self.Load = function (id) {
                var retD = $.Deferred();
                $.when(self.LoadLogicalObject(id)).done(function (isLoaded) {
                    retD.resolve(isLoaded);
                    if (isLoaded) {
                        self.InitializeClient();
                        self.InitializeITContact();
                        self.SizeChanged();
                        self.LoadAssetOperationControl();
                    }
                });
                //
                return retD.promise();
            };
            //
            self.ajaxControl_LogicalObject = new ajaxLib.control();
            //
            self.LoadLogicalObject = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_LogicalObject.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetLogicalObject'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var lo = newVal.LogicalObject;
                                if (lo) {
                                    require(['ui_forms/Asset/LogicalObject'], function (loLib) {
                                        self.LogicalObject(new loLib.LogicalObject(self, lo));
                                        self.ClassID(self.LogicalObject().ClassID());
                                        self.ClassID.valueHasMutated();
                                        //
                                        processed = true;
                                        loadSuccessD.resolve(true);
                                    });
                                }
                                else loadSuccessD.resolve(false);
                            }
                            else {
                                if (newVal.Result == 3) {//AccessError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                    });
                                    processed = true;
                                }
                                if (newVal.Result == 6) {//AccessError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('ObjectDeleted'), 'error');
                                    });
                                    processed = true;
                                }
                                else if (newVal.Result == 7) {//OperationError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                    });
                                    processed = true;
                                }
                                else {//GlobalError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                    });
                                    processed = true;
                                }
                                loadSuccessD.resolve(false);
                            }
                        }
                        else loadSuccessD.resolve(false);
                        //
                        $.when(loadSuccessD).done(function (loadSuccess) {
                            retD.resolve(loadSuccess);
                            if (loadSuccess == false && processed == false) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[frmLogicalObject.js, Load]', 'error');
                                });
                            }
                        });
                    });
                //
                return retD.promise();
            };
            //
            self.AfterRender = function () {
                self.SizeChanged();
            };
            //
            self.EditName = function () {
                if (!self.CanEdit() || !self.LogicalObject().CanEditName() || !self.LogicalObject().EditIPAddressName())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'Name',
                        fieldFriendlyName: getTextResource('AssetNumber_Name'),
                        oldValue: asset.Name(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.Name(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            
            self.EditLogicAssetName = function () {
                if (!self.CanEdit() || !self.LogicalObject().CanEditName())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicAssetName',
                        fieldFriendlyName: getTextResource('LogicAssetName'),
                        oldValue: asset.LogicAssetName(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.LogicAssetName(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            //
            self.EditIPAddress = function () {
                var asset = self.LogicalObject();
                if (!self.CanEdit() || !asset.EditIPAddressName())//если нет настройки "Использовать фоновую работу с узлами сети"
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: 'IPAddress',
                        fieldFriendlyName: getTextResource('AssetIPAddress'),
                        oldValue: asset.IPAddress(),
                        onSave: function (newText) {
                            asset.IPAddress(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.ipAddressEdit, options);
                });
            };
            //
            self.EditVCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'VCenter',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_VCenter'),
                        oldValue: asset.VCenter(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.VCenter(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            self.EditUUID = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'UUID',
                        fieldFriendlyName: 'UUID',
                        oldValue: asset.UUID(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.UUID(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            self.EditSerialNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'SerialNumber',
                        fieldFriendlyName: getTextResource('Asset_SerialNumber'),
                        oldValue: asset.SerialNumber(),
                        allowNull: true,
                        maxLength: 1000,
                        onSave: function (newText) {
                            asset.SerialNumber(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options); 
                });
            };
            self.EditClient = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'Client',
                        fieldFriendlyName: getTextResource('Client'),
                        oldValue: asset.ClientLoaded() ? { ID: asset.ClientID(), ClassID: asset.ClientClassID(), FullName: asset.ClientName() } : null,
                        object: ko.toJS(asset.Client()),
                        searcherName: 'UserWithQueueSearcher',
                        searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                        onSave: function (objectInfo) {
                            asset.ClientLoaded(false);
                            asset.Client(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditClient, false, false));
                            //
                            asset.ClientID(objectInfo ? objectInfo.ID : '');
                            asset.ClientClassID(objectInfo ? objectInfo.ClassID : '');
                            self.InitializeClient();
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.InitializeClient = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.LogicalObject();
                    if (a.ClientLoaded() == false && a.ClientID()) {
                        var type = null;
                        if (a.ClientClassID() == 9) {//IMSystem.Global.OBJ_USER
                            type = userLib.UserTypes.utilizer;
                        }
                        else if (a.ClientClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                            type = userLib.UserTypes.queueExecutor;
                        }
                        //
                        var options = {
                            UserID: a.ClientID(),
                            UserType: type,
                            UserName: null,
                            EditAction: self.EditClient,
                            RemoveAction: null,
                            //ShowLeftSide: false,
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        a.Client(user);
                        a.ClientLoaded(true);
                    }
                });
            };
            //
            self.EditITContact = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'ITContact',
                        fieldFriendlyName: getTextResource('Client'),
                        oldValue: asset.ITContactLoaded() ? { ID: asset.ITAssetContactID(), ClassID: asset.ITAssetContactClassID(), FullName: asset.ITAssetContactName() } : null,
                        object: ko.toJS(asset.ITContact()),
                        searcherName: 'UserWithQueueSearcher',
                        searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                        onSave: function (objectInfo) {
                            asset.ITContactLoaded(false);
                            asset.ITContact(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditITContact, false, false));
                            //
                            asset.ITAssetContactID(objectInfo ? objectInfo.ID : '');
                            asset.ITAssetContactClassID(objectInfo ? objectInfo.ClassID : '');
                            self.InitializeITContact();
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.InitializeITContact = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.LogicalObject();
                    if (a.ITContactLoaded() == false && a.ITAssetContactID()) {
                        var type = null;
                        if (a.ITAssetContactClassID() == 9) {//IMSystem.Global.OBJ_USER
                            type = userLib.UserTypes.utilizer;
                        }
                        else if (a.ITAssetContactClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                            type = userLib.UserTypes.queueExecutor;
                        }
                        //
                        var options = {
                            UserID: a.ITAssetContactID(),
                            UserType: type,
                            UserName: null,
                            EditAction: self.EditITContact,
                            RemoveAction: null,
                            //ShowLeftSide: false,
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        a.ITContact(user);
                        a.ITContactLoaded(true);
                    }
                });
            };
            //
            self.EditNote = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'Note',
                        fieldFriendlyName: getTextResource('AssetNumber_Note'),
                        oldValue: asset.Note(),
                        allowNull: true,
                        maxLength: 1000,
                        onSave: function (newText) {
                            asset.Note(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.ShowDeviceForm = function () {
                if (self.LogicalObject().HostID() == null || self.LogicalObject().HostID() == '')
                    return;
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var cu = self.LogicalObject();
                    classID = cu.HostClassID();
                    if (classID == 5 || classID == 6 || classID == 33 || classID == 34 )
                        fh.ShowAssetForm(cu.HostID(), classID);                                                           
                    else if(classID == 420) 
                        fh.ShowClusterForm(cu.HostID());
                    else if (classID == 419) 
                        fh.ShowConfigurationUnitForm(cu.HostID());
                    else if (classID == 415 || classID == 416 || classID == 417 || classID == 418 || classID == 12) //OBJ_LogicalObject
                        fh.ShowLogicalObjectForm(cu.HostID());
                });
            };
            //
            self.EditDateReceived = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.DateReceived',
                        fieldFriendlyName: getTextResource('CallDatePromise'),
                        oldValue: self.LogicalObject().DateReceivedDT(),
                        onSave: function (newDate) {
                            self.LogicalObject().DateReceived(parseDate(newDate));
                            self.LogicalObject().DateReceivedDT(new Date(parseInt(newDate)));                               
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                });
            };
            //
            self.DateReceivedCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                var retval = '';
                //
                if (!retval && self.LogicalObject) {
                    var lo = self.LogicalObject();
                    if (lo && lo.DateReceived)
                        retval = lo.DateReceived();
                }
                //
                return retval;
            });           
            //
            self.EditDateAnnuled = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.DateAnnuled',
                        fieldFriendlyName: getTextResource('CallDatePromise'),
                        oldValue: self.LogicalObject().DateAnnuledDT(),
                        onSave: function (newDate) {
                            self.LogicalObject().DateAnnuled(parseDate(newDate));
                            self.LogicalObject().DateAnnuledDT(new Date(parseInt(newDate)));
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                });
            };
            //
            self.DateAnnuledCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                var retval = '';
                //
                if (!retval && self.LogicalObject) {
                    var lo = self.LogicalObject();
                    if (lo && lo.DateAnnuled)
                        retval = lo.DateAnnuled();
                }
                //
                return retval;
            });
            //
            self.EditCPULimit = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.CPULimit',
                        fieldFriendlyName: getTextResource('LogicObject_CPULimit'),
                        oldValue: self.LogicalObject().CPULimit(),
                        maxValue: 1024,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().CPULimit(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditHost = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                    var fh = new fhModule.formHelper(true);
                var options = {
                        ID: self.LogicalObject().ID(),
                        ClassID: self.LogicalObject().ClassID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.Host',
                        fieldFriendlyName: getTextResource('Device'),
                        oldValue: self.LogicalObject().HostID() != null ? { ID: self.LogicalObject().HostID(), ClassID: self.LogicalObject().HostClassID(), FullName: self.LogicalObject().HostName() } : null,
                        searcherName: 'LogicObjectHostSearcher',
                        searcherPlaceholder: getTextResource('FilterEnterValue'),
                        searcherParams: [self.LogicalObject() ? self.LogicalObject().ID() : null, self.CurrentUser.UserID],
                        onSave: function (objectInfo) {
                            self.LogicalObject().HostID(objectInfo ? objectInfo.ID: null);
                            self.LogicalObject().HostClassID(objectInfo ? objectInfo.ClassID : null);
                            self.LogicalObject().HostName(objectInfo ? objectInfo.FullName : '');
                            if (objectInfo)
                                self.HasHost(true);
                            else
                                self.HasHost(false);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
            };
            //
            self.HasHost = ko.observable(false);
            //
            self.EditCPUNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.CPUNumber',
                        fieldFriendlyName: getTextResource('LogicObject_CPUReserve'),
                        oldValue: self.LogicalObject().CPUNumber(),
                        maxValue: 1024,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().CPUNumber(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditCPUShare = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.CPUShare',
                        fieldFriendlyName: getTextResource('LogicObject_CPUPriority'),
                        oldValue: self.LogicalObject().CPUShare(),
                        maxValue: 1024,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().CPUShare(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditRAMLimit = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.RAMLimit',
                        fieldFriendlyName: getTextResource('LogicObject_RAMLimit'),
                        oldValue: self.LogicalObject().MemoryLimit() == 0 ? '' : self.LogicalObject().MemoryLimit(),
                        maxValue: 99999999,//в бд decimal(10, 2)
                        stepperType: 'float',
                        floatPrecission: 2,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().MemoryLimit(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditRAMNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.RAMNumber',
                        fieldFriendlyName: getTextResource('LogicObject_RAMReserve'),
                        oldValue: self.LogicalObject().Memory() == 0 ? '' : self.LogicalObject().Memory() ,
                        maxValue: 99999999,//в бд decimal(10, 2)
                        stepperType: 'float',
                        floatPrecission: 2,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().Memory(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditRAMShare = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.LogicalObject().ID(),
                        objClassID: self.LogicalObject().ClassID(),
                        fieldName: 'LogicalObject.RAMShare',
                        fieldFriendlyName: getTextResource('LogicObject_RAMPriority'),
                        oldValue: self.LogicalObject().MemoryShare() == 0 ? '' : self.LogicalObject().MemoryShare(),
                        maxValue: 99999999,//в бд decimal(10, 2)
                        stepperType: 'float',
                        floatPrecission: 2,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().MemoryShare(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };   
            //
            self.ShowConfigurationUnitForm = function () {
                var asset = self.LogicalObject();
                if (!asset.ConfigurationUnitID())
                    return;
                //
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowConfigurationUnitForm(asset.ConfigurationUnitID());
                });
            };
            //
            {//attachments
                {//variables
                    self.attachmentsControl = null;
                }
                //
                {//events
                    self.CanEdit_handle = self.CanEdit.subscribe(function (newValue) {
                        if (self.attachmentsControl != null)
                            self.attachmentsControl.RemoveFileAvailable(newValue);
                        if (self.attachmentsControl != null)
                            self.attachmentsControl.ReadOnly(!newValue);
                    });
                }
                //
                //when tab selected
                self.InitializeAttachmentControl = function () {
                    require(['fileControl'], function (fcLib) {
                        var attachmentsElement = $('#' + self.frm.GetRegionID()).find('.documentList');
                        //
                        if (self.attachmentsControl != null) {
                            if (self.attachmentsControl.ObjectID != self.LogicalObject().ID())//previous object
                                self.attachmentsControl.RemoveUploadedFiles();
                        }
                        if (self.attachmentsControl == null) {
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        }
                        self.attachmentsControl.ReadOnly(!self.CanEdit());
                        self.attachmentsControl.RemoveFileAvailable(self.CanEdit());
                        //
                        if (self.attachmentsControl.ObjectID != self.LogicalObject().ID())
                            self.attachmentsControl.Initialize(self.LogicalObject().ID());
                        else if (self.attachmentsControl.IsLoaded() == false)
                            self.attachmentsControl.Load(attachmentsElement)
                    });
                };
                //
                self.DisposeAttacmentControl = function () {
                    if (self.attachmentsControl != null && !self.attachmentsControl.IsAllFilesUploaded())
                        require(['sweetAlert'], function () {
                            swal({
                                title: getTextResource('UploadedFileNotFoundAtServerSide'),
                                text: getTextResource('FormClosingQuestion'),
                                showCancelButton: true,
                                closeOnConfirm: true,
                                closeOnCancel: true,
                                confirmButtonText: getTextResource('ButtonOK'),
                                cancelButtonText: getTextResource('ButtonCancel')
                            },
                                function (value) {
                                    if (value == true) {
                                        self.attachmentsControl.StopUpload();
                                    }
                                });
                        });
                };
            }
        },
        ShowDialog: function (id, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            $.when(userD, operationIsGrantedD(957)).done(function (user, operation_update) {
                var isReadOnly = false;
                if (user.HasRoles == false || operation_update == false)
                    isReadOnly = true;
                //
                var frm = undefined;
                //
                var buttons = {
                }
                var vm = null;
                //
                frm = new fc.control(
                    'region_logicalObjectForm_',//form region prefix
                    'setting_logicalObjectForm_',//location and size setting
                    getTextResource('Asset_Properties'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    950, 800,//minSize
                    buttons,//form buttons
                    function () {
                        vm.DisposeAttacmentControl();
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmLogicalObject\', afterRender: AfterRender}"'//attributes of form region
                );
                //
                if (!frm.Initialized) {//form with that region and settingsName was open
                    hideSpinner();
                    //
                    var url = window.location.protocol + '//' + window.location.host + location.pathname + '?logicObjectID=' + id;
                    //
                    var wnd = window.open(url);
                    if (wnd) //browser cancel it?  
                        return;
                    //
                    require(['sweetAlert'], function () {
                        swal(getTextResource('OpenError'), getTextResource('CantDuplicateForm'), 'warning');
                    });
                    return;
                }            
                //
                frm.BeforeClose = function () {
                    hideSpinner();
                };
                //
                var $region = $('#' + frm.GetRegionID());
                vm = new module.ViewModel(isReadOnly, $region);
                vm.frm = frm;
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
                $.when(frm.Show(), vm.Load(id)).done(function (frmD, loadD) {
                    if (loadD == false) {//force close
                        frm.Close();
                    }
                    else {
                        if (!ko.components.isRegistered('logicalObjectFormCaptionComponent'))
                            ko.components.register('logicalObjectFormCaptionComponent', {
                                template: '<span data-bind="text: $captionText"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'logicalObjectFormCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Properties\') + ' ' + $data.LogicalObject().Name()} }");
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});