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
                self.Name = getTextResource('SoftwareLicence_UpdatesTab');
                self.Template = '../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicence_updatesTab';
                self.IconCSS = 'updatesTab';
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

                self.ClassID = 402;
                self.canAdd = ko.observable(false);
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
                        //открыть карточку объекта обновления
                        self.ViewSoftwareLicenceUpdateForm();
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
                            ViewName: 'SoftwareLicenceUpdates',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: vm.object().ID(),
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSoftwareLicenceUpdateObject'
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
                                SoftwareLicenceName: el.SoftwareLicenceName,
                                CorrespondingLicenceID: el.CorrespondingLicenceID,
                                CorrespondingContractID: el.CorrespondingContractID,
                                CorrespondingObjectName: el.CorrespondingObjectName,
                                OldEndDate: el.OldEndDate,
                                OldEndDateString: el.OldEndDateString,
                                NewEndDate: el.NewEndDate,
                                NewEndDateString: el.NewEndDateString,
                                OldVersion: el.OldVersion,
                                NewVersion: el.NewVersion,
                                UpdateTypeInt: el.UpdateTypeInt,
                                UpdateType: el.UpdateType,
                                EffectiveDateTimeString: el.EffectiveDateTimeString,
                                OldReferenceCount: el.OldReferenceCount,
                                NewReferenceCount: el.NewReferenceCount,
                                OldSoftwareModelName: el.OldSoftwareModelName,
                                NewSoftwareModelName: el.NewSoftwareModelName
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
                        self.viewSoftwareLicenceUpdate(contextMenu);
                        self.viewCorrespondingObject(contextMenu);
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

                //Открыть карточку обновления - контекстное меню или клик по строке
                self.ViewSoftwareLicenceUpdateForm = function () {
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var list = self.getSelectedItems();
                        if (list.length == 0)
                            return;
                        showSpinner();
                        var $retval = $.Deferred();
                        var object = self.getSelectedItems();

                        //Upgrade
                        if (object[0].UpdateTypeInt == 0) {
                            $.when(fh.ShowLicenceUpgradeReadOnly(object)).done(function (result) {
                                $retval.resolve(result);
                            });
                            //Version update / Обновление версии
                        } else if (object[0].UpdateTypeInt == 1) {
                            $.when(fh.ShowLicenceContractUpdateReadOnly(object)).done(function (result) {
                                $retval.resolve(result);
                            });
                            //Subscription Renewal / Продление подписки
                        } else if (object[0].UpdateTypeInt == 2) {
                            $.when(fh.ShowLicenceApplyingReadOnly(object)).done(function (result) {
                                $retval.resolve(result);
                            });
                            //Rent Contract Renewal / Продление аренды
                        } else if (object[0].UpdateTypeInt == 3) {
                            $.when(fh.ShowLicenceUpdateRent(object)).done(function (result) {
                                $retval.resolve(result);
                            });
                        }
                    });
                };

                //Открыть основание - контекстное меню
                self.ViewSoftwareCorrespondingObjectForm = function () {
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var list = self.getSelectedItems();
                        if (list.length == 0)
                            return;
                        showSpinner();

                        var object = self.getSelectedItems()[0];
                        //Upgrade
                        if (object.UpdateTypeInt == 0) {
                            fh.ShowSoftwareLicenceForm(object.CorrespondingLicenceID);
                            //Version update / Обновление версии
                        } else if (object.UpdateTypeInt == 1) {
                            fh.ShowServiceContract(object.CorrespondingContractID);
                            //Subscription Renewal / Продление подписки
                        } else if (object.UpdateTypeInt == 2) {
                            fh.ShowSoftwareLicenceForm(object.CorrespondingLicenceID);
                            //Rent Contract Renewal / Продление аренды
                        } else if (object.UpdateTypeInt == 3) {
                            fh.ShowServiceContract(object.CorrespondingContractID);
                        }
                    });
                };

                {//menu operations
                    self.viewSoftwareLicenceUpdate = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1; //self.operationIsGranted(461);
                        };
                        var isVisible = function () {
                            return vm.CanEdit() && vm.object() != null;
                        };
                        var action = function () {
                            self.ViewSoftwareLicenceUpdateForm();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('SoftwareLicenceUpdateType_Open');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.viewCorrespondingObject = function (contextMenu) {
                        var isEnable = function () {
                            return self.getSelectedItems().length === 1; //self.operationIsGranted(462);
                        };
                        var isVisible = function () {
                            return vm.CanEdit();
                        };
                        var action = function () {
                            self.ViewSoftwareCorrespondingObjectForm();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('SoftwareLicenceUpdateType_OpenCorresponding');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                }
            }
        };
        return module;
    });