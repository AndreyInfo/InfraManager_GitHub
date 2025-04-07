define(['knockout', 'jquery', 'ajax', 'formControl', './SoftwareInstallation',
    './frmSoftwareInstallation_generalTab',
    './frmSoftwareInstallation_licencesTab',
    './frmSoftwareInstallation_dependantsTab',
],
    function (ko, $, ajax, formControl, m_objects,
        tab_general,
        tab_licences,
        tab_depended,
    ) {
        var module = {
            ViewModel: function ($region, isAddNewSoftwareInstallation) {
                var self = this;

                self.$region = $region;
                self.asset = ko.observable(null);
                self.CanEdit = ko.observable(true);

                // базовый url
                self.baseUrl = 'SoftwareInstallations';

                // url сохранения изменений по объекту
                self.setFieldUrl = 'set-field';

                // добавляется новая схема лицензирования
                self.isAddNewSoftwareInstallation = ko.observable(isAddNewSoftwareInstallation);

                //Объект инсталляции
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    if (newObject == null || !newObject.ID()) {
                        return;
                    }

                    self.tabList().forEach(function (tab) {
                        tab.Initialize(newObject);
                    });
                    if (self.tabActive()) {
                        self.tabActive().load();//reload active tab
                    }
                });
                
                //заголовок
                self.captionText = ko.pureComputed(function () {
                    var obj = self.object();
                    var retval = '';
                    if (obj != null) {
                        retval += getTextResource('SoftwareInstallation') + ': ' + obj.Name();
                    }
                    return retval;
                });
                //

                self.raiseObjectModified = function () {
                    var object = self.object();
                    $(document).trigger('local_objectUpdated', [71, object.ID(), null]); // SoftwareInstallation
                };

                self.raiseObjectInserted = function () {
                    var object = self.object();
                    $(document).trigger('local_objectInserted', [71, object.ID(), null]); // SoftwareInstallation
                };

                //tabs
                {
                    //only for parameters
                    {
                        
                        self.objectClassID = 71; //OBJ_INSTALLATION
                    }

                    let arrayTabs = [];

                    arrayTabs =
                        [
                            new tab_general.Tab(self),                                                      // 0                        
                        ];


                    if (!self.isAddNewSoftwareInstallation()) {
                        arrayTabs.push(new tab_licences.Tab(self));                                          // 1
                        arrayTabs.push(new tab_depended.Tab(self));                                          // 1
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
                                fieldName: 'SoftwareInstallationName',
                                fieldFriendlyName: getTextResource('SoftwareInstallation_Form_Name'),
                                oldValue: object.Name(),
                                allowNull: true,
                                maxLength: 500,
                                onSave: function (newText) {
                                    object.Name(newText);
                                    self.raiseObjectModified();
                                },
                                nosave: self.isAddNewSoftwareInstallation(),
                                urlSetField: self.baseUrl + '/' + self.setFieldUrl
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
                };
                //rendering
                {
                    self.sizeChanged = function () {
                        var width = self.frm.GetInnerWidth();
                        var height = self.frm.GetInnerHeight();
                        $('#' + self.frm.GetRegionID()).find('.frmSoftwareInstallation').css('width', width + 'px').css('height', height + 'px');
                        //
                        var $content = $('#' + self.frm.GetRegionID()).find('.content');
                        $content.find('.tabActive').css('height',
                            Math.max(
                                $content.innerHeight()
                                - 10,
                                0) + 'px');
                    };
                    self.afterRender = function () {
                        self.frm.SizeChanged();
                    };
                }
                // Inquery
                self.DateInquiry = function () {
                    if (self.object()?.LastDiscoverDate())
                        return 'Дата последнего опроса: ' + self.object().LastDiscoverDate();
                    return 'Дата последнего опроса: ';
                };

                //initialization (пустой объект, выделяем первый таб)
                {
                    self.object(new m_objects.SoftwareInstallation());//fill object

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
                        var refreshObject = new m_objects.SoftwareInstallation();
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

                }                
            },

            // id - идентификатор схемы обсуживания, для подгрузки данных в форму
            // isAddNewSoftwareInstallation - режим создания новой инсталляции (влияет на наличие кнопок сохранение все формы и сохранение отдельных полей)
            // isSpinnerActive - признак, что спиннер был включен
            ShowDialog: function (id, isAddNewSoftwareInstallation, isSpinnerActive) {
                $.when( operationIsGrantedD(86),    //  SoftWare.Add
                    operationIsGrantedD(245)        //  SoftWare.Update
                ).done(function (can_create, can_edit) {

                    var can_edit = can_create || can_edit;
                    
                    if (isSpinnerActive != true)
                        showSpinner();
                    //
                    var frm = undefined;
                    var bindElement = null;

                    var buttons = [];                    

                    if (isAddNewSoftwareInstallation) {

                        var bSaveClose = {
                            text: getTextResource('SoftwareInstallation_Form_AddButtonSaveClose'),
                            click: function () {
                                if (!vm.object().validate())
                                    return;
                                $.when(vm.object().AddOrUpdate(vm.baseUrl)).done(function (result) {
                                    if (result) {
                                        vm.raiseObjectInserted();
                                        forceClose = true;
                                        frm.Close();
                                    }
                                    else {
                                        return;
                                    }
                                });                                
                            }
                        }

                        var bCancel = {
                            text: getTextResource('Close'),
                            click: function () {
                                require(['sweetAlert'], function () {
                                    swal({
                                        title: getTextResource('SoftwareInstallation_Form_FormClosingQuestion'),
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
                                                }, 300);//TODO? close event of swal
                                            }
                                        });
                                });
                            }
                        }

                        buttons.push(bSaveClose);
                        buttons.push(bCancel);
                    }             

                    frm = new formControl.control(
                        'region_frmSoftwareInstallation',//form region prefix
                        'setting_frmSoftwareInstallation',//location and size setting
                        getTextResource('SoftwareInstallation_Form_Caption'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        613, 551,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                            vm.dispose();                           
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareInstallation/frmSoftwareInstallation\', afterRender: afterRender}"'//attributes of form region
                    );
                    if (!frm.Initialized) {//form with that region and settingsName was open
                        hideSpinner();
                        //
                        var url = window.location.protocol + '//' + window.location.host + location.pathname + '?SoftwareInstallationID=' + id;
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
                    var vm = new module.ViewModel($region, isAddNewSoftwareInstallation);
                    vm.frm = frm;
                    var oldSizeChanged = frm.SizeChanged;
                    frm.SizeChanged = function () {
                        oldSizeChanged();
                        vm.sizeChanged();
                    };
                    //
                    frm.ExtendSize(613, 551);//normal size
                    //
                    bindElement = document.getElementById(frm.GetRegionID());
                    ko.applyBindings(vm, bindElement);

                    //
                    $.when(vm.object().load(id, vm.baseUrl), can_edit).done(function (loadD, can_edit) {

                        if (isAddNewSoftwareInstallation) {
                            vm.object().ID('');
                        }
                        //can_edit = false;

                        //инициализация списков на основной вкладке
                        vm.initGeneralTab();                       
                        vm.CanEdit(can_edit);
                        vm.object().CanEdit(can_edit);
                       

                        $.when(frm.Show()).done(function (frmD) {
                            if (!ko.components.isRegistered('SoftwareInstallationFormCaptionComponent'))
                                ko.components.register('SoftwareInstallationFormCaptionComponent', {
                                    template: '<span data-bind="text: $str"/>'
                                });
                            frm.BindCaption(vm, "component: {name: 'SoftwareInstallationFormCaptionComponent', params: { $str: captionText} }");
                            hideSpinner();
                        });
                        vm.LoadAssetOperationControl();
                    });
                });
            }            
        };
        return module;
    });