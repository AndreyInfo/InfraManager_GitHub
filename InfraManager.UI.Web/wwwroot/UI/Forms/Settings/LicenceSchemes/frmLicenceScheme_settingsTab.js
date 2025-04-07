define(['knockout', 'jquery', 'ajax', 'ui_controls/ExpressionEditor/ExpressionEditor'], function (ko, $, ajax, exEditor) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('LicenceScheme_Form_SettingsTabName');
            self.Template = '../UI/Forms/Settings/LicenceSchemes/frmLicenceScheme_settingsTab';
            self.IconCSS = 'settingsTab';
            //
            self.IsVisible = ko.observable(true);
            //

            {//fields
                self.historyList = ko.observableArray([]);
                self.isLoaded = false;
            }

            //
            //when object changed
            self.Initialize = function (obj) {
                self.isLoaded = false;
                self.historyList([]);
            };

            self.AfterRender = function () {
                self.initNumericUpDownControl('.installationLimits', vm.object().InstallationLimits, 0, 99999, vm.CanEdit() && vm.isAddNewLicenceScheme());
                self.initNumericUpDownControl('.InstallationLimitPerVM', vm.object().InstallationLimitPerVM, 0, 99999, vm.CanEdit() && vm.isAddNewLicenceScheme());
                self.initNumericUpDownControl('.IncreaseForVMStepper', vm.object().IncreaseForVM, 0, 99999, vm.CanEdit() && vm.isAddNewLicenceScheme());
            }

            self.initNumericUpDownControl = function (selector, ko_value, minValue, maxValue, is_enable) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmLicenceScheme');
                var $div = $frm.find(selector);
                showSpinner($div[0]);
                require(['jqueryStepper'], function () {
                    $div.stepper({
                        type: 'int',
                        floatPrecission: 0,
                        wheelStep: 1,
                        arrowStep: 1,
                        limit: [minValue, maxValue],
                        onStep: function (val, up) {
                             ko_value(val);
                        },
                        isReadonly: !is_enable
                    });
                    hideSpinner($div[0]);
                });
            };

            self.createComboBoxItem = function (simpleDictionary) {
                var thisObj = this;
                //
                thisObj.ID = simpleDictionary.ID ?? simpleDictionary.Id;
                thisObj.Name = simpleDictionary.Name;
            };

            self.createComboBoxHelper = function (container_selector, getUrl, comboBoxFunc) {
                var thisObj = this;
                if (!comboBoxFunc)
                    comboBoxFunc = self.createComboBoxItem;
                //
                thisObj.SelectedItem = ko.observable(null);
                //
                thisObj.ItemList = ko.observableArray([]);
                thisObj.ItemListD = $.Deferred();
                thisObj.getItemList = function (options) {
                    var data = thisObj.ItemList();
                    options.callback({ data: data, total: data.length });
                };
                //
                thisObj.ajaxControl = new ajax.control();
                thisObj.LoadList = function () {
                    thisObj.ajaxControl.Ajax($(container_selector),
                        {
                            url: getUrl,
                            method: 'GET'
                        },
                        function (response) {
                            if (response) {
                                thisObj.ItemList.removeAll();
                                //
                                $.each(response, function (index, simpleDictionary) {
                                    var u = new comboBoxFunc(simpleDictionary);
                                    thisObj.ItemList().push(u);
                                });
                                thisObj.ItemList.valueHasMutated();
                            }
                            thisObj.ItemListD.resolve();
                        });
                };
                //
                thisObj.GetObjectInfo = function (classID) {
                    return thisObj.SelectedItem() ? { ID: thisObj.SelectedItem().Id, ClassID: classID, FullName: thisObj.SelectedItem().Name } : null;
                };
                thisObj.SetSelectedItem = function (id) {
                    $.when(thisObj.ItemListD).done(function () {
                        var item = null;
                        if (id != undefined && id != null)
                            for (var i = 0; i < thisObj.ItemList().length; i++) {
                                var tmp = thisObj.ItemList()[i];
                                if (tmp.ID == id) {
                                    item = tmp;
                                    break;
                                }
                            }
                        thisObj.SelectedItem(item);
                    });
                };
            }

            self.EditLicenceObjectHelper = new self.createComboBoxHelper('.frmLicenceScheme  .editLicenceObject', '/licence-object');

            self.EditLicenceObjectHelper.SelectedItem.subscribe(function (newValue) {
                var object = vm.object();
                object.LicensingObjectType(newValue.ID);
                object.LicensingObjectTypeLabel(newValue.Name);
            });

            //// редактирование поля "Объекта лицензирования"
            self.EditLicenceObject = function () {
                if (!vm.CanEdit()) {
                    return;
                }
                var object = vm.object();
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'LicensingObjectType',
                        fieldFriendlyName: getTextResource('LicenceScheme_Form_SettingsTab_LicenceObject'),
                        comboBoxGetValueUrl: '/licence-object',
                        oldValue: {
                            ID: object.LicensingObjectType(),
                            Name: object.LicensingObjectTypeLabel()
                        },
                        save: function (data) {
                            vm.object().LicensingObjectType(JSON.parse(data.NewValue).id);
                            vm.object().LicensingObjectTypeLabel(JSON.parse(data.NewValue).name);
                            return $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                                if (result) {
                                    vm.raiseObjectModified();
                                }
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                });
            };

            function validateExpression(expression, fieldName) {
                var retD = $.Deferred();

                self.ajaxControl.Ajax(
                    null,
                    {
                        dataType: "json",
                        contentType: 'application/json',
                        method: "POST",
                        url: '/licence-scheme/' + fieldName + '/validate',
                        data: JSON.stringify({ 'Expression': expression })
                    },
                    function (response) {
                        if (response) {
                            retD.resolve(response);
                            //
                        } else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                            });
                        }
                    }
                );

                return retD.promise();
            }

            function validateAndSaveExpression(expression, fieldName, field) {
                var retD = $.Deferred();

                if (fieldName === 'LicenseCountPerObject') {
                    vm.object().LicenseCountPerObject(expression);
                } else if (fieldName === 'AdditionalRights') {
                    vm.object().AdditionalRights(expression);
                }

                $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                    retD.resolve(result ? { IsSuccess: true } : null);
                });

                return retD.promise();
            }

            function editExpression(field, fieldName, labels) {
                if (!vm.CanEdit()) {
                    return;
                }

                showSpinner();
                self.ajaxControl.Ajax(
                    null,
                    {
                        dataType: "json",
                        method: 'GET',
                        url: '/licence-scheme/statements'
                    },
                    function (response) {
                        if (response && response.Result === 0) {
                            $.when(
                                exEditor.ShowDialog({
                                    caption: getTextResource(labels.caption),
                                    legend: getTextResource(labels.legend),
                                    expression: field(),
                                    width: 869,
                                    height: 468,
                                    statements: response.Data,
                                    save: function (expression) {
                                        if (vm.isAddNewLicenceScheme()) {
                                            return validateExpression(expression, fieldName);
                                        } else {
                                            return validateAndSaveExpression(expression, fieldName, field);
                                        }
                                    },
                                    complete: function (expression) {
                                        field(expression);
                                    }
                                }))
                                .done(function () {
                                    hideSpinner();
                                });
                            //
                        } else {
                            hideSpinner();
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError'), 'error');
                            });
                        }
                    });
            }

            // редактирование поля "Требуемое количество прав на объект"
            self.editLicenseCountPerObject = function () {
                editExpression(
                    vm.object().LicenseCountPerObject,
                    'LicenseCountPerObject', {
                        caption: 'SoftwareLicenceScheme_LicencePerObjectCountEditorTitle',
                        legend: 'SoftwareLicenceScheme_LicencePerObjectCountEditorLegend'
                    });
            }         

            // редактирование поля " Привязывать права к объектам"
            self.EditIsLinkLicenseToObject = function () {
                if (!vm.CanEdit())
                    return true;

                if (vm.isAddNewLicenceScheme()) {
                    return true;
                }

                //
                var checkbox = vm.$region.find('#IsLinkLicenseToObjectCheckbox');
                //
                if (!checkbox || !checkbox[0])
                    return true;

                showSpinner();
                $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                    hideSpinner();
                    if (result) {
                        checkbox[0].checked = vm.object().IsLinkLicenseToObject();
                        vm.raiseObjectModified();
                    }
                });
            };

            //// редактирование поля "Лицензировать доступ пользователей"
            self.EditIsLinkLicenseToUser = function () {
                if (!vm.CanEdit())
                    return true;
                //
                var checkbox = vm.$region.find('#IsLinkLicenseToUserCheckbox');
                //
                if (!checkbox || !checkbox[0])
                    return true;

                // -------------
                if (vm.isAddNewLicenceScheme())
                    return true;

                // ---------------------
                showSpinner();

                $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                    hideSpinner();
                    if (result) {
                        checkbox[0].checked = vm.object().IsLinkLicenseToUser();
                        vm.raiseObjectModified();
                    }
                });
            };

            // редактирование поля "Лицензировать все хосты кластера (для кластеров) "
            self.EditIsLicenseAllHosts = function () {
                if (!vm.CanEdit())
                    return true;
                //
                var checkbox = vm.$region.find('#IsLicenseAllHostsCheckbox');
                //
                if (!checkbox || !checkbox[0])
                    return true;
                // -------------
                if (vm.isAddNewLicenceScheme())
                    return true;

                // ---------------------
                showSpinner();

                $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                    hideSpinner();
                    if (result) {
                        checkbox[0].checked = vm.object().IsLicenseAllHosts();
                        vm.raiseObjectModified();
                    }
                });
            };

            //редактирование поля "Допустимое число инсталляций на сервере"
            self.EditInstallationLimits = function () {
                if (!vm.CanEdit()) {
                    return;
                }

                showSpinner();
                var object = vm.object();

                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'InstallationLimits',
                        fieldFriendlyName: getTextResource('LicenceScheme_Form_SettingsTab_InstallationLimits'),
                        oldValue: object.InstallationLimits(),
                        maxLength: 255,
                        minValue: 1,
                        nosave: vm.isAddNewLicenceScheme(),
                        method: 'PUT',
                        save: function (data) {
                            object.InstallationLimits(JSON.parse(data.NewValue).val);
                            return $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                                if (result) {
                                    vm.raiseObjectModified();
                                }
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };

            // редактирование поля "Допустимое число инсталляций на сервере"
            self.EditInstallationLimitUnlimit = function () {
                if (!vm.CanEdit())
                    return true;
                //
                var checkbox = vm.$region.find('#InstallationLimitUnlimitRadioTrue');
                //
                if (!checkbox || !checkbox[0])
                    return true;
                // -------------
                if (vm.isAddNewLicenceScheme())
                    return true;

                // ---------------------
                showSpinner();

                $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                    hideSpinner();
                    if (result) {
                        vm.object().InstallationLimitUnlimit('1');
                        vm.object().InstallationLimits(0);
                        checkbox.prop('checked', true);
                        vm.raiseObjectModified();
                    }
                });
            };

            // редактирование поля "Число инсталляций на виртуальных машинах"
            self.EditInstallationLimitPerVM = function () {
                if (!vm.CanEdit()) {
                    return;
                }

                showSpinner();
                var object = vm.object();

                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'InstallationLimitPerVM',
                        fieldFriendlyName: getTextResource('LicenceScheme_Form_SettingsTab_InstallationLimitPerVM'),
                        oldValue: object.InstallationLimitPerVM(),
                        maxLength: 255,
                        minValue: 1,
                        save: function (data) {
                            object.InstallationLimitPerVM(JSON.parse(data.NewValue).val);
                            return $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                                if (result) {
                                    vm.raiseObjectModified();
                                }
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };

            // редактирование поля "Число инсталляций на виртуальных машинах"
            self.EditInstallationLimitPerVMUnlimit = function () {
                const allowInstallOnVM = vm.object().AllowInstallOnVM();
                if (!vm.CanEdit())
                    return true;
                //
                var checkbox = (allowInstallOnVM == 1) ? vm.$region.find('#AllowInstallOnVM_3') : vm.$region.find('#AllowInstallOnVM_0');
                //
                if (!checkbox || !checkbox[0])
                    return true;
                // -------------
                if (vm.isAddNewLicenceScheme())
                    return true;

                // ---------------------
                showSpinner();

                $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                    hideSpinner();
                    if (result) {
                        checkbox.prop('checked', true);
                        vm.object().InstallationLimitPerVM(0);
                        vm.raiseObjectModified();
                    }
                });
            };

            // редактирование поля "Кол-во дополнительных прав"
            self.editAdditionalRights = function () {
                editExpression(
                    vm.object().AdditionalRights,
                    'AdditionalRights', {
                        caption: 'SoftwareLicenceScheme_AdditionalRightsEditorTitle',
                        legend: 'SoftwareLicenceScheme_AdditionalRightsEditorLegend'
                });
            };

            // редактирование поля "Размер увеличения числа инсталляций на виртуальных машинах"
            self.EditIncreaseForVM = function () {
                if (!vm.CanEdit()) {
                    return;
                }

                showSpinner();
                var object = vm.object();

                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'IncreaseForVM',
                        fieldFriendlyName: getTextResource('LicenceScheme_Form_SettingsTab_IncreaseForVM'),
                        oldValue: object.IncreaseForVM(),
                        maxLength: 255,
                        minValue: 1,
                        nosave: vm.isAddNewLicenceScheme(),
                        save: function (data) {
                            object.IncreaseForVM(JSON.parse(data.NewValue).val);
                            return $.when(vm.object().AddOrUpdate(false, vm.baseUrl)).done(function (result) {
                                if (result) {
                                    vm.raiseObjectModified();
                                }
                            });
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };

            // ------------------------------------------ // 

            //when tab selected
            self.load = function () {
                if (self.isLoaded === true)
                    return;

                self.EditLicenceObjectHelper.LoadList();

                self.isLoaded = true;
            };
            //when tab unload
            self.dispose = function () {
                self.isLoaded = false;
                self.historyList.removeAll();
                self.ajaxControl.Abort();
            };
            //
            self.sortTapeRecord = function (list_obj) {
                if (!list_obj)
                    return;
                //
                list_obj.sort(
                    function (left, right) {
                        if (left.DateObj() == null)
                            return -1;
                        //
                        if (right.DateObj() == null)
                            return 1;
                        //
                        return left.DateObj() == right.DateObj() ? 0 : (left.DateObj() < right.DateObj() ? -1 : 1);
                    }
                );
            };
        }
    };
    return module;
});