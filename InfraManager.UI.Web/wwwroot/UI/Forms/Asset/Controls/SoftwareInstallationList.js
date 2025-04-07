define(['knockout', 'jquery', 'ajax', 'imList', 'models/Table/TableParts'], function (ko, $, ajaxLib, imLib, tpLib) {
        var module = {
            LinkList: function (ko_object, ajaxSelector, ko_canEdit) {
            var self = this;
            //
            self.ClassID = 71;//класс элементов списка
            self.isLoaded = ko.observable(false);
            self.imList = null;
            self.ajaxControl = new ajaxLib.control();
            self.CanEdit = ko_canEdit;
            self.Controller = null;//задается в AssetReferenceControl.js
            //
            self.CheckData = function () {
                var returnD = $.Deferred();
                if (!self.isLoaded()) {
                    $.when(self.imList.Load()).done(function () {
                        self.isLoaded(true);
                        returnD.resolve();
                    });
                }
                else
                    returnD.resolve();
                //
                return returnD.promise();
            };
            //
            self.PushData = function (list) {//загрузка списка 
                var returnD = $.Deferred();
                $.when(self.imList.Push(list)).done(function () {
                    returnD.resolve();
                });
                return returnD.promise();
            };
            //
            self.PushItemToStart = function (item) {//добавляет элемент в начало списка
                $.when(self.imList.PushToStart(item)).done(function () {
                    self.isLoaded(true);
                });
            };
            //
            self.ClearData = function () {
                self.imList.List([]);
                //
                self.isLoaded(false);
            };
            //
            self.ItemsCount = ko.computed(function () {
                var retval = 0;
                if (self.isLoaded())
                    retval = self.imList.List().length;
                //
                return ' (' + (retval ? retval : 0) + ')';
            });
            //
            self.HeaderText = ko.computed(function () {
                return getTextResource('Asset_SoftwareInstallationListHeader') + self.ItemsCount();
            });
            //
            self.sortList = function (sortAsc, fieldName) {
                self.imList.List.sort(function (left, right) {
                    if (left[fieldName]() == null)
                        return sortAsc ? -1 : 1;
                    //
                    if (right[fieldName]() == null)
                        return sortAsc ? 1 : -1;
                    //
                    var _left = left[fieldName]().toUpperCase();
                    var _right = right[fieldName]().toUpperCase();
                    //
                    return _left == _right ? 0 : (_left < _right ? (sortAsc ? -1 : 1) : (sortAsc ? 1 : -1));
                });
            };
            //
            self.SortTable = function () {
                if (!self.imList.List)
                    return;
                //
                $.each(self.columnList(), function (index, item) {
                    var fieldName = '';
                    if (item.SortAsc() !== null) {
                        if (index === 0) fieldName = 'TypeName'
                        if (index === 1) fieldName = 'ModelName'
                        if (index === 2) fieldName = 'SerialNumber'
                        if (index === 3) fieldName = 'InvNumber'
                        //
                        self.sortList(item.SortAsc(), fieldName);
                        self.Init(index, item.SortAsc());
                    }
                });
            };
            //
            self.Reload = function () {
                self.SortTable();
            };
            //
            {//splitter
                self.MaxHeight = 2147483647;
                self.MinHeight = -self.MaxHeight;
                self.Height = ko.observable(0);
                self.HeightCoeff = 0;
                self.Delta = 0;//self.Height() + self.Delta = высота до начала скролла вверх
                self.ActionsPanelHeight = 60;
                self.HeaderHeight = 30;
                self.TableHeaderHeight = 30;
                self.HorizontalPadding = 20 + 30;
                //
                self.IsActionsVisible = ko.observable(false);
                //
                self.HasSelectedItems = function () {
                    return self.imList.SelectedItemsCount() != 0;
                };
                //
                self.OnSelectionChanged = function () {
                    if (self.HasSelectedItems()) {
                        if (!self.IsActionsVisible()) {
                            var height = Math.max(self.Height() - self.ActionsPanelHeight, 0);
                            self.Height(height);
                            self.IsActionsVisible(true);
                        }
                    }
                    else {
                        var height = self.Height() + self.ActionsPanelHeight;
                        self.Height(height);
                        self.IsActionsVisible(false);
                    }
                    //
                };
                //
                self.ResetHeightCoeff = function () {
                    self.HeightCoeff = self.Height() / self.Controller.GetResizableContentHeight();
                };
                //
                self.ResizeFunction = function (newHeight, isCustomCall) {
                    if (!self.Controller || !self.IsExpanded())
                        return;
                    //
                    var height;
                    if (!isCustomCall) {
                        height = newHeight - self.HeaderHeight;
                        if (self.HasSelectedItems())
                            height -= self.ActionsPanelHeight;
                    }
                    else
                        height = newHeight;
                    //
                    var delta = height - self.Height();
                    if (delta === 0)
                        return;
                    //
                    if (isCustomCall) {
                        if (height >= self.MinHeight && height <= self.MaxHeight) {
                            self.Height(height);
                            self.ResetHeightCoeff();
                        }
                    }
                };
            }
            //
            self.IsExpanded = ko.observable(true);
            self.ExpandCollapseClick = function () {
                if (self.imList.List().length == 0)
                    return;
                //
                if (!self.Controller)
                    return;
                //
                self.IsExpanded(!self.IsExpanded());
                //
                var otherItem = self.Controller.GetOtherItem(self);
                var h = self.Controller.getTabHeight() - self.Controller.Items.length * self.HeaderHeight - self.Controller.ExpandedItemsCount() * self.TableHeaderHeight;
                if (self.IsExpanded()) {
                    var height =  otherItem && otherItem.IsExpanded() ? h / 2 : h;
                    self.Height(height - (self.HasSelectedItems() ? self.ActionsPanelHeight : 0));
                    if(otherItem)
                    otherItem.Height(height - (otherItem.HasSelectedItems() ? self.ActionsPanelHeight : 0));
                }
                else {
                    self.Height(0);
                    if (otherItem)
                        otherItem.Height(h - (otherItem.HasSelectedItems() ? self.ActionsPanelHeight : 0));
                }
                self.Controller.ResetHeightCoeffs();
            };
            //
            self.ShowObjectForm = function (softwareInstallation) {
                return;//not implemented
                //
                /*showSpinner();
                self.MarkRow(softwareInstallation.ID, false, 'highlight');
                self.MarkRow(softwareInstallation.ID, false, 'modified');
                //
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowAssetForm(softwareInstallation.ID, self.ClassID);
                });*/
            };
            //
            {
                self.moveTrumbData = ko.observable(null);
                self.moveThumbCancelTime = (new Date()).getTime();
                self.moveTrumbData.subscribe(function (newValue) {
                    if (newValue) {
                        var column = newValue.column;
                        if (self.columnList.indexOf(column) == self.columnList.length - 1) {
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
            }
            //
            self.columnList = ko.observableArray([]);
            self.rowList = ko.observableArray([]);
            //
            self.TableWidth = ko.computed(function () {//для синхронного размера шапки и самой таблицы
                var retval = 2;
                //
                for (var i = 0; i < self.columnList().length; i++) {
                    var column = self.columnList()[i];
                    retval += column.Width();
                }
                //
                return retval;
            });
            //
            self.GetOrCreateRow = function (id, classID, addToBeginOfList, realObjectID, operationInfo, data) {
                //
                var isNew = false;
                var row = self.rowHashList[id];
                if (!row) {
                    row = self.rowHashList[id] = new tpLib.createRow(id, classID, realObjectID, operationInfo, self.ShowObjectForm, self.RowSelectedChanged, self.thumbResizeCatch, self.moveTrumbData, data);
                    if (addToBeginOfList == true)
                        self.rowList().unshift(row);
                    else
                        self.rowList().push(row);
                    isNew = true;
                }
                else
                    row.OperationInfo = operationInfo;
                return {
                    row: row,
                    isNew: isNew
                };
            };
            //
            self._columnList = null;

            self.columnWithDataList = null;
            self.Init = function (idx, sortAsc) {
                var retD = $.Deferred();
                var tableModel = self;
                require(['models/Table/Columns'], function (vm) {
                    $.when(userD).done(function (user) {
                        //
                        if (!self._columnList) {
                            self._columnList = [];
                            ko.utils.arrayForEach(self.columnWithDataList, function (item) {
                                self._columnList.push(new module.Column(item.Text, item.Width, user.UserID, item.MemberName, item.Order, item.SortAsc));
                            });
                            //
                            $.each(self._columnList, function (index, item) {
                                var fieldName = '';
                                var sortAsc = item.ColumnSettings.SortAsc;
                                if (sortAsc !== null) {
                                    if (index === 0) fieldName = 'TypeName'
                                    //
                                    self.sortList(sortAsc, fieldName);
                                }
                            });
                        }

                        if (idx || idx === 0)
                            $.each(self._columnList, function (index, item) {
                                item.ColumnSettings.SortAsc = index === idx ? sortAsc : null;
                                item.Data = [];
                                item.ColumnSettings.Width = self.columnList()[index].Width();
                            });

                        var idList = [];

                        ko.utils.arrayForEach(self.imList.List(), function (item) {
                            self._columnList[0].Data.push(new module.ColumnData(item, item.TypeName));
                            //
                            idList.push(item.ID);
                        });

                        var mod = new vm.ViewModel(tableModel, null, null, $('.portList'));

                        self.rowHashList = {};
                        self.columnList.removeAll();
                        self.rowList.removeAll();
                        //
                        var dataList = [];
                        $.each(self._columnList, function (index, columnWithData) {
                            if (columnWithData && columnWithData.ColumnSettings && columnWithData.Data) {
                                var column = self.createColumn(columnWithData.ColumnSettings);
                                self.columnList().push(column);
                                //
                                var data = columnWithData.Data;
                                for (var i = 0; i < data.length; i++) {
                                    //var rowInfo = self.GetOrCreateRow(newVal.IDList[i], newVal.ClassIDList[i], false, realObjectID, newVal.OperationInfoList[i]);
                                    var rowInfo = self.GetOrCreateRow(idList[i], null, false, null, null, data[i].Item);
                                    rowInfo.row.AddCell(column, data[i].Text, null);
                                }
                            }
                        });
                        //                                                 
                        self.columnList.valueHasMutated();
                        self.rowList.valueHasMutated();
                        //
                        self.OnScroll = function () {
                            $('.installationList .tableHeader').css('margin-left', -this.scrollLeft);
                        };
                        //
                        $('.installationList .tabContent').bind('scroll', self.OnScroll);
                        //
                        retD.resolve();
                    });
                });
                return retD.promise();
            };
            //
            var imListOptions = {};//параметры imList для списка 
            {
                imListOptions.aliasID = 'ID';
                //
                imListOptions.LoadAction = function () {
                    var retvalD = $.Deferred();
                    if (self.Data) {
                        require(['models/AssetForms/AssetForm.SoftwareInstallationReference'], function (softwareInstallationLib) {
                            var retval = [];
                            self.columnWithDataList = self.Data.Columns;
                            ko.utils.arrayForEach(self.Data.Data, function (item) {
                                retval.push(new softwareInstallationLib.SoftwareInstallationReference(self.imList, item));
                            });
                            retvalD.resolve(retval);
                        });
                    }
                    return retvalD.promise();
                };
                //
                self.imList = new imLib.IMList(imListOptions);
                self.imList.OnSelectionChanged = self.OnSelectionChanged;
                //
                operationsOptions = {};
                {
                    operationsOptions.Text = getTextResource('RemoveReference');
                    operationsOptions.Validator = function () { return true; };
                    operationsOptions.IsGranted = function (grantedOperations) { return true; };

                    operationsOptions.Command = function () {
                        var selectedCalls = self.imList.SelectedItems();
                        if (selectedCalls.length == 0)
                            return;
                        //
                        var ids = [];
                        var question = '';
                        selectedCalls.forEach(function (el) {
                            ids.push(el.ID);
                            //
                            if (question.length < 200) {
                                question += (question.length > 0 ? ', ' : '') + '№ ' + el.ModelName;
                                if (question.length >= 200)
                                    question += '...';
                            }
                        });
                        //
                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('ReferenceRemoving') + ': ' + question,
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
                                    //
                                    var data = {
                                        'ObjectClassID': ko_object().ClassID(),
                                        'ObjectID': ko_object().ID(),
                                        'ReferenceClassID': 33,
                                        'ReferenceIDList': ids
                                    };
                                    self.ajaxControl.Ajax($(ajaxSelector),
                                        {
                                            dataType: "json",
                                            method: 'POST',
                                            data: data,
                                            url: '/sdApi/RemoveAssetReference'
                                        },
                                        function (newVal) {
                                            ko.utils.arrayForEach(ids, function (id) {
                                                self.imList.TryRemoveByID(id, true);
                                            });
                                        });
                                }
                            });
                        });
                    };
                }
                self.imList.AddOperation(operationsOptions);
            }
        },
        Column: function (name, width, userID, memberName, order, sortAsc) {
            var thisObj = this;
            thisObj.ColumnSettings = new module.ColumnSettings(name, width, userID, memberName, order, sortAsc);
            thisObj.Data = [];
            thisObj.Item = ko.observable(null);
            //
            thisObj.checkShowTooltip = function (data, event) {
                if (data && event) {
                    var $thisObj = $(event.currentTarget);
                    var labelElem = $thisObj.find('.columnLabel')[0];
                    //
                    if (labelElem.offsetWidth < labelElem.scrollWidth - 2) {//correction for ie
                        var ttcontrol = new tclib.control();
                        ttcontrol.init($thisObj, { text: thisObj.Text, showImmediat: true });
                    }
                }
            };
            //
            thisObj.showResizeThumb = ko.observable(false);
            thisObj.enableResizeThumb = function (m, e) {
                thisObj.showResizeThumb(true);
            };
            thisObj.disableResizeThumb = function (m, e) {
                thisObj.showResizeThumb(false);
            };
        },
        ColumnSettings: function (name, width, userID, memberName, order, sortAsc) {
            var thisObj = this;
            thisObj.Text = name;
            thisObj.Width = width;
            thisObj.UserID = userID
            thisObj.SortAsc = sortAsc;
            thisObj.ListName = 'InstallationList';
            thisObj.MemberName = memberName;
            thisObj.Order = order;
        },
        ColumnData: function (item, text) {
            var thisObj = this;
            thisObj.Item = item;
            thisObj.Text = text;
        }
    };
	return module;
});