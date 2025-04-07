define(['jquery', 'knockout', 'formControl', 'usualForms'], function ($, ko, fc, ufh) {
    var module = {
        formHelper: function (isSpinnerActive) {
            var self = this;
            //
            //Режимы открытия форм
            self.Mode = {
                Default: 0,//режим по умолчанию, форма откоется на главной вкладке
                ReadOnly: 1,//только чтение
                ClientMode: 2,//режим клиента вместо инженера
                SetGrade: 4,//режим установки оценки заяки
                TabNegotiation: 8,//вкладка согласования
                ForceEngineer: 16//открыть инженерную приоритетно
            };
            self.FormHelpers = () => {
                const createControl = (idForm, nameForm, nameResource, params = null) => {
                    let controlInfo = null;

                    if (params && params.columnViewModel && params.columnViewModel.showEntityService.viewModel && params.columnViewModel.showEntityService.viewModel.Visible()) {
                        const lastFormId = params.columnViewModel.showEntityService.formLastShowId;
                        if (lastFormId) {
                            ko.cleanNode($(`#${lastFormId}`).get(0));
                        }

                        const afterCloseCb = () => {
                            if (params.columnViewModel.showEntityService.viewModel.Visible()) {
                                params.columnViewModel.showEntityModel.Visible(false);
                                params.columnViewModel.showEntityService.destroy();
                            }
                        }

                        controlInfo = new fc.control(
                            nameForm, nameForm, getTextResource(nameResource),
                            false, false, true, 710, 500, {}, afterCloseCb,
                            `data-bind="template: {name: \'SDForms/${nameForm}\', afterRender: AfterRender}"`,
                            { appendTo: '#mainWrapperRightContent' }
                        );

                        params.columnViewModel.showEntityService.setLastShowFormId(controlInfo.GetRegionID())
                    } else {
                        controlInfo = new fc.control(
                            idForm, idForm, getTextResource(nameResource),
                            true, true, true, 710, 520, {}, null,
                            `data-bind="template: {name: \'SDForms/${nameForm}\', afterRender: AfterRender}"`
                        );
                    }

                    return controlInfo;
                }

                const dontInitialized = (idInPath) => {
                    hideSpinner();

                    const isWnd = window.open(window.location.protocol + '//' + window.location.host + location.pathname + `?${idInPath}ID=` + id);
                    if (isWnd) return; // browser cancel it?  

                    require(['sweetAlert'], () => {
                        swal(
                            getTextResource('OpenError'),
                            getTextResource('CantDuplicateForm'),
                            'warning'
                        );
                    });
                }

                const beforeClose = (model) => {
                    let isRetval = false;
                    const isAllFilesLoadByServer = () => {
                        return model.attachmentsControl == null || model.attachmentsControl != null && model.attachmentsControl.IsAllFilesUploaded();
                    }

                    if (isAllFilesLoadByServer()) {
                        isRetval = true;
                    }
                    else {
                        require(['sweetAlert'], () => {
                            swal({
                                title: getTextResource('UploadedFileNotFoundAtServerSide'),
                                text: getTextResource('FormClosingQuestion'),
                                showCancelButton: true,
                                closeOnConfirm: true,
                                closeOnCancel: true,
                                confirmButtonText: getTextResource('ButtonOK'),
                                cancelButtonText: getTextResource('ButtonCancel')
                            },
                                (value) => {
                                    if (value) {
                                        //TODO? close event of swal
                                        setTimeout(() => controlInfo.Close(), 300);
                                    }
                                });
                        });
                    }

                    if (isRetval) {
                        model.Unload();
                    }

                    return isRetval;
                }

                return {
                    createControl,
                    dontInitialized,
                    beforeClose
                }
            }
            //Форма просмотра заявки (универсальный вызов, режим определяется автоматом)
            //ИД заявки, дополнительные параметры
            self.ShowCallByContext = function (id, params) {
                $.when(userD).done(function (user) {
                    var mode = self.Mode.Default;
                    if (params && params.mode && isNaN(parseInt(params.mode)) == false)
                        mode |= params.mode;
                    //
                    if (params && params.NegotiationID)
                        mode |= self.Mode.TabNegotiation;

                    $.when(operationIsGrantedD(313)).done(function (callUpdateAllowed) {
                        if (user.HasRoles == false || callUpdateAllowed == false)
                            mode |= self.Mode.ReadOnly;

                        var useView = params ? (params.useView != false) : true;
                        if (user.ViewNameSD == 'ClientCallForTable' && useView) {//список заявок клиента
                            mode |= self.Mode.ClientMode;
                            self.ShowCall(id, mode, params);
                        }
                        else if (user.ViewNameSD == 'NegotiationForTable' && useView) {//список объектов, где я среди согласующих
                            mode |= self.Mode.TabNegotiation;
                            self.ShowCall(id, mode, params);
                        }
                        else if (user.HasRoles == false) {//нет ролей
                            mode |= self.Mode.ClientMode;
                            self.ShowCall(id, mode);
                        }
                        else//есть роли
                            $.when(operationIsGrantedD(518)).done(function (result) {//OPERATION_CALL_Properties - есть отрыть свойства
                                if (result == false)
                                    mode |= self.Mode.ClientMode;
                                self.ShowCall(id, mode, params);
                            });
                    });
                });
            };
            //Форма просмотра заявки клиентом / инженером
            //ИД заявки, режим формы, дополнительные параметры для определенных представлений (простановка оценки - newGrade)
            self.ShowCall = (id, mode, params) => {
                if (!isSpinnerActive) {
                    showSpinner();
                }
                
                $.when(userD, operationIsGrantedD(313)).done((user, call_update) => { //OPERATION_Call_Update
                    if (!user.HasRoles || !call_update) {
                        isReadOnly = true;
                    }

                    const { createControl } = self.FormHelpers()
                    const controlInfo = createControl('callForm', 'CallForm', 'Call', params);

                    if (!controlInfo.Initialized) {
                        const { dontInitialized } = self.FormHelpers();
                        dontInitialized('call');

                        return;
                    }

                    const controlInfoShowPromise = controlInfo.Show();
                    controlInfo.ExtendSize(1000, 800);

                    require(['models/SDForms/CallForm'], (vm) => {
                        const region = $(`#${controlInfo.GetRegionID()}`);

                        const isReadOnly = (mode & self.Mode.ReadOnly) == self.Mode.ReadOnly;
                        const isClientMode = (mode & self.Mode.ClientMode) == self.Mode.ClientMode;

                        const model = new vm.ViewModel(isReadOnly || (call_update != true && user.HasRoles == true && isClientMode == false), isClientMode, region, id, user);

                        model.CurrentUserID = user.UserID;
                        model.CurrentUserHasRoles = user.HasRoles;

                        const oldSizeChanged = controlInfo.SizeChanged;
                        controlInfo.SizeChanged = () => {
                            oldSizeChanged();
                            model.SizeChanged();
                        };

                        controlInfo.BeforeClose = () => {
                            const validate = model.DynamicOptionsService.Validate();
                            if (!validate.valid) {
                                validate.callBack();
                                return false;
                            };

                            const { beforeClose } = self.FormHelpers();
                            return beforeClose(model);
                        };

                        model.CloseForm = () => {
                            controlInfo.BeforeClose = null;
                            controlInfo.Close();
                        };

                        model.ControlForm = controlInfo;
                        const loadModelPromise = model.Load(id);
                        
                        ko.applyBindings(model, document.getElementById(controlInfo.GetRegionID()));
                        $.when(controlInfoShowPromise, loadModelPromise).done((frm, loadResult) => {
                            if (!loadResult) {//force close
                                model.CloseForm();
                            }
                            else {
                                if (!ko.components.isRegistered('callFormCaptionComponent'))
                                    ko.components.register('callFormCaptionComponent', {
                                        template: '<span class="color-point" style="margin-right:5px; background: rgb(255,255,255);" data-bind="style: { background: $priorityColor}"/><span data-bind="text: $captionText"/>'
                                    });
                                controlInfo.BindCaption(model, "component: {name: 'callFormCaptionComponent', params: { $priorityColor: $data.call().PriorityColor, $captionText: getTextResource(\'Call\') + ($data.call() != null ? (\' №\' + $data.call().Number() + \' \' + $data.call().CallSummaryName()) : \'\')} }");
                                //
                                if (model.mode && model.modes) {
                                    model.negotiationID = params ? params.NegotiationID : null;

                                    const tabNegotiation = (mode & self.Mode.TabNegotiation) == self.Mode.TabNegotiation;
                                    model.mode(tabNegotiation == true ? model.modes.negotiation : model.modes.main);
                                }
                                
                                const isSetGradeMode = (mode & self.Mode.SetGrade) == self.Mode.SetGrade;
                                if (isSetGradeMode && params) {
                                    $.when(model.$isLoaded, model.CreateGrade()).done(() => {
                                        const newGrade = params.newGrade;
                                        const gradeObj = ko.utils.arrayFirst(model.GradeArray(), (el) => el.Name == newGrade);
                                        if (gradeObj != null) {
                                            $.when(model.SetGrade(gradeObj, null, true)).done((result) => {
                                                if (!result) return;

                                                require(['sweetAlert'], () => {
                                                    swal(getTextResource('ThanksForSetGrade'), '', 'success');
                                                });
                                            });
                                        }
                                        else {
                                            require(['sweetAlert'], () => {
                                                swal(getTextResource('SaveError'), getTextResource('CantSetGrade'), 'error');
                                            });
                                        }
                                    });
                                }
                                //
                                if (model.mainTabLoaded)
                                    model.mainTabLoaded(true);
                            }
                            hideSpinner();
                        });
                    });
                });
            };
            //
            //Форма просмотра задания
            //ИД задания, режим формы
            self.ShowWorkOrder = (id, mode, params) => {
                let isReadOnly = (mode & self.Mode.ReadOnly) == self.Mode.ReadOnly;
                const tabNegotiation = (mode & self.Mode.TabNegotiation) == self.Mode.TabNegotiation;

                if (!isSpinnerActive) {
                    showSpinner();
                }

                $.when(userD, operationIsGrantedD(333)).done((user, workOrder_update) => {
                    if (!user.HasRoles || !workOrder_update) {
                        isReadOnly = true;
                    }
                                         
                    const { createControl } = self.FormHelpers();
                    const controlInfo = createControl('workOrderForm', 'WorkOrderForm', 'WorkOrder', params);

                    if (!controlInfo.Initialized) {
                        const { dontInitialized } = self.FormHelpers();
                        dontInitialized('workOrder');

                        return;
                    }

                    const controlInfoShowPromise = controlInfo.Show();
                    controlInfo.ExtendSize(1000, 800);
                    
                    require(['models/SDForms/WorkOrderForm'], vm => {
                        const region = $(`#${controlInfo.GetRegionID()}`);
                        const model = new vm.ViewModel(isReadOnly, region, id);

                        const oldSizeChanged = controlInfo.SizeChanged;
                        controlInfo.SizeChanged = () => {
                            oldSizeChanged();
                            model.SizeChanged();
                        };

                        controlInfo.BeforeClose = () => {
                            const validate = model.DynamicOptionsService.Validate();
                            if (!validate.valid) {
                                validate.callBack();
                                return false;
                            };

                            const { beforeClose } = self.FormHelpers();
                            return beforeClose(model);
                        };
                        
                        model.CloseForm = () => {
                            controlInfo.BeforeClose = null;
                            controlInfo.Close();
                        };

                        model.ControlForm = controlInfo;
                        const loadModelPromise = model.Load(id);
                        ko.applyBindings(model, document.getElementById(controlInfo.GetRegionID()));

                        $.when(controlInfoShowPromise, loadModelPromise).done((_, loadResult) => {
                            if (!loadResult) {
                                controlInfo.BeforeClose = null;
                                controlInfo.Close();
                            }
                            else {
                                if (!ko.components.isRegistered('workOrderFormCaptionComponent')) {
                                    ko.components.register('workOrderFormCaptionComponent', {
                                        template: `
                                            <span class="color-point" style="margin-right:5px;background: rgb(255,255,255);" data-bind="style: { background: $priorityColor}"></span>
                                            <span data-bind="text: $captionText"></span>
                                        `
                                    });
                                }
                                    

                                controlInfo.BindCaption(model, "component: {name: 'workOrderFormCaptionComponent', params: { $priorityColor: $data.workOrder().PriorityColor, $captionText: getTextResource(\'WorkOrder\') + ($data.workOrder() != null ? (\' №\' + $data.workOrder().Number() + \' \' + $data.workOrder().Name()) : \'\')} }");
                                
                                if (model.mode && model.modes) {
                                    model.negotiationID = params ? params.NegotiationID : null;
                                    model.mode(tabNegotiation == true ? model.modes.negotiation : model.modes.main);
                                }
                                
                                if (model.mainTabLoaded)
                                    model.mainTabLoaded(true);
                            }
                            hideSpinner();
                        });
                    });
                });
            };
            //

            //Форма просмотра массового инцидента
            //ИД задания, режим формы
            self.ShowMassIncident = function (uri, mode, params) {
                let isReadOnly = (mode & self.Mode.ReadOnly) == self.Mode.ReadOnly;
                const tabNegotiation = (mode & self.Mode.TabNegotiation) == self.Mode.TabNegotiation;

                if (!isSpinnerActive) {
                    showSpinner();
                }

                $.when(userD, operationIsGrantedD(983)).done((user, mi_update) => {
                    if (!user.HasRoles || !mi_update) {
                        isReadOnly = true;
                    }

                    const { createControl } = self.FormHelpers();

                    const controlInfo = createControl('massIncidentForm', 'MassIncidentForm', 'MassIncident', params);

                    if (!controlInfo.Initialized) {
                        const { dontInitialized } = self.FormHelpers();
                        dontInitialized('massIncident');

                        return;
                    }

                    const controlInfoShowPromise = controlInfo.Show();
                    controlInfo.ExtendSize(1000, 800);

                    require(['models/SDForms/MassIncidentForm'], function(vm) {
                        const region = $(`#${controlInfo.GetRegionID()}`);
                        const model = new vm.ViewModel(uri, isReadOnly, region);

                        const oldSizeChanged = controlInfo.SizeChanged;
                        controlInfo.SizeChanged = () => {
                            oldSizeChanged();
                            model.SizeChanged();
                        };

                        controlInfo.BeforeClose = () => {
                            const validate = model.DynamicOptionsService.Validate();
                            if (!validate.valid) {
                                validate.callBack();
                                return false;
                            };

                            const { beforeClose } = self.FormHelpers();
                            return beforeClose(model);
                        };

                        model.CloseForm = () => {
                            controlInfo.BeforeClose = null;
                            controlInfo.Close();
                        };

                        model.ControlForm = controlInfo;
                        const loadModelPromise = model.Load();
                        ko.applyBindings(model, document.getElementById(controlInfo.GetRegionID()));

                        $.when(controlInfoShowPromise, loadModelPromise).done((_, loadResult) => {
                            if (!loadResult) {
                                controlInfo.BeforeClose = null;
                                controlInfo.Close();
                            }
                            else {
                                if (!ko.components.isRegistered('massIncidentFormCaptionComponent')) {
                                    ko.components.register('massIncidentFormCaptionComponent', {
                                        template: `
                                            <span class="color-point" style="margin-right:5px;background: rgb(255,255,255);" data-bind="style: { background: $priorityColor}"></span>
                                            <span data-bind="text: $captionText"></span>
                                        `
                                    });
                                }


                                controlInfo.BindCaption(model, "component: {name: 'massIncidentFormCaptionComponent', params: { $priorityColor: $data.massIncident.priority.color, $captionText: $data.massIncident.title() }}");

                                if (model.mode && model.modes) {
                                    model.negotiationID = params ? params.NegotiationID : null;
                                    model.mode(tabNegotiation == true ? model.modes.negotiation : model.modes.main);
                                }

                                if (model.mainTabLoaded)
                                    model.mainTabLoaded(true);
                            }
                            hideSpinner();
                        });
                    });
                });
            };
            //
            //Форма просмотра проблемы
            //ИД проблемы, режим формы
            self.ShowProblem = (id, mode, params) => {
                let isReadOnly = (mode & self.Mode.ReadOnly) == self.Mode.ReadOnly;

                var tabNegotiation = (mode & self.Mode.TabNegotiation) == self.Mode.TabNegotiation;
                if (!isSpinnerActive) {
                    showSpinner();
                }

                $.when(userD, operationIsGrantedD(323)).done((user, problem_update) => {
                    if (!user.HasRoles || !problem_update) {
                        isReadOnly = true;
                    }

                    const { createControl } = self.FormHelpers()
                    const controlInfo = createControl('problemForm', 'ProblemForm', 'Problem', params);

                    if (!controlInfo.Initialized) {
                        const { dontInitialized } = self.FormHelpers();
                        dontInitialized('problem');

                        return;
                    }

                    const controlInfoShowPromise = controlInfo.Show();
                    controlInfo.ExtendSize(1000, 800);

                    require(['models/SDForms/ProblemForm'], (vm) => {
                        const region = $(`#${controlInfo.GetRegionID()}`);
                        const model = new vm.ViewModel(isReadOnly, region, id);

                        const oldSizeChanged = controlInfo.SizeChanged;
                        controlInfo.SizeChanged = () => {
                            oldSizeChanged();
                            model.SizeChanged();
                        };

                        controlInfo.BeforeClose = () => {
                            const validate = model.DynamicOptionsService.Validate();
                            if (!validate.valid) {
                                validate.callBack();
                                return false;
                            };

                            const { beforeClose } = self.FormHelpers();
                            return beforeClose(model);
                        };

                        model.CloseForm = () => {
                            controlInfo.BeforeClose = null;
                            controlInfo.Close();
                        };

                        model.ControlForm = controlInfo;
                        const loadModelPromise = model.Load(id);

                        ko.applyBindings(model, document.getElementById(controlInfo.GetRegionID()));
                        $.when(controlInfoShowPromise, loadModelPromise).done((frm, loadResult) => {
                            if (loadResult == false) {//force close
                                model.CloseForm();
                            }
                            else {
                                if (!ko.components.isRegistered('problemFormCaptionComponent')) {
                                    ko.components.register('problemFormCaptionComponent', {
                                        template: '<span class="color-point" style="margin-right:5px; " data-bind="style: { background: $priorityColor}" /><span data-bind="text: $captionText"/>'
                                    });
                                }
                                    
                                controlInfo.BindCaption(model, "component: {name: 'problemFormCaptionComponent', params: { $priorityColor: $data.problem().PriorityColor, $captionText: getTextResource(\'Problem\') + ($data.problem() != null ? (\' №\' + $data.problem().Number() + \' \' + $data.problem().Summary()) : \'\')} }");

                                if (model.mode && model.modes) {
                                    model.negotiationID = params ? params.NegotiationID : null;
                                    model.mode(tabNegotiation == true ? model.modes.negotiation : model.modes.main);
                                }

                                if (model.mainTabLoaded) {
                                    model.mainTabLoaded(true);
                                }                                
                            }
                            hideSpinner();
                        });
                    });
                });
            };
            //
            //Форма проекта
            self.ShowProject = function (id) {
                if (isSpinnerActive != true)
                    showSpinner();
                $.when(operationIsGrantedD(789)).done(function (project_update) {
                    var ctrl = undefined;
                    var mod = null;
                    ctrl = new fc.control('projectForm', 'projectForm', getTextResource('Project'), true, true, true, 800, 600, null, null, 'data-bind="template: {name: \'TimeManagement/ProjectForm\', afterRender: AfterRender}"');
                    if (!ctrl.Initialized)
                        return;
                    var ctrlD = ctrl.Show();
                    //
                    require(['models/TimeManagement/ProjectForm'], function (vm) {
                        var region = $('#' + ctrl.GetRegionID());
                        mod = new vm.ViewModel(!project_update, region, id);
                        mod.CloseForm = function () {
                            ctrl.BeforeClose = null;
                            ctrl.Close();
                        };

                        $.when(ctrlD, mod.Load(id)).done(function () {
                            ko.applyBindings(mod, document.getElementById(ctrl.GetRegionID()));
                            if (!ko.components.isRegistered('projectFormCaptionComponent'))
                                ko.components.register('projectFormCaptionComponent', {
                                    template: '<span data-bind="text: $captionText"/>'
                                });
                            ctrl.BindCaption(mod, "component: {name: 'projectFormCaptionComponent', params: {  $captionText: getTextResource(\'Project\') + (\' №\' + $data.project().Number() )} }");
                            hideSpinner();
                        });
                    });
                });
            };
            //
            //Форма просмотра RFC
            //ИД RFC, режим формы
            self.ShowRFC = (id, mode, params) => {
                let isReadOnly = (mode & self.Mode.ReadOnly) == self.Mode.ReadOnly;
                if (!isSpinnerActive) {
                    showSpinner();
                }
                
                $.when(userD, operationIsGrantedD(386)).done((user, rfc_update) => {
                    if (!user.HasRoles || !rfc_update) {
                        isReadOnly = true;
                    }
                        
                    const { createControl } = self.FormHelpers()
                    const controlInfo = createControl('rfcForm', 'RFCForm', 'RFC', params);

                    if (!controlInfo.Initialized) {
                        const { dontInitialized } = self.FormHelpers();
                        dontInitialized('rfc');

                        return;
                    }

                    const controlInfoShowPromise = controlInfo.Show();
                    controlInfo.ExtendSize(1000, 800);

                    require(['models/SDForms/RFCForm'], (vm) => {
                        const region = $(`#${controlInfo.GetRegionID()}`);
                        const model = new vm.ViewModel(isReadOnly, region, id);

                        const oldSizeChanged = controlInfo.SizeChanged;
                        controlInfo.SizeChanged = () => {
                            oldSizeChanged();
                            model.SizeChanged();
                        };

                        controlInfo.BeforeClose = () => {
                            const validate = model.DynamicOptionsService.Validate();
                            if (!validate.valid) {
                                validate.callBack();
                                return false;
                            };

                            const { beforeClose } = self.FormHelpers();
                            return beforeClose(model);
                        };

                        model.CloseForm = () => {
                            controlInfo.BeforeClose = null;
                            controlInfo.Close();
                        };

                        model.ControlForm = controlInfo;
                        const loadModelPromise = model.Load(id);

                        ko.applyBindings(model, document.getElementById(controlInfo.GetRegionID()));

                        $.when(controlInfoShowPromise, loadModelPromise).done((frm, loadResult) => {
                            if (!loadResult) { //force close
                                model.CloseForm();
                            }
                            else {
                                if (!ko.components.isRegistered('rfcFormCaptionComponent'))
                                    ko.components.register('rfcFormCaptionComponent', {
                                        template: '<span class="color-point" style="margin-right:5px; background: rgb(255,255,255);" data-bind="style: { background: $priorityColor}" /><span data-bind="text: $captionText"/>'
                                    });
                                controlInfo.BindCaption(model, "component: {name: 'rfcFormCaptionComponent', params: { $priorityColor: $data.RFC().PriorityColor, $captionText: getTextResource(\'RFC\') + ($data.RFC() != null ? (\' №\' + $data.RFC().Number() + \' \' + $data.RFC().Summary()) : \'\')} }");
                                //
                                if (model.mode && model.modes) {
                                    model.mode(model.modes.main);
                                }
                                //
                                if (model.mainTabLoaded) {
                                    model.mainTabLoaded(true);
                                }
                                    
                            }
                            hideSpinner();
                        });
                    });
                });
            };
            //
            //общий метод открытия карточки объекта
            self.ShowObjectForm = function (objectClassID, objectID, formMode, params) {
                if (formMode == undefined || formMode == null)
                    formMode = self.Mode.Default;
                //
                if (formMode == self.Mode.ForceEngineer && objectClassID == 701)//IMSystem.Global.OBJ_CALL
                    self.ShowCallByContext(objectID, { useView: false });
                    //
                else if (objectClassID == 701) //IMSystem.Global.OBJ_CALL
                    self.ShowCallByContext(objectID, params);
                else if (objectClassID == 119) //IMSystem.Global.OBJ_WORKORDER
                    self.ShowWorkOrder(objectID, formMode, params);
                else if (objectClassID == 702) //IMSystem.Global.OBJ_PROBLEM
                    self.ShowProblem(objectID, formMode, params);
                else if (objectClassID == 703) //IMSystem.Global.OBJ_RFC
                    self.ShowRFC(objectID, formMode, params);
                else if (objectClassID == 823) //IMSystem.Global.OBJ_MassIncident
                    self.ShowMassIncident('/api/massIncidents/' + objectID, formMode, params)
                else if (objectClassID == 137) //IMSystem.Global.OBJ_KBArticle
                {
                    var fh = new ufh.formHelper(isSpinnerActive);
                    fh.ShowKBAView(objectID);
                }
                else if (objectClassID == 371) //IMSystem.Global.OBJ_Project
                    self.ShowProject(objectID);
            };
            //
            //вызов формы выбора пользователя
            self.ShowUserSearchForm = function (userInfo, onSelected) {
                require(['ui_forms/User/frmUserSearch'], function (jsModule) {
                    jsModule.ShowDialog(userInfo, onSelected, isSpinnerActive);
                });
            };
            //
            //вызов формы 
            self.ShowKbaSendEmail = function (kbArticle) {
                require(['ui_forms/KB/frmKBASendEmail'], function (jsModule) {
                    jsModule.ShowDialog(kbArticle, isSpinnerActive);
                });

            };
            //вызов формы для отправки email
            self.ShowSendEmailForm = function (options) {
                require(['ui_forms/Email/frmUserSendEmail'], function (jsModule) {
                    jsModule.ShowDialog(options, isSpinnerActive);
                });

            };
        }
    }
    return module;
});