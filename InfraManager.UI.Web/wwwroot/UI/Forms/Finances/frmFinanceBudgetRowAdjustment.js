define(['knockout', 'jquery', 'ajax', 'formControl', 'dateTimeControl', 'usualForms'], function (ko, $, ajax, formControl, dtLib, fhModule) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //
            self.ID = null;
            self.CanEdit = ko.observable(true);
            //
            self.ID = null;
            self.TypeString = ko.observable('');
            self.Date = ko.observable(new Date());
            self.DateString = ko.observable(dtLib.Date2String(self.Date(), true));
            self.InitiatorID = ko.observable(null);
            self.InitiatorFullName = ko.observable('');
            self.Note = ko.observable('');
            //
            self.OldID = ko.observable(null);
            self.OldIdentifier = ko.observable(null);
            self.OldName = ko.observable(null);
            self.OldTotalSum = ko.observable(null);
            self.OldSum1 = ko.observable(null);
            self.OldSum2 = ko.observable(null);
            self.OldSum3 = ko.observable(null);
            self.OldSum4 = ko.observable(null);
            //
            self.NewID = ko.observable(null);
            self.NewIdentifier = ko.observable(null);
            self.NewName = ko.observable(null);
            self.NewTotalSum = ko.observable(null);
            self.NewSum1 = ko.observable(null);
            self.NewSum2 = ko.observable(null);
            self.NewSum3 = ko.observable(null);
            self.NewSum4 = ko.observable(null);
            //
            self.GetAdjustmentContext = function () {
                return {
                    InitiatorID: self.InitiatorID(),
                    Note: self.Note(),
                    DateJS: dtLib.GetMillisecondsSince1970(self.Date() == null ? new Date() : self.Date()),
                    OldID: self.OldID(),
                    NewID: self.NewID(),
                    Files: self.attachmentsControl = null ? null : self.attachmentsControl.GetData(),
                };
            };
            //
            self.AfterRenderBlockD = $.Deferred();
            self.AfterRender = function (editor, elements) {
                $.when(self.AfterRenderBlockD).done(function () {
                    var $frm = $('#' + self.frm.GetRegionID());
                    //
                    {//initiator
                        self.initiatorSearcher = null;
                        self.initiatorSearcherD = $.Deferred();
                        {
                            var fh = new fhModule.formHelper();
                            var initiatorD = fh.SetTextSearcherToField(
                                $frm.find('.initiator'),
                                'WebUserSearcher',
                                null,
                                [],
                                function (objectInfo) {//select
                                    self.InitiatorFullName(objectInfo.FullName);
                                    self.InitiatorID(objectInfo.ID);
                                },
                                function () {//reset
                                    self.InitiatorFullName('');
                                    self.InitiatorID(null);
                                },
                                function (selectedItem) {//close
                                    if (!selectedItem) {
                                        self.InitiatorFullName('');
                                        self.InitiatorID(null);
                                    }
                                });
                            $.when(initiatorD).done(function (ctrl) {
                                self.initiatorSearcher = ctrl;
                                self.initiatorSearcherD.resolve(ctrl);
                                ctrl.CurrentUserID = self.CurrentUserID;
                            });
                        };
                    }
                    //
                    var $date = $frm.find('.date');
                    showSpinner($date[0]);
                    require(['dateTimePicker'], function () {
                        if (locale && locale.length > 0)
                            $.datetimepicker.setLocale(locale.substring(0, 2));
                        var control = $date.datetimepicker({
                            startDate: self.Date(),
                            closeOnDateSelect: true,
                            format: 'd.m.Y',
                            mask: '39.19.9999',
                            timepicker: false,
                            dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                            value: self.Date(),
                            validateOnBlur: true,
                            onSelectDate: function (current_time, $input) {
                                self.Date(current_time);
                                self.DateString(dtLib.Date2String(current_time, true));
                            }
                        });
                        hideSpinner($date[0]);
                        //
                        var handler = null;
                        handler = self.DateString.subscribe(function (newValue) {
                            if (self.isLoading())
                                return;
                            if ($.contains(window.document, $date[0])) {
                                var dt = $date.datetimepicker('getValue');
                                //
                                if (!newValue || newValue.length == 0)
                                    self.Date(null);//clear field => reset value
                                else if (dtLib.Date2String(dt, true) != newValue) {
                                    self.Date(null);//value incorrect => reset value
                                    self.DateString('');
                                }
                                else
                                    self.Date(dt);
                            }
                            else {
                                handler.dispose();
                                $date.datetimepicker('destroy');
                            }
                        });
                    });
                    //
                    //attachments
                    {
                        self.attachmentsControl = null;
                        require(['fileControl'], function (fcLib) {
                            var attachmentsElement = $('#' + self.frm.GetRegionID()).find('.documentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.bAddDocument');
                            self.attachmentsControl.ReadOnly(self.ID == null ? false : true);
                            self.attachmentsControl.RemoveFileAvailable(!self.attachmentsControl.ReadOnly());
                            if (self.ID)
                                self.attachmentsControl.Initialize(self.ID);
                        });
                    }
                });
            };
            //
            self.isLoading = ko.observable(false);
            self.Load = function (id, oldRow, newRow) {
                var retval = $.Deferred();
                self.ID = id;
                //
                if (!id) {
                    $.when(userD).done(function (user) {
                        self.InitiatorID(user.UserID);
                        self.InitiatorFullName(user.UserFullName);
                    });
                    //
                    if (oldRow) {
                        self.OldID(oldRow.ID);
                        self.OldIdentifier(oldRow.Identifier);
                        self.OldName(oldRow.Name);
                        self.OldTotalSum(oldRow.TotalSum);
                        self.OldSum1(oldRow.Sum1);
                        self.OldSum2(oldRow.Sum2);
                        self.OldSum3(oldRow.Sum3);
                        self.OldSum4(oldRow.Sum4);
                    }
                    if (newRow) {
                        self.NewID(newRow.ID);
                        self.NewIdentifier(newRow.Identifier);
                        self.NewName(newRow.Name);
                        self.NewTotalSum(newRow.TotalSum);
                        self.NewSum1(newRow.Sum1);
                        self.NewSum2(newRow.Sum2);
                        self.NewSum3(newRow.Sum3);
                        self.NewSum4(newRow.Sum4);
                    }
                    //
                    if (oldRow == null && newRow != null)
                        self.TypeString(getTextResource('FinanceBudgetRowAdjustmentType_AddRow'));
                    else if (oldRow != null && newRow == null)
                        self.TypeString(getTextResource('FinanceBudgetRowAdjustmentType_RemoveRow'));
                    else if (oldRow != null && newRow != null) {
                        if (oldRow.TotalSum < newRow.TotalSum)
                            self.TypeString(getTextResource('FinanceBudgetRowAdjustmentType_IncrementRow'));
                        else
                            self.TypeString(getTextResource('FinanceBudgetRowAdjustmentType_DecrementRow'));
                    }
                    //
                    retval.resolve(true);
                    return retval;
                }
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/finApi/GetFinanceBudgetRowAdjustment',
                    method: 'GET',
                    data: { ID: self.ID }
                },
                function (adjustmentResult) {
                    if (adjustmentResult.Result === 0 && adjustmentResult.Data) {
                        var obj = adjustmentResult.Data;
                        //
                        self.isLoading(true);
                        {
                            self.TypeString(obj.TypeString);
                            self.Date(new Date(parseFloat(obj.UtcDateJS)));
                            self.InitiatorID(obj.InitiatorID);
                            self.InitiatorFullName(obj.InitiatorFullName);
                            self.Note(obj.Note);
                            //
                            self.OldID(obj.OldID);
                            self.OldIdentifier(obj.OldIdentifier);
                            self.OldName(obj.OldName);
                            self.OldTotalSum(obj.OldTotalSum);
                            self.OldSum1(obj.OldSum1);
                            self.OldSum2(obj.OldSum2);
                            self.OldSum3(obj.OldSum3);
                            self.OldSum4(obj.OldSum4);
                            //
                            self.NewID(obj.NewID);
                            self.NewIdentifier(obj.NewIdentifier);
                            self.NewName(obj.NewName);
                            self.NewTotalSum(obj.NewTotalSum);
                            self.NewSum1(obj.NewSum1);
                            self.NewSum2(obj.NewSum2);
                            self.NewSum3(obj.NewSum3);
                            self.NewSum4(obj.NewSum4);
                            //
                            self.CanEdit(false);
                        }
                        self.isLoading(false);
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[FinanceBudgetRowAdjustment.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
        },

        ShowDialog: function (id, oldRow, newRow, isSpinnerActive) {
            var retval = $.Deferred();
            //
            $.when(operationIsGrantedD(859)).done(function (can_update) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var vm = new module.ViewModel();
                var bindElement = null;
                var undo = false;
                //
                var buttons = {};
                if (can_update == true && !id)
                    buttons[getTextResource('ButtonSave')] = function () {
                        var context = vm.GetAdjustmentContext();
                        if (vm.CanEdit() == false) {
                            context = undefined;
                            undo = true;
                        }
                        retval.resolve(context);
                        frm.Close();
                    }
                buttons[getTextResource('Close')] = function () {
                    undo = true;
                    retval.resolve(undefined);
                    frm.Close();
                }
                //
                frm = new formControl.control(
                        'region_frmFinanceBudgetRowAdjustment',//form region prefix
                        'setting_frmFinanceBudgetRowAdjustment',//location and size setting
                        getTextResource('FinanceBudgetRowAdjustment'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        760, 400,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Finances/frmFinanceBudgetRowAdjustment\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized) {
                    retval.resolve(undefined);
                    return retval.promise();//form with that region and settingsName was open
                }
                frm.BeforeClose = function () {
                    if (undo == true && vm.attachmentControl)
                        vm.attachmentControl.RemoveUploadedFiles();
                    return true;
                };
                frm.ExtendSize(800, 500);//normal size
                vm.frm = frm;
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show(), vm.Load(id, oldRow, newRow)).done(function (frmD, loadD) {
                    vm.AfterRenderBlockD.resolve();
                    hideSpinner();
                });
            });
            //
            return retval.promise();
        }
    };
    return module;
});