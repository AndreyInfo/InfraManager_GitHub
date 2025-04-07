define([
    'knockout',
    'jquery',
    'usualForms',
    'ajax',
    'dynamicOptionsService',
    'iconHelper',
    'comboBox',
    'jqueryStepper'
], function (
    ko,
    $,
    fhModule,
    ajaxLib, 
    dynamicOptionsService,
    ihLib
) {
    var module = {
        MaxCount: Math.pow(2, 31) - 1,
        ViewModel: function (mainRegionID, reasonClassID, reasonID) {
            var self = this;
            //
            //outer actions
            self.frm = undefined;
            //
            //vars
            self.formClassID = 703;//OBJ_RFC
            self.MainRegionID = mainRegionID;
            self.$region = $('#' + mainRegionID);
            self.ID = null;//for registered rfc

            //Files
            self.RealizationDocumentID = null; 
            self.RollbackDocumentID = null;

            self.renderedD = $.Deferred();
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                parameterPrefix: 'parameter_'
            };
            self.mode = ko.observable(self.modes.nothing);

            self.SetTab = function (template) {
                self.mode(`${self.modes.parameterPrefix}${template.Tab.ID}`);
            };

            self.DynamicOptionsService = new dynamicOptionsService.ViewModel(self.MainRegionID, {
                typeForm: 'Create',
                changeTabCb: function (invalidInput) {
                    const parent = invalidInput.closest('[data-prefix-mode]');
                    const tabPrefix = parent.attr('data-prefix-mode');
                    self.mode(tabPrefix);
                }
            });

            self.DynamicOptionsServiceInit = function (typeId) {
                if (self.DynamicOptionsService.IsInit()) {
                    self.DynamicOptionsService.ResetData();
                };

                self.DynamicOptionsService.GetTemplate(dynamicOptionsService.Classes.ChangeRequestType, typeId);
            };

            self.IsClientMode = ko.observable(false);
            self.IsReadOnly = ko.observable(false);
            //
            //RFCTarget
            {
                self.RFCTarget = ko.observable('');
                self.RFCTargetText = getTextResource('Target') + ': ';
            }
            //           
            //initiator
            {
                self.InitiatorLoaded = ko.observable(false);
                self.InitiatorID = null;
                self.Initiator = ko.observable(null);
                //
                self.InitializeInitiator = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.InitiatorLoaded() == false) {
                            if (self.InitiatorID) {
                                var options = {
                                    UserID: self.InitiatorID,
                                    UserType: userLib.UserTypes.workOrderInitiator,
                                    UserName: null,
                                    EditAction: self.EditInitiator,
                                    RemoveAction: self.DeleteInitiator
                                };
                                var user = new userLib.User(self, options);
                                self.Initiator(user);
                                self.InitiatorLoaded(true);
                            }
                            else
                                self.Initiator(new userLib.EmptyUser(self, userLib.UserTypes.workOrderInitiator, self.EditInitiator));
                        }
                    });
                };
                self.EditInitiator = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            fieldName: 'WorkOrder.Initiator',
                            fieldFriendlyName: getTextResource('Initiator'),
                            oldValue: self.InitiatorLoaded() ? { ID: self.Initiator().ID(), ClassID: 9, FullName: self.Initiator().FullName() } : null,
                            object: ko.toJS(self.Initiator()),
                            searcherName: 'WebUserSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            onSave: function (objectInfo) {
                                self.InitiatorLoaded(false);
                                self.Initiator(new userLib.EmptyUser(self, userLib.UserTypes.workOrderInitiator, self.EditInitiator));
                                self.InitiatorID = objectInfo ? objectInfo.ID : null;
                                //
                                self.InitializeInitiator();
                            },
                            nosave: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.DeleteInitiator = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.InitiatorLoaded(false);
                        self.InitiatorID = null;
                        self.Initiator(new userLib.EmptyUser(self, userLib.UserTypes.workOrderInitiator, self.EditInitiator));
                    });
                };
            }
            //
            //owner
            {
                self.OwnerLoaded = ko.observable(false);
                self.OwnerID = null;
                self.Owner = ko.observable(null);
                //
                self.InitializeOwner = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.OwnerLoaded() == false) {
                            if (self.OwnerID) {
                                var options = {
                                    UserID: self.OwnerID,
                                    UserType: userLib.UserTypes.owner,
                                    UserName: null,
                                    EditAction: self.EditOwner,
                                    RemoveAction: self.DeleteOwner
                                };
                                var user = new userLib.User(self, options);
                                self.Owner(user);
                                self.Owner().TypeName = ko.observable(getTextResource('Coordinator_Owner'));
                                self.OwnerLoaded(true);
                            }
                            else {
                                self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner, true, true, getTextResource('Coordinator_Owner')));
                            }
                        }
                    });
                };
                self.EditOwner = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            fieldName: 'Problem.Owner',
                            fieldFriendlyName: getTextResource('Owner'),
                            oldValue: self.OwnerLoaded() ? { ID: self.Owner().ID(), ClassID: 9, FullName: self.Owner().FullName() } : null,
                            object: ko.toJS(self.Owner()),
                            searcherName: 'OwnerUserSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [self.QueueID],
                            onSave: function (objectInfo) {
                                self.OwnerLoaded(false);
                                self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner, true, true, getTextResource('Coordinator_Owner')));
                                self.OwnerID = objectInfo ? objectInfo.ID : null;
                                //
                                self.InitializeOwner();
                            },
                            nosave: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.DeleteOwner = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.OwnerLoaded(false);
                        self.OwnerID = null;
                        self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner, true, true, getTextResource('Coordinator_Owner')));
                    });
                };
            }
            //
            //priority     
            {
                self.PriorityID = null;
                self.PriorityName = ko.observable(getTextResource('PromptPriority'));
                self.PriorityColor = ko.observable('');
                //
                self.InfluenceID = null;
                self.UrgencyID = null;
                //
                self.priorityControl = null;
                self.LoadPriorityControl = function () {
                    require(['models/SDForms/SDForm.Priority'], function (prLib) {
                        if (self.priorityControl == null || self.priorityControl.IsLoaded() == false) {
                            self.priorityControl = new prLib.ViewModel($('#' + self.MainRegionID).find('.b-requestDetail-menu__priority'), self, false);
                            self.priorityControl.Initialize();
                        }
                        $.when(self.priorityControl.Load(null, 702, self.UrgencyID, self.InfluenceID, self.PriorityID)).done(function (result) {
                        });
                    });
                };
                self.RefreshPriority = function (priorityObj) {//invokes from priorityControl
                    if (priorityObj == null)
                        return;
                    //
                    self.PriorityName(priorityObj.Name);
                    self.PriorityColor(priorityObj.Color);
                    self.PriorityID = priorityObj.ID;
                    //
                    self.InfluenceID = self.priorityControl.CurrentInfluenceID();
                    self.UrgencyID = self.priorityControl.CurrentUrgencyID();
                };
            }
            //
            //ReasonForChange
            {
                self.CssIconClass = ko.pureComputed(function () {
                    if (self.ReasonObject == null)
                        return null;
                    var classID = self.ReasonObject().ClassID;
                    if (classID == 701)
                        return 'call-icon';
                    else if (classID == 702)
                        return 'problem-icon';
                    else if (classID == 119)
                        return 'workorder-icon';
                    else return 'finished-item-icon';
                });
                self.ReasonObject = ko.observable(null);
                self.ReasonName = ko.observable('');
                self.ReasonUtcDatePromised = ko.observable('');
                self.ReasonStateName = ko.observable('');
                self.ReasonOwner = ko.observable(null);
                self.ReasonModify = ko.observable('');
                self.IsReasonContainerVisible = ko.observable(false);
                self.VisibleReason = ko.observable(false);
                self.ToggleCauseContainer = function () {
                    self.IsReasonContainerVisible(!self.IsReasonContainerVisible());
                };
                //
                self.ShowReasonObjectForm = function (referenceObject) {
                    if (referenceObject.ReasonObject().ClassID === 701) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowCall(referenceObject.ReasonObject().ID(), self.IsReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);
                        });
                    }
                    else if (referenceObject.ReasonObject().ClassID === 702) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowProblem(referenceObject.ReasonObject().ID(), self.IsReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);
                        });
                    }
                    else if (referenceObject.ReasonObject().ClassID === 119) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowWorkOrder(referenceObject.ReasonObject().ID(), self.IsReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);
                        });
                    }
                };
                //
                self.ContextMenuVisible = ko.observable(false);
                //
                self.LinkSdObjectClick = function (data, e) {
                    var isVisible = self.ContextMenuVisible();
                    self.ContextMenuVisible(!isVisible);
                    //
                    e.stopPropagation();
                };
                //
                self.ReasonCallBackObject = function (obj, ClassID) {
                    if (obj.ID)
                        obj = obj.ID;
                    $.when(self.LoadReferenceObject(obj, ClassID)).done(function (loadResult) {
                        if (loadResult == true) {
                            var mself = self.ReasonObject();
                            if (mself.ClassID == 701) {
                                self.InitializeReasonObjectOwner();
                                self.ReasonName('№' + mself.Number() + ' ' + mself.CallSummaryName());
                            }
                            else if (mself.ClassID == 702) {
                                self.InitializeReasonObjectOwner();
                                self.ReasonName('№' + mself.Number() + ' ' + mself.Summary());
                            }
                            else {
                                self.ReasonOwner(null);
                                self.ReasonName('№' + mself.Number() + ' ' + mself.Name());
                            }
                            self.ReasonUtcDatePromised(mself.UtcDatePromised());
                            self.ReasonStateName(mself.EntityStateName());
                            self.ReasonModify(mself.UtcDateModified());
                            //
                            self.ReasonObject.valueHasMutated();
                            self.VisibleReason(true);
                            self.IsReasonContainerVisible(true);
                        }
                        else {
                            self.ReasonName('');
                            self.ReasonUtcDatePromised('');
                            self.ReasonStateName('');
                            self.ReasonModify('');
                            self.VisibleReason(false);
                        }
                    });
                }
                //
                self.InitializeReasonObjectOwner = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        tmp = self.ReasonObject();
                        if (tmp == null)
                            return
                        if (tmp.OwnerLoaded() == false && tmp.OwnerID()) {
                            var options = {
                                UserID: tmp.OwnerID(),
                                UserType: userLib.UserTypes.owner,
                                UserName: null,
                                CanNote: true
                            };

                            var user = new userLib.User(self, options);
                            tmp.Owner(user);
                            self.ReasonOwner(user);
                            tmp.OwnerLoaded(true);
                        }
                    });
                };
                //
                self.ajaxControl_load = new ajaxLib.control();
                self.LoadReferenceObject = function (id, classID) {
                    const retD = $.Deferred();
                    switch (classID) {
                        case 119:
                            self.ajaxControl_load.Ajax(self.$region,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    url: '/api/workorders/' + id
                                },
                                function (woInfo) {
                                    require(['models/SDForms/WorkOrderForm.WorkOrder'], function (woLib) {
                                        self.ReasonObject(new woLib.WorkOrder(self, woInfo));
                                        retD.resolve(true);
                                    });
                                }, function (err) {
                                    switch (err.status) {
                                        case 404:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('ObjectDeleted'), 'error');
                                            });
                                            break;
                                        case 403:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                            });
                                            break;
                                        case 400:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                            });
                                            break;
                                    }
                                    retD.resolve(false);
                                });
                            break;

                        case 702:
                            self.ajaxControl_load.Ajax(self.$region,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    url: '/api/problems/' + id
                                },
                                function (pInfo) {
                                    require(['models/SDForms/ProblemForm.Problem'], function (pLib) {
                                        self.ReasonObject(new pLib.Problem(self, pInfo));
                                        retD.resolve(true);
                                    });
                                }, function (err) {
                                    switch (err.status) {
                                        case 404:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('ObjectDeleted'), 'error');
                                            });
                                            break;
                                        case 403:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                            });
                                            break;
                                        case 400:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                            });
                                            break;
                                    }
                                    retD.resolve(false);
                                });
                            break;

                        case 701:
                            self.ajaxControl_load.Ajax(self.$region,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    url: '/api/calls/' + id
                                },
                                function (callInfo) {
                                    require(['models/SDForms/CallForm.Call'], function (callLib) {
                                        self.ReasonObject(new callLib.Call(self, callInfo));
                                        retD.resolve(true);
                                    });
                                }, function (err) {
                                    switch (err.status) {
                                        case 404:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('ObjectDeleted'), 'error');
                                            });
                                            break;
                                        case 403:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                            });
                                            break;
                                        case 400:
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                            });
                                            break;
                                    }
                                    retD.resolve(false);
                                });
                            break;

                        default:
                            retD.resolve(false)
                            break;
                    }
                    return retD.promise();
                };
                //
                self.LinkCall = function () {
                    var isVisible = self.ContextMenuVisible();
                    self.ContextMenuVisible(!isVisible);
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSearcherLite([701], null, null, null, null, self.ReasonCallBackObject);
                    });
                };
                //
                self.LinkWorkorder = function () {
                    var isVisible = self.ContextMenuVisible();
                    self.ContextMenuVisible(!isVisible);
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSearcherLite([119], null, null, null, null, self.ReasonCallBackObject);
                    });
                };
                //
                self.LinkProblem = function () {
                    var isVisible = self.ContextMenuVisible();
                    self.ContextMenuVisible(!isVisible);
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSearcherLite([702], null, null, null, null, self.ReasonCallBackObject);
                    });
                };
            }
            //
            //attachments
            {
                self.attachmentsRealizationControl = null;
                self.LoadRealizationAttachmentsControl = function () {
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsRealizationControl != null)
                            self.attachmentsRealizationControl.RemoveUploadedFiles();//previous object                   
                        //
                        if (self.attachmentsRealizationControl == null || self.attachmentsRealizationControl.IsLoaded() == false) {
                            var attachmentsElement = $('#' + self.MainRegionID).find('.documentRealizationList');
                            self.attachmentsRealizationControl = new fcLib.control(attachmentsElement, '.realizationFileField', '.addRealizationFileBtn', null, true, null, 1);
                        }
                        self.attachmentsRealizationControl.OnChange = function (url, onSuccess, docId) {
                            self.RealizationDocumentID = docId[0];
                        };
                        self.attachmentsRealizationControl.ReadOnly(false);
                    });
                };
                self.attachmentsRollbackControl = null;
                self.LoadRollbackAttachmentsControl = function () {
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsRollbackControl != null)
                            self.attachmentsRollbackControl.RemoveUploadedFiles();//previous object                   
                        //
                        if (self.attachmentsRollbackControl == null || self.attachmentsRollbackControl.IsLoaded() == false) {
                            var attachmentsElement = $('#' + self.MainRegionID).find('.documentRollbackList');
                            self.attachmentsRollbackControl = new fcLib.control(attachmentsElement, '.rollbackFileField', '.addRollbackFileBtn', null, true, null, 1);
                        }
                        self.attachmentsRollbackControl.OnChange = function (url, onSuccess, docId) {
                            self.RollbackDocumentID = docId[0];
                        };
                        self.attachmentsRollbackControl.ReadOnly(false);
                    });
                };
                self.attachmentsControl = null;
                self.LoadAttachmentsControl = function () {
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsControl != null)
                            self.attachmentsControl.RemoveUploadedFiles();//previous object                   
                        //
                        if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                            var attachmentsElement = $('#' + self.MainRegionID).find('.RFCdocumentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.RFCFileField', '.addFileBtn');
                        }
                        self.attachmentsControl.ReadOnly(false);
                    });
                };
            }
            //
            //FileContainer
            {
                self.IsFileContainerVisible = ko.observable(false);
                self.ToggleFileContainer = function () {
                    self.IsFileContainerVisible(!self.IsFileContainerVisible());
                };
            }
            //
            //
            //queue
            {
                self.QueueLoaded = ko.observable(false);
                self.QueueID = null;
                self.QueueName = null;
                self.Queue = ko.observable(null);
                //
                self.InitializeQueue = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.QueueLoaded() == false) {
                            if (self.QueueID) {
                                var options = {
                                    UserID: self.QueueID,
                                    UserType: userLib.UserTypes.queueExecutor,
                                    UserName: self.QueueName,
                                    EditAction: self.EditQueue,
                                    RemoveAction: self.DeleteQueue,
                                    IsFreezeSelectedClient: true
                                };
                                var queue = new userLib.User(self, options);
                                self.Queue(queue);
                                self.QueueLoaded(true);
                            }
                            else
                                self.Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                        }
                    });
                };
                //
                self.ClearOwner = ko.observable(true);
                self.CheckOwner = function () {
                    var UserIDs = self.Queue().QueueUserIDList();
                    self.ClearOwner(true);
                    UserIDs.forEach(function (item) {
                        if (item == self.OwnerID)
                            self.ClearOwner(false);
                    })
                    if (self.ClearOwner()) {
                        self.DeleteOwner();
                    }
                };
                //
                self.EditQueue = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            fieldName: 'RFC.Queue',
                            fieldFriendlyName: getTextResource('Queue'),
                            oldValue: self.QueueLoaded() ? { ID: self.Queue().ID(), ClassID: self.Queue().ClassID(), FullName: self.Queue().FullName() } : null,
                            object: ko.toJS(self.Queue()),
                            searcherName: "QueueSearcher",
                            searcherPlaceholder: getTextResource('EnterQueue'),
                            searcherParams: { Type: 16 },
                            onSave: function (objectInfo) {
                                self.QueueLoaded(false);
                                self.Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                                //
                                if (objectInfo && objectInfo.ClassID == 722) { //IMSystem.Global.OBJ_QUEUE
                                    self.QueueID = objectInfo.ID;
                                    self.QueueName = objectInfo.FullName;
                                }
                                else {
                                    self.QueueID = null;
                                    self.QueueName = null;
                                }
                                self.InitializeQueue();
                            },
                            nosave: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.DeleteQueue = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.QueueLoaded(false);
                        self.QueueID = null;
                        self.QueueName = null;
                        self.Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                    });
                };
            }
            //
            //PlanningForChange
            {
                self.IsPlanningContainerVisible = ko.observable(false);
                self.TogglePlanningContainer = function () {
                    self.IsPlanningContainerVisible(!self.IsPlanningContainerVisible());
                };
            }
            self.FundingAmountNumber = ko.observable(null);
            self.InitFundingAmountNumber = function () {
                var $input = self.$region.find('.funding-number-TextField');
                $input.stepper({
                    type: 'int',
                    floatPrecission: 0,
                    wheelStep: 1,
                    arrowStep: 1,
                    limit: [1, module.MaxCount],
                    onStep: function (val, up) {
                        self.FundingAmountNumber(val);
                    }
                });
            };
            //
            //description
            {
                self.HTMLDescription = ko.observable('');
                self.htmlDescriptionControlD = $.Deferred();
                self.IsDescriptionContainerVisible = ko.observable(true);
                self.ToggleDescriptionContainer = function () {
                    self.IsDescriptionContainerVisible(!self.IsDescriptionContainerVisible());
                };
                //
                self.htmlDescriptionControl = null;
                self.InitializeDescription = function () {
                    var htmlElement = $('#' + self.MainRegionID).find('.description');
                    if (self.htmlDescriptionControl == null)
                        require(['htmlControl'], function (htmlLib) {
                            self.htmlDescriptionControl = new htmlLib.control(htmlElement);
                            self.htmlDescriptionControl.OnHTMLChanged = function (htmlValue) {
                                self.HTMLDescription(htmlValue);
                            };
                            self.HTMLDescription.subscribe(function (newValue) {
                                if (self.htmlDescriptionControl != null)
                                    self.htmlDescriptionControl.SetHTML(newValue);
                            });
                        });
                    else
                        self.htmlDescriptionControl.Load(htmlElement);
                };
                
                self.EditDescription = function () {
                    showSpinner();
                    const fh = new fhModule.formHelper(true);
                    const options = {
                        fieldName: 'RFC.Description',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: self.HTMLDescription(),
                        onSave: function (newHTML) {
                            self.HTMLDescription(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
            }
            //
            //for comboBox
            {
                self.createComboBoxItem = function (simpleDictionary) {
                    var thisObj = this;
                    //
                    thisObj.ID = simpleDictionary.ID;
                    thisObj.Name = simpleDictionary.Name;
                };
                //
                self.createComboBoxHelper = function (container_selector, getUrl, comboBoxFunc) {
                    var thisObj = this;
                    if (!comboBoxFunc)
                        comboBoxFunc = self.createComboBoxItem;
                    //
                    thisObj.SelectedItem = ko.observable(null);
                    //
                    thisObj.ItemList = ko.observableArray([]);
                    thisObj.ItemListD = $.Deferred();
                    thisObj.getItemList = function (options) {
                        var data = thisObj.ItemList();
                        options.callback({ data: data, total: data.length });
                    };
                    //
                    thisObj.ajaxControl = new ajaxLib.control();
                    thisObj.LoadList = function (selectedItem) {
                        thisObj.ajaxControl.Ajax($(container_selector),
                            {
                                url: getUrl,
                                method: 'GET'
                            },
                            function (response) {
                                if (response) {
                                    thisObj.ItemList.removeAll();
                                    //
                                    $.each(response, function (index, simpleDictionary) {
                                        var u = new comboBoxFunc(simpleDictionary);
                                        thisObj.ItemList().push(u);
                                    });
                                    if (selectedItem && selectedItem != null)
                                        thisObj.SetSelectedItem(selectedItem);
                                    thisObj.ItemList.valueHasMutated();
                                }
                                thisObj.ItemListD.resolve();
                            });
                    };
                    //
                    thisObj.GetObjectInfo = function (classID) {
                        return thisObj.SelectedItem() ? { ID: thisObj.SelectedItem().ID, ClassID: classID, FullName: thisObj.SelectedItem().Name } : null;
                    };
                    thisObj.SetSelectedItem = function (id) {
                        $.when(thisObj.ItemListD).done(function () {
                            var item = null;
                            if (id != undefined && id != null)
                                for (var i = 0; i < thisObj.ItemList().length; i++) {
                                    var tmp = thisObj.ItemList()[i];
                                    if (tmp.ID == id) {
                                        item = tmp;
                                        break;
                                    }
                                }
                            thisObj.SelectedItem(item);
                        });
                    };
                }
            }
            //
            //rfcType
            {
                self.TypeID = ko.observable(null);
                self.RFCTypeHelper = new self.createComboBoxHelper('#' + self.MainRegionID + ' .rfcType', '/api/rfctypes');
                self.RFCTypeHelper.SelectedItem.subscribe(function (newValue) {
                    if (self.TypeID() !== newValue.ID && newValue !== null) {
                        self.DynamicOptionsServiceInit(newValue.ID);
                    };

                    self.TypeID(newValue == null ? null : newValue.ID);
                });
                self.EditRFCType = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'RFC.RFCType',
                        fieldFriendlyName: getTextResource('RFCType'),
                        oldValue: self.RFCTypeHelper.GetObjectInfo(136),
                        searcherName: 'RFCTypeSearcher',
                        searcherPlaceholder: getTextResource('RFCType'),
                        onSave: function (objectInfo) {
                            self.RFCTypeHelper.SetSelectedItem(objectInfo ? objectInfo.ID : null);
                            self.TypeID(objectInfo ? objectInfo.ID : null);
                            // TODO: тут смена шаблона
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }
            //rfcCategory
            {
                self.CategoryID = ko.observable(null);
                self.RFCCategoryHelper = new self.createComboBoxHelper('#' + self.MainRegionID + ' .rfcCategory', '/api/rfccategory');
                self.RFCCategoryHelper.SelectedItem.subscribe(function (newValue) {
                    self.CategoryID(newValue == null ? null : newValue.ID);
                });
                self.EditRFCCategory = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'RFC.RFCCategory',
                        fieldFriendlyName: getTextResource('RFCCategory'),
                        oldValue: self.RFCCategoryHelper.GetObjectInfo(136),
                        searcherName: 'RFCCategorySearcher',
                        searcherPlaceholder: getTextResource('RFCCategory'),
                        onSave: function (objectInfo) {
                            self.RFCCategoryHelper.SetSelectedItem(objectInfo ? objectInfo.ID : null);
                            self.CategoryID(objectInfo ? objectInfo.ID : null);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }
            //
            //
            self.ServiceItemName = ko.observable('');
            self.ServiceItemID = ko.observable(null);
            self.ServiceItemClassID = ko.observable(null);
            self.addServiceControl = null;
            self.LoadAddServiceControl = function () {
                if (self.addServiceControl == null || self.addServiceControl.IsLoaded() == false) {
                    self.addServiceControl = new module.AddServicesModel(self, self.$region.find('.rfc-form__services-add'));
                    self.addServiceControl.Initialize();
                }
            };

            self.FillServiceInfo = function (objectInfo) {
                self.ServiceItemName(objectInfo.FullName);
                self.ServiceItemID(objectInfo.ID);
                self.ServiceItemClassID(objectInfo.ClassID);
                return true;
            };
            self.SearchService = function () {
                var element = self.$region.find('.rfc-form__services-add');
                if (element.css('display') == 'none') {
                    element.show();
                    //
                    if (self.addServiceControl != null)
                        self.addServiceControl.FocusSearcher();
                }
                else
                    element.hide();
                return true;
            };
            //
            //for all user controls CANEDIT
            {
                self.CanEdit = ko.computed(function () {
                    return true;
                });
            }
            //
            //Summary
            {
                self.Summary = ko.observable('');
                //
                self.EditSummary = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Problem.Summary',
                        fieldFriendlyName: getTextResource('Summary'),
                        oldValue: self.Summary(),
                        onSave: function (newText) {
                            self.Summary(newText);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                };
            }
            //
            //Validate and register
            {
                self.ajaxControl_RegisterRFC = new ajaxLib.control();
                self.ValidateAndRegisterRFC = function (showSuccessMessage) {
                    const retval = $.Deferred();
                    //                
                    const data = {
                        'Summary': self.Summary(),
                        'Target': self.RFCTarget(),
                        'InitiatorID': self.InitiatorID,
                        'OwnerID': self.OwnerID,
                        'PriorityID': self.PriorityID,
                        'InfluenceID': self.InfluenceID,
                        'UrgencyID': self.UrgencyID,
                        'QueueID': self.QueueID,
                        'HTMLDescription': self.HTMLDescription(),
                        'ChangeRequestTypeID': self.TypeID(),
                        'CategoryID': self.CategoryID(),
                        'ServiceID': self.ServiceItemID(),
                        'ServiceName': self.ServiceItemName(),
                        'ReasonObjectID': self.ReasonObject() == null ? null : self.ReasonObject().ID(),
                        'ReasonObjectClassID': self.ReasonObject() == null ? null : self.ReasonObject().ClassID,
                        'FundingAmountNumber': self.FundingAmountNumber == null ? null : self.FundingAmountNumber(),
                        'Files': self.attachmentsControl == null ? null : self.attachmentsControl.GetData(),
                        'RealizationDocumentID': self.RealizationDocumentID,
                        'RollbackDocumentID': self.RollbackDocumentID,
                        'FormValuesData': self.DynamicOptionsService.SendByServer(),
                    };
                    //  
                    if (data.InitiatorID == null) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('MustSetInitiator'));
                        });
                        retval.resolve(null);
                        return;
                    }
                    if (!data.Summary || data.Summary.trim().length === 0) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('PromptRFCSummary'));
                        });
                        retval.resolve(null);
                        return;
                    }
                    if (data.ChangeRequestTypeID == null) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('PromptType'));
                        });
                        retval.resolve(null);
                        return;
                    }

                    if (data.FormValuesData === null && self.DynamicOptionsService.IsInit()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ParametersNotLoaded'));
                        });
                        retval.resolve(null);
                        return;
                    };

                    if (data.FormValuesData && data.FormValuesData.hasOwnProperty('valid')) {
                        data.FormValuesData.callBack();
                        retval.resolve(null);
                        return;
                    };

                    //
                    showSpinner();
                    self.ajaxControl_RegisterRFC.Ajax(null,
                        {
                            url: '/api/changerequests',
                            method: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            data: JSON.stringify(data)
                        },
                        function (response) {//RFCRegistrationResponse
                            const url = "api/DocumentReferences/" + self.formClassID + "/" + response.ID + "/documents";
                            self.ajaxControl_RegisterRFC.Ajax(null,
                                {
                                    url: url,
                                    method: "Post",
                                    dataType: "json",
                                    data: { 'docID': self.attachmentsControl.Items().map(function (file) { return file.ID; }) }
                                },
                                function () {
                                    hideSpinner();
                                    if (showSuccessMessage) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('RFCRegisteredMessage').replace('{0}', response.Number));
                                        });
                                    }
                                    retval.resolve({ Id: response.ID, RFCID: response.ID });
                                }, function () {
                                    hideSpinner();
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[RFCRegistration.js, RegisterRFC]', 'error');
                                    });
                                    retval.resolve(null);
                                });
                        },
                        function (response) {
                            hideSpinner();
                            require(['sweetAlert'], function () {
                                swal(response.responseText == null ? getTextResource('ErrorCaption') : response.responseText,
                                    getTextResource('AjaxError') + '\n[RFCRegistration.js, RegisterRFC]', 'info');
                            });
                            retval.resolve(null);
                        });
                    //
                    return retval.promise();
                };
            }
            self.Load = function () {
                self.mode(self.modes.main);
                //
                $.when(self.renderedD).done(function () {
                    $('#' + self.MainRegionID).find('.firstFocus').focus();
                    //
                    if (self.AddAs)
                        self.Fill(self.RFCData)
                    self.LoadPriorityControl();
                    // 
                    self.InitializeInitiator();
                    self.LoadRealizationAttachmentsControl();
                    self.LoadRollbackAttachmentsControl();
                    self.LoadAttachmentsControl();
                    if (reasonClassID && reasonClassID != null && reasonID && reasonID != null)
                        self.ReasonCallBackObject(reasonID, reasonClassID);
                    $.when(userD).done(function (user) {
                        self.InitiatorLoaded(false);
                        self.InitiatorID = user.UserID;
                        //
                        self.InitializeInitiator();
                    });
                    self.InitializeQueue();
                    self.InitializeOwner();
                    //
                    self.InitializeDescription();
                    //
                    if (!self.AddAs) {
                        self.RFCTypeHelper.LoadList();
                        self.RFCTypeHelper.SetSelectedItem('00000000-0703-0000-0000-000000000000');//rfc
                        self.RFCCategoryHelper.LoadList();
                    }
                });
            };
            //
            self.AddAs = false;//проблема создана по аналогии
            self.RFCData = null;//проблема, взятая за основу
            //
            self.CallList = null;//список связанных заявок (при создании проблемы по заявке)
            //
            self.Fill = function (RFCData) {
                if (RFCData.InfluenceID())
                    self.InfluenceID = RFCData.InfluenceID();
                //
                if (RFCData.PriorityID())
                    self.PriorityID = RFCData.PriorityID();
                if (RFCData.PriorityColor())
                    self.PriorityColor(RFCData.PriorityColor());
                if (RFCData.PriorityName())
                    self.PriorityName(RFCData.PriorityName());
                //
                if (RFCData.TypeID()) {
                    self.RFCTypeHelper.LoadList(RFCData.TypeID());
                    //self.RFCTypeHelper.SetSelectedItem(RFCData.TypeID());
                }
                //
                if (RFCData.CategoryID()) {
                    self.RFCCategoryHelper.LoadList(RFCData.CategoryID());
                    //self.RFCCategoryHelper.SetSelectedItem(RFCData.CategoryID());
                }
                //
                if (RFCData.QueueID())
                    self.QueueID = RFCData.QueueID();
                //
                if (RFCData.UrgencyID())
                    self.UrgencyID = RFCData.UrgencyID();
                //
                if (RFCData.ReasonObjectID())
                    self.ReasonCallBackObject(RFCData.ReasonObjectID(), RFCData.ReasonObjectClassID());
                //
                if (RFCData.Target())
                    self.RFCTarget(RFCData.Target());
                //
                if (RFCData.ServiceID()) {
                    self.ServiceItemID(RFCData.ServiceID());
                    self.ServiceItemName(RFCData.ServiceName());
                    self.LoadAddServiceControl();
                }
                //
                if (RFCData.FundingAmount())
                    self.FundingAmountNumber(RFCData.FundingAmount());
                //
                if (RFCData.OwnerID())
                    self.OwnerID = RFCData.OwnerID();
                //
                if (RFCData.Summary())
                    self.Summary(RFCData.Summary());
                //
                if (RFCData.Description())
                    $.when(self.htmlDescriptionControlD).done(function () { self.Description(RFCData.Description()); });
                //
                self.RFCData = RFCData;
            };
            //
            //AfterRender
            {
                self.AfterRender = function () {
                    self.renderedD.resolve();
                    self.LoadAddServiceControl();
                    self.InitFundingAmountNumber();
                };
            }
        },
        AddServicesModel: function (parentSelf, $regionAddUser) {
            var vself = this;
            var self = parentSelf;
            vself.options = {
                searcherName: 'ServiceSearcherForRFC',
                searcherParams: null,
                oldValue: null,
                searcherPlaceholder: getTextResource('ServiceCatalogueBrowserCaption')
            };
            //
            var fh = new fhModule.formHelper();
            vself.LoadD = $.Deferred();
            vself.divID = 'ServicesSearchControl_' + ko.getNewID();//main control div.ID
            vself.$regionAU = $regionAddUser;
            vself.IsLoaded = ko.observable(false);
            //
            vself.Initialize = function () {//NegotiationAddUser используется как шаблон окна
                vself.$regionAU.append('<div id="' + vself.divID + '" data-bind="template: {name: \'Negotiation/NegotiationAddUser\', afterRender: AfterRender}" ></div>');
                //
                try {
                    ko.applyBindings(vself, document.getElementById(vself.divID));
                }
                catch (err) {
                    if (document.getElementById(vself.divID))
                        throw err;
                }
            };
            vself.AfterRender = function () {
                vself.LoadD.resolve();
                vself.CreateSearcherEditor();
                //
                self.$region.find('.negotiation-form-textinput').focus();
            };
            //
            vself.FocusSearcher = function () {
                $.when(vself.LoadD, vself.objectSearcherControlD).done(function () {
                    var textbox = vself.$regionAU.find('.searcherInput');
                    if (textbox.length > 0) {
                        textbox.click();
                        textbox.focus();
                    }
                });
            };
            vself.selectedObjectInfo = null;
            vself.selectedObjectFullName = ko.observable(null);
            vself.emptyPlaceholder = ko.observable(vself.options.searcherPlaceholder);
            vself.objectSearcherControl = null;
            vself.objectSearcherControlD = $.Deferred();
            vself.ajaxControl_loadUserInfo = new ajaxLib.control();
            vself.CreateSearcherEditor = function () {
                require(['objectSearcher'], function (rtbLib) {
                    var loadD = fh.SetTextSearcherToField(
                        vself.$regionAU.find('.searcherInput'),
                        vself.options.searcherName,
                        null,
                        vself.options.searcherParams,
                        function (objectInfo) {//select
                            vself.selectedObjectFullName(objectInfo.FullName);
                            vself.selectedObjectInfo = objectInfo;
                            if (self.FillServiceInfo(objectInfo)) {
                                vself.selectedObjectFullName('');
                                vself.selectedObjectInfo = null;
                                self.SearchService();
                            }
                        },
                        function () {//reset
                            vself.selectedObjectFullName('');
                            vself.selectedObjectInfo = null;
                        },
                        function (selectedItem) {//close
                            if (!selectedItem) {
                                vself.selectedObjectFullName('');
                                vself.selectedObjectInfo = null;
                            }
                        });
                    $.when(loadD).done(function (vm) {
                        vself.objectSearcherControl = vm;
                        //
                        var old = vself.options.oldValue;
                        if (old != null) {
                            vself.selectedObjectFullName(old.FullName);
                            vm.SetSelectedItem(old.ID, old.ClassID, old.FullName, '');
                            vself.selectedObjectInfo = old;
                        }
                        //
                        $.when(vm.LoadD).done(function () {
                            vself.objectSearcherControlD.resolve();
                        });
                    });

                });
                return vself.objectSearcherControlD.promise();
            };
        },
    }
    return module;
});
