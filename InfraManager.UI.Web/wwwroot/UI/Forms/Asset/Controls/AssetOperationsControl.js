define(['jquery', 'ajax', 'knockout'], function ($, ajaxLib, ko) {
    var module = {
        //объект jQuery, который является формой; ko-сущность объекта; функция перезагрузки объекта
        control: function (formRegion, asset, asset_initializeFunc) {            
            var self = this;                        
            //
            self.ReadOnly = ko.observable(false);//режим только чтение
            //
            self.OperationList = ko.observableArray([]);//доступные текущему пользователю переходы
            //
            var createOperation = function (lifeCycleOperation, executeOperation) {
                var thisObj = this;
                //
                thisObj.Enabled = ko.observable(lifeCycleOperation.Enabled);
                thisObj.Text = lifeCycleOperation.Name;

                //
                thisObj.OperationClick = function () {
                    formRegion.find('.assetOperationsControl-menu').hide();
                    $.when(executeOperation(lifeCycleOperation, self.getLifeCycleObject())).done(function () {                        
                        asset_initializeFunc(asset().ID(), asset().ClassID());                       
                    });
                };
            };
            //
            self.ajaxControl = new ajaxLib.control();
            //
            self.MenuClick = function (obj, e) {//раскрытие списка состояний                
                e.stopPropagation();
                //
                
                openRegion(formRegion.find('.assetOperationsControl-menu'), e);
                
                return true;
            };
            //
            self.IsLoading = ko.observable(false);//крутилка над состоянием, сигнализирует о загрузке перечня состояний
            self.IsLoading.subscribe(function (newValue) {
                var assetOperationsBlock = formRegion.find('.assetOperationsBlock')[0];
                if (assetOperationsBlock) {
                    if (newValue == true)
                        showSpinner(assetOperationsBlock);
                    else
                        hideSpinner(assetOperationsBlock);
                }
            });
            //
            self.getLifeCycleObject = function () {
                var lifeCycleObjectList = [];
                var classID = asset().ClassID();
                var name = '';
                if (classID == 5 || classID == 6)
                    name = asset().FullName();            
                
                lifeCycleObjectList.push({ ClassID: asset().ClassID(), ID: asset().ID(), Name: name, ProductCatalogTemplate: asset().ProductCatalogTemplate() });
                
                return lifeCycleObjectList;
            };
            //
            self.fillDynamicItems = function () {
                var retval = $.Deferred();

                //
                self.ajaxControl.Ajax(null,
                    {
                        dataType: "json",
                        method: 'POST',
                        data: { DeviceList: self.getLifeCycleObject() },
                        url: '/sdApi/GetContextMenu'
                    },
                    function (newVal) {
                        if (newVal && newVal.List) {
                            require(['assetOperationsHelper'], function (module) {
                                newVal.List.forEach(function (lifeCycleOperation) {
                                    if (lifeCycleOperation.Enabled) {
                                        if (functionsAvailability.SoftwareDistributionCentres && (lifeCycleOperation.CommandType == 12 || lifeCycleOperation.CommandType ==13)) {
                                            return;
                                        }
                                        var state = new createOperation(lifeCycleOperation, module.executeLifeCycleOperation);
                                        self.OperationList().push(state);
                                    }
                                });
                                self.OperationList.valueHasMutated();
                                self.IsLoading(false);                                
                            });
                        }
                        retval.resolve();
                    });
                //
                return retval.promise();
            };
            //           
            self.LoadOperationList = function () {
                self.IsLoading(true);
                self.OperationList.removeAll();
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
                self.OperationList.removeAll();
            };
            //
            //загрузка контрола по объекту
            self.Initialize = function () {
                self.ajaxControl.Abort();
                //
                self.IsLoading(false);
                //
                self.LoadOperationList();
            };
        }
    }
    return module;
});