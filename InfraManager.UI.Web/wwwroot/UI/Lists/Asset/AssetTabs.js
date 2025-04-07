define(['knockout'], function (ko) {
    var module = {
        ViewModel: function (tabs) {
            var self = this;
            self.tabList = ko.observableArray(tabs);
            self.activeTab = ko.observable(null);
            self.State = ko.observable(false);
            self.activeTab(tabs[0]);

            self.AfterRender = function () {
                
            }

            self.SetActive = function (tab, event) {
                self.activeTab(tab);                
            };
            
            self.SetActiveTabByViewName = function (viewName) {
                for (let i = 0; i < self.tabList().length; i++) {
                    if (self.tabList()[i].ViewName == viewName) {
                        self.activeTab(self.tabList()[i]);
                    }
                }
            }
            
            self.SetVisible = function (state) {                                
                self.State(state);
            }
            
        },
        tab: function (name, viewName, index) {            
            this.Name = name;
            this.ViewName = viewName;
            this.Index = index;
        }        
    }
    return module;
});
    