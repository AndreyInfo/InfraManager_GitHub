define(
    [ 'knockout', 'restApiTree', 'restApiTreeControl' ],
    function (ko, tree, treeControl) {
        var module = {
            RootNodeData: {
                ID: '00000000-0000-0000-0000-000000000000',
                Name: getTextResource('ProductCatalogueTreeCaption'),
            },
            ExcludeCategories: [
                '00000000-0000-0000-0000-000000000115',
                '00000000-0000-0000-0000-000000000120',
            ], 
            ObjectClass: {
                Root: 0,
                ProductCategory: 374,
                ProductType: 378,
                NetworkDeviceModel: 93,
                TerminalDeviceModel: 94,
                AdapterModel: 95,
                PeripherialModel: 96
            },
            
            Creator: function (id, classId) {
                if (!module.creatorsMapping) {
                    module.creatorsMapping = {};
                    module.creatorsMapping[module.ObjectClass.RootNode] = module.RootNodeCreator;
                    module.creatorsMapping[module.ObjectClass.ProductCategory] = module.CategoryCreator;
                    module.creatorsMapping[module.ObjectClass.ProductType] = module.ProductTypeCreator;
                    module.creatorsMapping[module.ObjectClass.NetworkDeviceModel] = module.NetworkDeviceModelCreator;
                    module.creatorsMapping[module.ObjectClass.TerminalDeviceModel] = module.TerminalDeviceModelCreator;
                    module.creatorsMapping[module.ObjectClass.AdapterModel] = module.AdapterModelCreator;
                    module.creatorsMapping[module.ObjectClass.PeripherialModel] = module.PeripherialModelCreator;
                }
                return module.creatorsMapping[classId](id);
            },

            RootNodeCreator: function () { // 0 - Корень. Формируется на клиенте.
                return new module.RootNodeViewModel();
            },
            CategoryCreator: function(idOrDataItem, parent) { // 374 - Категория 
                return new module.CategoryNodeViewModel(idOrDataItem, parent);
            },
            ProductTypeCreator: function(idOrDataItem, parent) { // 378 - Тип
                return new module.ProductTypeNodeViewModel(idOrDataItem, parent);
            },
            NetworkDeviceModelCreator: function(idOrDataItem, parent) { // 93 - Типы активного оборудования
                return new module.NetworkDeviceModelNodeViewModel(idOrDataItem, parent);
            },
            TerminalDeviceModelCreator: function(idOrDataItem, parent) { // 94 - Типы оконечного оборудования
                return new module.TerminalDeviceModelNodeViewModel(idOrDataItem, parent);
            },
            AdapterModelCreator: function(idOrDataItem, parent) { // 95 - Типы адаптеров
                return new module.AdapterModelNodeViewModel(idOrDataItem, parent);
            },
            PeripherialModelCreator: function(idOrDataItem, parent) { // 96 - Типы периферийных устройств
                return new module.PeripherialModelNodeViewModel(idOrDataItem, parent);
            },

            RootNodeViewModel: function() {
                var self = this;

                tree.DataNodeViewModel.call(
                    self,
                    null,
                    module.RootNodeData.ID,
                    module.ObjectClass.Root,
                    null,
                    [ new module.CategoriesCollection('HasParentCatalogCategoryId=false') ]);

                self._getIconCss = function () {
                    return module.IconClasses.Root;
                };

                self.setData = function (data) {
                    self.text(data.Name);
                }

                self.selectable(false);
            },
            CategoryNodeViewModel: function (idOrDataItem, parent) {
                var self = this;
                var id = tree.getID(idOrDataItem, 'ID');
                
                self._getChildCategoriesCollections = function (parentID) {
                    var queryString = 'HasParentCatalogCategoryId=true&ParentCatalogCategoryID=' + parentID;
                    return [
                        new module.CategoriesCollection(queryString),
                        new module.ProductTypesCollection(parentID),
                    ];
                }

                tree.DataNodeViewModel.call(
                    self,
                    '/api/ProductCategories/' + id,
                    id,
                    module.ObjectClass.ProductCategory,
                    parent,
                    self._getChildCategoriesCollections(id));

                self.setData = function (data) {
                    self.text(data.Name);
                    self.parent = self.parent || module.RootNodeCreator();
                }

                self._getIconCss = function () {
                    return module.IconClasses.Category;
                };

                self.selectable(false);
            },
            ProductTypeNodeViewModel: function(idOrDataItem, parent) {
                var self = this;
                var id = tree.getID(idOrDataItem, 'ID');

                self._getChildCollections = function (modelClassID, typeId) {
                    let result = [];
                    if (modelClassID === module.ObjectClass.NetworkDeviceModel) {
                        result.push(new module.NetworkDeviceModelsCollection(typeId));
                    }
                    if (modelClassID === module.ObjectClass.TerminalDeviceModel) {
                        result.push(new module.TerminalDeviceModelsCollection(typeId));
                    }
                    if (modelClassID === module.ObjectClass.AdapterModel) {
                        result.push(new module.AdapterModelsCollection(typeId));
                    }
                    if (modelClassID === module.ObjectClass.PeripherialModel) {
                        result.push(new module.PeripherialModelsCollection(typeId));
                    }
                    return result;
                };

                tree.DataNodeViewModel.call(
                    self,
                    '/api/ProductTypes/' + id,
                    id,
                    module.ObjectClass.ProductType,
                    parent,
                    self._getChildCollections(idOrDataItem['ModelClassID'], id));

                self.setData = function (data) {
                    self.text(data.Name);
                }

                self._getIconCss = function () {
                    return module.getIconClass(idOrDataItem);
                };

                self.selectable(true);
            },
            DeviceModelNodeViewModelBase: function(idOrDataItem, classId, baseUri, parent) {
                var self = this;
                var id = tree.getID(idOrDataItem, 'ID');

                tree.DataNodeViewModel.call(self, baseUri + id, id, classId, parent, []);

                self.setData = function (data) {
                    self.text(data.Name);
                }

                self._getIconCss = function () {
                    return parent ? parent._getIconCss() : module.getIconClass();
                };

                self.selectable(true);
            },
            NetworkDeviceModelNodeViewModel: function (idOrDataItem, parent) {
                module.DeviceModelNodeViewModelBase.call(
                    this,
                    idOrDataItem,
                    module.ObjectClass.NetworkDeviceModel,
                    '/api/NetworkDeviceModels/',
                    parent);
            },
            TerminalDeviceModelNodeViewModel: function (idOrDataItem, parent) {
                module.DeviceModelNodeViewModelBase.call(
                    this,
                    idOrDataItem,
                    module.ObjectClass.TerminalDeviceModel,
                    '/api/TerminalDeviceModels/',
                    parent);
            },
            AdapterModelNodeViewModel: function (idOrDataItem, parent) {
                module.DeviceModelNodeViewModelBase.call(
                    this,
                    idOrDataItem,
                    module.ObjectClass.AdapterModel,
                    '/api/AdapterModels/',
                    parent); 
            },
            PeripherialModelNodeViewModel: function (idOrDataItem, parent) {
                module.DeviceModelNodeViewModelBase.call(
                    this,
                    idOrDataItem,
                    module.ObjectClass.PeripherialModel,
                    '/api/PeripheralModels/',
                    parent);
            },

            RootNodesCollection: function () {
                var self = this;

                self.data = {
                    Name: module.RootNodeData.Name,
                };

                tree.TreeNodeCollectionBase.call(self, null, module.RootNodeCreator);
                
                self.isLoaded(true);
            },
            CategoriesCollection: function (queryFilter) {
                let excludes = module.ExcludeCategories.reduce(
                    (str, currentValue) => str + '&ExcludeIDs=' + currentValue, '');
                let uri = '/api/ProductCategories?' + queryFilter + excludes;
                tree.TreeNodeCollectionBase.call(this, uri, module.CategoryCreator);
            },
            ProductTypesCollection: function (categoryID) {
                let uri = '/api/ProductTypes?HasParentProductCatalogCategoryID=true&ParentProductCatalogCategoryID=' + categoryID;
                tree.TreeNodeCollectionBase.call(this, uri, module.ProductTypeCreator);
            },
            NetworkDeviceModelsCollection: function (typeID) {
                let uri = '/api/NetworkDeviceModels?TypeID=' + typeID;
                tree.TreeNodeCollectionBase.call(this, uri, module.NetworkDeviceModelCreator);
            },
            TerminalDeviceModelsCollection: function (typeID) {
                let uri = '/api/TerminalDeviceModels?TypeID=' + typeID;
                tree.TreeNodeCollectionBase.call(this, uri, module.TerminalDeviceModelCreator);
            },
            AdapterModelsCollection: function (typeID) {
                let uri = '/api/AdapterModels?TypeID=' + typeID;
                tree.TreeNodeCollectionBase.call(this, uri, module.AdapterModelCreator);
            },
            PeripherialModelsCollection: function(typeID) {
                let uri = '/api/PeripheralModels?TypeID=' + typeID;
                tree.TreeNodeCollectionBase.call(this, uri, module.PeripherialModelCreator);
            },

            ProductCatalogTreeViewModel: function (fetcher) {
                var self = this;

                tree.LazyLoadingTreeViewModel.call(self, fetcher, new module.RootNodesCollection(), module.Creator);

                // добавляем загруженную root-ноду
                let node = self.children.creator(module.RootNodeData, self);
                node.setData(module.RootNodeData);
                self._nodeAdding(node);
                self.executeFilter(node);
                self.children.nodes.push(node);
                self.nodes.removeAll();
                ko.utils.arrayForEach(self.children.nodes, function (node) { self.nodes.push(node); });
            },

            Control: function () {
                var self = this;

                let fetcher = new tree.Fetcher('treeControlItems');
                let productCatalogTree = new module.ProductCatalogTreeViewModel(fetcher);

                let caption = getTextResource('ProductCatalogueTreeCaption');
                treeControl.Control.call(self, caption, productCatalogTree);
            },

            getIconClass: function(dataItem) {
                let templateId = dataItem && typeof dataItem === 'object'
                    ? dataItem['TemplateID']
                    : 0;
                return module.IconClasses[templateId] || module.IconClasses[0];
            },
            
            IconClasses: {
                Root: 'treeNodeIcon-owner',
                Category: 'treeNodeIcon-prodCat',
                0: 'treeNodeIcon-networkDefault-model',
                1: 'treeNodeIcon-router-model',
                2: 'treeNodeIcon-computer-model',
                3: 'treeNodeIcon-workStation-model',
                4: 'treeNodeIcon-swith-model',
                5: 'treeNodeIcon-printer-model',
                6: 'treeNodeIcon-server-model',
                7: 'treeNodeIcon-modem-model',
                8: 'treeNodeIcon-bridge-model',
                9: 'treeNodeIcon-phone-model',
                10: 'treeNodeIcon-fax-model',
                11: 'treeNodeIcon-storageSystem-model',
                12: 'treeNodeIcon-logicalComponent-model',
                13: 'treeNodeIcon-printer-model',
                36: 'treeNodeIcon-cartridge-model',
                115: 'treeNodeIcon-serviceContract-model',
                120: 'treeNodeIcon-material-model',
                164: 'treeNodeIcon-deviceApplication-model',
                165: 'treeNodeIcon-data-entity',
                183: 'treeNodeIcon-softwareLicenseStandalone-model',
                184: 'treeNodeIcon-softwareLicenseRent-model',
                185: 'treeNodeIcon-softwareLicenseUpgrade-model',
                186: 'treeNodeIcon-softwareLicenseSubscribe-model',
                187: 'treeNodeIcon-softwareLicenseProlongation-model',
                189: 'treeNodeIcon-softwareLicenseOEM-model',
                223: 'treeNodeIcon-softwareLicense-model',
                329: 'treeNodeIcon-adapterDefault-model',
                330: 'treeNodeIcon-motherboard-model',
                331: 'treeNodeIcon-processor-model',
                332: 'treeNodeIcon-memory-model',
                333: 'treeNodeIcon-videoadapter-model',
                334: 'treeNodeIcon-soundcard-model',
                335: 'treeNodeIcon-networkAdapter-model',
                336: 'treeNodeIcon-storage-model',
                337: 'treeNodeIcon-cdAndDvdDrive-model',
                338: 'treeNodeIcon-floppyDrive-model',
                340: 'treeNodeIcon-monitor-model',
                341: 'treeNodeIcon-keyboard-model',
                342: 'treeNodeIcon-mouse-model',
                345: 'treeNodeIcon-printer-model',
                346: 'treeNodeIcon-scaner-model',
                347: 'treeNodeIcon-modem-model',
                352: 'treeNodeIcon-storageController-model',
                360: 'treeNodeIcon-ITSystem',
                376: 'treeNodeIcon-pDefault-model',
                377: 'treeNodeIcon-modem-model',
                409: 'treeNodeIcon-icon-network-node-Ke',
                410: 'treeNodeIcon-icon-commutator',
                411: 'treeNodeIcon-icon-router-configuration',
                412: 'treeNodeIcon-print-server',
                413: 'treeNodeIcon-icon-data-storage',
                414: 'treeNodeIcon-icon-win-server',
                416: 'treeNodeIcon-icon-logical-server-model',
                417: 'treeNodeIcon-icon-logical-computer-model',
                418: 'treeNodeIcon-icon-logical-commutator-model',
                419: 'treeNodeIcon-host',
                420: 'treeNodeIcon-cluster',
            },
        };
        return module
    }
);