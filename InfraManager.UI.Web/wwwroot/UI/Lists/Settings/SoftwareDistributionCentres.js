define(['knockout', 'jquery', 'ajax', 'plainList', 'managedAccessObjectList', 'ui_controls/ContextMenu/ko.ContextMenu'], function (ko, $, ajaxLib, plainList, dataSourceLib) {
    var module = {
        ViewModel: function () {
            var self = this;
            //
            self.ajaxControl = new ajaxLib.control();
            //            
            self.ModelAfterRender = function () {
                if (self.listControl == null) {
                    self.InitList();
                }
                self.OnResize();
            };
            //
            self.CheckData = function () {//вызывается извне, сообщает, что пора обновлять/загружать данные
                self.InitList();
            };
            self.ViewTemplateName = ko.observable(''); //шаблон контента (справа) 
            self.ViewTemplateName.subscribe(function (newValue) {
                if (newValue == '') {
                    if (self.Model() != null) {
                        if (typeof self.Model().dispose !== "undefined") {
                            self.Model().dispose();
                        }
                        self.Model(null);
                    }
                }
            });
            self.Model = ko.observable(null); //контент
            //
            self.InitList = function () {
                var dataSource = new dataSourceLib.dataSource(
                    '/assetApi/SoftwareDistributionCentres',
                    function (rawData) {
                        return {
                            ID: rawData.ID,
                            Name: rawData.Name,
                            ResponsiblePerson: { Name: rawData.ResponsiblePersonName }
                        };
                    });
                self.listControl = new plainList.control(
                    '../UI/Lists/Settings/SoftwareDistributionCentres.List',
                    dataSource,
                    { id: 'ID' });
                self.listControl.init($('.dictionaries-sdc-tree'), 0, {
                    onClick: self.OnSelectSoftwareDistributionCentre,
                    ContextMenu: self.listControlContextMenu(),
                    IsContextMenuEnabled: true
                });
            };
            //
            self.OnSelectSoftwareDistributionCentre = function (dataItem) {
                return self.TryViewDetails(dataItem);
            };
            self.TryViewDetails = function (dataItem) {
                var model = self.Model();

                if (!!model) {

                    if (model.id == dataItem.ID) {
                        return true;
                    }

                    if (model.hasChanges()) {

                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('SoftwareDistributionCentreCancelTitle'),
                                text: getTextResource('SoftwareDistributionCentreConfirmDiscard'),
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
                                        if (self.ViewDetails(dataItem) && dataItem.ID) {
                                            self.listControl.SelectedItem(dataItem);
                                        }
                                    }
                                });
                        });

                        return null;
                    }
                }

                return self.ViewDetails(dataItem);              
            };
            self.ViewDetails = function (dataItem) {
                var asyncEvent = $.Deferred();
                var ss = function () { showSpinner($('.dictionaries-sdc-main')[0]); };
                var hs = function () { hideSpinner($('.dictionaries-sdc-main')[0]); };

                ss();
                self.ViewTemplateName('');
                require(['ui_lists/Settings/SoftwareDistributionCentres.Details'], function (module) {
                    var oldModel = self.Model();
                    if (oldModel) {
                        oldModel.changesSaved = null;
                    }
                    //
                    var viewModel = new module.ViewModel(dataItem);
                    self.Model(viewModel);
                    viewModel.changesSaved = self.itemChanged;
                    viewModel.onCancel = function () {
                        self.ViewTemplateName('');
                        self.Model(null);
                    };
                    //
                    self.ViewTemplateName("../UI/Lists/Settings/SoftwareDistributionCentres.Details");
                    viewModel.init();
                    hs();
                    asyncEvent.resolve();

                    if (viewModel.isNew()) {
                        self.listControl.DeselectItem();
                    }
                });
                
                return asyncEvent.promise();
            };
            self.itemChanged = function () {
                var currentId = self.Model().id;
                $.when(self.listControl.Load()).done(function () {
                    var modifiedItem = self.listControl.FirstOrDefault(currentId);
                    if (modifiedItem !== null) {
                        self.listControl.SelectItem(modifiedItem);
                    }
                });;
            };
            self.SelectSoftwareDistributionCentre = function (item) {

            };
            self.OnResize = function () {//чтобы была красивая прокрутка таблицы, а кнопки при этом оставались видны
                // 
            };
            {//ko.contextMenu
                self.listControlContextMenu = ko.observable(null);
                self.contextMenuInit = function (contextMenu) {
                    self.listControlContextMenu(contextMenu);//bind contextMenu
                    //
                    contextMenu.clearItems();

                    self.addMenuItem(contextMenu);
                    self.editMenuItem(contextMenu);
                    self.deleteMenuItem(contextMenu);
                };
                self.contextMenuOpening = function (contextMenu) {
                    var selectedItem = self.listControl.SelectedItem();

                    contextMenu.items().forEach(function (item) {
                        if (item.isEnable && item.isVisible) {
                            item.enabled(item.isEnable(selectedItem));
                            item.visible(item.isVisible(selectedItem));
                        }
                    });
                    //
                    if (contextMenu.visibleItems().length == 0)
                        contextMenu.close();
                };
                self.addMenuItem = function (contextMenu) {
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext("AddSoftwareDistributionCentreMenuItem");
                    cmd.isEnable = function () { return true; };
                    cmd.isVisible = function () { return self.listControl.dataSource.allowAdd; };
                    cmd.click(function () { self.TryViewDetails(self.listControl.dataSource.newObject); });
                };
                self.editMenuItem = function (contextMenu) {
                    var action = function () {
                        var selectedItem = self.listControl.SelectedItem();
                        var model = self.Model();

                        if (model && !model.editing()) {
                            model.editing(true);
                        } else if (selectedItem) {
                            var promise = self.TryViewDetails(selectedItem);
                            if (promise) {
                                promise.when(function () {
                                    self.Model().editing(true);
                                });
                            }
                        }
                    };

                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext("EditSoftwareDistributionCentreMenuItem");
                    cmd.isEnable = function (selectedItem) { return selectedItem.canBeEdited; };
                    cmd.isVisible = function (selectedItem) { return selectedItem.allowEdit; };
                    cmd.click(action);
                };
                self.deleteMenuItem = function (contextMenu) {
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext("DeleteSoftwareDistributionCentreMenuItem");
                    cmd.isEnable = function (selectedItem) { return selectedItem.canBeDeleted; };
                    cmd.isVisible = function (selectedItem) { return selectedItem.allowDelete; };
                    cmd.click(function () {
                        var selectedItem = self.listControl.SelectedItem();

                        if (!selectedItem) {
                            return;
                        }

                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('SoftwareDistributionCentreConfirmDeleteTitle'),
                                text: getTextResource('SoftwareDistributionCentreConfirmDeleteQuestion'),
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
                                    self.ajaxControl.Ajax(null,
                                        {
                                            dataType: "json",
                                            method: 'DELETE',
                                            url: '/assetApi/SoftwareDistributionCentres/' + selectedItem.ID
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
                                            self.listControl.Load();                                            
                                            self.ViewTemplateName('');
                                            self.Model(null);
                                        });
                                }
                            });
                        });
                    });
                }
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
                if (self.listControlContextMenu() != null)
                    self.listControlContextMenu().dispose();
                //TODO other fields and controls
            };
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