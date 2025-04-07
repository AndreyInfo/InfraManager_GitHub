define(['knockout', 'jquery', 'treeViewModel', 'usualForms'], function (ko, $, tree, fhModule) {
    var module = {
        TabViewModel: function (treeViewModel, dataProvider, options) {
            var self = this;

            function autoExpandPartiallyCheckedNode(node) {
                if (node.checkbox.isPartiallyChecked() && node.expandable()) {
                    node.expanded(true);
                }
            }

            function autoSelectFoundNode(node, foundItem, path) {
                if (node.id === foundItem.ID && node.classId === foundItem.ClassID) {
                    node.selected(true);
                    $('#' + node.key)[0].scrollIntoView();
                } else if (node.expandable() && ko.utils.arrayFirst(path, function (pathNode) { return pathNode.ID === node.id && pathNode.ClassID === node.classId })) {
                    node.expanded(true);
                }
            }

            treeViewModel.subscribe(tree.TreeViewEvents.onNodeAdded, function (sender, e) {
                if (self.showOnlyAllowed()) {
                    autoExpandPartiallyCheckedNode(e.node);
                }

                if (self.selectedItem() !== null) {
                    autoSelectFoundNode(e.node, self.selectedItem(), self.path);
                }
            });

            self.searcher = null;
            self.selectedItem = ko.observable(null);
            self.selectedItem.subscribe(function (item) {
                if (item !== null) {
                    self.searchText(item.FullName);
                    $.when(dataProvider.getPath(item.ID, item.ClassID))
                        .done(function (path) {
                            self.path = path;
                            treeViewModel.forEachNode(function (node) {
                                autoSelectFoundNode(node, item, path);
                            });
                        });

                } else {
                    self.path = [];
                    self.searchText('');
                    treeViewModel.forEachNode(function (node) { node.selected(false); });
                }
            });
            self.path = [];
            self.searchText = ko.observable('');

            self.initSearcher = function () {
                //
                var fh = new fhModule.formHelper();
                var searcherLoadD = fh.SetTextSearcherToField(
                    $(options.inputXPath),
                    options.name,
                    null,
                    null,
                    function (objectInfo) {//select
                        self.selectedItem(objectInfo);
                    },
                    function () {//reset
                        self.selectedItem(null);
                    },
                    function (selectedItem) {//close
                        if (!selectedItem) {
                            self.selectedItem(null);
                        }
                    },
                    undefined,
                    true);
                $.when(searcherLoadD, userD).done(function (ctrl, user) {
                    ctrl.CurrentUserID = user.ID;
                    self.searcher = ctrl;
                });
            };

            self.showOnlyAllowed = ko.observable(false);
            self.showOnlyAllowed.subscribe(function (value) {
                if (value) {
                    treeViewModel.applyFilter(function (node) { return node.checkbox.isChecked(); });
                    treeViewModel.forEachNode(autoExpandPartiallyCheckedNode);
                } else {
                    treeViewModel.applyFilter(function (node) { return true; })
                }
            });
            self.collapseAll = function () {
                treeViewModel.collapseAll();
            };
        }
    };

    return module;
});