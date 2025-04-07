define(['knockout', 'jquery', './ko.ListView.Cells', './ko.ListView.Columns', './ko.ListView.Rows', 'comboBox'], function (ko, $, m_cells, m_columns, m_rows) {
    (function () {
        var settings = {
            maxCount: 100000000,                                        //max items to load
            //
            template: '../UI/Controls/ListView/ko.ListView',            //template of control
            loadingClass: '_loading',                                   //css class when data in control is loading (first time or not virtual mode)
            retrievingClass: '_retrieving',                             //css class when data in control is retrieving (virtual mode)
            gridLinesClass: '_gridLines',                               //css class when table have separation lines
            emptyDataClass: '_emptyData',                               //css class when table don't have data
            multiColorClass: '_multiColor',                             //css class when table row have background-color by index
            rowColorEvenClass: '_rowColorEven',                         //css class when row have background-color by index
            rowColorOddClass: '_rowColorOdd',                           //css class when row have background-color by index
            compactModeClass: '_compact',                               //css class when table have small paddings
            rowOutdatedClass: 'row_outdated',                           //css class when row is outdated
            rowNewerClass: 'row_newer',                                 //css class when row is downloaded
            rowFocusedClass: 'row_focused',                             //css class when row is focused
            rowCheckedClass: 'row_checked',                             //css class when row is checked
            rowClicableClass: 'row_clicable',                           //css class when row is clicable
            //
            notLoadMessage: ko.observable('waiting for initialization...'),//message when control not loaded
            loadingMessage: ko.observable(getTextResource('ListIsLoading')),//message when data is loading
            emptyMessage: ko.observable(getTextResource('ListIsEmpty')),//message when data is empty
            multiSelect: ko.observable(true),                           //can select more than one item
            gridLines: ko.observable(true),                             //separation lines in table
            multiColor: ko.observable(true),                            //colored rows in table
            compactMode: ko.observable(false),                           //small paddings
            settingsName: ko.observable(''),                            //user settings of table (columnWidths, columnOrders, columnTypes, columnVisibles,...)
            virtualMode: ko.observable(true),                           //default mode of work (raise event retrieveItems to get all items and sort then or raise event retrieveVirtualItems to get sorted next items)
            contextMenu: ko.observable(null),                           //default contextMenu
            autoDispose: ko.observable(true),                           //when dom element removed - control disposed
            maxVisibleRowCount: ko.observable(null),                    //max visible row count
            //
            controlInitialized: ko.observable(                          //raise after control initialized, to get control viewModel
                function (listView) {
                }),
            controlRendered: ko.observable(                             //raise after control rendered
                function (listView, elements) {                         // "elements" is an array of DOM nodes just rendered by the template
                }),
            retrieveVirtualItems: ko.observable(                        //func to loading next virtual items
                function (startRecordIndex, countOfRecords) {
                    var d = $.Deferred();
                    d.resolve([]);//object array with fields which declared in settings
                    return d.promise();
                }),
            retrieveItems: ko.observable(                               //func to loading all items (not virtual mode)
                function () {
                    var d = $.Deferred();
                    d.resolve([]);//object array with fields which declared in settings
                    return d.promise();
                }),
            drawCell: ko.observable(                                    //func to drawing table cell
                function (obj, column, cell) {
                    cell.text = m_cells.textRepresenter(obj, column);
                    cell.imageSource = null;
                }),
            getObjectID: ko.observable(                                 //func to get row identificator
                function (obj) {
                    return obj.ID.toUpperCase();
                }),
            itemClick: ko.observable(                                   //func to open row form
                function (obj) {
                    alert('to disable rowClick set itemClick event to null');
                })
        };


        /*
            element - The DOM element involved in this binding
            valueAccessor - A JavaScript function that you can call to get the current model property that is involved in this binding. Call valueAccessor() to get the current model property value.
            allBindings -  A JavaScript object that you can use to access all the model values bound to this DOM element. Call allBindings.get('name') to retrieve the value of the name binding (returns undefined if the binding doesn’t exist);
            viewModel - This parameter is deprecated in Knockout 3.x. Use bindingContext.$data or bindingContext.$rawData to access the view model instead.
            bindingContext - An object that holds the binding context available to this element’s bindings. This object includes special properties including $parent, $parents, and $root that can be used to access data that is bound against ancestors of this context.
        */
        ko.bindingHandlers.koListView = {
            init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                if (ko.controls && element.id != '' && ko.controls[element.id]) {
                    //repeated initialization into the same dom element
                    var vmExists = ko.controls[element.id];
                    vmExists.prepareToRender();
                    //
                    ko.applyBindingsToNode(element,
                        { template: { name: vmExists.options.template, afterRender: vmExists.afterRender } },
                        vmExists);
                    return { controlsDescendantBindings: true };
                }
                //
                if (element.id == '')
                    element.id = 'koListView_' + ko.getNewID();
                //
                var options = ko.utils.unwrapObservable(valueAccessor());
                for (var key in settings)
                    if (options[key] === undefined)
                        options[key] = ko.cloneVariable(settings[key]);
                    else if (ko.isObservable(settings[key]) && !ko.isObservable(options[key])) //make ko-value
                        options[key] = ko.observable(options[key]);
                //
                var vm = new ko.bindingHandlers.koListView.ViewModel(options, element);
                var vm_dispose = vm.dispose;
                vm.dispose = function() {
                    vm_dispose();
                    ko.controls[element.id] = undefined;
                };
                //
                if (!ko.controls)
                    ko.controls = {};
                if (ko.controls[element.id] && ko.controls[element.id].dispose)
                    ko.controls[element.id].dispose();
                ko.controls[element.id] = vm;
                ko.utils.domNodeDisposal.addDisposeCallback(element,
                    function() {
                        if (options.autoDispose() === true)
                            vm.dispose();
                    });
                //
                ko.applyBindingsToNode(element,
                    { template: { name: options.template, afterRender: vm.afterRender } },
                    vm);
                //tell KO *not* to bind the descendants itself, otherwise they will be bound twice                
                return { controlsDescendantBindings: true };
            }
        };

        ko.bindingHandlers.koListView.ViewModel = function (options, element) {
            var self = this;
            self.options = options;
            //
            {//view models
                self.columnViewModel = new m_columns.columnViewModel(self);
                self.virtualColumnMemberNamePrefix = 'parameter_';
                self.cellCreator = m_cells.cellCreator;
                self.rowViewModel = new m_rows.rowViewModel(self);
            }
            //
            self.dispose = function () {
                self.columnViewModel.dispose();
                self.rowViewModel.dispose();
                //
                self.settingsName_handler.dispose();
                self.virtualMode_handler.dispose();
                self.scrollTop_handle.dispose();
                self.css.dispose();
                self.message.dispose();
                self.rowListCount_handle.dispose();
                clearTimeout(self.timeoutCalculate);
                //
                if (self.onScrollBinded == true) {
                    var $regionTable = $('#' + element.id);
                    $regionTable.find('.tableData').unbind('scroll', self.onScroll);
                    self.onScrollBinded = false;
                }
                if (self.renderTableBinded_window == true) {
                    $(window).unbind('resize', self.waitAndRenderTable);
                    self.renderTableBinded_window = false;
                }
                if (self.renderTableBinded_form == true) {
                    $(document).unbind('form_sizeChanged', self.waitAndRenderTable);
                    self.renderTableBinded_form = false;
                }
            };
            //
            {//state and css
                self.columnsLoaded = false;
                self.allDataLoaded = ko.observable(false);
                self.loading = ko.observable(false);
                self.retrieving = ko.observable(false);
                //
                self.css = ko.pureComputed(function () {
                    var retval = '';
                    if (self.loading() === true) retval += ' ' + self.options.loadingClass;
                    if (self.retrieving() === true) retval += ' ' + self.options.retrievingClass;
                    if (self.options.gridLines() == true) retval += ' ' + self.options.gridLinesClass;
                    if (self.options.compactMode() === true) retval += ' ' + self.options.compactModeClass;
                    if (self.options.multiColor() === true) retval += ' ' + self.options.multiColorClass;
                    if (self.rowViewModel.rowList().length === 0) retval += ' ' + self.options.emptyDataClass;
                    return retval;
                });
                self.message = ko.pureComputed(function () {
                    if (self.rowViewModel.rowList().length > 0)
                        return '';
                    //
                    if (self.columnViewModel.columnList().length == 0) {
                        return self.options.notLoadMessage();
                    }
                    if (self.loading() === true)
                        return self.options.loadingMessage();
                    else if (self.rowViewModel.rowList().length === 0)
                        return self.options.emptyMessage();
                    else
                        return '';
                });
            }
            //
            {//control panel (buttons panel)
                {//export button/menu
                    self.exportDataClick = function (m, e) {
                        if (self.loading() === true)
                            return;
                        //open menu
                        var $control = $('#' + element.id);
                        var $panel = $control.find('.exportPanel')
                        openRegion($panel, e);
                        {//set panel position
                            var controlOffset = $control.offset();
                            var $button = $(e.target);
                            var buttonOffset = $button.offset();
                            var buttonWidth = $button.width();
                            var buttonHeight = $button.height();
                            var top = $control.length > 0 && $button.length > 0 && $.contains($control[0], $button[0]) ? 2 * (buttonOffset.top - controlOffset.top) + buttonHeight : 0;
                            $panel.css('left', buttonOffset.left - controlOffset.left + 'px').css('top', top + 'px');
                            $panel.find('.nub').css('left', buttonWidth / 2 + 'px');
                        }
                    };
                    //
                    //algorithm of csv generation                        
                    self.exportObjectListToCSVAsync = function (objectList, afterCompleteFunc) {
                        if (self.columnsLoaded === false)
                            return;
                        //  
                        var worker = new Worker('UI/Controls/ListView/ko.ListView.ExportCSVWorker.js?uniqueVector=im_' + applicationVersion);
                        worker.onmessage = function (e) {
                            var blob = new Blob(['\ufeff', e.data], { type: 'data:text/csv;charset=utf-8;' });
                            //
                            if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                                window.navigator.msSaveOrOpenBlob(blob, 'export.csv');//IE
                            } else {//normal browser
                                var a = document.createElement("a");
                                a.style.visibility = 'hidden';
                                document.body.appendChild(a);//Firefox turdie
                                a.href = URL.createObjectURL(blob);
                                a.download = 'export.csv';
                                a.click();
                                document.body.removeChild(a);
                            }
                            //
                            afterCompleteFunc();
                        };
                        //
                        var columns = self.columnViewModel.sortedVisibleColumnList();
                        {//prepare columns (without func's)
                            var simpleColumnList = [];
                            columns.forEach(function (column) { simpleColumnList.push({ MemberName: column.MemberName, Text: column.Text() }); });
                        }
                        worker.postMessage(
                            {
                                objectList: objectList,
                                columnList: simpleColumnList,
                                //
                                locale: locale,
                                resourceArrayFromServer: resourceArrayFromServer,
                                applicationVersion: applicationVersion
                            });//run worker
                    };
                    //
                    self.exportLoadedData = function (m, e) {
                        var $control = $('#' + element.id);
                        $control.find('.exportPanel').hide();
                        //
                        var el = $control.find('.ko_ListView .exportButton')[0];
                        showSpinner(el);
                        var objectList = [];
                        self.rowViewModel.sortedRowList().forEach(function (row) {
                            objectList.push(row.object);
                        });
                        self.exportObjectListToCSVAsync(objectList, function () {
                            hideSpinner(el);
                        });
                    };
                    self.exportAllData = function (m, e) {
                        var $control = $('#' + element.id);
                        $control.find('.exportPanel').hide();
                        //
                        var el = $control.find('.ko_ListView')[0];
                        if (self.options.virtualMode() == false) {
                            var retrieveItemsFunc = self.options.retrieveItems();
                            if (retrieveItemsFunc) {
                                showSpinner(el);
                                var objectListD = retrieveItemsFunc();//get all object, not ordered
                                $.when(objectListD).done(function (objectList) {
                                    hideSpinner(el);
                                    //
                                    el = $control.find('.ko_ListView .exportButton')[0];
                                    showSpinner(el);
                                    self.exportObjectListToCSVAsync(objectList, function () {
                                        hideSpinner(el);
                                    });
                                });
                            }
                        }
                        else {
                            var retrieveVirtualItemsFunc = self.options.retrieveVirtualItems();
                            if (retrieveVirtualItemsFunc) {
                                showSpinner(el);
                                var objectListD = retrieveVirtualItemsFunc(0, self.options.maxCount);//get max available part of list, ordered
                                $.when(objectListD).done(function (objectList) {
                                    hideSpinner(el);
                                    //
                                    el = $control.find('.ko_ListView .exportButton')[0];
                                    showSpinner(el);
                                    self.exportObjectListToCSVAsync(objectList, function () {
                                        hideSpinner(el);
                                    });
                                });
                            }
                        }
                    };
                }
                //
                {//columns button
                    self.settingsClick = function (m, e) {
                        if (self.columnsLoaded == false)
                            return;
                        //
                        var $control = $('#' + element.id);
                        var $panel = $control.find('.columnsPanel')
                        openRegion($panel, e);
                        {//correct arrow
                            var controlOffset = $control.offset();
                            var $button = $(e.target);
                            var buttonOffset = $button.offset();
                            var buttonWidth = $button.width();
                            var buttonHeight = $button.height();
                            var panelOffset = $panel.offset();
                            var top = $control.length > 0 && $button.length > 0 && $.contains($control[0], $button[0]) ? 2 * (buttonOffset.top - controlOffset.top) + buttonHeight : 0;
                            $panel.find('.nub').css('left', (buttonOffset.left - panelOffset.left + buttonWidth / 2) + 'px');
                            $panel.css('top', top + 'px');
                        }
                    };
                    self.settingsCloseClick = function (m, e) {
                        var $control = $('#' + element.id);
                        $control.find('.columnsPanel').hide();
                    };
                }
                //
                {//counter
                    self.createCountItem = function (count, name) {
                        var self = this;
                        //
                        self.Count = count;
                        self.Name = name;
                        //
                        return self;
                    };
                    self.countItem_current = new self.createCountItem(0, self.rowViewModel.rowListCount);
                    self.counterValue = ko.observable(self.countItem_current);
                    self.rowListCount_handle = self.rowViewModel.rowListCount.subscribe(function (newValue) {
                        self.counterValue(self.countItem_current);
                    });
                    self.counterValue_handle = self.counterValue.subscribe(function (newValue) {
                        if (self.countItem_current != newValue) {
                            var el = $('#' + element.id).find('.ko_ListView')[0];
                            showSpinner(el);
                            $.when(self.append(newValue.Count)).done(function () {
                                hideSpinner(el);
                            });
                            self.counterValue(self.countItem_current);
                        }
                    });
                    //
                    self.countItem_100 = new self.createCountItem(100, '+100');
                    self.countItem_200 = new self.createCountItem(200, '+200');
                    self.countItem_300 = new self.createCountItem(300, '+300');
                    self.countItem_500 = new self.createCountItem(500, '+500');
                    self.countItem_all = new self.createCountItem(self.options.maxCount, getTextResource('ListView_DownloadAllItemsText'));
                    //
                    self.getCounterItems = function (options) {
                        var data = [];
                        data.push(self.countItem_current);
                        if (self.options.virtualMode() === true && self.allDataLoaded() === false) {
                            data.push(self.countItem_100);
                            data.push(self.countItem_200);
                            data.push(self.countItem_300);
                            data.push(self.countItem_500);
                            data.push(self.countItem_all);
                        }
                        //
                        options.callback({
                            data: data, total: data.length
                        });
                    };
                }
            }
            //
            {//loading data
                {//items height - to load, to append
                    self.getItemHeight = function () {
                        var $control = $('#' + element.id);
                        var headerHeight = $control.find('.tableHeader').find('.table tr').height();
                        //
                        var $tableData = $control.find('.tableData');
                        var rowHeight = $tableData.find('.table tr').height();
                        if (rowHeight === null)//data not loaded
                            rowHeight = headerHeight;
                        //
                        return rowHeight + 1;
                    };
                    self.getItemsCountByHeight = function () {
                        var $control = $('#' + element.id);
                        var headerHeight = $control.find('.tableHeader').find('.table tr').height();
                        //
                        var $tableData = $control.find('.tableData');
                        var tableHeight = $tableData.height();
                        var rowHeight = $tableData.find('.table tr').height();
                        if (rowHeight === null)//data not loaded
                            rowHeight = headerHeight;
                        //
                        if (tableHeight > 0 && rowHeight > 0)
                            return parseInt(tableHeight / rowHeight) + 2;//unvisible items (cause scroll and partial draw)
                        else
                            return null;
                    };
                    self.getItemCountToLoad = function () {
                        //количество первоначальных элементов - все те, что видны на экране
                        var count = self.getItemsCountByHeight();
                        return count == null ? null : count * 2;//without NaN
                    };
                    self.getItemsCountToAppend = function () {
                        //количество подгружаемых элементов - еще один экран
                        var count = self.getItemsCountByHeight();
                        return count == null ? null : count * 2;//without NaN
                    };
                }
                //
                {//virtual items - very fast drawing a lot of rows
                    self.unvisibleRowsBeforeHeight = ko.observable(0);
                    self.unvisibleRowsAfterHeight = ko.observable(0);
                    //
                    self.scrollTop = ko.observable(0);
                    self.scrollTop_handle = self.scrollTop.subscribe(function (newValue) {
                        clearTimeout(self.timeoutCalculate);
                        self.timeoutCalculate = setTimeout(self.calculateVisibleRows, 10);//wait when scrolling
                    });
                    //
                    self.visibleRowList = ko.observableArray([]);
                    self.timeoutCalculate = null;
                    self.calculateVisibleRows = function () {
                        if (self.ignoreTableHeight) {
                            return;
                        }

                        clearTimeout(self.timeoutCalculate);
                        self.timeoutCalculate = null;
                        //
                        var itemHeight = self.getItemHeight();
                        if (itemHeight === null || itemHeight === 0) {
                            self.timeoutCalculate = setTimeout(self.calculateVisibleRows, 100);//wait when rendering
                            return;
                        }
                        //
                        var itemsByHeight = self.getItemsCountByHeight();
                        var rowList = self.rowViewModel.sortedRowList();//in this step cells property in rows doesn't calculate!
                        var totalHeight = rowList.length * itemHeight;
                        var scrollTop = Math.min(Math.max(0, totalHeight - itemsByHeight), self.scrollTop());
                        var scrolledItems = parseInt(scrollTop / itemHeight);
                        //
                        self.unvisibleRowsBeforeHeight(scrolledItems * itemHeight);
                        var startIndex = scrolledItems;
                        var count = Math.min(itemsByHeight, rowList.length - scrolledItems);
                        self.unvisibleRowsAfterHeight(totalHeight - self.unvisibleRowsBeforeHeight() - itemHeight * count);
                        //
                        var list = [];
                        for (var i = startIndex; i < startIndex + count; i++)
                            list.push(rowList[i]);
                        self.visibleRowList(list);
                        //
                        //our calculation for value self.scrollTop()
                        if (scrollTop > 0 && self.allDataLoaded() == false && count < itemsByHeight) {
                            var loadingFooterHeight = $('#' + element.id).find('.tableData').find('.loadingFooter').height();
                            self.setScrollTop(self.scrollTop() - loadingFooterHeight);
                        }
                        else
                            self.setScrollTop(self.scrollTop());
                    };
                    self.showAllRows = function () {
                        var itemHeight = self.getItemHeight();
                        if (itemHeight === null || itemHeight === 0) {
                            self.timeoutCalculate = setTimeout(self.showAllRows, 100);//wait when rendering
                            return;
                        }

                        var rowList = self.rowViewModel.sortedRowList()
                        var list = [];
                        for (var i = 0; i < rowList.length; i++)
                            list.push(rowList[i]);
                        self.visibleRowList(list);
                    };
                    //
                    self.showAllRows = function () {
                        var itemHeight = self.getItemHeight();
                        if (itemHeight === null || itemHeight === 0) {
                            self.timeoutCalculate = setTimeout(self.showAllRows, 100);//wait when rendering
                            return;
                        }

                        var rowList = self.rowViewModel.sortedRowList()
                        var list = [];
                        for (var i = 0; i < rowList.length; i++)
                            list.push(rowList[i]);
                        self.visibleRowList(list);
                    };
                    //
                    self.setScrollTop = function (val) {
                        var container = $('#' + element.id).find('.tableData');
                        if (container.length > 0)
                            container[0].scrollTop = val;//after that will raise event scroll, will raise handler scrollTop changed, will raised calculateVisibleRows
                    };
                    self.getScrollMaxPosition = function () {
                        var loadingFooterHeight = $('#' + element.id).find('.tableData').find('.loadingFooter').height();
                        return self.unvisibleRowsAfterHeight() - (self.allDataLoaded() === false ? loadingFooterHeight : 0);
                    };
                    self.scrollUp = function () {
                        self.setScrollTop(0);
                    };
                }
                //
                self._loadD = null;
                self.load = function () {
                    var retvalD = $.Deferred();
                    self._loadD = retvalD;
                    //
                    self.retrieving(false);
                    self._appendD = null;
                    self.loading(true);
                    self.allDataLoaded(false);
                    self.rowViewModel.clear();
                    self.scrollUp();
                    self.renderTable();
                    //
                    var dWaitColumns = $.Deferred();
                    if (self.columnsLoaded == false) {
                        var columnsD = self.columnViewModel.loadColumns();
                        $.when(columnsD).done(function (val) {
                            self.columnsLoaded = true;
                            dWaitColumns.resolve(val);
                        });
                    }
                    else {
                        self.columnViewModel.virtualColumns([]);
                        dWaitColumns.resolve();
                    }
                    //
                    $.when(dWaitColumns).done(function (val) {
                        if (self.options.virtualMode() == false) {
                            var retrieveItemsFunc = self.options.retrieveItems();
                            if (retrieveItemsFunc) {
                                var objectListD = retrieveItemsFunc();//get all object, not ordered
                                $.when(objectListD).done(function (objectList) {
                                    if (self._loadD === retvalD) {
                                        if (objectList != null) {
                                            self.rowViewModel.load(objectList);
                                            self.allDataLoaded(true);
                                        }
                                        //
                                        self.loading(false);
                                        self._loadD = null;
                                        self.retrieving(false);
                                        self._appendD = null;
                                        self.renderTable();
                                    }
                                    retvalD.resolve();
                                });
                            }
                            else {
                                if (self._loadD === retvalD) {
                                    self.loading(false);
                                    self._loadD = null;
                                    self.retrieving(false);
                                    self._appendD = null;
                                }
                                retvalD.resolve();
                            }
                        }
                        else {
                            var retrieveVirtualItemsFunc = self.options.retrieveVirtualItems();
                            if (retrieveVirtualItemsFunc) {
                                var count = self.getItemCountToLoad();
                                if (count == null) {
                                    setTimeout(function () {//not rendered
                                        if (self._loadD === retvalD)
                                            $.when(self.load()).done(function () {
                                                retvalD.resolve();
                                            });
                                        else
                                            retvalD.resolve();
                                    }, 100);
                                    return;
                                }
                                let objectListD = retrieveVirtualItemsFunc(0, count);//get part of list, ordered
                                $.when(objectListD).done(function (objectList) {
                                    if (self._loadD === retvalD) {
                                        if (objectList != null) {
                                            self.rowViewModel.load(objectList);
                                            if (count > objectList.length || count > 0 && objectList.length === 0 || count > 0 && objectList.length > count)//вернулось меньше или больше нет или сервер вернул все
                                                self.allDataLoaded(true);
                                        }
                                        //
                                        self.loading(false);
                                        self._loadD = null;
                                        self.retrieving(false);
                                        self._appendD = null;
                                        self.renderTable();
                                    }
                                    retvalD.resolve();
                                });
                            }
                            else {
                                if (self._loadD === retvalD) {
                                    self.loading(false);
                                    self._loadD = null;
                                    self.retrieving(false);
                                    self._appendD = null;
                                }
                                retvalD.resolve();
                            }
                        }
                    });
                    return retvalD.promise();
                };
                self._appendD = null;
                self.append = function (countToLoad) {
                    var retvalD = $.Deferred();
                    self._appendD = retvalD;
                    //                    
                    if (self.options.virtualMode() === true && self.allDataLoaded() === false) {
                        self.retrieving(true);
                        //
                        var retrieveVirtualItemsFunc = self.options.retrieveVirtualItems();
                        if (retrieveVirtualItemsFunc) {
                            var startIndex = self.rowViewModel.rowList().length;
                            var nextCount = countToLoad ? countToLoad : self.getItemsCountToAppend();
                            if (nextCount == null) {
                                self.retrieving(false);
                                self._appendD = null;
                                retvalD.resolve();
                                return;
                            }
                            let objectListD = retrieveVirtualItemsFunc(startIndex, nextCount);//get part of list, ordered
                            $.when(objectListD).done(function (objectList) {
                                if (self._appendD === retvalD) {
                                    if (objectList != null) {
                                        self.rowViewModel.append(objectList);
                                        if (nextCount > objectList.length || nextCount > 0 && objectList.length === 0 || nextCount > 0 && objectList.length > nextCount)//вернулось меньше или больше нет или сервер вернул все
                                            self.allDataLoaded(true);
                                    }
                                    //
                                    self.retrieving(false);
                                    self._appendD = null;
                                    self.renderTable();
                                }
                                retvalD.resolve();
                            });
                        }
                        else {
                            self.retrieving(false);
                            self._appendD = null;
                            self.renderTable();
                            retvalD.resolve();
                        }
                    }
                    else {
                        self._appendD = null;
                        retvalD.resolve();
                    }
                    return retvalD.promise();
                };
                //
                {//reload events
                    self.settingsName_handler = self.options.settingsName.subscribe(function (newValue) {
                        self.columnsLoaded = false;
                        self.load().then(() => self.columnViewModel.mountEntityInWrapper());
                    });
                    self.virtualMode_handler = self.options.virtualMode.subscribe(function (newValue) {
                        self.load();
                    });
                }
            }
            //
            //appending data at scroll
            {
                self.scrollPercent = 0;//for events
                self.onScrollBinded = false;
                //
                self.onScroll = function (e) {
                    var scrollTop = this.scrollTop;
                    self.scrollTop(scrollTop);
                    //
                    self.scrollPercent = 100 * scrollTop / (this.scrollHeight - this.clientHeight);
                    var $regionTable = $('#' + element.id);
                    $regionTable.find('.tableHeader').css('margin-left', -this.scrollLeft);
                    //
                    if (self.allDataLoaded() === true || self.retrieving() === true || self.loading() === true)
                        return;
                    //
                    var loadingFooterHeight = $regionTable.find('.tableData .loadingFooter').height();
                    if (self.unvisibleRowsAfterHeight() <= loadingFooterHeight)
                        self.append();
                };
            }
            //
            {//size and rendering
                self.renderTableBinded_window = false;
                self.renderTableBinded_form = false;
                self.ignoreTableHeight = false;
                //
                self.renderTable = function () {
                    if (self.ignoreTableHeight) {
                        self.showAllRows();
                    }

                    var $regionTable = $('#' + element.id);
                    var controlPanelHeight = $regionTable.find(".tableControlPanel").outerHeight();
                    var headerHeight = $regionTable.find(".tableHeader").outerHeight();
                    var messageHeight = (self.message().length > 0) ? $regionTable.find(".messageContainer").outerHeight(true) : 0;
                    var dataHeight = $regionTable.height() - 2 - controlPanelHeight - headerHeight - messageHeight;//1px per border     
                    //

                    var maxVisibleRowCount = options.maxVisibleRowCount();
                    if (maxVisibleRowCount !== null && maxVisibleRowCount !== 0) {
                        var rowHeight = self.getItemHeight();
                        if (rowHeight === null || rowHeight === 0) {
                            self.waitAndRenderTable();
                            return;
                        }

                        var rowCount = Math.min(options.maxVisibleRowCount(), self.rowViewModel.rowListCount());
                        $regionTable.find(".tableData").css("height", rowHeight * rowCount + "px");

                        if (rowCount > 0 && self.visibleRowList().length < rowCount)
                        {
                            self.waitAndRenderTable();
                        }
                    } else {
                        if (dataHeight <= 0 || headerHeight <= 0) {
                            self.waitAndRenderTable();
                            return;
                        }

                        $regionTable.find(".tableData").css("height", dataHeight + "px");
                    }

                    self.calculateVisibleRows();
                };
                self.waitAndRenderTable_timeout = null;
                self.waitAndRenderTable = function () {
                    clearTimeout(self.waitAndRenderTable_timeout);
                    self.waitAndRenderTable_timeout = null;
                    self.waitAndRenderTable_timeout = setTimeout(self.renderTable, 100);
                };
                //
                self.afterRender = function (elements) {
                    var $regionTable = $('#' + element.id);
                    $regionTable.focus();
                    //
                    if (self.onScrollBinded == false) {
                        $regionTable.find('.tableData').bind('scroll', self.onScroll);
                        self.onScrollBinded = true;
                    }
                    //
                    if (self.renderTableBinded_window == false) {
                        $(window).bind('resize', self.waitAndRenderTable);
                        self.renderTableBinded_window = true;
                    }
                    //
                    if (self.renderTableBinded_form == false) {
                        $(document).bind('form_sizeChanged', self.waitAndRenderTable);
                        self.renderTableBinded_form = true;
                    }
                    //                    
                    self.renderTable();
                    self.options.controlRendered()(self, elements);
                };
                self.prepareToRender = function () {
                    self.onScrollBinded = false;
                };
            }
            //
            //return link to control
            self.options.controlInitialized()(self);
            //
            //default settins
            $.when(userD).done(function (user) {
                self.options.gridLines(user.ListView_GridLines);
                self.options.multiColor(user.ListView_Multicolor);
                self.options.compactMode(user.ListView_CompactMode);
            });
        };
    }());
});