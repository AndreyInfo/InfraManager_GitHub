define([
    'knockout',
    'jquery',
    'ajax',
    'models/SDForms/SDForm.LinkList',
    'models/SDForms/SDForm.CallReferenceList',
    'models/SDForms/References/MassIncidentReferencesTab',
    'models/SDForms/SDForm.WOReferenceList',
    'models/SDForms/SDForm.NegotiationList',
    'models/SDForms/SDForm.Tape',
    'models/SDForms/SDForm.KBAReferenceList',
    'dynamicOptionsService',
    'groupOperation',
    'comboBox'],
function (
    ko,
    $,
    ajaxLib,
    linkListLib,
    callReferenceListLib,
    miReferenceListLib,
    workOrderReferenceListLib,
    negotiationListLib,
    tapeLib,
    kbaRefListLib,
    dynamicOptionsService,
    groupOperation
) {
    var module = {
        Classes: {
            User: 9,
            Queue: 722,
        },
        ViewModel: function (isReadOnly, $region, id) {
            //PROPERTIES AND ENUMS BLOCK
            var self = this;
            var $isLoaded = $.Deferred();
            self.$region = $region;
            self.CloseForm = null; // set in fh
            self.ControlForm = null; //set in fh
            self.CurrentUserID = null;
            self.id = id;
            self.objectClassID = 702; //Problem object id
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                tape: 'tape',
                negotiation: 'negotiation',
                links: 'links',
                calls: 'calls',
                workorders: 'workorders',
                userFields: 'userFields',
                massIncidents: 'massIncidents'
            };
            $.when(userD).done(function (user) {
                self.CurrentUserID = user.UserID;
            });
            self.TabHeight = ko.observable(0);
            self.TabWidth = ko.observable(0);
            self.RefreshParameters = ko.observable(false);
            self.TabSize = ko.computed(function () {
                return {
                    h: self.TabHeight(),
                    w: self.TabWidth()
                };
            });
            //
            self.negotiationID = null;//negotiation to show 
            //
            //MAIN TAB BLOCK
            self.mainTabLoaded = ko.observable(false);
            self.solutionTabLoaded = ko.observable(false);
            self.mode = ko.observable(self.modes.nothing);
            self.mode.subscribe(function (newValue) {
                if (newValue == self.modes.nothing)
                    return;
                //
                if (newValue == self.modes.main) {
                    if (!self.mainTabLoaded()) {
                        self.mainTabLoaded(true);
                        //
                        self.SizeChanged();
                    }
                    self.kbaRefList.CheckData();
                }
                else if (newValue == self.modes.tape)
                    self.tapeControl.CheckData();
                else if (newValue == self.modes.workorders)
                    self.workOrderList.CheckData();
                else if (newValue == self.modes.calls)
                    self.callList.CheckData();
                else if (newValue == self.modes.links)
                    self.linkList.CheckData();
                else if (newValue == self.modes.negotiation)
                    self.negotiationList.CheckData(self.negotiationID);
                else if (newValue == self.modes.userFields)
                    self.CheckUserFields();
            });

            self.ClickMain = function () {
                self.mode(self.modes.main)
            }
            //
            //User Fields
            {
                self.UserFieldType = 4;
                self.UserFields = ko.observable(null);
                require(['models/SDForms/SDForm.UserFields'], function (ufLib) {
                    self.UserFields(new ufLib.UserFields(self.UserFieldType));
                });

                //
                self.InitializeUserFields = function () {
                    self.SetUserFields();
                    self.UserFields().ReadOnly(self.IsReadOnly());
                    self.UserFieldsLoaded(true);
                };
                self.CheckUserFields = function () {
                    if (!self.UserFieldsLoaded()) {
                        self.SetUserFields();
                        self.UserFieldsLoaded(true);
                    }
                };

                self.UserFieldsLoaded = ko.observable(false);
                self.SetUserFields = function () {
                    self.UserFields().SetFields(self.problem());
                };
            }
            //
            //     
            self.attachmentsControl = null;
            self.LoadAttachmentsControl = function () {
                if (!self.problem())
                    return;
                //
                require(['fileControl'], function (fcLib) {
                    if (self.attachmentsControl != null) {
                        if (self.attachmentsControl.ObjectID != self.problem().ID())//previous object  
                            self.attachmentsControl.RemoveUploadedFiles();
                        else if (!self.attachmentsControl.IsAllFilesUploaded())//uploading
                        {
                            setTimeout(self.LoadAttachmentsControl, 1000);//try to reload after second
                            return;
                        }
                    }
                    if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                        var attachmentsElement = self.$region.find('.documentList');
                        self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        self.attachmentsControl.OnChange = function (URL, onSuccess, fileId, isDeleting) {
                            if (self.workflowControl() != null) {
                                if (isDeleting) {
                                    URL = URL + self.objectClassID + "/" + self.attachmentsControl.ObjectID + "/documents/" + fileId;
                                    ajaxConfig =
                                    {
                                        url: URL,
                                        method: "Delete",
                                        dataType: "json"
                                    };
                                    self.workflowControl().OnSave(URL, onSuccess, ajaxConfig);
                                }
                                else {
                                    URL = URL + self.objectClassID + "/" + self.attachmentsControl.ObjectID + "/documents";
                                    ajaxConfig =
                                    {
                                        url: URL,
                                        method: "Post",
                                        dataType: "json",
                                        data: { 'docID': fileId }
                                    };
                                    self.workflowControl().OnSave(URL, onSuccess, ajaxConfig);
                                }
                            }
                        };
                    }
                    self.attachmentsControl.ReadOnly(self.IsReadOnly());
                    self.attachmentsControl.Initialize(self.problem().ID());
                });
            };
            //
            self.workflowControl = ko.observable(null);
            self.LoadWorkflowControl = function () {
                if (!self.problem())
                    return;
                //
                require(['workflow'], function (wfLib) {
                    if (self.workflowControl() == null) {
                        self.workflowControl(new wfLib.control(self.$region, self.IsReadOnly, self.problem));
                    }
                    self.workflowControl().ReadOnly(self.IsReadOnly());
                    self.workflowControl().Initialize();
                });
            };
            //
            self.EditManhoursWork = function () {
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper();
                    fh.ShowManhoursWorkList(self.problem, self.objectClassID, self.CanEdit);
                });
            };
            //
            self.priorityControl = null;
            self.LoadPriorityControl = function () {
                if (!self.problem())
                    return;
                //
                require(['models/SDForms/SDForm.Priority'], function (prLib) {
                    if (self.priorityControl == null || self.priorityControl.IsLoaded() == false) {
                        self.priorityControl = new prLib.ViewModel(self.$region.find('.b-requestDetail-menu__priority'), self, self.IsReadOnly());
                        self.priorityControl.Initialize();
                    }
                    $.when(self.priorityControl.Load(self.problem().ID(), self.objectClassID, self.problem().UrgencyID(), self.problem().InfluenceID(), self.problem().PriorityID())).done(function (result) {                        
                    });
                });
            };
            //
            self.ajaxControl_SetCustomControl = new ajaxLib.control();
            self.RefreshPriority = function (priorityObj) {
                if (priorityObj == null)
                    return;
                
                self.problem().PriorityName(priorityObj.Name);
                self.problem().PriorityColor(priorityObj.Color);
                self.problem().PriorityID(priorityObj.ID);
                self.problem().InfluenceID(self.priorityControl.CurrentInfluenceID());
                self.problem().UrgencyID(self.priorityControl.CurrentUrgencyID());

                var model = {
                    InfluenceID: self.priorityControl.CurrentInfluenceID(),
                    UrgencyID: self.priorityControl.CurrentUrgencyID(),
                    PriorityID: priorityObj.ID
                }
                self.ajaxControl_SetCustomControl.Ajax(
                    null,
                    {
                        dataType: "json",
                        contentType: "application/json",
                        method: "PATCH",
                        url: '/api/problems/' + self.problem().ID(),
                        data: JSON.stringify(model)
                    },
                    function () {                
                    });
            };
            
            self.ajaxControl_CustomControl = new ajaxLib.control();
            self.ajaxControl_CustomControlUsers = new ajaxLib.control();
            self.CustomControl = ko.observable(false);
            self.LoadCustomControl = function () {
                if (!self.problem())
                    return;
                //
                var param = {
                    objectID: self.problem().ID(),
                };
                self.ajaxControl_CustomControl.Ajax(self.$region.find('.b-requestDetail-menu__item-control'),
                    {
                        method: 'GET',
                        url: '/api/problems/' + param.objectID  + '/customControls/my'
                    },
                    function (details) {
                        self.CustomControl(details.UnderControl);
                    });
                //
                var param2 = {
                    objectID: self.problem().ID(),
                    objectClassID: self.objectClassID
                };
                self.ajaxControl_CustomControlUsers.Ajax(self.$region.find('.b-requestDetail-menu__item-control'),
                    {
                        method: 'GET',
                        url: '/userApi/GetUserInfoListOnCustomControl?' + $.param(param2)
                    },
                    function (result) {
                        if (result && result.length > 0)
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                ko.utils.arrayForEach(result, function (el) {
                                    var already = ko.utils.arrayFirst(self.ProblemUsersList(), function (item) {
                                        return item.ID() == el.ID;
                                    });
                                    //                    
                                    if (already != null)
                                        return;
                                    //
                                    var options = {
                                        UserID: el.ID,
                                        UserType: userLib.UserTypes.inspector,
                                        UserName: el.FullName,
                                        EditAction: null,
                                        RemoveAction: null,
                                        UserData: el,
                                        CanNote: true
                                    };
                                    var user = new userLib.User(self, options);
                                    //
                                    self.ProblemUsersList.push(user);
                                });
                            });
                    });
            };
            self.SaveCustomControl = function () {
                if (!self.problem())
                    return;
                //
                var data = {
                    UnderControl: self.CustomControl()
                };
                self.ajaxControl_CustomControl.Ajax(self.$region.find('.b-requestDetail-menu__item-control'),
                    {
                        dataType: 'json',
                        contentType: 'application/json',
                        method: 'PUT',
                        data: JSON.stringify(data),
                        url: '/api/problems/' + self.problem().ID() + '/customControls/my/'
                    },
                    function (details) {
                        self.CustomControl(details.UnderControl);
                    },
                    function () {
                        self.CustomControl(!self.CustomControl());//restore
                        //
                        require(['sweetAlert'], function () {
                            swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                        });
                    });
            };
            //
            self.problem = ko.observable(null);
            self.problem.subscribe(function (newValue) {
                $.when($isLoaded).done(function () {
                    self.SizeChanged();//block of problem completed
                    //
                    self.LoadAttachmentsControl();
                    self.LoadWorkflowControl();
                    if (self.IsReadOnly() == false) {
                        self.LoadPriorityControl();
                    }

                    if (!self.massIncidentList) {
                        var referenceData = { ProblemID: newValue.ID() };

                        self.massIncidentList = new miReferenceListLib.ViewModel(
                            self.$region,
                            miReferenceListLib.AddProblemViewModelCreator(newValue.ID()),
                            miReferenceListLib.RemoveProblemViewModelCreator(newValue.ID()), {
                                tab: {
                                    appendTemplate: 'SDForms/References/AddReference',
                                    isReadOnly: self.IsReadOnly,
                                    canEdit: self.CanEdit
                                },
                                list: {
                                    view: 'ProblemMassIncidents',
                                    ajax: {
                                        method: 'POST',
                                        url: '/api/massIncidents/reports/problemMassIncidents',
                                        data: referenceData
                                    }
                                },
                                append: {
                                    view: 'MassIncidentsToAssociate',
                                    ajax: {
                                        method: 'POST',
                                        url: '/api/massIncidents/reports/MassIncidentsToAssociate',
                                        data: referenceData
                                    },
                                    getMassIncidentUri: function (massIncident) {
                                        return '/api/massIncidents/' + massIncident.ID;
                                    }
                                }
                        });
                    };
                });
            });
            self.IsReadOnly = ko.observable(isReadOnly);
            self.IsReadOnly.subscribe(function (newValue) {
                var readOnly = self.IsReadOnly();
                //
                if (self.attachmentsControl != null)
                    self.attachmentsControl.ReadOnly(readOnly);
                if (self.workflowControl() != null)
                    self.workflowControl().ReadOnly(readOnly);
                if (self.priorityControl != null)
                    self.priorityControl.ReadOnly(readOnly);
                if (self.linkList != null)
                    self.linkList.ReadOnly(readOnly);
                if (self.callList != null)
                    self.callList.ReadOnly(readOnly);
                if (self.negotiationList != null) {
                    //all inside control already
                }
            });
            //
            self.CanEdit = ko.computed(function () {
                return !self.IsReadOnly();
            });
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.additionalClick = function () {
                //TODO
            };
            self.CustomControlClick = function () {//поставить/снять с контроля
                self.CustomControl(!self.CustomControl());
                self.SaveCustomControl();
            };
            //
            self.showContextMenu = function (obj, e) {
                var contextMenuViewModel = self.contextMenu();
                e.preventDefault();
                contextMenuViewModel.show(e);
                return true;
            };
            //
            self.contextMenu = ko.observable(null);

            self.contextMenuInit = function (contextMenu) {
                self.contextMenu(contextMenu);//bind contextMenu

                self.setCustomControl(contextMenu);
                //Поставить на контроль 
                self.removeCustomControl(contextMenu);
                //Снять с контроля 
            };
            //
            self.contextMenuOpening = function (contextMenu) {
                contextMenu.items().forEach(function (item) {
                    if (item.isEnable && item.isVisible) {
                        item.enabled(item.isEnable());
                        item.visible(item.isVisible());
                    }
                });
            };
            self.getItemInfos = function () {
                var retval = [];
                retval.push({
                    ClassID: self.objectClassID,
                    ID: self.id
                });
                return retval;
            };

            function toggleCustomControl(selectedUsers, underControl) {
                showSpinner();

                var groupOperationViewModel =
                    new groupOperation.CustomControlViewModel(
                        [{ Uri: '/api/problems/' + self.id }],
                        [ selectedUsers ],
                        underControl,
                        function (item) {
                            if (item.UserID == self.CurrentUserID)
                                self.CustomControlClick(underControl);
                        },
                        hideSpinner);

                groupOperationViewModel.start();
            }
            self.setCustomControl = function (contextMenu) {
                var isEnable = function () {
                    return true;
                };
                var isVisible = function () {
                    return true;
                };
                var action = function () {
                    var callback = function (selectedUserList) {
                        toggleCustomControl(selectedUserList, true);
                    };
                    //
                    require(['sdForms'], function (fhModule) {
                        var fh = new fhModule.formHelper();
                        var userInfo = {
                            UserID: self.CurrentUserID,
                            CustomControlObjectID: self.id,
                            CustomControlObjectClassID: self.objectClassID,
                            SetCustomControl: true,
                            UseTOZ: true
                        };
                        fh.ShowUserSearchForm(userInfo, callback);
                    });
                }
                var cmd = contextMenu.addContextMenuItem();
                cmd.restext('SetCustomControl');
                cmd.isEnable = isEnable;
                cmd.isVisible = isVisible;
                cmd.click(action);
            };
            //
            self.removeCustomControl = function (contextMenu) {
                var isEnable = function () {
                    return true;
                };
                var isVisible = function () {
                    return true;
                };
                var action = function () {
                    var callback = function (selectedUserList) {
                        toggleCustomControl(selectedUserList, false);
                    };
                    //
                    require(['sdForms'], function (fhModule) {
                        var fh = new fhModule.formHelper();
                        var userInfo = {
                            UserID: self.CurrentUserID,
                            CustomControlObjectID: self.id,
                            CustomControlObjectClassID: self.objectClassID,
                            SetCustomControl: false,
                            UseTOZ: true
                        };
                        fh.ShowUserSearchForm(userInfo, callback);
                    });
                }
                var cmd = contextMenu.addContextMenuItem();
                cmd.restext('RemoveCustomControl');
                cmd.isEnable = isEnable;
                cmd.isVisible = isVisible;
                cmd.click(action);
            };
            //
            self.SendMail = function () {
                showSpinner();
                require(['sdForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        Obj: {
                            ID: self.id,
                            ClassID: self.objectClassID
                        },
                        CanNote: true, 
                        Subject: getTextResource('Problem') + (self.problem() != null ? (' №' + self.problem().Number() + ' ' + self.problem().Summary()) : '')
                    }
                    fh.ShowSendEmailForm(options);
                });
            };

            function editOnWorkOrderExecutorControl(newVal) {
                const oldVal = self.problem().OnWorkOrderExecutorControl();
                if (oldVal !== newVal) {
                    put({ OnWorkOrderExecutorControl: newVal });
                }
            }

            function put(model) {
                showSpinner();
                var retD = $.Deferred();
                self.ajaxControl_SetCustomControl.Ajax(
                    null,
                    {
                        dataType: "json",
                        contentType: "application/json",
                        method: "PUT",
                        url: '/api/problems/' + id,
                        data: JSON.stringify(model)
                    },
                    function (problem) {
                        self.problem().TypeID(problem.TypeID);
                        self.problem().TypeName(problem.TypeName);
                        self.problem().HTMLDescription(problem.HTMLDescription);
                        self.problem().HTMLSolution(problem.HTMLSolution);
                        self.problem().HTMLCause(problem.HTMLCause);
                        self.problem().HTMLFix(problem.HTMLFix);
                        self.problem().ProblemCauseID(problem.ProblemCauseID || '');
                        self.problem().ProblemCauseName(problem.ProblemCauseName || '');
                        self.problem().Summary(problem.Summary);
                        self.problem().ManhoursNorm(problem.MahoursNormInMinutes);
                        
                        if (self.problem().OwnerID() != problem.OwnerID) {
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                self.problem().OwnerLoaded(false);
                                self.problem().Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                                //
                                self.problem().OwnerID(problem.OwnerID || "");
                                self.InitializeOwner();
                            });
                        };

                        if (self.problem().QueueID() != problem.QueueID) {
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                self.problem().QueueLoaded(false);
                                self.problem().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                                self.problem().QueueID(problem.QueueID);
                                self.problem().QueueName(problem.QueueName);
                                self.InitializeQueue();
                            });
                        };

                        if(self.problem().ExecutorID() != problem.ExecutorID) {
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                self.problem().ExecutorLoaded(false);
                                self.problem().Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                                //
                                self.problem().ExecutorID(problem.ExecutorID || "");
                                self.InitializeExecutor();
                            });
                        };

                        if (self.problem().InitiatorID() != problem.InitiatorID) {
                            require(['models/SDForms/SDForm.User'], function (userLib) {
                                self.problem().InitiatorLoaded(false);
                                self.problem().Initiator(new userLib.EmptyUser(self, userLib.UserTypes.initiator, self.EditInitiator));
                                self.problem().InitiatorID(problem.InitiatorID || '');
                                self.InitializeInitiator();
                            });
                        }

                        self.problem().UtcDatePromised(parseDate(problem.UtcDatePromised));
                        self.problem().UtcDatePromisedDT(new Date(parseInt(problem.UtcDatePromised)));
                        //
                        if (self.tapeControl && self.tapeControl.TimeLineControl && self.tapeControl.isTimeLineLoaded && self.tapeControl.isTimeLineLoaded()) {
                            var mainTLC = self.tapeControl.TimeLineControl();
                            if (mainTLC != null && mainTLC.TimeLine) {
                                var currentTL = mainTLC.TimeLine();
                                if (currentTL != null && currentTL.UtcDatePromised) {
                                    currentTL.UtcDatePromised.LocalDate(self.problem().UtcDatePromised());
                                    currentTL.UtcDatePromised.DateObj(self.problem().UtcDatePromisedDT());
                                }
                            }
                        }

                        self.DynamicOptionsService.ResetData();

                        retD.resolve(true);
                    }, function (e) {
                        retD.resolve(false);
                }, function () {
                        hideSpinner();
                });
                return retD;
            }
                //
            self.EditProblemType = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.Type',
                        fieldFriendlyName: getTextResource('ProblemType'),
                        oldValue: { ID: self.problem().TypeID(), ClassID: 708, FullName: self.problem().TypeName() },
                        searcherName: 'ProblemTypeSearcher',
                        save: function (data) {
                            return put({
                                TypeID: JSON.parse(data.NewValue).id
                            });
                        },
                        allowNull: false
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.EditOwner = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.Owner',
                        fieldFriendlyName: getTextResource('Owner'),
                        oldValue: self.problem().OwnerLoaded() ? { ID: self.problem().Owner().ID(), ClassID: 9, FullName: self.problem().Owner().FullName() } : null,
                        object: ko.toJS(self.problem().Owner()),
                        searcherName: 'OwnerUserSearcher',
                        searcherPlaceholder: getTextResource('EnterFIO'),
                        save: function (data) {
                            return put({ OwnerID: data.NewValue ? JSON.parse(data.NewValue).id : null });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            self.EditInitiator = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.Initiator',
                        fieldFriendlyName: getTextResource('Initiator'),
                        oldValue: self.problem().InitiatorLoaded() ? { ID: self.problem().Initiator().ID(), ClassID: 9, FullName: self.problem().Initiator().FullName() } : null,
                        object: ko.toJS(self.problem().Initiator()),
                        searcherName: 'WebUserSearcher',
                        searcherPlaceholder: getTextResource('EnterFIO'),
                        save: function (data) {
                            return put({ InitiatorID: data.NewValue ? JSON.parse(data.NewValue).id : null });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            }
            self.EditQueue = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.Queue',
                        fieldFriendlyName: getTextResource('Queue'),
                        oldValue: self.problem().QueueLoaded() ? { ID: self.problem().Queue().ID(), ClassID: self.problem().Queue().ClassID(), FullName: self.problem().Queue().FullName() } : null,
                        object: ko.toJS(self.problem().Queue()),
                        searcherName: "QueueSearcher",
                        searcherPlaceholder: getTextResource('EnterQueue'),
                        searcherParams: { Type: 8 },
                        onSave: function (objectInfo) {
                            self.problem().QueueLoaded(false);
                            self.problem().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                            //
                            if (objectInfo && objectInfo.ClassID === module.Classes.Queue) {
                                self.problem().QueueID(objectInfo.ID);
                                self.problem().QueueName(objectInfo.FullName);
                            }
                            else {
                                self.problem().QueueID('');
                                self.problem().QueueName('');
                            }
                            self.InitializeQueue();
                        },
                        save: function (data) {
                            if (!data.NewValue) {
                                return put({ QueueID: null });
                            };

                            const id = JSON.parse(data.NewValue).id;
                            return self.SaveQueue(id);
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            }
            self.EditExecutor = function () {
                require(['usualForms'], function (module) {
                    const fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.Executor',
                        fieldFriendlyName: getTextResource('Executor'),
                        oldValue: self.problem().ExecutorLoaded() ? { ID: self.problem().Executor().ID(), ClassID: 9, FullName: self.problem().Executor().FullName() } : null,
                        object: ko.toJS(self.problem().Executor()),
                        searcherName: 'ExecutorUserSearcher',
                        searcherPlaceholder: getTextResource('EnterFIO'),
                        searcherParams: { QueueId: self.QueueID },
                        save: function (data) {
                            return put({ ExecutorID: data.NewValue ? JSON.parse(data.NewValue).id : null });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            self.SaveQueue = function (queueID) {
                if (!queueID) {
                    return false;
                };

                let queueD = $.Deferred();
                const ajaxControl = new ajaxLib.control();
                ajaxControl.Ajax(null,
                    {
                        dataType: 'json',
                        contentType: 'application/json',
                        method: 'GET',
                        url: '/api/groups/' + queueID,
                    },
                    function (response) { queueD.resolve(response); }
                );

                $.when(queueD).done(function (queue) {
                    const executorID = self.problem().ExecutorLoaded() && self.problem().ExecutorID() ? self.problem().ExecutorID() : null;
                    let request = { QueueID: queueID };
                    if (queue && queue.QueueUserList && executorID) {
                        const queueUsers = queue.QueueUserList.map(i => i.ID);
                        if (!queueUsers.includes(executorID)) {
                            request = { ExecutorID: null, QueueID: queueID, };
                        }
                    }
                    put(request);
                });

                return true;
            }
            self.SaveExecutor = function (executorID) {
                if (!executorID) {
                    return false;
                }
                let request = { ExecutorID: executorID };
                const queue = self.problem().QueueLoaded() && self.problem().QueueID() ? self.problem().Queue() : null;
                if (queue) {
                    const queueUsers = queue.QueueUserIDList();
                    if (!queueUsers.includes(executorID)) {
                        request = { ExecutorID: executorID, QueueID: null, };
                    }
                }
                return put(request);
            }
            self.SaveExecutorOrQueue = function (obj) {
                if (!obj || self.CanEdit() === false) {
                    return false;
                }
                if (obj.ClassID === module.Classes.Queue) {
                    return self.SaveQueue(obj.ID);
                } else if (obj.ClassID === module.Classes.User) {
                    return self.SaveExecutor(obj.ID);
                }
                return false;
            }
            self.EditDescription = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.HTMLDescription',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: self.problem().HTMLDescription(),
                        save: function (data) {
                            return put({
                                HTMLDescription: JSON.parse(data.NewValue).text
                            });
                        },
                        readOnly: !self.CanEdit()
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                });
            };
            self.EditSolution = function () {                
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.HTMLSolution',
                        fieldFriendlyName: getTextResource('Solution'),
                        oldValue: self.problem().HTMLSolution(),
                        save: function (data) {
                            return put({
                                HTMLSolution: JSON.parse(data.NewValue).text
                            });
                        },
                        readOnly: !self.CanEdit()
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                });
            };
            self.EditFix = function () {               
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.HTMLFix',
                        fieldFriendlyName: getTextResource('Fix'),
                        oldValue: self.problem().HTMLFix(),
                        save: function (data) {
                            return put({
                                HTMLFix: JSON.parse(data.NewValue).text
                            });
                        },
                        readOnly: !self.CanEdit()
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                });
            };
            self.EditCause = function () {                
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.HTMLCause',
                        fieldFriendlyName: getTextResource('Cause'),
                        oldValue: self.problem().HTMLCause(),
                        save: function (data) {
                            return put({
                                HTMLCause: JSON.parse(data.NewValue).text
                            });
                        },
                        readOnly: !self.CanEdit()
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                });
            };
            self.EditShortCause = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.ProblemCause',
                        fieldFriendlyName: getTextResource('ShortCause'),
                        oldValue: self.problem().ProblemCauseID() ? { ID: self.problem().ProblemCauseID(), ClassID: 709, FullName: self.problem().ProblemCauseName() } : null,
                        searcherName: 'ProblemCauseSearcher',
                        searcherPlaceholder: getTextResource('EnterProblemCause'),
                        save: function (data) {
                            return put({
                                ProblemCauseID: data.NewValue ? JSON.parse(data.NewValue).id : null
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            self.EditSummary = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.Summary',
                        fieldFriendlyName: getTextResource('Summary'),
                        oldValue: self.problem().Summary(),
                        save: function (data) {
                            return put({ Summary: JSON.parse(data.NewValue).text });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            self.EditDatePromised = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.DatePromised',
                        fieldFriendlyName: getTextResource('ProblemDatePromise'),
                        oldValue: self.problem().UtcDatePromisedDT(),
                        save: function (data) {
                            return put({ UtcDatePromised: JSON.parse(data.NewValue).text });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                });
            };
            self.EditManhoursNorm = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Problem.ManhoursNorm',
                        fieldFriendlyName: getTextResource('ManhoursNorm'),
                        oldValue: self.problem().ManhoursNorm(),
                        save: function (data) {
                            return put({ ManhoursNormInMinutes: JSON.parse(data.NewValue).val });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.timeEdit, options);
                });
            };
            //
            self.ajaxControl_deleteUser = new ajaxLib.control();
            self.DeleteUser = function (options) {
                var data = {};
                data[options.FieldName] = null;
                //
                self.ajaxControl_deleteUser.Ajax(
                            self.$region,
                            {
                                dataType: 'json',
                                contentType: 'application/json',
                                method: 'PATCH',
                                url: '/api/problems/' + self.problem().ID(),
                                data: JSON.stringify(data)
                            },
                            function (result, status) {
                                if (status !== 'success') {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[ProblemForm.js, DeleteUser]', 'error');
                                    });
                                }
                            });
            };
            self.DeleteOwner = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'OwnerID',
                        onSave: function () {
                            self.problem().OwnerLoaded(false);
                            self.problem().Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                            //
                            self.problem().OwnerID('');
                        }
                    };
                    self.DeleteUser(options);
                });
            };
            self.DeleteInitiator = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'InitiatorID',
                        onSave: function () {
                            self.problem().InitiatorLoaded(false);
                            self.problem().Initiator(new userLib.EmptyUser(self, userLib.UserTypes.initiator, self.EditInitiator));
                            //
                            self.problem().InitiatorID('');
                        }
                    };
                    self.DeleteUser(options);
                });
            };
            self.DeleteQueue = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'QueueID',
                        onSave: function () {
                            self.problem().QueueLoaded(false);
                            self.problem().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                            //
                            self.problem().QueueID('');
                            self.problem().QueueName('');
                        }
                    };
                    self.DeleteUser(options);
                });
            }
            self.DeleteExecutor = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'ExecutorID',
                        onSave: function () {
                            self.problem().ExecutorLoaded(false);
                            self.problem().Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                            //
                            self.problem().ExecutorID('');
                        }
                    };
                    self.DeleteUser(options);
                });
            };
            //
            self.InitializeOwner = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var p = self.problem();
                    if (p.OwnerLoaded() == false && p.OwnerID()) {
                        var options = {
                            UserID: p.OwnerID(),
                            UserType: userLib.UserTypes.owner,
                            UserName: null,
                            EditAction: self.EditOwner,
                            RemoveAction: self.DeleteOwner,
                            CanNote: true
                        };
                        var user = new userLib.User(self, options);
                        p.Owner(user);
                        p.OwnerLoaded(true);
                        //
                        var already = ko.utils.arrayFirst(self.ProblemUsersList(), function (item) {
                            return item.ID() == p.OwnerID();
                        });
                        //
                        if (already == null)
                            self.ProblemUsersList.push(user);
                        else if (already.Type == userLib.UserTypes.withoutType) {
                            self.ProblemUsersList.remove(already);
                            self.ProblemUsersList.push(user);
                        }
                    }
                });
            };

            self.InitializeInitiator = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    const p = self.problem();
                    if (p.InitiatorLoaded() == false && p.InitiatorID()) {
                        const options = {
                            UserID: p.InitiatorID(),
                            UserType: userLib.UserTypes.workOrderInitiator,
                            UserName: null,
                            EditAction: self.EditInitiator,
                            RemoveAction: self.DeleteInitiator,
                            CanNote: true
                        };
                        const user = new userLib.User(self, options);
                        p.Initiator(user);
                        p.InitiatorLoaded(true);
                        //
                        const already = ko.utils.arrayFirst(self.ProblemUsersList(), function (item) {
                            return item.ID() == p.InitiatorID();
                        });
                        //                    
                        if (already == null)
                            self.ProblemUsersList.push(user);
                        else if (already.Type == userLib.UserTypes.withoutType) {
                            self.ProblemUsersList.remove(already);
                            self.ProblemUsersList.push(user);
                        }
                    }
                });
            };

            self.InitializeQueue = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    const p = self.problem();
                    //
                    if (p.QueueLoaded() == false) {
                        if (p.QueueID()) {
                            const options = {
                                UserID: p.QueueID(),
                                UserType: userLib.UserTypes.queueExecutor,
                                UserName: null,
                                EditAction: self.EditQueue,
                                RemoveAction: self.DeleteQueue,
                                CanNote: true,
                                IsFreezeSelectedClient: true
                            };
                            const user = new userLib.User(self, options);
                            p.Queue(user);
                            p.QueueLoaded(true);
                            //
                            const already = ko.utils.arrayFirst(self.ProblemUsersList(), function (item) {
                                return item.ID() == p.QueueID();
                            });
                            //
                            if (already == null)
                                self.ProblemUsersList.push(user);
                            else if (already.Type == userLib.UserTypes.withoutType) {
                                self.ProblemUsersList.remove(already);
                                self.ProblemUsersList.push(user);
                            }
                        }
                    }
                });
            };

            self.InitializeExecutor = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    const p = self.problem();
                    //
                    if (p.ExecutorLoaded() == false) {
                        if (p.ExecutorID()) {
                            const options = {
                                UserID: p.ExecutorID(),
                                UserType: userLib.UserTypes.executor,
                                UserName: null,
                                EditAction: self.EditExecutor,
                                RemoveAction: self.DeleteExecutor
                            };
                            const user = new userLib.User(self, options);
                            p.Executor(user);
                            p.ExecutorLoaded(true);
                            //
                            const already = ko.utils.arrayFirst(self.ProblemUsersList(), function (item) {
                                return item.ID() == p.ExecutorID();
                            });
                            //
                            if (already == null)
                                self.ProblemUsersList.push(user);
                            else if (already.Type == userLib.UserTypes.withoutType) {
                                self.ProblemUsersList.remove(already);
                                self.ProblemUsersList.push(user);
                            }
                        }
                    }
                });
            };

            self.CalculateUsersList = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    if (!self.problem()) {
                        self.ProblemUsersList([]);
                        self.ProblemUsersList.valueHasMutated();
                        return;
                    }
                    
                    self.InitializeInitiator();
                    self.InitializeOwner();
                    self.InitializeQueue();
                    self.InitializeExecutor();

                    //
                    self.ProblemUsersList.valueHasMutated();
                    //add currentUser to list
                    $.when(userD).done(function (userObj) {
                        require(['models/SDForms/SDForm.User'], function (userLib) {
                            var already = ko.utils.arrayFirst(self.ProblemUsersList(), function (item) {
                                return item.ID() == userObj.UserID;
                            });
                            //                    
                            if (already != null)
                                return;
                            //
                            var options = {
                                UserID: userObj.UserID,
                                UserType: userLib.UserTypes.withoutType,
                                UserName: userObj.UserName,
                                EditAction: null,
                                RemoveAction: null,
                                CanNote : true
                            };
                            var user = new userLib.User(self, options);
                            //
                            self.ProblemUsersList.push(user);
                        });
                    });
                    //
                    self.LoadCustomControl();
                });
            };
            //
            self.ProblemUsersList = ko.observableArray([]);

            self.ajaxControl_service = new ajaxLib.control();
            self.EditServiceItemAttendance = function () {
                if (!self.CanEdit()) {
                    return;
                };

                const setService = function (serviceID, serviceName, serviceCategoryName) {
                    const problem = self.problem();
                    //
                    problem.ServiceID(serviceID);
                    problem.ServiceName(serviceName);
                    problem.ServiceCategoryName(serviceCategoryName);

                    return put({ ServiceID: serviceID, ServiceName: serviceName, ServiceCategoryName: serviceCategoryName });
                };

                const extensionLoadD = $.Deferred();
                
                showSpinner();

                require(['usualForms'], function (module) {
                    const fh = new module.formHelper(true);
                    const options = {
                        ID: self.problem().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'ProblemService',
                        fieldFriendlyName: getTextResource('Service'),
                        oldValue: {
                            ID: self.problem().ServiceID(),
                            ClassID: self.objectClassID,
                            FullName: `${self.problem().ServiceCategoryName()} \\ ${self.problem().ServiceName()}`
                        },
                        allowNull: false,
                        searcherName: 'ServiceSearcher',
                        searcherTemplateName: 'SearchControl/SearchServiceItemAttendanceControl',//иной шаблон, дополненительные кнопки
                        searcherParams: { Types: [0, 1] },
                        searcherPlaceholder: getTextResource('PromptServiceItem'),
                        searcherLoadD: extensionLoadD,//ожидание дополнений для модели искалки
                        searcherLoad: function (vm, setObjectInfoFunc) {//дополнения искалки
                            vm.CurrentUserID = null;//для фильтрации данных по доступности их пользователю (клиенту)
                            vm.SelectFromServiceCatalogueClick = function () {//кнопка выбора из каталога сервисов
                                vm.Close();//close dropDownMenu
                                if (!self.problem())
                                    return;

                                showSpinner();

                                const mode = fh.ServiceCatalogueBrowserMode;
                                const result = fh.ShowServiceCatalogueBrowser(mode, null, self.problem().ServiceID(), 'OnlyService');

                                $.when(result).done(function (serviceItem) {
                                    if (serviceItem && setObjectInfoFunc) {
                                        setObjectInfoFunc({
                                            ID: serviceItem.ID,
                                            FullName: serviceItem.Name,
                                        });
                                    };
                                });
                            };
                            vm.HardToChooseClick = function () {
                            };//кнопка затрудняюсь выбрать
                            vm.HardToChooseClickVisible = ko.observable(false);
                            //
                            extensionLoadD.resolve();//можно рендерить искалку, формирование модели окончено
                            $.when(vm.LoadD).done(function () {
                                $('#' + vm.searcherDivID).find('.ui-dialog-buttonset').css({ opacity: 1 });//show buttons
                            });
                        },
                        onSave: function (objectInfo) {
                            if (!objectInfo)
                                return;

                            self.ajaxControl_service.Ajax(self.$region.find('.fieldService'),
                                {
                                    url: '/api/service/item?id=' + objectInfo.ID,
                                    method: 'GET'
                                },
                                function (response) {
                                    if (response) {
                                        setService(response.ID, response.Name, response.ServiceCategoryName);
                                    } else {
                                        self.ServiceSet(null, '', '');
                                    };
                                });
                        },
                        nosave: true,
                        newSearch: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };

            //
            self.DatePromisedCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                var retval = '';
                //
                if (self.tapeControl && self.tapeControl.TimeLineControl && self.tapeControl.isTimeLineLoaded && self.tapeControl.isTimeLineLoaded()) {
                    var mainTLC = self.tapeControl.TimeLineControl();
                    if (mainTLC != null && mainTLC.TimeLine) {
                        var currentTL = mainTLC.TimeLine();
                        if (currentTL != null && currentTL.UtcDatePromised)
                            retval = currentTL.UtcDatePromised.LocalDate();
                    }
                }
                //
                if (!retval && self.problem) {
                    var p = self.problem();
                    if (p && p.UtcDatePromised)
                        retval = p.UtcDatePromised();
                }
                //
                return retval;
            });
            //
            self.DateModifyCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                var retval = '';
                //
                if (self.tapeControl && self.tapeControl.TimeLineControl && self.tapeControl.isTimeLineLoaded && self.tapeControl.isTimeLineLoaded()) {
                    var mainTLC = self.tapeControl.TimeLineControl();
                    if (mainTLC != null && mainTLC.TimeLine) {
                        var currentTL = mainTLC.TimeLine();
                        if (currentTL != null && currentTL.UtcDateModified)
                            retval = currentTL.UtcDateModified.LocalDate();
                    }
                }
                //
                if (!retval && self.problem) {
                    var p = self.problem();
                    if (p && p.UtcDateModified)
                        retval = p.UtcDateModified();
                }
                //
                return retval;
            });
            //
            self.LeftPanelModel = null;
            self.HideShowLeftPanel = function () {
                if (!self.CanEdit())
                    return;
                //
                if (self.LeftPanelModel == null) {
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper();
                        if (self.LeftPanelModel == null) //multiple clicks
                            fh.ShowHelpSolutionPanel(self.ControlForm, self, self.problem);
                    });
                    //
                    return;
                }
                //
                var current = self.LeftPanelModel.IsVisible();
                self.LeftPanelModel.IsVisible(!current);
            };
            self.AddTextToSolution = function (newText) {
                if (!self.problem() || !newText)
                    return;
                //
                var currentValue = self.problem().HTMLSolution();
                if (!currentValue)
                    currentValue = '';
                var delimeter = '\n <div><br></div> ';
                var newValue = currentValue + delimeter + newText;
                put({ HTMLSolution: newValue });
            };
            self.ajaxControl_updateTextField = new ajaxLib.control();
            //
            //TAPE BLOCK
            self.TapeClick = function () {
                self.mode(self.modes.tape);
                self.SizeChanged();
                //
                if (self.tapeControl && self.tapeControl.CalculateTopPosition)
                    self.tapeControl.CalculateTopPosition();
            };
            self.CanViewNotes = ko.computed(function () {
                return true;
            });
            const timelineConfig = [
                { name: 'UtcDateDetected' },
                { name: 'UtcDatePromised' },
                { name: 'UtcDateClosed' },
                { name: 'UtcDateSolved' },
                { name: 'UtcDateModified' }
            ];
            self.tapeControl = new tapeLib.Tape(self.problem, self.objectClassID, '/api/problems/' + self.id, timelineConfig, put, self.$region.find('.tape__b').selector, self.$region.find('.tape__forms').selector, self.IsReadOnly, self.CanEdit, self.CanViewNotes, self.TabSize, self.ProblemUsersList);
            //
            //LINKS BLOCK
            self.linkList = new linkListLib.LinkList(self.problem, self.objectClassID, self.$region.find('.links__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //NEGOTIATION BLOCK
            self.negotiationList = new negotiationListLib.LinkList(self.problem, self.objectClassID, self.$region.find('.negotiations__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //WO REFERENCE BLOCK
            self.workOrderList = new workOrderReferenceListLib.LinkList(self.problem, self.objectClassID, self.$region.find('.woRef__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            self.workOrderList.OnWorkOrderExecutorControl.subscribe(editOnWorkOrderExecutorControl);
            //
            //CALL REFERENCE BLOCK
            self.callList = new callReferenceListLib.LinkList(self.problem, self.objectClassID, self.$region.find('.cRef__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            self.kbaRefList = new kbaRefListLib.KBAReferenceList(self.problem, self.objectClassID, self.$region.find('.solution-kb__b').selector, self.IsReadOnly, self.IsReadOnly);
            //

            //KO INIT BLOCK
            self.ajaxControl_load = new ajaxLib.control();
            self.Load = function (id) {
                $(document).unbind('objectInserted', self.onObjectInserted);
                $(document).unbind('objectUpdated', self.onObjectUpdated);
                $(document).unbind('objectDeleted', self.onObjectDeleted);
                $(document).unbind('local_objectInserted', self.onObjectInserted);
                $(document).unbind('local_objectUpdated', self.onObjectUpdated);
                $(document).unbind('local_objectDeleted', self.onObjectDeleted);
                self.InitRecalculationHeightTab();

                //
                var retD = $.Deferred();
                if (id) {
                    self.ajaxControl_load.Ajax(self.$region,
                        {
                            dataType: "json",
                            method: 'GET',
                            url: '/api/problems/' + id
                        },
                        function (newVal) {
                            var loadSuccessD = $.Deferred();
                            var processed = false;
                            //
                            if (newVal) {
                                var pInfo = newVal;
                                if (pInfo && pInfo.ID) {
                                    require(['models/SDForms/ProblemForm.Problem'], function (pLib) {
                                        self.problem(new pLib.Problem(self, pInfo));
                                        self.problem().getCalls();
                                        self.ProblemUsersList.removeAll();
                                        self.CalculateUsersList();
                                        //
                                        $(document).unbind('objectInserted', self.onObjectInserted).bind('objectInserted', self.onObjectInserted);
                                        $(document).unbind('objectUpdated', self.onObjectUpdated).bind('objectUpdated', self.onObjectUpdated);
                                        $(document).unbind('objectDeleted', self.onObjectDeleted).bind('objectDeleted', self.onObjectDeleted);
                                        $(document).unbind('local_objectInserted', self.onObjectInserted).bind('local_objectInserted', self.onObjectInserted);
                                        $(document).unbind('local_objectUpdated', self.onObjectUpdated).bind('local_objectUpdated', self.onObjectUpdated);
                                        $(document).unbind('local_objectDeleted', self.onObjectDeleted).bind('local_objectDeleted', self.onObjectDeleted);
                                        //
                                        processed = true;

                                        if (pInfo.FormValues) {
                                            self.DynamicOptionsServiceInit(pInfo.FormValues.FormID, newVal.FormValues.Values);
                                        } else {
                                            self.DynamicOptionsServiceInit(null, null);
                                        };

                                        self.InitializeUserFields();
                                        loadSuccessD.resolve(true);
                                    });
                                }
                                else loadSuccessD.resolve(false);
                            }
                            else loadSuccessD.resolve(false);
                            //
                            $.when(loadSuccessD).done(function (loadSuccess) {
                                retD.resolve(loadSuccess);
                                if (loadSuccess == false && processed == false) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[ProblemForm.js, Load]', 'error');
                                    });
                                }
                            });
                        });
                }
                else retD.resolve(false);
                //
                return retD.promise();
            };
            //
            self.Reload = function (id, readOnly) {
                if (id == null && self.id == null)
                    return;
                else if (id == null)
                    id = self.id;
                //
                if (readOnly === true || readOnly === false)
                    self.IsReadOnly(readOnly);
                //
                var currentTab = self.mode();
                self.mode(self.modes.nothing);
                self.mainTabLoaded(false);
                self.tapeControl.ClearData();
                self.negotiationList.ClearData();
                self.linkList.ClearData();
                self.solutionTabLoaded(false);
                self.workOrderList.ClearData();
                self.callList.ClearData();
                self.kbaRefList.ClearData();
                
                if (self.priorityControl != null)
                    self.priorityControl.IsLoaded(false);
                //
                showSpinner(self.$region[0]);
                //
                $.when(self.Load(id)).done(function (loadResult) {
                    if (loadResult == false && self.CloseForm != null) {
                        self.CloseForm();
                    }
                    else self.mode(currentTab);
                    //
                    hideSpinner(self.$region[0]);
                });
            };
            self.renderProblemComplete = function () {
                $isLoaded.resolve();
                self.SizeChanged();
            };
            //
            self.AfterRender = function () {
                self.SizeChanged();
            };
            //            
            self.IsSolutionContainerVisible = ko.observable(true);
            self.ToggleSolutionContainer = function () {
                self.IsSolutionContainerVisible(!self.IsSolutionContainerVisible());
            };
            self.IsCauseContainerVisible = ko.observable(true);
            self.ToggleCauseContainer = function () {
                self.IsCauseContainerVisible(!self.IsCauseContainerVisible());
            };
            self.IsDescriptionContainerVisible = ko.observable(true);
            self.ToggleDescriptionContainer = function () {
                self.IsDescriptionContainerVisible(!self.IsDescriptionContainerVisible());
            };
            //
            self.SizeChanged = function () {
                if (!self.problem())
                    return;//Critical - ko - with:problem!!!
                //

                const dialog = self.$region.closest('.ui-dialog');
                const parent = dialog.parent();
                let tabHeight = 0;

                const isModeDialogModal = !parent.length || parent.get(0).nodeName === "BODY";
                if (isModeDialogModal) {
                    tabHeight = self.$region.height();//form height
                    tabHeight -= self.$region.find('.b-requestDetail-menu').outerHeight(true);
                    tabHeight -= self.$region.find('.b-requestDetail__title-header').outerHeight(true);
                } else {
                    dialog.css('height', '100%');

                    const tabsContainer = self.$region.find('[data-form-tabs]');
                    if (tabsContainer.length) {
                        const domRect = tabsContainer.get(0).getBoundingClientRect();
                        const MARGIN_FROM_BODY = 10;
                        tabHeight = document.documentElement.clientHeight - domRect.top - MARGIN_FROM_BODY;
                    };
                };

                //
                var tabWidth = self.$region.width();//form width
                tabWidth -= self.$region.find('.b-requestDetail-right').outerWidth(true);
                //
                self.TabHeight(Math.max(0, tabHeight - 10) + 'px');
                self.TabWidth(Math.max(0, tabWidth - 5) + 'px');
            };

            self.InitRecalculationHeightTab = function () {
                let heightMenuCurrent = 0;
                $(document).on('resizeEntity', function () {
                    const menu = self.$region.find('[data-request-menu]');
                    const menuHeight = menu.height();
                    if (menuHeight != heightMenuCurrent) {
                        self.SizeChanged();
                    };
                    heightMenuCurrent = menuHeight;
                });
            };

            self.Unload = function () {
                $(document).unbind('objectInserted', self.onObjectInserted);
                $(document).unbind('objectUpdated', self.onObjectUpdated);
                $(document).unbind('objectDeleted', self.onObjectDeleted);
                $(document).unbind('local_objectInserted', self.onObjectInserted);
                $(document).unbind('local_objectUpdated', self.onObjectUpdated);
                $(document).unbind('local_objectDeleted', self.onObjectDeleted);
                $(document).unbind('resizeEntity');

                //
                if (self.attachmentsControl != null)
                    self.attachmentsControl.RemoveUploadedFiles();
                if (self.workflowControl() != null)
                    self.workflowControl().Unload();
            };
            //
            self.ajaxControl = new ajaxLib.control();

            self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.problem().ID();
                //
                if (objectClassID == 110 && currentID == parentObjectID) //OBJ_DOCUMENT
                    self.LoadAttachmentsControl();
                else if (objectClassID == 160 && currentID == parentObjectID) //OBJ_NEGOTIATION
                {
                    if (self.negotiationList.isLoaded())
                        self.negotiationList.imList.TryReloadByID(objectID);
                    else
                        self.Reload(currentID);
                }
                else if (objectClassID == 117 && currentID == parentObjectID) //OBJ_NOTIFICATION ~ SDNote
                {
                    if (self.tapeControl.isNoteListLoaded())
                        self.tapeControl.TryAddNoteByID(objectID);
                    else
                        self.Reload(currentID);
                }
                else if (objectClassID == 137 && currentID == parentObjectID) //OBJ_KBArticle
                {
                    if (self.kbaRefList.isLoaded())
                        self.kbaRefList.imList.ReloadAll();
                }
                else if (objectClassID == 119 && currentID) //OBJ_WORKORDER
                {
                    
                 
                    
                }
                else if (objectClassID == 701 && currentID) //OBJ_CALL
                {
                    self.Reload(currentID);
                }
            };
            self.onObjectUpdated = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.problem().ID();
                //
                if (objectClassID == 160 && currentID == parentObjectID) //OBJ_NEGOTIATION
                {
                    if (self.negotiationList.isLoaded())
                        self.negotiationList.imList.TryReloadByID(objectID);
                    else
                        self.Reload(currentID);
                }
                else if (objectClassID == 117 && currentID == parentObjectID) //OBJ_NOTIFICATION ~ SDNote
                {
                    if (self.tapeControl.isNoteListLoaded())
                        self.tapeControl.TryAddNoteByID(objectID);
                    else
                        self.Reload(currentID);
                }
                else if (objectClassID == 119 && currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase()) //OBJ_WORKORDER
                {
                    if (self.workOrderList.isLoaded())
                        self.workOrderList.imList.TryReloadByID(objectID);
                    else 
                        self.Reload(currentID);
                }
                else if (objectClassID == 701 && currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase()) //OBJ_CALL
                {
                    if (self.callList.isLoaded())
                        self.callList.imList.TryReloadByID(objectID);
                    else 
                        self.Reload(currentID);
                }
                else if (objectClassID == 702 && currentID == objectID && e.type != 'local_objectUpdated') //OBJ_PROBLEM
                    self.Reload(currentID);
            };
            self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.problem().ID();
                //
                if (objectClassID == 110 && currentID == parentObjectID) //OBJ_DOCUMENT
                    self.LoadAttachmentsControl();
                else if (objectClassID == 160 && currentID == parentObjectID) //OBJ_NEGOTIATION
                {
                    if (self.negotiationList.isLoaded())
                        self.negotiationList.imList.TryRemoveByID(objectID);
                    else
                        self.Reload(currentID);
                }
                else if (objectClassID == 137 && currentID == parentObjectID) //OBJ_KBArticle
                {
                    if (self.kbaRefList.isLoaded())
                        self.kbaRefList.imList.TryRemoveByID(objectID);
                }
                else if (objectClassID == 119) //OBJ_WORKORDER
                {
                    if (self.workOrderList.isLoaded())
                        self.workOrderList.imList.TryRemoveByID(objectID);
                    else if (currentID == parentObjectID)
                        self.Reload(currentID);
                }
                else if (objectClassID == 701) //OBJ_CALL
                {
                    if (self.callList.isLoaded())
                        self.callList.imList.TryRemoveByID(objectID);
                    else if (currentID == parentObjectID)
                        self.Reload(currentID);
                }
                else if (objectClassID == 702 && currentID == objectID) //OBJ_PROBLEM
                {
                    self.IsReadOnly(true);
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ObjectDeleted'), 'info');
                    });
                }
            };

            // Динамические параметры
            {
                self.DynamicOptionsService = new dynamicOptionsService.ViewModel(self.$region.attr('id'), {
                    typeForm: 'Completed',
                    currentTab: self.mode,
                    changeTabCb: function (invalidInput) {
                        const parent = invalidInput.closest('[data-prefix-mode]');
                        const tabPrefix = parent.attr('data-prefix-mode');
                        self.mode(tabPrefix);
                    },
                    updateCallBack: put,
                    isReadOnly: self.IsReadOnly
                });

                self.DynamicOptionsServiceInit = function (templateID, values) {
                    if (self.DynamicOptionsService.IsInit()) {
                        self.DynamicOptionsService.ResetData();
                    };
                    if (templateID) {
                        self.DynamicOptionsService.GetTemplateByID(templateID, values);
                    }
                };

                self.SetTab = function (template) {
                    self.mode(`${self.modes.parameterPrefix}${template.Tab.ID}`);
                };
            }
        }
    }
    return module;
});
