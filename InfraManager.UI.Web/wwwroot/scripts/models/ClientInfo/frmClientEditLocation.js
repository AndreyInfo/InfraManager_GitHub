define(['knockout', 'jquery', 'ajax', 'formControl', 'restApiTree', 'workplaceStructureTree', 'usualForms'],
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
                    'WorkplaceSearcher',
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
                if (options && options.ServicePlaceID && options.ServicePlaceClassID) { // если на вход передали ID рабочего места
                    self.tree.selectNode(options.ServicePlaceID, options.ServicePlaceClassID); // ищем его в дереве и выделяем
                }

                ko.utils.arrayForEach(self.tree.nodes(), function (rootNode) { // идем по всем корням дерева
                    $.when(self.tree.loadChildNodes(rootNode)).done(function () { // загружаем второй уровень 
                        if (!rootNode.expanded()) {
                            rootNode.expandCollapse(); // раскрываем корень дерева если не раскрыт
                        }
                    });
                });
            }
            
            let treeOptions = {
                SelectableClasses: [
                    structureTree.ObjectClass.Workplace,
                    structureTree.ObjectClass.Room
                ],
            };
            self.tree = new structureTree.UserSelectOrganizationTreeViewModel(fetcher, treeOptions);
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
            
            //
            self.navigatorObjectID = ko.observable(null);
            self.navigatorObjectClassID = ko.observable(null);
            //
            self.navigator = null;
            self.initNavigator = function () {
                var $div = $('#' + self.frm.GetRegionID()).find('.tvWrapper');
                //
                self.navigator = new treeLib.control();
                self.navigator.init($div, 1, {
                    onClick: self.navigator_nodeSelected,
                    UseAccessIsGranted: true,
                    ShowCheckboxes: false,
                    AvailableClassArray: [29, 101, 1, 2, 3, 22],
                    ClickableClassArray: [22],
                    AllClickable: false,
                    FinishClassArray: [22],
                    Title: getTextResource('LocationCaption'),
                    WindowModeEnabled: false,
                    HasLifeCycle: false,
                    ExpandFirstNodes: true,
                });
                $.when(self.navigator.$isLoaded).done(function () {
                    $div.find('.treeControlWrapper .treeControlHeader').click();//открыть размел по местоположению
                    var WorkplaceInfo = options.WorkplaceID;
                    if (WorkplaceInfo != null) {
                        console.log(WorkplaceInfo);
                        $.when(self.navigator.OpenToNode(WorkplaceInfo, 22)).done(function (finalNode) {
                            if (finalNode && finalNode.ID == WorkplaceInfo) {
                                self.navigator.SelectNode(finalNode);
                            }
                        });
                    }
                });
            };
        },
        
        SearcherType: {
            Workplace: {
                visibleClassIDs: [
                    structureTree.ObjectClass.Owner,
                    structureTree.ObjectClass.Organization,
                    structureTree.ObjectClass.Building,
                    structureTree.ObjectClass.Floor,
                    structureTree.ObjectClass.Room,
                    structureTree.ObjectClass.Workplace
                ],
                selectableClassIDs: [structureTree.ObjectClass.Workplace],
                finishedClassIDs: [structureTree.ObjectClass.Workplace]
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
                    var treePath = vm.tree.getSelectedNodeTreePath();
                    var treePathNames = treePath.map(function(node) {
                        return node.text();
                    })
                    onSelected({ 
                        ID: selectedNode.id,
                        ClassID: selectedNode.classId,
                        IMObjID: selectedNode.IMObjID,
                        SelectedTreePathNames: treePathNames
                    });
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
                getTextResource('LocationCaption'),//caption
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