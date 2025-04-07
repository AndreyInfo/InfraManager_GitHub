define(['knockout', 'jquery', 'ajax', 'dateTimeControl', 'usualForms',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu',],
    function (ko, $, ajaxLib, dtLib, fhModule, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            List: function (vm) {
                var self = this;
                self.ajaxControl = new ajaxLib.control();
                self.SelectedItemsChanged = null;
                self.isOperationeClusterAdd = ko.observable(false);
                //
                self.Template = '../UI/Forms/Asset/ClusterHostsForTable';
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
                        self.showObjectForm(obj.ID);
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };
                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 419 || objectClassID == 409;
                    };
                }
                //
                {//contextMenu
                    //
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
                        //
                        $.when(userD).done(function (user) {
                            self.grantedOperations = user.GrantedOperations;
                            //
                            self.isOperationeClusterAdd(self.operationIsGranted(956));//OPERATION_Cluster_Add
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
                        return;
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
                            return self.operationIsGranted(864);//OPERATION_SupplierContactPerson_Properties
                        };
                        var action = function () {
                            return;
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
                            return self.operationIsGranted(865);//OPERATION_SupplierContactPerson_Add
                        };
                        var action = function () {
                            return;
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.getItemName = function (item) {
                        return item.Surname + ' ' + item.Name;
                    };

                    self.clearSelection = function () {
                        self.listView.rowViewModel.checkedItems([]);
                    };

                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length > 0;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(866);//OPERATION_SupplierContactPerson_Delete
                        };
                        var action = function () {
                            return;
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
                //
                self.createConfigurationUnit = function () {                 
                    if (self.isOperationeClusterAdd() == true) {
                            require(['assetForms'], function (module) {
                                var fh = new module.formHelper(true);
                                fh.ShowConfigurationUnitRegistrationForm(null, vm, self.listView.load);
                            });
                        }
                        else return;
                    };
                //
                self.ajaxControl_ClusterHosts = new ajaxLib.control();
                //
                self.addСluster = function () {
                    require(['assetForms'], function (fhModule) {
                        var fh = new fhModule.formHelper();
                        fh.ShowAssetLink({
                            ClassID: null,
                            ID: null,
                            ServiceID: null,
                            ClientID: null,
                            ShowWrittenOff: false,
                            SelectOnlyOne: true,
                            IsConfigurationUnitAgentForm: false,
                            ConfigurationUnitAgentTypeID: null,
                            IsHostClusterForm: true

                        }, function (newValues) {
                            if (!newValues || newValues.length == 0)
                                return;
                            //
                            var retval = [];
                            ko.utils.arrayForEach(newValues, function (el) {
                                if (el && el.ID)
                                    retval.push({ ID: el.ID, ClassID: el.ClassID });

                            });
                            var data = {
                                'ClusterID': vm.cluster().ID(),
                                'DependencyList': retval
                            };
                            //
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/assetApi/AddClusterHosts'
                                },
                                function (model) {
                                    if (model.Result === 0) {
                                        self.listView.load();
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
                    self.viewName = 'ClusterHostsForTable';
                    self.getObjectList = function (idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var requestInfo = {
                            IDList: idArray ? idArray : [],
                            ViewName: self.viewName,
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.cluster().ID()
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetClusterHostsForTable'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                    return;
                                }
                                else if (newVal && newVal.Result === 1 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[ClusterHostsForTable.js getObjectList]', 'error');
                                    });
                                }
                                else if (newVal && newVal.Result === 2 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[ClusterHostsForTable.js getObjectList]', 'error');
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
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ClusterHostsForTable.js getObjectList]', 'error');
                                    });
                                }
                                //
                                retvalD.resolve([]);
                            },
                            function (XMLHttpRequest, textStatus, errorThrown) {
                                if (showErrors === true)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ClusterHostsForTable.js, getObjectList]', 'error');
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
                            fh.ShowConfigurationUnitForm(id);
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