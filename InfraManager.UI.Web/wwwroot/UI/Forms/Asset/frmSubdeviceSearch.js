define(['knockout', 'jquery', 'ajax', 'formControl', 'treeControl', 'usualForms', 'ui_controls/ListView/ko.ListView'], function (ko, $, ajaxLib, formControl, treeLib, fhModule) {
    var module = {
        ViewModel: function () {
            var self = this;
            //       
            {//left side
                self.tvSearchText = ko.observable('');
                //
                self.locationSearcher = null;
                self.initLocationSearcherControl = function () {
                    var $frm = $('#' + self.frm.GetRegionID()).find('.frmSubdeviceSearch');
                    var searcherControlD = $.Deferred();
                    //
                    var fh = new fhModule.formHelper();
                    var searcherLoadD = fh.SetTextSearcherToField(
                        $frm.find('.locationSearcher'),
                        'AssetLocationSearcher',
                        null,
                        [true, false, false, false, null],//room, rack, workplace, device, storageID
                        function (objectInfo) {//select
                            self.tvSearchText(objectInfo.FullName);
                            $.when(self.navigator.OpenToNode(objectInfo.ID, objectInfo.ClassID)).done(function (finalNode) {
                                if (finalNode && finalNode.ID == objectInfo.ID) {
                                    self.navigator.SelectNode(finalNode);
                                    self.reload();
                                }
                            });
                        },
                        function () {//reset
                            self.tvSearchText('');
                        },
                        function (selectedItem) {//close
                            if (!selectedItem) {
                                self.tvSearchText('');
                            }
                        },
                        undefined,
                        true);
                    $.when(searcherLoadD, userD).done(function (ctrl, user) {
                        searcherControlD.resolve(ctrl);
                        ctrl.CurrentUserID = user.ID;
                        self.locationSearcher = ctrl;
                    });
                };
                //
                self.navigatorObjectID = ko.observable(null);
                self.navigatorObjectClassID = ko.observable(null);
                //
                self.AvailableDeviceClassID = null;
                //
                self.navigator = null;
                self.initNavigator = function () {
                    var $div = $('#' + self.frm.GetRegionID()).find('.tvWrapper');
                    //
                    self.navigator = new treeLib.control();
                    self.navigator.init($div, 1, {
                        onClick: self.navigator_nodeSelected,
                        UseAccessIsGranted: true,
                        ShowCheckboxes: false,
                        AvailableClassArray: [29, 101, 1, 2, 3],
                        ClickableClassArray: [29, 101, 1, 2, 3],
                        AllClickable: false,
                        FinishClassArray: [3],
                        Title: getTextResource('LocationCaption'),
                        WindowModeEnabled: false,
                        HasLifeCycle: false,
                        ExpandFirstNodes: true,
                    });
                    $.when(self.navigator.$isLoaded).done(function () {
                        $div.find('.treeControlWrapper .treeControlHeader').click();//открыть размел по местоположению
                    });
                };
                self.navigator_nodeSelected = function (node) {
                    self.navigatorObjectClassID(node.ClassID);
                    self.navigatorObjectID(node.ID);
                    self.reload();
                    return true;
                };
            }
            {//right side
                self.lvSearchText = ko.observable('');
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
                //
                {//events of listView
                    self.lv = null;
                    self.lv_checkedItemsBeforeChanged_handle = null;
                    self.lv_checkedItemsChanged_handle = null;
                    //
                    self.lvInit = function (listView) {
                        self.lv = listView;
                        self.lv_checkedItemsBeforeChanged_handle = listView.rowViewModel.checkedItemsToSubscribe.subscribe(function (oldObjectList) {
                            self.tmp = oldObjectList;
                        }, null, "beforeChange");
                        self.lv_checkedItemsChanged_handle = listView.rowViewModel.checkedItemsToSubscribe.subscribe(function (newObjectList) {
                            var oldObjectList = self.tmp;
                            self.tmp = undefined;
                            //
                            if (self.selectedItemFreeze)
                                return;
                            var selectedItems = self.selectedItems();//информация о выбранных объектах
                            //
                            //нужно найти снятые чекбоксы
                            for (var j = 0; j < oldObjectList.length; j++) {
                                var exists = false;
                                var id = oldObjectList[j].ID.toUpperCase();
                                for (var i = 0; i < newObjectList.length; i++)
                                    if (newObjectList[i].ID.toUpperCase() == id) {
                                        exists = true;
                                        break;
                                    }
                                if (exists === false) {
                                    for (var i = 0; i < selectedItems.length; i++)
                                        if (selectedItems[i].ID.toUpperCase() == id) {
                                            selectedItems.splice(i, 1);//убираем снятый чекбокс
                                            break;
                                        }
                                }
                            }
                            //нужно добавить установленные чекбоксы
                            for (var j = 0; j < newObjectList.length; j++) {
                                var exists = false;
                                var id = newObjectList[j].ID.toUpperCase();
                                for (var i = 0; i < selectedItems.length; i++)
                                    if (selectedItems[i].ID.toUpperCase() == id) {
                                        exists = true;
                                        break;
                                    }
                                if (exists === false)
                                    selectedItems.push(newObjectList[j]);
                            }
                            //
                            self.selectedItems(selectedItems);
                        });
                        //
                        var storedLoad = self.lv.load;
                        self.lv.load = function () {
                            var retvalD = $.Deferred();
                            self.selectedItemFreeze = true;
                            $.when(storedLoad()).done(function () {
                                self.selectedItemFreeze = false;
                                self.markListViewSelection();
                                retvalD.resolve();
                            });
                            return retvalD.promise();
                        };
                        //
                        self.lv.load();
                    };
                    self.lvRetrieveVirtualItems = function (startRecordIndex, countOfRecords) {
                        var retvalD = $.Deferred();
                        $.when(self.getObjectList(startRecordIndex, countOfRecords, null, true)).done(function (objectList) {
                            retvalD.resolve(objectList);
                            //
                            self.markListViewSelection();
                        });
                        return retvalD.promise();
                    };
                    self.lvRowClick = function (obj) {
                        var classID = obj.ClassID;
                        var id = obj.ID.toUpperCase();
                        //                       
                        showSpinner();
                        require(['assetForms'], function (module) {
                            var fh = new module.formHelper(true);
                            if (classID == 33 || classID == 34)
                                fh.ShowAssetForm(id, classID);
                            else
                                throw 'classID not supported';
                        });
                    };
                }
                //
                {//geting data             
                    self.ajaxControl = new ajaxLib.control();
                    self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var requestInfo = {
                            StartRecordIndex: startRecordIndex,
                            CountRecords: countOfRecords,
                            IDList: [],
                            ViewName: self.lv.options.settingsName(),
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            CurrentFilterID: null,
                            WithFinishedWorkflow: false,
                            AfterModifiedMilliseconds: null,
                            TreeSettings: null,
                            SearchRequest: self.lvSearchText(),
                            FilterObjectID: self.navigatorObjectID(),
                            FilterObjectClassID: self.navigatorObjectClassID(),
                            AvailableObjectClassID: self.AvailableDeviceClassID
                        };
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSubdeviceList'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                    return;
                                }
                                else if (newVal && newVal.Result === 1 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSubdeviceSearch.js getObjectList]', 'error');
                                    });
                                }
                                else if (newVal && newVal.Result === 2 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSubdeviceSearch.js getObjectList]', 'error');
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
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSubdeviceSearch.js getObjectList]', 'error');
                                    });
                                }
                                //
                                retvalD.resolve([]);
                            },
                            function (XMLHttpRequest, textStatus, errorThrown) {
                                if (showErrors === true)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSubdeviceSearch.js getObjectList]', 'error');
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
                //
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
                    }
                    //
                    self.markListViewSelection = function () {
                        for (var i = 0; i < self.selectedItems().length; i++) {
                            var row = self.lv.rowViewModel.getRowByObjectID(self.selectedItems()[i].ID);
                            if (row != null && row.checked() == false)
                                row.checked(true);
                        }
                    };
                }
                //filter changed
                self.reload = function () {
                    if (self.lv != null)
                        self.lv.load();
                };
            }
            //
            {//splitter
                self.minSplitterWidth = 200;
                self.maxSplitterWidth = 500;
                self.splitterDistance = ko.observable(300);
                self.resizeSplitter = function (newWidth) {
                    if (newWidth && newWidth >= self.minSplitterWidth && newWidth <= self.maxSplitterWidth) {
                        self.splitterDistance(newWidth);
                    }
                };
            }
            //
            self.dispose = function () {
                //todo tv dispose
                self.locationSearcher.Remove();
                //
                self.lvSearchText_handle.dispose();
                //
                self.lv_checkedItemsBeforeChanged_handle.dispose();
                self.lv_checkedItemsChanged_handle.dispose();
                self.lv.dispose();
            };
            self.afterRender = function (editor, elements) {
                self.initLocationSearcherControl();
                self.initNavigator();
            };
        },

        ShowDialog: function (onSelected, closeParent, availableDeviceClassID, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            var frm = undefined;
            var vm = new module.ViewModel();
            var bindElement = null;
            var handleOfSelectedItems = null;
            var selected = false;
            //
            vm.AvailableDeviceClassID = availableDeviceClassID;
            //
            var buttons = [];
            var bSelect = {
                text: getTextResource('Select'),
                click: function () {
                    selected = true;
                    var selectedItems = vm.selectedItems();
                    var selectedItemsInfo = vm.getItemsInfos(selectedItems);
                    if (selectedItemsInfo.length > 0) {
                        onSelected(selectedItemsInfo);
                        frm.Close();
                    }
                }
            }
            var bCancel = {
                text: getTextResource('Close'),
                click: function () {
                    frm.Close();
                }
            }
            buttons.push(bCancel);
            //
            frm = new formControl.control(
                    'region_frmSubdeviceSearch',//form region prefix
                    'setting_frmSubdeviceSearch',//location and size setting
                    getTextResource('SelectDevice'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    600, 350,//minSize
                    buttons,//form buttons
                    function () {
                        if (closeParent && !selected) {
                            closeParent();
                        }
                        //
                        handleOfSelectedItems.dispose();
                        vm.dispose();
                        ko.cleanNode(bindElement);
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Asset/frmSubdeviceSearch\', afterRender: afterRender}"'//attributes of form region
                );
            if (!frm.Initialized)
                return;//form with that region and settingsName was open
            frm.ExtendSize(700, 500);//normal size
            vm.frm = frm;
            handleOfSelectedItems = vm.selectedItems.subscribe(function (objectList) {
                buttons = [];
                buttons.push(bCancel);
                if (objectList.length > 0) {
                    bSelect.text = getTextResource('Select') + ' (' + objectList.length + ')';
                    buttons.push(bSelect);
                }
                frm.UpdateButtons(buttons);
            });
            //
            bindElement = document.getElementById(frm.GetRegionID());
            ko.applyBindings(vm, bindElement);
            //
            $.when(frm.Show()).done(function (frmD) {
                hideSpinner();
            });
        },

    };
    return module;
});