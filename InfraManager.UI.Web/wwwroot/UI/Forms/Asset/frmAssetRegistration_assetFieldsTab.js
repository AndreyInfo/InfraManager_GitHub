define(['knockout', 'jquery', 'ajax', 'dateTimeControl', './AssetFieldsRegistration', 'usualForms'], function (ko, $, ajax, dtLib, assetFieldsLib, fhModule) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            self.CanEdit = ko.observable(true);
            //
            self.Name = getTextResource('TabProperty');
            self.Template = '../UI/Forms/Asset/frmAssetRegistration_assetFieldsTab';
            self.IconCSS = 'assetFieldsTab';
            //
            self.IsVisible = ko.observable(false);
            //
            self.dateTime_controls = [];
            self.searcher_controls = [];
            //
            //when object changed
            self.init = function (obj) {
                self.IsVisible = obj.ShowAssetFields;
            };
            self.AssetFields = null;
            //when tab selected
            self.load = function () {
                if (!self.AssetFields) {
                    self.AssetFields = vm.object().AssetFields;
                    self.InitializeUserFieldsList();
                }
            };
            //when tab validating
            self.validate = function () {
                return true;
            };
            //when tab unload
            self.dispose = function () {
                return;
                //
                self.ajaxControl.Abort();
                //
                for (var i = 0; i < self.dateTime_controls.length; i++)
                    self.dateTime_controls[i].datetimepicker('destroy');
                self.dateTime_controls.splice(0, self.dateTime_controls.length - 1);
                //
                for (var i = 0; i < self.searcher_controls.length; i++)
                    self.searcher_controls[i].Remove();
                self.searcher_controls.splice(0, self.searcher_controls.length - 1);
            };
            //            
            self.afterRender = function () {
                for (var i = 0; i < self.dateTime_controls.length; i++)
                    self.dateTime_controls[i].datetimepicker('destroy');
                self.dateTime_controls.splice(0, self.dateTime_controls.length - 1);
                //
                for (var i = 0; i < self.searcher_controls.length; i++)
                    self.searcher_controls[i].Remove();
                self.searcher_controls.splice(0, self.searcher_controls.length - 1);
                //
                self.initDateControl('.dateReceived', self.AssetFields().DateReceived, self.AssetFields().DateReceivedString);
                self.initDateControl('.warranty', self.AssetFields().Warranty, self.AssetFields().WarrantyString);
                self.initDateControl('.appointmentDate', self.AssetFields().AppointmentDate, self.AssetFields().AppointmentDateString);
                /*self.initDateControl('.dateStart', vm.object().dateStart, vm.object().dateStartString);
                self.initDateControl('.dateEnd', vm.object().dateEnd, vm.object().dateEndString);
                //
                
                //
                self.initSearcherControl('.productCatalogue', 'ServiceContractTypeAndModelSearcher', vm.object().productCatalogID, vm.object().productCatalogFullName, vm.object().productCatalogClassID);
                self.initSearcherControl('.supplier', 'SupplierSearcher', vm.object().supplierID, vm.object().supplierName);
                self.initSearcherControl('.manufacturer', 'ManufacturerSearcher', vm.object().manufacturerID, vm.object().manufacturerName);
                self.initSearcherControl('.financeCenter', 'FinanceCenterSearcher', vm.object().financeCenterID, vm.object().financeCenterFullName);*/
                self.initSearcherControl('.supplier', 'SupplierSearcher', self.AssetFields().SupplierID, self.AssetFields().SupplierName);
                self.initNumericUpDownControl('.cost', self.AssetFields().Cost, 0, 1000000000);
                self.initSearcherControl('.owner', 'OwnerSearcher', self.AssetFields().OwnerID, self.AssetFields().OwnerFullName);
                self.initSearcherControl('.utilizer', 'UtilizerSearcher', vm.object().UtilizerID, vm.object().UtilizerFullName, vm.object().UtilizerClassID);
                self.initSearcherControl('.serviceCenter', 'SupplierSearcher', self.AssetFields().ServiceCenterID, self.AssetFields().ServiceCenterName);
                self.initSearcherControl('.serviceContract', 'ServiceContractSearcher', self.AssetFields().ServiceContractID, self.AssetFields().ServiceContractNumber);
                self.initSearcherControl('.materialResponse', 'MaterialResponsibleUserSearcher', self.AssetFields().UserID, self.AssetFields().UserFullName);
            };
            //
            self.ajaxControl_UserFieldsList = new ajax.control();

            self.InitializeUserFieldsList = function () {
                var retD = $.Deferred();
                //
                self.ajaxControl_UserFieldsList.Ajax(null/*self.$region*/,
                {
                    dataType: "json",
                    method: 'GET',
                    url: '/assetApi/GetUserFields'
                },
                function (newVal) {
                    if (newVal) {
                        if (newVal.Result == 0) {
                            var list = newVal.UserFields;
                            if (list) {
                                vm.object().AssetFields().UserFieldList([]);
                                ko.utils.arrayForEach(list, function (item) {
                                    vm.object().AssetFields().UserFieldList.push(
                                    {
                                        FieldNumber: item.FieldNumber,
                                        FieldName: ko.observable(item.FieldName),
                                        Value: '',
                                    });
                                });
                                vm.object().AssetFields().UserFieldList.valueHasMutated();
                            }
                        }
                    }
                    else {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                        });
                    }
                });
                //
                return retD.promise();
            };
            //
            self.costClick = function () {
                var obj = vm.object();
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowCostNDS(obj.cost, obj.ndsTypeID, obj.ndsPercentID, obj.ndsCustomValue, obj.timePeriodID, ko.observable(true));
                });
            };
            //
            //
            self.initNumericUpDownControl = function (selector, ko_value, minValue, maxValue) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmAssetRegistration');
                var $div = $frm.find(selector);
                showSpinner($div[0]);
                require(['jqueryStepper'], function () {
                    $div.stepper({
                        type: 'int',
                        floatPrecission: 2,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [minValue, maxValue],
                        onStep: function (val, up) {
                            ko_value(val);
                        }
                    });
                    hideSpinner($div[0]);
                });
            };
            //
            self.initDateControl = function (selector, ko_value, ko_valueString) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmAssetRegistration');
                var $div = $frm.find(selector);
                showSpinner($div[0]);
                require(['dateTimePicker'], function () {
                    if (locale && locale.length > 0)
                        $.datetimepicker.setLocale(locale.substring(0, 2));
                    var control = $div.datetimepicker({
                        startDate: ko_value(),
                        closeOnDateSelect: true,
                        format: 'd.m.Y',
                        mask: '39.19.9999',
                        timepicker: false,
                        dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                        value: ko_value(),
                        validateOnBlur: true,
                        onSelectDate: function (current_time, $input) {
                            ko_valueString(dtLib.Date2String(current_time, true));
                        }
                    });
                    self.dateTime_controls.push(control);
                    hideSpinner($div[0]);
                });
            };
            //
            self.initSearcherControl = function (selector, searcherName, ko_id, ko_fullName, ko_classID) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmAssetRegistration');
                var searcherControlD = $.Deferred();
                //
                var fh = new fhModule.formHelper();
                var searcherLoadD = fh.SetTextSearcherToField(
                    $frm.find(selector),
                    searcherName,
                    null,
                    [],
                    function (objectInfo) {//select
                        ko_fullName(objectInfo.FullName);
                        ko_id(objectInfo.ID);
                        if (ko_classID) ko_classID(objectInfo.ClassID);
                    },
                    function () {//reset
                        ko_fullName('');
                        ko_id(null);
                        if (ko_classID) ko_classID(null);
                    },
                    function (selectedItem) {//close
                        if (!selectedItem) {
                            ko_fullName('');
                            ko_id(null);
                            if (ko_classID) ko_classID(0);
                        }
                    });
                $.when(searcherLoadD, userD).done(function (ctrl, user) {
                    searcherControlD.resolve(ctrl);
                    ctrl.CurrentUserID = user.UserID;
                    self.searcher_controls.push(ctrl);
                });
            };
            //
            self.IsLoaded = false;
            //
            //
            self.IsPurchaseContainerVisible = ko.observable(true);
            self.TogglePurchaseContainer = function () {
                self.IsPurchaseContainerVisible(!self.IsPurchaseContainerVisible());
            };
            //
            self.IsOwnerContainerVisible = ko.observable(true);
            self.ToggleOwnerContainer = function () {
                self.IsOwnerContainerVisible(!self.IsOwnerContainerVisible());
            };
            //
            self.IsServiceContainerVisible = ko.observable(true);
            self.ToggleServiceContainer = function () {
                self.IsServiceContainerVisible(!self.IsServiceContainerVisible());
            };
            //
            self.IsInventorizationContainerVisible = ko.observable(true);
            self.ToggleInventorizationContainer = function () {
                self.IsInventorizationContainerVisible(!self.IsInventorizationContainerVisible());
            };
            //
            self.IsUserFieldsContainerVisible = ko.observable(true);
            self.ToggleUserFiedlsContainer = function () {
                self.IsUserFieldsContainerVisible(!self.IsUserFieldsContainerVisible());
            };
        }
    };
    return module;
});