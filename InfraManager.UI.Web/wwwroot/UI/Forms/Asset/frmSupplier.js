define(['knockout', 'jquery', 'ajax', 'formControl',
    './Supplier', './frmSupplier_generalTab', './frmSupplier_contactPersonListTab',
    'usualForms', 'dateTimeControl', 'jqueryStepper'], function (ko, $, ajax, formControl, m_object, tab_general, tab_contactPersons, fhModule, dtLib) {
        var module = {
            ViewModel: function (existsInDataBase) {
                var self = this;
                //            
                self.ajaxControl = new ajax.control();
                self.CanEdit = ko.observable(true);//TODO bad name
                //
                self.object = ko.observable(null);
                self.object_handle = self.object.subscribe(function (newObject) {
                    self.tabList().forEach(function (tab) {
                        tab.init(newObject);
                    });
                    if (self.tabActive())
                        self.tabActive().load();//reload active tab
                });
                //
                {//tabs
                    self.tabList = ko.observableArray(
                        [
                            new tab_general.Tab(self),
                            new tab_contactPersons.Tab(self, existsInDataBase),
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
                self.dispose = function () {
                    self.object().ajaxControl.Abort();
                    //
                    self.object_handle.dispose();
                    self.tabActive_handle.dispose();
                    self.tabList().forEach(function (tab) {
                        tab.dispose();
                    });
                };
                //
                {//rendering
                    self.sizeChanged = function () {
                        var $content = $('#' + self.frm.GetRegionID()).find('.content');
                        $content.find('.tabActive').css('height',
                            Math.max(
                                $content.innerHeight() -
                                parseInt($content.find('.tabActive').css('margin-top')) - 2 * parseInt($content.find('.tabActive').css('margin-bottom')),
                                0) + 'px');
                    };
                    self.afterRender = function (editor, elements) {
                        self.frm.SizeChanged();
                    };
                }
                //
                {//initialization         
                    self.object(new m_object.supplier());//fill object
                    self.selectTabClick(self.tabList()[0]);//init general tab
                }
                //
                self.AfterRender = function (editor, elements) {
                    var $frm = $('#' + self.frm.GetRegionID());
                };
                //
                self.Save = function () {
                    var retval = $.Deferred();
                    //
                    if (!self.object().Name()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('MustSetName'));
                        });
                        return;
                    }
                    //    
                    var supplier =
                        {
                            ID: self.object().ID(),
                            Name: self.object().Name(),
                            Phone: self.object().Phone(),
                            Email: self.object().Email(),
                            Address: self.object().Address(),
                            RegisteredAddress: self.object().RegisteredAddress(),
                            Notice: self.object().Notice(),
                            INN: self.object().INN(),
                            KPP: self.object().KPP(),
                            TypeList: self.object().TypeList()
                        };
                    //
                    self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/EditSupplier',
                        method: 'POST',
                        data: supplier
                    },
                    function (response) {
                        if (response.Response.Result === 0 && response.NewModel) {
                            var obj = response.NewModel;
                            //
                            retval.resolve(true);
                        }
                        else {
                            retval.resolve(false);
                            require(['sweetAlert'], function () {
                                swal(response.Response.Message, '', 'info');
                            });
                        }
                    });
                    //
                    return retval.promise();
                };
            },

            ShowDialog: function (id, isSpinnerActive) {
                $.when(operationIsGrantedD(217), operationIsGrantedD(285)).done(function (can_properties, can_update) {
                    //OPERATION_ADD_SUPPLIER = 218,
                    //OPERATION_UPDATE_SUPPLIER = 285, 
                    //OPERATION_PROPERTIES_SUPPLIER = 217,
                    if (can_properties == false) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('OperationError'));
                        });
                        hideSpinner();
                        return;
                    }
                    //
                    if (isSpinnerActive != true)
                        showSpinner();
                    //
                    var frm = undefined;
                    var vm = new module.ViewModel(id ? true : false);
                    var bindElement = null;
                    //
                    var buttons = [];
                    var bSave = {
                        text: getTextResource('ButtonSave'),
                        click: function () {
                            if (vm.CanEdit() == false) {
                                frm.Close();
                                return;
                            }
                            $.when(vm.Save()).done(function (result) {
                                if (result)
                                    frm.Close();
                            });
                        }
                    }
                    var bCancel = {
                        text: getTextResource('Close'),
                        click: function () { frm.Close(); }
                    }
                    if (can_update == true || !id)
                        buttons.push(bSave);
                    buttons.push(bCancel);
                    //
                    frm = new formControl.control(
                            'region_frmSupplier',//form region prefix
                            'setting_frmSupplier',//location and size setting
                            getTextResource('Supplier'),//caption
                            true,//isModal
                            true,//isDraggable
                            true,//isResizable
                            500, 500,//minSize
                            buttons,//form buttons
                            function () {
                                vm.dispose();
                                ko.cleanNode(bindElement);
                            },//afterClose function
                            'data-bind="template: {name: \'../UI/Forms/Asset/frmSupplier\', afterRender: AfterRender}"'//attributes of form region
                        );
                    if (!frm.Initialized)
                        return;//form with that region and settingsName was open
                    var oldSizeChanged = frm.SizeChanged;
                    frm.SizeChanged = function () {
                        oldSizeChanged();
                        //var width = frm.GetInnerWidth();
                        //var height = frm.GetInnerHeight();
                        ////
                        //$('#' + frm.GetRegionID()).find('.frmSupplier').css('width', width + 'px').css('height', height + 'px');
                        vm.sizeChanged();
                    };
                    vm.frm = frm;
                    frm.ExtendSize(650, 700);//normal size
                    vm.CanEdit(can_update);
                    //
                    bindElement = document.getElementById(frm.GetRegionID());
                    ko.applyBindings(vm, bindElement);
                    //
                    $.when(frm.Show(), vm.object().load(id)).done(function (frmD, loadD) {
                        hideSpinner();
                    });
                });
            },
        };
        return module;
    });
