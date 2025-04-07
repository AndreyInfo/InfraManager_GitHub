define(['knockout', 'jquery', 'ajax', 'treeControl', 'ui_controls/ListView/ko.ListView.Cells'], function (ko, $, ajaxLib, treeLib, mCells) {
    var module = {
        Tab: function (vm) {
            var self = this;
            //
            self.ajaxControl = new ajaxLib.control();
            self.Template = '../UI/Forms/Asset/AssetOperations/tab_place';
            self.$region = vm.$region;

            {//fields
                self.isLoaded = false;
            }

            //when object changed
            self.Initialize = function (obj) {
            };
            //when tab selected
            self.load = function () {
                vm.navigatorObjectID(null);
                vm.navigatorObjectClassID(null);
                if (self.isLoaded === true)
                    return;
                //
                self.InitLocationTree = function () {
                    var retD = $.Deferred();

                        vm.locationControl(new treeLib.control());
                        vm.locationControl().init($('#regionLocation2'), 1, {
                            onClick: vm.navigator_nodeSelected,
                            ShowCheckboxes: false,
                            AvailableClassArray: [29, 101, 1, 2, 3, 4, 22],
                            ClickableClassArray: [29, 101, 1, 2, 3, 4, 22],
                            AllClickable: true,
                            FinishClassArray: [4, 22],
                            ExpandFirstNodes: true
                        });
                    //
                    $.when(vm.locationControl().$isLoaded).done(function () {
                        retD.resolve();
                    });
                    //
                    return retD.promise();
                };

                self.isLoaded = true;
            };

            self.AfterRender = function () {
                self.InitLocationTree();
                vm.navigatorObjectID(null);
                vm.navigatorObjectClassID(null);
            }

            //when tab unload
            self.dispose = function () {
                self.isLoaded = false;
                self.ajaxControl.Abort();
                if (self.lv != null) {
                    self.lv_checkedItemsBeforeChanged_handle.dispose();
                    self.lv_checkedItemsChanged_handle.dispose();
                    self.lv.dispose();
                }
            };

            {//events of listView

                self.lv = null;
                self.listViewID = 'listView_' + ko.getNewID();
                self.lv_checkedItemsBeforeChanged_handle = null;
                self.lv_checkedItemsChanged_handle = null;
                //
                self.lvInit = function (listView) {
                    self.lv = listView;
                    self.lv_checkedItemsBeforeChanged_handle = listView.rowViewModel.checkedItemsToSubscribe.subscribe(function (oldObjectList) {
                        self.tmp = oldObjectList;
                    }, null, "beforeChange");
                    self.lv_checkedItemsChanged_handle = listView.rowViewModel.checkedItemsToSubscribe.subscribe(function (newObjectList) {
                        var oldObjectList = self.tmp;
                        self.tmp = undefined;
                        //
                        if (self.selectedItemFreeze)
                            return;
                        var selectedItems = vm.selectedItems();//информация о выбранных объектах
                        //
                        //нужно найти снятые чекбоксы
                        for (var j = 0; j < oldObjectList.length; j++) {
                            var exists = false;
                            var id = oldObjectList[j].ID.toUpperCase();
                            for (var i = 0; i < newObjectList.length; i++)
                                if (newObjectList[i].ID.toUpperCase() == id) {
                                    exists = true;
                                    break;
                                }
                            if (exists === false) {
                                for (var i = 0; i < selectedItems.length; i++)
                                    if (selectedItems[i].ID.toUpperCase() == id) {
                                        let rows = self.lv.rowViewModel.rowList();
                                        rows.forEach(function (row) {
                                            if (row.object.ID == selectedItems[i].ID) {
                                                const cellList = row.cells();
                                                cellList.forEach(function (cell) {
                                                    if (cell.column.MemberName == "SoftwareExecutionCount") {
                                                        var value = parseInt(cell.value(), 10);
                                                        if (value > 1)
                                                            cell.value(1);
                                                    }
                                                });
                                            }
                                        });
                                        selectedItems.splice(i, 1);//убираем снятый чекбокс
                                        break;
                                    }
                            }
                        }
                        //нужно добавить установленные чекбоксы
                        for (var j = 0; j < newObjectList.length; j++) {
                            var exists = false;
                            var id = newObjectList[j].ID.toUpperCase();
                            for (var i = 0; i < selectedItems.length; i++)
                                if (selectedItems[i].ID.toUpperCase() == id) {
                                    exists = true;
                                    break;
                                }
                            if (exists === false && (vm.Rights() - vm.getReferenceCount()) > 0) {
                                var row = self.lv.rowViewModel.getRowByObjectID(newObjectList[j].ID);
                                const cellList = row.cells();
                                cellList.forEach(function (cell) {
                                    if (cell.column.MemberName == "SoftwareExecutionCount") {
                                        var value = parseInt(cell.value(), 10);
                                        if (value > 1)
                                            cell.value(1);
                                    }
                                });
                                selectedItems.push(newObjectList[j])
                            }
                            else if (exists === false && (vm.Rights() - vm.getReferenceCount()) <= 0) {
                                var row = self.lv.rowViewModel.getRowByObjectID(newObjectList[j].ID);
                                row.checked(false);
                            }
                        }
                        //
                        vm.selectedItems(selectedItems);
                    });
                    //
                    var storedLoad = self.lv.load;
                    self.lv.load = function () {
                        var retvalD = $.Deferred();
                        self.selectedItemFreeze = true;
                        $.when(storedLoad()).done(function () {
                            self.selectedItemFreeze = false;
                            //vm.markListViewSelection();
                            retvalD.resolve();
                        });
                        return retvalD.promise();
                    };
                    //
                    self.lv.load();
                };
                self.lvRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
                    var retvalD = $.Deferred();
                    $.when(vm.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                        retvalD.resolve(objectList);
                        //
                        //vm.markListViewSelection();
                    });
                    return retvalD.promise();
                };
                self.lvRowClick = function (obj) {

                };
                self.listViewDrawCell = function (obj, column, cell) {
                    if (column.IsEdit && column.MemberName == 'SoftwareExecutionCount') {
                        column.Template("../UI/Forms/Asset/AssetOperations/CellTemplates/sdNumberEditor");
                        cell.value(obj[column.MemberName]);
                    }
                    else {
                        cell.text = mCells.textRepresenter(obj, column);
                    }
                };
            }
        }
    }
    return module;
});