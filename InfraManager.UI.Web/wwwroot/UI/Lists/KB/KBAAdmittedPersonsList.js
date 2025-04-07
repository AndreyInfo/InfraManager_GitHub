define(['knockout', 'jquery', 'usualForms', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu',],
    function (ko, $, fhModule, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            ViewModel: function (kba, tmpKBAID) {
                var self = this;
                self.ajaxControl = new ajaxLib.control();
                self.dispose = function () {
                    $(document).unbind('objectInserted', self.onObjectInserted);
                    $(document).unbind('local_objectInserted', self.onObjectInserted);
                    $(document).unbind('objectUpdated', self.onObjectModified);
                    $(document).unbind('local_objectUpdated', self.onObjectModified);
                    $(document).unbind('objectDeleted', self.onObjectDeleted);
                    $(document).unbind('local_objectDeleted', self.onObjectDeleted);
                    //
                    if (self.listViewContextMenu() != null)
                        self.listViewContextMenu().dispose();
                    //
                    self.ajaxControl.Abort();
                    //
                    if (self.listView != null)
                        self.listView.dispose();
                };
                //
                $.when(userD).done(function (user) {
                    self.CurrentUserID = user.UserID;
                });
                //
                {

                    self.KBA = null;
                    self.TMPKBAID = tmpKBAID;
                    if (kba && kba.ID && kba.ID != null || self.TMPKBAID() != null) {
                        self.KBA = kba;
                    }
                    else {
                        self.ajaxControl_tmpGuid = new ajaxLib.control();
                        self.ajaxControl_tmpGuid.Ajax(null,
                            {
                                method: 'Get',
                                url: '/sdApi/GetKbaTmpGuid'
                            },
                            function (newVal) {
                                self.KBA = kba;
                                self.TMPKBAID(newVal);
                            }
                        );
                    }
                }

                self.KBAArticleName = kba.Name;
                self.KBAAccess = kba.AccessName;
                self.KBAAccessList = ko.observableArray([]);

                {//events of listView
                    self.listView = null;
                    self.viewName = 'KBAAdmittedPersons';
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
                        var test = obj;
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.KbArticleID.toUpperCase();
                    };
                }
                self.OnResize = function () {//чтобы была красивая прокрутка таблицы, а кнопки при этом оставались видны
                    // 
                };
                //
                {//
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

                    self.addAccessListItem = function (item) {
                        showSpinner();
                        //
                        self.ajaxControl_AddAdmittedUsers.Ajax(null,
                            {
                                dataType: "text",
                                contentType: "application/json",
                                method: 'POST',
                                data: JSON.stringify(item),
                                url: '/api/KBAccessList'
                            },
                            function () {
                                self.listView.load();
                                hideSpinner();
                            }
                        );
                    }

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
                        self.addUser(contextMenu);
                        self.addQueue(contextMenu);
                        self.addSub(contextMenu);
                        self.addOrg(contextMenu);
                        contextMenu.addSeparator();
                        self.remove(contextMenu);
                    };

                    self.contextMenuOpening = function (contextMenu) {
                        contextMenu.items().forEach(function (item) {
                            if (item.isEnable && item.isVisible) {
                                item.enabled(item.isEnable());
                                item.visible(item.isVisible());
                            }
                        });
                    };

                    self.getItemName = function (item) {
                        return item.Name;
                    };

                    self.ajaxControl_AddAdmittedUsers = new ajaxLib.control();
                    self.addUser = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return true;

                        };
                        var action = function () {
                            //
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                var fh = new fhModule.formHelper(true);
                                var options = {
                                    fieldName: 'KBAAdmitted.Users',
                                    fieldFriendlyName: getTextResource('User'),
                                    oldValue: null,
                                    object: ko.toJS(self.CurrentUserID.ID),
                                    searcherName: 'WebUserSearcherNoTOZ',
                                    searcherPlaceholder: getTextResource('EnterFIO'),
                                    nosave: true,
                                    onSave: function (selectedUser) {
                                        var newItem = {
                                            KbArticleID: self.KBA.ID ?? self.TMPKBAID(),
                                            ObjectID: selectedUser.ID,
                                            ObjectClass: selectedUser.ClassID,
                                            WithSub: false,
                                            ObjectName: selectedUser.FullName
                                        }

                                        self.addAccessListItem(newItem);
                                    }
                                };
                                fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('KBAAddUser');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.ajaxControl_AddAdmittedQueue = new ajaxLib.control();
                    self.addQueue = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return true;

                        };
                        var action = function () {
                            //
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                var fh = new fhModule.formHelper(true);
                                var options = {
                                    fieldName: 'KBAAdmitted.Queue',
                                    fieldFriendlyName: getTextResource('Queue'),
                                    oldValue: null,
                                    object: ko.toJS(self.CurrentUserID.ID),
                                    searcherName: 'QueueSearcher',
                                    searcherPlaceholder: getTextResource('EnterQueue'),
                                    nosave: true,
                                    onSave: function (selected) {
                                        var newItem = {
                                            KbArticleID: self.KBA.ID ?? self.TMPKBAID(),
                                            ObjectID: selected.ID,
                                            ObjectClass: selected.ClassID,
                                            WithSub: false,
                                            ObjectName: selected.FullName
                                        }

                                        self.addAccessListItem(newItem);
                                    }
                                };
                                fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('KBAAddQueue');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };


                    self.ajaxControl_AddAdmittedSub = new ajaxLib.control();
                    self.addSub = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return true;

                        };
                        var action = function () {
                            //
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                var fh = new fhModule.formHelper(true);
                                var options = {
                                    fieldName: 'KBAAdmitted.SubDivision',
                                    fieldFriendlyName: getTextResource('UserSubdivision'),
                                    oldValue: null,
                                    object: ko.toJS(self.CurrentUserID.ID),
                                    searcherName: 'SubDivisionSearcherNoTOZWithoutCurrentUser',
                                    searcherPlaceholder: getTextResource('UserSubdivision'),
                                    nosave: true,
                                    onSave: function (selected) {
                                        var newItem = {
                                            KbArticleID: self.KBA.ID ?? self.TMPKBAID(),
                                            ObjectID: selected.ID,
                                            ObjectClass: selected.ClassID,
                                            WithSub: selected.WithQueue,
                                            ObjectName: selected.FullName
                                        }

                                        self.addAccessListItem(newItem);
                                    }
                                };
                                fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEditForSubdivision, options);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('KBAAddSubdivision');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };


                    self.ajaxControl_AddAdmittedOrg = new ajaxLib.control();
                    self.addOrg = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return true;

                        };
                        var action = function () {
                            //
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                var fh = new fhModule.formHelper(true);
                                var options = {
                                    fieldName: 'KBAAdmitted.Organization',
                                    fieldFriendlyName: getTextResource('AddOrganizationMenuItem'),
                                    oldValue: null,
                                    object: ko.toJS(self.CurrentUserID.ID),
                                    searcherName: 'OrganizationSearcherNoTOZWithoutCurrentUser',
                                    searcherPlaceholder: getTextResource('OrgStructureLevel_Organization'),
                                    nosave: true,
                                    onSave: function (selected) {
                                        var newItem = {
                                            KbArticleID: self.KBA.ID ?? self.TMPKBAID(),
                                            ObjectID: selected.ID,
                                            ObjectClass: selected.ClassID,
                                            WithSub: false,
                                            ObjectName: selected.FullName
                                        }

                                        self.addAccessListItem(newItem);
                                    }
                                };
                                fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('AddOrganizationMenuItem');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;

                        };
                        var isVisible = function () {
                            var obj = self.getSelectedItems()[0];
                            if (obj) {
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
                                    title: getTextResource('KBARemoveAccess'),
                                    text: getTextResource('KBARemoveAccessError'),
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
                                                KbArticleID: obj.KbArticleID,
                                                ObjectID: obj.ObjectID,
                                            };
                                            
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    dataType: "text",
                                                    contentType: "application/json",
                                                    method: 'DELETE',
                                                    data: JSON.stringify(data),
                                                    url: '/api/KBAccessList'
                                                },
                                                function () {
                                                    self.listView.load();
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
                            StartRecordIndex: startRecordIndex,
                            CountRecords: idArray ? idArray.length : countOfRecords,
                            IDList: idArray ? idArray : [],
                            ViewName: self.viewName,
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),
                            KbArticleID: kba.ID ?? self.TMPKBAID(),
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                data: requestInfo,
                                url: '/api/KBAccessList'
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
                }
                self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                    objectID = objectID.toUpperCase();
                    self.reloadObjectByID(objectID);
                };
                self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                    objectID = objectID.toUpperCase();
                    var row = self.getRowByID(objectID);
                    if (row == null)
                        self.checkAvailabilityID(objectID);
                    else
                        self.reloadObjectByID(objectID);
                };
                //
                self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                    objectID = objectID.toUpperCase();
                    //
                    self.removeRowByID(objectID);
                    self.clearInfoByObject(objectID);
                };

                $(document).bind('objectInserted', self.onObjectInserted);
                $(document).bind('local_objectInserted', self.onObjectInserted);
                $(document).bind('objectUpdated', self.onObjectModified);
                $(document).bind('local_objectUpdated', self.onObjectModified);
                $(document).bind('objectDeleted', self.onObjectDeleted);
                $(document).bind('local_objectDeleted', self.onObjectDeleted);

                m_lazyEvents.init(self);//extend self
                //Переопределяем функцию, т.к. в этом списке нет информации о новых объектах
                self.addToModifiedObjectIDs = function (objectID) {
                    self.reloadObjectByID(objectID);
                };
            }
        };
        return module;
    });