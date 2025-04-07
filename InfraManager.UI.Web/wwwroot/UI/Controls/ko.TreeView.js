define(['knockout', 'jquery'], function (ko, $) {
    (function () {
        var settings = {
            template: '../UI/Controls/ko.TreeView',                     //template of control
            emptyMessage: ko.observable(getTextResource('ListIsEmpty')),//message when nodes count is zero
            //
            controlInitialized: ko.observable(                          //raise after control initialized, to get control viewModel
                function (treeView) {
                }),
            selectedNodeChanged: ko.observable(                         //func when selection changed
                function (treeNode) {
                })
        };

        /*
            element - The DOM element involved in this binding
            valueAccessor - A JavaScript function that you can call to get the current model property that is involved in this binding. Call valueAccessor() to get the current model property value.
            allBindings -  A JavaScript object that you can use to access all the model values bound to this DOM element. Call allBindings.get('name') to retrieve the value of the name binding (returns undefined if the binding doesn’t exist);
            viewModel - This parameter is deprecated in Knockout 3.x. Use bindingContext.$data or bindingContext.$rawData to access the view model instead.
            bindingContext - An object that holds the binding context available to this element’s bindings. This object includes special properties including $parent, $parents, and $root that can be used to access data that is bound against ancestors of this context.
        */
        ko.bindingHandlers.koTreeView = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                if (element.id == '')
                    element.id = 'koTreeView_' + ko.getNewID();
                //
                var options = ko.utils.unwrapObservable(valueAccessor());
                for (var key in settings)
                    if (options[key] === undefined)
                        options[key] = ko.cloneVariable(settings[key]);
                    else if (ko.isObservable(settings[key]) && !ko.isObservable(options[key])) //make ko-value
                        options[key] = ko.observable(options[key]);
                //
                var vm = new ko.bindingHandlers.koTreeView.ViewModel(options, element);
                var vm_dispose = vm.dispose;
                vm.dispose = function () {
                    vm_dispose();
                    ko.controls[element.id] = undefined;
                };
                //
                if (!ko.controls)
                    ko.controls = {};
                if (ko.controls[element.id] && ko.controls[element.id].dispose)
                    ko.controls[element.id].dispose();
                ko.controls[element.id] = vm;
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    vm.dispose();
                });
                //
                ko.applyBindingsToNode(element, { template: { name: options.template } }, vm);
                //tell KO *not* to bind the descendants itself, otherwise they will be bound twice                
                return { controlsDescendantBindings: true };
            }
        };

        ko.bindingHandlers.koTreeView.TreeNode = function (vm, parentNode) {
            var self = this;
            //
            self.isExpanded = ko.observable(false);
            self.name = ko.observable('');
            self.parentNode = parentNode;
            //
            self.nodes = ko.observableArray([]);//array of TreeNode        
            //
            self.addNode = function (item, itemFill_func) {
                vm.addNodeFunc(self.nodes, item, itemFill_func, self);
            };
            self.sort = function () {
                vm.sortNodesFunc(self.nodes);
            };
            //
            self.domElement = ko.observable(null);
            //
            self.dispose = function () {
                ko.utils.arrayForEach(self.nodes(), function (node) { node.dispose(); });
                self.nodes([]);
            };
        };

        ko.bindingHandlers.koTreeView.ViewModel = function (options, element) {
            var self = this;
            self.options = options;
            //
            {//nodes, selectedNode
                self.nodes = ko.observableArray([]);//array of TreeNode
                //
                self.selectedNode = ko.observable(null);
                self.selectedNode_handler = self.selectedNode.subscribe(function (newValue) {
                    self.options.selectedNodeChanged()(newValue);
                });
                //for keyboard manage
                self.openedNodes = ko.pureComputed(function () {
                    var retval = [];
                    var walk = null;
                    walk = function (items) {
                        for (var i = 0; i < items.length; i++) {
                            var item = items[i];
                            retval.push(item);
                            if (item.isExpanded() == true)
                                walk(item.nodes());
                        }
                    };
                    walk(self.nodes());
                    return retval;
                });
            }
            //
            {//ui methods - click, keydown
                self.toggleExpander = function (treeNode, element) {
                    treeNode.isExpanded(!treeNode.isExpanded());
                };
                self.nodeClick = function (treeNode, element) {
                    self.selectedNode(treeNode);
                };

                self.keyDown = function (vm, e) {
                    var k = e.which || e.keyCode;
                    var ensureVisible = function (scroll) {
                        if (self.selectedNode() != null && self.selectedNode().domElement() != null) {
                            self.selectedNode().domElement().focus();
                            if (scroll === true)
                                self.selectedNode().domElement().scrollIntoView(false);
                        }
                    };
                    switch (k) {
                        case 36: {//36 - home
                            var list = self.openedNodes();
                            if (list.length == 0)
                                return;
                            self.selectedNode(list[0]);
                            ensureVisible(true);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 38: {//38 - up        
                            var list = self.openedNodes();
                            if (list.length == 0)
                                return;
                            var index = list.indexOf(self.selectedNode());
                            if (index > 0) {
                                self.selectedNode(list[index - 1]);
                                ensureVisible(true);
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 33: {//38 - pageUp
                            var list = self.openedNodes();
                            if (list.length == 0)
                                return;
                            var index = self.selectedNode() ? list.indexOf(self.selectedNode()) : 0;
                            if (index > 0) {
                                self.selectedNode(list[Math.max(0, index - 10)]);
                                ensureVisible(true);
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 40: {//40 - down
                            var list = self.openedNodes();
                            if (list.length == 0)
                                return;
                            var index = self.selectedNode() ? list.indexOf(self.selectedNode()) : 0;
                            if (index < list.length - 1) {
                                self.selectedNode(list[index + 1]);
                                ensureVisible(true);
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 34: {//34 - pagedown
                            var list = self.openedNodes();
                            if (list.length == 0)
                                return;
                            var index = self.selectedNode() ? list.indexOf(self.selectedNode()) : 0;
                            if (index < list.length - 1) {
                                self.selectedNode(list[Math.min(index + 10, list.length - 1)]);
                                ensureVisible(true);
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 35: {//35 - end
                            var list = self.openedNodes();
                            if (list.length == 0)
                                return;
                            self.selectedNode(list[list.length - 1]);
                            ensureVisible(true);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 37: {//37 - left
                            var selNode = self.selectedNode();
                            if (selNode == null)
                                return;
                            if (selNode.isExpanded() == true)
                                selNode.isExpanded(false);
                            else if (selNode.parentNode != null) {
                                self.selectedNode(selNode.parentNode);
                                ensureVisible(true);
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 39: {//39 - right
                            var selNode = self.selectedNode();
                            if (selNode == null || selNode.nodes().length == 0)
                                return;
                            if (selNode.isExpanded() == false)
                                selNode.isExpanded(true);
                            else if (selNode.nodes().length > 0) {
                                self.selectedNode(selNode.nodes()[0]);
                                ensureVisible(true);
                            }
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 116: {//f5
                            window.location.reload(true);
                            //
                            e.stopPropagation();
                            break;
                        }
                    }
                    return false;
                };
            }
            //
            {//manage
                self.addNodeFunc = function (ko_array, item, itemFill_func, parentNode) {
                    var tn = new ko.bindingHandlers.koTreeView.TreeNode(self, parentNode);
                    itemFill_func(tn, item);
                    ko_array.push(tn);
                };
                self.addNode = function (item, itemFill_func) {
                    self.addNodeFunc(self.nodes, item, itemFill_func, null);
                };
                self.clear = function () {
                    ko.utils.arrayForEach(self.nodes(), function (node) { node.dispose(); });
                    self.nodes([]);
                    //
                    self.selectedNode(null);
                };
                //
                self.sortNodesFunc = function (ko_array) {
                    var list = ko_array.sort(function (n1, n2) {
                        var text1 = n1.name();
                        var text2 = n2.name();
                        //
                        return text1 == text2 ? 0 : (text1 < text2 ? -1 : 1);
                    })();
                    ko.utils.arrayForEach(list, function (node) { node.sort(); });
                    ko_array(list);
                    ko_array.valueHasMutated();
                };
                self.sort = function () {
                    self.sortNodesFunc(self.nodes);
                };
            }
            //
            self.dispose = function () {
                self.clear();
                self.selectedNode_handler.dispose();
            };
            //
            //return link to control
            self.options.controlInitialized()(self);
        };
    }());
});