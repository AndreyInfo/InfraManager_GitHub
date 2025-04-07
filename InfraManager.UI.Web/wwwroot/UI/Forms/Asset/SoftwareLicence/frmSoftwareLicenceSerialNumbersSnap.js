define(['knockout', 'jquery', 'ajax', 'formControl', 'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
        'ui_controls/ListView/ko.ListView'],
    function (ko, $, ajax, formControl, m_cells, m_helpers) {
        var module = {
            Form: function (ParentID, ObjectID) {
                var self = this;
                self.ajaxControl = new ajax.control();
                //
                
                self.canAdd = ko.observable(false);
                self.Name = getTextResource('AddSerialNumber');
                self.SerialNumber = ko.observable('');
                self.$isDone = $.Deferred();
                self.ParentID = ParentID;
                self.ObjectID = ObjectID;
                self.CanSaveLicence = false;
                //
                
                self.dispose = function () {
                    self.ajaxControl.Abort();
                };

                {//selection
                    self.selectedItemFreeze = false;
                    self.selectedItems = ko.observableArray([]);
                    self.getItemsInfos = function (items) {
                        var retval = [];
                        items.forEach(function (item) {                            
                            retval.push({
                                ClassID: item.ClassID,
                                ID: item.ID.toUpperCase(),
                            });
                        });
                        return retval;
                    };
                }

                self.grantedOperations = [];
                self.operationIsGranted = function (operationID) {
                    for (var i = 0; i < self.grantedOperations.length; i++)
                        if (self.grantedOperations[i] === operationID)
                            return true;
                    return false;
                };
                self.UserIsAdmin = false;
                $.when(userD).done(function (user) {
                    self.UserIsAdmin = user.HasAdminRole;
                    self.grantedOperations = user.GrantedOperations;
                    self.canAdd(self.operationIsGranted(461));
                    self.CanSaveLicence = self.operationIsGranted(442);
                });                

                self.ajaxControl = new ajax.control();

                self.SnapSerialNumber = function() {
                    const newSerialNumberID = self.selectedItems().ID;
                    
                    var model = {
                        'SoftwareLicenceID': self.ParentID,
                        'ObjectID': self.ObjectID,
                        'SerialNumberID': newSerialNumberID
                    };
                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: model,
                            url: '/assetApi/SnapSoftwareLicenceSerialKey'
                        },
                        function (model) {
                            if (model.Result === 0) {
                                self.$isDone.resolve(true);
                            } else {
                                self.$isDone.resolve(true);
                                if (model.Result === 1) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                    });
                                } else if (model.Result === 2) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                    });
                                } else if (model.Result === 3) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                                    });
                                } else if (model.Result === 8) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('ValidationError'), 'error');
                                    });
                                } else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                    });
                                }
                                //
                            }
                           // retval.resolve();
                        });
                    return self.$isDone.promise();
                }                
                
                self.AddSerialNumber = function () {
                    require(['ui_forms/Asset/SoftwareLicence/frmSoftwareLicenceSerialNumbersAdd'], function (fhModule) {
                        var form = new fhModule.Form(function (newSerialNumber) {
                            if (!newSerialNumber)
                                return;

                            var data = {
                                'SoftwareLicenceID': self.ParentID,
                                'SerialNumber': newSerialNumber
                            };
                            //
                            self.ajaxControl.Ajax($('.external-contacts'),
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: data,
                                    url: '/assetApi/AddSoftwareLicenceSerialNumber'
                                },
                                function (model) {
                                    if (model.Result === 0) {
                                        self.listView.load();
                                    }
                                    else {
                                        if (model.Result === 1) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                            });
                                        }
                                        else if (model.Result === 2) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                            });
                                        }
                                        else if (model.Result === 3) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                                            });
                                        }
                                        else if (model.Result === 8) {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('ValidationError'), 'error');
                                            });
                                        }
                                        else {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[SDForm.LinkList.js, addContactPerson]', 'error');
                                            });
                                        }
                                        //
                                    }
                                });
                        });
                        form.Show();
                    });
                };

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
                        var storedLoad = self.listView.load;
                        self.listView.load = function () {
                            var retvalD = $.Deferred();
                            self.selectedItemFreeze = true;
                            $.when(storedLoad()).done(function () {
                                self.selectedItemFreeze = false;                                
                                retvalD.resolve();
                            });
                            return retvalD.promise();
                        };
                        //
                        self.listView.load();
                    };
                    self.listViewRetrieveItems = function () {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(null, true)).done(function (objectList) {
                            retvalD.resolve(objectList);
                        });
                        return retvalD.promise();
                    };
                    self.listViewRowClick = function (obj) {
                        self.selectedItems([]);                        
                        self.selectedItems(obj);                        
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
                    self.ajaxControl = new ajax.control();
                    self.isAjaxActive = function () {
                        return self.ajaxControl.IsAcitve() == true;
                    };
                    //
                    self.getObjectList = function (idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var requestInfo = {
                            IDList: idArray ? idArray : [],
                            ViewName: 'SoftwareLicenceSerialNumbers',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: self.ParentID,
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSoftwareLicenceSerialNumberObject'
                            },
                            function (newVal) {
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

                    self.getSelectedItems = function () {
                        var selectedItems = self.listView.rowViewModel.checkedItems();
                        //
                        if (!selectedItems)
                            return [];
                        //
                        var retval = [];
                        selectedItems.forEach(function (el) {
                            var item =
                                {
                                    ID: el.ID.toUpperCase(),
                                    ClassID: self.ClassID,
                                    SoftwareLicenceID: el.SoftwareLicenceID,
                                    SerialNumber: el.SerialNumber,
                                    InUseCount: el.InUseCount
                                };
                            retval.push(item);
                        });
                        return retval;
                    };

                    self.clearSelection = function () {
                        self.listView.rowViewModel.checkedItems([]);
                    };

                    self.getConcatedItemNames = function (items) {
                        var retval = '';
                        items.forEach(function (item) {
                            if (retval.length < 200) {
                                retval += (retval.length > 0 ? ', ' : '') + self.getItemName(item);
                                if (retval.length >= 200)
                                    retval += '...';
                            }
                        });
                        return retval;
                    };

                    self.getItemInfos = function (items) {
                        var retval = [];
                        items.forEach(function (item) {
                            retval.push({
                                ClassID: item.ClassID,
                                ID: item.ID
                            });
                        });
                        return retval;
                    };

                    self.getItemName = function (item) {
                        return getTextResource('SerialNumber') + ' \'' + item.SerialNumber + '\'';
                    };
                }
                //show form
                self.Show = function () {
                    showSpinner();
                    var $retval = $.Deferred();
                    //
                    var buttons = [];
                    var forceClose = false;
                    var bCancel = {
                        text: getTextResource('Close'),
                        click: function () {
                            forceClose = true;
                            ctrl.Close();
                        }
                    }                    
                    buttons.push(bCancel);                    

                    var ctrl = undefined;
                    ctrl = new formControl.control(
                        'region_frmSoftwareLicenseSerialNumbersSnap',//form region prefix
                        'settings_frmSoftwareLicenseSerialNumbersSnap',//location and size setting
                        self.Name,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        500, 300,//minSize
                        buttons,//form buttons
                        function () {
                            self.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicenceSerialNumbersSnap\'}"'//attributes of form region
                    );
                    if (!ctrl.Initialized)
                        return;

                    self.$isDone = $retval;
                    
                    self.selectedItems.subscribe(function (newValue) {
                        buttons = [];                        
                        if (newValue != undefined && self.CanSaveLicence) {
                            var bSave = {
                                text: getTextResource('ButtonSave'),
                                click: function () {
                                    var d = self.SnapSerialNumber();
                                    $.when(d).done(function (result) {
                                        if (!result)
                                            return;
                                        //
                                        forceClose = true;
                                        ctrl.Close();
                                    });

                                }
                            }
                            buttons.push(bSave);
                            buttons.push(bCancel);
                        }
                        else {
                            buttons = [];
                            buttons.push(bCancel);
                        }
                        if (!forceClose)
                            ctrl.UpdateButtons(buttons);
                    });
                    
                    ctrl.Show();

                    bindElement = document.getElementById(ctrl.GetRegionID());
                    ko.applyBindings(self, bindElement);

                    hideSpinner();
                    
                    return $retval.promise();
                }
            }
        };
        return module;
    });