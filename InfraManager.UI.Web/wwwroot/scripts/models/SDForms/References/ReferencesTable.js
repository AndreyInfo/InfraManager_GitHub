define(['knockout', 'jquery', 'ajax', 'ui_controls/ListView/ko.ListView.Helpers'], function (ko, $, ajaxLib, mHelpers) {
    const module = {
        ViewModel: function (form, viewName, ajaxOptions) {
            const self = this;

            self.ajaxControl = new ajaxLib.control();
            self.form = form;

            self.viewName = viewName;
            self.listViewID = 'listView_' + ko.getNewID();
            self.count = ko.observable(0);

            // methods by table
            self.listViewInit = function (listView) {
                self.listView = listView;
                mHelpers.init(self, listView);  
                
                self.listView.load();
            };

            self.listViewRetrieveItems = function () {
                const retvalD = $.Deferred();

                $.when(self.getObjectList()).done(function (objectList) {
                    retvalD.resolve(objectList);
                    self.sizeChanged();
                });

                return retvalD.promise();
            };

            // для возможности переопределения снаружи
            self.listViewRowClick = function (_obj) { };

            function fetchData(idList, callback) {
                const retvalD = $.Deferred();

                self.ajaxControl.Ajax(null, self.buildAjaxOptions(idList), function (newVal) {
                    if (!idList) {
                        self.count(newVal ? newVal.length : 0);//
                    }
                    retvalD.resolve(newVal || []);
                    callback(newVal);
                }, function () { self.count(0); });

                return retvalD.promise();
            }

            self.getObjectList = function () {
                return fetchData(null, self.sizeChanged);
            };

            self.fetchData = function () {
                return fetchData(null, function () { });
            };

            function appendRow(data) {
                var row = self.listView.rowViewModel.createRow(data);
                self.listView.rowViewModel.rowList.push(row);
                self.count(self.count() + 1);
            }

            self.appendRow = function (data) {
                appendRow(data);
                self.sizeChanged();
            };

            self.getObjectID = function (obj) {
                return obj.ID;
            };

            self.listViewContextMenu = ko.observable(null);

            var contextMenuActions = [];
            self.addContextMenuAction = function (resText, action, enabled) {
                contextMenuActions.push(function (contextMenu) {
                    const cmd = contextMenu.addContextMenuItem();
                    cmd.restext(resText);
                    cmd.click(action);
                    cmd.enabled = enabled;
                });
            };
            self.contextMenuInit = function (contextMenu) {
                if (contextMenuActions.length === 0) {
                    return;
                }

                self.listViewContextMenu(contextMenu);

                // init
                contextMenuActions.forEach(function (button) {
                    const selectedItem = self.listView.rowViewModel.focusedRow;
                    button.call(this, contextMenu, selectedItem);
                });
            };

            // additional
            self.addReferencesByID = function (ids) {
                fetchData(ids, function (items) {
                    items.forEach(function (item) {
                        appendRow(item);
                    });

                    self.listView.waitAndRenderTable();
                });
            };

            self.removeReferencesByID = function (ids) {
                ids.forEach(function (id) {
                    if (self.removeRowByID(id)) {
                        self.count(self.count() - 1);
                    }
                });
            };

            // helpers
            self.buildAjaxOptions = function (idList) {
                var params = Object.assign({ contentType: 'application/json' }, ajaxOptions);
                params.method = params.method || 'GET';
                params.data = Object.assign({ ViewName: viewName }, typeof ajaxOptions.data === 'function' ? ajaxOptions.data() : ajaxOptions.data);

                if (idList) {
                    params.data.IDList = idList;
                    params.traditional = params.method === 'GET';
                }

                if (params.method === 'POST') {
                    params.dataType = 'json';
                    params.data = JSON.stringify(params.data);
                }

                return params;
            };

            self.getCheckedItems = function () {
                const selectedItem = self.listView.rowViewModel.focusedRow;
                const checkedItems = self.listView.rowViewModel.checkedItems();

                if (!selectedItem() && !checkedItems.length) {
                    return [];
                };

                if (!checkedItems.length) {
                    return [selectedItem().object];
                };

                return checkedItems;
            };

            self.sizeChanged = function () {
                const tableContainer = self.form.find('[data-table-container]');
                const tableContainerHeader = tableContainer.find('[data-table-container-header]');
                const heightContentTable = tableContainer.height() - tableContainerHeader.outerHeight(true); 

                tableContainer.find('.references-list').css({
                    height: heightContentTable
                });

                self.listView.renderTable();
            };

            // outher
            self.init = function () {
                const modal = form.closest('.ui-dialog');
                modal.resize(self.sizeChanged);
            };

            self.init();
        }
    };

    return module;
});
