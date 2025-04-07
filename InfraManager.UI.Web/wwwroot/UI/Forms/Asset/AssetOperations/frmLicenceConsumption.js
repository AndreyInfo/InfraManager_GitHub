define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    './tab_org',
    './tab_place',
    './tab_product',
    './tab_place_equip',
    './ValidationHelper'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        tab_org,
        tab_place,
        tab_product,
        tab_place_equip,
        helper
    ) {

        var module = {
            ViewModel: function ($region, selectedObjects, isEquip, isSubLi) {
                var self = this;
                self.$region = $region;
                self.selectedObjects = selectedObjects;
                self.isEquip = ko.observable(isEquip);
                self.isSubLi = isSubLi;
                self.$isDone = $.Deferred();//resolve, когда операция выполнена
                self.Rights = ko.observable(0);

                self.LifeCycleStateOperationID = null;//context
                //
                self.mode = ko.observable();
                self.treeMode = ko.observable();
                self.modes = {
                    org: 'org',
                    links: 'links',
                    place: 'place',
                    product: 'product',
                    placeEquip: 'placeEquip'
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

                    {//validate
                        self.InitSub = function () {
                            if (self.tabActive().lv) {
                                let rows = self.tabActive().lv.rowViewModel.rowList();
                                rows.forEach(function (row) {
                                    const cellList = row.cells();
                                    cellList.forEach(function (cell) {
                                        if (cell.column.MemberName == "SoftwareExecutionCount") {

                                            cell.value.subscribe(function (previousValue) {
                                                self.maxRights = self.Rights();
                                                let decPreviousValue = parseInt(previousValue, 10);
                                                if (decPreviousValue <= self.maxRights) {
                                                    self.previousValue = previousValue === undefined ? 1 : previousValue;
                                                    //
                                                    var elem = ko.utils.arrayFirst(self.getReferenceList(), function (el) {
                                                        return el.ObjectID.toUpperCase() == row.object.ID.toUpperCase();
                                                    });
                                                    //
                                                    if (elem) {
                                                        self.maxRights = self.maxRights - self.getReferenceCount() + parseInt(self.previousValue, 10);
                                                    } else {
                                                        self.maxRights = self.maxRights - self.getReferenceCount();
                                                    }
                                                }
                                            }, this, "beforeChange");

                                            cell.value.subscribe(function (newValue) {
                                                cell.value(Validate(self.previousValue, newValue, self.maxRights))
                                            });
                                        }
                                    });
                                });
                            }
                        }
                    }

                    {//geting data             
                        self.ajaxControl = new ajaxLib.control();
                        self.getObjectList = function (startRecordIndex, countOfRecords, idArray, showErrors) {
                            var retvalD = $.Deferred();
                            //
                            const softwareLicenceID = self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')
                                ? self.selectedObjects[0].SoftwareLicenceID
                                : null;

                            const softwareDistributionCentreID = self.selectedObjects[0].hasOwnProperty('SoftwareDistributionCentreID')
                                ? self.selectedObjects[0].SoftwareDistributionCentreID
                                : null;
                                                        
                            var idList = self.selectedObjects.map(function (it) {
                                return softwareLicenceID == null ? it.ID : it.SoftwareLicenceID;
                            });
                           
                            var treeSettings =
                            {
                                FiltrationObjectID: self.navigatorObjectID(),
                                FiltrationObjectClassID: self.navigatorObjectClassID(),
                                FiltrationObjectName: '',
                                FiltrationTreeType: self.treeMode(),
                                FiltrationField: ''
                            };

                            var requestInfo = {
                                StartRecordIndex: startRecordIndex,
                                CountRecords: countOfRecords,
                                IDList: idList,
                                ViewName: self.tabActive().lv.options.settingsName(),
                                TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                                CurrentFilterID: null,
                                WithFinishedWorkflow: false,
                                AfterModifiedMilliseconds: null,
                                TreeSettings: treeSettings,
                                SearchRequest: self.lvSearchText(),
                                FilterObjectID: self.navigatorObjectID(),
                                FilterObjectClassID: self.navigatorObjectClassID(),
                                AvailableObjectClassID: self.AvailableDeviceClassID,
                                SoftwareSubLicence: softwareLicenceID == null ? softwareLicenceID : self.selectedObjects[0].ID,
                                SoftwareDistributionCentreID: softwareDistributionCentreID
                            };
                            
                            let url = '/assetApi/GetUsersList';
                            if (self.isEquip() == true) {
                                url = '/assetApi/GetEquipList';
                            }
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
                                        self.InitSub();
                                        return;
                                    }
                                    else if (newVal && newVal.Result === 1 && showErrors === true) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmLicenceConsumption.js getObjectList]', 'error');
                                        });
                                    }
                                    else if (newVal && newVal.Result === 2 && showErrors === true) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmLicenceConsumption.js getObjectList]', 'error');
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
                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLicenceConsumption.js getObjectList]', 'error');
                                        });
                                    }
                                    //
                                    retvalD.resolve([]);
                                },
                                function (XMLHttpRequest, textStatus, errorThrown) {
                                    if (showErrors === true)
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLicenceConsumption.js getObjectList]', 'error');
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
                            new tab_org.Tab(self),                //0
                            new tab_place.Tab(self),               //1
                            new tab_product.Tab(self),
                            new tab_place_equip.Tab(self)
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

                    self.orgClick = function () {
                        self.selectTabClick(self.tabList[0]);
                        self.mode(self.modes.org);
                        self.treeMode(0);
                    };
                    self.linksClick = function () {
                        self.selectTabClick(null);
                        self.mode(self.modes.links);
                        self.treeMode(255);
                    };
                    self.placeClick = function () {
                        self.selectTabClick(self.tabList[1]);
                        self.mode(self.modes.place);
                        self.treeMode(1);
                    };
                    self.productClick = function () {
                        self.selectTabClick(self.tabList[2]);
                        self.mode(self.modes.product);
                        self.treeMode(2);
                    };
                    self.placeEquipClick = function () {
                        self.selectTabClick(self.tabList[3]);
                        self.mode(self.modes.placeEquip);
                        self.treeMode(1);
                    };
                    //
                    var canEdit = ko.observable(true);

                }
                {
                    self.selectTabClick(self.tabList[0]);
                    self.treeMode(0);
                    self.initGeneralTab = function () {                        
                        if (self.isEquip() == true) {
                            self.selectTabClick(self.tabList[2]);
                            self.treeMode(2);
                            self.mode(self.modes.product);
                        }
                        else {
                            self.selectTabClick(self.tabList[0]);
                            self.treeMode(0);
                            self.mode(self.modes.org);
                        }
                    }
                }
                //
                self.Load = function () {
                    var retD = $.Deferred();
                    
                    const isSub = (self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')  && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined');
                    const isPool = !isSub && self.selectedObjects[0].hasOwnProperty('SoftwareDistributionCentreID') && typeof self.selectedObjects[0].SoftwareDistributionCentreID !== 'undefined';
                    
                    if (!isPool) {
                        $.when(getSoftwareObject()).done(function (object) {
                            var data = object.Data;
                            if (data != null) {                                
                                self.manufacturer(data.SoftwareModelName + ' ' + data.ManufacturerName + ' ' + data.InventoryNumber);
                                var balance = data.Balance;
                                //
                                if (!isNaN(parseInt(balance)))
                                    self.Rights(balance);
                                else
                                    self.Rights(Math.pow(2, 31) - 1);
                            }
                            retD.resolve();
                        });
                    }
                    else {
                        $.when().done(function () {
                            loadDetails();
                            retD.resolve();
                        });
                    }
                    
                    return retD.promise();
                };

                self.getReferenceList = function () {
                    return self.selectedItems().map(function (item) {

                        let rows = self.tabActive().lv.rowViewModel.rowList();

                        let softwareExecutionCount = 1;

                        rows.forEach(function (row) {
                            if (row.object.ID == item.ID) {
                                const cellList = row.cells();
                                cellList.forEach(function (cell) {
                                    if (cell.column.MemberName == "SoftwareExecutionCount") {
                                        softwareExecutionCount = parseInt(cell.value(), 10);
                                    }
                                });
                            }
                        });
                        
                        if (isNaN(softwareExecutionCount) || softwareExecutionCount == null)
                            softwareExecutionCount = 1
                        
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
                        
                        const SubSoftwareLicenceID = self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID') 
                                            && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined'                         
                            ? self.selectedObjects[0].ID
                            : null;
                        
                        const SoftwareLicenceID =  self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')
                                            && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined'
                            ? self.selectedObjects[0].SoftwareLicenceID
                            : self.selectedObjects[0].ID;                     
                                                
                        let licenceReferenceParams = {
                            SoftwareLicenceID: SoftwareLicenceID,
                            SubSoftwareLicenceID: SubSoftwareLicenceID,
                            ManufacturerID : ManufacturerID,
                            SoftwareDistributionCentreID: SoftwareDistributionCentreID,                            
                            SoftwareLicenceSchemeID: SoftwareLicenceScheme,
                            SoftwareModelID: SoftwareModelID,
                            SoftwareTypeID: SoftwareTypeID,
                            SoftwareLicenseType: Type,
                            ObjectID: item["ID"],
                            SoftwareExecutionCount: softwareExecutionCount,
                            UniqueNumber: item["UserNumber"]
                        };
                        
                        return licenceReferenceParams;
                    });
                }

                self.raiseObjectModified = function () {
                    let id = self.selectedObjects[0].ID; 
                    if (isFunction(self.selectedObjects[0].ID))
                        id = self.selectedObjects[0].ID();                    
                    $(document).trigger('local_objectUpdated', [223, id, null]);//softwareLicence
                };

                function isFunction(functionToCheck) {
                    return functionToCheck && {}.toString.call(functionToCheck) === '[object Function]';
                }

                self.Consumption = function () {
                    //
                    var data =
                    {
                        'ReferenceList': self.getReferenceList(),
                        'ReasonNumber': "",
                        'PrintReport': false,
                        'LifeCycleStateOperationID': self.LifeCycleStateOperationID
                    }
                    self.ajaxControl.Ajax(self.$region,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: '/assetApi/AssetLicenceConsumption'
                        },
                        function (newVal) {
                            if (newVal) {
                                if (newVal.Result === 0) {
                                    self.$isDone.resolve(true);
                                    //
                                    var message = getTextResource('AssetLicenceConsumption_Succsess');
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
                                } else if (newVal.Result === 8) {
                                    self.$isDone.resolve(true);
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), newVal.Error, 'error');
                                    });
                                }
                            }
                        });
                    //
                    return self.$isDone.promise();
                };


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
                    self.ajaxControl.Abort();
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
                self.softwareModel = ko.observable(self.selectedObjects[0].hasOwnProperty('SoftwareModelName') ? self.selectedObjects[0].SoftwareModelName : null);
                self.softwareModelVersion = ko.observable(self.selectedObjects[0].hasOwnProperty('SoftwareModelVersion') ? ' / ' +self.selectedObjects[0].SoftwareModelVersion : null);
                
                function getResourceName() {
                    const isSub = (self.selectedObjects[0].hasOwnProperty('SoftwareLicenceID')  && typeof self.selectedObjects[0].SoftwareLicenceID !== 'undefined');
                    const isPool = !isSub && self.selectedObjects[0].hasOwnProperty('SoftwareDistributionCentreID') && typeof self.selectedObjects[0].SoftwareDistributionCentreID !== 'undefined';

                    if (isSub && isEquip && !isPool)
                        return getTextResource('IssueRightNameEquip') + ' ' +  self.manufacturer();
                    if (!isEquip && isSub&& !isPool)
                        return getTextResource('IssueRightNameUser') + ' ' +   self.manufacturer();
                    if (isPool && isEquip)
                        return getTextResource('SorcePoolNameEquip') + ' ' + self.softwareModel() + self.softwareModelVersion();
                    if (isPool && !isEquip)
                        return getTextResource('SorcePoolNameUser')+ ' ' + self.softwareModel()  +  self.softwareModelVersion();
                    
                    return "Выдать право";
                }
                
                self.FormName = ko.pureComputed(function () {
                    return getResourceName();                    
                });

                self.caption = ko.pureComputed(function () {
                    return self.FormName();
                });

                function getSoftwareObject() {
                    var retD = $.Deferred();

                    var param = { ID: self.selectedObjects[0].ID };

                    self.ajaxControl.Ajax($region, {
                            dataType: "json",
                            method: 'GET',
                            url: '/assetApi/GetSoftwareLicenceObject?' + $.param(param)
                        },
                        function (response) {                            
                            retD.resolve(response);
                        });                   

                    return retD.promise();
                }


                function setDetails(data) {                    
                    self.Rights(data.Balance);                                    
                }

                function loadDetails() {
                    var retD = $.Deferred();

                    self.ajaxControl.Ajax(null, {
                        dataType: "json",
                        method: 'GET',
                        url: '/assetApi/GetSoftwarePoolBalance/'
                            + self.selectedObjects[0].ManufacturerID
                            + '/'
                            + self.selectedObjects[0].SoftwareDistributionCentreID
                            + '/'
                            + self.selectedObjects[0].SoftwareModelID
                            + '/'
                            + self.selectedObjects[0].SoftwareTypeID
                            + '/'
                            + self.selectedObjects[0].SoftwareLicenceScheme
                            + '/'
                            + self.selectedObjects[0].Type
                            + '/'
                            + '12'
                    }, function (newVal) {
                        if (newVal && newVal.Result === 0) {
                            var data = newVal.Data;
                            if (data) {
                                setDetails(data);
                                retD.resolve();
                            }
                        }
                        else if (newVal && newVal.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                            });
                        else if (newVal && newVal.Result === 2)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                            });
                        else if (newVal && newVal.Result === 3)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                            });
                        else
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                            });
                    });

                    return retD.promise();
                }

            },
            ShowDialog: function (selectedObjects, operationName, lifeCycleStateOperationID, isSpinnerActive, isEquip, isSubLi) {
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
                    buttons[getTextResource('ButtonCancel')] = function () {
                        forceClose = true;
                        frm.Close();
                    };
                    //
                    frm = new fc.control(
                        'frmLicenceConsumption',//form region prefix
                        'frmLicenceConsumption_setting',//location and size setting
                        operationName,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        700, 700,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmLicenceConsumption\'}"'//attributes of form region
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
                    vm = new module.ViewModel($region, selectedObjects, isEquip, isSubLi);
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
                                    forceClose = true;
                                    vm.raiseObjectModified();
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

                        vm.initGeneralTab();
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