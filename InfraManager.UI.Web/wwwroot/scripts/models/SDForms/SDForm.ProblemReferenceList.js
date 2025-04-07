define(['knockout', 'jquery', 'ajax', 'imList'], function (ko, $, ajaxLib, imLib) {
    var module = {
        //сущность knockout, идентификатор класса сущности, селектор ajax-крутилки
        LinkList: function (ko_object, objectClassID, ajaxSelector, readOnly_object, canEdit_object) {
            var self = this;
            //
            self.AllCallReferences = undefined;
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
                    retval = ko_object().ProblemsCount();
                if (retval <= 0)
                    return null;
                if (retval > 99)
                    return '99';
                else
                    return '' + retval;
            });
            self.ShowObjectForm = function (pRef) {//отображает форму элемента списка
                showSpinner();
                require(['sdForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowProblem(pRef.ID, self.ReadOnly() == true ? fh.Mode.ReadOnly : fh.Mode.Default);
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
            self.ObjectUpdateIsGranted = function () {
                return self.CanEdit();
            };

            var imListOptions = {};//параметры imList для списка 
            {
                imListOptions.aliasID = 'ID';

                imListOptions.LoadAction = function () {
                    var retvalD = $.Deferred();
                    $.when(ko_object().getProblems($(ajaxSelector))).done(function (callRefs) {
                        if (callRefs && callRefs.length > 0) {
                            self.AllCallReferences = callRefs;
                            var problemIDs = callRefs.map(cr => cr.ObjectID);
                            var req = { 'IDs': problemIDs };
                            $.ajax({
                                method: "GET",
                                dataType: "json",
                                url: "/api/problems",
                                data: req,
                                traditional: true,
                                success: function (pList) {
                                    if (pList) {
                                        require(['models/SDForms/SDForm.ProblemReference'], function (prLib) {
                                            var retval = [];
                                            ko.utils.arrayForEach(pList, function (item) {
                                                retval.push(new prLib.ProblemReference(self.imList, item));
                                            });
                                            $.when(self.LoadGrantedOperations()).done(function () {
                                                retvalD.resolve(retval);
                                            });
                                        });
                                    }
                                    else {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SDForm.ProblemReferenceList.js, LoadAction]', 'error');
                                        });
                                        retvalD.resolve([]);
                                    }
                                }
                            })
                        }
                    });
                    return retvalD.promise();
                };

                imListOptions.PushAction = function (list) {
                    if (list) {
                        var retvalD = $.Deferred();
                        require(['models/SDForms/SDForm.ProblemReference'], function (pLib) {
                            var retval = [];
                            ko.utils.arrayForEach(list, function (item) {
                                retval.push(new pLib.ProblemReference(self.imList, item));
                            });
                            retvalD.resolve(retval);
                        });
                    }
                    return retvalD.promise();
                }
            }
            self.imList = new imLib.IMList(imListOptions);
            //
            self.ShowUserColumn = ko.computed(function () {
                if (self.imList && self.imList.List() && self.imList.List().length > 0) {
                    var exists = ko.utils.arrayFirst(self.imList.List(), function (el) {
                        return el.OwnerObj() != null;
                    });
                    //
                    if (exists == null)
                        return false;
                    else return true;
                }
                else return false;
            });
            //
            operationsOptions = {};
            {
                operationsOptions.Text = 'addProblemReference';
                operationsOptions.Validator = function () { return false; };
                operationsOptions.IsGranted = function (grantedOperations) { return self.ObjectUpdateIsGranted(grantedOperations); };
                operationsOptions.Command = function () {
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSearcherLite([702], objectClassID, ko_object, self.imList);
                    });
                };
            }
            self.imList.AddOperation(operationsOptions);
            //
            operationsOptions = {};
            {
                operationsOptions.Text = 'addNewProblem';
                operationsOptions.Validator = function () { return false; };
                operationsOptions.IsGranted = function (grantedOperations) { return self.ObjectUpdateIsGranted(grantedOperations) && self.OperationIsGranted(grantedOperations, module.Operations.OPERATION_Problem_Add); };
                operationsOptions.Command = function () {
                    showSpinner();

                    require(['registrationForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var s = fh.ShowProblemRegistration(null, objectClassID, ko_object().ID());
                        $.when(s).done(function (newProblemId) {
                            if (newProblemId) {
                                ko_object().addProblem(newProblemId);
                            }
                        });
                    });
                };
            }
            self.imList.AddOperation(operationsOptions);
            
            operationsOptions = {};
            {
                operationsOptions.Text = getTextResource('RemoveProblem');
                operationsOptions.Validator = function () { return self.parentList == null };
                operationsOptions.IsGranted = function (grantedOperations) { return self.ObjectUpdateIsGranted(grantedOperations) && self.OperationIsGranted(grantedOperations, module.Operations.OPERATION_Problem_Delete); };
                operationsOptions.Command = function () {
                    var selectedWorkOrders = self.imList.SelectedItems();
                    if (selectedWorkOrders.length == 0)
                        return;
                    //     
                    var ids = [];
                    var question = '';
                    selectedWorkOrders.forEach(function (el) {
                        ids.push(el.ID);
                        //
                        if (question.length < 200) {
                            question += (question.length > 0 ? ', ' : '') + '№ ' + el.Number();
                            if (question.length >= 200)
                                question += '...';
                        }
                    });
                    //
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('ProblemRemoving') + ': ' + question,
                            text: getTextResource('ConfirmRemoveQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                            function (value) {
                                swal.close();
                                if (value) {
                                    self.ajaxControl.Ajax($(ajaxSelector),
                                        {
                                            dataType: "html",
                                            method: 'DELETE',
                                            data: JSON.stringify(ids),
                                            contentType: "application/json; charset=utf-8",
                                            url: '/api/problems'
                                        }, function () {
                                            ids.forEach(function (id) {
                                                self.imList.TryRemoveByID(id, true);
                                            });
                                        });
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
                    var question = '';
                    selectedWorkOrders.forEach(function (el) {
                        ids.push(el.ID);
                        //
                        if (question.length < 200) {
                            question += (question.length > 0 ? ', ' : '') + '№ ' + el.Number();
                            if (question.length >= 200)
                                question += '...';
                        }
                    });
                    //
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('ReferenceRemoving') + ': ' + question,
                            text: getTextResource('ConfirmRemoveQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                            function (value) {
                                swal.close();
                                if (value) {
                                    ko_object().deleteProblems(ids, $(ajaxSelector),
                                        function (id) {
                                            self.imList.TryRemoveByID(id, true);
                                        });
                                }
                            });
                    });
                };
            }
            self.imList.AddOperation(operationsOptions);
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
            self.parentList;//when use LinkList in searchFormLite.js
            //
            self.OnSelectBtnClick = function () {
                self.disposeSubscriptions();
                //
                if (self.parentList != null) {
                    self.ReferenceToCall();
                }
                else {
                    var numberString = 'IM-PL-' + self.imList.SelectedItems()[0].Number();
                    if (self.SelectedItemNumber)
                        self.SelectedItemNumber(numberString);
                    if (self.callback) {
                        var selectedItem = self.imList.SelectedItems()[0];
                        self.callback(selectedItem, 702);
                    }
                    if (self.closeFunk)
                        self.closeFunk();
                }
            };
            //
            self.ReferenceToCall = function () {
                var ids = [];
                ko.utils.arrayForEach(self.imList.SelectedItems(), function (item) {
                    ids.push(item.ID);
                });                

                $.when(ko_object().addProblems(ids)).done(function () {
                    self.parentList.ReloadAll();
                });
            };
        },
        Operations: {
            OPERATION_Problem_Add: 319,
            OPERATION_Problem_Delete: 320,
            OPERATION_Call_Update: 313,
            OPERATION_MassIncident_Update: 983
        }
    };
    return module;
});