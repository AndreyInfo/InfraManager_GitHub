define(['knockout', 'jquery', 'ajax',
    'formControl', 'usualForms', 'ttControl',
    'models/AssetForms/AssetFields',
    'models/AssetForms/AssetForm.History',
    './DataEntityDependencyListForTable',
    'parametersControl'],
    function (ko, $, ajaxLib,
        fc, fhModule, tclib,
        assetFields,
        assetHistory, 
        dataEntityDependency,
        pcLib) {
    var module = {
        ViewModel: function (isReadOnly, $region) {
            var self = this;
            self.$region = $region;
            //
            self.ClassID = 165;
            self.DataEntityObject = ko.observable(null);
            //
            self.TabHeight = ko.observable(0);
            self.objectClassID = self.ClassID;
            self.TabWidth = ko.observable(0);
            self.TabSize = ko.computed(function () {
                return {
                    h: self.TabHeight(),
                    w: self.TabWidth()
                };
            });
            //
            self.SizeChanged = function () {
                if (!self.DataEntityObject())
                    return;
                //
                var tabHeight = self.$region.height();//form height
                tabHeight -= 140;
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
                if (!self.DataEntityObject())
                    return false;
                if (self.DataEntityObject().LifeCycleStateName() === 'Списано')
                    return false;
                //
                return !self.IsReadOnly();
            });
            //
            self.AssetOperationControl = ko.observable(null);
            self.LoadAssetOperationControl = function () {
                if (!self.DataEntityObject())
                    return;
                //
                require(['assetOperations'], function (wfLib) {
                    if (self.AssetOperationControl() == null) {
                        self.AssetOperationControl(new wfLib.control(self.$region, self.DataEntityObject, self.Load));
                    }
                    self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                    self.AssetOperationControl().Initialize();
                });
            };
            //
            self.AssetCriticalityControl = ko.observable(null);
            self.LoadAssetCriticalityControl = function () {
                if (!self.DataEntityObject())
                    return;
                //
                require(['../UI/Forms/Asset/Controls/AssetCriticalityControl'], function (CrLib) {
                    if (self.AssetCriticalityControl() == null) {
                        self.AssetCriticalityControl(new CrLib.control(self.$region, self.DataEntityObject,self.EditCriticality));
                    }
                    self.AssetCriticalityControl().ReadOnly(self.IsReadOnly());
                    self.AssetCriticalityControl().Initialize();
                });
            };
            self.EditCriticality = function (obj) {
                var options = {
                    FieldName: 'DataEntity.Criticality',
                    OldValue: self.DataEntityObject().CriticalityID==null ?null: JSON.stringify({ 'id': self.DataEntityObject().CriticalityID() }) ,
                    NewValue: obj.ID == null ?null: JSON.stringify({ 'id': obj.ID }) ,
                    onSave: function () {
                        self.DataEntityObject().CriticalityID(obj.ID);
                        self.DataEntityObject().CriticalityName(obj.Name);
                    }
                };
                self.UpdateField(false, options);
            }
            //
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                history: 'history',
                dependency: 'dependency'                
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
                else if(newValue == self.modes.history)              
                    self.assetHistory.CheckData();
                else if (newValue.indexOf(self.parameterModePrefix) != -1) {
                    self.InitializeParametersTabs();
                }
                //else if (newValue == self.modes.dependency)
                //    self.dataEntityDependency.CheckData();
               
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
            self.DependencyClick = function () {
                self.mode(self.modes.dependency);
                self.SizeChanged();
            };
                //
                self.assetHistory = new assetHistory.Tape(self.DataEntityObject, self.ClassID, self.$region.find('.assetHistory .tabContent').selector);
                //
                self.dataEntityDependencyList = new dataEntityDependency.List(self);
                //
                self.Load = function (id) {
                    var retD = $.Deferred();
                    $.when(self.LoadDataEntityObject(id)).done(function (isLoaded) {
                        retD.resolve(isLoaded);
                        if (isLoaded) {
                            self.objectClassID = self.ClassID;
                            self.InitializeClient();
                            self.InitializeAdministrator();
                            self.SizeChanged();
                            self.LoadAssetOperationControl();
                            self.LoadAssetCriticalityControl();
                            self.OnParametersChanged(false);
                        }
                    });
                    //
                    return retD.promise();
                };
                //
                self.ajaxControl_DataEntityObject = new ajaxLib.control();
                //
                self.LoadDataEntityObject = function (id) {
                    var retD = $.Deferred();
                    //
                    if (!id) {
                        retD.resolve(false);
                        return retD.promise();
                    }
                    //
                    var data = { 'ID': id };
                    self.ajaxControl_DataEntityObject.Ajax(self.$region,
                        {
                            dataType: "json",
                            method: 'GET',
                            data: data,
                            url: '/assetApi/GetDataEntityObject'
                        },
                        function (newVal) {
                            var loadSuccessD = $.Deferred();
                            var processed = false;
                            //
                            if (newVal) {
                                if (newVal.Result == 0) {
                                    var de = newVal.DataEntityObject;
                                    if (de) {
                                        require(['ui_forms/Asset/DataEntityObject'], function (deLib) {
                                            self.DataEntityObject(new deLib.DataEntityObject(self, de));
                                            self.ClassID = self.DataEntityObject().ClassID();
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
                                        swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[frmDataEntityObject.js, Load]', 'error');
                                    });
                                }
                            });
                        });
                    //
                    return retD.promise();
                };
                //           
                //PARAMETERS BLOCK
                self.parametersControl = null;
                self.parameterListByGroup = null;//кеш отсортирортированных параметров, разбитых по группам
                self.parameterModePrefix = 'parameter_';
                self.ParameterList = ko.observable([]);//параметры текущей выбранной группы
                self.ParameterListGroupName = ko.computed(function () {
                    if (self.mode().indexOf(self.parameterModePrefix) != 0)
                        return '';
                    //
                    var groupName = self.mode().substring(self.parameterModePrefix.length);
                    return groupName;
                });
                self.ParameterGroupList = ko.observable([]);
                self.InitializeParametersTabs = function () {
                    if (self.mode().indexOf(self.parameterModePrefix) != 0) {
                        self.ParameterList([]);
                        return;
                    }
                    //
                    var groupName = self.mode().substring(self.parameterModePrefix.length);
                    var list = self.parameterListByGroup;
                    for (var i = 0; i < list.length; i++)
                        if (list[i].GroupName == groupName) {
                            self.ParameterList(list[i].ParameterList);
                            return;
                        }
                    //
                    self.ParameterList([]);//groupName not found
                };
                self.InitializeParameters = function (recalculateParameters) {
                    var asset = self.DataEntityObject();
                    if (!asset)
                        self.parametersControl.InitializeOrCreate(null, null, null, false);//нет объекта - нет параметров
                    else
                        self.parametersControl.InitializeOrCreate(asset.ClassID(), asset.ID(), asset, recalculateParameters);
                };
                self.OnParametersChanged = function (recalculateParameters) {//обновления списка параметров по объекту
                    if (self.parametersControl == null) {
                        self.parametersControl = new pcLib.control();
                        self.parametersControl.ReadOnly(!self.CanEdit());
                        self.parametersControl.ParameterListByGroup.subscribe(function (newValue) {//изменилась разбивка параметров по группам
                            self.parameterListByGroup = newValue;
                            //
                            self.ParameterGroupList().splice(0, self.ParameterGroupList().length);//remove without ko update
                            var newParameterListContainsOldGroup = false;
                            //
                            for (var i = 0; i < newValue.length; i++) {
                                var groupName = newValue[i].GroupName;
                                //
                                self.ParameterGroupList().push({
                                    Index: i + 1,
                                    Name: groupName,
                                    IsValid: newValue[i].IsValid
                                });
                                //
                                if (self.mode().indexOf(self.parameterModePrefix + groupName) != -1)
                                    newParameterListContainsOldGroup = true;
                            }
                            self.ParameterGroupList.valueHasMutated();
                            //
                            if (self.mode().indexOf(self.parameterModePrefix) == 0) {//сейчас вкладка параметры
                                if (!newParameterListContainsOldGroup)
                                    self.mode(self.modes.main);//такой больше нет, идем на главную
                                else {//hack ko
                                    var tmp = self.mode();
                                    self.mode(self.modes.nothing);
                                    self.mode(tmp);
                                }
                            }
                        });
                    }
                    self.InitializeParameters(recalculateParameters);
                };
                //

                //

                self.AfterRender = function () {
                    self.SizeChanged();
                };
                //
                self.EditName = function () {
                    if (!self.CanEdit() || !self.DataEntityObject().CanEditName())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
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
                //Тип
                self.EditProductCatalogType = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'ProductCatalogType',
                            fieldFriendlyName: getTextResource('AssetNumber_ProductCatalogType'),
                            oldValue: asset.ProductCatalogTypeName(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                asset.ProductCatalogTypeName(newText.Name);
                                asset.ProductCatalogTypeID(newText.ID);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //Заказчик
                self.EditClient = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Client',
                            fieldFriendlyName: getTextResource('ContractInitiator'),
                            oldValue: asset.ClientLoaded() ? { ID: asset.ClientID(), ClassID: asset.ClientClassID(), FullName: asset.ClientName() } : null,
                            object: ko.toJS(asset.Client()),
                            searcherName: 'UserWithQueueSearcherNoTOZ',
                            searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                            onSave: function (objectInfo) {
                                asset.ClientLoaded(false);
                                asset.Client(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditClient, false, false));
                                //
                                asset.ClientID(objectInfo ? objectInfo.ID : '');
                                asset.ClientClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeClient();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //
                self.InitializeClient = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var a = self.DataEntityObject();
                        if (a.ClientLoaded() == false && a.ClientID()) {
                            var type = null;
                            if (a.ClientClassID() == 9) {//IMSystem.Global.OBJ_USER
                                type = userLib.UserTypes.utilizer;
                            }
                            else if (a.ClientClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                                type = userLib.UserTypes.queueExecutor;
                            }
                            //
                            var options = {
                                UserID: a.ClientID(),
                                UserType: type,
                                UserName: null,
                                EditAction: self.EditClient,
                                RemoveAction: null,
                                //ShowLeftSide: false,
                                ShowTypeName: false
                            };
                            var user = new userLib.User(self, options);
                            a.Client(user);
                            a.ClientLoaded(true);
                        }
                    });
                };
                //Администратор 
                self.EditAdministrator = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
                    require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'Administrator',
                            fieldFriendlyName: getTextResource('Administrator'),
                            oldValue: asset.AdministratorLoaded() ? { ID: asset.AdministratorID(), ClassID: asset.AdministratorClassID(), FullName: asset.AdministratorName() } : null,
                            object: ko.toJS(asset.Administrator()),
                            searcherName: 'UserWithQueueSearcherNoTOZ',
                            searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                            onSave: function (objectInfo) {
                                asset.AdministratorLoaded(false);
                                asset.Administrator(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditAdministrator, false, false));
                                //
                                asset.AdministratorID(objectInfo ? objectInfo.ID : '');
                                asset.AdministratorClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeAdministrator();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //
                self.InitializeAdministrator = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        var a = self.DataEntityObject();
                        if (a.AdministratorLoaded() == false && a.AdministratorID()) {
                            var type = null;
                            if (a.AdministratorClassID() == 9) {//IMSystem.Global.OBJ_USER
                                type = userLib.UserTypes.utilizer;
                            }
                            else if (a.AdministratorClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                                type = userLib.UserTypes.queueExecutor;
                            }
                            //
                            var options = {
                                UserID: a.AdministratorID(),
                                UserType: type,
                                UserName: null,
                                EditAction: self.EditAdministrator,
                                RemoveAction: null,
                                //ShowLeftSide: false,
                                ShowTypeName: false
                            };
                            var user = new userLib.User(self, options);
                            a.Administrator(user);
                            a.AdministratorLoaded(true);
                        }
                    });
                };
                //           
                //Примечание
                self.EditNote = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
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
                //
                self.EditType = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        //
                        var options = {
                            ID: asset.ID(),
                            objClassID: asset.ClassID(),
                            fieldName: 'Type',
                            fieldFriendlyName: getTextResource('Type'),
                            oldValue: { ID: asset.ProductCatalogTypeID(), FullName: asset.ProductCatalogTypeName() },
                            searcherName: 'ProductCatalogTypeAndModelSearcher',
                            searcherParams: [true, false, false, false, false, false, false, false, 165],
                            onSave: function (objectInfo) {
                                asset.ProductCatalogTypeID(objectInfo.ID);
                                asset.ProductCatalogTypeName(objectInfo.FullName);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
            };
            //
            self.ajaxControl_updateField = new ajaxLib.control();
            self.UpdateField = function (isReplaceAnyway, options) {
                var data = {
                    ID: self.DataEntityObject().ID(),
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
                                $(document).trigger('local_objectUpdated', [data.ObjClassID, data.ID, null]);
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
                //
                //Управляющее приложение
                self.EditDeviceApplication = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'DeviceApplicationName',
                            fieldFriendlyName: getTextResource('DataEntity_ControlApplication'),
                            oldValue: asset.DeviceApplicationName(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                asset.DeviceApplicationName(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };
                //Том данных
                self.EditVolume = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: asset.ID(),
                            objClassID: self.ClassID,
                            fieldName: 'VolumeName',
                            fieldFriendlyName: getTextResource('DataEntity_DataVolume'),
                            oldValue: asset.VolumeName(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                asset.VolumeName(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };
                //Сегмент инфраструктуры
                //
                self.EditInfrastructureSegment = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = self.DataEntityObject();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        //
                        var options = {
                            ID: asset.ID(),
                            objClassID: asset.ClassID(),
                            fieldName: 'InfrastructureSegment',
                            fieldFriendlyName: getTextResource('InfrastructureSegment'),
                            oldValue: { ID: asset.InfrastructureSegmentID(), ClassID: 366, FullName: asset.InfrastructureSegmentName() },
                            searcherName: 'InfrastructureSegmentSearcher',
                            onSave: function (objectInfo) {
                                asset.InfrastructureSegmentID(objectInfo ? objectInfo.ID : null);
                                asset.InfrastructureSegmentName(objectInfo ? objectInfo.FullName : '');
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
                //Размер
                self.EditSize = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: self.DataEntityObject().ID(),
                            objClassID: self.DataEntityObject().ClassID(),
                            fieldName: 'DataEntityObject.Size',
                            fieldFriendlyName: getTextResource('DataEntity_Size'),
                            oldValue: self.DataEntityObject().Size(),
                            maxValue: 99999999,//в бд decimal(10, 2)
                            stepperType: 'float',
                            floatPrecission: 2,
                            allowNull: true,
                            onSave: function (newVal) {
                                self.DataEntityObject().Size(newVal);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                };
                //Даты
                self.EditDateReceived = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.DataEntityObject().ID(),
                            objClassID: self.DataEntityObject().ClassID(),
                            fieldName: 'DataEntityObject.DateReceived',
                            fieldFriendlyName: getTextResource('Contract_CreateDate'),
                            oldValue: self.DataEntityObject().DateReceivedDT(),
                            onSave: function (newDate) {
                                self.DataEntityObject().DateReceived(parseDate(newDate));
                                self.DataEntityObject().DateReceivedDT(new Date(parseInt(newDate)));
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };
                //
                self.DateReceiveCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                    var retval = '';
                    //
                    if (!retval && self.DataEntityObject) {
                        var lo = self.DataEntityObject();
                        if (lo && lo.DateReceived)
                            retval = lo.DateReceived();
                    }
                    //
                    return retval;
                });
                //
                self.EditDateAnnuled = function () {
                    if (self.CanEdit() == false)
                        return;
                    showSpinner();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: self.DataEntityObject().ID(),
                            objClassID: self.DataEntityObject().ClassID(),
                            fieldName: 'DataEntityObject.DateAnnuled',
                            fieldFriendlyName: getTextResource('AssetNumber_DateAnnuled'),
                            oldValue: self.DataEntityObject().DateAnnuledDT(),
                            onSave: function (newDate) {
                                self.DataEntityObject().DateAnnuled(parseDate(newDate));
                                self.DataEntityObject().DateAnnuledDT(new Date(parseInt(newDate)));
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };
                //
                self.DateAnnuledCalculated = ko.computed(function () { //или из объекта, или из хода выполнения
                    var retval = '';
                    //
                    if (!retval && self.DataEntityObject) {
                        var lo = self.DataEntityObject();
                        if (lo && lo.DateAnnuled)
                            retval = lo.DateAnnuled();
                    }
                    //
                    return retval;
                });
            },
        ShowDialog: function (id, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            $.when(userD, operationIsGrantedD(617)).done(function (user, operation_update) {
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
                    'region_dataEntityObjectForm_',//form region prefix
                    'setting_dataEntityObjectForm_',//location and size setting
                    getTextResource('Asset_Properties'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    850, 650,//minSize
                    buttons,//form buttons
                    function () {
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmDataEntityObject\', afterRender: AfterRender}"'//attributes of form region
                );
                //
                if (!frm.Initialized) {//form with that region and settingsName was open
                    hideSpinner();
                    //
                    var url = window.location.protocol + '//' + window.location.host + location.pathname + '?dataEntityID=' + id;
                    //
                    var wnd = window.open(url);
                    if (wnd) //browser cancel it?  
                        return;
                    //
                    require(['sweetAlert'], function () {
                        swal(getTextResource('OpenError'), getTextResource('CantDuplicateForm'), 'warning');
                    });
                    return;
                }
                //
                frm.BeforeClose = function () {
                    if (vm.DataEntityObject().Name() == '' || vm.DataEntityObject().Name() == null) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('NullParamsError'), getTextResource('MustSetName'), 'error');
                        });
                        return false;
                    };
                    hideSpinner();
                    return true;
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
                        if (!ko.components.isRegistered('dataEntityObjectCaptionComponent'))
                            ko.components.register('dataEntityObjectCaptionComponent', {
                                template: '<span data-bind="text: $captionText"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'dataEntityObjectCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Properties\') + ' ' + $data.DataEntityObject().Name()} }");
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});