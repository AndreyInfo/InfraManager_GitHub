define(['knockout', 'jquery', 'ajax', 'formControl', 'ui_lists/Asset/LicenceList'], function (ko, $, ajax, formControl, licenseList) {
    var module = {
        CaptionComponentName: 'SoftwareSublicenseCaptionComponent',
        ShowDialog: function (id, isSpinnerActive) {
            if (isSpinnerActive != true) {
                showSpinner();
            }

            var frm = undefined;
            var bindElement = null;
            var buttons = [];
            frm = new formControl.control(
                'region_frmSoftwareSublicense', //form region prefix
                'setting_frmSoftwareSublicense',//location and size setting
                getTextResource('SoftwareLicenseView'),//caption
                true,//isModal
                true,//isDraggable
                true,//isResizable
                800, 600,//minSize
                buttons,//form buttons
                function () {
                    ko.cleanNode(bindElement);
                    vm.dispose();
                    if (vm.AssetOperationControl() != null) {
                        vm.AssetOperationControl().Unload();
                    }
                },//afterClose function
                'data-bind="template: {name: \'../UI/Forms/Asset/frmSublicense\', afterRender: afterRender}"'//attributes of form region
            );

            if (!frm.Initialized) {//form with that region and settingsName was open
                hideSpinner();
                //
                var url = window.location.protocol + '//' + window.location.host + location.pathname + '?softwareSublicenceID=' + id;
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
            var $region = $('#' + frm.GetRegionID());
            var vm = new module.ViewModel(id, $region, frm);
            var oldSizeChanged = frm.SizeChanged;
            frm.SizeChanged = function () {
                oldSizeChanged();
                vm.sizeChanged();
            };
            //
            frm.ExtendSize(800, 600);//normal size
            //
            bindElement = document.getElementById(frm.GetRegionID());
            ko.applyBindings(vm, bindElement);

            $.when(vm.load()).done(function () {
                $.when(frm.Show()).done(function () {
                    if (!ko.components.isRegistered(module.CaptionComponentName))
                        ko.components.register(module.CaptionComponentName, {
                            template: '<span data-bind="text: $str"/>'
                        });
                    frm.BindCaption(vm, "component: {name: '" + module.CaptionComponentName + "', params: { $str: caption } }");
                    hideSpinner();
                });
            });
        },
        ViewModel: function (id, $region, frm) {
            var self = this;

            // data properties
            {
                function set(data) {
                    self.licenseId = data.LicenseID;

                    self.licenceClass(data.LicenseClassName);
                    self.hasValidPeriod(data.HasDateLimits);
                    self.validPeriod(data.ValidPeriod);
                    self.modelName(data.ModelName);
                    self.manufacturerName(data.ManufacturerName);
                    self.licensingScheme(data.LicensingScheme);                    
                    self.version(data.VersionName);
                    self.licenceType(data.LicenseType);
                    self.licenceName(data.LicenseName);
                    self.softwareDistributionCentreName(data.SoftwareDistributionCentreName);
                    self.licenseTotalQuantity(data.Count);
                    self.licenseIssuedQuantity(data.InUseCount);
                    self.licenseAvailableQuantity(data.Balance);                    
                }

                self.licenseId = null;
                self.licenceClass = ko.observable('');
                self.validPeriod = ko.observable('');
                self.hasValidPeriod = ko.observable(false);
                self.modelName = ko.observable('');
                self.manufacturerName = ko.observable('');
                self.licensingScheme = ko.observable('');
                self.version = ko.observable('');
                self.licenceType = ko.observable('');
                self.licenceName = ko.observable('');
                self.inventoryNumber = ko.observable('');
                self.softwareDistributionCentreName = ko.observable('');
                self.licenseTotalQuantity = ko.observable('');
                self.licenseIssuedQuantity = ko.observable('');
                self.licenseAvailableQuantity = ko.observable('');

                self.caption = ko.pureComputed(function () {
                    return getTextResource('Sublicense') + ' ' + self.licenceName() + ' / ' + getTextResource('SDC_Object') + ' ' + self.softwareDistributionCentreName();
                });
                self.modelAndManufacturer = ko.pureComputed(function () {
                    return self.modelName() + ' / ' + self.manufacturerName();
                });
                self.licenceFullName = ko.pureComputed(function () {
                    return self.licenceName() + ' / ' + self.manufacturerName() + ' / ' + self.inventoryNumber();
                });                          
            }

            // load data
            {
                var ajaxControl = new ajax.control();

                self.load = function () {
                    var retval = $.Deferred();
                    //
                    ajaxControl.Ajax(null,
                        {
                            url: '/assetApi/SoftwareSublicenses/' + id,
                            method: 'GET'
                        },
                        function (response) {
                            if (response.Result === 0 && response.Data) {
                                var data = response.Data;
                                set(data);
                                retval.resolve(true);
                            }
                            else {
                                retval.resolve(false);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareSublicence.js, Load]', 'error');
                                });
                            }
                        });

                    return retval;
                };
            }

            // ViewModel common interface
            {
                self.dispose = function () {

                };
                self.AssetOperationControl = function () {

                };
                self.sizeChanged = function () {

                };
                self.afterRender = function () {

                };
            }

            // tabs
            {
                var tabs = {
                    common: 1,
                    permissions: 2
                };

                self.activeTab = ko.observable(tabs.common);
                self.commonTabActive =
                    ko.pureComputed(function () { return self.activeTab() === tabs.common; });
                self.permissionsTabActive =
                    ko.pureComputed(function () { return self.activeTab() === tabs.permissions; });

                self.activateCommonTab = function () {
                    self.activeTab(tabs.common);
                };
                self.activatePermissionsTab = function () {
                    self.activeTab(tabs.permissions);
                };
            }

            // view license details
            {
                self.viewLicenseDetails = function () {
                    showSpinner();
                    require(['assetForms'], function (assetForms) {
                        new assetForms
                            .formHelper(true)
                            .ShowSoftwareLicenceForm(self.licenseId)
                    });
                }
            }

            // list of references
            {
                self.viewName = ko.observable('SoftwareSublicenseReferences');
                self.list = new licenseList.List(self);

                self.list.hasFilter(false);
                self.list.SelectedItemsChanged = function () {
                };
                self.list.listViewRowClick = function () {
                };
                self.list.treeParams({ FiltrationObjectID: id });
            }

            // close
            {
                self.close = function () {
                    frm.Close();
                }
            }
        }
    };

    return module;
});