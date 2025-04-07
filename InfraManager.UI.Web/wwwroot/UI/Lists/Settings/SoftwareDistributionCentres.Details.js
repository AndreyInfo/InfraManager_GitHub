define(['knockout', 'jquery', 'ajax', 'treeViewModel', 'checkbox', 'navigatorTreeDataProvider', 'ui_lists/Settings/SoftwareDistributionCentres.TreeFilter', 'ui_lists/Settings/SoftwareDistributionCentres.ResponsiblePerson', 'ui_lists/Asset/AssetTabs', 'ui_lists/Asset/LicenceList'], 
    function (ko, $, ajaxLib, tree, checkbox, dataProvider, treeFilter, responsiblePersonModule, assetTabs, licenceList) {
    var module = {
        ViewModel: function (listItem) {
            var self = this;
            self.ajaxControl = new ajaxLib.control();

            function setDefaultFocus() {
                $('.sdc-name-editor').focus();
            }

            self.init = function () {
                updateTabsReadonly();
                self.referencesRecipients.subscribe(tree.TreeViewEvents.onDataLoaded,
                    function () {
                        if (self.referencesRecipients.nodes().length > 0) {
                            self.referencesRecipients.nodes()[0].expanded(true);
                        }
                    });
                self.distributedSoftware.subscribe(tree.TreeViewEvents.onDataLoaded,
                    function () {
                        if (self.distributedSoftware.nodes().length > 0) {
                            self.distributedSoftware.nodes()[0].expanded(true);
                        }
                    });
                self.referencesRecipients.load();
                self.distributedSoftware.load();
                if (listItem.ResponsiblePerson.ID) {
                    self.responsiblePerson.set(listItem.ResponsiblePerson.ID, listItem.ResponsiblePerson.ClassID);
                }

                self.getData();
                if (self.isNew()) {
                    setDefaultFocus();
                }
            };

            // data fields
            {
                self.id = listItem.ID;
                self.name = ko.observable(listItem.Name);
                self.note = ko.observable(listItem.Note || '');
                self.rowVersion = ko.observable();
                self.canEditDistributedSoftware = listItem.CanEditDistributedSoftware || false;
                self.canEditReferencesRecipients = listItem.CanEditReferencesRecipients || false;
                self.nameEditable = listItem.CanEditName || true;
            }

            // view / edit state
            {
                self.isNew = ko.observable(!self.id);
                self.editing = ko.observable(self.isNew());
                self.modifying = ko.pureComputed(function () {
                    return !self.isNew() && self.editing();
                });
                self.editing.subscribe(function () {
                    updateTabsReadonly();
                });
                self.nameEditing = ko.pureComputed(function () {
                    return self.editing() && self.nameEditable;
                });
                self.nameViewing = ko.pureComputed(function () {
                    return !self.nameEditing();
                });
                self.readonly = ko.pureComputed(function () {
                    return !self.editing();
                });

                self.allowEdit = ko.observable(self.isNew() || (listItem.AllowEdit && listItem.CanBeEdited));
                self.editVisible = ko.pureComputed(function () {
                    return self.readonly() && self.allowEdit();
                });

                self.onCancel;
                self.cancel = function () {
                    if (self.id) {
                        self.editing(false);
                        self.getData();
                    } else if (typeof self.onCancel === 'function') {
                        self.onCancel();
                    } else {
                        self.name('');
                        self.note('');
                    }
                };
                self.edit = function () {
                    if (self.allowEdit()) {
                        self.editing(true);
                    }
                }
            }

            // responsible person
            {
                self.responsiblePerson = new responsiblePersonModule.ViewModel(self.editing);
            }

            // get data
            {
                function updateModel(data) {
                    self.name(data.Name);
                    self.note(data.Note);
                    self.rowVersion(data.RowVersion);
                    self.referencesRecipients.setData(data.ReferencesRecipients);
                    self.distributedSoftware.setData(data.DistributedSoftware);
                    self.canEditDistributedSoftware = data.CanEditDistributedSoftware;
                    self.canEditReferencesRecipients = data.CanEditReferencesRecipients;
                    self.nameEditable = data.CanEditName;
                    self.responsiblePerson.set(data.ResponsiblePerson.ID, data.ResponsiblePerson.ClassID);
                    updateTabsReadonly();
                };

                function updateTabsReadonly() {
                    self.referencesRecipients.readonly(!(self.editing() && self.canEditReferencesRecipients));
                    self.distributedSoftware.readonly(!(self.editing() && self.canEditDistributedSoftware));
                }

                self.getData = function () {

                    if (self.isNew()) {
                        return;
                    }

                    self.ajaxControl.Ajax($('.b-content-dictionaries-container'), {
                            dataType: "json",
                            method: 'GET',
                            url: '/assetApi/SoftwareDistributionCentres/' + self.id
                        },
                        function (response) {
                            if (response && response.Result === 0 && response.Data) {
                                var data = response.Data;

                                self.initialData = data.ObjectData;
                                self.allowEdit(data.AllowEdit && data.CanBeEdited);
                                updateModel(self.initialData);
                            } else if (response && response.Result === 1)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[SoftwareDistributionCentres.Details.js, getData]', 'error');
                                });
                            else if (response && response.Result === 2)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[SoftwareDistributionCentres.Details.js, getData]', 'error');
                                });
                            else if (response && response.Result === 3)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                });
                            else if (response && response.Result === 7) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                                });
                            } else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[SoftwareDistributionCentres.Details.js, getData]', 'error');
                                });
                            }
                        });
                };
            }

            // save data
            {
                function toData() {
                    return {
                        Name: self.name(),
                        Note: self.note(),
                        ResponsiblePerson: self.responsiblePerson.get(),
                        RowVersion: self.rowVersion(),
                        ReferencesRecipients: self.referencesRecipients.getData(),
                        DistributedSoftware: self.distributedSoftware.getData()
                    };
                };

                self.changesSaved = null;

                self.save = function () {

                    if (!self.name() || self.name().trim().length === 0) {
                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('SoftwareDistributionCentre_Details_MissingTitle'),
                                text: '',
                                showCancelButton: false,
                                closeOnConfirm: false,
                                confirmButtonText: getTextResource('ButtonOK'),
                            }, function () {
                                swal.close();
                                setDefaultFocus();
                            });
                        });
                        return;
                    }

                    self.ajaxControl.Ajax($('.b-content-dictionaries-container'), {
                            dataType: "json",
                            data: toData(),
                            method: self.isNew() ? 'POST' : 'PUT',
                            url: '/assetApi/SoftwareDistributionCentres/' + (self.isNew() ? '' : self.id)
                        },
                        function (response) {
                            if (response && response.Result === 0) {

                                if (!self.id) {
                                    self.id = response.ID;
                                    self.isNew(false);
                                    self.initialData = toData();
                                    initSublicences();
                                }

                                if (typeof self.changesSaved === 'function') {
                                    self.changesSaved();
                                }

                                self.editing(false);
                                self.getData();
                            } else if (response && response.Result === 1)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[SoftwareDistributionCentres.Details.js, getData]', 'error');
                                });
                            else if (response && response.Result === 2)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[SoftwareDistributionCentres.Details.js, getData]', 'error');
                                });
                            else if (response && response.Result === 3)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                });
                            else if (response && response.Result === 7) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                                });
                            } else if (response && response.Result === 12) {
                                require(['sweetAlert'], function (swal) {
                                    swal({
                                        title: getTextResource('SaveError'),
                                        text: getTextResource('SoftwareDistributionCentre_Details_DuplicateTitle').replace("{0}", self.name()),
                                        showCancelButton: false,
                                        closeOnConfirm: false,
                                        confirmButtonText: getTextResource('ButtonOK'),
                                    }, function () {
                                        swal.close();
                                        setDefaultFocus();
                                    });
                                });
                            } else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[SoftwareDistributionCentres.Details.js, getData]', 'error');
                                });
                            }
                        });
                };
            }

            // detect changes
            {
                self.initialData = {};

                function diffSelection(fromModel, fromTree) {
                    var onlySelected = ko.utils.arrayFilter(fromModel, function (item) {
                        return !item.PartiallySelected;
                    });

                    if (onlySelected.length !== fromTree.length) {
                        return true;
                    }

                    var obj = {};
                    ko.utils.arrayForEach(onlySelected, function (item) {
                        obj[item.ClassID] = obj[item.ClassID] || {};
                        obj[item.ClassID][item.ID] = item;
                    });

                    var hasChanges = false;
                    ko.utils.arrayForEach(fromTree, function (treeItem) {
                        hasChanges = hasChanges || !obj[treeItem.ClassID] || !obj[treeItem.ClassID][treeItem.ID];
                    });

                    return hasChanges;
                }

                self.hasChanges = function () {
                    if (!self.editing()) {
                        return false;
                    }

                    return self.isNew()
                        || self.name() !== self.initialData.Name
                        || self.note() !== self.initialData.Note
                        || diffSelection(self.initialData.DistributedSoftware, self.distributedSoftware.getData())
                        || diffSelection(self.initialData.ReferencesRecipients, self.referencesRecipients.getData())
                        || self.responsiblePerson.hasChanges(self.initialData.ResponsiblePerson);
                };
            }

            // tabs
            {
                var tabs = {
                    distributedSoftware: 1,
                    referencesRecipients: 2,
                    sublicenses: 3
                };

                self.activeTab = ko.observable(tabs.distributedSoftware);
                self.distributedSoftwareActive = ko.pureComputed(function () {
                    return self.activeTab() === tabs.distributedSoftware;
                });
                self.referencesRecipientsActive = ko.pureComputed(function () {
                    return self.activeTab() === tabs.referencesRecipients;
                });
                self.sublicensesActive = ko.pureComputed(function () {
                    return self.activeTab() === tabs.sublicenses
                })

                self.activateDistributedSoftware = function () {
                    self.activeTab(tabs.distributedSoftware);
                };
                self.activateReferencesRecipients = function () {
                    self.activeTab(tabs.referencesRecipients);
                };

                self.activateSublicenses = function () {
                    self.activeTab(tabs.sublicenses);
                };

                self.sublicenceTabVisible = ko.pureComputed(function () {
                    return !self.isNew();
                });
            }

            // references recipients
            {
                var referencesRecipientsDataProvider =
                    new dataProvider.DataProvider({
                        type: 0,
                        AvailableCategoryID: null,
                        UseRemoveCategoryClass: null,
                        RemovedCategoryClassArray: [],
                        AvailableTypeID: null,
                        AvailableTemplateClassID: null,
                        AvailableTemplateClassArray: [],
                        HasLifeCycle: true,
                        CustomControlObjectID: null,
                        SetCustomControl: null,
                        AvailableClassArray: [29, 101, 102], // TODO: refactor navigator API or switch to alternative api resource or inject constants from web server to avoid "magic numbers"
                        UseAccessIsGranted: true,
                        OperationsID: [],
                        region: '.referenses-recipients-tab-content'
                    });

                self.referencesRecipients = new tree.MultiSelectTreeViewModel(
                    tree.LazyLoadingTreeViewModel,
                    referencesRecipientsDataProvider,
                    tree.DataNodeViewModel,
                    checkbox.ViewModel);

                self.referencesRecipientsFilter = new treeFilter.TabViewModel(
                    self.referencesRecipients,
                    referencesRecipientsDataProvider, {
                        inputXPath: '#sdc-referencesRecipients-filter .searchField input',
                        name: 'ReferencesRecipientSearcher'
                    });

                self.referencesRecipientsRendered = function () {
                    self.referencesRecipientsFilter.initSearcher();
                }
            }

            // distributed software
            {

                var distributedSoftwareDataProvider =
                    new dataProvider.DataProvider({
                        type: 4,
                        AvailableCategoryID: null,
                        UseRemoveCategoryClass: null,
                        RemovedCategoryClassArray: [],
                        AvailableTypeID: null,
                        AvailableTemplateClassID: null,
                        AvailableTemplateClassArray: [],
                        HasLifeCycle: true,
                        CustomControlObjectID: null,
                        SetCustomControl: null,
                        AvailableClassArray: [29, 92, 97], // TODO: refactor navigator API or switch to alternative API resource or inject constants from web server to avoid "magic numbers"
                        UseAccessIsGranted: true,
                        OperationsID: [],
                        region: '.distributed-software-tab-content'
                    });

                var distributedSoftwareNodeViewModel = function (dataItem, parent, config) {
                    tree.DataNodeViewModel.call(this, dataItem, parent, config);
                    this.expandable(dataItem.ClassID !== 97);
                };

                self.distributedSoftware = new tree.MultiSelectTreeViewModel(
                    tree.LazyLoadingTreeViewModel,
                    distributedSoftwareDataProvider,
                    distributedSoftwareNodeViewModel,
                    checkbox.ViewModel);

                self.distributedSoftwareFilter = new treeFilter.TabViewModel(
                    self.distributedSoftware,
                    distributedSoftwareDataProvider, {
                        inputXPath: '#sdc-distributedSoftware-filter .searchField input',
                        name: 'SoftwareCatalogSearcher'
                    });

                self.distributedSoftwareRendered = function () {
                    self.distributedSoftwareFilter.initSearcher();
                };

            }

            // sublicenses
            {
                self.SubLicensesTabs = ko.observableArray([]);

                var initTabs = [new assetTabs.tab("Grouped_into_pools", "SoftwareLicenseDistribution", 1),
                    new assetTabs.tab("NoGrouping", "SubSoftwareLicense", 2)];

                self.SubLicensesTabs(initTabs);
                self.activeSubLicensesTab = ko.observable();
                self.activeSubLicensesTab(initTabs[0]);
                self.viewName = ko.observable(self.activeSubLicensesTab().ViewName);

                self.SetActive = function (tab, event) {
                    self.activeSubLicensesTab(tab);
                    self.viewName(tab.ViewName);

                    self.list.reload();
                };
                self.list = new licenceList.List(self, self.id, 23);

                self.list.SelectedItemsChanged = function (checkedItemsCount) {

                }
                //
                self.list.listViewRowClick = function (obj) {

                };

                function initSublicences() {
                    self.list.parentID = self.id;
                }
            }
        }
    };

    return module;
});