define(['knockout', 'jquery', 'ajax', './OrgStructure.SubdivisionList', './OrgStructure.Attachments', './OrgStructure.CalendarWorkSchedule', 'ui_controls/ContextMenu/ko.ContextMenu'], function (ko, $, ajaxLib, subdivisionList, attachments, schedule) {
    var module = {
        modes: {// режимы: работы, отображения
            view: 'view', // Просмотр
            edit: 'edit', // Редактирование
            create: 'create', // Создание
        },
        //
        ViewModel: function (node, parentNode, selectDivision, mode) {//общая модель представления
            var self = this;
            //
            self.ID = getObjectID(node);
            self.ClassID = !!getObjectClassID(node) ? getObjectClassID(node) : 101;
            self.Name = ko.observable(getObjectName(node));
            self.IconClass = ko.observable(getObjectIconClass(node));
            self.LevelName = getObjectLevelName(self.ClassID);
            //
            self.Node = node;
            self.ParentNode = parentNode;
            //
            self.Path = ko.observable([]);
            self.Note = ko.observable("");
            self.IsLockedForOSI = ko.observable(false);
            self.CalendarWorkSchedule = ko.observable(null);
            //
            {//granted operations
                self.grantedOperations = ko.observableArray([]);
                $.when(userD).done(function (user) {
                    self.grantedOperations(user.GrantedOperations);
                });
                self.operationIsGranted = function (operationID, grantedOperations) {
                    for (var i = 0; i < grantedOperations.length; i++)
                        if (grantedOperations[i] === operationID)
                            return true;
                    return false;
                };
            }
            //
            self.ActiveMode = ko.observable(mode);//выбранное представление по умолчанию
            self.IsEditable = ko.computed(function () {
                return self.ActiveMode() != module.modes.view;
            });
            self.IsEditable.subscribe(function (newValue) {
                if (newValue) {
                    setTimeout(function () {
                        self.UpdateAllTextAreaHeight();
                        self.FocusOnNameTextArea();
                    }, 100);
                }
            });
            //
            self.SubdivisionList = ko.observable(mode != module.modes.create ? new subdivisionList.ViewModel(self.ClassID, self.ID, selectDivision) : null);
            self.Attachments = ko.observable(mode != module.modes.create ? new attachments.ViewModel(self.ClassID, self.ID, self.IsEditable) : null);
            //
            self.Schedule = ko.observable(new schedule.ViewModel(self.CalendarWorkSchedule, self.IsEditable));
            //
            self.Modified = ko.observable(false);
            self.MarkAsModified = function () {
                if (!self.Modified()) {
                    self.Modified(true);
                }
            };
            self.ResetModified = function () {
                if (self.Modified()) {
                    self.Modified(false);
                }
            };
            self.Name.subscribe(function () {
                self.MarkAsModified();
            });
            self.Note.subscribe(function () {
                self.MarkAsModified();
            });
            self.IsLockedForOSI.subscribe(function () {
                self.MarkAsModified();
            });
            self.CalendarWorkSchedule.subscribe(function () {
                self.MarkAsModified();
            });
            //
            self.ajaxControl = new ajaxLib.control();
            //
            self.CheckData = function () {//вызывается извне, сообщает, что пора обновлять/загружать данные
                if (self.ActiveMode() != module.modes.create) {
                    return self.InitNavigationBar().then(function () { return self.LoadOrganization().then(self.ResetModified) });
                } else if (!self.IconClass()) {
                    return self.InitIconClass().then(self.ResetModified);
                }
            };
            self.NavigationBarItemClick = function (node) {
                var nodeID = getObjectID(node);
                var nodeClassID = getObjectClassID(node);
                if (nodeID.toLowerCase() != self.ID.toLowerCase() || nodeClassID != self.ClassID) {
                    selectDivision(node);
                }
            };
            self.IsNoteEmptyOnViewMode = ko.computed(function () {
                return self.ActiveMode() == module.modes.view && (!self.Note() || self.Note() === "");
            })
            self.CanSwitchToEditMode = ko.computed(function () {
                return self.operationIsGranted(227, self.grantedOperations());
            });
            self.SwitchToEditMode = function () {
                self.ActiveMode(module.modes.edit);
            }
            self.UpdateAllTextAreaHeight = function () {
                var textAreas = $('.orgstructure-textarea');
                for (var i = 0; i < textAreas.length; i++) {
                    self.ResizeTextAreaHeight(textAreas[i]);
                }
            };
            self.UpdateTextAreaHeight = function (_, event) {
                self.ResizeTextAreaHeight(event.target);
            }
            self.ResizeTextAreaHeight = function (textarea) {
                textarea.style.height = 'auto';
                textarea.style.height = (textarea.scrollHeight + 4) + 'px';
            }
            self.FocusOnNameTextArea = function () {
                $(".orgstructure-header-name-editor").focus();
            };
            self.InitNavigationBar = function () {
                var retD = $.Deferred();
                //
                var data = {
                    id: self.ID,
                    classID: self.ClassID,
                    type: 0 // OrgStructure
                };

                self.ajaxControl.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/navigatorApi/GetPathToNode'
                    },
                    function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            var list = newVal.List;
                            if (!!list) {
                                self.Path(list.reverse());
                            }
                            retD.resolve();
                        }
                        else if (newVal && newVal.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[OrgStructure.Organization.js, InitNavigationBar]', 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 2)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[OrgStructure.Organization.js, InitNavigationBar]', 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 3)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                retD.resolve();
                            });
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[OrgStructure.Organization.js, InitNavigationBar]', 'error');
                                retD.resolve();
                            });
                    });
                //
                return retD.promise();
            }
            self.LoadOrganization = function () {
                var retD = $.Deferred();
                //
                var data = {
                    'ObjectClassID': self.ClassID,
                    'ObjectID': self.ID
                };
                self.ajaxControl.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetOrganization'
                    },
                    function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            var data = newVal.Data;
                            self.Name(data.Name);
                            self.Note(data.Note);
                            self.IsLockedForOSI(data.IsLockedForOSI);
                            self.CalendarWorkSchedule(data.CalendarWorkSchedule);
                            retD.resolve();
                        }
                        else if (newVal && newVal.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[OrgStructure.Organization.js, LoadOrganization]', 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 2)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[OrgStructure.Organization.js, LoadOrganization]', 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 3)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 7) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                            });
                        }
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[OrgStructure.Organization.js, LoadOrganization]', 'error');
                                retD.resolve();
                            });
                    });
                //
                return retD.promise();
            }
            self.SaveOrganization = function () {
                var retD = $.Deferred();
                //
                var data = {
                    'ID': self.ID,
                    'ClassID': self.ClassID,
                    'Name': self.Name(),
                    'Note': self.Note(),
                    'IsLockedForOSI': self.IsLockedForOSI(),
                    'CalendarWorkScheduleID': self.CalendarWorkSchedule() && self.CalendarWorkSchedule().ID
                };
                var command = self.ActiveMode() == 'create' ? 'AddOrganization' : 'EditOrganization';
                self.ajaxControl.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: data,
                        url: '/assetApi/' + command
                    },
                    function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            retD.resolve(newVal.Data);
                        }
                        else if (newVal && newVal.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[OrgStructure.Organization.js, ' + command + ']', 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 2)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[OrgStructure.Organization.js, ' + command + ']', 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 3)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                retD.resolve();
                            });
                        else if (newVal && newVal.Result === 7) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                            });
                        }
                        else if (newVal && newVal.Result === 8) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('ValidationError'), 'error');
                            });
                        }
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[OrgStructure.Organization.js, ' + command + ']', 'error');
                                retD.resolve();
                            });
                    });
                //
                return retD.promise();
            }
            self.InitIconClass = function () {
                var retD = $.Deferred();
                //
                var param = {
                    classID: self.ClassID,
                };
                self.ajaxControl.Ajax(self.$region,
                    {
                        method: 'GET',
                        url: '/navigatorApi/GetTreeNodeIconClass?' + $.param(param),
                    },
                    function (response) {
                        self.IconClass(response.classId);
                        retD.resolve();
                    });
                //
                return retD.promise();
            }
            self.SaveEditClick = function () {
                return self.SaveOrganization().then(function (savedOrganization) {
                    if (savedOrganization) {
                        self.ResetModified();
                        selectDivision(savedOrganization, self.ParentNode ? self.ParentNode : self.Node);
                    }
                });
            };
            self.CancelClick = function ()
            {
                selectDivision(!!self.Node ? self.Node : self.ParentNode);
            };
            self.dispose = function () {
                if (self.SubdivisionList() != null) {
                    self.SubdivisionList().dispose();
                    self.SubdivisionList(null);
                }
                if (self.Attachments() != null) {
                    self.Attachments().dispose();
                    self.Attachments(null);
                }
            };
        }
    }
    {//identification
        getObjectID = function (obj) {
            return obj && obj.ID.toUpperCase();
        };
        getObjectClassID = function (obj) {
            return obj && obj.ClassID;
        };
        getObjectName = function (obj) {
            return obj && obj.Name;
        };
        getObjectIconClass = function (obj) {
            return obj && obj.IconClass;
        };
        getObjectLevelName = function (classID) {
            switch (classID) {
                case 29: return getTextResource("OrgStructureLevel_Owner");
                case 101: return getTextResource("OrgStructureLevel_Organization");
                case 102: return getTextResource("OrgStructureLevel_Subdivision");
                default: return null;
            }
        }
    };
    return module;
});
