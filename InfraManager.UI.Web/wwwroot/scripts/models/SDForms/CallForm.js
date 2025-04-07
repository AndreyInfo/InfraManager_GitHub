define([
    'knockout',
    'jquery',
    'ajax',
    'models/SDForms/SDForm.LinkList',
    'models/SDForms/SDForm.ProblemReferenceList',
    'models/SDForms/SDForm.WOReferenceList',
    'models/SDForms/SDForm.NegotiationList',
    'models/SDForms/SDForm.Tape',
    'models/SDForms/SDForm.KBAReferenceList',
    'dynamicOptionsService',
    'groupOperation'
], function (
    ko,
    $,
    ajaxLib,
    linkListLib,
    problemReferenceListLib,
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
        ViewModel: function (isReadOnly, isClientMode, $region, id, user) {
            //PROPERTIES AND ENUMS BLOCK
            var self = this;
            var $isLoaded = $.Deferred();
            self.$region = $region;
            self.CloseForm = null; //set in fh
            self.ControlForm = null; //set in fh
            self.CurrentUserID = null; //set in fh            
            self.id = id;
            self.user = user;
            self.objectClassID = 701; // IMSystem.Global.OBJ_Call
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                tape: 'tape',
                workorders: 'workorders',
                problems: 'problems',
                negotiation: 'negotiation',
                links: 'links',
                parameterPrefix: 'parameter_',
                userFields: 'userFields'
            };
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
            self.mode = ko.observable(self.modes.nothing);
            self.mode.subscribe(function (newValue) {
                if (newValue == self.modes.nothing)
                    return;

                if (newValue == self.modes.main) {
                    if (!self.mainTabLoaded()) {
                        $.when(self.CreateGrade()).done(function () {
                            self.mainTabLoaded(true);
                            self.SizeChanged();
                        });
                    }
                    self.kbaRefList.CheckData();
                }
                else if (newValue == self.modes.tape)
                    self.tapeControl.CheckData();
                else if (newValue == self.modes.links)
                    self.linkList.CheckData();
                else if (newValue == self.modes.workorders)
                    self.workOrderList.CheckData();
                else if (newValue == self.modes.problems)
                    self.problemList.CheckData();
                else if (newValue == self.modes.userFields)
                    self.CheckUserFields();
                else if (newValue == self.modes.negotiation) {
                    if (!self.mainTabLoaded()) {
                        $.when(self.CreateGrade()).done(function () {
                            self.mainTabLoaded(true);
                            self.SizeChanged();
                        });
                    }
                    self.negotiationList.CheckData(self.negotiationID);
                }
            });
            //User Fields
            {
                self.UserFieldType = 2;
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
                    self.UserFields().SetFields(self.call());
                };
            }
            //

            self.ClickMain = function () {
                self.mode(self.modes.main)
            }
            //
            self.attachmentsControl = null;
            self.LoadAttachmentsControl = function () {
                if (!self.call())
                    return;
                //
                require(['fileControl'], function (fcLib) {
                    if (self.attachmentsControl != null) {
                        if (self.attachmentsControl.ObjectID != self.call().ID())//previous object  
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
                    self.attachmentsControl.AvailableAdd(self.CanAddFiles());
                    self.attachmentsControl.RemoveFileAvailable(self.CanDeleteFiles());
                    self.attachmentsControl.AvailableDownload(self.CanDownloadFiles())
                    self.attachmentsControl.Initialize(self.call().ID());
                });
            };
            //
            self.workflowControl = ko.observable(null);
            self.LoadWorkflowControl = function () {
                if (!self.call())
                    return;
                //
                require(['workflow'], function (wfLib) {
                    if (self.workflowControl() == null) {
                        self.workflowControl(new wfLib.control(self.$region, self.IsReadOnly, self.call));
                        self.workflowControl().CanOpenMenu = function () {
                            if (self.call() != null && self.call().ServiceID() == null && self.IsReadOnly() == false) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('PromptServiceItemAttendance'));
                                });
                                return false;
                            }
                            return true;
                        };
                    }
                    self.workflowControl().ReadOnly(self.IsReadOnly());
                    self.workflowControl().Initialize();
                });
            };
            //
            self.EditManhoursWork = function () {
                if (self.IsClientMode() == true)
                    return;
                //
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper();
                    fh.ShowManhoursWorkList(self.call, self.objectClassID, self.CanEdit);
                });
            };
            //
            //
            self.priorityControl = null;
            self.LoadPriorityControl = function () {
                if (!self.call())
                    return;
                //
                require(['models/SDForms/SDForm.Priority'], function (prLib) {
                    if (self.priorityControl == null || self.priorityControl.IsLoaded() == false) {
                        self.priorityControl = new prLib.ViewModel(self.$region.find('.b-requestDetail-menu__priority'), self, self.IsReadOnly());
                        self.priorityControl.Initialize();
                    }
                    $.when(self.priorityControl.Load(self.call().ID(), self.objectClassID, self.call().UrgencyID(), self.call().InfluenceID(), self.call().PriorityID())).done(function (result) {
                        //not needed now
                    });
                });
            };

            self.RefreshPriority = function (priorityObj) {
                if (priorityObj == null)
                    return;

                self.call().PriorityName(priorityObj.Name);
                self.call().PriorityColor(priorityObj.Color);
                self.call().PriorityID(priorityObj.ID);
                self.call().InfluenceID(self.priorityControl.CurrentInfluenceID());
                self.call().UrgencyID(self.priorityControl.CurrentUrgencyID());

                var model = {
                    InfluenceID: self.priorityControl.CurrentInfluenceID(),
                    UrgencyID: self.priorityControl.CurrentUrgencyID(),
                    PriorityID: priorityObj.ID
                };
                self.ajaxControl_SetCustomControl.Ajax(
                    null,
                    {
                        dataType: 'json',
                        contentType: 'application/json',
                        method: 'PATCH',
                        url: '/api/calls/' + self.call().ID(),
                        data: JSON.stringify(model)
                    },
                    function () {
                    });
            };

            self.ajaxControl_CustomControl = new ajaxLib.control();
            self.ajaxControl_CustomControlUsers = new ajaxLib.control();
            self.CustomControl = ko.observable(false);
            self.LoadCustomControl = function () {
                if (!self.call())
                    return;
                //
                var param = {
                    objectID: self.call().ID(),
                };
                self.ajaxControl_CustomControl.Ajax(self.$region.find('.b-requestDetail-menu__item-control'),
                    {
                        method: 'GET',
                        url: '/api/calls/' + param.objectID + '/customControls/my/'
                    },
                    function (details) {
                        self.CustomControl(details.UnderControl);
                    });
                //
                var param2 = {
                    objectID: self.call().ID(),
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
                                    var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
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
                                    self.CallUsersList.push(user);
                                });
                            });
                    });
            };
            self.SaveCustomControl = function () {
                if (!self.call())
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
                        url: '/api/calls/' + self.call().ID() + '/customControls/my'
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
            self.call = ko.observable(null);
            self.call.subscribe(function (newValue) {
                $.when($isLoaded).done(function () {
                    self.SizeChanged();//block of call completed
                    //
                    self.LoadAttachmentsControl();
                    self.LoadWorkflowControl();
                    //
                    if (self.IsClientMode() == false) {
                        self.LoadPriorityControl();
                    }
                    //
                    self.CheckIsHidePlaceOfServiceVisible();
                });
            });
            self.IsReadOnly = ko.observable(isReadOnly);
            self.IsReadOnly.subscribe(function (newValue) {
                var readOnly = self.IsReadOnly();
                //
                if (self.attachmentsControl != null)
                    self.attachmentsControl.RemoveFileAvailable(self.CanDeleteFiles());
                if (self.workflowControl() != null)
                    self.workflowControl().ReadOnly(readOnly);
                if (self.priorityControl != null)
                    self.priorityControl.ReadOnly(readOnly);
                if (self.linkList != null)
                    self.linkList.ReadOnly(readOnly);
                if (self.problemList != null)
                    self.problemList.ReadOnly(readOnly);
                if (self.negotiationList != null) {
                    //all inside control already
                }
                if (self.UserFields() != null) {
                    self.UserFields().ReadOnly(readOnly);
                }
            });
            self.IsClientMode = ko.observable(isClientMode);
            self.CanEdit = ko.computed(function () {
                return !(self.IsReadOnly() || self.IsClientMode());
            });
            self.CanAddFiles = ko.computed(function () {
                return (self.user.GrantedOperations.includes(180));
            });
            self.CanDeleteFiles = ko.computed(function () {
                return (self.user.GrantedOperations.includes(179));
            });
            self.CanDownloadFiles = ko.computed(function () {
                return (self.user.GrantedOperations.includes(176));
            });
            self.CanShow = ko.observable(self.CanEdit);
            //исключение из правил для контролов прикреплений и сообщений
            self.IsSemiReadOnly = ko.computed(function () {
                var call = self.call();
                if (call == null)
                    return true;//object not load
                //
                if (call.EntityStateID() == '')
                    return true;//terminal state
                //
                if (call.ClientID() == self.CurrentUserID ||
                    call.InitiatorID() == self.CurrentUserID ||
                    call.OwnerID == self.CurrentUserID ||
                    call.ExecutorID == self.CurrentUserID ||
                    self.CustomControl() == true ||
                    call.HaveNegotiationsWithCurrentUser())
                    return false;//я=(среди пользователей) или на контроле у меня или участник согласования
                //
                return self.IsReadOnly();
            });
            self.IsSemiReadOnly.subscribe(function (newValue) {
                if (self.attachmentsControl != null)
                    self.attachmentsControl.ReadOnly(newValue && self.IsReadOnly());
            });
            //
            self.additionalClick = function () {
                //TODO
            };
            self.CustomControlClick = function () {//поставить/снять с контроля
                if (self.CustomControl()) {
                    $.when(userD).done(function (user) {
                        if (user.HasRoles || self.call().ClientID() == user.UserID) {
                            self.CustomControl(!self.CustomControl());
                            self.SaveCustomControl();
                        }
                        else {
                            self.ajaxControl_setGrade.Ajax(self.$region,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    url: '/api/negotiations?objectClassID=' + self.objectClassID + '&objectID=' + self.call().ID() + '&userID=' + self.CurrentUserID
                                },
                                function (negotiations) {
                                    if (negotiations.length > 0) {
                                        require(['sweetAlert'], function () {
                                            swal({
                                                title: getTextResource('CallAccessWillBeLost'),
                                                text: self.Name,
                                                showCancelButton: true,
                                                closeOnConfirm: true,
                                                closeOnCancel: true,
                                                confirmButtonText: getTextResource('ButtonOK'),
                                                cancelButtonText: getTextResource('ButtonCancel')
                                            },
                                                function (value) {
                                                    if (value == true) {
                                                        self.CustomControl(!self.CustomControl());
                                                        self.SaveCustomControl();
                                                        self.CloseForm();
                                                    }
                                                });
                                        });
                                    }
                                    else {
                                        self.CustomControl(!self.CustomControl());
                                        self.SaveCustomControl();
                                    }
                                });
                        }
                    });
                }
                else {
                    self.CustomControl(!self.CustomControl());
                    self.SaveCustomControl();
                }
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

            function toggleCustomControl (selectedUsers, underControl) {
                showSpinner();

                var groupOperationViewModel =
                    new groupOperation.CustomControlViewModel(
                        [{ Uri: '/api/calls/' + self.id }],
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
            ///
            self.SendMail = function () {
                showSpinner();
                
                $.when(self.GetDefaultNotificationID()).done(function (notificationID) {
                    require(['sdForms'], function (module) {
                        var fh = new module.formHelper(true);
                        //
                        var options = {
                            Obj: {
                                ID: self.id,
                                ClassID: self.objectClassID,
                                ClientID: (self.call().ExecutorID() == self.CurrentUserID || self.call().OwnerID() == self.CurrentUserID) ? self.call().Client().ID() : '',
                                ClientEmail: (self.call().ExecutorID() == self.CurrentUserID || self.call().OwnerID() == self.CurrentUserID) ? self.call().Client().Email() : '',
                                NotificationID: notificationID,
                            },
                            CanNote: true
                        };
                        fh.ShowSendEmailForm(options);
                    });
                });
            };
            self.ViewMailButton = ko.computed(function () {
                return self.IsClientMode()!=true;
            });
            //
            self.GradeArray = ko.observableArray([]);
            self.CreateGrade = function () {
                var retD = $.Deferred();
                require(['models/SDForms/CallForm.Grade'], function (gradeLib) {
                    self.GradeArray.removeAll();
                    self.GradeArray(gradeLib.CreateClassicArray(self.call().Grade()));
                    //
                    self.GradeArray.valueHasMutated();
                    retD.resolve();
                });
                //
                return retD.promise();
            };
            self.CanPostGrade = ko.computed(function () {
                if (!self.IsClientMode() || self.IsSemiReadOnly())
                    return false;
                //
                if (!self.call() || (self.call().Grade() != null && self.call().Grade() != ''))
                    return false;
                //
                if (self.call().ClientID() != self.CurrentUserID &&
                    self.call().InitiatorID() != self.CurrentUserID)
                    return false;
                //
                return true;
            });
            self.ajaxControl_setGrade = new ajaxLib.control();
            self.PostGrade = function (obj) {
                var retD = $.Deferred();
                if (obj && obj.Name != null) {
                    newValue = obj.Name;
                    if (self.call().Grade() != newValue) {
                        $.when(putCall({ Grade: newValue })).done(function () { retD.resolve(true); });
                    }
                    else retD.resolve(true);
                }
                else retD.resolve(false);
                return retD.promise();
            };
            self.SetGrade = function (obj, region, skipCheck) {//есть вызов из форм хелпера, игнорирующий все запреты // DZ а если я "из POSTMAN-а", как вы меня заставите соблюдать ваши запреты???
                if (skipCheck !== true)
                    skipCheck = false;
                //
                if (!self.CanPostGrade() && !skipCheck)
                    return;
                //
                self.PostGrade(obj, false);
            };
            //
            self.EditRFCResult = function () {
                if (!self.CanEdit()) {
                    return;
                }

                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.RFCResult',
                        fieldFriendlyName: getTextResource('RFCResult'),
                        oldValue: self.call().RFCResultID() ? { ID: self.call().RFCResultID(), FullName: self.call().RFCResultName() } : null,
                        searcherName: 'RFCResultSearcher',
                        save: function (objectInfo) {
                            var newValue = JSON.parse(objectInfo.NewValue);
                            return putCall({ RequestForServiceResultID: newValue.id });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            self.EditIncidentResult = function () {
                if (!self.CanEdit()) {
                    return;
                }

                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.IncidentResult',
                        fieldFriendlyName: getTextResource('IncidentResult'),
                        oldValue: self.call().IncidentResultID() ? { ID: self.call().IncidentResultID(), FullName: self.call().IncidentResultName() } : null,
                        searcherName: 'IncidentResultSearcher',
                        save: function (objectInfo) {
                            var newValue = JSON.parse(objectInfo.NewValue);
                            return putCall({ IncidentResultID: newValue.id });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.ajaxControl_service = new ajaxLib.control();
            self.EditServiceItemAttendance = function () {
                if (!self.CanEdit()) {
                    return;
                }
                var getObjectInfo = function () {
                    var call = self.call();
                    var serviceFullName = call.ServiceID() ? [call.ServiceCategoryName(), call.ServiceName(), ''].join(' \\ ') : '';
                    if (call.ServiceItemID())
                        return {
                            ID: call.ServiceItemID(), ClassID: 406, FullName: serviceFullName + call.ServiceItemName()
                        };
                    else if (call.ServiceAttendanceID())
                        return {
                            ID: call.ServiceAttendanceID(), ClassID: 407, FullName: serviceFullName + call.ServiceAttendanceName()
                        };
                    else
                        return null;
                };
                var setService = function (serviceCategoryName, serviceID, serviceName, serviceItemID, serviceItemName, serviceAttendanceID, serviceAttendanceName) {
                    var call = self.call();
                    //
                    call.ServiceID(serviceID);
                    call.ServiceName(serviceName);
                    call.ServiceCategoryName(serviceCategoryName);
                    call.ServiceItemID(serviceItemID);
                    call.ServiceItemName(serviceItemName);
                    call.ServiceAttendanceID(serviceAttendanceID);
                    call.ServiceAttendanceName(serviceAttendanceName);
                    //
                    self.LoadWorkflowControl();
                    //
                    self.RefreshSLA(false);
                };
                var extensionLoadD = $.Deferred();
                //
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.ServiceItemAttendance',//одновременно оба поля
                        fieldFriendlyName: getTextResource('CallServiceItemOrAttendance'),
                        oldValue: getObjectInfo(),
                        allowNull: false,
                        searcherName: 'ServiceItemAndAttendanceSearcher',
                        searcherTemplateName: 'SearchControl/SearchServiceItemAttendanceControl',//иной шаблон, дополненительные кнопки
                        searcherParams: { CallTypeID: self.call().CallTypeID(), ClientID: self.call().ClientID() },//параметры искалки - тип заявки, для фильтрации - только элементы / только услуги
                        searcherLoadD: extensionLoadD,//ожидание дополнений для модели искалки
                        searcherLoad: function (vm, setObjectInfoFunc) {//дополнения искалки
                            vm.CurrentUserID = null;//для фильтрации данных по доступности их пользователю (клиенту)
                            vm.SelectFromServiceCatalogueClick = function () {//кнопка выбора из каталога сервисов
                                vm.Close();//close dropDownMenu
                                if (!self.call())
                                    return;
                                var mode = fh.ServiceCatalogueBrowserMode.Default;
                                if (self.call().CallTypeID() != '00000000-0000-0000-0000-000000000000') {//default callType
                                    if (self.call().IsRFCCallType() == true)
                                        mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceAttendances;
                                    else
                                        mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceItems;
                                }
                                if (self.call().CallTypeID() === '00000000-0000-0000-0000-000000000000') {//default callType
                                    mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceItems;
                                }

                                var clientID = self.call().ClientID();//показываем, что есть недоступное этому клиенту, но позволяем выбрать
                                showSpinner();
                                var result = fh.ShowServiceCatalogueBrowser(mode, clientID, self.call().ServiceID());
                                $.when(result).done(function (serviceItemAttendance) {
                                    if (serviceItemAttendance && setObjectInfoFunc)
                                        setObjectInfoFunc({
                                            ID: serviceItemAttendance.ID,
                                            ClassID: serviceItemAttendance.ClassID,
                                            FullName: serviceItemAttendance.FullName(),
                                            Details: null
                                        });
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
                        save: function (data) {
                            const classID = data.Params[0];
                            const serviceID = JSON.parse(data.NewValue).id;
                            const request = {};
                            request.ServiceItemAttendanceID = serviceID;
                            return putCall(request);
                        },
                        onSave: function (objectInfo) {
                            if (!objectInfo)
                                return;
                            self.ajaxControl_service.Ajax(self.$region.find('.fieldService'),
                                {
                                    url: '/api/serviceitems/' + objectInfo.ID,
                                    method: 'GET'
                                },
                                function (response) {
                                    if (response)
                                        setService(
                                            response.ServiceCategoryName, response.ServiceID, response.ServiceName,
                                            response.ServiceItemID, response.ServiceItemID ? response.Name : '',
                                            response.ServiceAttendanceID, response.ServiceAttendanceID ? response.Name : '');
                                });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //            
            self.EditReceiptType = function () {
                if (self.CanEdit() == false)
                    return;
                //
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.ReceiptType',
                        fieldFriendlyName: getTextResource('CallReceiptType'),
                        comboBoxGetValueUrl: '/sdApi/GetReceiptTypeList',
                        oldValue: {
                            ID: self.call().ReceiptType(), Name: self.call().ReceiptTypeName()
                        },
                        save: function (data) {
                            return putCall({ ReceiptType: JSON.parse(data.NewValue).id });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                });
            };
            self.ajaxControl_callType = new ajaxLib.control();
            self.EditCallType = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.CallType',
                        fieldFriendlyName: getTextResource('CallType'),
                        oldValue: {
                            ID: self.call().CallTypeID(), ClassID: 136, FullName: self.call().CallType()
                        },
                        searcherName: 'CallTypeSearcher',
                        save: function (data) {
                            return putCall({ CallTypeID: JSON.parse(data.NewValue).id });
                        },
                        allowNull: false
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //

            //
            //
            self.CallServicePlaceVisible = ko.observable(false);
            self.ajaxControl_ClientInfo = new ajaxLib.control();
            self.onLocationChanged = function (objectInfo) {//когда новое местоположение будет выбрано
                if (!objectInfo)
                    return;
                var options = {
                    FieldName: 'Call.ServiceLocation',
                    OldValue: self.call().ServicePlaceID() != null ? { ID: self.call().ServicePlaceID(), ClassID: self.call().ServicePlaceClassID()} : null,
                    newValue: { ID: objectInfo.IMObjID || objectInfo.ID, ClassID: objectInfo.ClassID },
                    onSave: function () {
                        self.ajaxControl_ClientInfo.Ajax(null,
                            {
                                dataType: "json",
                                url: '/bff/CallClientLocationInfo',
                                method: 'Get',
                                data: {
                                    LocationID: objectInfo.ID,
                                    LocationClassID: objectInfo.ClassID
                                }
                            },
                            function (response) {
                                if (response) {
                                    self.call().ServicePlaceID(response.PlaceID);
                                    self.call().ServicePlaceClassID(response.PlaceClassID);
                                    self.call().ServicePlaceName(response.PlaceName);
                                    self.call().ServicePlaceNameShort(response.PlaceName);
                                }
                            });
                    }
                };
                self.SetServiceLocation(false, options);
            };
            //
            self.CanEditLocation = ko.observable(self.CanEdit() == false? false : true);
            self.EditLocation = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                //
                require(['models/ClientInfo/frmClientEditLocation', 'sweetAlert'], function (module) {
                    //
                    let options = {
                        ServicePlaceID: self.call().ServicePlaceID(),
                        ServicePlaceClassID: self.call().ServicePlaceClassID(),
                        PlaceName: self.call().ServicePlaceName(),
                    };
                    var saveLocation = function (objectInfo) {
                        if (!objectInfo)
                            return;
                        self.onLocationChanged(objectInfo);
                    };
                    //
                    module.ShowDialog(options, saveLocation, true);
                });
            };
            //

            self.EditQueue = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.Queue',
                        fieldFriendlyName: getTextResource('Queue'),
                        oldValue: self.call().QueueLoaded() ? { ID: self.call().Queue().ID(), ClassID: self.call().Queue().ClassID(), FullName: self.call().Queue().FullName() } : null,
                        object: ko.toJS(self.call().Queue()),
                        searcherName: "QueueSearcher",
                        searcherPlaceholder: getTextResource('EnterQueue'),
                        searcherParams: { Type: 1 },//for call
                        onSave: function (objectInfo) {
                            self.call().QueueLoaded(false);
                            self.call().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                            //
                            if (objectInfo && objectInfo.ClassID === 722) { //IMSystem.Global.OBJ_QUEUE
                                self.call().QueueID(objectInfo.ID);
                                self.call().QueueName(objectInfo.FullName);
                            }
                            else {
                                self.call().QueueID('');
                                self.call().QueueName('');
                            }
                            self.InitializeQueue();
                        },
                        save: function (data) {
                            return putCall({ QueueID: data.NewValue ? JSON.parse(data.NewValue).id : null });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };

            self.EditOwner = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.Owner',
                        fieldFriendlyName: getTextResource('Owner'),
                        oldValue: self.call().OwnerLoaded() ? { ID: self.call().Owner().ID(), ClassID: 9, FullName: self.call().Owner().FullName() } : null,
                        object: ko.toJS(self.call().Owner()),
                        searcherName: 'OwnerUserSearcher',
                        searcherPlaceholder: getTextResource('EnterFIO'),
                        save: function (data) {
                            return putCall({ OwnerID: data.NewValue ? JSON.parse(data.NewValue).id : null });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };

            self.ajaxControl_getUser = new ajaxLib.control();
            self.EditClient = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.Client',
                        fieldFriendlyName: getTextResource('Client'),
                        oldValue: self.call().ClientLoaded() ? { ID: self.call().Client().ID(), ClassID: 9, FullName: self.call().Client().FullName() } : null,
                        object: ko.toJS(self.call().Client()),
                        searcherName: 'WebUserSearcher',
                        searcherPlaceholder: getTextResource('EnterFIO'),
                        save: function (data) {
                            let retD = $.Deferred();
                            let userId = JSON.parse(data.NewValue).id;
                            self.ajaxControl_getUser.Ajax(
                                null,
                                {
                                    dataType: 'json',
                                    contentType: 'application/json',
                                    method: 'GET',
                                    url: '/api/users/' + userId,
                                    data: JSON.stringify(data)
                                },
                                function (result) {
                                    if (result.OrganizationID == null) {
                                        swal(getTextResource('SC_NoUserSLA'));
                                        retD.resolve(false);
                                        return;
                                    }
                                    let updatedClientD = putCall({ ClientID: userId });
                                    $.when(updatedClientD).done(function (loadSuccess) {
                                        retD.resolve(true);
                                    });
                                },
                                function () {
                                    retD.resolve(false);
                                }
                            );
                            return retD;
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };

            self.SaveExecutorOrQueue = function (obj) {
                if (!obj || self.CanEdit() === false) {
                    return false;
                }
                if (obj.ClassID === module.Classes.Queue) {
                    return putCall({ QueueID: obj.ID });
                } else if (obj.ClassID === module.Classes.User) {
                    return putCall({ ExecutorID: obj.ID });
                }
                return false;
            }

            self.EditExecutor = function () {
                if (self.CanEdit() === false) {
                    return;
                }

                require(['usualForms'], function (module) {
                    let fh = new module.formHelper(true);
                    fh.ShowExecutorSelector(self);
                });
            };

            self.EditAccomplisher = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.Accomplisher',
                        fieldFriendlyName: getTextResource('Accomplisher'),
                        oldValue: self.call().AccomplisherLoaded() ? { ID: self.call().Accomplisher().ID(), ClassID: 9, FullName: self.call().Accomplisher().FullName() } : null,
                        object: ko.toJS(self.call().Accomplisher()),
                        searcherName: 'AccomplisherUserSearcher',
                        searcherPlaceholder: getTextResource('EnterFIO'),
                        save: function (data) {
                            return putCall({ AccomplisherID: data.NewValue ? JSON.parse(data.NewValue).id : null });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };

            function editOnWorkOrderExecutorControl(newVal) {
                const oldVal = self.call().OnWorkOrderExecutorControl();
                if (oldVal !== newVal) {
                    putCall({ OnWorkOrderExecutorControl: newVal });
                }
            }

            self.ajaxControl_SetCustomControl = new ajaxLib.control();
            function putCall(model) {
                showSpinner();
                var retD = $.Deferred();
                self.ajaxControl_SetCustomControl.Ajax(
                    null,
                    {
                        dataType: "json",
                        contentType: "application/json",
                        method: "PUT",
                        url: '/api/calls/' + id,
                        data: JSON.stringify(model)
                    },
                    function (call) {
                        if (call != undefined && call != null) {
                            self.call().Description(call.Description);
                            self.call().Solution(call.Solution);
                            self.call().CallSummaryName(call.CallSummaryName || '');
                            self.call().UtcDatePromised(parseDate(call.UtcDatePromised));
                            self.call().UtcDatePromisedDT(new Date(parseInt(call.UtcDatePromised)));
                            self.call().ManhoursNorm(call.ManhoursNormInMinutes);
                            self.call().ReceiptType(call.ReceiptType);
                            self.call().ReceiptTypeName(call.ReceiptTypeName);


                            if (self.call().OwnerID() != call.OwnerID) {
                                require(['models/SDForms/SDForm.User'], function (userLib) {
                                    self.call().OwnerLoaded(false);
                                    self.call().Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                                    //
                                    self.call().OwnerID(call.OwnerID || "");
                                    self.InitializeOwner();
                                });
                            }

                            if (self.call().QueueID() != call.QueueID) {
                                require(['models/SDForms/SDForm.User'], function (userLib) {
                                    self.call().QueueLoaded(false);
                                    self.call().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                                    self.call().QueueID(call.QueueID);
                                    self.call().QueueName(call.QueueName);
                                    self.InitializeQueue();
                                });
                            }

                            if (self.call().ClientID() != call.ClientID) {
                                require(['models/SDForms/SDForm.User'], function (userLib) {
                                    self.call().ClientLoaded(false);
                                    self.call().Client(new userLib.EmptyUser(self, userLib.UserTypes.client, self.EditClient));
                                    //
                                    self.call().ClientID(call.ClientID);
                                    self.InitializeClient();
                                });
                            }

                            if (self.call().ExecutorID() != call.ExecutorID) {
                                require(['models/SDForms/SDForm.User'], function (userLib) {
                                    self.call().ExecutorLoaded(false);
                                    self.call().Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                                    //
                                    self.call().ExecutorID(call.ExecutorID || "");
                                    self.InitializeExecutor();
                                });
                            }

                            if (self.call().AccomplisherID() != call.AccomplisherID) {
                                require(['models/SDForms/SDForm.User'], function (userLib) {
                                    self.call().AccomplisherLoaded(false);
                                    self.call().Accomplisher(new userLib.EmptyUser(self, userLib.UserTypes.accomplisher, self.EditAccomplisher));
                                    //
                                    self.call().AccomplisherID(call.AccomplisherID || "");
                                    self.InitializeAccomplisher();
                                });
                            }

                            var serviceItemChanged = (self.call().ServiceItemID() || '') != (call.ServiceItemID || '');
                            var serviceAttendanceChanged = (self.call().ServiceAttendanceID() || '') != (call.ServiceAttendanceID || '');
                            self.call().ServiceID(call.ServiceID);
                            self.call().ServiceName(call.ServiceName);
                            self.call().ServiceCategoryName(call.ServiceCategoryName);
                            self.call().ServiceItemID(call.ServiceItemID);
                            self.call().ServiceItemName(call.ServiceItemName);
                            self.call().ServiceAttendanceID(call.ServiceAttendanceID);
                            self.call().ServiceAttendanceName(call.ServiceAttendanceName);

                            var callTypeChanged = (self.call().CallTypeID() || '') !== (call.CallTypeID || '');
                            self.call().CallTypeID(call.CallTypeID);
                            self.call().IsRFCCallType(call.IsRFCCallType);
                            self.call().IsIncidentResultCallType(call.IsIncidentResultCallType);
                            self.call().IncidentResultID(call.IncidentResultID);
                            self.call().RFCResultID(call.RFCResultID);
                            self.call().CallType(call.CallType);

                            if (callTypeChanged || serviceItemChanged || serviceAttendanceChanged) {
                                self.LoadWorkflowControl();
                            }
                            //
                            if (self.tapeControl && self.tapeControl.TimeLineControl && self.tapeControl.isTimeLineLoaded && self.tapeControl.isTimeLineLoaded()) {
                                var mainTLC = self.tapeControl.TimeLineControl();
                                if (mainTLC != null && mainTLC.TimeLine) {
                                    var currentTL = mainTLC.TimeLine();
                                    if (currentTL != null && currentTL.UtcDatePromised) {
                                        currentTL.UtcDatePromised.LocalDate(self.call().UtcDatePromised());
                                        currentTL.UtcDatePromised.DateObj(self.call().UtcDatePromisedDT());
                                    }
                                }
                            }
                        }
                        self.DynamicOptionsService.ResetData();
                        hideSpinner();
                        retD.resolve(true);
                    }, function () { retD.resolve(false); });
                return retD;
            }

            self.EditDescription = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.Description',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: self.call().Description(),
                        readOnly: !self.CanEdit(),
                        save: function (data) {
                            return putCall({
                                Description: JSON.parse(data.NewValue).text
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                });
            };

            self.EditCallSummaryName = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.CallSummary',
                        fieldFriendlyName: getTextResource('CallSummary'),
                        oldValue: self.call().CallSummaryName(),
                        searcherName: 'CallSummarySearcher',
                        searcherPlaceholder: getTextResource('PromptCallSummary'),
                        searcherParams: [
                            self.call().ServiceItemID() != null ? self.call().ServiceItemID() : (self.call().ServiceAttendanceID() != null ? self.call().ServiceAttendanceID() : undefined),
                            self.call().CallTypeID().length > 0 ? self.call().CallTypeID() : undefined],
                        allowAnyText: true,
                        save: function (data) {
                            return putCall({ CallSummaryName: JSON.parse(data.NewValue).text });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            self.EditSolution = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.Solution',
                        fieldFriendlyName: getTextResource('Solution'),
                        oldValue: self.call().Solution(),
                        readOnly: !self.CanEdit(),
                        save: function (data) {
                            return putCall({ Solution: JSON.parse(data.NewValue).text });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                });
            };
            self.EditDatePromised = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.DatePromised',
                        fieldFriendlyName: getTextResource('CallDatePromise'),
                        oldValue: self.call().UtcDatePromisedDT(),
                        save: function (data) {
                            return putCall({ UtcDatePromisedMilliseconds: JSON.parse(data.NewValue).text });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                });
            };
            self.EditManhoursNorm = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.call().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'Call.ManhoursNorm',
                        fieldFriendlyName: getTextResource('ManhoursNorm'),
                        oldValue: self.call().ManhoursNorm(),
                        save: function (data) {
                            return putCall({ ManhoursNormInMinutes: JSON.parse(data.NewValue).val });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.timeEdit, options);
                });
            };
            //
            self.ajaxControl_SetServiceLocation = new ajaxLib.control();
            self.SetServiceLocation = function (isReplaceAnyway, options) {
                options.OldValue == null ? null : JSON.stringify({ 'ServicePlaceID': options.OldValue.ID, 'ServicePlaceClassID': options.OldValue.ClassID });
                options.newValue == null ? null : JSON.stringify({ 'ServicePlaceID': options.newValue.ID, 'ServicePlaceClassID': options.newValue.ClassID });
                let data = { 'ServicePlaceID': options.newValue.ID, 'ServicePlaceClassID': options.newValue.ClassID };
                self.ajaxControl_SetServiceLocation.Ajax(
                    self.$region,
                    {
                        dataType: 'json',
                        contentType: 'application/json',
                        method: 'PATCH',
                        url: '/api/calls/' + self.call().ID(),
                        data: JSON.stringify(data),
                    }, (retModel) => { }
                );
            };
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
                        url: '/api/calls/' + self.call().ID(),
                        data: JSON.stringify(data)
                    },
                    function (result, status) {
                        if (status !== 'success') {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[CallForm.js DeleteUser]', 'error');
                            });
                        }
                    });
            };
            self.DeleteOwner = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'OwnerID',
                        onSave: function () {
                            self.call().OwnerLoaded(false);
                            self.call().Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                            //
                            self.call().OwnerID('');
                        }
                    };
                    self.DeleteUser(options);
                });
            };
            self.DeleteAccomplisher = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'AccomplisherID',
                        onSave: function () {
                            self.call().AccomplisherLoaded(false);
                            self.call().Accomplisher(new userLib.EmptyUser(self, userLib.UserTypes.accomplisher, self.EditAccomplisher));
                            //
                            self.call().AccomplisherID('');
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
                            self.call().QueueLoaded(false);
                            self.call().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                            //
                            self.call().QueueID('');
                            self.call().QueueName('');
                        }
                    };
                    self.DeleteUser(options);
                });
            };
            self.DeleteExecutor = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'ExecutorID',
                        onSave: function () {
                            self.call().ExecutorLoaded(false);
                            self.call().Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                            //
                            self.call().ExecutorID('');
                        }
                    };
                    self.DeleteUser(options);
                });
            };
            //
            self.InitializeInitiator = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var c = self.call();
                    if (c.InitiatorLoaded() == false && c.InitiatorID()) {
                        var options = {
                            UserID: c.InitiatorID(),
                            UserType: userLib.UserTypes.initiator,
                            UserName: null,
                            EditAction: null,
                            RemoveAction: null,
                            CanNote: true
                        };
                        var user = new userLib.User(self, options);
                        c.Initiator(user);
                        c.InitiatorLoaded(true);
                        //
                        var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
                            return item.ID() == c.InitiatorID();
                        });
                        //                    
                        if (already == null)
                            self.CallUsersList.push(user);
                        else if (already.Type == userLib.UserTypes.withoutType) {
                            self.CallUsersList.remove(already);
                            self.CallUsersList.push(user);
                        }
                    }
                });
            };
            self.removeClientMass = function () {
                require(['sweetAlert'], function () {
                    swal(getTextResource('PromptClient'));
                });
            };
            self.InitializeClient = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var c = self.call();
                    if (c.ClientLoaded() == false && c.ClientID()) {
                        var options = {
                            UserID: c.ClientID(),
                            UserType: userLib.UserTypes.client,
                            UserName: null,
                            EditAction: self.EditClient,
                            RemoveAction: self.removeClientMass,
                            CanNote: true
                        };

                        var user = new userLib.User(self, options);
                        c.Client(user);
                        c.ClientLoaded(true);

                        user.$isLoadedUser.done(function () {
                            self.DynamicOptionsServiceInit();
                        });

                        //
                        var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
                            return item.ID() == c.ClientID();
                        });
                        //                    
                        if (already == null)
                            self.CallUsersList.push(user);
                        else if (already.Type == userLib.UserTypes.withoutType) {
                            self.CallUsersList.remove(already);
                            self.CallUsersList.push(user);
                        }
                        self.CallServicePlaceVisible(true);
                    }
                });
            };
            self.InitializeOwner = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var c = self.call();
                    if (c.OwnerLoaded() == false && c.OwnerID()) {
                        var options = {
                            UserID: c.OwnerID(),
                            UserType: userLib.UserTypes.owner,
                            UserName: null,
                            EditAction: self.EditOwner,
                            RemoveAction: self.DeleteOwner,
                            CanNote: true
                        };
                        var user = new userLib.User(self, options);
                        c.Owner(user);
                        c.OwnerLoaded(true);
                        //
                        var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
                            return item.ID() == c.OwnerID();
                        });
                        //
                        if (already == null)
                            self.CallUsersList.push(user);
                        else if (already.Type == userLib.UserTypes.withoutType) {
                            self.CallUsersList.remove(already);
                            self.CallUsersList.push(user);
                        }
                    }
                });
            };
            self.InitializeQueue = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var c = self.call();
                    //
                    if (c.QueueLoaded() == false) {
                        if (c.QueueID()) {
                            var options = {
                                UserID: c.QueueID(),
                                UserType: userLib.UserTypes.queueExecutor,
                                UserName: null,
                                EditAction: self.EditQueue,
                                RemoveAction: self.DeleteQueue,
                                CanNote: true,
                                IsFreezeSelectedClient: true
                            };
                            var user = new userLib.User(self, options);
                            c.Queue(user);
                            c.QueueLoaded(true);
                            //
                            var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
                                return item.ID() == c.QueueID();
                            });
                            //
                            if (already == null)
                                self.CallUsersList.push(user);
                            else if (already.Type == userLib.UserTypes.withoutType) {
                                self.CallUsersList.remove(already);
                                self.CallUsersList.push(user);
                            }
                        }
                    }
                });
            };
            self.InitializeExecutor = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var c = self.call();
                    //
                    if (c.ExecutorLoaded() == false) {
                        if (c.ExecutorID()) {
                            var options = {
                                UserID: c.ExecutorID(),
                                UserType: userLib.UserTypes.executor,
                                UserName: null,
                                EditAction: self.EditExecutor,
                                RemoveAction: self.DeleteExecutor
                            };
                            var user = new userLib.User(self, options);
                            c.Executor(user);
                            c.ExecutorLoaded(true);
                            //
                            var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
                                return item.ID() == c.ExecutorID();
                            });
                            //
                            if (already == null)
                                self.CallUsersList.push(user);
                            else if (already.Type == userLib.UserTypes.withoutType) {
                                self.CallUsersList.remove(already);
                                self.CallUsersList.push(user);
                            }
                        }
                    }
                });
            };
            self.InitializeAccomplisher = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var c = self.call();
                    if (c.AccomplisherLoaded() == false && c.AccomplisherID()) {
                        var options = {
                            UserID: c.AccomplisherID(),
                            UserType: userLib.UserTypes.accomplisher,
                            UserName: null,
                            EditAction: self.EditAccomplisher,
                            RemoveAction: self.DeleteAccomplisher
                        };
                        var user = new userLib.User(self, options);
                        c.Accomplisher(user);
                        c.AccomplisherLoaded(true);
                        //
                        var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
                            return item.ID() == c.AccomplisherID();
                        });
                        //
                        if (already == null)
                            self.CallUsersList.push(user);
                        else if (already.Type == userLib.UserTypes.withoutType) {
                            self.CallUsersList.remove(already);
                            self.CallUsersList.push(user);
                        }
                    }
                });
            };
            self.CalculateUsersList = function () {
                if (!self.call()) {
                    self.CallUsersList([]);
                    self.CallUsersList.valueHasMutated();
                    return;
                }
                //
                self.InitializeInitiator();
                //
                self.InitializeClient();
                //
                self.InitializeOwner();
                //      
                if (self.IsClientMode() == false) {
                    self.InitializeQueue();
                }
                self.InitializeExecutor();
                //
                self.InitializeAccomplisher();
                //
                self.CallUsersList.valueHasMutated();
                //add currentUser to list
                $.when(userD).done(function (userObj) {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var already = ko.utils.arrayFirst(self.CallUsersList(), function (item) {
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
                            CanNote: true
                        };
                        var user = new userLib.User(self, options);
                        //
                        self.CallUsersList.push(user);
                    });
                });
                //
                self.LoadCustomControl();
            };
            //
            self.CallUsersList = ko.observableArray([]);
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
                if (!retval && self.call) {
                    var c = self.call();
                    if (c && c.UtcDatePromised)
                        retval = c.UtcDatePromised();
                }
                //
                return retval;
            });
            self.ajaxControl_sla = new ajaxLib.control();
            self.RefreshSLAClick = function (customerChoice) {
                self.RefreshSLA(true);
            };
            self.RefreshSLA = function (customerChoice) {
                if (!self.call() || !self.call().ID())
                    return;
                var id = self.call().ID();
                putCall({ RefreshAgreement: {} });
                //
            };
            //
            self.DateRegisteredCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                var retval = '';
                //
                if (self.tapeControl && self.tapeControl.TimeLineControl && self.tapeControl.isTimeLineLoaded && self.tapeControl.isTimeLineLoaded()) {
                    var mainTLC = self.tapeControl.TimeLineControl();
                    if (mainTLC != null && mainTLC.TimeLine) {
                        var currentTL = mainTLC.TimeLine();
                        if (currentTL != null && currentTL.UtcDateRegistered)
                            retval = currentTL.UtcDateRegistered.LocalDate();
                    }
                }
                //
                if (!retval && self.call) {
                    var c = self.call();
                    if (c && c.UtcDateRegistered)
                        retval = c.UtcDateRegistered();
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
                if (!retval && self.call) {
                    var c = self.call();
                    if (c && c.UtcDateModified)
                        retval = c.UtcDateModified();
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
                            fh.ShowHelpSolutionPanel(self.ControlForm, self, self.call);
                    });
                    //
                    return;
                }
                //
                var current = self.LeftPanelModel.IsVisible();
                self.LeftPanelModel.IsVisible(!current);
            };
            self.AddTextToSolution = function (newText) {
                if (!self.call() || !newText)
                    return;
                //
                var currentValue = self.call().Solution();
                if (!currentValue)
                    currentValue = '';
                var newValue = currentValue + ' \n ' + newText;
                //
                putCall({Solution: newValue});
            };
            self.ajaxControl_updateTextField = new ajaxLib.control();
            self.UpdateTextField = function (isReplaceAnyway, options) {
                var data = {
                    ID: self.call().ID(),
                    ObjClassID: self.objectClassID,
                    Field: options.FieldName,
                    OldValue: options.OldValue == null ? null : JSON.stringify({ 'text': options.OldValue }),
                    NewValue: options.NewValue == null ? null : JSON.stringify({ 'text': options.NewValue }),
                    Params: null,
                    ReplaceAnyway: isReplaceAnyway
                };
                //
                self.ajaxControl_updateTextField.Ajax(
                    self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        url: '/sdApi/SetField',
                        data: data
                    },
                    function (retModel) {
                        if (retModel) {
                            var result = retModel.ResultWithMessage.Result;
                            //
                            if (result === 0) {
                                if (options.onSave != null)
                                    options.onSave(null);
                            }
                            else if (result === 1) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[CallForm.js UpdateTextField]', 'error');
                                });
                            }
                            else if (result === 2) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[CallForm.js UpdateTextField]', 'error');
                                });
                            }
                            else if (result === 3) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                                });
                            }
                            else if (result === 5 && isReplaceAnyway == false) {
                                require(['sweetAlert'], function () {
                                    swal({
                                        title: getTextResource('SaveError'),
                                        text: getTextResource('ConcurrencyError'),
                                        showCancelButton: true,
                                        closeOnConfirm: true,
                                        closeOnCancel: true,
                                        confirmButtonText: getTextResource('ButtonOK'),
                                        cancelButtonText: getTextResource('ButtonCancel')
                                    },
                                        function (value) {
                                            if (value == true) {
                                                self.UpdateTextField(true, options);
                                            }
                                        });
                                });
                            }
                            else if (result === 6) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('ObjectDeleted'), 'error');
                                });
                            }
                            else if (result === 7) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                                });
                            }
                            else if (result === 8) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('ValidationError'), 'error');
                                });
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[CallForm.js UpdateTextField]', 'error');
                                });
                            }
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[CallForm.js UpdateTextField]', 'error');
                            });
                        }
                    });
            };
            //
            //TAPE BLOCK
            self.TapeClick = function () {
                self.mode(self.modes.tape);
                self.SizeChanged();
                //
                if (self.tapeControl && self.tapeControl.CalculateTopPosition)
                    self.tapeControl.CalculateTopPosition('tapeclick');
            };
            self.CanViewNotes = ko.computed(function () {
                if (self.IsClientMode() == true)
                    return false;
                //
                if (self.IsReadOnly() == true && self.IsSemiReadOnly() == false)
                    return false;
                //
                return true;
            });

            const timelineConfig = [
                { name:'UtcDateModified'},
                { name:'UtcDateRegistered'},
                { name:'UtcDateOpened'},
                { name:'UtcDatePromised'},
                { name:'UtcDateAccomplished'},
                { name:'UtcDateClosed'}
            ];

            const timeLineParameters = { Parameters: [ 'Владелец', 'Исполнитель', 'Группа' ] };
            self.tapeControl = new tapeLib.Tape(self.call, self.objectClassID, `/api/calls/${self.id}`, timelineConfig, putCall, self.$region.find('.tape__b').selector, self.$region.find('.tape__forms').selector, self.IsSemiReadOnly, self.CanEdit, self.CanViewNotes, self.TabSize, self.CallUsersList, timeLineParameters);
            //
            //LINKS BLOCK
            self.linkList = new linkListLib.LinkList(self.call, self.objectClassID, self.$region.find('.links__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //WO REFERENCE BLOCK
            self.workOrderList = new workOrderReferenceListLib.LinkList(self.call, self.objectClassID, self.$region.find('.woRef__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            self.workOrderList.OnWorkOrderExecutorControl.subscribe(editOnWorkOrderExecutorControl);
            //
            //PROBLEM REFERENCE BLOCK
            self.problemList = new problemReferenceListLib.LinkList(self.call, self.objectClassID, self.$region.find('.pbRef__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //NEGOTIATION BLOCK
            self.negotiationList = new negotiationListLib.LinkList(self.call, self.objectClassID, self.$region.find('.negotiations__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            self.kbaRefList = new kbaRefListLib.KBAReferenceList(self.call, self.objectClassID, self.$region.find('.solution-kb__b').selector, self.IsReadOnly, self.IsClientMode);
            //
            //
            self.ShowResultAndKB = ko.computed(function () {
                if (!self.IsClientMode)
                    return false;
                //
                return !self.IsClientMode() || (self.CanEdit() || self.kbaRefList.imList.List().length > 0);
            });
            //
            self.ShowPlaceOfService = ko.observable(true);
            self.CheckIsHidePlaceOfServiceVisible = function () {
                var ajaxControl = new ajaxLib.control();
                ajaxControl.Ajax(null,
                    {
                        url: '/configApi/IsHidePlaceOfServiceVisible',
                        method: 'GET'
                    },
                    function (result) {
                        self.ShowPlaceOfService(!result);
                    });
            };
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
                            url: '/api/calls/' + id
                        },
                        function (newVal) {
                            var loadSuccessD = $.Deferred();
                            var processed = false;
                            //
                            if (newVal) {
                                var callInfo = newVal;
                                if (callInfo && callInfo.ID) {
                                    require(['models/SDForms/CallForm.Call'], function (callLib) {
                                        self.call(new callLib.Call(self, callInfo));
                                        self.InitializeUserFields();
                                        self.CallUsersList.removeAll();
                                        //
                                        $(document).unbind('objectInserted', self.onObjectInserted).bind('objectInserted', self.onObjectInserted);
                                        $(document).unbind('objectUpdated', self.onObjectUpdated).bind('objectUpdated', self.onObjectUpdated);
                                        $(document).unbind('objectDeleted', self.onObjectDeleted).bind('objectDeleted', self.onObjectDeleted);
                                        $(document).unbind('local_objectInserted', self.onObjectInserted).bind('local_objectInserted', self.onObjectInserted);
                                        $(document).unbind('local_objectUpdated', self.onObjectUpdated).bind('local_objectUpdated', self.onObjectUpdated);
                                        $(document).unbind('local_objectDeleted', self.onObjectDeleted).bind('local_objectDeleted', self.onObjectDeleted);
                                        //

                                        processed = true;

                                        if (newVal.FormValues) {
                                            self.FormID(newVal.FormValues.FormID);
                                            self.FormValues(newVal.FormValues.Values);
                                        } else {
                                            self.FormID(null);
                                        };

                                        self.CalculateUsersList();

                                        if (!self.negotiationList.isLoaded()) {
                                            self.negotiationList.CheckData(self.negotiationID);
                                        }

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
                                        swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[CallForm.js, Load]', 'error');
                                    });
                                }
                            });
                        },
                        function (failResp) {
                            var loadSuccessD = $.Deferred();
                            var processed = false;
                            if (failResp.status === 403) {//AccessError
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                });
                                processed = true;
                            }
                            loadSuccessD.resolve(false);
                            //
                            $.when(loadSuccessD).done(function (loadSuccess) {
                                retD.resolve(loadSuccess);
                                if (loadSuccess == false && processed == false) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[CallForm.js, Load]', 'error');
                                    });
                                }
                            });
                        }
                    );
                }
                else retD.resolve(false);
                //
                return retD.promise();
            };
            self.Reload = function (id, clientMode, readOnly) {
                if (id == null && self.id == null)
                    return;
                else if (id == null)
                    id = self.id;
                //
                if (readOnly === true || readOnly === false)
                    self.IsReadOnly(readOnly);
                if (clientMode === true || clientMode === false)
                    self.IsClientMode(clientMode);
                //
                var currentTab = self.mode();
                self.mode(self.modes.nothing);
                self.mainTabLoaded(false);
                self.tapeControl.ClearData();
                self.negotiationList.ClearData();
                self.kbaRefList.ClearData();
                self.linkList.ClearData();
                self.workOrderList.ClearData();
                self.problemList.ClearData();
                if (self.priorityControl != null)
                    self.priorityControl.IsLoaded(false);
                //
                showSpinner(self.$region[0]);
                //
                $.when(self.Load(id)).done(function (loadResult) {
                    if (loadResult == false && self.CloseForm != null) {
                        self.CloseForm();
                    }
                    else if (currentTab != self.modes.nothing)
                        self.mode(currentTab);
                    else
                        self.mode(self.modes.main);
                    //
                    hideSpinner(self.$region[0]);
                });
            };
            self.renderCallComplete = function () {
                $isLoaded.resolve();
                self.SizeChanged();
            };
            self.AfterRender = function () {
                self.SizeChanged();
            };

            //            
            self.IsSolutionContainerVisible = ko.observable(true);
            self.ToggleSolutionContainer = function () {
                self.IsSolutionContainerVisible(!self.IsSolutionContainerVisible());
            };
            self.IsSlaContainerVisible = ko.observable(false);
            self.ToggleSlaContainer = function () {
                self.IsSlaContainerVisible(!self.IsSlaContainerVisible());
            };
            self.IsDescriptionContainerVisible = ko.observable(true);
            self.ToggleDescriptionContainer = function () {
                self.IsDescriptionContainerVisible(!self.IsDescriptionContainerVisible());
            };
            //
            self.ShowPlaceOfService = ko.observable(true);

            //
            self.SizeChanged = function () {
                if (!self.call())
                    return;//Critical - ko - with:call!!!
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
                        const MARGIN_FROM_BODY = 20;
                        tabHeight = document.documentElement.clientHeight - domRect.top - MARGIN_FROM_BODY;
                    };
                };

                //
                var tabWidth = self.$region.width();//form width
                tabWidth -= self.$region.find('.b-requestDetail-right').outerWidth(true);
                //
                self.TabHeight(tabHeight + 'px');
                self.TabWidth(tabWidth + 'px');
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
            //
            self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.call().ID();
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
                else if (objectClassID == 119 && currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase()) //OBJ_WORKORDER
                {
                    if (self.workOrderList.isLoaded())
                        self.workOrderList.imList.TryReloadByID(objectID);
                    else
                        self.Reload(currentID);
                }
                else if (objectClassID == 702) //OBJ_PROBLEM
                {
                    if (self.problemList.isLoaded())
                        self.problemList.imList.TryReloadByID(objectID);
                    else (currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase())
                    self.Reload(currentID);
                }
            };
            self.onObjectUpdated = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.call().ID();
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
                else if (objectClassID == 702 && currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase()) //OBJ_PROBLEM
                {
                    if (self.problemList.isLoaded())
                        self.problemList.imList.TryReloadByID(objectID);
                    else
                        self.Reload(currentID);
                }
                else if (objectClassID == 701 && currentID == objectID && e.type != 'local_objectUpdated') //OBJ_CALL
                    self.Reload(currentID);
            };
            self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.call().ID();
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
                else if (objectClassID == 702) //OBJ_PROBLEM
                {
                    if (self.problemList.isLoaded())
                        self.problemList.imList.TryRemoveByID(objectID);
                    else if (currentID == parentObjectID)
                        self.Reload(currentID);
                }
                else if (objectClassID == 701 && currentID == objectID) //OBJ_CALL
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
                    updateCallBack: putCall,
                    isReadOnly: self.IsReadOnly,
                    isClientMode: isClientMode
                });

                self.DynamicOptionsServiceInit = function () {
                    const formID = self.FormID();
                    const selectUser = self.call().Client();
                    const isSelectUserNotEmpty = selectUser && selectUser.hasOwnProperty('ID');
                    const serviceItemID = self.call().ServiceItemID();
                    const serviceAttendanceID = self.call().ServiceAttendanceID();

                    if (!serviceItemID && !serviceAttendanceID) {
                        return;
                    };

                    if (self.DynamicOptionsService.IsInit()) {
                        self.DynamicOptionsService.ResetData();
                    };
                    
                    const dependences = {};
                    if (isSelectUserNotEmpty) {
                        dependences.User = {};
                        dependences.User.OrganizationID = selectUser.OrganizationID();
                        dependences.User.SubdivisionID = selectUser.SubdivisionID();
                    };

                    // идентификатор формы придет с бэка при изменении сервиса
                    if (formID) {
                        self.DynamicOptionsService.GetTemplateByID(formID, self.FormValues(), dependences);
                    };
                }

                self.SetTab = function (template) {
                    self.mode(`${self.modes.parameterPrefix}${template.Tab.ID}`);
                };

                self.FormValues = ko.observable(null);
                self.FormID = ko.observable(null);
            }

            self.GetDefaultNotificationID = function() {
                const CallEmailTemplateIDSetting = 67;
                let retD = $.Deferred();
                new ajaxLib.control().Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: `/api/settings/${CallEmailTemplateIDSetting}`,
                    },
                    function (response) { retD.resolve(response.Value); },
                    function () { retD.resolve(null); }
                );
                return retD.promise();
            };
        }
    }
    return module;
});
