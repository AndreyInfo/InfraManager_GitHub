define(['knockout',
        'jquery',
        'ajax',
        'formControl',
        'dateTimeControl',
        './templateParamsUpdate/UpdateCharacteristics(Active)'
    ],
    function (ko,
              $,
              ajaxLib,
              fc,
              dtLib,
              u
    ) {
        var module = {
            ViewModel: function ($region, selectedObjects) {
                var self = this;
                self.$region = $region;
                self.selectedObjects = selectedObjects;
                self.$isDone = $.Deferred();//resolve, когда операция выполнена            
                self.LifeCycleStateOperationID = null;//context

                self.SoftwareLicenceObject = ko.observable(null);
                self.UpdateCharacteristics = ko.observable(null);

                self.Load = function () {
                    self.getSoft();
                    self.UpdateCharacteristics(new u.Group(self));
                    self.UpdateCharacteristics().Load();
                };
                self.dispose = function () {
                    
                };

                self.AfterRender = function () {
                    self.InitDtp('.effective-date', self.periodEnd, self.periodEndDateTime, self.controlEnd);
                    self.$region.find('.effective-date').focus();
                }

                self.LimitInDays = ko.computed(function () {
                    return (self.SoftwareLicenceObject() != null && self.SoftwareLicenceObject().LimitInDays != null)
                        ? self.SoftwareLicenceObject().LimitInDays
                        : 0;
                });

                //Report
                {
                    self.printReport = ko.observable(false);
                }
                
                self.ActivatedLicense = ko.computed(function () {
                    if (self.SoftwareLicenceObject() == null)
                        return "";
                    
                    let newParentSoftwareLicenceName = self.SoftwareLicenceObject().Name + ', ' + self.SoftwareLicenceObject().ManufacturerName;
                    if (self.SoftwareLicenceObject().InventoryNumber != null && self.SoftwareLicenceObject().InventoryNumber != '' && self.SoftwareLicenceObject().InventoryNumber != undefined) {
                        newParentSoftwareLicenceName += ', инв. № ' + self.SoftwareLicenceObject().InventoryNumber;
                    }

                    return newParentSoftwareLicenceName;
                });

                self.controlEnd = ko.observable(null);
                //
                self.dateNow = Date.now();
                self.showOnlyDate = true;
                self.periodEnd = ko.observable(parseDate(self.dateNow, self.showOnlyDate));//always local string
                self.periodEnd.subscribe(function (newValue) {
                    var dt = self.controlEnd().dtpControl.length > 0 ? self.controlEnd().dtpControl.datetimepicker('getValue') : null;
                    //
                    if (!newValue || newValue.length == 0)
                        self.periodEndDateTime(null);//clear field => reset value
                    else if (dtLib.Date2String(dt, self.showOnlyDate) != newValue) {
                        self.periodEndDateTime(null);//value incorrect => reset value
                        self.periodEnd('');
                    }
                    else
                        self.periodEndDateTime(dt);
                });
                self.periodEndDateTime = ko.observable(new Date(parseInt(self.dateNow)));//always dateTime, auto convert serverUtcDateString to jsLocalTime
                //

                self.InitDtp = function (dtpClass, dateTimeStr, dateTime, control) {
                    var dtp = self.$region.find(dtpClass);
                    var ctrl = new dtLib.control();
                    ctrl.init(dtp, {
                        StringObservable: dateTimeStr,
                        ValueDate: dateTime(),
                        IsDisabled : true,
                        OnSelectDateFunc: function (current_time, $input) {
                            dateTime(current_time);
                            dateTimeStr(dtLib.Date2String(current_time, self.showOnlyDate));
                        },
                        OnSelectTimeFunc: function (current_time, $input) {
                            dateTime(current_time);
                            dateTimeStr(dtLib.Date2String(current_time, self.showOnlyDate));
                        },
                        HeaderText: '',
                        OnlyDate: self.showOnlyDate
                    });
                    control(ctrl);
                }

                self.getSoft = function () {
                    var retval = $.Deferred();
                    self.ajaxControl = new ajaxLib.control();
                    self.ajaxControl.Ajax(null,
                        {
                            url: '/sdApi/GetSoftwareLicence',
                            method: 'GET',
                            data: { ID: self.selectedObjects[0].ID }
                        },
                        function (response) {
                            if (response.Result === 0 && response.SoftwareLicence) {
                                var obj = response.SoftwareLicence;
                                self.SoftwareLicenceObject(obj);
                                retval.resolve(true);
                            }
                            else {
                                retval.resolve(false);
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLicenceApplying.js, getSoft]', 'error');
                                });
                            }
                        });
                }
                
                self.Active = function () {                    
                    const data = {
                        'DtActive': self.periodEndDateTime().toISOString(),
                        'SoftwareLicenceID' : self.selectedObjects[0].ID,
                        'LifeCycleStateOperationID' : self.LifeCycleStateOperationID
                    }

                    self.ajaxControl.Ajax(self.$region,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: '/assetApi/AssetLicenceActive'
                        },
                        function (newVal) {
                            if (newVal) {
                                if (newVal.Result === 0) {
                                    self.$isDone.resolve(true);
                                    //
                                    var message = getTextResource('AssetLicenceActive_Succsess');
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
                }
                
            },
            ShowDialog: function (selectedObjects, operationName, lifeCycleStateOperationID, isSpinnerActive) {
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
                    buttons[operationName] = function () {
                        var d = vm.Active();
                        $.when(d).done(function (result) {
                            if (!result)
                                return;
                            //                            
                            forceClose = true;
                            frm.Close();
                        });
                    };
                    
                    //

                    frm = new fc.control(
                        'frmLicenceActive',//form region prefix
                        'frmLicenceActive_setting',//location and size setting
                        operationName,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        400, 400,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();                            
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmLicenceActive\', afterRender: AfterRender}"'//attributes of form region
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
                    vm = new module.ViewModel($region, selectedObjects);
                    vm.LifeCycleStateOperationID = lifeCycleStateOperationID;
                    vm.$isDone = $retval;
                    vm.Load();


                    //
                    vm.frm = frm;
                    frm.SizeChanged = function () {
                        var width = frm.GetInnerWidth();
                        var height = frm.GetInnerHeight();
                        //
                        vm.$region.css('width', width + 'px').css('height', height + 'px');
                    };
                    //
                    ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                    $.when(frm.Show(), vm.LoadD).done(function (frmD, loadD) {
                        if (loadD == false) {//force close
                            frm.Close();
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