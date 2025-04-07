define(['knockout', 'jquery', 'ajax'], function (ko, $, ajax) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('Contract_GeneralTab');
            self.Template = '../UI/Forms/Asset/frmSupplier_generalTab';
            self.IconCSS = 'generalTab';
            //
            self.IsVisible = ko.observable(true);
            //
            //when object changed
            self.init = function (obj) {

            };
            //when tab selected
            self.load = function () {

            };
            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
            };
            //
            self.ShowSupplierList = function () {
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSupplierList();
                });
            };

            self.ShowForm = function (obj) {
                if (!obj || !obj.ID || !obj.ClassID)
                    return;
                //
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowAssetForm(obj.ID, obj.ClassID);
                });
            };
        }
    };
    return module;
});
