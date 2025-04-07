define(
    ['knockout', 'jquery', 'ajax', 'formControl', 'ui_forms/Asset/SoftwareLicence/frmSublicense', 'jqueryStepper'],
    function (ko, $, ajax, formControl, subLicenseModule) {
    var module = {
        CaptionComponentName: 'SoftwareSublicenseTransferCaptionComponent',
        ShowDialog: function (object, callback) {
            showSpinner();            
            var frm = undefined;
            var bindElement = null;
            var buttons = [];
            frm = new formControl.control(
                'region_frmSoftwareSublicenseTransfer', //form region prefix
                'setting_frmSoftwareSublicenseTransfer',//location and size setting
                getTextResource('SoftwareLicenseView'),//caption
                true,//isModal
                true,//isDraggable
                true,//isResizable
                400, 400,//minSize
                buttons,//form buttons
                function () {
                },//afterClose function
                'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareLicence/frmSublicenseTransfer\', afterRender: afterRender}"'//attributes of form region
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
            var vm = object.type === 'pool'
                ? new module.PoolTransferViewModel(object, frm, callback)
                : new module.SublicenceViewModel(object.id, frm, callback);
            //
            frm.ExtendSize(400, 400);//normal size
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
        SoftwareDistributionCentres: function (softwareModelId, softwareDistributionCentreId) {
            var self = this;

            self.list = ko.observableArray([]);
            self.load = function () {
                var retD = $.Deferred();

                new ajax.control().Ajax(null, {
                    dataType: "json",
                    method: 'GET',
                    url: '/assetApi/SoftwareDistributionCentres?softwareModelId=' + softwareModelId
                }, function (newVal) {
                    if (newVal && newVal.Result === 0) {
                        var data = newVal.Data;
                        if (data) {
                            self.list.removeAll();
                            ko.utils.arrayForEach(data.Objects, function (el) {
                                if (el.ObjectData.ID !== softwareDistributionCentreId) {
                                    self.list.push(el.ObjectData);
                                }
                            });
                        }
                        retD.resolve();
                    }
                    else if (newVal && newVal.Result === 1)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                    else if (newVal && newVal.Result === 2)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                    else if (newVal && newVal.Result === 3)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                        });
                    else
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                });

                return retD;
            };            
        },
        ViewModelBase: function (callback) {
            var self = this;
            self.detailsSet = $.Deferred();
            self.formType = ko.observable();
            self.license = ko.observable();
            self.softwareDistributionCentre = ko.observable();
            self.quantityTotal = ko.observable(0);
            self.quantity = ko.observable(0);            
            self.infinity = ko.computed(function () { return self.quantityTotal() == self.infinityValue; });
            self.infinityValue = subLicenseModule.InfinityValue;

            self.caption = ko.pureComputed(function () {
                return getTextResource('SublicenseTransfer_Caption').replace('{0}', self.license());
            });
            self.name = ko.pureComputed(function () {
                return self.license() + ' / ' + self.softwareDistributionCentre();
            });
            self.targetSoftwareDistributionCentre = ko.observable(null);  
            self.getListOfSDC = function (options) {
                var data = self.softwareDistributionCentres == null
                    ? []
                    : self.softwareDistributionCentres.list();

                options.callback({ data: data, total: data.length });
            };

            self.afterRender = function () {
                    $.when(self.detailsSet).done(function (data) {
                        var $input = $('.quantityInput').find('.sdNumberEditor');
                        var maxValue = self.quantityTotal() === self.infinityValue ? 0 : self.quantityTotal();
                        var minValue = (self.quantityTotal() === self.infinityValue || self.quantityTotal() === 0)
                            ? 0
                            : 1;
                        $input.stepper({
                            type: 'int',
                            floatPrecission: 0,
                            wheelStep: 1,
                            arrowStep: 1,
                            limit: [minValue, maxValue],
                            onStep: function (val, up) {
                                self.quantity(val);
                            }
                        });
                    });
                };

            self.validate = function () {
                if (self.targetSoftwareDistributionCentre() === null) {
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('SublicenceTransfer_MissingSDC'),
                            text: '',
                            showCancelButton: false,
                            closeOnConfirm: false,
                            confirmButtonText: getTextResource('ButtonOK'),
                        }, function () {
                            swal.close();
                        });
                    });

                    return false;
                }

                return true;
            };

            self.allowPost = ko.computed(function () {
                return self.quantityTotal() === self.infinityValue
                    || self.quantity() > 0;
            });

            self.toData = function() {
                return {
                    SoftwareDistributionCentreID: self.targetSoftwareDistributionCentre().ID,
                    Quantity: self.infinity() ? null : self.quantity(),
                    RowVersion: self.rowVersion
                };
            };
            self.post = function () {
                if (!self.validate()) {
                    return;
                }

                new ajax.control().Ajax(null, {
                    dataType: "json",
                    data: self.toData(),
                    method: 'PUT',
                    url: self.getPostUri()
                },
                    function (response) {
                        if (response && response.Result === 0) {
                            callback(response.Data && response.Data.Removed);
                            self.close();
                        } else if (response && response.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSublicenseTransfer.js, post]', 'error');
                            });
                        else if (response && response.Result === 2)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSublicenseTransfer.js, post]', 'error');
                            });
                        else if (response && response.Result === 3)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                            });
                        else if (response && response.Result === 5)
                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('ConcurrencyErrorTitle'),
                                    text: getTextResource('Sublicence_ConcurrencyError'),
                                    showCancelButton: false,
                                    closeOnConfirm: false,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                }, function () {
                                    swal.close();
                                    self.load();
                                });
                            });
                        else if (response && response.Result === 7) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                            });
                        } else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmSublicenseTransfer.js, post]', 'error');
                            });
                        }
                    });
            }
        },
        SublicenceViewModel: function (id, frm, callback) {
            var self = this;
            subLicenseModule.ViewModelBase.call(self, id, frm);
            module.ViewModelBase.call(self, callback);
            self.formType('Sub');
            self.softwareDistributionCentres = null;           
            
            self.set = function (data) {
                self.license(data.LicenseName);
                self.softwareDistributionCentre(data.SoftwareDistributionCentreName);
                self.quantityTotal(data.Count ? data.Balance : self.infinityValue);
                self.quantity(self.quantityTotal() === self.infinityValue || self.quantityTotal() === 0 ? 0 : 1);
                self.rowVersion = data.RowVersion; 
                self.softwareModelId = data.SoftwareModelID;
                self.softwareDistributionCentreId = data.SoftwareDistributionCentreID;

                self.detailsSet.resolve(data);
            };

            var baseLoad = self.load;
            self.load = function () {               

                $.when(self.detailsSet).done(function () {
                    self.softwareDistributionCentres =
                        new module.SoftwareDistributionCentres(self.softwareModelId, self.softwareDistributionCentreId);
                    self.softwareDistributionCentres.load();                
                });

                return baseLoad();
            };  
            self.getPostUri = function () {
                return '/assetApi/SoftwareSublicenses/' + id + '/transfer';
            }
        },
        PoolTransferViewModel: function (pool, frm, callback) {
            var self = this;
            module.ViewModelBase.call(self, callback);
            self.formType('pool');
            self.softwareDistributionCentres =
                new module.SoftwareDistributionCentres(pool.SoftwareModelID, pool.SoftwareDistributionCentreID);

            function setDetails(data) {
                self.license(data.Licence);
                self.softwareDistributionCentre(data.SoftwareDistributionCentre);
                self.quantityTotal(data.Balance ? data.Balance : self.infinityValue);
                self.quantity(self.quantityTotal() === self.infinityValue || self.quantityTotal() === 0 ? 0 : 1);
                self.detailsSet.resolve(data);
            };

            function loadDetails() {
                var retD = $.Deferred();

                new ajax.control().Ajax(null, {
                    dataType: "json",
                    method: 'GET',
                    url: '/assetApi/SoftwareSublicencePool/'
                        + pool.SoftwareDistributionCentreID
                        + '/'
                        + pool.SoftwareModelID
                        + '/'
                        + pool.SoftwareLicenceSchemeID
                        + '/'
                        + pool.SoftwareLicenseType
                }, function (newVal) {
                    if (newVal && newVal.Result === 0) {
                        var data = newVal.Data;
                        if (data) {
                            setDetails(data);
                            retD.resolve();
                        }
                    }
                    else if (newVal && newVal.Result === 1)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                    else if (newVal && newVal.Result === 2)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                    else if (newVal && newVal.Result === 3)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                        });
                    else
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                });

                return retD;
            }

            self.load = function () {
                var retD = $.Deferred();

                $.when(self.softwareDistributionCentres.load(), loadDetails())
                    .done(function () { retD.resolve(); });

                return retD;
            };

            self.getPostUri = function () {
                return '/assetApi/SoftwareSublicencePool/'
                    + pool.SoftwareDistributionCentreID
                    + '/'
                    + pool.SoftwareModelID
                    + '/'
                    + pool.SoftwareLicenceSchemeID
                    + '/'
                    + pool.SoftwareLicenseType
                    + '/tranfer';
            };

            self.dispose = function () {

            };

            self.close = function () {
                frm.Close();
            }
        }
    };
    return module;
});