define(['knockout', 'jquery', 'ajax', 'formControl', './Contract',
    './frmContract_agreementListTab', './frmContract_assetMaintenanceListTab',
    './frmContract_attachmentsTab', './frmContract_contactPersonListTab',
    './frmContract_generalTab', './frmContract_historyTab',
    './frmContract_licenceMaintenanceListTab', './frmContract_parameterListTab',
    './frmContract_licenceListTab',
    'usualForms', 'dateTimeControl', 'jqueryStepper'],
    function (ko, $, ajax, formControl, m_objects,
        tab_agreements, tab_assetMaintenences,
        tab_attachments, tab_contacts,
        tab_general, tab_history,
        tab_licenceMaintenances, tab_parameters,
        tab_licences,
    fhModule, dtLib) {
        var module = {
            ViewModel: function () {
                var self = this;
                //                
                self.CanEdit = ko.observable(true);//for edit settings
                self.CanUpdate = ko.observable(true);//for operationIsGrantedD(283) (update Contract)
                //
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    self.tab_parameters.init();
                    //
                    self.tabList().forEach(function (tab) {
                        tab.init(newObject);
                    });
                    if (self.tabActive())
                        self.tabActive().load();//reload active tab
                });
                self.captionText = ko.pureComputed(function () {
                    var obj = self.object();
                    var retval = getTextResource('Contract');
                    if (obj != null) {
                        retval += ' № ' + obj.Number() + ' ' + getTextResource('Contract_createdAt') + ' ' + obj.dateCreatedString();
                        if (obj.ExternalNumber().length > 0)
                            retval += ' ' + getTextResource('Contract_regNumber') + ' ' + obj.ExternalNumber();
                        if (obj.UtcDateRegistered())
                            retval += ' ' + getTextResource('Contract_registeredAt') + ' ' + obj.UtcDateRegistered();
                    }
                    //
                    return retval;
                });
                //
                self.raiseObjectModified = function () {
                    var object = self.object();
                    $(document).trigger('local_objectUpdated', [115, object.ID(), null]);//serviceContract
                };
                //
                {//tabs
                    {//only for parameters
                        self.tab_parameters = new tab_parameters.Tab(self);
                        self.objectClassID = 115;//OBJ_ServiceContract
                        self.$region = null;//set in showDialog
                    }
                    //
                 
                    //
                    $.when(operationIsGrantedD(880), operationIsGrantedD(881)).done(function (can_add_m, can_create_m) {
                        if (can_add_m || can_create_m)
                            self.tabList = ko.observableArray(
                                [
                                    new tab_general.Tab(self),
                                    new tab_contacts.Tab(self),
                                    new tab_assetMaintenences.Tab(self),
                                    new tab_licenceMaintenances.Tab(self),
                                    new tab_agreements.Tab(self),
                                    new tab_licences.Tab(self),
                                    new tab_history.Tab(self),
                                    new tab_attachments.Tab(self)
                                ]);
                        else
                            self.tabList = ko.observableArray(
                                [
                                    new tab_general.Tab(self),
                                    new tab_contacts.Tab(self),
                                    new tab_agreements.Tab(self),
                                    new tab_licences.Tab(self),
                                    new tab_history.Tab(self),
                                    new tab_attachments.Tab(self)
                                ]);
                    });
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
                                fieldName: 'Description',
                                fieldFriendlyName: getTextResource('Contract_Description'),
                                oldValue: object.Description(),
                                allowNull: true,
                                maxLength: 500,
                                onSave: function (newText) {
                                    object.Description(newText);
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
                        tab.dispose();
                    });
                    self.tab_parameters.dispose();
                };
                //
                {//rendering
                    self.sizeChanged = function () {
                        var width = self.frm.GetInnerWidth();
                        var height = self.frm.GetInnerHeight();
                        $('#' + self.frm.GetRegionID()).find('.frmContract').css('width', width + 'px').css('height', height + 'px');
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
                    self.object(new m_objects.contract());//fill object
                    self.selectTabClick(self.tabList()[0]);//init general tab
                }
            },

            ShowDialog: function (id, isSpinnerActive) {
                $.when(operationIsGrantedD(211), operationIsGrantedD(283)).done(function (can_properties, can_update) {
                    //OPERATION_PROPERTIES_SERVICECONTRACT = 211
                    //OPERATION_ADD_SERVICECONTRACT = 212
                    //OPERATION_UPDATE_SERVICECONTRACT = 283
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
                    var vm = new module.ViewModel();
                    var bindElement = null;
                    var buttons = [];
                    frm = new formControl.control(
                            'region_frmServiceContract',//form region prefix
                            'setting_frmServiceContract',//location and size setting
                            getTextResource('Contract'),//caption
                            true,//isModal
                            true,//isDraggable
                            true,//isResizable
                            730, 490,//minSize
                            buttons,//form buttons
                            function () {
                                ko.cleanNode(bindElement);
                                vm.dispose();
                            },//afterClose function
                            'data-bind="template: {name: \'../UI/Forms/Asset/Contracts/frmContract\', afterRender: afterRender}"'//attributes of form region
                        );
                    if (!frm.Initialized) {//form with that region and settingsName was open
                        hideSpinner();
                        //
                        var url = window.location.protocol + '//' + window.location.host + location.pathname + '?serviceContractID=' + id;
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
                    //
                    frm.ExtendSize(800, 700);//normal size
                    //
                    vm.frm = frm;
                    vm.$region = $('#' + frm.GetRegionID()).find('.frmContract');
                    vm.CanUpdate(can_update);
                    //
                    bindElement = document.getElementById(frm.GetRegionID());
                    ko.applyBindings(vm, bindElement);
                    //
                    $.when(frm.Show(), vm.object().load(id, vm)).done(function (frmD, loadD) {                      
                        hideSpinner();
                        vm.CanEdit(!vm.object().HasAgreement());
                        if (!ko.components.isRegistered('contractFormCaptionComponent'))
                            ko.components.register('contractFormCaptionComponent', {
                                template: '<span data-bind="text: $str"/>'
                            });
                        frm.BindCaption(vm, "component: {name: 'contractFormCaptionComponent', params: { $str: captionText} }");
                    });
                });
            },
        };
        return module;
    });