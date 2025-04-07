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
                
                self.Template = '../UI/Forms/Asset/AssetOperations/templateParamsUpdate/UpdateCharacteristics(Apply)';

                const infinitySign = '\u221E';                
                const infinity = 2147483647;
                
                self.Load = function () {
                   
                };

                self.AfterRender = function () {
                    
                };
                
                self.IsEqual = ko.observable(false);
                self.IsEqualTime = ko.observable(vm.SoftwareLicenceObject() != null && vm.ParentSoftwareLicence() != null);
                
                self.handle = vm.ParentSoftwareLicence.subscribe(function (newval) {
                    if (vm.SoftwareLicenceObject() != null && newval != null) {
                        let balance = vm.SoftwareLicenceObject().Balance == '\u221E' ? infinity : vm.SoftwareLicenceObject().Balance;
                        if (newval.Balance < balance)
                            self.IsEqual(false);
                        else
                            self.IsEqual(balance != vm.ParentSoftwareLicence().Balance);                        
                        
                        let pdt = new Date(newval.EndDate);                        
                        if (vm.SoftwareLicenceObject() != null && vm.periodEndDateTime() != null && newval.LimitInDays != null) {
                            let d = new Date(vm.periodEndDateTime().getFullYear(), vm.periodEndDateTime().getMonth(), vm.periodEndDateTime().getDate());
                            d.setDate(vm.periodEndDateTime().getDate() + vm.SoftwareLicenceObject().LimitInDays);
                            let dt = dtLib.Date2String(d, true);
                            let pdts = dtLib.Date2String(pdt, true);                            
                            self.IsEqualTime(dt != pdts);
                        }                        
                        else {
                            self.IsEqualTime(false);
                        }
                        
                    }
                    else {
                        self.IsEqualTime(false);
                    }
                });

                self.handleChild = vm.SoftwareLicenceObject.subscribe(function (newval) {
                    if (vm.ParentSoftwareLicence() != null && newval != null) {
                        
                        let balance = vm.ParentSoftwareLicence().Balance == '\u221E' ? infinity : vm.ParentSoftwareLicence().Balance;
                        if (newval.Balance > balance)
                            self.IsEqual(false)
                        else
                            self.IsEqual(balance != vm.SoftwareLicenceObject().Balance);
                        
                        let pdt = new Date(vm.ParentSoftwareLicence().EndDate / 1);
                        if (newval != null && vm.periodEndDateTime() != null && newval.LimitInDays != null) {
                            let d = new Date(vm.periodEndDateTime().getFullYear(), vm.periodEndDateTime().getMonth(), vm.periodEndDateTime().getDate());
                            d.setDate(vm.periodEndDateTime().getDate() + vm.SoftwareLicenceObject().LimitInDays);
                            let dt = dtLib.Date2String(d, true);
                            let pdts = dtLib.Date2String(pdt, true);                            
                            self.IsEqualTime(dt != pdts);
                        }
                        else {
                            self.IsEqualTime(false);
                        }
                    }
                    else {
                        self.IsEqualTime(false);
                    }
                });
                
                self.handledt = vm.periodEndDateTime.subscribe(function (val) {
                    if (vm.SoftwareLicenceObject() != null && val != null && vm.SoftwareLicenceObject().LimitInDays != null && vm.ParentSoftwareLicence() != null) {
                        let d = new Date(vm.periodEndDateTime().getFullYear(), vm.periodEndDateTime().getMonth(), vm.periodEndDateTime().getDate());
                        d.setDate(vm.periodEndDateTime().getDate() + vm.SoftwareLicenceObject().LimitInDays);
                        let dt = dtLib.Date2String(d, true);
                        let pdt = vm.ParentSoftwareLicence().EndDate / 1 === parseInt(vm.ParentSoftwareLicence().EndDate, 10) 
                            ? new Date(vm.ParentSoftwareLicence().EndDate / 1)                            
                            : new Date(vm.ParentSoftwareLicence().EndDate);                        
                        let pdts = dtLib.Date2String(pdt, true);
                        self.IsEqualTime(dt != pdts);
                    }
                    else {
                        self.IsEqualTime(false);
                    }
                })


                self.LimitInDays = ko.computed(function () {
                    if (vm.SoftwareLicenceObject() != null) {                        
                        return vm.SoftwareLicenceObject().LimitInDays;
                    }
                    
                    return getTextResource('SelectTheReason'); 
                });

                self.EndDate = ko.computed(function () {
                    if (vm.SoftwareLicenceUpdate() != null) {
                        return vm.SoftwareLicenceUpdate().NewEndDateString;
                    }
                    if (vm.SoftwareLicenceObject() != null && vm.periodEndDateTime() != null && vm.SoftwareLicenceObject().LimitInDays != null) {
                        let d = new Date(vm.periodEndDateTime().getFullYear(), vm.periodEndDateTime().getMonth(), vm.periodEndDateTime().getDate());
                        d.setDate(vm.periodEndDateTime().getDate() + vm.SoftwareLicenceObject().LimitInDays);
                        let dt = dtLib.Date2String(d, true);
                        return dt;
                    }
                    return "";
                });

                self.EndDateParent = ko.computed(function () { 
                    if (vm.SoftwareLicenceUpdate() != null) {
                        return vm.SoftwareLicenceUpdate().OldEndDateString;
                    }
                    if (vm.ParentSoftwareLicence() != null) {
                        if (vm.ParentSoftwareLicence().hasOwnProperty('EndDateString')) {
                            return vm.ParentSoftwareLicence().EndDateString;
                        }
                        else {
                            return dtLib.Date2String(new Date(vm.ParentSoftwareLicence().EndDate / 1 ), true);
                        }
                    }
                    
                    return "";
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

                function isValidDate(d) {
                    return d === typeof(Date) && !isNaN(d);
                }

            }

        };
        return module;
    });