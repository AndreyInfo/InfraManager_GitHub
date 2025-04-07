define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        LogicalObject: function (parent, obj) {
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
            self.CanEditName = ko.observable(true);
            //
            if (obj.Name)
                self.Name = ko.observable(obj.Name);
            else self.Name = ko.observable('');
            //
            if (obj.LogicAssetName)
                self.LogicAssetName = ko.observable(obj.LogicAssetName);
            else self.LogicAssetName = ko.observable('');
            //
            if (obj.IPAddress)
                self.IPAddress = ko.observable(obj.IPAddress);
            else self.IPAddress = ko.observable('');
            //
            if (obj.LifeCycleStateID)
                self.LifeCycleStateID = ko.observable(obj.LifeCycleStateID);
            else self.LifeCycleStateID = ko.observable('');
            //
            if (obj.LifeCycleStateName)
                self.LifeCycleStateName = ko.observable(obj.LifeCycleStateName);
            else self.LifeCycleStateName = ko.observable('');
            //
            if (obj.VCenter)
                self.VCenter = ko.observable(obj.VCenter);
            else self.VCenter = ko.observable('');
            //
            if (obj.UUID)
                self.UUID = ko.observable(obj.UUID);
            else self.UUID = ko.observable('');
            //
            if (obj.SerialNumber)
                self.SerialNumber = ko.observable(obj.SerialNumber);
            else self.SerialNumber = ko.observable('');
            //
            if (obj.ProductCatalogTypeName)
                self.ProductCatalogTypeName = ko.observable(obj.ProductCatalogTypeName);
            else self.ProductCatalogTypeName = ko.observable('');
            //
            if (obj.ProductCatalogTypeID)
                self.ProductCatalogTypeID = ko.observable(obj.ProductCatalogTypeID);
            else self.ProductCatalogTypeID = ko.observable('');
            //
            if (obj.ProductCatalogModelName)
                self.ProductCatalogModelName = ko.observable(obj.ProductCatalogModelName);
            else self.ProductCatalogModelName = ko.observable('');
            //
            if (obj.ProductCatalogModelID)
                self.ProductCatalogModelID = ko.observable(obj.ProductCatalogModelID);
            else self.ProductCatalogModelID = ko.observable('');
            //
            self.ClientLoaded = ko.observable(false);
            self.Client = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditClient, false, false));
            //
            if (obj.ClientName)
                self.ClientName = ko.observable(obj.ClientName);
            else self.ClientName = ko.observable('');
            //
            if (obj.ClientID)
                self.ClientID = ko.observable(obj.ClientID);
            else self.ClientID = ko.observable('');
            //
            if (obj.ClientClassID)
                self.ClientClassID = ko.observable(obj.ClientClassID);
            else self.ClientClassID = ko.observable('');
            //
            if (obj.HostID)
                self.HostID = ko.observable(obj.HostID);
            else self.HostID = ko.observable('');
            //
            if (obj.HostClassID)
                self.HostClassID = ko.observable(obj.HostClassID);
            else self.HostClassID = ko.observable('');
            //
            if (obj.HostName)
                self.HostName = ko.observable(obj.HostName);
            else self.HostName = ko.observable('');
            //
            self.ITContactLoaded = ko.observable(false);
            self.ITContact = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditITContact, false, false));
            //
             if (obj.ITAssetContactName)
                self.ITAssetContactName = ko.observable(obj.ITAssetContactName);
            else self.ITAssetContactName = ko.observable('');
            //
            if (obj.ITAssetContactID)
                self.ITAssetContactID = ko.observable(obj.ITAssetContactID);
            else self.ITAssetContactID = ko.observable('');
            //
            if (obj.ITAssetContactClassID)
                self.ITAssetContactClassID = ko.observable(obj.ITAssetContactClassID);
            else self.ITAssetContactClassID = ko.observable('');
            //
            if (obj.Note)
                self.Note = ko.observable(obj.Note);
            else self.Note = ko.observable('');
            //
            if (obj.DataStoreName)
                self.DataStoreName = ko.observable(obj.DataStoreName);
            else self.DataStoreName = ko.observable('');
            //
            if (obj.DataStoreID)
                self.DataStoreID = ko.observable(obj.DataStoreID);
            else self.DataStoreID = ko.observable('');
            //
            if (obj.DataStoreClassID)
                self.DataStoreClassID = ko.observable(obj.DataStoreClassID);
            else self.DataStoreClassID = ko.observable('');
            //
            if (obj.DeviceName)
                self.DeviceName = ko.observable(obj.DeviceName);
            else self.DeviceName = ko.observable('');
            //
            if (obj.DeviceID)
                self.DeviceID = ko.observable(obj.DeviceID);
            else self.DeviceID = ko.observable('');
            //
            if (obj.DeviceClassID)
                self.DeviceClassID = ko.observable(obj.DeviceClassID);
            else self.DeviceClassID = ko.observable('');
            //
            if (obj.DateReceived)
                self.DateReceived = ko.observable(parseDate(obj.DateReceived));
            else self.DateReceived = ko.observable('');
            //
            if (obj.DateReceived)
                self.DateReceivedDT = ko.observable(new Date(parseInt(obj.DateReceived)));
            else self.DateReceivedDT = ko.observable(null);
            //
            if (obj.DateAnnuled)
                self.DateAnnuled = ko.observable(parseDate(obj.DateAnnuled));
            else self.DateAnnuled = ko.observable('');
            //
            if (obj.DateAnnuled)
                self.DateAnnuledDT = ko.observable(new Date(parseInt(obj.DateAnnuled)));
            else self.DateAnnuledDT = ko.observable(null);
            //
            if (obj.CPUNumber)
                self.CPUNumber = ko.observable(obj.CPUNumber);
            else self.CPUNumber = ko.observable('');
            //
            if (obj.CPULimit)
                self.CPULimit = ko.observable(obj.CPULimit);
            else self.CPULimit = ko.observable('');
            //
            if (obj.CPUShare)
                self.CPUShare = ko.observable(obj.CPUShare);
            else self.CPUShare = ko.observable('');
            //
            if (obj.Memory)
                self.Memory = ko.observable(obj.Memory);
            else self.Memory = ko.observable('');
            //
            if (obj.MemoryLimit)
                self.MemoryLimit = ko.observable(obj.MemoryLimit);
            else self.MemoryLimit = ko.observable('');
            //
            if (obj.MemoryShare)
                self.MemoryShare = ko.observable(obj.MemoryShare);
            else self.MemoryShare = ko.observable('');
            //
            if (obj.ConfigurationUnitName)
                self.ConfigurationUnitName = ko.observable(obj.ConfigurationUnitName);
            else self.ConfigurationUnitName = ko.observable('');
            //
            if (obj.ConfigurationUnitID)
                self.ConfigurationUnitID = ko.observable(obj.ConfigurationUnitID);
            else self.ConfigurationUnitID = ko.observable('');
            //
            self.IconCss = ko.computed(function () {
                switch (obj.ClassID) {
                    case 12: return "model-logicComponentObject-icon";
                    case 416: return "model-logical-server-icon";
                    case 417: return "model-logical-computer-icon";
                    case 418: return "model-logical-router-icon";
                    default: return "model-logicObject-icon";          
                }
            });
            //
            self.HasHostFull = ko.computed(function () {
                if (obj.HostID == null)
                    parent.HasHost(false);
                else
                    parent.HasHost(true);
            });
            //
            self.ShowState = ko.observable(true);
            self.FullLocation = ko.observable('');
            self.UtilizerLoaded = ko.observable(true);            
            //
            self.CanEditName = ko.observable(true);
            //
            if (obj.ProductCatalogTemplateID)
                self.ProductCatalogTemplateID = ko.observable(obj.ProductCatalogTemplateID);
            else self.ProductCatalogTemplateID = ko.observable('');
            //
            if (obj.ProductCatalogTemplateName)
                self.ProductCatalogTemplateName = ko.observable(obj.ProductCatalogTemplateName);
            else self.ProductCatalogTemplateName = ko.observable('');
            //
            self.ProductCatalogTemplate = ko.computed(function () {
                return null;
            });
            //
            self.EditIPAddressName = ko.computed(function () {
                return obj.AutoCreateNetworkUnit;
            });
            //
            self.FullName = ko.observable(self.ProductCatalogTypeName() + ' ' + self.Name());
        },
        LogicalObjectRegistration: function (parent) {
            var self = this;
            //
            self.ID = ko.observable('');
            //
            self.ClassID = ko.observable(null);
            //
            self.CanEditName = ko.observable(true);
            //
            if (obj.ProductCatalogTemplateID)
                self.ProductCatalogTemplateID = ko.observable(obj.ProductCatalogTemplateID);
            else self.ProductCatalogTemplateID = ko.observable('');
            //
            if (obj.ProductCatalogTemplateName)
                self.ProductCatalogTemplateName = ko.observable(obj.ProductCatalogTemplateName);
            else self.ProductCatalogTemplateName = ko.observable('');
            //
            self.ProductCatalogTemplate = ko.computed(function () {
                return null;
            });
            //
            self.EditIPAddressName = ko.computed(function () {
                return obj.AutoCreateNetworkUnit;
            });
            //
            self.FullName = ko.observable(self.ProductCatalogTypeName() + ' ' + self.Name());
        },
        LogicalObjectRegistration: function (parent) {
            var self = this;
            //
            self.ID = ko.observable('');
            //
            self.ClassID = ko.observable(null);
            //
            self.CanEditName = ko.observable(true);
            //
            self.Name = ko.observable('');
            //
            self.LogicAssetName = ko.observable('');
            self.IPAddress = ko.observable('');
            //
            self.LifeCycleStateID = ko.observable(null);
            self.LifeCycleStateName = ko.observable('');
            //VCenter
            self.VCenter = ko.observable('');
            //UUID
            self.UUID = ko.observable('');
            //
            self.SerialNumber = ko.observable('');
            //Тип
            self.ProductCatalogTypeID = ko.observable(null);
            self.ProductCatalogTypeName = ko.observable('');
            //Модель 
            self.ProductCatalogModelID = ko.observable(null);
            self.ProductCatalogModelName = ko.observable('');
            //Клиент 
            self.ClientLoaded = ko.observable(false);
            self.Client = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditClient, false, false));
            //
            self.ClientID = ko.observable(null);
            self.ClientClassID = ko.observable(null);
            self.ClientName = ko.observable('');
            //
            self.HostID = ko.observable(null);
            self.HostClassID = ko.observable(null);
            self.HostName = ko.observable('');
            //
            self.ITContactLoaded = ko.observable(false);
            self.ITContact = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditITContact, false, false));
            //
            self.ITAssetContactID = ko.observable(null);
            self.ITAssetContactClassID = ko.observable(null);
            self.ITAssetContactName = ko.observable('');
            //
            //Примечание
            self.Note = ko.observable('');
            //
            self.DataStoreID = ko.observable(null);
            self.DataStoreClassID = ko.observable(null);
            self.DataStoreName = ko.observable('');
            //
            self.DeviceID = ko.observable(null);
            self.DeviceClassID = ko.observable(null);
            self.DeviceName = ko.observable('');
            //
            //Даты
            self.DateReceived = ko.observable('');
            //
            self.DateReceivedDT = ko.observable(null);
            //
            self.DateAnnuled = ko.observable('');
            //
            self.DateAnnuledDT = ko.observable(null);
            //
            self.CPUNumber = ko.observable('');
            //
            self.CPULimit = ko.observable('');
            //
            self.CPUShare = ko.observable('');
            //
            self.Memory = ko.observable('');
            //
            self.MemoryLimit = ko.observable('');
            //
            self.MemoryShare = ko.observable('');
            //
            self.ConfigurationUnitID = ko.observable(null);
            self.ConfigurationUnitName = ko.observable('');
            //
            self.IconCss = ko.computed(function () {
                   return "model-logicObject-icon";               
            });
            //
            //self.HasHostFull = ko.computed(function () {
            //    return parent.HasHost(true);
            //});
            //
            self.ShowState = ko.observable(true);
            self.FullLocation = ko.observable('');
            self.UtilizerLoaded = ko.observable(true);
            //
            self.CanEditName = ko.observable(true);
            //
            self.ProductCatalogTemplateID = ko.observable(null);
            self.ProductCatalogTemplateName = ko.observable('');
            //
            self.ProductCatalogTemplate = ko.computed(function () {
                return null;
            });
            //
            //
            self.FullName = ko.observable(self.ProductCatalogTypeName() + ' ' + self.Name());
        }
    };
    return module;
});