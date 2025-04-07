define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        ConfigurationUnit: function (parent, obj) {
            var self = this;
            //
            if (obj.ID)
                self.ID = ko.observable(obj.ID);
            else self.ID = ko.observable('');
            //
            if (obj.ClassID)
                self.ClassID = ko.observable(obj.ClassID);
            else self.ClassID = ko.observable('');
            //
            if (obj.ObjectID)
                self.ObjectID = ko.observable(obj.ObjectID);
            else self.ObjectID = ko.observable('');
            //
            if (obj.ObjectClassID)
                self.ObjectClassID = ko.observable(obj.ObjectClassID);
            else self.ObjectClassID = ko.observable('');
            //
            if (obj.AgentClassID)
                self.AgentClassID = ko.observable(obj.AgentClassID);
            else self.AgentClassID = ko.observable('');
            //
            if (obj.Name)
                self.Name = ko.observable(obj.Name);
            else self.Name = ko.observable('');
            //
            if (obj.FullName)
                self.FullName = ko.observable(obj.FullName);
            else self.FullName = ko.observable('');
            //
            if (obj.IPAddress)
                self.IPAddress = ko.observable(obj.IPAddress);
            else self.IPAddress = ko.observable('');
            //
            if (obj.IPMask)
                self.IPMask = ko.observable(obj.IPMask);
            else self.IPMask = ko.observable('');
            //
            if (obj.SerialNumber)
                self.SerialNumber = ko.observable(obj.SerialNumber);
            else self.SerialNumber = ko.observable('');
            //
            if (obj.OrganizationName)
                self.OrganizationName = ko.observable(obj.OrganizationName);
            else self.OrganizationName = ko.observable('');
            //
            if (obj.InfrastructureSegmentID)
                self.InfrastructureSegmentID = ko.observable(obj.InfrastructureSegmentID);
            else self.InfrastructureSegmentID = ko.observable('');
            //
            if (obj.InfrastructureSegmentName)
                self.InfrastructureSegmentName = ko.observable(obj.InfrastructureSegmentName);
            else self.InfrastructureSegmentName = ko.observable('');
            //
            if (obj.CriticalityID)
                self.CriticalityID = ko.observable(obj.CriticalityID);
            else self.CriticalityID = ko.observable('');
            //
            if (obj.CriticalityName)
                self.CriticalityName = ko.observable(obj.CriticalityName);
            else self.CriticalityName = ko.observable('');
            //
            if (obj.AdministratorClassID)
                self.AdministratorClassID = ko.observable(obj.AdministratorClassID);
            else self.AdministratorClassID = ko.observable('');
            //
            if (obj.AdministratorID)
                self.AdministratorID = ko.observable(obj.AdministratorID);
            else self.AdministratorID = ko.observable('');
            //
            if (obj.AdministratorName)
                self.AdministratorName = ko.observable(obj.AdministratorName);
            else self.AdministratorName = ko.observable('');
            //
            self.AdministratorLoaded = ko.observable(false);
            self.Administrator = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditAdministrator, false, false));
            //
            if (obj.ProductCatalogCategoryName)
                self.ProductCatalogCategoryName = ko.observable(obj.ProductCatalogCategoryName);
            else self.ProductCatalogCategoryName = ko.observable('');
            //
            if (obj.ProductCatalogTemplateID)
                self.ProductCatalogTemplateID = ko.observable(obj.ProductCatalogTemplateID);
            else self.ProductCatalogTemplateID = ko.observable('');
            //
            if (obj.ProductCatalogTypeName)
                self.ProductCatalogTypeName = ko.observable(obj.ProductCatalogTypeName);
            else self.ProductCatalogTypeName = ko.observable('');
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
            if (obj.ProductCatalogModelCode)
                self.ProductCatalogModelCode = ko.observable(obj.ProductCatalogModelCode);
            else self.ProductCatalogModelCode = ko.observable('');
            //
            if (obj.ClusterID)
                self.ClusterID = ko.observable(obj.ClusterID);
            else self.ClusterID = ko.observable('');
            //
            if (obj.ClusterName)
                self.ClusterName = ko.observable(obj.ClusterName);
            else self.ClusterName = ko.observable('');
            //
            if (obj.DataCenter)
                self.DataCenter = ko.observable(obj.DataCenter);
            else self.DataCenter = ko.observable('');
            //
            if (obj.VCenter)
                self.VCenter = ko.observable(obj.VCenter);
            else self.VCenter = ko.observable('');
            //
            if (obj.TotalSlotMemory)
                self.TotalSlotMemory = ko.observable(obj.TotalSlotMemory);
            else self.TotalSlotMemory = ko.observable('');
            //
            self.IsHost = ko.observable(obj.ProductCatalogTemplateID === 419);//OBJ_HostConfigurationUnit
            self.IsNetworkDevice = ko.observable(obj.ObjectClassID == 5);//OBJ_NetworkDevice
            //
            self.IconCss = ko.computed(function () {
                switch (obj.ProductCatalogTemplateID) {
                    case 409: return "treeNodeIcon-icon-network-node-Ke";                      //409 IMSystem.Global.OBJ_ConfigurationUnit: 
                    case 410: return "treeNodeIcon-icon-commutator";                     //410 IMSystem.Global.OBJ_SwitchConfigurationUnit
                    case 411: return "treeNodeIcon-icon-router-configuration";           //411 IMSystem.Global.OBJ_RouterConfigurationUnit
                    case 412: return "treeNodeIcon-print-server";                       //412  IMSystem.Global.OBJ_PrinterConfigurationUnit                                                                       
                    case 413: return "treeNodeIcon-icon-data-storage";            //413 IMSystem.Global.OBJ_StorageSystemConfigurationUnit
                    case 414: return "treeNodeIcon-icon-win-server";                     //414 IMSystem.Global.OBJ_ServerConfigurationUnit
                    case 419: return "treeNodeIcon-host";                                  //419 IMSystem.Global.OBJ_HostConfigurationUnit
                    default: return "treeNodeIcon-icon-win-server";
                }
            });
            //
            self.NameResource = self.IsHost() ? getTextResource('ConfigurationUnit_HostName') : getTextResource('NetworkUnitName');
            //
            if (obj.Description)
                self.Description = ko.observable(obj.Description);
            else self.Description = ko.observable('');
            //
            if (obj.Note)
                self.Note = ko.observable(obj.Note);
            else self.Note = ko.observable('');
            //
            if (obj.Memory)
                self.Memory = ko.observable(obj.Memory);
            else self.Memory = ko.observable('');
            //
            if (obj.LifeCycleStateName)
                self.LifeCycleStateName = ko.observable(obj.LifeCycleStateName);
            else self.LifeCycleStateName = ko.observable('');
            //
            self.CAT_LogicalComponent = ko.observable(obj.ProductCatalogTemplateID === 12);//Global.CAT_LogicalComponent (составной логический объект)
            self.CAT_Server = ko.observable(obj.ProductCatalogTemplateID === 6);//Global.CAT_SERVER (сервер)
            self.CAT_Switch = ko.observable(obj.ProductCatalogTemplateID === 4);//Global.CAT_SWITCH (Коммутатор)
            self.CAT_Router = ko.observable(obj.ProductCatalogTemplateID === 1);//Global.CAT_ROUTER (Маршрутизатор)
            self.CAT_Brigde = ko.observable(obj.ProductCatalogTemplateID === 8);//Global.CAT_BRIDGE (Мост)
            self.CAT_StorageSystem = ko.observable(obj.ProductCatalogTemplateID === 11);//Global.CAT_StorageSystem (Система хранения данных)
            self.CAT_NetworkDevicePrinter = ko.observable(obj.ProductCatalogTemplateID === 13);//Global.CAT_StorageSystem (Принтер (сетевое оборудование))
            //
            self.ShowIPAddress = ko.computed(function () {
                return true;
            });
            //
            self.ShowSerialNumber = ko.computed(function () {
                return true;
            });
            //
            self.ShowDeviceName = ko.computed(function () {
                return false;
            });
            //
            self.ShowDescription = ko.computed(function () {
                return true;
            });
            //
            self.ProductCatalogTemplate = ko.computed(function () {
                return null;
            });
            //
            self.ShowState = ko.observable(true);
            self.FullLocation = ko.observable('');
            self.UtilizerLoaded = ko.observable(true);
            //
            self.CanEditName = ko.observable(true);
            //
            if (!self.CAT_LogicalComponent())
                self.CategoryFullName = ko.observable(self.ProductCatalogCategoryName() + ' > ' + self.ProductCatalogTypeName() + ' > ' + self.ProductCatalogModelName());
            else
                self.CategoryFullName = ko.observable(self.ProductCatalogCategoryName() + ' > ' + self.ProductCatalogTypeName());
            //
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
            if (obj.WorkPlaceID)
                self.WorkPlaceID = ko.observable(obj.WorkPlaceID);
            else self.WorkPlaceID = ko.observable('');
            //
            if (obj.WorkPlaceName)
                self.WorkPlaceName = ko.observable(obj.WorkPlaceName);
            else self.WorkPlaceName = ko.observable('');
            //
            if (obj.PowerConsumption)
                self.PowerConsumption = ko.observable(obj.PowerConsumption);
            else self.PowerConsumption = ko.observable(0);
            //
            if (obj.Cpu)
                self.Cpu = ko.observable(obj.Cpu);
            else self.Cpu = ko.observable('');
            //
            self.FullLocation = ko.computed(function () {//используется для отображения на форме
                var retval = '';

                if (self.ObjectClassID() == 5) {
                    if (self.RackID())
                        retval += self.RackName() + ' > ';
                    //
                    retval += self.RoomName() + ' > ' + self.BuildingName() + ' > ' + self.OrganizationName();
                }
                else if (self.ObjectClassID() == 6) {
                    retval += self.OrganizationName() + ' \\ ' + self.BuildingName() + ' \\ ' + self.RoomName();
                    //
                    if (self.WorkPlaceID())
                        retval += ' \\ ' + self.WorkPlaceName();
                //
                }
                return retval;
            });

        }
    };
    return module;
});