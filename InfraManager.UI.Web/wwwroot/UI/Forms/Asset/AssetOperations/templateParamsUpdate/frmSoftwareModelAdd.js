define(['knockout', 'jquery', 'ajax', 'formControl'],
    function (ko, $, ajax, formControl) {
        var module = {
            Form: function (softwareLicence, onSaveCallback) {
                var self = this;
                self.ajaxControl = new ajax.control();
                //                
                self.Version = ko.observable(null);
                self.ProductName = ko.observable(softwareLicence.SoftwareModelName);
                self.Template = ko.observable(softwareLicence.SoftwareModelTemplateName);
                self.SoftwareTypeName = ko.observable(softwareLicence.SoftwareTypeName);
                self.VendorName = ko.observable(softwareLicence.ManufacturerName);
                self.onSaveCallback = onSaveCallback;
                //
                //when tab unload
                self.dispose = function () {
                    self.ajaxControl.Abort();
                };
                //
                self.Name = getTextResource('CreateVersion');                
                //show form
                self.Show = function () {
                    showSpinner();
                    //
                    var forceClose = false;

                    var buttons = {};

                    buttons[getTextResource('ButtonCancel')] = function () {
                        forceClose = false;
                        ctrl.Close();
                    };


                    self.Version.subscribe(function (newValue) {
                        var newButtons = {}
                        if (newValue != null && !/^\s*$/.test(newValue)) {

                            newButtons[getTextResource('ButtonSave')] = function () {
                                if (self.onSaveCallback)
                                    self.onSaveCallback(self.Version());
                                forceClose = false;
                                ctrl.Close();

                            };

                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                ctrl.Close();
                            };
                        }
                        else {
                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                ctrl.Close();
                            };
                        }
                        if (!forceClose)
                            ctrl.UpdateButtons(newButtons);
                    });
                    
                   
                    var ctrl = undefined;
                    ctrl = new formControl.control(
                        'frmSoftwareModel',//form region prefix
                        'frmSoftwareModelAdd',//location and size setting
                        self.Name,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        300, 350,//minSize
                        buttons,//form buttons
                        function () {
                            self.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/templateParamsUpdate/frmSoftwareModelAdd\'}"'//attributes of form region
                    );
                    if (!ctrl.Initialized)
                        return;
                    ctrl.Show();

                    bindElement = document.getElementById(ctrl.GetRegionID());
                    ko.applyBindings(self, bindElement);

                    hideSpinner();
                }
            }
        };
        return module;
    });