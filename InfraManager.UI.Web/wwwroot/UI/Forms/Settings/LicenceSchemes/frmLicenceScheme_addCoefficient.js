define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    './frmLicenceScheme_addCoeff_tabProduct'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        tab_product
    ) {

        var module = {
            ViewModel: function ($region) {
                var self = this;
                self.$region = $region;
                self.$isDone = $.Deferred();//resolve, когда операция выполнена
                self.callbackFunc = null;
                //
                self.mode = ko.observable();
                self.treeMode = ko.observable();
                self.modes = {
                    product: 'product',
                };
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
                                Type: 'ProductCatalogue',
                                AvailableClassID: [29, 374, 378],
                                startRecordIndex: startRecordIndex,
                                countRecords: countOfRecords,
                                ViewName: 'ProcessorModelSimpleEditable',
                                SearchRequest: self.lvSearchText(),
                                ProductCatalogID: self.navigatorObjectID(),
                                ProductCatalogClassID: self.navigatorObjectClassID(),
                                TemplateID: 331,
                                hasLifeCycle: true,
                            };

                            let url = '/licence-scheme/processors-for-select';
                            self.ajaxControl.Ajax(null,
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    data: requestInfo,
                                    url: url
                                },
                                function (newVal) {
                                    if (newVal && newVal.Result === 0) {
                                        retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                        return;
                                    }
                                    else if (newVal && newVal.Result === 1 && showErrors === true) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmLicenceScheme_addCoefficient.js getObjectList]', 'error');
                                        });
                                    }
                                    else if (newVal && newVal.Result === 2 && showErrors === true) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmLicenceScheme_addCoefficient.js getObjectList]', 'error');
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
                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLicenceScheme_addCoefficient.js getObjectList]', 'error');
                                        });
                                    }
                                    //
                                    retvalD.resolve([]);
                                },
                                function (XMLHttpRequest, textStatus, errorThrown) {
                                    if (showErrors === true)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLicenceScheme_addCoefficient.js getObjectList]', 'error');
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

                        self.getReferenceCount = ko.pureComputed(function () {
                            let count = self.getReferenceList().map(function (item) {
                                return parseInt(item["SoftwareExecutionCount"], 10);
                            }).reduce(function (accumulator, currentValue, index, array) {
                                return accumulator + currentValue;
                            }, 0);

                            if (isNaN(count) && self.getReferenceList().length === 0)
                                count = 0;

                            if (isNaN(count) && self.getReferenceList().length > 0)
                                count = 1;

                            return count;
                        });

                        self.markListViewSelection = function () {
                            for (var i = 0; i < self.selectedItems().length; i++) {
                                var row = self.tabActive().lv.rowViewModel.getRowByObjectID(self.selectedItems()[i].ID);
                                if (row != null && row.checked() == false)
                                    row.checked(true);
                            }
                        };
                    }
                    //filter changed
                    self.reload = function () {
                        if (self.tabActive != null) {
                            self.tabActive().lv.load();
                        }
                    };
                }

                {//tabs                          

                    let arrayTabs = [];

                    arrayTabs =
                        [
                            new tab_product.Tab(self)      //  0
                        ];

                    self.tabList = arrayTabs;
                    //
                    self.tabActive = ko.observable(null);
                    self.tabActive_handle = self.tabActive.subscribe(function (selectedTab) {
                        if (selectedTab.hasOwnProperty('load')) {
                            selectedTab.load();
                        }

                    });
                    //
                    self.selectTabClick = function (tab) {
                        self.tabActive(tab);
                    };

                    self.productClick = function () {
                        self.selectTabClick(self.tabList[0]);
                        self.mode(self.modes.product);
                        self.treeMode(0);
                    };
                    //
                    var canEdit = ko.observable(true);

                }
                {
                    self.selectTabClick(self.tabList[0]);
                    self.treeMode(0);
                    self.initProductTab = function () {
                        self.GetRights();
                        self.selectTabClick(self.tabList[0]);
                        self.treeMode(0);
                        self.mode(self.modes.product);
                    }
                }
                //
                self.Load = function () {
                };

                self.raiseObjectModified = function () {
                    let id = self.selectedObjects[0].ID;
                    if (isFunction(self.selectedObjects[0].ID))
                        id = self.selectedObjects[0].ID();
                    $(document).trigger('local_objectUpdated', [223, id, null]);//softwareLicence
                };

                function isFunction(functionToCheck) {
                    return functionToCheck && {}.toString.call(functionToCheck) === '[object Function]';
                }

                self.ajaxControl = new ajaxLib.control();

                self.Rights = ko.observable(0);

                self.GetRights = function () {
                    if (self.isSubLi == 1) {

                        const isSub = (self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID') && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined');
                        const isPool = !isSub && self.selectedObjects[0].hasOwnProperty('SoftwareDistributionCentreID');
                        if (isPool) {
                            var retD = $.Deferred();
                            $.when(loadDetails())
                                .done(function () {
                                    retD.resolve();
                                });
                        }
                    }

                }

                self.navigatorObjectID = ko.observable(null);
                self.navigatorObjectClassID = ko.observable(null);
                self.locationControl = ko.observable(null);


                self.navigator_nodeSelected = function (node) {
                    self.navigatorObjectClassID(node.ClassID);
                    self.navigatorObjectID(node.ID);
                    self.reload();
                    return true;
                };
                self.orgStructureControl = ko.observable(null);
                self.productCatalogueControl = ko.observable(null);

                self.mode = ko.observable();
                self.mode.subscribe(function (newValue) {
                    if (newValue == self.modes.links)
                        self.linkList.CheckListData();
                });

                self.dispose = function () {
                    //
                    self.lvSearchText_handle.dispose();
                    //
                    self.tabList.forEach(function (tab) {
                        if (tab.hasOwnProperty('dispose')) {
                            tab.dispose();
                        }
                    });
                };
                self.afterRender = function (editor, elements) {

                };

                self.manufacturer = ko.observable('');
                function getResourceName() {
                    return getTextResource('SoftwareLicenceScheme_Coefficients_Add');
                }

                self.FormName = ko.pureComputed(function () {
                    return getResourceName();
                });

                self.caption = ko.pureComputed(function () {
                    return self.FormName();
                });


                function setDetails(data) {
                    self.Rights(data.Balance);
                }


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
                        'region_frmLicenceSchemeAddCoeff',//form region prefix
                        'setting_frmLicenceSchemeAddCoeff',//location and size setting
                        getTextResource('SoftwareLicenceScheme_Coefficients_Add'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        700, 700,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Settings/LicenceSchemes/frmLicenceScheme_addCoefficient\'}"'//attributes of form region
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

                        vm.initProductTab();
                        hideSpinner();
                    });


                });
                //
                return $retval.promise();
            }
        };
        return module;
    });