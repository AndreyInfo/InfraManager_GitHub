define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        TapeRecord: function (obj, options) {
            var self = this;
            //
            self.entityClassID = options.entityClassID;
            self.entityID = options.entityID;
            //
            self.ID = obj.ID;
            self.Type = ko.observable(options.type);
            self.Text = ko.observable(obj.Message);
            self.AuthorID = ko.observable(obj.UserID);
            self.AuthorFullName = ko.observable(obj.UserName);
            if (obj.IsRead != undefined)
                self.IsRead = ko.observable(obj.IsRead);
            else
                self.IsRead = ko.observable(true);
            if (obj.IsNote != undefined)
                self.IsNote = ko.observable(obj.IsNote);
            else
                self.IsNote = ko.observable(false);
            //
            var date = new Date(getUtcDate(obj.UtcDate));
            self.LocalDate = ko.observable(convertToLocalDate(date));
            self.DateObj = ko.observable(date);
            //
            self.Merge = function (newData) {
                if (!newData)
                    return;
                //
                if (newData.Message != undefined)
                    self.Text(newData.Message);
                if (newData.AuthorID != undefined)
                    self.AuthorID(newData.UserID);
                if (newData.AuthorFullName != undefined)
                    self.AuthorFullName(newData.UserName);
                if (newData.IsRead != undefined)
                    self.IsRead(newData.IsRead);
                if (newData.IsNote != undefined)
                    self.IsNote(newData.IsNote);
                if (newData.DateForJs != undefined)
                    self.LocalDate(parseDate(newData.DateForJs));
                if (newData.UtcDate != undefined)
                    self.DateObj(new Date(getUtcDate(newData.UtcDate)));
            };
        }
    };
    return module;
});