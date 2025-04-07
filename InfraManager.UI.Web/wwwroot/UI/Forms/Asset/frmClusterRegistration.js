define(['knockout', 'jquery', 'ajax',
    'formControl', 'usualForms', 'ttControl',
    'models/AssetForms/AssetForm.History',
    'parametersControl'], function (ko, $, ajaxLib, fc, fhModule, tclib, assetHistory, pcLib) {
    var module = {
        ViewModel: function (isReadOnly, $region) {
            var self = this;
            self.$region = $region;
            //
            self.ID = null;
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
                var tabHeight = self.$region.height();//form height
                tabHeight -= 140;
                var tabWidth = self.$region.width();//form width
                tabWidth -= self.$region.find('.b-requestDetail-right').outerWidth(true);
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
            self.CanShow = ko.observable(self.CanEdit);
            //
            self.modes = {
                nothing: 'nothing',
                main: 'main'
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
                else if (newValue == self.modes.history) {
                    self.assetHistory.CheckData();
                }
            });
            //
            self.MainClick = function () {
                self.mode(self.modes.main);
            };
            //
            self.Load = function () {
                var retD = $.Deferred();
                $.when(self.LoadCluster()).done(function (isLoaded) {
                    if (isLoaded) {
                        self.InitializeAdministrator();
                        self.SizeChanged();
                    }
                    retD.resolve(isLoaded);
                });
                //
                return retD.promise();
            };
            //
            self.ajaxControl_Cluster = new ajaxLib.control();
            //
            self.LoadCluster = function () {
                var retDD = $.Deferred();
                require(['ui_forms/Asset/Cluster'], function (cLib) {
                    self.cluster(new cLib.ClusterRegistration(self));
                    retDD.resolve(true);
                });
                return retDD.promise();
            };
            //
            //
            self.AfterRender = function () {
                self.frm.SizeChanged();
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
                        nosave: true
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
                        fieldName: 'DataCenter',
                        fieldFriendlyName: getTextResource('DataCenter'),
                        oldValue: asset.DataCenter(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.DataCenter(newText);
                        },
                        nosave: true
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
                        fieldName: 'VCenter',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_VCenter'),
                        oldValue: asset.VCenter(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.VCenter(newText);
                        },
                        nosave: true
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
                        fieldName: 'UUID',
                        fieldFriendlyName: 'UUID',
                        oldValue: asset.UUID(),
                        allowNull: true,
                        maxLength: 250,
                        onSave: function (newText) {
                            asset.UUID(newText);
                        },
                        nosave: true
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
                        },
                        nosave: true

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
                        fieldName: 'Note',
                        fieldFriendlyName: getTextResource('ConfigurationUnit_Comment'),
                        oldValue: asset.Note(),
                        allowNull: true,
                        maxLength: 1000,
                        onSave: function (newText) {
                            asset.Note(newText);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
            //
            //
            self.EditType = function () {
                if (!self.CanEdit())
                    return;
                //
                showSpinner();
                var asset = self.cluster();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    //
                    var options = {
                        fieldName: 'Type',
                        fieldFriendlyName: getTextResource('Type'),
                        oldValue: { ID: asset.ProductCatalogTypeID(), FullName: asset.ProductCatalogTypeName() },
                        searcherName: 'ProductCatalogTypeAndModelSearcher',
                        searcherParams: [true, false, false, false, false, false, false, false, 420],
                        onSave: function (objectInfo) {
                            nosave: self.cluster().ProductCatalogTypeID(objectInfo ? objectInfo.ID : null);
                            nosave: self.cluster().ProductCatalogTypeName(objectInfo ? objectInfo.FullName : '');
                        },
                        nosave: true
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
                self.Save = function () {
                    var retval = $.Deferred();
                    // 
                    if (self.cluster().Name() == '' || self.cluster().Name() == null) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('NullParamsError'), getTextResource('MustSetName'), 'error');
                        });
                        retval.resolve(false);
                        return false;
                    };
                    if (self.cluster().ProductCatalogTypeName() == '' || self.cluster().ProductCatalogTypeName() == null) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('NullParamsError'), getTextResource('MustSetType'), 'error');
                        });
                        retval.resolve(false);
                        return false;
                    };
                    var data = {
                        'ID': self.cluster().ID,
                        'Name': self.cluster().Name,
                        'ProductCatalogTypeID': self.cluster().ProductCatalogTypeID,
                        'DataCenter': self.cluster().DataCenter,
                        'VCenter': self.cluster().VCenter,
                        'UUID': self.cluster().UUID,
                        'InfrastructureSegmentID': self.cluster().InfrastructureSegmentID,
                        'AdministratorID': self.cluster().AdministratorID,
                        'AdministratorClassID': self.cluster().AdministratorClassID,
                        'Note': self.cluster().Note
                    };
                    //
                    showSpinner();
                    self.ajaxControl_Cluster.Ajax(null,
                        {
                            url: '/assetApi/AddCluster',
                            method: 'POST',
                            dataType: 'json',
                            data: data
                        },
                        function (response) {
                            hideSpinner();
                            if (response) {
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
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmClusterRegistration.js, register]', 'error');
                            });
                            retval.resolve(false);
                            return false;
                        });
                    //
                    return retval.promise();
                };
            }
        },
        ShowDialog: function (isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            $.when(userD, operationIsGrantedD(956)).done(function (user, operation_update) {
                var isReadOnly = false;
                if (user.HasRoles == false || operation_update == false)
                    isReadOnly = true;
                //
                var buttons = {}
                var frm = undefined;
                var vm = null;
                //
                buttons[getTextResource('Add')] = function () {
                    $.when(vm.Save()).done(function (tmp) {
                        if (tmp == true)
                            frm.Close();
                    });
                };

                buttons[getTextResource('CancelButtonText')] = function () {
                    frm.Close();
                };              
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
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmClusterRegistration\', afterRender: AfterRender}"'//attributes of form region
                );
                //
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
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
                $.when(frm.Show(), vm.Load()).done(function (frmD, loadD) {
                    if (loadD == false) {//force close
                        frm.Close();
                    }
                    else {
                        if (!ko.components.isRegistered('clusterFormCaptionComponent'))
                            ko.components.register('clusterFormCaptionComponent', {
                                template: '<span data-bind="text: $captionText"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'clusterFormCaptionComponent', params: {  $captionText: getTextResource(\'Asset_Add\') } }");                       
                    }
                    hideSpinner();
                });
            });
        }
    }
    return module;
});