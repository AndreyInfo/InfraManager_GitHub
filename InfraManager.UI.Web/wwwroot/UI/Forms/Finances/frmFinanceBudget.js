define(['knockout', 'jquery', 'ajax', 'formControl', 'dateTimeControl'], function (ko, $, ajax, formControl, dtLib) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //
            self.modes = {
                general: 'general',
                adjustments: 'adjustments'
            };
            self.mode = ko.observable(self.modes.general);
            self.mode.subscribe(function (newValue) {
                if (newValue == self.modes.adjustments)
                    self.LoadAdjustmentList();
            });
            //
            self.ClassID = 179;
            self.ID = null;
            self.CanEdit = ko.observable(true);
            //
            self.Name = ko.observable('');
            self.Year = ko.observable((new Date()).getYear() + 1900);
            self.StateString = ko.observable('');
            self.DateCreated = ko.observable(new Date());
            self.DateCreatedString = ko.observable(dtLib.Date2String(self.DateCreated(), true));
            self.Note = ko.observable('');
            self.RowVersion = '';
            //
            self.AdjustmentList = ko.observableArray([]);
            self.adjustmentListLoaded = false;
            self.LoadAdjustmentList = function () {
                if (self.ID == null || self.adjustmentListLoaded == true)
                    return;
                self.adjustmentListLoaded = true;
                //
                self.ajaxControl.Ajax($('#' + self.frm.GetRegionID()),
                {
                    url: '/finApi/GetFinanceBudgetRowAdjustmentInfoList',
                    method: 'GET',
                    data: { FinanceBudgetID: self.ID }
                },
                function (result) {
                    if (result.Result === 0 && result.List) {
                        var list = result.List;
                        //
                        for (var i = 0; i < list.length; i++) {
                            var a = new module.AdjustmentInfo(list[i]);
                            self.AdjustmentList().push(a);
                        }
                        //
                        self.AdjustmentList.valueHasMutated();
                    }
                    else {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[FinanceBudgetForm.js, LoadAdjustmentList]', 'error');
                        });
                    }
                });
            };
            //
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
                //
                var $year = $frm.find('.year');
                showSpinner($year[0]);
                require(['jqueryStepper'], function () {
                    $year.stepper({
                        type: 'int',
                        floatPrecission: 0,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [1900, 9999],
                        onStep: function (val, up) {
                            self.Year(val);
                        }
                    });
                    hideSpinner($year[0]);
                });
                //
                var $dateCreated = $frm.find('.dateCreated');
                showSpinner($dateCreated[0]);
                require(['dateTimePicker'], function () {
                    if (locale && locale.length > 0)
                        $.datetimepicker.setLocale(locale.substring(0, 2));
                    var control = $dateCreated.datetimepicker({
                        startDate: self.DateCreated(),
                        closeOnDateSelect: true,
                        format: 'd.m.Y',
                        mask: '39.19.9999',
                        timepicker: false,
                        dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                        value: self.DateCreated(),
                        validateOnBlur: true,
                        onSelectDate: function (current_time, $input) {
                            self.DateCreated(current_time);
                            self.DateCreatedString(dtLib.Date2String(current_time, true));
                        }
                    });
                    hideSpinner($dateCreated[0]);
                    //
                    var handler = null;
                    handler = self.DateCreatedString.subscribe(function (newValue) {
                        if (self.isLoading())
                            return;
                        if ($.contains(window.document, $dateCreated[0])) {
                            var dt = $dateCreated.datetimepicker('getValue');
                            //
                            if (!newValue || newValue.length == 0)
                                self.DateCreated(null);//clear field => reset value
                            else if (dtLib.Date2String(dt, true) != newValue) {
                                self.DateCreated(null);//value incorrect => reset value
                                self.DateCreatedString('');
                            }
                            else
                                self.DateCreated(dt);
                        }
                        else {
                            handler.dispose();
                            $dateCreated.datetimepicker('destroy');
                        }
                    });
                });
            };
            //
            self.isLoading = ko.observable(false);
            self.Load = function (id) {
                var retval = $.Deferred();
                self.ID = id;
                //
                if (!id) {
                    retval.resolve(true);
                    return retval;
                }
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/finApi/GetFinanceBudget',
                    method: 'GET',
                    data: { FinanceBudgetID: self.ID }
                },
                function (bugetResult) {
                    if (bugetResult.Result === 0 && bugetResult.Data) {
                        var obj = bugetResult.Data;
                        //
                        self.isLoading(true);
                        {
                            self.Name(obj.Name);
                            self.Year(obj.Year);
                            self.StateString(obj.StateString);
                            self.DateCreated(new Date(parseFloat(obj.UtcDateCreatedJS)));
                            self.DateCreatedString(dtLib.Date2String(self.DateCreated(), true));
                            self.Note(obj.Note);
                            self.RowVersion = obj.RowVersion;
                            if(self.CanEdit() == true)
                                self.CanEdit(obj.State == 0 ? true : false);
                        }
                        self.isLoading(false);
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[FinanceBudgetForm.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
            //
            self.Save = function () {
                var retval = $.Deferred();
                //                
                var data = {
                    'ID': self.ID,
                    'Name': self.Name(),
                    'Note': self.Note(),
                    'Year': self.Year(),
                    'UtcDateCreatedJS': dtLib.GetMillisecondsSince1970(self.DateCreated()),
                    'RowVersion': self.RowVersion
                };
                //    
                if (data.Name.trim().length == 0) {
                    require(['sweetAlert'], function (swal) {
                        swal(getTextResource('ParametersMustBeSet') + ': ' + getTextResource('Name'));
                    });
                    retval.resolve(false);
                    return;
                }
                //
                var frmElement = $('#' + self.frm.GetRegionID())[0];
                showSpinner(frmElement);
                self.ajaxControl.Ajax(null,
                    {
                        url: '/finApi/saveFinanceBudget',
                        method: 'POST',
                        dataType: 'json',
                        data: data
                    },
                    function (response) {
                        hideSpinner(frmElement);
                        if (response) {
                            if (response.Result == 0) {//ok 
                                if (response.Message && response.Message.length > 0)
                                    require(['sweetAlert'], function () {
                                        swal({
                                            title: response.Message,
                                            showCancelButton: false,
                                            closeOnConfirm: true,
                                            cancelButtonText: getTextResource('Continue')
                                        });
                                    });
                                //
                                retval.resolve(true);
                                $(document).trigger(self.ID == null ? 'local_objectInserted' : 'local_objectUpdated', [self.ClassID, response.ID, null]);//OBJ_Budget
                                return;
                            }
                            else if (response.Result === 4)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmFinanceBudget.js, Save]', 'error');
                                });
                            else if (response.Result === 5)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('ConcurrencyErrorWithoutQuestion'), 'error');
                                });
                            else if (response.Result === 7)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                });
                            else if (response.Result === 8)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), response.Message && response.Message.length > 0 ? response.Message : getTextResource('ValidationError'), 'error');
                                });
                        }
                        retval.resolve(null);
                    },
                    function (response) {
                        hideSpinner(frmElement);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmFinanceBudget.js, Save]', 'error');
                        });
                        retval.resolve(null);
                    });
                //
                return retval.promise();
            };
        },

        AdjustmentInfo: function (obj) {
            var self = this;
            //
            self.ID = obj.ID;
            self.RowIdentifier = obj.RowIdentifier;
            self.RowName = obj.RowName;
            self.TypeString = obj.TypeString;
            self.UtcDateString = dtLib.Date2String(new Date(parseFloat(obj.UtcDateJS)), true);
            self.InitiatorFullName = obj.InitiatorFullName;
            //
            self.Show = function () {
                showSpinner();
                require(['financeForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowFinanceBudgetRowAdjustment(self.ID, null, null);
                });                
            };
        },

        ShowDialog: function (id, isSpinnerActive) {
            $.when(operationIsGrantedD(855)).done(function (can_update) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var vm = new module.ViewModel();
                var bindElement = null;
                //
                var buttons = [];
                var bSave = {
                    text: getTextResource('ButtonSave'),
                    click: function () {
                        if (vm.CanEdit() == false) {
                            frm.Close();
                            return;
                        }
                        $.when(vm.Save()).done(function (result) {
                            if (result)
                                frm.Close();
                        });
                    }
                }
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { frm.Close(); }
                }
                if (can_update == true || !id)
                    buttons.push(bSave);
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                        'region_frmFinanceBudget',//form region prefix
                        'setting_frmFinanceBudget',//location and size setting
                        getTextResource('BudgetName'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        400, 370,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Finances/frmFinanceBudget\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                frm.ExtendSize(550, 400);//normal size
                vm.frm = frm;
                vm.CanEdit.subscribe(function (newValue) {
                    if (newValue == false) {
                        buttons = [bCancel];
                        frm.UpdateButtons(buttons);
                    }
                });
                vm.CanEdit(can_update);
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show(), vm.Load(id)).done(function (frmD, loadD) {
                    hideSpinner();
                });
            });
        }
    };
    return module;
});