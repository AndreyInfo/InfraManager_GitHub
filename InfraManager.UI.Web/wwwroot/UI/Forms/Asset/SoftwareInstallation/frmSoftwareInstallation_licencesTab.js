define(['knockout', 'jquery', 'ajax', 'sweetAlert', 'ui_controls/ListView/ko.ListView.Helpers'], function (ko, $, ajax, swal, m_helpers) {
    var module = {
        Tab: function (vm) {

            var self = this;

            self.ajaxControl = new ajax.control();
            //
            self.Name = 'Лицензии';// getTextResource('SoftwareInstallation_Form_GeneralTabName');

            self.Template = '../UI/Forms/Asset/SoftwareInstallation/frmSoftwareInstallation_licencesTab';

            self.IconCSS = 'licencesTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.$region = vm.$region;
          
            //when object changed
            self.Initialize = function (obj) {
            };
            //when tab selected
            self.load = function () {
            };

            self.AfterRender = function () {
            }

            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
            };         
            self.licencesListView = null;
            self.licencesListViewID = 'licencesListView_' + ko.getNewID();
            self.licencesListViewInit = function (listView) {
                if (self.licencesListView != null)
                    throw 'listView inited already';
                //
                self.licencesListView = listView;
                self.licencesListView.ignoreTableHeight = true;
                m_helpers.init(self, listView);//extend self        
                //
                self.licencesListView.load();
            };
            //
            self.licencesRetrieveItems = function () {
                var retvalD = $.Deferred();
                $.when(self.getLicencesObjectList(null, true)).done(function (objectList) {
                    if (objectList) {
                    }
                    //
                    $.when(retvalD.resolve(objectList)).done(function () { self.licencesListView.showAllRows(); });
                });
                return retvalD.promise();

            };
            //
            self.getLicencesObjectList = function (idArray, showErrors) {
                var retvalD = $.Deferred();
                //
                var requestInfo = {
                    StartRecordIndex: 0,
                    CountRecords: 100,
                    IDList: idArray ? idArray : [],
                    ViewName: 'SoftwareInstallationLicences',
                    TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                    ParentID: vm.object().ID(),
                    ParentClassID: vm.object().ClassID,

                };

                self.ajaxControl.Ajax(null,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: requestInfo,
                        url: '/api/SoftwareInstallation/' + vm.object().ID() +'/licence-reference'
                    },
                    function (newVal) {
                        if (newVal && newVal.Success) {
                            retvalD.resolve(newVal.Result);//can be null, if server canceled request, because it has a new request                               
                            return;
                        }
                        else if (newVal && newVal.Fault === 1 && showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSoftwareInstallation_licencesTab.js getData]', 'error');
                            });
                        }
                        else if (newVal && newVal.Fault === 2 && showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSoftwareInstallation_licencesTab.js getData]', 'error');
                            });
                        }
                        else if (newVal && newVal.Fault === 3 && showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('AccessError_Table'));
                            });
                        }
                        else if (newVal && newVal.Fault === 7 && showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('OperationError_Table'));
                            });
                        }
                        else if (newVal && newVal.Fault === 9 && showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                            });
                        }
                        else if (newVal && newVal.Fault === 11 && showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SqlTimeout'));
                            });
                        }
                        else if (showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSoftwareInstallation_licencesTab.js getData]', 'error');
                            });
                        }
                        //
                        retvalD.resolve([]);
                    },
                    function (XMLHttpRequest, textStatus, errorThrown) {
                        if (showErrors === true)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSoftwareInstallation_licencesTab.js, getData]', 'error');
                            });
                        //
                        retvalD.resolve([]);
                    },
                    null
                );
                //
                return retvalD.promise();
            };

        }
    };
    return module;
});