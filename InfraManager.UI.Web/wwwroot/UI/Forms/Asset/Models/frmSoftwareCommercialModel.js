define(['knockout', 'jquery', 'ajax',
    'formControl', 'usualForms', 'ttControl',
    'models/AssetForms/AssetFields',
    'models/AssetForms/AssetForm.History',
    './SoftwareModelUpdateForTable', './SoftwareModelLicensesForTable', './frmSoftwareModelRecognition'],
    function (ko, $, ajaxLib,
        fc, fhModule, tclib,
        assetFields,
        assetHistory, softwareModelUpdate, softwareModelLicenses, softwareModelRecognition) {
        var module = {
            ViewModel: function (isReadOnly, $region) {
                var self = this;
                self.$region = $region;
                //
                self.ClassID = 38;
                self.SoftwareCommercialModel = ko.observable(null);
                //
                self.TabHeight = ko.observable(0);
                self.TabWidth = ko.observable(0);
                self.TabSize = ko.computed(function () {
                    return {
                        h: self.TabHeight(),
                        w: self.TabWidth()
                    };
                });
                //
                self.SizeChanged = function () {
                    if (!self.SoftwareCommercialModel())
                        return;
                    //
                    var tabHeight = self.$region.height();//form height
                    tabHeight -= 100;
                    //
                    var tabWidth = self.$region.width();//form width
                    tabWidth -= self.$region.find('.b-requestDetail-right').outerWidth(true);
                    //
                    self.TabHeight(Math.max(0, tabHeight - 10) + 'px');
                    self.TabWidth(Math.max(0, tabWidth - 5) + 'px');
                };
                //
                self.IsReadOnly = ko.observable(isReadOnly);
                self.CanEdit = ko.computed(function () {
                    if (!self.SoftwareCommercialModel())
                        return false;
                    return true;
                });
                //
                self.CanShow = ko.observable(self.CanEdit);
                //
                self.AssetOperationControl = ko.observable(null);
                self.LoadAssetOperationControl = function () {
                    if (!self.SoftwareCommercialModel())
                        return;
                    //
                    require(['assetOperations'], function (wfLib) {
                        if (self.AssetOperationControl() == null) {
                            self.AssetOperationControl(new wfLib.control(self.$region, self.SoftwareCommercialModel, self.Load));
                        }
                        self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                        self.AssetOperationControl().Initialize();
                    });
                };
                //             
                self.modes = {
                    nothing: 'nothing',
                    main: 'main',
                    history: 'history',
                    softwareModelUpdate: 'softwareModelUpdate',
                    softwareModelLicenses: 'softwareModelLicenses',
                    softwareModelRecognition: 'softwareModelRecognition'
                };
                //
                self.GetTabSize = function () {
                    return {
                        h: parseInt(self.TabHeight().replace('px', '')),
                        w: parseInt(self.TabWidth().replace('px', ''))
                    };
                };
                //
                //MAIN TAB BLOCK
                self.mode = ko.observable(self.modes.main);
                self.mode.subscribe(function (newValue) {
                    if (newValue == self.modes.nothing)
                        return;
                    else if (newValue == self.modes.history)
                        self.assetHistory.CheckData();
                    
                });
                self.MainClick = function () {
                    self.mode(self.modes.main);
                };
                //
                self.HistoryClick = function () {
                    self.mode(self.modes.history);
                    self.SizeChanged();
                };
                //
                self.UpdateModelListClick = function () {
                    self.mode(self.modes.softwareModelUpdate);
                    self.SizeChanged();
                };
                //
                self.LicensesModelListClick = function () {
                    self.mode(self.modes.softwareModelLicenses);
                    self.SizeChanged();
                };
                //
                self.RecognitionModelListClick = function () {
                    self.mode(self.modes.softwareModelRecognition);
                    self.SizeChanged();
                };
                //
                self.assetHistory = new assetHistory.Tape(self.SoftwareCommercialModel, self.ClassID, self.$region.find('.assetHistory .tabContent').selector);
                self.softwareModelUpdateForTable = new softwareModelUpdate.List(self);
                self.softwareModelLicensesForTable = new softwareModelLicenses.List(self);
                self.softwareModelRecognition = new softwareModelRecognition.Tab(self);
                //
                self.Load = function (id) {
                    var retD = $.Deferred();
                    $.when(self.LoadSoftwareCommercialModel(id)).done(function (isLoaded) {
                        retD.resolve(isLoaded);
                        if (isLoaded) {
                            self.InitializeOwnerModel();
                            self.InitializeModelSupportQueue();
                            self.SizeChanged();
                        }
                    });
                    //
                    return retD.promise();
                };
                //
                self.ajaxControl_SoftwareCommercialModel = new ajaxLib.control();
                //
                self.LoadSoftwareCommercialModel = function (id) {
                    var retD = $.Deferred();
                    //
                    if (!id) {
                        retD.resolve(false);
                        return retD.promise();
                    }
                    ///
                    var data = { 'ID': id };
                    self.ajaxControl_SoftwareCommercialModel.Ajax(self.$region,
                        {
                            dataType: "json",
                            method: 'GET',
                            data: data,
                            url: '/assetApi/GetSoftwareModel'
                        },
                        function (newVal) {
                            var loadSuccessD = $.Deferred();
                            var processed = false;
                            //
                            if (newVal) {
                                if (newVal.Result == 0) {
                                    var ce = newVal.SoftwareModel;
                                    if (ce) {
                                        require(['ui_forms/Asset/Models/SoftwareCommercialModel'], function (ceLib) {
                                            self.SoftwareCommercialModel(new ceLib.SoftwareCommercialModel(self, ce));
                                            self.SizeChanged();
                                            //
                                            processed = true;
                                            loadSuccessD.resolve(true);
                                        });
                                    }
                                    else loadSuccessD.resolve(false);
                                }
                                else {
                                    if (newVal.Result == 3) {//AccessError
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                        });
                                        processed = true;
                                    }
                                    if (newVal.Result == 6) {//AccessError
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('ObjectDeleted'), 'error');
                                        });
                                        processed = true;
                                    }
                                    else if (newVal.Result == 7) {//OperationError
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                        });
                                        processed = true;
                                    }
                                    else {//GlobalError
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                        });
                                        processed = true;
                                    }
                                    loadSuccessD.resolve(false);
                                }
                            }
                            else loadSuccessD.resolve(false);
                            //
                            $.when(loadSuccessD).done(function (loadSuccess) {
                                retD.resolve(loadSuccess);
                                if (loadSuccess == false && processed == false) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[frmSoftwareCommercialModel.js, Load]', 'error');
                                    });
                                }
                            });
                        });
                    //
                    return retD.promise();
                };
                //           
                self.AfterRender = function () {
                    self.SizeChanged();
                };
                //Название
                self.EditName = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Name',
                            fieldFriendlyName: getTextResource('AssetNumber_Name'),
                            oldValue: asset.Name(),
                            allowNull: true,
                            maxLength: 250,
                            onSave: function (newText) {
                                asset.Name(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                    });
                };
                //Версия
                self.EditVersion = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Version',
                            fieldFriendlyName: getTextResource('ModelVersion'),
                            oldValue: asset.Version(),
                            allowNull: true,
                            maxLength: 50,
                            onSave: function (newText) {
                                asset.Version(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                    });
                };
                //Код
                self.EditCode = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Code',
                            fieldFriendlyName: getTextResource('Code'),
                            oldValue: asset.Code(),
                            allowNull: true,
                            maxLength: 50,
                            onSave: function (newText) {
                                asset.Code(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                    });
                };
                //Внешний ИД
                self.EditExternal = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'ExternalID',
                            fieldFriendlyName: getTextResource('ExternalID'),
                            oldValue: asset.ExternalID(),
                            allowNull: true,
                            maxLength: 250,
                            onSave: function (newText) {
                                asset.ExternalID(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                    });
                };
                //Тип ПО
                self.EditSoftwareType = function () {
                    if (!self.CanEdit()) {
                        return;
                    }
                    var asset = self.SoftwareCommercialModel();
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Type',
                            fieldFriendlyName: getTextResource('SoftwareTypeName'),
                            comboBoxGetValueUrl: '/assetApi/GetCommercialModelTypeList',
                            oldValue: {
                                ID: asset.SoftwareTypeID(), Name: asset.SoftwareTypeName()
                            },
                            onSave: function (objectInfo) {
                                asset.SoftwareTypeID(objectInfo.ID);
                                asset.SoftwareTypeName(objectInfo.Name);
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                    });
                };
                //Использование
                self.EditUsingModel = function () {
                    if (!self.CanEdit())
                        return;
                    var asset = self.SoftwareCommercialModel();
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'UsingModel',
                            fieldFriendlyName: getTextResource('UsingAsset'),
                            comboBoxGetValueUrl: '/assetApi/GetUsingModelList',
                            oldValue: { 
                                ID: asset.SoftwareModelUsingTypeID(), Name: asset.SoftwareModelUsingTypeName()
                            },
                            onSave: function (objectInfo) {
                                asset.SoftwareModelUsingTypeID(objectInfo.ID);
                                asset.SoftwareModelUsingTypeName(objectInfo.Name);
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                    });
                };
                //Производитель
                self.EditManufacturer = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        //
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Manufacturer',
                            fieldFriendlyName: getTextResource('Maintenance_ManufacturerName'),
                            oldValue: { ID: asset.ManufacturerID(), ClassID: 89, FullName: asset.ManufacturerName() },
                            searcherName: 'ManufacturerSearcher',
                            searcherPlaceholder: getTextResource('Maintenance_ManufacturerName'),
                            searcherParams: ['false', 'false', 'false', 'false', 'true', 'false', 'false', 'false', 'false'],
                            onSave: function (objectInfo) {
                                asset.ManufacturerID(objectInfo ? objectInfo.ID : null);
                                asset.ManufacturerName(objectInfo ? objectInfo.FullName : '');
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //Редакция
                self.EditRedaction = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'ModelRedaction',
                            fieldFriendlyName: getTextResource('ModelRedaction'),
                            oldValue: asset.ModelRedaction(),
                            allowNull: true,
                            maxLength: 250,
                            onSave: function (newText) {
                                asset.ModelRedaction(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                    });
                };
                //Группа поддержки 
                self.EditSupportQueue = function () {
                    if (self.CanEdit() == false)
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'SupportQueue',
                            fieldFriendlyName: getTextResource('ModelSupportGroup'),
                            oldValue: asset.ModelSupportQueueLoaded() ? { ID: asset.ModelSupportQueueID(), ClassID: 722, FullName: asset.ModelSupportQueueName()} : null,
                            object: ko.toJS(asset.ModelSupportQueue()),
                            searcherName: "QueueSearcher",
                            searcherPlaceholder: getTextResource('EnterQueue'),
                            searcherParams: ['1'],//
                            onSave: function (objectInfo) {
                                asset.ModelSupportQueueLoaded(false);
                                asset.ModelSupportQueue(new userLib.EmptyUser(self, userLib.UserTypes.queueExecutor, self.EditSupportQueuefalse));
                                //
                                if (objectInfo && objectInfo.ClassID == 722) { //IMSystem.Global.OBJ_QUEUE
                                    asset.ModelSupportQueueID(objectInfo.ID);
                                    asset.ModelSupportQueueName(objectInfo.FullName);
                                }
                                else {
                                    asset.ModelSupportQueueID('');
                                    asset.ModelSupportQueueName('');
                                }
                                self.InitializeModelSupportQueue();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };

                self.InitializeModelSupportQueue = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {

                        var asset = self.SoftwareCommercialModel();
                        //
                        if (asset.ModelSupportQueueLoaded() == false) {
                            if (asset.ModelSupportQueueID()) {
                                var options = {
                                    UserID: asset.ModelSupportQueueID(),
                                    UserType: userLib.UserTypes.queueExecutor,
                                    UserName: null,
                                    EditAction: self.EditSupportQueue,
                                    RemoveAction: null,
                                    CanNote: true
                                };
                                var user = new userLib.User(self, options);
                                asset.ModelSupportQueue(user);
                                asset.ModelSupportQueueLoaded(true);
                                //
                                //var already = ko.utils.arrayFirst(self.ModelUsersList(), function (item) {
                                //    return item.ID() == asset.ModelSupportQueueID();
                                //});
                                ////
                                //if (already == null)
                                //    self.ModelUsersList.push(user);
                                //else if (already.Type == userLib.UserTypes.withoutType) {
                                //    self.ModelUsersList.remove(already);
                                //    self.ModelUsersList.push(user);
                                //}
                            }
                        }
                    });
                };
                //Владелец модели 
                self.EditOwnerModel = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'ModelOwner',
                            fieldFriendlyName: getTextResource('ModelOwner'),
                            oldValue: asset.OwnerModelLoaded() ? { ID: asset.OwnerModelID(), ClassID: asset.OwnerModelClassID(), FullName: asset.OwnerModelName() } : null,
                            object: ko.toJS(asset.OwnerModel()),
                            searcherName: 'UserWithQueueSearcherNoTOZ',
                            searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                            onSave: function (objectInfo) {
                                asset.OwnerModelLoaded(false);
                                asset.OwnerModel(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditOwnerModel, false, false));
                                //
                                asset.OwnerModelID(objectInfo ? objectInfo.ID : '');
                                asset.OwnerModelClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeOwnerModel();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //
                self.InitializeOwnerModel = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var a = self.SoftwareCommercialModel();
                        if (a.OwnerModelLoaded() == false && a.OwnerModelID()) {
                            var type = null;
                            if (a.OwnerModelClassID() == 9) {//IMSystem.Global.OBJ_USER
                                type = userLib.UserTypes.utilizer;
                            }
                            else if (a.OwnerModelClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                                type = userLib.UserTypes.queueExecutor;
                            }
                            //
                            var options = {
                                UserID: a.OwnerModelID(),
                                UserType: type,
                                UserName: null,
                                EditAction: self.EditOwnerModel,
                                RemoveAction: null,
                                ShowTypeName: false
                            };
                            var user = new userLib.User(self, options);
                            a.OwnerModel(user);
                            a.OwnerModelLoaded(true);
                        }
                    });
                };        
                //Примечание
                self.EditNote = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.SoftwareCommercialModel();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Note',
                            fieldFriendlyName: getTextResource('AssetNumber_Note'),
                            oldValue: asset.Note(),
                            allowNull: true,
                            maxLength: 500,
                            onSave: function (newText) {
                                asset.Note(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };
                //Лицензирование
                self.EditLicenceScheme = function () {
                    if (!self.CanEdit())
                        return;
                    var asset = self.SoftwareCommercialModel();
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'SoftwareLicenceControl',
                            fieldFriendlyName: getTextResource('WrittenOff_Licensing'),
                            comboBoxGetValueUrl: 'assetApi/GetLicenseControlList',
                            oldValue: {
                                ID: asset.SoftwareLicenceSchemeID(), Name: asset.SoftwareLicenceSchemeName()
                            },
                            onSave: function (objectInfo) {
                                asset.SoftwareLicenceSchemeID(objectInfo ? objectInfo.ID : null);
                                asset.SoftwareLicenceSchemeName(objectInfo ? objectInfo.Name : '');
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                    });
                };
                //Язык
                self.EditLanguage = function () {
                    if (!self.CanEdit())
                        return;
                    var asset = self.SoftwareCommercialModel();
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'SoftwareLanguage',
                            fieldFriendlyName: getTextResource('ModelLanguage'),
                            comboBoxGetValueUrl: 'assetApi/GetLicenseLanguageList',
                            oldValue: {
                                ID: asset.SoftwareLicenceLanguageID(), Name: asset.SoftwareLicenceLanguageName()
                            },
                            onSave: function (objectInfo) {
                                asset.SoftwareLicenceLanguageID(objectInfo ? objectInfo.ID : null);
                                asset.SoftwareLicenceLanguageName(objectInfo ? objectInfo.Name : '');
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                    });
                };
                self.ajaxControl_updateField = new ajaxLib.control();
                self.UpdateField = function (isReplaceAnyway, options) {
                    var data = {
                        ID: self.SoftwareCommercialModel().ID(),
                        ObjClassID: self.ClassID,
                        Field: options.FieldName,
                        OldValue: options.OldValue == null ? null : options.OldValue,
                        NewValue: options.NewValue == null ? null : options.NewValue,
                        ReplaceAnyway: isReplaceAnyway
                    };
                    //
                    self.ajaxControl_updateField.Ajax(
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
                                        swal(getTextResource('SaveError'), getTextResource('NullParamsError') + '\n[CallForm.js UpdateField]', 'error');
                                    });
                                }
                                else if (result === 2) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('BadParamsError') + '\n[CallForm.js UpdateField]', 'error');
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
                                                    self.UpdateField(true, options);
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
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[CallForm.js UpdateField]', 'error');
                                    });
                                }
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[CallForm.js UpdateField]', 'error');
                                });
                            }
                        });
                };
            },
            ShowDialog: function (id, isSpinnerActive) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                $.when(userD, operationIsGrantedD()).done(function (user, operation_update) {
                    var isReadOnly = false;
                    if (user.HasRoles == false || operation_update == false)
                        isReadOnly = true;
                    //
                    var frm = undefined;
                    //
                    var buttons = {
                    }
                    var vm = null;
                    //
                    frm = new fc.control(
                        'region_softwareCommercialModelForm_',//form region prefix
                        'setting_softwareCommercialModelForm_',//location and size setting
                        getTextResource('ModelCommerdialSoftware'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        850, 750,//minSize
                        buttons,//form buttons
                        function () {
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/Models/frmSoftwareCommercialModel\', afterRender: AfterRender}"'//attributes of form region
                    );
                    //
                    if (!frm.Initialized) {//form with that region and settingsName was open
                        hideSpinner();
                        return;
                    }
                    //
                    frm.BeforeClose = function () {
                        hideSpinner();
                    };
                    //
                    var $region = $('#' + frm.GetRegionID());
                    vm = new module.ViewModel(isReadOnly, $region);
                    vm.frm = frm;
                    //
                    frm.SizeChanged = function () {
                        var width = frm.GetInnerWidth();
                        var height = frm.GetInnerHeight();
                        //
                        vm.$region.css('width', width + 'px').css('height', height + 'px');
                        vm.SizeChanged();
                    };
                    //
                    ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                    $.when(frm.Show(), vm.Load(id)).done(function (frmD, loadD) {
                        if (loadD == false) {//force close
                            frm.Close();
                        }
                        else {
                            if (!ko.components.isRegistered('softwareCaptionComponent'))
                                ko.components.register('softwareCaptionComponent', {
                                    template: '<span data-bind="text: $captionText"/>'
                                });
                            frm.BindCaption(vm, "component: {name: 'softwareCaptionComponent', params: {  $captionText: getTextResource(\'ModelCommerdialSoftware\') + ' ' + $data.SoftwareCommercialModel().Name()} }");
                        }
                        hideSpinner();
                    });
                });
            }
        }
        return module;
    });