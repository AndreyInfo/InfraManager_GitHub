define(['knockout', 'jquery', 'ajax', 'formControl', './../../Asset/Controls/SoftwareLicenceList'],
    function (ko, $, ajax, formControl, softwareLicenceList) {
        var module = {
            Form: function (FilterProductCatalogTypeID, softwareModel, SoftwareLicenceSchemeID, ManufacturerName, LicenceType, onSaveCallback) {
                var self = this;
                self.ajaxControl = new ajax.control();
                //                
                self.FilterProductCatalogTypeID = FilterProductCatalogTypeID;
                self.softwareModel = softwareModel;
                self.SoftwareLicenceSchemeID = SoftwareLicenceSchemeID;
                self.ManufacturerName = ManufacturerName;
                self.LicenceType = LicenceType;
                self.list = new softwareLicenceList.List(self);
                //
                self.onSaveCallback = onSaveCallback;
                self.selectedItems = ko.observableArray([]);
                //
                //when tab unload
                self.dispose = function () {
                    self.list.dispose();
                    self.ajaxControl.Abort();
                };
                {//search
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
                }
                //
                self.searchPhraseObservable = ko.observable('');//set in ActivesLocatedLink.js
                self.Search = function () {
                    self.selectedItems(null);
                    self.searchPhraseObservable(self.SearchText());
                    self.list.listView.load();
                };
                self.list.SelectedItemsChanged = function (checkedItemsCount) {
                
                }
                //
                self.list.listViewRowClick = function (obj) {
                    self.selectedItems([]);
                    self.selectedItems(obj);
                };
                
                self.Name = getTextResource('SoftwareLicenseView'); 
                
                //show form
                self.Show = function () {
                    showSpinner();
                    //
                    var buttons = [];                                       
                  
                    var bCancel = {
                        text: getTextResource('Close'),
                        click: function () {
                            ctrl.Close();
                        }
                    }

                    let forceClose = false;
                    
                    buttons.push(bCancel);

                    var ctrl = undefined;
                    ctrl = new formControl.control(
                        'frmSoftwareTableSelector',//form region prefix
                        'frmSoftwareTableSelector_setting',//location and size setting
                        self.Name,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        700, 400,//minSize
                        buttons,//form buttons
                        function () {
                            self.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmSoftwareTableSelector\'}"'//attributes of form region
                    );
                    if (!ctrl.Initialized)
                        return;
                    ctrl.Show();

                    self.selectedItems.subscribe(function (newValue) {
                        var newButtons = {}
                        if (newValue != null) {

                            newButtons[getTextResource('Select')] = function () {                               
                                if (self.onSaveCallback)
                                    self.onSaveCallback(self.selectedItems());                                   
                                forceClose = true;
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
                        if (!forceClose)
                            ctrl.UpdateButtons(newButtons);
                    });
                    
                    bindElement = document.getElementById(ctrl.GetRegionID());
                    ko.applyBindings(self, bindElement);

                    hideSpinner();
                }
            }
        };
        return module;
    });