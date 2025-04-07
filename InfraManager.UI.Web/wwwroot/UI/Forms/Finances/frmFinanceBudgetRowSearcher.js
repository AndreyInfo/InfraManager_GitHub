define(['knockout', 'jquery', 'ajax', 'formControl', 'dateTimeControl', 'models/FinanceForms/ActivesRequestSpecification', 'models/Table/TableParts', 'treeAndSearchControl', 'jqueryStepper'],
    function (ko, $, ajaxLib, formControl, dtLib, specLib, tpLib, treeLib) {
        var module = {
            MaxCount: 1000000,

            ViewModel: function (purchaseSpecificationOrWorkOrderID) {
                var self = this;
                //
                self.ajaxControl = new ajaxLib.control();
                //
                self.modes = {
                    general: 'general',
                    selected: 'selected'
                };
                self.mode = ko.observable(self.modes.general);
                //
                self.SelectedList = ko.observableArray([]);
                self.SelectedList_Sum = ko.computed(function () {
                    if (self.SelectedList().length == 0)
                        return 0;
                    //
                    var retval = 0.0;
                    ko.utils.arrayForEach(self.SelectedList(), function (el) {
                        retval += self.getFloatValue(el.Sum());
                    });
                    retval = specLib.Normalize(retval);
                    //
                    return retval;
                });
                self.getFloatValue = function (val) {
                    if (val)
                        return parseFloat(val.toString().replace(',', '.').split(' ').join(''));
                    else
                        return 0;
                };
                self.SelectedList_SumString = ko.computed(function () {
                    return specLib.ToMoneyString(self.SelectedList_Sum());
                });
                self.SelectedTabText = ko.computed(function () {
                    var list = self.SelectedList();
                    if (list.length > 0)
                        return '(' + list.length + ')';
                    else
                        return '';
                });
                //            
                self.getSelectedObject = function (id) {
                    var tmp = null;
                    for (var i = 0; i < self.SelectedList().length; i++)
                        if (self.SelectedList()[i].ID.toUpperCase() == id.toUpperCase()) {
                            tmp = self.SelectedList()[i];
                            break;
                        }
                    return tmp;
                };
                self.rowSelected_lock = false;
                self.rowSelected = function (ids, checked) {
                    if (self.rowSelected_lock) return;
                    self.rowSelected_lock = true;
                    if (ids == null) {//clear selection
                        self.SelectedList([]);
                        self.rowSelected_lock = false;
                        return;
                    }
                    //
                    var toRequestID = [];
                    for (var j = 0; j < ids.length; j++) {
                        var tmp = self.getSelectedObject(ids[j]);
                        if (tmp != null) {
                            tmp.Selected(checked);
                            if (checked == false) {
                                var index = self.SelectedList().indexOf(tmp);
                                self.SelectedList().splice(index, 1);
                                //
                                self.table().UnckeckRow(ids[j]);
                            }
                        }
                        else
                            toRequestID.push(ids[j]);
                    }
                    if (self.SelectedList().length == 0)
                        self.mode(self.modes.general);
                    self.SelectedList.valueHasMutated();
                    //
                    if (toRequestID.length > 0 && checked == true) {
                        var data = {
                            'IDList': toRequestID,
                            'PurchaseSpecificationOrWorkOrderID': purchaseSpecificationOrWorkOrderID
                        };
                        //
                        var $region = $('#' + self.frm.GetRegionID());
                        self.ajaxControl.Ajax($region,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: data,
                                url: '/finApi/GetFinanceBudgetRowSearchList'
                            },
                            function (model) {
                                if (model.Result === 0) {
                                    var list = model.List;
                                    if (list) {
                                        ko.utils.arrayForEach(list, function (item) {
                                            var row = new module.FinanceBudgetRow(item, self.rowSelected, self.isRowSelected, self.showObjectForm);
                                            self.SelectedList().push(row);
                                            row.Selected(true);
                                        });
                                        //
                                        self.SelectedList.sort(function (left, right) {
                                            return left.Identifier == right.Identifier ?
                                                (left.Name == right.Name ? 0 : (left.Name < right.Name ? -1 : 1)) :
                                                (left.Identifier < right.Identifier ? -1 : 1);
                                        });
                                        self.SelectedList.valueHasMutated();
                                    }
                                }
                                else {
                                    if (model.Result === 1) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[frmFinanceBudgetRowSearcher.js, rowSelected]', 'error');
                                        });
                                    }
                                    else {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[frmFinanceBudgetRowSearcher.js, rowSelected]', 'error');
                                        });
                                    }
                                }
                            });
                    }
                    self.rowSelected_lock = false;
                };
                self.isRowSelected = function (id) {
                    var tmp = self.getSelectedObject(id);
                    return tmp == null ? false : tmp.Selected();
                };
                self.showObjectForm = function (id) {
                    require(['financeForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowFinanceBudgetRow(id);
                    });
                };
                //
                self.searchTimeout = null;
                self.SearchText = ko.observable('');
                self.SearchText.subscribe(function (newValue) {
                    clearTimeout(self.searchTimeout);
                    self.searchTimeout = setTimeout(function () {
                        if (newValue == self.SearchText())
                            self.Search();
                    }, 500);
                });
                self.FilterDicDate = ko.observable(new Date());
                self.FilterDic = ko.computed(function () {//'frmFinanceBudgetRowSearcher_Table'
                    self.FilterDicDate();
                    //
                    var financeCenterID = []
                    if (self.tscFinanceCenter)
                        ko.utils.arrayForEach(self.tscFinanceCenter.SelectedValues(), function (item) { financeCenterID.push(item.ID); });
                    //
                    var budgetArticleID = []
                    if (self.tscBudgetArticle)
                        ko.utils.arrayForEach(self.tscBudgetArticle.SelectedValues(), function (item) { budgetArticleID.push(item.ID); });
                    //
                    var initiatorID = []
                    if (self.tscInitiator)
                        ko.utils.arrayForEach(self.tscInitiator.SelectedValues(), function (item) { initiatorID.push(item.ID); });
                    //
                    return {
                        'PurchaseSpecificationOrWorkOrderID': purchaseSpecificationOrWorkOrderID,
                        'SearchText': self.SearchText(),
                        'FinanceCenterID': financeCenterID,
                        'BudgetArticleID': budgetArticleID,
                        'InitiatorID': initiatorID,
                    };
                });
                self.Search = function () {
                    self.table().Reload();
                };
                //
                self.tableColumns = ko.observable(null);
                self.table = ko.observable(null);
                //
                self.OnFilterChange = function () {
                    if (self.tscFinanceCenter)
                        self.tscFinanceCenter.GetValues();
                    if (self.tscBudgetArticle)
                        self.tscBudgetArticle.GetValues();
                    if (self.tscInitiator)
                        self.tscInitiator.GetValues();
                    //
                    self.FilterDicDate(new Date());
                    self.Search();
                };
                //
                self.filter_financeCenter_isExpanded = ko.observable(false);
                self.filter_budgetArticle_isExpanded = ko.observable(false);
                self.filter_initiator_isExpanded = ko.observable(false);
                //
                self.AfterRender = function () {
                    var $frm = $('#' + self.frm.GetRegionID());
                    self.table(new module.Table(
                        '#' + self.frm.GetRegionID() + ' .ars-link_tableColumn',
                        self.rowSelected,
                        self.isRowSelected,
                        self.showObjectForm,
                        self.FilterDic
                        ));
                    require(['models/Table/Columns'], function (vm) {
                        var $columnButton = $frm.find('.ars-link_columnsButton');
                        self.tableColumns(new vm.ViewModel(self.table(), $frm.find('.ars-link_tableSearchColumns'), $columnButton, $()))
                        //
                        self.table().Load();
                    });
                    //                
                    self.tscFinanceCenter = new treeLib.controlMulti();
                    self.tscFinanceCenter.init($frm.find('.financeCenterFilter_content'), {
                        SelectedValues: [],
                        TargetClassID: [29, 101, 102],
                        UseAccessIsGranted: true,
                        TreeType: 3,
                        AvailableClassID: [29, 101, 102],
                        FinishClassID: [102],
                        ClassSearcher: 'FinanceCenterSearcher',
                        OperationsID: [],
                        SearcherParams: []
                    });
                    self.tscFinanceCenter.OnSelectionChanged = self.OnFilterChange;
                    //                
                    self.tscBudgetArticle = new treeLib.controlMulti();
                    self.tscBudgetArticle.init($frm.find('.budgetArticleFilter_content'), {
                        SelectedValues: [],
                        TargetClassID: [143],
                        UseAccessIsGranted: true,
                        TreeType: 3,
                        AvailableClassID: [143],
                        FinishClassID: [143],
                        ClassSearcher: 'BudgetSearcher',
                        OperationsID: [],
                        SearcherParams: []
                    });
                    self.tscBudgetArticle.OnSelectionChanged = self.OnFilterChange;
                    //                
                    self.tscInitiator = new treeLib.controlMulti();
                    self.tscInitiator.init($frm.find('.initiatorFilter_content'), {
                        SelectedValues: [],
                        TargetClassID: [9],
                        UseAccessIsGranted: true,
                        TreeType: 0,
                        AvailableClassID: [29, 101, 102, 9],
                        FinishClassID: [9],
                        ClassSearcher: 'WebUserSearcher',
                        OperationsID: [],
                        SearcherParams: []
                    });
                    self.tscInitiator.OnSelectionChanged = self.OnFilterChange;
                };
            },

            Table: function (selectorTable, rowSelectedFunc, isRowSelectedFunc, showFormFunc, ko_filterDic) {
                var self = this;
                self.ajaxControl = new ajaxLib.control();
                //
                self.selectedItemsModel = ko.observable(null);
                self.createColumn = null;//set in columns.js
                self.columnList = ko.observableArray([]);
                self.TableWidth = ko.computed(function () {//для синхронного размера шапки и самой таблицы
                    var retval = 2;
                    //
                    for (var i = 0; i < self.columnList().length; i++) {
                        var column = self.columnList()[i];
                        if (column.Visible() == true)
                            retval += column.Width();
                    }
                    //
                    return retval;
                });
                //
                self.rowList = ko.observableArray([]);
                self.rowHashList = {};//для быстрого поиска строки по идентификатору объекта
                //
                self.viewName = 'frmFinanceBudgetRowSearcher_Table';
                self.isLoading = ko.observable(false);//скрываем текст "Список пуст", если список грузится
                self.isAppendRequestAvailable = ko.observable(false);
                //
                self.ShowForm = function (cell) {
                    self.MarkRow(cell.Row.ID, false, 'modified');
                    self.MarkRow(cell.Row.ID, false, 'highlight');
                    //               
                    showFormFunc(cell.Row.RealObjectID ? cell.Row.RealObjectID.toUpperCase() : cell.Row.ID.toUpperCase());
                };
                self.UnckeckRow = function (id) {
                    id = id.toUpperCase();
                    var row = self.rowHashList[id];
                    if (row)
                        row.Checked(false);
                };
                //
                self.GetOrCreateRow = function (id, classID, addToBeginOfList, realObjectID, operationInfo) {
                    id = id.toUpperCase();
                    self.ClearInfoByObject(id);
                    //
                    var isNew = false;
                    var row = self.rowHashList[id];
                    if (!row) {
                        row = self.rowHashList[id] = new tpLib.createRow(id, classID, realObjectID, operationInfo, self.ShowForm, self.RowSelectedChanged, self.thumbResizeCatch, self.moveTrumbData);
                        if (addToBeginOfList == true)
                            self.rowList().unshift(row);
                        else
                            self.rowList().push(row);
                        isNew = true;
                    }
                    else
                        row.OperationInfo = operationInfo;
                    //
                    if (isRowSelectedFunc(realObjectID ? realObjectID.toUpperCase() : id))
                        row.Checked(true);
                    //
                    return {
                        row: row,
                        isNew: isNew
                    };
                };
                //
                self.IsSelectAllWorking = false;
                self.SelectAllClick = function (item, event) {
                    var checking = $(selectorTable).find('.b-content-table__th-checkbox').is(':checked');
                    self.IsSelectAllWorking = true;
                    //
                    var ids = [];
                    for (var i = 0; i < self.rowList().length; i++) {
                        var row = self.rowList()[i];
                        row.Checked(checking);
                        ids.push(row.RealObjectID ? row.RealObjectID.toUpperCase() : row.ID.toUpperCase());
                    }
                    rowSelectedFunc(ids, checking);
                    //
                    self.IsSelectAllWorking = false;
                    return true;
                };
                //
                self.MarkRow = function (objectID, addMode, cssClass) {
                    var obj = $('#' + objectID);
                    if (obj.length == 0)
                        return false;
                    //
                    if (addMode) {
                        if (!obj.hasClass(cssClass))
                            obj.addClass(cssClass);
                    }
                    else {
                        if (obj.hasClass(cssClass))
                            obj.removeClass(cssClass);
                    }
                    return true;
                };
                self.RemoveRow = function (id) {
                    id = id.toUpperCase();
                    var row = self.rowHashList[id];
                    if (row) {
                        self.rowHashList[id] = null;
                        //
                        var index = self.rowList().indexOf(row);
                        if (index == -1)
                            return;//в кеше строка есть, а по факту нет - не может быть!
                        else {
                            self.rowList().splice(index, 1);
                            self.rowList.valueHasMutated();
                        }
                    }
                };
                self.ClearInfoByObject = function (id) {
                    id = id.toUpperCase();
                    var index = self.ModifiedObjectIDs().indexOf(id);
                    if (index > -1) {
                        self.ModifiedObjectIDs().splice(index, 1);//попали сюда - нет необходимости отображать уведомление
                        self.ModifiedObjectIDs.valueHasMutated();
                    }
                };
                //
                self.RowSelectedChanged = function (row, isAdd) {
                    if (!row || self.IsSelectAllWorking)
                        return;
                    //
                    rowSelectedFunc([row.RealObjectID ? row.RealObjectID.toUpperCase() : row.ID.toUpperCase()], isAdd);
                };
                //
                self.ClearSelection = function () {
                    rowSelectedFunc(null, false);
                    //
                    if (!self.selectedItemsModel())
                        return;
                    self.selectedItemsModel().ClearSelection();
                };
                //
                self.moveTrumbData = ko.observable(null);
                self.moveThumbCancelTime = (new Date()).getTime();
                self.moveTrumbData.subscribe(function (newValue) {
                    if (newValue) {
                        var column = newValue.column;
                        if (self.columnList().indexOf(column) == self.columnList().length - 1) {
                            column.Width(column.Width() + 200);
                        }
                    }
                    else
                        self.moveThumbCancelTime = (new Date()).getTime();
                });
                self.cancelThumbResize = function () {
                    if (self.moveTrumbData() != null) {
                        var column = self.moveTrumbData().column;
                        column.showResizeThumb(false);
                        self.moveTrumbData(null);
                    }
                }
                self.thumbResizeCatch = function (column, e) {
                    if (e.button == 0) {
                        self.moveTrumbData({ column: column, startX: e.screenX, startWidth: column.Width() });
                        self.moveTrumbData().column.showResizeThumb(true);
                    }
                    else
                        self.cancelThumbResize();
                };
                $(document).bind('mousemove', function (e) {
                    if (self.moveTrumbData() != null) {
                        var dx = e.screenX - self.moveTrumbData().startX;
                        self.moveTrumbData().column.Width(Math.max(self.moveTrumbData().startWidth + dx, 50));
                        self.moveTrumbData().column.showResizeThumb(true);
                    }
                });
                $(document).bind('mouseup', function (e) {
                    self.cancelThumbResize();
                });
                //
                self.SyncLoad = null;
                self.Load = function () {
                    self.SyncAppend = null;//cancel append blocking
                    //
                    var returnD = $.Deferred();
                    if (self.SyncLoad) {
                        returnD.resolve();
                        return returnD.promise();
                    }
                    self.SyncLoad = true;
                    //
                    self.ClearSelection();
                    //
                    var count = 30;
                    var requestInfo = {
                        StartRecordIndex: 0,
                        CountRecords: count,
                        IDList: [],
                        ViewName: self.viewName,
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),
                    };
                    requestInfo = $.extend(requestInfo, ko_filterDic());
                    self.ajaxControl.Ajax($(selectorTable),
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/finApi/GetFinanceBudgetRowTable'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                if (newVal.DataList) {
                                    self.columnList.removeAll();
                                    self.rowList.removeAll();
                                    self.rowHashList = {};
                                    self.ModifiedObjectIDs.removeAll();
                                    //
                                    $.each(newVal.DataList, function (index, columnWithData) {
                                        if (columnWithData && columnWithData.ColumnSettings && columnWithData.Data) {
                                            var column = self.createColumn(columnWithData.ColumnSettings);//set in Columns.js
                                            self.columnList().push(column);
                                            //
                                            var data = columnWithData.Data;
                                            for (var i = 0; i < data.length; i++) {
                                                var realObjectID = newVal.ObjectIDList ? newVal.ObjectIDList[i] : null;
                                                var rowInfo = self.GetOrCreateRow(newVal.IDList[i], newVal.ClassIDList[i], false, realObjectID, newVal.OperationInfoList[i]);
                                                rowInfo.row.AddCell(column, data[i].Text, data[i].ImageSource);
                                            }
                                        }
                                    });
                                    //                                                 
                                    self.columnList.valueHasMutated();
                                    self.rowList.valueHasMutated();
                                    self.ModifiedObjectIDs.valueHasMutated();
                                    self.RefreshArrows();
                                    self.isAppendRequestAvailable(self.rowList().length >= count);
                                }
                            }
                            else if (newVal && newVal.Result === 1) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmFinanceBudgetRowSearcher.js Load]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmFinanceBudgetRowSearcher.js Load]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 3) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('AccessError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 7) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('OperationError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 9) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 11) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SqlTimeout'));
                                });
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRowSearcher.js Load]', 'error');
                                });
                            }
                            //
                            self.SyncLoad = null;
                            returnD.resolve();
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRowSearcher.js, Load]', 'error');
                            });
                            //
                            self.SyncLoad = null;
                            returnD.resolve();
                        },
                        null
                    );
                    //
                    return returnD.promise();
                };
                //
                self.SyncAppend = null;
                self.scrollPosition = 0;
                self.OnScroll = function () {
                    self.scrollPosition = 100 * this.scrollTop / (this.scrollHeight - this.clientHeight);
                    var $regionTable = $(selectorTable);
                    $regionTable.find('.tableHeader').css('margin-left', -this.scrollLeft);
                    //
                    var tableHeight = $regionTable.find('.tableScroll .b-content-table__table').height();
                    var viewTableHeight = $regionTable.find('.tableScroll').height();
                    var loadingFooterHeight = $regionTable.find('.tableScroll .loadingFooter').height();
                    var scrollTop = this.scrollTop;
                    //
                    if (scrollTop + viewTableHeight >= tableHeight - loadingFooterHeight && !self.SyncAppend) {
                        self.SyncAppend = true;
                        try {
                            var countBefore = self.rowList().length;
                            $.when(self.AppendOrUpdate()).done(function () {
                                if (self.rowList().length == countBefore)
                                    setTimeout(function () {//докрутили до конца списка, больше данных нет - не дадим ничего подгpужать 10 сек
                                        self.SyncAppend = null;
                                    }, 10000);
                                else
                                    self.SyncAppend = null;
                            });
                        }
                        catch (e) { self.SyncAppend = null; }
                    }
                };
                //
                self.LoadRows = function (objectIDList, scrollToUp) {
                    if (!self.SyncAppend && objectIDList) {
                        self.SyncAppend = true;
                        try {
                            var scrollPositionBefore = self.scrollPosition;
                            $.when(self.AppendOrUpdate(objectIDList)).done(function (newRowsCount) {
                                self.SyncAppend = null;
                                //
                                if (newRowsCount > 0 && scrollToUp == true)//при загрузке появились новые данные (есть доступ к ним), нужно прокрутить до новых данных
                                    setTimeout(function () {
                                        if (self.scrollPosition != scrollPositionBefore)
                                            return;//прокручиваем или прокрутили
                                        //
                                        self.ScrollUp();
                                    }, 1000);
                            });
                        }
                        catch (e) { self.SyncAppend = null; }
                    }
                };
                self.LoadModifiedObjects = function () {//вызывается при клике на количество измененных объектов
                    if (self.SyncAppend == true) //сейчас прокручивается список или подгружаются другие объекты - новые грузить не можем
                        return;
                    //
                    var idList = [];
                    for (var i = 0; i < self.ModifiedObjectIDs().length; i++)
                        idList.push(self.ModifiedObjectIDs()[i]);
                    //
                    if (idList.length > 0)
                        self.LoadRows(idList, true);
                };
                //
                self.AppendOrUpdate = function (objectIDList) {
                    var returnD = $.Deferred();
                    //
                    var curFilterID = null;
                    var withFinishedWf = false;
                    var afterDayModMS = null;
                    var treeParams = null;
                    //
                    var rowsCount = self.rowList().length;
                    var count = 15;
                    var requestInfo = {
                        StartRecordIndex: rowsCount,
                        CountRecords: count,
                        IDList: objectIDList,
                        ViewName: self.viewName,
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),
                    };
                    requestInfo = $.extend(requestInfo, ko_filterDic());
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/finApi/GetFinanceBudgetRowTable'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                if (newVal.DataList) {
                                    var firstTime = true;
                                    var updatedRows = [];
                                    var newRowsCount = 0;
                                    var cloneObjectIDList = objectIDList ? JSON.parse(JSON.stringify(objectIDList)) : [];//копия для того, чтобы узнать, что теперь не возвращается сервером из запрошенных ids
                                    $.each(newVal.DataList, function (index, columnWithData) {
                                        if (columnWithData && columnWithData.ColumnSettings && columnWithData.Data) {
                                            var column = self.columnList()[index];
                                            var data = columnWithData.Data;
                                            //
                                            for (var i = requestInfo.StartRecordIndex, j = 0; i < data.length + requestInfo.StartRecordIndex; i++, j++) {
                                                var id = newVal.IDList[j];
                                                var realObjectID = newVal.ObjectIDList ? newVal.ObjectIDList[j] : null;
                                                var rowInfo = self.GetOrCreateRow(id, newVal.ClassIDList[j], objectIDList ? true : false, realObjectID, newVal.OperationInfoList[j]);//если загружаем по требованию определенные id - добавляем их в начало списка
                                                if (rowInfo.isNew)
                                                    newRowsCount++;
                                                if (!rowInfo.isNew && firstTime) {
                                                    rowInfo.row.Cells.removeAll();
                                                    self.MarkRow(rowInfo.row.ID, false, 'modified');
                                                    updatedRows.push(rowInfo.row);
                                                }
                                                if (firstTime) {
                                                    if (objectIDList) {
                                                        var index = cloneObjectIDList.indexOf(id.toUpperCase());//отметим, чтобы понимать, что загрузили, а что не загрузили
                                                        if (index > -1)
                                                            cloneObjectIDList.splice(index, 1);
                                                    }
                                                }
                                                rowInfo.row.AddCell(column, data[j].Text, data[j].ImageSource);
                                            }
                                            firstTime = false;
                                        }
                                    });
                                    //                                
                                    if (objectIDList && cloneObjectIDList.length > 0) {//остались те, что мы запрашивали, но которые не загрузились
                                        for (var i = 0; i < cloneObjectIDList.length; i++) {
                                            var id = cloneObjectIDList[i];
                                            self.RemoveRow(id);
                                            self.ClearInfoByObject(id);
                                        }
                                    }
                                    //
                                    self.columnList.valueHasMutated();
                                    self.rowList.valueHasMutated();
                                    for (var i = 0; i < updatedRows.length; i++)
                                        updatedRows[i].Cells.valueHasMutated();
                                    self.ModifiedObjectIDs.valueHasMutated();
                                    //
                                    if (objectIDList)
                                        setTimeout(function () {
                                            for (var i = 0; i < objectIDList.length; i++)
                                                self.MarkRow(objectIDList[i], true, 'highlight');
                                        }, 1000);//отрисует knockout (можно подписаться на его событие, но сложнее)
                                    else if (newRowsCount == 0)
                                        self.isAppendRequestAvailable(false);
                                }
                            }
                            else if (newVal && newVal.Result === 1) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmFinanceBudgetRowSearcher.js AppendOrUpdate]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmFinanceBudgetRowSearcher.js AppendOrUpdate]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 3) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('AccessError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 7) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('OperationError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 9) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 11) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SqlTimeout'));
                                });
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRowSearcher.js AppendOrUpdate]', 'error');
                                });
                            }
                            if (newVal && newVal.Result != 0 && !objectIDList)
                                self.isAppendRequestAvailable(false);
                            returnD.resolve(newRowsCount);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudgetRowSearcher.js AppendOrUpdate]', 'error');
                            });
                            //
                            if (!objectIDList)
                                self.isAppendRequestAvailable(false);
                            self.SyncLoad = null;
                            returnD.resolve(0);
                        });
                    //
                    return returnD.promise();
                };
                //
                self.IsObjectAvailableForMeNow = function (objectID) {
                    var returnD = $.Deferred();
                    //
                    var objectIDList = [];
                    objectIDList.push(objectID);
                    //
                    var requestInfo = {
                        StartRecordIndex: 0,
                        CountRecords: 1,
                        IDList: objectIDList,
                        ViewName: self.viewName,
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),
                    };
                    requestInfo = $.extend(requestInfo, ko_filterDic());
                    var ajaxControlAsync = new ajaxLib.control();
                    ajaxControlAsync.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/finApi/GetFinanceBudgetRowTable'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                if (newVal.DataList && newVal.DataList.length > 0) {
                                    returnD.resolve(newVal.DataList[0].Data.length > 0 ? true : false);//data in first column
                                    return;
                                }
                            }
                            returnD.resolve(false);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            returnD.resolve(false);
                        });
                    //
                    return returnD.promise();
                };
                //
                self.Reload = function () {
                    var returnD = $.Deferred();
                    //
                    $.when(self.Load()).done(function () {
                        returnD.resolve();
                        setTimeout(self.ScrollUp, 1000);//TODO? сразу не срабатывает из-за knockout
                    });
                    //
                    return returnD.promise();
                };
                //
                self.resizeTable = function () {
                    self.RenderTableComplete();
                };
                //
                self.ScrollUp = function () {
                    var container = $(selectorTable).find('.tableScroll');
                    if (container.length > 0)
                        container[0].scrollTop = 0;
                };
                //
                self.RenderTableComplete = function () {
                    var $regionTable = $(selectorTable);
                    var tableHeightWithoutHeader = $regionTable.height() - $regionTable.find(".tableHeader").outerHeight();
                    $regionTable.find(".tableScroll").css("height", tableHeightWithoutHeader + "px");//для скрола на таблице (без шапки)
                };
                //
                self.AfterRender = function () {
                    self.resizeTable();
                    $(window).resize(self.resizeTable);
                    //
                    $(selectorTable).find('.tableScroll').bind('scroll', self.OnScroll);
                };
                //
                self.ModifiedObjectIDs = ko.observableArray([]);//количество измененных, но не отображенных элементов, которые могут быть интересны пользователю
                self.ModifiedObjectCount = ko.computed(function () {
                    return self.ModifiedObjectIDs().length;
                });
                self.ModifiedObjectCountString = ko.computed(function () {
                    var retval = self.ModifiedObjectIDs().length;
                    return retval > 9 ? '9+' : retval.toString();
                });
                self.IsObjectClassVisible = function (objectClassID) {
                    return (objectClassID == 180);//OBJ_FinanceBudgetRow
                };
                self.IsModifiedObjectIDsContains = function (objectID) {
                    for (var i = 0; i < self.ModifiedObjectIDs().length; i++)
                        if (self.ModifiedObjectIDs()[i] == objectID)
                            return true;
                    //
                    return false;
                };
                self.AppendToModifiedObjectIDs = function (objectID) {
                    if (self.IsModifiedObjectIDsContains(objectID))
                        return;//уже уведомили
                    //
                    self.ModifiedObjectIDs().push(objectID);
                    self.ModifiedObjectIDs.valueHasMutated();
                };
                //
                self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                    if (!self.IsObjectClassVisible(objectClassID) || self.SyncLoad)
                        return;
                };
                self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                    if (!self.IsObjectClassVisible(objectClassID) || self.SyncLoad)
                        return;
                };
                self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                    if (!self.IsObjectClassVisible(objectClassID) || self.SyncLoad)
                        return;//в текущем списке удаляемый объект присутствовать не может
                    else if (parentObjectID && objectClassID != 160) {
                        self.onObjectModified(e, objectClassID, parentObjectID, null);//возможно изменилась часть объекта, т.к. в контексте указан родительский объект
                        return;
                    }
                    objectID = objectID.toUpperCase();
                    //
                    self.RemoveRow(objectID);
                    self.ClearInfoByObject(objectID);
                };
                //
                //отписываться не будем
                $(document).bind('objectInserted', self.onObjectInserted);
                $(document).bind('local_objectInserted', self.onObjectInserted);
                $(document).bind('objectUpdated', self.onObjectModified);
                $(document).bind('local_objectUpdated', self.onObjectModified);
                $(document).bind('objectDeleted', self.onObjectDeleted);
                $(document).bind('local_objectDeleted', self.onObjectDeleted);
            },

            FinanceBudgetRow: function (obj, rowSelectedFunc, isRowSelectedFunc, showFormFunc) {
                var self = this;
                var parentSelf = parent;
                //
                self.Selected = ko.observable(isRowSelectedFunc(obj.ID));
                self.Selected.subscribe(function (newValue) {
                    rowSelectedFunc([self.ID], newValue);
                });
                //
                self.ID = obj.ID;
                self.Identifier = obj.Identifier;
                self.Name = obj.Name;
                self.InitiatorFullName = obj.InitiatorFullName;
                self.BudgetFullName = obj.BudgetFullName;
                self.FinanceCenterFullName = obj.FinanceCenterFullName;
                self.AvailableSum = specLib.Normalize(obj.AvailableSum);
                self.Sum = ko.observable(self.AvailableSum);
                self.Sum.subscribe(function (newValue) {
                    if (newValue > self.AvailableSum)
                        self.Sum(self.AvailableSum);
                });
                //
                self.SumClick = function (obj, e) {
                    e.stopPropagation();
                };
                //
                self.ShowForm = function () {
                    showFormFunc(self.ID);
                };
                //
                self.OnRender = function (htmlNodes, thisObj) {
                    var node = ko.utils.arrayFirst(htmlNodes, function (html) {
                        return html.tagName == 'INPUT';
                    });
                    if (!node)
                        return;
                    //
                    var $input = $(node);
                    $input.stepper({
                        type: 'float',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [1, self.AvailableSum],
                        onStep: function (val, up) {
                            self.Sum(val);
                        }
                    });
                };
            },

            ShowDialog: function (purchaseSpecificationOrWorkOrderID, onSelectFunc, isSpinnerActive) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var vm = new module.ViewModel(purchaseSpecificationOrWorkOrderID);
                var bindElement = null;
                //
                var cancelButton = {
                    'text': getTextResource('Close'),
                    'class': 'GrayUIButton',
                    'click': function () { frm.Close(); }
                };
                var addButton = {
                    'text': getTextResource('Add'),
                    'click': function () {
                        onSelectFunc(vm.SelectedList());
                        frm.Close();
                    }
                };
                vm.SelectedList.subscribe(function (newValue) {
                    var buttons = [];
                    if (newValue.length > 0)
                        buttons.push(addButton);
                    buttons.push(cancelButton);
                    //
                    frm.UpdateButtons(buttons);
                });
                var buttons = [];
                buttons.push(cancelButton);
                //
                frm = new formControl.control(
                        'region_frmFinanceBudgetRowSearcher',//form region prefix
                        'setting_frmFinanceBudgetRowSearcher',//location and size setting
                        getTextResource('frmFinanceBudgetRowSearcherCaption'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        600, 400,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Finances/frmFinanceBudgetRowSearcher\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                frm.ExtendSize(800, 500);//normal size
                vm.frm = frm;
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show()).done(function (frmD) {
                    hideSpinner();
                });
            }
        }
        return module;
    });