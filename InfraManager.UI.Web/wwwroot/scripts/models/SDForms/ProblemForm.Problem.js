define(['knockout', 'jquery', 'models/SDForms/SDForm.User', 'groupOperation'], function (ko, $, userLib, groupOperation) {
    var module = {
        Problem: function (parentSelf, pData) {
            var pself = this;
            var self = parentSelf;
            if (!pData.EntityStateID && !pData.WorkflowSchemeID && pData.EntityStateName) {
                self.IsReadOnly(true);
            }
            //
            pself.ClassID = 702;
            //
            if (pData.ID)
                pself.ID = ko.observable(pData.ID)
            else pself.ID = ko.observable('');
            if (pData.Number)
                pself.Number = ko.observable(pData.Number)
            else pself.Number = ko.observable('');
            if (pData.UrgencyID)
                pself.UrgencyID = ko.observable(pData.UrgencyID)
            else pself.UrgencyID = ko.observable('');
            if (pData.UrgencyName)
                pself.UrgencyName = ko.observable(pData.UrgencyName)
            else pself.UrgencyName = ko.observable('');
            if (pData.InfluenceID)
                pself.InfluenceID = ko.observable(pData.InfluenceID)
            else pself.InfluenceID = ko.observable('');
            if (pData.InfluenceName)
                pself.InfluenceName = ko.observable(pData.InfluenceName)
            else pself.InfluenceName = ko.observable('');
            if (pData.EntityStateID)
                pself.EntityStateID = ko.observable(pData.EntityStateID)
            else pself.EntityStateID = ko.observable('');
            //
            if (pData.WorkflowSchemeID)
                pself.WorkflowSchemeID = ko.observable(pData.WorkflowSchemeID);
            else pself.WorkflowSchemeID = ko.observable(null);
            //
            if (pData.EntityStateName)
                pself.EntityStateName = ko.observable(pData.EntityStateName)
            else pself.EntityStateName = ko.observable('');
            pself.WorkflowImageSource = ko.observable(pData.WorkflowImageSource ? pData.WorkflowImageSource : null);
            if (pData.TypeName)
                pself.TypeName = ko.observable(pData.TypeName)
            else pself.TypeName = ko.observable('');
            if (pData.TypeID)
                pself.TypeID = ko.observable(pData.TypeID)
            else pself.TypeID = ko.observable('');
            if (pData.PriorityName)
                pself.PriorityName = ko.observable(pData.PriorityName)
            else pself.PriorityName = ko.observable('');
            if (pData.PriorityColor)
                pself.PriorityColor = ko.observable(pData.PriorityColor)
            else pself.PriorityColor = ko.observable('');
            if (pData.PriorityID)
                pself.PriorityID = ko.observable(pData.PriorityID)
            else pself.PriorityID = ko.observable('');
            if (pData.Summary)
                pself.Summary = ko.observable(pData.Summary)
            else pself.Summary = ko.observable('');
            if (pData.Description)
                pself.Description = ko.observable(pData.Description)
            else pself.Description = ko.observable('');
            if (pData.Solution)
                pself.Solution = ko.observable(pData.Solution)
            else pself.Solution = ko.observable('');
            if (pData.Cause)
                pself.Cause = ko.observable(pData.Cause)
            else pself.Cause = ko.observable('');
            if (pData.Fix)
                pself.Fix = ko.observable(pData.Fix)
            else pself.Fix = ko.observable('');

            if (pData.HTMLDescription)
                pself.HTMLDescription = ko.observable(pData.HTMLDescription)
            else pself.HTMLDescription = ko.observable('');
            if (pData.HTMLSolution)
                pself.HTMLSolution = ko.observable(pData.HTMLSolution)
            else pself.HTMLSolution = ko.observable('');
            if (pData.HTMLCause)
                pself.HTMLCause = ko.observable(pData.HTMLCause)
            else pself.HTMLCause = ko.observable('');
            if (pData.HTMLFix)
                pself.HTMLFix = ko.observable(pData.HTMLFix)
            else pself.HTMLFix = ko.observable('');

            pself.ServiceID = ko.observable(pData.ServiceID ? pData.ServiceID : null);

            pself.ServiceCategoryName = ko.observable(pData.ServiceCategoryName ? pData.ServiceCategoryName : '');
            pself.ServiceName = ko.observable(pData.ServiceName ? pData.ServiceName : '');

            // Owner
            if (pData.OwnerID) {
                pself.OwnerID = ko.observable(pData.OwnerID)
            } else {
                pself.OwnerID = ko.observable('');
            };

            pself.OwnerLoaded = ko.observable(false);
            pself.Owner = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.owner, parentSelf.EditOwner));

            // Initiator
            if (pData.InitiatorID) {
                pself.InitiatorID = ko.observable(pData.InitiatorID);
            } else {
                pself.InitiatorID = ko.observable('');
            };

            pself.InitiatorLoaded = ko.observable(false);
            pself.Initiator = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.workInitiator, parentSelf.EditInitiator));

            pself.QueueID = ko.observable(pData.QueueID ? pData.QueueID : '');
            pself.QueueName = ko.observable(pData.QueueName ? pData.QueueName : '');
            pself.QueueLoaded = ko.observable(false);
            pself.Queue = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.queueExecutor, parentSelf.EditQueue));
            //
            pself.ExecutorID = ko.observable(pData.ExecutorID ? pData.ExecutorID : '');
            pself.ExecutorLoaded = ko.observable(false);
            pself.Executor = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.executor, parentSelf.EditExecutor));

            //
            if (pData.UtcDateDetected)
                pself.UtcDateDetected = ko.observable(parseDate(pData.UtcDateDetected))
            else pself.UtcDateDetected = ko.observable('');
            pself.UtcDateDetectedJS = pData.UtcDateDetected;
            if (pData.UtcDatePromised)
                pself.UtcDatePromised = ko.observable(parseDate(pData.UtcDatePromised))
            else pself.UtcDatePromised = ko.observable('');
            pself.UtcDatePromisedJS = pData.UtcDatePromised;
            if (pData.UtcDatePromised)
                pself.UtcDatePromisedDT = ko.observable(new Date(parseInt(pData.UtcDatePromised)));
            else pself.UtcDatePromisedDT = ko.observable(null);
            //
            if (pData.UtcDateClosed)
                pself.UtcDateClosed = ko.observable(parseDate(pData.UtcDateClosed))
            else pself.UtcDateClosed = ko.observable('');
            pself.UtcDateClosedJS = pData.UtcDateClosed;
            if (pData.UtcDateSolved)
                pself.UtcDateSolved = ko.observable(parseDate(pData.UtcDateSolved))
            else pself.UtcDateSolved = ko.observable('');
            pself.UtcDateSolvedJS = pData.UtcDateSolved;
            if (pData.UtcDateModified)
                pself.UtcDateModified = ko.observable(parseDate(pData.UtcDateModified))
            else pself.UtcDateModified = ko.observable('');
            //
            if (pData.NegotiationCount)
                pself.NegotiationCount = ko.observable(pData.NegotiationCount);
            else pself.NegotiationCount = ko.observable(0);
            if (pData.HaveUnvotedNegotiation != null)
                pself.HaveUnvotedNegotiation = ko.observable(pData.HaveUnvotedNegotiation);
            else pself.HaveUnvotedNegotiation = ko.observable(false);
            //
            if (pData.UnreadNoteCount)
                pself.UnreadNoteCount = ko.observable(pData.UnreadNoteCount);
            else pself.UnreadNoteCount = ko.observable(0);
            if (pData.NoteCount)
                pself.NoteCount = ko.observable(pData.NoteCount);
            else pself.NoteCount = ko.observable(0);
            pself.TotalNotesCount = ko.computed(function () {
                return parseInt(pself.NoteCount());
            });
            //
            if (pData.DependencyObjectCount)
                pself.DependencyObjectCount = ko.observable(pData.DependencyObjectCount);
            else pself.DependencyObjectCount = ko.observable(0);
            //
            if (pData.WorkOrderCount)
                pself.WorkOrderCount = ko.observable(pData.WorkOrderCount);
            else pself.WorkOrderCount = ko.observable(0);
            //
            if (pData.CallCount)
                pself.CallCount = ko.observable(pData.CallCount);
            else pself.CallCount = ko.observable(0);

            if (pData.ManhoursString)
                pself.ManHours = ko.observable(pData.ManhoursString)
            else pself.ManHours = ko.observable('');

            if (pData.ManhoursInMinutes)
                pself.ManhoursInMinutes = ko.observable(pData.ManhoursInMinutes)
            else pself.ManhoursInMinutes = ko.observable(0);

            if (pData.ManhoursNormInMinutes)
                pself.ManhoursNorm = ko.observable(pData.ManhoursNormInMinutes)
            else pself.ManhoursNorm = ko.observable(0);
            //
            if (pData.ManhoursNormString)
                pself.ManhoursNormString = ko.observable(pData.ManhoursNormString)
            else pself.ManhoursNormString = ko.observable('');

            if (pData.ProblemCauseID)
                pself.ProblemCauseID = ko.observable(pData.ProblemCauseID)
            else pself.ProblemCauseID = ko.observable('');
            if (pData.ProblemCauseName)
                pself.ProblemCauseName = ko.observable(pData.ProblemCauseName)
            else pself.ProblemCauseName = ko.observable('');
            //
            pself.NumberName = ko.computed(function () {
                return `№${pself.Number()} ${pself.Summary()}`;
            });
            //
            if (pData.OnWorkOrderExecutorControl)
                pself.OnWorkOrderExecutorControl = ko.observable(pData.OnWorkOrderExecutorControl);
            else pself.OnWorkOrderExecutorControl = ko.observable(false);
            //
            pself.UserField1 = ko.observable(pData.UserField1 ? pData.UserField1 : '');
            pself.UserField2 = ko.observable(pData.UserField2 ? pData.UserField2 : '');
            pself.UserField3 = ko.observable(pData.UserField3 ? pData.UserField3 : '');
            pself.UserField4 = ko.observable(pData.UserField4 ? pData.UserField4 : '');
            pself.UserField5 = ko.observable(pData.UserField5 ? pData.UserField5 : '');
            //
            pself.UserField1Name = pData.UserField1Name;
            pself.UserField2Name = pData.UserField2Name;
            pself.UserField3Name = pData.UserField3Name;
            pself.UserField4Name = pData.UserField4Name;
            pself.UserField5Name = pData.UserField5Name;
            pself.UserFieldNamesDictionary = pData.UserFieldNamesDictionary;
            //
            pself.CallList = pData.CallList;//список связанных заявок (при создании проблемы по заявке)
            //
            pself.MassIncidentId = pData.MassIncidentId; // Связанный массовый инцидент (при создании проблемы по массовому инциденту)
            //
            pself.AddAs = function () {
                return pself.ID() && pself.ID() !== '00000000-0000-0000-0000-000000000000';
            };
            //
            pself.AddByCallList = function () {
                return pself.CallList && pself.CallList.length !== 0;
            };
            //
            pself.AddByMassIncident = function () {
                return pself.MassIncidentId;
            };

            const uri = `/api/problems/${pData.ID}`;
            pself.getCalls = function ($ctrl) {
                var retvalD = $.Deferred();
                require(['models/SDForms/Shared/callReferencesFetcher'], function (callRefs) {
                    $.when(callRefs.GetProblemCalls(uri, $ctrl)).done(function (result) {
                        if (result.success) {
                            pself.CallCount(result.ids.length);
                        }
                        retvalD.resolve({ IDs: result.ids });
                    });
                });
                return retvalD.promise();
            };
            pself.addCalls = function (ids, onAdd, $ctrl) {
                var retD = $.Deferred();
                var processed = 0;
                require(['models/SDForms/Shared/callReferencesFetcher'], function (callRefs) {
                    callRefs.ReferenceMultipleCallsWithProblemAsync(
                        uri,
                        ids,
                        $ctrl,
                        function (callID) {
                            processed++;
                            onAdd(callID);
                        },
                        function () {
                            self.Load(pData.ID)
                        },
                        function () {
                            pself.CallCount(pself.CallCount() + processed);
                            retD.resolve(processed === ids.length);
                        }
                    );
                });
                return retD.promise();
            };
            pself.deleteCalls = function (ids, onDelete, $ctrl) {
                var retD = $.Deferred();
                var processed = 0;

                require(['models/SDForms/Shared/callReferencesFetcher'], function (callRefs) {
                    callRefs.RemoveMultipleCallReferencesWithProblemAsync(
                        uri,
                        ids,
                        $ctrl,
                        function (callID) {
                            processed++;
                            onDelete(callID);
                        },
                        function () {
                            self.Load(pData.ID)
                        },
                        function () {
                            pself.CallCount(pself.CallCount() - processed);
                            retD.resolve(processed === ids.length);
                        }
                    );
                });
                return retD.promise();
            };

            // Список для вкладки Инфраструктура (добавление/удаление)
            const url = '/bff/ProblemDependencies';
            pself.AddDependencies = function (data, onAdd, $ctrl, list) {
                let retD = $.Deferred();
                const addDependencies = function (items) {
                    groupOperation.ViewModelBase.call(this, items, {
                        div: $ctrl,
                        ajax: {
                            dataType: 'json',
                            method: 'POST',
                            contentType: 'application/json',
                            traditional: true,
                        },
                        batchSize: 1
                    });
                    this._getUrl = function () {
                        return url;
                    };
                    this._getData = function (item) {
                        return JSON.stringify({
                            ProblemID: pself.ID(),
                            ObjectID: item.ID,
                            ClassID: item.ClassID,
                            ObjectLocation: item.FullObjectLocation,
                            ObjectName: item.FullObjectName,
                        });
                    };
                    this._onSuccess = function (item, response) {
                        pself.DependencyObjectCount(pself.DependencyObjectCount() + 1);
                        if (onAdd) {
                            onAdd(response);
                        }
                    }
                    this._onComplete = function () {
                        retD.resolve();
                    }
                };
                const itemsToAdd = !list || list.length === 0 ? data : data.filter(el => !list.find(link => link.ObjectID === el.ID));
                new addDependencies(itemsToAdd).start();
                return retD.promise();
            }

            pself.RemoveDependencies = function (data, onDelete, $ctrl) {
                let retD = $.Deferred();
                const removeDependencies = function (items) {
                    groupOperation.ViewModelBase.call(this, items, {
                        div: $ctrl,
                        ajax: groupOperation.DeleteAjaxOptions,
                        batchSize: 1
                    });
                    this._getUrl = function (item) {
                        return `${url}/${item}`;
                    }
                    this._onSuccess = function (item) {
                        pself.DependencyObjectCount(pself.DependencyObjectCount() - 1);
                        if (onDelete) {
                            onDelete(item);
                        }
                    };
                    this._onComplete = function () {
                        retD.resolve();
                    }
                }
                new removeDependencies(data).start();
                return retD.promise();
            }
        }
    };
    return module;
});