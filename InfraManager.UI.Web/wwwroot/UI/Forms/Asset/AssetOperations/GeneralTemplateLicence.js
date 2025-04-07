define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    'dateTimeControl'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        dtLib
    ) {
        var module = {
            GeneralTemplateLicence: function (vm) {
                var self = this;
                self.$region = vm.$region;

                self.Template = '../UI/Forms/Asset/AssetOperations/GeneralTemplateLicence';

                self.Load = function () {
                    self.getSoft();
                };
                self.dispose = function () {

                };

                self.AfterRender = function () {
                    if (vm.IsEditMode()) {
                        self.InitDtp('.effective-date', self.periodEnd, self.periodEndDateTime, self.controlEnd);
                        self.$region.find('.effective-date').focus();
                    }
                };

                self.CanEdit = ko.observable(true);
                self.ParentSoftwareLicence = ko.observable(null);

                self.SoftwareLicenceObject = ko.observable(null);

                self.SoftwareLicenceUpdate = ko.observable(null);

                self.SoftwareLicenceUpdateObject = ko.computed(function () {
                    if (vm.IsEditMode()) {
                        return self.ParentSoftwareLicence() != null;
                    }
                });

                //Проставлена ли связанная лицензия
                self.IsParentSoftwareLicenceExists = ko.computed(function () {
                    return self.ParentSoftwareLicence() != null;
                });

                //Проставлено ли основание
                self.IsChildSoftwareLicenceExists = ko.computed(function () {
                    return self.SoftwareLicenceObject() != null;
                });

                self.IsParentSelectable = ko.computed(function () {
                    return vm.IsParentSelectable();
                });

                self.IsChildSelectable = ko.computed(function () {
                    return vm.IsChildSelectable();
                });

                self.IsEditMode = ko.observable(vm.IsEditMode());

                self.ParentSoftwareLicenceName = ko.computed(function () {

                    if (self.ParentSoftwareLicence() == null)
                        return "";

                    let inventoryNumber =  (self.ParentSoftwareLicence().hasOwnProperty('InventoryNumber')) 
                        ? self.ParentSoftwareLicence().InventoryNumber 
                        : self.ParentSoftwareLicence().InvNumber;                  
                    

                    let newParentSoftwareLicenceName = self.ParentSoftwareLicence().Name + ', ' + self.ParentSoftwareLicence().ManufacturerName;
                    if (inventoryNumber && inventoryNumber.InventoryNumber != '' && inventoryNumber != undefined) {
                        newParentSoftwareLicenceName += ', инв. № ' + inventoryNumber;
                    }

                    return newParentSoftwareLicenceName;
                });

                self.ProductType = ko.computed(function () {
                    return vm.SoftwareLicenceTypeName;
                });

                //Дата вступления в силу
                self.EffectiveDate = ko.computed(function () {
                    return vm.EffectiveDate;
                });

                self.Reason = ko.computed(function () {
                    
                    if (vm.hasOwnProperty('Contract') && vm.Contract() != null) {                                    
                        const str = vm.Contract().Description + ", " +  vm.Contract().FullName.toString() + ", " + dtLib.Date2String(new Date(vm.Contract().UtcDateRegistered / 1), true);
                        return str;                        
                    }

                    if (self.SoftwareLicenceObject() == null)
                        return "";

                    let inventoryNumber =  (self.SoftwareLicenceObject().hasOwnProperty('InventoryNumber'))
                        ? self.SoftwareLicenceObject().InventoryNumber
                        : self.SoftwareLicenceObject().InvNumber;
                    
                    let newParentSoftwareLicenceName = self.SoftwareLicenceObject().Name + ', ' + self.SoftwareLicenceObject().ManufacturerName;
                    if (inventoryNumber && inventoryNumber != '' && inventoryNumber != undefined) {
                        newParentSoftwareLicenceName += ', инв. № ' + inventoryNumber;
                    }

                    return newParentSoftwareLicenceName;

                });

                self.LimitInDays = ko.computed(function () {
                    return (self.SoftwareLicenceObject() != null) ? self.SoftwareLicenceObject().LimitInDays : "";
                });

                self.EndDate = ko.computed(function () {
                    return (self.SoftwareLicenceObject() != null) ? new Date(parseInt(self.SoftwareLicenceObject().EndDate)) : "";
                });

                self.Balance = ko.computed(function () {
                    if (self.SoftwareLicenceUpdate() != null) {
                        return self.SoftwareLicenceUpdate().NewReferenceCount;
                    } else {
                        return (self.SoftwareLicenceObject() != null) ? self.SoftwareLicenceObject().Balance : "";
                    }
                });

                self.BalanceParent = ko.computed(function () {
                    if (self.SoftwareLicenceUpdate() != null) {
                        return self.SoftwareLicenceUpdate().OldReferenceCount;
                    } else {
                        return (self.ParentSoftwareLicence() != null) ? self.ParentSoftwareLicence().Balance : "";
                    }
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
                        IsDisabled: true,
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
                };

                self.getSoft = function () {
                    var retval = $.Deferred();
                    self.ajaxControl = new ajaxLib.control();
                    //заполнение основания или основной лицензии в зависимости от того, у кого вызывали контекстное меню
                    if (self.IsEditMode()) {
                        let ID = vm.selectedObjects[0].ID;

                        if ( vm.selectedObjects[0].hasOwnProperty('SoftwareLicenceID') && vm.selectedObjects[0].SoftwareLicenceID) {
                            ID = vm.selectedObjects[0].SoftwareLicenceID;
                        }
                        
                        self.ajaxControl.Ajax(null,
                            {
                                url: '/sdApi/GetSoftwareLicence',
                                method: 'GET',
                                data: { ID: ID }
                            },
                            function (response) {
                                if (response.Result === 0 && response.SoftwareLicence) {
                                    var obj = response.SoftwareLicence;

                                    if (self.IsParentSelectable()) {
                                        self.SoftwareLicenceObject(obj);
                                    }
                                    else {
                                        self.ParentSoftwareLicence(obj);
                                    }
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
                    //readOnly для просмотра карточки обновления
                    else {
                        //заполнение объекта обновления
                        self.SoftwareLicenceUpdate(vm.selectedObjects[0]);

                        //заполнение лицензии, к которой применили операцию
                        self.ajaxControl.Ajax(null,
                            {
                                url: '/sdApi/GetSoftwareLicence',
                                method: 'GET',
                                data: { ID: vm.selectedObjects[0].SoftwareLicenceID }
                            },
                            function (response) {
                                if (response.Result === 0 && response.SoftwareLicence) {
                                    var obj = response.SoftwareLicence;
                                    self.ParentSoftwareLicence(obj);

                                    //заполнение основания-лицензии, с помощью которой произвели операцию
                                    self.ajaxControl.Ajax(null,
                                        {
                                            url: '/sdApi/GetSoftwareLicence',
                                            method: 'GET',
                                            data: { ID: vm.selectedObjects[0].CorrespondingLicenceID }
                                        },
                                        function (response) {
                                            if (response.Result === 0 && response.SoftwareLicence) {
                                                var obj = response.SoftwareLicence;
                                                self.SoftwareLicenceObject(obj);
                                                retval.resolve(true);
                                            }
                                            else if (!(response.Result ===  6)){
                                                retval.resolve(false);
                                                require(['sweetAlert'], function () {
                                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLicenceApplying.js, getSoft]', 'error');
                                                });
                                            }
                                        });
                                }
                                else {
                                    retval.resolve(false);
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmLicenceApplying.js, getSoft]', 'error');
                                    });
                                }
                            });
                    }
                };

                //открыть форму просмотра основания 
                self.OpenCorrespondingObjectForm = function () {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        if (vm.hasOwnProperty('Contract') && vm.Contract() != null) {
                            fh.ShowServiceContract(vm.Contract().ID);
                        }
                        else {
                            fh.ShowSoftwareLicenceForm(self.SoftwareLicenceObject().ID);    
                        }                        
                    });
                };

                //открыть форму просмотра связанной лицензии
                self.OpenParentSoftwareLicenceForm = function () {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        fh.ShowSoftwareLicenceForm(self.ParentSoftwareLicence().ID);
                    });
                };

                //редактирование связанной лицензии
                self.EditParentSoftwareLicence = function () {
                    $("#searcherParentSoftwareLicenceElem").blur();
                    if (!self.CanEdit()) {
                        return;
                    }

                    require(['ui_forms/Asset/AssetOperations/frmSoftwareTableSelector'], function (fhModule) {

                        let softwareModel;
                        let SoftwareLicenceSchemeID;
                        let LicenceType;
                        let ManufacturerName;

                        if (vm.IsHasFilter) {
                            if (self.IsParentSelectable()) {
                                softwareModel = self.SoftwareLicenceObject().SoftwareModelName;
                                SoftwareLicenceSchemeID = self.SoftwareLicenceObject().SoftwareLicenceSchemeID;
                                ManufacturerName = self.SoftwareLicenceObject().ManufacturerName;
                                LicenceType = self.SoftwareLicenceObject().LicenceType;
                            } else {
                                softwareModel = self.ParentSoftwareLicence().SoftwareModelName;
                                SoftwareLicenceSchemeID = self.ParentSoftwareLicence().SoftwareLicenceSchemeID;
                                ManufacturerName = self.ParentSoftwareLicence().ManufacturerName;
                                LicenceType = self.ParentSoftwareLicence().LicenceType;
                            }
                        }

                        var form = new fhModule.Form(vm.FilterProductCatalogTypeID, softwareModel, SoftwareLicenceSchemeID,ManufacturerName, LicenceType, function (softwareLicenceParent) {
                            if (!softwareLicenceParent)
                                return;

                            if (self.IsParentSelectable()) {
                                self.ParentSoftwareLicence(softwareLicenceParent);
                            } else {
                                self.SoftwareLicenceObject(softwareLicenceParent);
                            }
                        });
                        form.Show();
                    });

                };

                self.ClearParentSoftwareLicence = function () {
                    self.ParentSoftwareLicence(null);
                };

                self.ClearChildSoftwareLicence = function () {
                    self.SoftwareLicenceObject(null);
                }

            }

        };
        return module;
    });