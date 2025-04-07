define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        TreeViewEvents: {
            onNodeAdded: 1,
            onDataLoaded: 2,
        },
        TreeViewModelBase: function (fetcher, root, creator, options) {
            var self = this;

            // common tree node interface
            {
                self.children = root;
                self.nodes = ko.observableArray();
                self.dataBound = function () {
                    return false;
                };
                self.parent = null;
                self.visible = ko.observable(true);
            }

            // control interface
            {
                self.selectedNode = ko.observable(null);
                self.load = function () {
                    if (self.children.isLoaded()) {
                        return;
                    }

                    var promise = fetcher.get(self.children.uri);
                    $.when(promise).done(function (list) {
                        self._setNodes(self, self.children, list);
                        self.nodes.removeAll();
                        ko.utils.arrayForEach(self.children.nodes, function (node) { self.nodes.push(node); });
                        raiseCallback(module.TreeViewEvents.onDataLoaded, {});
                    });
                };
            }           

            // configuration
            {
                var defaults = {
                    partiallyChecked: 'PartiallySelected'
                };
                self.config = $.extend({}, defaults, options);
            }

            // private methods
            {
                self._setNodes = function (root, collection, list) {
                    
                    ko.utils.arrayForEach(list, function (item) {
                        var node = collection.creator(item, root);
                        node.setData(item);

                        node.selected.subscribe(function (selected) {
                            if (selected) {
                                var currentNode = self.selectedNode();
                                self.selectedNode(node);

                                if (currentNode) {
                                    currentNode.selected(false);
                                }
                            }
                        });
                        self._nodeAdding(node);
                        self.executeFilter(node);

                        collection.nodes.push(node);

                        raiseCallback(module.TreeViewEvents.onNodeAdded, { node: node });
                    });
                };
                self._nodeAdding = function (node) {
                    // override this method to add logic after node is added into tree
                };
            }

            // events
            {
                var subscribers = {};
                subscribers[module.TreeViewEvents.onDataLoaded] = [];
                subscribers[module.TreeViewEvents.onNodeAdded] = [];

                function raiseCallback(eventId, args) {
                    ko.utils.arrayForEach(subscribers[eventId], function (subscriber) {
                        subscriber(self, args);
                    })
                }

                self.subscribe = function (eventId, subscriber) {
                    if (!subscribers[eventId]) {
                        throw {
                            message: "Событие (" + eventId + ") не поддерживается!"
                        };
                    }

                    if (typeof subscriber !== "function") {
                        throw {
                            message: "Некорректный подписчик"
                        }
                    }

                    subscribers[eventId].push(subscriber);
                };
            }

            // find node
            {
                function find(id, classId, root) {
                    var searchedNode = null;

                    self.forEachNode(function (node) {
                        if (node.id === id && node.classId === classId) {
                            searchedNode = node;
                        }
                    }, root);

                    return searchedNode;
                }

                self.findNode = function (id, classId) {
                    var retD = $.Deferred();
                    var searchedNode = find(id, classId);

                    if (searchedNode) {
                        retD.resolve(searchedNode);
                    } else {
                        searchedNode = creator(id, classId);
                        $.when(fetcher.get(searchedNode.uri)).done(function (data) {
                            // Если искали по IMObjID через фильтр коллекции, то вернется массив из 1 элемента
                            data = Array.isArray(data) ? data[0] : data;
                            // Если дерево было построено по id:int, а искать элемент пришлось по IMObjID:guid,
                            // то повторим поиск гарантированно по id:int
                            // TODO: убрать if (data.ID !== id) после того, как ObjectSearch будет возвращать ID:int
                            if (data.ID !== id) {
                                searchedNode = creator(data.ID, classId)
                            }
                            searchedNode.setData(data);

                            var parent = searchedNode.parent;
                            if (parent) {
                                $.when(self.findNode(parent.id, parent.classId)).done(function (parentNode) {
                                    if (parentNode) {
                                        $.when(self.loadChildNodes(parentNode)).done(function () {
                                            if (!parentNode.expanded()) {
                                                parentNode.expandCollapse();
                                            }
                                            retD.resolve(find(searchedNode.id, classId, parentNode));
                                        });
                                    } else { 
                                        // На вход пришла нода, которой нет в дереве (например пользователь, у которого нет SubdivisionID)
                                        retD.resolve(null);
                                    }
                                });
                            } else {
                                // На вход пришла нода, которой нет в дереве (например пользователь, у которого нет SubdivisionID)
                                retD.resolve(null);
                            }
                        });
                    }

                    return retD.promise();
                }
            }

            // filter
            {
                var nodeFilter = function () { return true; };
                self.applyFilter = function (filter) {
                    nodeFilter = filter;
                    self.executeFilter();
                };
                self.executeFilter = function (root) {
                    if (root) {
                        var count = self.count(nodeFilter, root);

                        root.visible(nodeFilter(root) || count > 0);
                    }

                    if (root && !root.visible()) {
                        return;
                    }

                    ko.utils.arrayForEach((root || self).nodes(), self.executeFilter);
                };
            }

            // helpers
            {
                self.forEachNode = function (action, root) {
                    ko.utils.arrayForEach((root || self).nodes(), function (node) {
                        action(node);
                        self.forEachNode(action, node);
                    });
                };
                self.collapseAll = function () {
                    self.forEachNode(function (node) {
                        if (node.expandable()) {
                            node.expanded(false);
                        }
                    });
                };
                self.expandAll = function () {
                    self.forEachNode(function (node) {
                        if (node.expandable()) {
                            node.expanded(true);
                        }
                    });
                };
                self.count = function (filter, root) {
                    var count = 0;
                    self.forEachNode(function (node) {
                        if (filter(node)) {
                            count++;
                        }
                    }, root);

                    return count;
                };
                self.loadChildNodes = function (node) {
                    var retD = $.Deferred();
                    var promises = [];

                    ko.utils.arrayForEach(node.children, function (collection) {
                        var promise = fetcher.get(collection.uri)
                        promises.push(promise);
                        $.when(promise).done(function (list) {
                            self._setNodes(node, collection, list);
                            collection.isLoaded(true);
                        });
                    });

                    $.when.apply($, promises).done(function () {
                        node.expandable(node.nodes().length > 0);
                        retD.resolve();
                    });

                    return retD.promise();
                };
                self.selectNode = function (id, classId) {
                    var retD = $.Deferred();

                    $.when(self.findNode(id, classId)).done(function (node) {
                        if (node !== null) {
                            node.select();
                            var $nodeElement = $('#' + node.key);
                            
                            if ($nodeElement.length > 0) {
                                $nodeElement[0].scrollIntoView();
                            }
                            retD.resolve(node);
                        } else {
                            retD.resolve(null);
                        }
                    });

                    return retD.promise();
                };
                self.getSelectedNodeTreePath = function () {
                    var currentNode = self.selectedNode();
                    var treePath = [];
                    
                    if (!currentNode) {
                        return treePath;
                    }
                    
                    while (currentNode != null) {
                        treePath.push(currentNode);
                        currentNode = currentNode.parent;
                    }
                    
                    return treePath;
                };
                self.deselectAll = function () {
                    self.forEachNode(function (node) { node.selected(false); });
                };
            }
        },
        LazyLoadingTreeViewModel: function (fetcher, root, creator, options) {
            var self = this;

            module.TreeViewModelBase.call(self, fetcher, root, creator, options);

            // overrides
            {
                self._nodeAdding = function (node) {
                    node.expanded.subscribe(function () {
                        if (node.expandable() && node.nodes().length === 0) {
                            self.loadChildNodes(node);
                         }
                    });                    
                };
            }
        },
        MultiSelectTreeViewModel: function (baseTreeViewModel, fetcher, root, creator, checkboxViewModel, options) {
            var self = this;
            baseTreeViewModel.call(self, fetcher, root, creator, options);

            // edit 
            {
                function uncheckAll(node) {
                    ko.utils.arrayForEach(
                        (node || self).nodes(),
                        function (node) {
                            node.checkbox.uncheck();
                        });
                };

                self.readonly = ko.observable(false);
                self.readonly.subscribe(function (readonly) {
                    self.forEachNode(function (node) {
                        node.checkbox.readonly(readonly);
                    });
                });
            }

            // checkboxes data source
            {
                var data = {};
                function tryGetCheckStateFromData(node) {
                    if (!self.inData(node)) {
                        return;
                    }

                    var dataItem = data[node.classId][node.id];

                    if (dataItem[self.config.partiallyChecked]) {
                        node.checkbox.checkPartially();
                    } else {
                        node.checkbox.check();
                    }
                };
                self.inData = function (node) {
                    return data[node.classId]
                        && data[node.classId][node.id];
                };
                self.setData = function (items) {
                    data = {};
                    uncheckAll();
                    ko.utils.arrayForEach(items, function (item) {
                        data[item[self.config.classId]] = data[item[self.config.classId]] || {};
                        data[item[self.config.classId]][item[self.config.id]] = item;
                    });
                    self.forEachNode(function (node) {
                        if (node.checkbox.isUnchecked()) {
                            tryGetCheckStateFromData(node);
                        }
                    });
                }

                function getCheckedNodes(root) {
                    var result = [];

                    if (root.dataBound() && root.checkbox.isChecked()) {
                        var data = {};
                        data[self.config.id] = root.id;
                        data[self.config.classId] = root.classId;

                        return [data];
                    } else if (!root.dataBound() || root.checkbox.isPartiallyChecked()) {
                        ko.utils.arrayForEach(root.nodes(), function (node) {
                            result = result.concat(getCheckedNodes(node));
                        });
                    }

                    return result;
                }
                self.getData = function () {
                    return $.map(getCheckedNodes(self), function (node) {
                        var data = {};
                        data[self.config.id] = node.id;
                        data[self.config.classId] = node.classId;

                        return data;
                    });
                };
                self.getCheckedNodes = function () {
                    return getCheckedNodes(self);
                };
            }

            // overrides
            {
                var baseNodeAdding = self._nodeAdding;
                self._nodeAdding = function (node) { // extend node by attaching a checkbox viewmodel 
                    baseNodeAdding(node);
                    node.checkbox = new checkboxViewModel(ko.pureComputed(function () { return self.readonly() || !node.visible(); }));

                    if (node.parent && node.parent.dataBound() // parent is a data node
                        && (node.parent.checkbox.isChecked() || node.parent.checkbox.isUnchecked())) {
                        node.checkbox.copyState(node.parent.checkbox);
                    } else {
                        tryGetCheckStateFromData(node);
                    }

                    node.checkbox.subscribe(function () {
                        self.nodeCheckStateChanged(node);
                    });

                    return node;
                };
            }

            // parent / child checkboxes relations
            {
                self.nodeCheckStateChanged = function (node) {
                    if (node.parent && node.parent.id) { // recalculate parent check state
                        self.determineParentNodeCheckState(node.parent);
                    }

                    if (!node.checkbox.isPartiallyChecked()) { // change child nodes states
                        var checked = node.checkbox.isChecked();
                        ko.utils.arrayForEach(node.nodes().filter(function (node) { return node.visible(); }), function (child) {
                            if (checked) {
                                child.checkbox.check();
                            } else {
                                child.checkbox.uncheck();
                            }
                        });
                    }

                    self.executeFilter(node);
                };
                self.determineParentNodeCheckState = function (root) {
                    if (root.nodes().length === 0 || !root.checkbox) {
                        return;
                    }

                    if (root.nodes().length === ko.utils.arrayFilter(root.nodes(), function (node) { return node.checkbox.isChecked(); }).length) {
                        root.checkbox.check();
                    } else if (root.nodes().length === ko.utils.arrayFilter(root.nodes(), function (node) { return node.checkbox.isUnchecked(); }).length) {
                        root.checkbox.uncheck();
                    } else {
                        root.checkbox.checkPartially();
                    }
                };
            }
        },
        TreeNodeCollectionBase: function (uri, creator, nodes) {
            var self = this;

            self.isLoaded = ko.observable(false);
            self.nodes = nodes || [];
            self.creator = creator;
            self.uri = uri;
        },
        DataNodeViewModel: function (uri, id, classId, parent, children) {
            var self = this;

            // common tree node interface
            {
                self.nodes = ko.observableArray();
                self.children = children;
                self.dataBound = function () { return true; };
                self.parent = parent;
                self.visible = ko.observable(parent && parent.visible());             
                self.checkbox = false;

                self._subscribeParent = function () {
                    if (!self.parent) {
                        return;
                    }

                    self.parent.visible.subscribe(function (visible) {
                        self.visible(visible); // hide children if parent is hidden
                    });

                    if (self.parent.dataBound()) {
                        self.visible.subscribe(function (visible) {
                            if (visible) {
                                self.parent.visible(true); // unhide parent if any child is set visible
                                self.parent.expandable(true);
                            } else if (ko.utils.arrayFilter(self.parent.nodes(), function (node) { return node.visible(); }).length === 0 && self.parent.dataBound()) {
                                self.parent.expandable(false);
                            }
                        });
                    }
                }

                if (parent) {
                    self._subscribeParent();
                }

                function addChildren() {
                    self.nodes.removeAll();
                    ko.utils.arrayForEach(self.children, function (childrenCollection) {
                        if (childrenCollection.isLoaded()) {
                            var sortedNodes = childrenCollection.nodes.sort(function (left, right) {
                                return left.text() >= right.text() ? 1 : -1;
                            });

                            ko.utils.arrayForEach(sortedNodes, function (node) {
                                self.nodes.push(node);
                            });
                        }
                    });
                }

                ko.utils.arrayForEach(children, function (collection) {
                    collection.isLoaded.subscribe(addChildren);
                });                
            }

            // node identification
            {
                self.uri = uri;
                self.id = id;
                self.classId = classId;
                self.key = self.classId + '_' + self.id;
            }

            // node visual elements
            {
                self.setData = function (data) {
                    throw 'Override this method "DataNodeViewModel.setData".'
                }

                self.text = ko.observable('');
                self.selected = ko.observable(false);
                self.expanded = ko.observable(false);
                self.expandable = ko.observable(children && children.length > 0);

                self._getIconCss = function () {
                    throw 'Override this method "DataNodeViewModel._getIconCss".';
                };
                self.iconCss = ko.pureComputed(
                    function () {
                        return self._getIconCss() + (self.selected() ? " active" : "");
                    });
            }

            // state change
            {
                self.selectable = ko.observable(true);
                self.select = function () {
                    if (self.selectable()) {
                        self.selected(true);
                        self.expandCollapse();
                    }
                };

                self.expandCollapse = function () {
                    if (self.expandable()) {
                        self.expanded(!self.expanded());
                    }
                };

                self.expandParent = function () {
                    if (parent && parent.dataBound() && !parent.expanded()) {
                        parent.expandCollapse();
                        parent.expandParent();
                    }
                }
            }
        },
        Fetcher: function (region) {
            this.get = function (uri) {
                var retD = $.Deferred();

                new ajaxLib.control().Ajax(
                    $(region), {
                        dataType: "json",
                        method: 'GET',
                        url: uri
                    }, function (data) {
                        retD.resolve(data);
                    },
                    function () {
                        retD.resolve({});
                    }
                );

                return retD.promise();
            }
        },
        getID: function (idOrDataItem, idProperty) {
            if (!idOrDataItem) {
                throw new 'idOrDataItem should be initialized';
            }

            return typeof idOrDataItem === 'object' ? idOrDataItem[idProperty] : idOrDataItem;
        },
    };

    return module;
});