define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu', ],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            LinkList: function (vm) {
                var self = this;
                self.Controller = ko.observable(null);//задается в ReferenceControl.js
                self.IsLoaded = false;
                //
                {//splitter
                    self.Height = ko.observable(0);
                    self.HeightCoeff = 0;
                    self.MaxHeight = 2147483647;
                    self.MinHeight = -self.MaxHeight;
                    //
                    self.ResizeFunction = function (height) {
                        if (!self.Controller())
                            return;
                        //
                        var other = self.Controller().GetOtherItem(self);
                        if (!other)
                            return;
                        //
                        var newHeight = Math.max(height - self.Controller().HeaderHeight, 0);
                        var dy = newHeight - self.Height();
                        //
                        if (other.Height() == 0 && dy > 0)
                            return;
                        //
                        if (!self.IsExpanded())
                            self.IsExpanded(true);
                        //
                        var heightOther = other.Height() - dy;
                        if (heightOther < 0) {
                            newHeight = Math.max(newHeight - Math.abs(heightOther), 0);
                            heightOther = 0;
                        }
                        //
                        self.SetHeight(newHeight);
                        other.SetHeight(heightOther);
                    };
                    //
                    self.SetHeight = function (height) {
                        if (!self.Controller())
                            return;
                        //
                        self.Height(height);
                        self.Controller().ResetHeightCoeff(self);
                        self.listView.waitAndRenderTable();
                    };
                }
                //
                self.IsSplitterVisible = ko.computed(function () {
                    if (!self.Controller())
                        return false;
                    //
                    var other = self.Controller().GetOtherItem(self);
                    return other ? true : false;
                });     
                //
                self.IsExpanded = ko.observable(true);
                self.ExpandCollapseClick = function () {
                    if (!self.Controller())
                        return;
                    //
                    self.Controller().ExpandCollapseClick(self);
                };
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
                    };
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
                        self.showObjectForm(obj.ID, obj.ClassID);
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };

                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 33;//OBJ_Adapter
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
                        contextMenu.addSeparator();
                    };
                    //
                    self.contextMenuOpening = function (contextMenu) {
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

                    self.wait = function (contextMenu) {
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Loading');
                        cmd.enabled(false);
                        //
                        cmd.isDynamic = true;
                        return cmd;
                    };
                    //
                    self.fillDynamicItems = function (contextMenu) {
                        var retval = $.Deferred();
                        var lifeCycleObjectList = self.getSelectedItems();
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: { DeviceList: lifeCycleObjectList, ParentID: vm.asset().ID(), ParentClassID: vm.asset().ClassID()  },
                                url: '/sdApi/GetContextMenu'
                            },
                            function (newVal) {
                                if (newVal && newVal.List) {
                                    newVal.List.forEach(function (lifeCycleOperation) {
                                        if (lifeCycleOperation.CommandType !== 7)//Установить в
                                            return;
                                        //
                                        var cmd = contextMenu.addContextMenuItem();
                                        cmd.enabled(lifeCycleOperation.Enabled);
                                        cmd.text(lifeCycleOperation.Name);
                                        cmd.click(function () { self.ContextMenuAction(lifeCycleOperation); });
                                        //
                                        cmd.isDynamic = true;
                                    });
                                }
                                retval.resolve();
                            });
                        //
                        return retval.promise();
                    };
                    //
                    self.ContextMenuAction = function (contextMenuItem) {
                        if (contextMenuItem.LifeCycleStateOperationID) {
                            if (contextMenuItem.CommandType == 7) {//asset from storage
                                showSpinner();
                                require(['assetForms'], function (module) {
                                    var fh = new module.formHelper(true);
                                    var parent = 
                                        [{
                                            ID: vm.asset().ID(),
                                            ClassID: vm.asset().ClassID(),
                                            Name: vm.asset().FullName(),
                                            //LifeCycleStateID: vm.asset().LifeCycleStateID(),
                                            IsLogical: vm.asset().IsLogical(),
                                        }];
                                    fh.ShowAssetMoveForm(parent, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID, contextMenuItem.CommandType, 33);
                                });
                            }

                        }
                    };
                    //
                    self.properties = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(77);//OPERATION_PROPERTIES_ADAPTER
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var obj = self.getSelectedItems()[0];
                            //     
                            showSpinner();
                            require(['assetForms'], function (module) {
                                var fh = new module.formHelper(true);
                                fh.ShowAssetForm(obj.ID, obj.ClassID);
                            });
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
                            return self.operationIsGranted(84) && vm.CanEdit();//OPERATION_ADD_ADAPTER
                        };
                        var action = function () {
                            showSpinner();
                            require(['assetForms'], function (module) {
                                var fh = new module.formHelper(true);
                                fh.ShowAssetRegistrationForm(33, vm.asset);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.getItemName = function (item) {
                        return item.TypeName + ' \\ ' + item.ModelName;
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
                                ParentObjectID: vm.asset().ID(),
                                ID: self.getObjectID(item),
                                ClassID: 33 //OBJ_ADAPTER
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
                            return self.operationIsGranted(78) && vm.CanEdit();//OPERATION_DELETE_ADAPTER
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
                                                function (newVal) {
                                                    data.ObjectList.forEach(function (info) {
                                                        $(document).trigger('local_objectDeleted', [info.ClassID, info.ID]);
                                                    });
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

                    /*self.contextMenuOpening = function (contextMenu) {
                        contextMenu.items().forEach(function (item) {
                            if (item.isEnable && item.isVisible) {
                                item.enabled(item.isEnable());
                                item.visible(item.isVisible());
                            }
                        });
                    };*/
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
                            ViewName: 'AdapterReference',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.asset().ID(),
                            ParentObjectClassID: vm.asset().ClassID(),
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetAdapterReferenceObject'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request  
                                    if (!self.IsLoaded) {
                                        self.IsLoaded = true;
                                        self.Controller().InitializeItemsHeight();
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
                {//open object form
                    self.showObjectForm = function (objectID, objectClassID) {
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
                        //
                        objectID = objectID.toUpperCase();
                        //
                        if (vm.asset() != null && parentObjectID != null && parentObjectID.toUpperCase() == vm.asset().ID().toUpperCase() || parentObjectID == null)
                            self.reloadObjectByID(objectID);
                    };
                    //
                    self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        //
                        objectID = objectID.toUpperCase();
                        //
                        if (vm.asset() != null && parentObjectID != null && parentObjectID.toUpperCase() == vm.asset().ID().toUpperCase() || parentObjectID == null)
                            self.reloadObjectByID(objectID);
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