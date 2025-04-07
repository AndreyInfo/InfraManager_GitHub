define(['knockout', 'jquery', 'ajax', 'dateTimeControl'], function (ko, $, ajax, dtLib) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('Contract_GeneralTab');
            self.Template = '../UI/Forms/Asset/Contracts/frmAgreement_generalTab';
            self.IconCSS = 'generalTab';
            //
            self.dateTime_controls = [];
            //
            self.IsVisible = ko.observable(true);
            //
            //when object changed
            self.init = function (obj) {
            };
            //when tab selected
            self.load = function () {
            };
            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
                //
                for (var i = 0; i < self.dateTime_controls.length; i++)
                    self.dateTime_controls[i].datetimepicker('destroy');
                self.dateTime_controls.splice(0, self.dateTime_controls.length - 1);
            };
            //when tab validating
            self.validate = function () {
                var obj = vm.object();
                if (obj.utcDateEndDT() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_DateEndPrompt'));
                    });
                    return false;
                }
                if (obj.utcDateStartDT() > obj.utcDateEndDT()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractAgreementRegistration_DateEndLessDateStart'));
                    });
                    return false;
                }               
                if (obj.utcDateStartDT() > obj.contract().UtcDateFinishedDT()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractAgreementRegistration_DateStartLessDateContractEnd'));
                    });
                    return false;
                }
                if (obj.cost() == null || parseFloat(obj.cost()) < 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractAgreementRegistration_CostPrompt'));
                    });
                    return false;
                } 
                return true;
            };
            //            
            self.afterRender = function () {
                for (var i = 0; i < self.dateTime_controls.length; i++)
                    self.dateTime_controls[i].datetimepicker('destroy');
                self.dateTime_controls.splice(0, self.dateTime_controls.length - 1);
                //
                if (vm.addMode) {
                    self.initDateControl('.dateStart', vm.object().utcDateStartDT, vm.object().utcDateStart);
                    self.initDateControl('.dateEnd', vm.object().utcDateEndDT, vm.object().utcDateEnd);
                }
            };
            //
            {//editors
                self.costClick = function () {
                    var obj = vm.object();
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        //
                        var oldCost = obj.cost();
                        var oldNDSType = obj.ndsType();
                        var oldNDSPercent = obj.ndsPercent();
                        var oldNDSCustomValue = obj.ndsCustomValue();
                        //
                        var editOnStore = function () {
                            if (!vm.canEdit())
                                return;
                            //
                            if (obj.cost() === oldCost && obj.ndsType() === oldNDSType && obj.ndsPercent() === oldNDSPercent && obj.ndsCustomValue() === oldNDSCustomValue)
                                return;
                            //
                            showSpinner();
                            var object = vm.object();
                            var newValue = JSON.stringify({ 'Cost': obj.cost(), 'NDSType': obj.ndsType(), 'NDSPercent': obj.ndsPercent(), 'NDSCustomValue': obj.ndsCustomValue(), 'TimePeriod': null });
                            //
                            var data = {
                                ID: object.id(),
                                ObjClassID: object.classID,
                                Field: 'Cost',
                                //OldValue: JSON.stringify({ 'val': oldCost }),
                                NewValue: newValue,
                                ReplaceAnyway: true
                            };
                            self.ajaxControl.Ajax(
                                null,//self.$region, two spinner problem
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    url: '/sdApi/SetField',
                                    data: data
                                },
                                function (retModel) {
                                    if (retModel) {
                                        var result = retModel.ResultWithMessage.Result;
                                        //
                                        hideSpinner();
                                        if (result === 0) {
                                            vm.raiseObjectModified();
                                        }
                                        else {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                            });
                                        }
                                    }
                                });
                        };
                        if (vm.addMode)
                            editOnStore = null;
                        //
                        fh.ShowCostNDS(obj.cost, obj.ndsType, obj.ndsPercent, obj.ndsCustomValue, null, vm.canEdit, editOnStore);
                    });
                };
                //
                self.editDateStart = function () {
                    if (!vm.canEdit())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: object.id(),
                            objClassID: object.classID,
                            ClassID: object.classID,
                            fieldName: 'DateStarted',
                            fieldFriendlyName: getTextResource('Contract_StartDate'),
                            oldValue: object.utcDateStartDT(),
                            allowNull: true,
                            OnlyDate: true,
                            onSave: function (newDate) {
                                object.utcDateStart(parseDate(newDate, true));
                                object.utcDateStartDT(newDate ? new Date(parseInt(newDate)) : null);
                                vm.raiseObjectModified();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };
                //
                self.editDateFinish = function () {
                    if (!vm.canEdit())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: object.id(),
                            objClassID: object.classID,
                            ClassID: object.classID,
                            fieldName: 'DateFinished',
                            fieldFriendlyName: getTextResource('Contract_EndDate'),
                            oldValue: object.utcDateEndDT(),
                            allowNull: true,
                            OnlyDate: true,
                            onSave: function (newDate) {
                                object.utcDateEnd(parseDate(newDate, true));
                                object.utcDateEndDT(newDate ? new Date(parseInt(newDate)) : null);
                                vm.raiseObjectModified();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };
            }
            //           
            {//show links form
                self.showWorkOrder = function () {
                    var obj = vm.object();
                    if (!obj || !obj.contract() || obj.contract().WorkOrderID() == null)
                        return;
                    //
                    showSpinner();
                    require(['sdForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowWorkOrder(obj.contract().WorkOrderID(), fh.Mode.Default);
                    });
                };
                //
                self.showContract = function () {
                    var obj = vm.object();
                    if (!obj || obj.contract() == null)
                        return;
                    //
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowServiceContract(obj.contract().ID());
                    });
                };
            }
            //
            self.initDateControl = function (selector, ko_value, ko_valueString) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmAgreement');
                var $div = $frm.find(selector);
                showSpinner($div[0]);
                require(['dateTimePicker'], function () {
                    if (locale && locale.length > 0)
                        $.datetimepicker.setLocale(locale.substring(0, 2));
                    var control = $div.datetimepicker({
                        startDate: ko_value(),
                        closeOnDateSelect: true,
                        format: 'd.m.Y',
                        mask: '39.19.9999',
                        timepicker: false,
                        dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                        value: ko_value(),
                        validateOnBlur: true,
                        onSelectDate: function (current_time, $input) {
                            ko_valueString(dtLib.Date2String(current_time, true));
                        }
                    });
                    self.dateTime_controls.push(control);
                    hideSpinner($div[0]);
                });
            };
        }
    };
    return module;
});