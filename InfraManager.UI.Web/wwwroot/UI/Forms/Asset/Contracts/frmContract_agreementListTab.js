define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu'],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            Tab: function (vm) {
                var self = this;
                self.ajaxControl = new ajaxLib.control();
                //
                self.Name = getTextResource('Contract_ServiceAgreementTabShort');
                self.Template = '../UI/Forms/Asset/Contracts/frmContract_agreementListTab';
                self.IconCSS = 'agreementListTab';
                //
                self.IsVisible = ko.observable(true);
                //
                self.CanAdd = ko.observable(true);
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
                    self.getObjectClassID = function (obj) {
                        return 386;//OBJ_ServiceContractAgreement
                    };
                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 386;//OBJ_ServiceContractAgreement
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
                            data: { DeviceList: lifeCycleObjectList },
                            url: '/sdApi/GetContextMenu'
                        },
                        function (newVal) {
                            if (newVal && newVal.List) {
                                newVal.List.forEach(function (lifeCycleOperation) {
                                    var cmd = contextMenu.addContextMenuItem();
                                    cmd.enabled(lifeCycleOperation.Enabled);
                                    cmd.text(lifeCycleOperation.Name);
                                    cmd.click(function () { self.executeLifeCycleOperation(lifeCycleOperation, lifeCycleObjectList); });
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
                    self.executeLifeCycleOperation = function (contextMenuItem, selectedObjects) {
                        self.ajaxControlExecuteContextMenu = new ajaxLib.control();
                        var cmd = {
                            Enabled: contextMenuItem.Enabled,
                            Name: contextMenuItem.Name,
                            CommandType: contextMenuItem.CommandType,
                            LifeCycleStateOperationID: contextMenuItem.LifeCycleStateOperationID
                        };
                        //
                        self.ajaxControlExecuteContextMenu.Ajax(null,
                        {
                            dataType: "json",
                            method: 'GET',
                            data: { DeviceList: selectedObjects, Command: cmd },
                            url: '/sdApi/ExecuteContextMenu'
                        },
                        function (newVal) {
                            if (newVal) {
                                if (newVal.Result == 0) {
                                    if (newVal.Message)
                                        require(['sweetAlert'], function () {
                                            swal(contextMenuItem.Name, newVal.Message, 'info');
                                        });
                                    ko.utils.arrayForEach(selectedObjects, function (el) {
                                        $(document).trigger('local_objectUpdated', [el.ClassID, el.ID]);
                                    });
                                }
                                else if (newVal.Result == 8) {//validation
                                    require(['sweetAlert'], function () {
                                        swal(contextMenuItem.Name, newVal.Message, 'warning');
                                    });
                                }
                                else if (newVal.Result != 0) {
                                    require(['sweetAlert'], function () {
                                        swal(contextMenuItem.Name, 'Операция не выполнена', 'error');
                                    });
                                }
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
                            ViewName: 'ContractAgreement',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.object().ID(),
                        };
                        self.CanAdd(true);
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetContractAgreementObject'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    newVal.Data.forEach(function (el) {
                                        vm.object().HasAgreement(true);
                                        if (!el.IsApplied)
                                            self.CanAdd(false);
                                    });
                                    //
                                    if (newVal.Data == 0) {
                                        vm.object().HasAgreement(false); 
                                        vm.CanEdit(true);
                                    }
                                    else {
                                        $.when(vm.object().load(vm.object().ID(), vm)).done(function () {
                                            if (vm.object().HasAgreement())
                                                vm.CanEdit(false);
                                            else vm.CanEdit(true);
                                        });                                      
                                    }
                                    //
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
                {//helper methods                               
                    self.getItemName = function (item) {
                        return getTextResource('Contract_Agreement') + ' № ' + item.Number;
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
                                ClassID: 386,//OBJ_ServiceContractAgreement
                                IsApplied: el.IsApplied,
                            };
                            retval.push(item);
                        });
                        return retval;
                    };
                    self.hasAppliedAgreement = function () {
                        var retval = false;
                        self.getSelectedItems().forEach(function (item) {
                            if (item.StateID == 4) {//Applied
                                retval = true;
                                return;
                            }
                        });
                        //
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
                                ID: self.getObjectID(item)
                            });
                        });
                        return retval;
                    };
                }
                //
                {//menu operations
                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            if (!self.CanAdd())
                                return false;
                            return true;
                        };
                        var isVisible = function () {
                            return vm.CanUpdate() && vm.object() != null && vm.object().CanCreateAgreement() == true && self.operationIsGranted(873);//OPERATION_ServiceContractAgreement_Add = 873
                        };
                        var action = function () {
                            showSpinner();
                            require(['assetForms'], function (module) {
                                var fh = new module.formHelper(true);
                                fh.ShowServiceContractAgreement(null, vm.object().ID());
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.properties = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(872);//OPERATION_ServiceContractAgreement_Properties = 872
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
                    //
                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length > 0;
                        };
                        var isVisible = function () {
                            var canRemove = true;
                            self.getSelectedItems().forEach(function (item) {
                                if (item.IsApplied)
                                    canRemove = false;
                            });
                            return (self.operationIsGranted(874) && vm.CanUpdate() && !(self.hasAppliedAgreement()) && canRemove) || self.UserIsAdmin;//OPERATION_ServiceContractAgreement_Delete = 874
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
                                                self.getObjectList(null, true);
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
                }
                //
                {//open object form
                    self.showObjectForm = function (objectID) {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowServiceContractAgreement(objectID);
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
                        if (vm.object() != null && parentObjectID != null && parentObjectID.toUpperCase() == vm.object().ID().toUpperCase() || parentObjectID == null)
                            self.reloadObjectByID(objectID);
                    };
                    //
                    self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        objectID = objectID.toUpperCase();
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
            }
        };
        return module;
    });