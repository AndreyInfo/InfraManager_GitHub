define(['knockout', 'jquery', 'ajax', 'formControl'],
    function (ko, $, ajax, formControl) {
        var module = {
            Form: function (onSaveCallback) {
                var self = this;
                self.ajaxControl = new ajax.control();
                //
                self.Name = getTextResource('AddSerialNumber');
                self.SerialNumber = ko.observable('');
                self.Ph = ko.observable(getTextResource('EnterSerialNumber'));
                self.onSaveCallback = onSaveCallback;
                //
                //when tab unload
                self.dispose = function () {
                    self.ajaxControl.Abort();
                };
                //show form
                self.Show = function () {
                    showSpinner();
                    //
                    var buttons = [];
                    var bSave = {
                        text: getTextResource('ButtonSave'),
                        click: function () {
                            if (self.onSaveCallback)
                                self.onSaveCallback(self.SerialNumber());
                            ctrl.Close();
                        }
                    }
                    var bCancel = {
                        text: getTextResource('Close'),
                        click: function () {
                            ctrl.Close();
                        }
                    }
                    buttons.push(bSave);
                    buttons.push(bCancel);

                    var ctrl = undefined;
                    ctrl = new formControl.control(
                        'frmSoftwareLicence',//form region prefix
                        'frmSoftwareLicenseSerialNumberAdd',//location and size setting
                        self.Name,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        300, 200,//minSize
                        buttons,//form buttons
                        function () {
                            self.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicenceSerialNumbersAdd\'}"'//attributes of form region
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