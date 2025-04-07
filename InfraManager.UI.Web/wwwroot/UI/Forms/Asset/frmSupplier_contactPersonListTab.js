define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'dateTimeControl', './../Asset/Controls/SupplierContactPersonList', 'jqueryStepper'], function (ko, $, ajax, formControl, fhModule, dtLib, supplierContactPersonList) {
    var module = {
        Tab: function (vm, existsInDataBase) {
            var self = this;
            //            
            self.IsVisible = ko.observable(existsInDataBase);
            //
            self.Name = getTextResource('Contract_ContactListTab');
            self.Template = '../UI/Forms/Asset/frmSupplier_contactPersonListTab';
            self.IconCSS = 'contactPersonTab';
            //
            self.list = null;
            //
            //when object changed
            self.init = function (obj) {

            };
            //when tab selected
            self.load = function () {
                self.list = new supplierContactPersonList.List(vm);
            };
            //when tab unload
            self.dispose = function () {
                if (self.list)
                    self.list.dispose();
            };
            //
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
            };
        },

    };
    return module;
});