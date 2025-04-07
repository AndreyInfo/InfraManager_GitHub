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

                self.Template = '../UI/Forms/Asset/AssetOperations/templateParamsUpdate/UpdateCharacteristics(Active)';

                self.Load = function () {};

                self.AfterRender = function () {};               
                

                self.EndDate= ko.computed(function () {
                    if (vm.SoftwareLicenceObject() != null && vm.periodEndDateTime() != null && vm.SoftwareLicenceObject().LimitInDays != null) {                        
                        let d = new Date(vm.periodEndDateTime().getFullYear(), vm.periodEndDateTime().getMonth(), vm.periodEndDateTime().getDate());
                        d.setDate(vm.periodEndDateTime().getDate() + vm.SoftwareLicenceObject().LimitInDays);                                                
                        let dt = dtLib.Date2String(d, true);                        
                        return dt;
                    }
                    return "";
                });

                self.StartDate= ko.computed(function () {                    
                    return (vm.periodEndDateTime() != null) ? dtLib.Date2String(vm.periodEndDateTime(), true) : "";
                });

            }

        };
        return module;
    });