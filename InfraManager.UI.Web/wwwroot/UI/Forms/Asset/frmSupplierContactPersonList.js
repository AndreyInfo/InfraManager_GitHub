define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'dateTimeControl', './../Asset/Controls/SupplierContactPersonList', 'jqueryStepper'], function (ko, $, ajax, formControl, fhModule, dtLib, supplierContactPersonList) {
    var module = {
        ViewModel: function (supplierID, supplierName) {
            var self = this;
            //            
            self.frm = null;
            self.saveFunc = null;//set in frmContract_generalTab.js
            self.list = new supplierContactPersonList.List();
            self.list.supplierID = supplierID;
            self.list.supplierName = supplierName;
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
                if (checkedItemsCount != 0)
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
                if (selected.length == 0)
                    return;
                //
                self.saveFunc(selected);
            };
            //
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
            };
        },

        ShowDialog: function (contract, saveFunc, isSpinnerActive) {
            $.when(operationIsGrantedD(864)).done(function (can_properties) {
                //OPERATION_SupplierContactPerson_Properties = 864,
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
                var vm = new module.ViewModel(contract().SupplierID(), contract().SupplierName());
                var bindElement = null;
                //
                var buttons = [];
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { frm.Close(); }
                }
                buttons.push(bCancel);
                //
                var formCaption = getTextResource('Contract_SupplierContactList') + ' \'' + contract().SupplierName() + '\'';
                frm = new formControl.control(
                        'region_frmSupplierContactPersonList',//form region prefix
                        'setting_frmSupplierContactPersonList',//location and size setting
                        formCaption,//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        700, 400,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                            vm.list.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/frmSupplierContactPersonList\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                //frm.ExtendSize(600, 700);//normal size
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