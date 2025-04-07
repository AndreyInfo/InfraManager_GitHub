define(['knockout', 'jquery', 'ajax', 'dateTimeControl', 'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents', './Table.Icons'], function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents, m_icons) {
    var module = {
        ListResources: {
            ProblemForTable: { URI: '/api/problems/reports/allproblems', Method: 'GET' },
            RFCForTable: { URI: '/api/changeRequests/reports/allChangeRequests', Method: 'GET' },
            CallForTable: { URI: '/api/calls/reports/allCalls', Method: 'POST' },
            WorkOrderForTable: { URI: '/api/workorders/reports/allWorkOrders', Method: 'GET'},
            NegotiationForTable: { URI: '/api/negotiations/reports/myNegotiations', Method: 'GET' },
            CommonForTable: { URI: '/api/tasks/reports/mytasks', Method: 'GET' },
            ClientCallForTable: { URI: '/api/calls/reports/callsFromMe', Method: 'GET' },
            CustomControlForTable: { URI: '/api/customControls/reports/underMyControl', Method: 'POST' },
            AllMassIncidentsList: { URI: '/api/massIncidents/reports/allMassIncidents', Method: 'POST' }
        },
        ViewModel: function (listModeModel, oldTableModel) {
            var self = this;
            //
            {//bind contextMenu
                self.listViewContextMenu = ko.observable(null);
            }
            //
            {//events of listView
                self.listView = null;
                //self.resetCount = ko.observable(true);
                self.loadWithoutFilter = ko.observable(false);
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
                    //self.resetCount(false);
                    $.when(self.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                        if (objectList) {
                            if (startRecordIndex === 0)//reloaded
                            //{
                            //    var modifiedIDs = self.modifiedObjectIDs();
                            //    var ids = [];
                            //    if (objectList)
                            //        for (var i = 0; i < objectList.length; i++)
                            //            ids.push(self.getObjectID(objectList[i]));
                            //    //
                            //    for (var i = 0; i < modifiedIDs.length; i++) {
                            //        var id = modifiedIDs[i].toUpperCase();
                            //        var index = ids.indexOf(id);
                            //        if (index != -1)
                            //            self.removeFromModifiedObjectIDs(id);
                            //    }
                                self.clearAllInfos();
                            //}
                            else
                                objectList.forEach(function (obj) {
                                    var id = self.getObjectID(obj);
                                    //self.resetCount(true);
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
                    self.showObjectForm(classID, id, obj.Uri);
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
                    return listModeModel.viewName() == 'NegotiationForTable' ? obj.ID.toUpperCase() : obj.IMObjID.toUpperCase();
                };
                self.getMainObjectID = function (obj) {
                    if (listModeModel.viewName() == 'NegotiationForTable')
                        return obj.ObjectID.toUpperCase();
                    else 
                        return obj.IMObjID.toUpperCase();
                };
                self.getObjectClassID = function (obj) {                   
                    var viewName = listModeModel.viewName();
                    if (viewName == 'CallForTable' || viewName == 'ClientCallForTable')
                        return 701; //IMSystem.Global.OBJ_Call
                    else if (viewName == 'ProblemForTable')
                        return 702; //IMSystem.Global.OBJ_Problem
                    else if (viewName == 'RFCForTable')
                        return 703; //IMSystem.Global.OBJ_RFC
                    else if (viewName == 'WorkOrderForTable')
                        return 119; //IMSystem.Global.OBJ_WorkOrder
                    else if (viewName == 'AllMassIncidentsList')
                        return 823; //IMSystem.Global.OBJ_MassIncident
                    else if (obj) {
                        return obj.ClassID;
                    }                 
                    //
                    return -1;
                };
                self.isObjectClassVisible = function (objectClassID) {
                    if (objectClassID == 110 || objectClassID == 117)//OBJ_DOCUMENT or OBJ_NOTIFICATION ~ SDNote
                        return true;//present in all views
                    //
                    var viewName = listModeModel.viewName();
                    if (viewName == 'CallForTable' || viewName == 'ClientCallForTable')
                        return objectClassID == 701; //IMSystem.Global.OBJ_Call
                    else if (viewName == 'ProblemForTable')
                        return objectClassID == 702; //IMSystem.Global.OBJ_Problem
                    else if (viewName == 'RFCForTable')
                        return objectClassID == 703; //IMSystem.Global.OBJ_RFC
                    else if (viewName == 'WorkOrderForTable')
                        return objectClassID == 119; //IMSystem.Global.OBJ_WorkOrder
                    else if (viewName == 'CommonForTable' || viewName == 'CustomControlForTable')
                        return objectClassID == 701 || objectClassID == 702 || objectClassID == 119 || objectClassID == 823;
                    else if (viewName == 'NegotiationForTable')
                        return objectClassID == 701 || objectClassID == 702 || objectClassID == 703 || objectClassID == 119 || objectClassID == 160 || objectClassID == 823;//obj_negotiation
                    else if (viewName == 'AllMassIncidentsList')
                        return objectClassID == 823;
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
                        //self.resetCount(false);
                        $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
                            if (objectList) {
                                var rows = self.appendObjectList(objectList, unshiftMode);
                                rows.forEach(function (row) {
                                    self.setRowAsNewer(row);
                                    //
                                    var obj = row.object;
                                    var id = self.getObjectID(obj);
                                    //self.resetCount(true);
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
                    //self.resetCount(false);
                    if (idArray.length > 0) {
                        $.when(self.getObjectList(0, 0, idArray, false)).done(function (objectList) {
                            //if (objectList.length != null)
                            //    self.resetCount(true);
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
                    if (self.listView != null)/* {*/
                        //self.resetCount(true);
                        //self.clearAllInfos();
                        self.listView.options.settingsName(listModeModel.viewName());
                    //}
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
                        curFilterID = ko.toJS(oldTableModel.filtersBlockModel.filtersModel.currentFilter().ID).ID;
                    //
                    if (oldTableModel.filtersBlockModel.filtersModel) {
                        withFinishedWf = oldTableModel.filtersBlockModel.filtersModel.WithFinishedWorkflow();
                        afterDayModMS = dtLib.GetMillisecondsSince1970(oldTableModel.filtersBlockModel.filtersModel.AfterDateModified());
                    }
                    //
                    var viewName = listModeModel.viewName();
                    var requestInfo = {
                        StartRecordIndex: idArray ? 0 : startRecordIndex,
                        CountRecords: idArray ? idArray.length : countOfRecords,
                        IDList: idArray ? idArray : [],
                        ViewName: viewName,
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                        CurrentFilterID:curFilterID,
                        WithFinishedWorkflow: withFinishedWf,
                        AfterModifiedMilliseconds: afterDayModMS,
                        TreeSettings: treeParams
                    };

                    var ajaxSettings = {
                        dataType: 'json',
                        method: 'POST',
                        data: requestInfo,
                        url: '/sdApi/GetListForObject',
                        traditional: true
                    };

                    if (module.ListResources[viewName]) {
                        ajaxSettings.url = module.ListResources[viewName].URI;
                        ajaxSettings.contentType = 'application/json';
                        ajaxSettings.method = module.ListResources[viewName].Method;
                        if (module.ListResources[viewName].Method === 'POST') {
                            ajaxSettings.data = JSON.stringify(ajaxSettings.data);
                        }
                    }

                    //self.urlText = self.loadWithoutFilter() ? 'sdApi/GetListForObjectWithoutFilter' : 'sdApi/GetListForObject';
                    ////
                    //self.loadWithoutFilter(false);
                    self.ajaxControl.Ajax(
                        null,
                        ajaxSettings,
                        function (newVal) {
                            if (module.ListResources[viewName]) {
                                retvalD.resolve(newVal);
                                return;
                            }

                            if (newVal && newVal.Result === 0) {
                                retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                return;
                            }
                            else if (newVal && newVal.Result === 1 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
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
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js getData]', 'error');
                                });
                            }
                            //
                            retvalD.resolve([]);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            if (showErrors === true) {
                                if (XMLHttpRequest.status === 404) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('ResourceNotFoundErrorText') + '\n[Lists/SD/Table.js, getData]', 'error');
                                    });
                                } else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js, getData]', 'error');
                                    });
                                }
                            }
                            //
                            retvalD.resolve([]);
                        },
                        null
                    );
                    //
                    return retvalD.promise();
                };

                self.getObjectStateInfo = function (el) {
                    var retD = $.Deferred();
                    var uri = el.ClassID == 823 ? '/api/massincidents/' + el.IMObjID : el.Uri;  // 823 - Mass Incident; 
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'GET',
                            data: null,
                            url: uri + '/summary'
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
            }
            //


            {//open object form
                self.showObjectForm = function (classID, id, uri) {
                    var viewName = listModeModel.viewName();
                    //
                    if (classID == 701) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowCallByContext(id, { columnViewModel: listModeModel.tableModel.listView.columnViewModel });
                        });
                    }
                    else if (classID == 119) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var mode = fh.Mode.Default;
                            if (viewName == 'NegotiationForTable') mode |= fh.Mode.TabNegotiation;
                            //
                            fh.ShowWorkOrder(id, mode, { columnViewModel: listModeModel.tableModel.listView.columnViewModel });
                            //fh.ShowMassIncident(id, mode, { columnViewModel: listModeModel.tableModel.listView.columnViewModel });
                        });
                    }
                    else if (classID == 702) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var mode = fh.Mode.Default;
                            if (viewName == 'NegotiationForTable') mode |= fh.Mode.TabNegotiation;
                            //
                            fh.ShowProblem(id, mode, { columnViewModel: listModeModel.tableModel.listView.columnViewModel });
                        });
                    }
                    else if (classID == 703) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var mode = fh.Mode.Default;
                            if (viewName == 'NegotiationForTable') mode |= fh.Mode.TabNegotiation;
                            //
                            fh.ShowRFC(id, mode, { columnViewModel: listModeModel.tableModel.listView.columnViewModel });
                        });
                    } else if (classID == 823) {
                        showSpinner();
                        require(['sdForms'], function (module) {
                            var fh = new module.formHelper(true);
                            var mode = fh.Mode.Default;
                            if (viewName == 'NegotiationForTable') mode |= fh.Mode.TabNegotiation;
                            //
                            fh.ShowMassIncident(uri, mode, { columnViewModel: listModeModel.tableModel.listView.columnViewModel });
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
                        else {
                            self.reloadObjectByID(objectID);
                        }
                    }
                };
                //

                function updateRow(row, id) {
                    self.setRowAsOutdated(row);
                    //
                    if (self.isAjaxActive() === true)
                        self.addToModifiedObjectIDs(id);
                    else {
                        self.reloadObjectByID(id);
                    }
                }

                self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                    if (!self.isObjectClassVisible(objectClassID))
                        return;//в текущем списке измененный объект присутствовать не может
                    else if (parentObjectID && objectClassID != 160) {
                        self.onObjectModified(e, objectClassID, parentObjectID, null);//возможно изменилась часть объекта, т.к. в контексте указан родительский объект
                        return;
                    }
                    
                    var viewName = listModeModel.viewName();
                    if (viewName == 'NegotiationForTable' && objectClassID != 160) { // обновить строку списка если изменился родительский объект
                        var row = self.getRowByObject(objectID, objectClassID, { ObjectIDName: 'ObjectID', ObjectClassIDName: 'ClassID' });
                        if (row != null) {
                            updateRow(row, row.object.ID);
                        }
                    }
                    //
                    objectID = objectID.toUpperCase();
                    var row = self.getRowByID(objectID);
                    if (row == null) {                       
                        if (viewName == 'CommonForTable' || viewName == 'NegotiationForTable')//пока что. из-за поля SetField
                            return;
                        //
                        self.checkAvailabilityID(objectID);
                    } else {
                        updateRow(row, objectID);
                    }
                };



                //
                self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
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
            //
            //self.loadModifiedObjects = function () {
            //    listModeModel.filtersModel.ResetClick();
            //    self.resetCount(true);
            //};
            //self.clearInfoByObject = function (id) {
            //    id = id.toUpperCase();
            //    //
            //    if (self.resetCount())
            //        self.removeFromModifiedObjectIDs(id);
            //    self.removeFromQueueToReloadIDs(id);
            //    self.removeFromQueueToCheckAvailabilityIDs(id);
            //    self.resetCount(false);
            //};
            //self.clearAllInfos = function () {
            //    self.queueToCheckAvailabilityIDs = [];
            //    self.queueToReloadIDs = [];
            //    if (self.resetCount())
            //        self.modifiedObjectIDs.removeAll();
            //    self.resetCount(false);
            //};
        }
    }
    return module;
});