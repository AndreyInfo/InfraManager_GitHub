define(['knockout', 'jquery', 'ajax', 'sweetAlert', 'ui_controls/ListView/ko.ListView.Helpers'], function (ko, $, ajax, swal, m_helpers) {
    var module = {
        Tab: function (vm) {

            var self = this;

            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('SoftwareInstallation_Form_DependantTabName');

            self.Template = '../UI/Forms/Asset/SoftwareInstallation/frmSoftwareInstallation_dependantsTab';

            self.IconCSS = 'dependantsTab';
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
            //
            self.dependantListView = null;
            self.dependantListViewID = 'dependantListView_' + ko.getNewID();
            self.dependantListViewInit = function (listView) {
                if (self.dependantListView != null)
                    throw 'listView inited already';
                //
                self.dependantListView = listView;
                self.dependantListView.ignoreTableHeight = true;
                m_helpers.init(self, listView);//extend self        
                //
                self.dependantListView.load();
            };
            //
            self.dependantRetrieveItems = function () {
                var retvalD = $.Deferred();
                $.when(self.getDependantObjectList(null, true)).done(function (objectList) {
                    if (objectList) {

                        var newDependants = [];
                        var object = vm.object();
                        ko.utils.arrayForEach(objectList, function (row) {
                            var loc = { ID: row.ID };
                            newDependants.push(loc);
                        });
                        object.DependantInstallations(newDependants);
                    }
                    //
                    $.when(retvalD.resolve(objectList)).done(function () { self.dependantListView.showAllRows(); });
                });
                return retvalD.promise();

            };
            //
            self.getDependantObjectList = function (idArray, showErrors) {
                var retvalD = $.Deferred();
                //
                var requestInfo = {
                    StartRecordIndex: 0,
                    CountRecords: 100,
                    IDList: idArray ? idArray : [],
                    ViewName: 'SoftwareInstallation',
                    TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                    ParentID: vm.object().ID(),
                    ParentClassID: vm.object().ClassID,

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
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSoftwareInstallation_dependantsTab.js getData]', 'error');
                            });
                        }
                        else if (newVal && newVal.Result === 2 && showErrors === true) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSoftwareInstallation_dependantsTab.js getData]', 'error');
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
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSoftwareInstallation_dependantsTab.js getData]', 'error');
                            });
                        }
                        //
                        retvalD.resolve([]);
                    },
                    function (XMLHttpRequest, textStatus, errorThrown) {
                        if (showErrors === true)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSoftwareInstallation_dependantsTab.js, getData]', 'error');
                            });
                        //
                        retvalD.resolve([]);
                    },
                    null
                );
                //
                return retvalD.promise();
            };
            //
            self.dependantContextMenu = ko.observable(null);
            self.contextMenuInit = function (contextMenu) {
                self.dependantContextMenu(contextMenu);//bind contextMenu

                self.dependantContextMenuAdd(contextMenu);
                self.dependantContextMenuDelete(contextMenu);
            };
            //
            self.contextMenuOpening = function (contextMenu) {
                contextMenu.items().forEach(function (item) {
                    if (item.isEnable && item.isVisible) {
                        item.enabled(item.isEnable());
                        item.visible(item.isVisible());
                    }
                });
            };
            self.DependantInstallationAdd = function () {
                if (!vm.CanEdit())
                    return;
                //
                showSpinner();
                //var asset = self.asset();
                //
                require(['ui_forms/Asset/SoftwareInstallation/frmSoftwareInstallation.AddDependant'], function (module) {
                    module.ShowDialog(self.saveDependantInstallations, true);
                });

            };
            self.Delete = function (ids) {
                var retval = $.Deferred();
                //
                var data = {
                    'IDList': ids,
                };
                var object = vm.object();
                var url = vm.baseUrl + '/' + object.ID() + '/delete-dependant'

                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                {
                    url: url,
                    method: 'POST',
                    dataType: 'json',
                    data: data,
                    headers: { 'x-device-fingerprint': object.fingerprintJs.fHash }
                },
                function (response) {//AddSoftwareLicenceOutModel
                    hideSpinner();

                    if (response) {
                        if (!response.Success)
                            require(['sweetAlert'], function () {
                                swal(response.Fault);//some problem
                            });
                        //
                        if (response.Success) {//ok 
                            self.dependantListView.load();
                            retval.resolve(response.Result);
                            return;
                        }
                    }
                    retval.resolve(null);
                },
                function (response) {
                    hideSpinner();
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareInstallation.js, add]', 'error');
                    });
                    retval.resolve(null);
                });
                return retval.promise();
            };
            self.DependantInstallationDelete = function () {
                if (!vm.CanEdit())
                    return;
                var retval = $.Deferred();

                var ids = self.dependantGetCheckedItems();
                if (ids.length >= 1) {
                    require(['sweetAlert'], function (swal) {
                        swal({
                            title: getTextResource('Removing'),
                            text: getTextResource('ConfirmRemoveQuestion'),
                            showCancelButton: true,
                            closeOnConfirm: false,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                            function (value) {
                                swal.close();
                                //
                                if (value == true) {
                                    clearTimeout(self.timeotDelete);
                                    self.timeotDelete = setTimeout(self.Delete, 100, ids);
                                }
                            });
                    });

                }

                //
                return retval.promise();

            };
            self.saveDependantInstallations = function (addedList) {
                if (!vm.CanEdit())
                    return;
                var retval = $.Deferred();
                var ids = [];
                addedList.forEach(function (el) {
                    ids.push(el.ID);
                });

                var object = vm.object();
                var url = vm.baseUrl + '/' + object.ID() + '/add-dependant'
                //             
                var data = {
                    'IDList': ids,
                };

                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: url,
                        method: 'POST',
                        dataType: 'json',
                        data: data,
                        headers: { 'x-device-fingerprint': object.fingerprintJs.fHash }
                    },
                    function (response) {//AddSoftwareLicenceOutModel
                        hideSpinner();

                        if (response) {
                            if (!response.Success)
                                require(['sweetAlert'], function () {
                                    swal(response.Fault);//some problem
                                });
                            //
                            if (response.Success) {//ok
                                self.dependantListView.load();
                                retval.resolve(response.Result);
                                return;
                            }
                        }
                        retval.resolve(null);
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareInstallation.js, add]', 'error');
                        });
                        retval.resolve(null);
                    });

                //
                return retval.promise();
            };

            self.dependantGetCheckedItems = function () {
                var selectedItems = self.dependantListView.rowViewModel.checkedItems();
                //
                if (!selectedItems)
                    return [];
                //
                var retval = [];
                selectedItems.forEach(function (el) {
                    retval.push(el.ID);
                });
                return retval;
            };

            self.dependantContextMenuAdd = function (contextMenu) {
                var isVisible = function () {
                    return vm.CanEdit() && vm.object() != null;
                };
                var action = function () {
                    self.DependantInstallationAdd();
                };
                //
                var cmd = contextMenu.addContextMenuItem();
                cmd.restext('SoftwareInstallation_Dependant_MenuAdd');
                cmd.isEnable = function () { return true; };
                cmd.isVisible = isVisible;
                cmd.click(action);
            };
            //
            self.dependantContextMenuDelete = function (contextMenu) {
                var isEnable = function () {
                    return self.dependantGetCheckedItems().length >= 1; //self.operationIsGranted(461);
                };
                var isVisible = function () {
                    return vm.CanEdit() && vm.object() != null;
                };
                var action = function () {
                    self.DependantInstallationDelete();
                };
                //
                var cmd = contextMenu.addContextMenuItem();
                cmd.restext('SoftwareInstallation_Dependant_MenuDelete');
                cmd.isEnable = isEnable;
                cmd.isVisible = isVisible;
                cmd.click(action);
            };

        }
    };
    return module;
});