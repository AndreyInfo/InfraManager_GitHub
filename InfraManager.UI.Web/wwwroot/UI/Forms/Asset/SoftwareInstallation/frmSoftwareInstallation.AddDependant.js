define(
    ['jquery', 'knockout', 'formControl', 'ajax', 'ui_controls/ListView/ko.ListView.Helpers'],
    function ($, ko, formModule, ajax, m_helpers) {
        var module = {
            ViewModel: function (installationID) {
                var self = this;
                self.ajaxControl = new ajax.control();

                self.installationListViewID = 'installationListView_' + ko.getNewID();

                self.afterRender = function () {
                };
                self.searchText = ko.observable('');
                self.dispose = function () {
                    self.ajaxControl.Abort();
                };

                self.getSelectedItems = function () {
                    var selectedItems = self.selectListView.rowViewModel.checkedItems();
                    //
                    if (!selectedItems)
                        return [];
                    return selectedItems;
                };

                self.selectListView = null;
                self.selectListViewInit = function (listView) {
                    if (self.selectListView != null)
                        throw 'listView inited already';
                    //
                    self.selectListView = listView;
                    self.selectListView.ignoreTableHeight = true;
                    m_helpers.init(self, listView);//extend self        
                    //
                    self.selectListView.load();
                };
                self.selectRetrieveItems = function () {
                    var retvalD = $.Deferred();
                    $.when(self.getInstallationObjectList(null, true)).done(function (objectList) {
                        if (objectList) {
                            self.selectListView.showAllRows();
                        }
                        //
                        retvalD.resolve(objectList);
                    });
                    return retvalD.promise();

                };
                //
                self.getInstallationObjectList = function (idArray, showErrors) {
                    var retvalD = $.Deferred();
                    //
                    var requestInfo = {
                        StartRecordIndex: 0,
                        CountRecords: 100,
                        IDList: idArray ? idArray : [],
                        ViewName: 'SoftwareInstallation',
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request

                    };

                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/api/SoftwareInstallation'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result == 0) {
                                retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                return;
                            }
                            else if (newVal && newVal.Result === 1 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSoftwareInstallation.AddDependant.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSoftwareInstallation.AddDependant.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 3 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('AccessError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 7 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('OperationError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 9 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 11 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SqlTimeout'));
                                });
                            }
                            else if (showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSoftwareInstallation.AddDependant.js getData]', 'error');
                                });
                            }
                            //
                            retvalD.resolve([]);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            if (showErrors === true)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSoftwareInstallation.AddDependant.js, getData]', 'error');
                                });
                            //
                            retvalD.resolve([]);
                        },
                        null
                    );
                    //
                    return retvalD.promise();
                };

            },
            ShowDialog: function (callback, installationID) {
                var form;
                var viewModel = new module.ViewModel(installationID);
                var bindElement = null;

                var buttons = [
                    { text: getTextResource('Select'), click: function () { callback(viewModel.getSelectedItems()); form.Close(); } },
                    { text: getTextResource('Close'), click: function () { form.Close(); } }];
                //
                form = new formModule.control(
                    'region_frmDependantInstallation',//form region prefix
                    'setting_frmDependantInstallation',//location and size setting
                    getTextResource('SoftwareInstallation_Form_DependantInstallations'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    800, 350,//minSize
                    buttons,//form buttons
                    function () {
                        viewModel.dispose();
                        ko.cleanNode(bindElement);
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareInstallation/frmSoftwareInstallation.AddDependant\', afterRender: afterRender}"'//attributes of form region
                );
                if (!form.Initialized) {
                    return null;
                }

                form.ExtendSize(800, 350);//normal size
                bindElement = document.getElementById(form.GetRegionID());
                ko.applyBindings(viewModel, bindElement);
                //
                $.when(form.Show()).done(function (frmD) {
                    hideSpinner();
                });
            }
        };

        return module;
    });