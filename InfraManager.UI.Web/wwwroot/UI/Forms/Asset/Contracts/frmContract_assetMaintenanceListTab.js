define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu',],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            Tab: function (vm) {
                var self = this;
                self.ajaxControl = new ajaxLib.control();
                //
                self.Name = getTextResource('Contract_AssetMaintenanceListTab');
                self.Template = '../UI/Forms/Asset/Contracts/frmContract_assetMaintenanceListTab';
                self.IconCSS = 'assetMaintenanceTab';
                //
                self.IsVisible = ko.observable(true);
                //
                self.canMaintenanceAdd = ko.observable(false);
                //
                //when object changed
                self.init = function (obj) {
                    self.IsVisible(obj.IsAssetMainteinanceTabVisible());
                };
                //when tab selected
                self.load = function () {
                    self.canMaintenanceAdd(!vm.object().HasAgreement() && self.operationIsGranted(881));
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
                        self.bindListTab();
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
                        if (self.operationIsGranted(880))
                            self.showObjectForm(obj.ServiceContractID, obj.ID, obj.ObjectClassID);
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };

                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 389;//OBJ_ServiceContractAssetMaintenance
                    };
                }
                //
                {//contextMenu
                    {//granted operations
                        self.grantedOperations = [];
                        //
                        self.operationIsGranted = function (operationID) {
                            for (var i = 0; i < self.grantedOperations.length; i++)
                                if (self.grantedOperations[i] === operationID)
                                    return true;
                            return false;
                        };
                        //
                        $.when(userD).done(function (user) {
                            self.grantedOperations = user.GrantedOperations;
                            //
                            self.canMaintenanceAdd(self.operationIsGranted(881));//OPERATION_ServiceContractMaintenance_Add
                        });
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
                        self.propertiesAsset(contextMenu);
                        contextMenu.addSeparator();
                        self.add(contextMenu);
                        self.remove(contextMenu);
                        contextMenu.addSeparator();
                        self.maintenanceFinish(contextMenu);
                    };
                    self.properties = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(880);//OPERATION_ServiceContractMaintenance_Properties
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var obj = self.getSelectedItems()[0];
                            //     
                            self.showObjectForm(obj.ServiceContractID, obj.ID, obj.ObjectClassID);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Properties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.propertiesAsset = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1 && !self.getSelectedItems()[0].ObjectRemoved;
                        };
                        var isVisible = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var obj = self.getSelectedItems()[0];

                            if (obj.ObjectClassID == 6)//OBJ_TERMINALDEVICE
                                return self.operationIsGranted(65); //OPERATION_PROPERTIES_TERMINALDEVICE
                            if (obj.ObjectClassID == 5)//OBJ_NETWORKDEVICE
                                return self.operationIsGranted(23); //OPERATION_PROPERTIES_NETWORKDEVICE
                            if (obj.ObjectClassID == 33)//OBJ_ADAPTER
                                return self.operationIsGranted(77); //OPERATION_PROPERTIES_ADAPTER
                            if (obj.ObjectClassID == 34)//OBJ_PERIPHERAL
                                return self.operationIsGranted(79); //OPERATION_PROPERTIES_PERIPHERAL
                            //
                            return false;
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
                                fh.ShowAssetForm(obj.ID, obj.ObjectClassID);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('AssetProperties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.addMaintenance = function () {
                        require(['assetForms'], function (fhModule) {
                            var fh = new fhModule.formHelper();
                            fh.ShowAssetLink({
                                ClassID: null,
                                ID: null,
                                ServiceID: null,
                                ClientID: null,
                                ShowWrittenOff: false
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
                                    'ContractID': vm.object().ID(),
                                    'DependencyList': retval
                                };
                                //
                                self.ajaxControl.Ajax(null,
                                    {
                                        dataType: "json",
                                        method: 'POST',
                                        data: data,
                                        url: '/assetApi/AddContractMaintenance'
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

                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            if (vm.object().HasAgreement())
                                return false;
                            return true;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(881) && vm.CanUpdate();//OPERATION_ServiceContractMaintenance_Add
                        };

                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(self.addMaintenance);
                    };

                    self.getItemName = function (item) {
                        return item.ObjectModel + ' - ' + item.ObjectIdentifier;
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
                                ParentObjectID: vm.object().ID(),
                                ID: self.getObjectID(item),
                                ClassID: 389 //OBJ_ServiceContractAssetMaintenance
                            });
                        });
                        return retval;
                    };

                    self.clearSelection = function () {
                        self.listView.rowViewModel.checkedItems([]);
                    };

                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            if (vm.object().HasAgreement())
                                return false;
                            return self.getSelectedItems().length > 0;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(882) && vm.CanUpdate();//OPERATION_ServiceContractMaintenance_Delete
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

                    self.maintenanceFinish = function (contextMenu) {
                        var isEnable = function () {
                            if (vm.object().HasAgreement())
                                return false;
                            var selected = self.getSelectedItems();
                            if (selected.length == 0)
                                return false;
                            //
                            var hasOnMaintenance = false;
                            selected.forEach(function (item) {
                                if (item.OnMaintenance) {
                                    hasOnMaintenance = true;
                                    return;
                                }
                            });
                            //
                            return hasOnMaintenance;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(883) && vm.CanUpdate();//OPERATION_ServiceContractMaintenance_Update
                        };
                        var action = function () {
                            var selected = self.getSelectedItems();
                            var objectList = [];
                            selected.forEach(function (item) {
                                objectList.push(item.ID);
                            });
                            //     
                            require(['usualForms'], function (module) {
                                var fh = new module.formHelper(true);
                                var options = {
                                    ID: vm.object().ID(),
                                    objClassID: 115,//OBJ_ServiceContract
                                    ClassID: 389,//OBJ_ServiceContractAssetMaintenance
                                    ObjectList: objectList,
                                    fieldName: 'FinishMaintenance',
                                    fieldFriendlyName: getTextResource('Contract_EndDate'),
                                    oldValue: null,
                                    allowNull: false,
                                    OnlyDate: true,
                                    onSave: function (newDate) {
                                        selected.forEach(function (item) {
                                            $(document).trigger('local_objectUpdated', [389, item.ID]);
                                        });
                                    }
                                };
                                fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('MaintenanceFinish');
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
                            ViewName: 'AssetMaintenance',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.object().ID(),
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetAssetMaintenanceObject'
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
                    self.showObjectForm = function (serviceContractID, objectID, objectClassID) {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowContractMaintenance(serviceContractID, objectID, objectClassID, vm.object().HasAgreement());
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
                    self.bindListTab = function () {
                        $(document).bind('objectInserted', self.onObjectInserted);
                        $(document).bind('local_objectInserted', self.onObjectInserted);
                        $(document).bind('objectUpdated', self.onObjectModified);
                        $(document).bind('local_objectUpdated', self.onObjectModified);
                        $(document).bind('objectDeleted', self.onObjectDeleted);
                        $(document).bind('local_objectDeleted', self.onObjectDeleted);
                    };
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