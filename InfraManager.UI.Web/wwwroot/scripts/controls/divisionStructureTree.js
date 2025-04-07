define(['restApiTree'], function (tree) { // TODO: Это копипаста organizationStructureTree.js
    var module = {
        OwnerID: '00000000-0000-0000-0000-000000000000', // ID единственного владельца в системе

        ObjectClass: {
            Owner: 29,
            Organization: 101,
            Subdivision: 102
        },

        OwnerCreator: function (idOrDataItem) { // 29 - Владелец
            return new module.OwnerNodeViewModel(idOrDataItem);
        },
        OrganizationCreator: function (idOrDataItem, parent) { // 101 - Организация
            return new module.OrganizationNodeViewModel(idOrDataItem, parent);
        },
        SubdivisionCreator: function (idOrDataItem, parent) { // 102 - Подразеделение
            return new module.SubdivisionNodeViewModel(idOrDataItem, parent);
        },

        Creator: function (id, classId) {
            if (!module.creatorsMapping) {
                module.creatorsMapping = {};
                module.creatorsMapping[module.ObjectClass.Owner] = module.OwnerCreator;
                module.creatorsMapping[module.ObjectClass.Organization] = module.OrganizationCreator;
                module.creatorsMapping[module.ObjectClass.Subdivision] = module.SubdivisionCreator;
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

            tree.DataNodeViewModel.call(
                self,
                '/api/organizations/' + id,
                id,
                module.ObjectClass.Organization,
                parent,
                [
                    new module.SubdivisionsCollection('organizationID=' + id + '&onlyRoots=true')
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

        SubdivisionNodeViewModel: function (idOrDataItem, parent) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            self._getChildSubdivisionCollections = function (id) {
                return [
                    new module.SubdivisionsCollection('parentID=' + id)
                ];
            }
            tree.DataNodeViewModel.call(
                self,
                '/api/subdivisions/' + id,
                id,
                module.ObjectClass.Subdivision,
                parent,
                self._getChildSubdivisionCollections(id)
            );

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent
                    || (data.SubdivisionID
                        ? new module.SubdivisionNodeViewModel(data.SubdivisionID)
                        : new module.OrganizationNodeViewModel(data.OrganizationID));
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-subdivision';
            };
            
            self.selectable(true);
        },

        OwnersCollection: function () {
            tree.TreeNodeCollectionBase.call(this, '/api/owners?take=1', module.OwnerCreator);
        },

        OrganizationsCollection: function () {
            tree.TreeNodeCollectionBase.call(this, '/api/organizations', module.OrganizationCreator);
        },

        SubdivisionsCollection: function (queryFilter) {
            tree.TreeNodeCollectionBase.call(this, '/api/subdivisions?' + queryFilter, module.SubdivisionCreator);
        },

        UserSelectOrganizationTreeViewModel: function (fetcher) {
            tree.LazyLoadingTreeViewModel.call(this, fetcher, new module.OwnersCollection(), module.Creator);
        }
    };

    return module;
});