define(['knockout', 'jquery', 'ajax', 'models/SDForms/SDForm.User'], function (ko, $, ajax, userLib) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('Contract_ContactListTab');
            self.Template = '../UI/Forms/Asset/Contracts/frmContract_contactPersonListTab';
            self.IconCSS = 'contactPersonTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.CanUpdate = vm.CanUpdate;
            self.CanEdit = vm.CanUpdate;//for userlib
            self.userProperties = ko.observable(false);
            //
            self.contactPersonList = ko.observableArray(null);
            self.userList = ko.observableArray(null);
            self.listLoaded = false;
            //
            self.addUser = function () {
                if (!vm.CanUpdate())
                    return;
                //
                showSpinner();
                var obj = vm.object();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    $.when(userD).done(function (user) {
                        var options = {
                            ID: obj.ID(),
                            objClassID: obj.ClassID,
                            fieldName: 'User',
                            fieldFriendlyName: getTextResource('User'),
                            oldValue: null,
                            object: ko.toJS(user),
                            searcherName: 'WebUserSearcherNoTOZ',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [user.UserID],
                            onSave: function (objectInfo) {
                                var exist = ko.utils.arrayFirst(self.userList(), function (exItem) {
                                    return exItem.ID() == objectInfo.ID;
                                });
                                if (!exist) {
                                    self.userList.push(self.getUserByID(objectInfo.ID, userLib.UserTypes.contractUser, false, self.removeUser));
                                }
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                });
            };
            //
            self.addContactPerson = function () {
                showSpinner();
                //
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var saveFunc = function (newValues) {
                        if (!newValues || newValues.length == 0)
                            return;
                        //
                        var retval = [];
                        ko.utils.arrayForEach(newValues, function (el) {
                            if (el && el.ID)
                                retval.push({ ID: el.ID, ClassID: el.ClassID });
                        });
                        //
                        var data = {
                            'ContractID': vm.object().ID(),
                            'DependencyList': retval
                        };
                        //
                        self.ajaxControl.Ajax($('.external-contacts'),
                            {
                                dataType: "json",
                                method: 'POST',
                                data: data,
                                url: '/assetApi/AddContactPerson'
                            },
                            function (model) {
                                if (model.Result === 0) {
                                    ko.utils.arrayForEach(retval, function (el) {
                                        var exist = ko.utils.arrayFirst(self.contactPersonList(), function (exItem) {
                                            return exItem.ID() == el.ID;
                                        });
                                        if (!exist) {
                                            self.contactPersonList.push(self.getUserByID(el.ID, userLib.UserTypes.contactPerson, true, self.removeContactPerson));
                                        }
                                    });
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
                    };
                    fh.ShowSupplierContactPersonList(vm.object, saveFunc);
                });
            };
            //
            self.removeUser = function (user) {
                self.remove(user.ID, 9, user.FullName(), '.internal-contacts');
            };
            //
            self.removeContactPerson = function (user) {
                self.remove(user.ID, 384, user.FullName(), '.external-contacts');//OBJ_SupplierContactPerson: 384
            };
            //
            self.editContactPerson = function (user) {
                showSpinner();

                var updateContactPerson = function (contactPerson) {
                    var exist = ko.utils.arrayFirst(self.contactPersonList(), function (exItem) {
                        return exItem.ID() == contactPerson.ID();
                    });
                    if (exist) {
                        var userData =
                            {
                                ID: contactPerson.ID(),
                                Name: contactPerson.Name(),
                                Family: contactPerson.Surname(),
                                Patronymic: contactPerson.Patronymic(),
                                Phone: contactPerson.Phone(),
                                PhoneInternal: contactPerson.SecondPhone(),
                                Email: contactPerson.Email(),
                                PositionName: contactPerson.PositionName(),
                                SupplierID: contactPerson.SupplierID(),
                                Note: contactPerson.Note(),
                            };
                        exist.MergeUser(userData);
                    }
                };

                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSupplierContactPerson(user.ID(), vm.object().SupplierID(), vm.object().SupplierName(), updateContactPerson);
                });
            };
            //
            self.getUserByID = function (userID, type, canShowForm, removeFunc) {
                var options = {
                    UserID: userID,
                    UserType: type,
                    UserName: null,
                    EditAction: canShowForm ? self.editContactPerson : null,
                    RemoveAction: removeFunc,
                    ShowTypeName: false,
                    ShowRightSideButtons: true,
                };
                var user = new userLib.User(self, options);
                return user;
            };
            //
            self.getUser = function (user, type, canShowForm, removeFunc) {
                var options = {
                    UserID: user.ID,
                    UserType: type,
                    UserName: null,
                    EditAction: canShowForm ? self.editContactPerson : null,
                    RemoveAction: removeFunc,
                    ShowTypeName: false,
                    ShowRightSideButtons: true,
                    UserData:
                    {
                        ID: user.ID,
                        Email: user.Email,
                        Family: user.Family,
                        Name: user.Name,
                        Patronymic: user.Patronymic,
                        PositionName: user.PositionName,
                        Note: user.Note,
                        BuildingName: user.BuildingName,
                        RoomName: user.RoomName,
                        SubdivisionFullName: user.SubdivisionFullName,
                        Fax: user.Fax,
                        FloorName: user.FloorName,
                        OrganizationName: user.OrganizationName,
                        Phone: user.Phone,
                        PhoneInternal: user.PhoneInternal,
                        WorkplaceName: user.WorkplaceName,
                    }
                };
                var user = new userLib.User(self, options);
                return user;
            };
            //
            self.remove = function (objectID, objectClassID, objectFullName, ajaxSelector) {
                require(['sweetAlert'], function (swal) {
                    swal({
                        title: getTextResource('Removing') + ': ' + objectFullName,
                        text: getTextResource('ConfirmRemoveQuestion'),
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
                            var data =
                                {
                                    contractID: vm.object().ID(),
                                    objectID: objectID,
                                    objectClassID: objectClassID
                                };
                            self.ajaxControl.Ajax($(ajaxSelector),
                            {
                                data: data,
                                url: '/assetApi/ContractRemoveUser',
                                method: 'POST'
                            },
                            function (response) {
                                if (response === 0) {
                                    if (objectClassID == 9) {//obj_user
                                        var exist = ko.utils.arrayFirst(self.userList(), function (exItem) {
                                            return exItem.ID == objectID;
                                        });
                                        if (exist) {
                                            self.userList.remove(exist);
                                        }
                                    }
                                    if (objectClassID == 384) {//OBJ_SupplierContactPerson
                                        var exist = ko.utils.arrayFirst(self.contactPersonList(), function (exItem) {
                                            return exItem.ID == objectID;
                                        });
                                        if (exist) {
                                            self.contactPersonList.remove(exist);
                                        }
                                    }
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContract_contactPersonListTab.js, Load]', 'error');
                                    });
                                }
                            });
                        }
                    });
                });
            };
            //
            self.getList = function () {
                if (self.listLoaded)
                    return;
                //
                var retval = $.Deferred();
                //
                $.when(operationIsGrantedD(71)).done(function (userProperties) {//OPERATION_PROPERTIES_USER
                    self.userProperties(userProperties);
                    self.ajaxControl.Ajax($('.contract-contacts'),
                    {
                        data: { contractID: vm.object().ID() },
                        url: '/assetApi/GetContractUserList',
                        method: 'GET'
                    },
                    function (response) {
                        if (response.Result === 0) {
                            self.contactPersonList.removeAll();
                            self.userList.removeAll();
                            //
                            ko.utils.arrayForEach(response.ContactPersonList, function (item) {
                                self.contactPersonList.push(self.getUser(item, userLib.UserTypes.contactPerson, true, self.removeContactPerson));
                            });
                            //
                            ko.utils.arrayForEach(response.UserList, function (item) {
                                self.userList.push(self.getUser(item, userLib.UserTypes.contractUser, false, self.removeUser));
                            });
                            //
                            retval.resolve(true);
                            self.listLoaded = true;
                        }
                        else {
                            retval.resolve(false);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContract_contactPersonListTab.js, Load]', 'error');
                            });
                        }
                    });
                });
                //
                return retval;
            };
            //
            //when object changed
            self.init = function (obj) {

            };
            //when tab selected
            self.load = function () {
                if (!vm.object().ID())
                {
                    vm.object().ID.subscribe(function () {
                        self.getList();
                    });
                    return;
                }
                //
                self.getList();
            };
            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
            };
        }
    };
    return module;
});