define(
    ['knockout', 'jquery', 'formControl', 'restApiTree', 'organizationStructureTree', 'usualForms'],
    function (ko, $, formControl, tree, orgStructureTree, fhModule) {
        var module = {
            CustomControlOrganizationStructureTreeNodeFactory: function (objectID, objectClassID, controlValue) {
                orgStructureTree.Factory.call(this);
                var baseGetUsersUri = this._getUsersUri;

                this._getUsersUri = function (subdivisionID) {
                    return baseGetUsersUri(subdivisionID)
                        + '&controlsObjectID=' + objectID
                        + '&controlsObjectClassID=' + objectClassID
                        + '&controlsObjectValue=' + !controlValue;
                }
            },
            ViewModel: function (userInfo) {
                var self = this;
                //       
                self.tvSearchText = ko.observable('');
                //
                self.userSearcher = null;
                self.initUserSearcherControl = function () {
                    var $frm = $('#' + self.frm.GetRegionID()).find('.frmUserSearch');
                    var searcherControlD = $.Deferred();
                    //
                    var fh = new fhModule.formHelper();
                    var searcherLoadD = fh.SetTextSearcherToField(
                        $frm.find('.userSearcher'),
                        userInfo.UseTOZ == false ? 'WebUserSearcherNoTOZ' : 'WebUserSearcher',
                        null,
                        userInfo.CustomControlObjectID ? {
                            ControlsObjectID: userInfo.CustomControlObjectID,
                            ControlsObjectClassID: userInfo.CustomControlObjectClassID,
                            ControlsObjectValue: !userInfo.SetCustomControl
                        } : null,
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
                        self.userSearcher = ctrl;
                    });
                };
                //

                var fetcher = new tree.Fetcher('.tvWrapper');

                function treeLoaded() {

                    if (userInfo && userInfo.UserID) { // если на вход передали ID пользователя
                        self.tree.selectNode(userInfo.UserID, orgStructureTree.ObjectClass.User); // ищем его в дереве и выделяем
                    }

                    ko.utils.arrayForEach(self.tree.nodes(), function (rootNode) { // идем по всем корням дерева
                        $.when(self.tree.loadChildNodes(rootNode)).done(function () { // загружаем второй уровень 
                            if (!rootNode.expanded()) {
                                rootNode.expandCollapse(); // раскрываем корень дерева если не раскрыт
                            }
                        });
                    });

                    if (userInfo.CustomControlObjectID && !userInfo.SetCustomControl) // Если нужно снять контроль с каких-то пользователей, то найдем их всех и покажем
                        $.when(fetcher.get('/api/users?controlsObjectID=' + userInfo.CustomControlObjectID
                            + '&controlsObjectClassID=' + userInfo.CustomControlObjectClassID
                            + '&controlsObjectValue=true'))
                            .done(function (users) {
                                ko.utils.arrayForEach(users, function (user) {
                                    self.tree.findNode(user.ID, orgStructureTree.ObjectClass.User);
                                });
                        });
                }

                var nodeFactory = userInfo.CustomControlObjectID
                    ? new module.CustomControlOrganizationStructureTreeNodeFactory(userInfo.CustomControlObjectID, userInfo.CustomControlObjectClassID, userInfo.SetCustomControl)
                    : new orgStructureTree.Factory();               

                self.tree = userInfo.ShowCheckboxes
                    ? new orgStructureTree.UserMultiSelectOrganizationTreeViewModel(fetcher, nodeFactory)
                    : new orgStructureTree.UserSelectOrganizationTreeViewModel(fetcher, nodeFactory);
                self.tree.subscribe(tree.TreeViewEvents.onDataLoaded, treeLoaded);

                self.initTree = function () {
                    self.tree.load();
                };
                //
                self.dispose = function () {
                    //todo tv dispose
                    self.userSearcher.Remove();
                };
                self.afterRender = function (editor, elements) {
                    self.initUserSearcherControl();
                    self.initTree();
                };
        },

            SearcherType: {
                User: { visibleClassIDs: [29, 101, 102, 9], selectableClassIDs: [9], finishedClassIDs: [9] },
            },

            ShowDialog: function (userInfo, onSelected, isSpinnerActive) {//userInfo : {UserID, CustomControlObjectID}; CustomControlObjectID - если задано, то отображаем лишь пользователей, у которых данный объект на контроле
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var vm = new module.ViewModel(userInfo);
                var bindElement = null;
                var handleOfSelectedNode = null;
                //
                var buttons = [];
                var bSelect = {
                    text: getTextResource('Select'),
                    'Class': 'btnVisibility',
                    click: function () {
                        if (userInfo.ShowCheckboxes) {
                            onSelected(
                                $.map(
                                    vm.tree.getSelectedNodes(),
                                    function (node) {
                                        return { ID: node.id, ClassID: node.classId, FullName: node.text() }
                                    }));
                        } else {
                            var selectedNode = vm.tree.selectedNode();
                            onSelected({ ID: selectedNode.id, ClassID: selectedNode.classId, FullName: selectedNode.text() });
                        }
                        frm.Close();
                    }
                }
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { frm.Close(); }
                }
                buttons.push(bSelect);
                buttons.push(bCancel);
                //
                var caption = userInfo.Title ? userInfo.Title : getTextResource('User');
                frm = new formControl.control(
                        'region_frmUserSearch',//form region prefix
                        'setting_frmUserSearch',//location and size setting
                        getTextResource('SDEditorCaption') + "'" + caption + "'",//caption
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
                        'data-bind="template: {name: \'../UI/Forms/User/frmUserSearch\', afterRender: afterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                frm.ExtendSize(400, 550);//normal size
                vm.frm = frm;
                handleOfSelectedNode = vm.tree.selectedNode.subscribe(function (newValue) {
                    var scrollEl = $('#' + frm.GetRegionID()).find('.users-tree');
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