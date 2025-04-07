define(['knockout', 'jquery', 'ajax', 'sweetAlert', 'assetForms'], function (ko, $, ajax, swal, fhModule) {
    var module = {
        Tab: function (vm) {

            var self = this;

            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('SoftwareInstallation_Form_GeneralTabName');

            self.Template = '../UI/Forms/Asset/SoftwareInstallation/frmSoftwareInstallation_generalTab';

            self.IconCSS = 'generalTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.canEdit = vm.CanEdit;//for userlib
            //
            self.$region = vm.$region;

            self.DateCreatedString = function () {
                var obj = vm.object();
                return obj.CreateDate();
            }
          
            self.DateInstalledString = function () {
                var obj = vm.object();
                return obj.InstallationDate();
            }
          
            //when object changed
            self.Initialize = function (obj) {
            };
            //when tab selected
            self.load = function () {
            };

            self.AfterRender = function () {
            }

            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
            };         

            //редактирование поля "Модель"
            self.EditModel = function () {
                if (!vm.CanEdit()) {
                    return;
                }
                require(['ui_forms/Asset/AssetOperations/templateParamsUpdate/frmSoftwareTableSelectorModel'], function (fhModule) {
                    var form = new fhModule.Form(
                        vm.object,
                        function (softwareModel) {
                            if (!softwareModel)
                                return;

                            var object = vm.object();
                            if (!vm.isAddNewSoftwareInstallation()) {
                                showSpinner();
                                var oldValue = object.ModelID();
                                var newValue = softwareModel.ID;

                                var data = {
                                    ID: vm.object().ID(),
                                    ObjClassID: vm.object().ClassID,
                                    Field: 'SoftwareModelID',
                                    OldValue: JSON.stringify({ 'id': oldValue }),
                                    NewValue: JSON.stringify({ 'id': newValue }),
                                    ReplaceAnyway: false
                                };

                                self.ajaxControl.Ajax(
                                    null,//self.$region, two spinner problem
                                    {
                                        dataType: "json",
                                        method: 'POST',
                                        url: vm.baseUrl + '/' + vm.setFieldUrl,
                                        data: data
                                    },
                                    function (retModel) {
                                        if (retModel) {
                                            var result = retModel.ResultWithMessage.Result;
                                            //
                                            if (result === 0) {
                                                object.ModelID(softwareModel.ID);
                                                object.ModelName(softwareModel.Name + ' / ' + softwareModel.Version);
                                                vm.raiseObjectModified();
                                            }
                                            else {
                                                hideSpinner();
                                                require(['sweetAlert'], function () {
                                                    swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                                });
                                            }
                                        }
                                        hideSpinner();
                                    });
                            } else {
                                //Модель ПО / версия ПО
                                object.ModelName(softwareModel.Name + ' / ' + softwareModel.Version);
                                object.ModelID(softwareModel.ID);
                            }
                        },
                        false,
                        self.$region);
                    form.UseNoCommercialModel(true);
                    form.Show();
                });

            };

            self.OpenSoftwareModelForm = function () {

            };
            //Edit InstallDate
            self.SelectInstallDate = function () {
                if (!vm.CanEdit())
                    return;
                //
                showSpinner();
                var object = vm.object();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'InstallDate',
                        fieldFriendlyName: getTextResource('SoftwareInstallation_Form_InstallationDate'),
                        oldValue: object.InstallationDate(),
                        allowNull: true,
                        maxLength: 500,
                        onSave: function (newText) {
                            object.InstallationDate(newText);
                            self.raiseObjectModified();
                        },
                        nosave: vm.isAddNewSoftwareInstallation(),
                        urlSetField: vm.baseUrl + '/' + vm.setFieldUrl,
                        allowTime: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                });

            };
            //Edit CreateDate
            self.SelectCreateDate = function () {

            };
            self.OpenDeviceForm = function () {
                var classid = vm.object().DeviceClass();
                if (classid != 6 && classid != 415) {
                    swal('Недопустимый класс устройства. Выберите другое устройство', '', 'error');
                }
                else {
                    require(['assetForms'], function (assetForms) {
                        var asset_fh = new assetForms.formHelper(true);
                        var id = vm.object().DeviceID();
                        asset_fh.ShowAssetForm(id, classid);
                    });
                }
            };

            // Device
            self.OpenSelectDevice = function () {
                var retvalD = $.Deferred();
                //
                if (vm.object() != null) {
                    var fh = new fhModule.formHelper();
                    fh.ShowAssetLink({
                        AllowedClassIDs: [6,415],
                        ID: vm.object().DeviceID(),
                        ClassID: vm.object().DeviceClass(),
                        SelectOnlyOne: true,
                        Caption: getTextResource('SoftwareInstallation_SelectDevice')
                    }, function (newValues) {
                        if (!newValues || newValues.length != 1)
                            return;
                        if (!vm.isAddNewSoftwareInstallation()) {
                            showSpinner();
                            var oldValue = vm.object().DeviceClass() + ':' + vm.object().DeviceID();
                            var newValue = newValues[0].ClassID + ':' + newValues[0].ID;

                            var data = {
                                ID: vm.object().ID(),
                                ObjClassID: vm.object().ClassID,
                                Field: 'DeviceID',
                                OldValue: JSON.stringify({ 'id': oldValue }),
                                NewValue: JSON.stringify({ 'id': newValue }),
                                ReplaceAnyway: false
                            };

                            self.ajaxControl.Ajax(
                                null,//self.$region, two spinner problem
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    url: vm.baseUrl + '/' + vm.setFieldUrl,
                                    data: data
                                },
                                function (retModel) {
                                    if (retModel) {
                                        var result = retModel.ResultWithMessage.Result;
                                        //
                                        if (result === 0) {
                                            vm.object().DeviceID(newValues[0].ID);
                                            vm.object().DeviceClass(newValues[0].ClassID);
                                            vm.object().DeviceName(newValues[0].Name);
                                            vm.raiseObjectModified();
                                        }
                                        else {
                                            hideSpinner();
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                            });
                                        }
                                    }
                                    hideSpinner();
                                });
                        } else {
                            vm.object().DeviceID(newValues[0].ID);
                            vm.object().DeviceClass(newValues[0].ClassID);
                            vm.object().DeviceName(newValues[0].Name);
                        }
                        //  
                        retvalD;
                    });
                }
                else retvalD.resolve();
                //
                return retvalD;

            };

            // InstallPath
            self.editInstallPath = function () {
                if (!vm.CanEdit()) {
                    return;
                }
                //
                showSpinner();
                var object = vm.object();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'InstallPath',
                        fieldFriendlyName: getTextResource('SoftwareInstallation_Form_InstallationPath'),
                        oldValue: object.InstallationPath(),
                        allowNull: true,
                        maxLength: 500,
                        onSave: function (newText) {
                            object.InstallationPath(newText);
                            vm.raiseObjectModified();
                        },
                        nosave: vm.isAddNewSoftwareInstallation(),
                        urlSetField: vm.baseUrl + '/' + vm.setFieldUrl
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };


        }
    };
    return module;
});