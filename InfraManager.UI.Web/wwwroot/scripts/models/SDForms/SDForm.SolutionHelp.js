define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        ViewModel: function (modelForm, ko_object, objectClassID, ajaxSelector) {
            var self = this;
            self.FormModel = modelForm;
            //
            self.solModel = ko.observable(null);
            self.kbModel = ko.observable(null);
            //
            self.IsVisible = ko.observable(true);
            //
            self.Load = function () {
                self.solModel(new module.SolutionPanel(self, ko_object, objectClassID, $(ajaxSelector).find('.help_panel-solutions')));
                self.kbModel(new module.KBPanel(self, ko_object, objectClassID, $(ajaxSelector).find('.help_panel-kb')));
                //
                self.solModel().LoadFullList();
                if (objectClassID == '701') //call
                {
                    self.kbModel().searchPhrase(ko_object().CallSummaryName());
                }
                else if (objectClassID == '702') //problem
                {
                    self.kbModel().searchPhrase(ko_object().Summary());
                }
                else if (objectClassID == '823') //massIncident
                {
                    self.kbModel().searchPhrase(ko_object().name());
                }
            };
        },
        KBPanel: function (parentModel, ko_object, objectClassID, $region) {
            var self = this;
            var parentSelf = parentModel;
            //
            self.findedList = ko.observableArray([]);
            self.searchPhrase = ko.observable('');
            self.searchPhrase.subscribe(function (newValue) {
                if (newValue && newValue.length > 0)
                    self.InitSearch(newValue);
                else {
                    self.findedList.removeAll();
                    self.findedList.valueHasMutated();
                }
            });
            //
            self.AddAndInclude = function (model, event) {
                $.when(parentSelf.FormModel.kbaRefList.AddKBAReference(model)).done(function (result) {
                    parentSelf.FormModel.AddTextToSolution(model.HTMLSolution);                    
                });                
            };
            self.IncludeInObject = function (model, event) {
                parentSelf.FormModel.kbaRefList.AddKBAReference(model);
            };
            self.ShowArticle = function (model, event) {
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowKBAView(model.ID);
                });
            };
            //
            self.ajaxControlSearch = new ajaxLib.control();
            self.syncTimeout = null;
            self.syncD = null;
            self.InitSearch = function (phrase) {
                var d = self.syncD;
                if (d == null || d.state() == 'resolved') {
                    d = $.Deferred();
                    self.syncD = d;
                }
                //
                if (self.syncTimeout)
                    clearTimeout(self.syncTimeout);
                self.syncTimeout = setTimeout(function () {
                    if (d == self.syncD && phrase != '') {
                        $.when(self.Search(phrase)).done(function () {
                            d.resolve();
                        });
                    }
                }, 1000);
                //
                return d.promise();
            };
            self.Search = function (phrase) {
                var serviceItemAttendanceID = null;//только для заявки
                if (ko_object && ko_object().ServiceItemID && ko_object().ServiceItemID())
                    serviceItemAttendanceID = ko_object().ServiceItemID();
                else if (ko_object && ko_object().ServiceAttendanceID && ko_object().ServiceAttendanceID())
                    serviceItemAttendanceID = ko_object().ServiceAttendanceID();
                //
                self.ajaxControlSearch.Ajax($region,
                    {
                        url: '/api/kb/searchInfo',
                        method: 'GET',
                        data: {
                            text: phrase,
                            clientRegistration: false,
                            serviceItemAttendanceID: serviceItemAttendanceID
                        }
                    },
                    function (response) {
                        if (response) {
                            self.findedList.removeAll();
                            response.forEach(function (el) {
                                self.findedList().push(new module.KBArticle(self, el));
                            });
                            self.findedList.valueHasMutated();
                        }
                        else require(['sweetAlert'], function () {
                            swal(getTextResource('SearchServiceNotWork'), getTextResource('AjaxError') + '\n[KBPanel, Search]', 'warning');
                        });
                    });
            };            
        },
        SolutionPanel: function (parentModel, ko_object, objectClassID, $region) {
            var self = this;
            var parentSelf = parentModel;
            //
            self.searchPhrase = ko.observable('');
            self.searchPhrase.subscribe(function (newValue) {
                if (newValue && newValue.length > 0) {
                    self.InitSearch(newValue);
                }
                else {
                    self.findedList.removeAll();
                    self.findedList.valueHasMutated();
                }
            });
            //
            self.fullList = ko.observableArray([]);
            self.findedList = ko.observableArray([]);
            //
            self.listToShow = ko.computed(function () {
                if (self.searchPhrase() && self.searchPhrase() != '')
                    return self.findedList();
                else return self.fullList();
            });
            //
            self.ajaxControlLoad = new ajaxLib.control();
            self.LoadFullList = function () {
                var callback =  function (response) {
                    if (response) {
                        var cList = response;
                        if (cList) {
                            self.fullList.removeAll();
                            cList.forEach(function (el) {
                                self.fullList().push(new module.SolutionData(self, el));
                            });
                            self.fullList.valueHasMutated();
                        }
                        else {
                            self.ShowAjaxError();
                        }
                    }
                };
                var criteria = '';
                self.InvokeSolutionSearcher(criteria, callback);
            };
            
            self.ShowAjaxError = function () {
                require(['sweetAlert'], function () {
                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SolutionPanel, LoadFullList]', 'error');
                });
            };
            self.ShowErrorByCode = function (responseResult) {
                switch (responseResult) {
                    case 1:
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[SolutionPanel, LoadFullList]', 'error');
                        });
                        break;
                    case 2:
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[SolutionPanel, LoadFullList]', 'error');
                        });
                        break;
                    case 3:
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                        });
                        break;
                    default:
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[SolutionPanel, LoadFullList]', 'error');
                        });
                }
            };
            //
            self.AddToSolution = function (model, event) {
                parentSelf.FormModel.AddTextToSolution(model.Description);
            };
            //
            self.ShowSolution = function (model, event) {
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowTextViewer(model.Name, model.Description, true);
                });
            };
            //
            self.ajaxControlSearch = new ajaxLib.control();
            self.syncTimeout = null;
            self.syncD = null;
            self.InitSearch = function (phrase) {
                var d = self.syncD;
                if (d == null || d.state() == 'resolved') {
                    d = $.Deferred();
                    self.syncD = d;
                }
                //
                if (self.syncTimeout)
                    clearTimeout(self.syncTimeout);
                self.syncTimeout = setTimeout(function () {
                    if (d == self.syncD && phrase != '') {
                        $.when(self.Search(phrase)).done(function () {
                            d.resolve();
                        });
                    }
                }, 1000);
                //
                return d.promise();
            };
            self.InvokeSolutionSearcher = function (criteriaText, callbackFunction) {
                var searchCriteria = { Text: criteriaText.trim() };
                self.ajaxControlSearch.Ajax($region,
                    {
                        url: '/api/searchrequests/SolutionSearcher',
                        method: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({ Content: JSON.stringify(searchCriteria) })
                    },
                    (response) => callbackFunction(response) 
                    );
            };
            self.Search = function (phrase) {
                var callback = function (response) {
                    if (response) {
                        self.findedList.removeAll();
                        response.forEach(function (el) {
                            self.findedList().push(new module.SolutionData(self, el));
                        });
                        self.findedList.valueHasMutated();
                    }
                    else require(['sweetAlert'], function () {
                        swal(getTextResource('SearchServiceNotWork'), getTextResource('AjaxError') + '\n[KBPanel, Search]', 'warning');});
                };
                self.InvokeSolutionSearcher(phrase, callback);
            };
        },
        KBArticle: function (parent, data) { //дублирует сущность из регистрации, но пока не стал выводить, там слишком больше специализированных обработчиков
            var kself = this;
            //
            var parentSelf = parent;
            kself.ID = data.ID;
            kself.Name = data.Name;
            kself.TagString = data.TagString;
            kself.Description = data.Description;
            kself.HTMLDescription = data.HTMLDescription;
            kself.HTMLSolution = data.HTMLSolution;
        },
        SolutionData: function (parent, data) {
            var dself = this;
            //
            var parentSelf = parent;
            dself.ID = data.ID;
            dself.Description = data.Details;
            dself.Name = data.FullName;
        }
    };
    return module;
});