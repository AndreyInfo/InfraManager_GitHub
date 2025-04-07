define(['knockout', 'jquery'], function (ko, $) {
    var module = {
        rowType: {
            loadedFromServer: 0,
            outdated: 1,
            newer: 2
        },
        row: function (object) {
            var self = this;
            //
            self.object = object;
            self.type = ko.observable(module.rowType.loadedFromServer);
            self.checked = ko.observable(false);
            //
            self.domElement = ko.observable(null);
            //
            return self;
        },
        rowViewModel: function (listView) {
            var self = this;
            self.rowList = ko.observableArray([]);
            self.rowListCount = ko.pureComputed(function () {
                return self.rowList().length;
            });
            self.sortedRowList = ko.pureComputed(function () {
                var sortColumn = null;
                if (listView.options.virtualMode() === false)
                    sortColumn = listView.columnViewModel.sortColumn();
                //
                var retval = null;
                if (sortColumn != null) {
                    var asc = sortColumn.SortAsc() === true;
                    var numberList = ko.observableArray([]);
                    var stringList = ko.observableArray([]);
                    self.rowList().forEach(function (row) {
                        var tmp = Number(row.object[sortColumn.MemberName])
                        if (!isNaN(tmp)) 
                            numberList.push(row);
                         else
                            stringList.push(row);
                    });
                    numberList = numberList.sort(function (r1, r2) {
                        var data1 = r1.object[sortColumn.MemberName];
                        var data2 = r2.object[sortColumn.MemberName];
                            data1 = Number(data1);
                            data2 = Number(data2);
                        return data1 == data2 ? 0 : (data1 < data2 ? -1 : 1) * (asc == true ? 1 : -1);
                    })();
                    stringList = stringList.sort(function (r1, r2) {
                        var data1 = r1.object[sortColumn.MemberName];
                        var data2 = r2.object[sortColumn.MemberName];
                            data1 = data1.toLowerCase();
                            data2 = data2.toLowerCase();
                        return data1 == data2 ? 0 : (data1 < data2 ? -1 : 1) * (asc == true ? 1 : -1);
                    })();
                    retval = asc == true ? numberList.concat(stringList) : stringList.concat(numberList);
                }
                else
                    retval = self.rowList();
                //
                return retval;
            }).extend({ rateLimit: { timeout: 100, method: "notifyWhenChangesStop" } });
            self.orderedSelectedList = [];

            //
            self.createRow = function (object) {
                if (object && object.ParameterValueList) {//virtual columns
                    for (var i = 0; i < object.ParameterValueList.length; i++) {
                        var parameterValueInfoAsColumn = object.ParameterValueList[i];
                        object[listView.virtualColumnMemberNamePrefix + parameterValueInfoAsColumn.ID] = parameterValueInfoAsColumn.Value;//declare property
                        listView.columnViewModel.createVirtualColumnIfNotExists(parameterValueInfoAsColumn.ID, parameterValueInfoAsColumn.Name);
                    }
                    object.ParameterValueList = undefined;
                    object.VirtualColumns = true;
                }
                //
                var row = new module.row(object);
                //
                row.css = ko.pureComputed(function () {
                    var retval = self.focusedRow() === row ? listView.options.rowFocusedClass : '';
                    if (row.checked() === true)
                        retval += ' ' + listView.options.rowCheckedClass;
                    if (self.canOpenRowForm() == true)
                        retval += ' ' + listView.options.rowClicableClass;
                    if (row.index() % 2 == 0)
                        retval += ' ' + listView.options.rowColorEvenClass;
                    else
                        retval += ' ' + listView.options.rowColorOddClass;
                    if (row.type() === module.rowType.outdated)
                        retval += ' ' + listView.options.rowOutdatedClass;
                    else if (row.type() === module.rowType.newer)
                        retval += ' ' + listView.options.rowNewerClass;
                    return retval;
                });
                row.cells = ko.pureComputed(function () {
                    var cellCreator = listView.cellCreator;
                    var sortedVisibleColumList = listView.columnViewModel.sortedVisibleColumnList();
                    //
                    var retval = [];
                    sortedVisibleColumList.forEach(function (column) {
                        var cell = cellCreator(listView, row.object, column, row);
                        retval.push(cell);
                    });
                    return retval;
                });
                row.index = ko.pureComputed(function () {
                    var list = self.sortedRowList();
                    return list.indexOf(row);
                });
                row.listView = listView;
                //
                return row;
            };
            self.disposeRow = function (row, notificate) {
                row.css.dispose();
                row.cells.dispose();
                row.index.dispose();
                //
                if (self.focusedRow() === row)
                    self.focusedRow(null);
                //
                var index = self.rowList().indexOf(row);
                if (index > -1) {
                    self.rowList().splice(index, 1);
                    if (notificate == true)
                        self.rowList.valueHasMutated();
                }
            };
            //
            self.dispose = function () {
                self.clear();
                self.checkedItems.dispose();
                self.canOpenRowForm.dispose();
                self.sortedRowList.dispose();
                self.rowListCount.dispose();
            };
            //
            {//fill, clear
                self.clear = function (refresh) {
                    for (var i = self.rowList().length - 1; i >= 0; i--) {
                        var row = self.rowList()[i];
                        self.disposeRow(row, false);
                    };

                    if (refresh) {
                        self.rowList.valueHasMutated();
                    };
                        
                };
                self.load = function (objectList) {
                    self.clear(false);
                    //
                    var retval = [];
                    var hasVirtualColumns = false;
                    objectList.forEach(function (object) {
                        var row = self.createRow(object);
                        if (object.VirtualColumns == true)
                            hasVirtualColumns = true;
                        self.rowList().push(row);
                        retval.push(row);
                    });
                    if (hasVirtualColumns)
                        listView.columnViewModel.columnList.valueHasMutated();
                    self.rowList.valueHasMutated();
                    return retval;
                };
                self.append = function (objectList, unshiftMode) {
                    var retval = [];
                    var getObjectIDFunc = listView.options.getObjectID();
                    var hasVirtualColumns = false;
                    objectList.forEach(function (object) {
                        //before we should remove equal object and replace it with new data
                        var objectIdentifier = getObjectIDFunc(object);
                        var existRow = self.getRowByObjectID(objectIdentifier);
                        var index = -1;
                        if (existRow != null) {
                            index = self.rowList().indexOf(existRow);
                            if (index != -1)
                                self.rowList().splice(index, 1);
                        }
                        //
                        var row = self.createRow(object);
                        if (object.VirtualColumns == true)
                            hasVirtualColumns = true;
                        if (index != -1)
                            self.rowList().splice(index, 0, row);
                        else if (unshiftMode === true)
                            self.rowList().unshift(row);
                        else
                            self.rowList().push(row);
                        retval.push(row);
                    });
                    if (hasVirtualColumns)
                        listView.columnViewModel.columnList.valueHasMutated();
                    self.rowList.valueHasMutated();
                    return retval;
                };
            }
            //
            {//row selection
                self.focusedRow = ko.observable(null);
                //                
                self.keyDown = function (vm, e) {
                    var k = e.which || e.keyCode;
                    var ensureVisible = function (scroll) {
                        if (self.focusedRow() != null && self.focusedRow().domElement() != null) {
                            self.focusedRow().domElement().focus();
                            if (scroll === true)
                                self.focusedRow().domElement().scrollIntoView(false);
                        }
                    };
                    switch (k) {
                        case 36: {//36 - home
                            var action = function () {
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > 0 ? list[0] : null);
                                ensureVisible(true);
                            };
                            if (listView.scrollTop() > 0) {
                                var handle = null;
                                handle = listView.visibleRowList.subscribe(function (newList) {
                                    action();
                                    handle.dispose();
                                });
                                listView.setScrollTop(0);
                            } else
                                action();
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 38: {//38 - up                            
                            var itemHeight = listView.getItemHeight();
                            if (itemHeight === null)
                                return;
                            var index = listView.visibleRowList().indexOf(self.focusedRow());
                            if (index === -1) {
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > 0 ? list[0] : null);
                                ensureVisible(true);
                            }
                            else if (index > 0) {
                                index--;
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > index ? list[index] : null);
                                ensureVisible();
                            }
                            else {
                                var action = function () {
                                    var list = listView.visibleRowList();
                                    self.focusedRow(list.length > 0 ? list[0] : null);
                                    ensureVisible(true);
                                };
                                var newScrollPosition = Math.max(0, listView.scrollTop() - itemHeight);
                                if (listView.scrollTop() > newScrollPosition) {
                                    var handle = null;
                                    handle = listView.visibleRowList.subscribe(function (newList) {
                                        action();
                                        handle.dispose();
                                    });
                                    listView.setScrollTop(newScrollPosition);
                                } else
                                    action();
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 33: {//38 - pageUp
                            var itemsByHeight = listView.getItemsCountByHeight();
                            var itemHeight = listView.getItemHeight();
                            if (itemsByHeight === null || itemHeight === null)
                                return;
                            var action = function () {
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > 0 ? list[0] : null);
                                ensureVisible(true);
                            };
                            var newScrollPosition = Math.max(0, listView.scrollTop() - itemHeight * itemsByHeight);
                            if (listView.scrollTop() > newScrollPosition) {
                                var handle = null;
                                handle = listView.visibleRowList.subscribe(function (newList) {
                                    action();
                                    handle.dispose();
                                });
                                listView.setScrollTop(newScrollPosition);
                            } else
                                action();
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 40: {//40 - down
                            var itemsByHeight = listView.getItemsCountByHeight();
                            var itemHeight = listView.getItemHeight();
                            if (itemsByHeight === null || itemHeight === null)
                                return;
                            var index = listView.visibleRowList().indexOf(self.focusedRow());
                            if (index === -1) {
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > 0 ? list[0] : null);
                                ensureVisible(true);
                            }
                            if (index < itemsByHeight && listView.visibleRowList().length > index + 1) {
                                index++;
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > index ? list[index] : null);
                                ensureVisible(false);
                            }
                            else {
                                var action = function () {
                                    var list = listView.visibleRowList();
                                    self.focusedRow(list.length > 0 ? list[list.length - 1] : null);
                                    ensureVisible(true);
                                };
                                var maxScrollPosition = listView.getScrollMaxPosition();
                                var newScrollPosition = Math.min(maxScrollPosition, listView.scrollTop() + itemHeight);
                                if (listView.scrollTop() < newScrollPosition) {
                                    var handle = null;
                                    handle = listView.visibleRowList.subscribe(function (newList) {
                                        action();
                                        handle.dispose();
                                    });
                                    listView.setScrollTop(newScrollPosition);
                                } else
                                    action();
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 34: {//34 - pagedown
                            var itemsByHeight = listView.getItemsCountByHeight();
                            var itemHeight = listView.getItemHeight();
                            if (itemsByHeight === null || itemHeight === null)
                                return;
                            var action = function () {
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > 0 ? list[list.length - 1] : null);
                                ensureVisible(true);
                            };
                            var maxScrollPosition = listView.getScrollMaxPosition();
                            var newScrollPosition = Math.min(maxScrollPosition, listView.scrollTop() + itemHeight * itemsByHeight);
                            if (listView.scrollTop() < newScrollPosition) {
                                var handle = null;
                                handle = listView.visibleRowList.subscribe(function (newList) {
                                    action();
                                    handle.dispose();
                                });
                                listView.setScrollTop(newScrollPosition);
                            } else
                                action();
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 35: {//35 - end
                            var action = function () {
                                var list = listView.visibleRowList();
                                self.focusedRow(list.length > 0 ? list[list.length - 1] : null);
                                ensureVisible(true);
                            };
                            var maxScrollPosition = listView.getScrollMaxPosition();
                            if (listView.scrollTop() < maxScrollPosition) {
                                var handle = null;
                                handle = listView.visibleRowList.subscribe(function (newList) {
                                    action();
                                    handle.dispose();
                                });
                                listView.setScrollTop(maxScrollPosition);
                            } else
                                action();
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 32: {//32 - space
                            var row = self.focusedRow();
                            if (row != null && listView.options.multiSelect() == true)
                                row.checked(!row.checked());
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 13: {//13 - enter
                            var row = self.focusedRow();
                            var showFormFunc = listView.options.itemClick();
                            if (showFormFunc != null && row != null)
                                showFormFunc(row.object);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 123: {//f12
                            return true;
                        }
                        case 116: {//f5
                            window.location.reload(true);
                            //
                            e.stopPropagation();
                            break;
                        }
                    }
                    return true;
                };
            }
            //
            {//row clicking
                self.canOpenRowForm = ko.pureComputed(function () {
                    var showFormFunc = listView.options.itemClick();
                    return showFormFunc != null;
                });
                self.rowClick = function (cell, e) {
                    self.focusedRow(cell.row);
                    //
                    var showFormFunc = listView.options.itemClick();
                    if (showFormFunc != null) {
                        showFormFunc(cell.row.object);
                    }                 
                };
                self.mouseDown = function (row, e) {
                    self.focusedRow(row);
                    return true;
                };
                //
                self.contextMenuRequested = function (vm, e) {
                    if (self.showContextMenu(e)) {
                        e.preventDefault();
                        return false;
                    }
                    return true;
                };
                //
                self.showContextMenu = function (e) {
                    var contextMenuViewModel = listView.options.contextMenu();
                    if (contextMenuViewModel != null)
                        return contextMenuViewModel.show(e);
                    else
                        return false;
                };
            }
            //
            {//row checking
                self.subscriptionList = [];
                self.rowChecked = ko.observable(null);//подписаться сюда, если нужно событие выделения строки
                //
                self.rowsCheckedSubscribe = ko.computed(function () {
                    for (var i in self.subscriptionList) {
                        self.subscriptionList[i].dispose();
                    }
                    self.subscriptionList = [];
                    //
                    self.rowList().forEach(function (row) {
                        var subscription = row.checked.subscribe(function () {
                            self.rowChecked(row);
                        });
                        self.subscriptionList.push(subscription);
                    });
                });
                //
                self.checkedItemsToSubscribe = ko.computed(function () {
                    self.rowList().forEach(function (row) {
                        if (row.checked() === true) {
                            if (!self.HasOrderedSelectedRow(row.object))
                                self.orderedSelectedList.push(row.object);
                        }
                        else {
                            if (self.HasOrderedSelectedRow(row.object))
                                self.RemoveOrderedSelectedRow(row.object);
                        }
                    });
                    return self.orderedSelectedList;
                });
                self.checkedItems = ko.pureComputed({
                    read: function () {
                        self.rowList().forEach(function (row) {
                            if (row.checked() === true) {
                                if (!self.HasOrderedSelectedRow(row.object))
                                    self.orderedSelectedList.push(row.object);
                            }
                            else {
                                if (self.HasOrderedSelectedRow(row.object))
                                    self.RemoveOrderedSelectedRow(row.object);
                            }
                        });
                        if (self.orderedSelectedList.length === 0) {
                            var focusedRow = self.focusedRow();
                            if (focusedRow != null)
                                self.orderedSelectedList.push(focusedRow.object);
                        }
                        return self.orderedSelectedList;
                    },
                    write: function (objectList) {
                        self.rowList().forEach(function (row) {
                            row.checked(false);
                        });
                        //
                        var getObjectIDFunc = listView.options.getObjectID();
                        objectList.forEach(function (obj) {
                            var objectID = getObjectIDFunc(obj);
                            var row = self.getRowByObjectID(objectID);
                            if (row != null)
                                row.checked(true);
                        });
                    }
                });
                self.allItemsChecked = ko.pureComputed({
                    read: function () {
                        var list = self.rowList();
                        if (list.length == 0)
                            return false;
                        for (var i = 0; i < list.length; i++)
                            if (list[i].checked() === false)
                                return false;
                        return true;
                    },
                    write: function (value) {
                        if (value == true) {
                            for (var i in self.subscriptionList) {
                                self.subscriptionList[i].dispose();
                            }
                            self.subscriptionList = [];
                        }
                        //
                        self.rowList().forEach(function (row) {
                            row.checked(value);
                            //
                            if (value == true) {
                                var subscription = row.checked.subscribe(function (newValue) {
                                    self.rowChecked(row);
                                });
                                self.subscriptionList.push(subscription);
                            }
                        });
                    }
                });
            }
            self.HasOrderedSelectedRow = function (row) {
                return self.orderedSelectedList.find(element => element.ID == row.ID);
            }
            self.RemoveOrderedSelectedRow = function(row) {
                self.orderedSelectedList = self.orderedSelectedList.filter(function (element) {
                    return element.ID != row.ID;
                });
            }
            //
            self.getRowByObjectID = function (objectIdentifier) {
                var searchID = typeof objectIdentifier.toUpperCase === 'function' ? objectIdentifier.toUpperCase() : objectIdentifier;
                var list = self.sortedRowList();
                var getObjectIDFunc = listView.options.getObjectID();

                if (!list) {
                    return null;
                }

                for (var i = list.length - 1; i >= 0; i--) {
                    var row = list[i];
                    var objectID = getObjectIDFunc(row.object);
                    if (objectID === searchID)
                        return row;

                }
                return null;
            };

        }
    }
    return module;
});