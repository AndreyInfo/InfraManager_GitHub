define(['knockout', 'jquery', 'ajax',
    'formControl', 'usualForms', 'ttControl',
    'models/AssetForms/AssetFields',
    'models/AssetForms/ReferenceControl',
    'models/AssetForms/AssetReferenceControl',
    'ui_forms/Asset/Controls/AdapterList',
    'ui_forms/Asset/Controls/PeripheralList',
    'ui_forms/Asset/Controls/PortList', 'ui_forms/Asset/Controls/SlotList',
    'ui_forms/Asset/Controls/SoftwareInstallationList', 'models/AssetForms/AssetForm.History',
    './ClusterVMForTable',
    './ClusterHostsForTable',
    'parametersControl'], function (ko, $, ajaxLib,
    fc, fhModule, tclib,
    assetFields,
    referenceControl,
    assetReferenceControl,
    adapterList,
    peripheralList,
    portList, slotList,
    installationList, assetHistory,
    clusterVM,
    clusterHosts,
    pcLib) {
    var module = {
        ViewModel: function (isReadOnly, $region) {
            var self = this;
            self.$region = $region;
            //
            self.ClassID = 420;//OBJ_Cluster
            self.cluster = ko.observable(null);
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
                if (!self.cluster())
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
                if (!self.cluster())
                    return false;
                if (self.cluster().LifeCycleStateName() === 'Списано')
                    return false;
                //
                return !self.IsReadOnly();
            });
            //
            self.AssetOperationControl = ko.observable(null);
            self.LoadAssetOperationControl = function () {
                if (!self.cluster())
                    return;
                //
                require(['assetOperations'], function (wfLib) {
                    if (self.AssetOperationControl() == null) {
                        self.AssetOperationControl(new wfLib.control(self.$region, self.cluster, self.Load));
                    }
                    self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                    self.AssetOperationControl().Initialize();
                });
            };
            //
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.modes = {
                nothing: 'nothing',
                main: 'main',
                attachments: 'attachments',
                history: 'history',
                clusterVM: 'clusterVM',
                clusterHosts: 'clusterHosts'

            };
            //
            self.GetTabSize = function () {
                return {
                    h: parseInt(self.TabHeight().replace('px', '')),
                    w: parseInt(self.TabWidth().replace('px', ''))
                };
            };
            //
            self.assetHistory = new assetHistory.Tape(self.cluster, self.ClassID, self.$region.find('.assetHistory .tabContent').selector);
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
            
            self.HostsClick = function () {
                self.mode(self.modes.clusterHosts);
                self.SizeChanged();
            };
            //
            self.clusterVMForTable = new clusterVM.List(self);
            self.clusterHostsForTable = new clusterHosts.List(self);
            //
            self.Load = function (id) {
                var retD = $.Deferred();
                $.when(self.LoadCluster(id)).done(function (isLoaded) {
                    retD.resolve(isLoaded);
                    if (isLoaded) {
                        self.InitializeAdministrator();
                        self.LoadAssetOperationControl();
                        self.SizeChanged();
                    }
                });
                //
                return retD.promise();
            };
            //
            self.ajaxControl_Cluster = new ajaxLib.control();
            //
            self.LoadCluster = function (id) {
                var retD = $.Deferred();
                //
                if (!id) {
                    retD.resolve(false);
                    return retD.promise();
                }
                //
                var data = { 'ID': id };
                self.ajaxControl_Cluster.Ajax(self.$region,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: data,
                        url: '/assetApi/GetCluster'
                    },
                    function (newVal) {
                        var loadSuccessD = $.Deferred();
                        var processed = false;
                        //
                        if (newVal) {
                            if (newVal.Result == 0) {
                                var c = newVal.Cluster;
                                if (c) {
                                    require(['ui_forms/Asset/Cluster'], function (cLib) {
                                        self.cluster(new cLib.Cluster(self, c));
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
                                    swal(getTextResource('UnhandledErrorServer'), getTextResource('AjaxError') + '\n[frmCluster.js, Load]', 'error');
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
            self.EditName = function () {
                if (!self.CanEdit() || !self.cluster().CanEditName())
                    return;
                //
                showSpinner();
                var asset = self.cluster();
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
            //
            self.EditDataCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.cluster();
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
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            self.EditVCenter = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.cluster();
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
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            self.EditUUID = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.cluster();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: asset.ID(),
                        objClassID: self.ClassID,
                        fieldName: 'UUID',
                        fieldFriendlyName: 'UUID',
                        oldValue: asset.UUID(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.UUID(newText);
                        },
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.singleLineTextEdit, options);
                });
            };
            //
            self.EditAdministrator = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.cluster();
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
            self.InitializeAdministrator = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    var a = self.cluster();
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
            self.EditNote = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.cluster();
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
                        ttcontrol.init($this, { text: self.cluster().Description(), showImmediat: true, showTime: false });
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
                    require(['fileControl'], function (fcLib) {
                        var attachmentsElement = $('#' + self.frm.GetRegionID()).find('.documentList');
                        //
                        if (self.attachmentsControl != null) {
                            if (self.attachmentsControl.ObjectID != self.cluster().ObjectID())//previous object  
                                self.attachmentsControl.RemoveUploadedFiles();
                        }
                        if (self.attachmentsControl == null) {
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                        }
                        self.attachmentsControl.ReadOnly(!self.CanEdit());
                        self.attachmentsControl.RemoveFileAvailable(self.CanEdit());
                        //
                        if (self.attachmentsControl.ObjectID != self.cluster().ObjectID())
                            self.attachmentsControl.Initialize(self.cluster().ObjectID());
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
            $.when(userD, operationIsGrantedD(957)).done(function (user, operation_update) {//OPERATION_Cluster_Update = 957
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
                    'region_clusterForm_',//form region prefix
                    'setting_clusterForm_',//location and size setting
                    getTextResource('Asset_Properties'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    850, 730,//minSize
                    buttons,//form buttons
                    function () {
                        vm.DisposeAttacmentControl();
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmCluster\', afterRender: AfterRender}"'//attributes of form region
                    );
                //
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
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
                        if (!ko.components.isRegistered('clusterFormCaptionComponent'))
                            ko.components.register('clusterFormCaptionComponent', {
                                template: '<span data-bind="text: $captionText"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'clusterFormCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Properties\') + ' ' + getTextResource(\'NumberSymbol\') + $data.asset().SerialNumber() } }");
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});