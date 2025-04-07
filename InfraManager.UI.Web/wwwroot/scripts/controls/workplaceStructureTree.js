define(['restApiTree', 'restApiTreeControl'], function (tree, treeControl) {
    var module = {
        OwnerID: '00000000-0000-0000-0000-000000000000', // ID единственного владельца в системе

        ObjectClass: {
            Owner: 29,
            Organization: 101,
            Building: 1,
            Floor: 2,
            Room: 3,
            Workplace: 22
        },

        OwnerCreator: function (idOrDataItem) { // 29 - Владелец
            return new module.OwnerNodeViewModel(idOrDataItem);
        },
        OrganizationCreator: function (idOrDataItem, parent) { // 101 - Организация
            return new module.OrganizationNodeViewModel(idOrDataItem, parent);
        },
        BuildingCreator: function(idOrDataItem, parent) { // 1 - Здание
            return new module.BuildingNodeViewModel(idOrDataItem, parent);
        },
        FloorCreator: function(idOrDataItem, parent) { // 2 - Этаж
            return new module.FloorNodeViewModel(idOrDataItem, parent);
        },
        RoomCreator: function(idOrDataItem, parent) { // 3 - Комната
            return new module.RoomNodeViewModel(idOrDataItem, parent);
        },
        WorkplaceCreator: function(idOrDataItem, parent) { // 22 - Рабочее место
            return new module.WorkplaceNodeViewModel(idOrDataItem, parent);
        },

        Creator: function (id, classId) {
            if (!module.creatorsMapping) {
                module.creatorsMapping = {};
                module.creatorsMapping[module.ObjectClass.Owner] = module.OwnerCreator;
                module.creatorsMapping[module.ObjectClass.Organization] = module.OrganizationCreator;
                module.creatorsMapping[module.ObjectClass.Building] = module.BuildingCreator;
                module.creatorsMapping[module.ObjectClass.Floor] = module.FloorCreator;
                module.creatorsMapping[module.ObjectClass.Room] = module.RoomCreator;
                module.creatorsMapping[module.ObjectClass.Workplace] = module.WorkplaceCreator;
            }

            return module.creatorsMapping[classId](id);
        },

        OwnerNodeViewModel: function (idOrDataItem) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'IMObjID');

            tree.DataNodeViewModel.call(
                self,
                '/api/owners/' + id,
                id,
                module.ObjectClass.Owner,
                null,
                [ new module.OrganizationsCollection() ]);

            self._getIconCss = function () {
                return 'treeNodeIcon-owner';
            };

            self.setData = function (data) {
                self.text(data.Name);
            }

            self.selectable(false);
        },

        OrganizationNodeViewModel: function (idOrDataItem, parent) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            self._getChildOrganizationCollections = function (id) {
                return [
                    new module.SubdivisionsCollection('organizationID=' + id + '&onlyRoots=true')
                ];
            }

            tree.DataNodeViewModel.call(
                self,
                '/api/organizations/' + id,
                id,
                module.ObjectClass.Organization,
                parent,
                [
                    new module.BuildingsCollection('organizationID=' + id)
                ]
            );

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || module.OwnerCreator(module.OwnerID);
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-organization';
            };

            self.selectable(false);
        },

        BuildingNodeViewModel: function(idOrDataItem, parent) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            tree.DataNodeViewModel.call(
                self,
                '/api/buildings/' + id,
                id,
                module.ObjectClass.Building,
                parent,
                [
                    new module.FloorsCollection('buildingID=' + id)
                ]);

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || (data.OrganizationID ? new module.OrganizationNodeViewModel(data.OrganizationID) : null);
                self.IMObjID = tree.getID(data, 'IMObjID');
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-building';
            };

            self.selectable(false);
        },

        FloorNodeViewModel: function(idOrDataItem, parent) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            tree.DataNodeViewModel.call(
                self,
                '/api/floors/' + id,
                id,
                module.ObjectClass.Floor,
                parent,
                [
                    new module.RoomsCollection('floorID=' + id)
                ]);

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || (data.BuildingID ? new module.BuildingNodeViewModel(data.BuildingID) : null);
                self.IMObjID = tree.getID(data, 'IMObjID');
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-floor';
            };

            self.selectable(false);
        },

        RoomNodeViewModel: function(idOrDataItem, parent) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');
            var uri = Number.isInteger(id)
                ? '/api/rooms/' + id
                : '/api/rooms/list?IMObjID=' + id;

            tree.DataNodeViewModel.call(
                self,
                uri,
                id,
                module.ObjectClass.Room,
                parent,
                [
                    new module.WorkplacesCollection('roomID=' + id)
                ]);

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || (data.FloorID ? new module.FloorNodeViewModel(data.FloorID) : null);
                self.IMObjID = tree.getID(data, 'IMObjID');
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-room';
            };

            self.selectable(false);
        },

        WorkplaceNodeViewModel: function(idOrDataItem, parent) {
            var self = this;
            var id;
            //Проверка для избежания возникновения ошибки, если пользователь без рабочего места
            if (idOrDataItem){
                id = tree.getID(idOrDataItem, 'ID');
            }
            else{
                //Дефолтный id отсутствующего рабочего места
                id = '0';
            }
            
            var restUri = '/api/workplaces/' + id;
            var webSearcherUri = '/api/workplaces/list?IMObjID=' + id;
            var uri = Number.isInteger(id) ? restUri : webSearcherUri;
            
            tree.DataNodeViewModel.call(
                self,
                uri,
                id,
                module.ObjectClass.Workplace,
                parent,
                []
            );

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || (data.RoomID ? new module.RoomNodeViewModel(data.RoomID) : null);
                self.IMObjID = tree.getID(data, 'IMObjID');
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-workplace';
            };

            self.selectable(true);
        },

        OwnersCollection: function () {
            tree.TreeNodeCollectionBase.call(this, '/api/owners?take=1', module.OwnerCreator);
        },

        OrganizationsCollection: function () {
            tree.TreeNodeCollectionBase.call(this, '/api/organizations/reports/allOrganizations', module.OrganizationCreator);
        },

        BuildingsCollection: function(queryFilter) {
            //TODO: использовть везде /api/buildings когда obsolete метод будет удален
            tree.TreeNodeCollectionBase.call(this, queryFilter ? '/api/buildings/list?' + queryFilter : '/api/buildings', module.BuildingCreator);
        },

        FloorsCollection: function(queryFilter) {
            //TODO: использовть везде /api/floors когда obsolete метод будет удален
            tree.TreeNodeCollectionBase.call(this, queryFilter ? '/api/floors/list?' + queryFilter : '/api/floors', module.FloorCreator);
        },

        RoomsCollection: function(queryFilter) {
            //TODO: использовть везде /api/rooms когда obsolete метод будет удален
            tree.TreeNodeCollectionBase.call(this, queryFilter ? '/api/rooms/list?' + queryFilter : '/api/rooms', module.RoomCreator);
        },

        WorkplacesCollection: function(queryFilter) {
            //TODO: использовть везде /api/workplaces когда obsolete метод будет удален
            tree.TreeNodeCollectionBase.call(this, queryFilter ? '/api/workplaces/list?' + queryFilter : '/api/workplaces', module.WorkplaceCreator);
        },

        UserSelectOrganizationTreeViewModel: function (fetcher, options) {
            var self = this;
            
            tree.LazyLoadingTreeViewModel.call(self, fetcher, new module.OwnersCollection(), module.Creator);
            
            let selectableClasses = options && options.SelectableClasses ? options.SelectableClasses : [];
            self.subscribe(tree.TreeViewEvents.onNodeAdded, function (sender, args) {
                args.node.selectable(selectableClasses.length === 0 || selectableClasses.includes(args.node.classId));
            });
        },

        Control: function (options) {
            var self = this;
            
            let fetcher = new tree.Fetcher('treeControlItems');
            let locationTree = new module.UserSelectOrganizationTreeViewModel(fetcher, options);

            let caption = getTextResource('LocationCaption');
            treeControl.Control.call(self, caption, locationTree);
        }
    };

    return module;
});