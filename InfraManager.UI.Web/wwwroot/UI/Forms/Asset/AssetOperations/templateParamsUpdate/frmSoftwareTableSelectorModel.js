define(['knockout', 'jquery', 'ajax', 'formControl', 'treeControl',
    './../../../Asset/Controls/SoftwareModelList'],
    function (ko, $, ajax, formControl, treeLib, softwareModel) {
        var module = {
            Form: function (softwareLicence, onSaveCallback, showCreateModelButton, $region) {
                var self = this;
                self.ajaxControl = new ajax.control();
                //             

                self.$region = $region;

                self.list = new softwareModel.List(self);

                //
                self.onSaveCallback = onSaveCallback;
                self.SoftwareLicence = softwareLicence;
                self.SoftModelObject = ko.observable(null);
                self.UseNoCommercialModel = ko.observable(false);

                //
                //when tab unload
                self.dispose = function () {
                    self.list.dispose();
                    self.ajaxControl.Abort();
                };
                {//search
                    self.SearchText = ko.observable(self.SoftwareLicence.SoftwareModelName ? self.SoftwareLicence.SoftwareModelName : '');
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
                }
                //
                self.searchPhraseObservable = ko.observable(self.SoftwareLicence.SoftwareModelName ? self.SoftwareLicence.SoftwareModelName : '');
                self.Search = function () {
                    self.SoftModelObject(null);
                    self.searchPhraseObservable(self.SearchText());
                    self.list.listView.load();
                };
                self.list.SelectedItemsChanged = function (checkedItemsCount) {

                }
                //
                self.list.listViewRowClick = function (obj) {
                    self.SoftModelObject(new module.SoftwareModel(obj.ID, obj.Version, obj.Name, obj.ManufacturerID, obj.ManufacturerName));
                };

                self.Name = getTextResource('SoftwareModels');

                //навигатор
                {
                    self.AfterRender = function () {
                        self.initTree($region);
                    }

                    self.treeControl = null;
                    self.treeClassID_Model = ko.observable(null);
                    self.treeID_Model = ko.observable(null);
                    self.treeID_Model_handle = self.treeID_Model.subscribe(function () {
                        self.lv_tabModel.waitAndReload();
                    });
                    //дерево Навигатор по моделям ПО
                    self.initTree = function () {
                        var retD = $.Deferred();
                        var regionTree = $('#productNavigator');
                        //
                        if (!self.treeControl) {
                            self.treeControl = new treeLib.control();
                            self.treeControl.init(regionTree,
                                4,
                                {
                                    onClick: self.OnSelectSoftCatalogue,
                                    ShowCheckboxes: false,
                                    AvailableClassArray: [29, 92, 97],
                                    ClickableClassArray: [29, 92, 97],
                                    AllClickable: false,
                                    FinishClassArray: [97],
                                    HasLifeCycle: false,
                                    ExpandFirstLevel: true
                                });
                        }
                        //
                        $.when(self.treeControl.$isLoaded).done(function () {
                            retD.resolve();
                        });
                        return retD.promise();
                    };

                    self.SelectedObjectClassID = ko.observable(null);
                    self.SelectedObjectID = ko.observable(null);
                    self.SelectedObjectName = ko.observable('');

                    self.OnSelectSoftCatalogue = function (node) {
                        self.SelectedObjectClassID(node.ClassID);
                        self.SelectedObjectID(node.ID);
                        self.SelectedObjectName(node.Name);
                        //
                        self.list.listView.load();
                        return true;
                    };
                }

                //show form
                self.Show = function () {
                    showSpinner();

                    var forceClose = false;

                    var buttons = {};

                    if (showCreateModelButton) {
                        buttons[getTextResource('AddSoftwareModel')] = function () {
                            require(['ui_forms/Asset/AssetOperations/templateParamsUpdate/frmSoftwareModelAdd'], function (fhModule) {
                                var form = new fhModule.Form(self.SoftwareLicence, function (version) {

                                    if (!version)
                                        return;

                                    forceClose = true;

                                    self.SoftModelObject(new module.SoftwareModel(null, version, self.SoftwareLicence.SoftwareModelName));
                                    
                                    if (self.onSaveCallback) {
                                        self.onSaveCallback(self.SoftModelObject());
                                    }

                                });
                                form.Show();
                            });
                            if (ctrl) {
                                ctrl.Close();
                            }
                        };
                    }

                    buttons[getTextResource('ButtonCancel')] = function () {
                        forceClose = false;
                        ctrl.Close();
                    };

                    var ctrl = undefined;
                    ctrl = new formControl.control(
                        'frmSoftwareTableSelector',//form region prefix
                        'frmSoftwareTableSelector_setting',//location and size setting
                        self.Name,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        900, 400,//minSize
                        buttons,//form buttons
                        function () {
                            self.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/templateParamsUpdate/frmSoftwareTableSelectorModel\'}"'//attributes of form region
                    );
                    if (!ctrl.Initialized)
                        return;

                    self.SoftModelObject.subscribe(function (newValue) {
                        var newButtons = {}
                        if (showCreateModelButton) {
                            newButtons[getTextResource('AddSoftwareModel')] = function () {
                                require(['ui_forms/Asset/AssetOperations/templateParamsUpdate/frmSoftwareModelAdd'], function (fhModule) {
                                    var form = new fhModule.Form(self.SoftwareLicence, function (version) {

                                        if (!version)
                                            return;

                                        forceClose = true;

                                        self.SoftModelObject(new module.SoftwareModel(null, version, self.SoftwareLicence.SoftwareModelName));

                                        if (self.onSaveCallback) {
                                            self.onSaveCallback(self.SoftModelObject());
                                        }

                                    });
                                    form.Show();
                                });
                                if (ctrl) {
                                    ctrl.Close();
                                }
                            };
                        }
                        
                        if (newValue != null) {
                            newButtons[getTextResource('Select')] = function () {
                                if (self.onSaveCallback) {
                                    self.onSaveCallback(self.SoftModelObject());
                                }
                                forceClose = false;
                                ctrl.Close();

                            };

                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                ctrl.Close();
                            };
                        }
                        else {
                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = false;
                                ctrl.Close();
                            };
                        }

                        if (!forceClose && ctrl) {                         
                            ctrl.UpdateButtons(newButtons);
                        }
                    });

                    ctrl.Show();

                    bindElement = document.getElementById(ctrl.GetRegionID());
                    ko.applyBindings(self, bindElement);

                    hideSpinner();
                }
            },
            SoftwareModel: function (id, version, commercialModelName, manufacturerID, manufacturerName) {
                const self = this;
                self.ID = id;
                self.Version = version;
                self.Name = commercialModelName;
                self.ManufacturerID = manufacturerID;
                self.ManufacturerName = manufacturerName;
            }
        };
        return module;
    });