define(['knockout', 'jquery', 'ajax','sweetAlert'], function (ko, $, ajax,swal) {
    var module = {
        Tab: function (vm) {

            var self = this;

            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('ELPTask_Form_GeneralTabName');

            self.Template = '../UI/Forms/Settings/ELP/frmELPTask_generalTab';

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
                        fieldFriendlyName: getTextResource('ELPTask_Form_Description'),
                        oldValue: object.Description(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            object.Description(newText);
                            $.when(vm.object().AddOrUpdate(vm.baseUrl)).done(function (result) {
                                if (result) {
                                    vm.raiseObjectModified();
                                }
                                else {
                                    return;
                                }
                            });
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });             
            };

            //  Редактирование производилетя
            self.EditVendor = function () {
                if (!self.canEdit())
                    return;
                //
                require(['../UI/Forms/Settings/ELP/frmELPTask_ChooseVendor'], function (module) {
                    //var edit = new module.ViewModel();
                    module.ShowDialog(self.setVendor);
                });

            };
            self.SetVendorAny = function () {
                self.setVendor({ID:null});
            };
            self.SetVendorSpecific = function () {
                self.EditVendor();
            };
            self.setVendor = function (result) {
                if (result) {
                    self.refreshVendor(result.ID, result.Label);
                    if (vm.object().ID()) {
                        $.when(vm.object().AddOrUpdate(vm.baseUrl)).done(function (result) {
                            if (result) {
                                vm.raiseObjectModified();
                            }
                            else {
                                return;
                            }
                        });
                    }
                }
            };

            self.refreshVendor = function (newID, newLabel) {
                var object = vm.object();
                object.VendorID(newID);
                object.ELPVendorAny(newID ? '0' : '1');
                if (newID && newLabel) {
                    object.VendorLabel(newLabel);
                }
                else {
                    object.VendorLabel('');
                }
                vm.raiseObjectModified();

            }
        }
    };
    return module;
});