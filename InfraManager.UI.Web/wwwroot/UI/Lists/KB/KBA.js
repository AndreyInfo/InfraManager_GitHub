define(['knockout', 'jquery', 'models/SDForms/SDForm.User'], function (ko, $, userLib) {
    var module = {
        KBArticle: function (kba, parent) {
            var self = this;
            self.ClassID = 137;
            //
            self.tagClick = ko.observable(null);
            //
            //self.MaxMailAttachmentSize = kba.MaxMailAttachmentSize;
            //
            self.ID = kba.ID;
            self.Number = ko.observable(kba.Number);
            self.NumberToShow = ko.observable('#' + kba.Number);
            self.Name = ko.observable(kba.Name ? kba.Name : '');
            self.HTMLDescription = ko.observable(kba.HTMLDescription ? kba.HTMLDescription : '');
            self.HTMLSolution = ko.observable(kba.HTMLSolution ? kba.HTMLSolution : '');
            self.HTMLAltSolution = ko.observable(kba.HTMLAlternativeSolution ? kba.HTMLAlternativeSolution : '');
            self.AuthorID = kba.AuthorID;
            self.AuthorFullName = kba.AuthorFullName;
            self.DateCreation = parseDate(kba.UtcDateCreated);
            self.DateModified = parseDate(kba.UtcDateModified);
            self.Readed = kba.ViewsCount;
            self.Used = kba.ApplicationCount;
            self.Rated = kba.Rated;
            self.StatusID = kba.StatusID;
            self.StatusName = kba.StatusName;
            self.TypeID = kba.TypeID;
            self.TypeName = kba.TypeName;
            self.Tags = ko.observableArray([]);
            self.TagsString = ko.observable('');
            self.VisibleForClient = ko.observable(kba.VisibleForClient);
            //Доступ
            self.AccessID = kba.AccessID;
            self.AccessName = kba.AccessName;
            //
            //Эксперт
            self.ExpertLoaded = ko.observable(false);
            self.Expert = ko.observable(new userLib.EmptyUser(parent, userLib.UserTypes.utilizer, parent.EditExpert, false, false));
            //
            if (kba.ExpertID)
                self.ExpertID = ko.observable(kba.ExpertID);
            else self.ExpertID = ko.observable(null);
            //
            if (kba.ExpertFullName)
                self.ExpertFullName = ko.observable(kba.ExpertFullName);
            else self.ExpertFullName = ko.observable('');
            //Даты
            if (kba.UtcDateValidUntil) 
                self.DateValidUntil = ko.observable(parseDate(Date.parse(kba.UtcDateValidUntil)));
            else self.DateValidUntil = ko.observable('');

            if (kba.DateValidUntil)
                self.DateValidUntilDT = ko.observable(new Date(parseInt(kba.UtcDateValidUntil)));
            else self.DateValidUntilDT = ko.observable(null);
            //Состояние 
            if (kba.LifeCycleStateID)
                self.LifeCycleStateID = ko.observable(kba.LifeCycleStateID);
            else self.LifeCycleStateID = ko.observable(null);

            if (kba.LifeCycleStateName)
                self.LifeCycleStateName = ko.observable(kba.LifeCycleStateName);
            else self.LifeCycleStateName = ko.observable('');
            //
            //Связанная статья 
            if (kba.KBArticleDependencyList)
                self.KBArticleDependencyList = ko.observable(kba.KBArticleDependencyList);
            else self.KBArticleDependencyList = ko.observableArray([]);
            //
            self.Section = ko.computed(function () {
                if (kba.Section)
                    return kba.Section.split('\\').join('>');
                else
                    return '';
            });
            //
            if (Array.isArray(kba.Tags)) {
                var str = '';
                ko.utils.arrayForEach(kba.Tags,
                    function (item) {
                        self.Tags.push(item);
                        //
                        if (str.length !== 0)
                            str += ', ';
                        //
                        str += item;
                    });
                self.TagsString(str);
                //
            }
            //
            self.Caption = ko.computed(function () {
                if (self.Name() && self.Number())
                    return (self.NumberToShow() + ' ' + self.Name());
                else return '';
            });
        }
    };
    //
    return module;
});