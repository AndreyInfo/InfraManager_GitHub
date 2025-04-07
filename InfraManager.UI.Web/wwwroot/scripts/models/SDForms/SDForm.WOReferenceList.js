define(['knockout', 'jquery', 'ajax', 'imList', 'groupOperation'], function (ko, $, ajaxLib, imLib, groupOperation) {
    var module = {
        //сущность knockout, идентификатор класса сущности, селектор ajax-крутилки
        LinkList: function (ko_object, objectClassID, ajaxSelector, readOnly_object, canEdit_object) {
            var self = this;
            //
            self.isLoaded = ko.observable(false);//факт загруженности данных для объекта ko_object()
            self.imList = null;//declared below
            self.ajaxControl = new ajaxLib.control();//единственный ajax для этого списка
            //
            self.CheckData = function () {//функция загрузки списка (грузится только раз)
                if (!self.isLoaded()) {
                    $.when(self.imList.Load()).done(function () {
                        self.isLoaded(true);
                    });
                }
            };
            self.options = (ko_object && ko_object() && ko_object().options) ? ko_object().options : {}; // костылинг
            //
            self.PushData = function (list) {//функция загрузки списка 
                var returnD = $.Deferred();
                $.when(self.imList.Push(list)).done(function () {
                    returnD.resolve();
                });
                return returnD.promise();
            };

            //
            self.ClearData = function () {//функция сброса данных
                self.imList.List([]);
                //
                self.isLoaded(false);
            };
            self.ReadOnly = readOnly_object;//флаг только чтение
            self.CanEdit = canEdit_object;//флаг для редактирования/создания
            //
            self.ItemsCount = ko.computed(function () {//вариант расчета количества элементов (по данным объекта / по реальному количеству из БД)
                var retval = 0;
                if (self.isLoaded())
                    retval = self.imList.List().length;
                else if (ko_object != null && ko_object() != null)
                    retval = ko_object().WorkOrderCount();
                //
                if (retval <= 0)
                    return null;
                if (retval > 99)
                    return '99';
                else
                    return '' + retval;
            });
            self.ShowObjectForm = function (woRef) {//отображает форму элемента списка
                showSpinner();
                require(['sdForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowWorkOrder(woRef.ID, self.ReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);                    
                });
            };
            //
            self.OperationIsGranted = function (grantedOperations, operation) {
                if (grantedOperations)
                    for (var i = 0; i < grantedOperations.length; i++)
                        if (grantedOperations[i] === operation)
                            return true;
                return false;
            };
            self.LoadGrantedOperations = function () {
                var retvalD = $.Deferred();
                //
                if (self.imList.GrantedOperations()) {
                    retvalD.resolve();
                    return retvalD.promise();
                }
                var CurrentUserID;
                $.when(userD).done(function (user) {
                    CurrentUserID = user.UserID;
                });

                self.ajaxControl.Ajax(null,
                {
                    dataType: "json",
                    method: 'GET',                    
                    url: '/api/users/' + CurrentUserID
                },
                function (user) {
                    if (user) {
                        var opList = user.GrantedOperations;
                        self.imList.GrantedOperations([]);
                        for (var i = 0; i < opList.length; i++) {
                            self.imList.GrantedOperations().push(opList[i]);
                        }
                self.imList.GrantedOperations.valueHasMutated();
                retvalD.resolve();
                    }
                });
                return retvalD.promise();
            };
            self.ObjectUpdateIsGranted = function (grantedOperations) {
                return self.CanEdit();
            };
            //
            var imListOptions = {};//параметры imList для списка 
            {
                imListOptions.aliasID = 'ID';
                //
                imListOptions.LoadAction = function () {
                    var data = {
                        'ReferencedObject': {
                            'Id': ko_object().ID(),
                            'ClassId': objectClassID
                        }
                    };
                    var retvalD = $.Deferred();
                    self.ajaxControl.Ajax($(ajaxSelector),
                        {
                            dataType: "json",
                            method: 'GET',
                            data: data,
                            url: '/api/workOrders?referencedObjectID=' + ko_object().ID() + '&referencedObjectClassID=' + objectClassID + '&orderByProperty=Number' 
                        },
                        function (workOrders) {
                            require(['models/SDForms/SDForm.WOReference'], function (woLib) {
                                var retval = [];
                                ko.utils.arrayForEach(workOrders, function (workOrder) {
                                    retval.push(new woLib.WorkOrderReference(self.imList, workOrder));
                                });
                                $.when(self.LoadGrantedOperations()).done(function () { retvalD.resolve(retval); });
                            });
                        });
                    return retvalD.promise();
                };
                
                imListOptions.ReloadByIDAction = function (woRef, id) {                    
                    self.imList.ReloadAll();
                };
                
                imListOptions.PushAction = function (list) {
                    if (list) {
                        var retvalD = $.Deferred();
                        require(['models/SDForms/SDForm.WOReference'], function (wLib) {
                            var retval = [];
                            ko.utils.arrayForEach(list, function (item) {
                                retval.push(new wLib.WorkOrderReference(self.imList, item));
                            });
                            retvalD.resolve(retval);
                        });
                    }
                    return retvalD.promise();
                }
            }
            self.imList = new imLib.IMList(imListOptions);
            //
            operationsOptions = {};
            {
                operationsOptions.Text = 'addWorkOrderReference';
                operationsOptions.Validator = function () { return false; };
                operationsOptions.IsGranted = function (grantedOperations) { return self.ObjectUpdateIsGranted(grantedOperations) && self.OperationIsGranted(grantedOperations, module.Operations.OPERATION_WorkOrder_Add) && objectClassID != 371; };
                operationsOptions.Command = function () {
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSearcherLite([119], objectClassID, ko_object, self.imList);
                    });
                };
            }
            self.imList.AddOperation(operationsOptions);

            var GroupOperationAddViewModel = function (workorderIds) {               
                var me = this;

                module.GroupOperationViewModelBase.call(this, workorderIds, self.options.addMethod); 

                if (self.options.uri) { //TODO: Убрать костыль
                    this._getData = function (item) {
                        return JSON.stringify({ ReferenceID: item.Id });
                    };
                    this._getUrl = function () {
                        return self.options.uri + '/workorders'
                    };
                } else {
                    this._getData = function () {
                        return JSON.stringify({
                            ReferencedObject: {
                                Id: ko_object().ID(),
                                ClassId: objectClassID
                            }
                        });
                    };
                }

                this._onSuccess = function (item, response) {
                    if (self.parentList) {
                        self.parentList.TryReloadByID(item.Id, true);
                    }
                    self.imList.TryRemoveByID(item.Id, true);
                }

                this._onComplete = function () {
                    self.imList.ReloadAll();
                };
            }

            var GroupOperationDeleteViewModel = function (workOrderIds) {

                module.GroupOperationViewModelBase.call(this, workOrderIds, self.options.deleteMethod);

                if (self.options.uri) { //TODO: Убрать костыль
                    this._getData = function () { return null; };
                    this._getUrl = function (item) { return self.options.uri + '/workorders/' + item.Id; };
                } else {
                    this._getData = function () {
                        return JSON.stringify({
                            ReferencedObject: {}
                        });
                    }
                }

                this._onSuccess = function (item) {
                    self.imList.TryRemoveByID(item.Id, true);
                }
            }

            operationsOptions = {};
            {
                operationsOptions.Text = 'addNewWorkOrder';
                operationsOptions.Validator = function () { return false; };
                operationsOptions.IsGranted = function (grantedOperations) { return self.ObjectUpdateIsGranted(grantedOperations) && self.OperationIsGranted(grantedOperations, module.Operations.OPERATION_WorkOrder_Add) && objectClassID != 371; };
                operationsOptions.Command = function () {
                    showSpinner();
                    require(['registrationForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var woReg = fh.ShowWorkOrderRegistration(objectClassID, ko_object().ID());
                        $.when(woReg).done(function (newWOId) {
                            if (newWOId) {
                                self.imList.ReloadAll();
                            }
                        });
                    });
                };
            }

            self.imList.AddOperation(operationsOptions);
            //
            operationsOptions = {};
            {
                operationsOptions.Text = getTextResource('RemoveWorkOrder');
                operationsOptions.Validator = function () { return self.parentList == null };
                operationsOptions.IsGranted = function (grantedOperations) { return self.ObjectUpdateIsGranted(grantedOperations) && self.OperationIsGranted(grantedOperations, module.Operations.OPERATION_WorkOrder_Delete); };
                operationsOptions.Command = function () {
                    var selectedWorkOrders = self.imList.SelectedItems();
                    if (selectedWorkOrders.length == 0)
                        return;
                    //     
                    var ids = [];
                    var refIds = [];
                    var question = '';
                    selectedWorkOrders.forEach(function (el) {
                        ids.push(el.ID);
                        refIds.push(el.ReferenceID);
                        //
                        if (question.length < 200) {
                            question += (question.length > 0 ? ', ' : '') + el.Name();
                            if (question.length >= 200)
                                question += '...';
                        }
                    });
                    
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('WorkOrderRemoving') + ': ' + question,
                            text: getTextResource('ConfirmRemoveQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                        function (value) {
                            swal.close();                            
                            if (value == true) {
                                new GroupOperationDeleteViewModel(ids).start();
                            }
                        });
                    })
                };
            }
            self.imList.AddOperation(operationsOptions);
            //
            operationsOptions = {};
            {
                operationsOptions.Text = getTextResource('RemoveReference');
                operationsOptions.Validator = function () { return self.parentList == null };
                operationsOptions.IsGranted = function (grantedOperations) { return self.ObjectUpdateIsGranted(grantedOperations); };
                operationsOptions.Command = function () {
                    var selectedWorkOrders = self.imList.SelectedItems();
                    if (selectedWorkOrders.length == 0)
                        return;
                    //           
                    var ids = [];
                    var refIds = [];
                    var question = '';
                    selectedWorkOrders.forEach(function (el) {
                        ids.push(el.ID);
                        refIds.push(el.ReferenceID);
                        //
                        if (question.length < 200) {
                            question += (question.length > 0 ? ', ' : '') + el.Name();
                            if (question.length >= 200)
                                question += '...';
                        }
                    });


                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: 'Удаление связи' + ': ' + question,
                            text: 'Вы действительно хотите удалить?',
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                        function (value) {
                            swal.close();
                            
                            if (value == true) {
                                new GroupOperationDeleteViewModel(ids).start();
                            }
                        });
                    });
                };
            }
            self.imList.AddOperation(operationsOptions);
            //
            self.ShowUserColumn = ko.computed(function () {
                if (self.imList && self.imList.List() && self.imList.List().length > 0) {
                    var exists = ko.utils.arrayFirst(self.imList.List(), function (el) {
                        return el.ExecutorObj() != null;
                    });
                    //
                    if (exists == null)
                        return false;
                    else return true;
                }
                else return false;
            });
            //
            self.SetClearButtonsList = null;
            self.SetFilledButtonsList = null;
            //
            //selected items changed event
            {
                self.subscriptionList = [];
                //
                var subscription = self.imList.SelectedItems.subscribe(function (newVal) {
                    if (newVal.length == 1 && (self.SelectedItemNumber || self.callback)) {
                        self.SetFilledButtonsList();
                    }
                    else if (newVal.length != 0 && self.parentList != null) {
                        self.SetFilledButtonsList();
                    }
                    else if (self.SetClearButtonsList) {
                        self.SetClearButtonsList();
                    }
                });
                self.subscriptionList.push(subscription);
                //
                self.disposeSubscriptions = function () {
                    for (var i in self.subscriptionList) {
                        self.subscriptionList[i].dispose();
                    }
                };
            }
            //
            self.SelectedItemNumber = null;
            self.closeFunk = null;
            //
            self.OnSelectBtnClick = function (entityClassID) {
                self.disposeSubscriptions();
                //
                if (self.parentList != null) {
                    self.ReferenceToEntity(entityClassID);
                }
                else {
                    var numberString = 'IM-TS-' + self.imList.SelectedItems()[0].Number();
                    if (self.SelectedItemNumber)
                        self.SelectedItemNumber(numberString);
                    if (self.callback) {
                        var selectedItem = self.imList.SelectedItems()[0];
                        self.callback(selectedItem,119);
                    }
                    if (self.closeFunk)
                        self.closeFunk();
                }
            };

            self.parentList;//when use LinkList in searchFormLite.js
            //
            self.ReferenceToEntity = function (entityClassID) {
                var ids = [];
                ko.utils.arrayForEach(self.imList.SelectedItems(), function (item) {
                    ids.push(item.ID);
                });
                new GroupOperationAddViewModel(ids).start();
            };

            
            {//OnWorkOrderExecutorControl
                self.OnWorkOrderExecutorControl = ko.observable(false);
                self.VisibleOnWorkOrderExecutorControl = ko.observable(true);

                if (ko_object) {
                    ko_object.subscribe(function (newVal) {
                        if (newVal.OnWorkOrderExecutorControl) {
                            self.OnWorkOrderExecutorControl(newVal.OnWorkOrderExecutorControl());
                        }
                    });
                }

                if (objectClassID == 371) {
                    self.VisibleOnWorkOrderExecutorControl(false);
                }

            }
        },
        GroupOperationViewModelBase: function (workorderIds, method) {

            groupOperation.ViewModelBase.call(
                this,
                workorderIds.map(function (workOrderId) {
                    return {
                        Id: workOrderId,
                        Uri: '/api/workorders/' + workOrderId,
                    };
                }), {
                ajax: { contentType: 'application/json', dataType: 'JSON', method: method || 'PUT' },
                batchSize: 1,
                div: null
            });
        },
        Operations: {
            OPERATION_WorkOrder_Add: 301,
            OPERATION_WorkOrder_Delete: 330,
            OPERATION_Call_Update: 313,
            OPERATION_Problem_Update: 323,
            OPERATION_RFC_Update: 386,
        }
    };
    return module;
});