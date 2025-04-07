define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        SoftwareCommercialModel: function (parent, obj) {
            var self = this;
            //
            if (obj.ID)
                self.ID = ko.observable(obj.ID);
            else self.ID = ko.observable('');
            //
            if (obj.ClassID)
                self.ClassID = ko.observable(obj.ClassID);
            else self.ClassID = ko.observable('');

            self.CanEditName = ko.observable(true);

            //Название
            if (obj.Name)
                self.Name = ko.observable(obj.Name);
            else self.Name = ko.observable('');

            //Версия
            if (obj.Version)
                self.Version = ko.observable(obj.Version);
            else self.Version = ko.observable('');

            //Шаблон
            if (obj.Template)
                self.Template = ko.observable(obj.Template);
            else self.Template = ko.observable('');

            //Лицензирование
            if (obj.SoftwareLicenceSchemeName)
                self.SoftwareLicenceSchemeName = ko.observable(obj.SoftwareLicenceSchemeName);
            else self.SoftwareLicenceSchemeName = ko.observable('');
            if (obj.SoftwareLicenceSchemeID || obj.SoftwareLicenceSchemeID === 0)
                self.SoftwareLicenceSchemeID = ko.observable(obj.SoftwareLicenceSchemeID);
            else self.SoftwareLicenceSchemeID = ko.observable(null);

            //Язык
            if (obj.SoftwareLanguageName)
                self.SoftwareLicenceLanguageName = ko.observable(obj.SoftwareLanguageName);
            else self.SoftwareLicenceLanguageName = ko.observable('');
            if (obj.SoftwareLanguageID || obj.SoftwareLicenceSchemeID === 0)
                self.SoftwareLicenceLanguageID = ko.observable(obj.SoftwareLanguageID);
            else self.SoftwareLicenceLanguageID = ko.observable(null);

            //Примечание
            if (obj.Note)
                self.Note = ko.observable(obj.Note);
            else self.Note = ko.observable('');

            //Код
            if (obj.Code)
                self.Code = ko.observable(obj.Code);
            else self.Code = ko.observable('');

            //Внешний ИД
            if (obj.ExternalID)
                self.ExternalID = ko.observable(obj.ExternalID);
            else self.ExternalID = ko.observable('');

            //Тип
            if (obj.SoftwareTypeName)
                self.SoftwareTypeName = ko.observable(obj.SoftwareTypeName);
            else self.SoftwareTypeName = ko.observable('');
            //
            if (obj.SoftwareTypeID)
                self.SoftwareTypeID = ko.observable(obj.SoftwareTypeID);
            else self.SoftwareTypeID = ko.observable('');

            //Производитель
            if (obj.ManufacturerName)
                self.ManufacturerName = ko.observable(obj.ManufacturerName);
            else self.ManufacturerName = ko.observable('');
            //
            if (obj.ManufacturerID)
                self.ManufacturerID = ko.observable(obj.ManufacturerID);
            else self.ManufacturerID = ko.observable('');


            //Редакция
            if (obj.ModelRedaction)
                self.ModelRedaction = ko.observable(obj.ModelRedaction);
            else self.ModelRedaction = ko.observable('');

            //Использование
            if (obj.SoftwareModelUsingTypeName)
                self.SoftwareModelUsingTypeName = ko.observable(obj.SoftwareModelUsingTypeName);
            else self.SoftwareModelUsingTypeName = ko.observable('');
            //
            if (obj.SoftwareModelUsingTypeID)
                self.SoftwareModelUsingTypeID = ko.observable(obj.SoftwareModelUsingTypeID);
            else self.SoftwareModelUsingTypeID = ko.observable('');

            //Владелец
            self.OwnerModelLoaded = ko.observable(false);
            self.OwnerModel = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditOwnerModel, false, false));

            if (obj.OwnerModelClassID)
                self.OwnerModelClassID = ko.observable(obj.OwnerModelClassID);
            else self.OwnerModelClassID = ko.observable('');
            //
            if (obj.OwnerModelID)
                self.OwnerModelID = ko.observable(obj.OwnerModelID);
            else self.OwnerModelID = ko.observable('');
            //
            if (obj.OwnerModelName)
                self.OwnerModelName = ko.observable(obj.OwnerModelName);
            else self.OwnerModelName = ko.observable('');

            //Группа
            self.ModelSupportQueueLoaded = ko.observable(false);
            self.ModelSupportQueue = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditSupportQueue, false, false));

            if (obj.QueueModelID)
                self.ModelSupportQueueID = ko.observable(obj.QueueModelID);
            else self.ModelSupportQueueID = ko.observable('');
            //
            if (obj.QueueModelName)
                self.ModelSupportQueueName = ko.observable(obj.QueueModelName);
            else self.ModelSupportQueueName = ko.observable('');
            //
            if (obj.QueueModelClassID)
                self.ModelSupportQueueClass = ko.observable(obj.QueueModelClassID);
            else self.ModelSupportQueueClass = ko.observable('');
            
            self.IconCss = ko.computed(function () {

                return "model-soft-commercial-icon";

            });
            //
            //
            self.FullName = ko.observable(self.Name());
            //
            if (obj.VersionRecognition)
                self.VersionRecognition = ko.observable(obj.VersionRecognition);
            else self.VersionRecognition = ko.observable('0');
            if (obj.VersionRecognitionLvl)
                self.VersionRecognitionLvl = ko.observable(obj.VersionRecognitionLvl);
            else self.VersionRecognitionLvl = ko.observable(0);
            if (obj.RedactionRecognition)
                self.RedactionRecognition = ko.observable(obj.RedactionRecognition);
            else self.RedactionRecognition = ko.observable('0');
        }
    };
    return module;
});