define(['knockout', 'jquery', 'parametersControl'], function (ko, $, pcLib) {
    var module = {
        TabParameter: function () {
            var self = this;
            //
            self.Name = '';//наименование вкладки
            self.Template = '../UI/Forms/Asset/frmAssetRegistration_parameterListTab';
            self.IconCSS = 'parameterListTab';
            //
            self.Index = ko.observable(0);
            self.IsValid = ko.observable(true);//валидация группы
            self.ParameterList = ko.observableArray([]);//параметры этой группы
            self.ParameterListGroupName = ko.observable('');//наименовае группы
            //
            //when object changed
            self.init = function (obj) {
            };
            //when tab selected
            self.load = function () {
            };
            //when tab validating
            self.validate = function () {
            };
            //when tab unload
            self.dispose = function () {
            };
        },

        Tab: function (vm) {
            var self = this;
            //
            {//variables
                self.parametersControl = null;
                self.tabParameters = [];//готовые вкладки с готовыми параметрами
                self.object_productCatalogID_handle = null;
            }
            //
            //when object changed
            self.init = function (obj) {
                if (self.object_productCatalogID_handle)
                    self.object_productCatalogID_handle.dispose();
                //
                self.OnParametersChanged();
                if (obj)
                    self.object_productCatalogID_handle = obj.productCatalogID.subscribe(function (newValue) {
                        self.OnParametersChanged();
                    });
            };
            //when tab selected
            self.load = function () {
            };
            //when tab validating
            self.validate = function () {
                if (self.parametersControl == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ParametersNotLoaded'));
                    });
                    return false;
                }
                else if (!self.parametersControl.Validate())
                    return false;
                else {
                    vm.object().parameterValueList(self.parametersControl.GetParameterValueList());
                    return true;
                }
            };
            //when tab unload
            self.dispose = function () {
                if (self.parametersControl != null)
                    self.parametersControl.DestroyControls();
                //
                if (self.parametersControl_ParameterListByGroup_handle)
                    self.parametersControl_ParameterListByGroup_handle.dispose();
                //
                if (self.object_productCatalogID_handle)
                    self.object_productCatalogID_handle.dispose();
            };
            //
            //
            self.InitializeParameters = function () {
                var obj = vm.object();
                if (!obj)
                    self.parametersControl.InitializeOrCreate(null, null, null, false);//нет объекта - нет параметров
                else
                    self.parametersControl.Create(obj.ClassID(), obj.productCatalogID(), false, null);
            };
            self.OnParametersChanged = function () {//обновления списка параметров по объекту
                if (self.parametersControl == null) {
                    self.parametersControl = new pcLib.control();
                    self.parametersControl.ClientID(null);//от клинета есть зависимые параметры
                    self.parametersControl.ReadOnly(false);
                    self.parametersControl_ParameterListByGroup_handle = self.parametersControl.ParameterListByGroup.subscribe(function (newValue) {//изменилась разбивка параметров по группам
                        {//clear tabs                                
                            for (var i = 0; i < self.tabParameters.length; i++) {
                                var tabParameter = self.tabParameters[i];
                                var index = vm.tabList().indexOf(tabParameter);
                                if (vm.tabActive() === tabParameter)//parameter tab
                                    vm.tabActive(vm.tabList()[0]);//select first tab
                                vm.tabList().splice(index, 1);
                            }
                            self.tabParameters.splice(0, self.tabParameters.length);
                        }
                        //create tabs
                        for (var i = 0; i < newValue.length; i++) {
                            var tab = newValue[i];
                            var tabParameter = new module.TabParameter();
                            tabParameter.Index(i + 1);
                            tabParameter.Name = tab.GroupName;
                            tabParameter.ParameterListGroupName(tab.GroupName);
                            tabParameter.IsValid = tab.IsValid;
                            tabParameter.ParameterList(tab.ParameterList);
                            //
                            self.tabParameters.push(tabParameter);
                            vm.tabList().splice(vm.tabList().length - 1, 0, tabParameter);//insert after general tab
                        }
                        vm.tabList.valueHasMutated();
                    });
                }
                self.InitializeParameters();
            };
        }
    };
    return module;
});