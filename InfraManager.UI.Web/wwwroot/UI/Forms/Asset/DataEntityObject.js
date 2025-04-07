define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        DataEntityObject: function (parent, obj) {
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
            //Состояние 
            if (obj.LifeCycleStateID)
                self.LifeCycleStateID = ko.observable(obj.LifeCycleStateID);
            else self.LifeCycleStateID = ko.observable('');
            
            if (obj.LifeCycleStateName)
                self.LifeCycleStateName = ko.observable(obj.LifeCycleStateName);
            else self.LifeCycleStateName = ko.observable('');
            //Управляющее приложение
            if (obj.DeviceApplicationID)
                self.DeviceApplicationID = ko.observable(obj.DeviceApplicationID);
            else self.DeviceApplicationID = ko.observable('');

            if (obj.DeviceApplicationName)
                self.DeviceApplicationName = ko.observable(obj.DeviceApplicationName);
            else self.DeviceApplicationName = ko.observable('');
            //Том данных 
            if (obj.VolumeID)
                self.VolumeID = ko.observable(obj.VolumeID);
            else self.VolumeID = ko.observable('');

            if (obj.VolumeName)
                self.VolumeName = ko.observable(obj.VolumeName);
            else self.VolumeName = ko.observable('');
            //Тип
            if (obj.ProductCatalogTypeName)
                self.ProductCatalogTypeName = ko.observable(obj.ProductCatalogTypeName);
            else self.ProductCatalogTypeName = ko.observable('');
            //
            if (obj.ProductCatalogTypeID)
                self.ProductCatalogTypeID = ko.observable(obj.ProductCatalogTypeID);
            else self.ProductCatalogTypeID = ko.observable('');
            //Критичность
            if (obj.CriticalityID)
                self.CriticalityID = ko.observable(obj.CriticalityID);
            else self.CriticalityID = ko.observable(null);
            if (obj.CriticalityName)
                self.CriticalityName = ko.observable(obj.CriticalityName);
            else self.CriticalityName = ko.observable('');
            //Сегмент инфраструктуры
            if (obj.InfrastructureSegmentID)
                self.InfrastructureSegmentID = ko.observable(obj.InfrastructureSegmentID);
            else self.InfrastructureSegmentID = ko.observable(null);
            if (obj.InfrastructureSegmentName)
                self.InfrastructureSegmentName = ko.observable(obj.InfrastructureSegmentName);
            else self.InfrastructureSegmentName = ko.observable('');
            //Администратор
            self.AdministratorLoaded = ko.observable(false);
            self.Administrator = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditAdministrator, false, false));

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
            //Клиент
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
            //Примечание
            if (obj.Note)
                self.Note = ko.observable(obj.Note);
            else self.Note = ko.observable('');
            // Даты
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
            if (obj.Size)
                self.Size = ko.observable(obj.Size);
            else self.Size = ko.observable('');
            //          
            self.IconCss = ko.computed(function () {
                switch (obj.ClassID) {
                    case 12: return "model-dataEntityComponentObject-icon";
                    default: return "model-dataEntityObject-icon";
                }
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
            //
            self.FullName = ko.observable(self.ProductCatalogTypeName() + ' ' + self.Name());
        },
        DataEntityObjectRegistration: function (parent) {
            var self = this;
            //
            self.ID = ko.observable('');
            //
            self.ClassID = ko.observable(165);
            //
            self.CanEditName = ko.observable(true);
            //
            self.Name = ko.observable('');
            //Состояние 
            self.LifeCycleStateID = ko.observable(null);
            self.LifeCycleStateName = ko.observable('');
            //Управляющее приложение
            self.DeviceApplicationID = ko.observable(null);
            self.DeviceApplicationName = ko.observable('');
            //Том данных 
            self.VolumeID = ko.observable(null);
            self.VolumeName = ko.observable('');
            //Тип
            self.ProductCatalogTypeName = ko.observable('');
            self.ProductCatalogTypeID = ko.observable(null);
            //Сегмент инфраструктуры
            self.InfrastructureSegmentID = ko.observable(null);
            self.InfrastructureSegmentName = ko.observable('');
            //Администратор
            self.AdministratorLoaded = ko.observable(false);
            self.Administrator = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditAdministrator, false, false));
            self.AdministratorClassID = ko.observable(null);
            self.AdministratorID = ko.observable(null);
            self.AdministratorName = ko.observable('');
            //
            //Клиент
            self.ClientLoaded = ko.observable(false);
            self.Client = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditClient, false, false));
            //
            self.ClientName = ko.observable('');
            //
            self.ClientID = ko.observable(null);
            //
            self.ClientClassID = ko.observable(null);
            //
            //Примечание
            self.Note = ko.observable('');
            // Даты
            self.DateReceived = ko.observable('');
            //
            self.DateReceivedDT = ko.observable(null);
            //
            self.DateAnnuled = ko.observable('');
            //
            self.DateAnnuledDT = ko.observable(null);
            //
            self.Size = ko.observable('');
            //          
            self.IconCss = ko.computed(function () {
                return "model-dataEntityObject-icon";
            });
            //
            self.ShowState = ko.observable(true);
            self.FullLocation = ko.observable('');
            self.UtilizerLoaded = ko.observable(true);
            //
            self.CanEditName = ko.observable(true);
            //
            self.ProductCatalogTemplateID = ko.observable(null);
            //
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