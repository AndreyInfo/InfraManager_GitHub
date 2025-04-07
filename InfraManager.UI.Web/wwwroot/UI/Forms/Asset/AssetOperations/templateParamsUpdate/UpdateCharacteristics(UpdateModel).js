define(['knockout',
    'jquery',
    'ajax'
],
    function (ko,
        $,
        ajaxLib
    ) {
        var module = {
            Group: function (vm) {
                var self = this;
                self.$region = vm.$region;

                self.SoftwareModelObj = ko.observable(null);

                self.Template = '../UI/Forms/Asset/AssetOperations/templateParamsUpdate/UpdateCharacteristics(UpdateModel)';

                self.Load = function () {

                };

                self.AfterRender = function () {

                };

                self.IsEqualSoftwareVersion = ko.observable(false);
                self.IsEqualSoftwareModel = ko.observable(false);



                self.CanEdit = ko.observable(true);

                self.ClearVersion = function () {
                    self.SoftwareModelObj(null);
                };

                self.IsVersionExists = ko.computed(function () {
                    return self.SoftwareModelObj() != null;
                });

                self.Version = ko.computed(function () {
                    let text =  (self.SoftwareModelObj() != null ) ? self.SoftwareModelObj().Version : "";
                    text = self.SoftwareModelObj() != null && /^\s*$/.test(self.SoftwareModelObj().Version) ? " -" : text; 
                    return text;
                });

                //редактирование связанной лицензии
                self.EditVersion = function () {

                    require(['ui_forms/Asset/AssetOperations/templateParamsUpdate/frmSoftwareTableSelectorModel'], function (fhModule) {
                        var form = new fhModule.Form(
                            vm.ParentSoftwareLicence(),
                            function (softwareModel) {
                                if (!softwareModel)
                                    return;

                                self.SoftwareModelObj(softwareModel);
                                self.IsEqualSoftwareVersion(softwareModel.Version == (vm.ParentSoftwareLicence() != null ? vm.ParentSoftwareLicence().SoftwareModelVersion : ""));
                                self.IsEqualSoftwareModel(softwareModel.CommercialModelName == (vm.ParentSoftwareLicence() != null ? vm.ParentSoftwareLicence().SoftwareModelName : ""));
                            },
                            true,
                            self.$region);
                        form.Show();
                    });
                }

                self.handle = vm.ParentSoftwareLicence.subscribe(function (newval) {

                });


                self.SoftwareModelParent = ko.computed(function () {
                    return vm.ParentSoftwareLicence() != null ? vm.ParentSoftwareLicence().SoftwareModelName : "";
                });

                self.SoftwareModel = ko.computed(function () {
                    return self.SoftwareModelObj() != null ? self.SoftwareModelObj().Name : "";
                });

                self.SoftwareVersionParent = ko.computed(function () {
                    return vm.ParentSoftwareLicence() != null ? vm.ParentSoftwareLicence().SoftwareModelVersion : "";
                });

            }

        };
        return module;
    });