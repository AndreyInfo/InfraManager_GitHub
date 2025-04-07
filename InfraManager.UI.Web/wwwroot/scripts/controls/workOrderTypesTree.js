define(
    [ 'knockout', 'jquery', 'ajax', 'restApiTree' ],
    function (ko, $, ajaxLib, tree) {
        var module = {
            ObjectClass: {
                WorkOrderType: 142,
                UserActivityType: 12,
            },

            TreeBuilder: function () {
                var self = this;

                let findParent = function (node, items) {
                    let found;
                    
                    ko.utils.arrayForEach(items, function (item) {
                        if (node.ClassID === module.ObjectClass.WorkOrderType) {
                            ko.utils.arrayForEach(item.References || [], function (reference) {
                                if (node.ID === reference.ObjectID) {
                                    found = {
                                        ID: reference.ID,
                                        ParentID: item.ParentID,
                                        Name: item.Name,
                                        ClassID: module.ObjectClass.UserActivityType,
                                        Selectable: false,
                                        Children: [],
                                    };
                                }
                            });
                        } else if (node.ParentID && node.ParentID === item.ID) {
                            found = {
                                ID: item.ID,
                                ParentID: item.ParentID,
                                Name: item.Name,
                                ClassID: module.ObjectClass.UserActivityType,
                                Selectable: false,
                                Children: [],
                            };
                        }
                    });

                    return found;
                };

                self.build = function (woTypes, uaTypes) {
                    let nodes = [];

                    ko.utils.arrayForEach(woTypes, function (woType) {
                        let current = {
                            ID: woType.ID,
                            Name: woType.Name,
                            ClassID: module.ObjectClass.WorkOrderType,
                            Selectable: true,
                            Children: [],
                        };

                        let parent = findParent(current, uaTypes);
                        while (parent) {
                            parent.Children.push(current);
                            current = parent;
                            parent = findParent(parent, uaTypes);
                        }

                        nodes.push(current);
                    });

                    return nodes;
                };
            },
            
            Creator: function (classId, id) {
                return module.NodeViewModelCreator(id);
            },
            NodeViewModelCreator: function (data, parent) {
                return new module.NodeViewModel(data, parent);
            },

            NodeViewModel: function (data, parent) {
                var self = this;

                let children = data.Children && data.Children.length > 0 ? [ new module.WorkOrderTypesTreeCollection() ] : [];

                tree.DataNodeViewModel.call(self, null, data.ID, data.ClassID, parent, children);

                self.setData = function(dataItem) {
                    self.text(dataItem.Name);
                };

                self._getIconCss = function () {
                    return 'treeNodeIcon-none';
                };

                self.selectable(data.Selectable);
                
                if (data.Children && data.Children.length > 0) {
                    let collection = self.children[0];
                    let creator = collection.creator;

                    ko.utils.arrayForEach(data.Children, function (child) {
                        let childNode = creator(child, self);
                        childNode.setData(child);
                        self.nodes.push(childNode);
                    });
                }
            },

            WorkOrderTypesTreeCollection: function () {
                var self = this;

                tree.TreeNodeCollectionBase.call(self, null, module.NodeViewModelCreator, null);

                self.isLoaded(true);
            },

            WorkOrderTypesTreeViewModel: function () {
                var self = this;

                let root = new module.WorkOrderTypesTreeCollection();

                tree.TreeViewModelBase.call(self, null, root, module.Creator);

                self.Load = function (region) {
                    let retD = $.Deferred();
                    let fetcher = new module.Fetcher(region);
                    let treeBuilder = new module.TreeBuilder();

                    $.when(fetcher.getWorkOrderTypes(), fetcher.getUserActivityTypes()).done(function (woTypes, uaTypes) {
                        let treeNodes = treeBuilder.build(woTypes, uaTypes);

                        // добавляем загруженные ноды первого уровня
                        ko.utils.arrayForEach(treeNodes, function (nodeData) {
                            let node = self.children.creator(nodeData, self);
                            node.setData(nodeData);
                            self._nodeAdding(node);
                            self.executeFilter(node);
                            self.children.nodes.push(node);
                            self.nodes.removeAll();
                        });

                        ko.utils.arrayForEach(self.children.nodes, function (node) {
                            self.nodes.push(node);
                        });

                        self.forEachNode(function (node) {
                            node.selected.subscribe(function (selected) {
                                if (selected) {
                                    let currentSelectedNode = self.selectedNode();
                                    self.selectedNode(node);
                                    if (currentSelectedNode) {
                                        currentSelectedNode.selected(false);
                                    }
                                }
                            });
                        });
                        self.expandAll();
                        retD.resolve();
                    });

                    return retD.promise();
                };
            },

            Fetcher: function (region) {
                var self = this;

                self.getWorkOrderTypes = function () {
                    let woTypesD = $.Deferred();
                    new ajaxLib.control().Ajax(
                        $(region),
                        {
                            dataType: 'json',
                            method: 'GET',
                            url: '/api/WorkOrderTypes',
                            contentType: 'application/json',
                        },
                        function (response) { woTypesD.resolve(response); },
                        function () { woTypesD.resolve([]); },
                    );
                    return woTypesD.promise();
                };

                let requestInfo = { ReferencedObjectClasses: [ module.ObjectClass.WorkOrderType ], };
                self.getUserActivityTypes = function () {
                    let uaTypesD = $.Deferred();
                    new ajaxLib.control().Ajax(
                        $(region),
                        {
                            dataType: 'json',
                            method: 'GET',
                            url: '/api/UserActivityTypes',
                            contentType: 'application/json',
                            data: JSON.stringify(requestInfo),
                        },
                        function (response) { uaTypesD.resolve(response); },
                        function () { uaTypesD.resolve([]); },
                    );
                    return uaTypesD.promise();
                };
            },
        };
        return module;
    }
);