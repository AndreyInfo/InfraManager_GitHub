define(['knockout', 'jquery', 'parametersControl'], function (ko, $, pcLib) {
    var module = {
        TabParameter: function () {
            var self = this;
            //
            self.Name = ''; //наименование вкладки
            self.Template = '../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicence_parameterListTab';
            self.IconCSS = 'parameterListTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.Index = ko.observable(0);
            self.IsValid = ko.observable(true);//валидация группы
            self.ParameterList = ko.observableArray([]);//параметры этой группы
            self.ParameterListGroupName = ko.observable('');//наименование группы
            self.hackObject = ko.observable({});//ParameterList template look at $parent object only
            //
            //when object changed
            self.Initialize = function (obj) {                
            };
            //when tab selected
            self.load = function () {
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
            }
            //
            {//events               
                self.CanEdit_handle = vm.CanEdit.subscribe(function (newValue) {
                    if (self.parametersControl != null)
                        self.parametersControl.ReadOnly(!newValue);
                });
            }
            //
            //when object changed
            self.Initialize = function (obj) {
                self.OnParametersChanged(false);
            };
            //when tab selected
            self.load = function () {
            };
            //when tab unload
            self.dispose = function () {
                if (self.parametersControl != null)
                    self.parametersControl.DestroyControls();
                //
                if (self.parametersControl_ParameterListByGroup_handle)
                    self.parametersControl_ParameterListByGroup_handle.dispose();
                //
                self.CanEdit_handle.dispose();
            };
            //
            self.InitializeParameters = function (recalculateParameters) {
                var obj = vm.object();
                if (!obj)
                    self.parametersControl.InitializeOrCreate(null, null, null, false);//нет объекта - нет параметров
                else
                    self.parametersControl.InitializeOrCreate(obj.ClassID(), obj.ID(), obj, recalculateParameters);
            };
            self.OnParametersChanged = function (recalculateParameters) {//обновления списка параметров по объекту
                if (self.parametersControl == null) {
                    self.parametersControl = new pcLib.control();
                    self.parametersControl.ClientID(null);//от клиента есть зависимые параметры
                    self.parametersControl.ReadOnly(!vm.CanEdit());
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
                        for (var i = newValue.length - 1; i >= 0; i--) {
                            var tab = newValue[i];
                            var tabParameter = new module.TabParameter();
                            tabParameter.Index(i + 1);
                            tabParameter.Name = tab.GroupName;
                            tabParameter.ParameterListGroupName(tab.GroupName);
                            tabParameter.IsValid = tab.IsValid;
                            tabParameter.ParameterList(tab.ParameterList);
                            //
                            self.tabParameters.push(tabParameter);
                            vm.tabList().splice(1, 0, tabParameter);//insert after general tab
                        } 
                        vm.tabList.valueHasMutated();
                    });
                }
                self.InitializeParameters(recalculateParameters);
            };
        }
    };
    return module;
});