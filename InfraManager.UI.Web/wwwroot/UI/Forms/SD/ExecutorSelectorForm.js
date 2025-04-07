define(['knockout', 'jquery', 'ajax', 'urlManager', 'comboBox'], function (ko, $, ajaxLib, urlManager) {
    var module = {
        //
        modes: {// режимы: работы, отображения
            none: 'none',
            executorTab: 'executorTab',
            queueTab: 'queueTab'
        },
        param_mode: 'mode',
        param_tab: 'tab',
        ViewModel: function (obj) {
            var self = this;
            self.loadD = $.Deferred();
            self.SelectedObj = ko.observable(null);
            self.availability = functionsAvailability;
            self.modes = module.modes;
            self.ExecutorTab = ko.observable(null);
            self.QueueTab = ko.observable(null);
            self.ViewTemplateName = ko.observable(''); //шаблон контента (таб)
            self.SelectedViewMode = ko.observable(module.modes.none);//выбранное представление по умолчанию
            self.SelectedViewMode.subscribe(function (newValue) {
                //
                self.CheckData();
            });
            self.CheckData = function () {//загрузка / перезагрузка вкладки
                var activeMode = self.SelectedViewMode();
                var ss = function () { showSpinner($('.settingsModule')[0]); };
                var hs = function () { hideSpinner($('.settingsModule')[0]); };
                
                self.ViewTemplateName('');
                if (activeMode === module.modes.executorTab) {
                    ss();

                    require([ '../../../UI/Forms/SD/ExecutorSelectorEmployeeList' ], function (vm) {
                        var mod = new vm.ViewModel(obj, self);
                        self.ExecutorTab(mod);
                        self.ExecutorTab().clearAllInfos();
                        //
                        self.ViewTemplateName('../../../UI/Forms/SD/ExecutorSelectorEmployeeList');
                        self.ViewTemplateName.valueHasMutated();
                        hs();
                    });
                    self.UpdateAddButton(null);
                }
                else if (activeMode === module.modes.queueTab) {
                        ss();
                        require(['../../../UI/Forms/SD/ExecutorSelectorQueueList'], function (vm) {
                            var mod = new vm.ViewModel(obj, self);
                            self.QueueTab(mod);
                            self.QueueTab().clearAllInfos();
                            //
                            self.ViewTemplateName('../../../UI/Forms/SD/ExecutorSelectorQueueList');
                            self.ViewTemplateName.valueHasMutated();
                            self.loadD.resolve();
                            hs();
                        });
                    self.UpdateAddButton(null);
                }
                else
                    self.ViewTemplateName('');
            };
            self.ShowExecutorTab = function () {
                self.SelectedViewMode(module.modes.executorTab);
            };
            self.ShowQueueTab = function () {
                self.SelectedViewMode(module.modes.queueTab);
            };
            self.IsExecutorTabActive = ko.computed(function () {
                return self.SelectedViewMode() === module.modes.executorTab;
            });
            self.IsQueueTabActive = ko.computed(function () {
                return self.SelectedViewMode() == module.modes.queueTab;
            });
            self.AfterRenderMode = function () {
                self.SelectedViewMode(module.modes.executorTab);
                self.onResize();
                $(window).resize(self.onResize);
                $('.settings-main').click(function (e) {
                    if ($(e.target).is('input'))
                        self.onResize();
                });
            };
            self.ExecutorSelector = ko.observable(null);
            self.ModelAfterRender = function () {
                if (self.SelectedViewMode() == module.modes.none) {
                    self.SelectedViewMode(module.modes.executorTab);
                }
                self.OnResize();
            };

            self.Load = function () {
                self.loadD = $.Deferred();
                if (self.SelectedViewMode() == module.modes.none) {
                    self.SelectedViewMode(module.modes.executorTab);
                    self.loadD.resolve();
                }
                //
                return self.loadD.promise();
            };

            self.Save = function () {
                obj.SaveExecutorOrQueue(self.SelectedObj());
            }

            self.UpdateAddButton = function (obj) {
                var hasClicked = false;
                if (obj != null) {
                    hasClicked = true;                    
                }
                var elem = document.getElementsByClassName('btnVisibility');
                if (elem.length > 0) {                    
                    if (hasClicked)
                        elem[0].style.visibility = 'visible';
                    else
                        elem[0].style.visibility = 'hidden';
                }
            }

            self.OnResize = function (e) {
                var executorTab = self.ExecutorTab();
                if (executorTab != null)
                    executorTab.OnResize(e);
            };
        }
    }
    return module;
});