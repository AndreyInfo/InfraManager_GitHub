define(['knockout', 'jquery', 'ajax', 'formControl', './SoftwareLicence',
    'models/AssetForms/AssetFields',
    './frmSoftwareLicence_assetFieldsTab',
    './frmSoftwareLicence_parameterListTab',
    './frmSoftwareLicence_generalTab',
    './frmSoftwareLicence_serialNumbersTab',
    './frmSoftwareLicence_referencesTab',
    './frmSoftwareLicence_attachmentsTab',
    './frmSoftwareLicence_historyTab',
    './frmSoftwareLicence_updatesTab',
    './frmSoftwareLicense_distributionTab',
    'usualForms', 'dateTimeControl', 'jqueryStepper'],
    function (ko, $, ajax, formControl, m_objects,
        assetFields,
        tab_assetFields,
        tab_parameters,
        tab_general,
        tab_serialNumbers,
        tab_references,
        tab_attachments,
        tab_history,
        tab_updates,
        tab_distribution,
        fhModule, dtLib) {
        var module = {
            ViewModel: function ($region, can_view_serialNumber) {
                var self = this;                
                self.$region = $region;
                self.asset = ko.observable(null);
                self.CanEdit = ko.observable(true);
                //Объект лицензии
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    
                    self.tab_parameters.Initialize();
                    
                    self.tabList().forEach(function (tab) {
                        tab.Initialize(newObject);
                    });
                    if (self.tabActive()) {
                        self.tabActive().load();//reload active tab
                    }
                });

                //Объект вкладки "Имущество"
                self.assetFields = new assetFields.AssetFields(self.object, self.$region, self.CanEdit);

                self.EditInventoryNumber = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var object = self.object();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID(),
                            fieldName: 'InventoryNumber',
                            fieldFriendlyName: getTextResource('Repair_InventoryNumber'),
                            oldValue: object.InventoryNumber(),
                            allowNull: true,
                            maxLength: 50,
                            ReplaceAnyway: true,
                            onSave: function (newText) {
                                object.InventoryNumber(newText);
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };

                //заголовок
                self.captionText = ko.pureComputed(function () {
                    var obj = self.object();
                    var retval = '';
                    if (obj != null) {
                        retval += getTextResource('SoftwareLicence_Properties') + ' ' + obj.Name() + ' ' + obj.ManufacturerName()
                            + ' ' + getTextResource('Asset_InventoryNumberCaption') + obj.InventoryNumber();
                    }
                    return retval;
                });
                //

                self.raiseObjectModified = function () {
                    var object = self.object();
                    $(document).trigger('local_objectUpdated', [223, object.ID(), null]);//softwareLicence
                };

                //tabs
                {
                    //only for parameters
                    {
                        self.tab_parameters = new tab_parameters.Tab(self);
                        self.objectClassID = 223;//OBJ_SOFTWARE_LICENSE
                    }

                    let arrayTabs = [];
                    if (can_view_serialNumber) {
                        arrayTabs =
                            [
                                new tab_general.Tab(self),                                                      //0
                                new tab_assetFields.Tab(self.object, self.$region, self.CanEdit),               //1
                                new tab_serialNumbers.Tab(self),                                                //2
                                new tab_references.Tab(self),                                                   //3
                                new tab_history.Tab(self),                                                      //4
                                new tab_attachments.Tab(self),                                                  //5
                                new tab_updates.Tab(self),                                                      //6
                                
                            ];
                    }
                    else {
                        arrayTabs =
                            [
                                new tab_general.Tab(self),                                                      //0
                                new tab_assetFields.Tab(self.object, self.$region, self.CanEdit),               //1
                                new tab_references.Tab(self),                                                   //3
                                new tab_history.Tab(self),                                                      //4
                                new tab_attachments.Tab(self),                                                  //5
                                new tab_updates.Tab(self),                                                      //6                                
                            ];
                    }

                    self.tabList = ko.observableArray(arrayTabs);
                    //
                    self.tabActive = ko.observable(null);
                    self.tabActive_handle = self.tabActive.subscribe(function (selectedTab) {
                        if (selectedTab.hasOwnProperty('load')) {
                            selectedTab.load();
                        }
                    });
                    //
                    self.selectTabClick = function (tab) {
                        self.tabActive(tab);
                    };
                }
                //editors
                {
                    self.editName = function () {
                        if (!self.CanEdit())
                            return;
                        //
                        showSpinner();
                        var object = self.object();
                        require(['usualForms'], function (fhModule) {
                            var fh = new fhModule.formHelper(true);
                            var options = {
                                ID: object.ID(),
                                objClassID: object.ClassID,
                                fieldName: 'Name',
                                fieldFriendlyName: getTextResource('SoftwareLicence_Description'),
                                oldValue: object.Name(),
                                allowNull: true,
                                maxLength: 500,
                                onSave: function (newText) {
                                    object.Name(newText);
                                    self.raiseObjectModified();
                                },
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                        });
                    };
                }
                //
                self.dispose = function () {
                    self.object().ajaxControl.Abort();
                    //
                    self.object_handle.dispose();
                    self.captionText.dispose();
                    self.tabActive_handle.dispose();
                    self.tabList().forEach(function (tab) {
                        if (tab.hasOwnProperty('dispose')) {
                            tab.dispose();
                        }
                    });
                    self.tab_parameters.dispose();
                };
                //rendering
                {
                    self.sizeChanged = function () {
                        var width = self.frm.GetInnerWidth();
                        var height = self.frm.GetInnerHeight();
                        $('#' + self.frm.GetRegionID()).find('.frmSoftwareLicence').css('width', width + 'px').css('height', height + 'px');
                        //
                        var $content = $('#' + self.frm.GetRegionID()).find('.content');
                        $content.find('.tabActive').css('height',
                            Math.max(
                                $content.innerHeight()
                                - $content.find('.b-requestDetail-menu').outerHeight(true)
                                - $content.find('.stateRow').outerHeight(true)
                                - $content.find('.identificationRow').outerHeight(true) 
                                - 2 * parseInt($content.find('.tabActive').css('margin-top'))
                                - 2 * parseInt($content.find('.tabActive').css('margin-bottom'))
                                - 10,
                                0) + 'px');
                    };
                    self.afterRender = function () {
                        self.frm.SizeChanged();
                    };
                }
                
                //initialization (пустой объект, выделяем первый таб)
                {
                    self.object(new m_objects.softwareLicence());//fill object                    
                    self.selectTabClick(self.tabList()[0]);//init general tab
                    self.IsReadOnly = ko.observable(false);
                    self.AssetOperationControl = ko.observable(null);
                    self.LoadAssetOperationControl = function () {

                        require(['assetOperations'], function (wfLib) {
                            if (self.AssetOperationControl() == null) {
                                self.AssetOperationControl(new wfLib.control(self.$region, self.object, self.reload));
                            }
                            self.AssetOperationControl().ReadOnly(self.IsReadOnly());
                            self.AssetOperationControl().Initialize();
                        });
                    };

                    self.LifeCycleStateName = ko.computed(function () {
                        return (self.object()) ? self.object().LifeCycleStateName() : "";
                    });

                    self.reload = function (id, classID) {
                        var retD = $.Deferred();
                        var refreshObject = new m_objects.softwareLicence();
                        if (self.object()) {
                            $.when(refreshObject.load(id)).done(function (loadD) {
                                self.object(refreshObject);
                                self.LoadAssetOperationControl();                                
                            });
                        }
                        return retD.promise();
                    };

                    self.initGeneralTab = function () {
                        self.selectTabClick(self.tabList()[0]);
                    }

                    self.initAssetTab = function () {
                        self.assetFields.Initialize();
                    }
                }
            },

            //OPERATION_SOFTWARELICENCE_DELETE = 443;
            //OPERATION_SOFTWARELICENCE_ADD = 440;

            ShowDialog: function (id, isSpinnerActive) {
                $.when(
                    //OPERATION_SOFTWARELICENCE_PROPERTIES = 441;
                    operationIsGrantedD(441),
                    //OPERATION_SOFTWARELICENCE_UPDATE = 442;
                    operationIsGrantedD(442),
                    //OPERATION_SOFTWARELICENCESERIALNUMBER_PROPERTIES = 523
                    operationIsGrantedD(523)).done(function (
                        can_properties,
                        can_update,
                        can_view_serialNumber) {

                        if (can_properties == false && id || !id) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('OperationError'));
                            });
                            return;
                        }
                        //
                        if (isSpinnerActive != true)
                            showSpinner();
                        //
                        var frm = undefined;
                        var bindElement = null;
                        var buttons = [];
                        frm = new formControl.control(
                            'region_frmSoftwareLicence',//form region prefix
                            'setting_frmSoftwareLicence',//location and size setting
                            getTextResource('SoftwareLicenseView'),//caption
                            true,//isModal
                            true,//isDraggable
                            true,//isResizable
                            1000, 750,//minSize
                            buttons,//form buttons
                            function () {
                                ko.cleanNode(bindElement);
                                vm.dispose();
                                if (vm.AssetOperationControl() != null) {
                                    vm.AssetOperationControl().Unload();
                                }
                            },//afterClose function
                            'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicence\', afterRender: afterRender}"'//attributes of form region
                        );
                        if (!frm.Initialized) {//form with that region and settingsName was open
                            hideSpinner();
                            //
                            var url = window.location.protocol + '//' + window.location.host + location.pathname + '?softwareLicenceID=' + id;
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
                        var $region = $('#' + frm.GetRegionID());
                        var vm = new module.ViewModel($region, can_view_serialNumber);
                        vm.frm = frm;
                        var oldSizeChanged = frm.SizeChanged;
                        frm.SizeChanged = function () {
                            oldSizeChanged();
                            vm.sizeChanged();
                        };
                        //
                        frm.ExtendSize(800, 750);//normal size
                        //
                        bindElement = document.getElementById(frm.GetRegionID());
                        ko.applyBindings(vm, bindElement);

                        //
                        $.when(vm.object().load(id, vm)).done(function (loadD) {
                            //добавляем вкладку сублицензии если применимо


                            if (functionsAvailability
                                && functionsAvailability.SoftwareDistributionCentres) {
                                vm.tabList.push(new tab_distribution.Tab(vm));
                            }

                            //инициализация списков на основной вкладке
                            vm.initGeneralTab();

                            //инициализация объекта вкладки "Имущество"
                            vm.initAssetTab();

                            vm.CanEdit(can_update);
                            vm.object().CanEdit(can_update);
                            vm.assetFields.CanEdit(can_update);

                            $.when(frm.Show()).done(function (frmD) {
                                if (!ko.components.isRegistered('softwareLicenceFormCaptionComponent'))
                                    ko.components.register('softwareLicenceFormCaptionComponent', {
                                        template: '<span data-bind="text: $str"/>'
                                    });
                                frm.BindCaption(vm, "component: {name: 'softwareLicenceFormCaptionComponent', params: { $str: captionText} }");
                                hideSpinner();
                            });

                            vm.LoadAssetOperationControl();
                        });
                    });
            }
        };
        return module;
    });