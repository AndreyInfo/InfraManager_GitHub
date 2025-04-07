define(['restApiTree', 'checkbox'], function (tree, checkbox) {
    var module = {
        OwnerID: '00000000-0000-0000-0000-000000000000', // ID единственного владельца в системе
        
        ObjectClass: { 
            Owner: 29,
            Organization: 101,
            Subdivision: 102,
            User: 9
        },
         
        Factory: function () {
            var self = this;

            this._getUsersUri = function (subdivisionID) {
                return '/api/users?subdivisionID=' + subdivisionID;
            };

            this.createOwner = function (idOrDataItem) {
                return new module.OwnerNodeViewModel(idOrDataItem, self);
            };
            this.createOrganization = function (idOrDataItem, parent) {
                return new module.OrganizationNodeViewModel(idOrDataItem, parent, self);
            };
            this.createSubdivision = function (idOrDataItem, parent) {
                return new module.SubdivisionNodeViewModel(idOrDataItem, parent, self);
            };
            this.createUser = function (idOrDataItem, parent) {
                return new module.UserNodeViewModel(idOrDataItem, parent, self);
            };
            this.createOwners = function () {
                return new tree.TreeNodeCollectionBase('/api/owners?take=1', self.createOwner);
            }
            this.createOrganizations = function () {
                return new tree.TreeNodeCollectionBase('/api/organizations/reports/allOrganizations', self.createOrganization);
            };
            this.createSubdivisions = function (parent) {
                var parentQuery = parent.organizationID ? 'organizationID=' + parent.organizationID + '&onlyRoots=true' : 'parentID=' + parent.parentSubdivisionID;
                return new tree.TreeNodeCollectionBase('/api/subdivisions?' + parentQuery, self.createSubdivision);
            };
            this.createUsers = function (subdivisionID) {
                return new tree.TreeNodeCollectionBase(self._getUsersUri(subdivisionID), self.createUser);
            };

            var classMapping = {};
            classMapping[module.ObjectClass.Owner] = function () { return self.createOwner; };
            classMapping[module.ObjectClass.Organization] = function () { return self.createOrganization; };
            classMapping[module.ObjectClass.Subdivision] = function () { return self.createSubdivision; };
            classMapping[module.ObjectClass.User] = function () { return self.createUser; };

            this.create = function (id, classId) {
                var creator = classMapping[classId]();
                return creator(id);
            };
        },
        
        OwnerNodeViewModel: function (idOrDataItem, factory) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'IMObjID');

            tree.DataNodeViewModel.call(
                self,
                '/api/owners/' + id,
                id,
                module.ObjectClass.Owner,
                null,
                [ factory.createOrganizations() ]);

            self._getIconCss = function () {
                return 'treeNodeIcon-owner';
            };

            self.setData = function (data) {
                self.text(data.Name);               
            }

            self.selectable(false);
        },
        
        OrganizationNodeViewModel: function (idOrDataItem, parent, factory) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            self._getChildOrganizationCollections = function (id) {
                return [
                    factory.createSubdivisions({ organizationID: id })                    
                ];
            }
            
            tree.DataNodeViewModel.call(
                self,
                '/api/organizations/' + id,
                id,
                module.ObjectClass.Organization,
                parent,
                self._getChildOrganizationCollections(id)
            );

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || factory.createOwner(module.OwnerID);
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-organization';
            };

            self.selectable(false);
        },
        
        SubdivisionNodeViewModel: function (idOrDataItem, parent, factory) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            tree.DataNodeViewModel.call(
                self,
                '/api/subdivisions/' + id,
                id,
                module.ObjectClass.Subdivision,
                parent,
                [
                    factory.createSubdivisions({ parentSubdivisionID: id }),
                    factory.createUsers(id)
                ]
                );

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent
                    || (data.SubdivisionID
                        ? factory.createSubdivision(data.SubdivisionID)
                        : factory.createOrganization(data.OrganizationID));
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-subdivision';
            };

            self.selectable(false);
        },
        
        UserNodeViewModel: function (idOrDataItem, parent, factory) {
            var self = this;
            var id = tree.getID(idOrDataItem, 'ID');

            tree.DataNodeViewModel.call(
                self,
                '/api/users/' + id,
                id,
                module.ObjectClass.User,
                parent,
                []);

            self.setData = function (data) {
                self.text(data.Name);
                self.parent = self.parent || (data.SubdivisionID ? factory.createSubdivision(data.SubdivisionID) : null);
            };

            self._getIconCss = function () {
                return 'treeNodeIcon-user';
            };
        },      
        
        UserSelectOrganizationTreeViewModel: function (fetcher, factory) {
            tree.LazyLoadingTreeViewModel.call(this, fetcher, factory.createOwners(), factory.create);
        },
        
        UserMultiSelectOrganizationTreeViewModel: function (fetcher, factory) {
            tree.MultiSelectTreeViewModel.call(this, tree.LazyLoadingTreeViewModel, fetcher, factory.createOwners(), factory.create, checkbox.ViewModel);
        },
    };

    return module;
});