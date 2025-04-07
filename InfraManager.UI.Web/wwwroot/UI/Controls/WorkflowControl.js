define(['jquery', 'ajax', 'knockout'], function ($, ajaxLib, ko) {
    var module = {
        //объект jQuery, который является формой; флаг формы observable IsReadOnly(), ko-сущность объекта
        control: function (formRegion, formReadOnly, entity) {
            var self = this;
            //
            self.ReadOnly = ko.observable(false);//режим только чтение
            //
            self.AvailableStateList = ko.observableArray([]);//доступные текущему пользователю переходы
            //
            self.CanOpenMenu = function () { return true; };//проверка возможности раскрытия меню
            self.CanRestart = ko.computed(function () {//проверка возможности переклассификации
                return entity() == null ? false : entity().EntityStateID() != '';
            });
            self.EntityWithoutState = ko.computed(function () {//индикация отсутствия состояния       
                if (entity() == null)//TODO || self.CanEdit() == false)
                    return false;
                return entity().EntityStateID() == '' && self.AvailableStateList().length > 0;
            });
            //
            //
            var createState = function (workflowStateInfo) {
                var thisObj = this;
                //
                thisObj.StateID = workflowStateInfo.StateID;
                thisObj.Text = workflowStateInfo.Text;
                thisObj.ImageBase64 = workflowStateInfo.ImageBase64;
                //
                thisObj.StateClick = function () {
                    self.SetState(thisObj.StateID);
                };
                thisObj.ImageBase64src = function () {
                    if (workflowStateInfo.ImageBase64)
                        return 'data:image/png;base64, ' + workflowStateInfo.ImageBase64;
                    return '';
                };

            };
            var createRestartCommand = function () {
                var thisObj = this;
                //
                thisObj.StateID = null;
                thisObj.Text = getTextResource('Workflow_RestartAction');
                thisObj.ImageBase64 = null;
                thisObj.ImageBase64src = ko.observable(null);
                //
                thisObj.StateClick = function () {
                    require(['sweetAlert'], function () {
                        swal({
                            title: getTextResource('Workflow_RestartQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: true,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                        function (value) {
                            if (value == true)
                                self.RestartWorkflow();
                        });
                    });
                };
            };
            //
            self.ajaxControl = new ajaxLib.control();
            //
            self.MenuClick = function (obj, e) {//раскрытие списка состояний
                e.stopPropagation();
                if (self.CanOpenMenu() == false)
                    return true;
                //
                self.BlockTooltip(false);
                openRegion(formRegion.find('.workflowControl-menu'), e);
                return true;
            };
            //
            self.IsLoading = ko.observable(false);//крутилка над состоянием, сигнализирует о загрузке перечня состояний
            self.IsLoading.subscribe(function (newValue) {
                var workflowBlock = formRegion.find('.workflowBlock')[0];
                if (workflowBlock) {
                    if (newValue == true)
                        showSpinner(workflowBlock);
                    else
                        hideSpinner(workflowBlock);
                }
            });
            //
            self.lockedTimeout = null;//автоматическая перезагрузка, если заблокировано свыше 30 сек
            self.IsFormLocked = ko.observable(false);//блокировка формы объекта из-за наличия для него внешних событий
            self.IsFormLocked.subscribe(function (newValue) {
                clearTimeout(self.lockedTimeout);
                //
                if (newValue == true) {
                    if (!formRegion.hasClass('ajaxElement'))
                        formRegion.addClass('ajaxElement');
                    //
                    self.lockedTimeout = setTimeout(self.LoadAvailableStateList, 30 * 1000);
                }
                else if (formRegion.hasClass('ajaxElement'))
                    formRegion.removeClass('ajaxElement');
            });
            //
            self.blockTooltipInterval = null;
            self.BlockTooltip = function (force) {
                var retval = $.Deferred();
                var obj = formRegion.find('.workflow');
                //
                if (force != true && obj.find('.workflowBlock').attr('tooltipDisabled') == 'true') {
                    retval.resolve();
                    return retval.promise();
                }
                //
                clearInterval(self.blockTooltipInterval);
                obj.find('.workflowBlock').attr('tooltipDisabled', 'true');//break all tooltips by ko
                self.blockTooltipInterval = setInterval(function () {
                    if (obj.find('.workflowControl-menu').css('display') == 'block') //menu opened
                        return;
                    //
                    clearInterval(self.blockTooltipInterval);
                    obj.find('.workflowBlock').removeAttr('tooltipDisabled');
                }, 1000);
                //
                require(['ttControl'], function (tclib) {
                    var ttcontrol = new tclib.control();
                    ttcontrol.destroy(obj);
                    retval.resolve();
                });
                //
                return retval.promise();
            };
            //
            self.RestartWorkflow = function () {
                formRegion.find('.workflowControl-menu').hide();
                var workflowBlock = formRegion.find('.workflow')[0];
                //
                if (entity() == null)
                    return;
                //
                var success = false;
                self.IsFormLocked(true);
                //
                var param = {
                    'entityID': entity().ID(),
                    'entityClassID': entity().ClassID
                };
                self.ajaxControl.Ajax(workflowBlock,
                    {
                        url: '/workflowApi/restartWorkflow?' + $.param(param),
                        method: 'POST',
                        dataType: "json"
                    },
                    function (response) {
                        if (response) {
                            if (response.Result == 0) {//success
                                //wait server before state changed
                                success = true;
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('Workflow_RestartSuccessfull'));
                                });
                            }
                            else if (response.Result == 2) {//BadParamsError
                                formReadOnly(true);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('BadParamsError'), getTextResource('AjaxError') + '\n[workflowControl.js, RestartWorkflow]', 'error');
                                });
                            }
                            else if (response.Result == 6) {//ObjectDeleted
                                formReadOnly(true);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ObjectDeleted'), getTextResource('AjaxError'), 'info');
                                });
                            }
                            else if (response.Result == 4) {//GlobalError
                                formReadOnly(true);
                                require(['sweetAlert'], function () {
                                    swal(response.Message ? response.Message : getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[workflowControl.js, RestartWorkflow]', 'error');
                                });
                            }
                        }
                        else
                            formReadOnly(true);//сервер не ответил - блокируем форму
                    },
                    null,
                    function () {
                        if (success == false)
                            self.IsFormLocked(false);
                    });
            };
            //
            self.SetState = function (targetStateID) {
                formRegion.find('.workflowControl-menu').hide();
                var workflowBlock = formRegion.find('.workflow')[0];
                //
                if (entity() == null)
                    return;
                //
                var success = false;
                self.IsFormLocked(true);
                //
                var param = {
                    'entityID': entity().ID(),
                    'entityClassID': entity().ClassID,
                    'targetStateID': targetStateID
                };
                self.ajaxControl.Ajax(workflowBlock,
                    {
                        url: '/api/workflowStates/' + entity().ClassID + '/' + entity().ID() + '?stateName=' + targetStateID,
                        method: 'PUT',
                        dataType: "text"
                    },
                    function () {
                    },
                    function () {                        
                    },
                    function () {
                        self.IsFormLocked(false);
                    }, {
                        onForbidden: function () {
                            formReadOnly(true);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('AccessError'));
                            })
                        },
                        onNotFound: function () {
                            formReadOnly(true);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ObjectDeleted'), getTextResource('AjaxError'), 'info');
                            });
                        },
                        onInternalServerError: function () {
                            formReadOnly(true);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('UnhandledErrorServer'));
                            });
                        },
                        onUnprocessableEntity: function (responseText) {
                            $.when(self.BlockTooltip(true)).done(function () {
                                require(['ttControl'], function (tclib) {
                                    var ttcontrol = new tclib.control();
                                    ttcontrol.init(formRegion.find('.workflowControl-expander'), { text: responseText, side: 'bottom', showImmediat: true, multilineText: true });
                                });
                            });
                        }
                    });
            };
            //           
            self.LoadAvailableStateList = function () {
                self.IsLoading(true);
                self.AvailableStateList.removeAll();
                //
                if (entity() == null)
                    return;
                //
                $.when(userD).done(function (user) {
                    self.ajaxControl.Ajax(null,
                        {
                            url: '/api/workflows/' + entity().ClassID + '/' + entity().ID(),
                            method: 'GET',
                            dataType: "json"
                        },
                        function (workflow) {
                            self.IsFormLocked(workflow.HasExternalEvents);
                            //
                            if (workflow.States.length > 0) {
                                $.each(workflow.States, function (index, stateInfo) {
                                    if (stateInfo) {
                                        var state = new createState(stateInfo);
                                        self.AvailableStateList().push(state);
                                    }
                                });
                                self.AvailableStateList.valueHasMutated();
                                            //
                                if (self.CanRestart())
                                    $.when(operationIsGrantedD(592)).done(function (result) {//IMSystem.Global.OPERATION_Workflow_Restart
                                        if (result == true) {
                                            var restartCommand = new createRestartCommand();
                                            self.AvailableStateList().push(restartCommand);
                                            self.AvailableStateList.valueHasMutated();
                                        }
                                    });
                                            //
                                entity().EntityStateName(workflow.EntityStateName);
                            }

                            // Workflow может инактивировать дополнительные поля, но
                            // не сделать их активными для Read-only формы.
                            if (!formReadOnly.peek() && workflow.ReadOnly) {
                                formReadOnly(true);
                            }
                        },
                        function (response) {
                            if (response.status !== 0) { // заспамили запросами на один и тот же URL, в случае aborted игнорируем
                                require(['sweetAlert'], function () {
                                    swal(response.responseText == null ? getTextResource('ErrorCaption') : response.responseText, getTextResource('AjaxError'), 'info');
                                });
                                formReadOnly(true);
                            }
                        },
                        function () {
                            self.IsLoading(false);
                        });
                });
            };
            //
            //удаление контрола
            self.Unload = function () {
                self.ajaxControl.Abort();
                clearInterval(self.blockTooltipInterval);
                clearTimeout(self.lockedTimeout);
                //
                self.IsLoading(false);
                self.IsFormLocked(false);
                //
                $(document).unbind('externalEventCreated', self.onExternalEventCreated);
                $(document).unbind('externalEventProcessed', self.onExternalEventProcessed);
                $(document).unbind('workflowOnSaveError', self.onWorkflowOnSaveError);
                //
                self.AvailableStateList.removeAll();
            };
            //
            //загрузка контрола по объекту
            self.Initialize = function () {
                self.ajaxControl.Abort();
                clearTimeout(self.lockedTimeout);
                //
                self.IsLoading(false);
                self.IsFormLocked(false);
                //
                self.LoadAvailableStateList();
            };
            //
            //инициировать запуск обработчика по сохранению (использовать в крайнем случае, т.к. он должен инициироваться какой-либо операцией на сервере)
            self.OnSave = function (URL, onSuccess, ajaxConfig) {
                $.when(self.LoadD).done(function () {
                    if (entity() == null)
                        return;

                    var workflowBlock = formRegion.find('.workflow')[0];
                    self.ajaxControl.Ajax(workflowBlock,
                        ajaxConfig,
                        null,
                        null,
                        function () {
                            if (onSuccess) onSuccess();
                        });
                });
            };
            //
            //
            self.onExternalEventCreated = function (e, objectID) {
                if (entity() != null && objectID == entity().ID())
                    self.IsFormLocked(true);
            };
            self.onExternalEventProcessed = function (e, objectID) {
                if (entity() != null && objectID == entity().ID()) {
                    self.Initialize();//перезагружаем информацию о текущем состоянии и доступных переходах
                }
            };
            self.onWorkflowOnSaveError = function (e, objectClassID, objectID, response) {
                if (entity() != null && objectID == entity().ID() && response) {
                    if (response.Result == 0) {//success
                    }
                    else if (response.Result == 2) {//BadParamsError
                        formReadOnly(true);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('BadParamsError'), getTextResource('AjaxError') + '\n[workflowControl.js, OnSave]', 'error');
                        });
                    }
                    else if (response.Result == 6) {//ObjectDeleted
                        formReadOnly(true);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ObjectDeleted'), getTextResource('AjaxError'), 'info');
                        });
                    }
                    else if (response.Result == 4) {//GlobalError
                        formReadOnly(true);
                        require(['sweetAlert'], function () {
                            swal(response.Message ? response.Message : getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[workflowControl.js, OnSave]', 'error');
                        });
                    }
                }
            };
            //
            //
            $(document).bind('externalEventCreated', self.onExternalEventCreated);
            $(document).bind('externalEventProcessed', self.onExternalEventProcessed);
            $(document).bind('workflowOnSaveError', self.onWorkflowOnSaveError);
        }
    }
    return module;
});