define(['knockout'],
    function (ko) {
        var module = {
            ViewModel: function () {
                var self = this;

                self.ID = ko.observable();
                self.ParentName = ko.observable();
                self.Name = ko.observable();
                self.Note = ko.observable();

                self.resetName = function () {
                    self.Name('');
                }

                self.Load = function (parentName) {
                    self.ParentName(parentName);
                    self.Name('');
                };
            }
        };
        return module;
    });