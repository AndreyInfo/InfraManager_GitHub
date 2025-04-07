define(['knockout', 'jquery', 'ajax', 'imList', 'models/SDForms/SDForm.Negotiation'], function (ko, $, ajaxLib, imLib, negLib) {
    var module = {
        //сущность knockout, идентификатор класса сущности, селектор ajax-крутилки
        LinkList: function (ko_object, objectClassID, ajaxSelector, readOnly_object, canEdit_object) {
            var self = this;
            //
            self.isLoaded = ko.observable(false);//факт загруженности данных для объекта ko_object()
            self.imList = null;//declared below
            self.ajaxControl = new ajaxLib.control();//единственный ajax для этого списка
            self.IsFinanceMode = ko.computed(function () {
                if (objectClassID == 119 && ko_object != null && ko_object() != null //WORKORDER ONLY
                    && ko_object().WorkOrderTypeClass && ko_object().WorkOrderTypeClass() != 0) //Purchase or ActivesRequest only
                    return true;
                else return false;
            });
            self.FinanceModeType = ko.computed(function () {
                if (self.IsFinanceMode())
                    return ko_object().WorkOrderTypeClass();
                else return null;
            });
            //
            self.CheckData = function (negotitionID) {//функция загрузки списка (грузится только раз). ID согласования, форму которого нужно открыть после загрузки списка
                if (!self.isLoaded()) {
                    $.when(self.imList.Load()).done(function () {
                        //show form

                        //if (negotitionID) {
                        //    var elem = ko.utils.arrayFirst(self.imList.List(), function (el) {
                        //        return el[self.imList.aliasID] == negotitionID;
                        //    });
                        //    self.ShowObjectForm(elem);
                        //    elem.ExpandCollapseClick();
                        //}
                        //

                        self.isLoaded(true);
                    });
                }
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
            self.HaveUnvoted = ko.observable(false);//есть ли непроголосованные текущим пользователем
            self.ItemsCount = ko.computed(function () {//вариант расчета количества элементов (по данным объекта / по реальному количеству из БД)                
                var retval = 0;
                var isNeedToVote = false;
                if (self.isLoaded()) {
                    retval = self.imList.List().length;
                    ko.utils.arrayForEach(self.imList.List(), function (el) {
                        if (el.CanVote())
                            isNeedToVote = true;
                    });
                }
                else if (ko_object != null && ko_object() != null) {
                    retval = ko_object().NegotiationCount();
                    isNeedToVote = ko_object().HaveUnvotedNegotiation();
                }
                //
                self.HaveUnvoted(isNeedToVote);
                //
                if (retval <= 0)
                    return null;
                if (retval > 99)
                    return '99';
                else
                    return '' + retval;
            });
            self.ShowObjectForm = function (negotiation) {//отображает форму элемента списка               
                showSpinner();
                require(['usualForms'], function (module) {
                    var options = {
                        ID: negotiation.ID,
                        Theme: negotiation.Theme(),
                        Mode: negotiation.Mode(),
                        Status: negotiation.Status(),
                        UtcDateVoteEnd: negotiation.UtcDateVoteEnd(),
                        UtcDateVoteStart: negotiation.UtcDateVoteStart(),
                        UsersList: negotiation.UserList,
                        IsFinanceMode: self.IsFinanceMode(),
                        FinanceModeType: self.FinanceModeType(),
                        SettingCommentPlacet: negotiation.SettingCommentPlacet(),
                        SettingCommentNonPlacet: negotiation.SettingCommentNonPlacet()
                    };
                    //
                    var fh = new module.formHelper(true);
                    fh.ShowNegotiation(negotiation.ID, ko_object().ID(), objectClassID, self.CanEdit, function (negotiationID) {
                        $(document).trigger('local_objectUpdated', [160, negotiationID, ko_object().ID()]);//OBJ_NEGOTIATION
                    }, options);
                });
            };
            //
            var imListOptions = {};//параметры imList для списка 
            {
                imListOptions.aliasID = 'ID';
                //
                imListOptions.LoadAction = function () {
                    var data = {
                        'ObjectID': ko_object().ID(),
                        'ObjectClassID': objectClassID
                    };
                    var retvalD = $.Deferred();
                    self.ajaxControl.Ajax($(ajaxSelector),
                        {
                            dataType: "json",
                            method: 'GET',
                            data: data,
                            url: '/api/negotiations'
                        },
                        function (negotiations) {
                            require(['models/SDForms/SDForm.Negotiation'], function (negotiationLib) {
                                var retval = [];
                                ko.utils.arrayForEach(negotiations, function (item) {
                                    retval.push(new negotiationLib.Negotiation(self.imList, item, self.IsFinanceMode, self.FinanceModeType, canEdit_object, readOnly_object));
                                });
                                retvalD.resolve(retval);
                            });
                        });
                    return retvalD.promise();
                };
                //
                imListOptions.ReloadByIDAction = function (negotiation, id) {
                    var retD = $.Deferred();
                    //
                    if (!self.isLoaded() || !id) {
                        retD.resolve(false);
                        return retD.promise();
                    }

                    //
                    var data = {
                        'EntityID': ko_object().ID(),
                        'EntityClassId': objectClassID,
                        'NegotiationID': id
                    };
                    self.ajaxControl.Ajax($(ajaxSelector),
                        {
                            dataType: "json",
                            method: 'GET',
                            data: data,
                            url: '/api/negotiations/' + id
                        },
                        function (newState) {

                            if (negotiation) {
                                negotiation.Merge(newState);
                            } else {
                                self.imList.TryRemoveByID(id);
                                //
                                self.imList.List.push(new negLib.Negotiation(self.imList, newState, self.IsFinanceMode, self.FinanceModeType, canEdit_object, readOnly_object));
                                self.imList.List.valueHasMutated();
                            }

                            retD.resolve(true);
                        },
                        function () {
                            retD.resolve(false);
                        });
                    //
                    return retD.promise();
                };
            }
            self.imList = new imLib.IMList(imListOptions);
            //
            //
            {//действия над списком
                var operationsOptions = {};
                {
                    operationsOptions.Text = getTextResource('NegotiationStart');
                    operationsOptions.Command = function (negArray) {
                        var PostD = $.Deferred();
                        $.when(userD).done(function (user) {
                            require(['sweetAlert'], function () {
                                var nameList = '';
                                ko.utils.arrayForEach(negArray, function (el) {
                                    nameList += el.Theme() + '\n';
                                });
                                //
                                swal({
                                    title: getTextResource('NegotiationOperationCaption'),
                                    text: getTextResource('NegotiationStartQuestion') + ' ' + nameList,
                                    showCancelButton: true,
                                    closeOnConfirm: false,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                function (value) {
                                    swal.close();
                                    //
                                    if (value == true) {
                                        showSpinner();
                                        //
                                        var promises = [];
                                        ko.utils.arrayForEach(negArray, function (el) {

                                            var promise = negLib.StartNegotiation(el.ID, function (negotiation) {
                                                $(document).trigger('local_objectUpdated', [160, el.ID, ko_object().ID()]);//OBJ_NEGOTIATION для обновления в списке
                                            });
                                            promises.push(promise);
                                        });
                                        //
                                        $.when.apply($, promises).done(function () {
                                            hideSpinner();
                                            PostD.resolve();
                                        });
                                    }
                                    else PostD.resolve();
                                });
                            });
                        });
                        return PostD.promise();
                    };
                    operationsOptions.Validator = function (negArray) {
                        if (self.imList.SelectedItemsCount() == 0)
                            return false;
                        //
                        var exist = ko.utils.arrayFirst(negArray, function (el) {
                            return el.Status() != 0;
                        });
                        return (exist == null);
                    };
                }
                self.imList.AddOperation(operationsOptions);
                //
                operationsOptions = {};
                {
                    operationsOptions.Text = getTextResource('NegotiationRemove');
                    operationsOptions.Command = function (negArray) {
                        var PostD = $.Deferred();
                        $.when(userD).done(function (user) {
                            require(['sweetAlert'], function () {
                                var nameList = '';
                                ko.utils.arrayForEach(negArray, function (el) {
                                    nameList += el.Theme() + '\n';
                                });
                                //
                                swal({
                                    title: getTextResource('NegotiationOperationCaption'),
                                    text: getTextResource('NegotiationDeleteQuestion') + ' ' + nameList,
                                    showCancelButton: true,
                                    closeOnConfirm: false,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                function (value) {
                                    swal.close();
                                    //
                                    if (value == true) {
                                        showSpinner();
                                        //
                                        var promises = [];
                                        ko.utils.arrayForEach(negArray, function (el) {
                                            var promise = negLib.DeleteNegotiation(el.ID);
                                            promises.push(promise);
                                            $.when(promise).done(function () {
                                                $(document).trigger('local_objectDeleted', [160, el.ID, ko_object().ID()]);//OBJ_NEGOTIATION для обновления в списке
                                            });
                                        });
                                        //
                                        $.when.apply($, promises).done(function () {
                                            hideSpinner();
                                            PostD.resolve();
                                        });
                                    }
                                    else PostD.resolve();
                                });
                            });
                        });
                        return PostD.promise();
                    };
                }
                self.imList.AddOperation(operationsOptions);
                //
                operationsOptions = {};
                {
                    operationsOptions.Text = getTextResource('AddNegotiation');
                    operationsOptions.Validator = function () { return self.imList.SelectedItemsCount() == 0; };
                    operationsOptions.Command = function (negArray) {
                        showSpinner();
                        require(['usualForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowNegotiation('', ko_object().ID(), objectClassID, self.CanEdit, function (negotiationID) {
                                $(document).trigger('local_objectInserted', [160, negotiationID, ko_object().ID()]);//OBJ_NEGOTIATION
                            },
                            {
                                IsFinanceMode: self.IsFinanceMode(),
                                FinanceModeType: self.FinanceModeType()
                            });
                        });
                    };
                }
                self.imList.AddOperation(operationsOptions);
            }
        }
    };
    return module;
});