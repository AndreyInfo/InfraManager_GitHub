define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    'ui_controls/ListView/ko.ListView.Cells'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        mCells
    ) {
        var module = {
            ViewModel: function ($region, selectedObjects, isSubLi) {
                var self = this;
                self.$region = $region;
                self.selectedObjects = selectedObjects;            
                self.$isDone = $.Deferred();//resolve, когда операция выполнена            
                self.LifeCycleStateOperationID = null;//context
                //
                //Report
                {
                    self.printReport = ko.observable(false);
                }
                //
                self.SizeChanged = function () {
                    if (self.lv)
                        self.lv.renderTable();
                };
                //
                {//events of listView

                    self.lv = null;
                    self.listViewID = 'listView_' + ko.getNewID();
                    self.lv_checkedItemsBeforeChanged_handle = null;
                    self.lv_checkedItemsChanged_handle = null;
                    self.lv_handle = null;
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

                    };
                    self.listViewDrawCell = function (obj, column, cell) {
                        if ((column.IsEdit && column.MemberName == 'Balance')) {
                            column.Template("../UI/Forms/Asset/AssetOperations/CellTemplates/sdText");
                            cell.value(obj[column.MemberName]);
                        }
                        else if (column.IsEdit && column.MemberName == 'ReturningCount') {
                            column.Template("../UI/Forms/Asset/AssetOperations/CellTemplates/sdNumberEditor");
                            cell.value(obj[column.MemberName]);
                        }
                        else {
                            cell.text = mCells.textRepresenter(obj, column);
                        }
                    };
                }

                {//geting data             
                    self.loadObjectListByIDs = function (idArray, unshiftMode) {
                        for (var i = 0; i < idArray.length; i++)
                            idArray[i] = idArray[i].toUpperCase();
                        //
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(idArray, false)).done(function (objectList) {
                                if (objectList) {
                                    var rows = self.appendObjectList(objectList, unshiftMode);
                                    rows.forEach(function (row) {
                                        self.setRowAsNewer(row);
                                        //
                                        var obj = row.object;
                                        var id = self.getObjectID(obj);
                                        self.clearInfoByObject(id);
                                        //
                                        var index = idArray.indexOf(id);
                                        if (index != -1)
                                            idArray.splice(index, 1);
                                    });
                                }
                                idArray.forEach(function (id) {
                                    self.removeRowByID(id);
                                    self.clearInfoByObject(id);
                                });
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };

                    self.getObjectListByIDs = function (idArray, unshift) {
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(idArray, false)).done(function (objectList) {
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };

                    self.ajaxControl = new ajaxLib.control();
                    self.isAjaxActive = function () {
                        return self.ajaxControl.IsAcitve() == true;
                    };
                    //                
                    self.getObjectList = function (idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //                       
                        const subSoftwareLicenceID = self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')
                        && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined'
                            ? self.selectedObjects[0].ID
                            : null;

                        const softwareLicenceID =  self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')
                        && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined'
                            ? self.selectedObjects[0].SoftwareLicenceID
                            : self.selectedObjects[0].ID;

                        const ManufacturerID = self.selectedObjects[0].hasOwnProperty('ManufacturerID')
                            ? self.selectedObjects[0].ManufacturerID
                            : null;
                        const SoftwareDistributionCentreID = self.selectedObjects[0].hasOwnProperty('SoftwareDistributionCentreID')
                            ? self.selectedObjects[0].SoftwareDistributionCentreID
                            : null;
                        const SoftwareLicenceModelID = self.selectedObjects[0].hasOwnProperty('SoftwareLicenceModelID')
                            ? self.selectedObjects[0].SoftwareLicenceModelID
                            : null;
                        const SoftwareLicenceScheme = self.selectedObjects[0].hasOwnProperty('SoftwareLicenceScheme')
                            ? self.selectedObjects[0].SoftwareLicenceScheme
                            : null;
                        const SoftwareModelID = self.selectedObjects[0].hasOwnProperty('SoftwareModelID')
                            ? self.selectedObjects[0].SoftwareModelID
                            : null;
                        const SoftwareTypeID = self.selectedObjects[0].hasOwnProperty('SoftwareTypeID')
                            ? self.selectedObjects[0].SoftwareTypeID
                            : null;

                        const Type = self.selectedObjects[0].hasOwnProperty('Type')
                            ? self.selectedObjects[0].Type
                            : null;
                        
                        var requestInfo = {
                            IDList: idArray ? idArray : [],
                            ViewName: 'AssertReturning',
                            TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                            ParentObjectID: softwareLicenceID,
                            ParentID: subSoftwareLicenceID,
                            ManufacturerID : ManufacturerID,
                            SoftwareDistributionCentreID: SoftwareDistributionCentreID,
                            SoftwareLicenceModelID: SoftwareLicenceModelID,
                            SoftwareLicenceSchemeID: SoftwareLicenceScheme,
                            SoftwareModelID: SoftwareModelID,
                            SoftwareTypeID: SoftwareTypeID,
                            SoftwareLicenseType: Type,
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'POST',
                                data: requestInfo,
                                url: '/assetApi/GetSoftwareLicenceReference'
                            },
                            function (newVal) {
                                if (newVal && newVal.Result === 0) {
                                    retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                    self.InitSub();
                                    return;
                                }
                                else if (newVal && newVal.Result === 1 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                                    });
                                }
                                else if (newVal && newVal.Result === 2 && showErrors === true) {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
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
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js getData]', 'error');
                                    });
                                }
                                //
                                retvalD.resolve([]);
                            },
                            function (XMLHttpRequest, textStatus, errorThrown) {
                                if (showErrors === true)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js, getData]', 'error');
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
                        let count = 0;
                        let rows = self.lv.rowViewModel.rowList();

                        self.selectedItems().map(function (item) {

                            rows.forEach(function (row) {
                                if (row.object.ID == item.ID) {
                                    const cellList = row.cells();
                                    cellList.forEach(function (cell) {
                                        if (cell.column.MemberName == "ReturningCount") {
                                            var val = parseInt(cell.value(), 10);
                                            if (!isNaN(val)) {
                                                count = count + val;
                                            }
                                        }
                                    });
                                }
                            });
                        });

                        return count;
                    });
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
                    if (self.lv != null) {
                        self.lv.load();
                    }
                };

                //
                self.Load = function () {
                    var retD = $.Deferred();
                    $.when(getSoftwareObject()
                    ).done(function (object ) {
                        var data = object.Data;
                        if (data != null)
                            self.manufacturer( data.SoftwareModelName + ' ' + data.ManufacturerName + ' ' + data.InventoryNumber);
                        retD.resolve();
                    });
                    return retD.promise();                    
                };

                self.subscriptionList = [];
                self.InitSub = function () {
                    if (self.lv) {
                        let rows = self.lv.rowViewModel.rowList();
                        rows.forEach(function (row) {
                            const cellList = row.cells();
                            cellList.forEach(function (cell) {
                                if (cell.column.MemberName == "ReturningCount") {
                                    self.returningCount = 0;
                                    cell.value.subscribe(function (previousValue) {
                                        var re = /^\d*$/;
                                        if (re.test(previousValue)) {
                                            var value = parseInt(previousValue, 10);
                                            if (!isNaN(value) && value <= row.object['SoftwareExecutionCount']) {
                                                self.returningCount = previousValue;
                                            }
                                        }
                                    }, this, "beforeChange");
                                    var subscription = cell.value.subscribe(function (newValue) {
                                        var value = parseInt(newValue, 10);
                                        var re = /^\d*$/;
                                        if (isNaN(value) || !re.test(newValue) || (!isNaN(value) && (value > row.object['SoftwareExecutionCount'] || value == 0))) {
                                            cell.value(self.returningCount);
                                        }
                                        cellList.forEach(function (c) {
                                            if (c.column.MemberName == "Balance") {
                                                var val = parseInt(newValue, 10);
                                                if (!isNaN(val) && val <= row.object['SoftwareExecutionCount'] && val > 0) {
                                                    c.value(row.object['SoftwareExecutionCount'] - val);
                                                }
                                            }
                                        });
                                    });
                                }
                            });
                        });
                    }
                }

                self.getReferenceList = function () {
                    let vs = self.selectedItems().map(function (item) {

                        let rows = self.lv.rowViewModel.rowList();

                        let softwareExecutionCount = 0;

                        rows.forEach(function (row) {
                            if (row.object.ObjectID == item.ObjectID) {
                                const cellList = row.cells();
                                cellList.forEach(function (cell) {
                                    if (cell.column.MemberName == "ReturningCount") {
                                        var val = parseInt(cell.value(), 10);
                                        if (!isNaN(val)) {
                                            softwareExecutionCount = softwareExecutionCount + val;
                                        }
                                    }
                                });
                            }
                        });

                        const SubSoftwareLicenceID = self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')
                        && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined'
                            ? self.selectedObjects[0].ID
                            : null;

                        const SoftwareLicenceID =  self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')
                        && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined'
                            ? self.selectedObjects[0].SoftwareLicenceID
                            : self.selectedObjects[0].ID;

                        const ManufacturerID = self.selectedObjects[0].hasOwnProperty('ManufacturerID')
                            ? self.selectedObjects[0].ManufacturerID
                            : null;
                        const SoftwareDistributionCentreID = self.selectedObjects[0].hasOwnProperty('SoftwareDistributionCentreID')
                            ? self.selectedObjects[0].SoftwareDistributionCentreID
                            : null;                        
                        const SoftwareLicenceScheme = self.selectedObjects[0].hasOwnProperty('SoftwareLicenceScheme')
                            ? self.selectedObjects[0].SoftwareLicenceScheme
                            : null;
                        const SoftwareModelID = self.selectedObjects[0].hasOwnProperty('SoftwareModelID')
                            ? self.selectedObjects[0].SoftwareModelID
                            : null;
                        const SoftwareTypeID = self.selectedObjects[0].hasOwnProperty('SoftwareTypeID')
                            ? self.selectedObjects[0].SoftwareTypeID
                            : null;

                        const Type = self.selectedObjects[0].hasOwnProperty('Type')
                            ? self.selectedObjects[0].Type
                            : null;
                        

                        let licenceReferenceParams = {
                            SoftwareLicenceID: SoftwareLicenceID,
                            SubSoftwareLicenceID: SubSoftwareLicenceID,
                            ManufacturerID : ManufacturerID,
                            SoftwareDistributionCentreID: SoftwareDistributionCentreID,
                            SoftwareLicenceSchemeID: SoftwareLicenceScheme,
                            SoftwareModelID: SoftwareModelID,
                            SoftwareTypeID: SoftwareTypeID,
                            SoftwareLicenseType: Type,
                            ObjectID: item["ObjectID"],
                            ReturnedSoftwareExecutionCount: softwareExecutionCount
                        };

                        return licenceReferenceParams;
                    });

                    return vs;
                }

                self.ajaxControlConsumption = new ajaxLib.control();

                self.Consumption = function () {
                    //
                    var data =
                    {
                        'ReferenceList': self.getReferenceList(),
                        'ReasonNumber': "",
                        'PrintReport': false,
                        'LifeCycleStateOperationID': self.LifeCycleStateOperationID
                    }
                    
                    self.ajaxControlConsumption.Ajax(self.$region,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: '/assetApi/AssetLicenceReturn'
                        },
                        function (newVal) {
                            if (newVal) {
                                if (newVal.Result === 0) {
                                    self.$isDone.resolve(true);
                                    //
                                    var message = getTextResource('AssetLicenceReturn_Succsess');
                                    //
                                    var succsess = true;
                                    if (self.printReport()) {
                                        if (newVal.PrintReportResult === 2)//no report
                                        {
                                            succsess = false;
                                            require(['sweetAlert'], function () {
                                                swal(message, getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoReport'), 'info');
                                            });
                                        }
                                        else if (newVal.PrintReportResult === 3)//no ID parameter
                                        {
                                            succsess = false;
                                            require(['sweetAlert'], function () {
                                                swal(message, getTextResource('ReportPrintError') + '\n' + getTextResource('ReportPrint_NoParam'), 'info');
                                            });
                                        }
                                        else {
                                            if (newVal.FileInfoList != null) {
                                                var reportControl = new fcLib.control();
                                                newVal.FileInfoList.forEach(function (el) {
                                                    var item = new reportControl.CreateItem(el.ID, el.ObjectID, el.FileName, '', '', '', 'pdf');
                                                    reportControl.DownloadFile(item);
                                                });
                                            }
                                        }
                                    }
                                    //
                                    if (succsess)
                                        require(['sweetAlert'], function () {
                                            swal(message);
                                        });
                                }
                                else {
                                    self.$isDone.resolve(true);
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AssetOperation_Error'), 'error');
                                    });
                                }
                            }
                        });
                    //
                    return self.$isDone.promise();
                };       

                self.raiseObjectModified = function (id) {                    
                    if (isFunction(id))
                        id = self.selectedObjects[0].ID();
                    $(document).trigger('local_objectUpdated', [223, id, null]);//softwareLicence
                };

                function isFunction(functionToCheck) {
                    return functionToCheck && {}.toString.call(functionToCheck) === '[object Function]';
                }

                self.dispose = function () {
                    //
                    if (self.lv != null)
                        self.lv.dispose();
                };
                self.afterRender = function (editor, elements) {

                };

                self.manufacturer = ko.observable('');

                function getResourceName() {
                    const isSub = (self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')  && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined');
                    if (isSub)
                        return getTextResource('ReturningName');                    
                    return "Вернуть право";
                }

                self.FormName = ko.pureComputed(function () {
                    return getResourceName() + ' ' +
                        self.manufacturer();
                });

                self.caption = ko.pureComputed(function () {
                    return self.FormName();
                });

                function getSoftwareObject() {
                    var retD = $.Deferred();
                    const softwareLicenceID =  self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')
                    && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined'
                        ? self.selectedObjects[0].SoftwareLicenceID
                        : self.selectedObjects[0].ID;

                    var param = { ID: softwareLicenceID };
                    new ajaxLib.control().Ajax($region, {
                            dataType: "json",
                            method: 'GET',
                            url: '/assetApi/GetSoftwareLicenceObject?' + $.param(param)
                        },
                        function (response) {                            
                            retD.resolve(response);
                        });

                    return retD.promise();
                }

            },
            ShowDialog: function (selectedObjects, operationName, lifeCycleStateOperationID, isSpinnerActive, isSubLi) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var $retval = $.Deferred();
                var bindElement = null;
                //
                $.when(userD).done(function (user) {
                    var isReadOnly = false;
                    var forceClose = false;
                    //
                    if (user.HasRoles == false)
                        isReadOnly = true;
                    //
                    var frm = undefined;
                    var vm = undefined;
                    //
                    var buttons = {};
                    var selectedRows_handle = null;
                    buttons[getTextResource('ButtonCancel')] = function () {
                        forceClose = true;
                        frm.Close();
                    };
                    //

                    frm = new fc.control(
                        'frmLicenceReturning',//form region prefix
                        'frmLicenceReturning_setting',//location and size setting
                        operationName,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        500, 500,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();
                            if (selectedRows_handle)
                                selectedRows_handle.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmLicenceReturning\'}"'//attributes of form region
                    );
                    //
                    if (!frm.Initialized)
                        return;//form with that region and settingsName was open
                    //
                    frm.BeforeClose = function () {
                        var retval = forceClose;
                        //
                        if (retval == false) {
                            require(['sweetAlert'], function () {
                                swal({
                                    title: getTextResource('FormClosingQuestion'),
                                    showCancelButton: true,
                                    closeOnConfirm: true,
                                    closeOnCancel: true,
                                    confirmButtonText: getTextResource('ButtonOK'),
                                    cancelButtonText: getTextResource('ButtonCancel')
                                },
                                    function (value) {
                                        if (value == true) {
                                            forceClose = true;
                                            setTimeout(function () {
                                                frm.Close();
                                            }, 300);//TODO? close event of swal
                                        }
                                    });
                            });
                        }
                        //
                        return retval;
                    };
                    //
                    var $region = $('#' + frm.GetRegionID());
                    vm = new module.ViewModel($region, selectedObjects, isSubLi);
                    vm.LifeCycleStateOperationID = lifeCycleStateOperationID;
                    vm.$isDone = $retval;
                    vm.Load();

                    vm.selectedItems.subscribe(function (newValue) {
                        var newButtons = {}
                        if (newValue.length > 0) {
                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = true;
                                frm.Close();
                            };

                            newButtons[operationName] = function () {
                                var d = vm.Consumption();
                                $.when(d).done(function (result) {
                                    if (!result)
                                        return;
                                    //
                                    vm.raiseObjectModified(vm.selectedObjects[0].ID);
                                    forceClose = true;
                                    frm.Close();
                                });
                            };
                        }
                        else {
                            newButtons[getTextResource('ButtonCancel')] = function () {
                                forceClose = true;
                                frm.Close();
                            };
                        }
                        if (!forceClose)
                            frm.UpdateButtons(newButtons);
                    });

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


                        $("testInput").on(function () {
                            alert("Handler for .change() called.");
                        });
                    });


                });
                //
                return $retval.promise();
            }
        };
        return module;
    });