define(['knockout', 'jquery', 'ajax', 'usualForms', 'models/SDForms/SDForm.User', 'dateTimeControl'], function (ko, $, ajaxLib, fhModule, userLib, dtLib) {
    var module = {
        ViewModel: function ($region, id, curUser) {
            var self = this;
            //
            self.$region = $region;
            self.ID = id;
            self.IsStarted = ko.observable(false);
            self.AfterRender = function () {
                self.InitializeUser();
                if(id==null)
                self.Object(new module.ObjectModel(null, self));
            };

            self.Object = ko.observable(null);
            //
            {
                self.CanEdit = ko.computed(function () {
                    return true;
                });
            }
        //
            $.when(userD).done(function (user) {
                self.UserParentID = ko.observable(curUser ? curUser : user.UserID);
            })
            //           
            //User
            {
                self.InitializeUser = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var obj = self.Object();
                        if (obj != null) {
                            if (obj.UserLoaded() == false && obj.UserID()) {
                                var options = {
                                    UserID: obj.UserID(),
                                    UserType: userLib.UserTypes.owner,
                                    UserName: null,
                                    EditAction: self.EditUser,
                                    RemoveAction: self.DeleteUser,
                                    CanNote: true
                                };
                                var user = new userLib.User(self, options);
                                obj.User(user);
                                obj.User().TypeName = ko.observable(getTextResource('DeputyProfileSettings'));
                                obj.UserLoaded(true);
                            }
                        } else {
                            obj.UserLoaded(false);
                            obj.User(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditUser, true, true, getTextResource('DeputyProfileSettings')));
                            obj.UserID('');
                        }
                    });
                };
                self.EditUser = function () {
                    if (self.IsStarted() == true)
                        return;
                    showSpinner();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.Object().ID,
                            objClassID: null,
                            fieldName: 'Object.ParentUser',
                            fieldFriendlyName: getTextResource('DeputyProfileSettings'),
                            oldValue: self.Object().UserLoaded() ? { ID: self.Object().User().ID(), ClassID: 9, FullName: self.Object().User().FullName() } : null,
                            object: ko.toJS(self.Object().User()),
                            searcherName: 'UserForDeputySearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: { NoToz: true, ExceptUserIDs: [self.UserParentID()] },
                            nosave: true,
                            onSave: function (objectInfo) {
                                self.Object().UserLoaded(false);
                                self.Object().User(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditUser, true, true, getTextResource('DeputyProfileSettings')));
                                //
                                self.Object().UserID(objectInfo ? objectInfo.ID : '');
                                self.InitializeUser();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //
                self.DeleteUser = function () {
                    if (self.IsStarted() == true)
                        return;
                    if (self.ID == null) {
                        self.Object().UserLoaded(false);
                        self.Object().User(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditUser, true, true, getTextResource('DeputyProfileSettings')));
                        self.Object().UserID('');
                        self.InitializeUser();
                    }
                    else
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var options = {
                            FieldName: 'Object.ParentUser',
                            oldValue: self.Object().UserLoaded() ? { ID: self.Object().User().ID(), ClassID: 9, FullName: self.Object().User().FullName() } : null,
                            onSave: function () {
                                self.Object().UserLoaded(false);
                                self.Object().User(new userLib.EmptyUser(self, userLib.UserTypes.owner, self.EditUser, true, true, getTextResource('DeputyProfileSettings')));
                                //
                                self.Object().UserID('');
                            }
                        };
                        self.DeleteDeputyUser(false, options);
                    });
                };
                //
                //DeleteUser
                {
                    self.ajaxControl_deleteUser = new ajaxLib.control();
                    self.DeleteDeputyUser = function (isReplaceAnyway, options) {
                        var data = {
                            ID: self.Object().ID,
                            Field: options.FieldName,
                            OldValue: options.OldValue == null ? null : JSON.stringify({ 'id': options.OldValue.ID, 'fullName': options.OldValue.FullName }),
                            NewValue: null,
                            Params: null,
                            ReplaceAnyway: isReplaceAnyway
                        };
                        //
                        self.ajaxControl_deleteUser.Ajax(
                            self.$region,
                            {
                                dataType: "json",
                                method: 'POST',
                                url: '/sdApi/SetField',
                                data: data
                            },
                            function (retModel) {
                                if (retModel) {
                                    var result = retModel.ResultWithMessage.Result;
                                    //
                                    if (result === 0) {
                                        if (options.onSave != null)
                                            options.onSave(null);
                                    }
                                    else if (result === 1) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[frmDeputy.js DeleteUser]', 'error');
                                        });
                                    }
                                    else if (result === 2) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[frmDeputy.js DeleteUser]', 'error');
                                        });
                                    }
                                    else if (result === 3) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                                        });
                                    }
                                    else if (result === 5 && isReplaceAnyway == false) {
                                        require(['sweetAlert'], function () {
                                            swal({
                                                title: getTextResource('SaveError'),
                                                text: getTextResource('ConcurrencyError'),
                                                showCancelButton: true,
                                                closeOnConfirm: true,
                                                closeOnCancel: true,
                                                confirmButtonText: getTextResource('ButtonOK'),
                                                cancelButtonText: getTextResource('ButtonCancel')
                                            },
                                                function (value) {
                                                    if (value == true) {
                                                        self.DeleteUser(true, options);
                                                    }
                                                });
                                        });
                                    }
                                    else if (result === 6) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('ObjectDeleted'), 'error');
                                        });
                                    }
                                    else if (result === 7) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                                        });
                                    }
                                    else if (result === 8) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('ValidationError'), 'error');
                                        });
                                    }
                                    else {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[frmDeputy.js DeleteUser]', 'error');
                                        });
                                    }
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[frmDeputy.js, DeleteUser]', 'error');
                                    });
                                }
                            });
                    };
                }
            }         
        //
            {
                self.EditDateDeputyBy = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var obj = self.Object();
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.ID,
                            objClassID: null,
                            fieldName: 'Object.UtcDataDeputyBy',
                            fieldFriendlyName: getTextResource('By'),
                            oldValue: obj.UtcDataDeputyByDT(),
                            nosave: true,
                            onSave: function (newDate) {
                                obj.UtcDataDeputyBy(parseDate(newDate));
                                obj.UtcDataDeputyByDT(new Date(parseInt(newDate)));
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                }
            }
           //
            {
                self.EditDateDeputyWith = function () {
                    if (self.CanEdit() == false)
                        return;
                    if (self.IsStarted() == true)
                        return;
                    //
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var obj = self.Object();
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.ID,
                            objClassID: null,
                            fieldName: 'Object.UtcDataDeputyWith',
                            fieldFriendlyName: getTextResource('With'),
                            oldValue: obj.UtcDataDeputyWithDT(),
                            nosave: true,
                            onSave: function (newDate) {
                                obj.UtcDataDeputyWith(parseDate(newDate));
                                obj.UtcDataDeputyWithDT(new Date(parseInt(newDate)));
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                }
            }
            //
            self.ajaxControl_loadModel = new ajaxLib.control();
            self.Load = function () {
                var retD = $.Deferred();
                //
                if (self.ID == null) {
                    return retD.resolve();
                }
                //
                self.ajaxControl_loadModel.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/api/deputies/' + self.ID
                    },
                    function (newVal) {
                        if (newVal) {
                            self.Object(new module.ObjectModel(newVal, self));
                            self.IsStarted(self.Object().UtcDataDeputyWithDT() < new Date());
                        }
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmDeputy.js, LoadInfo]', 'error');
                            });
                        retD.resolve();
                    },
                    null,
                    function () {
                        retD.resolve();
                    });
                //
                return retD;
            };
            self.ajaxControl_saveDeputy = new ajaxLib.control();
            self.Save = function () {
                var retval = $.Deferred();
                var obj = self.Object();
                //                
                var data = {
                    'ChildUserID': obj.UserID(),
                    'ParentUserID': self.UserParentID(),
                    'UtcDeputySince': dtLib.GetMillisecondsSince1970(obj.UtcDataDeputyWithDT()),
                    'UtcDeputyUntil': dtLib.GetMillisecondsSince1970(obj.UtcDataDeputyByDT())
                };
                //
                if (data.UtcDeputySince && data.UtcDeputyUntil) {
                    if (new Date(parseInt(data.UtcDeputySince)) > new Date(parseInt(data.UtcDeputyUntil))) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('SoftwareLicenceAdd_CheckDate'));
                        });
                        retval.resolve(null);
                        return;
                    }
                }
                if (data.UtcDeputyUntil && self.IsStarted()) {
                    if (new Date(parseInt(data.UtcDeputyUntil)) < new Date()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('DeputyUser_DataEndCannot'));
                        });
                        retval.resolve(null);
                        return;
                    }
                }   

                if (data.UtcDeputySince && !self.IsStarted()) {
                    if (new Date(parseInt(data.UtcDeputySince)) < new Date()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('DeputyUser_DataStartGreate'));
                        });
                        retval.resolve(null);
                        return;
                    }
                }
                if (!data.ChildUserID) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('MustSetUser'));
                    });
                    retval.resolve(null);
                    return;
                }
                if (data.ChildUserID == self.UserParentID()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('DeputyUser_ParentAndChildShouldBeDifferent'));
                    });
                    retval.resolve(null);
                    return;
                }
                if (!data.UtcDeputySince) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_DateStartPrompt'));
                    });
                    retval.resolve(null);
                    return;
                }
                if (!data.UtcDeputyUntil) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractRegistration_DateEndPrompt'));
                    });
                    retval.resolve(null);
                    return;
                }
                //
                showSpinner();
                self.ajaxControl_saveDeputy.Ajax(null,
                    {
                        url: self.ID ? '/api/deputies/' + self.ID : '/api/deputies/',
                        method: self.ID ? 'PUT' : 'POST',
                        dataType: 'json',
                        contentType: 'application/json',
                        data: JSON.stringify(data)
                    },
                    function (response) {
                        hideSpinner();
                        if (response) {
                            retval.resolve(response.ID);
                            return;
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmDeputy.js, Deputy]', 'error');
                            });
                            retval.resolve(null);
                        }
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmDeputy.js, Deputy]', 'error');
                        });
                        retval.resolve(null);
                    });
                //
                return retval.promise();
            }
        },
        ObjectModel: function (obj, parentself) {
            var nself = this;

            if (obj != null && obj.ID)
                nself.ID = obj.ID
            else nself.ID = null;

            if (obj != null && obj.UtcDataDeputyWithSt) {
                nself.UtcDataDeputyWith = ko.observable(parseDate(obj.UtcDataDeputyWithSt));
                nself.UtcDataDeputyWithDT = ko.observable(new Date(parseInt(obj.UtcDataDeputyWithSt)));
            }
            else {
                nself.UtcDataDeputyWith = ko.observable('');
                nself.UtcDataDeputyWithDT = ko.observable(null);
            }

            if (obj != null && obj.UtcDataDeputyBySt) {
                nself.UtcDataDeputyBy = ko.observable(parseDate(obj.UtcDataDeputyBySt));
                nself.UtcDataDeputyByDT = ko.observable(new Date(parseInt(obj.UtcDataDeputyBySt)));
            }
            else {
                nself.UtcDataDeputyBy = ko.observable('');
                nself.UtcDataDeputyByDT = ko.observable(null);
            }

            if (obj != null && obj.ChildUserID)
                nself.UserID = ko.observable(obj.ChildUserID)
            else nself.UserID = ko.observable('');

            nself.UserLoaded = ko.observable(false);
            nself.User = ko.observable(new userLib.EmptyUser(parentself, userLib.UserTypes.owner, parentself.EditUser, true, true, getTextResource('DeputyProfileSettings')));
        }
    };
    return module;

});