define(['knockout',
    'jquery',
    'ajax'
],
    function (ko,
        $,
        ajaxLib
    ) {
        var module = {
            Group: function (vm) {
                var self = this;
                self.$region = vm.$region;

                const infinity = 2147483647;
                const infinitySign = '\u221E';

                self.Template = '../UI/Forms/Asset/AssetOperations/templateParamsUpdate/UpdateCharacteristics(UpgradeReadOnly)';

                self.Load = function () {
                    self.IsEqualSoftwareModel();
                    self.IsEqualSoftwareVersion(self.SoftwareVersionParent != self.SoftwareVersion);
                };

                self.AfterRender = function () {

                };

                self.IsEqual = ko.computed(function () {
                    return false;
                });

                self.IsEqualSoftwareVersion = ko.computed(function () {
                    return self.SoftwareVersionParent != self.SoftwareVersion;
                });

                self.IsEqualSoftwareModel = ko.computed(function () {
                    return self.SoftwareModelParent != self.SoftwareModel;
                });

                self.handle = vm.ParentSoftwareLicence.subscribe(function (newval) {

                    if (vm.SoftwareLicenceObject() != null && newval != null) {
                        let balance = vm.SoftwareLicenceObject().Balance == infinitySign ? infinity : vm.SoftwareLicenceObject().Balance;
                        let parentBalance = vm.ParentSoftwareLicence().Balance == infinitySign ? infinity : vm.ParentSoftwareLicence().Balance;
                        self.IsEqual(balance != parentBalance);
                        self.IsEqualSoftwareModel(vm.SoftwareLicenceObject().SoftwareModelName != vm.ParentSoftwareLicence().SoftwareModelName);
                        self.IsEqualSoftwareVersion(vm.SoftwareLicenceObject().SoftwareModelVersion != vm.ParentSoftwareLicence().SoftwareModelVersion);
                    }
                    else {
                        self.IsEqual(false);
                        self.IsEqualSoftwareModel(false);
                        self.IsEqualSoftwareVersion(false);
                    }
                });

                self.handleChild = vm.SoftwareLicenceObject.subscribe(function (newval) {

                    if (vm.ParentSoftwareLicence() != null && newval != null) {
                        let balance = vm.SoftwareLicenceObject().Balance == infinitySign ? infinity : vm.SoftwareLicenceObject().Balance;
                        let parentBalance = vm.ParentSoftwareLicence().Balance == infinitySign ? infinity : vm.ParentSoftwareLicence().Balance;
                        self.IsEqual(balance != parentBalance);
                        self.IsEqualSoftwareModel(vm.SoftwareLicenceObject().SoftwareModelName != vm.ParentSoftwareLicence().SoftwareModelName);
                        self.IsEqualSoftwareVersion(vm.SoftwareLicenceObject().SoftwareModelVersion != vm.ParentSoftwareLicence().SoftwareModelVersion);
                    }
                    else {
                        self.IsEqual(false);
                        self.IsEqualSoftwareModel(false);
                        self.IsEqualSoftwareVersion(false);
                    }
                });
                
                self.SoftwareModelParent = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.OldSoftwareModelName;
                });

                self.SoftwareModel = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.NewSoftwareModelName;
                });

                self.SoftwareVersionParent = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.OldVersion;
                });

                self.SoftwareVersion = ko.computed(function () {
                    return vm.SoftwareLicenceUpdateObject.NewVersion;
                });

                self.Balance = ko.computed(function () {
                    if (vm.SoftwareLicenceUpdateObject != null) {
                        return vm.SoftwareLicenceUpdateObject.NewReferenceCount;
                    } else if (vm.SoftwareLicenceObject() != null && vm.ParentSoftwareLicence() != null) {
                        let balance = vm.SoftwareLicenceObject().Balance == infinitySign ? infinity : vm.SoftwareLicenceObject().Balance;
                        let parentBalance = vm.ParentSoftwareLicence().Balance == infinitySign ? infinity : vm.ParentSoftwareLicence().Balance;

                        if ((balance == infinity && parentBalance != infinity)
                            || (vm.ParentSoftwareLicence().Balance < vm.SoftwareLicenceObject().Balance)) {
                            return (vm.ParentSoftwareLicence().Balance) == infinity ? '\u221E' : vm.ParentSoftwareLicence().Balance;
                        }
                        else {
                            return (vm.SoftwareLicenceObject().Balance) == infinity ? '\u221E' : vm.SoftwareLicenceObject().Balance;
                        }

                    } else if (vm.SoftwareLicenceObject() != null) {
                        return (vm.SoftwareLicenceObject().Balance) == infinity ? '\u221E' : vm.SoftwareLicenceObject().Balance;
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

            }

        };
        return module;
    });