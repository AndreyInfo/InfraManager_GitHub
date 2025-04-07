define([
    'knockout', 
    'jquery',
    'ajax',
    'models/SDForms/SDForm.CallReferenceList',
    'usualForms',
    'dynamicOptionsService',
    'models/SDForms/SDForm.Tape',
    'models/SDForms/SDForm.WOReferenceList',
    'models/SDForms/SDForm.NegotiationList',
    'models/SDForms/SDForm.LinkList',
    'groupOperation',
    'comboBox'],
function (
    ko,
    $,
    ajaxLib,
    callReferenceListLib,
    fhModule,
    dynamicOptionsService,
    tapeLib,
    workOrderReferenceListLib,
    negotiationListLib,
    linkListLib,
    groupOperation
) {
    var module = {
        ViewModel: function (isReadOnly, $region, id) {
            var self = this;
            var $isLoaded = $.Deferred();
            self.$region = $region;
            self.CloseForm = null; // set in fh
            self.ControlForm = null; //set in fh
            self.CurrentUserID = null; //set in fh
            self.id = id;
            self.objectClassID = 703; //RFC object id
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                tape: 'tape',
                calls: 'calls',
                links: 'links',
                linksKE: 'linksKE',
                negotiation: 'negotiation',
                workorders: 'workorders'
            };
            $.when(userD).done(function (user) {
                self.CurrentUserID = user.UserID;
            });
            self.IsClientMode = ko.observable(false);
            self.IsReadOnly = ko.observable(isReadOnly);
            self.TabHeight = ko.observable(0);
            self.TabWidth = ko.observable(0);
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
                if (newValue == self.modes.nothing) {
                    return;
                };

                //
                if (newValue == self.modes.main) {
                    if (!self.mainTabLoaded()) {
                        self.mainTabLoaded(true);
                        //
                        self.SizeChanged();
                    }
                }
                else if (newValue == self.modes.tape)
                    self.tapeControl.CheckData();
                else if (newValue == self.modes.workorders)
                    self.workOrderList.CheckData();
                else if (newValue == self.modes.links)
                    self.linkList.CheckData();
                else if (newValue == self.modes.calls)
                    self.callList.CheckData();
                else if (newValue == self.modes.linksKE)
                    self.linkListKE.CheckData();
                else if (newValue == self.modes.negotiation)
                    self.negotiationList.CheckData(self.negotiationID);
            });

            self.ClickMain = function () {
                self.mode(self.modes.main)
            }
            //
            self.RFC = ko.observable(null);
            self.RFC.subscribe(function (newValue) {
                $.when($isLoaded).done(function () {
                    self.SizeChanged();//block
                    //
                    if (newValue.InRealization) {
                        self.InRealization(newValue.InRealization());
                    };
                        
                    //
                    self.LoadWorkflowControl();
                    if (!self.IsReadOnly()) {
                        self.LoadPriorityControl();
                    };
                });
            });
            //
            self.IsReadOnly = ko.observable(isReadOnly);
            self.IsReadOnly.subscribe(function (newValue) {
                var readOnly = self.IsReadOnly();
                //
                if (self.attachmentsControl != null)
                    self.attachmentsControl.ReadOnly(readOnly);
                //
                if (self.attachmentsRealizationControl != null)
                    self.attachmentsRealizationControl.ReadOnly(readOnly);
                //
                if (self.attachmentsRollbackControl != null)
                    self.attachmentsRollbackControl.ReadOnly(readOnly);
                //
                if (self.workflowControl() != null)
                    self.workflowControl().ReadOnly(readOnly);
                //
                if (self.priorityControl != null)
                    self.priorityControl.ReadOnly(readOnly);
                //
                if (self.linkList != null)
                    self.linkList.ReadOnly(readOnly);
            });
            //
            self.CanEdit = ko.computed(function () {
                return !self.IsReadOnly();
            });
            //
            self.workflowControl = ko.observable(null);
            self.LoadWorkflowControl = function () {
                if (!self.RFC())
                    return;
                //
                require(['workflow'], function (wfLib) {
                    if (self.workflowControl() == null) {
                        self.workflowControl(new wfLib.control(self.$region, self.IsReadOnly, self.RFC));
                    }
                    self.workflowControl().ReadOnly(self.IsReadOnly());
                    self.workflowControl().Initialize();
                });
            };
            //
            self.priorityControl = null;
            self.LoadPriorityControl = function () {
                if (!self.RFC())
                    return;
                //
                require(['models/SDForms/SDForm.Priority'], function (prLib) {
                    if (self.priorityControl == null || self.priorityControl.IsLoaded() == false) {
                        self.priorityControl = new prLib.ViewModel(self.$region.find('.b-requestDetail-menu__priority'), self, self.IsReadOnly());
                        self.priorityControl.Initialize();
                    }
                    $.when(self.priorityControl.Load(self.RFC().ID(), self.objectClassID, self.RFC().UrgencyID(), self.RFC().InfluenceID(), self.RFC().PriorityID())).done(function (result) {
                        //not needed now
                    });
                });
            };
            //
            self.RefreshPriority = function (priorityObj) {
                if (priorityObj == null)
                    return;
                //
                self.RFC().PriorityName(priorityObj.Name);
                self.RFC().PriorityColor(priorityObj.Color);
                self.RFC().PriorityID(priorityObj.ID);
                self.RFC().InfluenceID(self.priorityControl.CurrentInfluenceID());
                self.RFC().UrgencyID(self.priorityControl.CurrentUrgencyID());
            };
            //
            self.ajaxControl_CustomControl = new ajaxLib.control();
            self.ajaxControl_CustomControlUsers = new ajaxLib.control();
            self.CustomControl = ko.observable(false);
            self.LoadCustomControl = function () {
                if (!self.RFC())
                    return;
                //
                var param = {
                    objectID: self.RFC().ID(),
                };
                self.ajaxControl_CustomControl.Ajax(self.$region.find('.b-requestDetail-menu__item-control'),
                    {
                        method: 'GET',
                        url: '/api/changeRequests/' + param.objectID + '/customControls/my/'
                    },
                    function (details) {
                        self.CustomControl(details.UnderControl);
                    });
                //
                var param2 = {
                    objectID: self.RFC().ID(),
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
                                    var already = ko.utils.arrayFirst(self.RFCUsersList(), function (item) {
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
                                    self.RFCUsersList.push(user);
                                });
                            });
                    });
            };
            self.SaveCustomControl = function () {
                if (!self.RFC())
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
                        url: '/api/changeRequests/' + self.RFC().ID() + '/customControls/my/'
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
                        [{ Uri: '/api/changeRequests/' + self.id }],
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
                        Subject: getTextResource('RFC') + (self.RFC() != null ? (' №' + self.RFC().Number() + ' ' + self.RFC().Summary()) : '')
                    }
                    fh.ShowSendEmailForm(options);
                });
            };
            //
            //RFCTarget
            {
                self.RFCTargetText = getTextResource('Target') + ': ';
                self.EditTarget = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.RFC().ID(),
                            objClassID: self.objectClassID,
                            fieldName: 'RFC.Target',
                            fieldFriendlyName: getTextResource('Target'),
                            oldValue: self.RFC().Target(),
                            maxLength: 250,
                            onSave: function (newText) {
                                self.RFC().Target(newText);
                            },
                            save: function (data) {
                                return patchRfc({
                                    Target: JSON.parse(data.NewValue).text
                                }, function (rfc) { self.RFC().Target(rfc.Target) });
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };
            }
            //
            //EditManhoursWork
            self.EditManhoursWork = function () {
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper();
                    fh.ShowManhoursWorkList(self.RFC, self.objectClassID, self.CanEdit);
                });
            };
            self.EditManhoursNorm = function () {
                if (self.CanEdit() === false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    const fh = new module.formHelper(true);
                    const options = {
                        ID: self.RFC().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'RFC.ManhoursNorm',
                        fieldFriendlyName: getTextResource('ManhoursNorm'),
                        oldValue: self.RFC().ManhoursNorm(),
                        save: function (data) {
                            return patchRfc({ ManhoursNormInMinutes: JSON.parse(data.NewValue).val });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.timeEdit, options);
                });
            };
            //
            //attachments
            {
                self.attachmentsRealizationControl = null;
                self.LoadRealizationAttachmentsControl = function () {
                    if (!self.RFC())
                        return;
                    //
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsRealizationControl != null) {
                            if (self.attachmentsRealizationControl.ObjectID != self.RFC().ID())//previous object  
                                self.attachmentsRealizationControl.RemoveUploadedFiles();
                        }
                        if (self.attachmentsRealizationControl == null || self.attachmentsRealizationControl.IsLoaded() == false) {
                            var attachmentsElement = self.$region.find('.documentRealizationList');
                            self.attachmentsRealizationControl = new fcLib.control(attachmentsElement, '.realizationFileField', '.addRealizationFileBtn', null, true, false);
                            self.attachmentsRealizationControl.OnChange = function (url, onSuccess, docId, isDeleting) {
                                if (self.workflowControl() != null)
                               //     self.workflowControl().OnSave();

                                var options = {
                                    FieldName: 'RFC.RealizationAttachments',
                                    OldValue: self.RFC().RealizationDocumentID().length == 0 ? null : JSON.stringify({ 'id': self.RFC().RealizationDocumentID() }),
                                    NewValue: isDeleting ? null : docId,
                                    onSave: function () {
                                    },
                                    save: function (data) {
                                        return patchRfc({
                                            RealizationDocumentID: data.NewValue === null ? '00000000-0000-0000-0000-000000000000' : data.NewValue[0]
                                        }, function (rfc) { });
                                    }
                                    };
                                self.UpdateField(false, options);
                            };
                        }
                        self.attachmentsRealizationControl.ReadOnly(self.IsReadOnly());
                        self.attachmentsRealizationControl.InitializeOneSelectFileControl(self.RFC().ID(), self.RFC().RealizationDocumentID(), true);
                    });
                };
                self.attachmentsRollbackControl = null;
                self.LoadRollbackAttachmentsControl = function () {
                    if (!self.RFC())
                        return;
                    //
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsRollbackControl != null) {
                            if (self.attachmentsRollbackControl.ObjectID != self.RFC().ID())//previous object  
                                self.attachmentsRollbackControl.RemoveUploadedFiles();
                        }
                        if (self.attachmentsRollbackControl == null || self.attachmentsRollbackControl.IsLoaded() == false) {
                            var attachmentsElement = self.$region.find('.documentRollbackList');
                            self.attachmentsRollbackControl = new fcLib.control(attachmentsElement, '.rollbackFileField', '.addRollbackFileBtn', null, true, true);
                            self.attachmentsRollbackControl.OnChange = function (url, onSuccess, docId, isDeleting) {
                                if (self.workflowControl() != null)
                                    //self.workflowControl().OnSave();
                                var options = {
                                    FieldName: 'RFC.RollbackAttachments',
                                    OldValue: self.RFC().RollbackDocumentID().length == 0 ? null : JSON.stringify({ 'id': self.RFC().RollbackDocumentID() }),
                                    NewValue: isDeleting ? null : docId,
                                    onSave: function () {
                                    },
                                    save: function (data) {
                                        return patchRfc({
                                            RollbackDocumentID: data.NewValue === null ? '00000000-0000-0000-0000-000000000000' : data.NewValue[0]
                                        }, function (rfc) { });
                                    }
                                };
                                self.UpdateField(false, options);
                            };
                        }
                        self.attachmentsRollbackControl.ReadOnly(self.IsReadOnly());
                        self.attachmentsRollbackControl.InitializeOneSelectFileControl(self.RFC().ID(), self.RFC().RollbackDocumentID(), true);
                    });
                };
                self.attachmentsControl = null;
                self.LoadAttachmentsControl = function () {
                    if (!self.RFC())
                        return;
                    //
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsControl != null) {
                            if (self.attachmentsControl.ObjectID != self.RFC().ID())//previous object  
                                self.attachmentsControl.RemoveUploadedFiles();
                            else if (!self.attachmentsControl.IsAllFilesUploaded())//uploading
                            {
                                setTimeout(self.LoadAttachmentsControl, 1000);//try to reload after second
                                return;
                            }
                        }
                        if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                            var attachmentsElement = self.$region.find('.RFCdocumentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.RFCFileField', '.addFileBtn');
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
                        self.attachmentsControl.Initialize(self.RFC().ID());
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
            //EditFundingAmount
            {
                self.EditFundingAmount = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.RFC().ID(),
                            objClassID: self.objectClassID,
                            fieldName: 'RFC.FundingAmount',
                            fieldFriendlyName: getTextResource('FundingAmount'),
                            oldValue: self.RFC().FundingAmount(),
                            maxLength: 250,
                            onSave: function (newText) {
                                self.RFC().FundingAmount(newText);
                            },
                            save: function (data) {
                                return patchRfc({
                                    FundingAmountNumber: JSON.parse(data.NewValue).val
                                }, function (rfc) { self.RFC().FundingAmount(rfc.FundingAmount); });
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                };
                self.IsPlanningContainerVisible = ko.observable(false);
                self.TogglePlanningContainer = function () {
                    self.IsPlanningContainerVisible(!self.IsPlanningContainerVisible());
                };
            }
            //
            self.EditSummary = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.RFC().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'RFC.Summary',
                        fieldFriendlyName: getTextResource('Summary'),
                        oldValue: self.RFC().Summary(),
                        maxLength: 250,
                        onSave: function (newText) {
                            self.RFC().Summary(newText);
                        },
                        save: function (data) {
                            return patchRfc({
                                Summary: JSON.parse(data.NewValue).text
                            }, function (rfc) {
                                self.RFC().Summary(rfc.Summary);
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };

            self.ajaxControl_SetCustomControl = new ajaxLib.control();
            function patchRfc(model, callback) {
                var retD = $.Deferred();
                self.ajaxControl_SetCustomControl.Ajax(
                    null,
                    {
                        dataType: "json",
                        contentType: "application/json",
                        method: "PATCH",
                        url: '/api/changerequests/' + id,
                        data: JSON.stringify(model)
                    },
                    function (rfc) {
                        if (self.tapeControl && self.tapeControl.TimeLineControl && self.tapeControl.isTimeLineLoaded && self.tapeControl.isTimeLineLoaded()) {
                            var mainTLC = self.tapeControl.TimeLineControl();
                            if (mainTLC != null && mainTLC.TimeLine) {
                                var currentTL = mainTLC.TimeLine();
                                if (currentTL != null && currentTL.UtcDatePromised) {
                                    currentTL.UtcDatePromised.LocalDate(self.RFC().UtcDatePromised());
                                    currentTL.UtcDatePromised.DateObj(self.RFC().UtcDatePromisedDT());
                                }
                            }
                        }
                        if (callback === undefined) {
                        }
                        else {
                            callback(rfc);
                        }
                        self.DynamicOptionsService.ResetData();
                        hideSpinner();
                        retD.resolve(true);
                    });
                return retD;
            }

            self.EditDescription = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.RFC().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'RFC.Description',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: self.RFC().Description(),
                        onSave: function (newHTML) {
                            self.RFC().Description(newHTML);
                        },
                        readOnly: !self.CanEdit(),
                        save: function (data) {
                            return patchRfc({
                                HTMLDescription: JSON.parse(data.NewValue).text
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                });
            };
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
                    thisObj.LoadList = function () {
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
                self.EditRFCType = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.RFC().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'RFC.RFCType',
                        fieldFriendlyName: getTextResource('RFCType'),
                        oldValue: { ID: self.RFC().TypeID(), ClassID: 710, FullName: self.RFC().TypeName() },
                        searcherName: 'RFCTypeSearcher',
                        searcherPlaceholder: getTextResource('RFCType'),
                        onSave: function (objectInfo) {
                            if (!objectInfo)
                                return;
                            self.RFC().TypeID(objectInfo.ID);
                            self.RFC().TypeName(objectInfo.FullName);
                            self.LoadWorkflowControl();
                        },
                        save: function (data) {
                            // TODO: тут запрос на смену параметров
                            return patchRfc({
                                TypeID: JSON.parse(data.NewValue).id
                            }, function (rfc) {
                                self.RFC().TypeID(rfc.TypeID);
                                self.RFC().TypeName(rfc.TypeName);
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }
            //
            //rfcCategory
            {
                self.EditRFCCategory = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    const fh = new fhModule.formHelper(true);
                    const options = {
                        ID: self.RFC().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'RFC.RFCCategory',
                        fieldFriendlyName: getTextResource('RFCCategory'),
                        oldValue: { ID: self.RFC().CategoryID()},
                        searcherName: 'RFCCategorySearcher',
                        searcherPlaceholder: getTextResource('RFCCategory'),
                        onSave: function (objectInfo) {
                            self.RFC().CategoryID(objectInfo == null ? null : objectInfo.ID)
                            self.RFC().CategoryName(objectInfo == null ? null : objectInfo.FullName)
                        },
                        save: function (data) {
                            return patchRfc({
                                CategoryID: JSON.parse(data.NewValue).id
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }
            //
            //EditService
            {
                self.EditService = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: self.RFC().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'RFC.Service',
                        fieldFriendlyName: getTextResource('CallService'),
                        oldValue: { ID: self.RFC().ServiceID(), FullName: self.RFC().ServiceName() },
                        searcherName: 'ServiceSearcherForRFC',
                        searcherPlaceholder: getTextResource('CallService'),
                        onSave: function (objectInfo) {
                            self.RFC().ServiceID(objectInfo.ID)
                            self.RFC().ServiceName(objectInfo.FullName)
                        },
                        save: function (data) {
                            return patchRfc({
                                ServiceID: JSON.parse(data.NewValue).id
                            }, function (rfc) {
                                self.RFC().ServiceID(rfc.ServiceID);
                                self.RFC().ServiceName(rfc.ServiceName);
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }
            //
            //EditStartDate
            {
                self.EditStartDate = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.RFC().ID(),
                            objClassID: self.objectClassID,
                            fieldName: 'RFC.DateStarted',
                            fieldFriendlyName: getTextResource('ToBegin'),
                            oldValue: self.RFC().UtcDateStartedDT(),
                            onSave: function (newDate) {
                                self.RFC().UtcDateStarted(parseDate(newDate));
                                self.RFC().UtcDateStartedDT(new Date(parseInt(newDate)));
                            },
                            save: function (data) {
                                return patchRfc({ UtcDateStarted: JSON.parse(data.NewValue).text }, function (rfc) {
                                    self.RFC().UtcDateStarted(parseDate(rfc.UtcDateStarted));
                                    self.RFC().UtcDateStartedDT(new Date(parseInt(rfc.UtcDateStarted)));
                                });
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                }
                self.DateStarCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                    var retval = '';
                    //
                    if (!retval && self.RFC) {
                        var rfc = self.RFC();
                        if (rfc && rfc.UtcDateStarted)
                            retval = rfc.UtcDateStarted();
                    }
                    //
                    return retval;
                });     
            }
            //           
            //initiator
            {
                self.InitializeInitiator = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var rfc = self.RFC();
                        if (rfc.InitiatorLoaded() == false && rfc.InitiatorID()) {
                            var options = {
                                UserID: rfc.InitiatorID(),
                                UserType: userLib.UserTypes.initiator,
                                UserName: null,
                                EditAction: self.EditInitiator,
                                RemoveAction: self.DeleteInitiator,
                                CanNote: true
                            };
                            var user = new userLib.User(self, options);
                            rfc.Initiator(user);
                            rfc.InitiatorLoaded(true);
                            //
                            var already = ko.utils.arrayFirst(self.RFCUsersList(), function (item) {
                                return item.ID() == rfc.OwnerID();
                            });
                            //
                            if (already == null)
                                self.RFCUsersList.push(user);
                            else if (already.Type == userLib.UserTypes.withoutType) {
                                self.RFCUsersList.remove(already);
                                self.RFCUsersList.push(user);
                            }
                        }
                    });
                };
                self.EditInitiator = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.RFC().ID(),
                            objClassID: self.objectClassID,
                            fieldName: 'RFC.Initiator',
                            fieldFriendlyName: getTextResource('Initiator'),
                            oldValue: self.RFC().InitiatorLoaded() ? { ID: self.RFC().Initiator().ID(), ClassID: 9, FullName: self.RFC().Initiator().FullName() } : null,
                            object: ko.toJS(self.RFC().Initiator()),
                            searcherName: 'WebUserSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            onSave: function (objectInfo) {
                                self.RFC().InitiatorLoaded(false);
                                self.RFC().Initiator(new userLib.EmptyUser(self, userLib.UserTypes.initiator, self.EditInitiator));
                                //
                                self.RFC().InitiatorID(objectInfo ? objectInfo.ID : '');
                                self.InitializeInitiator();
                            },
                            save: function (data) {
                                return patchRfc({
                                    InitiatorID: JSON.parse(data.NewValue).id
                                }, function (rfc) {
                                    require(['models/SDForms/SDForm.User'], function (userLib) {
                                        self.RFC().InitiatorLoaded(false);
                                        self.RFC().Initiator(new userLib.EmptyUser(self, userLib.UserTypes.initiator, self.EditInitiator));
                                        self.RFC().InitiatorID(rfc.InitiatorID || "");
                                        self.InitializeInitiator();
                                    });
                                });
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };

                self.DeleteInitiator = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var options = {
                            FieldName: 'RFC.Initiator',
                            OldValue: self.RFC().InitiatorLoaded() ? { ID: self.RFC().Initiator().ID(), ClassID: 9, FullName: self.RFC().Initiator().FullName() } : null,
                            onSave: function () {
                                self.RFC().InitiatorLoaded(false);
                                self.RFC().Initiator(new userLib.EmptyUser(self, userLib.UserTypes.initiator, self.EditOwner));
                                //
                                self.RFC().InitiatorID('');
                            }
                        };
                        self.DeleteUser(false, options);
                    });
                };
            }
            //
            //owner
            {
                self.InitializeOwner = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var rfc = self.RFC();
                        if (rfc.OwnerLoaded() == false && rfc.OwnerID()) {
                            var options = {
                                UserID: rfc.OwnerID(),
                                UserType: userLib.UserTypes.owner,
                                UserName: null,
                                EditAction: self.EditOwner,
                                RemoveAction: self.DeleteOwner,
                                CanNote: true
                            };
                            var user = new userLib.User(self, options);
                            rfc.Owner(user);
                            rfc.Owner().TypeName = ko.observable(getTextResource('Coordinator_Owner'));
                            rfc.OwnerLoaded(true);
                            //
                            var already = ko.utils.arrayFirst(self.RFCUsersList(), function (item) {
                                return item.ID() == rfc.OwnerID();
                            });
                            //
                            if (already == null)
                                self.RFCUsersList.push(user);
                            else if (already.Type == userLib.UserTypes.withoutType) {
                                self.RFCUsersList.remove(already);
                                self.RFCUsersList.push(user);
                            }
                        }
                    });
                };
                self.EditOwner = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.RFC().ID(),
                            objClassID: self.objectClassID,
                            fieldName: 'RFC.Owner',
                            fieldFriendlyName: getTextResource('Owner'),
                            oldValue: self.RFC().OwnerLoaded() ? { ID: self.RFC().Owner().ID(), ClassID: 9, FullName: self.RFC().Owner().FullName() } : null,
                            object: ko.toJS(self.RFC().Owner()),
                            searcherName: 'OwnerUserSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [self.RFC().QueueID() == '' ? null : self.RFC().QueueID()],
                            onSave: function (objectInfo) {
                                self.RFC().OwnerLoaded(false);
                                self.RFC().Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner, true, true, getTextResource('Coordinator_Owner')));
                                //
                                self.RFC().OwnerID(objectInfo ? objectInfo.ID : '');
                                self.InitializeOwner();
                            },
                            save: function (data) {
                                return patchRfc({
                                    OwnerID: JSON.parse(data.NewValue).id
                                }, function (rfc) {
                                    require(['models/SDForms/SDForm.User'], function (userLib) {
                                        self.RFC().OwnerLoaded(false);
                                        self.RFC().Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                                        self.RFC().OwnerID(rfc.OwnerID || "");
                                        self.InitializeOwner();
                                    });
                                });
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.DeleteOwner = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var options = {
                            FieldName: 'RFC.Owner',
                            OldValue: self.RFC().OwnerLoaded() ? { ID: self.RFC().Owner().ID(), ClassID: 9, FullName: self.RFC().Owner().FullName() } : null,
                            onSave: function () {
                                self.RFC().OwnerLoaded(false);
                                self.RFC().Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner, true, true, getTextResource('Coordinator_Owner')));
                                //
                                self.RFC().OwnerID('');
                            }
                        };
                        self.DeleteUser(false, options);
                    });
                };
            }
            //
            self.EditQueue = function () {
                if (self.CanEdit() == false)
                    return;
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.RFC().ID(),
                        objClassID: self.objectClassID,
                        fieldName: 'RFC.Queue',
                        fieldFriendlyName: getTextResource('Queue'),
                        oldValue: self.RFC().QueueLoaded() ? { ID: self.RFC().Queue().ID(), ClassID: self.RFC().Queue().ClassID(), FullName: self.RFC().Queue().FullName() } : null,
                        object: ko.toJS(self.RFC().Queue()),
                        searcherName: "QueueSearcher",
                        searcherPlaceholder: getTextResource('EnterQueue'),
                        searcherParams: { Type: 16 },
                        onSave: function (objectInfo) {
                            self.RFC().QueueLoaded(false);
                            self.RFC().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                            //
                            if (objectInfo && objectInfo.ClassID == 722) { //IMSystem.Global.OBJ_QUEUE
                                self.RFC().QueueID(objectInfo.ID);
                                self.RFC().QueueName(objectInfo.FullName);
                            }
                            else {
                                self.RFC().QueueID('');
                                self.RFC().QueueName('');
                            }
                            self.InitializeQueue();
                        },
                        save: function (data) {
                            return patchRfc({
                                QueueID: data.NewValue ? JSON.parse(data.NewValue).id : null
                            }, function (rfc) {
                                self.RFC().QueueLoaded(false);
                                self.RFC().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                                self.RFC().QueueID(rfc.QueueId);
                                self.RFC().QueueName(rfc.QueueName);
                                self.InitializeQueue();
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            self.DeleteQueue = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var options = {
                        FieldName: 'Call.Queue',
                        OldValue: self.RFC().QueueLoaded() ? { ID: self.RFC().Queue().ID(), ClassID: self.RFC().Queue().ClassID(), FullName: self.RFC().Queue().FullName() } : null,
                        onSave: function () {
                            self.RFC().QueueLoaded(false);
                            self.RFC().Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                            //
                            self.RFC().QueueID('');
                            self.RFC().QueueName('');
                        }
                    };
                    self.DeleteUser(false, options);
                });
            };
            self.InitializeQueue = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var rfc = self.RFC();
                    //
                    if (rfc.QueueLoaded() == false) {
                        if (rfc.QueueID()) {
                            var options = {
                                UserID: rfc.QueueID(),
                                UserType: userLib.UserTypes.queueExecutor,
                                UserName: null,
                                EditAction: self.EditQueue,
                                RemoveAction: self.DeleteQueue,
                                CanNote: true,
                                IsFreezeSelectedClient: true
                            };
                            var user = new userLib.User(self, options);
                            rfc.Queue(user);
                            rfc.QueueLoaded(true);
                            //
                            var already = ko.utils.arrayFirst(self.RFCUsersList(), function (item) {
                                return item.ID() == rfc.QueueID();
                            });
                            //
                            if (already == null)
                                self.RFCUsersList.push(user);
                            else if (already.Type == userLib.UserTypes.withoutType) {
                                self.RFCUsersList.remove(already);
                                self.RFCUsersList.push(user);
                            }
                        }
                    }
                });
            };



            self.CalculateUsersList = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    if (!self.RFC()) {
                        self.RFCUsersList([]);
                        self.RFCUsersList.valueHasMutated();
                        return;
                    }
                    //
                    self.InitializeInitiator();
                    self.InitializeOwner();
                    self.InitializeQueue();
                    //
                    self.RFCUsersList.valueHasMutated();
                    //add currentUser to list
                    $.when(userD).done(function (userObj) {
                        require(['models/SDForms/SDForm.User'], function (userLib) {
                            var already = ko.utils.arrayFirst(self.RFCUsersList(), function (item) {
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
                            self.RFCUsersList.push(user);
                        });
                    });
                    self.LoadCustomControl();
                });
            };
            //
            self.RFCUsersList = ko.observableArray([]);
            //
            //DeleteUser
            {
                self.ajaxControl_deleteUser = new ajaxLib.control();
                self.DeleteUser = function (isReplaceAnyway, options) {
                    var data = {
                        ID: self.RFC().ID(),
                        ObjClassID: self.objectClassID,
                        Field: options.FieldName,
                        OldValue: options.OldValue == null ? null : JSON.stringify({ 'id': options.OldValue.ID, 'fullName': options.OldValue.FullName }),
                        NewValue: null,
                        Params: null,
                        ReplaceAnyway: isReplaceAnyway
                    };
                    //
                    self.ajaxControl_deleteUser.Ajax(
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
                                        swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[RFCForm.js DeleteUser]', 'error');
                                    });
                                }
                                else if (result === 2) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[RFCForm.js DeleteUser]', 'error');
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
                                                    self.DeleteUser(true, options);
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
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[RFCForm.js DeleteUser]', 'error');
                                    });
                                }
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[RFCForm.js, DeleteUser]', 'error');
                                });
                            }
                        });
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
                    if (referenceObject.ReasonObjectClassID() === 701) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowCall(referenceObject.ReasonObjectID(), self.IsReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);
                        });
                    }
                    else if (referenceObject.ReasonObjectClassID() === 702) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowProblem(referenceObject.ReasonObjectID(), self.IsReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);
                        });
                    }
                    else if (referenceObject.ReasonObjectClassID() === 119) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowWorkOrder(referenceObject.ReasonObjectID(), self.IsReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);
                        });
                    }
                };
                //
                self.ContextMenuVisible = ko.observable(false);
                //
                self.LinkSdObjectClick = function (data, e) {
                    if (self.CanEdit() == false)
                        return;
                    var isVisible = self.ContextMenuVisible();
                    self.ContextMenuVisible(!isVisible);
                    //
                    e.stopPropagation();
                };
                //
                self.ReasonCallBackObject = function (obj, ClassID, WithoutUpdateField) {
                    if (obj.ID)
                        obj = obj.ID;
                    $.when(self.LoadReferenceObject(obj, ClassID)).done(function (loadResult) {
                        if (loadResult === true) {
                            const mself = self.ReasonObject();
                            if (mself.ClassID === 701) {
                                self.InitializeReasonObjectOwner();
                                self.ReasonName('№' + mself.Number() + ' ' + mself.CallSummaryName());
                            }
                            else if (mself.ClassID === 702) {
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
                        if (!WithoutUpdateField) {
                            patchRfc({ 
                                ReasonObjectID: self.ReasonObject().ID(),
                                ReasonObjectClassID: self.ReasonObject().ClassID,
                            }, function (rfc) {
                                self.RFC().ReasonObjectID(rfc.ReasonObjectID);
                                self.RFC().ReasonObjectClassID(rfc.ReasonObjectClassID);
                            })
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
                                            self.VisibleReason(false);
                                            self.IsReasonContainerVisible(false);
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('RFCReasonNotAccess'), 'error');
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
                                            self.VisibleReason(false);
                                            self.IsReasonContainerVisible(false);
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('RFCReasonNotAccess'), 'error');
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
                                            self.VisibleReason(false);
                                            self.IsReasonContainerVisible(false);
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('RFCReasonNotAccess'), 'error');
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

                self.ajaxControl_updateField = new ajaxLib.control();
                self.UpdateField = function (isReplaceAnyway, options) {
                    var data = {
                        ID: self.RFC().ID(),
                        ObjClassID: self.objectClassID,
                        Field: options.FieldName,
                        OldValue: options.OldValue == null ? null : options.OldValue,
                        NewValue: options.NewValue == null ? null : options.NewValue,
                        ReplaceAnyway: isReplaceAnyway
                    };
                    if (typeof options.save === 'function') {
                        $.when(options.save(data))
                            .done(function (d) { });
                    } else {
                        self.ajaxControl_updateField.Ajax(
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
                                            swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[CallForm.js UpdateField]', 'error');
                                        });
                                    }
                                    else if (result === 2) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[CallForm.js UpdateField]', 'error');
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
                                                        self.UpdateField(true, options);
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
                                            swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[CallForm.js UpdateField]', 'error');
                                        });
                                    }
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[CallForm.js UpdateField]', 'error');
                                    });
                                }
                            });
                    }
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
            //EditPromisedDate
            {
                self.EditDatePromised = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.RFC().ID(),
                            objClassID: self.objectClassID,
                            fieldName: 'RFC.DatePromised',
                            fieldFriendlyName: getTextResource('CallDatePromise'),
                            oldValue: self.RFC().UtcDatePromisedDT(),
                            onSave: function (newDate) {
                                self.RFC().UtcDatePromised(parseDate(newDate));
                                self.RFC().UtcDatePromisedDT(new Date(parseInt(newDate)));
                            },
                            save: function (data) {
                                return patchRfc({ UtcDatePromised: JSON.parse(data.NewValue).text }, function (rfc) {
                                    self.RFC().UtcDatePromised(parseDate(rfc.UtcDatePromised));
                                    self.RFC().UtcDatePromisedDT(new Date(parseInt(rfc.UtcDatePromised)));
                                });
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                }
            }
            self.DatePromisedCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                var retval = '';
                //
                if (!retval && self.RFC) {
                    var rfc = self.RFC();
                    if (rfc && rfc.UtcDatePromised)
                        retval = rfc.UtcDatePromised();
                }
                //
                return retval;
            });
            self.DateModifyCalculated = ko.computed(function () {//или из объекта, или из хода выполнения
                var retval = '';
                if (self.tapeControl && self.tapeControl.TimeLineControl && self.tapeControl.isTimeLineLoaded && self.tapeControl.isTimeLineLoaded()) {
                    var mainTLC = self.tapeControl.TimeLineControl();
                    if (mainTLC != null && mainTLC.TimeLine) {
                        var currentTL = mainTLC.TimeLine();
                        if (currentTL != null && currentTL.UtcDateModified)
                            retval = currentTL.UtcDateModified.LocalDate();
                    }
                }
                if (!retval && self.RFC) {
                    var r = self.RFC();
                    if (r && r.UtcDateModified)
                        retval = r.UtcDateModified();
                }
                return retval;
            });
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
                { name: 'UtcDateModified' },
                { name: 'UtcDateStarted' }
            ];
            self.tapeControl = new tapeLib.Tape(self.RFC, self.objectClassID, '/api/changerequests/' + self.id, timelineConfig, patchRfc, self.$region.find('.tape__b').selector, self.$region.find('.tape__forms').selector, self.IsReadOnly, self.CanEdit, self.CanViewNotes, self.TabSize, self.RFCUsersList);
            //
            //WO REFERENCE BLOCK
            self.workOrderList = new workOrderReferenceListLib.LinkList(self.RFC, self.objectClassID, self.$region.find('.woRef__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //LINKS BLOCK
            self.linkList = new linkListLib.LinkList(self.RFC, self.objectClassID, self.$region.find('.links__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //CALL REFERENCE BLOCK
            self.callList = new callReferenceListLib.LinkList(self.RFC, self.objectClassID, self.$region.find('.cRef__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //LINKS KE BLOCK + переопределение list для загрузки только КУ
            {
                self.linkListKE = new linkListLib.LinkList(self.RFC, self.objectClassID, self.$region.find('.linksKE__b .tabContent').selector, self.IsReadOnly, self.CanEdit,null,true);
                self.linkListKE.imList.options.LoadAction = function () {
                    const retvalD = $.Deferred();
                    self.linkListKE.ajaxControl.Ajax($(self.$region.find('.linksKE__b .tabContent').selector),
                        {
                            dataType: "json",
                            method: 'GET',
                            url: `/api/changerequests/${self.RFC().ID()}/dependencies`,
                        },
                        function (newVal) {
                            require(['models/SDForms/SDForm.Link'], function (linkLib) {
                                const retval = [];
                                ko.utils.arrayForEach(newVal, function (item) {
                                    retval.push(new linkLib.Link(self.linkListKE.imList, item));
                                });
                                $.when(self.linkListKE.LoadGrantedOperations()).done(function () { retvalD.resolve(retval); });
                            });
                        },
                        function (err) {
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
                            retvalD.resolve([]);
                        });
                    return retvalD.promise();
                };
                //
                self.linkListKE.ItemsCount = ko.computed(function () {//вариант расчета количества элементов (по данным объекта / по реальному количеству из БД)
                    let retval = 0;
                    if (self.linkListKE.isLoaded())
                        retval = self.linkListKE.imList.List().length;
                    else if (self.RFC != null && self.RFC() != null)
                        retval = self.RFC().DependencyKEObjectCount();
                    //
                    if (retval <= 0)
                        return null;
                    if (retval > 99)
                        return '99';
                    else return '' + retval;
                });
            }
            //
            //NEGOTIATION BLOCK
            self.negotiationList = new negotiationListLib.LinkList(self.RFC, self.objectClassID, self.$region.find('.negotiations__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            //
            //Load
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
                            url: '/api/changerequests/' + id
                        },
                        function (newVal) {
                            var loadSuccessD = $.Deferred();
                            var processed = false;
                            //
                            if (newVal) {
                                    var pInfo = newVal;
                                    if (pInfo && pInfo.ID) {
                                        require(['models/SDForms/RFCForm.RFC'], function (pLib) {
                                            self.RFC(new pLib.RFC(self, pInfo));
                                            self.RFC().getCalls();
                                            self.RFCUsersList.removeAll();
                                            self.CalculateUsersList();
                                            self.LoadRollbackAttachmentsControl();
                                            self.LoadRealizationAttachmentsControl();
                                            self.LoadAttachmentsControl();
                                            self.ReasonCallBackObject(self.RFC().ReasonObjectID(), self.RFC().ReasonObjectClassID(),true);
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
                                                // TODO: тут запрос на сервер
                                                self.DynamicOptionsServiceInit(pInfo.FormValues.FormID, pInfo.FormValues.Values);
                                            };

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
                                        swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[RFCForm.js, Load]', 'error');
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
            self.Unload = function () {
                $(document).unbind('objectInserted', self.onObjectInserted);
                $(document).unbind('objectUpdated', self.onObjectUpdated);
                $(document).unbind('objectDeleted', self.onObjectDeleted);
                $(document).unbind('local_objectInserted', self.onObjectInserted);
                $(document).unbind('local_objectUpdated', self.onObjectUpdated);
                $(document).unbind('local_objectDeleted', self.onObjectDeleted);
                $(document).unbind('resizeEntity');
            };
            //
            self.CanEdit = ko.computed(function () {
                return !self.IsReadOnly();
            });
            //
            self.AfterRender = function () {
                self.SizeChanged();
            };
            self.renderRFCComplete = function () {
                $isLoaded.resolve();
                self.SizeChanged();
            };
            //
            self.SizeChanged = function () {
                if (!self.RFC())
                    return;//Critical - ko - with:rfc!!!
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
                self.callList.ClearData();
                self.linkListKE.ClearData();
                self.workOrderList.ClearData();
                self.solutionTabLoaded(false);
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
            //
            {//InRealization
                self.InRealization = ko.observable(false);
                self.InRealization.subscribe(function (newValue) {
                    if (!self.CanEdit() || self.RFC().InRealization() == newValue)
                        return;
                    var oldValue = !self.InRealization();
                    var options = {
                        FieldName: 'RFC.InRealization',
                        OldValue: oldValue == null ? null : JSON.stringify({ 'val': oldValue }),
                        NewValue: self.InRealization == null ? null : JSON.stringify({ 'val': self.InRealization() }),
                    };
                    self.UpdateField(false, options);
                    self.RFC().InRealization(newValue);
                });
            }
            //
            //Container
            self.IsDescriptionContainerVisible = ko.observable(true);
            self.ToggleDescriptionContainer = function () {
                self.IsDescriptionContainerVisible(!self.IsDescriptionContainerVisible());
            };
            //
            self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.RFC().ID();
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
                else if (objectClassID == 119 && currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase()) //OBJ_WORKORDER
                {
                    if (self.workOrderList.isLoaded())
                        self.workOrderList.imList.TryReloadByID(objectID);
                    else 
                        self.Reload(currentID);
                }
                else if (objectClassID == 701 && currentID) //OBJ_CALL
                {
                     self.Reload(currentID);
                }
                else if (objectClassID == 701 && currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase() ) //OBJ_CALL
                {
                    if (self.callList.isLoaded())
                        self.callList.imList.TryReloadByID(objectID);
                    else 
                        self.Reload(currentID);
                }
            };
            self.onObjectUpdated = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.RFC().ID();
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
                else if (objectClassID == 701 && currentID && parentObjectID && currentID.toLowerCase() == parentObjectID.toLowerCase()) //OBJ_CALL
                {
                    if (self.callList.isLoaded())
                        self.callList.imList.TryReloadByID(objectID);
                    else 
                        self.Reload(currentID);
                }
                else if (objectClassID == 703 && currentID == objectID && e.type != 'local_objectUpdated') //OBJ_RFC
                    self.Reload(currentID);
            };
            self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.RFC().ID();
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
                else if (objectClassID == 703 && currentID == objectID) //OBJ_RFC
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
                    updateCallBack: patchRfc,
                    isReadOnly: self.IsReadOnly
                });

                self.DynamicOptionsServiceInit = function (formID, values) {
                    if (self.DynamicOptionsService.IsInit()) {
                        self.DynamicOptionsService.ResetData();
                    };

                    self.DynamicOptionsService.GetTemplateByID(formID, values);
                };

                self.SetTab = function (template) {
                    self.mode(`${self.modes.parameterPrefix}${template.Tab.ID}`);
                };
            }
        }
    }
    return module;
});
