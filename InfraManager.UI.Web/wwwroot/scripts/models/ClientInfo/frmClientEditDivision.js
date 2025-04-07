define(['knockout', 'jquery', 'ajax', 'formControl', 'restApiTree', 'divisionStructureTree', 'usualForms'],
    function (ko, $, ajaxLib, formControl, tree, structureTree, fhModule) {
    
    var module = {
        ViewModel: function (options) {
            var self = this;
            //       
            self.tvSearchText = ko.observable('');
            //
            self.searchControl = null;
            self.initSearchControl = function () {
                var $frm = $('#' + self.frm.GetRegionID()).find('.frmAssetLocation');
                var searcherControlD = $.Deferred();
                //
                var fh = new fhModule.formHelper();
                var searcherLoadD = fh.SetTextSearcherToField(
                    $frm.find('.searchField__input'),
                    'SubDivisionSearcher',
                    null,
                    null,
                    function (objectInfo) {//select
                        self.tvSearchText(objectInfo.FullName);
                        self.tree.selectNode(objectInfo.ID, objectInfo.ClassID);
                    },
                    function () {//reset
                        self.tvSearchText('');
                    },
                    function (selectedItem) {//close
                        if (!selectedItem) {
                            self.tvSearchText('');
                        }
                    },
                    undefined,
                    true);
                $.when(searcherLoadD, userD).done(function (ctrl, user) {
                    searcherControlD.resolve(ctrl);
                    ctrl.CurrentUserID = user.ID;
                    self.searchControl = ctrl;
                });
            };
            var fetcher = new tree.Fetcher('.tvWrapper');
            
            function treeLoaded() {

                if (options && options.SubdivisionID) { // если на вход передали ID подразделения
                    self.tree.selectNode(options.SubdivisionID, structureTree.ObjectClass.Subdivision); // ищем его в дереве и выделяем
                }

                ko.utils.arrayForEach(self.tree.nodes(), function (rootNode) { // идем по всем корням дерева
                    $.when(self.tree.loadChildNodes(rootNode)).done(function () { // загружаем второй уровень 
                        if (!rootNode.expanded()) {
                            rootNode.expandCollapse(); // раскрываем корень дерева если не раскрыт
                        }
                    });
                });
            }

            self.tree = new structureTree.UserSelectOrganizationTreeViewModel(fetcher);
            self.tree.subscribe(tree.TreeViewEvents.onDataLoaded, treeLoaded);
            
            self.initTree = function () {
                self.tree.load();
            };
            //
            self.dispose = function () {
                //todo tv dispose
                self.searchControl.Remove();
            };
            self.afterRender = function (editor, elements) {
                self.initSearchControl();
                self.initTree();
            };
        },
        
        SearcherType: {
            SubDivision: {
                visibleClassIDs: [
                    structureTree.ObjectClass.Owner,
                    structureTree.ObjectClass.Organization,
                    structureTree.ObjectClass.Subdivision
                ],
                selectableClassIDs: [structureTree.ObjectClass.Subdivision],
                finishedClassIDs: [structureTree.ObjectClass.Subdivision]
            }
        },
        
        ShowDialog: function (options, onSelected, isSpinnerActive) {
            if (isSpinnerActive !== true) {
                showSpinner();
            }
            //
            var frm = undefined;
            var vm = new module.ViewModel(options);
            var bindElement = null;
            var handleOfSelectedNode = null;
            //
            var buttons = [];
            var bSelect = {
                text: getTextResource('Select'),
                click: function () {
                    var selectedNode = vm.tree.selectedNode();
                    onSelected({ ID: selectedNode.id, ClassID: selectedNode.classId });
                    frm.Close();
                }
            }
            var bCancel = {
                text: getTextResource('Close'),
                click: function () { frm.Close(); }
            }
            
            buttons.push(bCancel);
            //
            frm = new formControl.control(
                'region_frmClientLocation',//form region prefix
                'setting_frmClientLocation',//location and size setting
                getTextResource('Subdivision_Name'),//caption
                true,//isModal
                true,//isDraggable
                true,//isResizable
                300, 350,//minSize
                buttons,//form buttons
                function () {
                    handleOfSelectedNode.dispose();
                    vm.dispose();
                    ko.cleanNode(bindElement);
                },//afterClose function
                'data-bind="template: {name: \'../UI/Forms/Asset/frmAssetLocation\', afterRender: afterRender}"'//attributes of form region
            );
            if (!frm.Initialized) {
                return;//form with that region and settingsName was open
            }
            
            frm.ExtendSize(400, 550);//normal size
            vm.frm = frm;
            handleOfSelectedNode = vm.tree.selectedNode.subscribe(function (newValue) {
                buttons = [];
                buttons.push(bCancel);
                if (newValue != null)
                    buttons.push(bSelect);
                //
                var scrollEl = $('#' + frm.GetRegionID()).find('.tvWrapper');
                var scrollTop = scrollEl.length > 0 ? scrollEl[0].scrollTop : 0;
                frm.UpdateButtons(buttons);
                if (scrollEl.length > 0)
                    scrollEl[0].scrollTop = scrollTop;
            });
            //
            bindElement = document.getElementById(frm.GetRegionID());
            ko.applyBindings(vm, bindElement);
            //
            $.when(frm.Show()).done(function (frmD) {
                hideSpinner();
            });
        },

    };
    return module;
});