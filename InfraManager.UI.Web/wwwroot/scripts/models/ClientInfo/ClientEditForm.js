define(['knockout', 'jquery', 'ajax', 'formControl', 'usualForms', 'dateTimeControl', 'models/ClientInfo/ClientInfoFields', 'jqueryStepper'],
    function (ko, $, ajax, formControl, fhModule, dtLib, m_object) {
    var module = {
        ViewModel: function (options) {
            var self = this;
            //            
            self.ajaxControl = new ajax.control();
            //
            self.CanEdit = ko.observable(true);
            self.object = ko.observable(new m_object.client(options));
            //
            self.Save = function () {
                var retval = $.Deferred();

                $.when(self.object().saveClick()).done(function (data) {
                if (!data) {
                    retval.promise();
                    return;
                }
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        contentType: "application/json",
                        url: '/api/users/' + data.ID,
                        method: 'PUT',
                        data: JSON.stringify(data)
                    },
                    null,
                    null,
                    function () {
                        hideSpinner();
                        retval.resolve(true);
                    });
                });
                //
                return retval.promise();
            };
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
                var newObject = null;

            };
        },

        ShowDialog: function (options, ClientObj) {
            $.when(operationIsGrantedD(71), operationIsGrantedD(241)).done(function (can_properties, can_update) {
                if (can_properties == false) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('OperationError'));
                    });
                    return;
                }
                //
                var frm = undefined;
                var vm = new module.ViewModel(options);
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
                            ClientObj.Load();
                            if (result) {
                                frm.Close();
                            }
                            });
                    }
                }
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { frm.Close(); }
                }
                if (can_update == true) {
                    buttons.push(bSave);
                }
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                    'ClientInfoEditor',//form region prefix
                    options.fieldName,//location and size setting
                    getTextResource('EditUser') + ' ' + ClientObj.FullName(), //caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    300, 400,//minSize
                    buttons,//form buttons
                    function () {
                        ko.cleanNode(bindElement);
                    },//afterClose function
                    'data-bind="template: {name: \'ClientInfo/EditSelectClientInfo\', afterRender: AfterRender}"'//attributes of form region
                );
                if (!frm.Initialized) {
                    return;//form with that region and settingsName was open
                }
                frm.settingsExists = false;
                frm.ExtendSize(455, 594);//normal size
                vm.frm = frm;
                var $region = $('#' + frm.GetRegionID());
                vm.object().$region = $region;
                vm.CanEdit(can_properties);
                //
                bindElement = document.getElementById(frm.GetRegionID());
                
                vm.object().applyTemplate = function (Model) {
                    ko.applyBindings(Model, bindElement);
                };
                //
                $.when(frm.Show(), vm.object().loadFields(options)).done(function (frmD, loadD) {
                    hideSpinner();
                });
            });
        },

    };
    return module;
});