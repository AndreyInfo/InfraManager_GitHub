define(['knockout', 'jquery'], function (ko, $) {
    var module = {
        KBArticle: function (obj) {
            var kself = this;
            //
            this.Name = obj.Name;
            this.Section = obj.Section;
            this.Number = obj.Number;
            this.ID = obj.ID;
            this.Solution = obj.Solution;
            this.Author = getTextResource('Author') + obj.AuthorFullName;
            this.Description = obj.Description;
            this.FileCount = getTextResource('AttachmentsText') + obj.DocumentsCount;
            this.IsVisible = obj.Visible;
            this.Date = obj.UtcDateModified != null ?
                    getTextResource('Updated') + parseDate(obj.UtcDateModified) :
                    getTextResource('Published') + parseDate(obj.UtcDateCreated);
            this.Selected = ko.observable(false);
            this.Icon = obj.Icon;
            //
            kself.Caption = ko.computed(function () {
                if (kself.Name && kself.Number)
                    return ('#' + kself.Number + ' ' + kself.Name);
                else return kself.Name;
            });
        }
    };
    return module;
});