define(['knockout', 'jquery', 'ajax', 'formControl', './AssetRegistration',
    './frmAssetRegistration_generalTab', './frmAssetRegistration_assetFieldsTab', './frmAssetRegistration_parameterListTab', './frmAssetRegistration_attachmentsTab',
    'usualForms'],
    function (ko, $, ajax, formControl, m_objects,
    tab_general, tab_assetFields, tab_parameters, tab_attachments,
    fhModule) {
        var module = {
            ViewModel: function () {
                var self = this;
                //          
                self.CanEdit = ko.observable(true);
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    self.tab_parameters.init(newObject);
                    self.tabList().forEach(function (tab) {
                        tab.init(newObject);
                    });
                    if (self.tabActive())
                        self.tabActive().load();//reload active tab
                });
                self.searcher_controls = [];
                //
                {//tabs
                    {//only for parameters
                        self.tab_parameters = new tab_parameters.Tab(self);
                    }
                    //
                    self.tabList = ko.observableArray(
                        [
                            new tab_general.Tab(self),
                            new tab_assetFields.Tab(self),
                            new tab_attachments.Tab(self)
                        ]);
                    //
                    self.tabActive = ko.observable(null);
                    self.tabActive_handle = self.tabActive.subscribe(function (selectedTab) {
                        selectedTab.load();
                    });
                    //
                    self.selectTabClick = function (tab) {
                        self.tabActive(tab);
                    };
                }
                //
                {//editors
                    self.GetModelFormName = function () {
                        var name;
                        var classID = self.object().ClassID();
                        if (classID == 5)
                            name = '';//NETWORKDEVICE
                        else if (classID == 6)
                            name = '';//TERMINALDEVICE
                        else if (classID == 33)
                            name = getTextResource('Adapter_SelectModel');//ADAPTER
                        else if (classID == 34)
                            name = getTextResource('Peripheral_SelectModel');//PERIPHERAL
                        //
                        return name;
                    };
                    //
                    self.GetAvailableClassID = function () {

                    };
                    //
                    self.SelectModel = function () {
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            fh.ShowAssetModelLink(
                             false, false,
                             function (newValues) {
                                 if (!newValues || newValues.length == 0)
                                     return;
                                 //
                                 if (newValues.length == 1) {
                                     var object = self.object();
                                     var model = newValues[0];
                                     object.productCatalogID(model.ID);
                                     object.ProductCatalogModelID(model.ID);
                                     object.productCatalogClassID(model.ClassID);
                                     //
                                     object.ProductCatalogCategoryName(model.ExternalIdentifier);
                                     object.ProductCatalogTypeName(model.TypeName);
                                     object.ManufacturerName(model.ManufacturerName);
                                     object.ProductCatalogModelName(model.Name);
                                     object.ProductCatalogTemplateID(model.ProductCatalogTemplateID);
                                     //
                                     var modelFullName = model.ProductCatalogCategoryFullName + ' \\ ' + model.TypeName + ' \\ ' + model.Name;
                                     object.productCatalogFullName(modelFullName);
                                     //
                                     object.LifeCycleStateName(model.LifeCycleStateName);
                                     //
                                     self.InitializeSubdeviceParameterList();
                                 }
                             },
                            true,
                            {
                                TemplateClassID: self.object().ClassID(),
                                HasLifeCycle : false,
                            },
                            self.GetModelFormName()
                            );
                        });
                    };
                    //
                    self.ajaxControl_SubdeviceParameterList = new ajax.control();

                    self.InitializeSubdeviceParameterList = function () {
                        var retD = $.Deferred();
                        //
                        var data =
                            {
                                DeviceClassID: self.object().ClassID(),
                                DeviceID: self.object().ID(),
                                ProductCatalogTemplateID: self.object().ProductCatalogTemplateID(),
                                ProductCatalogModelID: self.object().ProductCatalogModelID()
                            };
                        self.ajaxControl_SubdeviceParameterList.Ajax(null/*self.$region*/,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: '/assetApi/GetSubdeviceDefaultParameterList'
                        },
                        function (newVal) {
                            if (newVal) {
                                if (newVal.Result == 0) {
                                    var list = newVal.SubDeviceParameterList;
                                    if (list) {
                                        self.object().SubDeviceParameterList([]);
                                        ko.utils.arrayForEach(list, function (item) {
                                            self.object().SubDeviceParameterList.push(
                                            {
                                                SubdeviceParameterType: item.SubdeviceParameterType,
                                                SubdeviceParameterFriendlyName: ko.observable(item.SubdeviceParameterFriendlyName),
                                                SubdeviceParameterValue: ko.observable(item.SubdeviceParameterValue),
                                            });
                                        });
                                        self.object().SubDeviceParameterList.valueHasMutated();
                                    }
                                }
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                                });
                            }
                        });
                        //
                        return retD.promise();
                    };
                }
                //
                self.validate = function () {
                    if (!self.object().ProductCatalogModelID()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('AssetRegistration_ModelMustBeSet'));
                        });
                        return false;
                    }
                    //
                    for (var i = 0; i < self.tabList().length; i++)
                        if (self.tabList()[i].validate() == false)
                            return false;
                    //
                    if (self.tab_parameters.validate() == false)
                        return false;
                    //
                    return true;
                };
                //
                self.dispose = function () {
                    self.object().dispose();
                    //
                    self.object_handle.dispose();
                    self.tabActive_handle.dispose();
                    self.tabList().forEach(function (tab) {
                        tab.dispose();
                    });
                    self.tab_parameters.dispose();
                };
                //
                self.IsIdentifiersContainerVisible = ko.observable(true);
                self.ToggleIdentifiersContainer = function () {
                    self.IsIdentifiersContainerVisible(!self.IsIdentifiersContainerVisible());
                };
                //
                self.IsLocationContainerVisible = ko.observable(true);
                self.ToggleLocationContainer = function () {
                    self.IsLocationContainerVisible(!self.IsLocationContainerVisible());
                };
                //
                self.IsUtilizerContainerVisible = ko.observable(true);
                self.ToggleUtilizerContainer = function () {
                    self.IsUtilizerContainerVisible(!self.IsUtilizerContainerVisible());
                };
                //
                self.IsClassifierContainerVisible = ko.observable(true);
                self.ToggleClassifierContainer = function () {
                    self.IsClassifierContainerVisible(!self.IsClassifierContainerVisible());
                };
                //
                self.IsCharacteristicsContainerVisible = ko.observable(true);
                self.ToggleCharacteristicsContainer = function () {
                    self.IsCharacteristicsContainerVisible(!self.IsCharacteristicsContainerVisible());
                };
                //
                self.IsNoteVisible = ko.observable(true);
                self.ToggleNoteContainer = function () {
                    self.IsNoteVisible(!self.IsNoteVisible());
                };
                //
                {//rendering
                    self.sizeChanged = function () {
                        var $content = $('#' + self.frm.GetRegionID()).find('.content');
                        $content.find('.tabActive').css('height',
                            Math.max(
                                $content.innerHeight() -
                                $content.find('.subjectRow').outerHeight(true) -
                                parseInt($content.find('.tabActive').css('margin-top')) - 2 * parseInt($content.find('.tabActive').css('margin-bottom')),
                                0) + 'px');
                    };
                    self.afterRender = function (editor, elements) {
                        self.frm.SizeChanged();
                    };
                }
                //
                {//initialization         
                    self.object(new m_objects.assetRegistration());//fill object
                    self.selectTabClick(self.tabList()[0]);//init general tab
                }
            },

            ShowDialog: function (classID, parentObject, isSpinnerActive) {//parentObject - если добавляем адаптер или периферию в оконечное или сетевое оборудование
                var operationID;
                var className = '';
                if (classID == 5) {
                    operationID = 25;//OPERATION_ADD_NETWORKDEVICE
                    className = getTextResource('NetworkDevice');
                }
                else if (classID == 6) {
                    operationID = 67;//OPERATION_ADD_TERMINALDEVICE
                    className = getTextResource('TerminalDevice');
                }
                else if (classID == 33) {
                    operationID = 84;//OPERATION_ADD_ADAPTER
                    className = getTextResource('Adapter');
                }
                else if (classID == 34) {
                    operationID = 85;//OPERATION_ADD_PERIPHERAL
                    className = getTextResource('PeripheralDevice');
                }
                //
                $.when(operationIsGrantedD(operationID)).done(function (can_add) {
                    if (can_add == false) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('OperationError'));
                        });
                        return;
                    }
                    //
                    var modelD = $.Deferred();
                    var vm = new module.ViewModel();
                    vm.object().ClassID(classID);
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowAssetModelLink(
                         false, false,
                         function (newValues) {
                             if (!newValues || newValues.length == 0)
                                 return;
                             //
                             if (newValues.length == 1) {
                                 var object = vm.object();
                                 var model = newValues[0];
                                 object.productCatalogID(model.ID);
                                 object.ProductCatalogModelID(model.ID);
                                 object.productCatalogClassID(model.ClassID);
                                 //
                                 object.ProductCatalogCategoryName(model.ExternalIdentifier);
                                 object.ProductCatalogTypeName(model.TypeName);
                                 object.ManufacturerName(model.ManufacturerName);
                                 object.ProductCatalogModelName(model.Name);
                                 object.ProductCatalogTemplateID(model.ProductCatalogTemplateID);
                                 //
                                 var modelFullName = model.ProductCatalogCategoryFullName + ' \\ ' + model.TypeName + ' \\ ' + model.Name;
                                 object.productCatalogFullName(modelFullName);
                                 //
                                 object.LifeCycleStateName(model.LifeCycleStateName);
                                 //
                                 modelD.resolve();
                             }
                         },
                        true,
                        {
                            TemplateClassID: vm.object().ClassID(),
                            HasLifeCycle: false,
                        },
                        vm.GetModelFormName());
                    });
                    //
                    $.when(modelD).done(function () {
                        if (isSpinnerActive != true)
                            showSpinner();
                        //
                        var forceClose = false;//for question before close
                        var frm = undefined;
                        var bindElement = null;
                        var buttons = [];
                        var bAdd = {
                            text: getTextResource('Add'),
                            click: function () {
                                if (!vm.validate())
                                    return;
                                $.when(vm.object().register(true)).done(function (result) {
                                    if (result) {
                                        forceClose = true;
                                        frm.Close();
                                    }
                                });
                            }
                        }
                        var bCancel = {
                            text: getTextResource('Close'),
                            click: function () { frm.Close(); }
                        }
                        buttons.push(bAdd);
                        buttons.push(bCancel);
                        //
                        frm = new formControl.control(
                                'region_frmAssetRegistration',//form region prefix
                                'setting_frmAssetRegistration',//location and size setting
                                className + ' \\ ' + getTextResource('DeviceAdd'),  //caption
                                true,//isModal
                                true,//isDraggable
                                true,//isResizable
                                730, 490,//minSize
                                buttons,//form buttons
                                function () {
                                    ko.cleanNode(bindElement);
                                    vm.dispose();
                                },//afterClose function
                                'data-bind="template: {name: \'../UI/Forms/Asset/frmAssetRegistration\', afterRender: afterRender}"'//attributes of form region
                            );
                        if (!frm.Initialized)
                            return;//form with that region and settingsName was open
                        frm.ExtendSize(800, 700);//normal size
                        frm.SizeChanged = function () {
                            var width = frm.GetInnerWidth();
                            var height = frm.GetInnerHeight();
                            //
                            $('#' + frm.GetRegionID()).find('.frmAssetRegistration').css('width', width + 'px').css('height', height + 'px');
                            vm.sizeChanged();
                        };
                        frm.BeforeClose = function () {
                            if (vm.object().ID() != null)
                                return true;
                            //
                            if (forceClose)
                                return true;
                            //
                            require(['sweetAlert'], function () {
                                swal({
                                    title: getTextResource('FormClosing'),
                                    text: getTextResource('FormClosingQuestion'),
                                    showCancelButton: true,
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                function (value) {
                                    if (value == true) {
                                        forceClose = true;
                                        setTimeout(function () {
                                            frm.Close();
                                        }, 300);
                                    }
                                });
                            });
                            return false;
                        };
                        //
                        vm.frm = frm;
                        //
                        bindElement = document.getElementById(frm.GetRegionID());
                        ko.applyBindings(vm, bindElement);
                        //
                        $.when(frm.Show()).done(function (frmD) {
                            if (parentObject) {
                                $.when(vm.object().load(classID, parentObject().ID(), parentObject().ClassID())).done(function (frmD) {
                                    vm.InitializeSubdeviceParameterList();
                                    vm.tab_parameters.init(vm.object());
                                    hideSpinner();
                                });
                            }
                            else
                                hideSpinner();
                        });
                    });
                });
            },

        };
        return module;
    });