define(['knockout',
    'jquery',
    'ajax',
    'dateTimeControl'
],
    function (ko,
        $,
        ajaxLib,
        dtLib
    ) {
        var module = {
            Group: function (vm) {
                var self = this;
                self.$region = vm.$region;

                self.Template = '../UI/Forms/Asset/AssetOperations/templateParamsUpdate/UpdateCharacteristics(Rent)';

                const infinitySign = '\u221E';
                const infinity = 2147483647;

                self.Load = function () {

                };

                self.AfterRender = function () {

                };
                self.ShowCount = ko.observable(false);
                self.IsEqual = ko.observable(false);
                self.IsEqualTime = ko.observable(vm.SofLicenceObject() != null && vm.ParentSoftwareLicence() != null);

                self.EndDate = ko.computed(function () {
                    if (vm.SofLicenceObject() != null && vm.SofLicenceObject().hasOwnProperty('NewEndDate')) {
                        let d = new Date(vm.NewEndDate().getFullYear(), vm.NewEndDate().getMonth(), vm.NewEndDate().getDate());
                        let dt = dtLib.Date2String(d, true);
                        return dt;
                    }
                    return "";
                });

                self.EndDateParent = ko.computed(function () {
                    if (vm.ParentSoftwareLicence() != null && vm.SofLicenceObject().hasOwnProperty('OldEndDate')) {
                        let d = new Date(vm.OldEndDate().getFullYear(), vm.OldEndDate().getMonth(), vm.OldEndDate().getDate());
                        let dt = dtLib.Date2String(d, true);
                        return dt;
                    }
                    return "";
                });


                self.Balance = ko.computed(function () {
                    var t1 = vm.SoftwareLicenceUpdateObject;
                    var t2 = vm.SofLicenceObject();
                    var t3 = vm.ParentSoftwareLicence();

                    if (vm.SoftwareLicenceUpdateObject != null) {
                        return vm.SoftwareLicenceUpdateObject.NewReferenceCount;
                    } else if (vm.SofLicenceObject() != null && vm.ParentSoftwareLicence() != null) {
                        let balance = vm.SofLicenceObject().Balance == infinitySign ? infinity : vm.SofLicenceObject().Balance;
                        let parentBalance = vm.ParentSoftwareLicence().Balance == infinitySign ? infinity : vm.ParentSoftwareLicence().Balance;

                        if ((balance == infinity && parentBalance != infinity)
                            || (vm.ParentSoftwareLicence().Balance < vm.SofLicenceObject().Balance)) {
                            return (vm.ParentSoftwareLicence().Balance) == infinity ? '\u221E' : vm.ParentSoftwareLicence().Balance;
                        }
                        else {
                            return (vm.SofLicenceObject().Balance) == infinity ? '\u221E' : vm.SofLicenceObject().Balance;
                        }

                    } else if (vm.SofLicenceObject() != null) {
                        return (vm.SofLicenceObject().Balance) == infinity ? '\u221E' : vm.SofLicenceObject().Balance;
                    }
                    return "";
                });

                self.BalanceParent = ko.computed(function () {
                    if (vm.SoftwareLicenceUpdateObject != null) {
                        return vm.SoftwareLicenceUpdateObject.OldReferenceCount;
                    }
                    else if (vm.ParentSoftwareLicence() != null) {
                        return (vm.ParentSoftwareLicence().Balance) == infinity ? '\u221E' : vm.ParentSoftwareLicence().Balance;
                    }
                    return "";
                });

                function isValidDate(d) {
                    return d === typeof (Date) && !isNaN(d);
                }

            }

        };
        return module;
    });