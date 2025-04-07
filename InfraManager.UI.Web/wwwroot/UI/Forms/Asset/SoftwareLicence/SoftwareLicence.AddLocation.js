define(
    ['jquery', 'knockout', 'formControl', 'treeViewModel', 'checkbox', 'navigatorTreeDataProvider', 'usualForms'],
    function ($, ko, formModule, tree, checkbox, dataProvider, fhModule) {
        var module = {
            ViewModel: function (initialData) {
                var self = this;

                // After Render
                {
                    self.afterRender = function () {
                        self.locations.setData(initialData);
                        self.locations.load();
                        initSearcher();
                    };
                }

                // Dispose
                {
                    self.dispose = function () {

                    };
                }

                // Locations tree
                {
                    var availableLocationClasses = [1, 101, 29];
                    var locationsDataProvider =
                        new dataProvider.DataProvider({
                            type: 1,
                            AvailableCategoryID: null,
                            UseRemoveCategoryClass: null,
                            RemovedCategoryClassArray: [],
                            AvailableTypeID: null,
                            AvailableTemplateClassID: null,
                            AvailableTemplateClassArray: [],
                            HasLifeCycle: true,
                            CustomControlObjectID: null,
                            SetCustomControl: null,
                            AvailableClassArray: availableLocationClasses,
                            UseAccessIsGranted: true,
                            OperationsID: [],
                            region: '.software-licence-select-locations'
                        });

                    var nodeViewModel = function (dataItem, parent, config) {
                        var self = this;
                        tree.DataNodeViewModel.call(self, dataItem, parent, config);

                        self.expandable(self.classId !== 1);
                        self.expanded(self.expandable());
                    };

                    var treeViewModel = function () {
                        var self = this;

                        tree.MultiSelectTreeViewModel.call(
                            self,
                            tree.TreeViewModelBase,
                            locationsDataProvider,
                            nodeViewModel,
                            checkbox.ViewModel);

                        var baseNodeAdding = self._nodeAdding;
                        self._nodeAdding = function (node) {
                            baseNodeAdding(node);

                            if (node.expandable()) {
                                $.when(self.loadChildNodes(node)).done(function () {
                                    if (node.classId === 101) { // org
                                        ko.utils.arrayForEach(node.nodes(), function (child) {
                                            if (self.inData(child)) {
                                                child.checkbox.check();
                                            }
                                        });
                                    }
                                });
                            }
                        }
                    }

                    self.locations = new treeViewModel();

                    self.getSelectedItems = function () {
                        var result = [];
                        self.locations.forEachNode(function (node) {
                            if (node.checkbox.isChecked() && node.classId === 1) {
                                result.push({ ID: node.id, ClassID: node.classId });
                            }
                        });

                        return result;
                    }
                }

                // Location search
                {
                    self.searchText = ko.observable('');
                    self.searcher = null;
                    var currentNode = null;

                    function initSearcher() {
                        //
                        var fh = new fhModule.formHelper();
                        var searcherLoadD = fh.SetTextSearcherToField(
                            $('#location-select .searchField input'),
                            'BuildingLocationSearcher',
                            null,
                            null,
                            function (objectInfo) {//select
                                if (availableLocationClasses.indexOf(objectInfo.ClassID) === -1) {
                                    return;
                                }

                                self.searchText(objectInfo.FullName);

                                $.when(locationsDataProvider.getPath(objectInfo.ID, objectInfo.ClassID))
                                    .done(function (path) {
                                        self.locations.forEachNode(function (node) {
                                            if (node.id === objectInfo.ID && node.classId === objectInfo.ClassID) {
                                                if (currentNode) {
                                                    currentNode.selected(false);
                                                }
                                                node.selected(true);
                                                node.expandParent();
                                                currentNode = node;
                                            }
                                        });
                                    });                                
                            },
                            function () {//reset
                                if (currentNode) {
                                    currentNode.selected(false);
                                }
                            },
                            function (selectedItem) {//close
                            },
                            undefined,
                            true);
                        $.when(searcherLoadD, userD).done(function (ctrl, user) {
                            ctrl.CurrentUserID = user.ID;
                            self.searcher = ctrl;
                        });
                    };
                }
            },
            ShowDialog: function (callback, initialData) {
                var form;
                var viewModel = new module.ViewModel(initialData);
                var bindElement = null;

                var buttons = [
                    { text: getTextResource('Select'), click: function () { callback(viewModel.getSelectedItems()); form.Close(); } },
                    { text: getTextResource('Close'), click: function () { form.Close(); } }];
                //
                form = new formModule.control(
                    'region_frmAssetLocation',//form region prefix
                    'setting_frmAssetLocation',//location and size setting
                    getTextResource('Asset_Location'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    300, 350,//minSize
                    buttons,//form buttons
                    function () {
                        viewModel.dispose();
                        ko.cleanNode(bindElement);
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareLicence/SoftwareLicence.AddLocation\', afterRender: afterRender}"'//attributes of form region
                );
                if (!form.Initialized) {
                    return null;
                }

                form.ExtendSize(400, 550);//normal size
                bindElement = document.getElementById(form.GetRegionID());
                ko.applyBindings(viewModel, bindElement);
                //
                $.when(form.Show()).done(function (frmD) {
                    hideSpinner();
                });
            }
        };

        return module;
    });