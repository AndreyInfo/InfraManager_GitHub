define(['jquery', 'models/SDForms/References/ReferencesTab', 'groupOperation'], function ($, referencesTabModule, groupOperation) {
    const module = {
        ViewModel: function (form, addReferencesViewModelCreator, removeReferencesViewModelCreator, options) {
            referencesTabModule.SetupMenuOptions(options, {
                addNew: 'MassIncident_CreateNew',
                addReference: 'MassIncident_AddReference',
                remove: 'MassIncident_Delete'
            });
            referencesTabModule.TableViewModel.call(this, form, addReferencesViewModelCreator, options);
            this.title = 'ReferencedMassIncidents';
            this.appendTitle = 'SelectedReferencesMassIncident';
            this.removeObjectTitle = 'MassIncidentRemoving';

            this.viewDetails = function (obj, fh) {
                fh.ShowMassIncident(options.append.getMassIncidentUri(obj), fh.Mode.Default);
            };
            this.createNew = function (fh) {
                var retD = $.Deferred();

                $.when(fh.ShowMassIncidentRegistration()).done(function (id) {
                    var groupOperationViewModel = addReferencesViewModelCreator([{ ID: id }]);
                    var baseSuccess = groupOperationViewModel._onSuccess;
                    groupOperationViewModel._onSuccess = function (obj, response) {
                        baseSuccess(obj, response);
                        retD.resolve([ response.MassIncidentID ]);
                    };
                    groupOperationViewModel.start();
                });
                return retD.promise();
            };            

            this.deleteReferences = function (selectedReferences) {
                var groupOperation = removeReferencesViewModelCreator(selectedReferences);
                return referencesTabModule.DeleteMany(groupOperation);
            };
            this.deleteObjects = function (selectedReferences) {
                return referencesTabModule.DeleteMany(new module.RemoveViewModel(selectedReferences));
            };
        },
        AddReferencesViewModel: function (referencesName, referenceID, selectedObjects) {
            groupOperation.ViewModelBase.call(this, selectedObjects, {
                batchSize: 1,
                ajax: groupOperation.PostAjaxOptions
            });
            this._getUrl = function (obj) { return '/api/massIncidents/' + obj.ID + '/' + referencesName };
            this._getData = function () { return JSON.stringify({ ReferenceID: referenceID }); };
        },
        RemoveReferencesViewModel: function (referencesName, referenceID, selectedObjects) {
            groupOperation.ViewModelBase.call(this, selectedObjects, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (obj) { return '/api/massIncidents/' + obj.ID + '/' + referencesName + '/' + referenceID };
            this._getData = function () { return null; };
        },
        AddProblemViewModelCreator: function (problemID) {
            return function (selectedObjects) {
                return new module.AddReferencesViewModel('problems', problemID, selectedObjects);
            };
        },
        RemoveProblemViewModelCreator: function (problemID) {
            return function (selectedObjects) {
                return new module.RemoveReferencesViewModel('problems', problemID, selectedObjects);
            };
        },
        RemoveViewModel: function (selectedObjects) {
            groupOperation.ViewModelBase.call(this, selectedObjects, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (obj) { return '/api/massIncidents/' + obj.ID };
            this._getData = function () { return null; };
        }
    };

    return module;
});

