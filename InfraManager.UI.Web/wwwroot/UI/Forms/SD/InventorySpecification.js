define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        InventorySpecification: function (parent, obj) {//InventorySpecificationForm.js, networkDevice
            var self = this;
            //
            if (obj.SolutionStr)
                self.IsWorking = ko.observable(obj.SolutionStr);
            else
                self.IsWorking = ko.observable(getTextResource('Inventory_UnderInventorization'));
            //
            if (obj.ID)
                self.ID = ko.observable(obj.ID);
            else self.ID = ko.observable('');
            //
            if (obj.ClassID)
                self.ClassID = ko.observable(obj.ClassID);
            else self.ClassID = ko.observable('');
            //
            if (obj.UtcDateReceived)
                self.UtcDateReceived = ko.observable(parseDate(obj.UtcDateReceived, true));//show only date
            else self.UtcDateReceived = ko.observable('');
            //
            if (obj.UtcDateReceived)
                self.UtcDateReceivedDT = ko.observable(new Date(parseInt(obj.UtcDateReceived)));
            else self.UtcDateReceivedDT = ko.observable(null);
            //
            if (obj.InventoryNumber)
                self.InventoryNumber = ko.observable(obj.InventoryNumber);
            else self.InventoryNumber = ko.observable('');
            //
            if (obj.SerialNumber)
                self.SerialNumber = ko.observable(obj.SerialNumber);
            else self.SerialNumber = ko.observable('');
            //
            if (obj.Code)
                self.Code = ko.observable(obj.Code);
            else self.Code = ko.observable('');
            //
            if (obj.Count)
                self.Count = ko.observable(obj.Count);
            else self.Count = ko.observable('');
            //
            if (obj.OrganizationName)
                self.OrganizationName = ko.observable(obj.OrganizationName);
            else self.OrganizationName = ko.observable('');
            //
            if (obj.BuildingName)
                self.BuildingName = ko.observable(obj.BuildingName);
            else self.BuildingName = ko.observable(null);
            //
            if (obj.RoomID)
                self.RoomID = ko.observable(obj.RoomID);
            else self.RoomID = ko.observable('');
            //
            if (obj.RoomName)
                self.RoomName = ko.observable(obj.RoomName);
            else self.RoomName = ko.observable('');
            //
            if (obj.RackID)
                self.RackID = ko.observable(obj.RackID);
            else self.RackID = ko.observable('');
            //
            if (obj.RackName)
                self.RackName = ko.observable(obj.RackName);
            else self.RackName = ko.observable('');
            //
            if (obj.RackPosition)
                self.RackPosition = ko.observable(obj.RackPosition);
            else self.RackPosition = ko.observable('');
            //
            if (obj.OnStore)
                self.OnStore = ko.observable(obj.OnStore);
            else self.OnStore = ko.observable(false);
            //
            if (obj.UtilizerID)
                self.UtilizerID = ko.observable(obj.UtilizerID);
            else self.UtilizerID = ko.observable('');
            //
            if (obj.UtilizerClassID)
                self.UtilizerClassID = ko.observable(obj.UtilizerClassID);
            else self.UtilizerClassID = ko.observable('');
            //
            self.UtilizerLoaded = ko.observable(false);
            self.Utilizer = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditUtilizer, false, false));
            //
            if (obj.UtilizerName)
                self.UtilizerName = ko.observable(obj.UtilizerName);
            else self.UtilizerName = ko.observable('');
            //
            if (obj.ProductCatalogCategoryName)
                self.ProductCatalogCategoryName = ko.observable(obj.ProductCatalogCategoryName);
            else self.ProductCatalogCategoryName = ko.observable('');
            //
            if (obj.ProductCatalogTemplateID || obj.ProductCatalogTemplateID === 0)
                self.ProductCatalogTemplateID = ko.observable(obj.ProductCatalogTemplateID);
            else self.ProductCatalogTemplateID = ko.observable('');
            //
            if (obj.ProductCatalogTypeName)
                self.ProductCatalogTypeName = ko.observable(obj.ProductCatalogTypeName);
            else self.ProductCatalogTypeName = ko.observable('');
            //
            if (obj.ManufacturerID)
                self.ManufacturerID = ko.observable(obj.ManufacturerID);
            else self.ManufacturerID = ko.observable('');
            //
            if (obj.ManufacturerName)
                self.ManufacturerName = ko.observable(obj.ManufacturerName);
            else self.ManufacturerName = ko.observable('');
            //
            if (obj.ProductCatalogModelID)
                self.ProductCatalogModelID = ko.observable(obj.ProductCatalogModelID);
            else self.ProductCatalogModelID = ko.observable(null);
            //
            if (obj.ProductCatalogModelName)
                self.ProductCatalogModelName = ko.observable(obj.ProductCatalogModelName);
            else self.ProductCatalogModelName = ko.observable('');
            //
            if (obj.Note)
                self.Note = ko.observable(obj.Note);
            else self.Note = ko.observable('');
            //
            if (obj.LocationName)
                self.LocationName = ko.observable(obj.LocationName);
            else self.LocationName = ko.observable('');
            //
            if (obj.NewCount || obj.NewCount === 0)
                self.NewCount = ko.observable(obj.NewCount);
            else self.NewCount = ko.observable(null);
            //
            if (obj.NewCount || obj.NewCount === 0)
                self.NewCountStr = ko.observable(obj.NewCount);
            else self.NewCountStr = ko.observable('');
            //
            if (obj.NetworkDeviceID)
                self.NetworkDeviceID = ko.observable(obj.NetworkDeviceID);
            else self.NetworkDeviceID = ko.observable('');
            //
            if (obj.TerminalDeviceID)
                self.TerminalDeviceID = ko.observable(obj.TerminalDeviceID);
            else self.TerminalDeviceID = ko.observable('');
            //
            if (obj.WorkPlaceID)
                self.WorkPlaceID = ko.observable(obj.WorkPlaceID);
            else self.WorkPlaceID = ko.observable('');
            //
            if (obj.Solution || obj.Solution === 0)
                self.Solution = ko.observable(obj.Solution);
            else self.Solution = ko.observable(null);
            //
            if (obj.NewRoomID)
                self.NewRoomID = ko.observable(obj.NewRoomID);
            else self.NewRoomID = ko.observable('');
            //
            if (obj.NewRackID)
                self.NewRackID = ko.observable(obj.NewRackID);
            else self.NewRackID = ko.observable('');
            //
            if (obj.NewWorkPlaceID)
                self.NewWorkPlaceID = ko.observable(obj.NewWorkPlaceID);
            else self.NewWorkPlaceID = ko.observable('');
            //
            if (obj.NewNetworkDeviceID)
                self.NewNetworkDeviceID = ko.observable(obj.NewNetworkDeviceID);
            else self.NewNetworkDeviceID = ko.observable('');
            //
            if (obj.NewTerminalDeviceID)
                self.NewTerminalDeviceID = ko.observable(obj.NewTerminalDeviceID);
            else self.NewTerminalDeviceID = ko.observable('');
            //
            if (obj.NewRoomName)
                self.NewRoomName = ko.observable(obj.NewRoomName);
            else self.NewRoomName = ko.observable('');
            //
            if (obj.NewRackName)
                self.NewRackName = ko.observable(obj.NewRackName);
            else self.NewRackName = ko.observable('');
            //
            if (obj.NewWorkPlaceName)
                self.NewWorkPlaceName = ko.observable(obj.NewWorkPlaceName);
            else self.NewWorkPlaceName = ko.observable('');
            //
            if (obj.NewNetworkDeviceName)
                self.NewNetworkDeviceName = ko.observable(obj.NewNetworkDeviceName);
            else self.NewNetworkDeviceName = ko.observable('');
            //
            if (obj.NewTerminalDeviceName)
                self.NewTerminalDeviceName = ko.observable(obj.NewTerminalDeviceName);
            else self.NewTerminalDeviceName = ko.observable('');
            //
            //
            if (obj.NewOnStore)
                self.NewOnStore = ko.observable(obj.NewOnStore);
            else self.NewOnStore = ko.observable('');
            //
            if (obj.NewManufacturerID)
                self.NewManufacturerID = ko.observable(obj.NewManufacturerID);
            else self.NewManufacturerID = ko.observable('');
            //
            if (obj.NewManufacturerName)
                self.NewManufacturerName = ko.observable(obj.NewManufacturerName);
            else self.NewManufacturerName = ko.observable('');
            //
            if (obj.NewUtilizerID)
                self.NewUtilizerID = ko.observable(obj.NewUtilizerID);
            else self.NewUtilizerID = ko.observable('');
            //
            if (obj.NewUtilizerClassID)
                self.NewUtilizerClassID = ko.observable(obj.NewUtilizerClassID);
            else self.NewUtilizerClassID = ko.observable('');
            //
            self.NewUtilizerLoaded = ko.observable(false);
            self.NewUtilizer = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditUtilizer, false, false));
            //
            if (obj.NewProductCatalogModelClassID)
                self.NewProductCatalogModelClassID = ko.observable(obj.NewProductCatalogModelClassID);
            else self.NewProductCatalogModelClassID = ko.observable('');
            //
            if (obj.NewProductCatalogModelID)
                self.NewProductCatalogModelID = ko.observable(obj.NewProductCatalogModelID);
            else self.NewProductCatalogModelID = ko.observable('');
            //
            if (obj.NewProductCatalogModelName)
                self.NewProductCatalogModelName = ko.observable(obj.NewProductCatalogModelName);
            else self.NewProductCatalogModelName = ko.observable('');
            //
            if (obj.NewProductCatalogTypeName)
                self.NewProductCatalogTypeName = ko.observable(obj.NewProductCatalogTypeName);
            else self.NewProductCatalogTypeName = ko.observable('');
            //
            if (obj.NewProductCatalogCategoryName)
                self.NewProductCatalogCategoryName = ko.observable(obj.NewProductCatalogCategoryName);
            else self.NewProductCatalogCategoryName = ko.observable('');
            //
            self.NewProductCatalogModelFullName = ko.computed(function () {
                var retval = '';
                if (self.NewProductCatalogCategoryName())
                    retval += self.NewProductCatalogCategoryName();
                if (self.NewProductCatalogTypeName() && retval)
                    retval += ' \\ ' + self.NewProductCatalogTypeName();
                if (self.NewProductCatalogModelName())
                    retval += ' \\ ' + self.NewProductCatalogModelName();
                //
                return retval;
            });
            //
            if (obj.NewOrganizationName)
                self.NewOrganizationName = ko.observable(obj.NewOrganizationName);
            else self.NewOrganizationName = ko.observable('');
            //

            if (obj.NewBuildingName)
                self.NewBuildingName = ko.observable(obj.NewBuildingName);
            else self.NewBuildingName = ko.observable('');
            //
            self.func = function (room) {
                var arr = room.split(' \\ ');
                var roomName = 'Комната "' + arr[1] + '", Этаж "' + arr[0] + '", ';
                return roomName;
            };
            //
            self.ObjectID = ko.observable(obj.ObjectID);
            self.ObjectClassID = ko.observable(obj.ObjectClassID);
            //
            if (obj.ObjectClassID == 5 || obj.ObjectClassID == 6) {
                self.NewFullLocation = ko.computed(function () {//используется для отображения на форме
                    var retval = '';
                    if (self.NewRackID())
                        retval += 'Шкаф "' + self.NewRackName() + '", ';
                    if (self.NewWorkPlaceID())
                        retval += 'Рабочее место "' + self.NewWorkPlaceName() + '", ';
                    //
                    if (self.NewRoomName())
                        retval += self.func(self.NewRoomName());
                    if (self.NewBuildingName())
                        retval += 'Здание "' + self.NewBuildingName() + '", ';
                    if (self.NewOrganizationName())
                        retval += 'Организация "' + self.NewOrganizationName() + '"';
                    return retval;
                })
            }
            else if (obj.ObjectClassID == 33 || obj.ObjectClassID == 34) {
                self.NewFullLocation = ko.computed(function () {//используется для отображения на форме
                    var retval = '';
                    if (self.NewNetworkDeviceID()) {
                        retval += 'Оборудование "' + self.NewNetworkDeviceName() + '", ';
                        if (self.RackID()) {
                            retval += 'Шкаф "' + self.NewRackName() + '", ';
                        }
                    }
                    else if (self.NewTerminalDeviceID()) {
                        retval += 'Оборудование "' + self.NewTerminalDeviceName() + '", ';
                        if (self.NewWorkPlaceID()) {
                            retval += 'Рабочее место "' + self.NewWorkPlaceName() + '", ';
                        }
                    }
                    //
                    if (self.NewRoomName())
                        retval += self.func(self.NewRoomName());
                    if (self.NewBuildingName())
                        retval += 'Здание "' + self.NewBuildingName() + '", ';
                    if (self.NewOrganizationName())
                        retval += 'Организация "' + self.NewOrganizationName() + '"';
                    return retval;
                });
            }
            else
                self.NewFullLocation = ko.observable('');
            //
            if (obj.ProductCatalogTemplateName)
                self.ProductCatalogTemplateName = ko.observable(obj.ProductCatalogTemplateName);
            else self.ProductCatalogTemplateName = ko.observable('');
            //
            if (obj.ObjectName)
                self.Name = ko.observable(obj.ObjectName);
            else if (obj.ObjectClassID == 33 || obj.ObjectClassID == 34)
                self.Name = ko.observable(self.ProductCatalogTemplateName() + ' ' + self.ProductCatalogModelName());
            else
                self.Name = ko.observable('');
            //
            self.FullName = ko.observable(self.ProductCatalogTypeName() + ' ' + self.ProductCatalogModelName() + ' ' + self.Name());
            //
            self.HasDeviations = ko.computed(function () {
                if ((self.NewCount() && self.Count() != self.NewCount()) ||
                    self.ManufacturerID().toUpperCase() != self.NewManufacturerID().toUpperCase() ||
                    (self.RackID() != self.NewRackID() && self.RoomID() != self.NewRoomID() && self.WorkPlaceID() != self.NewWorkPlaceID() && self.NetworkDeviceID().toUpperCase() != self.NewNetworkDeviceID().toUpperCase() && self.TerminalDeviceID().toUpperCase() != self.NewTerminalDeviceID().toUpperCase()) ||
                    self.UtilizerID().toUpperCase() != self.NewUtilizerID().toUpperCase() ||
                    self.ProductCatalogModelID().toUpperCase() != self.NewProductCatalogModelID().toUpperCase())
                    return true;
                //
                return false;
            });
        }
    };
    return module;
});