define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'dateTimeControl', './../Asset/Controls/SupplierList', 'jqueryStepper'], function (ko, $, ajax, formControl, fhModule, dtLib, supplierList) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.frm = null;
            self.saveFunc = null;//set in frmContract_generalTab.js
            self.list = new supplierList.List(self);
            //
            self.list.SelectedItemsChanged = function (checkedItemsCount) {
                if (!self.frm)
                    return;
                //
                var buttons = [];
                var bSave = {
                    text: getTextResource('Select'),
                    click: function () {
                        self.Save();
                        self.frm.Close();
                    }
                }
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { self.frm.Close(); }
                }
                if (checkedItemsCount === 1)
                    buttons.push(bSave);
                buttons.push(bCancel);
                //
                self.frm.UpdateButtons(buttons);
            };
            //
            self.Save = function () {
                if (!self.saveFunc)
                    return;
                //
                var selected = self.list.listView.rowViewModel.checkedItems();
                if (selected.length !== 1)
                    return;
                //
                self.saveFunc(selected[0]);
            };
            //
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
            };
        },

        ShowDialog: function (saveFunc, isSpinnerActive) {
            $.when(operationIsGrantedD(217)).done(function (can_properties) {
                //OPERATION_ADD_SUPPLIER = 218,
                //OPERATION_UPDATE_SUPPLIER = 285, 
                //OPERATION_PROPERTIES_SUPPLIER = 217,
                //OPERATION_DELETE_SUPPLIER = 219,
                if (can_properties == false) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('OperationError'));
                    });
                    return;
                }
                //
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var vm = new module.ViewModel();
                var bindElement = null;
                //
                var buttons = [];
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { frm.Close(); }
                }
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                        'region_frmSupplierList',//form region prefix
                        'setting_frmSupplierList',//location and size setting
                        getTextResource('SupplierList'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        700, 400,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                            vm.list.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/frmSupplierList\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                vm.frm = frm;
                vm.saveFunc = saveFunc;
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show()).done(function (frmD) {
                    hideSpinner();
                });
            });
        },

    };
    return module;
});