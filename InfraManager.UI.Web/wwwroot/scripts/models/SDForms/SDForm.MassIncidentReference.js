define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        MassIncidentReference: function (imList, obj) {
            var self = this;
            self.ClassID = obj.ClassID ? ko.observable(obj.ClassID) : ko.observable(823);
            self.ID = obj.IMObjID;
            self.CanEdit = ko.observable(false);
            self.Number = obj.ID ? ko.observable(obj.ID) : ko.observable('');
            self.Name = obj.Name ? ko.observable(obj.Name) : ko.observable('');
            self.Summary = ko.observable('');
            self.WorkflowSchemeID = obj.WorkflowSchemeID ? ko.observable(obj.WorkflowSchemeID) : ko.observable('');
            self.EntityStateName = obj.EntityStateName ? ko.observable(obj.EntityStateName) : ko.observable('');
            self.InitiatorID = obj.CreatedByUserID ? ko.observable(obj.CreatedByUserID) : ko.observable('');
            self.UtcDatePromised = obj.UtcCloseUntil ? ko.observable(parseDate(obj.UtcCloseUntil)) : ko.observable('');
            self.UtcDateModified = obj.UtcDateModified ? ko.observable(parseDate(obj.UtcDateModified)) : ko.observable('');
            self.Uri = obj.Uri;
            const options = {
                UserID: self.InitiatorID(),
                UserType: userLib.UserTypes.workInitiator,
                UserName: '',
                EditAction: null,
                RemoveAction: null
            };
            self.InitiatorObj = self.InitiatorID() !== '' ? ko.observable(new userLib.User(self, options)) : ko.observable(null); 
            self.Modify = ko.computed(function () {
                return getTextResource('LastChange') + ': ' + self.UtcDateModified();
            });
            self.CssIconClass = ko.computed(function () {
                return self.WorkflowSchemeID() ? 'mass-incident-icon': 'finished-item-icon';
            });
            self.NumberName = ko.computed(function () {
                return '№ ' + self.Number() + ' ' + (self.Summary() ? self.Summary() : self.Name());
            });
            self.Selected = ko.observable(false);
        }
    };
    return module;
});