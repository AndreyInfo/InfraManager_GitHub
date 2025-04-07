define(['knockout', 'ajax', 'dateTimeControl', 'typeHelper', './AssetFieldsRegistration'], function (ko, ajax, dtLib, typeHelper, assetFields) {
    var module = {
        assetRegistration: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            self.IsLoaded = false;
            //
            self.AssetFields = ko.observable(null);
            self.AssetFields(new assetFields.AssetFieldsRegistration());
            //            
            self.ID = ko.observable(null);
            self.ClassID = ko.observable(null);
            //
            self.SubDeviceParameterList = ko.observableArray([]);
            self.SubDeviceParameterList.subscribe(function (newValue) {
                if (self.IsLogical())
                    ko.utils.arrayForEach(newValue, function (item) {
                        if (item.SubdeviceParameterType == 49)//SubdeviceParameterType.Storage_StorageID
                            self.FullLocation(item.SubdeviceParameterValue);
                    });
            });
            //
            self.Note = ko.observable('');
            self.LifeCycleStateName = ko.observable(null);
            //
            self.CanEditName = ko.observable(true);
            //
            self.ShowIPAddress = ko.computed(function () {
                return false;
            });
            //
            self.DeviceID = ko.observable(null);
            self.DeviceClassID = ko.observable(5);
            self.RackID = ko.observable(0);
            //
            self.InventoryNumber = ko.observable('');
            //
            self.SerialNumber = ko.observable('');
            //
            self.Identifier = ko.observable('');
            //
            self.Code = ko.observable('');
            //
            self.OrganizationName = ko.observable('');
            //
            self.BuildingName = ko.observable('');
            //
            self.RoomID = ko.observable('');
            //
            self.RoomName = ko.observable('');
            //
            self.RackID = ko.observable('');
            //
            self.RackName = ko.observable('');
            //
            self.WorkPlaceID = ko.observable('');
            //
            self.WorkPlaceName = ko.observable('');
            //
            self.OnStore = ko.observable(false);
            //
            self.DeviceClassID = ko.observable('');
            //
            self.DeviceID = ko.observable('');
            //
            self.DeviceName = ko.observable('');
            //
            self.DeviceFullName = ko.observable('');
            //
            self.UtilizerID = ko.observable('');
            //
            self.UtilizerClassID = ko.observable('');
            self.UtilizerFullName = ko.observable('');
            //
            self.Integrated = ko.observable(false);
            self.IsLogical = ko.observable(false);
            //
            self.ShowModelCode = ko.computed(function () {
                return !self.Integrated() && !self.IsLogical();
            });
            //
            self.ShowInventoryNumber = ko.computed(function () {
                return !self.IsLogical() && !self.Integrated();
            });
            //
            self.ShowSerialNumber = ko.computed(function () {
                return true;
            });
            //
            self.ShowIdentifier = ko.computed(function () {
                return true;
            });
            //
            self.ShowAssetTag = ko.computed(function () {
                return false;
            });
            //
            self.ShowLocationBlock = ko.computed(function () {
                return true;
            });
            //
            self.ShowRackName = ko.computed(function () {
                return self.DeviceClassID() == 5 && self.RackID();
            });
            //
            self.ShowRackPosition = ko.computed(function () {
                return false;
            });
            //
            self.ShowWorkPlace = ko.computed(function () {
                return self.DeviceClassID() == 6 && self.WorkPlaceID();
            });
            //
            self.ShowDeviceName = ko.computed(function () {
                return self.DeviceID();
            });
            //
            self.ShowOnStore = ko.computed(function () {
                return !self.Integrated();
            });
            //
            self.ShowUtilization = ko.computed(function () {
                return !self.Integrated();
            });
            //
            self.ShowUtilizer = ko.computed(function () {
                return self.ShowUtilization();
            });
            //
            self.ShowManufacturerName = ko.computed(function () {
                return true;
            });
            //
            self.ShowDescription = ko.computed(function () {
                return false;
            });
            //
            self.ShowAssetCharacteristics = ko.computed(function () {
                return false;
            });
            //
            self.ShowPowerConsumption = ko.computed(function () {
                return false;
            });
            //
            self.ShowDefaultPrinter = ko.computed(function () {
                return false;
            });
            //
            self.ShowTotalSlotMemory = ko.computed(function () {
                return false;
            });
            //
            self.ShowCorpusCharacteristics = ko.computed(function () {
                return false;
            });
            //
            self.ShowAssetFields = ko.computed(function () {
                return self.LifeCycleStateName() && !self.Integrated();
            });
            //
            self.productCatalogID = ko.observable(null);
            self.productCatalogClassID = ko.observable(null);
            self.productCatalogFullName = ko.observable('');
            self.productCatalogTypeID = ko.pureComputed(function () {
                return self.productCatalogClassID() == 378 ? self.productCatalogID() : null;//OBJ_ProductCatalogType
            });
            //
            self.ProductCatalogCategoryName = ko.observable('');
            //
            self.ProductCatalogTemplateID = ko.observable('');
            //
            self.ProductCatalogTemplateName = ko.observable('');
            //
            self.ProductCatalogTypeName = ko.observable('');
            //
            self.ManufacturerName = ko.observable('');
            //
            self.ProductCatalogModelID = ko.observable(null);
            //
            self.ProductCatalogModelName = ko.observable('');
            //
            self.ProductCatalogModelCode = ko.observable('');
            //
            self.files = ko.observableArray([]);
            self.parameterValueList = ko.observableArray([]);
            //
            //
            self.dispose = function () {
                self.ajaxControl.Abort();
                self.productCatalogTypeID.dispose();
            };
            //
            self.register = function (showSuccessMessage) {
                var retval = $.Deferred();
                //
                var subdeviceParams = [];
                //
                ko.utils.arrayForEach(self.SubDeviceParameterList(), function (item) {
                    var param =
                        {
                            Type: item.SubdeviceParameterType,
                            Value: item.SubdeviceParameterValue()
                        };
                    subdeviceParams.push(param);
                });
                //                
                var device = {
                    'SerialNumber': self.SerialNumber(),
                    'Identifier': self.Identifier(),
                    'InventoryNumber': self.InventoryNumber(),
                    'Code': self.Code(),
                    'DeviceClassID': self.DeviceClassID(),
                    'DeviceID': self.DeviceID(),
                    'ProductCatalogModelID': self.ProductCatalogModelID(),
                    'Note': self.Note(),
                    'Files': self.files(),
                    'ParameterValueList': self.parameterValueList(),
                    'SubDeviceParameterList': subdeviceParams
                };

                var assetFields = {
                    'UtilizerID': self.UtilizerID(),
                    'UtilizerClassID': self.UtilizerClassID(),
                    'UtilizerFullName': self.UtilizerFullName(),
                    'UtcDateReceivedJS': dtLib.GetMillisecondsSince1970(self.AssetFields().DateReceived()),
                    'SupplierID': self.AssetFields().SupplierID(),
                    'Cost': self.AssetFields().Cost(),
                    'Document': self.AssetFields().Document(),
                    'OwnerID': self.AssetFields().OwnerID(),
                    'OwnerClassID': self.AssetFields().OwnerClassID(),
                    'OwnerFullName': self.AssetFields().OwnerFullName(),
                    'ServiceCenterID': self.AssetFields().ServiceCenterID(),
                    'ServiceContractID': self.AssetFields().ServiceContractID(),
                    'UserID': self.AssetFields().UserID(),
                    'Founding': self.AssetFields().Founding(),
                    'UtcAppointmentDateJS': dtLib.GetMillisecondsSince1970(self.AssetFields().AppointmentDate()),
                    'UtcWarrantyJS': dtLib.GetMillisecondsSince1970(self.AssetFields().Warranty()),
                    'UserFieldList': self.AssetFields().UserFieldList()
                };

                var data = {};
                var url = null;

                if (self.ClassID() == 33) {
                    data =
                    {
                        Adapter: device,
                        AssetFields: assetFields
                    };
                    url = '/assetApi/registerAdapter';
                }
                else if (self.ClassID() == 34) {
                    data =
                    {
                        Peripheral: device,
                        AssetFields: assetFields
                    };
                    url = '/assetApi/registerPeripheral';
                }
                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: url,
                        method: 'POST',
                        dataType: 'json',
                        data: data
                    },
                    function (response) {
                        hideSpinner();
                        if (response) {
                            if (response.Message && response.Message.length > 0 && (showSuccessMessage == true || response.Type != 0))
                                require(['sweetAlert'], function () {
                                    swal(response.Message);//some problem
                                });
                            //
                            if (response.Type == 0) {//ok 
                                retval.resolve(response);
                                return;
                            }
                        }
                        retval.resolve(null);
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[AssetRegistration.js, register]', 'error');
                        });
                        retval.resolve(null);
                    });
                //
                return retval.promise();
            };
            //
            //
            self.load = function (classID, locationID, locationClassID) {
                var retval = $.Deferred();
                self.ClassID(classID);
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/imApi/GetAssetLocationInfo',
                    method: 'POST',
                    data: {
                        DeviceID: null,
                        DeviceClassID: 34,
                        LocationID: locationID,
                        LocationClassID: locationClassID
                    }
                },
                function (response) {
                    if (response && response.Result === 0) {
                        var info = response.AssetLocationInfo;
                        //
                        self.OrganizationName(info.OrganizationName);
                        self.BuildingName(info.BuildingName);
                        self.RoomID(info.RoomID);
                        self.RoomName(info.RoomName);
                        //
                        if (self.ClassID() == 5) {
                            self.RackID(info.RackID);
                            self.RackName(info.RackName);
                            self.RackPosition(info.RackLocation);
                        }
                        else if (self.ClassID() == 6) {
                            self.WorkPlaceID(info.WorkPlaceID);
                            self.WorkPlaceName(info.WorkPlaceName);
                        }
                        else if (self.ClassID() == 33 || self.ClassID() == 34) {
                            self.DeviceClassID(info.DeviceClassID);
                            self.DeviceID(info.DeviceID);
                            self.DeviceName(info.DeviceName);
                            self.DeviceFullName(info.DeviceFullName);
                            self.RackID(info.RackID);
                            self.RackName(info.RackName);
                            self.WorkPlaceID(info.WorkPlaceID);
                            self.WorkPlaceName(info.WorkPlaceName);
                        }
                        //
                        self.OnStore(info.OnStore);
                        //self.SetOnStore();
                        self.IsLoaded = true;
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmAssetRegistration.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
        },
    };
    return module;
});