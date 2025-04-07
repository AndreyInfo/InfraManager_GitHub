define(['knockout', 'jquery', 'ajax', 'formControl', './ELPTask',
    './frmELPTask_generalTab',
    './frmELPTask_historyTab'
],
    function (ko, $, ajax, formControl, m_objects,
        tab_general,
        tab_history ) {
        var module = {
            ViewModel: function ($region, isAddNewELPTask) {
                var self = this;

                self.$region = $region;
                self.asset = ko.observable(null);
                self.CanEdit = ko.observable(true);

                // базовый url
                self.baseUrl = '/api/elpsettings';

                // добавляется новая схема лицензирования
                self.isAddNewELPTask = ko.observable(isAddNewELPTask);

                //Объект связи
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
                        retval += getTextResource('ELPTask') + (obj.Name()!='' ? ': ' + obj.Name() : '');
                    }
                    return retval;
                });
                //

                self.raiseObjectModified = function () {
                    var object = self.object();
                    $(document).trigger('local_objectUpdated', [820, object.ID(), null]); // ELPTask
                };

                self.raiseObjectInserted = function () {
                    var object = self.object();
                    $(document).trigger('local_objectInserted', [820, object.ID(), null]); // ELPTask
                };

                //tabs
                {
                    //only for parameters
                    {
                        
                        self.objectClassID = 820; //OBJ_Licence_Scheme
                    }

                    let arrayTabs = [];

                    arrayTabs =
                        [
                            new tab_general.Tab(self),                                                      // 0                        
                        ];


                    if (!self.isAddNewELPTask()) {
                        arrayTabs.push(new tab_history.Tab(self));                                          // 1
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
                                fieldFriendlyName: getTextResource('ELPTask_Form_Name'),
                                oldValue: object.Name(),
                                allowNull: true,
                                maxLength: 500,
                                onSave: function (newText) {
                                    object.Name(newText);
                                    $.when(self.object().AddOrUpdate(self.baseUrl)).done(function (result) {
                                        if (result) {
                                            self.raiseObjectModified();
                                        }
                                        else {
                                            return;
                                        }
                                    });
                                },
                                nosave: true
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                        });
                    };
                }
                self.DisplayVendor = function () {
                    return self.object().ELPVendorAny() == '0';
                };

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
                    //self.tab_parameters.dispose();
                };
                //rendering
                {
                    self.sizeChanged = function () {
                        var width = self.frm.GetInnerWidth();
                        var height = self.frm.GetInnerHeight();
                        $('#' + self.frm.GetRegionID()).find('.frmELPTask').css('width', width + 'px').css('height', height + 'px');
                        //
                        var $content = $('#' + self.frm.GetRegionID()).find('.content');
                        $content.find('.tabActive').css('height',
                            Math.max(
                                $content.innerHeight()
                                /*- $content.find('.b-requestDetail-menu').outerHeight(true)*/
                                /*- $content.find('.stateRow').outerHeight(true)*/
                                //- $content.find('.identificationRow').outerHeight(true)
                                //- 2 * parseInt($content.find('.tabActive').css('margin-top'))
                                //- 2 * parseInt($content.find('.tabActive').css('margin-bottom'))
                                - 10,
                                0) + 'px');
                    };
                    self.afterRender = function () {
                        self.frm.SizeChanged();
                    };
                }

                //initialization (пустой объект, выделяем первый таб)
                {
                    self.object(new m_objects.ELPTask());//fill object

                    self.selectTabClick(self.tabList()[0]);//init general tab                    

                    self.IsReadOnly = ko.observable(false);
                   

                    self.reload = function (id, classID) {
                        var retD = $.Deferred();
                        var refreshObject = new m_objects.ELPTask();
                        if (self.object()) {
                            $.when(refreshObject.load(id)).done(function (loadD) {
                                self.object(refreshObject);
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
            // isAddNewELPTask - режим создания новой схемы обсуживания (влияет на наличие кнопок сохранение все формы и сохранение отдельных полей)
            // isSpinnerActive - признак, что спиннер был включен
            ShowDialog: function (id, isAddNewELPTask, isSpinnerActive) {
                $.when( // OPERATION_ELPTask_Create = 820002;
                    operationIsGrantedD(820002),
                    // OPERATION_ELPTask_Edit = 820003;
                    operationIsGrantedD(820003)                    
                ).done(function (can_create, can_edit) {

                    var can_edit = can_create || can_edit;
                    
                    if (isSpinnerActive != true)
                        showSpinner();
                    //
                    var frm = undefined;
                    var bindElement = null;

                    var buttons = [];                    

                    if (isAddNewELPTask) {

                        var bSaveClose = {
                            text: getTextResource('ELPTask_Form_AddButtonSaveClose'),
                            click: function () {
                                //if (!vm.validate())
                                 //   return;
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
                                        title: getTextResource('ELPTask_Form_FormClosingQuestion'),
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
                        'region_frmELPTask',//form region prefix
                        'setting_frmELPTask',//location and size setting
                        getTextResource('ELPTask_Form_Caption'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        613, 551,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                            vm.dispose();                           
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Settings/ELP/frmELPTask\', afterRender: afterRender}"'//attributes of form region
                    );
                    if (!frm.Initialized) {//form with that region and settingsName was open
                        hideSpinner();
                        //
                        var url = window.location.protocol + '//' + window.location.host + location.pathname + '?ELPTaskID=' + id;
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
                    var vm = new module.ViewModel($region, isAddNewELPTask);
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

                        if (isAddNewELPTask) {
                            vm.object().ID('');
                        }
                        //can_edit = false;

                        //инициализация списков на основной вкладке
                        vm.initGeneralTab();                       
                        vm.CanEdit(can_edit);
                        vm.object().CanEdit(can_edit);
                       

                        $.when(frm.Show()).done(function (frmD) {
                            if (!ko.components.isRegistered('ELPTaskFormCaptionComponent'))
                                ko.components.register('ELPTaskFormCaptionComponent', {
                                    template: '<span data-bind="text: $str"/>'
                                });
                            frm.BindCaption(vm, "component: {name: 'ELPTaskFormCaptionComponent', params: { $str: captionText} }");
                            hideSpinner();
                        });

                    });
                });
            }            
        };
        return module;
    });