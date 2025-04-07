define(['knockout',
        'jquery',
        'ajax',
        'dateTimeControl',
        'ui_controls/ListView/ko.ListView.Cells',
        'ui_controls/ListView/ko.ListView.Helpers',
        'ui_controls/ListView/ko.ListView.LazyEvents',
        'ui_controls/ListView/ko.ListView'],
    function (ko,
              $,
              ajaxLib,
              dtLib,
              m_cells,
              m_helpers,
              m_lazyEvents) {
        var module = {
            List: function (vm) {
                var self = this;
                self.ajaxControl = new ajaxLib.control()
                //
                self.SelectedItemsChanged = null;//set in frmSoftwareLicenceList.js
                self.subscriptionList = [];
                //when tab unload
                self.dispose = function () {                    
                    //
                    self.ajaxControl.Abort();
                    //
                    for (var i in self.subscriptionList) {
                        self.subscriptionList[i].dispose();
                    }
                    //                   
                };
                //
                {//events of listView
                    self.listView = null;
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
                        //
                        var subscription = self.listView.rowViewModel.checkedItems.subscribe(function () {
                            var checkedItemsCount = self.listView.rowViewModel.checkedItems().length;
                            self.SelectedItemsChanged(checkedItemsCount);
                        });
                        //
                        self.subscriptionList.push(subscription);
                    };
                    self.listViewRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
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
                        });
                        return retvalD.promise();
                    };
                    self.listViewRowClick = function (obj) {
                        var id = self.getObjectID(obj);
                        //
                        var row = self.getRowByID(id);
                        if (row != null)
                            self.setRowAsLoaded(row);
                        //
                        self.showObjectForm(id);
                    };
                }
                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };
                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 97;//OBJ_SOFTWARE_MODEL
                    };
                }
                //
                {//geting data             
                    self.loadObjectListByIDs = function (idArray, unshiftMode) {
                        var retvalD = $.Deferred();
                        for (var i = 0; i < idArray.length; i++)
                            idArray[i] = idArray[i].toUpperCase();
                        //

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

                    self.isMultiSelect = function () {
                        if (vm.isMultiSelect != null) {
                            return vm.isMultiSelect;
                        } else {
                            return true;
                        }
                    }

                    self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //

                        var treeParams = {
                            FiltrationObjectID: vm.SelectedObjectID(),
                            FiltrationObjectClassID: vm.SelectedObjectClassID(),
                            FiltrationTreeType: 4,
                            FiltrationField: ''
                        };

                        var requestInfo = {
                            StartRecordIndex: idArray ? 0 : startRecordIndex,
                            CountRecords: idArray ? idArray.length : countOfRecords,
                            IDList: idArray ? idArray : [],
                            ViewName: 'SoftModel',
                            SearchRequest: vm.searchPhraseObservable ? vm.searchPhraseObservable() : null,
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request,      
                            TreeSettings: treeParams,
                            UseNoCommercialModel: vm.UseNoCommercialModel(),
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSoftwareModelObject'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                    return;
                                }
                                else if (newVal && newVal.Result === 1 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[SoftwareModelList.js getObjectList]', 'error');
                                    });
                                }
                                else if (newVal && newVal.Result === 2 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[SoftwareModelList.js getObjectList]', 'error');
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
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareModelList.js getObjectList]', 'error');
                                    });
                                }
                                //
                                retvalD.resolve([]);
                            },
                            function (XMLHttpRequest, textStatus, errorThrown) {
                                if (showErrors === true)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareModelList.js getObjectList]', 'error');
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
                m_lazyEvents.init(self);//extend self
            }            
        };
        return module;
    });