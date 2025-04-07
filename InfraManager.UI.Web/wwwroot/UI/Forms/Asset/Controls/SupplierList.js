define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu', ],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            List: function (vm) {
                var self = this;
                self.ajaxControl = new ajaxLib.control();
                //
                self.SelectedItemsChanged = null;//set in frmSupplierList.js
                self.subscriptionList = [];
                //
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
                        var subscription = self.listView.rowViewModel.checkedItems.subscribe(function () {
                            var checkedItemsCount = self.listView.rowViewModel.checkedItems().length;
                            self.SelectedItemsChanged(checkedItemsCount);
                        });
                        //
                        self.subscriptionList.push(subscription);
                    };

                    self.listViewRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                            if (objectList) {
                                if (startRecordIndex === 0)//reloaded
                                {
                                    self.clearAllInfos();
                                }
                                else
                                    objectList.forEach(function (obj) {
                                        var id = self.getObjectID(obj);
                                        self.clearInfoByObject(id);
                                    });
                            }
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
                        self.showObjectForm(id);
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };
                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 116;//OBJ_SUPPLIER
                    };
                }
                //
                {//contextMenu
                    {//granted operations
                        self.grantedOperations = [];
                        $.when(userD).done(function (user) {
                            self.grantedOperations = user.GrantedOperations;
                        });
                        self.operationIsGranted = function (operationID) {
                            for (var i = 0; i < self.grantedOperations.length; i++)
                                if (self.grantedOperations[i] === operationID)
                                    return true;
                            return false;
                        };
                    }
                    //
                    self.getSelectedItems = function () {
                        var selectedItems = self.listView.rowViewModel.checkedItems();
                        return selectedItems;
                    };
                    //
                    self.listViewContextMenu = ko.observable(null);
                    //
                    self.contextMenuInit = function (contextMenu) {
                        self.listViewContextMenu(contextMenu);//bind contextMenu
                        //
                        self.properties(contextMenu);
                        contextMenu.addSeparator();
                        self.add(contextMenu);
                        self.remove(contextMenu);
                    };
                    self.properties = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(217);//OPERATION_PROPERTIES_SUPPLIER
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var selected = self.getSelectedItems()[0];
                            var id = self.getObjectID(selected);
                            //     
                            self.showObjectForm(id);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Properties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(218);//OPERATION_ADD_SUPPLIER
                        };
                        var action = function () {
                            self.showObjectForm(null);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.getItemName = function (item) {
                        return item.Name;
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
                                ClassID: 116,
                                ID: self.getObjectID(item)
                            });
                        });
                        return retval;
                    };

                    self.clearSelection = function () {
                        self.listView.rowViewModel.checkedItems([]);
                    };

                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length > 0;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(219);//OPERATION_DELETE_SUPPLIER
                        };
                        var action = function () {
                            var list = self.getSelectedItems();
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
                                            function (res) {
                                                if (res.Result == 0) {
                                                }
                                                else {
                                                    require(['sweetAlert'], function () {
                                                        swal(res.Message, '', 'info');
                                                    });
                                                }
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

                    self.contextMenuOpening = function (contextMenu) {
                        contextMenu.items().forEach(function (item) {
                            if (item.isEnable && item.isVisible) {
                                item.enabled(item.isEnable());
                                item.visible(item.isVisible());
                            }
                        });
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
                            $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
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
                            $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
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
                    self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var requestInfo = {
                            StartRecordIndex: idArray ? 0 : startRecordIndex,
                            CountRecords: idArray ? idArray.length : countOfRecords,
                            IDList: idArray ? idArray : [],
                            ViewName: 'Supplier',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSupplierObject'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                    return;
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
                {//open object form
                    self.showObjectForm = function (id) {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                                fh.ShowSupplier(id);
                        });
                    };
                }
                //            
                {//server and local(only this browser tab) events                               
                    self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        //
                        objectID = objectID.toUpperCase();
                        //
                        var loadOjbect = true;//будем загружать
                        var row = self.getRowByID(objectID);
                        if (row != null) {
                            //используем грязное чтение, поэтому такое возможно
                            self.setRowAsOutdated(row);
                        }
                        //
                        if (loadOjbect == true) {
                            if (self.isAjaxActive() === true)
                                self.addToModifiedObjectIDs(objectID);
                            else
                                self.reloadObjectByID(objectID);
                        }
                    };
                    //
                    self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        //
                        objectID = objectID.toUpperCase();
                        //
                        var row = self.getRowByID(objectID);
                        if (row == null) {
                            var viewName = self.viewName;
                            //
                            self.checkAvailabilityID(objectID);
                        } else {
                            self.setRowAsOutdated(row);
                            //
                            if (self.isAjaxActive() === true)
                                self.addToModifiedObjectIDs(objectID);
                            else
                                self.reloadObjectByID(objectID);
                        }
                    };
                    //
                    self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке удаляемый объект присутствовать не может
                        //
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
            }
        };
        return module;
    });