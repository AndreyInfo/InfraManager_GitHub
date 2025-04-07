define(['knockout', 'jquery', 'restApiTree', 'checkbox', 'usualForms'], function (ko, $, tree, checkbox, fhModule) {
    var module = {
        ViewModel: function (selectedServices, exceptServices) {
            self = this;
            self.loadPromise = $.Deferred();

            self.Modes = {
                all: 'all',
                selected: 'selected'
            };

            self.CurrentTab = ko.observable(self.Modes.all);
            self.CurrentTab.subscribe(
                function (mode) {
                    if (mode == self.Modes.selected) {
                        self.tree.applyFilter(function (node) { return node.checkbox.isChecked(); });
                    } else {
                        self.tree.applyFilter(function () { return true; });
                    }
                });

            self.ChangeTab = function (tab) {
                self.CurrentTab(tab);
            };

            self.IsActiveTab = function (tab) {
                return self.CurrentTab() == tab;
            };

            self.getSelectedItems = function () {
                var selectedNodes = [];
                self.tree.forEachNode(function (node) {
                    if (node.classId == module.ObjectClass.Service && node.checkbox.isChecked()) {
                        selectedNodes.push(node);
                    }
                });

                return selectedNodes;
            };

            self.IsSelectedTab = function (tabName) {
                return tabName == self.CurrentTab;
            };

            self.Load = function () {
                self.tree.load();
            };

            // tree
            {
                var fetcher = new tree.Fetcher('.tvWrapper');
                var factory = new module.TreeNodeFactory();
                self.tree = new tree.MultiSelectTreeViewModel(
                    tree.LazyLoadingTreeViewModel,
                    fetcher,
                    factory.createRootCollection(),
                    factory.create,
                    checkbox.ViewModel,
                    {  });
               
                function treeLoaded() {
                    var promises = [];
                    self.tree.forEachNode(function (node) {
                        if (node.classId == module.ObjectClass.Category) {
                            promises.push(self.tree.loadChildNodes(node));
                        }
                    });

                    var allNodesLoaded = function () {
                        self.tree.forEachNode(function (node) {
                            if (selectedServices.includes(node.id)) {
                                node.checkbox.check();
                                node.expandParent();
                            }
                        });
                    };
                    if (promises.length > 0) {
                        $.when.apply($, promises).done(allNodesLoaded);
                    } else {
                        allNodesLoaded();
                    }

                    self.loadPromise.resolve();
                };
                self.tree.subscribe(tree.TreeViewEvents.onDataLoaded, treeLoaded);
                self.tree.applyFilter(function (node) { return !exceptServices.includes(node.id); });
            }

            self.ServiceListSelectedCount = ko.pureComputed(function () {
                return self.tree.count(function (node) { return node.classId == module.ObjectClass.Service && node.checkbox.isChecked(); });
            });

            self.ServiceSelectedCountText = ko.computed(function () {
                return `${getTextResource('AssetLinkHeaderChoosen')} (${self.ServiceListSelectedCount()})`;
            });
            // searcher
            {
                self.searchText = ko.observable('');
                self.serviceSearcher = null;

                function initSearcher() {
                    var $frm = $('.select-service-form');
                    var searcherControlD = $.Deferred();
                    //
                    var fh = new fhModule.formHelper();
                    var searcherLoadD = fh.SetTextSearcherToField(
                        $frm.find('.serviceSearcher'),
                        'ServiceSearcher',
                        null,
                        { Types: [0, 1], ExceptServiceIDs: exceptServices },
                        function (objectInfo) {//select
                            self.searchText(objectInfo.FullName);
                            $.when(self.tree.findNode(objectInfo.ID, objectInfo.ClassID)).done(
                                function (node) {
                                    if (node && !node.checkbox.isChecked()) {
                                        node.checkbox.check();
                                    }
                                });
                        },
                        function () {//reset
                            self.searchText('');
                        },
                        function (selectedItem) {//close
                            if (!selectedItem) {
                                self.searchText('');
                            }
                        },
                        undefined,
                        true);
                    $.when(searcherLoadD, userD).done(function (ctrl, user) {
                        searcherControlD.resolve(ctrl);
                        ctrl.CurrentUserID = user.ID;
                        self.serviceSearcher = ctrl;
                    });
                }

                self.afterRender = function () {
                    initSearcher();
                };
            }
        },
        ObjectClass: {
            Category: 405, // ServiceCategory
            Service: 408 //Service
        },
        TreeNodeFactory: function () {
            var self = this;

            self.createCategory = function (idOrDataItem) {
                return new module.CategoryTreeNodeViewModel(idOrDataItem, self);
            };
            self.createService = function (idOrDataItem, parent) {
                return new module.ServiceTreeNodeViewModel(idOrDataItem, self, parent);
            };
            self.createServiceCollection = function (categoryID) {
                //TODO на текущий момент вызывается только из массовых инцидентов. на будующее нужно передавать classId и в зависимости от класса редактировать фильтр
                var filter = "&StateList=1&StateList=2";
                return new tree.TreeNodeCollectionBase('/api/services?categoryID=' + categoryID + filter, self.createService);
            };
            self.createRootCollection = function () {
                return new tree.TreeNodeCollectionBase('/api/servicecategories/', self.createCategory);
            };

            var classMapping = {};
            classMapping[module.ObjectClass.Category] = function () { return self.createCategory; };
            classMapping[module.ObjectClass.Service] = function () { return self.createService; };

            this.create = function (id, classId) {
                var creator = classMapping[classId]();
                return creator(id);
            };
        },
        CategoryTreeNodeViewModel: function (idOrDataItem, factory) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            tree.DataNodeViewModel.call(self, '/api/servicecategories/' + id, id, module.ObjectClass.Category, null, [factory.createServiceCollection(id)]);

            self._getIconCss = function () {
                return ''
            };

            self.setData = function (data) {
                self.text(data.Name);
            };

            self.selectable(false);
        },
        ServiceTreeNodeViewModel: function (idOrDataItem, factory, parent) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            tree.DataNodeViewModel.call(self, '/api/services/' + id, id, module.ObjectClass.Service, parent, []);

            self._getIconCss = function () {
                return '';
            };

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || (data.CategoryID ? factory.createCategory(data.CategoryID) : null);
            };
        }
    };

    return module;
});