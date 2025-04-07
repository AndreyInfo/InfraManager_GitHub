define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'decimal', 'models/FinanceForms/ActivesRequestSpecification', 'dateTimeControl', 'jqueryStepper'], function (ko, $, ajax, formControl, fhModule, decimal, specLib, dtLib) {
    var module = {
        MaxCount: 1000000,
        MaxPrice: 1000000000,
        MaxTotalSum: 1000000000000000,
        ViewModel: function () {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //
            self.ClassID = 180;
            self.ID = null;
            self.CanEdit = ko.observable(true);
            //
            self.FinanceBudgetID = null;
            self.IsFinanceBudgetApproved = ko.observable(false);
            //
            self.getDecimal = function (val) {
                if (val == null || val == undefined || val == '')
                    return new decimal(0);
                val = self.getFloatValue(val);
                if (isNaN(val) || !isFinite(val))
                    return new decimal(0);
                return new decimal(val.toString());
            };
            self.getDecimalString = function (val) {
                if (!val || isNaN(parseFloat(val)) == true)
                    return val;
                //
                return getMoneyString(self.getFloatValue(val));
            };
            self.getFloatValue = function (val) {
                if (val)
                    return parseFloat(val.toString().replace(',', '.').split(' ').join(''));
                else
                    return 0;
            };
            //
            self.Identifier = ko.observable('');
            self.Name = ko.observable('');
            self.InitiatorID = ko.observable(null);
            self.InitiatorFullName = ko.observable('');
            self.FinanceCenterID = ko.observable(null);
            self.FinanceCenterClassID = ko.observable(0);
            self.FinanceCenterFullName = ko.observable('');
            self.ReasonObjectID = ko.observable(null);
            self.ReasonObjectClassID = ko.observable(0);
            self.ReasonObjectFullName = ko.observable('');
            self.BudgetID = ko.observable(null);
            self.BudgetFullName = ko.observable('');
            self.ProductID = ko.observable(null);
            self.ProductClassID = ko.observable(null);
            self.ProductFullName = ko.observable('');
            self.TotalSum = ko.observable(0);
            self.TotalSum.subscribe(function (newValue) {
                if (newValue > module.MaxTotalSum && self.ProductID() == null)
                    self.TotalSum(self.getDecimalString(module.MaxTotalSum));
            });
            self.Price = ko.observable(null);
            self.Price.subscribe(function (newValue) {
                if (newValue > module.MaxPrice)
                    self.Price(self.getDecimalString(module.MaxPrice));
                if (self.ProductID() != null)
                    self.TotalSum(self.getDecimalString(normalize(self.getDecimal(self.Price()).times(self.getDecimal(self.Count())))));
            });
            self.Count = ko.observable(null);
            self.Count.subscribe(function (newValue) {
                if (newValue > module.MaxCount)
                    self.Count(module.MaxCount);
                if (self.ProductID() != null)
                    self.TotalSum(self.getDecimalString(normalize(self.getDecimal(self.Price()).times(self.getDecimal(self.Count())))));
            });
            self.Rate1 = ko.observable(25);
            self.Rate1.subscribe(function (newValue) {
                if (newValue > module.MaxCount)
                    self.Rate1(module.MaxCount);
            });
            self.Rate2 = ko.observable(25);
            self.Rate2.subscribe(function (newValue) {
                if (newValue > module.MaxCount)
                    self.Rate2(module.MaxCount);
            });
            self.Rate3 = ko.observable(25);
            self.Rate3.subscribe(function (newValue) {
                if (newValue > module.MaxCount)
                    self.Rate3(module.MaxCount);
            });
            self.Rate4 = ko.observable(25);
            self.Rate4.subscribe(function (newValue) {
                if (newValue > module.MaxCount)
                    self.Rate4(module.MaxCount);
            });
            self.Sum1 = ko.computed(function () {
                if (self.ProductID() == null)
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate1()).times(self.getDecimal(self.TotalSum())).dividedBy(self.getDecimal(100))));
                else
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate1()).times(self.getDecimal(self.Price()))));
            });
            self.Sum2 = ko.computed(function () {
                if (self.ProductID() == null)
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate2()).times(self.getDecimal(self.TotalSum())).dividedBy(self.getDecimal(100))));
                else
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate2()).times(self.getDecimal(self.Price()))));
            });
            self.Sum3 = ko.computed(function () {
                if (self.ProductID() == null)
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate3()).times(self.getDecimal(self.TotalSum())).dividedBy(self.getDecimal(100))));
                else
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate3()).times(self.getDecimal(self.Price()))));
            });
            self.Sum4 = ko.computed(function () {
                if (self.ProductID() == null)
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate4()).times(self.getDecimal(self.TotalSum())).dividedBy(self.getDecimal(100))));
                else
                    return self.getDecimalString(normalize(self.getDecimal(self.Rate4()).times(self.getDecimal(self.Price()))));
            });
            self.Note = ko.observable('');
            self.RowVersion = '';
            //
            //for adjustment
            self.PreviousObject = ko.observable(null);
            self.CurrentObject = function () {
                return {
                    ID: self.ID,
                    Identifier: self.Identifier(),
                    Name: self.Name(),
                    TotalSum: self.TotalSum(),
                    Sum1: self.Sum1(),
                    Sum2: self.Sum2(),
                    Sum3: self.Sum3(),
                    Sum4: self.Sum4()
                };
            };
            //
            self.SetProduct = function (objectInfo) {
                if (objectInfo == null && self.ProductID() == null ||
                    objectInfo != null && self.ProductID() != null && objectInfo.ID == self.ProductID())
                    return;//non modified
                //
                if (objectInfo == null) {
                    self.TotalSum(self.getDecimalString(normalize(self.getDecimal(self.Count()).times(self.getDecimal(self.Price())))));
                    self.ProductID(null);//hack subscribers
                    self.Count(null);
                    self.Price(null);
                    self.Rate1(25);
                    self.Rate2(25);
                    self.Rate3(25);
                    self.Rate4(25);
                }
                else {
                    self.Count(self.ARSList_TotalCount() == 0 ? 1 : self.ARSList_TotalCount());
                    self.Price(self.getDecimalString(normalize(self.getDecimal(self.TotalSum()).dividedBy(self.getDecimal(self.Count())))));
                    var tmp = parseInt(self.Count() / 4);
                    self.Rate1(tmp);
                    self.Rate2(tmp);
                    self.Rate3(tmp);
                    self.Rate4(self.Count() - 3 * tmp);
                }
                //
                self.ProductFullName(objectInfo == null ? '' : objectInfo.FullName);
                self.ProductClassID(objectInfo == null ? null : objectInfo.ClassID);
                self.ProductID(objectInfo == null ? null : objectInfo.ID);
            };
            //
            //ARSLIST_BLOCK - copied from purchaseSpecificationForm
            {
                self.ARSList = ko.observableArray([]);
                self.ARSListExpanded = ko.observable(true);
                self.ExpandCollapseARSList = function () {
                    self.ARSListExpanded(!self.ARSListExpanded());
                };
                self.ARSList_TotalSumNDS = ko.computed(function () {
                    if (!self.ARSList || !self.ARSList() || self.ARSList().length == 0)
                        return 0;
                    //
                    var retval = 0.0;
                    ko.utils.arrayForEach(self.ARSList(), function (el) {
                        retval += el.SumNDS();
                    });
                    retval = specLib.Normalize(retval);
                    //
                    return retval;
                });
                self.ARSList_TotalSumNDSString = ko.computed(function () {
                    return specLib.ToMoneyString(self.ARSList_TotalSumNDS());
                });
                self.ARSList_TotalCostWithNDS = ko.computed(function () {
                    if (!self.ARSList || !self.ARSList() || self.ARSList().length == 0)
                        return 0;
                    //
                    var retval = 0.0;
                    ko.utils.arrayForEach(self.ARSList(), function (el) {
                        retval += el.CostWithNDS();
                    });
                    retval = specLib.Normalize(retval);
                    //
                    return retval;
                });
                self.ARSList_TotalCostWithNDSString = ko.computed(function () {
                    return specLib.ToMoneyString(self.ARSList_TotalCostWithNDS());
                });
                self.ARSList_TotalCostWithoutNDS = ko.computed(function () {
                    if (!self.ARSList || !self.ARSList() || self.ARSList().length == 0)
                        return 0;
                    //
                    var retval = 0.0;
                    ko.utils.arrayForEach(self.ARSList(), function (el) {
                        retval += el.CostWithoutNDS();
                    });
                    retval = specLib.Normalize(retval);
                    //
                    return retval;
                });
                self.ARSList_TotalCostWithoutNDSString = ko.computed(function () {
                    return specLib.ToMoneyString(self.ARSList_TotalCostWithoutNDS());
                });
                //
                self.ARSList_TotalCount = function () {
                    if (!self.ARSList || !self.ARSList() || self.ARSList().length == 0)
                        return 0;
                    //
                    var retval = 0;
                    ko.utils.arrayForEach(self.ARSList(), function (el) {
                        retval += parseInt(el.DependencyCount());
                    });
                    //
                    return retval;
                };
                //
                self.ARSList_SortTable = function () {
                    if (!self.ARSList)
                        return;

                    self.ARSList.sort(
                            function (left, right) {
                                if (left.OrderNumber() == null)
                                    return -1;
                                //
                                if (right.OrderNumber() == null)
                                    return 1;
                                //
                                return left.OrderNumber() == right.OrderNumber() ? 0 : (left.OrderNumber() < right.OrderNumber() ? -1 : 1);
                            }
                        );
                };
                //
                //
                self.ajaxControl_dependency = new ajax.control();
                self.LoadDependencyList = function () {
                    var retD = $.Deferred();
                    //
                    if (!self.ID) {
                        retD.resolve();
                        return retD;
                    }
                    //
                    var param = {
                        FinanceBudgetRowID: self.ID
                    };
                    var $region = $('#' + self.frm.GetRegionID()).find('.spec_form-arsBody');
                    self.ajaxControl_dependency.Ajax($region,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/finApi/GetFinanceBudgetRowDependencyList?' + $.param(param)
                    },
                    function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            if (newVal.ARSList) {
                                ko.utils.arrayForEach(newVal.ARSList, function (item) {
                                    var specRow = new module.DependencyARSObject(item, self);
                                    //
                                    self.ARSList.push(specRow);
                                });
                                //
                                self.ARSList_SortTable();
                                self.ARSList.valueHasMutated();
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRow.js, LoadDependencyList]', 'error');
                                });
                            }
                        }
                        else if (newVal && newVal.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmFinanceBudgetRow.js, LoadDependencyList]', 'error');
                            });
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmFinanceBudgetRow.js, LoadDependencyList]', 'error');
                            });
                        retD.resolve();
                    },
                    null,
                    function () {
                        retD.resolve();
                    });
                    //
                    return retD;
                };
                //
                self.AddDependencyClick = function (specArray) {
                    showSpinner();
                    require(['financeForms'], function (fh) {
                        var fh = new fh.formHelper(true);
                        fh.ShowActivesRequestSpecificationSearch({
                            ClassID: self.ClassID,
                            ID: self.ID,
                            PurchaseMode: false,
                            PurchaseLink: false,
                            ProductCatalogClassID: null,//self.ProductClassID(), фильтрацию сказано отключить
                            ProductCatalogID: null,//self.ProductID()
                        }, function (newValues, createSingle, defaultModelID, defaultModelClassID) {
                            if (!newValues || newValues.length == 0)
                                return;
                            //
                            var items = [];
                            ko.utils.arrayForEach(newValues, function (el) {
                                items.push({
                                    ActiveRequstSpecificationID: el.ID,
                                    Count: el.Count
                                });
                            });
                            //
                            var data = {
                                'FinanceBudgetRowID': self.ID,
                                'List': items
                            };
                            //
                            var $region = $('#' + self.frm.GetRegionID()).find('.spec_form-arsBody');
                            self.ajaxControl_dependency.Ajax($region,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/finApi/GetFinanceBudgetRowDependencyItem'
                                },
                                function (model) {
                                    if (model.Result === 0) {
                                        var list = model.ARSList;
                                        if (list) {
                                            ko.utils.arrayForEach(list, function (item) {
                                                var exist = ko.utils.arrayFirst(self.ARSList(), function (exItem) {
                                                    return exItem.ID == item.ID;
                                                });
                                                if (exist)
                                                    exist.DependencyCount(exist.DependencyCount() + item.DependencyCount);
                                                else {
                                                    var specRow = new module.DependencyARSObject(item, self);
                                                    self.ARSList.push(specRow);
                                                }
                                            });
                                            //
                                            self.ARSList_SortTable();
                                            self.ARSList.valueHasMutated();
                                        }
                                    }
                                    else {
                                        if (model.Result === 1) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[frmFinanceBudgetRow.js, AddDependencyClick]', 'error');
                                            });
                                        }
                                        else if (model.Result === 2) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[frmFinanceBudgetRow.js, AddDependencyClick]', 'error');
                                            });
                                        }
                                        else if (model.Result === 3) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                                            });
                                        }
                                        else if (model.Result === 8) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('ValidationError'), 'error');
                                            });
                                        }
                                        else {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[frmFinanceBudgetRow.js, AddDependencyClick]', 'error');
                                            });
                                        }
                                    }
                                });
                        });
                    });
                };
            }
            // 
            //
            //PurchaseSpecificationList
            {
                self.PurchaseSpecificationList = ko.observableArray([]);
                self.PurchaseSpecificationListExpanded = ko.observable(true);
                self.ExpandCollapsePurchaseSpecificationList = function () {
                    self.PurchaseSpecificationListExpanded(!self.PurchaseSpecificationListExpanded());
                };
                self.PurchaseSpecificationList_TotalSum = ko.computed(function () {
                    if (self.PurchaseSpecificationList().length == 0)
                        return 0;
                    //
                    var retval = 0.0;
                    ko.utils.arrayForEach(self.PurchaseSpecificationList(), function (el) {
                        if (el.TotalSum)
                            retval += el.TotalSum;
                    });
                    retval = specLib.Normalize(retval);
                    //
                    return retval;
                });
                self.PurchaseSpecificationList_TotalSumString = ko.computed(function () {
                    return specLib.ToMoneyString(self.PurchaseSpecificationList_TotalSum());
                });
                self.PurchaseSpecificationList_Sum = ko.computed(function () {
                    if (self.PurchaseSpecificationList().length == 0)
                        return 0;
                    //
                    var retval = 0.0;
                    ko.utils.arrayForEach(self.PurchaseSpecificationList(), function (el) {
                        retval += el.Sum;
                    });
                    retval = specLib.Normalize(retval);
                    //
                    return retval;
                });
                self.PurchaseSpecificationList_SumString = ko.computed(function () {
                    return specLib.ToMoneyString(self.PurchaseSpecificationList_Sum());
                });
                //
                self.PurchaseSpecificationList_SortTable = function () {
                    self.PurchaseSpecificationList.sort(
                            function (left, right) {
                                if (left.PurchaseSpecificationOrderNumber() == null)
                                    return -1;
                                //
                                if (right.PurchaseSpecificationOrderNumber() == null)
                                    return 1;
                                //
                                return left.PurchaseSpecificationOrderNumber() == right.PurchaseSpecificationOrderNumber() ? 0 : (left.PurchaseSpecificationOrderNumber() < right.PurchaseSpecificationOrderNumber() ? -1 : 1);
                            }
                        );
                };
                //
                //
                self.ajaxControl_dependency = new ajax.control();
                self.LoadPurchaseSpecificationDependencyList = function () {
                    var retD = $.Deferred();
                    //
                    if (!self.ID) {
                        retD.resolve();
                        return retD;
                    }
                    //
                    var param = {
                        FinanceBudgetRowID: self.ID
                    };
                    var $region = $('#' + self.frm.GetRegionID()).find('.frmFinanceBudgetRow-purchaseSpecificationListBody');
                    self.ajaxControl_dependency.Ajax($region,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/finApi/GetFinanceBudgetRowPurchaseSpecificationDependencyList?' + $.param(param)
                    },
                    function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            if (newVal.List) {
                                ko.utils.arrayForEach(newVal.List, function (item) {
                                    var ps = new module.DependencyPurchaseSpecification(item);
                                    self.PurchaseSpecificationList().push(ps);
                                });
                                //
                                self.PurchaseSpecificationList_SortTable();
                                self.PurchaseSpecificationList.valueHasMutated();
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRow.js, LoadPurchaseSpecificationDependencyList]', 'error');
                                });
                            }
                        }
                        else if (newVal && newVal.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmFinanceBudgetRow.js, LoadPurchaseSpecificationDependencyList]', 'error');
                            });
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmFinanceBudgetRow.js, LoadPurchaseSpecificationDependencyList]', 'error');
                            });
                        retD.resolve();
                    },
                    null,
                    function () {
                        retD.resolve();
                    });
                    //
                    return retD;
                };
            }
            //
            self.CopySumFromDependencyListClick = function () {
                if (self.ProductID() == null)
                    self.TotalSum(self.getDecimalString(normalize(self.ARSList_TotalCostWithNDS())));
                else
                    self.Price(self.getDecimalString(normalize(self.getDecimal(self.ARSList_TotalCostWithNDS()).dividedBy(self.getDecimal(self.Count())))));
            };
            //
            self.initiatorSearcherD = $.Deferred();
            self.financeCenterSearcherD = $.Deferred();
            self.reasonSearcherD = $.Deferred();
            self.budgetStateSearcherD = $.Deferred();
            self.productSearcherD = $.Deferred();
            //
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
                //                
                {//initiator
                    self.initiatorSearcher = null;                    
                    {
                        var fh = new fhModule.formHelper();
                        var initiatorD = fh.SetTextSearcherToField(
                            $frm.find('.initiator'),
                            'WebUserSearcher',
                            null,
                            [],
                            function (objectInfo) {//select
                                self.InitiatorFullName(objectInfo.FullName);
                                self.InitiatorID(objectInfo.ID);
                            },
                            function () {//reset
                                self.InitiatorFullName('');
                                self.InitiatorID(null);
                            },
                            function (selectedItem) {//close
                                if (!selectedItem) {
                                    self.InitiatorFullName('');
                                    self.InitiatorID(null);
                                }
                            });
                        $.when(initiatorD).done(function (ctrl) {
                            self.initiatorSearcher = ctrl;
                            self.initiatorSearcherD.resolve(ctrl);
                            ctrl.CurrentUserID = self.CurrentUserID;
                        });
                    };
                }
                //                
                {//financeCenter
                    self.financeCenterSearcher = null;
                    {
                        var fh = new fhModule.formHelper();
                        var financeCenterD = fh.SetTextSearcherToField(
                            $frm.find('.financeCenter'),
                            'FinanceCenterSearcher',
                            null,
                            [],
                            function (objectInfo) {//select
                                self.FinanceCenterFullName(objectInfo.FullName);
                                self.FinanceCenterID(objectInfo.ID);
                                self.FinanceCenterClassID(objectInfo.ClassID);
                            },
                            function () {//reset
                                self.FinanceCenterFullName('');
                                self.FinanceCenterID(null);
                                self.FinanceCenterClassID(0);
                            },
                            function (selectedItem) {//close
                                if (!selectedItem) {
                                    self.FinanceCenterFullName('');
                                    self.FinanceCenterID(null);
                                    self.FinanceCenterClassID(0);
                                }
                            });
                        $.when(financeCenterD).done(function (ctrl) {
                            self.financeCenterSearcher = ctrl;
                            self.financeCenterSearcherD.resolve(ctrl);
                            ctrl.CurrentUserID = self.CurrentUserID;
                        });
                    };
                }
                //                
                {//reason
                    self.reasonSearcher = null;
                    {
                        var fh = new fhModule.formHelper();
                        var reasonD = fh.SetTextSearcherToField(
                            $frm.find('.reason'),
                            'ProjectAndFinanceActionSearcher',
                            null,
                            [],
                            function (objectInfo) {//select
                                self.ReasonObjectFullName(objectInfo.FullName);
                                self.ReasonObjectID(objectInfo.ID);
                                self.ReasonObjectClassID(objectInfo.ClassID);
                            },
                            function () {//reset
                                self.ReasonObjectFullName('');
                                self.ReasonObjectID(null);
                                self.ReasonObjectClassID(0);
                            },
                            function (selectedItem) {//close
                                if (!selectedItem) {
                                    self.ReasonObjectFullName('');
                                    self.ReasonObjectID(null);
                                    self.ReasonObjectClassID(0);
                                }
                            });
                        $.when(reasonD).done(function (ctrl) {
                            self.reasonSearcher = ctrl;
                            self.reasonSearcherD.resolve(ctrl);
                            ctrl.CurrentUserID = self.CurrentUserID;
                        });
                    };
                }
                //                
                {//budgetState
                    self.budgetStateSearcher = null;
                    {
                        var fh = new fhModule.formHelper();
                        var budgetStateD = fh.SetTextSearcherToField(
                            $frm.find('.budgetState'),
                            'BudgetSearcher',
                            null,
                            [],
                            function (objectInfo) {//select
                                self.BudgetFullName(objectInfo.FullName);
                                self.BudgetID(objectInfo.ID);
                            },
                            function () {//reset
                                self.BudgetFullName('');
                                self.BudgetID(null);
                            },
                            function (selectedItem) {//close
                                if (!selectedItem) {
                                    self.BudgetFullName('');
                                    self.BudgetID(null);
                                }
                            });
                        $.when(budgetStateD).done(function (ctrl) {
                            self.budgetStateSearcher = ctrl;
                            self.budgetStateSearcherD.resolve(ctrl);
                            ctrl.CurrentUserID = self.CurrentUserID;
                        });
                    };
                }
                //                
                {//product
                    self.productSearcher = null;
                    {
                        var fh = new fhModule.formHelper();
                        var productD = fh.SetTextSearcherToField(
                            $frm.find('.product'),
                            'ProductCatalogTypeAndModelSearcher',
                            null,
                            [],
                            function (objectInfo) {//select
                                self.SetProduct(objectInfo);
                            },
                            function () {//reset
                                self.SetProduct(null);
                            },
                            function (selectedItem) {//close
                                if (!selectedItem) {
                                    self.SetProduct(null);
                                }
                            });
                        $.when(productD).done(function (ctrl) {
                            self.productSearcher = ctrl;
                            self.productSearcherD.resolve(ctrl);
                            ctrl.CurrentUserID = self.CurrentUserID;
                        });
                    };
                }
                //
                var $totalSum = $frm.find('.totalSum');
                showSpinner($totalSum[0]);
                require(['jqueryStepper'], function () {
                    $totalSum.stepper({
                        type: 'float',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [0, module.MaxTotalSum],
                        onStep: function (val, up) {
                            self.TotalSum(self.getDecimalString(self.getDecimal(val)));
                        }
                    });
                    $totalSum.blur(function () { self.TotalSum(self.getDecimalString(self.TotalSum())); })
                    hideSpinner($totalSum[0]);
                });
                //
                var $price = $frm.find('.price');
                showSpinner($price[0]);
                require(['jqueryStepper'], function () {
                    $price.stepper({
                        type: 'float',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [0, module.MaxPrice],
                        onStep: function (val, up) {
                            self.Price(self.getDecimalString(self.getDecimal(val)));
                        }
                    });
                    $price.blur(function () { self.Price(self.getDecimalString(self.Price())); })
                    hideSpinner($price[0]);
                });
                //
                var $count = $frm.find('.count');
                showSpinner($count[0]);
                require(['jqueryStepper'], function () {
                    $count.stepper({
                        type: 'int',
                        floatPrecission: 0,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [0, module.MaxCount],
                        onStep: function (val, up) {
                            self.Count(self.getDecimal(val));
                        }
                    });
                    hideSpinner($count[0]);
                });
                //
                var $rate1 = $frm.find('.rate1');
                showSpinner($rate1[0]);
                require(['jqueryStepper'], function () {
                    $rate1.stepper({
                        type: 'float',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [0, 1000000000],
                        onStep: function (val, up) {
                            self.Rate1(self.getDecimal(val));
                        }
                    });
                    hideSpinner($rate1[0]);
                });
                //
                var $rate2 = $frm.find('.rate2');
                showSpinner($rate2[0]);
                require(['jqueryStepper'], function () {
                    $rate2.stepper({
                        type: 'float',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [0, 1000000000],
                        onStep: function (val, up) {
                            self.Rate2(self.getDecimal(val));
                        }
                    });
                    hideSpinner($rate2[0]);
                });
                //
                var $rate3 = $frm.find('.rate3');
                showSpinner($rate3[0]);
                require(['jqueryStepper'], function () {
                    $rate3.stepper({
                        type: 'float',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [0, 1000000000],
                        onStep: function (val, up) {
                            self.Rate3(self.getDecimal(val));
                        }
                    });
                    hideSpinner($rate3[0]);
                });
                //
                var $rate4 = $frm.find('.rate4');
                showSpinner($rate4[0]);
                require(['jqueryStepper'], function () {
                    $rate4.stepper({
                        type: 'float',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [0, 1000000000],
                        onStep: function (val, up) {
                            self.Rate4(self.getDecimal(val));
                        }
                    });
                    hideSpinner($rate4[0]);
                });
            };
            //
            self.Load = function (id) {
                var retval = $.Deferred();
                self.ID = id;
                //
                if (!id) {
                    $.when(userD).done(function (user) {
                        self.InitiatorID(user.ID);
                        self.InitiatorFullName(user.FullName);
                    });
                    //
                    retval.resolve(true);
                    return retval;
                }
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/finApi/GetFinanceBudgetRow',
                    method: 'GET',
                    data: { FinanceBudgetRowID: self.ID }
                },
                function (bugetResult) {
                    if (bugetResult.Result === 0 && bugetResult.Data) {
                        var obj = bugetResult.Data;
                        //
                        self.PreviousObject(obj);
                        self.ID = obj.ID;
                        //
                        self.FinanceBudgetID = obj.FinanceBudgetID;
                        self.Identifier(obj.Identifier);
                        self.Name(obj.Name);
                        //
                        self.InitiatorID(obj.InitiatorID);
                        self.InitiatorFullName(obj.InitiatorFullName);
                        $.when(self.initiatorSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(self.InitiatorID(), 9, self.InitiatorFullName(), '');
                        });
                        //
                        self.FinanceCenterID(obj.FinanceCenterID);
                        self.FinanceCenterClassID(obj.FinanceCenterClassID);
                        self.FinanceCenterFullName(obj.FinanceCenterFullName);
                        $.when(self.financeCenterSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(self.FinanceCenterID(), self.FinanceCenterClassID(), self.FinanceCenterFullName(), '');
                        });
                        //
                        self.ReasonObjectID(obj.ReasonObjectID);
                        self.ReasonObjectClassID(obj.ReasonObjectClassID);
                        self.ReasonObjectFullName(obj.ReasonObjectFullName);
                        $.when(self.reasonSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(self.ReasonObjectID(), self.ReasonObjectClassID(), self.ReasonObjectFullName(), '');
                        });
                        //
                        self.BudgetID(obj.BudgetID);
                        self.BudgetFullName(obj.BudgetFullName);
                        $.when(self.budgetStateSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(self.BudgetID(), 143, self.BudgetFullName(), '');
                        });
                        //
                        self.ProductID(obj.ProductID);
                        self.ProductClassID(obj.ProductClassID);
                        self.ProductFullName(obj.ProductFullName);
                        $.when(self.productSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(self.ProductID(), self.ProductClassID(), self.ProductFullName(), '');
                        });
                        //
                        self.Price(self.getDecimalString(obj.Price));
                        self.Count(obj.Count);
                        self.TotalSum(self.getDecimalString(obj.TotalSum));
                        self.Rate1(obj.Rate1);
                        self.Rate2(obj.Rate2);
                        self.Rate3(obj.Rate3);
                        self.Rate4(obj.Rate4);
                        self.Note(obj.Note);
                        self.RowVersion = obj.RowVersion;
                        //
                        self.CanEdit(obj.FinanceBudgetState != 2? true : false);//not approved
                        self.IsFinanceBudgetApproved(obj.FinanceBudgetState == 1 ? true : false);//approved
                        //
                        $.when(self.LoadDependencyList()).done(function () {;
                            self.LoadPurchaseSpecificationDependencyList();
                        });
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRowForm.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
            //
            self.Save = function () {
                var retval = $.Deferred();
                //     
                $.when(userD).done(function (user) {
                    if (self.ID == null)
                        self.FinanceBudgetID = user.FinanceBudgetID;
                    //           
                    var dependencyList = [];
                    ko.utils.arrayForEach(self.ARSList(), function (el) {
                        dependencyList.push({
                            ActiveRequstSpecificationID: el.ID,
                            Count: el.DependencyCount()
                        });
                    });
                    //
                    var adjustmentContextD = $.Deferred();
                    if (self.IsFinanceBudgetApproved() == true) {
                        showSpinner();
                        require(['financeForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var oldRow = self.PreviousObject();
                            var newRow = self.CurrentObject();
                            $.when(fh.ShowFinanceBudgetRowAdjustment(null, oldRow, newRow)).done(function (context) {
                                adjustmentContextD.resolve(context);
                            });
                        });
                    }
                    else
                        adjustmentContextD.resolve(undefined);
                    //
                    $.when(adjustmentContextD).done(function (adjustmentContext) {
                        if (self.IsFinanceBudgetApproved() == true && adjustmentContext == undefined) {
                            retval.resolve(null);
                            return;
                        }
                        //
                        var data = {
                            'ID': self.ID,
                            'Identifier': self.Identifier(),
                            'Name': self.Name(),
                            'Note': self.Note(),
                            'FinanceBudgetID': self.FinanceBudgetID,
                            'InitiatorID': self.InitiatorID(),
                            'InitiatorFullName': self.InitiatorFullName(),
                            'FinanceCenterID': self.FinanceCenterID(),
                            'FinanceCenterClassID': self.FinanceCenterClassID(),
                            'FinanceCenterFullName': self.FinanceCenterFullName(),
                            'ReasonObjectID': self.ReasonObjectID(),
                            'ReasonObjectClassID': self.ReasonObjectClassID(),
                            'ReasonObjectFullName': self.ReasonObjectFullName(),
                            'BudgetID': self.BudgetID(),
                            'BudgetFullName': self.BudgetFullName(),
                            'ProductID': self.ProductID(),
                            'ProductClassID': self.ProductClassID(),
                            'ProductFullName': self.ProductFullName(),
                            'Price': self.ProductID() == null ? null : self.getFloatValue(self.Price()),
                            'Count': self.ProductID() == null ? null : self.getFloatValue(self.Count()),
                            'TotalSum': self.getFloatValue(self.TotalSum()),
                            'Sum1': self.getFloatValue(self.Sum1()),
                            'Sum2': self.getFloatValue(self.Sum2()),
                            'Sum3': self.getFloatValue(self.Sum3()),
                            'Sum4': self.getFloatValue(self.Sum4()),
                            'Rate1': self.getFloatValue(self.getDecimal(self.Rate1())),
                            'Rate2': self.getFloatValue(self.getDecimal(self.Rate2())),
                            'Rate3': self.getFloatValue(self.getDecimal(self.Rate3())),
                            'Rate4': self.getFloatValue(self.getDecimal(self.Rate4())),
                            'DependencyList': dependencyList,
                            'AdjustmentContext': adjustmentContext,
                            'RowVersion': self.RowVersion
                        };
                        //    
                        {//validations                            
                            if (data.InitiatorID == null) {
                                require(['sweetAlert'], function (swal) {
                                    swal(getTextResource('ParametersMustBeSet') + ': ' + getTextResource('Initiator'));
                                });
                                retval.resolve(false);
                                return;
                            }
                            if (data.FinanceCenterID == null) {
                                require(['sweetAlert'], function (swal) {
                                    swal(getTextResource('ParametersMustBeSet') + ': ' + getTextResource('FinanceCenter'));
                                });
                                retval.resolve(false);
                                return;
                            }
                            if (data.ReasonObjectID == null) {
                                require(['sweetAlert'], function (swal) {
                                    swal(getTextResource('ParametersMustBeSet') + ': ' + getTextResource('FinanceBudgetRow_Reason'));
                                });
                                retval.resolve(false);
                                return;
                            }
                            if (data.TotalSum < self.ARSList_TotalCostWithNDS()) {
                                require(['sweetAlert'], function (swal) {
                                    swal(getTextResource('FinanceBudgetRow_TotalSumMustBeGrandThanARS'));
                                });
                                retval.resolve(false);
                                return;
                            };
                            var rateSum = data.Rate1 + data.Rate2 + data.Rate3 + data.Rate4;
                            if (data.ProductID == null) {
                                if (rateSum != 100) {
                                    require(['sweetAlert'], function (swal) {
                                        swal(getTextResource('FinanceBudgetRow_TotalSumRatesMustBe100Percent'));
                                    });
                                    retval.resolve(false);
                                    return;
                                }
                            } else {
                                if (rateSum != data.Count) {
                                    require(['sweetAlert'], function (swal) {
                                        swal(getTextResource('FinanceBudgetRow_TotalSumRatesMustBeCount'));
                                    });
                                    retval.resolve(false);
                                    return;
                                }
                                if (data.Count < self.ARSList_TotalCount()) {
                                    require(['sweetAlert'], function (swal) {
                                        swal(getTextResource('FinanceBudgetRow_CountMustBeGrandThanARS'));
                                    });
                                    retval.resolve(false);
                                    return;
                                }
                            }
                        }
                        //
                        var frmElement = $('#' + self.frm.GetRegionID())[0];
                        showSpinner(frmElement);
                        self.ajaxControl.Ajax(null,
                            {
                                url: '/finApi/saveFinanceBudgetRow',
                                method: 'POST',
                                dataType: 'json',
                                data: data
                            },
                            function (response) {
                                hideSpinner(frmElement);
                                if (response) {
                                    if (response.Result == 0) {//ok 
                                        if (response.Message && response.Message.length > 0)
                                            require(['sweetAlert'], function () {
                                                swal({
                                                    title: response.Message,
                                                    showCancelButton: false,
                                                    closeOnConfirm: true,
                                                    cancelButtonText: getTextResource('Continue')
                                                });
                                            });
                                        //
                                        retval.resolve(true);
                                        if (data.AdjustmentContext && data.AdjustmentContext.OldID != null)
                                            $(document).trigger('local_objectDeleted', [self.ClassID, data.AdjustmentContext.OldID, null]);
                                        //
                                        if (data.AdjustmentContext && data.AdjustmentContext.OldID != null && self.ID != null)
                                            $(document).trigger('local_objectInserted', [self.ClassID, response.ID, null]);//OBJ_FinanceBudgetRow                                        
                                        else
                                            $(document).trigger(self.ID == null ? 'local_objectInserted' : 'local_objectUpdated', [self.ClassID, response.ID, null]);//OBJ_FinanceBudgetRow                                        
                                        return;
                                    }
                                    else if (response.Result === 3)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                        });
                                    else if (response.Result === 4)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmFinanceBudgetRow.js, Save]', 'error');
                                        });
                                    else if (response.Result === 5)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('ConcurrencyErrorWithoutQuestion'), 'error');
                                        });
                                    else if (response.Result === 7)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                        });
                                    else if (response.Result === 8)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), response.Message && response.Message.length > 0 ? response.Message : getTextResource('ValidationError'), 'error');
                                        });
                                }
                                retval.resolve(null);
                            },
                            function (response) {
                                hideSpinner(frmElement);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRowForm.js, Save]', 'error');
                                });
                                retval.resolve(null);
                            });
                    });
                });
                //
                return retval.promise();
            };
        },

        ShowDialog: function (id, isSpinnerActive) {
            $.when(operationIsGrantedD(859)).done(function (can_update) {
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
                if (can_update == true || !id)
                    buttons.push(bSave);
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                        'region_frmFinanceBudgetRow',//form region prefix
                        'setting_frmFinanceBudgetRow',//location and size setting
                        getTextResource('FinanceBudgetRow'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        540, 560,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Finances/frmFinanceBudgetRow\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                frm.ExtendSize(600, 700);//normal size
                vm.frm = frm;
                vm.CanEdit(can_update);
                vm.IsFinanceBudgetApproved.subscribe(function (newValue) {
                    if (newValue == true) {
                        bSave.text = getTextResource('FinanceBudgetRowAdjustment');
                        frm.UpdateButtons(buttons);
                    }
                });
                if (!id)
                    $.when(userD).done(function (user) {
                        vm.ajaxControl.Ajax(null,
                            {
                                url: '/finApi/GetFinanceBudget',
                                method: 'GET',
                                data: { FinanceBudgetID: user.FinanceBudgetID }
                            },
                            function (response) {
                                if (response.Data && response.Data.State == 1) //approved
                                    vm.IsFinanceBudgetApproved(true);
                            });
                    });
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(userD).done(function (user) {
                    vm.CurrentUserID = user.ID;
                    $.when(frm.Show(), vm.Load(id)).done(function (frmD, loadD) {
                        hideSpinner();
                    });
                });
            });
        },

        //copied from purchaseSpecificationForm
        DependencyARSObject: function (obj, parent) {
            var self = this;
            var parentSelf = parent;
            //
            self.ID = obj.ID;
            self.ClassID = obj.ClassID;
            self.WorkOrderID = obj.WorkOrderID;
            self.OrderNumber = ko.observable(obj.OrderNumber);
            self.PriceWithoutNDS = ko.observable(specLib.Normalize(obj.PriceWithoutNDS));
            self.PriceWithNDS = ko.observable(specLib.Normalize(obj.PriceWithNDS));
            self.Count = ko.observable(obj.Count);
            self.Price = ko.observable(obj.Price);//не для таблиц-списков, для формы
            self.State = ko.observable(obj.State);
            self.NDSType = ko.observable(obj.NDSType);
            self.InProcurement = ko.observable(parentSelf.IsFinanceBudgetApproved()? obj.State==5 : false);
            self.NDSPercent = ko.observable(obj.NDSPercent);
            self.NDSCustomValue = ko.observable(specLib.Normalize(obj.NDSCustomValue));
            self.Note = ko.observable(obj.Note);
            self.ProductCatalogModelID = obj.ProductCatalogModelID;
            self.ProductCatalogModelClassID = obj.ProductCatalogModelClassID;
            self.ProductCatalogModelFullName = ko.observable(obj.ProductCatalogModelFullName);
            //alex added
            self.ProductCatalogTypeID = obj.ProductCatalogTypeID;
            self.SumNDS = ko.observable(specLib.Normalize(obj.SumNDS));

            self.DependencyCount = ko.observable(obj.DependencyCount);
            //alex replaced
            //self.CostWithNDS = ko.observable(specLib.Normalize(obj.CostWithNDS));
            self.CostWithNDS = ko.computed(function () {
                return normalize(self.PriceWithNDS() == null ? null : new decimal(self.PriceWithNDS()).times(new decimal(self.DependencyCount())));
            });
            //alex replaced
            //self.CostWithoutNDS = ko.observable(specLib.Normalize(obj.CostWithoutNDS));
            self.CostWithoutNDS = ko.computed(function () {
                return normalize(self.PriceWithoutNDS() == null ? null : new decimal(self.PriceWithoutNDS()).times(new decimal(self.DependencyCount())));
            });
            self.UnitID = ko.observable(obj.UnitID);
            self.UnitName = ko.observable(obj.UnitName);
            //
            self.StateString = ko.observable(obj.StateString);
            self.NDSTypeString = ko.observable(obj.NDSTypeString);
            self.NDSPercentString = ko.observable(obj.NDSPercentString);
            //
            self.NDSCustomValueString = ko.computed(function () {
                return specLib.ToMoneyString(self.NDSCustomValue());
            });
            self.PriceWithNDSString = ko.computed(function () {
                return specLib.ToMoneyString(self.PriceWithNDS());
            });
            self.PriceWithoutNDSString = ko.computed(function () {
                return specLib.ToMoneyString(self.PriceWithoutNDS());
            });
            self.SumNDSString = ko.computed(function () {
                return specLib.ToMoneyString(self.SumNDS());
            });
            self.CostWithNDSString = ko.computed(function () {
                return specLib.ToMoneyString(self.CostWithNDS());
            });
            self.CostWithoutNDSString = ko.computed(function () {
                return specLib.ToMoneyString(self.CostWithoutNDS());
            });
            //
            self.NDSInfo = ko.computed(function () {
                if (self.NDSType() === 1) //Не облагается
                    return self.NDSTypeString();
                //
                if (self.NDSPercent() === 0) //Вручную
                    return self.NDSPercentString();
                else return self.NDSPercentString() + '%';
            });
            //
            self.WorkOrderNumber = ko.observable(obj.WorkOrderNumber);
            self.DependencyCountString = ko.observable(self.DependencyCount() + ' / ' + self.Count());
            self.AvailableCount = ko.observable(obj.AvailableCount);
            self.ActiveRequestResponsibleName = ko.observable(obj.ActiveRequestResponsibleName);
            self.ReferenceObjectName = ko.observable(obj.ReferenceObjectName);
            //
            self.Max = self.AvailableCount() > module.MaxCount ? module.MaxCount : self.AvailableCount();
            self.DependencyCount.subscribe(function (newValue) {
                var val = parseInt(newValue);
                if (val <= 0 || isNaN(val))
                    self.DependencyCount(1);
                else if (val > self.Max)
                    self.DependencyCount(self.Max);
                //
                self.updateDepencyCountString();
                //
                //alex removed
                //parentSelf.RecalculateCount();
            });
            //
            self.CountClick = function (obj, e) {
                e.stopPropagation();
            };
            //
            self.ShowForm = function () {
                require(['financeForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var ars = ko.toJS(self);
                    fh.ShowActivesRequestSpecification(ars, ko.observable(false));//parentSelf.CanEdit);
                });
            };
            self.RemoveClick = function () {
                var index = parentSelf.ARSList().indexOf(self);
                if (index > -1)
                    parentSelf.ARSList().splice(index, 1);
                parent.ARSList.valueHasMutated();
            };
            //
            self.updateDepencyCountString = function () {
                self.DependencyCountString(self.DependencyCount() + ' / ' + self.Count());
            };
            //
            self.OnRender = function (htmlNodes, thisObj) {
                var node = ko.utils.arrayFirst(htmlNodes, function (html) {
                    return html.tagName == 'INPUT';
                });
                if (!node || !parentSelf.CanEdit())
                    return;
                //
                var $input = $(node);
                $input.stepper({
                    type: 'int',
                    floatPrecission: 0,
                    wheelStep: 1,
                    arrowStep: 1,
                    limit: [1, self.Max],
                    onStep: function (val, up) {
                        self.DependencyCount(val);
                        //
                        self.updateDepencyCountString();
                    }
                });
            };
        },

        DependencyPurchaseSpecification: function (obj) {
            var self = this;
            //
            self.ID = obj.ID;
            self.WorkOrderID = obj.WorkOrderID;
            self.WorkOrderNumber = obj.WorkOrderNumber;
            self.WorkOrderCause = obj.WorkOrderCause;
            self.ResponsibleName = ko.observable(obj.ResponsibleName);
            self.PurchaseSpecificationOrderNumber = ko.observable(obj.PurchaseSpecificationOrderNumber);
            self.DateDelivered = obj.UtcDateDeliveredJS ? new Date(parseFloat(obj.UtcDateDeliveredJS)) : null;
            self.DateDeliveredString = dtLib.Date2String(self.DateDelivered, true);
            self.TotalSum = specLib.Normalize(obj.TotalSum);
            self.TotalSumString = specLib.ToMoneyString(self.TotalSum);
            self.Sum = specLib.Normalize(obj.Sum);
            self.SumString = specLib.ToMoneyString(self.Sum);
            self.EntityStateName = obj.EntityStateName;
            self.ExecutorFullName = obj.ExecutorFullName;
            self.InitiatorFullName = obj.InitiatorFullName;
            //
            self.ajaxControl = new ajax.control();
            self.ShowForm = function () {
                if (self.ID == null) {
                    require(['sdForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowWorkOrder(self.WorkOrderID, fh.Mode.ReadOnly);
                    });
                    return;
                }
                //
                var data = {
                    'WorkOrderID': self.WorkOrderID,
                    'SpecificationID': self.ID
                };
                self.ajaxControl.Ajax(null,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/finApi/GetPurchaseSpecification'
                    },
                    function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            var newValue = newVal.Elem;
                            //
                            if (newValue) {
                                var specification = new specLib.Specification(self.imList, newValue);
                                require(['financeForms'], function (module) {
                                    var fh = new module.formHelper(true);
                                    var ps = ko.toJS(specification);
                                    fh.ShowPurchaseSpecification(ps, ko.observable(false), null);
                                });
                            }
                        }
                        else if (newVal && newVal.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmFinanceBudgetRow.js, ShowForm]', 'error');
                            });
                        else if (newVal && newVal.Result == 3) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                            });
                        }
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmFinanceBudgetRow.js, ShowForm]', 'error');
                                retD.resolve(false);
                            });
                    });
            };
        }
    };
    return module;
});