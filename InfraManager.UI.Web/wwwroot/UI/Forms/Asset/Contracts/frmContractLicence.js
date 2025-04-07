define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'dateTimeControl', './ContractLicence', 'jqueryStepper'], function (ko, $, ajax, formControl, fhModule, dtLib, lic_object) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //
            self.$region = null;
            self.frm = null;
            //
            self.CanEdit = ko.observable(true);
            self.SoftwareModelName = ko.observable(null);
            self.object = ko.observable(new lic_object.contractLicence());
            //
            self.IsContractAgreement = ko.observable(false);
            //
            {//software model
                self.SoftwareModelID = self.object().SoftwareModelID;
                self.SoftwareModelName = self.object().SoftwareModelName;                
                //
                self.softwareModelSearcher = null;
                self.softwareModelSearcherD = $.Deferred();
                self.InitializeSoftwareModelSearcher = function () {
                    var fh = new fhModule.formHelper();
                    var softwareModelD = fh.SetTextSearcherToField(
                        self.$region.find('.contractLicence-softwareModel'),
                        'SoftwareModelSearcher',
                        null,
                        ['false'],//ShowNonCommercial
                        function (objectInfo) {//select
                            //self.LocationClassID = objectInfo.ClassID;                            
                            self.SoftwareModelID(objectInfo.ID);
                            self.SoftwareModelName(objectInfo.FullName);
                        },
                        function () {//reset
                            self.SoftwareModelID(null);
                            self.SoftwareModelName('');
                        });
                    $.when(softwareModelD).done(function (ctrl) {
                        self.softwareModelSearcher = ctrl;
                        self.softwareModelSearcher.ReadOnly(!self.CanEdit());
                        self.softwareModelSearcherD.resolve(ctrl);
                        ctrl.CurrentUserID = null;
                    });
                };
            }
            //
            {//productCatalogType / softwareLicenceModel                
                self.TypeOrModelTempName = ko.observable(null);
                self.TypeOrModelName = ko.pureComputed({
                    read: function () {
                        if (self.TypeOrModelTempName() != null)
                            return self.TypeOrModelTempName();
                        else
                            return self.object().SoftwareLicenceModelID() != null ? self.object().SoftwareLicenceModelName() : self.object().ProductCatalogTypeName();
                    },
                    write: function (value) {
                        self.TypeOrModelTempName(value);
                    }
                });
                //      
                self.ProductCatalogClass = ko.observable(null);
                self.ProductCatalogClass.subscribe(function () {
                    self.VisibleLimitInDay(self.ProductCatalogClass() == 186 || self.ProductCatalogClass() == 187);
                    var LimitInDays = self.object().LimitInDays();
                    self.object().LimitInDays(LimitInDays != 0 ? LimitInDays : 0);
                });
                self.VisibleLimitInDay = ko.observable(false);
                self.GetProductClass = function (TypeID) {
                    if (TypeID == null) {
                        self.ProductCatalogClass(null);
                        return;
                    }
                    var objectID = {
                        ID: TypeID
                    }
                    self.ajaxControl.Ajax(null,
                        {
                            url: '/assetApi/GetProductCatalogClass',
                            method: 'GET',
                            data: objectID
                        },
                        function (response) {
                            if (response.Result === 0) {
                                var id = response.ProductCatalogClass;
                                //
                                self.ProductCatalogClass(id);
                                hideSpinner();
                            }
                            else {
                                hideSpinner();
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContractLicence.js, GetProductClass]', 'error');
                                });
                            }
                        });
                    //
                };
                self.SetTypeOrModel = function (objectInfo) {
                    var obj = self.object();
                    if (objectInfo != null && objectInfo.ClassID == 378) {//type
                        obj.ProductCatalogTypeID(objectInfo.ID);
                        obj.ProductCatalogTypeName(objectInfo.FullName);
                        obj.SoftwareLicenceModelID(null);
                        obj.SoftwareLicenceModelName('');
                        self.ProductCatalogClass(null);
                        self.TypeOrModelTempName(objectInfo.FullName);
                        self.GetProductClass(objectInfo.ID);
                    }
                    else if (objectInfo != null && objectInfo.ClassID == 38) {//model
                        obj.ProductCatalogTypeID(null);
                        obj.ProductCatalogTypeName('');
                        obj.SoftwareLicenceModelID(objectInfo.ID);
                        obj.SoftwareLicenceModelName(objectInfo.FullName);
                        self.ProductCatalogClass(null);
                        self.TypeOrModelTempName(objectInfo.FullName);
                    }
                    else {
                        obj.ProductCatalogTypeID(null);
                        obj.ProductCatalogTypeName('');
                        obj.SoftwareLicenceModelID(null);
                        obj.SoftwareLicenceModelName('');
                        self.ProductCatalogClass(null);
                        self.TypeOrModelTempName('');
                    }
                };
                //
                self.productCatalogType = ko.observableArray([true, false, false, false, false, false, false, false, 223]);
                self.typeOrModelSearcher = null;
                self.InitializeTypeOrModelSearcher = function () {
                    var fh = new fhModule.formHelper();
                    var test = self.productCatalogType();
                    var productD = fh.SetTextSearcherToField(
                        self.$region.find('.typeOrModel'),
                        'ProductCatalogTypeAndModelSearcher',
                        null,
                        self.productCatalogType(),
                        function (objectInfo) {//select
                            self.SetTypeOrModel(objectInfo);
                        },
                        function () {//reset
                            self.SetTypeOrModel(null);
                        },
                        function (selectedItem) {//close
                            if (!selectedItem) {
                                self.SetTypeOrModel(null);
                            }
                        });
                    $.when(productD).done(function (ctrl) {
                        self.typeOrModelSearcher = ctrl;
                        self.typeOrModelSearcher.ReadOnly(!self.CanEdit());
                        ctrl.CurrentUserID = null;
                        //
                        self.$region.find('.typeOrModel').focus();
                    });
                };
            }
            //
            {//licence type
                self.selectedLicenceType = ko.observable(null);
                self.licenceTypeComboItems = ko.observableArray([]);
                self.getLicenceTypeComboItems = function () {
                    return {
                        data: self.licenceTypeComboItems(),
                        totalCount: self.licenceTypeComboItems().length
                    };
                };
                //
                self.ajaxControlLicenceType = new ajax.control();
                self.GetLicenceTypeList = function () {
                    self.ajaxControlLicenceType.Ajax(self.$region.find('.invoice-licenceType-combobox'),
                        {
                            dataType: "json",
                            method: 'GET',
                            url: '/assetApi/GetLicenceTypeList'
                        },
                        function (result) {
                            if (result) {
                                var selEl = null;
                                result.forEach(function (el) {
                                    self.licenceTypeComboItems().push(el);
                                    //
                                    if (self.object().LicenceType() === el.ID)
                                        selEl = el;
                                });
                                self.licenceTypeComboItems.valueHasMutated();
                                self.selectedLicenceType(selEl);
                            }
                        });
                };
            }
            //
            {//licence scheme
                self.selectedLicenceScheme = ko.observable(null);
                self.licenceSchemeComboItems = ko.observableArray([]);
                self.getLicenceSchemeComboItems = function () {
                    return {
                        data: self.licenceSchemeComboItems(),
                        totalCount: self.licenceSchemeComboItems().length
                    };
                };
                //
                self.ajaxControlLicenceScheme = new ajax.control();
                self.GetLicenceSchemeList = function () {
                    self.ajaxControlLicenceScheme.Ajax(self.$region.find('.invoice-licenceScheme-combobox'),
                        {
                            dataScheme: "json",
                            method: 'GET',
                            url: '/assetApi/GetLicenceSchemeList'
                        },
                        function (result) {
                            if (result) {
                                var selEl = null;
                                result.forEach(function (el) {
                                    self.licenceSchemeComboItems().push(el);
                                    //
                                    if (self.object().LicenceScheme() === el.ID)
                                        selEl = el;
                                });
                                self.licenceSchemeComboItems.valueHasMutated();
                                self.selectedLicenceScheme(selEl);
                            }
                        });
                };
            }
            //
            self.initNumericUpDownControl = function (selector, ko_value, minValue, maxValue, isInteger) {
                var $frm = $('#' + self.frm.GetRegionID()).find('.frmContractLicence');
                var $div = $frm.find(selector);
                showSpinner($div[0]);
                require(['jqueryStepper'], function () {
                    $div.stepper({
                        type: isInteger == true ? 'int' : 'float',
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [minValue, maxValue],
                        onStep: function (val, up) {
                            ko_value(isInteger ? parseInt(val) : getFormattedMoneyString(val.toString()));
                        }
                    });
                    hideSpinner($div[0]);
                });
            };            
            //

            self.EraseTextClick = function () {
                self.object().Name('');
            };

            self.Save = function () {
                var retval = $.Deferred();
                //
                if (self.object().Name() == null || self.object().Name().length == 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractLicence_NamePrompt'));
                    });
                    return false;
                }
                if (!self.object().ProductCatalogTypeID() && !self.object().SoftwareLicenceModelID()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractLicence_ProductCatalogTypeOrModelPrompt'));
                    });
                    return false;
                }
                if (!self.SoftwareModelID()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractLicence_SoftwareModelPrompt'));
                    });
                    return false;
                }
                if (!self.selectedLicenceType()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractLicence_TypePrompt'));
                    });
                    return false;
                }
                if (!self.selectedLicenceScheme()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractLicence_SchemePrompt'));
                    });
                    return false;
                }
                if (self.object().Count() == null || self.object().Count() < 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractLicence_CountPrompt'));
                    });
                    return false;
                }
                if (self.object().Cost() == null || self.object().Cost() < 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ContractLicence_CostPrompt'));
                    });
                    return false;
                }
                showSpinner();
                //
                var contractLicence =
                {
                    ID: self.object().ID(),
                    ServiceContractID: self.object().ServiceContractID(),
                    SoftwareModelID: self.SoftwareModelID(),
                    LicenceType: self.selectedLicenceType().ID,
                    LicenceScheme: self.selectedLicenceScheme().ID,
                    Count: self.object().NoLimits() ? null : self.object().Count(),
                    CanDowngrade: self.object().CanDowngrade(),
                    NoLimits: self.object().NoLimits(),
                    IsFull: self.object().IsFull(),
                    Cost: self.object().getDecimal(self.object().Cost()),
                    ProductCatalogTypeID: self.object().ProductCatalogTypeID(),
                    ProductCatalogTypeName: self.object().ProductCatalogTypeName(),
                    SoftwareLicenceModelID: self.object().SoftwareLicenceModelID(),
                    SoftwareLicenceModelName: self.object().SoftwareLicenceModelName(),
                    SoftwareLicenceID: self.object().SoftwareLicenceID(),
                    SoftwareLicenceName: self.object().SoftwareLicenceName(),
                    Name: self.object().Name(),
                    IsContractAgreement: self.IsContractAgreement(),
                    LimitInDays: self.object().LimitInDays(),
                    Version: self.object().Version()
                };
                //
                self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/EditContractLicence',
                        method: 'POST',
                        data: contractLicence
                    },
                    function (response) {
                        if (response.Response.Result === 0 && response.NewModel) {
                            var obj = response.NewModel;
                            //
                            hideSpinner();
                            retval.resolve(true);
                        }
                        else if (response.Response.Result === 8) {
                            hideSpinner();
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), response.Response.Message && response.Response.Message.length > 0 ? response.Response.Message : getTextResource('ValidationError'), 'error');
                            });
                        }
                        else {
                            hideSpinner();
                            retval.resolve(false);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContractLicence.js, Save]', 'error');
                            });
                        }
                    });
                //
                return retval.promise();
            };
            //форма лицензии
            self.EditCreatedLicence = function () {
                if (self.object().SoftwareLicenceID()) {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSoftwareLicenceForm(self.object().SoftwareLicenceID);
                    });
                }
            };
            //
            self.AfterRender = function (editor, elements) {
                self.InitializeTypeOrModelSearcher();
                self.InitializeSoftwareModelSearcher();
                self.initNumericUpDownControl('.count', self.object().Count, 0, Math.pow(2, 32) - 1, true);
                self.initNumericUpDownControl('.cost', self.object().Cost, 0, Math.pow(2, 32) - 1, false);

            };
            //
            self.Initialize = function () {
                self.GetLicenceSchemeList();
                self.GetLicenceTypeList();

                self.$region.find('#IdName').focus();
            };
            //
            self.dispose = function () {
                if (self.softwareModelSearcher != null)
                    self.softwareModelSearcher.Remove();
                if (self.typeOrModelSearcher != null)
                    self.typeOrModelSearcher.Remove();
                self.TypeOrModelName.dispose();
            };
        },

        ShowDialog: function (id, serviceContractID, isSpinnerActive, IsContractAgreement, obj) {
            $.when(operationIsGrantedD(891), operationIsGrantedD(894), operationIsGrantedD(892)).done(function (can_properties, can_update, can_add) {
                //OPERATION_ServiceContractLicence_Add = 892,
                //OPERATION_ServiceContractLicence_Update = 894,
                //OPERATION_ServiceContractLicence_Properties = 891,
                if (can_add && id == null) {
                    can_update = true;
                    can_properties = true;
                }
                if (can_properties == false) {
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
                //
                vm.productCatalogType.push(obj.IsSupplyOptionsAvailable());
                vm.productCatalogType.push(obj.IsRentOptionsAvailable());
                vm.productCatalogType.push(obj.IsUpdateOptionsAvailable());
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
                if (can_update == true)
                    buttons.push(bSave);
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                    'region_frmContractLicence',//form region prefix
                    'setting_frmContractLicence',//location and size setting
                    getTextResource('Contract_SoftwareUsageRights'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    300, 300,//minSize
                    buttons,//form buttons
                    function () {
                        vm.dispose();
                        ko.cleanNode(bindElement);
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/Contracts/frmContractLicence\', afterRender: AfterRender}"'//attributes of form region
                );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                frm.ExtendSize(600, 700);//normal size
                vm.frm = frm;
                vm.CanEdit(can_update);
                vm.$region = $('#' + frm.GetRegionID());
                //
                vm.IsContractAgreement(IsContractAgreement);
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show(), vm.object().load(id, serviceContractID, IsContractAgreement)).done(function (frmD, loadD) {
                    vm.GetProductClass(vm.object().ProductCatalogTypeID());
                    hideSpinner();
                    vm.Initialize();
                    //
                    if (vm.object().SoftwareLicenceID() != null)
                        vm.CanEdit(false);
                });
            });
        },

    };
    return module;
});