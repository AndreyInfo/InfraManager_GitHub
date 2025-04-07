define(['knockout', 'jquery', 'ajax', 'ui_controls/ListView/ko.ListView.Helpers'], function (ko, $, ajax, m_helpers) {
    var module = {
        Tab: function (vm) {

            var self = this;

            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('SoftwareLicenceScheme_Coefficients');

            self.Template = '../UI/Forms/Settings/LicenceSchemes/frmLicenceScheme_coeffTab';

            self.IconCSS = 'coeffTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.canEdit = vm.CanEdit;//for userlib
            //
            self.$region = vm.$region;
            //
            self.isLoaded = false;

            //when object changed
            self.Initialize = function (obj) {
            };
            //when tab selected
            self.load = function () {
                if (self.isLoaded === true)
                    return;
                self.isLoaded = true;
            };

            self.AfterRender = function () {
            }

            //when tab unload
            self.dispose = function () {
                self.isLoaded = false;
                self.ajaxControl.Abort();
            };

            self.coefficientsView = null;
            self.coefficientsViewID = 'coefficientsView_' + ko.getNewID();

            self.coefficientsViewInit = function (listView) {
                if (self.coefficientsView != null)
                    throw 'listView inited already';
                //
                self.coefficientsView = listView;
                self.coefficientsView.ignoreTableHeight = true;
                m_helpers.init(self, listView);//extend self        
                //
                self.coefficientsView.load();
            };
            //
            self.getProcessorObjectList = function (idArray) {
                var retvalD = $.Deferred();

                var requestInfo = {
                    IDList: idArray ? idArray : [],
                    ViewName: 'ProcessorModelSimple',
                    TemplateID: 331,
                };

                self.ajaxControl.Ajax(null,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: requestInfo,
                        url: '/licence-scheme/processors-for-select'
                    },
                    function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                            return;
                        }
                        else if (newVal && newVal.Result === 1) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                            });
                        }
                        else if (newVal && newVal.Result === 2) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                            });
                        }
                        else if (newVal && newVal.Result === 3) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('AccessError_Table'));
                            });
                        }
                        else if (newVal && newVal.Result === 7) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('OperationError_Table'));
                            });
                        }
                        else if (newVal && newVal.Result === 9) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                            });
                        }
                        else if (newVal && newVal.Result === 11) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SqlTimeout'));
                            });
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js getData]', 'error');
                            });
                        }
                        //
                        retvalD.resolve([]);
                    },
                    function (XMLHttpRequest, textStatus, errorThrown) {
                        if (showErrors === true)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js, getData]', 'error');
                            });
                        //
                        retvalD.resolve([]);
                    },
                    null
                );
                return retvalD.promise();
            }
            //
            self.coefficientsRetrieveItems = function () {
                var retvalD = $.Deferred();
                var objectCoeffs = vm.object().Coefficients();
                var idArr = [];
                if (objectCoeffs) {
                    ko.utils.arrayForEach(objectCoeffs, function (line) {
                        idArr.push(line.ProcessorTypeID);
                    });
                }
                if (idArr.length > 0) {
                    $.when(self.getProcessorObjectList(idArr)).done(function (objectList) {
                        if (objectList) {

                            var resultArr = [];
                            ko.utils.arrayForEach(objectList, function (row) {
                                var coeff = null;
                                ko.utils.arrayForEach(objectCoeffs, function (objCoeff) {
                                    if (objCoeff.ProcessorTypeID == row.ID)
                                        coeff = { Name: row.Name, Coefficient: objCoeff.Coefficient, ID: objCoeff.ProcessorTypeID };
                                });
                                if (coeff)
                                    resultArr.push(coeff);
                            });
                            self.coefficientsView.showAllRows();
                            retvalD.resolve(resultArr);
                        }
                        //
                    });
                }
                else
                    retvalD.resolve([]);
                return retvalD.promise();
            };
            //
            self.getCheckedItems = function () {
                var selectedItems = self.coefficientsView.rowViewModel.checkedItems();
                //
                if (!selectedItems)
                    return [];
                //
                var retval = [];
                selectedItems.forEach(function (el) {
                    var item = { ID: el.ID, Name: el.Name, Coefficient: el.Coefficient };
                    retval.push(item);
                });
                return retval;
            };

            //  меню
            {
                self.coefficientsContextMenu = ko.observable(null);
                self.contextMenuInit = function (contextMenu) {
                    self.coefficientsContextMenu(contextMenu);//bind contextMenu

                    self.coefficientsContextMenuAdd(contextMenu);
                    self.coefficientsContextMenuDelete(contextMenu);
                    self.coefficientsContextMenuEdit(contextMenu);
                };
                //
                self.contextMenuOpening = function (contextMenu) {
                    contextMenu.items().forEach(function (item) {
                        if (item.isEnable() && item.isVisible()) {
                            item.enabled(item.isEnable());
                            item.visible(item.isVisible());
                        }
                        else {
                            item.visible(false);
                        }
                    });
                };
                // add 
                self.coefficientsContextMenuAdd = function (contextMenu) {
                    var isVisible = function () {
                        return vm.CanEdit() && vm.object() != null;
                    };
                    var action = function () {
                        self.ViewSoftwareLicenceSchemeCoeffAdd();
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicenceScheme_Coefficients_MenuAdd');
                    cmd.isEnable = function () { return true; };
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                // Delete
                self.coefficientsContextMenuDelete = function (contextMenu) {
                    var isEnable = function () {
                        return self.getCheckedItems().length >= 1; //self.operationIsGranted(461);
                    };
                    var isVisible = function () {
                        return vm.CanEdit() && vm.object() != null;
                    };
                    var action = function () {
                        self.ViewSoftwareLicenceSchemeCoeffDelete();
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicenceScheme_Coefficients_MenuDelete');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                // Edit
                self.coefficientsContextMenuEdit = function (contextMenu) {
                    var isEnable = function () {
                        return self.getCheckedItems().length >= 1; //self.operationIsGranted(461);
                    };
                    var isVisible = function () {
                        return vm.CanEdit() && vm.object() != null;
                    };
                    var action = function () {
                        self.ViewSoftwareLicenceSchemeCoeffEdit();
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicenceScheme_Coefficients_MenuEdit');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //  implement addining 
                self.ViewSoftwareLicenceSchemeCoeffAdd = function () {
                    if (!vm.CanEdit())
                        return;
                    //
                    showSpinner();
                    //
                    require(['assetForms', 'sweetAlert'], function (module, swal) {
                        var fh = new module.formHelper(true);
                        fh.ShowAssetModelSelect({
                            TemplateID: 331,
                            ID: null,
                            ServiceID: null,
                            ClientID: null,
                            ShowWrittenOff: false,
                            Caption: getTextResource('SoftwareLicenceScheme_Coefficients_Add'),
                            SelectOnlyOne: false,
                            productCatalogueItemInfo: {
                                TemplateID: 331,
                                listName: 'ProcessorModelSimple',
                                url: '/licence-scheme/processors-for-select',
                                showContractTab: false,
                            }
                        }, function (objList) {
                            if (!objList || objList.length == 0)
                                return;
                            var object = vm.object();
                            var currList = object.Coefficients();
                            var oldValue = JSON.stringify(currList);
                            var newList = [];
                            if (currList) {
                                ko.utils.arrayForEach(currList, function (oldVal) {
                                    var newItem = { ProcessorTypeID: oldVal.ProcessorTypeID, Coefficient: oldVal.Coefficient };
                                    newList.push(newItem);
                                });
                            }
                            ko.utils.arrayForEach(objList, function (newType) {
                                var same = false;
                                ko.utils.arrayForEach(newList, function (oldType) {
                                    if (oldType.ProcessorTypeID == newType.ID)
                                        same = true;
                                });
                                if (!same) {
                                    var newItem = { ProcessorTypeID: newType.ID, Coefficient: 1 };
                                    newList.push(newItem);
                                }
                            });
                            self.saveList(newList, oldValue, object);
                        });
                    });
                };
                // implement remove
                self.ViewSoftwareLicenceSchemeCoeffDelete = function () {
                    var selectedItems = self.getCheckedItems();
                    if (selectedItems.length <= 0)
                        return;
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('Removing'),
                            text: getTextResource('ConfirmRemoveQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                            function (value) {
                                swal.close();
                                //
                                if (value == true) {
                                    var object = vm.object();
                                    var currList = object.Coefficients();
                                    var newList = [];
                                    var oldValue = JSON.stringify(currList);
                                        ko.utils.arrayForEach(currList, function (item) {
                                            var need = true;
                                            ko.utils.arrayForEach(selectedItems, function (todel) {
                                                if (item && item.ProcessorTypeID == todel.ID)
                                                    need = false;
                                            });
                                            if (need)
                                                newList.push(item);
                                        });
                                    self.saveList(newList, oldValue, object);
                                }
                            });
                    });

                };
                // implement edit
                self.ViewSoftwareLicenceSchemeCoeffEdit = function () {
                    var selectedItems = self.getCheckedItems();
                    if (selectedItems.length != 1)
                        return;
                    require(['../UI/Forms/Settings/LicenceSchemes/frmLicenceScheme_editCoeff'], function (module) {
                        var edit = new module.coefficientEditor();
                        edit.ShowEdit({ ProcessorTypeID: selectedItems[0].ID, ProcessorTypeName: selectedItems[0].Name, Coefficient: selectedItems[0].Coefficient, vmTab: self });
                    });
                };

                self.CoeffDeleteForEdit = function (typeID) {
                    var object = vm.object();
                    var currList = object.Coefficients();
                    var newList = [];
                    var oldValue = JSON.stringify(currList);
                    ko.utils.arrayForEach(currList, function (item) {
                        if (!item || item.ProcessorTypeID != typeID)
                            newList.push(item);
                    });
                    self.saveList(newList, oldValue, object);
                };
                self.CoeffReadyForEdit = function (typeID, oldTypeID, coefficient) {
                    var object = vm.object();
                    var currList = object.Coefficients();
                    var newList = [];
                    var oldValue = JSON.stringify(currList);
                    var isDublicate = false;
                    var isExists = false;
                    ko.utils.arrayForEach(currList, function (item) {
                        if (item && item.ProcessorTypeID == oldTypeID) {
                            newList.push({ ProcessorTypeID: typeID, Coefficient: coefficient });
                            isExists = true;
                        }
                        else if (item && item.ProcessorTypeID == typeID)
                            isDublicate = true;
                        else
                            newList.push(item);
                    });
                    if (isDublicate) {
                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('SaveError'),
                                text: getTextResource('SoftwareLicenceScheme_DublicateCoefficient'),
                                showCancelButton: false,
                                closeOnConfirm: true,
                                closeOnCancel: true,
                                confirmButtonText: getTextResource('ButtonOK'),
                                cancelButtonText: getTextResource('ButtonCancel')
                            });
                        });
                    }
                    else {
                        if (!isExists)
                            newList.push({ ProcessorTypeID: typeID, Coefficient: coefficient });
                        self.saveList(newList, oldValue, object);
                    }
                };

            }
            // save new List
            self.saveList = function (currList, oldValue, object) {
                if (!vm.isAddNewLicenceScheme()) {
                    var data = {
                        ID: object.ID(),
                        ObjClassID: object.ClassID(),
                        ClassID: null,
                        ObjectList: null,
                        Field: 'SoftwareLicenceSchemeCoefficients',
                        NewValue: JSON.stringify(currList),
                        OldValue: oldValue,
                        Params: '',
                        ReplaceAnyway: false,
                    };
                    //
                    self.ajaxControl.Ajax(
                        null,
                        {
                            dataType: "json",
                            method: 'PUT',
                            url: vm.baseUrl + '/' + object.ID() + '/SoftwareLicenceSchemeCoefficients',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                if (retModel.IsSuccess) {
                                    object.Coefficients(currList);
                                    $.when(self.coefficientsView.load()).done(function () {
                                        $('.coefficients .tableData').css('height', '100%');
                                    });
                                    return;
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource(retModel.MessageKey), 'error');
                                    });
                                }
                            }
                            else
                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                        },
                        function (errResult) {
                            swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                        }
                    );

                }
                else {
                    object.Coefficients(currList);
                    $.when(self.coefficientsView.load()).done(function () {
                        $('.coefficients .tableData').css('height', '100%');
                    });
                }


            }
        }
    };
    return module;
});