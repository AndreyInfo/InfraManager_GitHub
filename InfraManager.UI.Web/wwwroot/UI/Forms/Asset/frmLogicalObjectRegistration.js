define(['knockout', 'jquery', 'ajax', 'formControl',
    'usualForms', 'ttControl', 'models/AssetForms/AssetFields',
    'models/AssetForms/ReferenceControl', 'models/AssetForms/AssetReferenceControl',
    'ui_forms/Asset/Controls/AdapterList', 'ui_forms/Asset/Controls/PeripheralList',
    'ui_forms/Asset/Controls/PortList', 'ui_forms/Asset/Controls/SlotList',
    'ui_forms/Asset/Controls/SoftwareInstallationList', 'models/AssetForms/AssetForm.History',
    'parametersControl', 'dateTimeControl',], function (ko, $, ajaxLib, fc, fhModule, tclib,
        assetFields, referenceControl, assetReferenceControl, adapterList,
        peripheralList, portList, slotList, installationList, assetHistory, pcLib, dtLib) {
    var module = {
        ViewModel: function (isReadOnly, $region, parentObj, callBackFunc) {
            var self = this;
            self.$region = $region;
            //
            self.ParentObj = parentObj;
            //
            $.when(userD).done(function (user) {
                self.CurrentUser = user;
            });
            //
            self.ClassID = ko.observable(null);
            self.LogicalObject = ko.observable(null);
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
            };
            //
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.modes = {
                nothing: 'nothing',
                main: 'main',
            };
            //
            self.GetTabSize = function () {
                return {
                    h: parseInt(self.TabHeight().replace('px', '')),
                    w: parseInt(self.TabWidth().replace('px', ''))
                };
            };
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
            });
            //
            self.MainClick = function () {
                self.mode(self.modes.main);
            };
            //
            //
            self.Load = function () {
                var retD = $.Deferred();
                $.when(self.LoadLogicalObject(), self.InitializeSettings()).done(function (isLoaded) {
                    if (isLoaded) {
                        self.LogicalObject().ClientID(self.CurrentUser.UserID);
                        self.LogicalObject().ClientClassID('9');
                        self.LogicalObject().ClientName(self.CurrentUser.UserFullName);
                        self.LogicalObject().ITAssetContactID(self.CurrentUser.UserID);
                        self.LogicalObject().ITAssetContactClassID('9');
                        self.LogicalObject().ITAssetContactName(self.CurrentUser.UserFullName);
                        self.InitializeClient();
                        self.InitializeITContact();
                        self.InitializeHost();
                        self.LoadAssetOperationControl();
                    }
                    retD.resolve(isLoaded);
                });
                //
                return retD.promise();
            };
            //
            self.ajaxControl_LogicalObject = new ajaxLib.control();
            //
            self.LoadLogicalObject = function () {
                var retDD = $.Deferred();
                require(['ui_forms/Asset/LogicalObject'], function (deLib) {
                    self.LogicalObject(new deLib.LogicalObjectRegistration(self));
                    self.ClassID(self.LogicalObject().ClassID());
                    self.ClassID.valueHasMutated();
                    retDD.resolve(true);
                });
                return retDD.promise();
            };
            //
            self.AfterRender = function () {
                self.frm.SizeChanged();
            };
            //
            self.EditIPAddressName = ko.observable(false);
            self.ajaxControl_LogicalObjectSettings = new ajaxLib.control();
            self.InitializeSettings = function () {
                var retDDD = $.Deferred();
                self.ajaxControl_LogicalObjectSettings.Ajax(null,
                    {
                        url: '/assetApi/GetLogicalObjectSettings',
                        method: 'GET',
                        dataType: 'json'
                    },
                    function (response) {
                        if (response) {
                            //
                            if (response.Result == 0) {//ok 
                                self.EditIPAddressName(response.EditIPAddressName)
                                retDDD.resolve();
                            }
                            else if (response.Result != 0)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                });
                        }
                        retDDD.resolve();
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLogicalObjectRegistration.js, GetLogicalObjectSettings]', 'error');
                        });
                        retDDD.resolve();
                    });
                return retDDD.promise();
            };
            //
            self.EditName = function () {
                if (!self.CanEdit() || !self.LogicalObject().CanEditName() || !self.EditIPAddressName())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Name',
                        fieldFriendlyName: getTextResource('AssetNumber_Name'),
                        oldValue: asset.Name(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.Name(newText);
                        },
                        nosave: true
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
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            //
            self.EditIPAddress = function () {
                if (!self.CanEdit() || !self.EditIPAddressName())//если нет настройки "Использовать фоновую работу с узлами сети"
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
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
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.ipAddressEdit, options);
                });
            };
            //
            //VCenter
            self.EditVCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'VCenter',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_VCenter'),
                        oldValue: asset.VCenter(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.VCenter(newText);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //UUID 
            self.EditUUID = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'UUID',
                        fieldFriendlyName: 'UUID',
                        oldValue: asset.UUID(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.UUID(newText);
                        },
                        nosave: true
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
                        fieldName: 'SerialNumber',
                        fieldFriendlyName: getTextResource('Asset_SerialNumber'),
                        oldValue: asset.SerialNumber(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.SerialNumber(newText);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            ////
            self.EditClient = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
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
                            asset.ClientName(objectInfo ? objectInfo.ClientName : '');
                            self.InitializeClient();
                        },
                        nosave: true
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
                        fieldName: 'ITContact',
                        fieldFriendlyName: getTextResource('Administrator'),
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
                           asset.ITAssetContactName(objectInfo ? objectInfo.ITAssetContactName : '');
                            self.InitializeITContact();
                        },
                        nosave: true
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
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        a.ITContact(user);
                        a.ITContactLoaded(true);
                    }
                });
            };
            //Примечание 
            self.EditNote = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Note',
                        fieldFriendlyName: getTextResource('AssetNumber_Note'),
                        oldValue: asset.Note(),
                        allowNull: true,
                        maxLength: 1000,
                        onSave: function (newText) {
                           asset.Note(newText);
                        },
                        nosave: true
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
                    if (classID == 5 || classID == 6 || classID == 33 || classID == 34)
                        fh.ShowAssetForm(cu.HostID(), classID);
                    else if (classID == 420)
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
                        },

                        nosave: true
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
                        },                       
                        nosave: true
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
            //CPULimit
            self.EditCPULimit = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'LogicalObject.CPULimit',
                        fieldFriendlyName: getTextResource('LogicObject_CPULimit'),
                        oldValue: self.LogicalObject().CPULimit(),
                        maxValue: 1024,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().CPULimit(newVal);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.InitializeHost = function () {
                self.LogicalObject().HostID(self.ParentObj ? self.ParentObj().ID() : null);
                self.LogicalObject().HostClassID(self.ParentObj ? self.ParentObj().ClassID() : null);
                self.LogicalObject().HostName(self.ParentObj ? self.ParentObj().Name() : '');
                if (self.ParentObj)
                    self.HasHost(true);
                else
                    self.HasHost(false);
            };
            self.EditHost = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var fh = new fhModule.formHelper(true);
                var options = {
                    fieldName: 'LogicalObject.Host',
                    fieldFriendlyName: getTextResource('Device'),
                    oldValue: self.LogicalObject().HostID() != null ? { ID: self.LogicalObject().HostID(), ClassID: self.LogicalObject().HostClassID(), FullName: self.LogicalObject().HostName() } : null,
                    searcherName: 'LogicObjectHostSearcher',
                    searcherPlaceholder: getTextResource('FilterEnterValue'),
                    searcherParams: [self.LogicalObject() ? self.LogicalObject().ID() : null, self.CurrentUser.UserID],
                    onSave: function (objectInfo) {
                        self.LogicalObject().HostID(objectInfo ? objectInfo.ID : null);
                        self.LogicalObject().HostClassID(objectInfo ? objectInfo.ClassID : null);
                        self.LogicalObject().HostName(objectInfo ? objectInfo.FullName : '');
                        if (objectInfo)
                            self.HasHost(true);
                        else
                            self.HasHost(false);
                    },
                    nosave: true
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
                        fieldName: 'LogicalObject.CPUNumber',
                        fieldFriendlyName: getTextResource('LogicObject_CPUReserve'),
                        oldValue: self.LogicalObject().CPUNumber(),
                        maxValue: 1024,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().CPUNumber(newVal);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //CPU
            self.EditCPUShare = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'LogicalObject.CPUShare',
                        fieldFriendlyName: getTextResource('LogicObject_CPUPriority'),
                        oldValue: self.LogicalObject().CPUShare(),
                        maxValue: 1024,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().CPUShare(newVal);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //RAM
            self.EditRAMLimit = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'LogicalObject.RAMLimit',
                        fieldFriendlyName: getTextResource('LogicObject_RAMLimit'),
                        oldValue: self.LogicalObject().MemoryLimit() == 0 ? '' : self.LogicalObject().MemoryLimit(),
                        maxValue: 99999999,//
                        stepperType: 'float',
                        floatPrecission: 2,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().MemoryLimit(newVal);
                        },
                        nosave: true
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
                        fieldName: 'LogicalObject.RAMNumber',
                        fieldFriendlyName: getTextResource('LogicObject_RAMReserve'),
                        oldValue: self.LogicalObject().Memory() == 0 ? '' : self.LogicalObject().Memory(),
                        maxValue: 99999999,//в бд decimal(10, 2)
                        stepperType: 'float',
                        floatPrecission: 2,
                        allowNull: true,
                        onSave: function (newVal) {
                            self.LogicalObject().Memory(newVal);
                        },
                        nosave: true
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
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditType = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: 'Type',
                        fieldFriendlyName: getTextResource('Type'),
                        oldValue: { ID: asset.ProductCatalogTypeID(), FullName: asset.ProductCatalogTypeName() },
                        searcherName: 'ProductCatalogTypeAndModelSearcher',
                        searcherParams: [true, false, false, false, false, false, false, false, null, true],
                        onSave: function (objectInfo) {
                            asset.ProductCatalogTypeID(objectInfo ? objectInfo.ID : null);
                            asset.ProductCatalogTypeName(objectInfo ? objectInfo.FullName : '');
                            asset.ProductCatalogModelID(null);
                            asset.ProductCatalogModelName('');
                            if (objectInfo)
                                self.InitLogicalObjectType(objectInfo.ID);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.ajaxControl_LogicalObjectType = new ajaxLib.control();
            //
            self.InitLogicalObjectType = function (id) {
                var data = { 'ID': id };
                self.ajaxControl_LogicalObjectType.Ajax(null,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetType'
                    },
                    function (newVal) {                        
                        //
                        if (newVal.Result == 0) {
                            self.LogicalObject().ClassID(newVal.Type.ClassID);
                            self.ClassID(self.LogicalObject().ClassID());
                            self.ClassID.valueHasMutated();
                                 if (self.ClassID() == 12)
                                 {
                                     self.LogicalObject().HostID(null);
                                     self.LogicalObject().HostClassID(null);
                                     self.LogicalObject().HostName('');
                                     self.HasHost(false);
                                 }
                            }
                            else {
                                if (newVal.Result == 3) {//AccessError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                    });
                                }
                                if (newVal.Result == 6) {//AccessError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('ObjectDeleted'), 'error');
                                    });
                                }
                                else if (newVal.Result == 7) {//OperationError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                    });
                                }
                                else {//GlobalError
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }                        
                    });
                //
            };
            //
            //
            //
            self.EditModel = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.LogicalObject();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: 'Model',
                        fieldFriendlyName: getTextResource('Model'),
                        oldValue: { ID: asset.ProductCatalogModelID(), FullName: asset.ProductCatalogModelName() },
                        searcherName: 'ProductCatalogTypeAndModelSearcher',
                        searcherParams: [false, false, false, false, true, false, false, false, null, true, asset.ProductCatalogTypeID()],
                        onSave: function (objectInfo) {
                            asset.ProductCatalogModelID(objectInfo ? objectInfo.ID : null);
                            asset.ProductCatalogModelName(objectInfo ? objectInfo.FullName : '');
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
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
            self.Save = function () {
                var retval = $.Deferred();
                // 
                if (self.LogicalObject().LogicAssetName() == '' || self.LogicalObject().LogicAssetName() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('NullParamsError'), getTextResource('MustSetLogicAssetName'), 'error');
                    });
                    retval.resolve(false);
                    return false;
                }
                if (self.LogicalObject().ProductCatalogTypeName() == '' || self.LogicalObject().ProductCatalogTypeName() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('NullParamsError'), getTextResource('MustSetType'), 'error');
                    });
                    retval.resolve(false);
                    return false;
                };
                if (self.LogicalObject().ProductCatalogModelName() == '' || self.LogicalObject().ProductCatalogModelName() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('NullParamsError'), getTextResource('MustSetModel'), 'error');
                    });
                    retval.resolve(false);
                    return false;
                };
                
                    var data = {
                        'ID': self.LogicalObject().ID,
                        'Name': self.LogicalObject().Name,
                        'LogicAssetName': self.LogicalObject().LogicAssetName,
                        'IPAddress': self.LogicalObject().IPAddress,
                        'VCenter': self.LogicalObject().VCenter,
                        'UUID': self.LogicalObject().UUID,
                        'SerialNumber': self.LogicalObject().SerialNumber,
                        'ProductCatalogTypeID': self.LogicalObject().ProductCatalogTypeID,
                        'ProductCatalogModelID': self.LogicalObject().ProductCatalogModelID,
                        'ProductCatalogModelGuidID': self.LogicalObject().ProductCatalogModelID,
                        'ClientID': self.LogicalObject().ClientID,
                        'ClientClassID': self.LogicalObject().ClientClassID,
                        'ClientName': self.LogicalObject().ClientName,

                        'ITAssetContactID': self.LogicalObject().ITAssetContactID,
                        'ITAssetContactClassID': self.LogicalObject().ITAssetContactClassID,
                        'ITAssetContactName': self.LogicalObject().ITAssetContactName,                       

                        'Note': self.LogicalObject().Note,
                        'HostID': self.LogicalObject().HostID,
                        'HostClassID': self.LogicalObject().HostClassID,
                        'HostName':self.LogicalObject().HostName,

                        'DeviceID': self.LogicalObject().DeviceID,
                        'DeviceClassID': self.LogicalObject().DeviceClassID,

                        'CPUNumber': self.LogicalObject().CPUNumber,
                        'CPULimit': self.LogicalObject().CPULimit,
                        'CPUShare': self.LogicalObject().CPUShare,
                        'Memory': self.LogicalObject().Memory,
                        'MemoryLimit': self.LogicalObject().MemoryLimit,
                        'MemoryShare': self.LogicalObject().MemoryShare,
                        'ConfigurationUnitID': self.LogicalObject().ConfigurationUnitID,

                        'DateReceivedDT': self.LogicalObject().DateReceived() == null ? '' : dtLib.GetMillisecondsSince1970(self.LogicalObject().DateReceivedDT()),
                        'DateAnnuledDT': self.LogicalObject().DateAnnuled() == null ? '' : dtLib.GetMillisecondsSince1970(self.LogicalObject().DateAnnuledDT()),
                        'DateReceived': self.LogicalObject().DateReceivedDT(),
                        'DateAnnuled': self.LogicalObject().DateAnnuledDT(),

                    };
                    //
                    if (data.DateAnnuledDT && data.DateReceivedDT) {
                        if (new Date(parseInt(data.DateAnnuledDT)) < new Date(parseInt(data.DateReceivedDT))) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SoftwareLicenceAdd_CheckDate'));
                            });
                        retval.resolve(null);
                        return;
                        };
                     };

                    showSpinner();
                    self.ajaxControl_LogicalObject.Ajax(null,
                        {
                            url: '/assetApi/AddLogicalObject',
                            method: 'POST',
                            dataType: 'json',
                            data: data
                        },
                        function (response) {//
                            hideSpinner();
                            if (response) {                               
                                //
                                if (response.Result == 0) {//ok 
                                    if (callBackFunc)
                                        callBackFunc();
                                    retval.resolve(true);
                                    return true;
                                }
                                else if (response.Result != 0)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                    });
                            }
                            retval.resolve(false);
                        },
                        function (response) {
                            hideSpinner();
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLogicalObjectRegistration.js, register]', 'error');
                            });
                            retval.resolve(false);
                            return false;
                        });
                    //
                    return retval.promise();
                };
            },
        ShowDialog: function (isSpinnerActive, parentObj, callBackFunc) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                $.when(userD, operationIsGrantedD(956)).done(function (user, operation_add) {
                    var isReadOnly = false;
                    if (user.HasRoles == false || operation_add == false)
                        isReadOnly = true;
                    //
                    var frm = undefined;
                    //
                    var buttons = {}
                    var vm = null;
                    buttons[getTextResource('Add')] = function () {
                        //frm.BeforeSave();
                        $.when(vm.Save()).done(function (tmp) {
                            if (tmp == true)
                                frm.Close();
                        });
                    };
                    buttons[getTextResource('CancelButtonText')] = function () {
                        frm.Close();
                    };

                    //
                    frm = new fc.control(
                        'region_logicalObjectForm_',//form region prefix
                        'setting_logicalObjectForm_',//location and size setting
                        getTextResource('Asset_Add'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizableSaveSave
                        950, 865,//minSize
                        buttons,//form buttons
                        function () {
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/frmLogicalObjectRegistration\', afterRender: AfterRender}"'//attributes of form region
                    );
                    //
                    if (!frm.Initialized) {//form with that region and settingsName was open

                        require(['sweetAlert'], function () {
                            swal(getTextResource('OpenError'), getTextResource('CantDuplicateForm'), 'warning');
                        });
                        //
                        return;
                    }
                    //
                    //
                    var $region = $('#' + frm.GetRegionID());
                    vm = new module.ViewModel(isReadOnly, $region, parentObj, callBackFunc);
                    vm.frm = frm;
                    //
                    frm.SizeChanged = function () {
                        var width = frm.GetInnerWidth();
                        var height = frm.GetInnerHeight();
                        //
                        vm.$region.css('width', width + 'px').css('height', height + 'px');
                    };
                    //
                    ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                    $.when(frm.Show(), vm.Load()).done(function (frmD, loadD) {
                        if (loadD == false) {//force close
                            frm.Close();
                        }
                        else {
                            if (!ko.components.isRegistered('logicalObjectFormCaptionComponent'))
                                ko.components.register('logicalObjectFormCaptionComponent', {
                                    template: '<span data-bind="text: $captionText"/>'
                                });
                            frm.BindCaption(vm, "component: {name: 'logicalObjectFormCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Add\') + ' ' + $data.LogicalObject().Name()} }");
                        }
                        hideSpinner();
                    });
                });
            }
        }   
    return module;
});