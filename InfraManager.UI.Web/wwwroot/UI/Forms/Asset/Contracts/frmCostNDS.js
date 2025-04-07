define(['knockout', 'jquery', 'ajax', 'formControl', 'models/FinanceForms/ActivesRequestSpecificationForm'], function (ko, $, ajax, formControl, specForm) {
    var module = {
        MaxPrice: 1000000000,

        ViewModel: function () {
            var self = this;
            //            
            self.timePeriodValues = [
                { ID: 0, Name: getTextResource('IntervalEnum_Once') },
                { ID: 1, Name: getTextResource('IntervalEnum_Monthly') },
                { ID: 2, Name: getTextResource('IntervalEnum_Quarterly') },
                { ID: 3, Name: getTextResource('IntervalEnum_Annually') },
            ];
            self.timePeriod = ko.observable(self.timePeriodValues[0]);
            self.timePeriodID = ko.pureComputed({
                read: function () {
                    return self.timePeriod() ? self.timePeriod().ID : null;
                },
                write: function (value) {
                    if (value != null)
                        self.timePeriod(self.timePeriodValues[value]);
                    else
                        self.timePeriod(null);
                }
            });
            self.timePeriodDataSource = function (options) {
                var data = self.timePeriodValues;
                options.callback({ data: data, total: data.length });
            };
            //
            self.ndsTypeValues = [
                { ID: 0, Name: getTextResource('NDSType_AlreadyIncluded') },
                { ID: 1, Name: getTextResource('NDSType_NotNeeded') },
                { ID: 2, Name: getTextResource('NDSType_AddToPrice') },
            ]
            self.ndsType = ko.observable(self.ndsTypeValues[0]);
            self.ndsType_handle = self.ndsType.subscribe(function () { self.calculate(); });
            self.ndsTypeDownload = ko.observable(self.ndsType());
            self.ndsTypeDownload_handle = self.ndsTypeDownload.subscribe(function () {
                self.calculate();
            });
            self.ndsTypeID = ko.pureComputed({
                read: function () {
                    self.ndsTypeDownload(self.ndsType());
                    return self.ndsType() ? self.ndsType().ID : null;
                },
                write: function (value) {
                    self.ndsType(self.ndsTypeValues[value]);
                }
            });
            self.ndsTypeDataSource = function (options) {
                var data = self.ndsTypeValues;
                options.callback({ data: data, total: data.length });
            };
            //
            self.ndsPercentValues = [
                { ID: 0, Name: getTextResource('NDSPercent_Custom') },
                { ID: 1, Name: getTextResource('NDSPercent_Seven') + '%' },
                { ID: 2, Name: getTextResource('NDSPercent_Ten') + '%' },
                { ID: 3, Name: getTextResource('NDSPercent_Eighteen') + '%' },
                { ID: 4, Name: getTextResource('NDSPercent_Twenty') + '%' },
            ]
            self.ndsPercent = ko.observable(self.ndsPercentValues[4]);
            self.ndsPercent_handle = self.ndsPercent.subscribe(function () { self.calculate(); });
            self.ndsPercentID = ko.pureComputed({
                read: function () {
                    return self.ndsPercent() ? self.ndsPercent().ID : null;
                },
                write: function (value) {
                    self.ndsPercent(self.ndsPercentValues[value]);
                }
            });
            self.ndsPercentDataSource = function (options) {
                var data = self.ndsPercentValues;
                options.callback({ data: data, total: data.length });
            };
            //
            self.ndsCustomValue = ko.observable(0);
            self.ndsCustomValue_handle = self.ndsCustomValue.subscribe(function () { self.calculate();});
            //
            self.cost = ko.observable(0);
            self.cost_handle = self.cost.subscribe(function () { self.calculate(); });
            //
            //
            self.costWithoutNDS = ko.observable(0);
            self.costWithNDS = ko.observable(0);
            self.ndsSum = ko.observable(0);
            //
            self.getDecimal = function (val) {
                if (val == null || val == undefined || val == '')
                    return parseFloat(0);
                val = val.toString().replace(',', '.').split(' ').join('');
                val = parseFloat(val);
                if (isNaN(val) || !isFinite(val))
                    return parseFloat(0);
                return val;
            };
            self.calculate = function () {
                var ndsSelectedType = self.ndsTypeDownload().ID;
                var tmp = specForm.CalculatePriceWithNDS(self.getDecimal(self.cost()), 1, ndsSelectedType, self.ndsPercentID(), self.getDecimal(self.ndsCustomValue()));//price, count, type, percent, customValue
                //return object: CostWithoutNDS, CostWithNDS, SumNDS, PriceWithoutNDS, PriceWithNDS
                self.costWithoutNDS(getFormattedMoneyString(tmp.CostWithoutNDS ? tmp.CostWithoutNDS.toString() : '0') + ' ' + getTextResource('CurrentCurrency'));
                self.costWithNDS(getFormattedMoneyString(tmp.CostWithNDS ? tmp.CostWithNDS.toString() : '0') + ' ' + getTextResource('CurrentCurrency'));
                self.ndsSum(getFormattedMoneyString(tmp.SumNDS ? tmp.SumNDS.toString() : '0') + ' ' + getTextResource('CurrentCurrency'));
            };
            //
            //
            self.load = function (ko_cost, ko_ndsTypeID, ko_ndsPercentID, ko_ndsCustomValue, ko_timePeriodID) {
                self.cost(getFormattedMoneyString(ko_cost() ? ko_cost().toString() : '0'));
                self.ndsTypeID(ko_ndsTypeID());
                self.ndsPercentID(ko_ndsPercentID());
                self.ndsCustomValue(getFormattedMoneyString(ko_ndsCustomValue() ? ko_ndsCustomValue().toString() : '0'));
                self.timePeriodID(ko_timePeriodID ? ko_timePeriodID() : null);
            };
            //
            self.save = function (ko_cost, ko_ndsTypeID, ko_ndsPercentID, ko_ndsCustomValue, ko_timePeriodID) {
                if (self.cost() == null || self.getDecimal(self.cost()) < 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('CostNDS_CostPrompt'));
                    });
                    return false;
                }
                if (self.ndsTypeID() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('CostNDS_NDSTypePrompt'));
                    });
                    return false;
                }
                if (self.ndsPercentID() == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('CostNDS_NDSPercentPrompt'));
                    });
                    return false;
                }
                if (self.ndsPercentID() == 0) {
                    if (self.ndsCustomValue() == null || self.getDecimal(self.ndsCustomValue()) < 0 || self.getDecimal(self.ndsCustomValue()) > self.getDecimal(self.cost())) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('CostNDS_NDSCusomValueMoreThanCost'));
                        });
                        return false;
                    }
                }
                //
                ko_cost(self.getDecimal(self.cost()));
                ko_ndsTypeID(self.ndsTypeID());
                ko_ndsPercentID(self.ndsPercentID());
                ko_ndsCustomValue(self.getDecimal(self.ndsCustomValue()));
                if (ko_timePeriodID) ko_timePeriodID(self.timePeriodID());
                //
                return true;
            };
            //
            self.dispose = function () {
                self.timePeriodID.dispose();
                self.ndsTypeID.dispose();
                self.ndsPercentID.dispose();
                //
                self.ndsTypeDownload_handle.dispose();
                self.ndsType_handle.dispose();
                self.ndsPercent_handle.dispose();
                self.ndsCustomValue_handle.dispose();
                self.cost_handle.dispose();
            };
            //
            self.afterRender = function (editor, elements) {
                self.initNumericUpDownControl('.cost', self.cost, 0, module.MaxPrice);
                self.initNumericUpDownControl('.ndsCustomValue', self.ndsCustomValue, 0, module.MaxPrice);
            };
            //
            self.initNumericUpDownControl = function (selector, ko_value, minValue, maxValue) {
                var $frm = $('#' + self.frm.GetRegionID()).find('.frmCostNDS');
                var $div = $frm.find(selector);
                showSpinner($div[0]);
                require(['jqueryStepper'], function () {
                    $div.stepper({
                        type: 'float',
                        floatPrecission: 2,
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
        },

        ShowDialog: function (ko_cost, ko_ndsTypeID, ko_ndsPercentID, ko_ndsCustomValue, ko_timePeriodID, ko_canEdit, saveFunc, isSpinnerActive) {
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
                    var initial_cost = ko_cost();
                    var initial_ndsTypeID = ko_ndsTypeID();
                    var initial_ndsPercent = ko_ndsPercentID();
                    var initial_ndsCustomValue = ko_ndsCustomValue();
                    var initial_timePeriodID = ko_timePeriodID && ko_timePeriodID() ? ko_timePeriodID() : null;
                    //
                    if (vm.save(ko_cost, ko_ndsTypeID, ko_ndsPercentID, ko_ndsCustomValue, ko_timePeriodID)) {
                        if (saveFunc) {
                            var tmp = saveFunc();
                            if (tmp && tmp.resolve) {
                                $.when(tmp).done(function (val) {
                                    if (val == false) {
                                        ko_cost(initial_cost);
                                        ko_ndsTypeID(initial_ndsTypeID);
                                        ko_ndsPercentID(initial_ndsPercent);
                                        ko_ndsCustomValue(initial_ndsCustomValue);
                                        ko_timePeriodID(initial_timePeriodID);
                                    }
                                    else
                                        frm.Close();
                                });
                                return;
                            }
                        }
                        frm.Close();
                    }
                }
            }
            var bCancel = {
                text: getTextResource('Close'),
                click: function () { frm.Close(); }
            }
            if (ko_canEdit())
                buttons.push(bSave);
            buttons.push(bCancel);
            //
            frm = new formControl.control(
                'region_frmCostNDS',//form region prefix
                'setting_frmCostNDS',//location and size setting
                getTextResource('CostNDS'),//caption
                true,//isModal
                true,//isDraggable
                true,//isResizable
                420, 335,//minSize
                buttons,//form buttons
                function () {
                    ko.cleanNode(bindElement);
                    vm.dispose();
                },//afterClose function
                'data-bind="template: {name: \'../UI/Forms/Asset/Contracts/frmCostNDS\', afterRender: afterRender}"'//attributes of form region
            );
            if (!frm.Initialized)
                return;//form with that region and settingsName was open
            frm.ExtendSize(500, 360);//normal size
            vm.frm = frm;
            vm.load(ko_cost, ko_ndsTypeID, ko_ndsPercentID, ko_ndsCustomValue, ko_timePeriodID);
            //
            bindElement = document.getElementById(frm.GetRegionID());
            ko.applyBindings(vm, bindElement);
            //
            $.when(frm.Show()).done(function (frmD) {
                hideSpinner();
            });
        },

    };
    return module;
});