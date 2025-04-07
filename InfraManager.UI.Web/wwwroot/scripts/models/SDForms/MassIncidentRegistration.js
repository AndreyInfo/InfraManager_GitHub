define(['knockout', 'jquery', 'usualForms', 'ajax', 'dynamicOptionsService'], function (ko, $, fhModule, ajaxLib, dynamicOptionsService) {
    const module = {
        ViewModel: function (options, formID) {
            var self = this;

            self.FormID = formID;

            self.renderedD = $.Deferred();
            self.PriorityName = ko.observable(getTextResource('PromptPriority'));

            self.modes = {
                nothing: 'nothing',
                main: 'main',
                parameterPrefix: 'parameter_'
            };

            self.mode = ko.observable(self.modes.main);

            self.SetTab = function (template) {
                self.mode(`${self.modes.parameterPrefix}${template.Tab.ID}`);
            };

            self.DynamicOptionsService = new dynamicOptionsService.ViewModel(self.FormID, {
                typeForm: 'Create',
                changeTabCb: function (invalidInput) {
                    const parent = invalidInput.closest('[data-prefix-mode]');
                    const tabPrefix = parent.attr('data-prefix-mode');
                    self.mode(tabPrefix);
                }
            });
            
            self.DynamicOptionsServiceInit = function (typeID) {
                if (self.DynamicOptionsService.IsInit()) {
                    self.DynamicOptionsService.ResetData();
                }

                self.DynamicOptionsService.GetTemplate(dynamicOptionsService.Classes.MassIncidentType, typeID);
            }

            self.AfterRender = function () {
                self.renderedD.resolve();
            };
            self.ajaxControl_ServiceItemAttendance = new ajaxLib.control();

            self.techFailureCategory = {
                id: ko.observable(null),
                name: ko.observable(''),
                serviceReferences: [],
                reset: function () {
                    self.techFailureCategory.id(null);
                    self.techFailureCategory.name('');
                    self.techFailureCategory.serviceReferences = [];
                }
            };

            self.editTechFailureCategory = function () {
                if (self.ServiceID == null) {
                    return self.ShowError('PromptMassiveIncidentService');
                };

                var fh = new fhModule.formHelper(true);
                var options = {
                    ID: null,
                    objClassID: 783,
                    fieldName: 'MassIncident.TechnicalFailureCategory',
                    fieldFriendlyName: getTextResource(`MassIncidentTechnicalFailureCategoryLabel`),
                    comboBoxGetValueUrl: '/api/technicalFailureCategories?ServiceID=' + self.ServiceID,
                    oldValue: { ID: self.techFailureCategory.id(), Name: self.techFailureCategory.name() },
                    onSave: function (data) {
                        if (!data)
                            return;
                        self.techFailureCategory.id(data.ID);
                        self.techFailureCategory.name(data.Name);
                        self.techFailureCategory.serviceReferences = [];
                        new ajaxLib.control().Ajax(null, {
                            method: 'GET',
                            url: '/api/technicalFailureCategories/' + data.ID
                        }, function (result) {
                            self.techFailureCategory.serviceReferences = result.ServiceReferences;
                        });
                    },
                    nosave: true
                };
                fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
            };

            self.Load = function () {
                $.when(self.renderedD).done(function () {
                    if (self.AddAs) {
                        self.Fill(self.miData);
                    }
                    self.LoadCall();
                    self.InitializeDescription();
                    self.InitializeCause();
                    self.InitializeSolution();
                    self.LoadAttachmentsControl();

                    self.InitializeInitiator();
                    self.InitializeQueue();
                    self.InitializeOwner();
                    self.InitExecutor();
                    self.LoadPriorityControl();
                    self.LoadCriticalityControl();

                    $.when(userD).done(function (user) {
                        $('#' + self.FormID).find('.firstFocus').focus();
                        self.InitiatorLoaded(false);
                        if (!self.InitiatorID) 
                            self.InitiatorID = user.UserID;
                        //
                        self.InitializeInitiator();

                        self.serviceItemAttendanceSearcherD = $.Deferred();
                        if (self.serviceItemAttendanceSearcher != null)
                            self.serviceItemAttendanceSearcher.Remove();
                        var fh = new fhModule.formHelper();
                        var textInput = $('#' + self.FormID).find('.serviceItemAttendance');

                        fh.SetTextSearcherToField(
                            textInput,
                            'ServiceSearcher',
                            null,
                            { Types: [0, 1] },
                            function (objectInfo) {//select
                                //
                                self.ServiceSet(objectInfo.ID, objectInfo.FullName, false);
                            },
                            function () {//reset
                                self.ServiceClear();
                            },
                            function (selectedItem) {//close
                                if (!selectedItem)
                                    self.ServiceClear();
                            },
                            null);
                    });
                    self.TypeSelectHelper.LoadList();
                    self.InformationChannelSelectHelper.LoadList();
                    self.CauseSelectHelper.LoadList();
                })
            };

            self.createComboBoxItem = function (simpleDictionary) {
                const thisObj = this;
                //
                thisObj.ID = simpleDictionary.ID;
                thisObj.IMObjID = simpleDictionary.IMObjID;
                thisObj.Name = simpleDictionary.Name;
            };

            self.createComboBoxHelper = function (container_selector, getUrl, defaultValue, comboBoxFunc) {
                const thisObj = this;
                if (!comboBoxFunc)
                    comboBoxFunc = self.createComboBoxItem;
                //
                thisObj.SelectedItem = ko.observable(null);
                //
                thisObj.ItemList = ko.observableArray([]);
                thisObj.ItemListD = $.Deferred();
                thisObj.getItemList = function (options) {
                    const data = thisObj.ItemList();
                    options.callback({ data: data, total: data.length });
                };
                //
                thisObj.ajaxControl = new ajaxLib.control();
                thisObj.LoadList = function (data) {
                    thisObj.ajaxControl.Ajax($(container_selector),
                        {
                            url: getUrl,
                            method: 'GET',
                            data: data
                        },
                        function (response) {
                            if (response) {
                                thisObj.ItemList.removeAll();
                                //
                                $.each(response, function (index, simpleDictionary) {
                                    var u = new comboBoxFunc(simpleDictionary);
                                    thisObj.ItemList().push(u);

                                    if (u.ID == defaultValue) {
                                        thisObj.SelectedItem(u);
                                    }
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
                        let item = null;
                        if (id != undefined && id != null)
                            for (let i = 0; i < thisObj.ItemList().length; i++) {
                                const tmp = thisObj.ItemList()[i];
                                if (tmp.ID == id) {
                                    item = tmp;
                                    break;
                                }
                            }
                        thisObj.SelectedItem(item);
                    });
                };
            }

            // Call
            {
                self.Call = null;

                self.LoadCall = function () {
                    if (options && options.calls) {
                        const getCall = new ajaxLib.control();
                        getCall.Ajax(
                            null, {
                            url: `/api/calls/${options.calls[0]}`,
                            method: 'GET',
                            dataType: 'json',
                            contentType: 'application/json'
                        }, function (response) {
                            if (response != null) {
                                self.Call = response;
                                self.Name(self.Call.CallSummaryName);
                                self.Description(self.Call.Description);
                                if (self.PriorityID != null) {
                                    self.SetPriority();
                                }
                                if (self.Call.ServiceID != null && (self.Call.ServiceID != '00000000-0000-0000-0000-000000000000')) {
                                    self.ServiceSet(self.Call.ServiceID, `${self.Call.ServiceCategoryName} \\ ${self.Call.ServiceName}`, false);
                                    self.LoadService(self.Call.ServiceID);
                                }
                            }
                        });
                    }
                }
            }

            // Name / Summary / Short Description
            {
                self.Name = ko.observable('');
                //
                self.EditName = function () {
                    showSpinner();
                    const fh = new fhModule.formHelper(true);
                    const options = {
                        fieldName: 'MassIncident.Name',
                        fieldFriendlyName: getTextResource('Summary'),
                        oldValue: self.Name(),
                        onSave: function (newText) {
                            self.Name(newText);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                };
            }

            // Full Description
            {
                self.Description = ko.observable('');
                self.Description.subscribe(function (newValue) {
                    if (self.htmlDescriptionControl != null)
                        self.htmlDescriptionControl.SetHTML(newValue);
                });

                self.htmlDescriptionControlD = $.Deferred();
                self.htmlDescriptionControl = null;
                self.InitializeDescription = function () {
                    const htmlElement = $('#' + self.FormID).find('.fullDescription');
                    if (self.htmlDescriptionControl == null)
                        require(['htmlControl'], function (htmlLib) {
                            self.htmlDescriptionControl = new htmlLib.control(htmlElement);
                            self.htmlDescriptionControlD.resolve();
                            self.htmlDescriptionControl.OnHTMLChanged = function (htmlValue) {
                                self.Description(htmlValue);
                            };
                        });
                    else
                        self.htmlDescriptionControl.Load(htmlElement);
                };

                //
                self.EditDescription = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'MassIncident.Description',
                        fieldFriendlyName: getTextResource('MassIncident_Description'),
                        oldValue: self.Description(),
                        onSave: function (newHTML) {
                            self.Description(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
            }

            // cause
            {
                self.IsCauseContainerVisible = ko.observable(false);
                self.ToggleCauseContainer = function () {
                    self.IsCauseContainerVisible(!self.IsCauseContainerVisible());
                };
                //
                self.htmlCauseControl = null;
                self.InitializeCause = function () {
                    const htmlElement = $('#' + self.FormID).find('.cause');
                    if (self.htmlCauseControl == null)
                        require(['htmlControl'], function (htmlLib) {
                            self.htmlCauseControl = new htmlLib.control(htmlElement);
                            self.htmlCauseControl.OnHTMLChanged = function (htmlValue) {
                                self.Cause(htmlValue);
                            };
                        });
                    else
                        self.htmlCauseControl.Load(htmlElement);
                };
                //
                self.Cause = ko.observable('');
                self.Cause.subscribe(function (newValue) {
                    if (self.htmlCauseControl != null)
                        self.htmlCauseControl.SetHTML(newValue);
                });
                //
                self.EditCause = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'MassIncident.Cause',
                        fieldFriendlyName: getTextResource('MassIncident_Cause'),
                        oldValue: self.Cause(),
                        onSave: function (newHTML) {
                            self.Cause(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
            }

            // mass incident type
            {
                self.TypeSelectHelper = new self.createComboBoxHelper('#' + self.FormID + ' .type', '/api/massincidenttypes');

                self.CurrentTypeID = null;
                self.TypeSelectHelper.SelectedItem.subscribe(function (newValue) {
                    if (newValue && self.CurrentTypeID !== newValue.IMObjID) {
                        self.CurrentTypeID = newValue.IMObjID;
                        self.DynamicOptionsServiceInit(self.CurrentTypeID);
                    }
                });
            }

            // Info Channel
            {
                var defaultInfoChannelID = 3; //TODO: Добавить атрибут Default к каналу приема информации или системную настройку
                self.InformationChannelSelectHelper = new self.createComboBoxHelper('#' + self.FormID + ' .receiveСhannel', '/api/massincidentinformationchannels', defaultInfoChannelID);
            }

            // short cause
            {
                self.CauseSelectHelper = new self.createComboBoxHelper('#' + self.FormID + ' .shortCause', '/api/massIncidentCauses/');
            }

            //UI (areas visibility)
            {
                self.ServiceContainer = ko.observable(null);
                self.ParamsContainer = ko.observable(null);
                self.SolutionContainer = ko.observable(null);
                self.AdditionalServiceContainer = ko.observable(null);

                require(['models/SDForms/Shared/visibleContainerHandler'], function (module) {
                    self.ServiceContainer(module.InitContainer());
                    self.ParamsContainer(module.InitContainer());
                    self.AdditionalServiceContainer(module.InitContainer());
                    self.SolutionContainer(module.InitContainer());
                });
            }

            // Service
            {
                self.Service = null;
                self.ServiceItemAttendanceFullName = ko.observable('');
                self.ServiceID = null;
                self.serviceItemAttendanceSearcher = null;
                self.serviceItemAttendanceSearcherD = null;

                self.ServiceSet = function (serviceID, name, setSearcher) {
                    self.ServiceItemAttendanceFullName(name);
                    if (setSearcher != false) {
                        $.when(self.serviceItemAttendanceSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(name);
                        });
                    }
                    //
                    if (self.ServiceID != serviceID) {
                        if (self.techFailureCategory.id() != null &&
                            self.techFailureCategory.serviceReferences.filter(function (item) { item.ServiceID == serviceID }).length === 0) {
                            self.techFailureCategory.reset();
                        }
                        self.ServiceID = serviceID;
                        var matchingAdditionalServices = self.AdditionalServices().filter(function (item) {
                            return item.ServiceID === serviceID;
                        });
                        matchingAdditionalServices.forEach(function (item) {
                            self.AdditionalServices.remove(item);
                        });
                    }
                };

                self.ServiceClear = function () {
                    self.ServiceItemAttendanceFullName('');
                    //
                    self.ServiceID = null;
                };

                self.LoadService = function (serviceID) {
                    const getService = new ajaxLib.control();
                    getService.Ajax(
                        null, {
                        url: `/api/services/${serviceID}`,
                        method: 'GET',
                        dataType: 'json',
                        contentType: 'application/json'
                    }, function (response) {
                            if (response != null) {
                                self.Service = response;
                                self.SetCriticality();
                            }
                    });
                }
            }

            // AdditionalServices
            {
                self.AdditionalServices = ko.observableArray();

                self.AdditionalServicesSave = function (selectedItems) {
                    if (!selectedItems) {
                        return;
                    };

                    self.AdditionalServices.removeAll();
                    selectedItems.forEach(function (item) {
                        self.AdditionalServices.push({
                            Value: (item.parent ? item.parent.text() + ' \\ ' : '') +  item.text(),
                            ServiceID: item.id
                        });
                    });
                };

                self.AddAdditionalService = function () {
                    const fh = new fhModule.formHelper(true);
                    fh.ShowSelectService({
                        selected: self.AdditionalServices().map(function (x) { return x.ServiceID }),
                        excluded: self.ServiceID ? [ self.ServiceID ] : [],
                        onSave: self.AdditionalServicesSave
                    });
                };

                self.RemoveAdditionalService = function (item) {
                    self.AdditionalServices.remove(item);
                };
            }

            //
            //attachments
            {
                self.attachmentsControl = null;
                var fileIds = [];
                self.LoadAttachmentsControl = function () {
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsControl) {
                            self.attachmentsControl.RemoveUploadedFiles();  
                        };                  
                        
                        if (!self.attachmentsControl || !self.attachmentsControl.IsLoaded()) {
                            const attachmentsElement = $(`#${self.FormID}`).find('.documentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        };

                        self.attachmentsControl.ReadOnly(false);
                    });
                };
            }

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
                                const options = {
                                    UserID: self.InitiatorID,
                                    UserType: userLib.UserTypes.workOrderInitiator,
                                    UserName: null,
                                    EditAction: self.EditInitiator,
                                    RemoveAction: self.DeleteInitiator
                                };
                                const user = new userLib.User(self, options);
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
                        const fh = new fhModule.formHelper(true);
                        const options = {
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

            function userNotInGroup(groupID, executorID, callback) {
                new ajaxLib.control().Ajax(null, {
                    method: 'GET',
                    url: '/api/groups/' + groupID
                }, function (group) {
                    if (group.QueueUserList.filter(function (u) {
                        return u.IMObjID == executorID;
                    }).length === 0) {
                        callback();
                    }
                });
            }

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
                                const options = {
                                    UserID: self.QueueID,
                                    UserType: userLib.UserTypes.queueExecutor,
                                    UserName: self.QueueName,
                                    EditAction: self.EditQueue,
                                    RemoveAction: self.DeleteQueue,
                                    IsFreezeSelectedClient: true
                                };
                                const queue = new userLib.User(self, options);
                                self.Queue(queue);
                                self.QueueLoaded(true);
                            }
                            else
                                self.Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                        }
                    });
                };
                //
                self.EditQueue = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        const fh = new fhModule.formHelper(true);
                        const options = {
                            fieldName: 'MassIncident.Queue',
                            fieldFriendlyName: getTextResource('Queue'),
                            oldValue: self.QueueLoaded() ? { ID: self.Queue().ID(), ClassID: self.Queue().ClassID(), FullName: self.Queue().FullName() } : null,
                            object: ko.toJS(self.Queue()),
                            searcherName: "QueueSearcher",
                            searcherPlaceholder: getTextResource('EnterQueue'),
                            searcherParams: { Type: 4 },
                            onSave: function (objectInfo) {
                                self.QueueLoaded(false);
                                self.Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                                //
                                if (objectInfo && objectInfo.ClassID == 722) { //IMSystem.Global.OBJ_QUEUE
                                    self.QueueID = objectInfo.ID;
                                    self.QueueName = objectInfo.FullName;

                                    if (self.ExecutorID) {
                                        userNotInGroup(self.QueueID, self.ExecutorID, self.DeleteExecutor);
                                    }
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
                                const options = {
                                    UserID: self.OwnerID,
                                    UserType: userLib.UserTypes.owner,
                                    UserName: null,
                                    EditAction: self.EditOwner,
                                    RemoveAction: self.DeleteOwner
                                };
                                const user = new userLib.User(self, options);
                                self.Owner(user);
                                self.OwnerLoaded(true);
                            }
                            else
                                self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                        }
                    });
                };
                self.EditOwner = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        const fh = new fhModule.formHelper(true);
                        const options = {
                            fieldName: 'MassIncident.Owner',
                            fieldFriendlyName: getTextResource('Owner'),
                            oldValue: self.OwnerLoaded() ? { ID: self.Owner().ID(), ClassID: 9, FullName: self.Owner().FullName() } : null,
                            object: ko.toJS(self.Owner()),
                            searcherName: 'MassIncidentOwnerSearcher',
                            searcherParams: {},
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            onSave: function (objectInfo) {
                                self.OwnerLoaded(false);
                                self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                                self.OwnerID = objectInfo ? objectInfo.ID : null;
                                //
                                self.InitializeOwner();
                            },
                            nosave: true,
                            newSearch: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.DeleteOwner = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.OwnerLoaded(false);
                        self.OwnerID = null;
                        self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                    });
                };
            }

            //executor
            {               
                self.ExecutorID = null;
                self.ExecutorLoaded = ko.observable(false);
                self.Executor = ko.observable(null);

                function emptyExecutor(userLib) {
                    return new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor)
                }

                self.InitExecutor = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.Executor(emptyExecutor(userLib));
                    });
                }
                //
                self.EditExecutor = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        const fh = new fhModule.formHelper(true);
                        const options = {
                            fieldName: 'MassIncident.Executor',
                            fieldFriendlyName: getTextResource('Executor'),
                            oldValue: self.ExecutorLoaded() ? { ID: self.Executor().ID(), ClassID: 9, FullName: self.Executor().FullName() } : null,
                            object: ko.toJS(self.Executor()),
                            searcherName: 'ExecutorUserSearcher',
                            searcherParams: { QueueId: self.QueueID },
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            onSave: function (objectInfo) {
                                self.ExecutorLoaded(false);
                                self.Executor(emptyExecutor(userLib));
                                self.ExecutorID = objectInfo ? objectInfo.ID : null;

                                if (self.ExecutorID) {
                                    const options = {
                                        UserID: self.ExecutorID,
                                        UserType: userLib.UserTypes.executor,
                                        UserName: objectInfo.Name,
                                        EditAction: self.EditExecutor,
                                        RemoveAction: self.DeleteExecutor
                                    };
                                    const user = new userLib.User(self, options);
                                    self.Executor(user);
                                    self.ExecutorLoaded(true);
                                }
                            },
                            nosave: true,
                            newSearch: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.DeleteExecutor= function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.ExecutorLoaded(false);
                        self.ExecutorID = null;
                        self.Executor(emptyExecutor(userLib));
                    });
                };
            }

            //for all user controls
            {
                self.CanEdit = ko.computed(function () {
                    return true;
                });
            }

            // validate and save
            {
                self.ShowError = function (propertyResourceName) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource(propertyResourceName));
                    });
                };

                self.ValidateAndSave = function () {
                    const retD = $.Deferred();
                    const postMassIncident = new ajaxLib.control();

                    const data = {
                        Name: self.Name(),
                        ServiceID: self.ServiceID,
                        AffectedServiceIDs: self.AdditionalServices().map(function (item) { return item.ServiceID }),
                        TypeID: self.TypeSelectHelper.SelectedItem() == null ? null : self.TypeSelectHelper.SelectedItem().ID,
                        InformationChannelID: self.InformationChannelSelectHelper.SelectedItem() == null ? null : self.InformationChannelSelectHelper.SelectedItem().ID,
                        CreatedByUserID: self.InitiatorID,
                        OwnedByUserID: self.OwnerID,
                        Description: self.Description(),                        
                        CauseID: self.CauseSelectHelper.SelectedItem() == null ? null : self.CauseSelectHelper.SelectedItem().ID,
                        Cause: self.Cause(),
                        Solution: self.Solution(),
                        ExecutedByGroupID: self.QueueID,
                        ExecutedByUserID: self.ExecutorID,
                        PriorityID: self.PriorityID,
                        CriticalityID: self.CriticalityID,
                        TechnicalFailureCategoryID: self.techFailureCategory.id(),
                        Calls: options ? options.calls || [] : [],
                        DocumentIds: self.attachmentsControl.Items().map(function (file) { return file.ID; }),
                        FormValuesData: self.DynamicOptionsService.SendByServer()
                    };

                    if (!data.Name || !data.Name.trim().length) {
                        retD.resolve(null);
                        return self.ShowError('PromptMassiveIncidentSummary');
                    };

                    if (data.ServiceID == null) {
                        retD.resolve(null);
                        return self.ShowError('PromptMassiveIncidentService');
                    };

                    if (data.TypeID == null) {
                        retD.resolve(null);
                        return self.ShowError('PromtMassiveIncidentType');
                    };

                    if (data.InformationChannelID == null) {
                        retD.resolve(null);
                        return self.ShowError('PromtMassiveIncidentInformationChannel');
                    };

                    if (data.FormValuesData === null && self.DynamicOptionsService.IsInit()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ParametersNotLoaded'));
                        });
                        retD.resolve(null);
                        return;
                    }

                    if (data.FormValuesData && data.FormValuesData.hasOwnProperty('valid')) {
                        data.FormValuesData.callBack();
                        retD.resolve(null);
                        return;
                    }

                    postMassIncident.Ajax(
                        $('#' + self.FormID), {
                        url: '/api/massincidents',
                        method: 'POST',
                        dataType: 'json',
                        contentType: 'application/json',
                        data: JSON.stringify(data)
                    }, function (response) {
                            retD.resolve({ ID: response.ID, IMObjID: response.IMObjID });
                    }, function () {
                        retD.resolve(null);
                    });

                    return retD.promise();
                };
            }

            // priority
            {
                self.PriorityID = null;
                self.PriorityName = ko.observable(getTextResource('PromptPriority'));
                self.PriorityColor = ko.observable('');

                self.priorityControl = null;

                self.RefreshPriority = function (priorityObj) {
                    if (priorityObj == null)
                        return;
                    
                    self.PriorityName(priorityObj.Name);
                    self.PriorityColor(priorityObj.Color);
                    self.PriorityID = priorityObj.ID;
                };

                self.LoadPriorityControl = function () {
                    require(['models/SDForms/Shared/dropDownMenu'], function (prLib) {
                        if (!self.priorityControl) {
                            self.priorityControl = new prLib.ViewModel(
                                $(`#${self.FormID}`).find('[data-drop-down-wrapper-priority]'),
                                false,
                                '\'SDForms/Controls/PriorityList\'',
                                self.RefreshPriority
                            );
                            self.priorityControl.Initialize();
                        };
                        $.when(self.priorityControl.Load(null, 823, null, '/api/priorities'))
                            .done(self.SetPriority);
                    });
                };

                self.SetPriority = function () {
                    ko.utils.arrayForEach(self.priorityControl.MenuItemsList(), function (elem) {
                        if (self.PriorityID) {
                            if(elem.ID === self.PriorityID) {
                                self.priorityControl.SelectItem(elem);
                            }                                
                        } else if (self.Call != null) {
                            if (elem.ID === self.Call.PriorityID) {
                                self.priorityControl.SelectItem(elem);
                            }
                        } else {
                            if (elem.Default) {
                                self.priorityControl.SelectItem(elem);
                            }
                        }
                    });
                }
            }

            // criticality
            {
                self.CriticalityID = null;
                self.CriticalityName = ko.observable(getTextResource('Criticality'));

                self.criticalityControl = null;

                self.RefreshCriticality = function (criticalityObj) {
                    if (criticalityObj == null) {
                        return;  
                    };
                        
                    self.CriticalityName(criticalityObj.Name);
                    self.CriticalityID = criticalityObj.ID;
                };

                self.LoadCriticalityControl = function () {
                    require(['models/SDForms/Shared/dropDownMenu'], function (prLib) {
                        if (!self.criticalityControl) {

                            self.criticalityControl = new prLib.ViewModel(
                                $(`#${self.FormID}`).find('[data-drop-down-wrapper-criticality]'),
                                false,
                                '\'SDForms/Controls/CriticalityList\'',
                                self.RefreshCriticality
                            );

                            self.criticalityControl.Initialize();
                        }
                        $.when(self.criticalityControl.Load(null, 823, null, '/api/criticalities'))
                            .done(self.SetCriticality);
                    });
                };

                self.SetCriticality = function () {
                    ko.utils.arrayForEach(self.criticalityControl.MenuItemsList(), function (elem) {
                        if (self.CriticalityID && elem.ID === self.CriticalityID) {
                            self.criticalityControl.SelectItem(elem);
                        } else if (self.Service != null && elem.ID === self.Service.CriticalityID) {                            
                            self.criticalityControl.SelectItem(elem);                            
                        }
                    })
                }
            }

            // solution
            {
                self.htmlSolutionControl = null;
                self.InitializeSolution = function () {
                    const htmlElement = $('#' + self.FormID).find('.solution');
                    if (self.htmlSolutionControl == null)
                        require(['htmlControl'], function (htmlLib) {
                            self.htmlSolutionControl = new htmlLib.control(htmlElement);
                            self.htmlSolutionControl.OnHTMLChanged = function (htmlValue) {
                                self.Solution(htmlValue);
                            };
                        });
                    else
                        self.htmlSolutionControl.Load(htmlElement);
                };
                //
                self.Solution = ko.observable('');
                self.Solution.subscribe(function (newValue) {
                    if (self.htmlSolutionControl != null)
                        self.htmlSolutionControl.SetHTML(newValue);
                });
                //
                self.EditSolution = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'MassIncident.Solution',
                        fieldFriendlyName: getTextResource('MassIncident_Solution'),
                        oldValue: self.Solution(),
                        onSave: function (newHTML) {
                            self.Solution(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
            }

            self.AddAs = false;//проблема создана по аналогии
            self.miData = null;//проблема, взятая за основу

            self.Fill = function (miData) {
                if (miData.Name()) self.Name(miData.Name());

                if (miData.CreatedByUserID) {
                    self.InitiatorID = miData.CreatedByUserID();
                }                

                if (miData.TypeID) {
                    self.TypeSelectHelper.LoadList();
                    self.TypeSelectHelper.SetSelectedItem(miData.TypeID());
                }
                if (miData.ServiceID) {
                    self.ServiceID = miData.ServiceID();
                }
                if (miData.ServiceID && miData.ServiceUri) 
                    loadMainService(miData.ServiceUri());

                if (miData.CriticalityID)
                    self.CriticalityID = miData.CriticalityID();

                if (miData.PriorityID) 
                    self.PriorityID = miData.PriorityID();

                if (miData.Description) 
                    $.when(self.htmlDescriptionControlD).done(function () {
                    self.Description(miData.Description());
                });

                if (miData.InformationChannelID) {
                    self.InformationChannelSelectHelper.LoadList();
                    self.InformationChannelSelectHelper.SetSelectedItem(miData.InformationChannelID());
                }
                
                if (miData.TechnicalFailureCategoryID && miData.TechnicalFailureCategoryUri) {
                    self.TechnicalFailureCategoryUri = miData.TechnicalFailureCategoryUri();
                    loadTechnicalFailureCategory(self.TechnicalFailureCategoryUri);
                }
                loadAdditionalServices(miData.ID());
                self.miData = miData;
            }

            function loadTechnicalFailureCategory(uri) {
                sendGetRequest(uri,
                function (result) {
                    self.techFailureCategory.id(result.ID);
                    self.techFailureCategory.name(result.Name);
                    self.techFailureCategory.serviceReferences = result.ServiceReferences;
                });
            }

            function loadMainService(serviceUri) {
                sendGetRequest(serviceUri, function (service) {
                    sendGetRequest(service.CategoryUri, function (serviceCategory) {
                        self.ServiceItemAttendanceFullName(serviceCategory.Name + ' \\ ' + service.Name);
                    });
                });
            }
            
            function loadAdditionalServices(id) {
                let url = '/api/massIncidents/' + id + '/affectedServices';
                sendGetRequest(url, function (result) {
                    result.forEach(function (item) {
                        sendGetRequest(item.ServiceUri, function (service) {
                            sendGetRequest(service.CategoryUri, function (serviceCategory) {
                                self.AdditionalServices.push({
                                    Value: serviceCategory.Name + ' \\ ' + service.Name, 
                                    ServiceID: service.ID
                                });
                            });
                        });
                    });
                });
            }

            function sendGetRequest(url, callback) {
                new ajaxLib.control().Ajax(null, {
                    method: 'GET', url: url
                }, callback)
            }
        }
    }

    return module;
});