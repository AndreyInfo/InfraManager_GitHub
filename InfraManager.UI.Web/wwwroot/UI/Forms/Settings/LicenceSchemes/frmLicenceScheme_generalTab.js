define(['knockout', 'jquery', 'ajax'], function (ko, $, ajax) {
    var module = {
        Tab: function (vm) {

            var self = this;

            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('LicenceScheme_Form_GeneralTabName');

            self.Template = '../UI/Forms/Settings/LicenceSchemes/frmLicenceScheme_generalTab';

            self.IconCSS = 'generalTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.canEdit = vm.CanEdit;//for userlib
            //
            self.$region = vm.$region;
          
            //when object changed
            self.Initialize = function (obj) {
            };
            //when tab selected
            self.load = function () {
            };

            self.AfterRender = function () {
            }

            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
            };         

            //редактирование поля "Описание"
            self.editDescription = function () {
                if (!vm.CanEdit()) {
                    return;
                }

                showSpinner();
                var object = vm.object();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'Description',
                        fieldFriendlyName: getTextResource('LicenceScheme_Form_Description'),
                        oldValue: object.Description(),
                        allowNull: true,
                        maxLength: 255,
                        save: function (data) {
                            vm.object().Description(JSON.parse(data.NewValue).text);
                            return $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                                if (result) {
                                    vm.raiseObjectModified();
                                }
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });             
            };
        }
    };
    return module;
});