define([
    'knockout',
    'jquery',
    'ajax',
    'models/SDForms/SDForm.User',
    'dynamicOptionsService',
    'models/SDForms/SDForm.Tape',
    'models/SDForms/SDForm.NegotiationList',
    'models/SDForms/SDForm.KBAReferenceList',
    'models/SDForms/References/CallReferencesTab',
    'models/SDForms/References/ChangeRequestReferencesTab',
    'models/SDForms/References/ProblemReferencesTab',
    'models/SDForms/References/WorkOrderReferencesTab',
    'groupOperation',
    'typeHelper'], function (
    ko,
    $,
    ajax,
    userLib,
    dynamicOptionsService,
    tapeLib,
    negotiationListLib,
    kbaRefListLib,
    callRefsModule,
    changeRequestRefsModule,
    problemRefsModule,
    workOrderRefsModule,
    groupOperation
) {
    var module = {
        ViewModel: function (uri, readOnly, $region) {
            var self = this;

            // copy&paste from other forms
            {
                self.$region = $region;
                self.TabHeight = ko.observable(0);
                self.TabWidth = ko.observable(0);
                self.SizeChanged = function () {

                    const dialog = self.$region.closest('.ui-dialog');
                    const parent = dialog.parent();
                    let tabHeight = 0;

                    const isModeDialogModal = !parent.length || parent.get(0).nodeName === "BODY";
                    if (isModeDialogModal) {
                        tabHeight = self.$region.height();//form height
                        tabHeight -= self.$region.find('.b-requestDetail-menu').outerHeight(true);
                        tabHeight -= self.$region.find('.b-requestDetail__title-header').outerHeight(true);
                        tabHeight = Math.max(0, tabHeight - 10);
                    } else {
                        dialog.css('height', '100%');

                        const tabsContainer = self.$region.find('[data-form-tabs]');
                        if (tabsContainer.length) {
                            const domRect = tabsContainer.get(0).getBoundingClientRect();
                            const MARGIN_FROM_BODY = 15;
                            tabHeight = document.documentElement.clientHeight - domRect.top - MARGIN_FROM_BODY;
                        };
                    };
                    //
                    var tabWidth = self.$region.width();//form width
                    tabWidth -= self.$region.find('.b-requestDetail-right').outerWidth(true);
                    //

                    self.TabHeight(tabHeight + 'px');
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
                self.AfterRender = function () {
                    self.SizeChanged();
                    $(document).bind('objectUpdated', self.onObjectUpdated);
                    $(document).bind('objectInserted', self.onObjectInserted);
                    $(document).bind('objectDeleted', self.onObjectDeleted);
                    $(document).bind('local_objectUpdated', self.onObjectUpdated);
                    $(document).bind('local_objectInserted', self.onObjectInserted);
                    $(document).bind('local_objectDeleted', self.onObjectDeleted);
                };
                self.Unload = function () {
                    if (self.workflowControl() != null)
                        self.workflowControl().Unload();
                };

                self.IsReadOnly = ko.observable(readOnly);
                self.IsReadOnly.subscribe(function (readOnly) {
                    if (self.workflowControl() != null)
                        self.workflowControl().ReadOnly(readOnly);
                    if (self.criticalityControl() != null)
                        self.criticalityControl().ReadOnly(readOnly);
                    if (self.priorityControl() != null)
                        self.priorityControl().ReadOnly(readOnly);
                });
                self.updateGranted = ko.observable(false);
                self.CanEdit = ko.pureComputed(function () { return !self.IsReadOnly() && self.updateGranted(); });
                self.CanAddFiles = ko.pureComputed(function () { return !self.IsReadOnly(); });
                self.objectClassID = module.MassIncidentClassID; //mass incident
            }

            //Вкладки
            {
                self.modes = {
                    nothing: 'nothing',
                    main: 'main',
                    tape: 'tape',
                    calls: 'calls',
                    workorders: 'workorders',
                    problems: 'problems',
                    changeRequests: 'changeRequests',
                    negotiation: 'negotiation'
                };
                self.mainTabLoaded = ko.observable(false);
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
                        self.tapeControl.CalculateTopPosition();
                    else if (newValue == self.modes.negotiation)
                        self.negotiationList.CheckData(self.negotiationID);
                });
                self.ClickMain = function () {
                    self.mode(self.modes.main);
                };
                self.TapeClick = function () {
                    self.mode(self.modes.tape);
                    self.SizeChanged();
                }
            }

            //Users
            {
                function setNull(property) {
                    var data = {};
                    data[property] = null;
                    patch(data);
                }

                function editUser(userViewModel, fieldName, propertyName, searcher, searcherParams) {
                    if (!self.CanEdit())
                        return;
                    showSpinner();

                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.massIncident.imObjID(),
                            objClassID: self.objectClassID,
                            fieldName: 'MassIncident.' + fieldName,
                            fieldFriendlyName: getTextResource(fieldName),
                            oldValue: !userViewModel.empty() ? { ID: userViewModel.id(), ClassID: 9, FullName: userViewModel.name() } : null,
                            object: ko.toJS(userViewModel.model()),
                            searcherName: searcher,
                            searcherParams: searcherParams || {},
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            save: function (data) {
                                var obj = {};
                                obj[propertyName] = data.NewValue ? JSON.parse(data.NewValue).id : null;
                                return patch(obj);
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                }

                self.editOwner = function () {
                    editUser(self.massIncident.owner, 'Owner', 'OwnedByUserID', 'MassIncidentOwnerSearcher');
                };

                self.editExecutor = function () {
                    editUser(self.massIncident.executor, 'Executor', 'ExecutedByUserID', 'ExecutorUserSearcher', { QueueId: self.massIncident.group.id() });
                };

                self.editInitiator = function () {
                    editUser(self.massIncident.initiator, 'Initiator', 'CreatedByUserID', 'WebUserSearcher');
                }

                self.editGroup = function () {
                    if (!self.CanEdit())
                        return;
                    showSpinner();
                    var group = self.massIncident.group;
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.massIncident.imObjID(),
                            objClassID: self.objectClassID,
                            fieldName: 'MassIncident.Group',
                            fieldFriendlyName: getTextResource('Queue'),
                            oldValue: !group.empty() ? { ID: group.id(), ClassID: 722, FullName: group.name() } : null,
                            object: ko.toJS(group.model()),
                            searcherName: "QueueSearcher",
                            searcherPlaceholder: getTextResource('EnterQueue'),
                            searcherParams: { Type: 4 }, // for mass incident
                            save: function (data) {
                                return patch({ GroupID: data.NewValue ? JSON.parse(data.NewValue).id : null });
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                }

                self.setOwnerEmpty = function () {
                    setNull('OwnedByUserID');
                };

                self.setExecutorEmpty = function () {
                    setNull('ExecutedByUserID');
                };

                self.setGroupEmpty = function () {
                    setNull('GroupID');
                };

                self.setInitiatorEmpty = function () {
                    setNull('CreatedByUserID');
                };
            }       

            //
            self.workflowControl = ko.observable(null);
            self.LoadWorkflowControl = function () {
                //
                require(['workflow'], function (wfLib) {
                    if (self.workflowControl() == null) {
                        self.workflowControl(new wfLib.control(self.$region, self.IsReadOnly, self.massIncident.workflowEntity));
                        self.workflowControl().CanOpenMenu = function () {
                            return true;
                        };
                    }
                    self.workflowControl().ReadOnly(self.IsReadOnly());
                    self.workflowControl().Initialize();
                });
            };
            //

            //Load
            {
                self.massIncident = new module.MassIncidentViewModel(uri, self, {
                    editInitiator: self.editInitiator,
                    removeInitiator: self.setInitiatorEmpty,
                    editOwner: self.editOwner,
                    removeOwner: self.setOwnerEmpty,
                    editGroup: self.editGroup,
                    removeGroup: self.setGroupEmpty,
                    editExecutor: self.editExecutor,
                    removeExecutor: self.setExecutorEmpty
                }, self.CanEdit, $region);
                
                self.Load = function () {
                    self.InitRecalculationHeightTab();
                    self.massIncident.loadServices();                    
                    var promise = self.massIncident.load();
                    self.callReferencesTab.Table.fetchData();
                    self.problemReferencesTab.Table.fetchData();
                    self.changeRequestReferencesTab.Table.fetchData();
                    self.workOrderReferencesTab.Table.fetchData();
                    self.tapeControl.CheckData();

                    $.when(promise).done(function () {
                        self.LoadCriticalityControl();
                        self.LoadWorkflowControl();
                        self.LoadAttachmentsControl();
                        if (!self.negotiationList.isLoaded()) {
                            self.negotiationList.CheckData(self.negotiationID);
                        }
                    });

                    return promise;
                };               
                self.Refresh = function (callback) {
                    $.when(self.massIncident.load()).done(function () {
                        if (typeof callback === 'function') {
                            callback();
                        }
                        hideSpinner();
                    });
                };
                self.renderedD = $.Deferred();
                self.renderComplete = function () {
                    self.renderedD.resolve();
                    self.SizeChanged();
                };
            }

            //Save
            {
                function patch(data) {
                    var retD = $.Deferred();
                    showSpinner();

                    $(document).unbind('objectUpdated', self.onObjectUpdated);
                    $(document).unbind('objectInserted', self.onObjectInserted);
                    $(document).unbind('objectDeleted', self.onObjectDeleted);
                    $(document).unbind('local_objectUpdated', self.onObjectUpdated);
                    $(document).unbind('local_objectInserted', self.onObjectInserted);
                    $(document).unbind('local_objectDeleted', self.onObjectDeleted);
                    var currentTypeUri = self.massIncident.type.uri();
                    var promise = self.massIncident.patch(data);
                    $.when(promise).done(function (result) {
                        hideSpinner();

                        if (currentTypeUri !== self.massIncident.type.uri()) {
                            self.LoadWorkflowControl();
                        }                        

                        retD.resolve(result.success || result.readonly);                        
                        if (result.readonly) {
                            self.IsReadOnly(true);
                            self.Refresh();
                        }
                        $(document).bind('objectUpdated', self.onObjectUpdated);
                        $(document).bind('objectInserted', self.onObjectInserted);
                        $(document).bind('objectDeleted', self.onObjectDeleted);
                        $(document).bind('local_objectUpdated', self.onObjectUpdated);
                        $(document).bind('local_objectInserted', self.onObjectInserted);
                        $(document).bind('local_objectDeleted', self.onObjectDeleted);
                    });

                    return retD.promise();
                }
            }

            //История
            {
                const timelineConfig = [
                    { name: 'UtcDateModified' },
                    { name: 'UtcDateRegistered' },
                    { name: 'UtcDateOpened' },
                    { name: 'UtcDateClosed' },
                    { name: 'UtcDateCreated' },
                    { name: 'UtcDatePromised' },  
                    { name: 'UtcDateAccomplished' }
                ];
                self.IsSemiReadOnly = function () { return false; };
                self.CanViewNotes = function () { return true; };
                self.tapeControl = new tapeLib.Tape(
                        self.massIncident.tape,
                        self.objectClassID,
                        uri,
                        timelineConfig,
                        patch,
                        $region.find('.tape__b').selector,
                        $region.find('.tape__forms').selector,
                        self.IsSemiReadOnly,
                        self.CanEdit,
                        self.CanViewNotes,
                        self.TabSize,
                        self.massIncident.users);
            }

            // Связанные объекты
            {
                var referenceData = function () {
                    return { IMObjID: self.massIncident.imObjID() };
                };

                self.callReferencesTab = new callRefsModule.ViewModel(
                    self.$region,
                    function (selectedCalls) {
                        return new callRefsModule.AddMassIncidentViewModel(uri, selectedCalls);
                    },
                    function (selectedCalls) {
                        return new callRefsModule.RemoveMassIncidentViewModel(uri, selectedCalls);
                    }, {
                        tab: {
                            appendTemplate: 'SDForms/References/AddReference',
                            isReadOnly: self.IsReadOnly,
                            canEdit: self.CanEdit
                        },
                        list: {
                            view: 'MassIncidentReferencedCalls',
                            ajax: { url: uri + '/reports/referencedCalls' }
                        },
                        append: {
                            view: 'AvailableNotReferencedCalls',
                            ajax: {
                                url: '/api/calls/reports/availableNotReferencedByMassIncident/',
                                data: referenceData
                            }
                        }
                });

                self.workOrderReferencesTab = new workOrderRefsModule.ViewModel(
                    self.$region,
                    function () { return { Id: self.massIncident.imObjID(), ClassId: self.objectClassID }; }, {
                    tab: {
                        appendTemplate: 'SDForms/References/AddReference',
                        isReadOnly: self.IsReadOnly,
                        canEdit: self.CanEdit
                    },
                    list: {
                        view: 'MassIncidentReferencedWorkOrders',
                        ajax: {
                            url: uri + '/reports/referencedWorkOrders',
                        }
                    },
                    append: {
                        view: 'AvailableNotReferencedWorkOrders',
                        ajax: {
                            url: '/api/workorders/reports/availableNotReferencedByMassIncident/',
                            data: referenceData
                        }
                    }
                });

                self.problemReferencesTab = new problemRefsModule.ViewModel(
                    self.$region,
                    function (selectedProblems) {
                        return new problemRefsModule.AddMassIncidentViewModel(uri, selectedProblems);
                    },
                    function (selectedProblems) {
                        return new problemRefsModule.RemoveMassIncidentViewModel(uri, selectedProblems);
                    }, {
                    tab: {
                        appendTemplate: 'SDForms/References/AddReference',
                        isReadOnly: self.IsReadOnly,
                        canEdit: self.CanEdit
                    },
                    list: {
                        view: 'MassIncidentReferencedProblems',
                        ajax: {
                            url: uri + '/reports/referencedProblems',
                        }
                    },
                    append: {
                            view: 'AvailableNotReferencedProblems',
                            ajax: {
                                url: '/api/problems/reports/availableNotReferencedByMassIncident/',
                                data: referenceData
                            }
                    }
                });

                self.changeRequestReferencesTab = new changeRequestRefsModule.ViewModel(
                    self.$region,
                    function (selectedObjects) {
                        return new changeRequestRefsModule.AddMassIncidentViewModel(uri, selectedObjects);
                    },
                    function (selectedObjects) {
                        return new changeRequestRefsModule.RemoveMassIncidentViewModel(uri, selectedObjects);
                    }, {
                    tab: {
                        appendTemplate: 'SDForms/References/AddReference',
                        isReadOnly: self.IsReadOnly,
                        canEdit: self.CanEdit
                    },
                    list: {
                        view: 'MassIncidentReferencedChangeRequests',
                        ajax: {
                            url: uri + '/reports/referencedChangeRequests',
                        }
                    },
                    append: {
                        view: 'AvailableNotReferencedChangeRequests',
                        ajax: {
                            url: '/api/changeRequests/reports/availableNotReferencedByMassIncident/',
                            data: referenceData
                        }
                    }
                });
            }

            //Согласования
            {
                self.negotiationID = null;
                self.negotiationList = new negotiationListLib.LinkList(self.massIncident.negotiationsRef, self.objectClassID, self.$region.find('.negotiations__b .tabContent').selector, self.IsReadOnly, self.CanEdit);
            }

            //Dynamic options
            {
                self.DynamicOptionsService = new dynamicOptionsService.ViewModel(self.$region.attr('id'), {
                    typeForm: 'Completed',
                    currentTab: self.mode,
                    updateCallBack: self.massIncident.patch,
                    isReadOnly: self.IsReadOnly
                });
                
                self.DynamicOptionsServiceInit = function (templateID, values) {
                    if (self.DynamicOptionsService.IsInit()) {
                        self.DynamicOptionsService.ResetData();
                    };
                    if (templateID) {
                        self.DynamicOptionsService.GetTemplateByID(templateID, values);
                    }
                }

                self.SetTab = function (template) {
                    self.mode(`${self.modes.parameterPrefix}${template.Tab.ID}`);
                };
            }

            //Priority Control
            {
                self.priorityControl = ko.observable(null);

                self.massIncident.priority.id.subscribe(function (priorityID) {
                    if (priorityID) {
                        self.LoadPriorityControl();
                    }
                });

                self.LoadPriorityControl = function () {
                    require(['models/SDForms/Shared/dropDownMenu'], function (prLib) {
                        if (self.priorityControl() == null) {
                            self.priorityControl(new prLib.ViewModel(
                                self.$region.find('[data-drop-down-wrapper-priority]'),
                                self.IsReadOnly(),
                                '\'SDForms/Controls/PriorityList\'',
                                self.RefreshPriority
                            ));
                            self.priorityControl().ReadOnly(self.IsReadOnly());
                            self.priorityControl().Initialize();
                        }
                        self.priorityControl().Load(self.massIncident.imObjID(), self.objectClassID, self.massIncident.priority.id(), '/api/priorities');
                    });
                };

                self.RefreshPriority = function (priorityObj) {
                    if (priorityObj == null)
                        return;

                    self.massIncident.priority.name(priorityObj.Name);
                    self.massIncident.priority.name(priorityObj.Color);
                    patch({ PriorityID: priorityObj.ID });                    
                };
            }

            // criticality
            {
                self.criticalityControl = ko.observable(null);
                self.massIncident.criticality.id.subscribe(function (id) {
                    if (id) {
                        self.LoadCriticalityControl();
                    }
                });

                self.RefreshCriticality = function (criticalityObj) {
                    if (criticalityObj == null) {
                        return;
                    };

                    self.massIncident.criticality.name(criticalityObj.Name);
                    self.massIncident.criticality.id(criticalityObj.ID);
                    patch({ CriticalityID: criticalityObj.ID });
                };

                self.LoadCriticalityControl = function () {
                    require(['models/SDForms/Shared/dropDownMenu'], function (prLib) {
                        if (self.criticalityControl() == null) {
                            self.criticalityControl (new prLib.ViewModel(
                                self.$region.find('[data-drop-down-wrapper-criticality]'),
                                self.IsReadOnly(),
                                '\'SDForms/Controls/CriticalityList\'',
                                self.RefreshCriticality
                            ));
                            self.criticalityControl().ReadOnly(self.IsReadOnly());
                            self.criticalityControl().Initialize();                            
                        };
                        self.criticalityControl().Load(null, 823, self.massIncident.criticality.id(), '/api/criticalities');
                    });                    
                };
            }     

            {
                self.massIncident$ = ko.observable(self.massIncident);
                self.kbaRefList = new kbaRefListLib.KBAReferenceList(self.massIncident$, self.objectClassID, self.$region.find('.solution-kb__b').selector, self.IsReadOnly, self.IsReadOnly);

                self.ShowResultAndKB = ko.computed(function () {
                    return self.CanEdit() || self.kbaRefList.imList.List().length > 0;
                });

                self.LeftPanelModel = null;
                self.HideShowLeftPanel = function () {
                    if (!self.CanEdit())
                        return;

                    if (self.LeftPanelModel == null) {
                        require(['usualForms'], function (fhModule) {
                            var fh = new fhModule.formHelper();
                            if (self.LeftPanelModel == null) 
                                fh.ShowHelpSolutionPanel(self.ControlForm, self, self.massIncident$);
                        });

                        return;
                    }

                    var current = self.LeftPanelModel.IsVisible();
                    self.LeftPanelModel.IsVisible(!current);
                };

                self.AddTextToSolution = function (newText) {
                    if (!self.massIncident || !newText)
                        return;

                    var currentValue = self.massIncident.solution();
                    if (!currentValue)
                        currentValue = '';
                    var delimeter = '\n <div><br></div> ';
                    var newValue = currentValue + delimeter + newText;
                    self.massIncident.patch({ Solution: newValue });
                };
                self.RefreshOLAClick = function () {
                    patch ({ RefreshAgreement: {} });
                };

            }

            //Editors
            {
                self.editSummary = function () {
                    if (!self.CanEdit())
                        return;

                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.massIncident.imObjID(),
                            objClassID: self.objectClassID,
                            fieldName: 'MassIncident.Summary',
                            fieldFriendlyName: getTextResource('Summary'),
                            oldValue: self.massIncident.name(),
                            save: function (data) {
                                return patch({
                                    Name: JSON.parse(data.NewValue).text
                                });
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };

                self.editService = function () {
                    if (!self.CanEdit()) {
                        return;
                    }

                    var getObjectInfo = function () {
                        var service = self.massIncident.service;
                        if (service.id())
                            return {
                                ID: service.id(),
                                ClassID: 408,
                                FullName: service.fullname()
                            };
                        else
                            return null;
                    };
                    //
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.massIncident.imObjID(),
                            objClassID: self.objectClassID,
                            fieldName: 'MassIncident.Service',//одновременно оба поля
                            fieldFriendlyName: getTextResource('MassIncident_ServiceLabel'),
                            oldValue: getObjectInfo(),
                            allowNull: false,
                            searcherName: 'ServiceSearcher',
                            searcherTemplateName: null,//иной шаблон, дополненительные кнопки
                            searcherParams: { Types: [0, 1] },//параметры искалки
                            searcherLoadD: null,//ожидание дополнений для модели искалки,
                            save: function (data) {
                                var promise = patch({ ServiceID: JSON.parse(data.NewValue).id });

                                $.when(promise).done(function () { self.massIncident.loadServices(); });

                                return promise;
                            },
                            onSave: function () { }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };

                function editRichText(property, currentValue) {
                    if (!self.CanEdit()) {
                        return;
                    }

                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.massIncident.imObjID(),
                            objClassID: self.objectClassID,
                            fieldName: 'MassIncident.' + property,
                            fieldFriendlyName: getTextResource('MassIncident_' + property),
                            oldValue: currentValue,
                            readOnly: !self.CanEdit(),
                            save: function (data) {
                                var request = {};
                                request[property] = JSON.parse(data.NewValue).text;
                                return patch(request);
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                    });
                }

                self.editDescription = function () {
                    editRichText('Description', self.massIncident.description());
                };

                self.editCause = function () {
                    editRichText('Cause', self.massIncident.cause());
                };

                self.editSolution = function () {
                    editRichText('Solution', self.massIncident.solution());
                };
            }

            //Lookups
            {
                function editLookup(property, currentValue, uri, label) {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.massIncident.imObjID(),
                            objClassID: self.objectClassID,
                            fieldName: 'MassIncident.' + property,
                            fieldFriendlyName: getTextResource(label || `MassIncident${property}Label`),
                            comboBoxGetValueUrl: uri,
                            oldValue: currentValue,
                            save: function (data) {
                                var request = {};
                                var value = JSON.parse(data.NewValue);
                                if (value)
                                    request[property + 'ID'] = value.id;
                                return patch(request);
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                    });
                };

                self.editType = function () {
                    editLookup('Type', {
                        ID: self.massIncident.type.id(),
                        Name: self.massIncident.type.name()
                    }, '/api/massincidenttypes');
                };

                self.editInfoChannel = function () {
                    editLookup('InformationChannel', {
                        ID: self.massIncident.infoChannel.id(),
                        Name: self.massIncident.infoChannel.name()
                    }, '/api/massincidentinformationchannels');
                };

                self.editShortCause = function () {
                    editLookup('Cause', {
                        ID: self.massIncident.shortCause.id(),
                        Name: self.massIncident.shortCause.name()
                    }, '/api/massincidentcauses', 'MassIncidentShortCauseLabel');
                };

                self.editTechFailureCategory = function () {
                    editLookup('TechnicalFailureCategory', {
                        ID: self.massIncident.techFailureCategory.id(),
                        Name: self.massIncident.techFailureCategory.name()
                    }, '/api/technicalFailuresCategories?ServiceID=' + self.massIncident.service.id());
                };
            }

            //UI (areas visibility)
            {
                self.ServiceContainer = ko.observable(null);
                self.ParamsContainer = ko.observable(null);
                self.SolutionContainer = ko.observable(null);
                self.SLAContainer = ko.observable(null);
                self.AdditionalServiceContainer = ko.observable(null);

                require(['models/SDForms/Shared/visibleContainerHandler'], function (module) {
                    self.ServiceContainer(module.InitContainer());
                    self.ParamsContainer(module.InitContainer());
                    self.SolutionContainer(module.InitContainer());
                    self.SLAContainer(module.InitContainer());
                    self.AdditionalServiceContainer(module.InitContainer());
                });
            }

            // Файлы
            {
                self.attachmentsControl = null;
                self.LoadAttachmentsControl = function () {
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsControl != null) {
                            if (self.attachmentsControl.ObjectID != self.massIncident().ID())//previous object  
                                self.attachmentsControl.RemoveUploadedFiles();
                            else if (!self.attachmentsControl.IsAllFilesUploaded())//uploading
                            {
                                setTimeout(self.LoadAttachmentsControl, 1000);//try to reload after second
                                return;
                            }
                        }
                        if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                            const attachmentsElement = self.$region.find('.documentList');
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
                        self.attachmentsControl.ReadOnly(self.IsSemiReadOnly() && self.IsReadOnly());
                        self.attachmentsControl.RemoveFileAvailable(!self.IsReadOnly());
                        self.attachmentsControl.Initialize(self.massIncident.ID());
                    });
                };
            }

            // Затронутые сервисы
            self.addAffectedService = function () {
                //
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    fh.ShowSelectService({
                        selected: self.massIncident.affectedServices().map(function (x) { return x.id(); }),
                        excluded: [ self.massIncident.service.id() ],
                        onSave: self.massIncident.saveAffectedServices
                    });
                });
            };

            self.onObjectUpdated = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.massIncident.imObjID();

                if (objectClassID == 823 && currentID == objectID && e.type != 'local_objectUpdated') {
                    self.Refresh();
                } else if (objectClassID == 117 && currentID == parentObjectID) { //OBJ_NOTIFICATION ~ SDNote                
                    if (self.tapeControl.isNoteListLoaded())
                        self.tapeControl.TryAddNoteByID(objectID);
                    else
                        self.Refresh();
                } else if (objectClassID == 160 && currentID == parentObjectID) { //OBJ_NEGOTIATION                
                    if (self.negotiationList.isLoaded())
                        self.negotiationList.imList.TryReloadByID(objectID);
                    else
                        self.Reload(currentID);
                }
            };
            self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.massIncident.imObjID();
                if (objectClassID == 110 && currentID == parentObjectID) { //OBJ_DOCUMENT
                    self.LoadAttachmentsControl();
                }
                else if (objectClassID == 160 && currentID == parentObjectID) //OBJ_NEGOTIATION
                {
                    if (self.negotiationList.isLoaded())
                        self.negotiationList.imList.TryReloadByID(objectID);
                    else
                        self.Reload(currentID);                
                } else if (objectClassID == 117 && currentID == parentObjectID) { //OBJ_NOTIFICATION ~ SDNote                
                    if (self.tapeControl.isNoteListLoaded())
                        self.tapeControl.TryAddNoteByID(objectID);
                    else
                        self.Refresh();
                }
            };
            self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                var currentID = self.massIncident.imObjID();
                if (objectClassID == 110 && currentID == parentObjectID) { //OBJ_DOCUMENT
                    self.LoadAttachmentsControl();
                } else if (objectClassID == 160 && currentID == parentObjectID) { //OBJ_NEGOTIATION                
                    if (self.negotiationList.isLoaded())
                        self.negotiationList.imList.TryRemoveByID(objectID);
                    else
                        self.Reload(currentID);
                }
            };

            $.when(operationIsGrantedD(module.Operations.Edit)).done(function (granted) {
                self.updateGranted(granted);
            }); 
            $.when(operationIsGrantedD(module.Operations.ChangeOwner)).done(function (granted) {
                self.massIncident.owner.editAllowed(granted);
            }); 
        },
        Operations: {
            ChangeOwner: 1330,
            Edit: 983
        },
        MassIncidentViewModel: function (uri, parent, actions, canEdit, $region) {
            var self = this;

            self.ID = ko.observable(null);
            self.imObjID = ko.observable(null);
            self.number = ko.observable('');
            self.name = ko.observable('');
            self.title = ko.pureComputed(function () { return getTextResource('MassIncident') + ' №' + self.number() + '\'' + self.name() + '\'' });
            self.description = ko.observable('');
            self.cause = ko.observable('');
            self.solution = ko.observable('');
            self.createdAt = ko.observable(null);
            self.modifiedAt = ko.observable(null);
            self.lastModifiedLabel = ko.pureComputed(function () { return getTextResource('LastChange') + ' ' + self.modifiedAt(); });
            self.openedAt = ko.observable(null);
            self.closedAt = ko.observable(null);
            self.closeUntil = ko.observable(null);
            self.completedAt = ko.observable(null);
            self.registeredAt = ko.observable(null);
            self.closeUntilText = ko.pureComputed(function () {
                return self.closeUntil() ? getTextResource('Until') + ' ' + self.closeUntil() : '';
            });
            self.sla = new module.ReferenceViewModel();
            self.canEdit = canEdit;

            self.priority = new module.PriorityViewModel();
            self.service = new module.ServiceViewModel();            

            self.users = ko.pureComputed(function () {
                return [
                    self.initiator.model(),
                    self.owner.model(),
                    self.executor.model()
                ].filter(function (x) { return !(x instanceof userLib.EmptyUser) });
            });

            self.initiator = new module.UserViewModel(parent, userLib.UserTypes.workOrderInitiator, actions.editInitiator, actions.removeInitiator);
            self.owner = new module.OwnerViewModel(parent, userLib.UserTypes.owner, actions.editOwner, actions.removeOwner);
            self.executor = new module.UserViewModel(parent, userLib.UserTypes.executor, actions.editExecutor, actions.removeExecutor);
            self.group = new module.UserViewModel(parent, userLib.UserTypes.queueExecutor, actions.editGroup, actions.removeGroup, { IsFreezeSelectedClient: true });

            self.infoChannel = new module.LookupViewModel();
            self.criticality = new module.LookupViewModel();
            self.criticality.label = ko.pureComputed(function () {
                return self.criticality.name() || getTextResource('Criticality');
            });

            self.shortCause = new module.LookupViewModel();
            self.type = new module.LookupViewModel();
            self.techFailureCategory = new module.LookupViewModel();

            self.negotiations = new module.NegotiationsViewModel(self.imObjID);
            self.negotiationsRef = function () { return self.negotiations; };

            self.convertToLocalDate = function (d, onlyDate) {
                var options = onlyDate ?
                    {
                        year: "numeric",
                        month: "2-digit",
                        day: "2-digit",
                    } :
                    {
                        year: "numeric",
                        month: "2-digit",
                        day: "2-digit",
                        hour: "2-digit",
                        minute: "2-digit",
                        hour12: false
                    };
                //
                return d.toLocaleString(locale, options).replace(',', '');
            };
            self.parseDate = function (fromServer, onlyDate) {
                if (!fromServer)
                    return '';
                if (typeof fromServer === 'string' || fromServer instanceof String)
                    fromServer = parseFloat(fromServer);
                return self.convertToLocalDate(new Date(fromServer), onlyDate);
            };

            function update(data) {
                if(!data) return;
                self.ID(data.IMObjID);
                self.imObjID(data.IMObjID);
                self.number(data.ID);
                self.name(data.Name);
                self.description(data.Description);
                self.solution(data.Solution);

                self.createdAt(self.parseDate(data.UtcCreatedAt));
                self.modifiedAt(self.parseDate(data.UtcDateModified));
                self.registeredAt(self.parseDate(data.UtcRegisteredAt));
                self.openedAt(self.parseDate(data.UtcOpenedAt));
                self.completedAt(self.parseDate(data.UtcDateAccomplished));
                self.closedAt(self.parseDate(data.UtcDateClosed));
                self.closeUntil(self.parseDate(data.UtcCloseUntil));
                self.priority.uri(data.PriorityUri);
                self.initiator.uri(data.CreatedByUserUri);
                self.owner.uri(data.OwnedByUserUri);
                self.executor.uri(data.ExecutedByUserUri);
                self.group.uri(data.ExecutedByGroupUri);
                self.service.uri(data.ServiceUri);
                self.type.uri(data.TypeUri);
                self.infoChannel.uri(data.InformationChannelUri);
                self.criticality.uri(data.CriticalityUri);
                self.shortCause.uri(data.CauseUri);
                self.cause(data.Cause);
                self.techFailureCategory.uri(data.TechnicalFailureCategoryUri);
                self.workflowEntity().EntityStateID(data.EntityStateID);
                self.workflowEntity().EntityStateName(data.EntityStateName);
                self.customControl.uri('/api/massIncidents/' + data.IMObjID + '/customControls/my');
                // TODO: вернуть когда разберемся со SLA: self.sla.uri(data.SlaID ? '/api/sla/' + data.SlaID : null);
                if (data.FormValues) {
                    parent.DynamicOptionsServiceInit(data.FormValues.FormID, data.FormValues.Values);
                } else {
                    parent.DynamicOptionsServiceInit(null, null);
                }
            }

            var ajaxControl = new ajax.control();
            self.load = function () {
                var retD = $.Deferred();
                ajaxControl.Ajax($region,
                    {
                        method: "GET",
                        url: uri
                    }, function (massIncidentData) {
                        update(massIncidentData);
                        if (massIncidentData.FormValues) {
                            parent.DynamicOptionsServiceInit(massIncidentData.FormValues.FormID, massIncidentData.FormValues.Values);
                        } else {
                            parent.DynamicOptionsServiceInit(null, null);
                        }

                        retD.resolve(true);
                    }, function () {
                        retD.resolve(false);
                });
                return retD.promise();
            };

            var affectedServicesCache = {};
            self.affectedServices = ko.observableArray([]);

            var affectedServicesAjaxControl = new ajax.control();

            function pushAffectedService(item) {
                var viewModel;
                if (affectedServicesCache[item.ReferenceID]) {
                    viewModel = affectedServicesCache[item.ReferenceID];
                } else {
                    viewModel = new module.ServiceViewModel();
                    viewModel.uri(item.ServiceUri);
                    affectedServicesCache[item.ReferenceID] = viewModel;
                }

                self.affectedServices.push(viewModel);
            }

            self.loadServices = function () {
                var retD = $.Deferred();

                affectedServicesAjaxControl.Ajax(null, {
                    method: 'GET',
                    url: uri + '/affectedServices'
                }, function (data) {
                    if (!data) {
                        return;
                    }

                    self.affectedServices.removeAll();

                    ko.utils.arrayForEach(data, function (item) {
                        pushAffectedService(item);
                    }); 

                    retD.resolve(false);
                }, function () {
                    self.affectedServices.removeAll();
                    retD.resolve(false);
                });

                return retD.promise();
            }                
            self.addAffectedServices = function (serviceIds) {
                var retD = $.Deferred();

                if (serviceIds.length > 0) {
                    var groupOperation = new module.AddAffectedServicesViewModel(uri + '/affectedServices', serviceIds);
                    groupOperation.subscribeSuccess(function (serviceID, response) {
                        pushAffectedService(response);
                    });
                    groupOperation.subscribeComplete(function () {
                        retD.resolve();
                    });

                    groupOperation.start();
                } else {
                    retD.resolve();
                }

                return retD.promise();
            }
            self.removeAffectedService = function (service) {
                return self.removeAffectedServices([service]);
            };
            self.removeAffectedServices = function (services) {
                var retD = $.Deferred();

                if (services.length > 0) {
                    var groupOperation = new module.RemoveAffectedServicesViewModel(
                        uri + '/affectedServices',
                        services.map(function (s) { return s.id(); }));
                    groupOperation.subscribeSuccess(function (serviceID, response) {
                        var removedService = services.filter(function (s) { return s.id() == serviceID; })[0];
                        self.affectedServices.remove(removedService);
                    });
                    groupOperation.subscribeComplete(function () {
                        retD.resolve();
                    });
                    groupOperation.start();
                } else {
                    retD.resolve();
                }

                return retD.promise();
            }
            self.saveAffectedServices = function (selectedServices) {
                var retD = $.Deferred();

                var currentServices = self.affectedServices().map(function (x) { return x.id(); });
                var newServices = selectedServices.map(function (x) { return x.id; });

                var addServiceIds = newServices.filter(function (id) { return !currentServices.includes(id); });
                var servicesToRemove = self.affectedServices().filter(
                    function (service) {
                        return !newServices.includes(service.id());
                    });

                $.when(self.addAffectedServices(addServiceIds)).done(function () {
                    $.when(self.removeAffectedServices(servicesToRemove)).done(function () {
                        retD.resolve();
                    });                    
                });

                return retD.promise();
            };

            self.customControl = new module.CustomControlViewModel();

            self.patch = function (data) {
                var retD = $.Deferred();
                ajaxControl.Ajax(
                    $region,
                    {
                        dataType: "json",
                        contentType: "application/json",
                        method: "PATCH",
                        url: uri,
                        data: JSON.stringify(data)
                    },
                    function (massIncidentData) {
                        update(massIncidentData);
                        retD.resolve({ success: true });
                    },
                    function () {
                        retD.resolve({ success: false, readonly: false });
                    }, {
                    onMethodNotAllowed: function () {
                        retD.resolve({ success: false, readonly: true });
                    }
                });
                return retD.promise();
            };

            function patchDate(propertyName) {
                return function (newVal) {
                    var data = {};
                    data[propertyName] = newVal;
                    return self.patch(data);
                }
            }
            self.tape = function () {
                return {
                    ID: self.imObjID,
                    UnreadNoteCount: function () { return 0; },
                    MessageCount: function () { return 0; },
                    TotalNotesCount: function () { return 0; },
                    HaveUnread: function () { return false; },
                    UtcDateModified: self.modifiedAt,
                    UtcDateOpened: self.openedAt,
                    UtcDateClosed: self.closedAt,
                    UtcDatePromised: self.closeUntil,
                    UtcDateCreated: self.createdAt,
                    UtcDateRegistered: self.registeredAt,
                    UtcDateAccomplished: self.completedAt,
                    patchDateRegistered: patchDate('UtcDateRegistered'),
                    patchDateOpened: patchDate('UtcOpenedAt'),                     
                    patchDatePromised: patchDate('UtcCloseUntil'),
                    patchDateAccomplished: patchDate('UtcDateAccomplished'),
                    patchDateClosed: patchDate('UtcDateClosed')
                };
            };

            self.workflowEntity = ko.observable({
                    ID: self.imObjID,
                    ClassID: module.MassIncidentClassID,
                    EntityStateID: ko.observable(''),
                    EntityStateName: ko.observable('')
            });
        },
        AddAffectedServicesViewModel: function (uri, keys) {
            groupOperation.ViewModelBase.call(this, keys, { ajax: groupOperation.PostAjaxOptions, batchSize: 1 });
            this._getUrl = function () {
                return uri;
            };
            this._getData = function (serviceID) {
                return JSON.stringify({ ReferenceID: serviceID });
            };
        },
        RemoveAffectedServicesViewModel: function (uri, keys) {
            groupOperation.ViewModelBase.call(this, keys, { ajax: groupOperation.DeleteAjaxOptions, batchSize: 1 });
            this._getUrl = function (serviceID) {
                return uri + '/' + serviceID;
            };
            this._getData = function () {
                return null;
            };
        },
        ReferenceViewModel: function () {
            var self = this;
            self.uri = ko.observable(null);

            self._reset = function () { };
            self._update = function (data) { };

            var ajaxControl = new ajax.control();
            self.uri.subscribe(function (uri) {
                if (uri) {
                    ajaxControl.Ajax(null, { method: 'GET', url: uri }, function (data) { self._update(data); }, function () { self._reset(); });
                } else {
                    self._reset();
                }
            });
        },
        SlaViewModel: function () {
            var self = this;
            module.ReferenceViewModel.call(this);
                        
            this.name = ko.observable('');
            this._update = function (data) {
                self.name(data.Name);
            };
            this._reset = function () {
                self.name('');
            };
        },
        PriorityViewModel: function () {
            var self = this;

            module.ReferenceViewModel.call(this);
            self._update = function (data) {
                self.name(data.Name);
                self.color(data.Color);
                self.id(data.ID);
            };
            self._reset = function () {
                self._update({});
            };

            self.id = ko.observable(null);
            self.name = ko.observable(null);
            self.color = ko.observable(null);
        },
        ServiceViewModel: function () {
            var self = this;

            module.ReferenceViewModel.call(this);

            self._update = function (data) {
                self.id(data.ID);
                self.category.uri(data.CategoryUri);
                self.name(data.Name || '');
            };

            self._reset = function () {
                self._update({});
            };

            self.id = ko.observable(null);
            self.category = new module.LookupViewModel();
            self.name = ko.observable('');
            self.fullname = ko.pureComputed(function () {
                return [self.category.name(), self.name()].join(' \\ ');
            });
        },
        UserViewModel: function (root, type, edit, remove, options) {
            var self = this;
            module.ReferenceViewModel.call(this);

            self.edit = ko.observable(edit || null);
            self.remove = ko.observable(remove || null);
            self._setModel = function () {
                var initOptions = Object.assign({
                    UserID: self.id(),
                    UserType: type,
                    UserName: self.name(),
                    EditAction: self.edit(),
                    RemoveAction: self.remove(),
                    CanNote: false
                }, options);

                self.model(null); // начинаем костыллинг
                var newModel = self.uri() ? new userLib.User(root, initOptions) : new userLib.EmptyUser(root, type, self.edit());
                self.empty(!self.uri()); // шаблон переключился с null в модели (не должен падать)
                self.model(newModel); // теперь можно подставить модель
            };
            self._update = function (data) {
                self.id(data.ID);
                self.name(data.FullName || data.Name);           

                self._setModel();
            };

            self._reset = function () {
                self.id(null);
                self.empty(true);
                self.name('');
                self._setModel();
            };

            self.id = ko.observable(null);
            self.name = ko.observable('');            
            self.model = ko.observable(null);
            self.empty = ko.observable(false);
            self._setModel();
        },
        OwnerViewModel: function (root, type, edit, remove, options) {
            var self = this;
            module.UserViewModel.call(this, root, type, edit, remove, options);

            function toggleEdit(allowed) {
                self.edit(allowed || self.empty() ? edit : null);
                self.remove(allowed || self.empty() ? remove : null);
                self._setModel();
            }

            self.editAllowed = ko.observable(false);
            self.editAllowed.subscribe(toggleEdit);
            toggleEdit(self.editAllowed());
        },
        LookupViewModel: function () {
            var self = this;
            module.ReferenceViewModel.call(self);

            self._update = function (data) {
                self.name(data.Name);
                self.id = ko.observable(data.ID);
            };

            self._reset = function (data) {
                self.name('');
                self.id = ko.observable(null);
            };

            self.name = ko.observable('');
            self.id = ko.observable(null);
        },
        CriticalityViewModel: function () {
            var self = this;
            module.LookupViewModel.call(self);

            self.label = ko.pureComputed(function () { return getTextResource('MassIncident_CriticalityLabel') + ': ' + self.name(); });
        },
        TabViewModel: function (id) {
            var self = this;

            self.ID = id;
        },
        NegotiationsViewModel: function (id) {
            var self = this;
            module.TabViewModel.call(self, id);

            self.NegotiationCount = ko.observable(0);
            self.HaveUnvotedNegotiation = ko.observable(false);
        },
        CustomControlViewModel: function () {
            var self = this;
            module.ReferenceViewModel.call(this);

            this.underControl = ko.observable(false);

            var updateControl = new ajax.control();

            function toggle() {
                self.underControl(!self.underControl());
            }

            this._update = function (data) {
                self.underControl(data.UnderControl);
            }

            this._reset = function () {
                self.underControl(false);
            }

            this.toggle = function () {
                toggle();
                updateControl.Ajax(null, {
                    method: 'PUT',
                    url: self.uri(),
                    data: JSON.stringify({ UnderControl: self.underControl() }),
                    dataType: "json",
                    contentType: "application/json"
                },
                function (data) {
                    self._update(data);
                }, function () {
                    toggle();
                });                
            }
        },
        MassIncidentClassID: 823
    };

    return module;
});