define(['knockout', 'jquery', 'ttControl', 'ajax', 'jqueryMouseWheel'], function (ko, $, tclib, ajaxLib) {
    var module = {
        ViewModel: function () {
            var self = this;
            self.ajaxControl = new ajaxLib.control();
            //
            {//current view of list
                self.viewName = ko.observable();
                self.viewName.subscribe(function (newValue) {
                    $.when(userD).done(function (user) {
                        if (user.ViewNameFinance == newValue)
                            return;
                        //            
                        if (self.tableModel)
                            self.tableModel.clearAllInfos();

                        showSpinner($('#regionListMode')[0]);
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                contentType: "application/json",
                                url: '/api/UserSettings',
                                method: 'POST',
                                data: JSON.stringify({ ViewNameFinance: newValue }),
                                dataType: "text"
                            },
                            function () {
                                hideSpinner($('#regionListMode')[0]);
                                //
                                    user.ViewNameFinance = newValue;
                                    //
                                    if (self.tableModel && self.filtersModel)
                                        $.when(self.filtersModel.Load()).done(function () {
                                            self.tableModel.reload();
                                        });                              
                            });
                    });
                });
            }
            //
            {//css of buttons
                self.ActivesRequestClass = ko.computed(function () {
                    return self.viewName() === 'FinanceActivesRequest' ? 'b-content-table__financeActivesRequest_active' : 'b-content-table__financeActivesRequest';
                });
                self.PurchaseClass = ko.computed(function () {
                    return self.viewName() === 'FinancePurchase' ? 'b-content-table__financePurchase_active' : 'b-content-table__financePurchase';
                });
                self.ProjectsClass = ko.computed(function () {
                    return self.viewName() === 'FinanceProjects' ? 'b-content-table__financeProjects_active' : 'b-content-table__financeProjects';
                });
                self.BudgetClass = ko.computed(function () {
                    return self.viewName() === 'FinanceBudgetRow' ? 'b-content-table__financeBudgets_active' : 'b-content-table__financeBudgets';
                });
            }
            //
            self.tableModel = null; //модель таблицы, которой управляем
            self.ready = ko.observable(false);
            self.filtersModel = null; //модель управления фильтрами
            //
            {//click on buttons
                self.ShowActivesRequest = function () {
                    self.viewName('FinanceActivesRequest');
                };
                self.ShowPurchase = function () {
                    self.viewName('FinancePurchase');
                };
                self.ShowProjects = function () {
                    self.viewName('FinanceProjects');
                };
                self.ShowBudgets = function () {
                    self.viewName('FinanceBudgetRow');
                };
            }
            //
            {//scrolling buttons
                self.ScrollButtonsVisible = ko.observable(true);
                self.OnScrollDownClick = function () {
                    var $scrollContainer = $('.b-content-table__left');
                    //
                    if ($scrollContainer.length > 0) {
                        var oldvalue = $scrollContainer.scrollTop();
                        if ((oldvalue + $scrollContainer.height()) < $scrollContainer[0].scrollHeight) {
                            var newvalue = oldvalue + 400;
                            $scrollContainer.animate({ scrollTop: newvalue }, 800);
                        }
                    }
                };
                self.OnScrollUpClick = function () {
                    var $scrollContainer = $('.b-content-table__left');
                    //
                    if ($scrollContainer.length > 0) {
                        var oldvalue = $scrollContainer.scrollTop();
                        if (oldvalue > 0) {
                            var newvalue = oldvalue - 400 >= 0 ? oldvalue - 400 : 0;
                            $scrollContainer.animate({ scrollTop: newvalue }, 800);
                        }
                    }
                };
                self.InitScroll = function () {
                    var $scrollcontainer = $('.b-content-table__left');
                    $scrollcontainer.mousewheel(function (event, delta) {
                        this.scrollTop -= (delta * 30);
                        event.preventDefault();
                    });
                };
                self.UpdateScrollButtonsVisibility = function () {
                    var $region = $('.b-content-table__left');
                    if ($region.length > 0) {
                        if ($region[0].scrollHeight > $region.height())
                            self.ScrollButtonsVisible(true);
                        else self.ScrollButtonsVisible(false);
                    }
                    else {
                        self.ScrollButtonsVisible(true);
                        setTimeout(self.UpdateScrollButtonsVisibility, 200);//try again later
                    };
                };
            }
            //
            //filter buttons
            self.AfterRender = function () {
                $.when(userD).done(function (user) {
                    if (user.HasRoles) {
                        $('#listModeOrders').css('display', 'block');
                        $('#listModeBuying').css('display', 'block');
                        $('#listModeProjects').css('display', 'block');
                        if (user.BudgetEnabled == true)
                            $('#listModeBudgets').css('display', 'block');
                        else
                            $('#listModeBudgets').remove();
                    }
                    else {
                        $('#listModeOrders').remove();
                        $('#listModeBuying').remove();
                        $('#listModeProjects').remove();
                        $('#listModeBudgets').remove();
                    }
                    //
                    self.UpdateScrollButtonsVisibility();
                    self.InitScroll();
                    $(window).resize(self.UpdateScrollButtonsVisibility);
                    //
                    self.viewName(user.ViewNameFinance);
                });
            };
        }
    }
    return module;
});