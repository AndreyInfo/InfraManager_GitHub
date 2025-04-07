define(['knockout', 'ajax', 'dateTimeControl', './Contract'], function (ko, ajax, dtLib, m_contract) {
    var module = {
        contractLicence: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            //            
            self.ID = ko.observable(null);
            self.ClassID = 391;///OBJ_ServiceContractLicence = 391
            //
            self.ServiceContractID = ko.observable(null);
            self.SoftwareModelID = ko.observable(null);
            self.SoftwareModelName = ko.observable(null);
            self.LicenceType = ko.observable(null);
            self.LicenceScheme = ko.observable(null);
            self.Count = ko.observable(1);
            self.CanDowngrade = ko.observable(true);
            self.IsFull = ko.observable(false);
            self.Cost = ko.observable(0);
            self.ProductCatalogTypeID = ko.observable(null);
            self.ProductCatalogTypeName = ko.observable('');
            self.SoftwareLicenceModelID = ko.observable(null);
            self.SoftwareLicenceModelName = ko.observable('');
            self.SoftwareLicenceID = ko.observable(null);
            self.SoftwareLicenceName = ko.observable('');
            self.Name = ko.observable('');
            self.LimitInDays = ko.observable(0);
            self.Version = ko.observable(null);            
            //
            self.NoLimits = ko.observable(false);
            //
            self.getDecimal = function (val) {
                if (val == null || val == undefined || val == '')
                    return parseFloat(0);
                val = val.toString().replace(',', '.').split(' ').join('');
                val = parseFloat(val);
                if (isNaN(val) || !isFinite(val))
                    return parseFloat(0);
                return val;
            };
            //
            self.loadFields = function (obj) {
                self.ID(obj != null ? obj.ID : null);
                self.ServiceContractID(obj != null ? obj.ServiceContractID : null);
                self.SoftwareModelID(obj != null ? obj.SoftwareModelID : null);
                self.SoftwareModelName(obj != null ? obj.SoftwareModelName + ' / ' + obj.Version : null);
                self.LicenceType(obj != null ? obj.LicenceType : null);
                self.LicenceScheme(obj != null ? obj.LicenceScheme : null);
                self.CanDowngrade(obj != null ? obj.CanDowngrade : 0);
                self.IsFull(obj != null ? obj.IsFull : false);
                self.Cost(getFormattedMoneyString((obj != null ? obj.Cost : 0).toString()));
                self.ProductCatalogTypeID(obj != null ? obj.ProductCatalogTypeID : null);
                self.ProductCatalogTypeName(obj != null ? obj.ProductCatalogTypeName : '');
                self.SoftwareLicenceModelID(obj != null ? obj.SoftwareLicenceModelID : null);
                self.SoftwareLicenceModelName(obj != null ? obj.SoftwareLicenceModelName : '');
                self.SoftwareLicenceID(obj != null ? obj.SoftwareLicenceID : null);
                self.SoftwareLicenceName(obj != null ? obj.SoftwareLicenceName : '');
                self.LimitInDays(obj != null ? (obj.LimitInDays != null ? obj.LimitInDays:0): 0);
                self.Name(obj != null ? obj.Name : '');
                self.Version(obj != null ? obj.Version : '');
                //
                if (obj && obj.Count !== null)
                    self.Count(obj.Count);
                else
                    self.NoLimits(true);
            };
            self.load = function (id, serviceContractID, IsAgreementLicence) {
                var retval = $.Deferred();
                //
                if (id) {
                    self.ajaxControl.Ajax(null,
                        {
                            url: '/assetApi/GetContractLicence',
                            method: 'GET',
                            data: { ID: id, IsAgreementLicence: IsAgreementLicence }
                        },
                        function (response) {
                            if (response.Result === 0 && response.ContractLicence) {
                                var obj = response.ContractLicence;                                
                                self.loadFields(obj);
                                retval.resolve(true);
                            }
                            else {
                                retval.resolve(false);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ContractLicence.js, Load]', 'error');
                                });
                            }
                        });
                }
                else if (serviceContractID) {
                    self.ServiceContractID(serviceContractID);
                    retval.resolve(true);
                }
                return retval;
            };            
        },
    };
    return module;
});