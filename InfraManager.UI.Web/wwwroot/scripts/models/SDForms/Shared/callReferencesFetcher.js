define(['jquery', 'ajax', 'groupOperation'], function ($, ajax, groupOperation) {
    var callIDProp = 'CallID';
    var refIDProp = 'ObjectID';

    var get = function (uri, prop, $ctrl) {
        var retD = $.Deferred();

        new ajax.control().Ajax(
            $ctrl, {
            method: 'GET',
            url: uri
        }, function (data) {
            retD.resolve({ ids: data.map(function (item) { return item[prop]; }), success: true });
        }, function () {
            retD.resolve({ ids: [], success: false });
        });

        return retD.promise();
    };

    var module = {
        GetCallProblems: function (callUri, $ctrl) {
            return get(callUri + '/problems', refIDProp, $ctrl);
        },
        GetCallChangeRequests: function (callUri, $ctrl) {
            return get(callUri + '/changeRequests', refIDProp, $ctrl);
        },
        GetProblemCalls: function (problemUri, $ctrl) {
            return get(problemUri + '/calls', callIDProp, $ctrl);
        },
        GetChangeRequestCalls: function (changeRequestUri, $ctrl) {
            return get(changeRequestUri + '/calls', callIDProp, $ctrl);
        },
        PostGroupViewModel: function (uri, ids, $ctrl, onSuccess, onComplete) {
            groupOperation.ViewModelBase.call(
                this,
                ids, {
                div: $ctrl,
                batchSize: 1, // из-за того что каждый запрос меняет массовый инцидент, то их нельзя запускать параллельно
                ajax: {
                    method: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                }
            });

            this._getUrl = function () { return uri; };
            this._getData = function (id) { return JSON.stringify({ ID: id }) };
            this._onSuccess = onSuccess || this._onSuccess;
            this._onComplete = onComplete || this._onComplete;
        },
        DeleteGroupViewModel: function (uri, ids, $ctrl, onSuccess, onComplete) {
            groupOperation.ViewModelBase.call(
                this,
                ids, {
                div: $ctrl,
                batchSize: 1,
                ajax: {
                    method: 'DELETE',
                    dataType: 'text',
                    contentType: 'application/json',
                }
            });

            this._getUrl = function (id) { return uri + '/' + id; };
            this._onSuccess = onSuccess || this._onSuccess;
            this._onComplete = onComplete || this._onComplete;
        },
        ReferenceMultipleProblemsWithCallAsync: function (callUri, problemIDs, $ctrl, onSuccess, onComplete) {
            new module.PostGroupViewModel(callUri + '/problems', problemIDs, $ctrl, onSuccess, onComplete).start();
        },
        ReferenceMultipleChangeRequestsWithCallAsync: function (callUri, rfcIDs, $ctrl, onSuccess, onComplete) {
            new module.PostGroupViewModel(callUri + '/changeRequests', rfcIDs, $ctrl, onSuccess, onComplete).start();
        },
        ReferenceMultipleCallsWithProblemAsync: function (problemUri, callIDs, $ctrl, onSuccess, onComplete) {
            new module.PostGroupViewModel(problemUri + '/calls', callIDs, $ctrl, onSuccess, onComplete).start();
        },
        ReferenceMultipleCallsWithChangeRequestAsync: function (rfcUri, callIDs, $ctrl, onSuccess, onComplete) {
            new module.PostGroupViewModel(rfcUri + '/calls', callIDs, $ctrl, onSuccess, onComplete).start();
        },
        RemoveMultipleProblemReferencesWithCallAsync: function (callUri, problemIDs, $ctrl, onSuccess, onComplete) {
            new module.DeleteGroupViewModel(callUri + '/problems', problemIDs, $ctrl, onSuccess, onComplete).start();
        },
        RemoveMultipleChangeRequestReferencesWithCallAsync: function (callUri, rfcIDs, $ctrl, onSuccess, onComplete) {
            new module.DeleteGroupViewModel(callUri + '/changeRequests', rfcIDs, $ctrl, onSuccess, onComplete).start();
        },
        RemoveMultipleCallReferencesWithProblemAsync: function (problemUri, callIDs, $ctrl, onSuccess, onComplete) {
            new module.DeleteGroupViewModel(problemUri + '/calls', callIDs, $ctrl, onSuccess, onComplete).start();
        },
        RemoveMultipleCallReferencesWithChangeRequestAsync: function (rfcUri, callIDs, $ctrl, onSuccess, onComplete) {
            new module.DeleteGroupViewModel(rfcUri + '/calls', callIDs, $ctrl, onSuccess, onComplete).start();
        },
    };

    return module;
});