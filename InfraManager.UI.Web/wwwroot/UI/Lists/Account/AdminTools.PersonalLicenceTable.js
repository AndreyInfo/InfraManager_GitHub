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
                            listView.options.settingsName('PersonalLicenceList');
                        else
                            listView.load();
                    }
                };
                self.lv_Render = function (listView, elements) {
                    onRendered();
                };
                self.lv_RetrieveItems = function () {
                    var retvalD = $.Deferred();
                    $.when(self.getObjectList(null, true)).done(function (objectList) {
                        if (objectList)
                            self.clearAllInfos();
                        retvalD.resolve(objectList);
                    });
                    return retvalD.promise();
                };
                self.lv_RowClick = function (obj) {
                    //empty, don't show forms
                };
            }
            //
            {//identification
                self.getObjectID = function (obj) {
                    return obj.UserID.toUpperCase();
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
                        $.when(self.getObjectList(idArray, false)).done(function (objectList) {
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
                        $.when(self.getObjectList(idArray, false)).done(function (objectList) {
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
                self.getObjectList = function (idArray, showErrors) {
                    var retvalD = $.Deferred();
                    //
                    var requestInfo = {
                        IDList: idArray ? idArray : []
                    };
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/accountApi/GetPersonalLicenceList'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                return;
                            }
                            else if (newVal && newVal.Result === 1 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/Account/AdminTools.PersonalLicenceTable.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/Account/AdminTools.PersonalLicenceTable.js getData]', 'error');
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
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Account/AdminTools.PersonalLicenceTable.js getData]', 'error');
                                });
                            }
                            //
                            retvalD.resolve([]);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            if (showErrors === true)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Account/AdminTools.PersonalLicenceTable.js, getData]', 'error');
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
                self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                    if (objectClassID != 365)
                        return;
                    objectID = objectID.toUpperCase();
                    //
                    var row = self.getRowByID(objectID);
                    if (row != null)
                        return;
                    //
                    self.reloadObjectByID(objectID);
                };
                //               
                self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                    if (objectClassID != 365)
                        return;
                    objectID = objectID.toUpperCase();
                    //
                    self.removeRowByID(objectID);
                    self.clearInfoByObject(objectID);
                };
                //
                //отписываться не будем
                $(document).bind('objectInserted', self.onObjectInserted);
                $(document).bind('objectDeleted', self.onObjectDeleted);
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