define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/AssetForms/AssetFields', 'models/AssetForms/ReferenceControl', 'models/AssetForms/AssetReferenceControl', 'ui_forms/Asset/Controls/AdapterList', 'ui_forms/Asset/Controls/PeripheralList', 'ui_forms/Asset/Controls/PortList', 'ui_forms/Asset/Controls/SlotList', 'ui_forms/Asset/Controls/SoftwareInstallationList', 'models/AssetForms/AssetForm.History', 'parametersControl'], function (ko, $, ajaxLib, fc, fhModule, tclib, assetFields, referenceControl, assetReferenceControl, adapterList, peripheralList, portList, slotList, installationList, assetHistory, pcLib) {
    var module = {
        ViewModel: function (isReadOnly, $region) {
            var self = this;
            self.$region = $region;
            //
            self.specification = ko.observable(null);
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
            self.UtilizerHeight = ko.observable(0);
            //
            self.SizeChanged = function () {
                if (!self.specification())
                    return;
                //
                var tabHeight = self.$region.height();//form height
                tabHeight -= 85;
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
                if (!self.specification())
                    return false;
                //
                var solution = self.specification().Solution();
                if (solution === 0 ||//Confirmed
                    solution === 2)  //DB_Updated
                    return false;
                //
                return !self.IsReadOnly();
            });
            //
            self.GetTabSize = function () {
                return {
                    h: parseInt(self.TabHeight().replace('px', '')),
                    w: parseInt(self.TabWidth().replace('px', ''))
                };
            };
            //
            self.Load = function (id) {
                var retD = $.Deferred();
                $.when(self.LoadInventorySpecification(id)).done(function (isLoaded) {
                    retD.resolve(isLoaded);
                    if (isLoaded) {
                        //
                        self.InitializeUtilizer();
                        //
                        self.SetOnStore();
                        self.SetNewOnStore();
                        //
                        self.SizeChanged();
                    }
                });
                //
                return retD.promise();
            };
            //
            self.ajaxControl = new ajaxLib.control();
            //
            self.LoadInventorySpecification = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetInventorySpecification'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var spec = newVal.InventorySpecification;
                                if (spec) {
                                    require(['ui_forms/SD/InventorySpecification'], function (specLib) {
                                        self.specification(new specLib.InventorySpecification(self, spec));
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
            self.AfterRender = function () {
                self.SizeChanged();
            };
            //
            //
            self.EditManufacturer = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var specification = self.specification();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: specification.ID(),
                        objClassID: specification.ClassID(),
                        fieldName: 'NewManufacturer',
                        fieldFriendlyName: getTextResource('Maintenance_ManufacturerName'),
                        oldValue: { ID: specification.NewManufacturerID(), ClassID: 89, FullName: specification.NewManufacturerName() },
                        searcherName: 'ManufacturerSearcher',
                        searcherPlaceholder: getTextResource('Maintenance_ManufacturerName'),
                        searcherParams: ['true', 'true', 'true', 'true', 'true', 'true', 'true', 'true', 'true'],
                        onSave: function (objectInfo) {
                            specification.NewManufacturerID(objectInfo ? objectInfo.ID : null);
                            specification.NewManufacturerName(objectInfo ? objectInfo.FullName : '');
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.ajaxControl_location = new ajaxLib.control();
            self.EditLocation = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var specification = self.specification();
                //
                require(['ui_forms/Asset/frmAssetLocation', 'sweetAlert'], function (module) {
                    var locationInfo = null;//начальное местоположение
                    var locationType = null;//режим выбора местоположения
                    if (specification.ObjectClassID() == 5) {
                        if (specification.NewRackID() || specification.NewRoomID())
                            locationInfo = { ID: specification.NewRackID() ? specification.NewRackID() : specification.NewRoomID(), ClassID: specification.NewRackID() ? 4 : 3 };
                        //
                        locationType = module.LocationType.NetworkDeviceLocation;
                    }
                    else if (specification.ObjectClassID() == 6) {
                        if (specification.NewWorkPlaceID() || specification.NewRoomID())
                            locationInfo = { ID: specification.NewWorkPlaceID() ? specification.NewWorkPlaceID() : specification.NewRoomID(), ClassID: specification.NewWorkPlaceID() ? 22 : 3 };
                        //
                        locationType = module.LocationType.TerminalDeviceLocation;
                    }
                    else if (specification.ObjectClassID() == 33 || specification.ObjectClassID() == 34) {
                        var newDeviceID = specification.NewNetworkDeviceID() ? specification.NewNetworkDeviceID() : specification.NewTerminalDeviceID();
                        var newDeviceClassID = specification.NewNetworkDeviceID() ? 5 : 6;
                        if (specification.NewRoomID() || newDeviceID) {
                            locationInfo = { ID: newDeviceID ? newDeviceID : specification.NewRoomID(), ClassID: newDeviceID ? newDeviceClassID : 3 };
                        }
                        //
                        locationType = module.LocationType.SubdeviceLocation;
                    }
                    //
                    var onLocationChanged = function (objectInfo) {//когда новое местоположение будет выбрано
                        var retval = $.Deferred();
                        //
                        if (!objectInfo)
                            return retval.resolve(false);
                        //
                        self.ajaxControl_location.Ajax(self.$region.find('.network-device-location-header'),
                            {
                                url: '/imApi/GetAssetLocationInfo',
                                method: 'POST',
                                data: {
                                    DeviceID: specification.ObjectID(),
                                    DeviceClassID: specification.ObjectClassID(),
                                    LocationID: objectInfo.ID,
                                    LocationClassID: objectInfo.ClassID
                                }
                            },
                            function (response) {
                                if (response && response.Result === 0) {
                                    var info = response.AssetLocationInfo;
                                    //
                                    specification.NewNetworkDeviceID(null);
                                    specification.NewNetworkDeviceName(null);
                                    //
                                    specification.NewTerminalDeviceID(null);
                                    specification.NewTerminalDeviceName(null);
                                    //
                                    specification.NewOrganizationName(info.OrganizationName);
                                    specification.NewBuildingName(info.BuildingName);
                                    specification.NewRoomID(info.RoomID);
                                    specification.NewRoomName(info.RoomName);
                                    //
                                    if (specification.ObjectClassID() == 5) {
                                        specification.NewRackID(info.RackID);
                                        specification.NewRackName(info.RackName);
                                        //specification.NewRackPosition(info.RackLocation);
                                    }
                                    else if (specification.ObjectClassID() == 6) {
                                        specification.NewWorkPlaceID(info.WorkPlaceID);
                                        specification.NewWorkPlaceName(info.WorkPlaceName);
                                    }
                                    else if (specification.ObjectClassID() == 33 || specification.ObjectClassID() == 34) {
                                        if (info.DeviceClassID == 5) {
                                            specification.NewNetworkDeviceID(info.DeviceID);
                                            specification.NewNetworkDeviceName(info.DeviceName);

                                        }
                                        else if (info.DeviceClassID == 6) {
                                            specification.NewTerminalDeviceID(info.DeviceID);
                                            specification.NewTerminalDeviceName(info.DeviceName);
                                        }
                                        /*specification.NewDeviceClassID(info.DeviceClassID);
                                        specification.NewDeviceID(info.DeviceID);
                                        specification.NewDeviceName(info.DeviceName);
                                        specification.NewDeviceFullName(info.DeviceFullName);*/
                                        specification.NewRackID(info.RackID);
                                        specification.NewRackName(info.RackName);
                                        specification.NewWorkPlaceID(info.WorkPlaceID);
                                        specification.NewWorkPlaceName(info.WorkPlaceName);
                                    }
                                    //
                                    specification.NewOnStore(info.OnStore);
                                    self.SetNewOnStore();
                                    //
                                    retval.resolve(true);
                                }
                            });
                        //
                        return retval.promise();
                    };
                    var saveLocation = function (objectInfo, isReplaceAnyway) {//для сохранения нового местоположения
                        if (!objectInfo)
                            return;
                        //
                        $.when(onLocationChanged(objectInfo)).done(function () {
                            var data = {
                                ID: specification.ID(),
                                ObjClassID: specification.ClassID(),
                                ClassID: null,
                                ObjectList: null,
                                Field: 'NewLocation',
                                NewValue: JSON.stringify({ 'id': objectInfo.ID }),
                                OldValue: JSON.stringify({ 'id': locationInfo ? locationInfo.ID : null }),
                                Params: ['' + objectInfo.ClassID, specification.NewFullLocation()],
                                ReplaceAnyway: true,
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
                                            $(document).trigger('local_objectUpdated', [specification.ClassID(), specification.ID(), null]);
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
                        });
                        //
                    };
                    //
                    module.ShowDialog(locationType, locationInfo, saveLocation, true);
                });
            };
            //
            self.OnStoreCanEdit = ko.computed(function () {
                var specification = self.specification();
                if (!specification)
                    return false;
                //
                if (specification.ClassID() == 5 && specification.RackID())
                    return false;
                else if (specification.ClassID() == 6)
                    return false;
                else if (specification.ClassID() == 33 || specification.ClassID() == 34)
                    return false;
                //
                return true;
            });
            //
            self.EditModel = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var specification = self.specification();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: specification.ID(),
                        objClassID: specification.ClassID(),
                        fieldName: 'NewModel',
                        fieldFriendlyName: getTextResource('Inventory_ActualModel'),
                        oldValue: { ID: specification.NewProductCatalogModelID(), ClassID: specification.NewProductCatalogModelClassID(), FullName: specification.NewProductCatalogModelFullName() },
                        searcherName: 'ProductCatalogTypeAndModelSearcher',
                        searcherParams: [false, true, true, true, true, true, true, true, specification.ProductCatalogTemplateID],
                        onSave: function (objectInfo) {
                            if (!objectInfo) {
                                specification.NewProductCatalogModelName(null);
                                specification.NewProductCatalogTypeName(null);
                                specification.NewProductCatalogModelID(null);
                                specification.NewProductCatalogCategoryName(null);
                                specification.NewManufacturerName(null);
                                //
                                return;
                            }
                            //
                            var arr = objectInfo.FullName.split(' \\ ');
                            //
                            var length = arr.length;
                            specification.NewProductCatalogModelName(arr[length - 1]);
                            specification.NewProductCatalogTypeName(arr[length - 2]);
                            var category = '';
                            for (var i = 0; i <= length - 3; i++) {
                                if (i != 0)
                                    category += ' \\ ';
                                category += arr[i];
                            }
                            //
                            specification.NewProductCatalogModelID(objectInfo.ID);
                            specification.NewProductCatalogCategoryName(category);
                            //
                            specification.NewManufacturerName(objectInfo.Info);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            //
            self.EditUtilizer = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var specification = self.specification();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    $.when(userD).done(function (user) {
                        var options = {
                            ID: specification.ID(),
                            objClassID: specification.ClassID(),
                            fieldName: 'NewUtilizer',
                            fieldFriendlyName: getTextResource('AssetNumber_UtilizerName'),
                            oldValue: specification.NewUtilizerLoaded() ? { ID: specification.NewUtilizer().ID(), ClassID: specification.NewUtilizer().ClassID(), FullName: specification.NewUtilizer().FullName() } : null,
                            object: ko.toJS(specification.NewUtilizer()),
                            searcherName: 'UtilizerSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [user.UserID],
                            onSave: function (objectInfo) {
                                specification.NewUtilizerLoaded(false);
                                specification.NewUtilizer(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditUtilizer, false, false));
                                //    
                                specification.NewUtilizerID(objectInfo ? objectInfo.ID : '');
                                specification.NewUtilizerClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeUtilizer();
                                //
                                self.SetUtilizerHeight();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                });
            };
            //
            self.SetUtilizerHeight = function () {
                var h = self.$region.find('.newUtilizer').height();
                //
                self.UtilizerHeight(h + 'px');
            };
            //
            self.InitializeUtilizer = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.specification();
                    if (a.NewUtilizerLoaded() == false && a.NewUtilizerID()) {
                        var type = null;
                        if (a.NewUtilizerClassID() == 9) {//IMSystem.Global.OBJ_USER
                            type = userLib.UserTypes.utilizer;
                        }
                        else if (a.NewUtilizerClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                            type = userLib.UserTypes.queueExecutor;
                        }
                        else if (a.NewUtilizerClassID() == 101) {//IMSystem.Global.OBJ_ORGANIZATION
                            type = userLib.UserTypes.organization;
                        }
                        else if (a.NewUtilizerClassID() == 102) {//IMSystem.Global.OBJ_DIVISION
                            type = userLib.UserTypes.subdivision;
                        }
                        var options = {
                            UserID: a.NewUtilizerID(),
                            UserType: type,
                            UserName: null,
                            EditAction: self.EditUtilizer,
                            RemoveAction: null,
                            //ShowLeftSide: false,
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        a.NewUtilizer(user);
                        a.NewUtilizerLoaded(true);
                        //
                        $.when(user.$isLoaded).done(function () {
                            setTimeout(self.SetUtilizerHeight, 250);
                        });
                    }
                });
            };
            //
            self.EditNewCount = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var specification = self.specification();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: specification.ID(),
                        objClassID: specification.ClassID(),
                        fieldName: 'NewCount',
                        fieldFriendlyName: getTextResource('Inventory_ActualCount'),
                        oldValue: specification.NewCountStr(),
                        maxValue: 1,//в бд decimal(10, 2)
                        onSave: function (newVal) {
                            var newValStr = newVal.toString();
                            specification.NewCount(newVal);
                            specification.NewCountStr(newValStr);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };
            //
            self.EditNote = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var specification = self.specification();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: specification.ID(),
                        objClassID: specification.ClassID(),
                        fieldName: 'Note',
                        fieldFriendlyName: getTextResource('CommentHeader'),
                        oldValue: specification.Note(),
                        allowNull: true,
                        maxLength: 500,
                        onSave: function (newText) {
                            specification.Note(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.SetOnStore = function () {
                if (!self.specification())
                    return;
                //
                var checkbox = $region.find('.onStorage');
                if (checkbox && checkbox[0])
                    checkbox[0].checked = self.specification().OnStore();
            };
            //
            self.SetNewOnStore = function () {
                if (!self.specification())
                    return;
                //
                var checkbox = $region.find('.newOnStorage');
                if (checkbox && checkbox[0])
                    checkbox[0].checked = self.specification().NewOnStore();
            };
            //
            self.EditOnStore = function () {
                if (!self.CanEdit())
                    return;
                //
                var checkbox = $region.find('.onStorage');
                //
                if (!checkbox || !checkbox[0])
                    return;
                //
                var oldValue = !checkbox[0].checked;
                //
                showSpinner();
                var specification = self.specification();
                specification.OnStore(checkbox[0].checked);

                var data = {
                    ID: specification.ID(),
                    ObjClassID: specification.ClassID(),
                    Field: 'OnStore',
                    OldValue: JSON.stringify({ 'val': oldValue }),
                    NewValue: JSON.stringify({ 'val': checkbox[0].checked }),
                    ReplaceAnyway: false
                };

                self.ajaxControl.Ajax(
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
                                $(document).trigger('local_objectUpdated', [specification.ClassID(), specification.ID(), null]);
                                checkbox[0].checked = specification.OnStore();
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                });
                            }
                        }
                    });
            };
        },
        ShowDialog: function (id, readOnly, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            $.when(userD, operationIsGrantedD(899)).done(function (user, operation_update) {//OPERATION_InventorySpecification_Update = 899
                var isReadOnly = false;
                //
                if (readOnly)
                    isReadOnly = true;
                //
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
                    'region_inventorySpecificationForm',//form region prefix
                    'setting_inventorySpecificationForm',//location and size setting
                    getTextResource('Asset_Properties'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    710, 520,//minSize
                    buttons,//form buttons
                    function () {
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/SD/frmInventorySpecification\', afterRender: AfterRender}"'//attributes of form region
                );
                //
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
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
                        if (!ko.components.isRegistered('inventorySpecificationFormCaptionComponent'))
                            ko.components.register('inventorySpecificationFormCaptionComponent', {
                                template: '<span data-bind="text: $captionText"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'inventorySpecificationFormCaptionComponent', params: {  $captionText: getTextResource(\'Inventory_ObjectInventory\') + ' ' + getTextResource(\'NumberSymbol\') + $data.specification().SerialNumber() +', ' + getTextResource(\'Asset_InventoryNumberCaption\') + $data.specification().InventoryNumber() } }");
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});