define([
    'knockout',
    'jquery',
    'usualForms',
    'ajax',
    'dynamicOptionsService',
    'parametersControl',
    'comboBox'
], function (ko, $, fhModule, ajaxLib, dynamicOptionsService, pcLib) {
    var module = {
        Classes: {
            User: 9,
            Queue: 722
        },

        ViewModel: function (mainRegionID, objectClassID, objectID, dependencyList) {
            var self = this;
            //
            //outer actions
            self.frm = undefined;
            //
            //vars
            self.formClassID = 702;//OBJ_PROBLEM
            self.MainRegionID = mainRegionID;
            self.ID = null;//for registered problem
            self.renderedD = $.Deferred();
            //
            self.ObjectClassID = objectClassID;//объект, с которым связана проблема
            self.ObjectID = objectID;//объект, с которым связана проблема
            //
            //tab pages
            {
                self.modes = {
                    nothing: 'nothing',
                    main: 'main',
                    parameterPrefix: 'parameter_'
                };
                //
                self.mode = ko.observable(self.modes.nothing);

                self.SetTab = function (template) {
                    self.mode(`${self.modes.parameterPrefix}${template.Tab.ID}`);
                };

                self.DynamicOptionsService = new dynamicOptionsService.ViewModel(self.MainRegionID, {
                    typeForm: 'Create',
                    changeTabCb: function (invalidInput, tab) {
                        const parent = invalidInput.closest('[data-prefix-mode]');
                        const tabPrefix = parent.attr('data-prefix-mode');
                        self.mode(tabPrefix);
                    }
                });

                self.DynamicOptionsServiceInit = function (id) {
                    if (self.DynamicOptionsService.IsInit()) {
                        self.DynamicOptionsService.ResetData();
                    };

                    self.DynamicOptionsService.GetTemplate(dynamicOptionsService.Classes.ProblemType, id);
                };
            }
            //User Fields
            {
                self.UserFieldType = 4;
                self.IsUserFieldsContainerVisible = ko.observable(false);
                self.ToggleUserFieldsContainer = function () {
                    self.IsUserFieldsContainerVisible(!self.IsUserFieldsContainerVisible());
                };
                self.UserFields = ko.observable(null);
                //
                self.InitializeUserFields = function () {
                    require(['models/SDForms/SDForm.UserFields'], function (ufLib) {
                        self.UserFields(new ufLib.UserFields(self.UserFieldType));
                        $.when(self.UserFields().Initialize()).done(function () {
                            self.UserFieldsLoaded(true);
                            self.UserFields().ReadOnly(false);
                            self.UserFieldsLoaded(true);
                        });
                    });
                };
                self.UserFieldsLoaded = ko.observable(false);
            }
            //          
            //priority / influence / urgency
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
            //description
            {
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
                            self.htmlDescriptionControlD.resolve();
                            self.htmlDescriptionControl.OnHTMLChanged = function (htmlValue) {
                                self.Description(htmlValue);
                            };
                        });
                    else
                        self.htmlDescriptionControl.Load(htmlElement);
                };
                //
                self.Description = ko.observable('');
                self.Description.subscribe(function (newValue) {
                    if (self.htmlDescriptionControl != null)
                        self.htmlDescriptionControl.SetHTML(newValue);
                });
                //
                self.EditDescription = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Problem.HTMLDescription',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: self.Description(),
                        onSave: function (newHTML) {
                            self.Description(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
            }
            //
            //attachments
            {
                self.attachmentsControl = null;
                self.LoadAttachmentsControl = function () {
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsControl != null)
                            self.attachmentsControl.RemoveUploadedFiles();//previous object                   
                        //
                        if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                            var attachmentsElement = $('#' + self.MainRegionID).find('.documentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        };
                    })
                };
            }
            //
            //for comboBox
            {
                self.createComboBoxItem = function (simpleDictionary) {
                    var thisObj = this;
                    //
                    thisObj.ID = simpleDictionary.ID;
                    if (simpleDictionary.FullName)
                        thisObj.Name = (simpleDictionary.FullName || '').replaceAll('\\', ' \\ ');
                    else
                        thisObj.Name = (simpleDictionary.Name || '').replaceAll('\\', ' \\ ');
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
            //problemType
            {
                self.ProblemTypeHelper = new self.createComboBoxHelper('#' + self.MainRegionID + ' .callType', '/api/problemtypes');
                self.EditProblemType = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Problem.ProblemType',
                        fieldFriendlyName: getTextResource('ProblemType'),
                        oldValue: self.ProblemTypeHelper.GetObjectInfo(136),
                        searcherName: 'ProblemTypeSearcher',
                        searcherPlaceholder: getTextResource('PromptType'),
                        onSave: function (objectInfo) {
                            self.ProblemTypeHelper.SetSelectedItem(objectInfo ? objectInfo.ID : null);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };

                self.CurrentProblemTypeSelectedItemID = null;
                self.ProblemTypeHelper.SelectedItem.subscribe(function (newValue) {
                    if (newValue && self.CurrentProblemTypeSelectedItemID !== newValue.ID) {
                        self.CurrentProblemTypeSelectedItemID = newValue.ID;
                        self.DynamicOptionsServiceInit(self.CurrentProblemTypeSelectedItemID);
                    };
                });
            }
            //

            //service
            {
                self.ServiceID = null;
                self.ServiceItemAttendanceFullName = ko.observable('');
                //
                self.ServiceClear = function () {
                    self.ServiceID = null;
                    self.ServiceItemAttendanceFullName('');
                };

                self.ServiceSet = function (serviceID, fullName) {
                    self.ServiceID = serviceID;
                    self.ServiceItemAttendanceFullName(fullName);
                };

                self.serviceItemAttendanceSearcher = null;
                self.serviceItemAttendanceSearcherD = $.Deferred();

                self.ajaxControl_ServiceItemAttendance = new ajaxLib.control();
                self.InitializeServiceSearcher = function () {
                    const fh = new fhModule.formHelper();

                    const serviceSearcherExtensionLoadedD = $.Deferred();
                    const serviceD = fh.SetTextSearcherToField(
                        $('#' + self.MainRegionID).find('.serviceItemAttendance'),
                        'ServiceSearcher',
                        'SearchControl/SearchServiceItemAttendanceControl',
                        { Types: [0, 1] },
                        function (objectInfo) {//select        
                            //
                            self.ServiceSet(objectInfo.ID, objectInfo.FullName);
                        },
                        function () {//reset
                            self.ServiceClear();
                        },
                        function (selectedItem) {//close
                            if (!selectedItem) {
                                self.ServiceClear();
                            };
                        },
                        serviceSearcherExtensionLoadedD,
                        false);

                    $.when(serviceD).done(function (vm) {//after load searcher
                        self.serviceItemAttendanceSearcher = vm;

                        self.serviceItemAttendanceSearcher.resetParameters = function () {
                            self.serviceItemAttendanceSearcher.SetSearchParameters(getParameters());
                            self.serviceItemAttendanceSearcher.ClearValues();
                        };

                        self.serviceItemAttendanceSearcherD.resolve(vm);

                        vm.SelectFromServiceCatalogueClick = function () {
                            vm.Close();//close dropDownMenu

                            const mode = fh.ServiceCatalogueBrowserMode;
                            const result = fh.ShowServiceCatalogueBrowser(mode, null, self.ServiceID, 'OnlyService', [0, 1]);

                            $.when(result).done(function (serviceItem) {
                                if (serviceItem) {
                                    self.ServiceSet(
                                        serviceItem.ID,
                                        serviceItem.Name,
                                    );
                                };
                            });
                        };
                        //
                        vm.HardToChooseClick = function () { };
                        vm.HardToChooseClickVisible = ko.observable(false);
                        //      
                        serviceSearcherExtensionLoadedD.resolve();
                        $.when(vm.LoadD).done(function () {
                            $('#' + vm.searcherDivID).find('.ui-dialog-buttonset').css({ opacity: 1 });
                        });
                    });
                };

                self.EditServiceItemAttendance = function () {
                    const extensionLoadD = $.Deferred();

                    showSpinner();

                    const fh = new fhModule.formHelper(true);
                    const options = {
                        fieldName: 'ProblemService',
                        fieldFriendlyName: getTextResource('Service'),
                        oldValue: {
                            ID: self.ServiceID,
                            ClassID: self.objectClassID,
                            FullName: self.ServiceItemAttendanceFullName(),
                        },
                        searcherName: 'ServiceSearcher',
                        searcherTemplateName: 'SearchControl/SearchServiceItemAttendanceControl',//иной шаблон, дополненительные кнопки
                        searcherParams: { Types: [0, 1] },
                        searcherLoadD: extensionLoadD,//ожидание дополнений для модели искалки
                        searcherPlaceholder: getTextResource('PromptServiceItem'),
                        searcherLoad: function (vm, setObjectInfoFunc) {//дополнения искалки
                            vm.CurrentUserID = null;//для фильтрации данных по доступности их пользователю (клиенту)
                            vm.SelectFromServiceCatalogueClick = function () {//кнопка выбора из каталога сервисов
                                vm.Close();//close dropDownMenu

                                showSpinner();

                                const mode = fh.ServiceCatalogueBrowserMode;
                                const result = fh.ShowServiceCatalogueBrowser(mode, null, self.ServiceID, 'OnlyService', [0, 1]);

                                $.when(result).done(function (serviceItem) {
                                    if (serviceItem && setObjectInfoFunc) {
                                        setObjectInfoFunc({
                                            ID: serviceItem.ID,
                                            FullName: serviceItem.Name,
                                        });
                                    };
                                });
                            };

                            vm.HardToChooseClick = function () { };//кнопка затрудняюсь выбрать
                            vm.HardToChooseClickVisible = ko.observable(false);

                            extensionLoadD.resolve();//можно рендерить искалку, формирование модели окончено
                            $.when(vm.LoadD).done(function () {
                                $('#' + vm.searcherDivID).find('.ui-dialog-buttonset').css({ opacity: 1 });//show buttons
                            });
                        },
                        onSave: function (objectInfo) {
                            if (objectInfo) {
                                self.ServiceSet(objectInfo.ID, objectInfo.FullName);
                            } else {
                                self.ServiceSet(null, '');
                            };
                        },
                        nosave: true,
                        newSearch: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }

            //for all user controls
            {
                self.CanEdit = ko.computed(function () {
                    return true;
                });
            }
            //

            //initiator
            {
                self.Initiator = ko.observable(null);
                self.InitiatorID = null;
                self.InitiatorLoaded = ko.observable(false);
                //
                self.InitializeInitiator = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.InitiatorLoaded()) {
                            return;
                        };

                        if (self.InitiatorID) {
                            const options = {
                                UserID: self.InitiatorID,
                                UserType: userLib.UserTypes.workInitiator,
                                UserName: null,
                                EditAction: self.EditInitiator,
                                RemoveAction: self.DeleteInitiator
                            };

                            const user = new userLib.User(self, options);
                            self.Initiator(user);
                            self.InitiatorLoaded(true);
                        }
                        else {
                            self.Initiator(new userLib.EmptyUser(self, userLib.UserTypes.workInitiator, self.EditInitiator));
                        };
                    });
                };

                self.EditInitiator = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        const fh = new fhModule.formHelper(true);
                        const options = {
                            fieldName: 'ProblemInitiator',
                            fieldFriendlyName: getTextResource('InitiatorReally'),
                            oldValue: self.InitiatorLoaded() ? { ID: self.Initiator().ID(), ClassID: 9, FullName: self.Initiator().FullName() } : null,
                            object: ko.toJS(self.Initiator()),
                            searcherName: 'WebUserSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            onSave: function (objectInfo) {
                                self.InitiatorLoaded(false);
                                self.Initiator(new userLib.EmptyUser(self, userLib.UserTypes.workInitiator, self.EditInitiator));
                                self.InitiatorID = objectInfo ? objectInfo.ID : null;
                                //
                                self.InitializeInitiator();
                            },
                            nosave: true,
                            newSearch: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                self.DeleteInitiator = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.InitiatorLoaded(false);
                        self.InitiatorID = null;
                        self.Initiator(new userLib.EmptyUser(self, userLib.UserTypes.workInitiator, self.EditInitiator));
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
                                var options = {
                                    UserID: self.OwnerID,
                                    UserType: userLib.UserTypes.owner,
                                    UserName: null,
                                    EditAction: self.EditOwner,
                                    RemoveAction: self.DeleteOwner
                                };
                                var user = new userLib.User(self, options);
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
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            fieldName: 'Problem.Owner',
                            fieldFriendlyName: getTextResource('Owner'),
                            oldValue: self.OwnerLoaded() ? { ID: self.Owner().ID(), ClassID: 9, FullName: self.Owner().FullName() } : null,
                            object: ko.toJS(self.Owner()),
                            searcherName: 'OwnerUserSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            onSave: function (objectInfo) {
                                self.OwnerLoaded(false);
                                self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
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
                        self.Owner(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditOwner));
                    });
                };
            }
            //

            //queue
            {
                self.Queue = ko.observable(null);
                self.QueueID = null;
                self.QueueLoaded = ko.observable(false);
                self.QueueName = null;

                self.InitializeQueue = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.QueueLoaded()) {
                            return;
                        };

                        if (self.QueueID) {
                            const options = {
                                UserID: self.QueueID,
                                UserType: userLib.UserTypes.queueExecutor,
                                UserName: self.QueueName,
                                EditAction: self.EditQueue,
                                RemoveAction: self.DeleteQueue,
                                IsFreezeSelectedClient: true
                            };
                            self.Queue(new userLib.User(self, options));
                            self.QueueLoaded(true);
                        }
                        else {
                            self.Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                        };
                    });
                };

                self.EditQueue = function () {
                    showSpinner();
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        const fh = new fhModule.formHelper(true);
                        const options = {
                            fieldName: 'Call.Queue',
                            fieldFriendlyName: getTextResource('Queue'),
                            oldValue: self.QueueLoaded() ? { ID: self.Queue().ID(), ClassID: self.Queue().ClassID(), FullName: self.Queue().FullName() } : null,
                            object: ko.toJS(self.Queue()),
                            searcherName: "QueueSearcher",
                            searcherPlaceholder: getTextResource('EnterQueue'),
                            searcherParams: { Type: 8 },
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
                                self.SetQueue(self.QueueID, self.QueueName, self.ExecutorID);
                            },
                            nosave: true,
                            newSearch: true
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

            //executor
            {
                self.ExecutorLoaded = ko.observable(false);
                self.ExecutorID = null;
                self.Executor = ko.observable(null);
                //
                self.InitializeExecutor = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.ExecutorLoaded()) {
                            return;
                        };

                        if (self.ExecutorID) {
                            const options = {
                                UserID: self.ExecutorID,
                                UserType: userLib.UserTypes.executor,
                                UserName: null,
                                EditAction: self.EditExecutor,
                                RemoveAction: self.DeleteExecutor
                            };

                            const user = new userLib.User(self, options);
                            self.Executor(user);
                            self.ExecutorLoaded(true);
                        } else {
                            self.Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                        };
                    });
                };

                self.EditExecutor = function () {
                    require(['usualForms'], function (module) {
                        showSpinner();
                        require(['models/SDForms/SDForm.User'], function (userLib) {
                            const fh = new fhModule.formHelper(true);
                            const options = {
                                fieldName: 'Problem.Executor',
                                fieldFriendlyName: getTextResource('Executor'),
                                oldValue: self.ExecutorLoaded() ? { ID: self.Executor().ID(), ClassID: 9, FullName: self.Executor().FullName() } : null,
                                object: ko.toJS(self.Executor()),
                                searcherName: 'ExecutorUserSearcher',
                                searcherPlaceholder: getTextResource('EnterFIO'),
                                searcherParams: { QueueId: self.QueueID },
                                onSave: function (objectInfo) {
                                    self.ExecutorLoaded(false);
                                    self.Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                                    self.ExecutorID = objectInfo ? objectInfo.ID : null;
                                    //
                                    self.InitializeExecutor();
                                },
                                nosave: true,
                                newSearch: true
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    });
                };

                self.DeleteExecutor = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.ExecutorLoaded(false);
                        self.ExecutorID = null;
                        self.Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                    });
                };

                const getQueue = function (queueID) {
                    let queueD = $.Deferred();
                    const ajaxControl = new ajaxLib.control();
                    ajaxControl.Ajax(null,
                        {
                            dataType: 'json',
                            contentType: 'application/json',
                            method: 'GET',
                            url: '/api/groups/' + queueID,
                        },
                        function (response) { queueD.resolve(response); },
                        function () { queueD.resolve(); }
                    );
                    return queueD;
                };

                self.SetQueue = function (queueID, queueName, executorID) {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.QueueLoaded(false);
                        self.Queue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditQueue));
                        self.QueueID = queueID;
                        self.QueueName = queueName;
                        self.InitializeQueue();
                    });
                    if (!executorID) {
                        return false;
                    }
                    $.when(getQueue(queueID)).done(function (queue) {
                        if (queue && queue.QueueUserList) {
                            const queueUsers = queue.QueueUserList.map(i => i.ID);
                            if (!queueUsers.includes(executorID)) {
                                self.DeleteExecutor();
                            }
                        }
                    });
                };
                self.SetExecutor = function (executorID, queueID) {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.ExecutorLoaded(false);
                        self.Executor(new userLib.EmptyUser(self, userLib.UserTypes.executor, self.EditExecutor));
                        self.ExecutorID = executorID;
                        self.InitializeExecutor();
                    });
                    if (!queueID) {
                        return false;
                    }
                    $.when(getQueue(queueID)).done(function (queue) {
                        if (queue && queue.QueueUserList) {
                            const queueUsers = queue.QueueUserList.map(i => i.ID);
                            if (!queueUsers.includes(executorID)) {
                                self.DeleteQueue();
                            }
                        }
                    });
                }

                self.SaveExecutorOrQueue = function (obj) {
                    if (!obj) {
                        return false;
                    }

                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (obj.ClassID === module.Classes.Queue) {
                            self.SetQueue(obj.ID, obj.FullName, self.ExecutorID);
                        }
                        else if (obj.ClassID === module.Classes.User) {
                            self.SetExecutor(obj.ID, self.QueueID);
                        }
                    });
                }
            }

            //shortCause + cause
            {
                self.IsCauseContainerVisible = ko.observable(false);
                self.ToggleCauseContainer = function () {
                    self.IsCauseContainerVisible(!self.IsCauseContainerVisible());
                };
                //
                self.ProblemCauseHelper = new self.createComboBoxHelper('#' + self.MainRegionID + ' .shortCause', '/api/problemcauses');
                self.EditShortCause = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Problem.ProblemCause',
                        fieldFriendlyName: getTextResource('ShortCause'),
                        oldValue: self.ProblemCauseHelper.GetObjectInfo(709),
                        searcherName: 'ProblemCauseSearcher',
                        searcherPlaceholder: getTextResource('EnterProblemCause'),
                        onSave: function (objectInfo) {
                            self.ProblemCauseHelper.SetSelectedItem(objectInfo ? objectInfo.ID : null);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
                //
                self.htmlCauseControl = null;
                self.InitializeCause = function () {
                    var htmlElement = $('#' + self.MainRegionID).find('.cause');
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
                        fieldName: 'Problem.Cause',
                        fieldFriendlyName: getTextResource('Cause'),
                        oldValue: self.Cause(),
                        onSave: function (newHTML) {
                            self.Cause(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
            }
            //
            //solution + fix
            {
                self.IsSolutionContainerVisible = ko.observable(false);
                self.ToggleSolutionContainer = function () {
                    self.IsSolutionContainerVisible(!self.IsSolutionContainerVisible());
                };
                //
                self.htmlSolutionControl = null;
                self.InitializeSolution = function () {
                    var htmlElement = $('#' + self.MainRegionID).find('.solution');
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
                        fieldName: 'Problem.Solution',
                        fieldFriendlyName: getTextResource('Solution'),
                        oldValue: self.Solution(),
                        onSave: function (newHTML) {
                            self.Solution(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
                //
                self.htmlFixControl = null;
                self.InitializeFix = function () {
                    var htmlElement = $('#' + self.MainRegionID).find('.fix');
                    if (self.htmlFixControl == null)
                        require(['htmlControl'], function (htmlLib) {
                            self.htmlFixControl = new htmlLib.control(htmlElement);
                            self.htmlFixControl.OnHTMLChanged = function (htmlValue) {
                                self.Fix(htmlValue);
                            };
                        });
                    else
                        self.htmlFixControl.Load(htmlElement);
                };
                //
                self.Fix = ko.observable('');
                self.Fix.subscribe(function (newValue) {
                    if (self.htmlFixControl != null)
                        self.htmlFixControl.SetHTML(newValue);
                });
                //
                self.EditFix = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Problem.Fix',
                        fieldFriendlyName: getTextResource('Fix'),
                        oldValue: self.Fix(),
                        onSave: function (newHTML) {
                            self.Fix(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                };
            }
            //
            self.Load = function () {
                self.mode(self.modes.main);
                //
                $.when(self.renderedD).done(function () {
                    $('#' + self.MainRegionID).find('.firstFocus').focus();
                    //
                    if (self.AddAs || self.CallList || self.MassIncidentId)
                        self.Fill(self.problemData);
                    //
                    self.LoadPriorityControl();
                    //
                    self.InitializeUserFields();
                    //
                    self.InitializeDescription();
                    self.InitializeCause();
                    self.LoadAttachmentsControl();
                    //
                    self.InitializeSolution();
                    self.InitializeFix();
                    //
                    if (!self.AddAs) {
                        self.ProblemTypeHelper.LoadList();
                        self.ProblemTypeHelper.SetSelectedItem('00000000-0000-0000-0000-000000000000');//problem
                    }
                    //
                    self.ProblemCauseHelper.LoadList();
                    //
                    self.InitializeExecutor();
                    self.InitializeOwner();
                    self.InitializeQueue();
                    self.InitializeInitiator();

                    self.InitializeServiceSearcher();
                });
            };
            //
            self.AfterRender = function () {
                self.renderedD.resolve();
            };
            //                                         
            self.IsRegisteringProblem = false;
            //
            self.ajaxControl_RegisterProblem = new ajaxLib.control();
            self.ValidateAndRegisterProblem = function (showSuccessMessage) {
                var retval = $.Deferred();
                //
                if (self.IsRegisteringProblem)
                    return;
                //
                const data = {
                    'TypeID': self.ProblemTypeHelper.SelectedItem() == null ? null : self.ProblemTypeHelper.SelectedItem().ID,
                    'UrgencyID': self.UrgencyID == '00000000-0000-0000-0000-000000000000' ? null : self.UrgencyID,
                    'PriorityID': self.PriorityID == '00000000-0000-0000-0000-000000000000' ? null : self.PriorityID,
                    'InfluenceID': self.InfluenceID == '00000000-0000-0000-0000-000000000000' ? null : self.InfluenceID,
                    'Summary': self.Summary(),
                    'ProblemCauseID': self.ProblemCauseHelper.SelectedItem() == null ? null : self.ProblemCauseHelper.SelectedItem().ID,
                    'ProblemCauseName': self.ProblemCauseHelper.SelectedItem() == null ? null : self.ProblemCauseHelper.SelectedItem().Name,
                    'HTMLDescription': self.Description(),
                    'HTMLCause': self.Cause(),
                    'HTMLSolution': self.Solution(),
                    'HTMLFix': self.Fix(),
                    'ServiceID': self.ServiceID,
                    'InitiatorID': self.InitiatorID, // Id инициатора заявки
                    'ExecutorID': self.ExecutorID, // Id исполнителя заявки
                    'QueueID': self.QueueID, // ID группы заявки
                    'OwnerID': self.OwnerID, // ID владельца заявки
                    'Files': self.attachmentsControl == null ? null : self.attachmentsControl.GetData(),
                    'ObjectClassID': self.ObjectClassID,
                    'ObjectID': self.ObjectID,
                    'CallList': self.CallList,
                    'MassIncidentId': self.MassIncidentId,
                    'DependencyList': dependencyList,
                    'FormValuesData': self.DynamicOptionsService.SendByServer(),
                };

                //    
                if (self.UserFields() !== undefined) {
                    data['UserField1'] = self.UserFields()['UserField1']();
                    data['UserField2'] = self.UserFields()['UserField2']();
                    data['UserField3'] = self.UserFields()['UserField3']();
                    data['UserField4'] = self.UserFields()['UserField4']();
                    data['UserField5'] = self.UserFields()['UserField5']();
                }
                if (data.TypeID == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('PromptType'));
                    });
                    retval.resolve(null);
                    return;
                }
                if (!data.Summary || data.Summary.trim().length == 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('PromptProblemSummary'));
                    });
                    retval.resolve(null);
                    return;
                }

                if (data.ServiceID == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('PromptServiceItem'));
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
                }

                if (data.FormValuesData && data.FormValuesData.hasOwnProperty('valid')) {
                    data.FormValuesData.callBack();
                    retval.resolve(null);
                    return;
                }

                //
                self.IsRegisteringProblem = true;

                self.finishProblemRegistration = function (responseData) {
                    hideSpinner();
                    if (responseData) {
                        if (showSuccessMessage)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ProblemRegisteredMessage').replace('{0}', responseData.Number));//some problem
                            });
                        retval.resolve({
                            ProblemID: responseData.ID
                        });
                    } else {
                        retval.resolve(null);
                    }

                    self.IsRegisteringProblem = false;
                };

                self.linkProblemToCalls = function (callList, problemData) {
                    if (callList.length == 0) {
                        self.finishProblemRegistration(problemData);
                        return;
                    }

                    var callID = callList[0];
                    var linkWithCallUrl = "api/calls/" + callID + "/problems/"
                    self.ajaxControl_RegisterProblem.Ajax(null,
                        {
                            url: linkWithCallUrl,
                            method: "POST",
                            dataType: "json",
                            contentType: 'application/json',
                            data: JSON.stringify({
                                ID: problemData.ID
                            })
                        },
                        function () {
                            // Process remaining calls.
                            self.linkProblemToCalls(callList.slice(1), problemData);
                        },
                        function () {
                            hideSpinner();
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ProblemRegistration.js, RegisterProblem]', 'error');
                            });
                            retval.resolve(null);
                            self.IsRegisteringProblem = false;
                        }
                    );
                };

                showSpinner();
                self.ajaxControl_RegisterProblem.Ajax(null,
                    {
                        url: '/api/problems',
                        method: 'POST',
                        dataType: 'json',
                        contentType: 'application/json',
                        data: JSON.stringify(data)
                    },
                    function (response) {//ProblemRegistrationResponse
                        var URL = "api/DocumentReferences/" + self.formClassID + "/" + response.ID + "/documents";
                        self.ajaxControl_RegisterProblem.Ajax(null,
                            {
                                url: URL,
                                method: "Post",
                                dataType: "json",
                                data: { 'docID': self.attachmentsControl.Items().map(function (file) { return file.ID; }) }
                            },
                            function () {
                                if (data.CallList) {
                                    self.linkProblemToCalls(data.CallList, response);
                                } else if (data.MassIncidentId) {
                                    var linkWithMassIncidentUrl = "api/massIncidents/" + data.MassIncidentId + "/problems";
                                    self.ajaxControl_RegisterProblem.Ajax(null,
                                        {
                                            url: linkWithMassIncidentUrl,
                                            method: "POST",
                                            dataType: "json",
                                            contentType: 'application/json',
                                            data: JSON.stringify({
                                                ReferenceID: response.ID
                                            })
                                        },
                                        function () {
                                            // TODO: Process linking problem result here if need.
                                            // Response is in the following format.
                                            // {"ID":1,"MassIncidentID":"e32f2962-3237-4d08-9c4a-259a6d4e277b","ReferenceID":"329e3b63-04ea-4e42-a3ba-6ea63b13baeb"}
                                            self.finishProblemRegistration(response);
                                        },
                                        function () {
                                            hideSpinner();
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ProblemRegistration.js, RegisterProblem]', 'error');
                                            });
                                            retval.resolve(null);
                                            self.IsRegisteringProblem = false;
                                        }
                                    );
                                } else {
                                    self.finishProblemRegistration(response);
                                }
                            },
                            function () {
                                hideSpinner();
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ProblemRegistration.js, RegisterProblem]', 'error');
                                });
                                retval.resolve(null);
                                self.IsRegisteringProblem = false;
                            });
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(response.responseText == null ? getTextResource('ErrorCaption') : response.responseText,
                                getTextResource('AjaxError') + '\n[ProblemRegistration.js, RegisterProblem]', 'info');
                        });
                        retval.resolve(null);
                        self.IsRegisteringProblem = false;
                    });
                //
                return retval.promise();
            };

            self.AddAs = false;//проблема создана по аналогии
            self.problemData = null;//проблема, взятая за основу
            //
            self.CallList = null;//список связанных заявок (при создании проблемы по заявке)
            //
            self.MassIncidentId = null; // Массовый инцидент (при создании проблемы по массовому инциденту)
            //
            self.Fill = function (problemData) {
                if (problemData.InfluenceID())
                    self.InfluenceID = problemData.InfluenceID();
                //
                if (problemData.PriorityID())
                    self.PriorityID = problemData.PriorityID();
                if (problemData.PriorityColor())
                    self.PriorityColor(problemData.PriorityColor());
                if (problemData.PriorityName())
                    self.PriorityName(problemData.PriorityName());
                //
                self.ProblemTypeHelper.LoadList();
                if (problemData.TypeID()) {
                    self.ProblemTypeHelper.SetSelectedItem(problemData.TypeID());
                }
                //
                if (problemData.UrgencyID())
                    self.UrgencyID = problemData.UrgencyID();
                //
                if (problemData.OwnerID())
                    self.OwnerID = problemData.OwnerID();
                //
                if (problemData.InitiatorID())
                    self.InitiatorID = problemData.InitiatorID();
                //
                if (problemData.ExecutorID())
                    self.ExecutorID = problemData.ExecutorID();
                //
                if (problemData.QueueID())
                    self.QueueID = problemData.QueueID();
                //
                if (problemData.QueueName())
                    self.QueueName = problemData.QueueName();
                //
                if (problemData.ServiceID())
                    self.ServiceID = problemData.ServiceID();
                //
                if (problemData.ServiceName())
                    self.ServiceName = problemData.ServiceName();
                //
                if (problemData.ServiceCategoryName())
                    self.ServiceCategoryName = problemData.ServiceCategoryName();
                //
                self.ServiceItemAttendanceFullName(problemData.ServiceCategoryName() + ' \\ ' + problemData.ServiceName());

                if (problemData.Summary())
                    self.Summary(problemData.Summary());
                //
                if (problemData.Description())
                    $.when(self.htmlDescriptionControlD).done(function () { self.Description(problemData.HTMLDescription()); });
                //
                if (problemData.CallList) {
                    self.CallList = problemData.CallList;
                } else if (problemData.MassIncidentId) {
                    self.MassIncidentId = problemData.MassIncidentId;
                } else if (problemData.ID()) {
                    self.primaryObjectID = problemData.ID();
                }
                //
                self.problemData = problemData;
            };
        }
    }
    return module;
});
