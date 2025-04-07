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
                
                self.Template = '../UI/Forms/Asset/AssetOperations/templateParamsUpdate/UpdateCharacteristics(Upgrade)';

                self.Load = function () {
                    
                };

                self.AfterRender = function () {
                    
                };
                
                self.IsEqual = ko.observable(false);
                self.IsEqualSoftwareVersion = ko.observable(false);
                self.IsEqualSoftwareModel = ko.observable(false);

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
                
                
                self.SoftwareModelParent= ko.computed(function () {
                    return vm.ParentSoftwareLicence() != null ? vm.ParentSoftwareLicence().SoftwareModelName : "";
                });

                self.SoftwareModel= ko.computed(function () {
                    return vm.SoftwareLicenceObject() != null ? vm.SoftwareLicenceObject().SoftwareModelName : "";
                });

                self.SoftwareVersionParent= ko.computed(function () {
                    return vm.ParentSoftwareLicence() != null ? vm.ParentSoftwareLicence().SoftwareModelVersion : "";
                });
                
                self.SoftwareVersion= ko.computed(function () {
                    return vm.SoftwareLicenceObject() != null ? vm.SoftwareLicenceObject().SoftwareModelVersion : "";
                });

                self.Balance = ko.computed(function () {
                    if (vm.SoftwareLicenceUpdate() != null) {
                        return vm.SoftwareLicenceUpdate().NewReferenceCount;
                    } else if (vm.SoftwareLicenceObject() != null && vm.ParentSoftwareLicence() != null) {
                        let balance = vm.SoftwareLicenceObject().Balance == infinitySign ? infinity : vm.SoftwareLicenceObject().Balance;
                        let parentBalance = vm.ParentSoftwareLicence().Balance == infinitySign ? infinity : vm.ParentSoftwareLicence().Balance;
                        
                        if (   (balance == infinity && parentBalance != infinity)
                            || (vm.ParentSoftwareLicence().Balance < vm.SoftwareLicenceObject().Balance) )
                        {
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
                    if (vm.SoftwareLicenceUpdate() != null) {
                        return vm.SoftwareLicenceUpdate().OldReferenceCount;
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