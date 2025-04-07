define(['knockout', 'jquery', 'ajax', 'dateTimeControl', 'usualForms'],
    function (ko, $, ajax, dtLib, fhModule) {
        var module = {
            Tab: function (vm) {
                var self = this;
                self.ajaxControl = new ajax.control();
                self.CanEdit = ko.observable(true);
                //
                self.Name = getTextResource('TabProperty');
                self.Template = '../UI/Forms/Asset/Controls/AssetFields';
                self.IconCSS = 'assetFieldsTab';
                //
                self.IsVisible = ko.observable(true);
                //

                //when object changed
                self.Initialize = function (obj) {
                };

                //when tab selected
                self.load = function () {

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
                };
                //            
                self.afterRender = function () {
                };
                //
            }
        };
        return module;
    });