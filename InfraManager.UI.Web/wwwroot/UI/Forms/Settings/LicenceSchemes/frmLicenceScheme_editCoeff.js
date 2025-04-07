define(['knockout', 'jquery', 'ajax', 'formControl'], function (ko, $, ajax, formControl) {
    var module = {
        ViewModel: function (obj, $region) {
            var self = this;

            self.vmTab = obj.vmTab;
            self.$region = $region;

            self.ProcessorModelName = ko.observable(obj.ProcessorTypeName);
            self.ProcessorModelID = ko.observable(obj.ProcessorTypeID);
            self.oldProcessorTypeID = obj.ProcessorTypeID;
            //
            self.CoefficientValue = ko.observable(obj.Coefficient);
            //
            self.frm = null;
            //
            self.captionText = ko.pureComputed(function () { return getTextResource('LicenceSchemeCoeffEdit_Caption'); });
            //
            self.afterRender = function () {
                self.initNumericUpDownControl('.coefficientValue', self.CoefficientValue, 1, 999999, true);
            };

            self.initNumericUpDownControl = function (selector, ko_value, minValue, maxValue, is_enable) {
                var $frm = $('#' + self.frm.GetRegionID()).find('.frmLicenceScheme_editCoeff');
                var $div = $frm.find(selector);
                showSpinner($div[0]);
                require(['jqueryStepper'], function () {
                    $div.stepper({
                        type: 'int',
                        floatPrecission: 0,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [minValue, maxValue],
                        onStep: function (val, up) {
                            ko_value(val);
                        },
                        isReadonly: !is_enable
                    });
                    hideSpinner($div[0]);
                });
            };

            self.SelectProcessorName = function () {
                require(['ui_forms/Settings/LicenceSchemes/frmLicenceScheme_addCoefficient'], function (jsModule) {
                    $.when(jsModule.ShowDialog(self.ChangeHandle)).done(function () {
                        retval.resolve();
                    });
                });


            };
            self.dispose = function () {

            };
            //
            self.Load = function () {
            };

            self.handleReady = function () {

            };

            self.ChangeHandle = function (newValue) {
                self.ProcessorModelID(newValue.ID);
                self.ProcessorModelName(newValue.Name);
                self.CoefficientValue(newValue.Coefficient);
            };
        },
        //
        coefficientEditor: function () {
            var self = this;
            self.vm = null;

            self.ShowEdit = function (obj) {
                showSpinner();

                var frm = undefined;
                var buttons = [];
                var bReady = {
                    text: getTextResource('LicenceSchemeCoeffEdit_ReadyButton'),
                    click: function () {
                        vm.vmTab.CoeffReadyForEdit(vm.ProcessorModelID(), vm.oldProcessorTypeID, vm.CoefficientValue());
                        setTimeout(function () {
                            frm.Close();
                        }, 300);
                    }
                }

                var bDelete = {
                    text: getTextResource('LicenceSchemeCoeffEdit_DeleteButton'),
                    click: function () {
                        require(['sweetAlert'], function () {
                            swal({
                                title: getTextResource('LicenceSchemeCoeffEdit_DeleteQuestion'),
                                showCancelButton: true,
                                closeOnConfirm: true,
                                closeOnCancel: true,
                                confirmButtonText: getTextResource('ButtonOK'),
                                cancelButtonText: getTextResource('ButtonCancel')
                            },
                                function (value) {
                                    if (value == true) {
                                        vm.vmTab.CoeffDeleteForEdit(vm.ProcessorModelID());
                                        setTimeout(function () {
                                            frm.Close();
                                        }, 300);//TODO? close event of swal
                                    }
                                });
                        });
                    }
                }

                var bCancel = {
                    text: getTextResource('LicenceSchemeCoeffEdit_CancelButton'),
                    click: function () {
                        forceClose = true;
                        setTimeout(function () {
                            frm.Close();
                        }, 300);//TODO? close event of swal
                    }
                }

                buttons.push(bDelete);
                buttons.push(bCancel);
                buttons.push(bReady);

                frm = new formControl.control(
                    'region_frmLicenceSchemeEditCoeff',//form region prefix
                    'setting_frmLicenceSchemeEditCoeff',//location and size setting
                    getTextResource('LicenceScheme_EditCoeff_Caption'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    500, 200,//minSize
                    buttons,//form buttons
                    function () {
                        //ko.cleanNode(bindElement);
                        vm.dispose();                           
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Settings/LicenceSchemes/frmLicenceScheme_editCoeff\', afterRender: afterRender}"'//attributes of form region
                );
                if (!frm.Initialized) {
                    return;
                }
                frm.ExtendSize(500, 200);

                var region = $('#' + frm.GetRegionID());
                var vm = new module.ViewModel(obj, region);
                vm.frm = frm;
                self.vm = vm;
                ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                //
                var frmD = frm.Show();
                var LoadD = vm.Load();
                $.when(frmD, LoadD).done(function (ctrlResult, loadResult) {
                    hideSpinner();
                });
            };

        }
    }
    return module;
});