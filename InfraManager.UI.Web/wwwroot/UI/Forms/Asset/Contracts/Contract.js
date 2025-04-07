define(['knockout', 'ajax', 'models/FinanceForms/ActivesRequestSpecificationForm'], function (ko, ajax, specForm) {
    var module = {
        contract: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            //            
            self.ID = ko.observable(null);
            self.ClassID = 115;//OBJ_ServiceContract = 115
            //
            self.dateCreatedString = ko.observable('');
            //
            self.Number = ko.observable('');
            //
            self.UtcInitialDateStart = ko.observable('');
            self.UtcInitialDateStartDT = ko.observable(null);
            //
            self.UtcInitialDateFinish = ko.observable('');
            self.UtcInitialDateFinishDT = ko.observable(null);
            //
            self.UtcDateRegistered = ko.observable('');
            self.UtcDateRegisteredDT = ko.observable(null);
            //
            self.UtcDateStarted = ko.observable('');
            self.UtcDateStartedDT = ko.observable(null);
            //
            self.UtcDateFinished = ko.observable('');
            self.UtcDateFinishedDT = ko.observable(null);
            //
            self.Cost = ko.observable(0);
            self.CostWithNDS = ko.observable(0);
            self.getDecimal = function (val) {
                if (val == null || val == undefined || val == '')
                    return parseFloat(0);
                val = val.toString().replace(',', '.').split(' ').join('');
                val = parseFloat(val);
                if (isNaN(val) || !isFinite(val))
                    return parseFloat(0);
                return val;
            };
            self.CostString = ko.pureComputed(function () {
                if (self.Cost()) {
                    var tmp = specForm.CalculatePriceWithNDS(self.getDecimal(self.Cost()), 1, self.NDSType(), self.NDSPercent(), self.getDecimal(self.NDSCustomValue()));//price, count, type, percent, customValue
                    self.CostWithNDS(tmp.CostWithNDS);
                }
                return getFormattedMoneyString(self.Cost() ? tmp.CostWithNDS.toString() : '0') + ' ' + getTextResource('CurrentCurrency');
            });
            //
            self.InitialCost = ko.observable(0);
            self.InitialCostString = ko.pureComputed(function () {
                return getFormattedMoneyString(self.InitialCost() ? self.InitialCost().toString() : '0') + ' ' + getTextResource('CurrentCurrency');
            });
            //
            self.Notice = ko.observable('');
            //
            self.SupplierID = ko.observable('');
            self.SupplierName = ko.observable('');
            //
            self.ProductCatalogTypeID = ko.observable('');
            self.ProductCatalogTypeName = ko.observable('');
            //
            self.TimePeriod = ko.observable(0);
            //
            self.LifeCycleStateID = ko.observable('');
            self.LifeCycleStateName = ko.observable('');
            //
            self.ModelID = ko.observable('');
            self.ModelName = ko.observable('');
            //
            self.AddressAsset = ko.observable('');
            //
            self.LoginAsset = ko.observable('');
            //
            self.PasswordAsset = ko.observable('');
            //
            self.AddressLicence = ko.observable('');
            //
            self.LoginLicence = ko.observable('');
            //
            self.IsReInit = ko.observable(false);
            //
            self.PasswordLicence = ko.observable('');
            //
            self.ExternalNumber = ko.observable('');
            //
            self.Description = ko.observable('');
            //
            self.ManufacturerID = ko.observable('');
            self.ManufacturerName = ko.observable('');
            //
            self.ManagerLoaded = ko.observable(false);
            self.ManagerID = ko.observable('');
            self.ManagerClassID = ko.observable('');
            self.ManagerName = ko.observable('');
            self.ManagerPositionName = ko.observable('');
            self.ManagerDivisionFullName = ko.observable('');
            //
            self.InitiatorLoaded = ko.observable(false);
            self.InitiatorID = ko.observable(null);
            self.InitiatorClassID = ko.observable(null);
            self.InitiatorName = ko.observable('');
            self.InitiatorPositionName = ko.observable('');
            self.InitiatorDivisionFullName = ko.observable('');
            //
            self.UpdateType = ko.observable(0);

            self.prolongationAvailable = ko.computed(function () {
                return self.UpdateType && self.UpdateType() == 0;
            });
            //
            self.UpdatePeriod = ko.observable(0);
            //
            self.WorkOrderID = ko.observable(null);
            self.WorkOrderName = ko.observable('');
            //
            self.FinanceCenterID = ko.observable('');
            self.FinanceCenterName = ko.observable('');
            //
            self.NDSType = ko.observable(0);
            //
            self.NDSPercent = ko.observable(0);
            //
            self.NDSCustomValue = ko.observable(null);
            //
            self.LastAgreementID = ko.observable(null);
            self.LastAgreementName = ko.observable('');
            self.CanCreateAgreement = ko.observable(null);
            //
            self.IsSoftwareLicenceTabVisible = ko.observable(true);
            self.IsLicenceMainteinanceTabVisible = ko.observable(true);
            self.IsAssetMainteinanceTabVisible = ko.observable(true);
            //
            self.IsSupplyOptionsAvailable = ko.observable(true);
            self.IsRentOptionsAvailable = ko.observable(true);
            self.IsUpdateOptionsAvailable = ko.observable(true);
            //
            self.HasAgreement = ko.observable(false);
            //
            self.loadFields = function (obj) {
                self.ID(obj.ID);
                if (obj.Number)
                    self.Number(obj.Number);
                //
                self.dateCreatedString(parseDate(obj.UtcDateCreated, true));
                if (obj.UtcInitialDateStart)
                    self.UtcInitialDateStart(parseDate(obj.UtcInitialDateStart, true));//show only date
                //
                if (obj.UtcInitialDateStart)
                    self.UtcInitialDateStartDT(new Date(parseInt(obj.UtcInitialDateStart)));
                //
                if (obj.UtcInitialDateFinish)
                    self.UtcInitialDateFinish(parseDate(obj.UtcInitialDateFinish, true));//show only date
                //
                if (obj.UtcInitialDateFinish)
                    self.UtcInitialDateFinishDT(new Date(parseInt(obj.UtcInitialDateFinish)));
                //
                if (obj.UtcDateRegistered)
                    self.UtcDateRegistered(parseDate(obj.UtcDateRegistered, true));//show only date
                //
                if (obj.UtcDateRegistered)
                    self.UtcDateRegisteredDT(new Date(parseInt(obj.UtcDateRegistered)));
                //
                if (obj.UtcDateStarted)
                    self.UtcDateStarted(parseDate(obj.UtcDateStarted, true));//show only date
                //
                if (obj.UtcDateStarted)
                    self.UtcDateStartedDT(new Date(parseInt(obj.UtcDateStarted)));
                //
                if (obj.UtcDateFinished)
                    self.UtcDateFinished(parseDate(obj.UtcDateFinished, true));//show only date
                //
                if (obj.UtcDateFinished)
                    self.UtcDateFinishedDT(new Date(parseInt(obj.UtcDateFinished)));
                //
                if (obj.InitialCost)
                    self.InitialCost(obj.InitialCost);
                //
                if (obj.Notice)
                    self.Notice(obj.Notice);
                //
                if (obj.SupplierID)
                    self.SupplierID(obj.SupplierID);
                //
                if (obj.SupplierName)
                    self.SupplierName(obj.SupplierName);
                //
                if (obj.ProductCatalogTypeID)
                    self.ProductCatalogTypeID(obj.ProductCatalogTypeID);
                //
                if (obj.ProductCatalogTypeName)
                    self.ProductCatalogTypeName(obj.ProductCatalogTypeName);
                //
                if (obj.TimePeriod)
                    self.TimePeriod(obj.TimePeriod);
                //
                if (obj.LifeCycleStateID)
                    self.LifeCycleStateID(obj.LifeCycleStateID);
                //
                if (obj.LifeCycleStateName)
                    self.LifeCycleStateName(obj.LifeCycleStateName);
                //
                if (obj.ModelID)
                    self.ModelID(obj.ModelID);
                //
                if (obj.ModelName)
                    self.ModelName(obj.ModelName);
                //
                if (obj.AddressAsset)
                    self.AddressAsset(obj.AddressAsset);
                //
                if (obj.LoginAsset)
                    self.LoginAsset(obj.LoginAsset);
                //
                if (obj.PasswordAsset)
                    self.PasswordAsset(obj.PasswordAsset);
                //
                if (obj.AddressLicence)
                    self.AddressLicence(obj.AddressLicence);
                //
                if (obj.LoginLicence)
                    self.LoginLicence(obj.LoginLicence);
                //
                if (obj.IsReInit)
                    self.IsReInit(obj.IsReInit);
                //
                if (obj.PasswordLicence)
                    self.PasswordLicence(obj.PasswordLicence);
                //
                if (obj.ExternalNumber)
                    self.ExternalNumber(obj.ExternalNumber);
                //
                if (obj.Description)
                    self.Description(obj.Description);
                //
                if (obj.ManufacturerID)
                    self.ManufacturerID(obj.ManufacturerID);
                //
                if (obj.ManufacturerName)
                    self.ManufacturerName(obj.ManufacturerName);
                //
                self.ManagerID(obj.ManagerID);
                self.ManagerClassID(obj.ManagerClassID);
                self.ManagerName(obj.ManagerName ? obj.ManagerName : '');
                self.ManagerPositionName(obj.ManagerPositionName ? obj.ManagerPositionName : '');
                self.ManagerDivisionFullName(obj.ManagerDivisionFullName ? obj.ManagerDivisionFullName : '');
                self.ManagerLoaded(true);
                //
                self.InitiatorID(obj.InitiatorID);
                self.InitiatorClassID(obj.InitiatorClassID);
                self.InitiatorName(obj.InitiatorName ? obj.InitiatorName : '');
                self.InitiatorPositionName(obj.InitiatorPositionName ? obj.InitiatorPositionName : '');
                self.InitiatorDivisionFullName(obj.InitiatorDivisionFullName ? obj.InitiatorDivisionFullName : '');
                self.InitiatorLoaded(true);
                //
                if (obj.UpdateType)
                    self.UpdateType(obj.UpdateType);
                //
                if (obj.UpdatePeriod)
                    self.UpdatePeriod(obj.UpdatePeriod);
                //
                if (obj.WorkOrderID)
                    self.WorkOrderID(obj.WorkOrderID);
                //
                if (obj.WorkOrderName)
                    self.WorkOrderName(obj.WorkOrderName);
                //
                if (obj.FinanceCenterID)
                    self.FinanceCenterID(obj.FinanceCenterID);
                //
                if (obj.FinanceCenterName)
                    self.FinanceCenterName(obj.FinanceCenterName);
                //
                if (obj.NDSType)
                    self.NDSType(obj.NDSType);
                //
                if (obj.NDSPercent)
                    self.NDSPercent(obj.NDSPercent);
                //
                if (obj.NDSCustomValue)
                    self.NDSCustomValue(obj.NDSCustomValue);
                //
                self.LastAgreementID(obj.LastAgreementID);
                self.LastAgreementName(obj.LastAgreementName ? obj.LastAgreementName : '');
                self.CanCreateAgreement(obj.CanCreateAgreement);
                //
                self.IsAssetMainteinanceTabVisible(obj.IsAssetMainteinanceTabVisible);
                self.IsLicenceMainteinanceTabVisible(obj.IsLicenceMainteinanceTabVisible);
                self.IsSoftwareLicenceTabVisible(obj.IsSoftwareLicenceTabVisible);
                //                
                self.IsSupplyOptionsAvailable(obj.IsSupplyOptionsAvailable);
                self.IsRentOptionsAvailable(obj.IsRentOptionsAvailable);
                self.IsUpdateOptionsAvailable(obj.IsUpdateOptionsAvailable);
                //
                self.HasAgreement(obj.HasAgreement);
                //
                if (obj.Cost) {
                    self.Cost(obj.Cost);
                    self.CostString();
                }
                //
            };
            //
            self.load = function (ID, vm) {
                var retval = $.Deferred();
                self.ID(ID);
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/assetApi/GetContract',
                    method: 'GET',
                    data: { ID: ID }
                },
                function (response) {
                    if (response.Result === 0 && response.Contract) {
                        var obj = response.Contract;
                        //      
                        self.loadFields(obj);
                        //
                        vm.tabList().forEach(function (tab) {
                            tab.init(self);
                        });
                        //
                        vm.tab_parameters.init();
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContract.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
        },
    };
    return module;
});