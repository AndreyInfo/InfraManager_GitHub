define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/AssetForms/AssetFields', 'models/AssetForms/ReferenceControl', 'models/AssetForms/AssetReferenceControl', 'ui_forms/Asset/Controls/AdapterList', 'ui_forms/Asset/Controls/PeripheralList', 'ui_forms/Asset/Controls/PortList', 'ui_forms/Asset/Controls/SlotList', 'ui_forms/Asset/Controls/SoftwareInstallationList', 'models/AssetForms/AssetForm.History', 'parametersControl'], function (ko, $, ajaxLib, fc, fhModule, tclib, assetFields, referenceControl, assetReferenceControl, adapterList, peripheralList, portList, slotList, installationList, assetHistory, pcLib) {
    var module = {
        ViewModel: function (isReadOnly, $region, parentObj, callBackFunc) {
            var self = this;
            self.$region = $region;
            //
            self.ClassID = 409;//OBJ_ConfigurationUnit
            self.configurationUnit = ko.observable(null);
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
            self.parentObj = ko.observable(parentObj ? (parentObj.ClassID == 420 ? parentObj.cluster() : null) : null);
            //
            self.SizeChanged = function () {
                if (!self.configurationUnit())
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
                if (!self.configurationUnit())
                    return false;
                if (self.configurationUnit().LifeCycleStateName() === 'Списано')
                    return false;
                if (!self.selectedTypeItem())
                    return false;
                //
                return !self.IsReadOnly();
            });
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                attachments: 'attachments',
                history: 'history'
            };
            //
            self.GetTabSize = function () {
                return {
                    h: parseInt(self.TabHeight().replace('px', '')),
                    w: parseInt(self.TabWidth().replace('px', ''))
                };
            };
            //
            self.assetHistory = new assetHistory.Tape(self.configurationUnit, self.ClassID, self.$region.find('.assetHistory .tabContent').selector);
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
            });
            //
            self.MainClick = function () {
                self.mode(self.modes.main);
            };
            //
            self.HistoryClick = function () {
                self.mode(self.modes.history);
                self.SizeChanged();
            };
            self.AttachmentsClick = function () {
                self.mode(self.modes.attachments);
            };
            //
            self.ajaxControl = new ajaxLib.control();
            //
            self.CreateConfigurationUnit = function () {
                var data = {
                    'Name': self.configurationUnit().Name(),
                    'IPAddress': self.configurationUnit().IPAddress(),
                    'IPMask': self.configurationUnit().IPMask(),
                    'ObjectID': self.configurationUnit().ObjectID(),
                    'ObjectClassID': self.configurationUnit().ObjectClassID(),
                    'ProductCatalogTypeID': self.selectedTypeItem().ID,
                    'InfrastructureSegmentID': self.configurationUnit().InfrastructureSegmentID(),
                    'CriticalityID': self.configurationUnit().CriticalityID(),
                    'AdministratorID': self.configurationUnit().AdministratorID(),
                    'ClusterID': self.parentObj() != null ? self.parentObj().ID() : null
                };
                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/CreateConfigurationUnit',
                        method: 'POST',
                        dataType: 'json',
                        data: data
                    },
                    function (response) {//ServiceContractRegistrationResponse
                        hideSpinner();
                        if (response) {
                            require(['sweetAlert'], function () {
                                swal("Конфигурационный элемент успешно создан");
                            });
                            if (callBackFunc)
                                callBackFunc();
                        }
                    });
            };
            //
            self.CanSelectNewDevice = ko.observable(true);
            //
            self.Load = function (device) {
                require(['ui_forms/Asset/ConfigurationUnit'], function (cuLib) {
                    self.configurationUnit(new cuLib.ConfigurationUnit(self, {}));
                    //
                    self.SizeChanged();
                    //
                    self.initSearcherControl('.infrastructureSegment', 'InfrastructureSegmentSearcher', self.configurationUnit().InfrastructureSegmentID, self.configurationUnit().InfrastructureSegmentName);
                    self.initSearcherControl('.criticality', 'CriticalitySearcher', self.configurationUnit().CriticalityID, self.configurationUnit().CriticalityName);
                    //
                    if (device)
                        self.CanSelectNewDevice(false);
                    //
                    self.GetTypeList(device);
                });
                //var retD = $.Deferred();
                //$.when(self.LoadConfigurationUnit(id)).done(function (isLoaded) {
                //    retD.resolve(isLoaded);
                //    if (isLoaded) {
                //        self.InitializeAdministrator();
                //        self.SizeChanged();
                //    }
                //});
                //
                //return retD.promise();
            };
            //
            self.initSearcherControl = function (selector, searcherName, ko_id, ko_fullName, ko_classID) {
                var $frm = $('#' + self.frm.GetRegionID()).find('.configurationUnitRegistrationForm');
                var searcherControlD = $.Deferred();
                //
                var fh = new fhModule.formHelper();
                var searcherLoadD = fh.SetTextSearcherToField(
                    $frm.find(selector),
                    searcherName,
                    null,
                    [],
                    function (objectInfo) {//select
                        ko_fullName(objectInfo.FullName);
                        ko_id(objectInfo.ID);
                        if (ko_classID) ko_classID(objectInfo.ClassID);
                    },
                    function () {//reset
                        ko_fullName('');
                        ko_id(null);
                        if (ko_classID) ko_classID(null);
                    },
                    function (selectedItem) {//close
                        if (!selectedItem) {
                            ko_fullName('');
                            ko_id(null);
                            if (ko_classID) ko_classID(0);
                        }
                    });
                $.when(searcherLoadD, userD).done(function (ctrl, user) {
                    searcherControlD.resolve(ctrl);
                    ctrl.CurrentUserID = user.ID;
                    self.PreInitAdministrator(user);
                    self.searcher_controls.push(ctrl);
                });
            };
            //
            self.PreInitAdministrator = function (user) {
                self.CurrentUser = user;
                self.configurationUnit().AdministratorID(user.UserID);
                self.configurationUnit().AdministratorClassID(9);
                self.InitializeAdministrator();
            };
            //
            self.searcher_controls = [];
            //
            self.selectedTypeItem = ko.observable(null);
            self.typeComboItems = ko.observableArray([]);
            self.getTypeComboItems = function () {
                return {
                    data: self.typeComboItems(),
                    totalCount: self.typeComboItems().length
                };
            };
            //
            self.selectedTypeItem.subscribe(function (newValue) {
                if (!self.CanEditDevice())
                    return;
                //
                var cu = self.configurationUnit();
                cu.ObjectClassID(null);
                cu.ObjectID(null);
                cu.FullName('');
                //self.GetServiceContractList();
            });
            //
            self.ajaxControlServiceCenter = new ajaxLib.control();
            self.GetTypeList = function (device) {
                var data = {
                    'onlyHosts': self.parentObj() != null,
                    deviceID: device ? device.ID : null,
                    deviceClassID: device ? device.ClassID : null
                };
                self.ajaxControlServiceCenter.Ajax($region.find('.type-combobox'),
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetConfigurationUnitTypeList'
                    },
                    function (result) {
                        if (result) {
                            var selEl = null;
                            result.forEach(function (el) {
                                if (selEl == null && device)
                                    selEl = el;
                                //
                                self.typeComboItems().push(el);
                            });
                            self.typeComboItems.valueHasMutated();
                            self.selectedTypeItem(selEl);
                            if (device)
                                self.DoAddDevice(device);
                        }
                    });
            };
            //
            self.ajaxControl_ConfigurationUnit = new ajaxLib.control();
            //
            self.DoAddDevice = function (device) {
                var cu = self.configurationUnit();
                cu.ObjectClassID(device.ClassID);
                cu.ObjectID(device.ID);
                //
                if (device.Name)
                    cu.FullName(device.Name);
                else if (device.TypeName || device.ModelName)
                    cu.FullName(device.TypeName + " " + device.ModelName);
                else if (device.ProductCatalogTypeName || device.ProductCatalogModelName)//logical device
                    cu.FullName(device.ProductCatalogTypeName + " " + device.ProductCatalogModelName);
                //
                cu.OrganizationName(device.OrganizationName);
                cu.BuildingName(device.BuildingName);
                cu.RoomName(device.RoomName);
                cu.WorkPlaceName(device.WorkPlaceName);
                //
                if (!cu.Name())
                    cu.Name(device.Name);
            };
            //
            self.addDevice = function () {
                if (!self.CanEditDevice())
                    return;
                //
                require(['assetForms'], function (fhModule) {
                    var fh = new fhModule.formHelper();
                    fh.ShowAssetLink({
                        ClassID: null,
                        ID: null,
                        ServiceID: null,
                        ClientID: null,
                        ShowWrittenOff: false,
                        SelectOnlyOne: true,
                        IsConfigurationUnitAgentForm: true,
                        ConfigurationUnitAgentTypeID: self.selectedTypeItem().ID,
                    }, function (newValues) {
                        if (!newValues || newValues.length == 0)
                            return;
                        //
                        var cu = self.configurationUnit();
                        cu.ObjectClassID(newValues[0].ClassID);
                        cu.ObjectID(newValues[0].ID);
                        cu.FullName(newValues[0].Name ? newValues[0].Name : newValues[0].Type + " " + newValues[0].Model);
                        cu.OrganizationName(newValues[0].Organization);
                        cu.BuildingName(newValues[0].Building);
                        cu.RoomName(newValues[0].Room);
                        cu.WorkPlaceName(newValues[0].WorkPlace);
                        //
                        if (!cu.Name())
                            cu.Name(newValues[0].Name);
                    });
                });
            };
            //
            self.validate = function () {
                if (!self.selectedTypeItem()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ConfigurationUnit_TypeMustBeSet'));
                    });
                    return false;
                }
                //
                if (!self.configurationUnit().ObjectID()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('DeviceMustBeSet'));
                    });
                    return false;
                }
                //
                if (!self.configurationUnit().Name()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('MustSetName'));
                    });
                    return false;
                }
                //
                return true;
            };
            //
            self.LoadConfigurationUnit = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_ConfigurationUnit.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetConfigurationUnit'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var cu = newVal.ConfigurationUnit;
                                if (cu) {
                                    require(['ui_forms/Asset/ConfigurationUnit'], function (cuLib) {
                                        self.configurationUnit(new cuLib.ConfigurationUnit(self, cu));
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
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[frmConfigurationUnit.js, Load]', 'error');
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
            self.DeviceIsEmpty = ko.computed(function () {
                var cu = self.configurationUnit();
                return cu ? !cu.ObjectID() : true;
            });
            //
            self.CanEditDevice = ko.computed(function () {
                return self.selectedTypeItem() != null && self.CanSelectNewDevice();
            });
            //
            self.EditName = function () {
                if (!self.CanEdit() || !self.configurationUnit().CanEditName())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
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
            self.EditSerialNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'SerialNumber',
                        fieldFriendlyName: getTextResource('Asset_SerialNumber'),
                        oldValue: asset.SerialNumber(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.SerialNumber(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
             self.EditInfrastructureSegment = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'InfrastructureSegment',
                        fieldFriendlyName: getTextResource('InfrastructureSegment'),
                        oldValue: { ID: asset.InfrastructureSegmentID(), ClassID: 366, FullName: asset.InfrastructureSegmentName() },
                        searcherName: 'InfrastructureSegmentSearcher',
                        onSave: function (objectInfo) {
                            asset.InfrastructureSegmentID(objectInfo ? objectInfo.ID : null);
                            asset.InfrastructureSegmentName(objectInfo ? objectInfo.FullName : '');
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.EditAdministrator = function () {
                if (!self.CanEdit() && self.parentObj() != null)
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Administrator',
                        fieldFriendlyName: getTextResource('Administrator'),
                        oldValue: asset.AdministratorLoaded() ? { ID: asset.Administrator().ID(), ClassID: asset.Administrator().ClassID(), FullName: asset.Administrator().FullName() } : null,
                        object: ko.toJS(asset.Administrator()),
                        searcherName: 'AssetAdministratorSearcher',
                        searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                        onSave: function (objectInfo) {
                            asset.AdministratorLoaded(false);
                            asset.Administrator(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditAdministrator, false, false));
                            //
                            asset.AdministratorID(objectInfo ? objectInfo.ID : '');
                            asset.AdministratorClassID(objectInfo ? objectInfo.ClassID : '');
                            self.InitializeAdministrator();
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            //открыть форму просмотра связанной лицензии
            self.ShowDeviceForm = function () {
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var cu = self.configurationUnit();
                    if (cu.AgentClassID() == 415 ||
                        cu.AgentClassID() == 416 ||
                        cu.AgentClassID() == 417 ||
                        cu.AgentClassID() == 418 ||
                        cu.AgentClassID() == 12)
                        fh.ShowLogicalObjectForm(cu.ObjectID());
                    else
                        fh.ShowAssetForm(cu.ObjectID(), cu.ObjectClassID());
                });
            };
            //
            self.InitializeAdministrator = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.configurationUnit();
                    if (a.AdministratorLoaded() == false && a.AdministratorID()) {
                        var type = null;
                        if (a.AdministratorClassID() == 9) {//IMSystem.Global.OBJ_USER
                            type = userLib.UserTypes.utilizer;
                        }
                        else if (a.AdministratorClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                            type = userLib.UserTypes.queueExecutor;
                        }
                        //
                        var options = {
                            UserID: a.AdministratorID(),
                            UserType: type,
                            UserName: null,
                            EditAction: null,
                            RemoveAction: null,
                            //ShowLeftSide: false,
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        a.Administrator(user);
                        a.AdministratorLoaded(true);
                    }
                });
            };
            //
            //for comboBox
            {
                self.createComboBoxItem = function (simpleDictionary) {
                    var thisObj = this;
                    //
                    thisObj.ID = simpleDictionary.ID;
                    thisObj.Name = simpleDictionary.Name;
                };
                //
                self.createComboBoxHelper = function (container_selector, getUrl, comboBoxFunc) {
                    var thisObj = this;
                    if (!comboBoxFunc)
                        comboBoxFunc = self.createComboBoxItem;
                    //
                    thisObj.SelectedItem = ko.observable(null);
                    //
                    thisObj.ItemList = ko.observableArray([]);
                    thisObj.ItemListD = $.Deferred();
                    thisObj.getItemList = function (options) {
                        var data = thisObj.ItemList();
                        options.callback({ data: data, total: data.length });
                    };
                    //
                    thisObj.ajaxControl = new ajaxLib.control();
                    thisObj.LoadList = function () {
                        thisObj.ajaxControl.Ajax($(container_selector),
                            {
                                url: getUrl,
                                method: 'GET'
                            },
                            function (response) {
                                if (response) {
                                    thisObj.ItemList.removeAll();
                                    //
                                    $.each(response, function (index, simpleDictionary) {
                                        var u = new comboBoxFunc(simpleDictionary);
                                        thisObj.ItemList().push(u);
                                    });
                                    thisObj.ItemList.valueHasMutated();
                                }
                                thisObj.ItemListD.resolve();
                            });
                    };
                    //
                    thisObj.GetObjectInfo = function (classID) {
                        return thisObj.SelectedItem() ? { ID: thisObj.SelectedItem().ID, ClassID: classID, FullName: thisObj.SelectedItem().Name } : null;
                    };
                    thisObj.SetSelectedItem = function (name) {
                        $.when(thisObj.ItemListD).done(function () {
                            var item = null;
                            if (name != undefined && name != null)
                                for (var i = 0; i < thisObj.ItemList().length; i++) {
                                    var tmp = thisObj.ItemList()[i];
                                    if (tmp.Name == name) {
                                        item = tmp;
                                        break;
                                    }
                                }
                            thisObj.SelectedItem(item);
                        });
                    };
                }
            }
            //
            self.EditIPAddress = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
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
            self.EditDescription = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Description',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: asset.Description(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.Description(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditNote = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Note',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_Comment'),
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
            self.EditDataCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'DataCenter',
                        fieldFriendlyName: getTextResource('DataCenter'),
                        oldValue: asset.DataCenter(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.DataCenter(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditVCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'VCenter',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_VCenter'),
                        oldValue: asset.VCenter(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.VCenter(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditCluster = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Cluster',
                        fieldFriendlyName: getTextResource('Cluster'),
                        oldValue: { ID: asset.ClusterID(), FullName: asset.ClusterName() },
                        searcherName: 'ClusterSearcher',
                        searcherPlaceholder: getTextResource('Cluster'),
                        searcherParams: [420, true],
                        onSave: function (objectInfo) {
                            asset.ClusterID(objectInfo ? objectInfo.ID : null);
                            asset.ClusterName(objectInfo ? objectInfo.FullName : '');
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.CheckShowDescriptionTooltipMessage = function (obj, event) {
                if (obj && event) {
                    var $this = $(event.currentTarget);
                    //
                    var hiddenElement = $this.find('span:first').clone().appendTo('body');
                    var nameWidth = hiddenElement.width();
                    hiddenElement.remove();
                    //
                    var areaWidth = $('.asset-text-field.asset-description').width();
                    //
                    if (!nameWidth || !areaWidth)
                        return;
                    //
                    if (nameWidth > areaWidth) {
                        var ttcontrol = new tclib.control();
                        ttcontrol.init($this, { text: self.configurationUnit().Description(), showImmediat: true, showTime: false });
                    }
                }
                return true;
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
                            if (self.attachmentsControl.ObjectID != self.configurationUnit().ObjectID())//previous object  
                                self.attachmentsControl.RemoveUploadedFiles();
                        }
                        if (self.attachmentsControl == null) {
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        }
                        self.attachmentsControl.ReadOnly(!self.CanEdit());
                        self.attachmentsControl.RemoveFileAvailable(self.CanEdit());
                        //
                        if (self.attachmentsControl.ObjectID != self.configurationUnit().ObjectID())
                            self.attachmentsControl.Initialize(self.configurationUnit().ObjectID());
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
        ShowDialog: function (device, isSpinnerActive, parentObj, callBackFunc) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            $.when(userD, operationIsGrantedD(953)).done(function (user, operation_update) {//OPERATION_ConfigurationUnit_Update = 953
                var isReadOnly = false;
                if (user.HasRoles == false || operation_update == false)
                    isReadOnly = true;
                //
                var frm = undefined;
                //
                //
                var buttons = [];
                var bAdd = {
                    text: getTextResource('Add'),
                    click: function () {
                        if (!vm.validate())
                            return;

                        vm.CreateConfigurationUnit();

                        forceClose = true;
                        frm.Close();
                    }
                }
                //
                var bCancel = {
                    text: getTextResource('ButtonCancel'),
                    click: function () { frm.Close(); }
                }
                buttons.push(bAdd);
                buttons.push(bCancel);
                //
                var vm = null;
                //
                frm = new fc.control(
                    'region_configurationUnitForm_',//form region prefix
                    'setting_configurationUnitForm_',//location and size setting
                    getTextResource('Asset_Properties'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    600, 370,//minSize
                    buttons,//form buttons
                    function () {
                        vm.DisposeAttacmentControl();
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmConfigurationUnitRegistration\', afterRender: AfterRender}"'//attributes of form region
                    );
                //
                if (!frm.Initialized) {
                    hideSpinner();
                    //
                    //var wnd = window.open(window.location.protocol + '//' + window.location.host + location.pathname + '?configurationUnitID=' + id);
                    //if (wnd) //browser cancel it?  
                    //    return;
                    ////
                    //require(['sweetAlert'], function () {
                    //    swal(getTextResource('OpenError'), getTextResource('CantDuplicateForm'), 'warning');
                    //});
                    //return;
                }
                //
                frm.BeforeClose = function () {
                    hideSpinner();
                };
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
                    vm.SizeChanged();
                };
                //
                ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                //
                $.when(frm.Show()).done(function (frmD, loadD) {
                    vm.Load(device);
                    hideSpinner();
                });
            });
        }
    }
    return module;
});