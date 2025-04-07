define(['knockout', 'jquery', 'ajax', 'formControl', './ContractRegistration',
    './frmContractRegistration_generalTab', './frmContractRegistration_parameterListTab', './frmContractRegistration_attachmentsTab',
    'usualForms'],
    function (ko, $, ajax, formControl, m_objects,
    tab_general, tab_parameters, tab_attachments,
    fhModule) {
        var module = {
            ViewModel: function () {
                var self = this;
                //                
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    self.tab_parameters.init(newObject);
                    self.tabList().forEach(function (tab) {
                        tab.init(newObject);
                    });
                    if (self.tabActive())
                        self.tabActive().load();//reload active tab
                });
                //
                {//tabs
                    {//only for parameters
                        self.tab_parameters = new tab_parameters.Tab(self);
                        self.formClassID = 115;//OBJ_ServiceContract
                    }
                    //
                    self.tabList = ko.observableArray(
                        [
                            new tab_general.Tab(self),
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
                    self.editDescription = function () {
                        showSpinner();
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            fieldName: 'Contract.Description',
                            fieldFriendlyName: getTextResource('Contract_Description'),
                            oldValue: self.object().description(),
                            onSave: function (newText) {
                                self.object().description(newText);
                            },
                            nosave: true
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    };
                }
                //
                self.validate = function () {
                    if (self.object().description().trim().length == 0) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ContractRegistration_SubjectPrompt'));
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
                        $('#' + self.frm.GetRegionID()).find('.content .description').focus();
                    };
                }
                //
                {//initialization         
                    self.object(new m_objects.contractRegistration());//fill object
                    self.selectTabClick(self.tabList()[0]);//init general tab
                }
            },

            ShowDialog: function (isSpinnerActive) {
                $.when(operationIsGrantedD(212)).done(function (can_add) {
                    //OPERATION_ADD_SERVICECONTRACT = 212
                    if (can_add == false) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('OperationError'));
                        });
                        return;
                    }
                    //
                    if (isSpinnerActive != true)
                        showSpinner();
                    //
                    var forceClose = false;//for question before close
                    var frm = undefined;
                    var vm = new module.ViewModel();
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
                        text: getTextResource('ButtonCancel'),
                        click: function () { frm.Close(); }
                    }
                    buttons.push(bAdd);
                    buttons.push(bCancel);
                    //
                    frm = new formControl.control(
                            'region_frmServiceContractRegistration',//form region prefix
                            'setting_frmServiceContractRegistration',//location and size setting
                            getTextResource('ContractRegistration'),//caption
                            true,//isModal
                            true,//isDraggable
                            true,//isResizable
                            730, 490,//minSize
                            buttons,//form buttons
                            function () {
                                ko.cleanNode(bindElement);
                                vm.dispose();
                            },//afterClose function
                            'data-bind="template: {name: \'../UI/Forms/Asset/Contracts/frmContractRegistration\', afterRender: afterRender}"'//attributes of form region
                        );
                    if (!frm.Initialized)
                        return;//form with that region and settingsName was open
                    frm.ExtendSize(800, 700);//normal size
                    frm.SizeChanged = function () {
                        var width = frm.GetInnerWidth();
                        var height = frm.GetInnerHeight();
                        //
                        $('#' + frm.GetRegionID()).find('.frmContractRegistration').css('width', width + 'px').css('height', height + 'px');
                        vm.sizeChanged();
                    };                    
                    frm.BeforeClose = function () {
                        if (vm.object().id() != null)
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
                        hideSpinner();
                    });
                });
            },

        };
        return module;
    });