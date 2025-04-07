define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu',],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            TableType: {// режимы: работы, отображения
                All: 'all',
                ideputyfor: 'ideputyfor',
                mydeputies: 'mydeputies'
            },
            ViewModel: function (withCompleted, tableType, userID) {
                var self = this;
                self.curUser = ko.observable(userID); 
                self.ajaxControl = new ajaxLib.control();
                self.dispose = function () {
                    //
                    self.ajaxControl.Abort();
                    //
                    if (self.listViewContextMenu() != null)
                        self.listViewContextMenu().dispose();
                    if (self.listView != null)
                        self.listView.dispose();
                };
                //
                {
                    self.WithCompletedDeputy = ko.observable(withCompleted);
                    self.DeputyTableType = ko.observable(tableType);
                }
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

                    self.listViewRetrieveItems = function (startRecordIndex, countOfRecords) {
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
                        var data = obj.UtcDataDeputyWith;
                        if (self.DeputyTableType() == module.TableType.ideputyfor) 
                            return;
                        else if (new Date(data) < new Date())
                            return;
                        //
                        var id = self.getObjectID(obj);
                        var row = self.getRowByID(id);
                        if (row != null)
                            self.setRowAsLoaded(row);
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
                self.OnResize = function () {//чтобы была красивая прокрутка таблицы, а кнопки при этом оставались видны
                    // 
                };
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
                        self.add(contextMenu);
                        self.edit(contextMenu);
                        self.remove(contextMenu);
                    };
                    
                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return true;
                           
                        };
                        var action = function () {
                            self.showObjectForm(null);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('AddSubstitutionProfileSettings');
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

                    self.edit = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            var obj = self.getSelectedItems()[0];
                            if (obj) {
                                if (new Date(parseInt(obj.UtcDataDeputyBySt)) > new Date())
                                    return true;
                            };
                            return false;
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
                        cmd.restext('EditSubstitutionProfileSettings');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //

                    self.CheckData = function () {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(0, 0, null, true)).done(function (objectList) {
                            if (objectList)
                                self.clearAllInfos();
                            //
                            retvalD.resolve(objectList);
                        });
                        return retvalD.promise();
                    };
                    //
                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                           
                        };
                        var isVisible = function () {
                            var obj = self.getSelectedItems()[0];
                            if (obj) {
                                if (new Date(parseInt(obj.UtcDataDeputyWithSt)) > new Date())
                                    return true;
                            };
                            return false;
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            var obj = self.getSelectedItems()[0];
                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('DeletingSubstitutionProfileSettings'),
                                    text: getTextResource('DeleteSubstitutionProfileSettings'),
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
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    dataType: "json",
                                                    method: 'DELETE',
                                                    url: '/api/deputies/' + obj.ID 
                                                },
                                                function () {
                                                    self.onObjectDeleted(obj.ID);
                                                },
                                                null,
                                                function () {
                                                    self.onObjectDeleted(obj.ID);
                                                }
                                            );
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
                            Skip: idArray ? 0 : startRecordIndex,
                            Take: countOfRecords,
                            ShowFinished: self.WithCompletedDeputy(),
                            OrderByProperty: 'UtcDataDeputyBy',
                            UserID: self.curUser(),
                            DeputyMode: +(self.DeputyTableType() === module.TableType.ideputyfor)
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                data: requestInfo,
                                url: '/api/deputies/'
                            },
                            function (newVal) {
                                if (newVal && Array.isArray(newVal)) {
                                    newVal = {
                                        Result: 0,
                                        Data: newVal
                                    };
                                };

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
                            fh.ShowDeputy(id, self.onObjectModified, self.curUser());
                        });
                    };
                }
                //            

                self.isFilterActive = function () {
                    return false;
                };
                self.onObjectModified = function (objectID) {
                    var loadOjbect = true;
                    objectID = objectID.toUpperCase();
                    var row = self.getRowByID(objectID);
                    if (row == null)
                        if (self.isFilterActive() === true) {
                            loadOjbect = false;
                            self.checkAvailabilityID(objectID);
                        } else
                            self.setRowAsOutdated(row);
                    //
                    if (loadOjbect == true) {
                        if (self.isAjaxActive() === true)
                            self.addToModifiedObjectIDs(objectID);
                        else
                            self.reloadObjectByID(objectID);
                    }
                };
                //
                self.onObjectDeleted = function (objectID) {
                    objectID = objectID.toUpperCase();
                    //
                    self.removeRowByID(objectID);
                    self.clearInfoByObject(objectID);
                };

                m_lazyEvents.init(self);//extend self
                //Переопределяем функцию, т.к. в этом списке нет информации о новых объектах
                self.addToModifiedObjectIDs = function (objectID) {
                    self.reloadObjectByID(objectID);
                };
            }
        };
        return module;
    });