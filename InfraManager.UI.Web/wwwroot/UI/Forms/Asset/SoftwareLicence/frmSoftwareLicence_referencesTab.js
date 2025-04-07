define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu'],
    function (ko, $, ajax, dtLib, m_cells, m_helpers, m_lazyEvents) {
        var module = {
            Tab: function (vm) {
                var self = this;
                self.ajaxControl = new ajax.control();
                //
                self.Name = getTextResource('SoftwareLicence_ReferencesTab');
                self.Template = '../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicence_referencesTab';
                self.IconCSS = 'referencesTab';
                //
                //Типы лицензий 
                self.ProductCatalogTemplateIDSingle = 183;       //ID самостоятельной лицензии
                self.ProductCatalogTemplateIDSubscription = 186; //подписка
                self.ProductCatalogTemplateIDUpgrade = 185;      //upgrade
                self.ProductCatalogTemplateIDApply = 187;        //продление подписки
                self.ProductCatalogTemplateIDRent = 184;         //аренда
                //видимость вкладки на форме лицензии
                self.IsVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent
                        )
                        : false;
                });

                self.ClassID = 403;
                self.canAdd = ko.observable(false);
                //
                {//fields
                    self.isLoaded = false;
                }
                //
                //when object changed
                self.Initialize = function (obj) {
                    if (self.isLoaded)
                        self.listView.load();
                    else
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
                            retvalD.resolve(objectList);
                        });
                        return retvalD.promise();
                    };
                    self.listViewRowClick = function (obj) {
                        
                    };

                    self.getSelectedLifeCycleObjects = function () {
                        var list = [];
                        var selectedItems = self.getSelectedItems();
                        let count = 0;
                        selectedItems.forEach(function (el) {
                            count++;
                            list.push(new module.LifeCycleObject(223, el.ID.toUpperCase(), el.ObjectName, el.LifeCycleStateID, el.UserID, el.OwnerClassID, el.OwnerID, el.UtilizerID, el.UtilizerClassID, el.IsLogical));
                        });
                        if (count == 0) {
                            list.push(new module.LifeCycleObject(223, vm.object().ID().toUpperCase(), vm.object().ObjectName, vm.object().LifeCycleStateID, vm.object().UserID, vm.object().OwnerClassID, vm.object().OwnerID, vm.object().UtilizerID, vm.object().UtilizerClassID, vm.object().IsLogical));
                        }                        
                        return list;
                    };

                }
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
                        });

                    }
                    //
                    self.listViewContextMenu = ko.observable(null);
                    //
                    self.contextMenuInit = function (contextMenu) {
                        self.listViewContextMenu(contextMenu);//bind contextMenu
                        //
                        contextMenu.addSeparator();
                        self.add(contextMenu);
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
                        var cmd = contextMenu.unshiftContextMenuItem();
                        cmd.restext('Loading');
                        cmd.enabled(false);
                        //
                        cmd.isDynamic = true;
                        return cmd;
                    };

                    self.add = function(contextMenu) {
                        const isEnable = function () {
                            return self.getSelectedItems().length == 1 
                                && self.operationIsGranted(523);
                        };
                        const isVisible = function () {                            
                            return true;
                        };
                        const action = function () {
                            var $retval = $.Deferred();
                            const ObjectID = self.getSelectedItems()[0].ObjectID;

                            require(['ui_forms/Asset/SoftwareLicence/frmSoftwareLicenceSerialNumbersSnap'], function (fhModule) {
                                var form = new fhModule.Form(vm.object().ID(), ObjectID);
                                $.when(form.Show()).done(function (result) {
                                    self.listView.load();                                    
                                });                                
                            });                                                       
                        };
                        //
                        const cmd = contextMenu.addContextMenuItem();
                        cmd.restext('AddSerialNumberContextMemu');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);                        
                    };

                    self.fillDynamicItems = function (contextMenu) {
                        var retval = $.Deferred();
                        var lifeCycleObjectList = self.getSelectedLifeCycleObjects();
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
                                        newVal.List.forEach(function (lifeCycleOperation) {
                                            if (!functionsAvailability.SoftwareDistributionCentres &&
                                                (lifeCycleOperation.CommandType == 12 ||
                                                lifeCycleOperation.CommandType == 13)) {
                                                var cmd = contextMenu.unshiftContextMenuItem();
                                                cmd.enabled(lifeCycleOperation.Enabled);
                                                cmd.text(lifeCycleOperation.Name);
                                                cmd.click(function() {
                                                    var await = module.executeLifeCycleOperation(lifeCycleOperation,lifeCycleObjectList);
                                                    $.when(await).done(function (res) {                                                        
                                                        self.listView.load();
                                                    });
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
                }
                {
                    //helpers
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
                                ClassID: 403,                                
                                ObjectID: el.ObjectID.toUpperCase()
                            };
                            retval.push(item);
                        });
                        return retval;
                    };

                    self.clearSelection = function () {
                        self.listView.rowViewModel.checkedItems([]);
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
                            ViewName: 'SoftwareLicenceReferences',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.object().ID()
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSoftwareLicenceReferenceObject'
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
                m_lazyEvents.init(self);//extend self
            },
            LifeCycleObject: function (classID, ID, name, lifeCycleStateID, userID, ownerClassID, ownerID, utilizerID, utilizerClassID, isLogical) {
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
                    self.IsLogical = isLogical
            }
        };
        return module;
    });