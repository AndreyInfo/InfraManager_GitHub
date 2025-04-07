define(['knockout', 'jquery', 'ajax', 'formControl'], function (ko, $, ajax, formControl) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //           
            self.purchaseSpecificationID = null;
            self.workOrderID = null;
            //
            self.currentStateString = ko.observable('');
            self.stateList = ko.observableArray([]);
            self.selectedState = ko.observable(null);
            self.stateDataSource = function (options) {
                var data = self.stateList();
                options.callback({ data: data, total: data.length });
            };
            //
            self.isLoading = ko.observable(false);
            self.Load = function () {
                var retval = $.Deferred();
                //       
                self.ajaxControl.Ajax(null,
                {
                    url: '/finApi/GetContractOrAgreementInfoForMarkAsExecuted?purchaseSpecificationID=' + self.purchaseSpecificationID,
                    method: 'GET'
                },
                function (result) {
                    if (result.Result === 0) {
                        var list = result.StateList;
                        //
                        var stateList = [];
                        for (var i = 0; i < list.length; i++)
                            stateList.push(
                                {
                                    ID: list[i].ID,
                                    Name: list[i].Name
                                });
                        self.stateList(stateList);
                        self.currentStateString(result.CurrentState);
                        self.workOrderID = result.WorkOrderID;
                        self.isLoading(true);
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmExecutePurchaseSpecification.js, Load]', 'error');
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
                    'PurchaseSpecificationID': self.purchaseSpecificationID,
                    'NewStateID': self.selectedState() ? self.selectedState().ID : null
                };
                //    
                var frmElement = $('#' + self.frm.GetRegionID())[0];
                showSpinner(frmElement);
                self.ajaxControl.Ajax(null,
                    {
                        url: '/finApi/MarkAsExecutedOnContractOrAgreement',
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
                                $(document).trigger('local_objectUpdated', [381, self.purchaseSpecificationID, self.workOrderID]);//OBJ_PurchaseSpecification
                                return;
                            }
                            else if (response.Result === 4)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmExecutePurchaseSpecification.js, Save]', 'error');
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
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmExecutePurchaseSpecification.js, Save]', 'error');
                        });
                        retval.resolve(null);
                    });
                //
                return retval.promise();
            };
        },

        ShowDialog: function (purchaseSpecificationID, isSpinnerActive) {
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
            buttons.push(bSave);
            buttons.push(bCancel);
            //
            frm = new formControl.control(
                    'region_frmExecutePurchaseSpecification',//form region prefix
                    'setting_frmExecutePurchaseSpecification',//location and size setting
                    getTextResource('PurchaseSpecification_MarkAsExecuted'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    300, 170,//minSize
                    buttons,//form buttons
                    function () {
                        ko.cleanNode(bindElement);
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/Finances/frmExecutePurchaseSpecification\'}"'//attributes of form region
                );
            if (!frm.Initialized)
                return;//form with that region and settingsName was open
            frm.ExtendSize(400, 180);//normal size
            vm.frm = frm;
            vm.purchaseSpecificationID = purchaseSpecificationID;
            //
            bindElement = document.getElementById(frm.GetRegionID());
            ko.applyBindings(vm, bindElement);
            //
            $.when(frm.Show(), vm.Load()).done(function (frmD, loadD) {
                hideSpinner();
            });
        }
    };
    return module;
});