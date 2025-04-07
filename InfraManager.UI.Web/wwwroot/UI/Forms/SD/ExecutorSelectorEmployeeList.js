define(['knockout', 'jquery', 'ajax',  'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView'],
    function (ko, $, ajaxLib, m_helpers, m_lazyEvents) {
        var module = {
            Classes: {
                Call: 701,
                WorkOrder: 119,
            },
            ViewModel: function (obj, form) {
                var self = this;
                self.obj = obj;
                self.ajaxControl = new ajaxLib.control();
                self.dispose = function () {
                    //
                    self.ajaxControl.Abort();
                    //
                    if (self.listView != null)
                        self.listView.dispose();
                };
                self.isAjaxActive = function () {
                    return self.ajaxControl.IsAcitve() == true;
                };
                //
                {//events of listView
                    self.listView = null;
                    self.viewName = 'ExecutorSelectorEmployeeList';
                    self.listViewID = 'listView_' + ko.getNewID();//bind same listView to another template instance (when tab changed), without reload list
                    //
                    self.listViewInit = function (listView) {
                        if (self.listView != null)
                            throw 'listView inited already';
                        //
                        self.listView = listView;
                        m_helpers.init(self, listView);//extend self        
                        //
                        self.listView.load();
                    };

                    self.listViewRetrieveItems = function (startRecordIndex, countOfRecords) {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                            if (objectList) {
                                if (startRecordIndex === 0)//reloaded
                                {
                                    self.clearAllInfos();
                                }
                                else
                                    objectList.forEach(function (obj) {
                                        var id = self.getObjectID(obj);
                                        self.clearInfoByObject(id);
                                    });
                            }
                            retvalD.resolve(objectList);
                            self.SizeChanged();
                        });
                        return retvalD.promise();
                    };

                    self.listViewRowClick = function (obj) {
                        obj.ClassID = 9;
                        form.UpdateAddButton(obj);
                        form.SelectedObj(obj);
                        return;
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };
                }
                self.OnResize = function () {//чтобы была красивая прокрутка таблицы, а кнопки при этом оставались видны
                    $('.tableData').css('height', '100%');
                };
                //
                {//contextMenu
                    {//granted operations
                        self.grantedOperations = [];
                        $.when(userD).done(function (user) {
                            self.grantedOperations = user.GrantedOperations;
                        });
                        self.operationIsGranted = function (operationID) {
                            for (var i = 0; i < self.grantedOperations.length; i++)
                                if (self.grantedOperations[i] === operationID)
                                    return true;
                            return false;
                        };
                    }

                    self.CheckData = function () {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(0, 0, null, true)).done(function (objectList) {
                            if (objectList)
                                self.clearAllInfos();
                            //
                            retvalD.resolve(objectList);
                            self.SizeChanged();
                        });
                        return retvalD.promise();
                    };
                }
                //
                self.SizeChanged = function () {
                    var $regionTable = $('.ExecutorTab');
                    $regionTable.find(".region-Table").css("height", "auto");//для скрола на таблице (без шапки)
                    if (self.listView)
                        self.listView.renderTable();
                };
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
                                self.SizeChanged();
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
                                self.OnResize();
                                retvalD.resolve(objectList);
                                self.SizeChanged();
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };
                    //
                    self.ajaxControl = new ajaxLib.control();
                    //
                    self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                        let retvalD = $.Deferred();
                        let requestInfo = { ClassID: self.obj.objectClassID ? self.obj.objectClassID : self.obj.formClassID, };
                        if (requestInfo.ClassID === module.Classes.Call) {
                            requestInfo.UtcDatePromisedMilliseconds = self.obj.ID !== null && self.obj.call() && self.obj.call().UtcDatePromisedJS
                                ? self.obj.call().UtcDatePromisedJS
                                : null;
                        } else if (requestInfo.ClassID === module.Classes.WorkOrder) {
                            requestInfo.UtcDatePromisedMilliseconds = self.obj.ID !== null && self.obj.workOrder() && self.obj.workOrder().UtcDatePromisedJS
                                ? self.obj.workOrder().UtcDatePromisedJS
                                : null;
                        }
                        
                        const skip = idArray ? 0 : startRecordIndex || 0;
                        const take = idArray ? idArray.length : countOfRecords || 0;

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: 'json',
                                method: 'POST',
                                data: JSON.stringify(requestInfo),
                                url: `api/users/reports/workload?take=${take}&skip=${skip}`,
                                contentType: 'application/json'
                            },
                            function (newVal) {
                                retvalD.resolve(newVal || []);
                            });
                        self.SizeChanged();
                        return retvalD.promise();
                    };
                }

                m_lazyEvents.init(self);//extend self
            }
        };
        return module;
    });