define(['jquery', 'models/SDForms/References/ReferencesTab', 'groupOperation'], function ($, referencesTabModule, groupOperation) {
    var module = {
        ViewModel: function (form, addCallsViewModel, removeReferencesViewModelCreator, options) {
            referencesTabModule.SetupMenuOptions(options, {
                addNew: 'AddNewCall',
                addReference: 'AddCallReference',
                remove: 'RemoveCall'
            });
            referencesTabModule.TableViewModel.call(this, form, addCallsViewModel, options);
            this.title = 'AddCallReferenceDialogTitle';
            this.appendTitle = 'AddCallReferenceDialogTitle';
            this.removeObjectTitle = 'CallRemoving';

            this.viewDetails = function (obj, fh) {
                fh.ShowCall(obj.IMObjID, fh);
            };
            this.createNew = function (fh) {
                var retD = $.Deferred();
                $.when(fh.ShowCallRegistrationEngineer()).done(function (callID) {
                    var groupOperationViewModel = addCallsViewModel([{ IMObjID: callID }]);
                    groupOperationViewModel.subscribeSuccess(function (obj, response) {
                        retD.resolve([ response.ReferenceID ]);
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
                return referencesTabModule.DeleteMany(new module.RemoveCallViewModel(selectedReferences));
            };
        },
        AddMassIncidentViewModel: function (uri, selectedCalls) {
            groupOperation.ViewModelBase.call(this, selectedCalls, {
                batchSize: 1,
                ajax: groupOperation.PostAjaxOptions
            });
            this._getUrl = function () { return uri + '/calls' };
            this._getData = function (call) { return JSON.stringify({ ReferenceID: call.IMObjID }); };
        },
        RemoveMassIncidentViewModel: function (uri, selectedCalls) {
            groupOperation.ViewModelBase.call(this, selectedCalls, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (call) { return uri + '/calls/' + call.IMObjID; };
            this._getData = function () { return null; };
        },
        RemoveCallViewModel: function (selectedCalls) {
            groupOperation.ViewModelBase.call(this, selectedCalls, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (call) { return '/api/calls/' + call.IMObjID; };
            this._getData = function () { return null; };
        }
    };

    return module;
});

