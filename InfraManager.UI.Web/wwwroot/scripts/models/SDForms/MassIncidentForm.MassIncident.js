define(['ajax', 'knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ajaxLib, ko, $, userLib) {
    var module = {
        MassIncident: function (parentSelf, miData) {
            var pself = this;
            var self = parentSelf;
            
            pself.ClassID = 823;
            if (miData.ID)
                pself.ID = ko.observable(miData.ID);

            if (miData.Name)
                pself.Name = ko.observable(miData.Name)
            else pself.Name = ko.observable('');
            
            if(miData.CreatedByUserID)
                pself.CreatedByUserID = ko.observable(miData.CreatedByUserID);

            if(miData.TypeID)
                pself.TypeID = ko.observable(miData.TypeID);

            if(miData.ServiceID)
                pself.ServiceID = ko.observable(miData.ServiceID);
            if(miData.ServiceUri)
                pself.ServiceUri = ko.observable(miData.ServiceUri);
            
            if(miData.TechnicalFailureCategoryID)
                pself.TechnicalFailureCategoryID = ko.observable(miData.TechnicalFailureCategoryID);

            if(miData.TechnicalFailureCategoryUri)
                pself.TechnicalFailureCategoryUri = ko.observable(miData.TechnicalFailureCategoryUri);
            
            if(miData.CriticalityID)
                pself.CriticalityID = ko.observable(miData.CriticalityID);

            if(miData.CriticalityUri)
                pself.CriticalityUri = ko.observable(miData.CriticalityUri);

            if(miData.PriorityUri)
                pself.PriorityUri = ko.observable(miData.PriorityUri);

            if(miData.PriorityID)
                pself.PriorityID = ko.observable(miData.PriorityID);

            if(miData.Description)
                pself.Description = ko.observable(miData.Description);

            if(miData.InformationChannelID)
                pself.InformationChannelID = ko.observable(miData.InformationChannelID);

            pself.AddAs = function () {
                return pself.ID() && pself.ID() !== '00000000-0000-0000-0000-000000000000';
            };
        }
    };
    return module;
});