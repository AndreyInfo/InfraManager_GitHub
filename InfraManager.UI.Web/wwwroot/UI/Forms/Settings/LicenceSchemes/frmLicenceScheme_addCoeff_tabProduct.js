define(['knockout', 'jquery', 'ajax', 'treeControl', 'ui_controls/ListView/ko.ListView.Cells'], function (ko, $, ajaxLib, treeLib, mCells) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajaxLib.control();
            self.Template = '../UI/Forms/Settings/LicenceSchemes/frmLicenceScheme_addCoeff_tabProduct';
            self.$region = vm.$region;
            //self.callbackFunc = null;

            {//fields
                self.isLoaded = false;
            }

            //when object changed
            self.Initialize = function (obj) {
                self.isLoaded = false;
            };
            //when tab selected
            self.load = function () {
                if (self.isLoaded === true)
                    return;
                //
                self.isLoaded = true;

                self.InitProductCatalogueTree = function () {
                    vm.productCatalogueControl(new treeLib.control());
                    vm.productCatalogueControl().init($('#regionSoftCatalogue2'), 2, {
                        onClick: vm.navigator_nodeSelected,
                        ShowCheckboxes: false,
                        AvailableClassArray: [374, 378],
                        ClickableClassArray: [374, 378],
                        AllClickable: true,
                        FinishClassArray: [378],
                        HasLifeCycle: false,
                        ExpandFirstNodes: false,
                        AvailableTemplateID: 331 
                    });
                    //
                    $.when(vm.productCatalogueControl().$isLoaded).done(function () {

                    });
                };

                self.lv = null;
                self.listViewID = 'listView_' + ko.getNewID();
                self.lv_checkedItemsBeforeChanged_handle = null;
                self.lv_checkedItemsChanged_handle = null;
                self.selectedItemID = null;
                //
                self.lvInit = function (listView) {
                    self.lv = listView;
                    self.lv_checkedItemsChanged_handle = listView.rowViewModel.checkedItemsToSubscribe.subscribe(function (newObjectList) {
                        //
                        if (self.selectedItemFreeze)
                            return;
                        var buttons = [];
                        var bCancel = {
                            text: getTextResource('ButtonCancel'),
                            click: function () {
                                self.selectedItemFreeze = true;
                                vm.frm.Close();
                            }
                        };
                        buttons.push(bCancel);
                        //
                        if (newObjectList && newObjectList.length == 1) {
                            self.selectedItemID = newObjectList[0].ID;
                            var bReady = {
                                text: getTextResource('LicenceSchemeCoeffEdit_ReadyButton'),
                                click: function () {
                                    if (vm.callbackFunc) {
                                        var result = self.lv.rowViewModel.getRowByObjectID(self.selectedItemID);
                                        if (result && result.object) {
                                            var coeff = result.object.CoefficientValue;
                                            ko.utils.arrayForEach(result.cells(), function (cell) {
                                                if (cell.column.MemberName == 'CoefficientValue') {
                                                    coeff = cell.value();
                                                }
                                            });
                                            vm.callbackFunc({ ID: result.object.ID, Name: result.object.Name, Coefficient: coeff });
                                        }
                                    }
                                    //
                                    self.selectedItemFreeze = true;
                                    vm.frm.Close();
                                }
                            };
                            buttons.push(bReady);
                        } else {
                            self.selectedItem = null;
                        }
                        vm.frm.UpdateButtons(buttons);
                    });
                    //
                    var storedLoad = self.lv.load;
                    self.lv.load = function () {
                        var retvalD = $.Deferred();
                        self.selectedItemFreeze = true;
                        $.when(storedLoad()).done(function () {
                            self.selectedItemFreeze = false;
                            retvalD.resolve();
                        });
                        return retvalD.promise();
                    };
                    //
                    self.lv.load();
                };
                self.lvRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
                    var retvalD = $.Deferred();
                    $.when(vm.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                        retvalD.resolve(objectList);
                        self.lv.showAllRows();
                    });
                    return retvalD.promise();
                };
                self.lvRowClick = function (obj) {

                };
                self.listViewDrawCell = function (obj, column, cell) {
                    if (column.IsEdit && column.MemberName == 'CoefficientValue') {
                        column.Template("../UI/Forms/Asset/AssetOperations/CellTemplates/sdNumberEditor");
                        cell.value(obj[column.MemberName]);
                    }
                    else {
                        cell.text = mCells.textRepresenter(obj, column);
                    }
                };
            };

            self.AfterRender = function () {
                self.InitProductCatalogueTree();
                vm.navigatorObjectID(null);
                vm.navigatorObjectClassID(null);
                vm.treeMode(0);
            }

            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
                self.isLoaded = false;
                if (self.lv != null)
                    self.lv.dispose();
            };
        }
    }
    return module;
});