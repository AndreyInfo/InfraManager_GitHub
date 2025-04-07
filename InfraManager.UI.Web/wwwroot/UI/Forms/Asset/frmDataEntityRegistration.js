define(['knockout', 'jquery', 'ajax',
    'formControl', 'usualForms', 'ttControl',
    'models/AssetForms/AssetFields',
    'models/AssetForms/AssetForm.History',
    './DataEntityDependencyListForTable',
    'parametersControl', 'dateTimeControl'],
    function (ko, $, ajaxLib,
        fc, fhModule, tclib,
        assetFields,
        assetHistory,
        dataEntityDependency,
        pcLib,dtLib) {
        var module = {
            ViewModel: function (isReadOnly, $region) {
                var self = this;
                self.$region = $region;
                //
                self.ID = null;
                self.ClassID = 165;
                self.DataEntityObject = ko.observable(null);
                //
                //
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
                //
                self.CanShow = ko.observable(self.CanEdit);
                //
                self.modes = {
                    nothing: 'nothing',
                    main: 'main',
                    //history: 'history',
                    //dependency: 'dependency'
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
                    //else if (newValue == self.modes.dependency)
                    //    self.dataEntityDependency.CheckData();

                });
                self.MainClick = function () {
                    self.mode(self.modes.main);
                };
                //
                //
                self.Load = function () {
                    var retD = $.Deferred();
                    $.when(self.LoadDataEntityObject()).done(function (isLoaded) {
                        retD.resolve(isLoaded);
                        if (isLoaded) {
                            self.InitializeClient();
                            self.InitializeAdministrator();
                            self.LoadAssetOperationControl();
                        }
                    });
                    //
                    return retD.promise();
                };
                //
                self.ajaxControl_DataEntityObject = new ajaxLib.control();
                //

                self.LoadDataEntityObject = function () {
                    require(['ui_forms/Asset/DataEntityObject'], function (deLib) {
                        self.DataEntityObject(new deLib.DataEntityObjectRegistration(self));
                        self.ClassID = self.DataEntityObject().ClassID();     
                    });          
                };
                //
                self.AfterRender = function () {
                    self.frm.SizeChanged();
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
                            fieldName: 'Name',
                            fieldFriendlyName: getTextResource('AssetNumber_Name'),
                            oldValue: asset.Name(),
                            allowNull: false,
                            maxLength: 250,
                            onSave: function (newText) {
                                self.DataEntityObject().Name(newText);
                            },
                            nosave: true
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
                            fieldName: 'ProductCatalogType',
                            fieldFriendlyName: getTextResource('AssetNumber_ProductCatalogType'),
                            oldValue: asset.ProductCatalogTypeName(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                self.DataEntityObject().ProductCatalogTypeName(newText.Name);
                                self.DataEntityObject().ProductCatalogTypeID(newText.ID);
                            },
                            nosave: true
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
                            fieldName: 'Client',
                            fieldFriendlyName: getTextResource('ContractInitiator'),
                            oldValue: asset.ClientLoaded() ? { ID: asset.ClientID(), ClassID: asset.ClientClassID(), FullName: asset.ClientName() } : null,
                            object: ko.toJS(asset.Client()),
                            searcherName: 'UserWithQueueSearcherNoTOZ',
                            searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                            onSave: function (objectInfo) {
                                self.DataEntityObject().ClientLoaded(false);
                                self.DataEntityObject().Client(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditClient, false, false));
                                //
                                self.DataEntityObject().ClientID(objectInfo ? objectInfo.ID : '');
                                self.DataEntityObject().ClientClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeClient();
                            },
                            nosave: true
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
                            fieldName: 'Administrator',
                            fieldFriendlyName: getTextResource('Administrator'),
                            oldValue: asset.AdministratorLoaded() ? { ID: asset.AdministratorID(), ClassID: asset.AdministratorClassID(), FullName: asset.AdministratorName() } : null,
                            object: ko.toJS(asset.Administrator()),
                            searcherName: 'UserWithQueueSearcherNoTOZ',
                            searcherPlaceholder: getTextResource('EnterUserOrGroupName'),
                            onSave: function (objectInfo) {
                                self.DataEntityObject().AdministratorLoaded(false);
                                self.DataEntityObject().Administrator(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditAdministrator, false, false));
                                //
                                self.DataEntityObject().AdministratorID(objectInfo ? objectInfo.ID : '');
                                self.DataEntityObject().AdministratorClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeAdministrator();
                            },
                            nosave: true
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
                            fieldName: 'Note',
                            fieldFriendlyName: getTextResource('AssetNumber_Note'),
                            oldValue: asset.Note(),
                            allowNull: true,
                            maxLength: 500,
                            onSave: function (newText) {
                                self.DataEntityObject().Note(newText);
                            },
                            nosave: true
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
                            fieldName: 'Type',
                            fieldFriendlyName: getTextResource('Type'),
                            oldValue: { ID: asset.ProductCatalogTypeID(), FullName: asset.ProductCatalogTypeName() },
                            searcherName: 'ProductCatalogTypeAndModelSearcher',
                            searcherParams: [true, false, false, false, false, false, false, false, 165],
                            onSave: function (objectInfo) {
                                nosave: self.DataEntityObject().ProductCatalogTypeID(objectInfo ? objectInfo.ID : null);
                                nosave: self.DataEntityObject().ProductCatalogTypeName(objectInfo ? objectInfo.FullName : '');
                            },
                            nosave: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                };
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
                            fieldName: 'DeviceApplicationName',
                            fieldFriendlyName: getTextResource('DataEntity_ControlApplication'),
                            oldValue: asset.DeviceApplicationName(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                self.DataEntityObject().DeviceApplicationName(newText);
                            },
                            nosave: true
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
                            fieldName: 'VolumeName',
                            fieldFriendlyName: getTextResource('DataEntity_DataVolume'),
                            oldValue: asset.VolumeName(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                self.DataEntityObject().VolumeName(newText);
                            },
                            nosave: true
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
                            fieldName: 'InfrastructureSegment',
                            fieldFriendlyName: getTextResource('InfrastructureSegment'),
                            oldValue: { ID: asset.InfrastructureSegmentID(), ClassID: 366, FullName: asset.InfrastructureSegmentName() },
                            searcherName: 'InfrastructureSegmentSearcher',
                            onSave: function (objectInfo) {
                                self.DataEntityObject().InfrastructureSegmentID(objectInfo ? objectInfo.ID : null);
                                self.DataEntityObject().InfrastructureSegmentName(objectInfo ? objectInfo.FullName : '');
                            },
                            nosave: true
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
                            nosave: true
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
                            fieldName: 'DataEntityObject.DateReceived',
                            fieldFriendlyName: getTextResource('Contract_CreateDate'),
                            oldValue: self.DataEntityObject().DateReceivedDT(),
                            onSave: function (newDate) {
                                self.DataEntityObject().DateReceived(parseDate(newDate));
                                self.DataEntityObject().DateReceivedDT(new Date(parseInt(newDate)));
                            },
                            nosave: true
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
                            fieldName: 'DataEntityObject.DateAnnuled',
                            fieldFriendlyName: getTextResource('AssetNumber_DateAnnuled'),
                            oldValue: self.DataEntityObject().DateAnnuledDT(),
                            onSave: function (newDate) {
                                self.DataEntityObject().DateAnnuled(parseDate(newDate));
                                self.DataEntityObject().DateAnnuledDT(new Date(parseInt(newDate)));
                            },
                            nosave: true
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
                //
                self.Save = function () {
                        var retval = $.Deferred();
                        // 
                    if (self.DataEntityObject().Name() == '' || self.DataEntityObject().Name() == null) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('NullParamsError'), getTextResource('MustSetName'), 'error');
                        });
                        retval.resolve(false);
                        return false;
                    }
                    if (self.DataEntityObject().ProductCatalogTypeName() == '' || self.DataEntityObject().ProductCatalogTypeName() == null) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('NullParamsError'), getTextResource('MustSetType'), 'error');
                        });
                        retval.resolve(false);
                        return false;
                    };
                        var data = {
                            'ID': self.DataEntityObject().ID,
                            'Name': self.DataEntityObject().Name,
                            //'LifeCycleStateID': self.DataEntityObject().LifeCycleStateID,
                            'DeviceApplicationID': self.DataEntityObject().DeviceApplicationID,
                            'VolumeID': self.DataEntityObject().VolumeID,
                            'ProductCatalogTypeID': self.DataEntityObject().ProductCatalogTypeID,
                            'CriticalityID': self.DataEntityObject().CriticalityID,
                            'InfrastructureSegmentID': self.DataEntityObject().InfrastructureSegmentID,
                            'ClientID': self.DataEntityObject().ClientID,
                            'ClientClassID': self.DataEntityObject().ClientClassID,
                            'AdministratorID': self.DataEntityObject().AdministratorID,
                            'AdministratorClassID': self.DataEntityObject().AdministratorClassID,
                            'Note': self.DataEntityObject().Note,
                            'DateReceivedDT': self.DataEntityObject().DateReceived() == null ? '' : dtLib.GetMillisecondsSince1970(self.DataEntityObject().DateReceivedDT()),
                            'DateAnnuledDT': self.DataEntityObject().DateAnnuled() == null ? '' : dtLib.GetMillisecondsSince1970(self.DataEntityObject().DateAnnuledDT()),
                            'DateReceived': self.DataEntityObject().DateReceivedDT(),
                            'DateAnnuled': self.DataEntityObject().DateAnnuledDT(),
                            'Size': self.DataEntityObject().Size,
                        };
                        //
                        showSpinner();
                    self.ajaxControl_DataEntityObject.Ajax(null,
                            {
                                url: '/assetApi/AddDataEntityObject',
                                method: 'POST',
                                dataType: 'json',
                                data: data
                            },
                            function (response) {//ServiceContractRegistrationResponse
                                hideSpinner();
                                if (response) {                                    
                                    //
                                    if (response.Result == 0) {//ok 
                                        retval.resolve(true);
                                        return true;
                                    }
                                    else if (response.Result != 0)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                        });
                                }
                                retval.resolve(false);
                            },
                            function (response) {
                                hideSpinner();
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmDataEntityRegistration.js, register]', 'error');
                                });
                                retval.resolve(false);
                                return false;
                            });
                        //
                        return retval.promise();
                };
            },
            ShowDialog: function (isSpinnerActive) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                $.when(userD, operationIsGrantedD(615)).done(function (user, operation_add) {
                    var isReadOnly = false;
                    if (user.HasRoles == false || operation_add == false)
                        isReadOnly = true;
                    //
                    var forceClose = false;
                    var frm = undefined;
                    //
                    var buttons = {}
                    var vm = null;
                    buttons[getTextResource('Add')] = function () {
                        //frm.BeforeSave();
                        $.when(vm.Save()).done(function (tmp) {
                            if (tmp == true)
                                frm.Close();
                        });
                    };
                    buttons[getTextResource('CancelButtonText')] = function () {
                        //frm.BeforeClose();
                        frm.Close();
                    };

                //
                    //
                    frm = new fc.control(
                        'region_dataEntityObjectForm_',//form region prefix
                        'setting_dataEntityObjectForm_',//location and size setting
                        getTextResource('Asset_Add'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        850, 650,//minSize
                        buttons,//form buttons
                        function () {
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/frmDataEntityRegistration\', afterRender: AfterRender}"'//attributes of form region
                    );
                    //
                    if (!frm.Initialized) {//form with that region and settingsName was open                    
                        //
                        require(['sweetAlert'], function () {
                            swal(getTextResource('OpenError'), getTextResource('CantDuplicateForm'), 'warning');
                        });
                        return;
                    }
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
                    };
                    //
                    ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                    $.when(frm.Show(), vm.Load()).done(function (frmD, loadD) {
                        if (loadD == false) {//force close
                            frm.Close();
                        }
                        else {
                            if (!ko.components.isRegistered('dataEntityObjectCaptionComponent'))
                                ko.components.register('dataEntityObjectCaptionComponent', {
                                    template: '<span data-bind="text: $captionText"/>'
                                });
                            frm.BindCaption(vm, "component: {name: 'dataEntityObjectCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Add\') + ' ' + $data.DataEntityObject().Name()} }");
                        }
                        hideSpinner();
                    });
                });
            }
        }
        return module;
    });