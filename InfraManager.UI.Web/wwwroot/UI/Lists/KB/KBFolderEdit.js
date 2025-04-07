define(['knockout'],
    function (ko) {
        var module = {
            ViewModel: function () {
                var self = this;

                self.ID = ko.observable();
                self.Name = ko.observable();
                self.Note = ko.observable();

                self.resetName = function () {
                    self.Name('');
                }
                
                self.Load = function (name, note) {
                    self.Name(name);
                    self.Note(note);
                };
            }
        };
        return module;
    });