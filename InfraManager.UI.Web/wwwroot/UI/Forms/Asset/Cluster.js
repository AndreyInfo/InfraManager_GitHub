define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        Cluster: function (parent, obj) {
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
            if (obj.Name)
                self.Name = ko.observable(obj.Name);
            else self.Name = ko.observable('');
            //
            if (obj.DataCenter)
                self.DataCenter = ko.observable(obj.DataCenter);
            else self.DataCenter = ko.observable('');
            //
            if (obj.VCenter)
                self.VCenter = ko.observable(obj.VCenter);
            else self.VCenter = ko.observable('');
            //
            if (obj.UUID)
                self.UUID = ko.observable(obj.UUID);
            else self.UUID = ko.observable('');
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
            if (obj.ProductCatalogTypeID)
                self.ProductCatalogTypeID = ko.observable(obj.ProductCatalogTypeID);
            else self.ProductCatalogTypeID = ko.observable('');
            //
            if (obj.ProductCatalogTypeName)
                self.ProductCatalogTypeName = ko.observable(obj.ProductCatalogTypeName);
            else self.ProductCatalogTypeName = ko.observable('');
            //
            if (obj.ClusterID)
                self.ClusterID = ko.observable(obj.ClusterID);
            else self.ClusterID = ko.observable('');
            //
            if (obj.ClusterName)
                self.ClusterName = ko.observable(obj.ClusterName);
            else self.ClusterName = ko.observable('');
            //
            if (obj.TotalSlotMemory)
                self.TotalSlotMemory = ko.observable(obj.TotalSlotMemory);
            else self.TotalSlotMemory = ko.observable('');
            //
            self.NameResource = getTextResource('ConfigurationUnit_HostName');
            //
            if (obj.Note)
                self.Note = ko.observable(obj.Note);
            else self.Note = ko.observable('');
            //
            //Состояние 
            if (obj.LifeCycleStateID)
                self.LifeCycleStateID = ko.observable(obj.LifeCycleStateID);
            else self.LifeCycleStateID = ko.observable('');

            if (obj.LifeCycleStateName)
                self.LifeCycleStateName = ko.observable(obj.LifeCycleStateName);
            else self.LifeCycleStateName = ko.observable('');
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
            self.CategoryFullName = ko.observable(self.ProductCatalogCategoryName() + ' > ' + self.ProductCatalogTypeName());
            //
            self.FullName = ko.observable(self.ProductCatalogTypeName() + ' ' + self.Name());
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
                return retval;
            });

        },

        ClusterRegistration: function (parent, obj) {
            var self = this;
            //
            self.ID = ko.observable('');
            //
            self.ClassID = ko.observable('');
            //
            self.Name = ko.observable('');
            //
            self.DataCenter = ko.observable('');
            //
            self.VCenter = ko.observable('');
            //
            self.UUID = ko.observable('');
            //
            self.OrganizationName = ko.observable('');
            //
            self.InfrastructureSegmentID = ko.observable('');
            self.InfrastructureSegmentName = ko.observable('');
            //Состояние 
            self.LifeCycleStateID = ko.observable(null);
            self.LifeCycleStateName = ko.observable('');
            //
            //Администратор
            self.AdministratorLoaded = ko.observable(false);
            self.Administrator = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditAdministrator, false, false));
            self.AdministratorClassID = ko.observable(null);
            self.AdministratorID = ko.observable(null);
            self.AdministratorName = ko.observable('');
            //
            self.ProductCatalogCategoryName = ko.observable('');
            self.ProductCatalogTemplateID = ko.observable(null);
            self.ProductCatalogTemplate = ko.computed(function () {
                return null;
            });
            self.ProductCatalogTypeID = ko.observable(null);
            self.ProductCatalogTypeName = ko.observable('');
           
            //
            self.ClusterID = ko.observable('');
            self.ClusterName = ko.observable('');
            //
           self.TotalSlotMemory = ko.observable('');
            //
            self.NameResource = getTextResource('ConfigurationUnit_HostName');
            //
            self.Note = ko.observable('');
            //
            self.LifeCycleStateName = ko.observable('');
            //
            self.ShowState = ko.observable(true);
            self.FullLocation = ko.observable('');
            self.UtilizerLoaded = ko.observable(true);
            //
            self.CanEditName = ko.observable(true);
            //
            self.CategoryFullName = ko.observable(self.ProductCatalogCategoryName() + ' > ' + self.ProductCatalogTypeName());
            //
            self.FullName = ko.observable(self.ProductCatalogTypeName() + ' ' + self.Name());
            //
            self.OrganizationName = ko.observable('');
            self.BuildingName = ko.observable(null);
            self.RoomID = ko.observable('');
            self.RoomName = ko.observable('');
            self.RackID = ko.observable('');
            self.RackName = ko.observable('');
            self.RackPosition = ko.observable('');
            self.OnStore = ko.observable(false);
            self.WorkPlaceID = ko.observable('');
            self.WorkPlaceName = ko.observable('');
            self.PowerConsumption = ko.observable(0);
            self.Cpu = ko.observable('');
            //
            self.FullLocation = ko.computed(function () {//используется для отображения на форме
                var retval = '';
                return retval;
            });

        }

    };
    return module;
});