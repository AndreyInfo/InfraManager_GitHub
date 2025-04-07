define(['knockout', 'jquery', 'ajax', 'models/SDForms/SDForm.User', 'jqueryStepper'], function (ko, $, ajax, userLib, m_st) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.test = ko.observable('Witout');
            self.parent = vm;
            self.Obj = vm.SoftwareCommercialModel;
            self.ajaxControl = new ajax.control();
            self.Template = '../UI/Forms/Asset/Models/frmSoftwareModelRecognition';
            //
            self.IsVisible = ko.observable(false);

            self.VersionRecognition = ko.computed({
                read: function () {
                    if (self.Obj() == null)
                        return '0';
                    return self.Obj() == null ? '0' : String(self.Obj().VersionRecognition());
                },
                write: function (newValue) {
                    var oldVal = self.Obj().VersionRecognition();

                    self.Obj().VersionRecognition(newValue);

                    var options = {
                        FieldName: 'SoftwareCommercialModel.VersionRecognitionID',
                        OldValue: JSON.stringify({ 'id': oldVal }),
                        NewValue: JSON.stringify({ 'id': newValue }),
                        onSave: function () {
                        }
                    };
                    self.parent.UpdateField(false, options)
                    self.VisibleVersionRecognition();
                }
            });
            self.Obj.subscribe(function () {
                self.VisibleVersionRecognition();
            })
            self.VisibleVersionRecognition = function () {
                if (self.Obj() && self.Obj().VersionRecognition() == 2) {
                    self.IsVisible(true);
                }
                else self.IsVisible(false);
            };

            self.RedactionRecognition = ko.computed({
                read: function () {
                    if (self.Obj() == null)
                        return '0';
                    return self.Obj() == null ? '0' : String(self.Obj().RedactionRecognition());
                },
                write: function (newValue) {
                    var oldVal = self.Obj().RedactionRecognition();

                    self.Obj().RedactionRecognition(newValue);

                    var options = {
                        FieldName: 'SoftwareCommercialModel.RedactionRecognition',
                        OldValue: JSON.stringify({ 'val': oldVal }),
                        NewValue: JSON.stringify({ 'val': newValue }),
                        onSave: function () {
                        }
                    };
                    self.parent.UpdateField(false, options)
                }
            });

            self.VersionRecognitionLvl = ko.computed({
                read: function () {
                    if (self.Obj() == null)
                        return '0';
                    return self.Obj() == null ? '0' : String(self.Obj().VersionRecognitionLvl());
                },
                write: function (newValue) {
                    self.SetVersionRecognitionLvl(newValue);
                }              
            });

            self.VersionRecognitionLvlMinus = function () {
                if (self.Obj().VersionRecognitionLvl() != 0)
                    self.SetVersionRecognitionLvl(self.Obj().VersionRecognitionLvl() - 1);
                return;
            }
            
            self.VersionRecognitionLvlPlus = function () {
                    self.SetVersionRecognitionLvl(self.Obj().VersionRecognitionLvl() + 1);
            }           

            self.SetVersionRecognitionLvl = function (newValue) {
                var oldVal = self.Obj().VersionRecognitionLvl();

                self.Obj().VersionRecognitionLvl(newValue);

                var options = {
                    FieldName: 'SoftwareCommercialModel.VersionRecognitionLvl',
                    OldValue: JSON.stringify({ 'val': oldVal }),
                    NewValue: JSON.stringify({ 'val': newValue }),
                    onSave: function () {
                    }
                };
                self.parent.UpdateField(false, options)
                
            }           
        }
    };
    return module;
});