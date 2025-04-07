define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    './GeneralTemplateLicence',
    './templateParamsUpdate/UpdateCharacteristics(Upgrade)'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        t,
        u
    ) {
        var module = {
            ViewModel: function ($region, selectedObjects, mode) {
                var self = this;
                self.$region = $region;
                self.selectedObjects = selectedObjects;
                self.$isDone = $.Deferred();//resolve, когда операция выполнена            
                self.LifeCycleStateOperationID = null;//context
                self.ProductCatalogTypeIDUpgrade = 185;//Upgrade
                self.ProductCatalogTypeIDSingle = 183;//unit
                self.IsHasFilter = true;

                self.template = ko.observable(null);
                self.UpdateCharacteristics = ko.observable(null);
                self.IsEditMode = ko.observable(mode == 'edit');

                self.template_handle = self.template.subscribe(function (f) {
                    if (f.hasOwnProperty('Load')) {
                        f.Load();
                    }
                });

                self.mode = mode;

                //Report
                {
                    self.printReport = ko.observable(false);
                }

                self.Load = function () {

                    self.FilterProductCatalogTypeID = (self.selectedObjects[0].ProductCatalogTemplate === self.ProductCatalogTypeIDUpgrade)
                        ? self.ProductCatalogTypeIDSingle
                        : self.ProductCatalogTypeIDUpgrade
                    //Активно поле выбора "Обновляемая лицензия" 
                    self.IsParentSelectable = ko.computed(function () {
                        return (self.IsEditMode()) ?
                            self.selectedObjects[0].ProductCatalogTemplate === self.ProductCatalogTypeIDUpgrade
                            : false;
                    });

                    //Активно поле выбора "Основания"
                    self.IsChildSelectable = ko.computed(function () {
                        return (self.IsEditMode()) ?
                            self.selectedObjects[0].ProductCatalogTemplate === self.ProductCatalogTypeIDSingle
                            : false;
                    });

                    //Дата вступления в силу
                    self.EffectiveDate = ko.computed(function() {
                        return (self.IsEditMode()) ?
                            self.selectedObjects[0].NewEndDateString
                            : self.selectedObjects[0].EffectiveDateTimeString;
                    });

                    self.SoftwareLicenceTypeName = getTextResource('SoftwareLicenceUpdateType_Upgrade');

                    self.template(new t.GeneralTemplateLicence(self));
                    self.UpdateCharacteristics(new u.Group(self.template()));

                };
                self.dispose = function () {
                };

                self.AfterRender = function () {
                }

                self.Upgrade = function () {

                    self.ajaxControl = new ajaxLib.control();

                    let data = {
                        'DtApply': self.template().periodEndDateTime().toISOString(),
                        'SoftwareLicenceID': self.template().SoftwareLicenceObject().ID,
                        'LifeCycleStateOperationID': self.LifeCycleStateOperationID,
                        'SoftwareLicenceParentID': self.template().ParentSoftwareLicence().ID
                    };

                    if (self.IsChildSelectable()) {
                        data = {
                            'DtApply': self.template().periodEndDateTime().toISOString(),
                            'SoftwareLicenceID': self.template().ParentSoftwareLicence().ID,
                            'LifeCycleStateOperationID': self.LifeCycleStateOperationID,
                            'SoftwareLicenceParentID': self.template().SoftwareLicenceObject().ID
                        }
                    }

                    self.ajaxControl.Ajax(self.$region,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: '/assetApi/AssetLicenceUpgrade'
                        },
                        function (newVal) {                            
                            if (newVal) {
                                if (newVal.Result === 0) {
                                    self.$isDone.resolve(true);
                                    //
                                    var message = getTextResource('AssetLicenceUpgrade_Sucsess');
                                    //
                                    var succsess = true;
                                    if (self.printReport()) {
                                        if (newVal.PrintReportResult === 2)//no report
                                        {
                                            succsess = false;
                                            require(['sweetAlert'], function () {
                                                swal(message, getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoReport'), 'info');
                                            });
                                        }
                                        else if (newVal.PrintReportResult === 3)//no ID parameter
                                        {
                                            succsess = false;
                                            require(['sweetAlert'], function () {
                                                swal(message, getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoParam'), 'info');
                                            });
                                        }
                                        else {
                                            if (newVal.FileInfoList != null) {
                                                var reportControl = new fcLib.control();
                                                newVal.FileInfoList.forEach(function (el) {
                                                    var item = new reportControl.CreateItem(el.ID, el.ObjectID, el.FileName, '', '', '', 'pdf');
                                                    reportControl.DownloadFile(item);
                                                });
                                            }
                                        }
                                    }
                                    //
                                    if (succsess)
                                        require(['sweetAlert'], function () {                                            
                                            swal(message);
                                        });
                                }
                                else {
                                    self.$isDone.resolve(true);
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AssetOperation_Error'), 'error');
                                    });
                                }
                            }
                        });
                    //
                    return self.$isDone.promise();
                }

            },
            ShowDialog: function (selectedObjects, operationName, lifeCycleStateOperationID, isSpinnerActive, mode) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var $retval = $.Deferred();
                var bindElement = null;
                //
                $.when(userD).done(function (user) {
                    var isReadOnly = false;
                    var forceClose = false;
                    //
                    if (user.HasRoles == false)
                        isReadOnly = true;
                    //
                    var frm = undefined;
                    var vm = undefined;
                    //
                    var buttons = {};

                    if (mode == 'edit') {
                        buttons[getTextResource('ButtonCancel')] = function () {
                            forceClose = false;
                            frm.BeforeClose();
                        };
                    } else {
                        //Кнопка "Закрыть"
                        forceClose = true;
                        buttons[getTextResource('Close')] = function () {
                            frm.Close();
                        };
                    }

                    frm = new fc.control(
                        'frmLicenceUpgrade',//form region prefix
                        'frmLicenceUpgrade_setting',//location and size setting
                        operationName,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        500, 500,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmLicenceUpgrade\', afterRender: AfterRender}"'//attributes of form region
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
                    vm = new module.ViewModel($region, selectedObjects, mode);
                    vm.LifeCycleStateOperationID = lifeCycleStateOperationID;
                    vm.$isDone = $retval;
                    vm.Load();

                    vm.template().periodEndDateTime.subscribe(function (newValue) {
                        var newButtons = {}
                        if (newValue != null && vm.template().SoftwareLicenceObject() != null && vm.template().ParentSoftwareLicence() != null && vm.IsEditMode) {

                            newButtons[operationName] = function () {
                                var d = vm.Upgrade();
                                $.when(d).done(function (result) {
                                    if (!result)
                                        return;
                                    //                                    
                                    forceClose = true;
                                    frm.Close();
                                });
                            };

                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                frm.BeforeClose();
                            };
                        }
                        else {
                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                frm.BeforeClose();
                            };
                        }
                        if (!forceClose)
                            frm.UpdateButtons(newButtons);
                    });

                    vm.template().SoftwareLicenceObject.subscribe(function (newValue) {
                        var newButtons = {}
                        if (newValue != null && vm.template().periodEndDateTime() != null && vm.template().ParentSoftwareLicence() != null && vm.IsEditMode()) {

                            newButtons[operationName] = function () {
                                var d = vm.Upgrade();
                                $.when(d).done(function (result) {
                                    if (!result)
                                        return;
                                    //                                    
                                    forceClose = true;
                                    frm.Close();
                                });
                            };

                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                frm.BeforeClose();
                            };
                        }
                        else {
                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                frm.BeforeClose();
                            };
                        }
                        if (!forceClose)
                            frm.UpdateButtons(newButtons);
                    });

                    vm.template().ParentSoftwareLicence.subscribe(function (newValue) {
                        var newButtons = {}
                        if (newValue != null && vm.template().periodEndDateTime() != null && vm.template().SoftwareLicenceObject() != null && vm.IsEditMode()) {

                            newButtons[operationName] = function () {
                                var d = vm.Upgrade();
                                $.when(d).done(function (result) {
                                    if (!result)
                                        return;
                                    //                                    
                                    forceClose = true;
                                    frm.Close();
                                });
                            };

                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                frm.BeforeClose();
                            };
                        }
                        else {
                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                frm.BeforeClose();
                            };
                        }
                        if (!forceClose)
                            frm.UpdateButtons(newButtons);
                    });

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
