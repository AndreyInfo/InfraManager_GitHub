define(['knockout', 'jquery', 'ajax', 'formControl', './Agreement',
    './frmAgreement_generalTab', './frmAgreement_assetMaintenanceListTab',
    './frmAgreement_licenceMaintenanceListTab',
    './frmAgreement_licenceListTab',
    'usualForms', 'dateTimeControl', 'jqueryStepper'],
    function (ko, $, ajax, formControl, m_objects,
        tab_general, tab_assetMaintenences,
        tab_licenceMaintenances,
        tab_licences,
    fhModule, dtLib) {
        var module = {
            ViewModel: function (id, serviceContractID) {
                var self = this;
                //                
                self.addMode = (id == null);
                self.parentsContractId = serviceContractID;
                self.canEdit = ko.observable(true);//TODO bad name
                //
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    self.tabList().forEach(function (tab) {
                        tab.init(newObject);
                    });
                    if (self.tabActive())
                        self.tabActive().load();//reload active tab
                });
                self.captionText = ko.pureComputed(function () {
                    var obj = self.object();
                    if (obj.id() != null) {
                        var retval = getTextResource('ContractAgreement');
                        if (obj != null && obj.contract()) {
                            retval += ' № ' + obj.number() + ' ' + getTextResource('Contract_genetive') + ' № ' + obj.contract().Number() + ', ' + getTextResource('Contract_createdAt') + ' ' + obj.contract().dateCreatedString();
                        }
                    }
                    else {
                        var retval = getTextResource('ContractAgreementRegistration');
                        if (obj != null && obj.contract()) {
                            retval += ' ' + obj.contract().Number() + ', ' + getTextResource('Contract_createdAt') + ' ' + obj.contract().dateCreatedString();
                        }                        
                    }
                    //
                    return retval;
                });
                //
                self.raiseObjectModified = function () {
                    var object = self.object();
                    if(object.id())
                    $(document).trigger('local_objectUpdated', [386, object.id(), object.contract().ID()]);//OBJ_ServiceContractAgreement = 386
                };
                //
                {//tabs
                    if (self.addMode)
                        self.tabList = ko.observableArray(
                            [
                                new tab_general.Tab(self)
                            ]);
                    else {
                        $.when(operationIsGrantedD(880), operationIsGrantedD(881)).done(function (can_add_m, can_create_m) {
                            if (can_add_m || can_create_m)
                        self.tabList = ko.observableArray(
                            [
                                new tab_general.Tab(self),
                                new tab_assetMaintenences.Tab(self),
                                new tab_licenceMaintenances.Tab(self),
                                new tab_licences.Tab(self)
                            ]);
                            else
                                self.tabList = ko.observableArray(
                                    [
                                        new tab_general.Tab(self),
                                        new tab_licences.Tab(self)
                                    ]);
                       
                        });
                    }
                    
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
                    return true;
                };
                //
                self.dispose = function () {
                    self.object().ajaxControl.Abort();
                    //
                    self.object_handle.dispose();
                    self.captionText.dispose();
                    self.tabActive_handle.dispose();
                    self.tabList().forEach(function (tab) {
                        tab.dispose();
                    });
                };
                //
                {//editors
                    self.editDescription = function () {
                        if (!self.canEdit())
                            return;
                        //
                        showSpinner();
                        var object = self.object();
                        var noSave = object.id() ? false : true;
                        require(['usualForms'], function (fhModule) {
                            var fh = new fhModule.formHelper(true);
                            var options = {
                                ID: object.id(),
                                objClassID: object.classID,
                                fieldName: 'Description',
                                fieldFriendlyName: getTextResource('Contract_Description'),
                                oldValue: object.description(),
                                allowNull: true,
                                maxLength: 1000,
                                onSave: function (newText) {
                                    object.description(newText);
                                    self.raiseObjectModified();
                                },
                                nosave: noSave
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                        });
                    };
                }
                //
                {//rendering
                    self.sizeChanged = function () {
                        var width = self.frm.GetInnerWidth();
                        var height = self.frm.GetInnerHeight();
                        $('#' + self.frm.GetRegionID()).find('.frmAgreement').css('width', width + 'px').css('height', height + 'px');
                        //
                        var $content = $('#' + self.frm.GetRegionID()).find('.content');
                        $content.find('.tabActive').css('height',
                            Math.max(
                                $content.innerHeight() -
                                $content.find('.stateRow').outerHeight(true) -
                                $content.find('.identificationRow').outerHeight(true) -
                                parseInt($content.find('.tabActive').css('margin-top')) - 2 * parseInt($content.find('.tabActive').css('margin-bottom')),
                                0) + 'px');
                    };
                    self.afterRender = function () {
                        self.frm.SizeChanged();
                    };
                }
                //
                {//initialization         
                    self.object(new m_objects.agreement());//fill object
                    self.selectTabClick(self.tabList()[0]);//init general tab
                }
            },

            ShowDialog: function (id, serviceContractID, isSpinnerActive) {
                $.when(operationIsGrantedD(872), operationIsGrantedD(875), operationIsGrantedD(873)).done(function (can_properties, can_update, can_add) {
                    //OPERATION_ServiceContractAgreement_Properties = 872,
                    //OPERATION_ServiceContractAgreement_Add = 873,
                    //OPERATION_ServiceContractAgreement_Update = 875
                    if (can_properties == false && id || !id && can_add == false) {
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
                    var vm = new module.ViewModel(id, serviceContractID);
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
                    var bAddAndOpen = {
                        text: getTextResource('SaveAndOpen'),
                        click: function () {
                            if (!vm.validate())
                                return;
                            $.when(vm.object().register(true), operationIsGrantedD(880), operationIsGrantedD(881)).done(function (result, can_open_maintenance, can_add_maintenance) {
                                if (result) {
                                    vm.object().id(result.ContractAgreementID);
                                    //
                                    if (can_open_maintenance || can_add_maintenance) 
                                        vm.tabList.push(
                                            new tab_assetMaintenences.Tab(vm),
                                            new tab_licenceMaintenances.Tab(vm),
                                            new tab_licences.Tab(vm)
                                        );                                    
                                        else
                                        vm.tabList.push(
                                            new tab_licences.Tab(vm)
                                        );
                                    vm.object().load(result.ContractAgreementID, null, vm);
                                    buttons = [];
                                    frm.UpdateButtons(buttons);
                                }
                            });
                        }
                    }
                    var bCancel = {
                        text: getTextResource('Close'),
                        click: function () { frm.Close(); }
                    }
                    if (!id) {//add mode
                        buttons.push(bAdd);
                        if (can_properties)
                        buttons.push(bAddAndOpen);
                        buttons.push(bCancel);
                    }
                    //
                    frm = new formControl.control(
                            'region_frmServiceContractAgreement',//form region prefix
                            'setting_frmServiceContractAgreement',//location and size setting
                            getTextResource('ContractAgreement'),//caption
                            true,//isModal
                            true,//isDraggable
                            true,//isResizable
                            730, 490,//minSize
                            buttons,//form buttons
                            function () {
                                ko.cleanNode(bindElement);
                                vm.dispose();
                            },//afterClose function
                            'data-bind="template: {name: \'../UI/Forms/Asset/Contracts/frmAgreement\', afterRender: afterRender}"'//attributes of form region
                        );
                    if (!frm.Initialized) {//form with that region and settingsName was open
                        hideSpinner();
                        //
                        var url = window.location.protocol + '//' + window.location.host + location.pathname;
                        if (id)
                            url += '?contractAgreementID=' + id;
                        else
                            url += '?agreement_serviceContractID=' + serviceContractID;
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
                    var oldSizeChanged = frm.SizeChanged;
                    frm.SizeChanged = function () {
                        oldSizeChanged();
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
                    frm.ExtendSize(800, 700);//normal size
                    //
                    vm.frm = frm;
                    vm.$region = $('#' + frm.GetRegionID()).find('.frmAgreement');
                    vm.canEdit(can_update);      
                                        //
                    bindElement = document.getElementById(frm.GetRegionID());
                    ko.applyBindings(vm, bindElement);
                    //
                    $.when(frm.Show(), vm.object().load(id, serviceContractID, vm)).done(function (frmD, loadD) {
                        vm.canEdit(!vm.object().IsApplied() && can_update);
                        hideSpinner();
                        if (!ko.components.isRegistered('contractAgreementFormCaptionComponent'))
                            ko.components.register('contractAgreementFormCaptionComponent', {
                                template: '<span data-bind="text: $str"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'contractAgreementFormCaptionComponent', params: { $str: captionText} }");
                    });
                });
            },
        };
        return module;
    });