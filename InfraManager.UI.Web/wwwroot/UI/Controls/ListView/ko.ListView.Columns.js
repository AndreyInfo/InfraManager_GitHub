define(['knockout', 'jquery', 'ttControl', 'ajax', 'showEntityService'], function (ko, $, tclib, ajaxLib, ShowEntityService) {
    var module = {
        migratedViews: [
            'ProblemForTable',
            'CallForTable', 'RFCForTable',
            'WorkOrderForTable',
            'NegotiationForTable',
            'CommonForTable',
            'ClientCallForTable',
            'CustomControlForTable',
            'MyDeputy',
            'IDeputyFor',
            'KBAAdmittedPersons',
            'AllMassIncidentsList',
            'Hardware',
            'ClientsHardware',
            'SoftwareInstallation',
            'Inventories',
            'AssetSearch',
            'ELPList',
            'ELPTaskVendorList',
            'ExecutorSelectorEmployeeList',
            'ExecutorSelectorQueueList',
            'SoftwareLicenseSchemeForTable',
            'MassIncidentsToAssociate',
            'ProblemMassIncidents',
            'MassIncidentReferencedCalls',
            'MassIncidentReferencedWorkOrders',
            'MassIncidentReferencedProblems',
            'MassIncidentReferencedChangeRequests',
            'AvailableNotReferencedCalls',
            'AvailableNotReferencedWorkOrders',
            'AvailableNotReferencedProblems',
            'AvailableNotReferencedChangeRequests'
        ], // TODO: remove migratedViews after all lists are migrates
        column: function (columnSettings) {
            var self = this;
            //
            self.UserID = columnSettings.UserID;
            self.ListName = columnSettings.ListName;
            self.MemberName = columnSettings.MemberName;
            {
                var memberName = self.MemberName.toLowerCase();
                self.IsNumeric = memberName.indexOf('count') != -1 || memberName.indexOf('number') != -1;//TODO in list settings
            }
            self.Order = ko.observable(columnSettings.Order);
            self.Width = ko.observable(columnSettings.Width);
            self.Visible = ko.observable(columnSettings.Visible);
            self.SortAsc = ko.observable(columnSettings.SortAsc);
            self.CtrlSortAsc = ko.observable(columnSettings.CrtlSortAsc);
            self.Text = ko.observable(columnSettings.Text);
            //
            self.showResizeThumb = ko.observable(false);
            //
            self.IsEdit = ko.observable(columnSettings.IsEdit);

            self.Template = ko.observable("../Templates/SDForms/EditFieldTemplates/NumberEditor");

            self.IsShowColumnInTable = ko.observable(columnSettings.IsShowInTable);

            return self;
        },
        columnViewModel: function (listView) {
            var self = this;
            //
            self.columnList = ko.observableArray([]);
            self.getColumnByMemberName = (memberName) => {
                return ko.utils.arrayFirst(self.columnList(), column => {
                    return column.MemberName === memberName;
                });
            };

            self.columnListByMemberName = {};
            self.virtualColumns = ko.observableArray([]);//memberName
            //
            self.showEntityModel = {
                isShow: ko.observable(false),
                Visible: ko.observable(false),
            };

            self.showEntityModel.Visible.subscribe((newValue) => {
                self.showEntityService.viewModel.Visible(newValue);
            });

            self.MAX_SCREEN_EXTENSION_WIDTH = 1630;
            self.showEntityService = new ShowEntityService(
                '#mainWrapper',
                '#mainWrapperLeft',
                '#mainWrapperRight',
                '#showEntityResize',
                {
                    minWidthRight: 567,
                    minWidthLeft: 520
                }
            );

            self.showEntityInit = () => {
                const windowWidth = window.screen.width;
                if (windowWidth < self.MAX_SCREEN_EXTENSION_WIDTH) {
                    self.showEntityService.updateScreen();
                    return;
                };

                const plugView = this.getColumnByMemberName('PlugView');

                if (!plugView) return;

                self.showEntityService.init(plugView);
                self.showEntityModel.isShow(true);
                self.showEntityModel.Visible(plugView.Visible());
            };

            self.mountEntityInWrapper = () => {
                const regionTable = $('#regionTable').get(0);
                const regionTableModel = ko.dataFor(regionTable);
                const row = regionTableModel.listView.rowViewModel.rowList()[0];

                if (!row) {
                    self.showEntityService.destroy();
                    return;
                };

                if (!self.showEntityService.viewModel || !self.showEntityService.viewModel.Visible()) {
                    return;
                }

                regionTableModel.listViewRowClick(row.object);
            }
            //
            self.sortedColumnList = ko.pureComputed(function () {
                var retval = [];
                self.columnList().forEach(function (column) {
                    if (column.IsShowColumnInTable() &&
                        (self.virtualColumns().indexOf(column.MemberName) != -1 ||//в данных есть такой дополнительный параметр
                        column.MemberName.indexOf(listView.virtualColumnMemberNamePrefix) === -1)) { //это не дополнительный параметр {
                        retval.push(column);
                    } 
                });
                retval.sort(function (c1, c2) {
                    return c1.Order() == c2.Order() ? 0 : (c1.Order() < c2.Order() ? -1 : 1);
                });
                return retval;
            });
            self.sortedVisibleColumnList = ko.pureComputed(function () {
                var list = self.sortedColumnList();
                var retval = [];
                list.forEach(function (column) {
                    if (column.Visible() === true && column.IsShowColumnInTable())
                        retval.push(column);
                });
                return retval;
            }).extend({ rateLimit: { timeout: 100, method: "notifyWhenChangesStop" } });
            self.sortColumn = ko.pureComputed(function () {
                var list = self.columnList();
                for (var i = 0; i < list.length; i++)
                    if (list[i].SortAsc() != null)
                        return list[i];
                return null;
            });
            //
            self.ctrlSortColumn = ko.pureComputed(function () {
                var list = self.columnList();
                for (var i = 0; i < list.length; i++)
                    if (list[i].CtrlSortAsc() != null)
                        return list[i];
                return null;
            });
            //
            self.createColumn = function (columnSettings) {
                var column = new module.column(columnSettings);
                //
                column.width_handler = column.Width.subscribe(function (newValue) {
                    self.waitAndSaveColumns();
                });
                column.visible_handler = column.Visible.subscribe((newValue) => {
                    if (!column.IsShowColumnInTable()) {
                        self.showEntityService.updateScreen();
                        self.waitAndSaveColumns();

                        if (!newValue) return;

                        self.mountEntityInWrapper();
                        return;
                    };

                    if (newValue == false) {
                        var allHidden = true;
                        self.columnList().forEach(function (column) {
                            if (column.Visible() == true)
                                allHidden = false;
                        });
                        //
                        if (allHidden) {
                            column.Visible(true);
                            return;
                        }
                    }
                    //
                    self.columnList.valueHasMutated();
                    self.waitAndSaveColumns();
                });
                //
                return column;
            };
            self.createVirtualColumnIfNotExists = function (memberName, text) {                
                if (self.columnList().length == 0)
                    throw 'columnList is empty, cannot add a new virtual column';
                memberName = listView.virtualColumnMemberNamePrefix + memberName;
                if (self.virtualColumns().indexOf(memberName) === -1)
                    self.virtualColumns.push(memberName);
                //
                var retval = self.columnListByMemberName[memberName];
                if (retval) {
                    if (!retval.Text())
                        retval.Text(text);
                    return retval;//exists
                }
                //
                var firstColumn = self.columnList()[0];
                var columnSettings = {
                    UserID: firstColumn.UserID,
                    ListName: firstColumn.ListName,
                    MemberName: memberName,
                    Order: self.columnList().length,
                    Width: 150,
                    Visible: true,
                    SortAsc: null,
                    Text: text,
                };
                retval = self.createColumn(columnSettings);
                self.columnListByMemberName[memberName] = retval;
                self.columnList().push(retval);
                return retval;
            };
            //           
            self.disposeColumn = function (column) {
                if (column.width_handler) {
                    column.width_handler.dispose();
                    column.width_handler = null;
                }
                if (column.visible_handler) {
                    column.visible_handler.dispose();
                    column.visible_handler = null;
                }
            };
            self.disposeColumns = function () {
                self.columnList().forEach(function (column) {
                    self.disposeColumn(column);
                });
                self.columnList([]);
                self.columnListByMemberName = {};
            };
            self.dispose = function () {
                self.disposeColumns();
                self.sortedColumnList.dispose();
                self.sortedVisibleColumnList.dispose();
                self.sortColumn.dispose();
                //
                if (self.syncTimeout)
                    clearTimeout(self.syncTimeout);
                self.ajaxControl.Abort();
                //
                self.totalWidth.dispose();
                self.moveThumbDataHandler.dispose();
                //
                $(document).unbind('mousemove', self.onMouseMove);
                $(document).unbind('mouseup', self.onMouseUp);
            };
            //
            {//tips methods
                self.checkShowTooltip = function (data, event) {
                    var $thisObj = $(event.currentTarget);
                    var captionTextElem = $thisObj.find('.captionText')[0];
                    //
                    if (captionTextElem.offsetWidth < captionTextElem.scrollWidth - 2) {//correction for ie
                        var ttcontrol = new tclib.control();
                        ttcontrol.init($thisObj, { text: data.Text(), showImmediat: true });
                    }
                };
            }
            //
            {//sort methods
                self.sortRowsByColumn = function (column) {
                    const sortColumn = self.sortColumn();
                    if (sortColumn != null && sortColumn !== column)
                        sortColumn.SortAsc(null);
                    //
                    self.columnList().forEach(function (column) {                       
                        column.CtrlSortAsc(null);
                    });                  
                    //
                    column.SortAsc(column.SortAsc() ? !column.SortAsc() : true);
                    //
                    const columnsD = self.saveColumns();
                    if (listView.options.virtualMode()) {
                        $.when(columnsD).done(function () {
                            listView.load();
                        });
                    }
                    else
                        listView.calculateVisibleRows();
                };
                //                
                self.sortRowsByColumnWithCtrl = function (column) {
                    //
                    const ctrlSortColumn = self.ctrlSortColumn();
                    const sortColumn = self.sortColumn();
                    if (sortColumn == null)
                        return;
                    if (column.MemberName.indexOf('parameter_') === 0 || sortColumn.MemberName.indexOf('parameter_') === 0) {
                        self.sortRowsByColumn(column);
                        return;
                    }
                    if (ctrlSortColumn != null && ctrlSortColumn !== column)
                        ctrlSortColumn.CtrlSortAsc(null);
                    
                    if (sortColumn === column)
                        self.sortRowsByColumn(column);
                    else {
                        //
                        column.CtrlSortAsc(column.CtrlSortAsc() ? !column.CtrlSortAsc() : true);
                        //
                        const columnsD = self.saveColumns();
                            $.when(columnsD).done(function () {
                                listView.load();
                        });
                    }
                };

                self.columnClick = function (column, event) {
                    if (listView.loading())
                        return;
                    if (column && event.target.className !== 'columnResizeThumb' && ((new Date()).getTime() - self.moveThumbCancelTime) > 3000)
                        if (!event.ctrlKey)
                            self.sortRowsByColumn(column);
                        else {
                            self.sortRowsByColumnWithCtrl(column);
                        }
                };
            }
            //
            {//resize methods
                self.enableResizeThumb = function (column, e) {
                    column.showResizeThumb(true);
                };
                self.disableResizeThumb = function (column, e) {
                    column.showResizeThumb(false);
                };
                //
                self.totalWidth = ko.pureComputed(function () {//для синхронного размера шапки и самой таблицы
                    var retval = 2;
                    self.columnList().forEach(function (column) {
                        if (column.Visible() == true)
                            retval += column.Width();
                    });
                    return retval;
                });
                //
                self.moveThumbData = ko.observable(null);
                self.moveThumbCancelTime = (new Date()).getTime();
                self.moveThumbDataHandler = self.moveThumbData.subscribe(function (newValue) {
                    if (newValue) {
                        var column = newValue.column;
                        if (self.sortedVisibleColumnList().indexOf(column) == self.sortedVisibleColumnList().length - 1) {
                            column.Width(column.Width() + 200);
                        }
                    }
                    else
                        self.moveThumbCancelTime = (new Date()).getTime();
                });
                self.cancelThumbResize = function () {
                    if (self.moveThumbData() != null) {
                        var column = self.moveThumbData().column;
                        column.showResizeThumb(false);
                        self.moveThumbData(null);
                    }
                }
                self.thumbResizeCatch = function (column, e) {
                    if (e.button == 0) {
                        self.moveThumbData({ column: column, startX: e.screenX, startWidth: column.Width() });
                        self.moveThumbData().column.showResizeThumb(true);
                    }
                    else
                        self.cancelThumbResize();
                };
                //
                self.onMouseMove = function (e) {
                    if (self.moveThumbData() != null) {
                        var dx = e.screenX - self.moveThumbData().startX;
                        self.moveThumbData().column.Width(Math.max(self.moveThumbData().startWidth + dx, 50));
                        self.moveThumbData().column.showResizeThumb(true);
                    }
                };
                self.onMouseUp = function (e) {
                    self.cancelThumbResize();
                };
                //
                self.bindHandlersResize = function () {
                    $(document).bind('mousemove', self.onMouseMove);
                    $(document).bind('mouseup', self.onMouseUp);
                };
            }
            //
            {//moving methods               
                self.moveDown = function (column) {
                    if (self.columnList().length === 0)
                        return;
                    var columnIndex = self.sortedColumnList().indexOf(column);
                    if (columnIndex === -1)
                        return;
                    var nextColumn = (columnIndex === self.sortedColumnList().length - 1) ? self.sortedColumnList()[0] : self.sortedColumnList()[columnIndex + 1];
                    //
                    var tmp = column.Order();
                    column.Order(nextColumn.Order());
                    nextColumn.Order(tmp);
                    //
                    self.waitAndSaveColumns();
                };
                self.moveUp = function (column) {
                    if (self.columnList().length === 0)
                        return;
                    var columnIndex = self.sortedColumnList().indexOf(column);
                    if (columnIndex === -1)
                        return;
                    var nextColumn = (columnIndex === 0) ? self.sortedColumnList()[self.sortedColumnList().length - 1] : self.sortedColumnList()[columnIndex - 1];
                    //
                    var tmp = column.Order();
                    column.Order(nextColumn.Order());
                    nextColumn.Order(tmp);
                    //
                    self.waitAndSaveColumns();
                };
                //
                self.moveColumn = function (dragColumn, targetColumn) {
                    if (!dragColumn || !targetColumn)
                        return;
                    //
                    var list = self.sortedColumnList();
                    for (var i = 0; i < list.length; i++)
                        if (list[i].Order() != i)
                            list[i].Order(i);
                    //
                    var dragColumnOrder = dragColumn.Order();
                    var targetColumnOrder = targetColumn.Order();
                    if (dragColumnOrder < targetColumnOrder && targetColumnOrder > 0) {
                        var columns = [];
                        for (var i = dragColumnOrder + 1; i <= targetColumnOrder; i++)
                            columns.push(self.sortedColumnList()[i]);
                        //
                        for (var i = 0; i < columns.length; i++)
                            columns[i].Order(columns[i].Order() - 1);
                        //                        
                        dragColumn.Order(targetColumnOrder);
                        //
                        self.waitAndSaveColumns();
                    }
                    else if (dragColumnOrder > targetColumnOrder) {
                        var columns = [];
                        for (var i = targetColumnOrder; i < dragColumnOrder; i++)
                            columns.push(self.sortedColumnList()[i]);
                        //
                        for (var i = 0; i < columns.length; i++)
                            columns[i].Order(columns[i].Order() + 1);
                        //                        
                        dragColumn.Order(targetColumnOrder);
                        //
                        self.waitAndSaveColumns();
                    }
                };
            }
            //
            {//save methods                
                self.syncTimeout = null;
                self.syncD = null;
                self.ajaxControl = new ajaxLib.control();
                //
                self.waitAndSaveColumns = function () {
                    var d = self.syncD;
                    if (d == null || d.state() == 'resolved') {
                        d = $.Deferred();
                        self.syncD = d;
                    }
                    //
                    if (self.syncTimeout)
                        clearTimeout(self.syncTimeout);
                    self.syncTimeout = setTimeout(function () {
                        if (d == self.syncD) {
                            $.when(self.saveColumns()).done(function () {
                                d.resolve();
                            });
                        }
                    }, 2000);
                    //
                    return d.promise();
                };
                //
                self.getColumnDTL = function (column) {
                    return {
                        UserID: column.UserID,
                        ListName: column.ListName,
                        MemberName: column.MemberName,
                        Order: column.Order(),
                        Width: column.Width(),
                        Visible: column.Visible(),
                        SortAsc: column.SortAsc(),
                        Text: column.Text(),
                        CtrlSortAsc: column.CtrlSortAsc(),
                        IsShowInTable: column.IsShowColumnInTable()
                    };
                };
                //
                self.saveColumns = function () {
                    var d = $.Deferred();

                    if (module.migratedViews.includes(listView.options.settingsName())) {
                        var param = [];
                        self.columnList().forEach(function (obj) {
                            param.push(self.getColumnDTL(obj));
                        });
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                url: '/api/ColumnSettings/' + listView.options.settingsName(),
                                method: 'POST',
                                dataType: "text",
                                contentType: "application/json",
                                data: JSON.stringify(param)
                            },
                            function () {
                            }).done(function () { d.resolve(); });
                    } else { // TODO: remove block else { ... } after all lists are migrated
                        //
                        var param = [];
                        self.columnList().forEach(function (obj) {
                            param.push(self.getColumnDTL(obj));
                        });
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                url: '/accountApi/SetColumnSettingsList',
                                method: 'POST',
                                contentType: "application/json",
                                data: JSON.stringify(param)
                            },
                            function (response) {
                                if (response == false) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('AjaxError') + '\n[ko.ListView.Columns.js, saveColumns]', 'error');
                                    });
                                }
                            }).done(function () { d.resolve(); });
                    }

                    return d.promise();
                };
                //
                self.loadColumns = function () {
                    self.disposeColumns();
                    self.columnList([]);
                    self.columnListByMemberName = {};
                    self.virtualColumns([]);
                    //
                    var d = $.Deferred();
                    var param = {
                        listName: listView.options.settingsName()
                    };
                    if (param.listName.length === 0)
                        throw 'settingName not set';
                    if (module.migratedViews.includes(param.listName)) {
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                url: '/api/columnsettings/' + param.listName,
                                method: 'GET'
                            },
                            function (list) {
                                if (list) {
                                    list.forEach(function (settings) {
                                        var column = self.createColumn(settings);
                                        self.columnList().push(column);
                                        self.columnListByMemberName[column.MemberName] = column;
                                    });

                                    self.columnList.valueHasMutated();
                                    self.showEntityInit();

                                    self.bindHandlersResize();
                                }
                            },
                            function () {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ko.ListView.Columns.js, loadColumns]', 'error');
                                });
                            }).done(function () { d.resolve(); });
                    } else { // TODO: remove this block after all lists migrated
                        //
                        self.ajaxControl.Ajax(null,
                            {
                                url: '/api/GetColumnSettingsList?' + $.param(param),
                                method: 'GET'
                            },
                            function (list) {
                                if (list) {
                                    list.forEach(function (settings) {
                                        var column = self.createColumn(settings);
                                        self.columnList().push(column);
                                        self.columnListByMemberName[column.MemberName] = column;
                                    });

                                    self.columnList.valueHasMutated();
                                    self.showEntityInit();

                                    self.bindHandlersResize();
                                }
                            },
                            function () {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ko.ListView.Columns.js, loadColumns]', 'error');
                                });
                            }).done(function () { d.resolve(); });
                    }

                    return d.resolve();
                };
            }
        }
    }
    return module;
});