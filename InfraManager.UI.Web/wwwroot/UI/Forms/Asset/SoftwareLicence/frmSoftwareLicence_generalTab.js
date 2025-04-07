define(['knockout', 'jquery', 'ajax', 'assetForms', 'ui_controls/ListView/ko.ListView.Helpers'], function (ko, $, ajax, fhModule, m_helpers) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('SoftwareLicence_GeneralTab');
            self.Template = '../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicence_generalTab';
            self.IconCSS = 'generalTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.CanEdit = vm.CanEdit;//for userlib
            //
            self.$region = vm.$region;
            //Типы лицензий 
            self.ProductCatalogTemplateIDSingle = 183;       //ID самостоятельной лицензии
            self.ProductCatalogTemplateIDSubscription = 186; //подписка
            self.ProductCatalogTemplateIDUpgrade = 185;      //upgrade
            self.ProductCatalogTemplateIDApply = 187;        //продление подписки
            self.ProductCatalogTemplateIDRent = 184;         //аренда            
            self.ProductCatalogTemplateIDOEM = 189;         //OEM

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
            //editors
            {
                //редактирование поля "Количество прав"
                self.EditCount = function () {
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
                            fieldName: 'Count',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Count'),
                            oldValue: object.Count(),
                            maxLength: 255,
                            minValue: 1,
                            onSave: function (newCount) {
                                object.Count(newCount);
                                
                                if (typeof vm.tabList()[7] !== 'undefined') {
                                    if (vm.tabList()[7].isLoaded === true)                                   
                                        vm.tabList()[7].listView.load();
                                }
                                
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                };

                //редактирование поля "Действительна с"
                self.EditBeginDate = function () {
                    if (!vm.CanEdit())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            ClassID: object.ClassID,
                            fieldName: 'BeginDate',
                            fieldFriendlyName: getTextResource('SoftwareLicence_BeginDate'),
                            oldValue: object.BeginDateDT(),
                            allowNull: false,
                            OnlyDate: true,
                            onSave: function (newDate) {
                                object.BeginDate(parseDate(newDate, true));
                                object.BeginDateDT(newDate ? new Date(parseInt(newDate)) : null);
                                vm.raiseObjectModified();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };

                //редактирование поля "Действительна по"
                self.EditEndDate = function () {
                    if (!vm.CanEdit())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            ClassID: object.ClassID,
                            fieldName: 'EndDate',
                            fieldFriendlyName: getTextResource('SoftwareLicence_EndDate'),
                            oldValue: object.EndDateDT(),
                            allowNull: false,
                            OnlyDate: true,
                            onSave: function (newDate) {
                                object.EndDate(parseDate(newDate, true));
                                object.EndDateDT(newDate ? new Date(parseInt(newDate)) : null);
                                vm.raiseObjectModified();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };

                //редактирование поля "Ограничение, дней"
                self.EditLimitInDays = function () {

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
                            fieldName: 'LimitInDays',
                            fieldFriendlyName: getTextResource('SoftwareLicence_LimitInDays'),
                            oldValue: object.LimitInDays(),
                            maxValue: 500000,
                            maxLength: 255,
                            onSave: function (newLimitInDays) {
                                object.LimitInDays(newLimitInDays);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                };

                //редактирование поля "Описание"
                self.EditNote = function () {

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
                            fieldName: 'Note',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Note'),
                            oldValue: object.Note(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                object.Note(newText);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };

                self.ajaxControl_location = new ajax.control();
                self.EditLocation = function () {
                    if (!self.CanEdit())
                        return;
                    //
                    showSpinner();
                    var asset = vm.object();
                    //
                    require(['ui_forms/Asset/frmAssetLocation', 'sweetAlert'], function (module) {
                        //начальное местоположение
                        var locationInfo = { ID: asset.RoomGuidID(), ClassID: 3 };
                        //режим выбора местоположения
                        var locationType = module.LocationType.SoftwareLicenceLocation;

                        var onLocationChanged = function (objectInfo) {//когда новое местоположение будет выбрано
                            self.locationListView.load();
                        };
                        var saveLocation = function (objectInfo, isReplaceAnyway) {//для сохранения нового местоположения
                            if (!objectInfo)
                                return;
                            //
                            var data = {
                                ID: asset.ID(),
                                ObjClassID: asset.ClassID(),
                                ClassID: null,
                                ObjectList: null,
                                Field: 'Location',
                                NewValue: JSON.stringify({ 'id': objectInfo.ID, 'fullName': '' }),
                                OldValue: JSON.stringify({ 'id': locationInfo.ID, 'fullName': '' }),
                                Params: ['' + objectInfo.ClassID, ''],
                                ReplaceAnyway: isReplaceAnyway == true ? true : false,
                            };
                            //
                            self.ajaxControl_location.Ajax(
                                self.$region.find('.network-device-location-header'),
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    url: '/sdApi/SetField',
                                    data: data
                                },
                                function (retModel) {
                                    if (retModel) {
                                        var result = retModel.ResultWithMessage.Result;
                                        var message = retModel.ResultWithMessage.Message;
                                        //
                                        if (result === 0) {
                                            onLocationChanged(objectInfo);
                                            $(document).trigger('local_objectUpdated', [asset.ClassID(), asset.ID(), null]);
                                        }
                                        else if (result === 1)
                                            swal(getTextResource('SaveError'), getTextResource('NullParamsError'), 'error');
                                        else if (result === 2)
                                            swal(getTextResource('SaveError'), getTextResource('BadParamsError'), 'error');
                                        else if (result === 3)
                                            swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                                        // 4 - is global error
                                        else if (result === 5 && isReplaceAnyway == false) {
                                            hideSpinner();//we start him in formHelper when clicked
                                            swal({
                                                title: getTextResource('SaveError'),
                                                text: getTextResource('ConcurrencyError'),
                                                showCancelButton: true,
                                                closeOnConfirm: true,
                                                closeOnCancel: true,
                                                confirmButtonText: getTextResource('ButtonOK'),
                                                cancelButtonText: getTextResource('ButtonCancel')
                                            },
                                                function (value) {
                                                    if (value == true)
                                                        saveLocation(objectInfo, true);
                                                });
                                        }
                                        else if (result === 6)
                                            swal(getTextResource('SaveError'), getTextResource('ObjectDeleted'), 'error');
                                        else if (result === 7)
                                            swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                                        else if (result === 8)
                                            swal(getTextResource('SaveError'), message, 'info');
                                        else
                                            swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    }
                                    else
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                });
                        };
                        //
                        module.ShowDialog(locationType, locationInfo, saveLocation, true);
                    });
                };

                //редактирование поля "Возможность downgrade"
                self.EditDowngradeAvailable = function () {
                    if (!vm.CanEdit())
                        return;
                    //
                    var checkbox = vm.$region.find('#downgradeAvailableCheckbox');
                    //
                    if (!checkbox || !checkbox[0])
                        return;
                    //
                    var oldValue = !checkbox[0].checked;
                    var newValue = checkbox[0].checked;
                    //
                    showSpinner();
                    var obj = vm.object();
                    obj.DowngradeAvailable(newValue);
                    var data = {
                        ID: obj.ID(),
                        ObjClassID: obj.ClassID,
                        Field: 'DowngradeAvailable',
                        OldValue: JSON.stringify({ 'val': oldValue }),
                        NewValue: JSON.stringify({ 'val': newValue }),
                        ReplaceAnyway: false
                    };

                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                //
                                hideSpinner();
                                if (result === 0) {
                                    checkbox[0].checked = obj.DowngradeAvailable();
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });
                };

                //редактирование поля "Без ограничений количества прав"
                self.EditSoftwareUsageNoLimits = function () {
                    if (!vm.CanEdit())
                        return;
                    //
                    var checkbox = vm.$region.find('#noLimitsCheckbox');
                    //
                    if (!checkbox || !checkbox[0])
                        return;
                    //
                    var newNoLimitsValue = checkbox[0].checked;
                    //
                    showSpinner();
                    var obj = vm.object();

                    let newCountValue;
                    if (newNoLimitsValue) {
                        newCountValue = null;
                    } else {
                        newCountValue = 1;
                    }
                    var data = {
                        ID: obj.ID(),
                        ObjClassID: obj.ClassID,
                        Field: 'Count',
                        OldValue: JSON.stringify({ 'val': obj.Count() }),
                        NewValue: JSON.stringify({ 'val': newCountValue }),
                        ReplaceAnyway: false
                    };

                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                //
                                hideSpinner();
                                if (result === 0) {
                                    checkbox[0].checked = obj.SoftwareUsageNoLimits();
                                    obj.Count(obj.SoftwareUsageNoLimits() ? null : 1);
                                    vm.raiseObjectModified();
                                }
                                else if (result === 8) {
                                    obj.SoftwareUsageNoLimits(!obj.SoftwareUsageNoLimits());
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), retModel.ResultWithMessage.Message, 'error');
                                    });
                                }
                                else {
                                    obj.SoftwareUsageNoLimits(!obj.SoftwareUsageNoLimits());
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });
                };

                //редактирование вида лицензии
                self.EditLicenceType = function () {
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
                            fieldName: 'LicenceType',
                            fieldFriendlyName: getTextResource('SoftwareLicence_LicenceType'),
                            comboBoxGetValueUrl: '/assetApi/GetLicenceTypeForSoftwareLicenceList',
                            oldValue: {
                                ID: object.LicenceTypeID(), Name: object.LicenceTypeName
                            },
                            onSave: function (selectedValue) {
                                object.LicenceTypeID(selectedValue.ID);
                                object.LicenceTypeName(selectedValue.Name);
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.comboBoxEdit, options);
                    });
                };

                //редактирование схемы лицензирования
                self.EditLicenceScheme = function () {

                    $("#searchLicenceSchemeInputElelemnt").blur();
                    if (!vm.CanEdit()) {
                        return;
                    }

                    var object = vm.object();
                    var oldValue = object.SoftwareLicenceSchemeID();
                    var oldLocation = object.IsRestrictionsLocationVisible();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper();
                        fh.ShowSoftwareLicenceSchemeLink(
                            false,
                            false,
                            function (newValues) {
                                if (!newValues || newValues.length == 0)
                                    return;
                                //
                                if (newValues.length == 1) {
                                    var model = newValues[0];
                                    var newValue = model.ID;

                                    var data = {
                                        ID: object.ID(),
                                        ObjClassID: object.ClassID,
                                        Field: 'LicenceScheme',
                                        OldValue: JSON.stringify({ 'id': oldValue, 'IsLocationRestrictionApplicable': oldLocation }),
                                        NewValue: JSON.stringify({ 'id': newValue, 'IsLocationRestrictionApplicable': model.IsLocationRestrictionApplicable }),
                                        ReplaceAnyway: false
                                    };

                                    self.ajaxControl.Ajax(
                                        null,//self.$region, two spinner problem
                                        {
                                            dataType: "json",
                                            method: 'POST',
                                            url: '/sdApi/SetField',
                                            data: data
                                        },
                                        function (retModel) {
                                            if (retModel) {
                                                var result = retModel.ResultWithMessage.Result;
                                                //
                                                //hideSpinner();
                                                if (result === 0) {
                                                    object.SoftwareLicenceSchemeID(model.ID);
                                                    object.SoftwareLicenceSchemeName(model.Name);
                                                    var needReload = model.IsLocationRestrictionApplicable && !object.IsRestrictionsLocationVisible();
                                                    object.IsRestrictionsLocationVisible(model.IsLocationRestrictionApplicable);
                                                    if (needReload) {
                                                        object.load(object.ID());
                                                    }
                                                    vm.raiseObjectModified();
                                                    if (needReload) {
                                                        $.when(self.locationListView.load()).done(function () {
                                                            self.locationListView.showAllRows();
                                                        });
                                                    }
                                                }
                                                else {
                                                    require(['sweetAlert'], function () {
                                                        swal(getTextResource('SaveError'), retModel.ResultWithMessage.Message ?? getTextResource('GlobalError'), 'error');
                                                    });
                                                }
                                            }
                                        });
                                    return true;
                                }
                            },
                            true,
                            {
                                TemplateClassID: 745,
                                HasLifeCycle: false,
                            },
                            getTextResource('SoftwareLicenceScheme_Select'),
                            self.OpenSoftwareLicenceSchemeForm
                        );
                    });
                };

                // показать Схему лицензирвоания
                self.OpenSoftwareLicenceSchemeForm = function (obj) {
                    require(['assetForms'], function (module) {
                        if (self.CanOpenSoftwareLicenceSchemeForm()) {
                            var fh = new module.formHelper(true);
                            if (obj)
                                fh.ShowLicenceSchemeForm(obj.ID);
                            else {
                                var object = vm.object();
                                fh.ShowLicenceSchemeForm(object.SoftwareLicenceSchemeID());
                            }
                        }
                    });

                };
                // можно ли показать Схему лицензирвоания
                self.CanOpenSoftwareLicenceSchemeForm = function () {
                    return vm.object().operationIsGranted(750001);
                };

                //Видимость поля "Выдано"
                self.IsInUseVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent
                        )
                        : false;
                });

                //Видимость поля "Свободно"
                self.IsBalanceVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent
                        )
                        : false;
                });

                //Видимость поля "Действительна с"
                self.IsBeginDateVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent)
                        : false;
                });

                //Видимость поля "Действительна по"
                self.IsEndDateVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent)
                        : false;
                });

                //Видимость поля "Ограничение, дней"
                self.IsLimitInDaysVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDApply)
                        : false;
                });

                //Видимость блока "Аппаратный ключ защиты"
                self.IsHASPAdapterVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent
                        )
                        : false;
                });

                //Видимость блока "Cвязанная лицензия"
                self.IsParentSoftwareLicenceVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDApply
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDUpgrade)
                        : false;
                });

                //Видимость блока "Связи"
                self.IsLinksDivVisible = ko.computed(function () {
                    return self.IsHASPAdapterVisible() || self.IsParentSoftwareLicenceVisible();
                });

                //Видимость поля "Возможность downgrade"
                self.IsDowngradeAvailableVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent
                        )
                        : false;
                });

                self.EditHASPAdapter = function () {
                    $("#searcherHaspAdapterInputElem").blur();
                    if (!vm.CanEdit()) {
                        return;
                    }

                    var object = vm.object();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper();
                        fh.ShowAssetLink({
                            ClassID: 33,
                            ID: null,
                            ServiceID: null,
                            ClientID: null,
                            ShowWrittenOff: false,
                            Caption: getTextResource('HASP_Adapter_Select'),
                            SelectOnlyOne: true,
                            IsHaspAdapterForm: true,
                            UniqueAssetTypeToShow: 329
                        }, function (newHASPAdapter) {
                            if (!newHASPAdapter || newHASPAdapter.length == 0)
                                return;

                            var data = {
                                ID: object.ID(),
                                ObjClassID: object.ClassID,
                                Field: 'HASPAdapter',
                                OldValue: JSON.stringify({ 'val': object.HASPAdapterID }),
                                NewValue: JSON.stringify({ 'val': newHASPAdapter[0].ID }),
                                ReplaceAnyway: true
                            };
                            self.ajaxControl.Ajax(
                                null,//self.$region, two spinner problem
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    url: '/sdApi/SetField',
                                    data: data
                                },
                                function (retModel) {
                                    if (retModel) {
                                        var result = retModel.ResultWithMessage.Result;
                                        hideSpinner();
                                        if (result === 0) {
                                            vm.object().HASPAdapterID(newHASPAdapter[0].ID);
                                            vm.object().HASPAdapterName(newHASPAdapter[0].Model);
                                            vm.raiseObjectModified();
                                        }
                                        else {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                            });
                                        }
                                    }
                                });
                        })
                    });
                }

                self.OpenHASPAdapterForm = function () {
                    require(['assetForms'], function (module) {
                        var object = vm.object();
                        var fh = new module.formHelper(true);
                        fh.ShowAssetForm(object.HASPAdapterID(), 33);
                    });
                }

                self.OpenCommercialSoftwareModelForm = function () {
                    require(['assetForms'], function (module) {
                        var object = vm.object();
                        var fh = new module.formHelper(true);
                        fh.ShowCommercialSoftwareModelForm(object.SoftwareModelID());
                    });
                }


                self.ClearHASPAdapter = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }

                    var object = vm.object();
                    var data = {
                        ID: object.ID(),
                        ObjClassID: object.ClassID,
                        Field: 'HASPAdapter',
                        OldValue: JSON.stringify({ 'val': object.HASPAdapterID }),
                        NewValue: JSON.stringify({ 'val': null }),
                        ReplaceAnyway: true
                    };
                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                hideSpinner();
                                if (result === 0) {
                                    vm.object().HASPAdapterID(null);
                                    vm.object().HASPAdapterName('');
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });
                }

                //открыть форму просмотра связанной лицензии
                self.OpenParentSoftwareLicenceForm = function () {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var object = vm.object();
                        var fh = new module.formHelper(true);
                        fh.ShowSoftwareLicenceForm(object.ParentSoftwareLicenceID());
                    });
                };

                //удаление связанной лицензии
                self.ClearParentSoftwareLicence = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }

                    var object = vm.object();
                    var data = {
                        ID: object.ID(),
                        ObjClassID: object.ClassID,
                        Field: 'ParentSoftwareLicenceID',
                        OldValue: JSON.stringify({ 'val': object.ParentSoftwareLicenceID }),
                        NewValue: JSON.stringify({ 'val': null }),
                        ReplaceAnyway: true
                    };
                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                hideSpinner();
                                if (result === 0) {
                                    vm.object().ParentSoftwareLicenceID(null);
                                    vm.object().ParentSoftwareLicenceName('');
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });
                };

                //редактирование связанной лицензии
                self.EditParentSoftwareLicence = function () {
                    $("#searcherParentSoftwareLicenceElem").blur();
                    if (!vm.CanEdit()) {
                        return;
                    }

                    require(['assetForms', 'sweetAlert'], function (fhModule) {
                        var fh = new fhModule.formHelper();
                        //
                        var saveFunc = function (newParentSoftwareLicence) {
                            if (!newParentSoftwareLicence || newParentSoftwareLicence.length == 0)
                                return;

                            var object = vm.object();

                            self.ajaxControl.Ajax(
                                null,
                                {
                                    dataType: "json",
                                    method: 'GET',
                                    url: '/sdApi/CanChangeSoftwareLicenceParent',
                                    data: { ID: object.ID(), ParentID: newParentSoftwareLicence[0].ID }
                                },
                                function (retModel) {
                                    if (retModel.CanChange) {
                                        var data = {
                                            ID: object.ID(),
                                            ObjClassID: object.ClassID,
                                            Field: 'ParentSoftwareLicenceID',
                                            OldValue: JSON.stringify({ 'val': object.ParentSoftwareLicenceID }),
                                            NewValue: JSON.stringify({ 'val': newParentSoftwareLicence[0].ID }),
                                            ReplaceAnyway: true
                                        };
                                        self.ajaxControl.Ajax(
                                            null,
                                            {
                                                dataType: "json",
                                                method: 'POST',
                                                url: '/sdApi/SetField',
                                                data: data
                                            },
                                            function (retModel) {
                                                if (retModel) {
                                                    var result = retModel.ResultWithMessage.Result;

                                                    hideSpinner();
                                                    if (result === 0) {
                                                        vm.object().ParentSoftwareLicenceID(newParentSoftwareLicence[0].ID);

                                                        let newParentSoftwareLicenceName = newParentSoftwareLicence[0].SoftwareModelName + ' / ' + newParentSoftwareLicence[0].ManufacturerName;
                                                        if (newParentSoftwareLicence[0].InvNumber != '') {
                                                            newParentSoftwareLicenceName += ' / инв. № ' + newParentSoftwareLicence[0].InvNumber;
                                                        }
                                                        vm.object().ParentSoftwareLicenceID(newParentSoftwareLicence[0].ID);
                                                        vm.object().ParentSoftwareLicenceName(newParentSoftwareLicenceName);
                                                        vm.raiseObjectModified();
                                                    }
                                                    else {
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                                        });
                                                    }
                                                }
                                            });
                                    } else {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ParentSoftwareLicence_ErrorTitle'), getTextResource('ParentSoftwareLicence_LinkError'), 'warning');
                                        });
                                    }
                                })
                        }
                        //
                        fh.ShowSoftwareLicenceLink(saveFunc, false);
                    })
                };

                //редактирование типа ограничения процессоров - без ограничения
                self.EditCpuRestrictionsRadioUnlimit = function () {
                    return self.EditCpuRestrictionsRadio('#allowedNumberOfCPUUnlimitRadioTrue', '1', '0');
                };
                //редактирование типа ограничения процессоров - диапазон
                self.EditCpuRestrictionsRadioRange = function () {
                    return self.EditCpuRestrictionsRadio('#allowedNumberOfCPUUnlimitRadioFalse', '0', '1');
                };

                //редактирование типа ограничения процессоров
                self.EditCpuRestrictionsRadio = function (editRegion, checkedValue, uncheckedValue) {
                    if (!vm.CanEdit())
                        return;
                    //
                    var checkbox = vm.$region.find(editRegion);
                    //
                    if (!checkbox || !checkbox[0])
                        return;
                    //
                    var oldValue = (!checkbox[0].checked) ? checkedValue : uncheckedValue;
                    var newValue = checkbox[0].checked ? checkedValue : uncheckedValue;
                    //
                    showSpinner();
                    var obj = vm.object();
                    obj.RestrictionsCPUUnlimit(newValue);
                    var data = {
                        ID: obj.ID(),
                        ObjClassID: obj.ClassID,
                        Field: 'RestrictionsCPUUnlimit',
                        OldValue: JSON.stringify({ 'val': oldValue }),
                        NewValue: JSON.stringify({ 'val': newValue }),
                        ReplaceAnyway: false
                    };

                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                //
                                hideSpinner();
                                if (result === 0) {
                                    checkbox[0].checked = obj.RestrictionsCPUUnlimit() == checkedValue;
                                    if (checkedValue == '0') {
                                        obj.RestrictionsCPUFrom(1);
                                        obj.RestrictionsCPUTill(1);
                                    }
                                    if (checkedValue == '1') {
                                        obj.RestrictionsCPUFrom(null);
                                        obj.RestrictionsCPUTill(null);
                                    }
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });

                    return true;
                };

                // Редкатриования нижнего лимита ограничения числа процессоров
                self.EditRestrictionsCPUFrom = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }
                    showSpinner();
                    var object = vm.object();
                    var oldValue = object.RestrictionsCPUFrom();
                    if (oldValue == null)
                        oldValue = 1;

                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'RestrictionsCPUFrom',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CPUCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_From'),
                            oldValue: oldValue,
                            maxLength: 255,
                            minValue: 1,
                            onSave: function (newValue) {
                                object.RestrictionsCPUFrom(newValue);
                                if (object.RestrictionsCPUTill() == null || object.RestrictionsCPUTill() < newValue)
                                    object.RestrictionsCPUTill(newValue);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                    return true;
                };

                // Редактирвоание врехнего лимита ограничения числа процессоров
                self.EditRestrictionsCPUTill = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }
                    showSpinner();
                    var object = vm.object();
                    var oldValue = object.RestrictionsCPUTill();
                    if (oldValue == null)
                        oldValue = 1;

                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'RestrictionsCPUTill',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CPUCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_Till'),
                            oldValue: oldValue,
                            maxLength: 255,
                            minValue: 1,
                            onSave: function (newValue) {
                                object.RestrictionsCPUTill(newValue);
                                if (object.RestrictionsCPUFrom() == null || object.RestrictionsCPUFrom() > newValue)
                                    object.RestrictionsCPUFrom(newValue);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                    return true;

                };

                //редактирование типа ограничения ядер - без ограничения
                self.EditCoreRestrictionsRadioUnlimit = function () {
                    return self.EditCoreRestrictionsRadio('#allowedNumberOfCoreUnlimitRadioTrue', '1', '0');
                };
                //редактирование типа ограничения ядер - диапазон
                self.EditCoreRestrictionsRadioRange = function () {
                    return self.EditCoreRestrictionsRadio('#allowedNumberOfCoreUnlimitRadioFalse', '0', '1');
                };

                //редактирование типа ограничения ядер
                self.EditCoreRestrictionsRadio = function (editRegion, checkedValue, uncheckedValue) {
                    if (!vm.CanEdit())
                        return;
                    //
                    var checkbox = vm.$region.find(editRegion);
                    //
                    if (!checkbox || !checkbox[0])
                        return;
                    //
                    var oldValue = (!checkbox[0].checked) ? checkedValue : uncheckedValue;
                    var newValue = checkbox[0].checked ? checkedValue : uncheckedValue;
                    //
                    showSpinner();
                    var obj = vm.object();
                    obj.RestrictionsCoreUnlimit(newValue);
                    var data = {
                        ID: obj.ID(),
                        ObjClassID: obj.ClassID,
                        Field: 'RestrictionsCoreUnlimit',
                        OldValue: JSON.stringify({ 'val': oldValue }),
                        NewValue: JSON.stringify({ 'val': newValue }),
                        ReplaceAnyway: false
                    };

                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                //
                                hideSpinner();
                                if (result === 0) {
                                    checkbox[0].checked = obj.RestrictionsCoreUnlimit() == checkedValue;
                                    if (checkedValue == '0') {
                                        obj.RestrictionsCoreFrom(1);
                                        obj.RestrictionsCoreTill(1);
                                    }
                                    if (checkedValue == '1') {
                                        obj.RestrictionsCoreFrom(null);
                                        obj.RestrictionsCoreTill(null);
                                    }
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });

                    return true;
                };

                // Редкатриования нижнего лимита ограничения числа ядер
                self.EditRestrictionsCoreFrom = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }
                    showSpinner();
                    var object = vm.object();
                    var oldValue = object.RestrictionsCoreFrom();
                    if (oldValue == null)
                        oldValue = 1;

                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'RestrictionsCoreFrom',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CoreCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_From'),
                            oldValue: oldValue,
                            maxLength: 255,
                            minValue: 1,
                            onSave: function (newValue) {
                                object.RestrictionsCoreFrom(newValue);
                                if (object.RestrictionsCoreTill() == null || object.RestrictionsCoreTill() < newValue)
                                    object.RestrictionsCoreTill(newValue);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                    return true;
                };

                // Редактирвоание врехнего лимита ограничения числа ядер
                self.EditRestrictionsCoreTill = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }
                    showSpinner();
                    var object = vm.object();
                    var oldValue = object.RestrictionsCoreTill();
                    if (oldValue == null)
                        oldValue = 1;

                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'RestrictionsCoreTill',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CoreCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_Till'),
                            oldValue: oldValue,
                            maxLength: 255,
                            minValue: 1,
                            onSave: function (newValue) {
                                object.RestrictionsCoreTill(newValue);
                                if (object.RestrictionsCoreFrom() == null || object.RestrictionsCoreFrom() > newValue)
                                    object.RestrictionsCoreFrom(newValue);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                    return true;

                };

                //редактирование типа ограничения тактовой частоты - без ограничения
                self.EditHzRestrictionsRadioUnlimit = function () {
                    return self.EditHzRestrictionsRadio('#allowedNumberOfHzUnlimitRadioTrue', '1', '0');
                };
                //редактирование типа ограничения тактовой частоты - диапазон
                self.EditHzRestrictionsRadioRange = function () {
                    return self.EditHzRestrictionsRadio('#allowedNumberOfHzUnlimitRadioFalse', '0', '1');
                };

                //редактирование типа ограничения тактовой частоты
                self.EditHzRestrictionsRadio = function (editRegion, checkedValue, uncheckedValue) {
                    if (!vm.CanEdit())
                        return;
                    //
                    var checkbox = vm.$region.find(editRegion);
                    //
                    if (!checkbox || !checkbox[0])
                        return;
                    //
                    var oldValue = (!checkbox[0].checked) ? checkedValue : uncheckedValue;
                    var newValue = checkbox[0].checked ? checkedValue : uncheckedValue;
                    //
                    showSpinner();
                    var obj = vm.object();
                    obj.RestrictionsHzUnlimit(newValue);
                    var data = {
                        ID: obj.ID(),
                        ObjClassID: obj.ClassID,
                        Field: 'RestrictionsHzUnlimit',
                        OldValue: JSON.stringify({ 'val': oldValue }),
                        NewValue: JSON.stringify({ 'val': newValue }),
                        ReplaceAnyway: false
                    };

                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                //
                                hideSpinner();
                                if (result === 0) {
                                    checkbox[0].checked = obj.RestrictionsHzUnlimit() == checkedValue;
                                    if (checkedValue == '0') {
                                        obj.RestrictionsHzFrom(1);
                                        obj.RestrictionsHzTill(1);
                                    }
                                    if (checkedValue == '1') {
                                        obj.RestrictionsHzFrom(null);
                                        obj.RestrictionsHzTill(null);
                                    }
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });

                    return true;
                };

                // Редкатриования нижнего лимита ограничения тактовой частоты
                self.EditRestrictionsHzFrom = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }
                    showSpinner();
                    var object = vm.object();
                    var oldValue = object.RestrictionsHzFrom();
                    if (oldValue == null)
                        oldValue = 1;

                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'RestrictionsHzFrom',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_Frequency') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_From'),
                            oldValue: oldValue,
                            maxLength: 255,
                            minValue: 1,
                            onSave: function (newValue) {
                                object.RestrictionsHzFrom(newValue);
                                if (object.RestrictionsHzTill() == null || object.RestrictionsHzTill() < newValue)
                                    object.RestrictionsHzTill(newValue);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                    return true;
                };

                // Редактирвоание врехнего лимита ограничения тактовой частоты
                self.EditRestrictionsHzTill = function () {
                    if (!vm.CanEdit()) {
                        return;
                    }
                    showSpinner();
                    var object = vm.object();
                    var oldValue = object.RestrictionsHzTill();
                    if (oldValue == null)
                        oldValue = 1;

                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'RestrictionsHzTill',
                            fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_Frequency') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_Till'),
                            oldValue: oldValue,
                            maxLength: 255,
                            minValue: 1,
                            onSave: function (newValue) {
                                object.RestrictionsHzTill(newValue);
                                if (object.RestrictionsHzFrom() == null || object.RestrictionsHzFrom() > newValue)
                                    object.RestrictionsHzFrom(newValue);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                    return true;

                };
            }

            //Блок "Лимиты"
            self.IsLimitsContainerVisible = ko.observable(true);
            self.ToggleLimitsContainer = function () {
                self.IsLimitsContainerVisible(!self.IsLimitsContainerVisible());
            };

            //Блок "Местоположение"
            self.IsLocationContainerVisible = ko.observable(true);
            self.ToggleLocationContainer = function () {
                self.IsLocationContainerVisible(!self.IsLocationContainerVisible());
            };

            //Блок "Характеристики"
            self.IsCharacteristicsContainerVisible = ko.observable(true);
            self.ToggleCharacteristicsContainer = function () {
                self.IsCharacteristicsContainerVisible(!self.IsCharacteristicsContainerVisible());
            };

            //Блок "Информация"
            self.IsNoteVisible = ko.observable(true);
            self.ToggleNoteContainer = function () {
                self.IsNoteVisible(!self.IsNoteVisible());
            };

            //Блок "Классификатор"
            self.IsClassifierContainerVisible = ko.observable(true);
            self.ToggleClassifierContainer = function () {
                self.IsClassifierContainerVisible(!self.IsClassifierContainerVisible());
            };

            //Блок "Связи"
            self.IsLinksContainerVisible = ko.observable(true);
            self.ToggleLinksContainer = function () {
                self.IsLinksContainerVisible(!self.IsLinksContainerVisible());
            };

            //Блок "Ограничения"
            self.IsRestrictionsVisible = ko.observable(true);
            self.ToggleRestrictionsContainer = function () {
                self.IsRestrictionsVisible(!self.IsRestrictionsVisible());
            };
            // Ограничения раположения
            {
                self.locationListView = null;
                self.locationListViewID = 'locationListView_' + ko.getNewID();
                self.locationListViewInit = function (listView) {
                    if (self.locationListView != null)
                        throw 'listView inited already';
                    //
                    self.locationListView = listView;
                    self.locationListView.ignoreTableHeight = true;
                    m_helpers.init(self, listView);//extend self        
                    //
                    self.locationListView.load();                   
                };
                //

                self.locationRetrieveItems = function () {
                    var retvalD = $.Deferred();
                    $.when(self.getLocationObjectList(null, true)).done(function (objectList) {
                        if (objectList) {

                            var newLocations = [];
                            var object = vm.object();
                            ko.utils.arrayForEach(objectList, function (row) {
                                var loc = { LocationID: row.LocationID, SoftwareLicenceID: row.SoftwareLicenceID, LocationType: row.SoftwareLicenceLocationType };
                                newLocations.push(loc);
                            });
                            object.RestrictionsLocations(newLocations);
                            self.locationListView.showAllRows();
                        }
                        //
                        retvalD.resolve(objectList);
                    });
                    return retvalD.promise();
                };
                //
                self.locationGetCheckedItems = function () {
                    var selectedItems = self.locationListView.rowViewModel.checkedItems();
                    //
                    if (!selectedItems)
                        return [];
                    //
                    var retval = [];
                    selectedItems.forEach(function (el) {
                        var item =
                        {
                            ID: el.ID.toUpperCase(),
                            LocationID: el.LocationID,
                            SoftwareLicenceID: el.SoftwareLicenceID,
                            SoftwareLicenceLocationType: el.SoftwareLicenceLocationType
                        };
                        retval.push(item);
                    });
                    return retval;
                };

                //
                self.getLocationObjectList = function (idArray, showErrors) {
                    var retvalD = $.Deferred();
                    //
                    var requestInfo = {
                        IDList: idArray ? idArray : [],
                        ViewName: 'SoftwareLicenceLocations',
                        TimezoneOffsetInMinutes: new Date().getTimezoneOffset(),//not used in this request
                        ParentObjectID: vm.object().ID(),
                    };

                    self.ajaxControl.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: requestInfo,
                            url: '/assetApi/GetSoftwareLicenceLocationObject'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                retvalD.resolve(newVal.Data);//can be null, if server canceled request, because it has a new request                               
                                return;
                            }
                            else if (newVal && newVal.Result === 1 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 2 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[Lists/SD/Table.js getData]', 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 3 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('AccessError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 7 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('OperationError_Table'));
                                });
                            }
                            else if (newVal && newVal.Result === 9 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError'), 'error');
                                });
                            }
                            else if (newVal && newVal.Result === 11 && showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('SqlTimeout'));
                                });
                            }
                            else if (showErrors === true) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js getData]', 'error');
                                });
                            }
                            //
                            retvalD.resolve([]);
                        },
                        function (XMLHttpRequest, textStatus, errorThrown) {
                            if (showErrors === true)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/SD/Table.js, getData]', 'error');
                                });
                            //
                            retvalD.resolve([]);
                        },
                        null
                    );
                    //
                    return retvalD.promise();
                };


                //
                self.locationsContextMenu = ko.observable(null);
                self.contextMenuInit = function (contextMenu) {
                    self.locationsContextMenu(contextMenu);//bind contextMenu

                    self.locationContextMenuAdd(contextMenu);
                    self.locationContextMenuDelete(contextMenu);
                };
                //
                self.contextMenuOpening = function (contextMenu) {
                    contextMenu.items().forEach(function (item) {
                        if (item.isEnable && item.isVisible) {
                            item.enabled(item.isEnable());
                            item.visible(item.isVisible());
                        }
                    });
                };
               //

            self.saveRestrictionLocations = function (objectInfo, onSuccess) {//для сохранения нового местоположения
                if (!objectInfo)
                    return false;
                if (!objectInfo || objectInfo.length <= 0) {
                    swal(getTextResource('SaveError'), getTextResource('Restriction_NeedOneLocation'), 'error');
                    return false;
                }
                var newLocations = [];
                var object = vm.object();
                ko.utils.arrayForEach(objectInfo, function (el) {
                    if (el.ClassID && el.ClassID == 1)
                        var item = { LocationID: el.ID, LocationType: el.ClassID, SoftwareLicenceID: object.ID() };
                    else if (el.LocationType && el.LocationType == 1)
                        var item = { LocationID: el.LocationID, LocationType: el.LocationType, SoftwareLicenceID: object.ID() };
                    if(item)
                        newLocations.push(item);
                });
                //
                var data = {
                    ID: object.ID(),
                    ObjClassID: object.ClassID(),
                    ClassID: null,
                    ObjectList: null,
                    Field: 'RestrictionsLocations',
                    NewValue: JSON.stringify(newLocations),
                    OldValue: JSON.stringify(object.RestrictionsLocations()),
                    Params: '',
                    ReplaceAnyway: false,
                };
                var result = false;
                //
                self.ajaxControl_location.Ajax(
                    null,
                    {
                        dataType: "json",
                        method: 'POST',
                        url: '/sdApi/SetField',
                        data: data
                    },
                    function (retModel) {
                        if (retModel) {
                            var result = retModel.ResultWithMessage.Result;
                            var message = retModel.ResultWithMessage.Message;
                            //
                            if (result === 0) {
                                $.when(self.locationListView.load()).done(function () {
                                    $('.resrtiction-location .tableData').css('height', '100%');
                                });
                                if (onSuccess)
                                    onSuccess();
                                result = true;
                            }
                            else if (result === 1)
                                swal(getTextResource('SaveError'), getTextResource('NullParamsError'), 'error');
                            else if (result === 2)
                                swal(getTextResource('SaveError'), getTextResource('BadParamsError'), 'error');
                            else if (result === 3)
                                swal(getTextResource('SaveError'), getTextResource('AccessError'), 'error');
                            // 4 - is global error
                            else if (result === 6)
                                swal(getTextResource('SaveError'), getTextResource('ObjectDeleted'), 'error');
                            else if (result === 7)
                                swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                            else if (result === 8)
                                swal(getTextResource('SaveError'), message, 'info');
                            else
                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                        }
                        else
                            swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                    });

                return result;
            };

                self.clearRestrictionLocationsSelection = function () {
                    self.locationListView.rowViewModel.checkedItems([]);
                };

                //  menu operations
                {
                    self.ViewSoftwareLicenceLocationAdd = function () {
                        if (!vm.CanEdit())
                            return;
                        //
                        showSpinner();
                        //var asset = self.asset();
                        //
                        require(['ui_forms/Asset/SoftwareLicence/SoftwareLicence.AddLocation', 'sweetAlert'], function (module, swal) {
                            var selectionInfo = [];//current selection
                            var object = vm.object();
                            ko.utils.arrayForEach(object.RestrictionsLocations(), function (location) {
                                selectionInfo.push({ ID: location.LocationID, ClassID: 1 });
                            });
                            //
                            module.ShowDialog(self.saveRestrictionLocations, selectionInfo, true);
                        });
                    };
                    //
                    self.ViewSoftwareLicenceLocationDelete = function () {
                        var selectedItems = self.locationGetCheckedItems();
                        if (selectedItems.length <= 0)
                            return;
                        var remain = [];
                        ko.utils.arrayForEach(vm.object().RestrictionsLocations(), function (el) {
                            var found = false;
                            ko.utils.arrayForEach(selectedItems, function (sel) {
                                if (sel.LocationID == el.LocationID)
                                    found = true;
                            });
                            if (!found)
                                remain.push(el);
                        });
                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('Removing'),
                                text: getTextResource('ConfirmRemoveQuestion'),
                                showCancelButton: true,
                                closeOnConfirm: false,
                                closeOnCancel: true,
                                confirmButtonText: getTextResource('ButtonOK'),
                                cancelButtonText: getTextResource('ButtonCancel')
                            },
                                function (value) {
                                    swal.close();
                                    //
                                    if (value == true) {
                                        clearTimeout(self.timeotDelete);
                                        self.timeotDelete = setTimeout(self.checkAndDelete, 100, remain);
                                    }
                                });
                        });
                        
                    };
                    self.checkAndDelete = function (remain) {
                        if (remain.length <= 0) {
                            require(['sweetAlert'], function (swalErr) {
                                swalErr(getTextResource('SaveError'), getTextResource('Restriction_CantDeleteALlLocation'), 'error');
                            });
                        }
                        else {
                            self.saveRestrictionLocations(remain, function () { self.clearRestrictionLocationsSelection(); });
                        }
                    }

                    //
                    self.locationContextMenuAdd = function (contextMenu) {
                        var isVisible = function () {
                            return vm.CanEdit() && vm.object() != null;
                        };
                        var action = function () {
                            self.ViewSoftwareLicenceLocationAdd();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('SoftwareLicence_Restrictions_Location_MenuAdd');
                        cmd.isEnable = function () { return true; };
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                    //
                    self.locationContextMenuDelete = function (contextMenu) {
                        var isEnable = function () {
                            return self.locationGetCheckedItems().length >= 1; //self.operationIsGranted(461);
                        };
                        var isVisible = function () {
                            return vm.CanEdit() && vm.object() != null;
                        };
                        var action = function () {
                            self.ViewSoftwareLicenceLocationDelete();
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext('SoftwareLicence_Restrictions_Location_MenuDelete');
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };
                }

            }

            //Отображать или нет модель продукта на форме
            self.IsSoftwareLicenceModelNameVisible = ko.computed(function () {
                var object = vm.object();
                if (object) {
                    return object.SoftwareLicenceModelName() != null;
                } else {
                    return true;
                }
            });

            //На складе
            self.ShowOnStore = ko.observable(true);

            // OEM Device
            self.OpenSelectOEMDevice = function () {
                var retvalD = $.Deferred();
                //
                if (vm.object() != null) {
                    var fh = new fhModule.formHelper();
                    fh.ShowAssetLink({
                        ClassID: 189,
                        ID: vm.object().ID(),
                        SelectOnlyOne: true
                    }, function (newValues) {
                        if (!newValues || newValues.length != 1)
                                return;
                            showSpinner();
                            var oldValue = vm.object().OEMDeviceClassID() + ':' + vm.object().OEMDeviceID();
                            var newValue = newValues[0].ClassID + ':' + newValues[0].ID;

                        var data = {
                            ID: vm.object().ID(),
                            ObjClassID: vm.object().ClassID,
                            Field: 'OEMDeviceID',
                            OldValue: JSON.stringify({ 'val': oldValue }),
                            NewValue: JSON.stringify({ 'val': newValue }),
                            ReplaceAnyway: false
                        };

                        self.ajaxControl.Ajax(
                            null,//self.$region, two spinner problem
                            {
                                dataType: "json",
                                method: 'POST',
                                url: '/sdApi/SetField',
                                data: data
                            },
                            function (retModel) {
                                if (retModel) {
                                    var result = retModel.ResultWithMessage.Result;
                                    //
                                    if (result === 0) {
                                        //
                                        var oemRequest = { 'ClassID': newValues[0].ClassID, 'ID': newValues[0].ID };

                                        self.ajaxControl.Ajax(
                                            null,//self.$region, two spinner problem
                                            {
                                                dataType: "json",
                                                method: 'POST',
                                                url: '/sdApi/GetSoftwareOEMLicenceDevice',
                                                data: oemRequest
                                            },
                                            function (retModel) {
                                                if (retModel) {
                                                    var result = retModel.Result;
                                                    //
                                                    hideSpinner();
                                                    if (result === 0 && retModel.OEMDevice) {
                                                        //
                                                        vm.object().OEMDeviceID(newValues[0].ID);
                                                        vm.object().OEMDeviceClassID(newValues[0].ClassID);
                                                        vm.object().OEMDeviceLocation(retModel.OEMDevice.Location);
                                                        vm.object().OEMDeviceFullName(retModel.OEMDevice.FullName);
                                                        vm.object().OEMDeviceType(retModel.OEMDevice.TypeName);
                                                        vm.raiseObjectModified();
                                                        retvalD.resolve(true);
                                                    }
                                                    else {
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                                        });
                                                    }
                                                }
                                                else {
                                                    hideSpinner();
                                                }
                                            });
                                    }
                                    else {
                                        hideSpinner();
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                        });
                                    }
                                }
                            });

                        //  
                        retvalD;
                    });
                }
                else retvalD.resolve();
                //
                return retvalD;

            };
            self.CanOpenSelectOEMDevice = function () {
                return true;
            };

            self.ShowOEMDevice = function () {
                var object = vm.object();
                if (object.OEMDeviceClassID() && object.OEMDeviceID()) {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    if (object.OEMDeviceClassID() == 5 || object.OEMDeviceClassID() == 6 || object.OEMDeviceClassID() == 33 || object.OEMDeviceClassID() == 34)
                        fh.ShowAssetForm(object.OEMDeviceID(), object.OEMDeviceClassID());
                    else if (object.OEMDeviceClassID() == 115)
                        fh.ShowServiceContract(object.OEMDeviceID());
                    else if (object.OEMDeviceClassID() == 386)
                        fh.ShowServiceContractAgreement(object.OEMDeviceID());
                    else if (object.OEMDeviceClassID() == 223)
                        fh.ShowSoftwareLicenceForm(object.OEMDeviceID());
                    else if (object.OEMDeviceClassID() == 165)
                        fh.ShowDataEntityObjectForm(object.OEMDeviceID());
                    else if (object.OEMDeviceClassID() == 409 || object.OEMDeviceClassID() == 410 || object.OEMDeviceClassID() == 411 || object.OEMDeviceClassID() == 412 ||
                        object.OEMDeviceClassID() == 413 || object.OEMDeviceClassID() == 414 || object.OEMDeviceClassID() == 415 || object.OEMDeviceClassID() == 419)
                        fh.ShowConfigurationUnitForm(object.OEMDeviceID());
                }
            };

        }
    };
    return module;
});
