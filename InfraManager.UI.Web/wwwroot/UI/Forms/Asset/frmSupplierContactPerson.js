define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'dateTimeControl', './SupplierContactPerson', 'jqueryStepper'], function (ko, $, ajax, formControl, fhModule, dtLib, m_object) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //
            self.ClassID = 384;//OBJ_SupplierContactPerson = 384
            self.CanEdit = ko.observable(true);
            self.object = ko.observable(new m_object.contactPerson());
            //
            self.Save = function () {
                var retval = $.Deferred();
                //
                if (!self.object().Surname()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('MustSetSurname'));
                    });
                    return;
                }
                //
                showSpinner();
                //
                var contact =
                    {
                        ID: self.object().ID(),
                        Name: self.object().Name(),
                        Surname: self.object().Surname(),
                        Patronymic: self.object().Patronymic(),
                        Phone: self.object().Phone(),
                        SecondPhone: self.object().SecondPhone(),
                        Email: self.object().Email(),
                        PositionName: self.object().PositionName(),
                        SupplierID: self.object().SupplierID(),
                        Note: self.object().Note(),
                    };
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/assetApi/EditSupplierContactPerson',
                    method: 'POST',
                    data: contact
                },
                function (response) {
                    if (response.Response.Result === 0 && response.NewModel) {
                        var obj = response.NewModel;
                        //
                        if (self.callback)
                            self.callback(self.object());
                        //$(document).trigger('local_objectUpdated', [384, obj.ID, obj.SupplierID]);//OBJ_SupplierContactPerson
                        hideSpinner();
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSupplierContactPerson.js, Save]', 'error');
                        });
                    }
                });
                //
                return retval.promise();
            };
            //
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
                var newObject = null;
                //self.list.init(newObject);
                //self.list.load();//reload active tab

            };
        },

        ShowDialog: function (id, supplierID, supplierName, callback, isSpinnerActive) {
            $.when(operationIsGrantedD(864), operationIsGrantedD(867)).done(function (can_properties, can_update) {
                //OPERATION_SupplierContactPerson_Add = 865,
                //OPERATION_SupplierContactPerson_Update = 867, 
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
                if (can_update == true)
                    buttons.push(bSave);
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                        'region_frmSupplierContactPerson',//form region prefix
                        'setting_frmSupplierContactPerson',//location and size setting
                        getTextResource('ContactPerson'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        300, 400,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/frmSupplierContactPerson\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                frm.ExtendSize(400, 450);//normal size
                vm.frm = frm;
                vm.callback = callback;
                vm.CanEdit(can_update);
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show(), vm.object().load(id, supplierID, supplierName)).done(function (frmD, loadD) {
                    hideSpinner();
                });
            });
        },

    };
    return module;
});