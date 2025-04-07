define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/AssetForms/AssetFields', 'models/AssetForms/AssetReferenceControl', 'ui_forms/Asset/Controls/AdapterList', 'ui_forms/Asset/Controls/PeripheralList', 'ui_forms/Asset/Controls/PortList', 'ui_forms/Asset/Controls/SlotList', 'ui_forms/Asset/Controls/SoftwareInstallationList', 'models/AssetForms/AssetForm.History'], function (ko, $, ajaxLib, fc, fhModule, tclib, assetFields, assetReferenceControl, adapterList, peripheralList, portList, slotList, installationList, assetHistory) {
    var module = {
        ViewModel: function ($region, openedModule) {
            var self = this;
            self.$region = $region;
            self.$isLoaded = $.Deferred();

            self.Load = function () {
                self.$isLoaded.resolve();
                return self.$isLoaded.promise();
            };

            self.AfterRender = function () {
                self.currentMode(self.modes.SDSearcher);
            };

            self.modes = {
                SDSearcher: 'SDSearcher',
                AssetSearcher: 'AssetSearcher',
            };
            //
            self.SizeChanged = function () {
                if (self.AssetSearcherReady())
                    self.modelAssetSearcher().SizeChanged();
            };
            //
            self.currentMode = ko.observable(null);
            self.currentMode.subscribe(function (newValue) {
                $.when(self.$isLoaded).done(function () {

                    if (newValue == self.modes.SDSearcher && !self.modelSDSearcher())
                        self.initSDSearcher();
                    //
                    if (newValue == self.modes.AssetSearcher && !self.modelAssetSearcher())
                        self.initAssetSearcher();
                    //
                });
            });
            //
            self.modelSDSearcher = ko.observable(null);
            self.SDSearcherReady = ko.observable(false);
            self.initSDSearcher = function () {
                require(['models/Search/searchForm'], function (vm) {
                    var mod = new vm.ViewModel($region, openedModule, null);
                    mod.Load();
                    self.modelSDSearcher(mod);
                    self.SDSearcherReady(true);
                    self.modelSDSearcher().FocusSearcher();
                });

                //self.modelSDSearcher(new module.SDSearcherModel(self.$region, self.OnSelectedChangeHandler, self.IsSelectedChecker));
            };
            self.selectSDSearcher = function () {
                self.currentMode(self.modes.SDSearcher);
                if (self.SDSearcherReady()) {
                    self.modelSDSearcher().FocusSearcher();
                }
            };
            self.isSDSearcherSelected = ko.computed(function () {
                return self.currentMode() == self.modes.SDSearcher;
            });
            //
            self.OnSelectedChangeHandler = function () {

            };
            self.IsSelectedChecker = function () {

            };
            //
            self.FocusAssetSearcher = function () {
                var searcher = self.$region.find('.searchForm__searcher.assetSearcher .text-input');
                searcher.focus();
            };
            //
            self.modelAssetSearcher = ko.observable(null);
            self.AssetSearcherReady = ko.observable(false);
            self.initAssetSearcher = function () {
                //
                require(['models/AssetForms/AssetLink/AssetLink'], function (vm) {
                    var mod = new vm.ParameterSelectorModel($region, self.OnSelectedChangeHandler, self.IsSelectedChecker, false, self.FocusAssetSearcher);
                    self.modelAssetSearcher(mod);
                    self.AssetSearcherReady(true);
                });
            };
            self.selectAssetSearcher = function () {
                self.currentMode(self.modes.AssetSearcher);
                if (self.AssetSearcherReady()) {
                    self.modelAssetSearcher().SizeChanged();
                    self.FocusAssetSearcher();
                }
            };
            self.isAssetSearcherSelected = ko.computed(function () {
                return self.currentMode() == self.modes.AssetSearcher;
            });
            //
        },
        ShowDialog: function (openedModule, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            $.when(userD).done(function (user) {
                var frm = undefined;
                var vm = undefined;
                //
                var buttons = {
                }
                //
                frm = new fc.control(
                    'region_searchForm',//form region prefix
                    'setting_searchForm',//location and size setting
                    getTextResource('SearchCaption'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    710, 520,//minSize
                    buttons,//form buttons
                    null,//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Search/frmSearch\', afterRender: AfterRender}"'//attributes of form region
                    );
                //
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                //
                frm.BeforeClose = function () {
                    if (vm.modelAssetSearcher())
                        vm.modelAssetSearcher().dispose();
                    hideSpinner();
                };
                //
                var $region = $('#' + frm.GetRegionID());
                vm = new module.ViewModel($region, openedModule);
                //
                frm.SizeChanged = function () {
                    var width = frm.GetInnerWidth();
                    var height = frm.GetInnerHeight();
                    //
                    vm.$region.css('width', width + 'px').css('height', height + 'px');
                    vm.SizeChanged();
                };
                //
                ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                $.when(frm.Show(), vm.Load()).done(function (frmD, loadD) {
                    if (loadD == false) {//force close
                        frm.Close();
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});