define(['knockout', 'jquery', 'ajax', 'dateTimeControl', 'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents', './Table.Icons'], function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents, m_icons) {
    var module = {
        ViewModel: function (listModeModel, oldTableModel) {
            var self = this;
            //
            {//bind contextMenu
                self.listViewContextMenu = ko.observable(null);
            }
            //
            {//events of listView
                self.listView = null;
                //
                self.listViewInit = function (listView) {
                    self.listView = listView;
                    m_helpers.init(self, listView);//extend self
                    //
                    oldTableModel.load = function () {
                        //calls when viewName is changed, but ko.listView reload data itself
                        listView.options.settingsName(listModeModel.viewName());
                    };
                    oldTableModel.Reload = function () {
                        //calls when filter changed
                        return listView.load();
                    };
                    //
                    $.when(userD).done(function (user) {
                        //ko.listView reload data itself when settingsName changed
                        listView.options.settingsName(listModeModel.viewName());
                    });
                };
                self.listViewRender = function (listView, elements) {
                    oldTableModel.listViewRendered.resolve(elements);
                };
                self.listViewRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
                    var retvalD = $.Deferred();
                    $.when(self.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                        if (objectList) {
                            if (startRecordIndex === 0)//reloaded
                                self.clearAllInfos();
                            else
                                objectList.forEach(function (obj) {
                                    var id = self.getObjectID(obj);
                                    self.clearInfoByObject(id);
                                });
                        }
                        retvalD.resolve(objectList);
                    });
                    return retvalD.promise();
                };
                self.listViewRowClick = function (obj) {
                    var classID = self.getObjectClassID(obj);
                    var id = self.getMainObjectID(obj);
                    //
                    var row = self.getRowByID(id);
                    if (row != null)
                        self.setRowAsLoaded(row);
                    //
                    self.showObjectForm(classID, id);
                };
                self.listViewDrawCell = function (obj, column, cell) {
                    var viewName = listModeModel.viewName();
                    cell.text = m_icons.isTextShowingInColumn(obj, column, viewName) === true ? m_cells.textRepresenter(obj, column) : null;
                    cell.imageSource = m_icons.getImageSourceInColumn(obj, column, viewName);
                };
            }
            //
            {//identification
                self.getObjectID = function (obj) {
                    return obj.ID.toUpperCase();
                };
                self.getMainObjectID = function (obj) {
                    return obj.ID.toUpperCase();
                };
                self.getObjectClassID = function (obj) {
                    var viewName = listModeModel.viewName();
                    if (viewName == 'FinanceActivesRequest' || viewName == 'FinancePurchase')
                        return 119; //IMSystem.Global.OBJ_WorkOrder
                    else if (viewName == 'FinanceBudgetRow')
                        return 180; //IMSystem.Global.OBJ_FinanceBudgetRow                   
                    //
                    return -1;
                };
                self.isObjectClassVisible = function (objectClassID) {
                    if (objectClassID == 110 || objectClassID == 117)//OBJ_DOCUMENT or OBJ_NOTIFICATION ~ SDNote
                        return true;//present in all views
                    //
                    var viewName = listModeModel.viewName();
                    if (viewName == 'FinanceActivesRequest' || viewName == 'FinancePurchase')
                        return objectClassID == 119; //IMSystem.Global.OBJ_WorkOrder
                    else if (viewName == 'FinanceBudgetRow')
                        return objectClassID == 180; // IMSystem.Global.OBJ_FinanceBudgetRow
                    else
                        return false;
                };
            }
            //
            {//geting data             
                self.loadObjectListByIDs = function (idArray, unshiftMode) {
                    for (var i = 0; i < idArray.length; i++)
                        idArray[i] = idArray[i].toUpperCase();
                    //
                    var retvalD = $.Deferred();
                    if (idArray.length > 0) {
                        $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
                            if (objectList) {
                                var rows = self.appendObjectList(objectList, unshiftMode);
                                rows.forEach(function (row) {
                                    self.setRowAsNewer(row);
                                    //
                                    var obj = row.object;
                                    var id = self.getObjectID(obj);
                                    self.clearInfoByObject(id);
                                    //
                                    var index = idArray.indexOf(id);
                                    if (index != -1)
                                        idArray.splice(index, 1);
                                });
                            }
                            idArray.forEach(function (id) {
                                self.removeRowByID(id);
                                self.clearInfoByObject(id);
                            });
                            retvalD.resolve(objectList);
                        });
                    }
                    else
                        retvalD.resolve([]);
                    return retvalD.promise();
                };
                self.getObjectListByIDs = function (idArray, unshift) {
                    var retvalD = $.Deferred();
                    if (idArray.length > 0) {
                        $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
                            retvalD.resolve(objectList);
                        });
                    }
                    else
                        retvalD.resolve([]);
                    return retvalD.promise();
                };
                //
                //calls when viewName changed from Table.ListMode
                self.reload = function () {
                    //ko.listView reload data itself when settingsName changed
                    if (self.listView != null)
                        self.listView.options.settingsName(listModeModel.viewName());
                };
                //
                self.ajaxControl = new ajaxLib.control();
                self.isAjaxActive = function () {
                    return self.ajaxControl.IsAcitve() == true;
                };
                //
                self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                    var retvalD = $.Deferred();
                    //
                    var curFilterID = null;
                    var withFinishedWf = false;
                    var afterDayModMS = null;
                    var treeParams = null;
                    //
                    if (oldTableModel.filtersBlockModel && oldTableModel.filtersBlockModel.filtersModel && oldTableModel.filtersBlockModel.filtersModel.currentFilter())
                        curFilterID = ko.toJS(oldTableModel.filtersBlockModel.filtersModel.currentFilter()).ID;
                    //
                    if (oldTableModel.filtersBlockModel.filtersModel) {
                        withFinishedWf = oldTableModel.filtersBlockModel.filtersModel.WithFinishedWorkflow();
                        afterDayModMS = dtLib.GetMillisecondsSince1970(oldTableModel.filtersBlockModel.filtersModel.AfterDateModified());
                    }
                    //
                    var requestInfo = {
                        StartRecordIndex: idArray ? 0 : startRecordIndex,
                        CountRecords: idArray ? idArray.length : countOfRecords,
                        IDList: idArray ? idArray : [],
                        ViewName: listModeModel.viewName(),
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                        CurrentFilterID: curFilterID,
                        WithFinishedWorkflow: withFinishedWf,
                        AfterModifiedMilliseconds: afterDayModMS,
                        TreeSettings: treeParams
                    };
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/finApi/GetListForObject'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                return;
                            }
                            else if (newVal && newVal.Result === 1 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/Finance/Table.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/Finance/Table.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 3 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('AccessError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 7 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('OperationError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 9 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 11 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SqlTimeout'));
                                });
                            }
                            else if (showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Finance/Table.js getData]', 'error');
                                });
                            }
                            //
                            retvalD.resolve([]);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            if (showErrors === true)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js, getData]', 'error');
                                });
                            //
                            retvalD.resolve([]);
                        },
                        null
                    );
                    //
                    return retvalD.promise();
                };
                //
                self.getObjectStateInfo = function (el) {
                    var retD = $.Deferred();

                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'GET',
                            data: null,
                            url: '/sdApi/stateInfo/' + el.ClassID + '/' + el.ID + '/'
                        }, function (result) {
                            if (result) {
                                el.HasState = true;
                                el.NoteCount = result.NoteCount;
                                el.MessageCount = result.MessageCount;
                                el.InControl = result.InControl;
                                el.CanBePicked = result.CanBePicked;
                            }

                            retD.resolve();
                        });

                    return retD.promise();
                }
                //
            }
            //
            {//open object form
                self.showObjectForm = function (classID, id) {
                    var viewName = listModeModel.viewName();
                    //
                    if (classID == 119) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var mode = fh.Mode.Default;
                            //
                            fh.ShowWorkOrder(id, mode);
                        });
                    }
                    else if (classID == 180) {//budgetRow                    
                        showSpinner();
                        require(['financeForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowFinanceBudgetRow(id);//properties mode
                        });
                    }
                };
            }
            //            
            {//server and local(only this browser tab) events                
                self.isFilterActive = function () {
                    if (oldTableModel.filtersBlockModel.filtersModel)
                        return oldTableModel.filtersBlockModel.filtersModel.IsCurrentFilterActive();
                    return false;
                };
                //
                self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                    if (!self.isObjectClassVisible(objectClassID))
                        return;//в текущем списке измененный объект присутствовать не может
                    else if (parentObjectID && objectClassID != 160) {
                        self.onObjectModified(e, objectClassID, parentObjectID, null);//возможно изменилась часть объекта, т.к. в контексте указан родительский объект
                        return;
                    }
                    objectID = objectID.toUpperCase();
                    //
                    var loadOjbect = true;//будем загружать
                    var row = self.getRowByID(objectID);
                    if (row == null) {
                        if (self.isFilterActive() === true) {//активен фильтр => самостоятельно не загружаем                                   
                            loadOjbect = false;
                            self.checkAvailabilityID(objectID);
                        }
                    } else //используем грязное чтение, поэтому такое возможно
                        self.setRowAsOutdated(row);
                    //
                    if (loadOjbect == true) {
                        if (self.isAjaxActive() === true)
                            self.addToModifiedObjectIDs(objectID);
                        else
                            self.reloadObjectByID(objectID);
                    }
                };
                //
                self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                    if (!self.isObjectClassVisible(objectClassID))
                        return;//в текущем списке измененный объект присутствовать не может
                    else if (parentObjectID && objectClassID != 160) {
                        self.onObjectModified(e, objectClassID, parentObjectID, null);//возможно изменилась часть объекта, т.к. в контексте указан родительский объект
                        return;
                    }
                    objectID = objectID.toUpperCase();
                    //
                    var row = self.getRowByID(objectID);
                    if (row == null) {
                        self.checkAvailabilityID(objectID);
                    } else {
                        self.setRowAsOutdated(row);
                        //
                        if (self.isAjaxActive() === true)
                            self.addToModifiedObjectIDs(objectID);
                        else
                            self.reloadObjectByID(objectID);
                    }
                };
                //
                self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                    if (self.SelectedFinanceBudget() && self.SelectedFinanceBudget().ID.toUpperCase() == objectID) {
                        $.when(userD).done(function (user) {
                            user.FinanceBudgetID = null;
                            self.InitializeFinanceBudget();
                            oldTableModel.Reload();
                        });
                    }
                    //
                    if (!self.isObjectClassVisible(objectClassID))
                        return;//в текущем списке удаляемый объект присутствовать не может
                    else if (parentObjectID && objectClassID != 160) {
                        self.onObjectModified(e, objectClassID, parentObjectID, null);//возможно изменилась часть объекта, т.к. в контексте указан родительский объект
                        return;
                    }
                    objectID = objectID.toUpperCase();
                    //
                    self.removeRowByID(objectID);
                    self.clearInfoByObject(objectID);
                };
                //
                //отписываться не будем
                $(document).bind('objectInserted', self.onObjectInserted);
                $(document).bind('local_objectInserted', self.onObjectInserted);
                $(document).bind('objectUpdated', self.onObjectModified);
                $(document).bind('local_objectUpdated', self.onObjectModified);
                $(document).bind('objectDeleted', self.onObjectDeleted);
                $(document).bind('local_objectDeleted', self.onObjectDeleted);
            }
            //
            m_lazyEvents.init(self);//extend self
            //Переопределяем функцию, т.к. в этом списке нет информации о новых объектах
            self.addToModifiedObjectIDs = function (objectID) {
                self.reloadObjectByID(objectID);
            };
            //
            //
            {//budget panel
                self.setSizeOfControls = oldTableModel.setSizeOfControls;
                //
                self.ajaxBudget = new ajaxLib.control();
                self.InitializeFinanceBudget = function () {
                    $.when(userD).done(function (user) {
                        if (user.FinanceBudgetID == null) {
                            self.SelectedFinanceBudget(undefined);
                            return;
                        }
                        self.SelectedFinanceBudget(null);
                        //   
                        self.ajaxBudget.Ajax($('.finance-selectedBudgetName'),
                            {
                                dataType: 'json',
                                url: '/finApi/GetFinanceBudget',
                                method: 'GET',
                                data: { FinanceBudgetID: user.FinanceBudgetID }
                            },
                            function (bugetResult) {
                                if (bugetResult.Result === 0 && bugetResult.Data)
                                    self.SelectedFinanceBudget(bugetResult.Data ? bugetResult.Data : undefined);
                                else
                                    self.SelectedFinanceBudget(undefined);
                            });
                    });
                };
                //
                listModeModel.viewName.subscribe(function (newValue) {
                    self.IsBudgetPanelVisible(newValue == 'FinanceBudgetRow');
                    oldTableModel.setSizeOfControls();
                });
                //
                self.IsBudgetPanelVisible = ko.observable(listModeModel.viewName() == 'FinanceBudgetRow');
                self.IsBudgetPanelVisible.subscribe(function (newValue) {
                    self.InitializeFinanceBudget();
                });
                //
                self.SelectedFinanceBudget = ko.observable(undefined);
                self.SelectedBudgetFullName = ko.computed(function () {
                    var tmp = self.SelectedFinanceBudget();
                    if (tmp === undefined)
                        return getTextResource('FinanceBudget_PromptBudget');
                    else if (tmp === null)
                        return getTextResource('Search_InProgress');
                    else
                        return tmp.FullName;
                });
                self.SelectBudgetClick = function () {
                    showSpinner();
                    require(['financeForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var selectedID = self.SelectedFinanceBudget() ? self.SelectedFinanceBudget().ID : null;
                        fh.ShowFinanceBudgetList(selectedID, function (selectedID) {
                            if (!selectedID)
                                return;
                            $.when(userD).done(function (user) {
                                if (selectedID == user.FinanceBudgetID)
                                    return;
                                //
                                user.FinanceBudgetID = selectedID
                                self.ajaxBudget.Ajax($('.finance-selectedBudgetName'),
                                    {
                                        url: '/accountApi/SetFinanceBudget',
                                        method: 'POST',
                                        data: { '': user.FinanceBudgetID }
                                    },
                                    function (response) {
                                        if (response == false)
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('AjaxError') + '\n[Finance/Table.js, SaveFinanceBudgetID]', 'error');
                                            });
                                        else {
                                            self.InitializeFinanceBudget();
                                            oldTableModel.Reload();
                                        }
                                    });
                            });
                        });
                    });
                };
                //
                self.InitializeFinanceBudget();
            }
        }
    }
    return module;
});