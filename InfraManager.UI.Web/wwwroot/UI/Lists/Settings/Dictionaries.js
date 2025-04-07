define(['knockout', 'jquery', 'ajax', 'treeControl', 'urlManager', 'ui_controls/ContextMenu/ko.ContextMenu'], function (ko, $, ajaxLib, treeLib, urlManager) {
    var module = {
        setParameters: function (values, raiseEvent) {//values - is object, сохранение в историю браузера значения url и изменение параметров url
            if (!values)
                return;
            //
            var params = urlManager.getQueryParams();
            var isChanged = false;
            for (var key in values) {
                var paramName = key;
                var paramValue = values[key];
                //
                if (params[paramName] != paramValue) {
                    params[paramName] = paramValue;
                    isChanged = true;
                }
            }
            //
            if (isChanged == true) {
                window.history.pushState(null, null, '/Asset/Settings' + urlManager.toQueryString(params));
                if (raiseEvent == undefined || raiseEvent == true)
                    $(document).trigger('settings_urlChanged', []);
            }
        },
        //
        modes: {// режимы: работы, отображения
            none: 'none',
            orgstructure: 'orgstructure', // Оргструктура,
            sdc: 'sdc', // ЦРПО
            rfccategory: 'rfccategory', // ЦРПО
            licenceSchemes: ' licenceSchemes', // схемы лицензирования
            elpList: 'elpList'  // Связи Инсталяций и Лицензий
        },
        param_mode: 'mode',
        param_tab: 'tab',
        //
        ViewModel: function () {//общая модель представления 
            var self = this;

            //
            self.availability = functionsAvailability;

            self.modes = module.modes;

            //модель справочников Орг. структура
            self.OrgStructure = ko.observable(null); 

            // модель справочника ЦРПО
            self.SoftwareDistributionCentres = ko.observable(null); 

            // модель справочника категорий RFC
            self.RFCCategory = ko.observable(null); 

            // модель схем лицензирования
            self.licenceSchemes = ko.observable(null); 

            // модель связей иснталляций и лицензий
            self.elpList = ko.observable(null); 

            //

            //шаблон контента (таб) 
            self.ViewTemplateName = ko.observable(''); 

            //

            //выбранное представление по умолчанию
            self.SelectedViewMode = ko.observable(module.modes.none);

            self.SelectedViewMode.subscribe(function (newValue) {
                var params = {};
                params[module.param_mode] = 'dictionaries';
                params[module.param_tab] = newValue;
                module.setParameters(params, false);
                //
                self.CheckData();
            });

            {//granted operations
                {
                    self.grantedOperations = [];
                    self.userID;
                    self.userHasRoles;
                    self.UserIsAdmin = false;
                    $.when(userD).done(function (user) {
                        self.userID = user.UserID;
                        self.userHasRoles = user.HasRoles;
                        self.grantedOperations = user.GrantedOperations;
                        self.UserIsAdmin = user.HasAdminRole;
                    });
                    self.operationIsGranted = function (operationID) {
                        for (var i = 0; i < self.grantedOperations.length; i++)
                            if (self.grantedOperations[i] === operationID)
                                return true;
                        return false;
                    };
                }
            }
            self.CheckMode = function () {//распознавание режима
                var mode = urlManager.getUrlParam(module.param_tab);
                var currentMode = self.SelectedViewMode().toLowerCase();
                //
                if (mode == module.modes.orgstructure.toLowerCase())
                    self.SelectedViewMode(module.modes.orgstructure);
                else if (mode == module.modes.sdc.toLowerCase())
                    self.SelectedViewMode(module.modes.sdc);
                else if (mode == module.modes.rfccategory.toLowerCase())
                    self.SelectedViewMode(module.modes.rfccategory);
                else if (mode == module.modes.licenceSchemes.toLowerCase())
                    self.SelectedViewMode(module.modes.licenceSchemes);
                else
                    self.SelectedViewMode(module.modes.orgstructure); // режим по умолчанию
                //
                return (mode != currentMode);
            };
            //
            self.CheckData = function () {//загрузка / перезагрузка вкладки
                var activeMode = self.SelectedViewMode();
                var ss = function () { showSpinner($('.settingsModule')[0]); };
                var hs = function () { hideSpinner($('.settingsModule')[0]); };
                //
                if (activeMode == module.modes.orgstructure) {
                    if (self.OrgStructure() == null) {
                        ss();

                        require(['ui_lists/Settings/OrgStructure'], function (vm) {
                            var mod = new vm.ViewModel();
                            self.OrgStructure(mod);
                            //
                            self.ViewTemplateName('../UI/Lists/Settings/OrgStructure');
                            hs();
                        });
                    }
                    else {
                        self.ViewTemplateName('../UI/Lists/Settings/OrgStructure');
                        self.OrgStructure().CheckData();
                    }
                }
                else if (activeMode == module.modes.sdc) {
                    if (self.SoftwareDistributionCentres() == null) {
                        ss();

                        require(['ui_lists/Settings/SoftwareDistributionCentres'], function (vm) {
                            var viewModel = new vm.ViewModel();
                            self.SoftwareDistributionCentres(viewModel);
                            //
                            self.ViewTemplateName('../UI/Lists/Settings/SoftwareDistributionCentres');
                            hs();
                        });
                    }
                    else {
                        self.ViewTemplateName('../UI/Lists/Settings/SoftwareDistributionCentres');
                        self.SoftwareDistributionCentres().CheckData();
                    }
                }
                else if (activeMode == module.modes.rfccategory) {                   
                    if (self.RFCCategory() == null) {
                        ss();
                        
                        require(['ui_lists/Settings/RFC/RFCCategory'], function (vm) {
                            var viewModel = new vm.ViewModel();
                            self.RFCCategory(viewModel);
                            //
                            self.ViewTemplateName('../UI/Lists/Settings/RFC/RFCCategory');
                            hs();
                        });
                    }
                    else {
                        self.ViewTemplateName('../UI/Lists/Settings/RFC/RFCCategory');
                        //self.RFCCategory().CheckData();
                    }
                }
                else if (activeMode == module.modes.licenceSchemes) {
                    if (self.licenceSchemes() == null) {
                        ss();
                       
                        require(['ui_lists/Settings/LicenceSchemes/LicenceSchemes'], function (vm) {
                            var viewModel = new vm.ViewModel();
                            self.licenceSchemes(viewModel);
                            //
                            self.ViewTemplateName('../UI/Lists/Settings/LicenceSchemes/LicenceSchemes');
                            hs();
                        });
                    }
                    else {
                        self.ViewTemplateName('../UI/Lists/Settings/LicenceSchemes/LicenceSchemes');                 
                    }
                }
                else if (activeMode == module.modes.elpList) {
                    if (self.elpList() == null) {
                        ss();
                       
                        require(['ui_lists/Settings/ELP/ELPList'], function (vm) {
                            var viewModel = new vm.ViewModel();
                            self.elpList(viewModel);
                            //
                            self.ViewTemplateName('../UI/Lists/Settings/ELP/ELPList');
                            hs();
                        });
                    }
                    else {
                        self.ViewTemplateName('../UI/Lists/Settings/ELP/ELPList');                 
                    }
                }
                else
                    self.ViewTemplateName('');
            };
            //
            self.ShowOrgStructure = function () {
                self.SelectedViewMode(module.modes.orgstructure);
            };
            self.ShowSoftwareDistributionCentres = function () {
                self.SelectedViewMode(module.modes.sdc);
            };
            self.ShowRFCCategory = function () {
                self.SelectedViewMode(module.modes.rfccategory);
            };
            self.CanShowRFCCategory = function () {
                if (self.operationIsGranted(711003))
                    return true;
                return false;
            }
            self.IsOrgStructureActive = ko.computed(function () {
                return self.SelectedViewMode() == module.modes.orgstructure;
            });
            self.IsDistributionCentresActive = ko.computed(function () {
                return self.SelectedViewMode() == module.modes.sdc;
            });
            self.IsRFCCategoryActive = ko.computed(function () {
                return self.SelectedViewMode() == module.modes.rfccategory;
            });

            
            // открыть раздел: "Схемы лицнзирования"
            self.ShowLicenceSchemes = function () {
                self.SelectedViewMode(module.modes.licenceSchemes);
            };

            // возможность работы с пунктом меню: "Схемы лицензирования"
            self.CanShowLicenceSchemes = function () {
                if (self.UserIsAdmin || self.operationIsGranted(750001))
                    return true;
                return false;
            };

            // активен пункт меню: "Схемы лицензирования"
            self.IsLicenceSchemesActive = ko.computed(function () {
                return self.SelectedViewMode() == module.modes.licenceSchemes;
            });

            
            // открыть раздел: "Связи инсталляций и лицензий"
            self.ShowELPList = function () {
                self.SelectedViewMode(module.modes.elpList);
            };

            // возможность работы с пунктом меню: "Связи ...."
            self.CanShowELPList = function () {
                return true;
            };

            // активен пункт меню: "Связи ...."
            self.IsELPListActive = ko.computed(function () {
                return self.SelectedViewMode() == module.modes.elpList;
            });



            self.AfterRenderMode = function () {
                self.CheckMode();
                self.onResize();
                $(window).resize(self.onResize);
                $('.settings-main').click(function (e) {//контекстные команды могут отобразится после клика на чекбоксе
                    if ($(e.target).is('input'))
                        self.onResize();
                });
            };
            self.ModelAfterRender = function () {
                if (self.SelectedViewMode() == module.modes.none) {
                    self.SelectedViewMode(module.modes.orgstructure);
                }
                self.OnResize();
            };
            self.Load = function () {
                var loadD = $.Deferred();
                //
                return loadD.promise();
            };
            self.OnResize = function (e) {
                var orgStructure = self.OrgStructure();
                if (orgStructure != null)
                    orgStructure.OnResize(e);
            };
            $(window).bind('settings_urlChanged', function () {//поменяется url (изменили режимы в модуле, ткнули куда-либо)
                if (self.CheckMode() == false)
                    self.CheckData();
            });
        }
    }
    return module;
});
