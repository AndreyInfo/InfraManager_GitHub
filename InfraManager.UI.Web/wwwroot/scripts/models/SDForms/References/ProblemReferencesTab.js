define(['jquery', 'models/SDForms/References/ReferencesTab', 'groupOperation'], function ($, referencesTabModule, groupOperation) {
    const module = {
        ViewModel: function (form, addProblemsViewModelCreator, removeReferencesViewModelCreator, options) {
            referencesTabModule.SetupMenuOptions(options, {
                addNew: 'AddNewProblem',
                addReference: 'AddProblemReference',
                remove: 'RemoveProblem'
            });
            referencesTabModule.TableViewModel.call(this, form, addProblemsViewModelCreator, options);
            this.title = 'LinkProblems';
            this.appendTitle = 'AddProblemReferenceDialogTitle';
            this.removeObjectTitle = 'ProblemRemoving';
            this.viewDetails = function (obj, fh) {
                fh.ShowProblem(obj.IMObjID, fh.Mode.Default);
            };
            this.createNew = function (fh) {
                var retD = $.Deferred();
                $.when(fh.ShowProblemRegistration()).done(function (problemID) {
                    var groupOperationViewModel = addProblemsViewModelCreator([{ IMObjID: problemID }]);
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
                return referencesTabModule.DeleteMany(new module.RemoveProblemViewModel(selectedReferences));
            };
        },
        AddMassIncidentViewModel: function (uri, selectedProblems) {
            groupOperation.ViewModelBase.call(this, selectedProblems, {
                batchSize: 1,
                ajax: groupOperation.PostAjaxOptions
            });
            this._getUrl = function () { return uri + '/problems' };
            this._getData = function (problem) { return JSON.stringify({ ReferenceID: problem.IMObjID }); };
        },
        RemoveMassIncidentViewModel: function (uri, selectedProblems) {
            groupOperation.ViewModelBase.call(this, selectedProblems, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (p) { return uri + '/problems/' + p.IMObjID; };
            this._getData = function () { return null; };
        },
        RemoveProblemViewModel: function (selectedProblems) {
            groupOperation.ViewModelBase.call(this, selectedProblems, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (p) { return '/api/problems/' + p.IMObjID; };
            this._getData = function () { return null; };
        }
    };

    return module;
});

