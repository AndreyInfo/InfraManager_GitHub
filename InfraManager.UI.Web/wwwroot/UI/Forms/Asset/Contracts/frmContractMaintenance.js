define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'dateTimeControl', './ContractMaintenance', 'jqueryStepper'], function (ko, $, ajax, formControl, fhModule, dtLib, m_object) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //
            self.CanEdit = ko.observable(true);
            self.object = ko.observable(new m_object.contractMaintenance());
            //
            {//control date start
                self.controlStart = ko.observable(null);
                //
                self.showOnlyDate = true;
                self.object().UtcDateMaintenanceStart.subscribe(function (newValue) {
                    if (!self.controlStart())
                        return;
                    //
                    var obj = self.object();
                    var dt = self.controlStart().dtpControl.length > 0 ? self.controlStart().dtpControl.datetimepicker('getValue') : null;
                    //
                    if (!newValue || newValue.length == 0)
                        obj.UtcDateMaintenanceStartDT(null);//clear field => reset value
                    else if (dtLib.Date2String(dt, self.showOnlyDate) != newValue) {
                        obj.UtcDateMaintenanceStartDT(null);//value incorrect => reset value
                        obj.UtcDateMaintenanceStart('');
                    }
                    else
                        obj.UtcDateMaintenanceStartDT(dt);
                });
                //
                self.OpenSoftwareLicenceProperties = function () {
                    if (self.object().ObjectClassID()==223) {
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowSoftwareLicenceForm(self.object().ID);
                        }); 
                    }
                    else {
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowAssetForm(self.object().ID(), self.object().ObjectClassID());
                        });
                    };
                };
                //
                self.InitDtp = function (dtpClass, dateTimeStr, dateTime, control) {
                    var dtp = self.$region.find(dtpClass);
                    var ctrl = new dtLib.control();
                    ctrl.init(dtp, {
                        StringObservable: dateTimeStr,
                        ValueDate: dateTime(),
                        OnSelectDateFunc: function (current_time, $input) {
                            dateTime(current_time);
                            dateTimeStr(dtLib.Date2String(current_time, self.showOnlyDate));
                        },
                        OnSelectTimeFunc: function (current_time, $input) {
                            dateTime(current_time);
                            dateTimeStr(dtLib.Date2String(current_time, self.showOnlyDate));
                        },
                        HeaderText: '',
                        OnlyDate: self.showOnlyDate
                    });
                    control(ctrl);
                };
            }
            //
            {//control date end
                self.controlEnd = ko.observable(null);
                //
                self.showOnlyDate = true;
                self.object().UtcDateMaintenanceEnd.subscribe(function (newValue) {
                    if (!self.controlEnd())
                        return;
                    //
                    var obj = self.object();
                    var dt = self.controlEnd().dtpControl.length > 0 ? self.controlEnd().dtpControl.datetimepicker('getValue') : null;
                    //
                    if (!newValue || newValue.length == 0)
                        obj.UtcDateMaintenanceEndDT(null);//clear field => reset value
                    else if (dtLib.Date2String(dt, self.showOnlyDate) != newValue) {
                        obj.UtcDateMaintenanceEndDT(null);//value incorrect => reset value
                        obj.UtcDateMaintenanceEnd('');
                    }
                    else
                        obj.UtcDateMaintenanceEndDT(dt);
                });
            }
            //
            self.Save = function () {
                var retval = $.Deferred();
                //
                var onMaintenance = self.object().OnMaintenance();
                var dateStart = dtLib.GetMillisecondsSince1970(self.object().UtcDateMaintenanceStartDT());
                var dateEnd = onMaintenance ? '' : dtLib.GetMillisecondsSince1970(self.object().UtcDateMaintenanceEndDT());
                //
                if (!onMaintenance && dateStart > dateEnd) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('AssetToRepairCheckDate'));
                    });
                    return;
                }
                //
                var maintenance =
                    {
                        ID: self.object().ID(),
                        ObjectClassID: self.object().ObjectClassID(),
                        ServiceContractID: self.object().ServiceContractID(),
                        UtcDateMaintenanceStartStr: dateStart,
                        UtcDateMaintenanceEndStr: dateEnd,
                        OnMaintenance: onMaintenance,
                    };
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/assetApi/EditContractMaintenance',
                    method: 'POST',
                    data: maintenance
                },
                function (response) {
                    if (response.Response.Result === 0 && response.NewModel) {
                        var obj = response.NewModel;
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContractMaintenance.js, Save]', 'error');
                        });
                    }
                });
                //
                return retval.promise();
            };
            //
            self.InitDates = function () {
                self.InitDtp('.date-start', self.object().UtcDateMaintenanceStart, self.object().UtcDateMaintenanceStartDT, self.controlStart);
                self.InitDtp('.date-end', self.object().UtcDateMaintenanceEnd, self.object().UtcDateMaintenanceEndDT, self.controlEnd);
            };
            //
            self.AfterRender = function (editor, elements) {
            };
        },

        ShowDialog: function (serviceContractID, objectID, objectClassID, isSpinnerActive, HasAgreement) {
            $.when(operationIsGrantedD(880), operationIsGrantedD(883)).done(function (can_properties, can_update) {
                //OPERATION_ServiceContractMaintenance_Add = 881,
                //OPERATION_ServiceContractMaintenance_Update = 883, 
                //OPERATION_ServiceContractMaintenance_Properties = 880,
                if (can_properties == false) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('OperationError'));
                    });
                    return;
                }
                //
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var vm = new module.ViewModel();
                var bindElement = null;
                //
                var buttons = [];
                var bSave = {
                    text: getTextResource('ButtonSave'),
                    click: function () {
                        if (vm.CanEdit() == false) {
                            frm.Close();
                            return;
                        }
                        $.when(vm.Save()).done(function (result) {
                            if (result)
                                frm.Close();
                        });
                    }
                }
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { frm.Close(); }
                }
                var hasAgreement = (HasAgreement != null ? HasAgreement : true);
                if (can_update == true && hasAgreement)
                    buttons.push(bSave);
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                        'region_frmContractMaintenance',//form region prefix
                        'setting_frmContractMaintenance',//location and size setting
                        objectClassID == 223 /*OBJ_SOFTWARE_LICENSE*/ ? getTextResource('Contract_LicenceMaintenanceListTab') : getTextResource('Contract_AssetMaintenanceListTab'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        300, 300,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/Contracts/frmContractMaintenance\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                frm.ExtendSize(600, 700);//normal size
                vm.frm = frm;
                vm.CanEdit(can_update);
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show(), vm.object().load(serviceContractID, objectID, objectClassID)).done(function (frmD, loadD) {
                    hideSpinner();
                    vm.$region = $('#' + frm.GetRegionID()).find('.frmContractMaintenance');
                    vm.InitDates();
                });
            });
        },

    };
    return module;
});