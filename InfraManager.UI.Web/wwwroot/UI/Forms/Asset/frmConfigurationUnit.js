define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'ttControl', 'models/AssetForms/AssetFields', 'models/AssetForms/ReferenceControl', 'models/AssetForms/AssetReferenceControl', 'ui_forms/Asset/Controls/AdapterList', 'ui_forms/Asset/Controls/PeripheralList', 'ui_forms/Asset/Controls/PortList', 'ui_forms/Asset/Controls/SlotList', 'ui_forms/Asset/Controls/SoftwareInstallationList', 'models/AssetForms/AssetForm.History', 'parametersControl', './ClusterVMForTable',], function (ko, $, ajaxLib, fc, fhModule, tclib, assetFields, referenceControl, assetReferenceControl, adapterList, peripheralList, portList, slotList, installationList, assetHistory, pcLib, clusterVM) {
    var module = {
        ViewModel: function (isReadOnly, $region) {
            var self = this;
            self.$region = $region;
            //
            self.ClassID = 409;//OBJ_ConfigurationUnit
            self.configurationUnit = ko.observable(null);
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
                if (!self.configurationUnit())
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
                if (!self.configurationUnit())
                    return false;
                if (self.configurationUnit().LifeCycleStateName() === 'Списано')
                    return false;
                if (!self.configurationUnit().ObjectID())
                    return false;
                //
                return !self.IsReadOnly();
            });
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                attachments: 'attachments',
                history: 'history',
                clusterVM: 'clusterVM'
            };
            //
            self.GetTabSize = function () {
                return {
                    h: parseInt(self.TabHeight().replace('px', '')),
                    w: parseInt(self.TabWidth().replace('px', ''))
                };
            };
            //
            self.assetHistory = new assetHistory.Tape(self.configurationUnit, self.ClassID, self.$region.find('.assetHistory .tabContent').selector);
            //
            //MAIN TAB BLOCK
            self.mode = ko.observable(self.modes.main);
            self.mode.subscribe(function (newValue) {
                if (newValue == self.modes.nothing)
                    return;
                //
                else if (newValue == self.modes.history) {
                    self.assetHistory.CheckData();
                }
                else if (newValue == self.modes.attachments) {
                    self.InitializeAttachmentControl();
                }
            });
            //
            self.isHost = function () {
                return self.configurationUnit().ProductCatalogTemplateID() == 419;
            }
            //
            self.MainClick = function () {
                self.mode(self.modes.main);
            };
            //
            self.HistoryClick = function () {
                self.mode(self.modes.history);
                self.SizeChanged();
            };
            self.AttachmentsClick = function () {
                self.mode(self.modes.attachments);
            };
            //
            self.VMClick = function () {
                self.mode(self.modes.clusterVM);
                self.SizeChanged();
            };
            //
            self.clusterVMForTable = new clusterVM.List(self);
            //
            self.Load = function (id) {
                var retD = $.Deferred();
                $.when(self.LoadConfigurationUnit(id)).done(function (isLoaded) {
                    retD.resolve(isLoaded);
                    if (isLoaded) {
                        self.InitializeAdministrator();
                        self.SizeChanged();
                        //
                        self.LoadAssetOperationControl();
                    }
                });
                //
                return retD.promise();
            };
            //
            self.ajaxControl_ConfigurationUnit = new ajaxLib.control();
            //
            self.LoadConfigurationUnit = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_ConfigurationUnit.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetConfigurationUnit'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var cu = newVal.ConfigurationUnit;
                                if (cu) {
                                    require(['ui_forms/Asset/ConfigurationUnit'], function (cuLib) {
                                        self.configurationUnit(new cuLib.ConfigurationUnit(self, cu));
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
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[frmConfigurationUnit.js, Load]', 'error');
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
            //
            self.AssetOperationControl = ko.observable(null);
            self.LoadAssetOperationControl = function () {
                if (!self.configurationUnit())
                    return;
                //
                require(['assetOperations'], function (assetOperationLib) {
                    if (self.AssetOperationControl() == null) {
                        self.AssetOperationControl(new assetOperationLib.control(self.$region, self.configurationUnit, self.Load));
                    }
                    self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                    self.AssetOperationControl().Initialize();
                });
            };
            //
            self.EditName = function () {
                if (!self.CanEdit() || !self.configurationUnit().CanEditName())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Name',
                        fieldFriendlyName: getTextResource('AssetNumber_Name'),
                        oldValue: asset.Name(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.Name(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            self.EditSerialNumber = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'SerialNumber',
                        fieldFriendlyName: getTextResource('Asset_SerialNumber'),
                        oldValue: asset.SerialNumber(),
                        allowNull: true,
                        maxLength: 255,
                        onSave: function (newText) {
                            asset.SerialNumber(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditInfrastructureSegment = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
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
            //
            self.EditCriticality = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Criticality',
                        fieldFriendlyName: getTextResource('Criticality'),
                        oldValue: { ID: asset.CriticalityID(), ClassID: 367, FullName: asset.CriticalityName() },
                        searcherName: 'CriticalitySearcher',
                        onSave: function (objectInfo) {
                            asset.CriticalityID(objectInfo ? objectInfo.ID : null);
                            asset.CriticalityName(objectInfo ? objectInfo.FullName : '');
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.EditAdministrator = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Administrator',
                        fieldFriendlyName: getTextResource('Administrator'),
                        oldValue: asset.AdministratorLoaded() ? { ID: asset.Administrator().ID(), ClassID: asset.Administrator().ClassID(), FullName: asset.Administrator().FullName() } : null,
                        object: ko.toJS(asset.Administrator()),
                        searcherName: 'AssetAdministratorSearcher',
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
            //открыть форму просмотра связанной лицензии
            self.ShowDeviceForm = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var cu = self.configurationUnit();
                    if (cu.AgentClassID() == 415 ||
                        cu.AgentClassID() == 416 ||
                        cu.AgentClassID() == 417 || 
                        cu.AgentClassID() == 418 || 
                        cu.AgentClassID() == 12)
                        fh.ShowLogicalObjectForm(cu.ObjectID());
                    else
                        fh.ShowAssetForm(cu.ObjectID(), cu.ObjectClassID());
                });
            };
            //
            self.InitializeAdministrator = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.configurationUnit();
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
            //for comboBox
            {
                self.createComboBoxItem = function (simpleDictionary) {
                    var thisObj = this;
                    //
                    thisObj.ID = simpleDictionary.ID;
                    thisObj.Name = simpleDictionary.Name;
                };
                //
                self.createComboBoxHelper = function (container_selector, getUrl, comboBoxFunc) {
                    var thisObj = this;
                    if (!comboBoxFunc)
                        comboBoxFunc = self.createComboBoxItem;
                    //
                    thisObj.SelectedItem = ko.observable(null);
                    //
                    thisObj.ItemList = ko.observableArray([]);
                    thisObj.ItemListD = $.Deferred();
                    thisObj.getItemList = function (options) {
                        var data = thisObj.ItemList();
                        options.callback({ data: data, total: data.length });
                    };
                    //
                    thisObj.ajaxControl = new ajaxLib.control();
                    thisObj.LoadList = function () {
                        thisObj.ajaxControl.Ajax($(container_selector),
                            {
                                url: getUrl,
                                method: 'GET'
                            },
                            function (response) {
                                if (response) {
                                    thisObj.ItemList.removeAll();
                                    //
                                    $.each(response, function (index, simpleDictionary) {
                                        var u = new comboBoxFunc(simpleDictionary);
                                        thisObj.ItemList().push(u);
                                    });
                                    thisObj.ItemList.valueHasMutated();
                                }
                                thisObj.ItemListD.resolve();
                            });
                    };
                    //
                    thisObj.GetObjectInfo = function (classID) {
                        return thisObj.SelectedItem() ? { ID: thisObj.SelectedItem().ID, ClassID: classID, FullName: thisObj.SelectedItem().Name } : null;
                    };
                    thisObj.SetSelectedItem = function (name) {
                        $.when(thisObj.ItemListD).done(function () {
                            var item = null;
                            if (name != undefined && name != null)
                                for (var i = 0; i < thisObj.ItemList().length; i++) {
                                    var tmp = thisObj.ItemList()[i];
                                    if (tmp.Name == name) {
                                        item = tmp;
                                        break;
                                    }
                                }
                            thisObj.SelectedItem(item);
                        });
                    };
                }
            }
            //
            self.EditIPAddress = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'IPAddress',
                        fieldFriendlyName: getTextResource('AssetIPAddress'),
                        oldValue: asset.IPAddress(),
                        onSave: function (newText) {
                            asset.IPAddress(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.ipAddressEdit, options);
                });
            };
            //
            self.EditIPMask = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'IPMask',
                        fieldFriendlyName: getTextResource('Asset_IPMask'),
                        oldValue: asset.IPMask(),
                        onSave: function (newText) {
                            asset.IPMask(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.ipAddressEdit, options);
                });
            };
            //
            self.EditDescription = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Description',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: asset.Description(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.Description(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditNote = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Note',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_Comment'),
                        oldValue: asset.Note(),
                        allowNull: true,
                        maxLength: 1000,
                        onSave: function (newText) {
                            asset.Note(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditDataCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'DataCenter',
                        fieldFriendlyName: getTextResource('DataCenter'),
                        oldValue: asset.DataCenter(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.DataCenter(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditVCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'VCenter',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_VCenter'),
                        oldValue: asset.VCenter(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.VCenter(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            self.EditCluster = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.configurationUnit();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'Cluster',
                        fieldFriendlyName: getTextResource('Cluster'),
                        oldValue: { ID: asset.ClusterID(), FullName: asset.ClusterName() },
                        searcherName: 'ClusterSearcher',
                        searcherPlaceholder: getTextResource('Cluster'),
                        searcherParams: [420, true],
                        onSave: function (objectInfo) {
                            asset.ClusterID(objectInfo ? objectInfo.ID : null);
                            asset.ClusterName(objectInfo ? objectInfo.FullName : '');
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                });
            };
            //
            self.CheckShowDescriptionTooltipMessage = function (obj, event) {
                if (obj && event) {
                    var $this = $(event.currentTarget);
                    //
                    var hiddenElement = $this.find('span:first').clone().appendTo('body');
                    var nameWidth = hiddenElement.width();
                    hiddenElement.remove();
                    //
                    var areaWidth = $('.asset-text-field.asset-description').width();
                    //
                    if (!nameWidth || !areaWidth)
                        return;
                    //
                    if (nameWidth > areaWidth) {
                        var ttcontrol = new tclib.control();
                        ttcontrol.init($this, { text: self.configurationUnit().Description(), showImmediat: true, showTime: false });
                    }
                }
                return true;
            };
            //
            {//attachments
                {//variables
                    self.attachmentsControl = null;
                }
                //
                {//events
                    self.CanEdit_handle = self.CanEdit.subscribe(function (newValue) {
                        if (self.attachmentsControl != null)
                            self.attachmentsControl.RemoveFileAvailable(newValue);
                        if (self.attachmentsControl != null)
                            self.attachmentsControl.ReadOnly(!newValue);
                    });
                }
                //
                //when tab selected
                self.InitializeAttachmentControl = function () {
                    if (!self.configurationUnit() || !self.configurationUnit().ObjectID())
                        return;
                    //
                    require(['fileControl'], function (fcLib) {
                        var attachmentsElement = $('#' + self.frm.GetRegionID()).find('.documentList');
                        //
                        if (self.attachmentsControl != null) {
                            if (self.attachmentsControl.ObjectID != self.configurationUnit().ObjectID())//previous object  
                                self.attachmentsControl.RemoveUploadedFiles();
                        }
                        if (self.attachmentsControl == null) {
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        }
                        self.attachmentsControl.ReadOnly(!self.CanEdit());
                        self.attachmentsControl.RemoveFileAvailable(self.CanEdit());
                        //
                        if (self.attachmentsControl.ObjectID != self.configurationUnit().ObjectID())
                            self.attachmentsControl.Initialize(self.configurationUnit().ObjectID());
                        else if (self.attachmentsControl.IsLoaded() == false)
                            self.attachmentsControl.Load(attachmentsElement)
                    });
                };
                //
                self.DisposeAttacmentControl = function () {
                    if (self.attachmentsControl != null && !self.attachmentsControl.IsAllFilesUploaded())
                        require(['sweetAlert'], function () {
                            swal({
                                title: getTextResource('UploadedFileNotFoundAtServerSide'),
                                text: getTextResource('FormClosingQuestion'),
                                showCancelButton: true,
                                closeOnConfirm: true,
                                closeOnCancel: true,
                                confirmButtonText: getTextResource('ButtonOK'),
                                cancelButtonText: getTextResource('ButtonCancel')
                            },
                            function (value) {
                                if (value == true) {
                                    self.attachmentsControl.StopUpload();
                                }
                            });
                        });
                };
            }
        },
        ShowDialog: function (id, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            $.when(userD, operationIsGrantedD(953)).done(function (user, operation_update) {//OPERATION_ConfigurationUnit_Update = 953
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
                    'region_configurationUnitForm_',//form region prefix
                    'setting_configurationUnitForm_',//location and size setting
                    getTextResource('Asset_Properties'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    600, 370,//minSize
                    buttons,//form buttons
                    function () {
                        vm.DisposeAttacmentControl();
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmConfigurationUnit\', afterRender: AfterRender}"'//attributes of form region
                    );
                //
                if (!frm.Initialized) {
                    hideSpinner();
                    //
                    var wnd = window.open(window.location.protocol + '//' + window.location.host + location.pathname + '?configurationUnitID=' + id);
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
                        if (!ko.components.isRegistered('configurationUnitFormCaptionComponent'))
                            ko.components.register('configurationUnitFormCaptionComponent', {
                                template: '<span data-bind="text: $captionText"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'configurationUnitFormCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Properties\') + ' ' + getTextResource(\'NumberSymbol\') + $data.asset().SerialNumber() } }");
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});