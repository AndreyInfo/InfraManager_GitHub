define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu',],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            List: function (vm, selectedList, addInventorySpecification) {
                var self = this;
                //
                vm.object = vm.workOrder;
                self.searchPhraseObservable = ko.observable('');//set in InventorySpecificationControl.js
                self.ShowAccepted = ko.observable(false);//set in InventorySpecificationControl.js
                self.StreamSelection;//set in InventorySpecificationControl.js
                self.SearchText;//set in InventorySpecificationControl.js
                selectedList.baseList = self;
                //
                self.ajaxControl = new ajaxLib.control();
                self.subscriptionList = [];
                //
                //when object changed
                self.init = function (obj) {
                };
                //when tab selected
                self.load = function () {
                };
                //when tab unload
                self.dispose = function () {
                    $(document).unbind('objectInserted', self.onObjectInserted);
                    $(document).unbind('local_objectInserted', self.onObjectInserted);
                    $(document).unbind('objectUpdated', self.onObjectModified);
                    $(document).unbind('local_objectUpdated', self.onObjectModified);
                    $(document).unbind('objectDeleted', self.onObjectDeleted);
                    $(document).unbind('local_objectDeleted', self.onObjectDeleted);
                    //
                    self.ajaxControl.Abort();
                    //
                    for (var i in self.subscriptionList) {
                        self.subscriptionList[i].dispose();
                    }
                    //
                    if (self.listViewContextMenu() != null)
                        self.listViewContextMenu().dispose();
                    if (self.listView != null)
                        self.listView.dispose();
                };
                //

                //
                {//events of listView
                    self.listView = null;
                    self.listViewID = 'listView_' + ko.getNewID();//bind same listView to another template instance (when tab changed), without reload list
                    //
                    self.listViewInit = function (listView) {
                        if (self.listView != null)
                            throw 'listView inited already';
                        //
                        self.listView = listView;
                        m_helpers.init(self, listView);//extend self        
                        //
                        self.listView.load();
                        //
                        var subscription = self.listView.rowViewModel.rowChecked.subscribe(function (row) {
                            var obj = row.object;
                            if (row.checked()) {
                                var exists = ko.utils.arrayFirst(selectedList.list(), function (el) {
                                    return el.ID.toUpperCase() == obj.ID.toUpperCase();
                                });
                                if (!exists) {
                                    selectedList.list.push(obj);
                                    selectedList.listView.rowViewModel.rowList.push(selectedList.listView.rowViewModel.createRow(obj));
                                    selectedList.listView.waitAndRenderTable();
                                }
                            }
                            else {
                                for (var i = 0; i < selectedList.list().length; i++) {
                                    var _obj = selectedList.list()[i];
                                    if (_obj.ID.toUpperCase() == obj.ID.toUpperCase()) {
                                        selectedList.list.remove(_obj);
                                    }
                                }
                                //selectedList.list.remove(obj);
                                selectedList.removeRowByID(obj.ID);
                            }
                        });
                        self.subscriptionList.push(subscription);
                        //
                        subscription = self.listView.rowViewModel.allItemsChecked.subscribe(function (allItemsChecked) {
                            var checkedItems = self.listView.rowViewModel.checkedItems;
                            var selectedListVM = selectedList.listView.rowViewModel;
                            //
                            if (allItemsChecked) {
                                checkedItems().forEach(function (obj) {
                                    var exists = ko.utils.arrayFirst(selectedList.list(), function (el) {
                                        return el.ID.toUpperCase() == obj.ID.toUpperCase();
                                    });
                                    if (!exists) {
                                        selectedList.list.push(obj);
                                        selectedListVM.rowList.push(selectedListVM.createRow(obj));
                                    }
                                });
                                selectedList.listView.waitAndRenderTable();
                            }
                            else {
                                if (checkedItems().length !== 0)
                                    return;
                                //
                                self.listView.rowViewModel.rowList().forEach(function (row) {
                                    var exists = ko.utils.arrayFirst(selectedList.list(), function (el) {
                                        return el.ID.toUpperCase() == row.object.ID.toUpperCase();
                                    });
                                    if (exists) {
                                        for (var i = 0; i < selectedList.list().length; i++) {
                                            var _obj = selectedList.list()[i];
                                            if (_obj.ID.toUpperCase() == row.object.ID.toUpperCase()) {
                                                selectedList.list.remove(_obj);
                                            }
                                        }
                                        //
                                        selectedList.removeRowByID(row.object.ID);
                                    }
                                });
                            }
                        });
                        self.subscriptionList.push(subscription);
                    };
                    //
                    self.listViewRetrieveItems = function () {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(null, true)).done(function (objectList) {
                            if (objectList)
                                self.clearAllInfos();
                            //
                            retvalD.resolve(objectList);
                        });
                        return retvalD.promise();
                    };
                    self.listViewRowClick = function (obj) {
                        var id = self.getObjectID(obj);
                        //
                        var row = self.getRowByID(id);
                        if (row != null)
                            self.setRowAsLoaded(row);
                        //
                        self.showSpecificationForm(id);
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };
                    self.getObjectClassID = function (obj) {
                        return 392;//OBJ_InventorySpecification
                    };
                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 392;//OBJ_InventorySpecification
                    };
                }
                //
                {//contextMenu
                    {//granted operations
                        self.grantedOperations = [];
                        self.UserIsAdmin = false;
                        $.when(userD).done(function (user) {
                            self.UserIsAdmin = user.HasAdminRole;
                            self.grantedOperations = user.GrantedOperations;
                        });
                        self.operationIsGranted = function (operationID) {
                            for (var i = 0; i < self.grantedOperations.length; i++)
                                if (self.grantedOperations[i] === operationID)
                                    return true;
                            return false;
                        };
                        //
                        self.needUpdateDisabledOperationsWF = ko.observable(true);
                        self.DisabledOperationsWF = [];//операции, выключенные через воркфлоу 
                        //
                        self.ajaxControl_DisabledOperationsWF = new ajaxLib.control();
                        self.CheckDisabledOperationsWF = function () {
                            if (!self.needUpdateDisabledOperationsWF())
                                return;
                            //
                            self.ajaxControl_DisabledOperationsWF.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    url: '/assetApi/GetDisabledOperationsWF'
                                },
                                function (result) {
                                    if (result.Result === 0) {
                                        self.DisabledOperationsWF = result.Retval;
                                        //
                                        self.needUpdateDisabledOperationsWF(false);
                                    }
                                });
                        };
                        //
                        self.operationIsDisabled = function (operationID) {
                            for (var i = 0; i < self.DisabledOperationsWF.length; i++)
                                if (self.DisabledOperationsWF[i] === operationID)
                                    return true;
                            return false;
                        };
                        //
                        self.AddSpecificationEnabled = ko.computed(function () {
                            self.CheckDisabledOperationsWF();
                            return !self.operationIsDisabled(1);//InvAdd = 1
                        });
                    }
                    //
                    self.listViewContextMenu = ko.observable(null);
                    //
                    self.contextMenuInit = function (contextMenu) {
                        self.listViewContextMenu(contextMenu);//bind contextMenu
                        //
                        self.properties(contextMenu);
                        contextMenu.addSeparator();
                        self.assetProperties(contextMenu);
                        contextMenu.addSeparator();
                        self.add(contextMenu);
                        self.remove(contextMenu);
                        contextMenu.addSeparator();
                        self.confirm(contextMenu);
                        self.ignore(contextMenu);
                        self.updateDB(contextMenu);
                        contextMenu.addSeparator();
                    };
                    //

                    self.wait = function (contextMenu) {
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Loading');
                        cmd.enabled(false);
                        //
                        cmd.isDynamic = true;
                        return cmd;
                    };

                    self.contextMenuOpening = function (contextMenu) {
                        //
                        self.CheckDisabledOperationsWF();
                        //
                        var oldDynamicItems = [];
                        contextMenu.items().forEach(function (item) {
                            if (item.isDynamic)
                                oldDynamicItems.push(item);
                            else if (item.isEnable && item.isVisible) {
                                item.enabled(item.isEnable());
                                item.visible(item.isVisible());
                            }
                        });
                        for (var i = 0; i < oldDynamicItems.length; i++)
                            contextMenu.removeItem(oldDynamicItems[i]);
                        //
                        var waitItem = self.wait(contextMenu);
                        $.when(self.fillDynamicItems(contextMenu)).done(function () {
                            contextMenu.removeItem(waitItem);
                            if (contextMenu.visibleItems().length == 0)
                                contextMenu.close();
                        });
                    };

                    self.getSelectedLifeCycleObjects = function (getSelectedItems) {
                        if (!getSelectedItems)
                            getSelectedItems = self.getSelectedItems;
                        //
                        var list = [];
                        var selectedItems = getSelectedItems();
                        selectedItems.forEach(function (el) {
                            list.push(new module.LifeCycleObject(el.ObjectClassID, el.ObjectID, el.ObjectName, el.LifeCycleStateID));
                        });
                        return list;
                    };

                    self.fillDynamicItems = function (contextMenu, getSelectedItems) {
                        if (!getSelectedItems)
                            getSelectedItems = self.getSelectedItems;
                        //
                        var retval = $.Deferred();
                        if (self.operationIsDisabled(8))//InvRunAction = 8
                        {
                            retval.resolve();
                            return retval.promise();
                        }
                        //
                        var lifeCycleObjectList = self.getSelectedLifeCycleObjects(getSelectedItems);
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: { DeviceList: lifeCycleObjectList },
                                url: '/sdApi/GetContextMenu'
                            },
                            function (newVal) {
                                if (newVal && newVal.List) {
                                    require(['assetOperationsHelper'], function (module) {
                                        newVal.List.forEach(function (lifeCycleOperation) {
                                            var cmd = contextMenu.addContextMenuItem();
                                            cmd.enabled(lifeCycleOperation.Enabled);
                                            cmd.text(lifeCycleOperation.Name);
                                            cmd.click(function () {
                                                $.when(module.executeLifeCycleOperation(lifeCycleOperation, lifeCycleObjectList)).done(function () {
                                                    var list = getSelectedItems();
                                                    if (list.length == 0)
                                                        return;
                                                    //
                                                    var data = {
                                                        'InventorySpecificationList': self.getItemInfos(list)
                                                    };
                                                    //
                                                    self.ajaxControl.Ajax(null,
                                                        {
                                                            dataType: "json",
                                                            method: 'POST',
                                                            data: data,
                                                            url: '/assetApi/InventoryOperationExecuted'
                                                        },
                                                        function (newVal) {
                                                        });
                                                })
                                            });
                                            //
                                            cmd.isDynamic = true;
                                        });
                                    });
                                }
                                retval.resolve();
                            });
                        //
                        return retval.promise();
                    };

                }
                //
                {//geting data             
                    self.loadObjectListByIDs = function (idArray, unshiftMode) {
                        for (var i = 0; i < idArray.length; i++)
                            idArray[i] = idArray[i].toUpperCase();
                        //
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(idArray, false)).done(function (objectList) {
                                if (objectList) {
                                    var rows = self.appendObjectList(objectList, unshiftMode);
                                    rows.forEach(function (row) {
                                        self.setRowAsNewer(row);
                                        //
                                        var obj = row.object;
                                        var id = self.getObjectID(obj);
                                        self.clearInfoByObject(id);
                                        //
                                        var index = idArray.indexOf(id);
                                        if (index != -1)
                                            idArray.splice(index, 1);
                                    });
                                }
                                idArray.forEach(function (id) {
                                    self.removeRowByID(id);
                                    self.clearInfoByObject(id);
                                });
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };
                    self.getObjectListByIDs = function (idArray, unshift) {
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(idArray, false)).done(function (objectList) {
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };
                    //
                    self.ajaxControl = new ajaxLib.control();
                    self.isAjaxActive = function () {
                        return self.ajaxControl.IsAcitve() == true;
                    };
                    //
                    self.getObjectList = function (idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var requestInfo = {
                            IDList: idArray ? idArray : [],
                            ViewName: 'InventorySpecification',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.object().ID(),
                            SearchRequest: self.searchPhraseObservable ? self.searchPhraseObservable() : null,
                            ShowAccepted: self.ShowAccepted()
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetInventorySpecificationObject'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request     
                                    //
                                    if (self.StreamSelection() && self.SearchText()) {
                                        newVal.Data.forEach(function (obj) {
                                            self.addRowToSelected(obj);
                                        });
                                        //
                                        self.SearchText('');
                                    }
                                    //
                                    if (newVal.Data && newVal.Data.length != 0) {
                                        newVal.Data.forEach(function (obj) {
                                            self.modifyRowToSelected(obj);
                                        });
                                    }
                                    else if (idArray) {
                                        idArray.forEach(function (id) {
                                            var obj = { ID: id };
                                            self.removeRowFromSelected(obj);
                                        });
                                    }
                                }
                                else if (newVal && newVal.Result === 1 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                                    });
                                }
                                else if (newVal && newVal.Result === 2 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                                    });
                                }
                                else if (newVal && newVal.Result === 3 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('AccessError_Table'));
                                    });
                                }
                                else if (newVal && newVal.Result === 7 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('OperationError_Table'));
                                    });
                                }
                                else if (newVal && newVal.Result === 9 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                                    });
                                }
                                else if (newVal && newVal.Result === 11 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SqlTimeout'));
                                    });
                                }
                                else if (showErrors === true) {
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
                        //
                        return retvalD.promise();
                    };
                }
                //
                {//helper methods                               
                    self.getItemName = function (item) {
                        return getTextResource('InventorySpecification') + ' ' + item.OrderNumber;
                    };
                    //
                    self.getSelectedItems = function () {
                        var selectedItems = self.listView.rowViewModel.checkedItems();
                        //
                        if (!selectedItems)
                            return [];
                        //
                        var retval = [];
                        selectedItems.forEach(function (el) {
                            var item =
                            {
                                ID: el.ID.toUpperCase(),
                                ClassID: 392,//OBJ_InventorySpecification
                                ObjectID: el.ObjectID,
                                ObjectClassID: el.ObjectClassID,
                                Name: el.OrderNumber + ':  \'' + el.ProductCatalogTypeName + ' ' + el.ProductCatalogModelName + '\'',
                                LifeCycleStateID: el.LifeCycleStateID,
                                OrderNumber: el.OrderNumber,

                            };
                            retval.push(item);
                        });
                        return retval;
                    };

                    self.clearSelection = function () {
                        self.listView.rowViewModel.checkedItems([]);
                    };
                    self.getConcatedItemNames = function (items) {
                        var retval = '';
                        items.forEach(function (item) {
                            if (retval.length < 200) {
                                retval += (retval.length > 0 ? ', ' : '') + self.getItemName(item);
                                if (retval.length >= 200)
                                    retval += '...';
                            }
                        });
                        return retval;
                    };
                    self.getItemInfos = function (items) {
                        var retval = [];
                        items.forEach(function (item) {
                            retval.push({
                                ClassID: self.getObjectClassID(item),
                                ID: self.getObjectID(item),
                                ObjectID: item.ObjectID,
                                ObjectClassID: item.ObjectClassID
                            });
                        });
                        return retval;
                    };
                }
            //
                {//menu operations
                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            return !self.operationIsDisabled(1);//InvAdd = 1
                        };
                        var isVisible = function () {
                            return vm.CanEdit() && vm.object() != null && self.operationIsGranted(897);//OPERATION_InventorySpecification_Add = 897
                        };
                        var action = function () {
                            addInventorySpecification();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.properties = function (contextMenu, getSelectedItems) {
                        if (!getSelectedItems)
                            getSelectedItems = self.getSelectedItems;
                        //
                        var isEnable = function () {
                            return getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(896);//OPERATION_InventorySpecification_Properties = 896
                        };
                        var action = function () {
                            if (getSelectedItems().length != 1)
                                return false;
                            //
                            var selected = getSelectedItems()[0];
                            var id = self.getObjectID(selected);
                            //     
                            self.showSpecificationForm(id);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Properties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.assetProperties = function (contextMenu, getSelectedItems) {
                        if (!getSelectedItems)
                            getSelectedItems = self.getSelectedItems;
                        //
                        var isEnable = function () {
                            return getSelectedItems().length === 1 && !self.operationIsDisabled(3);//InvEditSource = 3
                        };
                        var isVisible = function () {
                            return true;
                        };
                        var action = function () {
                            if (getSelectedItems().length != 1)
                                return false;
                            //
                            var selected = getSelectedItems()[0];
                            //     
                            self.showAssetForm(selected.ObjectID, selected.ObjectClassID);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('PropertiesObject');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.remove = function (contextMenu, getSelectedItems) {
                        var isSelectedList = null;
                        if (isSelectedList === null)
                            isSelectedList = getSelectedItems ? true : false;
                        //
                        var isEnable = function () {
                            var list = isSelectedList ?
                                selectedList.listView.rowViewModel.checkedItems() :
                                self.listView.rowViewModel.checkedItems();
                            //
                            var existsWithLicence = false;
                            for (var i = 0; i < list.length; i++)
                                if (list[i].SoftwareLicenceID != null) {
                                    existsWithLicence = true;
                                    break;
                                }
                            //
                            return list.length > 0 && existsWithLicence === false && (isSelectedList || !self.operationIsDisabled(5));// InvDelete = 5
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(898) && vm.CanEdit();//OPERATION_InventorySpecification_Delete = 898
                        };
                        var action = function () {
                            if (!getSelectedItems)
                                getSelectedItems = self.getSelectedItems;
                            //
                            var list = getSelectedItems();
                            if (list.length == 0)
                                return;
                            //     
                            var question = self.getConcatedItemNames(list);
                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('Removing') + ': ' + question,
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
                                            var data = {
                                                'ObjectList': self.getItemInfos(list)
                                            };
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    dataType: "json",
                                                    method: 'POST',
                                                    data: data,
                                                    url: '/sdApi/RemoveObjectList'
                                                },
                                                function (newVal) {
                                                    data.ObjectList.forEach(function (info) {
                                                        $(document).trigger('local_objectDeleted', [info.ClassID, info.ID]);
                                                        self.removeRowFromSelected(info);
                                                    });
                                                    if (isSelectedList)
                                                        selectedList.listView.rowViewModel.checkedItems([]);
                                                    else
                                                        self.clearSelection();
                                                });
                                        }
                                    });
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('ActionRemove');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.IsEqualGuid = function (a, b) {
                        if (!a && !b)
                            return true;
                        if (!a && b || a && !b)
                            return false;
                        //
                        return a.toUpperCase() == b.toUpperCase();
                    };
                    //
                    self.HasNewLocation = function (spec) {
                        if (spec.NewRackID || spec.NewRoomID || spec.NewWorkPlaceID || spec.NewNetworkDeviceID || spec.NewTerminalDeviceID)
                            return true;
                        //
                        return false;
                    };
                    //
                    self.speficationHasDeviations = function (spec) {
                        if (((spec.NewCount || spec.NewCount === 0) && spec.Count != spec.NewCount) ||
                            (spec.NewManufacturerID && !self.IsEqualGuid(spec.ManufacturerID, spec.NewManufacturerID)) ||
                            (self.HasNewLocation(spec) && (spec.RackID != spec.NewRackID || spec.RoomID != spec.NewRoomID || spec.WorkPlaceID != spec.NewWorkPlaceID || !self.IsEqualGuid(spec.NetworkDeviceID, spec.NewNetworkDeviceID) || !self.IsEqualGuid(spec.TerminalDeviceID, spec.NewTerminalDeviceID))) ||
                            (spec.NewUtilizerID && !self.IsEqualGuid(spec.UtilizerID, spec.NewUtilizerID)) ||
                            (spec.NewProductCatalogModelID && !self.IsEqualGuid(spec.ProductCatalogModelID, spec.NewProductCatalogModelID)))
                            return true;
                        //
                        return false;
                    };
                    //
                    self.selectedItemsHasDeviations = function (isSelectedList) {
                        var list = isSelectedList ?
                            selectedList.listView.rowViewModel.checkedItems() :
                            self.listView.rowViewModel.checkedItems();
                        //
                        if (!list)
                            return false;
                        //
                        var retval = false;
                        list.forEach(function (el) {
                            retval = self.speficationHasDeviations(el);
                            if (retval)
                                return;
                        });
                        //
                        return retval;
                    };
                    //
                    self.canDoInventoryOperation = function (isSelectedList) {
                        var list = isSelectedList ?
                            selectedList.listView.rowViewModel.checkedItems() :
                            self.listView.rowViewModel.checkedItems();
                        //
                        if (!list)
                            return false;
                        //
                        var retval = true;
                        list.forEach(function (el) {
                            if (el.SolutionEnum === 0 ||//Confirmed
                                el.SolutionEnum === 2 ||//DB_Updated
                                el.SolutionEnum === 3)//Ignore
                            {
                                retval = false;
                                return;
                            }
                        });
                        //
                        return retval;
                    };
                    //
                    self.confirm = function (contextMenu, getSelectedItems) {
                        var isSelectedList = null;
                        if (isSelectedList === null)
                            isSelectedList = getSelectedItems ? true : false;
                        //
                        if (!getSelectedItems)
                            getSelectedItems = self.getSelectedItems;
                        //
                        var isEnable = function () {
                            return self.canDoInventoryOperation(isSelectedList) && !self.selectedItemsHasDeviations(isSelectedList) && self.operationIsGranted(899) && !self.operationIsDisabled(6);//OPERATION_InventorySpecification_Update = 899; InvConfirm = 6
                        };
                        var isVisible = function () {
                            return getSelectedItems().length != 0;
                        };
                        var action = function () {
                            var list = getSelectedItems();
                            if (list.length == 0)
                                return;
                            //
                            var data = {
                                'InventorySpecificationList': self.getItemInfos(list)
                            };
                            //
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/assetApi/InventoryConfirm'
                                },
                                function (newVal) {
                                });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('InventorySpecification_Confirm');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.updateDB = function (contextMenu, getSelectedItems) {
                        var isSelectedList = null;
                        if (isSelectedList === null)
                            isSelectedList = getSelectedItems ? true : false;
                        //
                        if (!getSelectedItems)
                            getSelectedItems = self.getSelectedItems;
                        //
                        var isEnable = function () {
                            return self.canDoInventoryOperation(isSelectedList) && self.selectedItemsHasDeviations(isSelectedList) && self.operationIsGranted(899) && !self.operationIsDisabled(9);;//OPERATION_InventorySpecification_Update = 899, InvUpdate = 9
                        };
                        var isVisible = function () {
                            return getSelectedItems().length != 0;
                        };
                        var action = function () {
                            var list = getSelectedItems();
                            if (list.length == 0)
                                return;
                            //
                            var data = {
                                'List': self.getItemInfos(list)
                            };
                            //
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/assetApi/InventoryUpdateDB'
                                },
                                function (newVal) {
                                    /*data.ObjectList.forEach(function (info) {
                                        $(document).trigger('local_objectDeleted', [info.ClassID, info.ID]);
                                    });*/
                                    //self.clearSelection();
                                });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('InventorySpecification_UpdateDB');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.ignore = function (contextMenu, getSelectedItems) {
                        var isSelectedList = null;
                        if (isSelectedList === null)
                            isSelectedList = getSelectedItems ? true : false;
                        //
                        if (!getSelectedItems)
                            getSelectedItems = self.getSelectedItems;
                        //
                        var isEnable = function () {
                            return self.canDoInventoryOperation(isSelectedList) && self.selectedItemsHasDeviations(isSelectedList) && self.operationIsGranted(899) && !self.operationIsDisabled(7);;//OPERATION_InventorySpecification_Update = 899, InvIgnore = 7
                        };
                        var isVisible = function () {
                            return getSelectedItems().length != 0;
                        };
                        var action = function () {
                            var list = getSelectedItems();
                            if (list.length == 0)
                                return;
                            //
                            var data = {
                                'InventorySpecificationList': self.getItemInfos(list)
                            };
                            //
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/assetApi/InventoryIgnore'
                                },
                                function (newVal) {
                                    /*data.ObjectList.forEach(function (info) {
                                        $(document).trigger('local_objectDeleted', [info.ClassID, info.ID]);
                                    });*/
                                    //self.clearSelection();
                                });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('InventorySpecification_Ignore');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                }
                //
                {//selected list handle
                    self.selectedListContextMenuInit = function (contextMenu) {
                        selectedList.listViewContextMenu(contextMenu);
                        //
                        self.properties(contextMenu, selectedList.getSelectedItems);
                        contextMenu.addSeparator();
                        self.assetProperties(contextMenu, selectedList.getSelectedItems);
                        contextMenu.addSeparator();
                        //self.add(contextMenu, selectedList.getSelectedItems);
                        self.remove(contextMenu, selectedList.getSelectedItems);
                        contextMenu.addSeparator();
                        self.confirm(contextMenu, selectedList.getSelectedItems);
                        self.ignore(contextMenu, selectedList.getSelectedItems);
                        self.updateDB(contextMenu, selectedList.getSelectedItems);
                        contextMenu.addSeparator();
                    };
                    //
                    self.selectedListcontextMenuOpening = function (contextMenu) {
                        var oldDynamicItems = [];
                        contextMenu.items().forEach(function (item) {
                            if (item.isDynamic)
                                oldDynamicItems.push(item);
                            else if (item.isEnable && item.isVisible) {
                                item.enabled(item.isEnable());
                                item.visible(item.isVisible());
                            }
                        });
                        for (var i = 0; i < oldDynamicItems.length; i++)
                            contextMenu.removeItem(oldDynamicItems[i]);
                        //
                        var waitItem = self.wait(contextMenu);
                        $.when(self.fillDynamicItems(contextMenu, selectedList.getSelectedItems)).done(function () {
                            contextMenu.removeItem(waitItem);
                            if (contextMenu.visibleItems().length == 0)
                                contextMenu.close();
                        });
                    };
                    //
                    self.removeRowFromSelected = function (obj) {
                        var exists = ko.utils.arrayFirst(selectedList.list(), function (el) {
                            return el.ID.toUpperCase() == obj.ID.toUpperCase();
                        });
                        //
                        if (exists) {
                            selectedList.list.remove(exists);
                            selectedList.removeRowByID(obj.ID);
                            return;
                            //
                            var row = ko.utils.arrayFirst(selectedList.listView.rowViewModel.rowList(), function (el) {
                                return el.object.ID.toUpperCase() == obj.ID.toUpperCase();
                            });
                            selectedList.listView.rowViewModel.rowList.remove(row);
                        }
                    };
                    //
                    self.addRowToSelected = function (obj) {
                        var exists = ko.utils.arrayFirst(selectedList.list(), function (el) {
                            return el.ID.toUpperCase() == obj.ID.toUpperCase();
                        });
                        if (!exists) {
                            selectedList.list.push(obj);
                            selectedList.listView.rowViewModel.rowList.push(selectedList.listView.rowViewModel.createRow(obj));
                            selectedList.listView.waitAndRenderTable();
                        }
                    };
                    //
                    self.modifyRowToSelected = function (obj) {
                        var exists = ko.utils.arrayFirst(selectedList.list(), function (el) {
                            return el.ID.toUpperCase() == obj.ID.toUpperCase();
                        });
                        if (exists) {
                            selectedList.list.remove(exists);
                            //
                            var row = ko.utils.arrayFirst(selectedList.listView.rowViewModel.rowList(), function (el) {
                                return el.object.ID.toUpperCase() == obj.ID.toUpperCase();
                            });
                            //
                            selectedList.listView.rowViewModel.rowList.remove(row);
                            //
                            selectedList.list.push(obj);
                            var newRow = selectedList.listView.rowViewModel.createRow(obj);
                            newRow.type(2);//module.rowType.newer
                            //
                            selectedList.listView.rowViewModel.rowList.push(newRow);
                            selectedList.listView.waitAndRenderTable();
                            //
                            var _row = ko.utils.arrayFirst(self.listView.rowViewModel.rowList(), function (el) {
                                return el.object.ID.toUpperCase() == obj.ID.toUpperCase();
                            });
                            //
                            _row.checked(true);
                        }
                    };
                }
                //
                {//open object form
                    self.showSpecificationForm = function (objectID) {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var isReadOnly = self.operationIsDisabled(4);//InvEditResults = 4
                            //
                            fh.ShowInventorySpecificationForm(objectID, isReadOnly);
                        });
                    };

                    self.showAssetForm = function (objectID, objectClassID) {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowAssetForm(objectID, objectClassID);
                        });
                    };
                }
                //            
                {//server and local(only this browser tab) events                               
                    self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        objectID = objectID.toUpperCase();
                        //
                        if (vm.object() != null && parentObjectID != null && parentObjectID.toUpperCase() == vm.object().ID().toUpperCase() || parentObjectID == null)
                            self.reloadObjectByID(objectID);
                    };
                    //
                    self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                        objectID = objectID.toUpperCase();
                        //
                        if (!self.isObjectClassVisible(objectClassID)) {
                            if (objectID == vm.object().ID().toUpperCase()) {
                                self.needUpdateDisabledOperationsWF(true);
                            }
                            return;//в текущем списке измененный объект присутствовать не может
                        }
                        //
                        if (vm.object() != null && parentObjectID != null && parentObjectID.toUpperCase() == vm.object().ID().toUpperCase() || parentObjectID == null)
                            self.reloadObjectByID(objectID);
                    };
                    //
                    self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке удаляемый объект присутствовать не может
                        objectID = objectID.toUpperCase();
                        //
                        self.removeRowByID(objectID);
                        self.clearInfoByObject(objectID);
                    };
                    //
                    $(document).bind('objectInserted', self.onObjectInserted);
                    $(document).bind('local_objectInserted', self.onObjectInserted);
                    $(document).bind('objectUpdated', self.onObjectModified);
                    $(document).bind('local_objectUpdated', self.onObjectModified);
                    $(document).bind('objectDeleted', self.onObjectDeleted);
                    $(document).bind('local_objectDeleted', self.onObjectDeleted);
                }
                //
                m_lazyEvents.init(self);//extend self
                //Переопределяем функцию, т.к. в этом списке нет информации о новых объектах
                self.addToModifiedObjectIDs = function (objectID) {
                    self.reloadObjectByID(objectID);
                };
            },
            LifeCycleObject: function (classID, ID, name, lifeCycleStateID) {
                var self = this;
                //
                self.ClassID = classID,
                    self.ID = ID,
                    self.Name = name,
                    self.LifeCycleStateID = lifeCycleStateID
            },
        };
        return module;
    });