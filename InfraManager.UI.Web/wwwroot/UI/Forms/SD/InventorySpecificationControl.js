define(['knockout', 'ajax', 'ui_forms/SD/InventorySpecificationList', 'ui_forms/SD/InventorySpecificationSelectedList'],
    function (ko, ajaxLib, inventory_specificationListLib, inventory_specificationSelectedListLib) {
        var module = {
            Control: function (vm) {
                var self = this;
                //
                self.ajaxControl = new ajaxLib.control();
                self.addInventorySpecification = function () {
                    require(['assetForms'], function (fhModule) {
                        var fh = new fhModule.formHelper();
                        fh.ShowAssetLink({
                            ClassID: null,
                            ID: null,
                            ServiceID: null,
                            ClientID: null,
                            ShowWrittenOff: false,
                            Caption: getTextResource('Inventory_AssetAddCaption'),
                            IsInventory: true
                        }, function (newValues) {
                            if (!newValues || newValues.length == 0)
                                return;
                            //
                            var retval = [];
                            ko.utils.arrayForEach(newValues, function (el) {
                                if (el && el.ID)
                                    retval.push({ ID: el.ID, ClassID: el.ClassID });
                            });
                            //
                            var data = {
                                'WorkOrderID': vm.object().ID(),
                                'DependencyList': retval
                            };
                            //
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/assetApi/AddInventorySpecification'
                                },
                                function (model) {
                                    if (model.Result === 0) {
                                    }
                                    else {
                                        if (model.Result === 1) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[SDForm.LinkList.js, AddMaintenance]', 'error');
                                            });
                                        }
                                        else if (model.Result === 2) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[SDForm.LinkList.js, AddMaintenance]', 'error');
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
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[SDForm.LinkList.js, AddMaintenance]', 'error');
                                            });
                                        }
                                        //
                                    }
                                });
                        });
                    });
                };
                //
                //INVENTORY SPECIFICATIONS BLOCK
                self.selectedList = new inventory_specificationSelectedListLib.List(vm);
                self.list = new inventory_specificationListLib.List(vm, self.selectedList, self.addInventorySpecification);
                //
                self.addSpecificationEnabled = ko.computed(function () {
                    return self.list.AddSpecificationEnabled();
                });
                //
                self.ParameterReady = ko.observable(true);
                self.isParameterSelected = ko.observable(true);
                self.isChoosenSelected = ko.observable(false);
                self.isChoosenVisible = ko.observable(true);
                //
                self.selectParameterSelector = function () {
                    self.isParameterSelected(true);
                    self.isChoosenSelected(false);
                };
                //
                self.selectChoosen = function () {
                    self.isParameterSelected(false);
                    self.isChoosenSelected(true);
                };
                //
                self.ChoosenCounterText = ko.computed(function () {
                    return ' (' + self.selectedList.list().length + ')';
                });
                //
                self.ChoosenReady = ko.observable(true);
                //
                self.StreamSelection = ko.observable(false);
                self.list.StreamSelection = self.StreamSelection;
                self.showAccepted = ko.observable(false);
                self.list.ShowAccepted = self.showAccepted;
                self.showAccepted.subscribe(function (newValue) {
                    self.Search();
                });
                //
                self.SearchText = ko.observable('');
                self.list.SearchText = self.SearchText;
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
                    self.list.searchPhraseObservable(self.SearchText());
                    self.list.listView.load();
                };
            }
        }
        return module;
    });