define(['knockout', 'jquery', 'ttControl', 'ajax'], function (ko, $, tclib, ajaxLib) {
    var module = {
        Modes: {
            Classic: 'Classic',
            Small: 'Small',
            Minimal: 'Minimal'
        },
        ViewModel: function () {
            var self = this;
            self.ajaxControl = new ajaxLib.control();
            self.isLoaded = ko.observable(false);
            //
            self.userFullName = ko.observable("");
            self.userPositionName = ko.observable("");
            self.MainLogoSrc = ko.observable('');
            self.appAbout = ko.observable(`${getTextResource('MainLinkName')} ${applicationVersion}`);
            //
            self.OnlyMinimal = ko.observable(false);
            self.Mode = ko.observable();
            self.Mode.subscribe(function (newValue) {
                if (newValue == module.Modes.Classic)
                    $('.b-main').css({ top: 128 });
                //
                if (newValue == module.Modes.Small)
                    $('.b-main').css({ top: 98 });
                //
                if (newValue == module.Modes.Minimal)
                    $('.b-main').css({ top: 64 });
            });
            self.IsMinimalMode = ko.computed(function () {
                return self.Mode() == module.Modes.Minimal;
            });
            self.IsSmallMode = ko.computed(function () {
                return self.Mode() == module.Modes.Small;
            });
            self.IsClassicMode = ko.computed(function () {
                return self.Mode() == module.Modes.Classic;
            });
            //
            self.GetOpenedModuleName = function () {
                var currAbsolutePath = window.location.pathname;
                if (currAbsolutePath.indexOf('/SD/DashboardSearch') != -1)
                    return 'DashboardSearch';
                else if (currAbsolutePath.indexOf('/SD/KBArticleSearch') != -1)
                    return 'KBArticleSearch';
                else if (currAbsolutePath.indexOf('/SD/ServiceCatalogue') != -1)
                    return 'ServiceCatalogue';
                else if (currAbsolutePath.indexOf('/SD/TimeManagement') != -1)
                    return 'TimeManagement';
                else if (currAbsolutePath.indexOf('/Account') != -1)
                    return 'Account';
                else if (currAbsolutePath.indexOf('/Asset/Table') != -1)
                    return 'AssetTable';
                else if (currAbsolutePath.indexOf('/Finance/Table') != -1)
                    return 'Finance';
                else if (currAbsolutePath.indexOf('/Asset/Settings') != -1) 
                    return 'Settings';
                else if (currAbsolutePath.indexOf('SD/Table') != -1)
                    return 'SD';
                else return 'SD'; //hack
            };
            self.CurrentPageName = ko.observable('');
            self.CalculatedMenuActionsCount = ko.observable(0);
            self.CalculatedMenuActionsWithoutUserNameCount = ko.observable(0);
            self.RecalculateSize = function () {
                var maxWidth = $('#mainMenu').width();
                //
                if (!self.OnlyMinimal()) {
                    if (maxWidth > 1674)
                        self.Mode(module.Modes.Classic);
                    else if (maxWidth > 1359)
                        self.Mode(module.Modes.Small);
                    else self.Mode(module.Modes.Minimal);
                }
                //
                if (self.IsClassicMode()) {
                    var withoutLogo = maxWidth - $('#mainMenu').find('.mainMenuCompany').outerWidth();
                    var withoutBlueButtons = Math.max(withoutLogo - $('#mainMenu').find('.mainMenuBlueActionsList').outerWidth(true), 0);
                    var withoutName = withoutBlueButtons - 80; // ширина кнопки
                    var withName = withoutBlueButtons - 300; //ширина юзерблока 300
                    //
                    var oneButtonSize = Math.max($('#mainMenu').find('.mainMenuAction').outerWidth(true));
                    var count = (withName - withName % oneButtonSize) / oneButtonSize;
                    var countWithoutName = (withoutName - withoutName % oneButtonSize) / oneButtonSize;
                    //
                    self.CalculatedMenuActionsCount(count);
                    self.CalculatedMenuActionsWithoutUserNameCount(countWithoutName);
                }
                else if (self.IsSmallMode()) {
                    var withoutLogo = maxWidth - $('#mainMenu').find('.mainMenuCompany').outerWidth();
                    var withoutBlueButtons = withoutLogo - $('#mainMenu').find('.mainMenuBlueActionsList').outerWidth(true);
                    var withoutName = withoutBlueButtons - 75; // ширина кнопки 60 + 15 люфта
                    var withName = withoutBlueButtons - 240; //ширина юзерблока 240
                    //
                    var oneButtonSize = Math.max($('#mainMenu').find('.mainMenuAction').outerWidth(true));
                    var count = (withName - withName % oneButtonSize) / oneButtonSize;
                    var countWithoutName = (withoutName - withoutName % oneButtonSize) / oneButtonSize;
                    //
                    self.CalculatedMenuActionsCount(count);
                    self.CalculatedMenuActionsWithoutUserNameCount(countWithoutName);
                }
            };
            //
            self.HideUserName = ko.observable(false);
            //
            self.moreButton = new module.ButtonElement({
                Name: getTextResource('More'),
                IconClass: 'mainMenuBtn-More',
                ClickAction: function (button, e) {
                    if (self.ShowMoreClick)
                        return self.ShowMoreClick(button, e);
                    else return false;
                },
                Order: 9999
            });
            self.profileSettingsButton = new module.ButtonElement({
                Name: getTextResource('ProfileSettings'),
                IconClass: 'mainMenuBtn-Settings',
                ClickAction: function () {
                    showSpinner();
                    setLocation('Account/ProfileSettings');
                },
                IsSelected: function () {
                    return self.GetOpenedModuleName() == 'Account';
                },
                Order: 1
            });
            self.profileSettingsWithNameButton = new module.ButtonElement({
                Name: '', //in function
                IconClass: 'mainMenuBtn-Settings',
                ClickAction: function () {
                    showSpinner();
                    setLocation('Account/ProfileSettings');
                },
                IsSelected: function () {
                    return self.GetOpenedModuleName() == 'Account';
                },
                Order: 1
            });
            self.configButton = new module.ButtonElement({
                Name: getTextResource('WebServiceSettings'),
                IconClass: 'mainMenuBtn-SettingWeb',
                ClickAction: function () {
                    showSpinner();
                    $.when(userD).done(function (user) {
                        user.UserID = null;//eventManager.js - userSessionChanged (skip redirect)
                        setLocation('Config/Settings');
                    });
                },
                Order: 3
            });
            //
            self.exitButton = new module.ButtonElement({
                Name: getTextResource('SignOut'),
                IconClass: '',
                ClickAction: function () {
                    showSpinner();
                    self.ajaxControl.Ajax(null,
                        {
                            url: '/accountApi/SignOut',
                            method: 'POST'
                        },
                        function (response) {
                            if (response == false) {
                                hideSpinner();
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('FailedToLogout'), 'error');
                                });
                            }
                            else
                                setLocation('SD/Table');
                        });
                },
                Order: 5
            });
            //
            self.taskActions = ko.observableArray([]);
            self.IsAnyTaskActionVisible = ko.observable(true);
            self.blueActionsList = ko.observableArray([]);
            //
            self.actionsList = ko.observableArray([]);
            self.actionsListWithAccess = ko.computed(function () {
                var tmp = self.actionsList();
                var retval = [];
                ko.utils.arrayForEach(tmp, function (el) {
                    if (el && el.Visible())
                        retval.push(el);
                });
                return retval;
            });
            self.showedActionsList = ko.computed(function () {
                var count = self.CalculatedMenuActionsCount();
                var countWithoutName = self.CalculatedMenuActionsWithoutUserNameCount();
                var retval = [];
                var moreNeeded = true;
                if (count > 1) {
                    var actions = self.actionsListWithAccess();
                    if (actions.length <= count) {
                        moreNeeded = false;
                        ko.utils.arrayForEach(actions, function (action) {
                            retval.push(action);
                        });
                        self.HideUserName(false);
                    }
                    else if (actions.length <= countWithoutName) {
                        moreNeeded = false;
                        ko.utils.arrayForEach(actions, function (action) {
                            retval.push(action);
                        });
                        self.HideUserName(true);
                    }
                    else {
                        self.HideUserName(true);
                        count = countWithoutName;
                        //
                        for (var i = 0; i < actions.length && count > 1; i++, count--) {
                            retval.push(actions[i]);
                        }
                    }
                }
                //
                retval.sort(function (a, b) {
                    return a.Order == b.Order ? 0 : a.Order < b.Order ? -1 : 1;
                })
                //
                if (moreNeeded)
                    retval.push(self.moreButton);
                //
                return retval;
            });
            self.moreActionsList = ko.computed(function () {
                var count = self.CalculatedMenuActionsCount();
                if (self.HideUserName())
                    count = self.CalculatedMenuActionsWithoutUserNameCount();
                var retval = [];
                var actions = self.actionsListWithAccess();
                if (count > 1) {
                    if (actions.length > count) {
                        var needed = actions.length - count + 1;
                        for (var i = actions.length - needed; i < actions.length; i++) {
                            retval.push(actions[i]);
                        }
                    }
                }
                else ko.utils.arrayForEach(actions, function (action) {
                    retval.push(action);
                });
                //
                retval.sort(function (a, b) {
                    return a.Order == b.Order ? 0 : a.Order < b.Order ? -1 : 1;
                })
                //
                if (self.moreButton)
                    self.moreButton.SubMenuList(retval);
                //
                return retval;
            });
            //
            self.profileActions = ko.computed(function () {
                var retval = [];
                //
                if (self.IsMinimalMode()) {
                    var tmp = self.actionsListWithAccess();
                    ko.utils.arrayForEach(tmp, function (el) {
                        if (el)
                            retval.push(el);
                    });
                }
                //
                if (self.configButton.Visible())
                    retval.push(self.configButton);

                if (self.IsMinimalMode() || self.HideUserName())
                    retval.push(self.profileSettingsWithNameButton);
                //
                return retval;
            });
            //
            self.CreateProfileButtons = function () {
                var retD = $.Deferred();
                //
                $.when(userD).done(function (user) {
                    //
                    self.profileSettingsWithNameButton.Visible(true);
                    self.profileSettingsWithNameButton.Name(user.UserFullName);
                    self.profileSettingsWithNameButton.Details(user.UserPositionName);
                    //                    
                    var adminD = $.Deferred();
                    $.when(operationIsGrantedD(357), userD).done(function (result, user) { //OPERATION_SD_General_Administrator = 357
                        self.configButton.Visible(result);
                        //
                        adminD.resolve();
                       
                    });
                    //
                    self.exitButton.Visible(true);
                    //
                       $.when(adminD).done(function () {
                        retD.resolve();
                    });
                });
                //
                return retD.promise();
            };
            self.CreateTaskButtons = function () {
                var retD = $.Deferred();
                //
                $.when(userD).done(function (user) {
                    var call = new module.ButtonElement({
                        Name: getTextResource('Call'),
                        IconClass: 'mainMenuBtn-CreateCall',
                        ClickAction: function () {
                            showSpinner();
                            require(['registrationForms'], function (lib) {
                                var fh = new lib.formHelper(true);
                                fh.ShowCallRegistrationEngineer();
                            });
                        },
                        Order: 1
                    });
                    var callD = $.Deferred();
                    $.when(operationIsGrantedD(309)).done(function (result) { //OPERATION_Call_Add = 309
                        if (result == true) {
                            call.Visible(true);
                            self.taskActions.push(call);
                        }
                        else 
                            call.Visible(false);                        
                        callD.resolve();
                    });
                    //
                    var wo = new module.ButtonElement({
                        Name: getTextResource('WorkOrder'),
                        IconClass: 'mainMenuBtn-CreateWO',
                        ClickAction: function () {
                            showSpinner();
                            require(['registrationForms'], function (lib) {
                                var fh = new lib.formHelper(true);
                                fh.ShowWorkOrderRegistration();
                            });
                        },
                        Order: 2
                    });
                    var woD = $.Deferred();
                    $.when(operationIsGrantedD(301)).done(function (result) { //OPERATION_WorkOrder_Add = 301
                        if (result == true) {
                            wo.Visible(true);
                            self.taskActions.push(wo);
                        }
                        else 
                            wo.Visible(false);                        
                        woD.resolve();
                    });
                    //
                    var pb = new module.ButtonElement({
                        Name: getTextResource('Problem'),
                        IconClass: 'mainMenuBtn-CreateProblem',
                        ClickAction: function () {
                            showSpinner();
                            require(['registrationForms'], function (lib) {
                                var fh = new lib.formHelper(true);
                                fh.ShowProblemRegistration();
                            });
                        },
                        Order: 3
                    });
                    var pbD = $.Deferred();
                    $.when(operationIsGrantedD(319)).done(function (result) { //OPERATION_Problem_Add = 319
                        if (result == true) {
                            pb.Visible(true);
                            self.taskActions.push(pb);
                        }
                        else 
                            pb.Visible(false);                        
                        pbD.resolve();
                    });
                    //
                    var rfc = new module.ButtonElement({
                        Name: getTextResource('RFC'),
                        IconClass: 'mainMenuBtn-CreateRFC',
                        ClickAction: function () {
                            showSpinner();
                            require(['registrationForms'], function (lib) {
                                var fh = new lib.formHelper(true);
                                fh.ShowRFCRegistration();
                            });
                        },
                        Order: 1
                    });
                    var rfcD = $.Deferred();
                    $.when(operationIsGrantedD(384)).done(function (result) { //OPERATION_RFC_Add = 384
                        if (result == true) {
                            rfc.Visible(true);
                            self.taskActions.push(rfc);
                        }
                        else
                            rfc.Visible(false);
                        rfcD.resolve();
                    });

                    // TODO: MI Button
                    var miD = $.Deferred();
                    const massIncident = new module.ButtonElement({
                        Name: getTextResource('MassIncident_MainMenuButtonCaption'),
                        IconClass: 'mainMenuBtn-CreateMassIncident',
                        ClickAction: function () {
                            showSpinner();
                            require(['registrationForms'], function (lib) {
                                var fh = new lib.formHelper(true);
                                fh.ShowMassIncidentRegistration();
                            });
                        },
                        Order: 1
                    });
                    $.when(operationIsGrantedD(982)).done(function (result) {
                        massIncident.Visible(result || false);

                        if (result) {
                            self.taskActions.push(massIncident);
                        }

                        miD.resolve();
                    });
                    
                    //
                    $.when(callD, pbD, woD, rfcD, miD.promise()).done(function () {
                        retD.resolve();
                        //
                        if (call.Visible() == false && pb.Visible() == false && wo.Visible() == false && rfc.Visible() == false && !massIncident.Visible())
                            self.IsAnyTaskActionVisible(false);
                    });
                });
                //
                return retD.promise();
            };
            //
            self.CreateBlueButtons = function () {
                var retD = $.Deferred();
                //
                $.when(userD).done(function (user) {
                    var findClient = new module.ButtonElement({
                        Name: getTextResource('ClientSearch'),
                        IconClass: 'mainMenuBlueBtn-FindClient',
                        ClickAction: function () {
                            showSpinner();
                            require(['usualForms'], function (lib) {
                                var fvm = new lib.formHelper(true);
                                fvm.ShowClientSearcher();
                            });
                        },
                        Order: 1,
                        NeedTooltip: function () { return self.IsMinimalMode() === true; }
                    });
                    self.blueActionsList.push(findClient);

                    var findClientD = $.Deferred();
                    $.when(operationIsGrantedD(309), operationIsGrantedD(518), operationIsGrantedD(71))
                     .done(function (call_add, call_properties, user_properties) {
                        // 309 Call.Add, 518 Call.Properties, 71 User.Properties
                        if ((call_add == true || call_properties == true) && user_properties == true)
                            findClient.Visible(true);
                        else
                            findClient.Visible(false);
                        findClientD.resolve();
                    });
                    //
                    var createTask = new module.ButtonElement({
                        Name: (user.HasRoles == false || user.ViewNameSD == 'ClientCallForTable') ? getTextResource('ButtonCreate') : getTextResource('TooltipCreateTask'),
                        IconClass: 'mainMenuBlueBtn-Create',
                        ClickAction: function (button, e) {
                            $.when(userD).done(function (userNew) {
                                if
                                (
                                    userNew.HasRoles == false ||
                                    (self.GetOpenedModuleName() == 'SD' && userNew.ViewNameSD == 'ClientCallForTable') ||
                                    (self.IsAnyTaskActionVisible() == false || self.taskActions().length == 0)
                                ) {
                                    showSpinner();
                                    require(['registrationForms'], function (lib) {
                                        var fh = new lib.formHelper(true);
                                        fh.ShowCallRegistration();
                                    });
                                }
                                else {
                                    button.SubMenuList(self.taskActions());
                                    openRegion($(e.currentTarget).find('.mainMenuBlueAction-submenu'), e);
                                }
                            });
                        },
                        Order: 2,
                        NeedTooltip: function () { return self.IsMinimalMode() === true; }
                    });
                    self.blueActionsList.push(createTask);
                    createTask.Visible(true);
                    //
                    var search = new module.ButtonElement({
                        Name: getTextResource('Search'),
                        IconClass: 'mainMenuBlueBtn-Search',
                        ClickAction: function () {
                            showSpinner();
                            require(['usualForms'], function (lib) {
                                var fvm = new lib.formHelper(true);
                                fvm.ShowSearcher(self.GetOpenedModuleName());
                            });
                        },
                        Order: 3,
                        NeedTooltip: function () { return self.IsMinimalMode() === true; }
                    });
                    self.blueActionsList.push(search);
                    search.Visible(true);
                    //
                    $.when(findClientD).done(function () {
                        retD.resolve();
                    });
                });
                //
                return retD.promise();
            };
            //
            self.CreateButtons = function () {
                var retD = $.Deferred();
                //
                $.when(userD).done(function (user) {
                    var tasks = new module.ButtonElement({
                        Name: getTextResource('TooltipMyWorkplace'),
                        IconClass: 'mainMenuBtn-Tasks',
                        ClickAction: function () {
                            showSpinner();
                            setLocation('SD/Table');
                        },
                        IsSelected: function () {
                            return self.GetOpenedModuleName() == 'SD';
                        },
                        Order: 1
                    });
                    self.actionsList.push(tasks);
                    tasks.Visible(true);
                    //
                    var serviceCatalog = new module.ButtonElement({
                        Name: getTextResource('TooltipServiceCatalogue'),
                        IconClass: 'mainMenuBtn-Catalogue',
                        ClickAction: function () {
                            showSpinner();
                            setLocation('SD/ServiceCatalogue');
                        },
                        IsSelected: function () {
                            return self.GetOpenedModuleName() == 'ServiceCatalogue';
                        },
                        Order: 2
                    });
                    self.actionsList.push(serviceCatalog);
                    serviceCatalog.Visible(true);
                    //  
                    var assetsD = $.Deferred();
                    $.when(operationIsGrantedD(650)).done(function (configuration_module) {
                        if (configuration_module == true) {
                            var assets = new module.ButtonElement({
                                Name: getTextResource('TooltipAssetTable'),
                                IconClass: 'mainMenuBtn-Assets',
                                ClickAction: function () {
                                    showSpinner();
                                    setLocation('Asset/Table');
                                },
                                IsSelected: function () {
                                    return self.GetOpenedModuleName() == 'AssetTable';
                                },
                                Order: 3
                            });
                            assets.Visible(true);
                            assets.Enable(false);
                            self.actionsList.push(assets);
                        }
                        assetsD.resolve();
                    });
                    //
                    var kb = new module.ButtonElement({
                        Name: getTextResource('TooltipKB'),
                        IconClass: 'mainMenuBtn-KB',
                        ClickAction: function () {
                            showSpinner();
                            setLocation('SD/KBArticleSearch');
                        },
                        IsSelected: function () {
                            return self.GetOpenedModuleName() == 'KBArticleSearch';
                        },
                        Order: 4
                    });
                    self.actionsList.push(kb);
                    kb.Visible(true);
                    //
                    var dashboardsD = $.Deferred();
                    $.when(operationIsGrantedD(654)).done(function (dashboard_module) {
                        if (dashboard_module == true) {
                            var dashboards = new module.ButtonElement({
                                Name: getTextResource('TooltipDashboards'),
                                IconClass: 'mainMenuBtn-Dashboards',
                                ClickAction: function () {
                                    showSpinner();
                                    setLocation('SD/DashboardSearch');
                                },
                                IsSelected: function () {
                                    return self.GetOpenedModuleName() == 'DashboardSearch';
                                },
                                Order: 5
                            });
                            dashboards.Visible(true);
                            self.actionsList.push(dashboards);
                        }
                        dashboardsD.resolve();
                    });
                    //
                    var timeManager = new module.ButtonElement({
                        Name: getTextResource('TooltipTimeManagement'),
                        IconClass: 'mainMenuBtn-Time',
                        ClickAction: function () {
                            showSpinner();
                            setLocation('SD/TimeManagement');
                        },
                        IsSelected: function () {
                            return self.GetOpenedModuleName() == 'TimeManagement';
                        },
                        Order: 6
                    });
                    //self.actionsList.push(timeManager);
                    var timeManagerD = $.Deferred();
                    if (user.TimeManagementEnabled == true)
                        timeManager.Visible(true);
                    else timeManager.Visible(false);
                    timeManagerD.resolve();
                    //
                    var financeD = $.Deferred();
                    $.when(operationIsGrantedD(652)).done(function (configuration_module) {
                        if (configuration_module == true) {
                            var finance = new module.ButtonElement({
                                Name: getTextResource('TooltipFinance'),
                                IconClass: 'mainMenuBtn-Finance',
                                ClickAction: function () {
                                    showSpinner();
                                    setLocation('Finance/Table');
                                },
                                IsSelected: function () {
                                    return self.GetOpenedModuleName() == 'Finance';
                                },
                                Order: 7
                            });
                            finance.Visible(true);
                            // self.actionsList.push(finance);
                        }
                        financeD.resolve();
                    });
                
                    $.when(timeManagerD, dashboardsD, assetsD, financeD).done(function () {
                        retD.resolve();
                    });
                });
                //
                return retD.promise();
            };
            //            
            self.ShowMoreClick = function (button, e) {
                $(e.currentTarget).addClass('menuexpanded');
                openRegion($(e.currentTarget).find('.mainMenuAction-submenu'), e, function () {
                    $(e.currentTarget).removeClass('menuexpanded');
                });
            };
            self.ShowProfileOptionsClick = function (button, e) {
                openRegion($(e.currentTarget).find('.mainMenuUserBlock-submenu'), e);
            };
            self.LogoClick = function () {
                showSpinner();
                setLocation('SD/Table');
            };
            //
            self.AfterRender = function () {
                getLogoPath('menu').done(function (path) {
                    self.MainLogoSrc(path);
                });
                //
                $.when(userD, self.CreateProfileButtons(), self.CreateTaskButtons(), self.CreateBlueButtons(), self.CreateButtons()).done(function (user) {
                    self.userFullName(user.UserFullName);
                    self.userPositionName(user.UserPositionName);
                    if (user.UseCompactMenuOnly === true) {
                        self.OnlyMinimal(true);
                        self.Mode(module.Modes.Minimal);
                    }
                    //
                    var curPage = self.GetOpenedModuleName();
                    if (curPage == 'DashboardSearch')
                        self.CurrentPageName(getTextResource('TooltipDashboards'));
                    else if (curPage == 'KBArticleSearch')
                        self.CurrentPageName(getTextResource('TooltipKB'));
                    else if (curPage == 'ServiceCatalogue')
                        self.CurrentPageName(getTextResource('TooltipServiceCatalogue'));
                    else if (curPage == 'TimeManagement')
                        self.CurrentPageName(getTextResource('TooltipTimeManagement'));
                    else if (curPage == 'Account')
                        self.CurrentPageName(getTextResource('ProfileSettings'));
                    else if (curPage == 'AssetTable')
                        self.CurrentPageName(getTextResource('TooltipAssetTable'));
                    else if (curPage == 'SD')
                        self.CurrentPageName(getTextResource('TooltipMyWorkplace'));
                    //
                    self.isLoaded(true);
                    //
                    self.RecalculateSize();
                    $(window).resize(self.RecalculateSize);
                });
            };
        },
        ButtonElement: function (params) {
            var self = this;
            //
            self.Name = ko.observable(params.Name);
            self.Details = ko.observable(params.Details ? params.Details : '');//вторая строка в выпадающем меню
            //
            self.IconClass = params.IconClass;
            self.ClickAction = function (button, e) {
                if (params.ClickAction && self.Enable())
                    return params.ClickAction(button, e);
                else return false;
            };
            self.Order = params.Order;
            self.IsSelected = function () {
                if (params.IsSelected)
                    return params.IsSelected();
                else return false;
            };
            self.Enable = ko.observable(true);
            self.Visible = ko.observable(false);
            self.SubMenuList = ko.observableArray([]);
            //
            self.Tooltip = ko.computed(function () {
                if (!self.Name())
                    return '';
                //
                if (params.NeedTooltip && params.NeedTooltip())
                    return self.Name();
                //
                return '';
            });
        }
    }
    return module;
});
