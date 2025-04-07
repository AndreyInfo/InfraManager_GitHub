define(['knockout',
    'jquery',
    'ajax',
    'iconHelper',
    'selectControl',
    'productCatalogTree',
    'workplaceStructureTree',
    'organizationStructureTreeControl',
    'ui_controls/ListView/ko.ListView.Cells',
    'ui_controls/ListView/ko.ListView.Helpers',
    'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView',
    'ui_controls/ContextMenu/ko.ContextMenu'],
    function (ko,
        $,
        ajaxLib,
        ihLib,
        scLib,
        productCatalogTree,
        locationTree,
        orgStructureTree,
        m_cells,
        m_helpers,
        m_lazyEvents) {
        var module = {
            ObjectClasses: {
                Building:     {ClassId:   1, getText: function (name) { return getTextResource('AssetNumber_BuildingName') + ': ' + name; },},
                Floor:        {ClassId:   2, getText: function (name) { return getTextResource('AssetNumber_FloorName') + ': ' + name; },},
                Room:         {ClassId:   3, getText: function (name) { return getTextResource('AssetNumber_RoomName') + ': ' + name; },},
                Rack:         {ClassId:   4, getText: function (name) { return getTextResource('AssetNumber_RackName') + ': ' + name; },},
                User:         {ClassId:   9, getText: function (name) { return getTextResource('User') + ': ' + name; },},
                Workplace:    {ClassId:  22, getText: function (name) { return getTextResource('AssetNumber_WorkplaceName') + ': ' + name; },},
                Owner:        {ClassId:  29, getText: function (name) { return name; },},
                Organization: {ClassId: 101, getText: function (name) { return getTextResource('Organization_Name') + ': ' + name; },},
                Subdivision:  {ClassId: 102, getText: function (name) { return getTextResource('OrgStructureLevel_Subdivision') + ': ' + name; },},
                ProductType:  {ClassId: 378,},
            },

            ViewModel: function ($region, bindedObject) {
                var self = this;
                self.$isLoaded = $.Deferred();
                self.$region = $region;
                //
                self.bindedClassID = bindedObject.ClassID;
                self.bindedObjectID = bindedObject.ID;
                self.bindedServiceID = bindedObject.ServiceID;
                self.bindedClientID = bindedObject.ClientID;
                self.showWrittenOff = bindedObject.ShowWrittenOff;
                self.selectOnlyOne = bindedObject.SelectOnlyOne;
                self.uniqueAssetTypeToShow = bindedObject.UniqueAssetTypeToShow;
                self.ShowKE = bindedObject.ShowKE;
                self.IsHaspAdapterForm = bindedObject.IsHaspAdapterForm;
                self.IsConfigurationUnitAgentForm = bindedObject.IsConfigurationUnitAgentForm;
                self.ConfigurationUnitAgentTypeID = bindedObject.ConfigurationUnitAgentTypeID;
                self.IsHostClusterForm = bindedObject.IsHostClusterForm;
                self.IsVMClusterForm = bindedObject.IsVMClusterForm;
                self.IsInventory = bindedObject.IsInventory;
                self.AllowedClassIDs = bindedObject.AllowedClassIDs;
                //
                self.modes = {
                    ClientOrService: 'ClientOrService',
                    Location: 'Location',
                    ParameterSelector: 'ParameterSelector',
                    Choosen: 'Choosen',
                    SoftwareModelCatalog: 'SoftwareModelCatalog'
                };
                //
                self.FocusSearcher = function () {
                    var searcher = self.$region.find('.asset-link_searchText .text-input');
                    searcher.focus();
                };
                //
                self.CanSeeClientOrService = ko.observable(self.bindedClassID == 701); //CALL
                //
                self.currentMode = ko.observable(null);
                self.currentMode.subscribe(function (newValue) {
                    $.when(self.$isLoaded).done(function () {
                        if (newValue == self.modes.ClientOrService && !self.modelClientService()) {
                            self.initClientService();
                            self.modelClientService().CalculateSize();
                        }
                        //
                        if (newValue == self.modes.Location && !self.modelLocation())
                            self.initLocation();
                        //
                        if (newValue == self.modes.ParameterSelector && !self.modelParameterSelector())
                            self.initParameterSelector();
                        //
                        if (newValue == self.modes.Choosen && !self.modelChoosen())
                            self.initChoosen();
                    });
                });
                //
                self.modelClientService = ko.observable(null);
                self.ClientServiceReady = ko.observable(false);
                self.initClientService = function () {
                    self.modelClientService(new module.ClientServiceModel(self.bindedClientID, self.bindedServiceID, self.OnSelectedChangeHandler, self.IsSelectedChecker, self.FocusSearcher));
                    self.ClientServiceReady(true);
                    self.modelClientService().Search();
                };
                self.selectClientService = function () {
                    self.currentMode(self.modes.ClientOrService);
                    self.FocusSearcher();
                };
                self.isClientServiceSelected = ko.computed(function () {
                    return self.currentMode() == self.modes.ClientOrService;
                });
                //
                self.modelLocation = ko.observable(null);
                self.LocationReady = ko.observable(false);
                self.initLocation = function () {
                    self.modelLocation(new module.LocationModel());
                    self.LocationReady(true);
                };
                self.selectLocation = function () {
                    self.currentMode(self.modes.Location);
                };
                self.isLocationSelected = ko.computed(function () {
                    return self.currentMode() == self.modes.Location;
                });
                //
                self.dispose = function () {
                    if (self.modelParameterSelector())
                        self.modelParameterSelector().dispose();
                };
                //
                self.modelParameterSelector = ko.observable(null);
                self.ParameterReady = ko.observable(false);
                self.initParameterSelector = function () {
                    self.modelParameterSelector(new module.ParameterSelectorModel(self.$region, self.OnSelectedChangeHandler, self.IsSelectedChecker, true, self.FocusSearcher, self.showWrittenOff, self.uniqueAssetTypeToShow, self.IsHaspAdapterForm, self.ShowKE, self.IsConfigurationUnitAgentForm, self.ConfigurationUnitAgentTypeID, self.IsHostClusterForm, self.IsVMClusterForm, self.IsInventory, self.AllowedClassIDs));
                    self.ParameterReady(true);
                };
                self.selectParameterSelector = function () {
                    self.currentMode(self.modes.ParameterSelector);
                    if (self.ParameterReady()) {
                        self.modelParameterSelector().SizeChanged();
                        self.FocusSearcher();
                    }
                };
                self.isParameterSelected = ko.computed(function () {
                    return self.currentMode() == self.modes.ParameterSelector;
                });
                //
                self.modelChoosen = ko.observable(null);
                self.ChoosenReady = ko.observable(false);
                self.initChoosen = function () {
                    self.modelChoosen(new module.ChoosenModel(self.selectedAssets));
                    self.ChoosenReady(true);
                };
                self.selectChoosen = function () {
                    self.currentMode(self.modes.Choosen);
                };
                self.isChoosenSelected = ko.computed(function () {
                    return self.currentMode() == self.modes.Choosen;
                });
                //
                self.selectedAssets = ko.observableArray([]);
                self.isChoosenVisible = ko.computed(function () {
                    var assets = self.selectedAssets();
                    if (assets && assets.length > 0)
                        return true;
                    //
                    return false;
                });
                self.ChoosenCounterText = ko.computed(function () {
                    var assets = self.selectedAssets();
                    //
                    if (assets && assets.length > 0)
                        return '(' + assets.length + ')';
                    //
                    return '';
                });
                //
                self.OnSelectedChangeHandler = function (obj, newValue) {
                    if (!obj)
                        return;
                    //
                    var startSelectedCounter = self.selectedAssets().length;
                    //
                    if (newValue) {
                        var exist = ko.utils.arrayFirst(self.selectedAssets(), function (el) {
                            return el.ID.toUpperCase() === obj.ID.toUpperCase();
                        });
                        //
                        if (!exist) {
                            self.selectedAssets.push(obj);
                            //
                            if (self.ClientServiceReady())
                                self.modelClientService().CheckAndSetSelectedState(obj.ID, true);
                            if (self.ParameterReady())
                                self.modelParameterSelector().CheckAndSetSelectedState(obj.ID, true);
                        }
                    }
                    else {
                        var exist = ko.utils.arrayFirst(self.selectedAssets(), function (el) {
                            return el.ID.toUpperCase() === obj.ID.toUpperCase();
                        });
                        //
                        if (exist) {
                            self.selectedAssets.remove(function (el) { return el.ID.toUpperCase() == obj.ID.toUpperCase(); });
                            //
                            if (self.ClientServiceReady())
                                self.modelClientService().CheckAndSetSelectedState(obj.ID, false);
                            if (self.ParameterReady())
                                self.modelParameterSelector().CheckAndSetSelectedState(obj.ID, false);
                            //
                            if (self.selectedAssets().length == 0 && self.isChoosenSelected()) {
                                if (self.CanSeeClientOrService())
                                    self.selectClientService();
                                else self.selectParameterSelector();
                            }
                        }
                    }
                    //
                    var endSelectedCounter = self.selectedAssets().length;
                    //
                    if (self.selectOnlyOne) {
                        if (endSelectedCounter == 1) {
                            self.SetFilledButtonsList();
                        } else if (endSelectedCounter > 1) {
                            self.SetClearSelectionButtonsList();
                        } else {
                            self.SetCLearButtonsList();
                        }
                    } else {
                        if (startSelectedCounter == 0 && endSelectedCounter > 0 && self.SetFilledButtonsList) {
                            self.SetFilledButtonsList();
                        } else if (startSelectedCounter != 0 && endSelectedCounter == 0 && self.SetCLearButtonsList) {
                            self.SetCLearButtonsList();
                        }
                    }

                };
                self.IsSelectedChecker = function (id) {
                    if (!id)
                        return false;
                    //
                    var exist = ko.utils.arrayFirst(self.selectedAssets(), function (el) {
                        return el.ID.toUpperCase() === id.toUpperCase();
                    });
                    //
                    if (exist)
                        return true;
                    else return false;
                };
                //uses from formHelper
                self.GetFinalList = function () {
                    return ko.toJS(self.selectedAssets());
                };
                self.ClearSelection = function () {
                    while (self.selectedAssets().length > 0) {
                        var el = self.selectedAssets()[0];
                        if (el && el.Selected && el.Selected() === true)
                            el.Selected(false);
                        else break;
                    }
                    self.modelParameterSelector().listView.rowViewModel.allItemsChecked(false);
                };
                self.SetCLearButtonsList = null;
                self.SetFilledButtonsList = null;
                self.SetClearSelectionButtonsList = null;
                //
                self.AfterRender = function () {
                    self.$isLoaded.resolve();
                    //
                    if (self.CanSeeClientOrService())
                        self.selectClientService();
                    else self.selectParameterSelector();
                };
                self.SizeChanged = function () {
                    if (self.ParameterReady())
                        self.modelParameterSelector().SizeChanged();
                };
            },

            ClientServiceModel: function (clientID, serviceID, mainOnChangeSelected, mainCheckerAlreadySelected, focusSearcher) {
                var self = this;
                self.bindedServiceID = ko.observable(serviceID);
                self.bindedClientID = ko.observable(clientID);
                //
                self.FindedClientObjects = ko.observableArray([]);
                self.FindedServiceObjects = ko.observableArray([]);
                self.ChoosenObjects = ko.observableArray([]);
                //
                self.OnChangeSelected = function (obj, newValue) {
                    if (!obj || !mainOnChangeSelected)
                        return;
                    //
                    mainOnChangeSelected(obj, newValue);
                };
                self.CheckAndSetSelectedState = function (id, newState) {
                    var exist = ko.utils.arrayFirst(self.FindedClientObjects(), function (el) {
                        return el.ID.toUpperCase() === id.toUpperCase();
                    });
                    if (exist && exist.Selected() !== newState)
                        exist.Selected(newState);
                    //
                    exist = ko.utils.arrayFirst(self.FindedServiceObjects(), function (el) {
                        return el.ID.toUpperCase() === id.toUpperCase();
                    });
                    if (exist && exist.Selected() !== newState)
                        exist.Selected(newState);
                };
                //
                self.ClientObjectsExpanded = ko.observable(true);
                self.ServiceObjectsExpanded = ko.observable(true);
                self.ExpandCollapseClient = function () {
                    self.ClientObjectsExpanded(!self.ClientObjectsExpanded());
                    //
                    self.CalculateSize();
                };
                self.ExpandCollapseService = function () {
                    self.ServiceObjectsExpanded(!self.ServiceObjectsExpanded());
                    //
                    self.CalculateSize();
                };
                //
                self.CanSearchClient = ko.computed(function () {
                    if (self.bindedClientID())
                        return true;
                    else return false;
                });
                self.CanSearchService = ko.computed(function () {
                    if (self.bindedServiceID())
                        return true;
                    else return false;
                });
                self.CanSearch = ko.computed(function () {
                    return self.CanSearchClient() || self.CanSearchService();
                });
                //
                self.EmptyTextClient = ko.computed(function () {
                    if (self.CanSearchClient())
                        return getTextResource('ListIsEmpty');
                    else return getTextResource('ClientNotSet');
                });
                self.EmptyTextService = ko.computed(function () {
                    if (self.CanSearchService())
                        return getTextResource('ListIsEmpty');
                    else return getTextResource('ServiceNotSet');
                });
                //
                self.SearchText = ko.observable('');
                self.SearchText.subscribe(function (newValue) {
                    self.WaitAndSearch(newValue);
                });
                self.IsSearchTextEmpty = ko.computed(function () {
                    var text = self.SearchText();
                    if (!text)
                        return true;
                    //
                    return false;
                });
                //
                self.SearchKeyPressed = function (data, event) {
                    if (event.keyCode == 13) {
                        if (!self.IsSearchTextEmpty())
                            self.Search();
                    }
                    else
                        return true;
                };
                self.EraseTextClick = function () {
                    self.SearchText('');
                };
                //
                self.searchTimeout = null;
                self.WaitAndSearch = function (text) {
                    clearTimeout(self.searchTimeout);
                    self.searchTimeout = setTimeout(function () {
                        if (text === self.SearchText()) {
                            self.Search();
                        }
                    }, 500);
                };
                //
                self.ajaxControl_search = new ajaxLib.control();
                self.Search = function () {
                    var returnD = $.Deferred();
                    //
                    if (self.CanSearch())
                        $.when(userD).done(function (user) {
                            var param = {
                                Query: encodeURIComponent(self.SearchText()),
                                NeedClient: self.CanSearchClient(),
                                NeedService: self.CanSearchService(),
                                ServiceID: self.bindedServiceID(),
                                ClientID: self.bindedClientID(),
                                StartPosition: 0
                            };
                            self.ajaxControl_search.Ajax($('.asset-link-clientservice'),
                                {
                                    url: '/imApi/SearchByClientAndService?' + $.param(param),
                                    method: 'GET'
                                },
                                function (response) {
                                    if (response) {
                                        if (response.Result === 0) {
                                            if (response.ClientList && self.CanSearchClient()) {
                                                self.FindedClientObjects.removeAll();
                                                response.ClientList.forEach(function (el) {
                                                    self.FindedClientObjects().push(new module.ListAssetObject(el, self.OnChangeSelected, mainCheckerAlreadySelected));
                                                });
                                                self.FindedClientObjects.valueHasMutated();
                                            }
                                            //
                                            if (response.ServiceList && self.CanSearchService()) {
                                                self.FindedServiceObjects.removeAll();
                                                response.ServiceList.forEach(function (el) {
                                                    self.FindedServiceObjects().push(new module.ListAssetObject(el, self.OnChangeSelected, mainCheckerAlreadySelected));
                                                });
                                                self.FindedServiceObjects.valueHasMutated();
                                            }
                                        }
                                        else if (response.Result == 1) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + ' ' + '\n[AssetLink.js, Search]', 'error');
                                            });
                                        }
                                        else {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + ' ' + '\n[AssetLink.js, Search]', 'error');
                                            });
                                        }
                                        //
                                        self.CalculateSize();
                                        returnD.resolve();
                                    }
                                    else {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + ' ' + '\n[AssetLink.js, Search]', 'error');
                                        });
                                        //
                                        returnD.resolve();
                                    }
                                });
                        });
                    else returnD.resolve();
                    //
                    return returnD;
                };
                //
                self.CalculatedClientHeight = ko.observable('0px');
                self.CalculatedServiceTop = ko.observable('0px');
                //
                self.CalculateSize = function () {
                    var h = 0;
                    //
                    if (!self.ClientObjectsExpanded())
                        h = 30;
                    else if (self.FindedClientObjects().length == 0)
                        h = 80;
                    else if ((self.FindedServiceObjects().length == 0 || !self.ServiceObjectsExpanded()) && self.FindedClientObjects().length > 3)
                        h = 480;
                    else h = 345;
                    //
                    self.CalculatedClientHeight(h + 'px');
                    self.CalculatedServiceTop(h + 60 + 10 + 'px'); //строка поиска и отступ между
                };
                //
                self.ShowForm = function (obj) {
                    if (!obj || !obj.ID || !obj.ClassID)
                        return;
                    //
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowAssetForm(obj.ID, obj.ClassID);
                    });
                };
                //
                self.AfterRender = function () {
                    if (focusSearcher)
                        focusSearcher();
                };
            },

            ParameterSelectorModel: function ($region, mainOnChangeSelected, mainCheckerAlreadySelected, showListForEmptyQuery, focusSearcher, showWrittenOff, uniqueAssetTypeToShow, NewHaspAdapterForm, ShowKE, IsConfigurationUnitAgentForm, ConfigurationUnitAgentTypeID, IsHostClusterForm, IsVMClusterForm, IsInventory, allowedClassIDs) {
                var self = this;
                //
                self.IsHaspAdapterForm = ko.observable(false);
                if (NewHaspAdapterForm) {
                    self.IsHaspAdapterForm(NewHaspAdapterForm);
                }
                //
                self.startLoadingTable = ko.observable(false);
                self.startLoadingColumns = ko.observable(false);
                self.$region = $region;
                self.subscriptionList = [];
                //
                self.AllowedClassIDs = allowedClassIDs;
                //
                self.dispose = function () {
                    if (self.listViewContextMenu && self.listViewContextMenu() != null)
                        self.listViewContextMenu().dispose();
                    if (self.listView != null)
                        self.listView.dispose();
                    //
                    for (var i in self.subscriptionList) {
                        self.subscriptionList[i].dispose();
                    }
                    //TODO other fields and controls
                };
                //
                {//bind contextMenu
                    self.listViewContextMenu = null;// contextMenu;
                }

                self.viewName = 'AssetSearch';
                {//events of listView
                    self.listView = null;
                    //
                    self.listViewInit = function (listView) {
                        self.listView = listView;
                        m_helpers.init(self, listView);//extend self
                        listView.load();
                        //
                        var subscription = self.listView.rowViewModel.rowChecked.subscribe(function (row) {
                            if (row.checked())
                                self.RowSelected([row]);
                            else
                                self.RowDeselected([row]);
                        });
                        //
                        self.subscriptionList.push(subscription);
                        //
                        subscription = self.listView.rowViewModel.allItemsChecked.subscribe(function (allItemsChecked) {
                            if (allItemsChecked)
                                self.RowSelected(self.listView.rowViewModel.checkedItems());
                            else
                                self.RowDeselected(self.listView.rowViewModel.rowList());
                        });
                        //
                        self.subscriptionList.push(subscription);
                    };
                    //
                    self.listViewRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                            if (objectList) {
                                if (startRecordIndex === 0)//reloaded
                                {
                                    self.clearAllInfos();
                                }
                                else
                                    objectList.forEach(function (obj) {
                                        var id = self.getObjectID(obj);
                                        self.clearInfoByObject(id);
                                    });
                            }
                            retvalD.resolve(objectList);
                        });
                        return retvalD.promise();
                    };
                    self.listViewRowClick = function (obj) {
                        var classID = self.getObjectClassID(obj);
                        var id = self.getMainObjectID(obj);
                        //
                        var row = self.getRowByID(id);
                        if (row != null)
                            self.setRowAsLoaded(row);
                        //
                        self.showObjectForm(classID, id);
                    };

                    self.showObjectForm = function (classID, id) {
                        // todo: Тут будет показ формы
                        // showSpinner();
                        // require(['assetForms'], function (module) {
                        //     var fh = new module.formHelper(true);
                        //     if (classID == 5 || classID == 6 || classID == 33 || classID == 34 || classID == 415)
                        //         fh.ShowAssetForm(id, classID);
                        //     else if (classID == 115)//contract
                        //         fh.ShowServiceContract(id);
                        //     else if (classID == 386)
                        //         fh.ShowServiceContractAgreement(id);
                        //     else if (classID == 223) //software licence
                        //     {
                        //         fh.ShowSoftwareLicenceForm(id);
                        //     }
                        //     else if (classID == 409 || classID == 410 || classID == 411 || classID == 412 ||
                        //         classID == 413 || classID == 414 || classID == 415 || classID == 419)
                        //         fh.ShowConfigurationUnitForm(id);
                        //     else if (classID == 165)
                        //         fh.ShowDataEntityObjectForm(id);    
                        //     else if (/*classID == 415 ||*/ classID == 416 || classID == 417 || classID == 418 || classID == 12) //OBJ_LogicalObject
                        //         fh.ShowLogicalObjectForm(id);
                        //     else
                        //         hideSpinner();
                        // });
                    };

                    self.listViewDrawCell = function (obj, column, cell) {
                    };
                    //
                    self.loadObjectListByIDs = function (idArray, unshiftMode) {
                        for (var i = 0; i < idArray.length; i++)
                            idArray[i] = idArray[i].toUpperCase();
                        //
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
                                if (objectList) {
                                    var rows = self.appendObjectList(objectList, unshiftMode);
                                    rows.forEach(function (row) {
                                        self.setRowAsNewer(row);
                                        //
                                        var obj = row.object;
                                        var id = self.getMainObjectID(obj);
                                        self.clearInfoByObject(id);
                                        //
                                        var index = idArray.indexOf(id);
                                        if (index != -1)
                                            idArray.splice(index, 1);
                                    });
                                }
                                idArray.forEach(function (id) {
                                    self.removeRowByID(id);
                                    self.clearInfoByObject(id);
                                });
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };
                    self.getObjectListByIDs = function (idArray, unshift) {
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };
                    //
                    self.ajaxControl = new ajaxLib.control();
                    self.isAjaxActive = function () {
                        return self.ajaxControl.IsAcitve() === true;
                    };
                    //
                    self.searchFilterData = ko.observable({ //set in ActivesLocatedLink.js
                        TypesID: [],
                        ModelsID: [],
                        LocationClassID: null,
                        LocationID: null,
                        OrgStructureObjectClassID: null,
                        OrgStructureObjectID: null,
                        OrgStructureFilterType: 'MOL',
                    });
                    //
                    self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                        var retvalD = $.Deferred();

                        let filter = {
                            StartRecordIndex: idArray ? 0 : startRecordIndex,
                            CountRecords: idArray ? idArray.length : countOfRecords,
                            ViewName: 'AssetSearch',
                            TypesID: self.searchFilterData() ? self.searchFilterData().TypesID : null,
                            ModelsID: self.searchFilterData() ? self.searchFilterData().ModelsID : null,
                            LocationID: self.searchFilterData() ? self.searchFilterData().LocationID : null,
                            LocationClassID: self.searchFilterData() ? self.searchFilterData().LocationClassID : null,
                            OrgStructureObjectID: self.searchFilterData() ? self.searchFilterData().OrgStructureObjectID : null,
                            OrgStructureObjectClassID: self.searchFilterData() ? self.searchFilterData().OrgStructureObjectClassID : null,
                            OrgStructureFilterType: self.searchFilterData() && self.searchFilterData().OrgStructureFilterType ? self.searchFilterData().OrgStructureFilterType : 'MOL',
                            SearchText: self.searchPhraseObservable(),
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                method: 'GET',
                                data: filter,
                                url: '/api/Hardwares/Reports/AssetSearch',
                                traditional: true,
                            },
                            function (newVal) {
                                retvalD.resolve(newVal || []);
                            },
                            function (XMLHttpRequest, textStatus, errorThrown) {
                                if (XMLHttpRequest.status === 404 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('ResourceNotFoundErrorText') + '\n[AssetForms/AssetLink/AssetLink.js getObjectList]', 'error');
                                    });
                                } else if (XMLHttpRequest.status === 403 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AccessError_Table') + '\n[AssetForms/AssetLink/AssetLink.js getObjectList]', 'error');
                                    });
                                } else if (XMLHttpRequest.statusText !== 'abort' && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[AssetForms/AssetLink/AssetLink.js getObjectList]', 'error');
                                    });
                                }
                                //
                                retvalD.resolve([]);
                            },
                            null);

                        return retvalD.promise();
                    };
                }
                //
                {//identification
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };
                    self.getObjectClassID = function (obj) {
                        return obj.ClassID;
                    };
                    self.getMainObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };
                    self.isObjectClassVisible = function (objectClassID) {
                        if (
                            //obj_networkDevice
                            objectClassID == 5

                            //obj_terminalDevice
                            || objectClassID == 6

                            //obj_adapter
                            || objectClassID == 33

                            //obj_peripheral
                            || objectClassID == 34

                            //obj_licence
                            || objectClassID == 223

                            //obj_material
                            || objectClassID == 120

                            //obj_serviceContract
                            || objectClassID == 115

                            //OBJ_LogicalObject
                            ||classID == 415 || classID == 416 || classID == 417 || classID == 418 || classID == 12) 
                            return true;
                    };
                }
                //
                self.SizeChanged = function () {
                    var $regionTable = self.$region.find('.asset-link_tableColumn');
                    var tableHeightWithoutHeader = $regionTable.height() - $regionTable.find(".tableHeader").outerHeight();
                    $regionTable.find(".region-Table").css("height", $regionTable.height() + "px");//для скрола на таблице (без шапки)
                    if (self.listView)
                        self.listView.renderTable();
                };
                //
                self.ajaxControl_load = new ajaxLib.control();
                self.RowSelected = function (rowArray) {
                    if (rowArray && rowArray.length > 0) {
                        ko.utils.arrayForEach(rowArray, function (el) {
                            const selectedItem = el.object || el; 
                            const item = {
                                ID: selectedItem.ID,
                                ClassID: selectedItem.ClassID,
                                Name: selectedItem.Name,
                                State: selectedItem.LifeCycleStateName,
                                Type: selectedItem.TypeName,
                                Model: selectedItem.ModelName,
                                Building: selectedItem.BuildingName,
                                Floor: selectedItem.FloorName,
                                Room: selectedItem.RoomName,
                                Rack: selectedItem.RackName,
                                Workplace: selectedItem.WorkplaceName,
                                Organization: selectedItem.OrganizationName,
                                FullObjectLocation: selectedItem.FullObjectLocation,
                                FullObjectName: selectedItem.FullObjectName,
                            };
                            const obj = new module.ListAssetObject(item, mainOnChangeSelected, mainCheckerAlreadySelected);
                            obj.Selected(true);
                        });
                    }
                };

                self.RowDeselected = function (rowArray) {
                    if (rowArray && rowArray.length > 0) {
                        ko.utils.arrayForEach(rowArray, function (el) {
                            var item = {
                                ID: el.object.ID,
                                ClassID: el.object.ClassID,
                                Name: el.object.Name,
                                State: el.object.LifeCycleStateName,
                                Type: el.object.TypeName,
                                Model: el.object.ModelName,
                                Building: el.object.BuildingName,
                                Floor: el.object.FloorName,
                                Room: el.object.RoomName,
                                Rack: el.object.RackName,
                                Workplace: el.object.WorkplaceName,
                                Organization: el.object.OrganizationName,
                                FullObjectLocation: el.object.FullObjectLocation,
                                FullObjectName: el.object.FullObjectName,
                            };
                            var obj = new module.ListAssetObject(item, mainOnChangeSelected, mainCheckerAlreadySelected);
                            mainOnChangeSelected(obj, false);
                        });
                    }
                };
                self.CheckAndSetSelectedState = function (id, newState) {
                    if (!self.startLoadingTable()) {
                        return;
                    }
                    id = id.toUpperCase();
                    var row = self.tableModel.rowHashList[id];
                    if (!row) {
                        row = ko.utils.arrayFirst(self.tableModel.rowList(), function (el) {
                            return el.ID.toUpperCase() === id;
                        });
                    }
                    if (row && row.Checked() !== newState) {
                        row.Checked(newState);
                    }
                };
                //
                self.TypeTreeControl = new productCatalogTree.Control();
                self.InitializeTypeSelector = function () {
                    self.TypeTreeControl.Tree.load();
                    self.TypeTreeControl.Tree.selectedNode.subscribe(self.OnTypeSelected);
                };
                self.SelectedType = ko.observable(null);
                self.IsTypeSelected = ko.computed(function () {
                    return self.SelectedType() != null;
                });
                self.SelectedModels = ko.observableArray(null);
                self.IsModelSelected = ko.computed(function () {
                    return self.SelectedModels().length > 0;
                });
                self.OnTypeSelected = function (element) {
                    if (!element || !element.id) {
                        self.SelectedType(null);
                        self.SelectedModels.removeAll();
                        self.ImplementFilter();
                        return;
                    }

                    self.SelectedModels.removeAll();
                    self.TypeTreeControl.deselectAll();
                    self.TypeTreeControl.CollapseExpandHeader();
                    
                    let modelsD = $.Deferred();
                    if (element.classId === module.ObjectClasses.ProductType.ClassId) {
                        self.SelectedType(element);
                        $.when(self.TypeTreeControl.Tree.loadChildNodes(element)).done(function () {
                            self.TypeTreeControl.Tree.forEachNode(function (node) {
                                self.SelectedModels.push(node);
                            }, element);
                            modelsD.resolve();
                        });
                    } else {
                        if (element.parent) {
                            self.SelectedType(element.parent);
                        }
                        self.SelectedModels.push(element);
                        modelsD.resolve();
                    }

                    $.when(modelsD).done(function () {
                        self.ImplementFilter();
                    });
                };
                self.EraseSelectedTypeClick = function () {
                    self.OnTypeSelected();
                };
                self.EraseSelectedModelClick = function (node) {
                    self.SelectedModels.remove(node);
                    if (self.SelectedModels().length === 0) {
                        self.OnTypeSelected();
                    }
                    self.ImplementFilter();
                }
                //
                self.visibleInfo = function () {
                    return self.IsTypeSelected();
                };
                self.visibleModelInfo = function () {
                    return self.SelectedModels().length > 0;
                };
                self.visibleLocation = function () {
                    return self.SelectedTextLocation() != null;
                };
                self.visibleOrgStructure = function () {
                    return self.SelectedOrgStructure() != null;
                };
                //
                self.SelectedOrgStructure = ko.observable(null);
                self.SelectedOrgStructureType = ko.observable(null);
                self.SelectedTextOrgStructure = ko.observable(null);
                //
                self.OnSelectOrgStructure = function (node) {
                    if (!node || !node.id || node.classId === module.ObjectClasses.Owner.ClassId) {
                        if (self.SelectedOrgStructure(null)) {
                            self.ResetTreeSettingsClick();
                        }
                        return;
                    }

                    self.SelectedOrgStructure(node);
                    self.SelectedOrgStructureType(self.OrgStructureTreeControl.FilterType.SelectedValue().ID);
                    self.OrgStructureTreeControl.CollapseExpandHeader();

                    self.OrgStructureTreeControl.deselectAll();
                    self.LocationTreeControl.deselectAll();
                    self.MakeTextLocation(node, self.SelectedTextOrgStructure);

                    self.ImplementFilter();
                };
                self.EraseSelectedOrgStructureClick = function () {
                    self.OnSelectOrgStructure();
                }
                //
                self.ResetTreeSettingsClick = function () {
                    self.SelectedOrgStructure(null);
                    self.SelectedTextOrgStructure(null);
                    //
                    if (self.OrgStructureTreeControl) {
                        self.OrgStructureTreeControl.deselectAll();
                    }
                    //
                    self.ImplementFilter();
                };

                self.OrgStructureTreeControl = new orgStructureTree.Control({
                    EnableFilterType: true,
                });
                self.InitOrgStructureTree = function () {
                    self.OrgStructureTreeControl.Tree.load();
                    self.OrgStructureTreeControl.Tree.selectedNode.subscribe(self.OnSelectOrgStructure);
                };

                self.LocationTreeControl = new locationTree.Control({
                    SelectableClasses: [
                        module.ObjectClasses.Organization.ClassId,
                        module.ObjectClasses.Building.ClassId,
                        module.ObjectClasses.Floor.ClassId,
                        module.ObjectClasses.Room.ClassId,
                        module.ObjectClasses.Workplace.ClassId,
                    ]
                });
                self.InitLocationTree = function () {
                    self.LocationTreeControl.Tree.load();
                    self.LocationTreeControl.Tree.selectedNode.subscribe(self.OnSelectLocation);
                };
                self.SelectedLocation = ko.observable(null);
                self.LocationSelected = ko.computed(function () {
                    return self.SelectedLocation() == null;
                });
                self.OnSelectLocation = function (node) {
                    if (!node || !node.id || node.classId === module.ObjectClasses.Owner.ClassId) {
                        self.SelectedLocation(null);
                        self.SelectedTextLocation(null);
                        self.LocationTreeControl.deselectAll();
                        return;
                    }
                    self.SelectedLocation(node);
                    self.MakeTextLocation(node, self.SelectedTextLocation);
                    self.LocationTreeControl.CollapseExpandHeader();
                    self.ImplementFilter();
                };
                self.SelectedTextLocation = ko.observable(null);
                self.MakeTextLocation = function (node, textChanged) {
                    switch (node.classId) {
                        case module.ObjectClasses.Owner.ClassId:
                            textChanged(module.ObjectClasses.Owner.getText(node.text()));
                            return true;
                        case module.ObjectClasses.Organization.ClassId:
                            textChanged(module.ObjectClasses.Organization.getText(node.text()));
                            return true;
                        case module.ObjectClasses.Subdivision.ClassId:
                            textChanged(module.ObjectClasses.Subdivision.getText(node.text()));
                            return true;
                        case module.ObjectClasses.Building.ClassId:
                            textChanged(module.ObjectClasses.Building.getText(node.text()));
                            return true;
                        case module.ObjectClasses.Floor.ClassId:
                            textChanged(module.ObjectClasses.Floor.getText(node.text()));
                            return true;
                        case module.ObjectClasses.Room.ClassId:
                            textChanged(module.ObjectClasses.Room.getText(node.text()));
                            return true;
                        case module.ObjectClasses.Workplace.ClassId:
                            textChanged(module.ObjectClasses.Workplace.getText(node.text()));
                            return true;
                        case module.ObjectClasses.Rack.ClassId:
                            textChanged(module.ObjectClasses.Rack.getText(node.text()));
                            return true;
                        case module.ObjectClasses.User.ClassId:
                            textChanged(module.ObjectClasses.User.getText(node.text()));
                            return true;
                    }
                    return false;
                };

                self.EraseSelectedLocationClick = function (node) {
                    self.OnSelectLocation();
                };

                self.ImplementFilter = function () {
                    let returnD = $.Deferred();

                    let types = [];
                    let models = [];
                    let locationClassID = null;
                    let locationID = null;
                    let orgStructureObjectClassID = null;
                    let orgStructureObjectID = null;
                    let orgStructureFilterType = null;

                    if (self.SelectedType()) {
                        types.push(self.SelectedType().id);

                        if (self.SelectedModels().length > 0) {
                            ko.utils.arrayForEach(self.SelectedModels(), function (item) {
                                models.push(item.id);
                            });
                        }
                    }

                    let selectedLocation = self.SelectedLocation(); 
                    if (selectedLocation) {
                        locationClassID = selectedLocation.classId;
                        locationID = selectedLocation.IMObjID !== undefined ? selectedLocation.IMObjID : selectedLocation.id;
                    }

                    if (self.SelectedOrgStructure()) {
                        orgStructureObjectClassID = self.SelectedOrgStructure().classId;
                        orgStructureObjectID = self.SelectedOrgStructure().id;
                        orgStructureFilterType = self.SelectedOrgStructureType();
                    }

                    var old = self.searchFilterData();
                    var newData = {
                        TypesID: types,
                        ModelsID: models,
                        LocationClassID: locationClassID,
                        LocationID: locationID,
                        OrgStructureObjectClassID: orgStructureObjectClassID,
                        OrgStructureObjectID: orgStructureObjectID,
                        OrgStructureFilterType: orgStructureFilterType,
                    };
                    if (self.IsFilterDataDifferent(old, newData)) {
                        self.searchFilterData(newData);
                        self.UpdateTableByFilter(newData);
                    } else {
                        returnD.resolve();
                    }
                    return returnD;
                };
                //
                self.ReloadTable = function () {
                    self.listView.load();
                };
                //
                self.updateTableByFilterTimeout = null;
                self.UpdateTableByFilter = function (data) {
                    clearTimeout(self.updateTableByFilterTimeout);
                    self.updateTableByFilterTimeout = setTimeout(function () {
                        if (!self.IsFilterDataDifferent(self.searchFilterData(), data))
                            self.listView.load();
                    }, 500);
                };
                //
                self.IsFilterDataDifferent = function (oldData, newData) {
                    if (!oldData || !newData)
                        return false;

                    if (arr_diff(oldData.TypesID, newData.TypesID).length > 0)
                        return true;

                    if (arr_diff(oldData.ModelsID, newData.ModelsID).length > 0)
                        return true;

                    if (oldData.LocationID !== newData.LocationID)
                        return true;

                    if (oldData.OrgStructureObjectID !== newData.OrgStructureObjectID)
                        return true;

                    return false;
                };
                var arr_diff = function (a1, a2) {
                    var a = [], diff = [];
                    for (var i = 0; i < a1.length; i++) {
                        a[a1[i]] = true;
                    }
                    //
                    for (var i = 0; i < a2.length; i++) {
                        if (a[a2[i]]) {
                            delete a[a2[i]];
                        } else {
                            a[a2[i]] = true;
                        }
                    }
                    //
                    for (var k in a) {
                        diff.push(k);
                    }
                    //
                    return diff;
                };
                //
                self.SearchText = ko.observable('');
                self.SearchText.subscribe(function (newValue) {
                    self.WaitAndSearch(newValue);
                });
                self.IsSearchTextEmpty = ko.computed(function () {
                    var text = self.SearchText();
                    if (!text)
                        return true;
                    //
                    return false;
                });
                //
                self.ClearSearchTextAndFilters = function () {
                    self.SearchText('');
                    //
                    self.OnTypeSelected();
                    self.SelectedLocation(null);
                    // 
                    self.ImplementFilter();
                    self.LocationTreeControl.deselectAll();
                    self.TypeTreeControl.deselectAll();
                };
                //
                self.SearchKeyPressed = function (data, event) {
                    if (event.keyCode == 13) {
                        if (!self.IsSearchTextEmpty())
                            self.Search();
                    }
                    else
                        return true;
                };
                self.EraseTextClick = function () {
                    self.SearchText('');
                };
                //
                self.searchTimeout = null;
                self.WaitAndSearch = function (text) {
                    clearTimeout(self.searchTimeout);
                    self.searchTimeout = setTimeout(function () {
                        if (text == self.SearchText())
                            self.Search();
                    }, 500);
                };
                //
                self.ajaxControl_search = new ajaxLib.control();
                self.searchPhraseObservable = ko.observable('');//set in ActivesLocatedLink.js
                self.Search = function () {
                    self.searchPhraseObservable(self.SearchText());
                    self.listView.load();
                };
                //
                self.AfterRender = function () {
                    self.InitializeTypeSelector();
                    self.InitLocationTree();
                    self.InitOrgStructureTree();
                    if (focusSearcher) {
                        focusSearcher();
                    }
                };
                m_lazyEvents.init(self);//extend self
            },

            ChoosenModel: function (observableList) {
                var self = this;
                //
                self.ChoosenObjectsList = observableList;
                self.ShowForm = function (obj) {
                    if (!obj || !obj.ID || !obj.ClassID)
                        return;
                    //
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        if (obj.ClassID == 5 || obj.ClassID == 6 || obj.ClassID == 33 || obj.ClassID == 34)
                            fh.ShowAssetForm(obj.ID, obj.ClassID);
                        else if (obj.ClassID == 115)
                            fh.ShowServiceContract(obj.ID);
                        else if (obj.ClassID == 386)
                            fh.ShowServiceContractAgreement(obj.ID);
                        else if (obj.ClassID == 223)
                            fh.ShowSoftwareLicenceForm(obj.ID);    
                    });
                };
            },

            ListAssetObject: function (obj, onSelectedChange, mainCheckerAlreadySelected) {
                var self = this;

                self.ID = obj.ID;
                self.Name = obj.Name;
                self.State = obj.State;
                self.Type = obj.Type;
                self.Model = obj.Model;
                self.ClassID = obj.ClassID;
                self.Building = obj.Building;
                self.Floor = obj.Floor;
                self.Room = obj.Room;
                self.Rack = obj.Rack;
                self.Workplace = obj.Workplace;
                self.Organization = obj.Organization;
                self.FullObjectName = obj.FullObjectName;
                self.FullObjectLocation = obj.FullObjectLocation;

                self.Selected = ko.observable(mainCheckerAlreadySelected ? mainCheckerAlreadySelected(self.ID) : false);
                self.Selected.subscribe(function (newValue) {
                    if (onSelectedChange) {
                        onSelectedChange(self, newValue);
                    }
                });
                self.CssIconClass = ko.computed(function () {
                    return ihLib.getIconByClassID(self.ClassID);
                });
            }
        }
        return module;
    });
