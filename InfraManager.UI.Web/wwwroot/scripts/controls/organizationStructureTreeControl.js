define(
    ['knockout', 'jquery', 'ajax', 'restApiTree', 'restApiTreeControl', 'organizationStructureTree'],
    function (ko, $, ajaxLib, tree, treeControl, orgStructureTree) {
        var module = {
            Filters: {
                'MOL': [ orgStructureTree.ObjectClass.Organization, orgStructureTree.ObjectClass.Subdivision, orgStructureTree.ObjectClass.User ],
                'Owner': [ orgStructureTree.ObjectClass.Organization ],
                'Utilizer': [ orgStructureTree.ObjectClass.Organization, orgStructureTree.ObjectClass.Subdivision, orgStructureTree.ObjectClass.User ],
            },

            Control: function (options) {
                var self = this;

                let fetcher = new tree.Fetcher('treeControlItems');
                let orgTree = options && options.ShowCheckboxes === true
                    ? new orgStructureTree.UserMultiSelectOrganizationTreeViewModel(fetcher, new orgStructureTree.Factory())
                    : new orgStructureTree.UserSelectOrganizationTreeViewModel(fetcher, new orgStructureTree.Factory());

                let caption = getTextResource('OrgStructureCaption');

                treeControl.Control.call(self, caption, orgTree, options);

                // обработчик выбора типа фильтра
                self.OnTypeSelected = function (newValue) {
                    if (!newValue || !newValue.ID) {
                        return;
                    }

                    if (newValue.ID === 'Owner') { // исключительный случай: когда даем выбирать только организации - сбросить выбор
                        const selectedNode = self.Tree.selectedNode();
                        if (selectedNode && selectedNode.classId === orgStructureTree.ObjectClass.Organization) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('FilterTreeOwnerInvalidHeader'), getTextResource('FilterTreeOwnerInvalidText'), 'warning');
                            });
                            self.Tree.deselectAll();
                        }
                    }
                    
                    self.Tree.forEachNode(function (node) {
                        node.selectable(module.Filters[newValue.ID].includes(node.classId));
                    });
                };
                
                if (options && options.EnableFilterType === true) {
                    self.FilterType = new module.FilterTypeViewModel();
                    self.FilterType.SelectedValue.subscribe(self.OnTypeSelected);
                    self.Tree.subscribe(tree.TreeViewEvents.onNodeAdded, function (sender, args) {
                        var node = args.node;
                        node.selectable(module.Filters[self.FilterType.SelectedValue().ID].includes(node.classId));
                    });
                }
            },

            FilterTypeViewModel: function () {
                var self = this;
                
                self.SelectedValue = ko.observable();
                self.DDList = [
                    { ID: 'MOL', Name: getTextResource('AssetNumber_UserName'), },
                    { ID: 'Owner', Name: getTextResource('AssetNumber_OwnerName'), },
                    { ID: 'Utilizer', Name: getTextResource('AssetNumber_UtilizerName'), },
                ];
                self.getOrgDDList = function (options) {
                    const data = self.DDList;
                    options.callback({ data: data, total: data.length });
                }
                self.SelectedValue(self.DDList[0]);
            },
        };
        return module;
    }
);