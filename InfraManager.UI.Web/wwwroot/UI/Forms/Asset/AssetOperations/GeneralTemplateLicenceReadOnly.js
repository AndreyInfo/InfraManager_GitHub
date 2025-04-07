define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    'dateTimeControl'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        dtLib
    ) {
        var module = {
            GeneralTemplateLicenceReadOnly: function (vm) {
                var self = this;
                self.$region = vm.$region;

                self.Template = '../UI/Forms/Asset/AssetOperations/GeneralTemplateLicenceReadOnly';

                self.Load = function () {
                    self.ParentSoftwareLicence(vm.SoftwareLicenceUpdateObject);
                    self.SoftwareLicenceUpdateObject = vm.SoftwareLicenceUpdateObject;
                    self.SofLicenceObject = ko.observable(vm.SoftwareLicenceUpdateObject);
                    self.OldEndDate(new Date(self.SofLicenceObject().OldEndDate));
                    self.NewEndDate(new Date(self.SofLicenceObject().NewEndDate));
                };
                self.dispose = function () {

                };
                
                self.AfterRender = function () {
                };

                self.CanEdit = ko.observable(true);
                self.ParentSoftwareLicence = ko.observable(null);

                self.SoftwareLicenceObject = ko.observable(null);

                self.SoftwareLicenceUpdateObject = ko.observable(null);

                //Проставлено ли основание
                self.IsChildSoftwareLicenceExists = ko.computed(function () {
                    return self.SoftwareLicenceObject() != null;
                });

                //Обновляемая лицензия
                self.ParentSoftwareLicenceName = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.SoftwareLicenceName;
                });

                //Дата вступления в силу
                self.EffectiveDate = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.EffectiveDateTimeString;
                });

                //Тип обновления
                self.ProductType = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.UpdateType;
                });

                //Основание
                self.Reason = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.CorrespondingObjectName;
                });

                self.getSoft = function () {
                    var softLicenceUpdate = vm.selectedObjects[0];
                    //заполнение объекта обновления
                    self.SoftwareLicenceUpdateObject(softLicenceUpdate);
                };

                //открыть форму просмотра основания 
                self.OpenCorrespondingObjectForm = function () {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        if (vm.SoftwareLicenceUpdateObject.CorrespondingLicenceID) {
                            fh.ShowSoftwareLicenceForm(vm.SoftwareLicenceUpdateObject.CorrespondingLicenceID);
                        } else if (vm.SoftwareLicenceUpdateObject.CorrespondingContractID) {
                            fh.ShowServiceContract(vm.SoftwareLicenceUpdateObject.CorrespondingContractID);
                        }
                    });
                };

                //открыть форму просмотра связанной лицензии
                self.OpenParentSoftwareLicenceForm = function () {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSoftwareLicenceForm(vm.SoftwareLicenceUpdateObject.SoftwareLicenceID);
                    });
                };
                //
                self.OldEndDate = ko.observable(null);
                self.NewEndDate = ko.observable(null);
            }
        };
        return module;
    });