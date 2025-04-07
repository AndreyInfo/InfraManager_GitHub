define(['knockout', 'jquery', 'ajax', 'iconHelper', 'selectControl', 'treeControl', 'models/SDForms/AssetReferenceList'], function (ko, $, ajaxLib, ihLib, scLib, treeLib, assetReferenceListLib) {
    var module = {
        ViewModel: function ($region) {
            var self = this;
            self.$region = $region;
            //

            self.SizeChanged = function () {

            };
            //
            self.startLoadingTable = ko.observable(false);
            self.assetList = ko.observable(null);
            //
            self.ajaxControl_load = new ajaxLib.control();
            //
            self.ajaxControl_loadTypes = new ajaxLib.control();
            self.controlTypeSelector = null;
            self.InitializeTypeSelector = function () {
                var retD = $.Deferred();
                //
                var $regionType = self.$region.find('.asset-link_paramsColumnType');
                var deffered = $.Deferred();
                self.controlTypeSelector = new scLib.control();
                self.controlTypeSelector.init($regionType,
                    {
                        Title: getTextResource('AssetNumber_TypeName'),
                        IsSelectMultiple: false,
                        AllowDeselect: true,
                        OnSelect: self.OnTypeSelected,
                        ShowSingleSelectionInRow: true
                    }, deffered.promise());
                //
                self.ajaxControl_loadTypes.Ajax($regionType,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/imApi/GetAssetLinkTypes'
                    },
                    function (newData) {
                        if (newData != null && newData.Result === 0 && newData.List) {
                            var retval = [];
                            //
                            newData.List.forEach(function (el) {
                                retval.push({
                                    ID: el.ID,
                                    Name: el.Name,
                                    Checked: false
                                });
                            });
                            //
                            deffered.resolve(retval);
                        }
                        else deffered.resolve();
                        //
                        $.when(self.controlTypeSelector.$initializeCompleted).done(function () {
                            retD.resolve();
                        });
                    });
                //
                return retD.promise();
            };
            self.SelectedType = ko.observable(null);
            self.IsTypeSelected = ko.computed(function () {
                return self.SelectedType() != null;
            });
            self.OnTypeSelected = function (type) {
                if (!type || type.Checked === false) {
                    self.SelectedType(null);
                    self.ImplementFilter();
                    return;
                }
                //
                self.SelectedType(type);
                //
                $.when(self.InitializeModelSelector(), self.InitializeVendorSelector()).done(function () {
                    self.ImplementFilter();
                });
            };
            //
            self.ajaxControl_loadModels = new ajaxLib.control();
            self.controlModelSelector = null;
            self.InitializeModelSelector = function () {
                var retD = $.Deferred();
                if (!self.IsTypeSelected()) {
                    retD.resolve();
                    return retD;
                }
                //
                var deffered = $.Deferred();
                var $regionModel = self.$region.find('.asset-link_paramsColumnModel');
                //
                if (!self.controlModelSelector) {
                    self.controlModelSelector = new scLib.control();
                    self.controlModelSelector.init($regionModel,
                        {
                            Title: getTextResource('AssetNumber_ModelName'),
                            AlwaysShowTitle: true,
                            IsSelectMultiple: true,
                            OnEditComplete: self.ImplementFilter
                        }, deffered.promise());
                }
                else {
                    self.controlModelSelector.ClearItemsList();
                    $.when(deffered).done(function (values) {
                        self.controlModelSelector.AddItemsToControl(values);
                    });
                }
                //
                var param = {
                    typeID: self.SelectedType().ID,
                };
                //
                self.ajaxControl_loadModels.Ajax($regionModel,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/imApi/GetAssetLinkModels?' + $.param(param)
                    },
                    function (newData) {
                        if (newData != null && newData.Result === 0 && newData.List) {
                            var retval = [];
                            //
                            newData.List.forEach(function (el) {
                                retval.push({
                                    ID: el.ID,
                                    Name: el.Name,
                                    Checked: false
                                });
                            });
                            //
                            deffered.resolve(retval);
                        }
                        else deffered.resolve();
                        //
                        $.when(self.controlModelSelector.$initializeCompleted).done(function () {
                            retD.resolve();
                        });
                    });
                //
                return retD.promise();
            };
            //
            self.ajaxControl_loadVendors = new ajaxLib.control();
            self.controlVendorSelector = null;
            self.InitializeVendorSelector = function () {
                var retD = $.Deferred();
                if (!self.IsTypeSelected()) {
                    retD.resolve();
                    return retD;
                }
                //
                var deffered = $.Deferred();
                var $regionVendor = self.$region.find('.asset-link_paramsColumnVendor');
                //
                if (!self.controlVendorSelector) {
                    self.controlVendorSelector = new scLib.control();
                    self.controlVendorSelector.init($regionVendor,
                        {
                            Title: getTextResource('AssetNumber_VendorName'),
                            AlwaysShowTitle: true,
                            IsSelectMultiple: true,
                            OnEditComplete: self.ImplementFilter
                        }, deffered.promise());
                }
                else {
                    self.controlVendorSelector.ClearItemsList();
                    $.when(deffered).done(function (values) {
                        self.controlVendorSelector.AddItemsToControl(values);
                    });
                }
                //
                var param = {
                    typeID: self.SelectedType().ID,
                };
                //
                self.ajaxControl_loadVendors.Ajax($regionVendor,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/imApi/GetAssetLinkVendors?' + $.param(param)
                    },
                    function (newData) {
                        if (newData != null && newData.Result === 0 && newData.List) {
                            var retval = [];
                            //
                            newData.List.forEach(function (el) {
                                retval.push({
                                    ID: el.ID,
                                    Name: el.Name,
                                    Checked: false
                                });
                            });
                            //
                            deffered.resolve(retval);
                        }
                        else deffered.resolve();
                        //
                        $.when(self.controlVendorSelector.$initializeCompleted).done(function () {
                            retD.resolve();
                        });
                    });
                //
                return retD.promise();
            };
            //
            self.locationControl = null;
            self.InitLocationTree = function () {
                var retD = $.Deferred();
                var $regionLocation = self.$region.find('.asset-link_paramsColumnLocation');
                //
                if (!self.locationControl) {

                    self.locationControl = new treeLib.control();
                    self.locationControl.init($regionLocation, 1, {
                        onClick: self.OnSelectLocation,
                        UseAccessIsGranted: true,
                        ShowCheckboxes: false,
                        AvailableClassArray: [29, 101, 1, 2, 3, 4, 22],
                        ClickableClassArray: [29, 101, 1, 2, 3, 4, 22],
                        AllClickable: false,
                        FinishClassArray: [4, 22],
                        Title: getTextResource('LocationCaption'),
                        WindowModeEnabled: true
                    });
                }
                //
                $.when(self.locationControl.$isLoaded).done(function () {
                    retD.resolve();
                });
                //
                return retD.promise();
            };
            self.SelectedLocation = ko.observable(null);
            self.LocationSelected = ko.computed(function () {
                return self.SelectedLocation() == null;
            });
            self.OnSelectLocation = function (node) {
                if (node && node.ClassID == 29) {
                    if (self.SelectedLocation()) {
                        self.SelectedLocation(null);
                        self.ImplementFilter();
                    }
                    self.locationControl.DeselectNode();
                    //
                    return false;
                }
                //
                self.SelectedLocation(node);
                self.ImplementFilter();
                //
                return true;
            };
            //
            self.ImplementFilter = function () {
                var returnD = $.Deferred();
                //
                var models = [];
                var vendors = [];
                var typeID = self.SelectedType() ? self.SelectedType().ID : null;
                var locationClassID = self.SelectedLocation() ? self.SelectedLocation().ClassID : null;
                var locationID = self.SelectedLocation() ? self.SelectedLocation().ID : null;
                //
                if (typeID && self.controlModelSelector && self.controlVendorSelector) {
                    var currentModels = self.controlModelSelector.GetSelectedItems();
                    if (currentModels)
                        ko.utils.arrayForEach(currentModels, function (el) {
                            models.push(el.ID);
                        });
                    //
                    var currentVendors = self.controlVendorSelector.GetSelectedItems();
                    if (currentVendors)
                        ko.utils.arrayForEach(currentVendors, function (el) {
                            vendors.push(el.ID);
                        });
                }
                //
                //var old = self.tableModel.searchFilterData();
                var old = null;
                var newData = {
                    TypeID: typeID,
                    ModelsID: models,
                    VendorsID: vendors,
                    LocationClassID: locationClassID,
                    LocationID: locationID
                };
                //
                if (self.IsFilterDataDifferent(old, newData)) {
                    //self.tableModel.searchFilterData(newData);
                    //
                    /*$.when(self.UpdateTable()).done(function () {
                        returnD.resolve();
                    });*/
                    returnD.resolve();
                }
                else returnD.resolve();
                //
                return returnD;
            };
            self.IsFilterDataDifferent = function (oldData, newData) {
                if (!oldData || !newData)
                    return false;
                //
                if (oldData.TypeID !== newData.TypeID)
                    return true;
                //
                if (arr_diff(oldData.ModelsID, newData.ModelsID).length != 0)
                    return true;
                //
                if (arr_diff(oldData.VendorsID, newData.VendorsID).length != 0)
                    return true;
                //
                if (oldData.LocationID !== newData.LocationID)
                    return true;
                //
                return false;
            };
            var arr_diff = function (a1, a2) {
                var a = [], diff = [];
                for (var i = 0; i < a1.length; i++) {
                    a[a1[i]] = true;
                }
                //
                for (var i = 0; i < a2.length; i++) {
                    if (a[a2[i]]) {
                        delete a[a2[i]];
                    } else {
                        a[a2[i]] = true;
                    }
                }
                //
                for (var k in a) {
                    diff.push(k);
                }
                //
                return diff;
            };
            //
            self.SearchText = ko.observable('');
            self.SearchText.subscribe(function (newValue) {
                self.WaitAndSearch(newValue);
            });
            self.IsSearchTextEmpty = ko.computed(function () {
                var text = self.SearchText();
                if (!text)
                    return true;
                //
                return false;
            });
            //
            self.SearchKeyPressed = function (data, event) {
                if (event.keyCode == 13) {
                    if (!self.IsSearchTextEmpty())
                        self.Search();
                }
                else
                    return true;
            };
            self.EraseTextClick = function () {
                self.SearchText('');
            };
            //
            self.searchTimeout = null;
            self.WaitAndSearch = function (text) {
                clearTimeout(self.searchTimeout);
                self.searchTimeout = setTimeout(function () {
                    if (text == self.SearchText())
                        self.Search();
                }, 500);
            };
            //
            self.ajaxControl_search = new ajaxLib.control();
            self.Search = function () {
                var returnD = $.Deferred();
                //
                //self.tableModel.searchPhraseObservable(self.SearchText());
                //
                return returnD;
            };
            //
            //
            self.AfterRender = function () {
                //self.InitializeTable();
                self.InitializeTypeSelector();
                self.InitLocationTree();
            };
        }
    }
    return module;
});
