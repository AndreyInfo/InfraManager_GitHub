define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu'],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            Tab: function (vm) {
                var self = this;
                self.ajaxControl = new ajaxLib.control();
                //
                self.Name = getTextResource('Contract_SoftwareUsageRights');
                self.Template = '../UI/Forms/Asset/Contracts/frmAgreement_licenceListTab';
                self.IconCSS = 'licenceMaintenanceListTab';
                //
                self.IsVisible = ko.observable(false);
                //
                self.availableAddPlus = ko.observable(false);
                //when object changed
                self.init = function (obj) {
                    self.IsVisible(obj.IsSoftwareLicenceTabVisible());
                };
                //when tab selected
                self.load = function () {
                    self.availableAddPlus(vm.canEdit() && self.operationIsGranted(892));
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

                    self.summa = ko.observable(0);
                    self.availableCost = ko.observable(0);
                    self.agreementCost = ko.pureComputed(function () {
                        return getFormattedMoneyString(vm.object().costWithNDS() ? vm.object().costWithNDS().toString() : '0') + ' ' + getTextResource('CurrentCurrency');
                    });

                    self.availableCostString = ko.pureComputed(function () {
                        self.availableCost(Number(vm.object().costWithNDS()) + Number(vm.object().contract().CostWithNDS()) - self.summa());
                        return getFormattedMoneyString(self.availableCost() ? self.availableCost().toString() : '0') + ' ' + getTextResource('CurrentCurrency');
                    });

                    self.contractCost = ko.pureComputed(function () {
                        return getFormattedMoneyString(vm.object().contract().CostWithNDS() ? vm.object().contract().CostWithNDS().toString() : '0') + ' ' + getTextResource('CurrentCurrency');
                    });

                    self.summaString = ko.pureComputed(function () {
                        return getFormattedMoneyString(self.summa() ? self.summa().toString() : '0') + ' ' + getTextResource('CurrentCurrency');
                    });

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
                        return 339;//OBJ_ServiceContractLicence
                    };
                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 339;//OBJ_ServiceContractLicence
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
                        self.softwareLicenceProperties(contextMenu);
                        contextMenu.addSeparator();
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
                    self.SetCost = function () {
                        var param = { ID: vm.object().id() };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                url: '/assetApi/GetAgreementCost?' + $.param(param)
                            }, function (data) {
                                if (data) {
                                    self.summa(data);
                                    self.availableCost(vm.object().cost() - data);
                                }
                            });
                        return;
                    }


                    self.getObjectList = function (idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var requestInfo = {
                            IDList: idArray ? idArray : [],
                            ViewName: 'ContractLicence',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.object().id(),
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetContractLicenceAgreementObject'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request
                                    self.SetCost();
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
                        return getTextResource('ContractSoftwareLicence') + ' \'' + item.Name + '\'';
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
                                ClassID: 339,//OBJ_ServiceContractLicence
                                Name: el.SoftwareModelName,
                                SoftwareLicenceID: el.SoftwareLicenceID
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
                                ParentObjectID: vm.object().id()
                            });
                        });
                        return retval;
                    };
                }
                //
                {//menu operations
                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return vm.canEdit() && vm.object() != null && self.operationIsGranted(892);//OPERATION_ServiceContractLicence_Add = 892
                        };
                        var action = function () {
                            showSpinner();
                            require(['assetForms'], function (module) {
                                var fh = new module.formHelper(true);
                                fh.ShowServiceContractLicence(null, vm.object().id(), true, vm.object());
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
                            return self.operationIsGranted(891);//OPERATION_ServiceContractLicence_Properties = 891
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var selected = self.getSelectedItems()[0];
                            var id = self.getObjectID(selected);
                            //     
                            self.showObjectForm(id, vm.object().id());
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Properties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.softwareLicenceProperties = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            //OPERATION_SOFTWARELICENCE_PROPERTIES
                            if (self.operationIsGranted(441)
                                && self.getSelectedItems().length === 1 && self.getSelectedItems()[0].SoftwareLicenceID) {
                                return true;
                            } else {
                                return false;
                            }
                        };
                        var action = function () {
                            var obj = self.getSelectedItems()[0];
                            //
                            if (obj.SoftwareLicenceID) {
                                require(['assetForms'], function (module) {
                                    var fh = new module.formHelper(true);
                                    fh.ShowSoftwareLicenceForm(obj.SoftwareLicenceID);
                                });
                            }
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('SoftwareLicenceContextMenu_Properties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            var list = self.listView.rowViewModel.checkedItems();
                            var existsWithLicence = false;
                            for (var i = 0; i < list.length; i++)
                                if (list[i].SoftwareLicenceID != null) {
                                    existsWithLicence = true;
                                    break;
                                }
                            //
                            return list.length > 0 && existsWithLicence === false;
                        };
                        var isVisible = function () {
                            return self.operationIsGranted(893) && vm.canEdit();//OPERATION_ServiceContractLicence_Delete = 893
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
                                                    self.SetCost();
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
                    self.showObjectForm = function (objectID, serviceContractID) {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowServiceContractLicence(objectID, serviceContractID, true, vm.object());
                        });
                    };
                }
                //         
                self.addLicence = function () {
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowServiceContractLicence(null, vm.object().id(), true, vm.object());
                    });
                };
                {//server and local(only this browser tab) events                               
                    self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        objectID = objectID.toUpperCase();
                        //
                        if (vm.object() != null && parentObjectID != null && parentObjectID.toUpperCase() == vm.object().id().toUpperCase() || parentObjectID == null)
                            self.reloadObjectByID(objectID);
                    };
                    //
                    self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        objectID = objectID.toUpperCase();
                        //
                        if (vm.object() != null && parentObjectID != null && parentObjectID.toUpperCase() == vm.object().id().toUpperCase() || parentObjectID == null)
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