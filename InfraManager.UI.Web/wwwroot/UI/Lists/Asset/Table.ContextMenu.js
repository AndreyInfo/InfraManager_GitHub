define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        ViewModel: function (tableViewModel) {
            var self = this;
            //
            self.ajaxControl = new ajaxLib.control();
            //
            {//granted operations
                self.grantedOperations = [];
                $.when(userD).done(function (user) {
                    self.grantedOperations = user.GrantedOperations;
                    self.isAdmin = user.HasAdminRole;
                    self.userHasRoles = user.HasRoles;
                });
                self.operationIsGranted = function (operationID) {
                    for (var i = 0; i < self.grantedOperations.length; i++)
                        if (self.grantedOperations[i] === operationID)
                            return true;
                    return false;
                };
            }
            //
            {//ko.contextMenu
                self.contextMenuInit = function (contextMenu) {
                    tableViewModel.listViewContextMenu(contextMenu);//bind contextMenu
                    //
                    self.properties(contextMenu);
                    contextMenu.addSeparator();
                    //пул
                    self.StructurePool(contextMenu);
                    //Выданные права
                    self.References(contextMenu);
                    //Выдать право
                    self.IssueRights(contextMenu);                    
                    self.ReturnRight(contextMenu);
                    self.TransferRights(contextMenu);
                    
                    
                    //добавление для формы лицензии и других происходит в разных пунктах контекстного меню
                    self.addStandartSoftwareLicence(contextMenu);
                    self.addContractSoftwareLicence(contextMenu);
                    self.add(contextMenu);
                    //
                    self.addCluster(contextMenu);//to remove
                    self.createConfigurationUnit(contextMenu);
                    //
                    self.addAgreement(contextMenu);
                    self.сreateDataEntity(contextMenu);
                    self.createLogicObject(contextMenu);
                    self.remove(contextMenu);
                    contextMenu.addSeparator();
                    self.createCall(contextMenu);
                    self.createWorkOrder(contextMenu);
                    self.createProblem(contextMenu);
                    contextMenu.addSeparator();
                    self.Active(contextMenu);
                    self.Upgrade(contextMenu);
                    self.sendToEmailContextMenuItem(contextMenu);

                    // инсталляции
                    self.createSoftwareInstallation(contextMenu);
                    self.propertySoftwareInstallation(contextMenu);                    
                    self.deleteSoftwareInstallation(contextMenu);
                };
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

                    var viewName = tableViewModel.listView.options.settingsName();
                    if (viewName != 'SoftwareLicenseDistribution') {

                        for (var i = 0; i < oldDynamicItems.length; i++)
                            contextMenu.removeItem(oldDynamicItems[i]);
                        //
                        var waitItem = self.wait(contextMenu);
                        $.when(self.fillDynamicItemsForInfrastructure(contextMenu)).done(function () {
                            $.when(self.fillDynamicItems(contextMenu)).done(function () {
                                contextMenu.removeItem(waitItem);
                                if (contextMenu.visibleItems().length == 0)
                                    contextMenu.close();
                            });
                        });
                    }
                };
            }
            //
            {//helper methods                               
                self.getItemName = function (item) {
                    var classID = tableViewModel.getObjectClassID(item);
                    if (classID == module.Operations.OBJ_NETWORKDEVICE || classID == module.Operations.OBJ_TERMINALDEVICE ||
                        classID == module.Operations.OBJ_ADAPTER || classID == module.Operations.OBJ_PERIPHERAL || classID == module.Operations.OBJ_SOFTWARELICENCE ||
                        classID == module.Operations.OBJ_Cluster || classID == module.Operations.OBJ_LogicalObject || classID == module.Operations.OBJ_LogicalServer ||
                        classID == module.Operations.OBJ_LogicalComputer || classID == module.Operations.OBJ_LogicalCommutator || classID == module.Operations.OBJ_SLO || classID == module.Operations.OBJ_DataEntity)
                        return item.Name ? item.Name : '';
                    else if (classID == module.Operations.OBJ_SERVICECONTRACT)
                        return getTextResource('Contract') + ' № ' + item.Number;
                    else if (classID == module.Operations.OBJ_ServiceContractAgreement)
                        return getTextResource('Contract_Agreement') + ' № ' + item.Number;
                    else if (classID == module.Operations.OBJ_WORKORDER)
                        return getTextResource('Inventory') + ' № ' + item.Number + ' \'' + item.Name + '\'';
                    else if (classID == module.Operations.OBJ_ServiceContractAgreement)
                        return getTextResource('Inventory') + ' № ' + item.Number + ' \'' + item.Name + '\'';
                    else if (classID == module.Operations.OBJ_ConfigurationUnit || classID == module.Operations.OBJ_HostConfigurationUnit)
                        return item.Name;
                    else if (classID == module.Operations.OBJ_Cluster)
                        return item.Name;
                    else
                        throw 'object classID not supported'
                };
                //
                self.getSelectedItems = function () {                    
                    var selectedItems = tableViewModel.listView.rowViewModel.checkedItems();                    
                    return selectedItems;
                };
                self.clearSelection = function () {
                    tableViewModel.listView.rowViewModel.checkedItems([]);
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
                            ClassID: tableViewModel.getObjectClassID(item),
                            ID: tableViewModel.getMainObjectID(item)
                        });
                    });
                    return retval;
                };
                
                self.GetPoolObject = function () {
                    var selectedObjects = self.getSelectedItems();
                    var requests = [];
                    
                    selectedObjects.forEach(function (item) {
                        requests.push({
                            ID: item.ID,
                            SoftwareModelID: item.SoftwareModelID,
                            ManufacturerID: item.ManufacturerID,
                            SoftwareTypeID: item.SoftwareTypeID,
                            SoftwareLicenceScheme: item.SoftwareLicenceSchemeID,
                            Type: item.SoftwareLicenseType,
                            SoftwareDistributionCentreID: item.SoftwareDistributionCentreID,
                            Balance: item.Balance,
                            IsEquip: item.IsEquip,
                            SoftwareModelName: item.SoftwareModelName,
                            SoftwareModelVersion: item.SoftwareModelVersion
                        });
                    });
                                      
                    return requests;
                };
                
                self.getSelectedLifeCycleObjects = function () {
                    var list = [];
                    var selectedItems = self.getSelectedItems();
                    selectedItems.forEach(function (el) {
                        var classID = tableViewModel.getObjectClassID(el);
                        var id = tableViewModel.getMainObjectID(el);
                        //
                        if (classID === module.Operations.OBJ_ServiceContractAgreement) {
                            id = el.AgreementID;
                        }
                        //
                        list.push(new module.LifeCycleObject(tableViewModel.getObjectClassID(el), id, el.ObjectName, el.LifeCycleStateID, el.UserID, el.OwnerClassID, el.OwnerID, el.UtilizerID, el.UtilizerClassID, el.IsLogical, el.ProductCatalogTemplate, el.SoftwareLicenceID, el.SoftwareDistributionCentreID));
                    });
                    return list;
                };
                self.getSelectedAgreementsFromContracts = function () {//используется для выполнения операций допсоглашений над контрактами
                    var list = [];
                    var selectedItems = self.getSelectedItems();
                    selectedItems.forEach(function (el) {
                        var classID = tableViewModel.getObjectClassID(el);
                        var id = tableViewModel.getMainObjectID(el);
                        //
                        if (classID === module.Operations.OBJ_SERVICECONTRACT) {
                            classID = module.Operations.OBJ_ServiceContractAgreement;
                            id = el.AgreementID;
                        }
                        //
                        list.push(new module.LifeCycleObject(classID, id, el.ObjectName, el.LifeCycleStateID, el.UserID, el.OwnerClassID, el.OwnerID, el.UtilizerID, el.UtilizerClassID, el.IsLogical));
                    });
                    return list;
                };
            }
            //
            {//menu operations
                self.add = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();

                        if (viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense')
                            return false;

                        if (viewName == 'Contracts')
                            return self.operationIsGranted(module.Operations.OPERATION_ADD_SERVICECONTRACT);
                        //                        
                        return false;
                    };
                    //
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowServiceContractRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Add');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                // добавить инсталляцию 
                self.createSoftwareInstallation = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();

                        if (viewName != 'SoftwareInstallation')
                            return false;
                        
                        return self.operationIsGranted(module.Operations.OPERATION_ADD_INSTALLATION);
                       
                    };
                    var action = function () {
                        showSpinner();
                        //вызов формы добавления Инсталляции
                        require(['ui_forms/Asset/SoftwareInstallation/frmSoftwareInstallation'], function (jsModule) {
                            jsModule.ShowDialog(null, true, true);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('software_installation_list_menu_create');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                // удалить инсталляцию 
                self.deleteSoftwareInstallation = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();

                        if (viewName != 'SoftwareInstallation')
                            return false;

                        return self.operationIsGranted(module.Operations.OPERATION_DELETE_INSTALLATION);

                    };
                    var action = function () {
                        require(['sweetAlert'], function (swal) {
                            swal('Функция не реализована');
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('software_installation_list_menu_delete');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                // свойства  инсталляции
                self.propertySoftwareInstallation = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();

                        if (viewName != 'SoftwareInstallation')
                            return false;

                        return self.operationIsGranted(module.Operations.OPERATION_PROPERTIES_INSTALLATION);

                    };
                    var action = function () {
                        let selectedItems = self.getSelectedItems();

                        if (selectedItems.length > 0) {
                            showSpinner();

                            //вызов формы связи
                            require(['ui_forms/Asset/SoftwareInstallation/frmSoftwareInstallation'], function (jsModule) {
                                jsModule.ShowDialog(selectedItems[0].ID, false, true);
                            });
                        }
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('software_installation_list_menu_property');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                self.addCluster = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'Clusters')
                            return self.operationIsGranted(module.Operations.OPERATION_Cluster_Add);
                        return false;
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowClusterRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('AddCluster');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //Узел сети Добавить
                self.createConfigurationUnit = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'ConfigurationUnits') {
                            return true;
                            //return self.operationIsGranted(module.Operations.OPERATION_DataEntity_Add);
                        }
                        return false;
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowConfigurationUnitRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Add');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };



                //ИО Добавить
                self.сreateDataEntity = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'DataEntities')
                            return self.operationIsGranted(module.Operations.OPERATION_DataEntity_Add);
                        return false;
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowDataEntityRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Add');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //ЛО Добавить
                self.createLogicObject = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'LogicObjects')
                            return self.operationIsGranted(module.Operations.OPERATION_Cluster_Add);
                        return false;
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowLogicalObjectRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('AddCluster');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //Узел сети Добавить
                self.createConfigurationUnit = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'ConfigurationUnits') {
                            return true;
                            //return self.operationIsGranted(module.Operations.OPERATION_DataEntity_Add);
                        }
                        return false;
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowConfigurationUnitRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Add');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };



                //ИО Добавить
                self.сreateDataEntity = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'DataEntities')
                            return self.operationIsGranted(module.Operations.OPERATION_DataEntity_Add);
                        return false;
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowDataEntityRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Add');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //ЛО Добавить
                self.createLogicObject = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'LogicObjects')
                            return self.operationIsGranted(module.Operations.OPERATION_Cluster_Add);
                        return false;
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowLogicalObjectRegistrationForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Add');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                //Структура пула
                self.StructurePool = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'SoftwareLicenseDistribution';
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowStructurePoolForm(self.GetPoolObject()[0]);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('StructurePool');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //Выдано прав
                self.References = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'SoftwareLicenseDistribution';
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowPoolReferencesForm(self.GetPoolObject()[0]);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicence_ReferencesTab_ListCaption');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                //IssueRights
                self.IssueRights = function (contextMenu) {
                    var isEnable = function () {
                        return self.getSelectedItems().length == 1;
                    };
                    isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'SoftwareLicenseDistribution';
                    };
                    var action = function () {
                        var pools = self.GetPoolObject();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowLicenceConsumption(pools, getTextResource('IssueRights'), null, pools[0].IsEquip == 1, 1);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('IssueRights');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //
                self.ReturnRight = function (contextMenu) {
                    var isEnable = function () {
                        return self.getSelectedItems().length == 1;
                    };
                    isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'SoftwareLicenseDistribution';
                    };
                    var action = function () {
                        var pools = self.GetPoolObject();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowLicenceReturning(pools, getTextResource('ReturnRight'), null, 1);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('ReturnRight');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //TransferRights
                self.TransferRights = function (contextMenu) {
                    var isEnable = function () {
                        if (self.getSelectedItems().length != 1)
                            return false;
                        //
                        var selected = self.getSelectedItems()[0];

                        return selected.EnableTransferToDifferentSFDC;
                    };
                    isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        return viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense';
                    };
                    var action = function () {
                        if (self.getSelectedItems().length != 1)
                            return false;
                        //
                        var selected = self.getSelectedItems()[0];
                        var viewName = tableViewModel.listView.options.settingsName();

                        if (viewName === 'SoftwareLicenseDistribution') {
                            require(['assetForms'], function (assetForms) {
                                new assetForms
                                    .formHelper(false)
                                    .ShowSoftwareSublicenseTransferFromPoolForm(selected, function () { tableViewModel.listView.load(); });
                            });

                        } else {
                            var id = tableViewModel.getMainObjectID(selected);

                            require(['assetForms'], function (assetForms) {
                                new assetForms
                                    .formHelper(false)
                                    .ShowSoftwareSublicenseTransferForm(id, function () { tableViewModel.listView.load(); });
                            });
                        }
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Sublicense_SDCTransfer');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                //Добавить стандартную лицензию
                self.addStandartSoftwareLicence = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'SoftwareLicense')
                            return self.operationIsGranted(module.Operations.OPERATION_SoftwareLicence_Add);
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowSoftwareLicenceAddForm('standart');
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicenceContext_AddStandartLicence');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                //Добавить лицензию по контракту
                self.addContractSoftwareLicence = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'SoftwareLicense')
                            return self.operationIsGranted(module.Operations.OPERATION_SoftwareLicence_Add);
                    };
                    var action = function () {
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowSoftwareLicenceAddByContractForm();
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicenceContext_AddContractLicence');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };

                self.addAgreement = function (contextMenu) {
                    var isEnable = function () {
                        return self.getSelectedItems().length === 1;
                    };
                    var isVisible = function () {
                        var items = self.getSelectedItems();
                        if (items.length == 1) {
                            var classID = tableViewModel.getObjectClassID(items[0]);
                            if (classID == 115 && self.operationIsGranted(873) && items[0].CanCreateAgreement == true && items[0].AgreementID == null)//OPERATION_ServiceContractAgreement_Add = 873
                                return true;
                        }
                        return false;
                    };
                    var action = function () {
                        if (self.getSelectedItems().length != 1)
                            return false;
                        //
                        var selected = self.getSelectedItems()[0];
                        var id = tableViewModel.getMainObjectID(selected);
                        var classID = tableViewModel.getObjectClassID(selected);
                        if (classID == 115 && selected.CanCreateAgreement == true) {
                            showSpinner();
                            require(['assetForms'], function (module) {
                                var fh = new module.formHelper(true);
                                fh.ShowServiceContractAgreement(null, id);
                            });
                        }
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('ContractAgreement_CreateFromContract');
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
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'SoftwareLicenseDistribution')
                            return false;

                        if (viewName == 'SoftwareInstallation')
                            return false;
                        
                        var retval = true;
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            if ((classID == module.Operations.OBJ_NETWORKDEVICE && !self.operationIsGranted(module.Operations.OPERATION_PROPERTIES_NETWORKDEVICE)) ||
                                (classID == module.Operations.OBJ_TERMINALDEVICE && !self.operationIsGranted(module.Operations.OPERATION_PROPERTIES_TERMINALDEVICE)) ||
                                (classID == module.Operations.OBJ_ADAPTER && !self.operationIsGranted(module.Operations.OPERATION_PROPERTIES_ADAPTER)) ||
                                (classID == module.Operations.OBJ_PERIPHERAL && !self.operationIsGranted(module.Operations.OPERATION_PROPERTIES_PERIPHERAL)) ||
                                (classID == module.Operations.OBJ_SERVICECONTRACT && !self.operationIsGranted(module.Operations.OPERATION_PROPERTIES_SERVICECONTRACT)) ||
                                (classID == module.Operations.OBJ_ServiceContractAgreement && !self.operationIsGranted(module.Operations.OPERATION_Properties_ServiceContractAgreement)) ||
                                (classID == module.Operations.OBJ_SOFTWARELICENCE && !self.operationIsGranted(module.Operations.OPERATION_SoftwareLicence_Properties)) ||
                                (classID == module.Operations.OBJ_WORKORDER && !self.operationIsGranted(module.Operations.OPERATION_WorkOrder_Properties)) ||
                                (classID == module.Operations.OBJ_Cluster && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Properties)) ||
                                (classID == module.Operations.OBJ_LogicalObject && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Properties)) ||
                                (classID == module.Operations.OBJ_LogicalServer && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Properties)) ||
                                (classID == module.Operations.OBJ_LogicalComputer && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Properties)) ||
                                (classID == module.Operations.OBJ_LogicalCommutator && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Properties)) ||
                                (classID == module.Operations.OBJ_SLO && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Properties)))
                                retval = false;
                        })
                        return retval;
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
                self.Active = function (contextMenu) {
                    var isEnable = function () {
                        return self.getSelectedItems().length === 1;
                    }
                    var isVisible = function () {
                        var retval = false;
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense')
                            return false;
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            var productTypeID = el.ProductCatalogTemplate;

                            if (classID == module.Operations.OBJ_SOFTWARELICENCE
                                && self.operationIsGranted(442)
                                && productTypeID == module.Operations.SUB_CLASS_SubscriptionRenewal_ID) {
                                retval = true;
                            }

                        });
                        return retval;
                    }

                    var action = function () {
                        if (self.getSelectedItems().length != 1)
                            return false;
                        //
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowLicenceApplying(self.getSelectedItems(), 'Применить продление', null);
                        });
                    };


                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicenceUpdateType_SubscriptionRenewal');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action)

                }
                //
                self.sendToEmailContextMenuItem = function (contextMenu) {
                    var isEnable = function () {
                        return self.getSelectedItems().length === 1;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName != 'Inventories')
                            return false;
                        if (!self.userHasRoles)
                            return false;
                        return true;
                    };
                    var action = function () {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var tmpObj = self.getSelectedItems()[0];
                            var options = {
                                Obj: {
                                    ID: tmpObj.ID,
                                    ClassID: tmpObj.ClassID
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


                self.Upgrade = function (contextMenu) {
                    var isEnable = function () {
                        return self.getSelectedItems().length === 1;
                    }
                    var isVisible = function () {
                        var retval = false;
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense')
                            return false;
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            var productTypeID = el.ProductCatalogTemplate;

                            if (classID == module.Operations.OBJ_SOFTWARELICENCE
                                && self.operationIsGranted(442)
                                && productTypeID == module.Operations.SUB_CLASS_Single_ID) {
                                retval = true;
                            }

                        });
                        return retval;
                    }

                    var action = function () {
                        if (self.getSelectedItems().length != 1)
                            return false;
                        //
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowLicenceUpgrade(self.getSelectedItems(), 'Upgrade', null)
                        });
                    };


                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('SoftwareLicenceUpdateType_Upgrade');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action)

                }
                //
                self.remove = function (contextMenu) {
                    var isEnable = function () {
                        var retval = true;
                        if (!self.getSelectedItems().length > 0) {
                            retval = false;
                        };
                        self.getSelectedItems().forEach(function (el) {
                            switch (el.ClassID) {
                                case module.Operations.OBJ_SERVICECONTRACT: {
                                    if (el.CanCreateAgreement)
                                        retval = false;
                                    break;
                                }
                            }
                        });
                        if (self.isAdmin)
                            retval = true;
                        return retval;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense')
                            return false;

                        if (viewName == 'SoftwareInstallation')
                            return false;
                        
                        var retval = true;    
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            if ((classID == module.Operations.OBJ_NETWORKDEVICE && !self.operationIsGranted(module.Operations.OPERATION_DELETE_NETWORKDEVICE)) ||
                                (classID == module.Operations.OBJ_TERMINALDEVICE && !self.operationIsGranted(module.Operations.OPERATION_DELETE_TERMINALDEVICE)) ||
                                (classID == module.Operations.OBJ_ADAPTER && !self.operationIsGranted(module.Operations.OPERATION_DELETE_ADAPTER)) ||
                                (classID == module.Operations.OBJ_PERIPHERAL && !self.operationIsGranted(module.Operations.OPERATION_DELETE_PERIPHERAL)) ||
                                (classID == module.Operations.OBJ_SERVICECONTRACT && !self.operationIsGranted(module.Operations.OPERATION_DELETE_SERVICECONTRACT)) ||
                                (classID == module.Operations.OBJ_ServiceContractAgreement && !self.operationIsGranted(module.Operations.OPERATION_ServiceContractAgreement_Delete)) ||
                                (classID == module.Operations.OBJ_SOFTWARELICENCE && !self.operationIsGranted(module.Operations.OPERATION_SoftwareLicence_Delete)) ||
                                (classID == module.Operations.OBJ_ServiceContractAgreement && !self.operationIsGranted(module.Operations.OPERATION_ServiceContractAgreement_Delete)) ||
                                (classID == module.Operations.OBJ_WORKORDER && !self.operationIsGranted(module.Operations.OPERATION_WorkOrder_Delete)) ||
                                (classID == module.Operations.OBJ_Cluster && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Delete)) ||
                                (classID == module.Operations.OBJ_LogicalObject && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Delete)) ||
                                (classID == module.Operations.OBJ_LogicalServer && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Delete)) ||
                                (classID == module.Operations.OBJ_LogicalComputer && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Delete)) ||
                                (classID == module.Operations.OBJ_LogicalCommutator && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Delete)) ||
                                (classID == module.Operations.OBJ_SLO && !self.operationIsGranted(module.Operations.OPERATION_Cluster_Delete)) ||
                                (classID == module.Operations.OBJ_DataEntity && !self.operationIsGranted(module.Operations.OPERATION_DataEntity_Delete)))
                                retval = false;
                        });
                        if (self.isAdmin)
                            retval = true;
                        return retval;
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
                                            function (response) {
                                                setTimeout(function () {
                                                    if (response && response.Result === 0) {
                                                        data.ObjectList.forEach(function (info) {
                                                            $(document).trigger('local_objectDeleted', [info.ClassID, info.ID]);
                                                            self.clearSelection();
                                                        });
                                                    } else if (response && response.Result === 8) {
                                                        swal(getTextResource('DeleteError'), getTextResource('SoftwareLicenceDelete_ErrorMessage'), 'error');
                                                        self.clearSelection();
                                                    } else if (response && response.Result === 4 && response.Message != '') {
                                                        swal(getTextResource('DeleteError'), response.Message, 'error');
                                                        self.clearSelection();
                                                    } else {
                                                        swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[Table.ContextMenu.js, remove]', 'error');
                                                        self.clearSelection();
                                                    }
                                                }, 300);
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
                self.wait = function (contextMenu) {
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('Loading');
                    cmd.enabled(false);
                    //
                    cmd.isDynamic = true;
                    return cmd;
                };
                //
                self.createCall = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'Inventories' || viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense' || viewName == 'DataEntities')
                            return false;

                        if (viewName == 'SoftwareInstallation')
                            return false;
                        //
                        return self.operationIsGranted(module.Operations.OPERATION_Call_Add);
                    };
                    var action = function () {
                        showSpinner();
                        require(['registrationForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var dependencyObjects = self.getItemInfos(self.getSelectedItems());
                            fh.ShowCallRegistrationEngineer(null, null, null, null, dependencyObjects);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('ButtonCreateCall');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //
                self.createWorkOrder = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense' || viewName == 'DataEntities')
                            return false;

                        if (viewName == 'SoftwareInstallation')
                            return false;

                        return self.operationIsGranted(module.Operations.OPERATION_WorkOrder_Add);
                    };
                    var action = function () {
                        showSpinner();
                        require(['registrationForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var dependencyObjects = self.getItemInfos(self.getSelectedItems());
                            fh.ShowWorkOrderRegistration(null, null, null, dependencyObjects);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('ButtonCreateWorkOrder');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                //
                self.createProblem = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        var viewName = tableViewModel.listView.options.settingsName();
                        if (viewName == 'Inventories' || viewName == 'SoftwareLicenseDistribution' || viewName == 'SubSoftwareLicense' || viewName == 'DataEntities')
                            return false;

                        if (viewName == 'SoftwareInstallation')
                            return false;
                        //
                        return self.operationIsGranted(module.Operations.OPERATION_Problem_Add);
                    };
                    var action = function () {
                        showSpinner();
                        require(['registrationForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var dependencyObjects = self.getItemInfos(self.getSelectedItems());
                            fh.ShowProblemRegistration(null, null, null, null, dependencyObjects);
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
                self.fillDynamicItems = function (contextMenu) {
                    var retval = $.Deferred();
                    var lifeCycleObjectList = self.getSelectedLifeCycleObjects();
                    var selectedAgreementsFromContracts = self.getSelectedAgreementsFromContracts();
                    var selectedItem = self.getSelectedItems()[0];
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
                                require(['assetOperationsHelper'], function (module) {
                                    var viewName = tableViewModel.listView.options.settingsName();
                                    newVal.List.forEach(function (lifeCycleOperation) {
                                        if (lifeCycleOperation.CommandType === null)//сепаратор
                                        {
                                            if (lifeCycleOperation.Name) {
                                                contextMenu.addSeparator();
                                                //
                                                var cmd = contextMenu.addContextMenuItem();
                                                cmd.enabled(false);
                                                cmd.text(lifeCycleOperation.Name);
                                            }
                                            else {
                                                var cmd = contextMenu.addSeparator();
                                            }
                                            //
                                            cmd.isDynamic = true;
                                        }
                                        else {
                                            if (self.ClassID == 115 && lifeCycleOperation.CommandType == 18) {
                                                return;
                                            }


                                            if (functionsAvailability.SoftwareDistributionCentres && selectedItem.ClassID == 223) {
                                                if (viewName == 'SoftwareLicense' && (lifeCycleOperation.CommandType == 12 || lifeCycleOperation.CommandType == 13)) {
                                                    return;
                                                }

                                                if (lifeCycleOperation.CommandType != 12 && lifeCycleOperation.CommandType != 13)
                                                    return;
                                            }

                                            var cmd = contextMenu.addContextMenuItem();
                                            cmd.enabled(lifeCycleOperation.Enabled);
                                            cmd.text(lifeCycleOperation.Name);
                                            var selectedObjects = !lifeCycleOperation.ObjectClassID ? lifeCycleObjectList : selectedAgreementsFromContracts;
                                            cmd.click(function () {
                                                module.executeLifeCycleOperation(lifeCycleOperation, selectedObjects);
                                            });
                                            //
                                            cmd.isDynamic = true;
                                        }
                                    });
                                });
                            }
                            retval.resolve();
                        });
                    //
                    return retval.promise();
                };
                //
                //self.includeToInfrastructure = function (contextMenu) {
                //    var isEnable = function () {
                //        var retval = true;
                //        var list = self.getSelectedItems();
                //        if (list.length == 0)
                //            return false;
                //        //
                //        self.getSelectedItems().forEach(function (el) {
                //            var classID = tableViewModel.getObjectClassID(el);
                //            if (classID == module.Operations.OBJ_NETWORKDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_NETWORKDEVICE) ||
                //                classID == module.Operations.OBJ_TERMINALDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_TERMINALDEVICE))
                //                retval = false;
                //        });
                //        if (!retval)
                //            return false;
                //        //
                //        var data = {
                //            'ObjectList': self.getItemInfos(list)
                //        };
                //        self.ajaxControl.Ajax(null,
                //            {
                //                dataType: "json",
                //                method: 'POST',
                //                data: data,
                //                url: '/assetApi/CanIncludeToInfrastructure'
                //            },
                //            function (response) {
                //                if (response && response.Result === 0) {
                //                    return retval;
                //                }
                //            });
                //        //
                //    };
                //    //
                //    var cmd = contextMenu.addContextMenuItem();
                //    cmd.restext('IncludeToIfrastructure');
                //    cmd.isEnable = isEnable;
                //    cmd.isVisible = isVisible;
                //    cmd.click(action);
                //};
                //
                self.fillDynamicItemsForInfrastructure = function (contextMenu) {
                    var retval = $.Deferred();
                    //
                    var viewName = tableViewModel.listView.options.settingsName();
                    if (viewName != 'Hardware' && viewName != 'LogicObjects') {
                        retval.resolve();
                        return retval.promise();
                    }
                    //
                    var action = function () {
                        var list = self.getSelectedItems();
                        if (list.length !== 1)
                            return;
                        //
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var device = self.getSelectedItems()[0];
                            fh.ShowConfigurationUnitRegistrationForm(device);
                        });
                        //
                        //var question = self.getConcatedItemNames(list);
                        //require(['sweetAlert'], function (swal) {
                        //    swal({
                        //        title: getTextResource('IncludeToIfrastructureQuestion'),
                        //        text: question,
                        //        showCancelButton: true,
                        //        closeOnConfirm: false,
                        //        closeOnCancel: true,
                        //        confirmButtonText: getTextResource('ButtonOK'),
                        //        cancelButtonText: getTextResource('ButtonCancel')
                        //    },
                        //        function (value) {
                        //            swal.close();
                        //            //
                        //            if (value == true) {
                        //                var data = {
                        //                    'ObjectList': self.getItemInfos(list)
                        //                };
                        //                self.ajaxControl.Ajax(null,
                        //                    {
                        //                        dataType: "json",
                        //                        method: 'POST',
                        //                        data: data,
                        //                        url: '/assetApi/CreateConfigurationUnitsByObjects'
                        //                    },
                        //                    function (response) {
                        //                        setTimeout(function () {
                        //                            if (response && response.Result === 0) {
                        //                                self.clearSelection();
                        //                                if (response.Error)
                        //                                    swal(getTextResource('ErrorCaption'), response.Error, 'info');
                        //                                else
                        //                                    swal(getTextResource('IncludeToIfrastructureSuccess'));
                        //                            }
                        //                            else {
                        //                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                        //                                self.clearSelection();
                        //                            }
                        //                        }, 300);
                        //                    });
                        //            }
                        //        });
                        //});
                    };
                    //
                    var isVisible = function () {
                        var retval = true;
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            if ((classID != module.Operations.OBJ_NETWORKDEVICE) &&
                                (classID != module.Operations.OBJ_TERMINALDEVICE) &&
                                (classID != module.Operations.OBJ_LogicalObject) &&
                                (classID != module.Operations.OBJ_LogicalServer) &&
                                (classID != module.Operations.OBJ_LogicalComputer) &&
                                (classID != module.Operations.OBJ_LogicalCommutator) /*&&
                                (classID != module.Operations.OBJ_SLO)*/)
                                retval = false;
                        });

                        return retval;
                    };
                    //
                    var isEnable = function () {
                        var retval = true;
                        var list = self.getSelectedItems();
                        if (list.length !== 1)
                            return false;
                        //
                        if (!self.operationIsGranted(module.Operations.OPERATION_ConfigurationUnit_Add))
                            return false;
                        //
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            if (classID == module.Operations.OBJ_NETWORKDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_NETWORKDEVICE) ||
                                classID == module.Operations.OBJ_TERMINALDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_TERMINALDEVICE))
                                retval = false;
                        });
                        //
                        return retval;
                    };
                    //
                    var list = self.getSelectedItems();
                    //
                    var data = {
                        'ObjectList': self.getItemInfos(list)
                    };
                    //
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: '/assetApi/CanIncludeToInfrastructure'
                        },
                        function (newVal) {
                            if (newVal) {
                                var cmd = contextMenu.addContextMenuItem();
                                cmd.restext('IncludeToIfrastructure');
                                cmd.enabled(isEnable());
                                cmd.click(action);
                                //
                                cmd.isDynamic = true;
                                //
                            }
                            retval.resolve();
                        });
                    //
                    return retval.promise();
                };
                //
                //self.includeToInfrastructure = function (contextMenu) {
                //    var isEnable = function () {
                //        var retval = true;
                //        var list = self.getSelectedItems();
                //        if (list.length == 0)
                //            return false;
                //        //
                //        self.getSelectedItems().forEach(function (el) {
                //            var classID = tableViewModel.getObjectClassID(el);
                //            if (classID == module.Operations.OBJ_NETWORKDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_NETWORKDEVICE) ||
                //                classID == module.Operations.OBJ_TERMINALDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_TERMINALDEVICE))
                //                retval = false;
                //        });
                //        if (!retval)
                //            return false;
                //        //
                //        var data = {
                //            'ObjectList': self.getItemInfos(list)
                //        };
                //        self.ajaxControl.Ajax(null,
                //            {
                //                dataType: "json",
                //                method: 'POST',
                //                data: data,
                //                url: '/assetApi/CanIncludeToInfrastructure'
                //            },
                //            function (response) {
                //                if (response && response.Result === 0) {
                //                    return retval;
                //                }
                //            });
                //        //
                //    };
                //    //
                //    var cmd = contextMenu.addContextMenuItem();
                //    cmd.restext('IncludeToIfrastructure');
                //    cmd.isEnable = isEnable;
                //    cmd.isVisible = isVisible;
                //    cmd.click(action);
                //};
                //
                self.fillDynamicItemsForInfrastructure = function (contextMenu) {
                    var retval = $.Deferred();
                    //
                    var viewName = tableViewModel.listView.options.settingsName();
                    if (viewName != 'Hardware' && viewName != 'LogicObjects') {
                        retval.resolve();
                        return retval.promise();
                    }
                    //
                    var action = function () {
                        var list = self.getSelectedItems();
                        if (list.length !== 1)
                            return;
                        //
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var device = self.getSelectedItems()[0];
                            fh.ShowConfigurationUnitRegistrationForm(device);
                        });
                        //
                        //var question = self.getConcatedItemNames(list);
                        //require(['sweetAlert'], function (swal) {
                        //    swal({
                        //        title: getTextResource('IncludeToIfrastructureQuestion'),
                        //        text: question,
                        //        showCancelButton: true,
                        //        closeOnConfirm: false,
                        //        closeOnCancel: true,
                        //        confirmButtonText: getTextResource('ButtonOK'),
                        //        cancelButtonText: getTextResource('ButtonCancel')
                        //    },
                        //        function (value) {
                        //            swal.close();
                        //            //
                        //            if (value == true) {
                        //                var data = {
                        //                    'ObjectList': self.getItemInfos(list)
                        //                };
                        //                self.ajaxControl.Ajax(null,
                        //                    {
                        //                        dataType: "json",
                        //                        method: 'POST',
                        //                        data: data,
                        //                        url: '/assetApi/CreateConfigurationUnitsByObjects'
                        //                    },
                        //                    function (response) {
                        //                        setTimeout(function () {
                        //                            if (response && response.Result === 0) {
                        //                                self.clearSelection();
                        //                                if (response.Error)
                        //                                    swal(getTextResource('ErrorCaption'), response.Error, 'info');
                        //                                else
                        //                                    swal(getTextResource('IncludeToIfrastructureSuccess'));
                        //                            }
                        //                            else {
                        //                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                        //                                self.clearSelection();
                        //                            }
                        //                        }, 300);
                        //                    });
                        //            }
                        //        });
                        //});
                    };
                    //
                    var isVisible = function () {
                        var retval = true;
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            if ((classID != module.Operations.OBJ_NETWORKDEVICE) &&
                                (classID != module.Operations.OBJ_TERMINALDEVICE) &&
                                (classID != module.Operations.OBJ_LogicalObject) &&
                                (classID != module.Operations.OBJ_LogicalServer) &&
                                (classID != module.Operations.OBJ_LogicalComputer) &&
                                (classID != module.Operations.OBJ_LogicalCommutator) /*&&
                                (classID != module.Operations.OBJ_SLO)*/)
                                retval = false;
                        });

                        return retval;
                    };
                    //
                    var isEnable = function () {
                        var retval = true;
                        var list = self.getSelectedItems();
                        if (list.length !== 1)
                            return false;
                        //
                        if (!self.operationIsGranted(module.Operations.OPERATION_ConfigurationUnit_Add))
                            return false;
                        //
                        self.getSelectedItems().forEach(function (el) {
                            var classID = tableViewModel.getObjectClassID(el);
                            if (classID == module.Operations.OBJ_NETWORKDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_NETWORKDEVICE) ||
                                classID == module.Operations.OBJ_TERMINALDEVICE && !self.operationIsGranted(module.Operations.OPERATION_UPDATE_TERMINALDEVICE))
                                retval = false;
                        });
                        //
                        return retval;
                    };
                    //
                    var list = self.getSelectedItems();
                    //
                    var data = {
                        'ObjectList': self.getItemInfos(list)
                    };
                    //
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: '/assetApi/CanIncludeToInfrastructure'
                        },
                        function (newVal) {
                            if (newVal) {
                                var cmd = contextMenu.addContextMenuItem();
                                cmd.restext('IncludeToIfrastructure');
                                cmd.enabled(isEnable());
                                cmd.click(action);
                                //
                                cmd.isDynamic = true;
                                //
                            }
                            retval.resolve();
                        });
                    //
                    return retval.promise();
                };
            }
        },

        LifeCycleObject: function (classID, ID, name, lifeCycleStateID, userID, ownerClassID, ownerID, utilizerID, utilizerClassID, isLogical, ProductCatalogTemplate, SoftwareLicenceID, SoftwareDistributionCentreID) {
            var self = this;
            //
            self.ClassID = classID,
            self.ID = ID,
            self.Name = name,
            self.LifeCycleStateID = lifeCycleStateID,
            self.UserID = userID,
            self.OwnerClassID = ownerClassID,
            self.OwnerID = ownerID,
            self.UtilizerID = utilizerID,
            self.UtilizerClassID = utilizerClassID,
            self.IsLogical = isLogical,
            self.ProductCatalogTemplate = ProductCatalogTemplate,
            self.SoftwareLicenceID = SoftwareLicenceID,
            self.SoftwareDistributionCentreID = SoftwareDistributionCentreID    
        },

        Operations: {
            OBJ_TERMINALDEVICE: 6,
            OBJ_NETWORKDEVICE: 5,
            OBJ_ADAPTER: 33,
            OBJ_PERIPHERAL: 34,
            OBJ_SERVICECONTRACT: 115,
            OBJ_ServiceContractAgreement: 386,
            OBJ_WORKORDER: 119,
            OBJ_SOFTWARELICENCE: 223,
            OBJ_ConfigurationUnit: 409,
            OBJ_Cluster: 420,
            OBJ_LogicalObject: 415,
            OBJ_LogicalServer: 416,
            OBJ_LogicalComputer: 417,
            OBJ_LogicalCommutator: 418,
            OBJ_SLO: 12,
            OBJ_DataEntity: 165,
            OBJ_HostConfigurationUnit : 419,
            //

            SUB_CLASS_SubscriptionRenewal_ID: 186,
            SUB_CLASS_Single_ID: 183,

            OPERATION_DELETE_NETWORKDEVICE: 24,
            OPERATION_DELETE_TERMINALDEVICE: 66,
            OPERATION_DELETE_ADAPTER: 78,
            OPERATION_DELETE_PERIPHERAL: 80,
            OPERATION_DELETE_SERVICECONTRACT: 213,
            OPERATION_ServiceContractAgreement_Delete: 874,
            OPERATION_WorkOrder_Delete: 330,
            OPERATION_SoftwareLicence_Delete: 443,
            OPERATION_Cluster_Delete: 958,
            OPERATION_DataEntity_Delete: 616,

            OPERATION_Call_Add: 309,
            OPERATION_WorkOrder_Add: 301,
            OPERATION_Problem_Add: 319,
            OPERATION_ADD_SERVICECONTRACT: 212,
            OPERATION_SoftwareLicence_Add: 440,
            OPERATION_Cluster_Add: 956,
            OPERATION_DataEntity_Add: 615,
            OPERATION_ConfigurationUnit_Add: 952,

            OPERATION_PROPERTIES_NETWORKDEVICE: 23,
            OPERATION_PROPERTIES_TERMINALDEVICE: 65,
            OPERATION_PROPERTIES_ADAPTER: 77,
            OPERATION_PROPERTIES_PERIPHERAL: 79,
            OPERATION_PROPERTIES_SERVICECONTRACT: 211,
            OPERATION_Properties_ServiceContractAgreement: 872,
            OPERATION_WorkOrder_Properties: 302,
            OPERATION_SoftwareLicence_Properties: 441,
            OPERATION_Cluster_Properties: 959,

            OPERATION_ADD_INSTALLATION: 86,
            OPERATION_PROPERTIES_INSTALLATION: 87,
            OPERATION_DELETE_INSTALLATION: 88,

            OPERATION_UPDATE_NETWORKDEVICE: 233,
            OPERATION_UPDATE_TERMINALDEVICE: 240,
                  
        }

    }
    return module;
});