define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/AssetForms/AssetFields', 'models/AssetForms/ReferenceControl', 'models/AssetForms/AssetReferenceControl', 'ui_forms/Asset/Controls/AdapterList', 'ui_forms/Asset/Controls/PeripheralList', 'ui_forms/Asset/Controls/PortList', 'ui_forms/Asset/Controls/SlotList', 'ui_forms/Asset/Controls/SoftwareInstallationList', 'models/AssetForms/AssetForm.History', 'parametersControl'], function (ko, $, ajaxLib, fc, fhModule, tclib, assetFields, referenceControl, assetReferenceControl, adapterList, peripheralList, portList, slotList, installationList, assetHistory, pcLib) {
    var module = {
        ViewModel: function (isReadOnly, $region, classID) {
            var self = this;
            self.$region = $region;
            //
            self.asset = ko.observable(null);
            //
            self.objectClassID = classID;
            self.networkDeviceModel = ko.observable(null);
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
                if (!self.asset())
                    return;
                //
                var tabHeight = self.$region.height();//form height
                tabHeight -= 140;
                //
                var tabWidth = self.$region.width();//form width
                tabWidth -= self.$region.find('.b-requestDetail-right').outerWidth(true);
                //
                if (self.mode() == self.modes.adapter_peripheral_List)
                    self.TabHeight(Math.max(0, tabHeight - 30) + 'px');
                else if (self.mode() == self.modes.port_slot_List)
                    self.TabHeight(Math.max(0, tabHeight - 50) + 'px');
                else if (self.mode() == self.modes.installationList)
                    self.TabHeight(Math.max(0, tabHeight - 14) + 'px');
                else
                    self.TabHeight(Math.max(0, tabHeight - 10) + 'px');
                self.TabWidth(Math.max(0, tabWidth - 5) + 'px');
                //
                if (self.adapter_peripheral_Control() && self.adapter_peripheral_Control().ResetHeight)
                    self.adapter_peripheral_Control().ResetHeight();
                //
                if (self.port_slot_Control() && self.port_slot_Control().IsLoaded)
                    self.port_slot_Control().ResetHeight();
                if (self.installation_Control() && self.installation_Control().IsLoaded)
                    self.installation_Control().ResetHeight();
            };
            //
            self.IsReadOnly = ko.observable(isReadOnly);
            self.CanEdit = ko.computed(function () {
                if (!self.asset())
                    return false;
                if (self.asset().LifeCycleStateName() === 'Списано')
                    return false;
                //
                return !self.IsReadOnly();
            });
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                utility: 'utility',
                adapter_peripheral_List: 'adapter_peripheral_List',
                port_slot_List: 'port_slot_List',
                installationList: 'installationList',
                model: 'model',
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
            self.AssetOperationControl = ko.observable(null);
            self.LoadAssetOperationControl = function () {
                if (!self.asset())
                    return;
                //
                require(['assetOperations'], function (wfLib) {
                    if (self.AssetOperationControl() == null) {
                        self.AssetOperationControl(new wfLib.control(self.$region, self.asset, self.Load));
                    }
                    self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                    self.AssetOperationControl().Initialize();
                });
            };
            //
            //
            //tabs BLOCK
            self.assetFields = new assetFields.AssetFields(self.asset, self.$region, self.CanEdit);
            //
            //tab configuration
            {
                self.adapter_peripheral_Control = ko.observable(null);
                //
                self.InitializeConfiguration = function () {
                    if (self.adapter_peripheral_Control() != null)
                        return;
                    //
                    if (self.objectClassID == 5 || self.objectClassID == 6)//Obj_NetworkDevice, Obj_TerminalDevice
                    {
                        self.adapterList = new adapterList.LinkList(self, self.$region.find('.adapterList').selector, self.CanEdit);
                        self.peripheralList = self.asset().ShowPeripherals() ? new peripheralList.LinkList(self, self.$region.find('.peripheralList').selector, self.CanEdit) : null;
                        //
                        var configurationList = [];
                        configurationList.push({ List: self.adapterList, TemplateName: '../UI/Forms/Asset/Controls/AdapterList' });
                        if (self.peripheralList)
                            configurationList.push({ List: self.peripheralList, TemplateName: '../UI/Forms/Asset/Controls/PeripheralList' });
                        //
                        self.adapter_peripheral_Control(new referenceControl.Control
                            (
                                configurationList,
                                self.GetTabSize
                            ));
                    }
                };
            }
            //tab portList & slotList
            {
                self.port_slot_Control = ko.observable(null);
                //
                self.Initialize_Ports_Slots = function () {
                    if (self.port_slot_Control() != null)
                        return;
                    //
                    if (self.asset().ClassID() == 5 && self.asset().IsLogical())
                        return;
                    //
                    self.portList = self.asset().ShowPorts() ? new portList.LinkList(self.asset, self.$region.find('.portList').selector, self.CanEdit) : null;
                    self.slotList = self.asset().ShowSlots() ? new slotList.LinkList(self.asset, self.$region.find('.slotList').selector, self.CanEdit) : null;
                    //
                    var port_slot_list = [];
                    if (self.portList)
                        port_slot_list.push({ List: self.portList, TemplateName: '../UI/Forms/Asset/Controls/PortList' });
                    if (self.slotList)
                        port_slot_list.push({ List: self.slotList, TemplateName: '../UI/Forms/Asset/Controls/SlotList' });
                    //
                    self.port_slot_Control(new assetReferenceControl.Control
                        (
                            self.asset,
                            port_slot_list,
                            function () {
                                var retD = $.Deferred();
                                $.when(self.portList.CheckData(), self.slotList.CheckData()).done(function () {
                                    retD.resolve();
                                });
                                return retD.promise();
                            },
                            '.port-slot-list',
                            self.GetTabSize
                        ));
                    //
                    self.port_slot_Control().Initialize();
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
                    if (!self.asset().ShowInstallations())
                        return;
                    //
                    self.installationList(new installationList.LinkList(self.asset, self.$region.find('.softwareInstallationList').selector, self.CanEdit));
                    self.installation_Control(new assetReferenceControl.Control
                        (
                            self.asset,
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
            //self.assetModel = new assetModel.AssetModel(self.asset, self.IsReadOnly, self.CanEdit);
            self.assetModel = function () {
                return null;
            };
            //
            self.assetHistory = new assetHistory.Tape(self.asset, self.objectClassID, self.$region.find('.assetHistory .tabContent').selector);
            //
            //MAIN TAB BLOCK
            self.mode = ko.observable(self.modes.main);
            self.mode.subscribe(function (newValue) {
                if (newValue == self.modes.nothing)
                    return;
                //
                if (newValue == self.modes.main) {
                }
                else if (newValue == self.modes.utility) {
                    self.assetFields.Initialize();
                }
                else if (newValue == self.modes.adapter_peripheral_List) {
                    self.InitializeConfiguration();
                }
                else if (newValue == self.modes.port_slot_List) {
                    self.Initialize_Ports_Slots();
                }
                else if (newValue == self.modes.installationList) {
                    self.InitializeInstallationControl();
                }
                else if (newValue == self.modes.model) {
                    self.assetModel.Initialize(self.asset.ModetIntID);
                }
                else if (newValue == self.modes.history)
                    self.assetHistory.CheckData();
                else if (newValue.indexOf(self.parameterModePrefix) != -1) {
                    self.InitializeParametersTabs();
                }
                else if (newValue == self.modes.attachments) {
                    self.InitializeAttachmentControl();
                }
                self.SizeChanged();
            });
            //
            self.UtilityClick = function () {
                self.mode(self.modes.utility);
            };
            self.Adapter_peripheral_ListClick = function () {
                self.mode(self.modes.adapter_peripheral_List);
            };
            self.Port_slot_ListClick = function () {
                self.mode(self.modes.port_slot_List);
            };
            self.InstallationListClick = function () {
                self.mode(self.modes.installationList);
            };
            //
            self.ModelClick = function () {
                self.mode(self.modes.model);
            };
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
            self.Load = function (id, classID) {
                var retD = $.Deferred();
                $.when(self.LoadDevice(id, classID)).done(function (isLoaded) {
                    retD.resolve(isLoaded);
                    if (isLoaded) {
                        //
                        self.objectClassID = classID;
                        //
                        self.InitializeUtilizer();
                        //
                        if (classID != 33 && classID != 34) {
                            self.CsModelHelper.LoadList();
                            self.CsModelHelper.SetSelectedItem(self.asset().CsModel());
                            self.CsFormFactorHelper.LoadList();
                            self.CsFormFactorHelper.SetSelectedItem(self.asset().CsFormFactor());
                        }
                        //
                        if (classID == 5) {
                            self.InitializeModel();
                            self.InitializeAdministrator();
                        }
                        else if (classID == 33)
                            self.InitializeSubdeviceParameterList();
                        else if (classID == 34) {
                            self.InitializeSubdeviceParameterList();
                            self.InitializePeripheralModel();
                        }
                        //
                        self.SetOnStore();
                        //
                        self.OnParametersChanged(false);
                        //
                        self.LoadAssetOperationControl();
                        //
                        self.SizeChanged();
                    }
                });
                //
                return retD.promise();
            };
            //
            self.ajaxControl_NetworkDevice = new ajaxLib.control();
            self.LoadDevice = function (id, classID) {
                var retD = null;
                if (classID == 5)
                    retD = self.LoadNetworkDevice(id);
                else if (classID == 6)
                    retD = self.LoadTerminalDevice(id, classID);
                else if (classID == 33)
                    retD = self.LoadAdapter(id, classID);
                else if (classID == 34)
                    retD = self.LoadPeripheral(id, classID);
                else {
                    alert(classID);
                    hideSpinner();
                }
                return retD;
            };
            self.LoadNetworkDevice = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_NetworkDevice.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/sdApi/GetNetworkDevice'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var nd = newVal.NetworkDevice;
                                if (nd) {
                                    require(['models/AssetForms/AssetForm.NetworkDevice'], function (ndLib) {
                                        self.asset(new ndLib.NetworkDevice(self, nd));
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
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[NetworkDeviceForm.js, Load]', 'error');
                                });
                            }
                        });
                    });
                //
                return retD.promise();
            };
            //
            self.LoadTerminalDevice = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_NetworkDevice.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/sdApi/GetTerminalDevice'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var nd = newVal.TerminalDevice;
                                if (nd) {
                                    require(['models/AssetForms/AssetForm.TerminalDevice'], function (tdLib) {
                                        self.asset(new tdLib.TerminalDevice(self, nd));
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
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[NetworkDeviceForm.js, Load]', 'error');
                                });
                            }
                        });
                    });
                //
                return retD.promise();
            };

            self.LoadAdapter = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_NetworkDevice.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/sdApi/GetAdapter'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var ad = newVal.Adapter;
                                if (ad) {
                                    require(['models/AssetForms/AssetForm.Adapter'], function (adLib) {
                                        self.asset(new adLib.Adapter(self, ad));
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
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[NetworkDeviceForm.js, Load]', 'error');
                                });
                            }
                        });
                    });
                //
                return retD.promise();
            };

            self.LoadPeripheral = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_NetworkDevice.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/sdApi/GetPeripheral'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var p = newVal.Peripheral;
                                if (p) {
                                    require(['models/AssetForms/AssetForm.Peripheral'], function (pLib) {
                                        self.asset(new pLib.Peripheral(self, p));
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
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[NetworkDeviceForm.js, Load]', 'error');
                                });
                            }
                        });
                    });
                //
                return retD.promise();
            };


            self.ajaxControl_Model = new ajaxLib.control();
            self.InitializeModel = function () {
                var retD = $.Deferred();
                //
                var data = { 'ID': self.asset().ProductCatalogModelID() };
                self.ajaxControl_Model.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/imApi/GetNetworkDeviceModel'
                    },
                    function (newVal) {
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var model = newVal.NetworkDeviceModel;
                                if (model) {
                                    require(['models/AssetForms/NetworkDeviceModel'], function (modelLib) {
                                        self.networkDeviceModel(new modelLib.Model(model));
                                        retD.resolve();
                                    });
                                }
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                });
                            }
                        }
                    });
                //
                return retD.promise();
            };

            self.InitializePeripheralModel = function () {
                var retD = $.Deferred();
                //
                var data = { 'ID': self.asset().ProductCatalogModelID() };
                self.ajaxControl_Model.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/imApi/GetPeripheralModel'
                    },
                    function (newVal) {
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var model = newVal.PeripheralModel;
                                if (model) {
                                    require(['models/AssetForms/PeripheralModel'], function (modelLib) {
                                        self.networkDeviceModel(new modelLib.Model(model));
                                        retD.resolve();
                                    });
                                }
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                });
                            }
                        }
                    });
                //
                return retD.promise();
            };

            //
            self.ajaxControl_SubdeviceParameterList = new ajaxLib.control();
            self.SubDeviceParameterList = ko.observableArray([]);
            self.SubDeviceParameterList.subscribe(function (newValue) {
                if (self.asset().IsLogical())
                    ko.utils.arrayForEach(newValue, function (item) {
                        if (item.SubdeviceParameterType == 49)//SubdeviceParameterType.Storage_StorageID
                            self.asset().FullLocation(item.SubdeviceParameterValue);
                    });
            });
            self.InitializeSubdeviceParameterList = function () {
                var retD = $.Deferred();
                //
                var asset = self.asset();
                var data =
                {
                    DeviceClassID: asset.ClassID(),
                    DeviceID: asset.ID(),
                    ProductCatalogTemplateID: asset.ProductCatalogTemplateID(),
                };
                self.ajaxControl_SubdeviceParameterList.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/imApi/GetSubdeviceParameterList'
                    },
                    function (newVal) {
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var list = newVal.SubDeviceParameterList;
                                if (list) {
                                    self.SubDeviceParameterList.removeAll();
                                    ko.utils.arrayForEach(list, function (item) {
                                        self.SubDeviceParameterList.push(
                                            {
                                                SubdeviceParameterType: item.SubdeviceParameterType,
                                                SubdeviceParameterFriendlyName: ko.observable(item.SubdeviceParameterFriendlyName),
                                                SubdeviceParameterValue: ko.observable(item.SubdeviceParameterValue),
                                            });
                                    });
                                    self.SubDeviceParameterList.valueHasMutated();
                                }
                            }
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                            });
                        }
                    });
                //
                return retD.promise();
            };
            //
            //PARAMETERS BLOCK
            self.parametersControl = null;
            self.parameterListByGroup = null;//кеш отсортирортированных параметров, разбитых по группам
            self.parameterModePrefix = 'parameter_';
            self.ParameterList = ko.observable([]);//параметры текущей выбранной группы
            self.ParameterListGroupName = ko.computed(function () {
                if (self.mode().indexOf(self.parameterModePrefix) != 0)
                    return '';
                //
                var groupName = self.mode().substring(self.parameterModePrefix.length);
                return groupName;
            });
            self.ParameterGroupList = ko.observable([]);
            self.InitializeParametersTabs = function () {
                if (self.mode().indexOf(self.parameterModePrefix) != 0) {
                    self.ParameterList([]);
                    return;
                }
                //
                var groupName = self.mode().substring(self.parameterModePrefix.length);
                var list = self.parameterListByGroup;
                for (var i = 0; i < list.length; i++)
                    if (list[i].GroupName == groupName) {
                        self.ParameterList(list[i].ParameterList);
                        return;
                    }
                //
                self.ParameterList([]);//groupName not found
            };
            self.InitializeParameters = function (recalculateParameters) {
                var asset = self.asset();
                if (!asset)
                    self.parametersControl.InitializeOrCreate(null, null, null, false);//нет объекта - нет параметров
                else
                    self.parametersControl.InitializeOrCreate(asset.ClassID(), asset.ID(), asset, recalculateParameters);
            };
            self.OnParametersChanged = function (recalculateParameters) {//обновления списка параметров по объекту
                if (self.parametersControl == null) {
                    self.parametersControl = new pcLib.control();
                    self.parametersControl.ReadOnly(!self.CanEdit());
                    self.parametersControl.ParameterListByGroup.subscribe(function (newValue) {//изменилась разбивка параметров по группам
                        self.parameterListByGroup = newValue;
                        //
                        self.ParameterGroupList().splice(0, self.ParameterGroupList().length);//remove without ko update
                        var newParameterListContainsOldGroup = false;
                        //
                        for (var i = 0; i < newValue.length; i++) {
                            var groupName = newValue[i].GroupName;
                            //
                            self.ParameterGroupList().push({
                                Index: i + 1,
                                Name: groupName,
                                IsValid: newValue[i].IsValid
                            });
                            //
                            if (self.mode().indexOf(self.parameterModePrefix + groupName) != -1)
                                newParameterListContainsOldGroup = true;
                        }
                        self.ParameterGroupList.valueHasMutated();
                        //
                        if (self.mode().indexOf(self.parameterModePrefix) == 0) {//сейчас вкладка параметры
                            if (!newParameterListContainsOldGroup)
                                self.mode(self.modes.main);//такой больше нет, идем на главную
                            else {//hack ko
                                var tmp = self.mode();
                                self.mode(self.modes.nothing);
                                self.mode(tmp);
                            }
                        }
                    });
                }
                self.InitializeParameters(recalculateParameters);
            };
            //
            self.AfterRender = function () {
                self.SizeChanged();
            };
            //
            self.IsIdentifiersContainerVisible = ko.observable(true);
            self.ToggleIdentifiersContainer = function () {
                self.IsIdentifiersContainerVisible(!self.IsIdentifiersContainerVisible());
            };
            //
            self.IsLocationContainerVisible = ko.observable(true);
            self.ToggleLocationContainer = function () {
                self.IsLocationContainerVisible(!self.IsLocationContainerVisible());
            };
            //
            self.IsUtilizerContainerVisible = ko.observable(true);
            self.ToggleUtilizerContainer = function () {
                self.IsUtilizerContainerVisible(!self.IsUtilizerContainerVisible());
            };
            //
            self.IsClassifierContainerVisible = ko.observable(true);
            self.ToggleClassifierContainer = function () {
                self.IsClassifierContainerVisible(!self.IsClassifierContainerVisible());
            };
            //
            self.IsCharacteristicsContainerVisible = ko.observable(true);
            self.ToggleCharacteristicsContainer = function () {
                self.IsCharacteristicsContainerVisible(!self.IsCharacteristicsContainerVisible());
            };
            //
            self.IsNoteVisible = ko.observable(true);
            self.ToggleNoteContainer = function () {
                self.IsNoteVisible(!self.IsNoteVisible());
            };
            //
            self.EditName = function () {
                var asset = self.asset();
                if (!self.CanEdit() || !self.asset().CanEditName())
                    return;
                //
                if ((self.asset().ClassID() == 5 || self.asset().ClassID() == 6) && !self.asset().EditIPAddressName())
                    return;
                showSpinner();               
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'Name',
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
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'SerialNumber',
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
            self.EditInventoryNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'InventoryNumber',
                        fieldFriendlyName: getTextResource('Repair_InventoryNumber'),
                        oldValue: asset.InventoryNumber(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.InventoryNumber(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditAssetTag = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'AssetTag',
                        fieldFriendlyName: 'AssetTag',
                        oldValue: asset.AssetTag(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.AssetTag(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditIdentifier = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'Identifier',
                        fieldFriendlyName: 'ID',
                        oldValue: asset.Identifier(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.Identifier(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditCode = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'Code',
                        fieldFriendlyName: getTextResource('Code'),
                        oldValue: asset.Code(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.Code(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.ShowConfigurationUnitForm = function () {
                var asset = self.asset();
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
            self.ajaxControl_location = new ajaxLib.control();
            self.EditLocation = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                //
                require(['ui_forms/Asset/frmAssetLocation', 'sweetAlert'], function (module) {
                    var locationInfo = null;//начальное местоположение
                    var locationType = null;//режим выбора местоположения
                    if (asset.ClassID() == 5) {
                        locationInfo = { ID: asset.RackID() ? asset.RackID() : asset.RoomID(), ClassID: asset.RackID() ? 4 : 3 };
                        locationType = module.LocationType.NetworkDeviceLocation;
                    }
                    else if (asset.ClassID() == 6) {
                        locationInfo = { ID: asset.WorkPlaceID() ? asset.WorkPlaceID() : asset.RoomID(), ClassID: asset.WorkPlaceID() ? 22 : 3 };
                        locationType = module.LocationType.TerminalDeviceLocation;
                    }
                    else if (asset.ClassID() == 33 || asset.ClassID() == 34) {
                        locationInfo = { ID: asset.RoomID() ? asset.RoomID() : asset.DeviceID(), ClassID: asset.RoomID() ? 3 : asset.DeviceClassID() };
                        locationType = module.LocationType.SubdeviceLocation;
                    }
                    //
                    var onLocationChanged = function (objectInfo) {//когда новое местоположение будет выбрано
                        if (!objectInfo)
                            return;
                        self.ajaxControl_location.Ajax(self.$region.find('.network-device-location-header'),
                            {
                                url: '/imApi/GetAssetLocationInfo',
                                method: 'POST',
                                data: {
                                    DeviceID: asset.ID(),
                                    DeviceClassID: asset.ClassID(),
                                    LocationID: objectInfo.ID,
                                    LocationClassID: objectInfo.ClassID
                                }
                            },
                            function (response) {
                                if (response && response.Result === 0) {
                                    var info = response.AssetLocationInfo;
                                    //
                                    asset.OrganizationName(info.OrganizationName);
                                    asset.BuildingName(info.BuildingName);
                                    asset.RoomID(info.RoomID);
                                    asset.RoomName(info.RoomName);
                                    //
                                    if (asset.ClassID() == 5) {
                                        asset.RackID(info.RackID);
                                        asset.RackName(info.RackName);
                                        asset.RackPosition(info.RackLocation);
                                    }
                                    else if (asset.ClassID() == 6) {
                                        asset.WorkPlaceID(info.WorkPlaceID);
                                        asset.WorkPlaceName(info.WorkPlaceName);
                                    }
                                    else if (asset.ClassID() == 33 || asset.ClassID() == 34) {
                                        asset.DeviceClassID(info.DeviceClassID);
                                        asset.DeviceID(info.DeviceID);
                                        asset.DeviceName(info.DeviceName);
                                        asset.DeviceFullName(info.DeviceFullName);
                                        asset.RackID(info.RackID);
                                        asset.RackName(info.RackName);
                                        asset.WorkPlaceID(info.WorkPlaceID);
                                        asset.WorkPlaceName(info.WorkPlaceName);
                                    }
                                    //
                                    asset.OnStore(info.OnStore);
                                    self.SetOnStore();
                                }
                            });
                    };
                    var saveLocation = function (objectInfo, isReplaceAnyway) {//для сохранения нового местоположения
                        if (!objectInfo)
                            return;
                        //
                        var data = {
                            ID: asset.ID(),
                            ObjClassID: asset.ClassID(),
                            ClassID: null,
                            ObjectList: null,
                            Field: self.getClassName() + '.' + 'Location',
                            NewValue: JSON.stringify({ 'id': objectInfo.ID, 'fullName': '' }),
                            OldValue: JSON.stringify({ 'id': locationInfo.ID, 'fullName': '' }),
                            Params: ['' + objectInfo.ClassID, ''],
                            ReplaceAnyway: isReplaceAnyway == true ? true : false,
                        };
                        //
                        self.ajaxControl_location.Ajax(
                            self.$region.find('.network-device-location-header'),
                            {
                                dataType: "json",
                                method: 'POST',
                                url: '/sdApi/SetField',
                                data: data
                            },
                            function (retModel) {
                                if (retModel) {
                                    var result = retModel.ResultWithMessage.Result;
                                    var message = retModel.ResultWithMessage.Message;
                                    //
                                    if (result === 0) {
                                        onLocationChanged(objectInfo);
                                        $(document).trigger('local_objectUpdated', [asset.ClassID(), asset.ID(), null]);
                                    }
                                    else if (result === 1)
                                        swal(getTextResource('SaveError'), getTextResource('NullParamsError'), 'error');
                                    else if (result === 2)
                                        swal(getTextResource('SaveError'), getTextResource('BadParamsError'), 'error');
                                    else if (result === 3)
                                        swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                                    // 4 - is global error
                                    else if (result === 5 && isReplaceAnyway == false) {
                                        hideSpinner();//we start him in formHelper when clicked
                                        swal({
                                            title: getTextResource('SaveError'),
                                            text: getTextResource('ConcurrencyError'),
                                            showCancelButton: true,
                                            closeOnConfirm: true,
                                            closeOnCancel: true,
                                            confirmButtonText: getTextResource('ButtonOK'),
                                            cancelButtonText: getTextResource('ButtonCancel')
                                        },
                                            function (value) {
                                                if (value == true)
                                                    saveLocation(objectInfo, true);
                                            });
                                    }
                                    else if (result === 6)
                                        swal(getTextResource('SaveError'), getTextResource('ObjectDeleted'), 'error');
                                    else if (result === 7)
                                        swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                                    else if (result === 8)
                                        swal(getTextResource('SaveError'), message, 'info');
                                    else
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                }
                                else
                                    swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                            });
                    };
                    //
                    module.ShowDialog(locationType, locationInfo, saveLocation, true);
                });
            };
            //
            self.OnStoreCanEdit = ko.computed(function () {
                var asset = self.asset();
                if (!asset)
                    return false;
                //
                if (asset.ClassID() == 5 && asset.RackID())
                    return false;
                else if (asset.ClassID() == 6)
                    return false;
                else if (asset.ClassID() == 33 || asset.ClassID() == 34)
                    return false;
                //
                return true;
            });
            //
            self.getClassName = function () {
                var classID = self.asset().ClassID();
                //
                var className = '';
                if (classID == 5)
                    className = 'NetworkDevice';
                else if (classID == 6)
                    className = 'TerminalDevice';
                else if (classID == 33)
                    className = 'Adapter';
                else if (classID == 34)
                    className = 'Peripheral';
                //
                return className;
            };
            //
            self.EditCriticality = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'Criticality',
                        fieldFriendlyName: getTextResource('Criticality'),
                        oldValue: { ID: asset.CriticalityID(), ClassID: 367, FullName: asset.CriticalityName() },
                        searcherName: 'CriticalitySearcher',
                        onSave: function (objectInfo) {
                            asset.CriticalityID(objectInfo ? objectInfo.ID : null);
                            asset.CriticalityName(objectInfo ? objectInfo.FullName : '');
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.EditInfrastructureSegment = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'InfrastructureSegment',
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
            self.EditUtilizer = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    $.when(userD).done(function (user) {
                        var options = {
                            ID: asset.ID(),
                            objClassID: asset.ClassID(),
                            fieldName: self.getClassName() + '.' + 'Utilizer',
                            fieldFriendlyName: getTextResource('AssetNumber_UtilizerName'),
                            oldValue: asset.UtilizerLoaded() ? { ID: asset.Utilizer().ID(), ClassID: asset.Utilizer().ClassID(), FullName: asset.Utilizer().FullName() } : null,
                            object: ko.toJS(asset.Utilizer()),
                            searcherName: 'UtilizerSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [user.UserID],
                            onSave: function (objectInfo) {
                                asset.UtilizerLoaded(false);
                                asset.Utilizer(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditUtilizer, false, false));
                                //
                                asset.UtilizerID(objectInfo ? objectInfo.ID : '');
                                asset.UtilizerClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeUtilizer();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                });
            };
            //
            self.InitializeUtilizer = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.asset();
                    if (a.UtilizerLoaded() == false && a.UtilizerID()) {
                        var type = null;
                        if (a.UtilizerClassID() == 9) {//IMSystem.Global.OBJ_USER
                            type = userLib.UserTypes.utilizer;
                        }
                        else if (a.UtilizerClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                            type = userLib.UserTypes.queueExecutor;
                        }
                        else if (a.UtilizerClassID() == 101) {//IMSystem.Global.OBJ_ORGANIZATION
                            type = userLib.UserTypes.organization;
                        }
                        else if (a.UtilizerClassID() == 102) {//IMSystem.Global.OBJ_DIVISION
                            type = userLib.UserTypes.subdivision;
                        }
                        var options = {
                            UserID: a.UtilizerID(),
                            UserType: type,
                            UserName: null,
                            EditAction: self.EditUtilizer,
                            RemoveAction: null,
                            //ShowLeftSide: false,
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        a.Utilizer(user);
                        a.UtilizerLoaded(true);
                    }
                });
            };
            //
            self.EditAdministrator = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'Administrator',
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
            self.InitializeAdministrator = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.asset();
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
                            EditAction: self.EditAdministrator,
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
            self.EditManufacturer = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'CsManufacturer',
                        fieldFriendlyName: getTextResource('Maintenance_ManufacturerName'),
                        oldValue: { ID: asset.CsManufacturerID(), ClassID: 89, FullName: asset.CsManufacturerName() },
                        searcherName: 'ManufacturerSearcher',
                        searcherPlaceholder: getTextResource('Maintenance_ManufacturerName'),
                        searcherParams: ['false', 'false', 'false', 'false', 'true', 'false', 'false', 'false', 'false'],
                        onSave: function (objectInfo) {
                            asset.CsManufacturerID(objectInfo ? objectInfo.ID : null);
                            asset.CsManufacturerName(objectInfo ? objectInfo.FullName : '');
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
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
            self.CsModelHelper = new self.createComboBoxHelper($region.find('.csModel').selector, '/imApi/GetCsModelList');
            self.EditCsModel = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                var fh = new fhModule.formHelper(true);
                var options = {
                    ID: asset.ID(),
                    objClassID: asset.ClassID(),
                    fieldName: self.getClassName() + '.' + 'CsModel',
                    fieldFriendlyName: getTextResource('AssetCSModel'),
                    comboBoxGetValueUrl: '/imApi/GetCsModelList',
                    oldValue: self.CsModelHelper.SelectedItem() ? { ID: self.CsModelHelper.SelectedItem().ID, Name: self.CsModelHelper.SelectedItem().Name } : { ID: null, Name: asset.CsModel() },
                    maxLength: 50,
                    onSave: function (selectedValue) {
                        asset.CsModel(selectedValue ? selectedValue.Name : '');
                        self.CsModelHelper.SetSelectedItem(selectedValue ? selectedValue.Name : '');
                    },
                    readOnly: false
                };
                fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
            };
            //
            self.CsFormFactorHelper = new self.createComboBoxHelper($region.find('.csFormFactor').selector, '/imApi/GetCsFormFactorList');
            self.EditCsFormFactor = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                var fh = new fhModule.formHelper(true);
                var options = {
                    ID: asset.ID(),
                    objClassID: asset.ClassID(),
                    fieldName: self.getClassName() + '.' + 'CsFormFactor',
                    fieldFriendlyName: getTextResource('AssetCsFormFactor'),
                    comboBoxGetValueUrl: '/imApi/GetCsFormFactorList',
                    oldValue: self.CsFormFactorHelper.SelectedItem() ? { ID: self.CsFormFactorHelper.SelectedItem().ID, Name: self.CsFormFactorHelper.SelectedItem().Name } : { ID: null, Name: asset.CsFormFactor() },
                    maxLength: 50,
                    onSave: function (selectedValue) {
                        asset.CsFormFactor(selectedValue ? selectedValue.Name : '');
                        self.CsFormFactorHelper.SetSelectedItem(selectedValue ? selectedValue.Name : '');
                    },
                    readOnly: false
                };
                fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
            };
            //
            self.EditCsSize = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'CsSize',
                        fieldFriendlyName: getTextResource('CsSize'),
                        oldValue: asset.CsSize(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.CsSize(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditBIOSModel = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'BIOSModel',
                        fieldFriendlyName: getTextResource('AssetBiosModel'),
                        oldValue: asset.BIOSModel(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.BIOSModel(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditBIOSVersion = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'BIOSVersion',
                        fieldFriendlyName: getTextResource('AssetBiosVersion'),
                        oldValue: asset.BIOSVersion(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.BIOSVersion(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditCPUModel = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                var fh = new fhModule.formHelper(true);
                var options = {
                    ID: asset.ID(),
                    objClassID: asset.ClassID(),
                    fieldName: self.getClassName() + '.' + 'CPUModel',
                    fieldFriendlyName: getTextResource('ConfigurationUnit_CpuModel'),
                    comboBoxGetValueUrl: '/imApi/GetCPUModelList',
                    oldValue: asset.CPUModel() != null ? { ID: asset.CPUModel(), Name: asset.CPUModelName() } : {ID: null, Name: ''},
                    onSave: function (newValue) {
                        asset.CPUModel(newValue ? newValue.ID:'');
                        asset.CPUModelName(newValue ? newValue.Name : null);
                    },
                    readOnly: false
                };
                fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
            };
            //
            self.EditCPUClockFrequency = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'CPUClockFrequency',
                        fieldFriendlyName: getTextResource('ClockFrequency'),
                        oldValue: asset.CPUClockFrequency(),
                        maxValue: 99999999,//в бд decimal(10, 2)
                        onSave: function (newVal) {
                            asset.CPUClockFrequency(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditCPUCoreNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'CPUCoreNumber',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_CoreNumber'),
                        oldValue: asset.CPUCoreNumber(),
                        maxValue: 1024,
                        onSave: function (newVal) {
                            asset.CPUCoreNumber(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditCPUNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'CPUNumber',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_CpuNumber'),
                        oldValue: asset.CPUNumber(),
                        maxValue: 1024,
                        onSave: function (newVal) {
                            asset.CPUNumber(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditRAMSpace = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'RAMSpace',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_RAM'),
                        oldValue: asset.RAMSpace(),
                        maxValue: 99999999,//в бд decimal(10, 2)
                        stepperType: 'float',
                        floatPrecission: 2,
                        onSave: function (newVal) {
                            asset.RAMSpace(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditDiskType = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                //
                var fh = new fhModule.formHelper(true);
                var options = {
                    ID: asset.ID(),
                    objClassID: asset.ClassID(),
                    fieldName: self.getClassName() + '.' + 'DiskType',
                    fieldFriendlyName: getTextResource('DiskType'),
                    comboBoxGetValueUrl: '/imApi/GetDiskTypeList',
                    oldValue: asset.DiskType() != null ? { ID: asset.DiskType(), Name: asset.DiskTypeName() } : { ID: null, Name: '' },
                    onSave: function (newValue) {
                        asset.DiskType(newValue ? newValue.ID : '');
                        asset.DiskTypeName(newValue ? newValue.Name : null);
                    },
                    readOnly: false
                };
                fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
            };
            //
            //
            self.EditDiskSpaceTotal = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'DiskSpaceTotal',
                        fieldFriendlyName: getTextResource('TotalStorage'),
                        oldValue: asset.DiskSpaceTotal(),
                        maxValue: 99999999,//в бд decimal(10, 2)
                        stepperType: 'float',
                        floatPrecission: 2,
                        onSave: function (newVal) {
                            asset.DiskSpaceTotal(newVal);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.CPUAutoInfo = ko.observable(false);
            self.DiskAutoInfo = ko.observable(false);
            self.RAMAutoInfo = ko.observable(false);
            self.CPUAutoInfo.subscribe(function (newValue) {
                self.EditAutoSetAdapterField(newValue);
            });
            self.DiskAutoInfo.subscribe(function (newValue) {
                self.EditAutoSetAdapterField(newValue);
            });
            self.RAMAutoInfo.subscribe(function (newValue) {
                self.EditAutoSetAdapterField(newValue);
            });
            self.EditAutoSetAdapterField = function (newValue) {

                if (self.asset()==null)
                    return;

                var obj = self.asset();
                var data = {
                    ID: obj.ID(),
                    ClassID: obj.ClassID(),
                    CPUAutoInfo: self.CPUAutoInfo(),
                    DiskAutoInfo: self.DiskAutoInfo(),
                    RAMAutoInfo: self.RAMAutoInfo()
                };
                var ajaxinfo = new ajaxLib.control();
                ajaxinfo.Ajax(
                    self.$region,//self.$region, two spinner problem
                    {
                        dataType: "json",
                        method: 'POST',
                        url: '/imApi/GetDeviceAutoInfo',
                        data: data
                    },
                    function (retModel) {
                        if (retModel) {
                            //
                            if (retModel.Result === 0) {
                                var result = retModel.Info;
                                obj.CPUClockFrequency(result.CPUClockFrequency);
                                obj.CPUCoreNumber(result.CPUCoreNumber);
                                obj.CPUModel(result.CPUModel);
                                obj.CPUModelName(result.CPUModelName);
                                obj.CPUNumber(result.CPUNumber);
                                obj.DiskSpaceTotal(result.DiskSpaceTotal);
                                obj.DiskType(result.DiskType);
                                obj.DiskTypeName(result.DiskTypeName);
                                obj.RAMSpace(result.RAMSpace);
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                });
                            }
                            hideSpinner();
                        }
                    });
            };
            //
            self.EditPowerConsumption = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'PowerConsumption',
                        fieldFriendlyName: getTextResource('AssetPowerConsumption'),
                        oldValue: asset.PowerConsumption(),
                        maxValue: 10000,//в бд decimal(10, 2)
                        onSave: function (newVal) {
                            var newValStr = newVal.toString();
                            asset.PowerConsumption(newVal);
                            asset.PowerConsumptionStr(newValStr);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditDefaultPrinter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'DefaultPrinter',
                        fieldFriendlyName: getTextResource('Asset_DefaultPrinter'),
                        oldValue: asset.DefaultPrinter(),
                        allowNull: true,
                        maxLength: 50,
                        onSave: function (newText) {
                            asset.DefaultPrinter(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditIPAddress = function () {
                var asset = self.asset();
                if (!self.CanEdit() || !self.asset().EditIPAddressName())//если нет настройки "Использовать фоновую работу с узлами сети"
                    return;
               //
                showSpinner();                
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'IPAddress',
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
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'Description',
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
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'Note',
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
            self.EditSubDeviceParameter = function (data) {
                if (!self.CanEdit())
                    return;
                //
                var getMaxTextLength = function () {
                    if (data.SubdeviceParameterType == 28)/*NetworkAdapter_MACAddress*/
                        return 23;
                    else if (data.SubdeviceParameterType == 50)/*StorageController_WWN*/
                        return 23;
                    else
                        return 255;
                };
                //
                showSpinner();
                var asset = self.asset();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: asset.ClassID(),
                        fieldName: self.getClassName() + '.' + 'SubDeviceParameter',
                        fieldFriendlyName: data.SubdeviceParameterFriendlyName(),
                        oldValue: data.SubdeviceParameterValue(),
                        allowNull: true,
                        maxLength: getMaxTextLength(),
                        subdeviceParameterType: data.SubdeviceParameterType,
                        onSave: function (newText) {
                            data.SubdeviceParameterValue(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.SetOnStore = function () {
                if (!self.asset())
                    return;
                //
                var checkbox = $region.find('.network-device-onStorage');
                if (checkbox && checkbox[0])
                    checkbox[0].checked = self.asset().OnStore();
            };
            //
            self.EditOnStore = function () {
                if (!self.CanEdit())
                    return;
                //
                var checkbox = $region.find('.network-device-onStorage');
                //
                if (!checkbox || !checkbox[0])
                    return;
                //
                var oldValue = !checkbox[0].checked;
                //
                showSpinner();
                var asset = self.asset();
                asset.OnStore(checkbox[0].checked);

                var data = {
                    ID: asset.ID(),
                    ObjClassID: asset.ClassID(),
                    Field: self.getClassName() + '.' + 'OnStore',
                    OldValue: JSON.stringify({ 'val': oldValue }),
                    NewValue: JSON.stringify({ 'val': checkbox[0].checked }),
                    ReplaceAnyway: false
                };

                self.ajaxControl_NetworkDevice.Ajax(
                null,//self.$region, two spinner problem
                {
                    dataType: "json",
                    method: 'POST',
                    url: '/sdApi/SetField',
                    data: data
                },
                function (retModel) {
                    if (retModel) {
                        var result = retModel.ResultWithMessage.Result;
                        //
                        hideSpinner();
                        if (result === 0) {
                            $(document).trigger('local_objectUpdated', [asset.ClassID(), asset.ID(), null]);
                            checkbox[0].checked = asset.OnStore();
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                            });
                        }
                    }
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
                        ttcontrol.init($this, { text: self.asset().Description(), showImmediat: true, showTime: false });
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
                            if (self.attachmentsControl.ObjectID != self.asset().ID())//previous object  
                                self.attachmentsControl.RemoveUploadedFiles();
                        }
                        if (self.attachmentsControl == null) {
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        }
                        self.attachmentsControl.ReadOnly(!self.CanEdit());
                        self.attachmentsControl.RemoveFileAvailable(self.CanEdit());
                        //
                        if (self.attachmentsControl.ObjectID != self.asset().ID())
                            self.attachmentsControl.Initialize(self.asset().ID());
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
        ShowDialog: function (id, classID, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            var getOperationUpdate = function (classID) {
                if (classID == 5)//IMSystem.Global.OBJ_NETWORKDEVICE
                    return 233;
                else if (classID == 6)//IMSystem.Global.OBJ_TERMINALDEVICE
                    return 240;
                else if (classID == 33)//IMSystem.Global.OBJ_ADAPTER
                    return 242;
                else if (classID == 34)//IMSystem.Global.OBJ_PERIPHERAL
                    return 243;
            };
            //
            $.when(userD, operationIsGrantedD(getOperationUpdate(classID))).done(function (user, operation_update) {
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
                    'region_assetForm_' + classID,//form region prefix
                    'setting_assetForm_' + classID,//location and size setting
                    getTextResource('Asset_Properties'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    730, 540,//minSize
                    buttons,//form buttons
                    function () {
                        vm.DisposeAttacmentControl();
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmAsset\', afterRender: AfterRender}"'//attributes of form region
                    );
                //
                if (!frm.Initialized) {//form with that region and settingsName was open
                    hideSpinner();
                    //
                    switch (classID) {
                        case 5:
                            var url = window.location.protocol + '//' + window.location.host + location.pathname + '?networkDeviceID=' + id;
                            break;
                        case 6:
                            var url = window.location.protocol + '//' + window.location.host + location.pathname + '?terminalDeviceID=' + id;
                            break;
                        case 33:
                            var url = window.location.protocol + '//' + window.location.host + location.pathname + '?adapterID=' + id;
                            break;
                        case 34:
                            var url = window.location.protocol + '//' + window.location.host + location.pathname + '?peripheralID=' + id;
                            break;
                    //
                    };
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
                    if (vm.adapterList)
                        vm.adapterList.dispose();
                    if (vm.peripheralList)
                        vm.peripheralList.dispose();
                    hideSpinner();
                };
                //
                var $region = $('#' + frm.GetRegionID());
                vm = new module.ViewModel(isReadOnly, $region, classID);
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
                $.when(frm.Show(), vm.Load(id, classID)).done(function (frmD, loadD) {
                    if (loadD == false) {//force close
                        frm.Close();
                    }
                    else {
                        if (!ko.components.isRegistered('networkDeviceFormCaptionComponent'))
                            ko.components.register('networkDeviceFormCaptionComponent', {
                                template: '<span data-bind="text: $captionText"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'networkDeviceFormCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Properties\') + ' ' + getTextResource(\'NumberSymbol\') + $data.asset().SerialNumber() +', ' + getTextResource(\'Asset_InventoryNumberCaption\') + $data.asset().InventoryNumber() } }");
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});