define(['knockout',
    'jquery',
    'ajax',
    'formControl'
],
    function (ko,
        $,
        ajaxLib,
        fc
    ) {

        var module = {
            ViewModel: function ($region) {
                var self = this;
                self.$region = $region;
                self.$isDone = $.Deferred();//resolve, когда операция выполнена
                self.callbackFunc = null;
                self.ELPVendorAny = ko.observable('0');

                //
                self.hasUser = ko.observable(false);
                self.lvSearchText = ko.observable('');
                //
                self.SizeChanged = function () {
                    if (self.tabActive && self.tabActive() && self.tabActive().lv)
                        self.tabActive().lv.renderTable();
                };
                //
                {//
                    self.lvSearchText.extend({ rateLimit: { timeout: 500, method: "notifyWhenChangesStop" } });
                    self.lvSearchText_handle = self.lvSearchText.subscribe(function (newValue) {
                        self.reload();
                    });
                    //
                    self.eraseTextClick = function () {
                        self.lvSearchText('');
                    };
                    self.isSearchTextEmpty = ko.computed(function () {
                        var text = self.lvSearchText();
                        if (!text)
                            return true;
                        //
                        return false;
                    });
                    //Report
                    {
                        self.printReport = ko.observable(false);
                    }

                    {//geting data             
                        self.ajaxControl = new ajaxLib.control();
                        self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                            var retvalD = $.Deferred();
                            //
                            var requestInfo = {
                                Skip: startRecordIndex,
                                Take: countOfRecords,
                                Name: self.lvSearchText(),
                                IsSoftware: true
                            };

                            let url = '/bff/ProductCatalog/Manufacturers';
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    data: requestInfo,
                                    url: url
                                },
                                function (newVal) {
                                    retvalD.resolve(newVal);
                                },
                                function (XMLHttpRequest, textStatus, errorThrown) {
                                    if (showErrors === true)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmELPTask_ChooseVendor.js getObjectList]', 'error');
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

                    {//selection
                        self.selectedItemFreeze = false;
                        self.selectedItems = ko.observableArray([]);
                        self.getItemsInfos = function (items) {
                            var retval = [];
                            items.forEach(function (item) {
                                retval.push({
                                    ClassID: item.ClassID,
                                    ID: item.ID.toUpperCase(),
                                });
                            });
                            return retval;
                        };

                    }
                    //filter changed
                    self.reload = function () {
                        self.lv.load();
                    };
                }

                var canEdit = ko.observable(true);
                //
                self.Load = function () {
                };


                self.ajaxControl = new ajaxLib.control();
                self.dispose = function () {
                    //
                    self.lvSearchText_handle.dispose();
                };
                self.afterRender = function (editor, elements) {

                };

                function getResourceName() {
                    return getTextResource('ELPTaskVendor_Select');
                }

                self.FormName = ko.pureComputed(function () {
                    return getResourceName();
                });

                self.caption = ko.pureComputed(function () {
                    return self.FormName();
                });

                self.lvInit = function (listView) {
                    self.lv = listView;
                    self.lv_checkedItemsChanged_handle = listView.rowViewModel.checkedItemsToSubscribe.subscribe(function (newObjectList) {
                        self.selectedItems(newObjectList);
                        self.ShowButtons();
                    });
                        //
                    //
                    var storedLoad = self.lv.load;
                    //self.lv.load = function () {
                    //    var retvalD = $.Deferred();
                    //    self.selectedItemFreeze = true;
                    //    $.when(storedLoad()).done(function () {
                    //        self.selectedItemFreeze = false;
                    //        retvalD.resolve();
                    //    });
                    //    return retvalD.promise();
                    //};
                    //
                    self.lv.load();
                };
                self.lvRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
                    var retvalD = $.Deferred();
                    $.when(self.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                        $.when(retvalD.resolve(objectList)).done(function () { self.lv.showAllRows(); });
                        
                    });
                    return retvalD.promise();
                };
                self.lvRowClick = function (obj) {

                };

                self.ShowButtons = function () {
                    var buttons = [];
                    var bCancel = {
                        text: getTextResource('ButtonCancel'),
                        click: function () {
                            self.selectedItemFreeze = true;
                            self.frm.Close();
                        }
                    };
                    buttons.push(bCancel);

                    if (self.ELPVendorAny() == '1' || (self.ELPVendorAny() == '0' && self.selectedItems().length == 1)) {
                        var bSelect = {
                            text: getTextResource('ELPVendor_ReadyButton'),
                            click: function () {
                                self.selectedItemFreeze = true;
                                if (self.callbackFunc) {
                                    var result = null;
                                    var label = '';
                                    if (self.ELPVendorAny() == '0' && self.selectedItems().length == 1) {
                                        result = self.selectedItems()[0].ID;
                                        label = self.selectedItems()[0].Name;
                                    }
                                    self.callbackFunc({ ID: result, Label: label });
                                }
                                self.frm.Close();
                            }
                        };
                        buttons.push(bSelect);
                    }
                    self.frm.UpdateButtons(buttons);
                };

            },
            ShowDialog: function (resultHandler) {
                showSpinner();
                //
                var $retval = $.Deferred();
                var bindElement = null;
                //
                $.when(userD).done(function (user) {
                    var forceClose = false;
                    //
                    var frm = undefined;
                    var vm = undefined;
                    //
                    var buttons = [];
                    var bCancel = {
                        text: getTextResource('ButtonCancel'),
                        click: function () {
                            frm.Close();
                        }
                    };
                    buttons.push(bCancel);
                    //
                    frm = new fc.control(
                        'region_frmELPTaskChooseVendor',//form region prefix
                        'setting_frmELPTaskChooseVendor',//location and size setting
                        getTextResource('ELPTaskVendor_Select'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        700, 700,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Settings/ELP/frmELPTask_ChooseVendor\'}"'//attributes of form region
                    );
                    //
                    if (!frm.Initialized)
                        return;//form with that region and settingsName was open
                    //                
                    var $region = $('#' + frm.GetRegionID());
                    vm = new module.ViewModel($region);
                    vm.$isDone = $retval;
                    vm.callbackFunc = resultHandler;
                    //
                    vm.frm = frm;
                    frm.SizeChanged = function () {
                        var width = frm.GetInnerWidth();
                        var height = frm.GetInnerHeight();
                        //
                        vm.$region.css('width', width + 'px').css('height', height + 'px');
                        vm.SizeChanged();
                    };
                    //
                    ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                    $.when(frm.Show(), vm.LoadD).done(function (frmD, loadD) {
                        if (loadD == false) {//force close
                            frm.Close();
                        } else {
                            if (!ko.components.isRegistered(module.CaptionComponentName))
                                ko.components.register(module.CaptionComponentName, {
                                    template: '<span data-bind="text: $str"/>'
                                });
                            frm.BindCaption(vm, "component: {name: '" + module.CaptionComponentName + "', params: { $str: caption } }");
                        }
                        hideSpinner();
                    });


                });
                //
                return $retval.promise();
            }
        };
        return module;
    });