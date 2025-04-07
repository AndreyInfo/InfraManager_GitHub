define(['knockout', 'ajax', 'dateTimeControl', 'typeHelper', 'models/FinanceForms/ActivesRequestSpecificationForm'], function (ko, ajax, dtLib, typeHelper, specForm) {
    var module = {
        contractRegistration: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            //            
            self.id = ko.observable(null);
            self.classID = 115;//OBJ_ServiceContract = 115
            //
            self.externalNumber = ko.observable('');
            self.description = ko.observable('');
            self.note = ko.observable('');
            //
            self.supplierID = ko.observable(null);
            self.supplierName = ko.observable('');
            //
            self.productCatalogID = ko.observable(null);
            self.productCatalogClassID = ko.observable(null);
            self.productCatalogFullName = ko.observable('');
            self.productCatalogTypeID = ko.pureComputed(function () {
                return self.productCatalogClassID() == 378 ? self.productCatalogID() : null;//OBJ_ProductCatalogType
            });
            self.serviceContractModelID = ko.pureComputed(function () {
                return self.productCatalogClassID() == 182 ? self.productCatalogID() : null;//OBJ_ServiceContractModel
            });
            self.serviceContractModelID_handle = self.serviceContractModelID.subscribe(function (newValue) {
                if (!newValue)
                    return;
                //
                self.ajaxControl.Ajax(null,
                    {
                        url: '/imApi/GetContractModel?id=' + newValue,
                        method: 'GET'
                    },
                    function (response) {//GetContractModelOutModel
                        if (response && response.Result == 0 && response.Model) {
                            var model = response.Model;
                            //
                            if (model.ManufacturerID && model.ManufacturerName) {
                                self.manufacturerID(model.ManufacturerID);
                                self.manufacturerName(model.ManufacturerName);
                            }
                            if (model.Note)
                                self.note(model.Note);
                            if (model.ContractSubject)
                                self.description(model.ContractSubject);
                            if (model.UpdateAvailable)
                                self.updateTypeEnabled(model.UpdateAvailable);
                        }
                    });
            });
            //
            self.manufacturerID = ko.observable(null);
            self.manufacturerName = ko.observable('');
            //
            self.dateStart = ko.observable(new Date());
            self.dateStartString = ko.pureComputed({
                read: function () {
                    return self.dateStart() ? dtLib.Date2String(self.dateStart(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.dateStart(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.dateStart(null);//value incorrect => reset value                    
                    else
                        self.dateStart(dtLib.StringToDate(value));
                }
            });
            //
            self.dateEnd = ko.observable(new Date());
            self.dateEndString = ko.pureComputed({
                read: function () {
                    return self.dateEnd() ? dtLib.Date2String(self.dateEnd(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.dateEnd(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.dateEnd(null);//value incorrect => reset value                    
                    else
                        self.dateEnd(dtLib.StringToDate(value));
                }
            });
            //
            self.dateRegistered = ko.observable(new Date());
            self.dateRegisteredString = ko.pureComputed({
                read: function () {
                    return self.dateRegistered() ? dtLib.Date2String(self.dateRegistered(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.dateRegistered(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.dateRegistered(null);//value incorrect => reset value                    
                    else
                        self.dateRegistered(dtLib.StringToDate(value));
                }
            });
            //          
            self.timePeriodID = ko.observable(0);
            //
            self.updateTypeValues = [
                { ID: 0, Name: getTextResource('True') },
                { ID: 1, Name: getTextResource('False') },
                { ID: 2, Name: '' },//automatic disabled
            ]
            self.updateType = ko.observable(self.updateTypeValues[1]);
            self.updateTypeEnabled = ko.pureComputed({
                read: function () {
                    return self.updateType() ? self.updateType().ID == 0 : false;
                },
                write: function (value) {
                    self.updateType(value && value == true ? self.updateTypeValues[0] : self.updateTypeValues[1]);
                }
            });
            self.updateTypeID = ko.pureComputed(function () {
                return self.updateType() ? self.updateType().ID : null;
            });
            //
            self.updatePeriod = ko.observable(0);
            //
            self.financeCenterID = ko.observable(null);
            self.financeCenterFullName = ko.observable('');
            //
            self.initiatorID = ko.observable(null);
            self.initiatorClassID = ko.observable(null);
            self.initiatorFullName = ko.observable('');
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
                    var tmp = specForm.CalculatePriceWithNDS(self.getDecimal(self.cost()), 1, self.ndsTypeID(), self.ndsPercentID(), self.getDecimal(self.ndsCustomValue()));//price, count, type, percent, customValue
                return getFormattedMoneyString(self.cost()? tmp.CostWithNDS.toString() : '0') + ' ' + getTextResource('CurrentCurrency');
            });
            //            
            self.ndsTypeID = ko.observable(0);
            self.ndsPercentID = ko.observable(4);
            self.ndsCustomValue = ko.observable(0);
            //
            self.files = ko.observableArray([]);
            self.parameterValueList = ko.observableArray([]);
            //
            //
            self.dispose = function () {
                self.ajaxControl.Abort();
                self.productCatalogTypeID.dispose();
                self.serviceContractModelID.dispose();
                self.serviceContractModelID_handle.dispose();
                self.dateStartString.dispose();
                self.dateEndString.dispose();
                self.dateRegisteredString.dispose();
                self.updateTypeEnabled.dispose();
                self.updateTypeID.dispose();
                self.costString.dispose();
            };
            //
            self.register = function (showSuccessMessage) {
                var retval = $.Deferred();
                //                
                var data = {
                    'ExternalNumber': self.externalNumber(),
                    'SupplierID': self.supplierID(),
                    'ProductCatalogTypeID': self.productCatalogTypeID(),
                    'ServiceContractModelID': self.serviceContractModelID(),
                    'ManufacturerID': self.manufacturerID(),
                    'Description': self.description(),
                    'Note': self.note(),
                    'UtcDateRegisteredJS': dtLib.GetMillisecondsSince1970(self.dateRegistered()),
                    'UtcDateStartJS': dtLib.GetMillisecondsSince1970(self.dateStart()),
                    'UtcDateEndJS': dtLib.GetMillisecondsSince1970(self.dateEnd()),
                    'TimePeriod': self.timePeriodID(),
                    'UpdateType': self.updateTypeID(),
                    'UpdatePeriod': parseInt(self.updatePeriod()),
                    'FinanceCenterID': self.financeCenterID(),
                    'InitiatorID': self.initiatorID(),
                    'InitiatorClassID': self.initiatorClassID(),
                    'InitiatorName': self.initiatorFullName(),
                    'Cost': self.cost(),
                    'NDSType': self.ndsTypeID(),
                    'NDSPercent': self.ndsPercentID(),
                    'NDSCustomValue': self.ndsCustomValue(),
                    'Files': self.files(),
                    'ParameterValueList': self.parameterValueList(),
                };
                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/registerContract',
                        method: 'POST',
                        dataType: 'json',
                        data: data
                    },
                    function (response) {//ServiceContractRegistrationResponse
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
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ContractRegistration.js, register]', 'error');
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