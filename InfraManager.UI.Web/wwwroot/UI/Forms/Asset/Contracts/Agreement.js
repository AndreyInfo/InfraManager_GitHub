define(['knockout', 'ajax', 'dateTimeControl', './Contract', 'models/FinanceForms/ActivesRequestSpecificationForm'], function (ko, ajax, dtLib, m_contract, specForm) {
    var module = {
        agreement: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            //            
            self.id = ko.observable(null);
            self.classID = 386;///OBJ_ServiceContractAgreement = 386
            //
            self.contract = ko.observable(new m_contract.contract());
            self.contractFullName = ko.pureComputed(function () {
                var obj = self.contract();
                var retval = getTextResource('Contract');
                if (obj != null) {
                    retval += ' № ' + obj.Number() + ' ' + obj.dateCreatedString();
                    if (obj.ExternalNumber().length > 0)
                        retval += ' ' + obj.ExternalNumber();
                    if (obj.UtcDateRegistered())
                        retval += ' ' + obj.UtcDateRegistered();
                }
                //
                return retval;
            });
            //
            self.number = ko.observable('');
            //
            self.type = ko.observable(null);
            self.typeValues = [
             { ID: 0, Name: getTextResource('Contract_Prolongation') },
            ];
            self.typeID = ko.pureComputed({
                read: function () {
                    return self.type() ? self.type().ID : null;
                },
                write: function (value) {
                    if (value != null)
                        self.type(self.typeValues[value]);
                    else
                        self.type(null);
                }
            });
            self.typeString = ko.pureComputed({
                read: function () {
                    return self.type() ? self.type().Name : '';
                }
            });
            self.typeDataSource = function (options) {
                var data = self.typeValues;
                options.callback({ data: data, total: data.length });
            };
            //
            self.state = ko.observable(0);
            self.stateName = ko.observable('');
            //
            self.utcDateStartDT = ko.observable(null);
            self.utcDateStart = ko.pureComputed({
                read: function () {
                    return self.utcDateStartDT() ? dtLib.Date2String(self.utcDateStartDT(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.utcDateStartDT(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.utcDateStartDT(null);//value incorrect => reset value                    
                    else
                        self.utcDateStartDT(dtLib.StringToDate(value));
                }
            });
            //
            self.utcDateEndDT = ko.observable(null);
            self.utcDateEnd = ko.pureComputed({
                read: function () {
                    return self.utcDateEndDT() ? dtLib.Date2String(self.utcDateEndDT(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.utcDateEndDT(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.utcDateEndDT(null);//value incorrect => reset value                    
                    else
                        self.utcDateEndDT(dtLib.StringToDate(value));
                }
            });
            //
            self.description = ko.observable('');
            //
            self.cost = ko.observable(0);
            self.getDecimal = function (val) {
                if (val == null || val == undefined || val == '')
                    return parseFloat(0);
                val = val.toString().replace(',', '.').split(' ').join('');
                val = parseFloat(val);
                if (isNaN(val) || !isFinite(val))
                    return parseFloat(0);
                return val;
            };
            self.costString = ko.pureComputed(function () {
                if (self.cost())
                    var tmp = specForm.CalculatePriceWithNDS(self.getDecimal(self.cost()), 1, self.ndsType(), self.ndsPercent(), self.getDecimal(self.ndsCustomValue()));//price, count, type, percent, customValue
                return getFormattedMoneyString(self.cost() ? tmp.CostWithNDS.toString() : '0') + ' ' + getTextResource('CurrentCurrency');
            });
            self.costWithNDS = ko.pureComputed(function () {
                var NDSCost = 0;
                if (self.cost()) {
                    var tmp = specForm.CalculatePriceWithNDS(self.getDecimal(self.cost()), 1, self.ndsType(), self.ndsPercent(), self.getDecimal(self.ndsCustomValue()));//price, count, type, percent, customValue
                    NDSCost = tmp.CostWithNDS;
                }
                return NDSCost;
            });
            //
            self.ndsType = ko.observable(0);
            self.ndsPercent = ko.observable(0);
            self.ndsCustomValue = ko.observable(null);
            //
            self.IsApplied = ko.observable(false);
            //
            self.IsSoftwareLicenceTabVisible = ko.observable(true);
            self.IsLicenceMainteinanceTabVisible = ko.observable(true);
            self.IsAssetMainteinanceTabVisible = ko.observable(true);
            //
            self.IsSupplyOptionsAvailable = ko.observable(true);
            self.IsRentOptionsAvailable = ko.observable(true);
            self.IsUpdateOptionsAvailable = ko.observable(true);
            //
            self.loadFields = function (obj) {
                self.id(obj != null ? obj.ID : null);
                self.contract(new m_contract.contract());
                if (obj != null && obj.Contract != null)
                    self.contract().loadFields(obj.Contract);
                //
                self.number(obj != null ? obj.Number : '');
                //
                self.typeID(obj != null ? obj.Type : null);
                //
                self.state(obj != null ? obj.LifeCycleStateID : '');
                self.stateName(obj != null ? obj.LifeCycleStateName : '');
                //
                self.IsApplied(obj != null ? obj.IsApplied : false);
                //
                self.utcDateStartDT(obj != null && obj.UtcDateStart ? new Date(parseInt(obj.UtcDateStart)) : null);
                //
                self.utcDateEndDT(obj != null && obj.UtcDateEnd ? new Date(parseInt(obj.UtcDateEnd)) : null);
                //
                self.description(obj != null ? obj.Description : '');
                //
                self.cost(obj != null ? obj.Cost : 0);
                //
                self.ndsType(obj != null ? obj.NDSType : 0);
                self.ndsPercent(obj != null ? obj.NDSPercent : 0);
                self.ndsCustomValue(obj != null ? obj.NDSCustomValue : 0);
                //
                self.IsAssetMainteinanceTabVisible(self.contract() != null ? self.contract().IsAssetMainteinanceTabVisible() : true);
                self.IsLicenceMainteinanceTabVisible(self.contract() != null ? self.contract().IsLicenceMainteinanceTabVisible() : true);
                self.IsSoftwareLicenceTabVisible(self.contract() != null ? self.contract().IsSoftwareLicenceTabVisible() : true);
                //
                self.IsSupplyOptionsAvailable(self.contract() != null ? self.contract().IsSupplyOptionsAvailable() : true);
                self.IsRentOptionsAvailable(self.contract() != null ? self.contract().IsRentOptionsAvailable() : true);
                self.IsUpdateOptionsAvailable(self.contract() != null ? self.contract().IsUpdateOptionsAvailable() : true);
            };
            //
            self.load = function (id, serviceContractID,vm) {
                var retval = $.Deferred();
                //
                if (id) {
                    self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/GetContractAgreement',
                        method: 'GET',
                        data: { ID: id }
                    },
                    function (response) {
                        if (response.Result === 0 && response.ContractAgreement) {
                            var obj = response.ContractAgreement;
                            self.loadFields(obj);
                            vm.tabList().forEach(function (tab) {
                                tab.init(self);
                            });
                            retval.resolve(true);
                        }
                        else {
                            retval.resolve(false);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Agreement.js, Load]', 'error');
                            });
                        }
                    });
                }
                else if (serviceContractID) {
                    self.ajaxControl.Ajax(null,
                      {
                          url: '/assetApi/GetContract',
                          method: 'GET',
                          data: { ID: serviceContractID }
                      },
                      function (response) {
                          if (response.Result === 0 && response.Contract) {
                              var obj = response.Contract;
                              self.loadFields(null);
                              self.contract().loadFields(obj);
                              self.utcDateStartDT(self.contract().UtcDateFinishedDT());
                              //
                              self.description(self.contract().Description());
                              self.typeID(0);
                              //
                              retval.resolve(true);
                          }
                          else {
                              retval.resolve(false);
                              require(['sweetAlert'], function () {
                                  swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Agreement.js, Load]', 'error');
                              });
                          }
                      });
                }
                return retval;
            };
            //
            self.register = function (showSuccessMessage) {
                var retval = $.Deferred();
                //                
                var data = {
                    'ContractID': self.contract().ID(),
                    'Type': self.typeID(),
                    'Description': self.description(),
                    'UtcDateStartJS': dtLib.GetMillisecondsSince1970(self.utcDateStartDT()),
                    'UtcDateEndJS': dtLib.GetMillisecondsSince1970(self.utcDateEndDT()),
                    'Cost': self.cost(),
                    'NDSType': self.ndsType(),
                    'NDSPercent': self.ndsPercent(),
                    'NDSCustomValue': self.ndsCustomValue()
                };
                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/registerContractAgreement',
                        method: 'POST',
                        dataType: 'json',
                        data: data
                    },
                    function (response) {//RegisterContractAgreementResponse
                        hideSpinner();
                        if (response) {
                            if (response.Message && response.Message.length > 0 && (showSuccessMessage == true || response.Type != 0))
                                require(['sweetAlert'], function () {
                                    swal(response.Message);//some problem
                                });
                            //
                            if (response.Type == 0) {//ok 
                                retval.resolve(response);
                                return;
                            }
                        }
                        retval.resolve(null);
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Agreement.js, register]', 'error');
                        });
                        retval.resolve(null);
                    });
                //
                return retval.promise();
            };
        },
    };
    return module;
});