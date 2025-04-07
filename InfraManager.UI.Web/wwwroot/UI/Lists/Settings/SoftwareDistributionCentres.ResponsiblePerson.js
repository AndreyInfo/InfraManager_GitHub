define(['jquery', 'knockout', 'ajax'], function ($, ko, ajaxLib) {
    var module = {
        ViewModel: function (editing) {
            var self = this;

            self.editing = editing;
            self.CanEdit = function () {
                return self.editing();
            };

            self.initialized = ko.observable(false);
            self.id = null;
            self.objectClassID = null;

            self.data = ko.observable(null);
            self.get = function () {
                return {
                    ID: self.id,
                    ClassID: self.objectClassID
                };
            }
            self.set = function (id, classId) {
                self.id = id;
                self.objectClassID = classId;
                
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    if (self.initialized() === false) {
                        var type = null;
                        if (classId == module.Classes.user) {//IMSystem.Global.OBJ_USER
                            type = userLib.UserTypes.utilizer;
                        }
                        else if (classId == module.Classes.group) {//IMSystem.Global.OBJ_QUEUE
                            type = userLib.UserTypes.queueExecutor;
                        }
                        else if (classId == module.Classes.subdivision) {//IMSystem.Global.OBJ_DIVISION
                            type = userLib.UserTypes.subdivision;
                        }
                        var options = {
                            UserID: id,
                            UserType: type,
                            UserName: null,
                            EditAction: self.edit,
                            RemoveAction: null,
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        self.data(user);
                        self.initialized(true);
                    }
                });
            };
            self.edit = function () {
                if (!self.editing()) {
                    return;
                }
                //
                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var responsiblePersonText = getTextResource("SoftwareDistributionCentre_ResponsiblePerson_FieldName");
                    $.when(userD).done(function (user) {
                        var options = {
                            ID: self.id,
                            objClassID: self.objectClassID,
                            ClassID: self.objectClassID,
                            fieldName: 'ResponsiblePerson',
                            fieldFriendlyName: responsiblePersonText,
                            oldValue: { ID: self.id, ClassID: self.objectClassID, FullName: self.data().FullName() },
                            object: ko.toJS(self.data()),
                            searcherName: 'ResponsiblePersonSearcher',
                            searcherPlaceholder: responsiblePersonText,
                            allowNull: false,
                            nosave: true,
                            onSave: function (objectInfo) {
                                if (objectInfo) {
                                    self.initialized(false);
                                    self.set(objectInfo.ID, objectInfo.ClassID);
                                }
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                });
            };
            self.hasChanges = function (initialData) {
                return self.id !== initialData.ID
                    || self.objectClassID !== initialData.ClassID;
            };
        },
        Classes: {
            user: 9,
            group: 722,
            subdivision: 102
        }
    };

    return module;
});