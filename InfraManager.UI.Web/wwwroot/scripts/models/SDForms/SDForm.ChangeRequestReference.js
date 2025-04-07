define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        ChangeRequestReference: function (imList, obj) {
            var self = this;
            self.ClassID = ko.observable(703);
            self.ID = obj.ID;
            self.CanEdit = ko.observable(false);
            self.Number = obj.Number ? ko.observable(obj.Number) : ko.observable('');
            self.Name = obj.Name ? ko.observable(obj.Name) : ko.observable('');
            self.Summary = obj.Summary ? ko.observable(obj.Summary) : ko.observable('');
            self.WorkflowSchemeID = obj.WorkflowSchemeID ? ko.observable(obj.WorkflowSchemeID) : ko.observable('');
            self.EntityStateName = obj.EntityStateName ? ko.observable(obj.EntityStateName) : ko.observable('');
            self.InitiatorID = obj.InitiatorID ? ko.observable(obj.InitiatorID) : ko.observable('');
            self.UtcDatePromised = obj.UtcDatePromised ? ko.observable(parseDate(obj.UtcDatePromised)) : ko.observable('');
            self.UtcDateModified = obj.UtcDateModified ? ko.observable(parseDate(obj.UtcDateModified)) : ko.observable('');
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
                return self.WorkflowSchemeID() ? 'change-request-icon': 'finished-item-icon';
            });
            self.NumberName = ko.computed(function () {
                return '№ ' + self.Number() + ' ' + (self.Summary() ? self.Summary() : self.Name());
            });
            self.Selected = ko.observable(false);
        }
    };
    return module;
});