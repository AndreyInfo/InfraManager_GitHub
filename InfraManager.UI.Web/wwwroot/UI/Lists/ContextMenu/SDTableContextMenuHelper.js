define(['knockout', 'jquery', 'ajax', 'groupOperation'], function (ko, $, ajaxLib, groupOperation) {
    var module = {
        helper: function (tableViewModel) {
            var self = this;
            self.ajaxControl = new ajaxLib.control();
            //
            {//ko.contextMenu
                {
                    self.contextMenuLoading = false;
                    self.contextMenuInit = function (contextMenu) {
                        tableViewModel.listViewContextMenu(contextMenu);//bind contextMenu
                        //
                        self.loading(contextMenu);
                        self.properties(contextMenu);
                        self.propertiesBudgetRow(contextMenu);
                        self.propertiesBudgetRowAdjustment(contextMenu);
                        contextMenu.addSeparator();
                        self.markAsRead(contextMenu);
                        self.markAsUnread(contextMenu);
                        self.sendToEmailContextMenuItem(contextMenu);
                        contextMenu.addSeparator();
                        self.setCustomControl(contextMenu);
                        self.removeCustomControl(contextMenu);
                        contextMenu.addSeparator();
                        self.add(contextMenu);
                        self.addAs(contextMenu);
                        self.remove(contextMenu);
                        self.changeState(contextMenu);
                        contextMenu.addSeparator();
                        self.transfer(contextMenu);
                        self.pickFromQueue(contextMenu);
                        self.createProblem(contextMenu);
                        self.createRFC(contextMenu);
                        self.createBKArticle(contextMenu);
                        self.createMassIncident(contextMenu);
                        //
                        contextMenu.addSeparator();
                        self.addBudgetRow(contextMenu);
                        self.addBudgetRowAdjustment(contextMenu);
                        self.removeBudgetRow(contextMenu);
                        self.removeBudgetRowAdjustment(contextMenu);
                    };
                    self.contextMenuOpening = function (contextMenu) {

                        var selectedItems = self.getSelectedItems();

                        var setItemsAvailability = function () {
                            contextMenu.items().forEach(function (item) {
                                if (typeof item.isEnable === 'function'
                                    && typeof item.isVisible === 'function') {
                                    item.enabled(false);
                                    Promise.resolve(item.isEnable()).then(enabled => item.enabled(enabled));
                                    item.visible(item.isVisible() && !self.contextMenuLoading);
                                } else if (item.loading) {
                                    item.enabled(false);
                                    item.visible(self.contextMenuLoading);
                                }
                            });
                        };

                        if (selectedItems && selectedItems.some(item => item?.HasState !== true)) {
                            self.contextMenuLoading = true;
                            var objectStatePromise = Promise.resolve();
                            selectedItems.forEach(item => {
                                objectStatePromise = objectStatePromise.then(() => tableViewModel.getObjectStateInfo(item))
                            });
                            objectStatePromise.then(() => {
                                self.contextMenuLoading = false;
                                if (contextMenu.visibleItems().length != 0) {
                                    setItemsAvailability();
                                }
                            });
                        } else {
                            setItemsAvailability();
                        }
                    };
                }
            }
            //
            self.loading = function(contextMenu) {
                var cmd = contextMenu.addContextMenuItem();
                cmd.restext('Loading');
                cmd.loading = true;
                cmd.click(function () { });
            };
            //
            self.createBKArticle = function (contextMenu) {
                var isEnable = function () {
                    return self.operationIsGranted(490) && self.getSelectedItems().length == 1;//OPERATION_KBArticle_Add
                };
                var isVisible = function () {
                    if (self.isBudgetRowListActive() == true)
                        return false;
                    //
                    var viewName = tableViewModel.listView.options.settingsName();
                    if (viewName != 'ProblemForTable' && viewName != 'NegotiationForTable' && viewName != 'CustomControlForTable' && viewName != 'CommonForTable')
                        return false;
                    //
                    var selected = self.getSelectedItems()[0];
                    var classID = tableViewModel.getObjectClassID(selected);
                    //
                    if (classID != 702)//problem
                        return false;
                    //
                    return true;
                };
                var action = function () {
                    showSpinner();
                    //
                    var selected = self.getSelectedItems()[0];
                    var id = tableViewModel.getMainObjectID(selected);
                    //
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowKBAView(null, id);
                    });
                };
                //
                var cmd = contextMenu.addContextMenuItem();
                cmd.restext('Add_article');
                cmd.isEnable = isEnable;
                cmd.isVisible = isVisible;
                cmd.click(action);
            };
            //
            {//granted operations
                {
                    self.grantedOperations = [];
                    self.userID;
                    self.userHasRoles;
                    $.when(userD).done(function (user) {
                        self.userID = user.UserID;
                        self.userHasRoles = user.HasRoles;
                        self.grantedOperations = user.GrantedOperations;
                    });
                    self.operationIsGranted = function (operationID) {
                        for (var i = 0; i < self.grantedOperations.length; i++)
                            if (self.grantedOperations[i] === operationID)
                                return true;
                        return false;
                    };
                }
            }
            //
            {//helper methods  
                {
                    self.getSelectedItems = function () {
                        var selectedItems = tableViewModel.listView.rowViewModel.checkedItems();
                        return selectedItems;
                    };
                    self.isBudgetRowListActive = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'FinanceBudgetRow';
                    };
                    self.isBudgetListActive = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'FinanceBudgetRow' || viewName == 'FinanceActivesRequest' || viewName == 'FinancePurchase';
                    };
                    self.isClientListActive = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'NegotiationForTable' || viewName == 'CustomControlForTable' ? true : false;
                    };
                    self.isClientListVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'ClientCallForTable';
                    };
                    self.isNegotiationListVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'NegotiationForTable';
                    };

                    self.getItemInfos = function (items) {
                        var retval = [];
                        items.forEach(function (item) {
                            retval.push({
                                ClassID: tableViewModel.getObjectClassID(item),
                                ID: tableViewModel.getMainObjectID(item),
                                Uri: item.Uri
                            });
                        });
                        return retval;
                    };
                    self.clearSelection = function () {
                        tableViewModel.listView.rowViewModel.checkedItems([]);
                    };
                    self.clearSelectedItem = function (item) {
                        var selectedItems = self.getSelectedItems();
                        tableViewModel.listView.rowViewModel.checkedItems(selectedItems.filter(si => si.IMObjID !== item.ID))
                    }
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
                    self.getItemName = function (item) {
                        var className;
                        var classID = tableViewModel.getObjectClassID(item);
                        if (classID == 701)//call
                            className = getTextResource('Call');
                        else if (classID == 119)//workOrder
                            className = getTextResource('WorkOrder');
                        else if (classID == 702)//problem
                            className = getTextResource('Problem');
                        else if (classID == 703)//problem
                            className = getTextResource('RFC');
                        else if (classID == 180)//financeBudgetRow
                            return getTextResource('FinanceBudgetRow') + ' ' + item.Identifier;
                        else if (classID == 823)//massIncident
                            className = getTextResource('MassIncident');
                        else
                            throw 'object classID not supported'
                        //
                        return className + ' № ' + item.Number;
                    };
                    self.getItemIDs = function (items) {
                        var infos = self.getItemInfos(items);
                        var retval = [];
                        for (var i = 0; i < infos.length; i++)
                            retval.push(infos[i].ID);
                        return retval;
                    };
                }
            }
            //
            {//menu operations
                {
                    self.properties = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            if (self.isBudgetRowListActive() == true)
                                return false;
                            var selected = self.getSelectedItems()[0];
                            var classID = tableViewModel.getObjectClassID(selected);
                            if ((classID == 701 && !self.operationIsGranted(module.Operations.OPERATION_CALL_Properties)) ||
                                (classID == 702 && !self.operationIsGranted(module.Operations.OPERATION_Problem_Properties)) ||
                                (classID == 119 && !self.operationIsGranted(module.Operations.OPERATION_WorkOrder_Properties)) ||
                                (classID == 703 && !self.operationIsGranted(module.Operations.OPERATION_RFC_Properties)) ||
                                (classID == 823 && !self.operationIsGranted(module.Operations.OPERATION_MassIncident_Properties)))
                                return false;
                            //
                            return true;

                        };

                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var selected = self.getSelectedItems()[0];
                            var id = tableViewModel.getMainObjectID(selected);
                            var classID = tableViewModel.getObjectClassID(selected);
                            //     
                            tableViewModel.showObjectForm(classID, id, selected.Uri);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Properties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.markAsRead = function (contextMenu) {
                        var isEnable = function () {
                            if (self.getSelectedItems().length == 0)
                                return false;
                            //
                            var retval = true;
                            self.getSelectedItems().forEach(function (el) {
                                if ((el.NoteCount == 0 && el.MessageCount == 0) || el.UnreadMessageCount == 0 || el.NoteCount == undefined || el.MessageCount == undefined)
                                    retval = false;
                            });
                            return retval;
                        };
                        var isVisible = function () {
                            var retval = true;
                            if (self.isBudgetRowListActive() == true)
                                retval = false;
                            self.getSelectedItems().forEach(function (el) {
                                if (el.NoteCount == undefined || el.MessageCount == undefined)
                                    retval = false;
                            });
                            if (!retval == false) {
                                retval = self.operationIsGranted(module.Operations.OPERATION_Note_MarkAsReaded);
                            }
                            return retval;

                        };
                        var action = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //     
                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('MarkAsReaded_Question'),
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
                                                'NoteIsReaded': true,
                                                'ObjectList': self.getItemInfos(list)
                                            };
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    contentType: 'application/json',
                                                    method: 'PUT',
                                                    data: JSON.stringify(data),
                                                    url: list[0].Uri + '/notes/status'
                                                },
                                                function (response) {
                                                    if(!response)
                                                        return;

                                                    list.forEach(function (el) {
                                                        el.UnreadMessageCount = 0;
                                                        $(document).trigger('local_objectUpdated', [tableViewModel.getObjectClassID(el), tableViewModel.getMainObjectID(el)]);
                                                    });
                                                    self.clearSelection();
                                                });
                                        }
                                    });
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('MarkAsReaded');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    self.markAsUnread = function (contextMenu) {
                        var isEnable = function () {
                            if (self.getSelectedItems().length == 0)
                                return false;
                            //
                            var retval = true;
                            self.getSelectedItems().forEach(function (el) {
                                if ((el.NoteCount == 0 && el.MessageCount == 0) || el.NoteCount + el.MessageCount == el.UnreadMessageCount || el.NoteCount == undefined || el.MessageCount == undefined)
                                    retval = false;
                            });
                            return retval;
                        };
                        var isVisible = function () {
                            var retval = true;
                            if (self.isBudgetRowListActive() == true)
                                retval = false;
                            self.getSelectedItems().forEach(function (el) {
                                if (el.NoteCount == undefined || el.MessageCount == undefined)
                                    retval = false;
                            });
                            if (!retval == false) {
                                retval = self.operationIsGranted(module.Operations.OPERATION_Note_MarkAsUnread);
                            }
                            return retval;

                        };
                        var action = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //     
                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('MarkAsUnreaded_Question'),
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
                                                'NoteIsReaded': false,
                                                'ObjectList': self.getItemInfos(list)
                                            };
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    contentType: 'application/json',
                                                    method: 'PUT',
                                                    data: JSON.stringify(data),
                                                    url: list[0].Uri + '/notes/status'
                                                },
                                                function(response) {
                                                    if(!response)
                                                        return;

                                                    list.forEach(function (el) {
                                                        el.UnreadMessageCount = el.NoteCount + el.MessageCount;
                                                        $(document).trigger('local_objectUpdated', [tableViewModel.getObjectClassID(el), tableViewModel.getMainObjectID(el)]);
                                                    });
                                                    self.clearSelection();
                                                });
                                        }
                                    });
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('MarkAsUnreaded');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    function toggleCustomControl(underControl, selectedItems, selectedUser) {
                        var onSuccess = function (item) {
                            item.Object.InControl = underControl;
                            $(document).trigger('local_objectUpdated', [tableViewModel.getObjectClassID(item.Object), tableViewModel.getMainObjectID(item.Object)]);
                        };
                        var onComplete = function (item) {
                            hideSpinner();
                            self.clearSelection();
                        };

                        function mapItem(item) {
                            return Object.assign(item, {
                                Uri: item.ClassID == 823
                                    ? '/api/massIncidents/' + item.IMObjID
                                    : item.Uri
                            });
                        }

                        var viewModel = selectedUser
                            ? new groupOperation.CustomControlViewModel(
                                selectedItems.map(mapItem),
                                [selectedUser],
                                underControl,
                                onSuccess,
                                onComplete)
                            : new groupOperation.MyCustomControlViewModel(
                                selectedItems.map(mapItem),
                                underControl,
                                onSuccess,
                                onComplete);
                        showSpinner();
                        viewModel.start();
                    }

                    self.setCustomControl = function (contextMenu) {
                        var isEnable = function () {
                            if (self.getSelectedItems().length == 0)
                                return false;
                            //
                            if (self.userHasRoles && !self.isClientListActive())
                                return true;
                            else {
                                var retval = true;
                                if (!self.isClientListVisible())
                                    self.getSelectedItems().forEach(function (el) {
                                        if (el.InControl === true)
                                            retval = false;
                                    });
                                return retval;
                            }
                        };
                        //
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() == true)
                                return false;
                            return self.operationIsGranted(module.Operations.OPERATION_Control_Add);
                        };
                        //
                        var actionForEngineer = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //
                            var callback = function (selectedUserList) {
                                
                                if (!selectedUserList || selectedUserList.length == 0)
                                    return;

                                toggleCustomControl(true, self.getSelectedItems(), selectedUserList);
                            };
                            //
                            require(['sdForms'], function (fhModule) {
                                var fh = new fhModule.formHelper();
                                var selectedItem = self.getSelectedItems().length != 1
                                    ? {}
                                    : self.getSelectedItems()[0];
                                var userInfo = {
                                    UserID: self.userID,
                                    CustomControlObjectID: selectedItem.IMObjID,
                                    CustomControlObjectClassID: selectedItem.ClassID,
                                    SetCustomControl: true,
                                    UseTOZ: !self.isClientListVisible()
                                };
                                fh.ShowUserSearchForm(userInfo, callback);
                            });
                        };
                        //
                        var actionForClient = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //
                            toggleCustomControl(true, list);
                        };
                        //
                        var action = function () {
                            if (self.isClientListVisible())
                                actionForEngineer()
                            else
                                self.userHasRoles && !self.isClientListActive() ? actionForEngineer() : actionForClient();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('SetCustomControl');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.removeCustomControl = function (contextMenu) {
                        var isEnable = function () {
                            if (self.userHasRoles && !self.isClientListActive()) {
                                if (self.getSelectedItems().length !== 1)
                                    return false;
                                //
                                return true;
                            }
                            else {
                                if (self.getSelectedItems().length == 0)
                                    return false;
                                //
                                var retval = true;
                                if (!self.isClientListVisible())
                                    self.getSelectedItems().forEach(function (el) {
                                        if (el.InControl === false)
                                            retval = false;
                                    });
                                return retval;
                            }
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() == true)
                                return false;
                            return self.operationIsGranted(module.Operations.OPERATION_Control_Remove);
                        };
                        var actionForEngineer = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //
                            var callback = function (selectedUserList) {
                                
                                if (!selectedUserList || selectedUserList.length == 0)
                                    return;
                                
                                toggleCustomControl(false, self.getSelectedItems(), selectedUserList);
                            };
                            //
                            require(['sdForms'], function (fhModule) {
                                var fh = new fhModule.formHelper();
                                var selectedItem = self.getSelectedItems()[0];
                                var userInfo = {
                                    UserID: self.userID,
                                    CustomControlObjectID: selectedItem.IMObjID,
                                    CustomControlObjectClassID: selectedItem.ClassID,
                                    SetCustomControl: false,
                                    UseTOZ: !self.isClientListVisible()
                                };
                                fh.ShowUserSearchForm(userInfo, callback);
                            });
                        };
                        //
                        var actionForClient = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //   
                            toggleCustomControl(false, list);
                        };
                        //
                        var action = function () {
                            if (self.isClientListVisible())
                                actionForEngineer()
                            else
                                self.userHasRoles && !self.isClientListActive() ? actionForEngineer() : actionForClient();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('RemoveCustomControl');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.add = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() == true)
                                return false;
                            var operationID = -1;
                            var viewName = tableViewModel.listView.options.settingsName();
                            if (viewName == 'CallForTable')
                                operationID = 309; //OPERATION_Call_Add = 309
                            else if (viewName == 'ClientCallForTable')
                                operationID = 0; //always available
                            else if (viewName == 'ProblemForTable')
                                operationID = 319; //OPERATION_Problem_Add = 319
                            else if (viewName == 'RFCForTable')
                                operationID = 384; //OPERATION_RFC_Add = 384
                            else if (viewName == 'AllMassIncidentsList')
                                operationID = module.Operations.OPERATION_MassIncident_Add;
                            else
                                operationID = module.Operations.OPERATION_WorkOrder_Add;
                            if ((viewName == 'NegotiationForTable') || (viewName == 'CustomControlForTable') || (viewName == 'CommonForTable'))
                                return false;
                            if (!self.operationIsGranted(operationID))
                                return false;
                            //
                            return true;
                        };
                        var action = function () {
                            showSpinner();
                            require(['registrationForms'], function (lib) {
                                var fh = new lib.formHelper(true);
                                //
                                var viewName = tableViewModel.listView.options.settingsName();
                                if (viewName == 'CallForTable')
                                    fh.ShowCallRegistrationEngineer();
                                else if (viewName == 'ClientCallForTable')
                                    fh.ShowCallRegistration();
                                else if (viewName == 'ProblemForTable')
                                    fh.ShowProblemRegistration();
                                else if (viewName == 'WorkOrderForTable')
                                    fh.ShowWorkOrderRegistration();
                                else if (viewName == 'RFCForTable')
                                    fh.ShowRFCRegistration();
                                else if (viewName == 'AllMassIncidentsList')
                                    fh.ShowMassIncidentRegistration();
                                else fh.ShowWorkOrderRegistration();

                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    self.remove = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length > 0;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() == true)
                                return false;
                            var retval = true;
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if ((classID == 701 && !self.operationIsGranted(module.Operations.OPERATION_Call_Delete)) ||
                                    (classID == 702 && !self.operationIsGranted(module.Operations.OPERATION_Problem_Delete)) ||
                                    (classID == 119 && !self.operationIsGranted(module.Operations.OPERATION_WorkOrder_Delete)) ||
                                    (classID == 703 && !self.operationIsGranted(module.Operations.OPERATION_RFC_Delete)) ||
                                    (classID == 823 && !self.operationIsGranted(module.Operations.OPERATION_MassIncident_Delete)))
                                    retval = false;
                            });
                            return retval;
                        };
                        var deleteGroupViewModel = function (items) {
                            var that = this;

                            groupOperation.ViewModelBase.call(
                                that,
                                items, {
                                    ajax: { contentType: false, method: 'DELETE' },
                                    div: null
                            });

                            that._onComplete = self.clearSelection;
                            that._onSuccess = function (item) {
                                $(document).trigger('local_objectDeleted', [item.ClassID, item.ID]);
                            }
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
                                    if (value === true) {                                        
                                        var items = self.getItemInfos(list);
                                        var groupOperation = new deleteGroupViewModel(items);
                                        groupOperation.start();
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
                    self.transfer = function (contextMenu) {
                        var isValidItem = function (el) {
                            if ((el.ClassID == 701 && !self.operationIsGranted(module.Operations.OPERATION_Call_Transmit)) ||
                                    (el.ClassID == 702 && !self.operationIsGranted(module.Operations.OPERATION_Problem_Transmit)) ||
                                    (el.ClassID == 119 && !self.operationIsGranted(module.Operations.OPERATION_WorkOrder_Transmit)) ||
                                    (el.ClassID == 703 && !self.operationIsGranted(module.Operations.OPERATION_RFC_Transmit)) ||
                                    (el.ClassID == 823) && !self.operationIsGranted(module.Operations.OPERATION_MassIncident_Transmit))
                                    return false;
                                return true;
                        }
                        var isEnable = function () {
                            var enebled = true;
                            if (self.getSelectedItems().length == 0)
                                return false;
                            if (self.getSelectedItems().length == 1) {
                                var el = self.getSelectedItems()[0];
                                return isValidItem(el);
                            }
                            self.getSelectedItems().forEach(function (el) {
                                if (!isValidItem(el))
                                    enabled = false;

                            });
                            return enebled;
                        };
                        
                        var isVisible = function () {
                            let visible = true;
                            if (self.isBudgetRowListActive() == true || self.isClientListVisible() == true)
                                return false;
                            self.getSelectedItems().forEach(function (el) {
                                if (!isValidItem(el))
                                    visible = false;
                            });
                            //
                            return visible;
                        };

                        var transferToUser = function () {
                            var classID = tableViewModel.getObjectClassID();
                            if (self.getSelectedItems().length == 0)
                                return;

                            var transferToExecutor = classID == 119 || classID == 701 || classID == 702 || classID == 823;

                            var callback = function (selectedUser) {
                                
                                if (!selectedUser)
                                    return;

                                require(['sweetAlert'], () => {
                                swal({
                                    title: getTextResource(data = transferToExecutor ? 'TransferToExecutorQuestion' : 'TransferToOwnerQuestion'),
                                    text: `${getTextResource('ForItemNumbers')} ${self.getSelectedItems().map(el => el.Number).join()} 
                                            ${getTextResource(data = transferToExecutor ? 'ExecutorWillBeChanged' : 'OwnerWillBeChanged')} ${selectedUser.FullName}`,
                                    showCancelButton: true,
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                    (value) => {
                                        if (value) {
                                            showSpinner();
                                            let data = transferToExecutor
                                                ? { ExecutorID: selectedUser.ID }
                                                : { OwnerID: selectedUser.ID };

                                            let list = self.getSelectedItems();
                                            var updateNext = function (i) {
                                                if (i == list.length) {
                                                    hideSpinner();
                                                    self.clearSelection();
                                                    return;
                                                }
                                                let el = list[i];
                                                self.ajaxControl.Ajax(null,
                                                    {
                                                        dataType: 'json',
                                                        contentType: 'application/json',
                                                        method: 'PUT',
                                                        data: JSON.stringify(data),
                                                        url: el.Uri
                                                    }, function () { 
                                                        updateNext(i+1); 
                                                    });
                                            }
                                            updateNext(0);
                                        }
                                    });
                                })                                
                             }
    
                            require(['sdForms'], function (fhModule) {
                                let title = transferToExecutor ? 'Executor' : 'Owner';
                                var fh = new fhModule.formHelper();
                                var userInfo = { UserID: self.userID, ShowCheckboxes: false, Title: getTextResource(title), UseTOZ: true, SetTransferOwner: true};
                                fh.ShowUserSearchForm(userInfo, callback);
                            });
                        };

                        var transferToGroup = function () {
                            var classID = tableViewModel.getObjectClassID();
                            if (self.getSelectedItems().length == 0)
                                return;

                            var callback = function (selected) {
                                
                                if (!selected)
                                    return;

                                require(['sweetAlert'], () => {
                                swal({
                                    title: getTextResource('TransferToGroupQuestion'),
                                    text: `${getTextResource('ForItemNumbers')} ${self.getSelectedItems().map(el => el.Number).join()} 
                                            ${getTextResource('GroupBeChanged')} ${selected.Queue.FullName}`,
                                    showCancelButton: true,
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                    (value) => {
                                        if (value) {
                                            showSpinner();
                                            let data = {};
                                            if (selected.User) {
                                                if (classID == 119) {
                                                    data = { QueueID: selected.Queue.ID, ExecutorID: selected.User.ID };
                                                } else if (classID == 823) {
                                                    data = { GroupID: selected.Queue.ID, ExecutorID: selected.User.ID };
                                                } else {
                                                    data = { QueueID: selected.Queue.ID, OwnerID: selected.User.ID };
                                                }
                                            }
            
                                            let list = self.getSelectedItems();
                                            var updateNext = function (i) {
                                                if (i == list.length) {
                                                    hideSpinner();
                                                    self.clearSelection();
                                                    return;
                                                }
                                                let el = list[i];
                                                self.ajaxControl.Ajax(null,
                                                    {
                                                        dataType: 'json',
                                                        contentType: 'application/json',
                                                        method: 'PUT',
                                                        data: JSON.stringify(data),
                                                        url: el.Uri
                                                    }, function () { 
                                                        updateNext(i+1); 
                                                    });
                                            }
                                            updateNext(0);
                                        }
                                    });
                                })                                
                             }

                            require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                                var selected = self.getSelectedItems()[0];
                                var classID = tableViewModel.getObjectClassID(selected);
                                var fh = new module.formHelper(true);
                                var options = {
                                    ID: tableViewModel.getMainObjectID(selected),
                                    objClassID: classID,
                                    fieldName: fieldName(classID),
                                    fieldNameQueue: fieldNameQueue(classID),
                                    fieldFriendlyName: fieldFriendlyName(classID),
                                    oldValue: selected.OwnerID ? { ID: selected.OwnerID, ClassID: 9, FullName: selected.ExecutorFullName ? selected.ExecutorFullName : selected.OwnerFullName } : null,
                                    oldValueQueue: selected.QueueID ? { ID: selected.QueueID, ClassID: 722, FullName: selected.QueueName } : null,
                                    object: ko.toJS(selected),
                                    searcherName: searcherName(classID) ,
                                    searcherNameQueue: 'QueueSearcher',
                                    searcherPlaceholder: getTextResource('EnterFIO'),
                                    searcherPlaceholderQueue: getTextResource('EnterQueue'),
                                    searcherParams: searcherParams(classID),
                                    searcherParamsQueue: searcherParamsQueue(classID),
                                    nosave: true,
                                    onSave: callback
                                };

                                fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEditWithQueue, options);

                                function fieldName(classID) {
                                    if (classID == 119) {
                                        return 'WorkOrder.Executor';
                                    } else if (classID == 823) {
                                        return 'MassIncident.Owner';
                                    } else {
                                        return 'Call.Owner';
                                    }
                                }

                                function fieldNameQueue(classID) {
                                    if (classID == 119) {
                                        return 'WorkOrder.Queue';
                                    } else if (classID == 823) {
                                        return 'MassIncident.Queue';
                                    } else {
                                        return null;
                                    }
                                }

                                function fieldFriendlyName(classID) {
                                    if (classID == 119) {
                                        return getTextResource('Executor');
                                    } else if (classID == 823) {
                                        return getTextResource('Executor');
                                    } else {
                                        return getTextResource('Owner');
                                    }
                                }

                                function searcherName(classID) {
                                    if (classID == 119) {
                                        return 'ExecutorUserSearcher'
                                    } else if (classID == 823) {
                                        return 'ExecutorUserSearcher';
                                    } else {
                                        return 'OwnerUserSearcher';
                                    }
                                }

                                function searcherParams(classID) {
                                    if (classID == 119 || classID == 701 || classID == 823) {
                                        return { QueueId: selected.QueueID == '' ? null : selected.QueueID }
                                    } else {
                                        return null;
                                    }
                                }

                                function searcherParamsQueue(classID) {
                                    if (classID == 119) {
                                        return { Type: 2};
                                    } else if (classID == 823) {
                                        return { Type: 4 };
                                    } else {
                                        return { Type: 0 };
                                    }
                                }
                            });
                        };

                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Transfer');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        var child = contextMenu.addContextMenuItem();
                        child.restext('ToExecutor');
                        child.isEnable = isEnable;
                        child.isVisible = function () { return false; };
                        child.click(transferToUser);
                        cmd.children.push(child);
                        child = contextMenu.addContextMenuItem();
                        child.restext('ToGroup'); 
                        child.isEnable = function () 
                        { 
                            const allowedClasses = [119, 701, 703, 823];
                            for (var i = 0; i < self.getSelectedItems().length; i++) {
                                var classID = tableViewModel.getObjectClassID(self.getSelectedItems()[i]);
                                if (!allowedClasses.includes(classID)) return false;
                            }
                            return true;
                        };
                        child.isVisible = function () {return false;}
                        child.click(transferToGroup);
                        cmd.children.push(child); 
                    };
                    //
                    self.addAs = function (contextMenu) {
                        const isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        const isVisible = function () {
                            if (self.isBudgetRowListActive() == true)
                                return false;
                            if (self.getSelectedItems().length != 1)
                                return false;
                            const selected = self.getSelectedItems()[0];
                            const classID = tableViewModel.getObjectClassID(selected);
                            if ((classID == 701 && !self.operationIsGranted(module.Operations.OPERATION_Call_AddAs)) ||
                                (classID == 702 && !self.operationIsGranted(module.Operations.OPERATION_Problem_AddAs)) ||
                                (classID == 119 && !self.operationIsGranted(module.Operations.OPERATION_WorkOrder_AddAs)) ||
                                (classID == 703 && !self.operationIsGranted(module.Operations.OPERATION_RFC_AddAs)) ||
                                (classID == 823 && !self.operationIsGranted(module.Operations.OPERATION_Massincident_AddAs)))
                                return false;
                            //
                            return true;
                        };
                        const action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            const selected = self.getSelectedItems()[0];
                            const classID = tableViewModel.getObjectClassID(selected);
                            //     
                            const objectId =  selected.ID;
                            const fakeWrapper = {
                                CanEdit: ko.observable(true),
                                CanShow: ko.observable(true),
                                IsClientMode: ko.observable(false),
                                IsReadOnly: ko.observable(false)
                            };
                            let objectName = '';
                            switch (classID) {
                                case 701:
                                    objectName = 'Calls';
                                    break;
                                case 702:
                                    objectName = 'Problems';
                                    break;
                                case 119:
                                    objectName = 'WorkOrders';
                                    break;
                                case 703:
                                    objectName = 'ChangeRequests';
                                    break;
                                case 823:
                                    objectName = 'MassIncidents';
                                    break;
                            }
                            if (objectName.length > 0) {
                                const apiUrl = `api/${objectName}/${objectId}`;

                                self.ajaxControl.Ajax(null,
                                    {
                                        dataType: "json",
                                        method: 'GET',
                                        url: apiUrl
                                    },
                                    function (newVal) {
                                        if (newVal) {
                                            const objInfo = newVal;
                                            if (objInfo.ID) {
                                                switch (classID) {
                                                    case 701:
                                                        require(['registrationForms', 'models/SDForms/CallForm.Call'], function (lib, callLib) {
                                                            const fh = new lib.formHelper(true);
                                                            fh.ShowCallRegistrationEngineer(null, null, null, new callLib.Call(fakeWrapper, objInfo));
                                                        });
                                                        break;
                                                    case 702:
                                                        require(['registrationForms', 'models/SDForms/ProblemForm.Problem'], function (lib, problemLib) {
                                                            const fh = new lib.formHelper(true);
                                                            fh.ShowProblemRegistration(null, null, null, new problemLib.Problem(fakeWrapper, objInfo));
                                                        });
                                                        break;
                                                    case 119:
                                                        require(['registrationForms', 'models/SDForms/WorkOrderForm.WorkOrder'], function (lib, woLib) {
                                                            const fh = new lib.formHelper(true);
                                                            fh.ShowWorkOrderRegistration(null, null, new woLib.WorkOrder(fakeWrapper, objInfo));
                                                        });
                                                        break;
                                                    case 703:
                                                        require(['registrationForms', 'models/SDForms/RFCForm.RFC'], function (lib, rfcLib) {
                                                            const fh = new lib.formHelper(true);
                                                            fh.ShowRFCRegistration(null, null, new rfcLib.RFC(fakeWrapper, objInfo));
                                                        });
                                                        break;
                                                    case 823:
                                                        require(['registrationForms', 'models/SDForms/MassIncidentForm.MassIncident'], function (lib, miLib) {
                                                            const fh = new lib.formHelper(true);
                                                            fh.ShowMassIncidentRegistration(null, new miLib.MassIncident(fakeWrapper, objInfo));
                                                        });
                                                        break;
                                                }
                                                self.clearSelection();
                                            }
                                        }
                                    });
                            }
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('AddAs');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    self.pickFromQueue = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length > 0;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() == true)
                                return false;
                            var retval = true;
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if (!el.CanBePicked ||
                                    (classID == 701 && !self.operationIsGranted(module.Operations.OPERATION_Call_Pick)) ||
                                    (classID == 119 && !self.operationIsGranted(module.Operations.OPERATION_WorkOrder_Pick)))
                                    retval = false;
                            });
                            return retval;
                        };
                        var action = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //     
                            var data = {
                                'ObjectList': self.getItemInfos(list)
                            };
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/sdApi/PickObjects'
                                },
                                function (newVal) {
                                    list.forEach(function (el) {
                                        el.CanBePicked = false;
                                        $(document).trigger('local_objectUpdated', [tableViewModel.getObjectClassID(el), tableViewModel.getMainObjectID(el)]);
                                    });
                                    self.clearSelection();
                                });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('PickFromQueue');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    }; 
                    //
                    self.createProblem = function (contextMenu) {
                        var isEnable = function () {
                            var selectedItemsCount = self.getSelectedItems().length;
                            var retval = selectedItemsCount > 0;
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if (classID != 701 /* Call */ && classID != 823  /* MassIncident */)
                                    retval = false;
                                if (classID == 823 && selectedItemsCount != 1)
                                    retval = false;
                                if (!self.operationIsGranted(module.Operations.OPERATION_Problem_Add))
                                    retval = false;
                            });
                            if (!self.userHasRoles)
                                retval = false;
                            return retval;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() == true || self.isClientListVisible() == true)
                                return false;
                            if (!self.operationIsGranted(module.Operations.OPERATION_Problem_Add))
                                return false;
                            var retval = self.getSelectedItems().length > 0;
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if (classID != 701 /* Call */ && classID != 823 /* MassIncident */)
                                    retval = false;
                                if (!self.operationIsGranted(module.Operations.OPERATION_Problem_Add))
                                    retval = false;
                            });
                            if (!self.userHasRoles)
                                retval = false;
                            return retval;
                            //При необходимости отображать везде кроме "заявки" недоступный пункт меню "создать проблему" - расскоментировать 
                            //return self.operationIsGranted(module.Operations.OPERATION_Problem_Add); 
                        };
                        var action = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //     
                            var data = {
                                'ObjectList': self.getItemInfos(list)
                            };

                            var getServiceInfo = function (serviceID) {
                                return new Promise(function (resolve, reject) {
                                    self.ajaxControl.Ajax(null,
                                        {
                                            dataType: "json",
                                            method: 'GET',
                                            url: '/api/services/' + serviceID
                                        },
                                        function (serviceObject) {
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    dataType: "json",
                                                    method: 'GET',
                                                    url: serviceObject.CategoryUri
                                                },
                                                function (serviceCategoryObject) {
                                                    resolve({
                                                        serviceID: serviceID,
                                                        serviceName: serviceObject.Name,
                                                        categoryID: serviceCategoryObject.ID,
                                                        categoryName: serviceCategoryObject.Name
                                                    });
                                                },
                                                function (err) {
                                                    reject(err);
                                                });
                                        },
                                        function (err) {
                                            reject(err);
                                        });
                                });
                            };

                            var url = '/sdApi/CreateProblemByCall';
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if (classID == 823) { // MassIncident 
                                    const selected = self.getSelectedItems()[0];
                                    const apiUrl = `api/massincidents/${selected.ID}`;
                                    self.ajaxControl.Ajax(null,
                                        {
                                            dataType: "json",
                                            method: 'GET',
                                            url: apiUrl
                                        },
                                        function (newVal) {
                                            if (newVal) {
                                                const objInfo = newVal;
                                                if (objInfo.ID) {
                                                    objInfo.MassIncidentId = selected.ID;
                                                    getServiceInfo(objInfo.ServiceID).then(function (serviceInfo) {
                                                        objInfo.ServiceName = serviceInfo.serviceName;
                                                        objInfo.ServiceCategoryID = serviceInfo.categoryID;
                                                        objInfo.ServiceCategoryName = serviceInfo.categoryName;
                                                        objInfo.Summary = selected.ShortDescription;
                                                        var fakeWrapper = {
                                                            CanEdit: ko.observable(true),
                                                            CanShow: ko.observable(true),
                                                            IsClientMode: ko.observable(false),
                                                            IsReadOnly: ko.observable(false)
                                                        };
                                                        require(['registrationForms', 'models/SDForms/ProblemForm.Problem'], function (lib, problemLib) {
                                                            const fh = new lib.formHelper(true);
                                                            fh.ShowProblemRegistration(null, null, null, new problemLib.Problem(fakeWrapper, objInfo));
                                                        });
                                                    })
                                                }
                                            }
                                        });
                                
                                } else if (classID == 701) {
                                    const selected = self.getSelectedItems()[0];
                                    const apiUrl = `api/calls/${selected.ID}`;
                                    self.ajaxControl.Ajax(null,
                                        {
                                            dataType: "json",
                                            method: 'GET',
                                            url: apiUrl
                                        },
                                        function (newVal) {
                                            if (newVal) {
                                                const objInfo = newVal;
                                                if (objInfo.ID) {
                                                    objInfo.CallList = self.getSelectedItems().map(item => item.ID);
                                                    getServiceInfo(objInfo.ServiceID).then(function (serviceInfo) {
                                                        objInfo.ServiceName = serviceInfo.serviceName;
                                                        objInfo.ServiceCategoryID = serviceInfo.categoryID;
                                                        objInfo.ServiceCategoryName = serviceInfo.categoryName;
                                                        objInfo.Summary = selected.Summary;
                                                        var fakeWrapper = {
                                                            CanEdit: ko.observable(true),
                                                            CanShow: ko.observable(true),
                                                            IsClientMode: ko.observable(false),
                                                            IsReadOnly: ko.observable(false)
                                                        };
                                                        require(['registrationForms', 'models/SDForms/ProblemForm.Problem'], function (lib, problemLib) {
                                                            const fh = new lib.formHelper(true);
                                                            fh.ShowProblemRegistration(null, null, null, new problemLib.Problem(fakeWrapper, objInfo));
                                                        });

                                                        self.clearSelection();
                                                    });
                                                }
                                            }
                                        });
                                }
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('ButtonCreateProblem');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.createRFC = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            var viewName = tableViewModel.listView.options.settingsName();
                            if (viewName == 'NegotiationForTable')
                                return false;
                            var selected = self.getSelectedItems()[0];
                            var classID = tableViewModel.getObjectClassID(selected);
                            if ((classID == 701 || classID == 702 || classID == 119) && self.operationIsGranted(384))
                                return true;
                            //
                            return false;
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            var selected = self.getSelectedItems()[0];
                            //    
                            require(['registrationForms'], function (lib) {
                                var fh = new lib.formHelper(true);
                                fh.ShowRFCRegistration(selected.ClassID,selected.ID);
                            });
                            self.clearSelection();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('ButtonAddRFC');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.createMassIncident = function (contextMenu) {
                        var isEnable = function () {
                            var retval = true;
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if (classID != 701) {
                                    retval = false;
                                }
                            });
                            return retval;
                        };
                        var isVisible = function () {
                            let viewName = tableViewModel.listView.options.settingsName();
                            if (!(viewName == 'CommonForTable' || viewName === 'CallForTable'))
                                return false;
                            var selected = self.getSelectedItems()[0];
                            var classID = tableViewModel.getObjectClassID(selected);
                            if (classID == 701 && self.operationIsGranted(module.Operations.OPERATION_MassIncident_Add))
                                return true;
                            return false;
                        };
                        var action = function () {
                            var result = true;
                            var callIds = [];
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if (classID != 701) {
                                    retval = false;
                                } else {
                                    callIds.push(tableViewModel.getMainObjectID(el));
                                }
                            });

                            if (result) {
                                require(['registrationForms'], function (lib) {
                                    var fh = new lib.formHelper(true);
                                    fh.ShowMassIncidentRegistration({ calls: callIds });
                                });
                                self.clearSelection();
                            }
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('CreateMassIncidentLabel');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.propertiesBudgetRow = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() != true ||
                                self.getSelectedItems().length != 1 ||
                                tableViewModel.SelectedFinanceBudget() == null)
                                return false;
                            if (!self.operationIsGranted(856))
                                return false;

                            return true;
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var selected = self.getSelectedItems()[0];
                            var id = tableViewModel.getMainObjectID(selected);
                            var classID = tableViewModel.getObjectClassID(selected);
                            //     
                            tableViewModel.showObjectForm(classID, id);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('Properties');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.addBudgetRow = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() != true ||
                                tableViewModel.SelectedFinanceBudget() == null || tableViewModel.SelectedFinanceBudget().State != 0)
                                return false;
                            if (!self.operationIsGranted(857))
                                return false;
                            //
                            return true;
                        };
                        var action = function () {
                            $.when(userD).done(function (user) {
                                if (user.FinanceBudgetID == null) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('FinanceBudget_PromptBudget').replace('<', '').replace('>', ''), 'info');
                                    });
                                    return;
                                }
                                //
                                tableViewModel.showObjectForm(180, null);
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
                    self.removeBudgetRow = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length > 0;
                        };
                        var isVisible = function () {
                            var retval = true;
                            if (self.isBudgetRowListActive() != true ||
                                self.getSelectedItems().length == 0 ||
                                tableViewModel.SelectedFinanceBudget() == null || tableViewModel.SelectedFinanceBudget().State != 0)
                                return false;
                            if (!self.operationIsGranted(858))
                                return false;
                            //
                            return true;
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
                                            var ids = self.getItemIDs(list);
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    dataType: "json",
                                                    method: 'POST',
                                                    data: { IDList: ids, AdjustmentContext: undefined },
                                                    url: '/finApi/deleteFinanceBudgetRow'
                                                },
                                                //function (newVal) {
                                                //    data.ObjectList.forEach(function (info) {
                                                //        $(document).trigger('local_objectDeleted', [info.ClassID, info.ID]);
                                                //    });
                                                //    self.clearSelection();
                                                //});
                                                function (response) {
                                                    hideSpinner();
                                                    if (response) {
                                                        if (response.Result == 0) {//ok                                            
                                                            for (var i = 0; i < ids.length; i++)
                                                                $(document).trigger('local_objectDeleted', [180, ids[i], null]);//OBJ_BudgetRow
                                                            //
                                                            tableViewModel.setSizeOfControls();
                                                            //
                                                            if (response.Message && response.Message.length > 0)
                                                                require(['sweetAlert'], function () {
                                                                    swal({
                                                                        title: response.Message,
                                                                        showCancelButton: false,
                                                                        closeOnConfirm: true,
                                                                        cancelButtonText: getTextResource('Continue')
                                                                    });
                                                                });
                                                        }
                                                        else if (response.Result === 3)
                                                            require(['sweetAlert'], function () {
                                                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                                            });
                                                        else if (response.Result === 7) {
                                                            require(['sweetAlert'], function () {
                                                                swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                                            });
                                                        }
                                                    }
                                                },
                                                function (response) {
                                                    hideSpinner();
                                                    require(['sweetAlert'], function () {
                                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Finance/Table.SelectedItems.js, Delete]', 'error');
                                                    });
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
                    //for adjustment
                    //semi like properties
                    self.propertiesBudgetRowAdjustment = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() != true ||
                                self.getSelectedItems().length != 1 ||
                                tableViewModel.SelectedFinanceBudget() == null || tableViewModel.SelectedFinanceBudget().State != 1)
                                return false;
                            if (!self.operationIsGranted(859) || !self.operationIsGranted(856))
                                return false;
                            //
                            return true;
                        };
                        var action = function () {
                            if (self.getSelectedItems().length != 1)
                                return false;
                            //
                            var selected = self.getSelectedItems()[0];
                            var id = tableViewModel.getMainObjectID(selected);
                            var classID = tableViewModel.getObjectClassID(selected);
                            //     
                            tableViewModel.showObjectForm(classID, id);
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('FinanceBudgetRowAdjustment_Modify');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    //semi like add
                    self.addBudgetRowAdjustment = function (contextMenu) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            if (self.isBudgetRowListActive() != true ||
                                tableViewModel.SelectedFinanceBudget() == null || tableViewModel.SelectedFinanceBudget().State != 1)
                                return false;
                            if (!self.operationIsGranted(857))
                                return false;
                            //
                            return true;
                        };
                        var action = function () {
                            $.when(userD).done(function (user) {
                                if (user.FinanceBudgetID == null) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('FinanceBudget_PromptBudget').replace('<', '').replace('>', ''), 'info');
                                    });
                                    return;
                                }
                                //     
                                tableViewModel.showObjectForm(180, null);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('FinanceBudgetRowAdjustment_Add');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.sendToEmailContextMenuItem = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1;
                        };
                        var isVisible = function () {
                            if (self.isBudgetListActive() == true)
                                return false;
                            if (!self.userHasRoles)
                                return false;

                            var retval = true;
                            self.getSelectedItems().forEach(function (el) {
                                var classID = tableViewModel.getObjectClassID(el);
                                if (classID == 823) {
                                    retval = false;
                                }
                            });
                            return retval;
                        };
                        var action = function () {
                            showSpinner();
                            require(['sdForms'], function (module) {
                                var fh = new module.formHelper(true);
                                var tmpObj = self.getSelectedItems()[0];
                                var options = {
                                    Obj: {
                                        ID: self.isNegotiationListVisible() ? tmpObj.ObjectID : tmpObj.ID,
                                        ClassID: tmpObj.ClassID,
                                        ClientID: (self.userID == tmpObj.OwnerID || self.userID == tmpObj.ExecutorID) ? tmpObj.ClientID : '',
                                        ClientEmail: (self.userID == tmpObj.OwnerID || self.userID == tmpObj.ExecutorID) ? tmpObj.ClientEmail : '',
                                    },
                                    CanNote: true
                                };
                                fh.ShowSendEmailForm(options);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('SendToEmail');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //

                    self.removeBudgetRowAdjustment = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length == 1;
                        };
                        var isVisible = function () {
                            var retval = true;
                            if (self.isBudgetRowListActive() != true ||
                                self.getSelectedItems().length != 1 ||
                                tableViewModel.SelectedFinanceBudget() == null || tableViewModel.SelectedFinanceBudget().State != 1)
                                return false;
                            if (!self.operationIsGranted(858))
                                return false;
                            //
                            return true;
                        };
                        var action = function () {
                            var list = self.getSelectedItems();
                            if (list.length == 0)
                                return;
                            //
                            var selected = list[0];
                            var id = tableViewModel.getMainObjectID(selected);
                            var classID = tableViewModel.getObjectClassID(selected);
                            //     
                            showSpinner();
                            self.ajaxControl.Ajax(null,
                                {
                                    url: '/finApi/GetFinanceBudgetRow',
                                    method: 'GET',
                                    data: { FinanceBudgetRowID: id }
                                },
                                function (bugetResult) {
                                    if (bugetResult.Result === 0 && bugetResult.Data) {
                                        var obj = bugetResult.Data;
                                        //
                                        require(['financeForms'], function (module) {
                                            var fh = new module.formHelper(true);
                                            $.when(fh.ShowFinanceBudgetRowAdjustment(null, obj, null)).done(function (adjustment) {
                                                if (adjustment == undefined)
                                                    return;
                                                //
                                                showSpinner();
                                                self.ajaxControl.Ajax(null,
                                                    {
                                                        url: '/finApi/deleteFinanceBudgetRow',
                                                        method: 'POST',
                                                        data: { IDList: [id], AdjustmentContext: adjustment }
                                                    },
                                                    function (response) {
                                                        hideSpinner();
                                                        if (response) {
                                                            if (response.Result == 0) {//ok                                            
                                                                $(document).trigger('local_objectDeleted', [180, id, null]);//OBJ_BudgetRow
                                                                //
                                                                tableViewModel.setSizeOfControls();
                                                                //
                                                                if (response.Message && response.Message.length > 0)
                                                                    require(['sweetAlert'], function () {
                                                                        swal({
                                                                            title: response.Message,
                                                                            showCancelButton: false,
                                                                            closeOnConfirm: true,
                                                                            cancelButtonText: getTextResource('Continue')
                                                                        });
                                                                    });
                                                            }
                                                            else if (response.Result === 3)
                                                                require(['sweetAlert'], function () {
                                                                    swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                                                });
                                                            else if (response.Result === 7) {
                                                                require(['sweetAlert'], function () {
                                                                    swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                                                });
                                                            }
                                                            else if (response.Result === 8) {
                                                                require(['sweetAlert'], function () {
                                                                    swal(getTextResource('ErrorCaption'), response.Message && response.Message.length > 0 ? response.Message : getTextResource('ValidationError'), 'error');
                                                                });
                                                            }
                                                        }
                                                    },
                                                    function (response) {
                                                        hideSpinner();
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Finance/Table.ContextMenu.js, Remove]', 'error');
                                                        });
                                                    });
                                            });
                                        });
                                    }
                                    else {
                                        hideSpinner();
                                        //
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Finance/Table.ContextMenu.js, RemoveAsAdjustment]', 'error');
                                        });
                                    }
                                });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('FinanceBudgetRowAdjustment_Remove');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    self.changeState = function (contextMenu) {
                        let entityStates = [];
                        const getAllowedStates = () => entityStates;
                        const isEnable = function () {
                            // Смена состояния для нескольких объектов доступна, если:
                            // 1. Объекты одного класса
                            // 2. Объекты находятся в одном состоянии
                            // 3. Объекты запущены по одной рабочей процедуре
                            
                            const selectedItems = self.getSelectedItems();
                            const anyIsReadonly = selectedItems.some(el => el.ReadOnly);
                            const itemsHaveSameClass = [...new Set(selectedItems.map(el => el.ClassID))].length === 1;
                            const itemsHaveSameStateID = [...new Set(selectedItems.map(el => el.EntityStateID))].length === 1;
                            const itemsHaveSameStateName = [...new Set(selectedItems.map(el => el.EntityStateName))].length === 1;
                            const itemsHaveSameWorkflow = [...new Set(selectedItems.map(el => el.WorkflowSchemeID))].length === 1;
                            
                            if (anyIsReadonly
                                || !itemsHaveSameClass
                                || !itemsHaveSameStateID
                                || !itemsHaveSameStateName
                                || !itemsHaveSameWorkflow) {
                                return false;
                            }
                            
                            entityStates = [];
                            return Promise.all(selectedItems.map(item => fetch('/api/workflows/' + item.ClassID + '/' + item.IMObjID)
                                        .then(response => response.json())
                                        .then(response => {
                                            return Promise.all(response.States.map(state => {
                                                return fetch('/api/workflows/transitionAllowed/' + item.ClassID + '/' + item.IMObjID + '?stateName=' + state.StateID)
                                                    .then(response => response.json())
                                                    .then(transitionIsAllowedResult => {
                                                        state.isAllowed = transitionIsAllowedResult.IsAllowed;
                                                        return state;
                                                    })
                                            })).then(itemStates => itemStates.filter(state => state.isAllowed));  
                                        })
                                ))
                                .then(itemStatesList => {
                                    const firstItemStates = itemStatesList.shift();
                                    firstItemStates.forEach(firstItemState => {
                                        const exists = itemStatesList.reduce(
                                            (exists, itemStates) => exists 
                                            && itemStates.some(itemState => itemState.StateID === firstItemState.StateID),
                                            true);
                                        if (exists) {
                                            entityStates.push(firstItemState);
                                        }
                                    });
                                    return entityStates.length > 0;
                                });
                        };
                        const isVisible = () => true;
                        const changeStateGroupViewModel = function (items) {
                            groupOperation.ViewModelBase.call(
                                this,
                                items, {
                                    ajax: { contentType: false, method: 'PUT' },
                                    div: null,
                                });

                            this._onComplete = self.clearSelectedItem;
                            this._onSuccess = function (item) {
                                $(document).trigger('local_objectUpdated', [item.ClassID, item.ID]);
                            };
                        };
                        const action = function () {
                            const selectedItems = self.getSelectedItems();
                            //const firstItem = selectedItems[0];
                            require(['usualForms'], function (fhModule) {
                                const fh = new fhModule.formHelper(true);
                                const options = {
                                    popupTitle: getTextResource('ChangingState'),
                                    // firstItem.ID - int для MassIncident. Поэтому используем IMObjID - GUID + равен workflowID
                                    comboBoxValueList: getAllowedStates().map(state => ({ID: state.StateID, Name: state.Text})),
                                    onSave: function (selectedValue) {
                                        const popupQuestion = `${getTextResource('ForObjects')}` +
                                            selectedItems.map(el => el.Number).join(', ') + '\r\n' +
                                            getTextResource('ChangeFieldState') + '\r\n' +
                                            '\u2022' + ` ${selectedValue.Name}`;
                                        require(['sweetAlert'], function (swal) {
                                            swal({
                                                    title: `${getTextResource('ChangeState')}?`,
                                                    text: popupQuestion,
                                                    showCancelButton: true,
                                                    closeOnConfirm: false,
                                                    closeOnCancel: true,
                                                    confirmButtonText: getTextResource('ChangeYes'),
                                                    cancelButtonText: getTextResource('ButtonCancel')
                                                },
                                                function (value) {
                                                    swal.close();
                                                    if (value === true) {
                                                        const items = selectedItems.map(function(item) {
                                                            return {
                                                                ClassID: item.ClassID,
                                                                ID: item.IMObjID, // firstItem.ID - int для MassIncident. Поэтому используем IMObjID - GUID + равен workflowID
                                                                Uri: '/api/workflowStates/' + item.ClassID + '/' + item.IMObjID + '?stateName=' + selectedValue.ID,
                                                            }
                                                        });
                                                        const groupOperation = new changeStateGroupViewModel(items);
                                                        groupOperation.start();
                                                    }
                                                });
                                        });
                                    },
                                    comboBoxExpanded: true,
                                    nosave: true
                                };
                                fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                            });
                        };
                        
                        const cmd = contextMenu.addContextMenuItem();
                        cmd.restext('ChangeState');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    }
                }
            }
        },
        //

        Operations: {
            OPERATION_Call_Delete: 310,
            OPERATION_Call_Pick: 701009,
            OPERATION_Call_AddAs: 315,
            OPERATION_CALL_Properties: 518,
            OPERATION_Control_Add: 369,
            OPERATION_Control_Remove: 370,
            OPERATION_Note_MarkAsReaded: 688,
            OPERATION_Note_MarkAsUnread: 689,
            OPERATION_Call_Transmit: 701010,
            OPERATION_Problem_Add: 319,
            OPERATION_Problem_Delete: 320,
            OPERATION_Problem_AddAs: 221,
            OPERATION_Problem_Transmit: 702008,
            OPERATION_Problem_Properties: 222,
            OPERATION_WorkOrder_Add: 301,
            OPERATION_WorkOrder_AddAs: 331,
            OPERATION_WorkOrder_Delete: 330,
            OPERATION_WorkOrder_Pick: 119009,
            OPERATION_WorkOrder_Transmit: 119010,
            OPERATION_WorkOrder_Properties: 302,
            OPERATION_RFC_Add: 384,
            OPERATION_RFC_Delete: 385,
            OPERATION_RFC_AddAs: 378,
            OPERATION_RFC_Properties: 383,
            OPERATION_RFC_Transmit: 703010,
            OPERATION_MassIncident_Properties: 980,
            OPERATION_MassIncident_Add: 982,
            OPERATION_MassIncident_Delete: 984,
            OPERATION_Massincident_AddAs: 1328,
            OPERATION_MassIncident_Transmit: 1330
        }

    }
    return module;
});