define(['knockout', 'jquery', 'ajax', 'dateTimeControl', 'usualForms'], function (ko, $, ajax, dtLib, fhModule) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('Contract_GeneralTab');
            self.Template = '../UI/Forms/Asset/Contracts/frmContractRegistration_generalTab';
            self.IconCSS = 'generalTab';
            //
            self.dateTime_controls = [];
            self.searcher_controls = [];
            //
            //when object changed
            self.init = function (obj) {

            };
            //when tab selected
            self.load = function () {

            };
            //when tab validating
            self.validate = function () {
                var obj = vm.object();              
                if (obj.productCatalogTypeID() == null && obj.serviceContractModelID() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_TypeOrModelPrompt'));
                    });
                    return false;
                }
                if (obj.supplierID() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_SupplierPrompt'));
                    });
                    return false;
                }
                if (obj.dateRegistered() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_DateRegistrationPrompt'));
                    });
                    return false;
                }
                if (obj.dateStart() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_DateStartPrompt'));
                    });
                    return false;
                }
                if (obj.dateEnd() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_DateEndPrompt'));
                    });
                    return false;
                }
                else if (obj.dateStart() > obj.dateEnd()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_DateEndLessDateStart'));
                    });
                    return false;
                }
                if (obj.timePeriodID() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_TimePeriodPrompt'));
                    });
                    return false;
                }
                if (obj.updateTypeID() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_UpdateTypePrompt'));
                    });
                    return false;
                }
                if (obj.updatePeriod() == null || parseInt(obj.updatePeriod()) < 0 || parseInt(obj.updatePeriod()) > 255) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_UpdatePeriodOutOfRange'));
                    });
                    return false;
                }
                if (obj.cost() == null || parseFloat(obj.cost()) < 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_CostPrompt'));
                    });
                    return false;
                }
                return true;
            };
            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
                //
                for (var i = 0; i < self.dateTime_controls.length; i++)
                    self.dateTime_controls[i].datetimepicker('destroy');
                self.dateTime_controls.splice(0, self.dateTime_controls.length - 1);
                //
                for (var i = 0; i < self.searcher_controls.length; i++)
                    self.searcher_controls[i].Remove();
                self.searcher_controls.splice(0, self.searcher_controls.length - 1);
            };
            //            
            self.afterRender = function () {
                for (var i = 0; i < self.dateTime_controls.length; i++)
                    self.dateTime_controls[i].datetimepicker('destroy');
                self.dateTime_controls.splice(0, self.dateTime_controls.length - 1);
                //
                for (var i = 0; i < self.searcher_controls.length; i++)
                    self.searcher_controls[i].Remove();
                self.searcher_controls.splice(0, self.searcher_controls.length - 1);
                //
                self.initDateControl('.dateRegistered', vm.object().dateRegistered, vm.object().dateRegisteredString);
                self.initDateControl('.dateStart', vm.object().dateStart, vm.object().dateStartString);
                self.initDateControl('.dateEnd', vm.object().dateEnd, vm.object().dateEndString);
                //
                self.initNumericUpDownControl('.updatePeriod', vm.object().updatePeriod, 0, 255);
                //
                self.initSearcherControl('.productCatalogue', 'ServiceContractTypeAndModelSearcher', vm.object().productCatalogID, vm.object().productCatalogFullName, vm.object().productCatalogClassID);
                self.initSearcherControl('.manufacturer', 'ManufacturerSearcher', vm.object().manufacturerID, vm.object().manufacturerName);
                self.initSearcherControl('.financeCenter', 'FinanceCenterSearcher', vm.object().financeCenterID, vm.object().financeCenterFullName);
                self.initSearcherControl('.initiator', 'UtilizerSearcher', vm.object().initiatorID, vm.object().initiatorFullName, vm.object().initiatorClassID);
            };
            //
            self.costClick = function () {
                var obj = vm.object();
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowCostNDS(obj.cost, obj.ndsTypeID, obj.ndsPercentID, obj.ndsCustomValue, obj.timePeriodID, ko.observable(true));
                });
            };
            //
            //
            self.initNumericUpDownControl = function (selector, ko_value, minValue, maxValue) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmContractRegistration');
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
                        }
                    });
                    hideSpinner($div[0]);
                });
            };
            //
            self.initDateControl = function (selector, ko_value, ko_valueString) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmContractRegistration');
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
            //
            self.initSearcherControl = function (selector, searcherName, ko_id, ko_fullName, ko_classID) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmContractRegistration');
                var searcherControlD = $.Deferred();
                //
                var fh = new fhModule.formHelper();
                var searcherLoadD = fh.SetTextSearcherToField(
                    $frm.find(selector),
                    searcherName,
                    null,
                    [],
                    function (objectInfo) {//select
                        ko_fullName(objectInfo.FullName);
                        ko_id(objectInfo.ID);
                        if (ko_classID) ko_classID(objectInfo.ClassID);
                    },
                    function () {//reset
                        ko_fullName('');
                        ko_id(null);
                        if (ko_classID) ko_classID(null);
                    },
                    function (selectedItem) {//close
                        if (!selectedItem) {
                            ko_fullName('');
                            ko_id(null);
                            if (ko_classID) ko_classID(0);
                        }
                    });
                $.when(searcherLoadD, userD).done(function (ctrl, user) {
                    searcherControlD.resolve(ctrl);
                    ctrl.CurrentUserID = user.ID;
                    self.searcher_controls.push(ctrl);
                });
            };
            //
            self.showSupplierList = function () {
                showSpinner();
                //
                var editSupplier = function (newSupplier) {
                    var obj = vm.object();
                    obj.supplierID(newSupplier.ID);
                    obj.supplierName(newSupplier.Name);
                };
                //
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSupplierList(editSupplier);
                });
            };
        }
    };
    return module;
});