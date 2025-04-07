define(['knockout', 'jquery', 'ajax', 'dateTimeControl', 'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents'], function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents) {
    var module = {
        ViewModel: function (adminToolsVM, onRendered) {
            var self = this;
            //
            {//events of listView
                self.listView = null;
                //
                self.lv_Init = function (listView) {
                    self.listView = listView;
                    m_helpers.init(self, listView);//extend self
                    //
                    self.load = function () {
                        if (listView.options.settingsName() === '')
                            listView.options.settingsName('UserSessionHistoryList');
                        else
                            listView.load();
                    };
                };
                //
                self.lv_Render = function (listView, elements) {
                    onRendered();
                    $('.lvSessionHistory .contentPanel').append($('.outdatedText'));
                };
                self.lv_RetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
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
                    //
                    return retvalD.promise();
                };
                self.lv_RowClick = function (obj) {
                    //empty, don't show forms
                };
            }
            //
            {//identification
                self.getObjectID = function (obj) {
                    return obj.ID.toUpperCase();
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
                self.ajaxControl = new ajaxLib.control();
                self.isAjaxActive = function () {
                    return self.ajaxControl.IsAcitve() == true;
                };
                //
                self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                    var retvalD = $.Deferred();
                    //
                    var requestInfo = {
                        StartRecordIndex: idArray ? 0 : startRecordIndex,
                        CountRecords: idArray ? idArray.length : countOfRecords,
                        IDList: idArray ? idArray : [],
                        ViewName: self.listView.options.settingsName(),
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                        CurrentFilterID: null,
                        WithFinishedWorkflow: false,
                        AfterModifiedMilliseconds: null,
                        TreeSettings: null
                    };
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/accountApi/GetListForObject'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                self.listIsOutDated(false);
                                return;
                            }
                            else if (newVal && newVal.Result === 1 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/Account/AdminTools.SessionHistoryTable.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/Account/AdminTools.SessionHistoryTable.js getData]', 'error');
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
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Account/AdminTools.SessionHistoryTable.js getData]', 'error');
                                });
                            }
                            //
                            retvalD.resolve([]);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            if (showErrors === true)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Account/AdminTools.SessionHistoryTable.js, getData]', 'error');
                                });
                            //
                            retvalD.resolve([]);
                        },
                        null
                    );
                    //
                    return retvalD.promise();
                };
            }
            //            
            {//server and local(only this browser tab) events        
                self.listIsOutDated = ko.observable(false);
                self.onUserSessionChanged = function (e, userID, userAgent) {
                    self.listIsOutDated(true);
                };
                //
                //отписываться не будем
                $(document).bind('userSessionChanged', self.onUserSessionChanged);
            }
            //
            m_lazyEvents.init(self);//extend self
            //Переопределяем функцию, т.к. в этом списке нет информации о новых объектах
            self.addToModifiedObjectIDs = function (objectID) {
                self.reloadObjectByID(objectID);
            };
        }
    }
    return module;
});