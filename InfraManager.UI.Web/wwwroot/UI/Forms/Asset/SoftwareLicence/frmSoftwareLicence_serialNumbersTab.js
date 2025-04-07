define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu'
    ],
    function (ko, $, ajax, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            Tab: function (vm) {
                var self = this;
                self.ajaxControl = new ajax.control();
                //
                self.Name = getTextResource('SoftwareLicence_SerialNumbersTab');
                self.Template = '../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicence_serialNumbersTab';
                self.IconCSS = 'serialNumbersTab';
                self.ClassID = 402;
                self.canAdd = ko.observable(false);
                //
                self.IsVisible = ko.observable(true);
                //
                {//fields
                    self.isLoaded = false;
                }
                //
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
                };
                //when tab unload
                self.dispose = function () {
                    self.isLoaded = false;
                    self.ajaxControl.Abort();
                    if (self.listViewContextMenu() != null)
                        self.listViewContextMenu().dispose();
                    if (self.listView != null)
                        self.listView.dispose();
                };
                //
                self.AddSerialNumber = function () {
                    require(['ui_forms/Asset/SoftwareLicence/frmSoftwareLicenceSerialNumbersAdd'], function (fhModule) {
                        var form = new fhModule.Form(function (newSerialNumber) {
                            if (!newSerialNumber)
                                return;

                            var data = {
                                'SoftwareLicenceID': vm.object().ID(),
                                'SerialNumber': newSerialNumber
                            };
                            //
                            self.ajaxControl.Ajax($('.external-contacts'),
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/assetApi/AddSoftwareLicenceSerialNumber'
                                },
                                function (model) {
                                    if (model.Result === 0) {
                                        self.listView.load();
                                    }
                                    else {
                                        if (model.Result === 1) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                            });
                                        }
                                        else if (model.Result === 2) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
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
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                            });
                                        }
                                        //
                                    }
                                });
                        });
                        form.Show();
                    });
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
                            if (objectList) {
                                //self.clearAllInfos();
                            }
                            //
                            retvalD.resolve(objectList);
                        });
                        return retvalD.promise();
                    };
                    self.listViewRowClick = function (obj) {
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
                    self.ajaxControl = new ajax.control();
                    self.isAjaxActive = function () {
                        return self.ajaxControl.IsAcitve() == true;
                    };
                    //
                    self.getObjectList = function (idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var requestInfo = {
                            IDList: idArray ? idArray : [],
                            ViewName: 'SoftwareLicenceSerialNumbers',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.object().ID(),
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSoftwareLicenceSerialNumberObject'
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
                                ClassID: self.ClassID,
                                SoftwareLicenceID: el.SoftwareLicenceID,
                                SerialNumber: el.SerialNumber,
                                InUseCount: el.InUseCount
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
                                ClassID: item.ClassID,
                                ID: item.ID
                            });
                        });
                        return retval;
                    };

                    self.getItemName = function (item) {
                        return getTextResource('SerialNumber') + ' \'' + item.SerialNumber + '\'';
                    };
                }
                //
                m_lazyEvents.init(self);//extend self
                //
                {//contextMenu
                    {//granted operations
                        self.grantedOperations = [];
                        self.operationIsGranted = function (operationID) {
                            for (var i = 0; i < self.grantedOperations.length; i++)
                                if (self.grantedOperations[i] === operationID)
                                    return true;
                            return false;
                        };
                        self.UserIsAdmin = false;
                        $.when(userD).done(function (user) {
                            self.UserIsAdmin = user.HasAdminRole;
                            self.grantedOperations = user.GrantedOperations;
                            self.canAdd(self.operationIsGranted(461));
                        });
                        
                    }
                    //
                    self.listViewContextMenu = ko.observable(null);
                    //
                    self.contextMenuInit = function (contextMenu) {
                        self.listViewContextMenu(contextMenu);//bind contextMenu
                        //
                        self.add(contextMenu);
                        self.remove(contextMenu);
                        contextMenu.addSeparator();
                    };
                    //
                    self.contextMenuOpening = function (contextMenu) {
                        contextMenu.items().forEach(function (item) {
                            if (item.isEnable && item.isVisible) {
                                item.enabled(item.isEnable());
                                item.visible(item.isVisible());
                            }
                        });
                    };
                }
                {//menu operations
                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            return self.operationIsGranted(461);
                        };
                        var isVisible = function () {
                            return vm.CanEdit() && vm.object() != null;//OPERATION_SoftwareLicenceSerialNumber.Add
                        };
                        var action = function () {
                            self.AddSerialNumber();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            return self.operationIsGranted(462);
                        };
                        var isVisible = function () {
                            return vm.CanEdit();//OPERATION_SoftwareLicenceSerialNumber.Delete
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
                                                    list.forEach(function (item) {
                                                        self.removeRowByID(item.ID);
                                                        self.clearInfoByObject(item.ID);
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
                }
            }
        };
        return module;
    });