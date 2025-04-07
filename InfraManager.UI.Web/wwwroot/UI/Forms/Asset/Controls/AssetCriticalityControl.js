define(['jquery', 'ajax', 'knockout'], function ($, ajaxLib, ko) {
    var module = {
        //объект jQuery, который является формой; ko-сущность объекта; функция перезагрузки объекта
        control: function (formRegion, asset, callBackFunction) {
            var self = this;

            //
            self.ReadOnly = ko.observable(false);//режим только чтение
            //
            self.CriticalityList = ko.observableArray([]);//доступные текущему пользователю переходы
            //
            var createCriticality = function (Criticality) {
                var thisObj = this;
                //
                thisObj.Text = Criticality.Name;

                //
                thisObj.CriticalityClick = function () {
                    formRegion.find('.assetCriticalityControl-menu').hide();     
                    callBackFunction(Criticality);
                };
            };
            //
            self.ajaxControl = new ajaxLib.control();
            //
            self.MenuClick = function (obj, e) {//раскрытие списка                 
                e.stopPropagation();
                //
                openRegion(formRegion.find('.assetCriticalityControl-menu'), e);
                return true;
            };
            //
            self.IsLoading = ko.observable(false);//крутилка, сигнализирует о загрузке перечня
            self.IsLoading.subscribe(function (newValue) {
                var assetCriticalityBlock = formRegion.find('.assetCriticalityBlock')[0];
                if (assetCriticalityBlock) {
                    if (newValue == true)
                        showSpinner(assetCriticalityBlock);
                    else
                        hideSpinner(assetCriticalityBlock);
                }
            });
            //
            self.fillDynamicItems = function () {
                var retval = $.Deferred();
                //
                self.ajaxControl.Ajax(null,
                    {
                        dataType: "json",
                        method: 'Get',
                        url: '/sdApi/GetCriticalityList'
                    },
                    function (newVal) {
                        if (newVal && newVal.CriticalityList) {
                            newVal.CriticalityList.forEach(function (Criticality) {
                                        var CriticalityObj = new createCriticality(Criticality);
                                    self.CriticalityList().push(CriticalityObj);
                                });
                                self.CriticalityList.valueHasMutated();
                                self.IsLoading(false);
                        }
                        retval.resolve();
                    });
                //
                return retval.promise();
            };
            //           
            self.LoadCriticalityList = function () {
                self.IsLoading(true);
                self.CriticalityList.removeAll();
                //
                if (asset() == null)
                    return;
                self.fillDynamicItems();
            };
            //
            //удаление контрола
            self.Unload = function () {
                self.ajaxControl.Abort();
                //
                self.IsLoading(false);
                //
                self.CriticalityList.removeAll();
            };
            //
            //загрузка контрола по объекту
            self.Initialize = function () {
                self.ajaxControl.Abort();
                //
                self.IsLoading(false);
                //
                self.LoadCriticalityList();
            };
        }
    }
    return module;
});