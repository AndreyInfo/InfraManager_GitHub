define(['knockout', 'jquery', 'ajax', 'models/SDForms/SDForm.User', 'models/FinanceForms/ActivesRequestSpecification'], function (ko, $, ajaxLib, userLib, specLib) {
    var module = {
        WorkOrder: function (parentSelf, woData) {
            var mself = this;
            var self = parentSelf;
            if (!woData.EntityStateID && !woData.WorkflowSchemeID && woData.EntityStateName) {
                self.IsReadOnly(true);
            }
            //
            mself.ClassID = 119;
            //
            if (woData.ID)
                mself.ID = ko.observable(woData.ID)
            else mself.ID = ko.observable('');
            if (woData.Number)
                mself.Number = ko.observable(woData.Number)
            else mself.Number = ko.observable('');
            if (woData.Name)
                mself.Name = ko.observable(woData.Name)
            else mself.Name = ko.observable('');
            if (woData.HTMLDescription)
                mself.Description = ko.observable(woData.HTMLDescription)
            else if (woData.Description)
                mself.Description = ko.observable(woData.Description)
            else mself.Description = ko.observable('');

            if (woData.ManhoursString)
                mself.ManHours = ko.observable(woData.ManhoursString)
            else mself.ManHours = ko.observable('');

            if (woData.ManhoursInMinutes)
                mself.ManhoursInMinutes = ko.observable(woData.ManhoursInMinutes)
            else mself.ManhoursInMinutes = ko.observable(0);

            if (woData.ManhoursNormInMinutes)
                mself.ManhoursNorm = ko.observable(woData.ManhoursNormInMinutes)
            else mself.ManhoursNorm = ko.observable(0);

            if (woData.ManhoursNormString)
                mself.ManhoursNormString = ko.observable(woData.ManhoursNormString)
            else mself.ManhoursNormString = ko.observable('');

            if (woData.EntityStateID)
                mself.EntityStateID = ko.observable(woData.EntityStateID)
            else mself.EntityStateID = ko.observable('');
            if (woData.EntityStateName)
                mself.EntityStateName = ko.observable(woData.EntityStateName)
            else mself.EntityStateName = ko.observable('');
            //
            if (woData.WorkflowSchemeID)
                mself.WorkflowSchemeID = ko.observable(woData.WorkflowSchemeID);
            else mself.WorkflowSchemeID = ko.observable(null);
            //
            mself.WorkflowImageSource = ko.observable(woData.WorkflowImageSource ? woData.WorkflowImageSource : null);
            if (woData.TypeName)
                mself.TypeName = ko.observable(woData.TypeName)
            else mself.TypeName = ko.observable('');
            if (woData.TypeID)
                mself.TypeID = ko.observable(woData.TypeID)
            else mself.TypeID = ko.observable('');
            if (woData.PriorityName)
                mself.PriorityName = ko.observable(woData.PriorityName)
            else mself.PriorityName = ko.observable('');
            if (woData.PriorityID)
                mself.PriorityID = ko.observable(woData.PriorityID)
            else mself.PriorityID = ko.observable('');
            if (woData.PriorityColor)
                mself.PriorityColor = ko.observable(woData.PriorityColor)
            else mself.PriorityColor = ko.observable('');
            //
            if (woData.ReferencedObjectClassID)
                mself.ReferenceClassID = ko.observable(woData.ReferencedObjectClassID);
            else mself.ReferenceClassID = ko.observable('');
            if (woData.ReferencedObjectID)
                mself.ReferenceObjectID = ko.observable(woData.ReferencedObjectID);
            else mself.ReferenceObjectID = ko.observable('');
            if (woData.ReferenceObjectNumber)
                mself.ReferenceObjectNumber = ko.observable(woData.ReferenceObjectNumber);
            else mself.ReferenceObjectNumber = ko.observable('');
            if (woData.CallClientID)
                mself.CallClientID = ko.observable(woData.CallClientID);
            else mself.CallClientID = ko.observable(null);
            //
            if (woData.InitiatorID)
                mself.InitiatorID = ko.observable(woData.InitiatorID)
            else mself.InitiatorID = ko.observable('');
            mself.InitiatorLoaded = ko.observable(false);
            mself.Initiator = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.workOrderInitiator));
            //
            if (woData.AssigneeID)
                mself.AssignorID = ko.observable(woData.AssigneeID)
            else mself.AssignorID = ko.observable('');
            mself.AssignorLoaded = ko.observable(false);
            mself.Assignor = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.assignor, parentSelf.EditAssignor));
            //
            mself.QueueID = ko.observable(woData.QueueID ? woData.QueueID : '');
            mself.QueueName = ko.observable(woData.QueueName ? woData.QueueName : '');
            mself.QueueLoaded = ko.observable(false);
            mself.Queue = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.queueExecutor, parentSelf.EditQueue));
            //
            mself.ExecutorID = ko.observable(woData.ExecutorID ? woData.ExecutorID : '');
            mself.ExecutorLoaded = ko.observable(false);
            mself.Executor = ko.observable(new userLib.EmptyUser(parentSelf, userLib.UserTypes.executor, parentSelf.EditExecutor));
            //
            if (woData.UtcDateCreated)
                mself.UtcDateCreated = ko.observable(parseDate(woData.UtcDateCreated))
            else mself.UtcDateCreated = ko.observable('');
            mself.UtcDateCreatedJS = woData.UtcDateCreated;
            if (woData.UtcDateAssigned)
                mself.UtcDateAssigned = ko.observable(parseDate(woData.UtcDateAssigned))
            else mself.UtcDateAssigned = ko.observable('');
            mself.UtcDateAssignedJS = woData.UtcDateAssigned;
            if (woData.UtcDateAccepted)
                mself.UtcDateAccepted = ko.observable(parseDate(woData.UtcDateAccepted))
            else mself.UtcDateAccepted = ko.observable('');
            mself.UtcDateAcceptedJS = woData.UtcDateAccepted;
            if (woData.UtcDatePromised)
                mself.UtcDatePromised = ko.observable(parseDate(woData.UtcDatePromised))
            else mself.UtcDatePromised = ko.observable('');
            mself.UtcDatePromisedJS = woData.UtcDatePromised;
            if (woData.UtcDatePromised)
                mself.UtcDatePromisedDT = ko.observable(new Date(parseInt(woData.UtcDatePromised)));
            else mself.UtcDatePromisedDT = ko.observable(null);
            if (woData.UtcDateStarted)
                mself.UtcDateStarted = ko.observable(parseDate(woData.UtcDateStarted))
            else mself.UtcDateStarted = ko.observable('');
            mself.UtcDateStartedJS = woData.UtcDateStarted;
            if (woData.UtcDateAccomplished)
                mself.UtcDateAccomplished = ko.observable(parseDate(woData.UtcDateAccomplished))
            else mself.UtcDateAccomplished = ko.observable('');
            mself.UtcDateAccomplishedJS = woData.UtcDateAccomplished;
            if (woData.UtcDateModified)
                mself.UtcDateModified = ko.observable(parseDate(woData.UtcDateModified))
            else mself.UtcDateModified = ko.observable('');
            //
            if (woData.NegotiationCount)
                mself.NegotiationCount = ko.observable(woData.NegotiationCount);
            else mself.NegotiationCount = ko.observable(0);
            if (woData.HaveUnvotedNegotiation != null)
                mself.HaveUnvotedNegotiation = ko.observable(woData.HaveUnvotedNegotiation);
            else mself.HaveUnvotedNegotiation = ko.observable(false);
            //
            if (woData.UnreadNoteCount)
                mself.UnreadNoteCount = ko.observable(woData.UnreadNoteCount);
            else mself.UnreadNoteCount = ko.observable(0);
            if (woData.NoteCount)
                mself.NoteCount = ko.observable(woData.NoteCount);
            else mself.NoteCount = ko.observable(0);
            mself.TotalNotesCount = ko.computed(function () {
                return parseInt(mself.NoteCount());
            });
            //
            if (woData.DependencyObjectCount)
                mself.DependencyObjectCount = ko.observable(woData.DependencyObjectCount);
            else mself.DependencyObjectCount = ko.observable(0);
            //
            //
            if (woData.WorkOrderTypeClass !== null)
                mself.WorkOrderTypeClass = ko.observable(woData.WorkOrderTypeClass);
            else mself.WorkOrderTypeClass = ko.observable();
            //
            if (woData.WorkOrderTypeClassString)
                mself.WorkOrderTypeClassString = ko.observable(woData.WorkOrderTypeClassString);
            else mself.WorkOrderTypeClassString = ko.observable();
            //
            mself.NumberName = ko.computed(function () {
                return '№' + mself.Number() + ' ' + mself.Name();
            });
            //
            mself.TotalCostWithNDSS = ko.observable(woData.TotalCostWithNDS ? woData.TotalCostWithNDS : null);
            mself.TotalCostWithNDSString = ko.observable(specLib.ToMoneyString(mself.TotalCostWithNDSS()) + ' ' + getTextResource('CurrentCurrency'));
            //
            mself.UserField1 = ko.observable(woData.UserField1 ? woData.UserField1 : '');
            mself.UserField2 = ko.observable(woData.UserField2 ? woData.UserField2 : '');
            mself.UserField3 = ko.observable(woData.UserField3 ? woData.UserField3 : '');
            mself.UserField4 = ko.observable(woData.UserField4 ? woData.UserField4 : '');
            mself.UserField5 = ko.observable(woData.UserField5 ? woData.UserField5 : '');
            //
            mself.UserField1Name = woData.UserField1Name;
            mself.UserField2Name = woData.UserField2Name;
            mself.UserField3Name = woData.UserField3Name;
            mself.UserField4Name = woData.UserField4Name;
            mself.UserField5Name = woData.UserField5Name;
            mself.UserFieldNamesDictionary = woData.UserFieldNamesDictionary;
            //
            mself.ReferenceObject = ko.observable(null);
            mself.ajaxControl_woRefObject = new ajaxLib.control();

            //
            mself.CauseObject = ko.observable(null);
            mself.CauseObjectID = ko.observable(null);
            mself.CauseClassID = ko.observable(null);
            //
            mself.Client = ko.observable(null);
            mself.ClientID = ko.observable('');
            mself.ClientLoaded = ko.observable(false);
            mself.CreateCallReference = function (data, out_ko_object) {
                require(['models/SDForms/SDForm.CallReference'], function (lib) {
                    out_ko_object(new lib.CallReference(self, data));
                });
            };
            mself.CreateProblemReference = function (data, out_ko_object) {
                require(['models/SDForms/SDForm.ProblemReference'], function (lib) {
                    out_ko_object(new lib.ProblemReference(self, data));
                });
            };
            mself.CreateChangeRequestReference = function (data, out_ko_object) {
                require(['models/SDForms/SDForm.ChangeRequestReference'], function (lib) {
                    out_ko_object(new lib.ChangeRequestReference(self, data));
                });
            }
            mself.CreateMassIncidentReference = function (data, out_ko_object) {
                require(['models/SDForms/SDForm.MassIncidentReference'], function (lib) {
                    out_ko_object(new lib.MassIncidentReference(self, data));
                });
            }
            //
            mself.LoadReferenceObject = function (id, classID, out_ko_object, out_ko_classID, initClient, referenceExists, resizeFunc) {
                const refClasses = [701, 702, 703, 823];
                if (!id || !classID || !refClasses.includes(classID)) {
                    out_ko_object(null);
                    return false;
                }

                let callback;
                let ajaxSettings = {
                    dataType: 'json',
                    method: 'GET',
                };
                if (classID === 701) { // Call
                    ajaxSettings.url = `/api/Calls/${id}`;
                    callback = function (data) {
                        mself.CreateCallReference(data, out_ko_object);
                        if (out_ko_classID) {
                            out_ko_classID(701);
                        }
                        if (initClient) {
                            mself.ClientLoaded(false);
                            mself.ClientID(newValue.ClientID);
                            initClient();
                        }
                        if (resizeFunc) {
                            resizeFunc();
                        }
                    }
                } else if (classID === 702 ) { // Problem
                    ajaxSettings.url = `/api/Problems/${id}`;
                    callback = function (data) {
                        mself.CreateProblemReference(data, out_ko_object);
                    }
                } else if (classID === 703) { // ChangeRequest
                    ajaxSettings.url = `/api/ChangeRequests/${id}`;
                    callback = function (data) {
                        mself.CreateChangeRequestReference(data, out_ko_object);
                    }
                } else if (classID === 823) { // MassIncident
                    ajaxSettings.url = `/api/MassIncidents`;
                    ajaxSettings.data = { GlobalIdentifiers: [id] };
                    ajaxSettings.traditional = true;
                    // ajaxSettings.contentType = 'application/json';
                    callback = function (data) {
                        if (data.length && data.length > 0) {
                            mself.CreateMassIncidentReference(data[0], out_ko_object);
                        }
                    }
                }
                
                if (!callback) {
                    return false;
                }

                mself.ajaxControl_woRefObject.Ajax(self.$region.find('.woRef__b'),
                    ajaxSettings,
                    function (response) {
                        if (response) {
                            callback(response);
                        } else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[WorkOrderForm.WorkOrder.js, LoadReferenceObject]', 'error');
                            });
                        }
                    });
            };
            //

            mself.CounterPartyID = ko.observable(null);
            mself.CounterPartyName = ko.observable(null);
            mself.PurchaseConcord = ko.observable(null);
            mself.TotalCostWithNDS = ko.observable(null);
            mself.UtcDateDelivered = ko.observable('');
            mself.PurchaseBill = ko.observable(null);
            mself.PurchaseDetailBudget = ko.observable(null);
            mself.UtcDateDeliveredDT = ko.observable(null);
            mself.WaitPurchaseCostWithNDS = ko.observable(null);

            if (woData.FinancePurchase) { 
                mself.CounterPartyID(woData.FinancePurchase.SupplierID);
                mself.CounterPartyName(woData.FinancePurchase.SupplierName);
                mself.PurchaseConcord(woData.FinancePurchase.Concord);
                mself.TotalCostWithNDS(woData.FinancePurchase.TotalCostWithNDS);    
                mself.PurchaseDetailBudget(woData.FinancePurchase.DetailBudget);
                mself.PurchaseBill(woData.FinancePurchase.Bill);
                mself.WaitPurchaseCostWithNDS(woData.FinancePurchase.WaitPurchaseCostWithNDS);
                if (woData.FinancePurchase.UtcDateDelivered) {
                    mself.UtcDateDelivered(parseDate(woData.FinancePurchase.UtcDateDelivered));
                    mself.UtcDateDeliveredDT(new Date(parseInt(woData.FinancePurchase.UtcDateDelivered)));
                }
            }
            
            //
            if (woData.InventoryDocument)
                mself.InventoryDocument = ko.observable(woData.InventoryDocument);
            else mself.InventoryDocument = ko.observable('');
            //
            if (woData.InventoryFounding)
                mself.InventoryFounding = ko.observable(woData.InventoryFounding);
            else mself.InventoryFounding = ko.observable('');
            //
            if (woData.PurchaseDetailBudgetReadOnly)
                mself.PurchaseDetailBudgetReadOnly = ko.observable(woData.PurchaseDetailBudgetReadOnly);
            else mself.PurchaseDetailBudgetReadOnly = ko.observable(false);
            //
            if (woData.PurchaseFinanceBudgetRowList_Sum)
                mself.PurchaseFinanceBudgetRowList_Sum = woData.PurchaseFinanceBudgetRowList_Sum;
            else mself.PurchaseFinanceBudgetRowList_Sum = null;
            //
            if (woData.PurchasedAndPlacedCostWithNDS)
                mself.PurchasedAndPlacedCostWithNDS = ko.observable(woData.PurchasedAndPlacedCostWithNDS);
            else mself.PurchasedAndPlacedCostWithNDS = ko.observable(null);
        }
    };
    return module;
});