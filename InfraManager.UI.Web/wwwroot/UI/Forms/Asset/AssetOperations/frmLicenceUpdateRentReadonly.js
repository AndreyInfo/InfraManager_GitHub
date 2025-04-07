define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    './GeneralTemplateLicenceReadOnly',
    './templateParamsUpdate/UpdateCharacteristics(Rent)'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        t,
        u
    ) {
        var module = {
            ViewModel: function ($region, selectedObjects,mode) {
                var self = this;
                self.$region = $region;
                self.selectedObjects = selectedObjects;
                self.$isDone = $.Deferred();//resolve, когда операция выполнена            
                self.LifeCycleStateOperationID = null;//context
                self.ProductCatalogTypeIDApply = 187;//продление подписки
                self.ProductCatalogTypeIDSubscription = 186;//подписка
                self.ProductCatalogTypeIDRent = 184;//подписка
                self.IsHasFilter = true;

                self.template = ko.observable(null);
                self.UpdateCharacteristics = ko.observable(null);

                self.template_handle = self.template.subscribe(function (f) {
                    if (f.hasOwnProperty('Load')) {
                        f.Load();
                    }
                });

                //сущность обновления
                self.SoftwareLicenceUpdateObject = ko.observable(null);

                //Report
                {
                    self.printReport = ko.observable(false);
                }
                self.IsEditMode = ko.observable(mode == 'edit');
                self.Load = function () {
                    //Активно поле выбора "Обновляемая лицензия" 
                    self.IsParentSelectable = ko.computed(function () {
                        return false;
                    });

                    //Активно поле выбора "Основания"
                    self.IsChildSelectable = ko.computed(function () {
                        return false;
                    });

                    self.SoftwareLicenceUpdateObject = selectedObjects[0];

                    //Дата вступления в силу
                    self.EffectiveDate = ko.computed(function () {
                        return self.selectedObjects[0].EffectiveDateTimeString;
                    });

                    self.SoftwareLicenceTypeName = getTextResource('SoftwareLicenceUpdateType_RentContractRenewal');

                    self.template(new t.GeneralTemplateLicenceReadOnly(self));
                    self.UpdateCharacteristics(new u.Group(self.template()));

                };

                self.dispose = function () {
                };

                self.AfterRender = function () {
                }

            },
            ShowDialog: function (selectedObjects, operationName, lifeCycleStateOperationID, isSpinnerActive,mode) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var $retval = $.Deferred();
                var bindElement = null;
                //
                $.when(userD).done(function (user) {
                    var forceClose = false;
                    //
                    var frm = undefined;
                    var vm = undefined;
                    //
                    var buttons = {};

                    //Кнопка "Закрыть"
                    forceClose = true;
                    buttons[getTextResource('Close')] = function () {
                        frm.Close();
                    };

                    frm = new fc.control(
                        'RentContractRenewal',//form region prefix
                        'RentContractRenewal_setting',//location and size setting
                        operationName,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        455, 472,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmLicenceUpdateRentReadOnly\', afterRender: AfterRender}"'//attributes of form region
                    );
                    //
                    if (!frm.Initialized)
                        return;//form with that region and settingsName was open
                    //
                    frm.BeforeClose = function () {
                        var retval = forceClose;
                        //
                        if (retval == false) {
                            require(['sweetAlert'], function () {
                                swal({
                                    title: getTextResource('FormClosingQuestion'),
                                    showCancelButton: true,
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                    function (value) {
                                        if (value == true) {
                                            forceClose = true;
                                            setTimeout(function () {
                                                frm.Close();
                                            }, 300);//TODO? close event of swal
                                        }
                                    });
                            });
                        }
                        //
                        return retval;
                    };
                    //
                    var $region = $('#' + frm.GetRegionID());
                    vm = new module.ViewModel($region, selectedObjects,mode);
                    vm.LifeCycleStateOperationID = lifeCycleStateOperationID;
                    vm.$isDone = $retval;
                    vm.Load();

                    //
                    vm.frm = frm;
                    frm.SizeChanged = function () {
                        var width = frm.GetInnerWidth();
                        var height = frm.GetInnerHeight();
                        //
                        vm.$region.css('width', width + 'px').css('height', height + 'px');
                    };
                    //
                    ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                    $.when(frm.Show(), vm.LoadD).done(function (frmD, loadD) {
                        if (loadD == false) {//force close
                            frm.Close();
                        }
                        hideSpinner();
                    });


                });
                //
                return $retval.promise();
            }
        };

        return module;
    });
