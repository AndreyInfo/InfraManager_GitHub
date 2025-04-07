define(['knockout', 'jquery', 'ttControl', 'ajax'], function (ko, $, tclib, ajaxLib) {
    var module = {
        ConstMaxSize:  2147483646,
        ViewModel: function (ko_object, objClassID, canEdit_object, regionID) {
            var self = this;
            self.LoadD = $.Deferred();
            //
            self.$region = $('#' + regionID);
            //
            if (ko_object != null && ko_object() != null && ko_object().ID != null)
                self.objID = ko_object().ID();
            else self.objID = '';
            self.objClassID = objClassID;
            self.ManhoursWorkShowFullInterval = ko.observable(false);
            self.ManhoursInClosed = ko.observable(true); //setting 
            //
            self.CanEditComputed = ko.computed(function () {
                if (self.ManhoursInClosed() == false && ko_object().WorkflowSchemeID && ko_object().WorkflowSchemeID() == null)
                    return false;
                else return true;
            });
            //
            self.ManhoursWorkList = ko.observableArray([]);
            self.SortedManhoursWorkList = ko.computed(function () {
                return self.ManhoursWorkList.slice().sort(function (l, r) {
                    return l.Number() == r.Number() ? 0 : (l.Number() < r.Number() ? -1 : 1);
                });
            });
            self.TotalManhours = ko.computed(function () {
                var total = 0;
                ko.utils.arrayForEach(self.ManhoursWorkList(), function (el) {
                    total += el.TotalManhours();
                });
                //
                if (total >= module.ConstMaxSize)
                    total = module.ConstMaxSize;
                else total = Math.round(total * 100) / 100;
                return total;
            });
            self.TotalManhoursString = ko.computed(function () {
                var val = self.TotalManhours();
                if (val >= module.ConstMaxSize)
                    return getLocaleHourMinString(val) + ' (' + getTextResource('ManhoursMaxValueLabel') + ')';
                else return getLocaleHourMinString(val);
            });
            self.RefreshManhours = function () {
                var total = self.TotalManhours();
                if (total == null || total == NaN || total < 0)
                    total = 0;
                ko_object().ManHours(getLocaleHourMinString(total));
            };
            //
            self.ajaxControl_load = new ajaxLib.control();
            self.ajaxControl_loadMode = new ajaxLib.control();
            self.Load = function () {
                $(document).unbind('objectInserted', self.onObjectModifiedSignal);
                $(document).unbind('objectUpdated', self.onObjectModifiedSignal);
                $(document).unbind('objectDeleted', self.onObjectModifiedSignal);
                $(document).unbind('local_objectInserted', self.onObjectModified);
                $(document).unbind('local_objectUpdated', self.onObjectModified);
                $(document).unbind('local_objectDeleted', self.onObjectModified);
                //
                var loadD = $.Deferred();
                //
                $.when(userD).done(function (user) {
                    var param = {
                        'objectId': self.objID,
                        'objectClassId': self.objClassID,
                    };
                    self.ajaxControl_load.Ajax(self.$region,
                        {
                            url: '/api/manhours?' + $.param(param),
                            method: 'GET'
                        },
                        function(response) {
                            self.ManhoursWorkList.removeAll();
                            //
                            if (response) {
                                ko.utils.arrayForEach(response,
                                    function(el) {
                                        self.ManhoursWorkList.push(new module.ManhoursWork(el));
                                    });
                                //
                                self.ManhoursWorkList.valueHasMutated();
                                //
                                $(document).unbind('objectInserted', self.onObjectModifiedSignal)
                                    .bind('objectInserted', self.onObjectModifiedSignal);
                                $(document).unbind('objectUpdated', self.onObjectModifiedSignal)
                                    .bind('objectUpdated', self.onObjectModifiedSignal);
                                $(document).unbind('objectDeleted', self.onObjectModifiedSignal)
                                    .bind('objectDeleted', self.onObjectModifiedSignal);
                                $(document).unbind('local_objectInserted', self.onObjectModified)
                                    .bind('local_objectInserted', self.onObjectModified);
                                $(document).unbind('local_objectUpdated', self.onObjectModified)
                                    .bind('local_objectUpdated', self.onObjectModified);
                                $(document).unbind('local_objectDeleted', self.onObjectModified)
                                    .bind('local_objectDeleted', self.onObjectModified);
                                //
                                self.ajaxControl_loadMode.Ajax(self.$region,
                                    {
                                        url: '/api/manhours/settings',
                                        method: 'GET',
                                        dataType: "json"
                                    },
                                    function(retval) {
                                        self.ManhoursWorkShowFullInterval(retval.ShowFullInterval);
                                        self.ManhoursInClosed(retval.AllowInClosed);
                                        loadD.resolve(true);
                                    });
                                loadD.resolve(false);
                            }
                        });
                });
                //
                return loadD.promise();
            };
            self.AfterRender = function () {
                $.when(self.Load(), operationIsGrantedD(314)).done(function (resultLoad, resultOperation) {
                    self.LoadD.resolve(resultLoad);
                    //
                    if (resultLoad && (self.ManhoursWorkList().length == 0 || self.TotalManhours() == 0) && resultOperation)
                        self.AddWork();
                });
            };
            //
            self.EditWork = function (workObj) {
                if (workObj && workObj.ID)
                    require(['usualForms', 'sweetAlert'], function (fmodule, swal) {
                        var fh = new fmodule.formHelper();
                        //
                        var options = {};
                        options.onSave = function (newWorkObj) {
                            if (newWorkObj) {
                                $(document).trigger('local_objectUpdated', [18, newWorkObj]);
                            }
                        };
                        options.showLink = false;
                        options.manhoursWorkShowFullInterval = self.ManhoursWorkShowFullInterval();
                        //
                        fh.ShowManhoursWorkForm(ko.toJS(workObj), self.CanEditComputed, options);
                    });
            };
            //
            self.AddWork = function () {
                if (self.CanEditComputed())
                    require(['usualForms', 'sweetAlert'], function (fmodule, swal) {
                        $.when(userD).done(function (user) {
                            var fh = new fmodule.formHelper();
                            var newWork = {
                                ObjectID: self.objID,
                                ObjectClassID: self.objClassID,
                                ExecutorID: user.UserID,
                                ExecutorFullName: user.UserFullName,
                                InitiatorID: null,
                                InitiatorFullName: null
                            };
                            if (ko_object != null && ko_object() != null) {
                                if (self.objClassID == 701) {
                                    if (ko_object().ClientLoaded()) {
                                        newWork.InitiatorID = ko_object().ClientID();
                                        newWork.InitiatorFullName = ko_object().Client().FullName();
                                    }
                                }
                                else if (self.objClassID == 702) {
                                    if (ko_object().OwnerLoaded()) {
                                        newWork.InitiatorID = ko_object().OwnerID();
                                        newWork.InitiatorFullName = ko_object().Owner().FullName();
                                    }
                                }
                                else if (self.objClassID == 119) {
                                    if (ko_object().InitiatorLoaded()) {
                                        newWork.InitiatorID = ko_object().InitiatorID();
                                        newWork.InitiatorFullName = ko_object().Initiator().FullName();
                                    }
                                }
                            }
                            //
                            var options = {};
                            options.onSave = function (newWorkObj) {
                                if (newWorkObj) {
                                    $(document).trigger('local_objectInserted', [18, newWorkObj]);
                                }
                            };
                            options.showLink = false;
                            options.manhoursWorkShowFullInterval = self.ManhoursWorkShowFullInterval();
                            //
                            fh.ShowManhoursWorkForm(newWork, self.CanEditComputed, options);
                        });
                    });
            };
            //
            self.ajaxControl_delete = new ajaxLib.control();
            self.RemoveWork = function (workObj) {
                if (workObj && workObj.ID && self.CanEditComputed())
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('RemoveManhoursWorkCaption') + " '" + workObj.NumberName() + "'",
                            text: getTextResource('RemoveManhoursWorkQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                        function (value) {
                            if (value == true) {
                                showSpinner();
                                //
                                $.when(userD).done(function (user) {
                                    self.ajaxControl_delete.Ajax(self.$region,
                                        {
                                            dataType: 'text',
                                            url: `/api/manhours/${workObj.ID()}`,
                                            method: 'DELETE'
                                        },
                                        function () {
                                            swal.close();
                                            $(document).trigger('local_objectDeleted', [18, workObj]);
                                        },
                                            null,
                                            function () {
                                                hideSpinner();
                                            });
                                });
                            }
                        });
                    });
            };
            //
            self.Unload = function () {
                $(document).unbind('objectInserted', self.onObjectModifiedSignal);
                $(document).unbind('objectUpdated', self.onObjectModifiedSignal);
                $(document).unbind('objectDeleted', self.onObjectModifiedSignal);
                $(document).unbind('local_objectInserted', self.onObjectModified);
                $(document).unbind('local_objectUpdated', self.onObjectModified);
                $(document).unbind('local_objectDeleted', self.onObjectModified);
            };
            //            
            self.onSaveWork = function (details) {
                self.ManhoursWorkList.remove(function (el) {
                    return el.ID() == details.ID;
                });
                self.ManhoursWorkList().push(new module.ManhoursWork(details));
                //
                self.ManhoursWorkList.valueHasMutated();
                self.RefreshManhours();
            };

            self.onObjectModifiedSignal = function (e, objectClassID, objectID, parentObjectID) {//ловим всё
                if (objectClassID == 18) {//OBJ_ManhoursWork
                    if (self.objID == parentObjectID) {
                        if (e.type == 'objectInserted' || e.type == 'objectUpdated')
                            self.ajaxControl_load.Ajax(self.$region,
                                {
                                    url: `/api/manhours/${objectID}`,
                                    method: 'GET'
                                }, function (objectDetails) {
                                    self.onSaveWork(objectDetails);
                                });
                        else if (e.type == 'objectDeleted') {
                            self.ManhoursWorkList.remove(function (el) {
                                return el.ID() == objectID;
                            });
                            //
                            self.ManhoursWorkList.valueHasMutated();
                            self.RefreshManhours();
                        }
                    }
                }
            };

            self.onObjectModified = function (e, objectClassID, objectDetails) {//ловим всё
                if (objectClassID == 18) {//OBJ_ManhoursWork
                    if (self.objID == objectDetails.ObjectID) {
                        if (e.type == 'objectInserted' || e.type == 'objectUpdated' || e.type == 'local_objectInserted' || e.type == 'local_objectUpdated')
                            self.onSaveWork(objectDetails);
                        else if (e.type == 'objectDeleted' || e.type == 'local_objectDeleted') {
                            self.ManhoursWorkList.remove(function (el) {
                                return el.ID() == objectDetails.ID;
                            });
                            //
                            self.ManhoursWorkList.valueHasMutated();
                            self.RefreshManhours();
                        }
                    }
                }
            };
        },
        ManhoursWork: function (obj) {
            var self = this;
            //
            self.ID = ko.observable(obj.ID);
            self.Number = ko.observable(obj.Number);
            self.Description = ko.observable(obj.Description);
            self.ObjectID = ko.observable(obj.ObjectID);
            self.ObjectClassID = ko.observable(obj.ObjectClassID);
            self.ExecutorID = ko.observable(obj.ExecutorID);
            self.ExecutorFullName = ko.observable(obj.ExecutorFullName);
            self.InitiatorID = ko.observable(obj.InitiatorID);
            self.InitiatorFullName = ko.observable(obj.InitiatorFullName);
            self.UserActivityTypeID = ko.observable(obj.UserActivityTypeID);
            self.UserActivityTypeName = ko.observable(obj.UserActivityTypeName);
            //
            self.ManhoursList = ko.observableArray([]);
            if (obj.ManhoursList && obj.ManhoursList.length > 0) {
                ko.utils.arrayForEach(obj.ManhoursList, function (m) {
                    self.ManhoursList().push(new module.Manhour(m));
                });
                //
                self.ManhoursList.valueHasMutated();
            }
            self.SortedManhoursList = ko.computed(function () {
                return self.ManhoursList.slice().sort(function (l, r) {
                    return l.StartDate() == r.StartDate() ? 0 : (l.StartDate() < r.StartDate() ? -1 : 1);
                });
            });
            //
            self.NumberName = ko.computed(function () {
                return '№' + self.Number() + ': ' + self.Description();
            });
            //
            self.TotalManhours = ko.computed(function () {
                var total = 0;
                ko.utils.arrayForEach(self.ManhoursList(), function (el) {
                    if (el.Value() != null)
                        total += parseFloat(el.Value());
                });
                return total;
            });
        },
        Manhour: function (obj) {
            var self = this;
            //
            self.ID = obj.ID;
            self.WorkID = obj.WorkID;
            self.Value = ko.observable(0 + obj.Value);
            //
            self.ValueString = ko.computed(function () {
                return getLocaleHourMinString(self.Value());
            });
            //
            if (obj.UtcDate) //из базы
                self.StartDate = ko.observable(new Date(getUtcDate(obj.UtcDate)));
            else if (obj.StartDate) //из редактора
                self.StartDate = ko.observable(new Date(obj.StartDate));
            //
            self.EndDate = ko.computed(function () {
                var retval = new Date(self.StartDate().getTime());
                //
                retval.setMinutes(retval.getMinutes() + self.Value());
                return retval;
            });
            //
            self.DateStringSinceTo = ko.computed(function () {
                var startDate = self.StartDate();
                var endDate = self.EndDate();
                //
                var first = startDate.toLocaleString(locale, {
                    year: "numeric",
                    month: "2-digit",
                    day: "2-digit"
                });
                //
                var second = startDate.toLocaleString(locale, {
                    hour: "2-digit",
                    minute: "2-digit"
                });
                //
                var last = '';
                if (startDate.toDateString() != endDate.toDateString()) //not same day
                {
                    last = endDate.toLocaleString(locale, {
                        year: "numeric",
                        month: "2-digit",
                        day: "2-digit"
                    });
                    last += ' ' + endDate.toLocaleString(locale, { hour: '2-digit', minute: '2-digit' });
                    return getTextResource('Since') + ' ' + first + ' ' + second + ' ' + getTextResource('To') + ' ' + last;
                }
                else
                {
                    last = endDate.toLocaleString(locale, { hour: '2-digit', minute: '2-digit' });
                    return first + ' ' + getTextResource('Since') + ' ' + second + ' ' + getTextResource('To') + ' ' + last;
                }
            });
            //
            self.DateStringSimple = ko.computed(function () {
                return self.StartDate().toLocaleString(locale, {
                    year: "numeric",
                    month: "2-digit",
                    day: "2-digit"
                });
            });
        }
    };
    return module;
});