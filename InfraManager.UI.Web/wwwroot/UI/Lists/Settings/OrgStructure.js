define(['knockout', 'jquery', 'ajax', 'treeControl', 'ui_controls/ContextMenu/ko.ContextMenu'], function (ko, $, ajaxLib, treeLib) {
    var module = {
        ViewModel: function () {//общая модель представления
            var self = this;
            //
            self.ajaxControl = new ajaxLib.control();
            //            
            self.ModelAfterRender = function () {
                if (self.orgStructureControl == null) {
                    self.InitOrgStructureTree();
                }
                self.OnResize();
            };
            //
            self.CheckData = function () {//вызывается извне, сообщает, что пора обновлять/загружать данные
                self.InitOrgStructureTree();
            };
            self.ViewTemplateName = ko.observable(''); //шаблон контента (справа) 
            self.ViewTemplateName.subscribe(function (newValue) {
                if (newValue == '')
                {
                    if (self.Model() != null) {
                        if (typeof self.Model().dispose !== "undefined") {
                            self.Model().dispose();
                        }
                        self.Model(null);
                    }
                }
            });
            self.Model = ko.observable(null); //контент
            self.IsEditing = ko.computed(function () {
                return !!self.Model() && self.Model().ActiveMode() != 'view';
            })
            self.IsEditing.subscribe(function (newValue) {
                if (!!self.orgStructureControl) {
                    self.orgStructureControl.IsContextMenuEnabled(!newValue);
                }
            });
            //
            self.InitOrgStructureTree = function () {
                self.orgStructureControl = new treeLib.control('../UI/Lists/Settings/OrgStructure.TreeControl');
                self.orgStructureControl.init($('.dictionaries-orgstructure-tree'), 0, {
                    onClick: self.OnSelectOrgStructure,
                    ShowCheckboxes: false,
                    AvailableClassArray: [29, 101, 102], // 29 - Владелец, 101 - Организация, 102 - Подразделение
                    ClickableClassArray: [29, 101, 102],
                    AllClickable: false,
                    ExpandFirstLevel: true,
                    ContextMenu: self.treeControlContextMenu(),
                    IsContextMenuEnabled: !self.IsEditing()
                });

                $.when(self.orgStructureControl.$isLoaded).done(function () {
                    // 
                });
            };
            //
            self.OnSelectOrgStructure = function (node) {
                var classID = self.getObjectClassID(node);
                return self.OpenNodeEditor(classID, node, 'view');
            };
            self.OpenNodeEditor = function (classID, node, mode, confirmCancel) {
                confirmCancel = typeof confirmCancel === 'boolean' ? confirmCancel : true;
                var model = self.Model();

                if (!!model) {

                    if (confirmCancel && model.Modified())
                    {
                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('CancelingOrgStructureNodeEditTitle'),
                                text: getTextResource('ConfirmCancelOrgStructureNodeEditQuestion'),
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
                                        if (self.OpenNodeEditor(classID, node, mode, false)) {
                                            self.orgStructureControl.SelectedNode(node);
                                        }
                                    }
                                });
                        });

                        return false;
                    }

                    if (model.ClassID == classID && model.ID == self.getObjectID(node)) {
                        if (model.ActiveMode() == mode) {
                            return true;
                        } 

                        if (mode == 'edit') {
                            model.ActiveMode(mode);
                            return true;
                        }
                    }
                }
                
                var ss = function () { showSpinner($('.settingsModule')[0]); };
                var hs = function () { hideSpinner($('.settingsModule')[0]); };

                switch (classID) {
                    case 29:
                        ss();
                        self.ViewTemplateName('');
                        require(['ui_lists/Settings/OrgStructure.Owner'], function (vm) {
                            var mod = new vm.ViewModel(node, self.SelectOrgStructure, mode);
                            self.Model(mod);
                            mod.CheckData();
                            //
                            self.ViewTemplateName("../UI/Lists/Settings/OrgStructure.Owner");
                            hs();
                        });
                        break;
                    case 101:
                        ss();
                        self.ViewTemplateName('');
                        require(['ui_lists/Settings/OrgStructure.Organization'], function (vm) {
                            var mod = null;
                            if (mode == 'create') {
                                mod = new vm.ViewModel(null, node, self.SelectOrgStructure, mode);
                            } else {
                                mod = new vm.ViewModel(node, null, self.SelectOrgStructure, mode);
                            }

                            self.Model(mod);
                            mod.CheckData();
                            //
                            self.ViewTemplateName("../UI/Lists/Settings/OrgStructure.Organization");
                            hs();
                        });
                        break;
                    case 102:
                        ss();
                        self.ViewTemplateName('');
                        require(['ui_lists/Settings/OrgStructure.Subdivision'], function (vm) {
                            var mod = null;
                            if (mode == 'create') {
                                mod = new vm.ViewModel(null, node, self.SelectOrgStructure, mode);
                            } else {
                                mod = new vm.ViewModel(node, null, self.SelectOrgStructure, mode);
                            }

                            self.Model(mod);
                            mod.CheckData();
                            //
                            self.ViewTemplateName("../UI/Lists/Settings/OrgStructure.Subdivision");
                            hs();
                        });
                        break;
                    default:
                        self.ViewTemplateName("");
                }
                return true;
            };
            self.SelectOrgStructure = function (node, refreshNode) {
                if (!!refreshNode) {
                    var refreshNodeID = self.getObjectID(refreshNode);
                    var refreshNodeClassID = self.getObjectClassID(refreshNode);
                    $.when(self.orgStructureControl.refresh(refreshNodeID, refreshNodeClassID)).done(function () { return self.SelectOrgStructure(node); });
                    return;
                } 

                var nodeID = self.getObjectID(node);
                var nodeClassID = self.getObjectClassID(node);
                
                $.when(self.orgStructureControl.OpenToNode(nodeID, nodeClassID)).done(function (finalNode) {
                    if (finalNode && finalNode.ID.toLowerCase() == nodeID.toLowerCase()) {
                        if (self.OpenNodeEditor(nodeClassID, finalNode, 'view')) {
                            self.orgStructureControl.SelectNode(finalNode);
                            return true;
                        }

                        return false;
                    } 
                });
            };
            self.OnResize = function () {//чтобы была красивая прокрутка таблицы, а кнопки при этом оставались видны
                // 
            };
            {//ko.contextMenu
                self.treeControlContextMenu = ko.observable(null);
                self.contextMenuInit = function (contextMenu) {
                    self.treeControlContextMenu(contextMenu);//bind contextMenu
                    //
                    contextMenu.clearItems();
                    // Редактировать (приводит карточку Владельца в режим редактирования)
                    self.editMenuItem(contextMenu, "EditOwnerMenuItem", 29, [226]);
                    // Добавить организацию (открывает карточку Добавления Организации)
                    self.addMenuItem(contextMenu, "AddOrganizationMenuItem", 29, 101, [4]);
                    // Редактировать (приводит карточку Организации в режим редактирования)
                    self.editMenuItem(contextMenu, "EditOrganizationMenuItem", 101, [227]);
                    // Удалить организацию (должно сопровождаться контрольным вопросом Вы действительно хотите безвозвратно удалить «Наименование организации» со всеми вложенными подразделениями ? Отмена - Да, см.картинку справа)
                    self.deleteMenuItem(contextMenu, "DeleteOrganizationMenuItem", 101, [3]);
                    // Добавить подразделение (открывает карточку Добавления Подразделения)
                    self.addMenuItem(contextMenu, "AddSubdivistionMenuItem", 101, 102, [7]);
                    // Редактировать (приводит карточку Владельца в режим редактирования)
                    self.editMenuItem(contextMenu, "EditSubdivistionMenuItem", 102, [228]);
                    // Удалить подразделение (должно сопровождаться контрольным вопросом Вы действительно хотите безвозвратно удалить «Наименование подразделения» со всеми вложенными подразделениями ? Отмена - Да, см.картинку справа)
                    self.deleteMenuItem(contextMenu, "DeleteSubdivistionMenuItem", 102, [6]);
                    // Добавить подразделение (открывает карточку Добавления Подразделения)
                    self.addMenuItem(contextMenu, "AddSubdivistionMenuItem", 102, 102, [7]);
                };
                self.contextMenuOpening = function (contextMenu) {
                    contextMenu.items().forEach(function (item) {
                        if (item.isEnable && item.isVisible) {
                            item.enabled(item.isEnable());
                            item.visible(item.isVisible());
                        }
                    });
                    //
                    if (contextMenu.visibleItems().length == 0)
                        contextMenu.close();
                };
                self.editMenuItem = function (contextMenu, restext, classID, operations) {
                    var isEnable = function () {
                        return self.anyOperationIsGranted(operations);
                    };
                    var isVisible = function () {
                        //if (!self.anyOperationIsGranted(operations)) return false;

                        var selectedNode = self.orgStructureControl.SelectedNode()
                        return !!selectedNode && self.getObjectClassID(selectedNode) == classID;
                    };
                    var action = function () {
                        if (!self.anyOperationIsGranted(operations)) return false;

                        var selectedNode = self.orgStructureControl.SelectedNode()
                        if (!selectedNode || self.getObjectClassID(selectedNode) != classID) return false;

                        self.OpenNodeEditor(classID, selectedNode, 'edit');
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext(restext);
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                self.addMenuItem = function (contextMenu, restext, parentClassID, classID, operations) {
                    var isEnable = function () {
                        return self.anyOperationIsGranted(operations);
                    };
                    var isVisible = function () {
                        //if (!self.anyOperationIsGranted(operations)) return false;

                        var selectedNode = self.orgStructureControl.SelectedNode()
                        return !!selectedNode && self.getObjectClassID(selectedNode) == parentClassID;
                    };
                    var action = function () {
                        if (!self.anyOperationIsGranted(operations)) return false;

                        var selectedNode = self.orgStructureControl.SelectedNode()
                        if (!selectedNode || self.getObjectClassID(selectedNode) != parentClassID) return false;

                        self.OpenNodeEditor(classID, selectedNode, 'create');
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext(restext);
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                self.deleteMenuItem = function (contextMenu, restext, classID, operations) {
                    var isEnable = function () {
                        return self.anyOperationIsGranted(operations);
                    };
                    var isVisible = function () {
                        //if (!self.anyOperationIsGranted(operations)) return false;

                        var selectedNode = self.orgStructureControl.SelectedNode()
                        return !!selectedNode && self.getObjectClassID(selectedNode) == classID;
                    };
                    var action = function () {
                        if (!self.anyOperationIsGranted(operations)) return false;

                        var selectedNode = self.orgStructureControl.SelectedNode()
                        if (!selectedNode) return false;

                        var objectClassID = self.getObjectClassID(selectedNode);
                        if (objectClassID != classID) return false;

                        var objectID = self.getObjectID(selectedNode);
                        var name = self.getObjectName(selectedNode);
                        var question = classID == 101
                            ? getTextResource('ConfirmRemoveOrganizationQuestion').replace('{0}', name)
                            : getTextResource('ConfirmRemoveSubdivisionQuestion').replace('{0}', name)
                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('RemovingOrgStructureNodeTitle'),
                                text: question,
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
                                            'ObjectClassID': objectClassID,
                                            'ObjectID': objectID
                                        };
                                        self.ajaxControl.Ajax(null,
                                            {
                                                dataType: "json",
                                                method: 'POST',
                                                data: data,
                                                url: '/assetApi/RemoveOrgStructureObject'
                                            },
                                            function (res) {
                                                if (res.Result == 0) {
                                                }
                                                else {
                                                    require(['sweetAlert'], function () {
                                                        swal(res.Message, '', 'info');
                                                    });
                                                }
                                                // Обновляем дерево с задержкой
                                                self.orgStructureControl.refresh(objectID, objectClassID); 
                                            });
                                    }
                                });
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext(restext);
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                {//splitter
                    self.minSplitterWidth = 200;
                    self.maxSplitterWidth = 700;
                    self.splitterDistance = ko.observable(300);
                    self.resizeSplitter = function (newWidth) {
                        if (newWidth && newWidth >= self.minSplitterWidth && newWidth <= self.maxSplitterWidth) {
                            self.splitterDistance(newWidth);
                        }
                    };
                }
            }
            self.ContextMenuRequested = function (sender, e) {
                e.preventDefault();
                return true;
            }
            self.dispose = function () {
                if (self.treeControlContextMenu() != null)
                    self.treeControlContextMenu().dispose();
                //TODO other fields and controls
            };
            {//identification
                self.getObjectID = function (obj) {
                    return obj.ID.toUpperCase();
                };
                self.getObjectClassID = function (obj) {
                    return obj.ClassID;
                };
                self.getObjectName = function (obj) {
                    return obj.Name;
                };
                self.getObjectIconClass = function (obj) {
                    return obj.IconClass;
                };
            }
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
                self.anyOperationIsGranted = function (operationIDs) {
                    for (var i = 0; i < self.grantedOperations.length; i++)
                        if (operationIDs.indexOf(self.grantedOperations[i]) >= 0)
                            return true;
                    return false;
                };
            }
        }
    }
    return module;
});
