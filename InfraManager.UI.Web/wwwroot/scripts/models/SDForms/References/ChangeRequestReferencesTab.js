define(['jquery', 'models/SDForms/References/ReferencesTab', 'groupOperation'], function ($, referencesTabModule, groupOperation) {
    const module = {
        ViewModel: function (form, addChangeRequestsViewModelCreator, removeReferencesViewModelCreator, options) {
            referencesTabModule.SetupMenuOptions(options, {
                addNew: 'ChangeRequest_Create',
                addReference: 'ChangeRequestReference_Add',
                remove: 'ChangeRequest_Delete'
            });
            referencesTabModule.TableViewModel.call(this, form, addChangeRequestsViewModelCreator, options);
            this.title = 'LinkChangeRequests';
            this.appendTitle = 'AddChangeRequestReferenceDialogTitle';
            this.removeObjectTitle = 'ChangeRequest_DeleteConfirmTitle';
            this.viewDetails = function (obj, fh) {
                fh.ShowRFC(obj.IMObjID, fh.Mode.Default);
            };
            this.createNew = function (fh) {
                var retD = $.Deferred();
                $.when(fh.ShowRFCRegistration()).done(function (rfcID) {
                    var groupOperationViewModel = addChangeRequestsViewModelCreator([{ IMObjID: rfcID }]);
                    groupOperationViewModel.subscribeSuccess(function (obj, response) {
                        retD.resolve([response.ReferenceID]);
                    });
                    groupOperationViewModel.start();
                });
                return retD.promise();
            };
            this.deleteReferences = function (selectedReferences) {
                var groupOperation = removeReferencesViewModelCreator(selectedReferences);
                return referencesTabModule.DeleteMany(groupOperation);
            };
            this.deleteObjects = function (selectedReferences) {
                return referencesTabModule.DeleteMany(new module.RemoveChangeRequestViewModel(selectedReferences));
            };
        },
        AddMassIncidentViewModel: function (uri, selectedChangeRequests) {
            groupOperation.ViewModelBase.call(this, selectedChangeRequests, {
                batchSize: 1,
                ajax: groupOperation.PostAjaxOptions
            });
            this._getUrl = function () { return uri + '/changeRequests' };
            this._getData = function (rfc) { return JSON.stringify({ ReferenceID: rfc.IMObjID }); };
        },
        RemoveMassIncidentViewModel: function (uri, selectedCalls) {
            groupOperation.ViewModelBase.call(this, selectedCalls, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (rfc) { return uri + '/changeRequests/' + rfc.IMObjID; };
            this._getData = function () { return null; };
        },
        RemoveChangeRequestViewModel: function (selectedCalls) {
            groupOperation.ViewModelBase.call(this, selectedCalls, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (rfc) { return '/api/changeRequests/' + rfc.IMObjID; };
            this._getData = function () { return null; };
        }
    };

    return module;
});

