define(['knockout', 'jquery', 'ajax', 'dateTimeControl', 'assetForms', 'ui_controls/ListView/ko.ListView.Helpers'], function (ko, $, ajax, dtLib, fhModule, m_helpers) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('SoftwareLicence_GeneralTab');
            self.Template = '../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicenceAdd_generalTab';
            self.IconCSS = 'generalTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.$region = vm.$region;

            //when object changed
            self.init = function (obj) {                
            };
            //when tab selected
            self.load = function () {                
                var retD = $.Deferred();
                $.when(self.softwareDistributionCentres.load())
                    .done(function () { retD.resolve(); });
                return retD;
            };

            //----------------------------------------------------------------------------------------------------------------------------------------------

            //Типы лицензий 
            self.ProductCatalogTemplateIDSingle = 183;       //ID самостоятельной лицензии
            self.ProductCatalogTemplateIDSubscription = 186; //подписка
            self.ProductCatalogTemplateIDUpgrade = 185;      //upgrade
            self.ProductCatalogTemplateIDApply = 187;        //продление подписки
            self.ProductCatalogTemplateIDRent = 184;         //аренда         

            //Из модели:
            //Не задано
            self.SoftwareExecutionCountIsNotDefined = ko.observable(false);
            //Без ограничений
            self.SoftwareExecutionUnlimited = ko.observable(false);
            //Задано
            self.SoftwareExecutionCountSet = ko.observable(false);
            self.softwareDistributionCentres = new module.SoftwareDistributionCentres(null);
            self.getListOfSDC = function (options) {
                var data = self.softwareDistributionCentres == null
                    ? []
                    : self.softwareDistributionCentres.list();

                options.callback({ data: data, total: data.length });
            };

            self.softwareDistributionCentres_handle = self.softwareDistributionCentres.list.subscribe(function (newObject) {
                if (newObject != null) {
                    vm.object().targetSoftwareDistributionCentre(newObject[0]);
                }
            });
            
            //Редактирование (выбор) Модели ПО
            self.EditSoftwareModel = function () {
                require(['ui_forms/Asset/AssetOperations/templateParamsUpdate/frmSoftwareTableSelectorModel'], function (fhModule) {
                    var form = new fhModule.Form(
                        vm.object,
                        function (softwareModel) {
                            if (!softwareModel)
                                return;

                            var object = vm.object();
                            //Модель ПО / версия ПО
                            object.SoftwareModelName(softwareModel.Name + ' / ' + softwareModel.Version);
                            object.SoftwareModelID(softwareModel.ID);
                        },
                        false,
                        self.$region);
                    form.Show();
                });
            }

            //Редактирование (выбор) Модели лицензии ПО
            self.EditSoftwareLicenceModel = function () {
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var object = vm.object();
                    fh.ShowSoftwareLicenceModelLink(
                        false,
                        false,
                        function (newValues) {
                            if (!newValues || newValues.length == 0)
                                return;
                            //
                            if (newValues.length == 1) {
                                var model = newValues[0];

                                //1 Количество прав
                                //Не задано
                                self.SoftwareExecutionCountIsNotDefined(model.SoftwareExecutionCountIsNotDefined);
                                //Без ограничений
                                self.SoftwareExecutionUnlimited(model.SoftwareExecutionUnlimited);
                                //Задано
                                self.SoftwareExecutionCountSet(model.SoftwareExecutionCountSet);

                                //Количество прав
                                if (model.SoftwareExecutionCount) {
                                    object.Count(model.SoftwareExecutionCount);
                                } else if (!model.SoftwareExecutionUnlimited) {
                                    object.Count(1);
                                }

                                //2 Ограничение, дней
                                object.LimitInDays(model.LimitInHours ? (model.LimitInHours / 24) : 0);

                                //3 Тип продукта 
                                object.ProductCatalogTypeName(model.SoftwareTypeName);
                                object.ProductCatalogTypeID(model.SoftwareTypeID);

                                //4 Модель лицензии:
                                object.SoftwareModelName(model.SoftwareModelName);
                                object.SoftwareModelID(model.SoftwareModelID);

                                //5 Модель ПО / версия ПО
                                object.SoftwareLicenceModelName(model.Name);
                                object.SoftwareLicenceModelID(model.ID);

                                //6 Производитель ПО
                                object.ManufacturerID(model.ManufacturerID);
                                object.ManufacturerName(model.ManufacturerName);

                                //7 Вид лицензии
                                object.LicenceTypeID(model.SoftwareLicenceType);
                                object.LicenceTypeName(model.SoftwareLicenceTypeName);

                                //8 Схема лицензирования
                                object.SoftwareLicenceSchemeID(model.SoftwareLicenceScheme);
                                object.SoftwareLicenceSchemeName(model.SoftwareLicenceSchemeName);

                                //9
                                object.IsFull(model.IsFull);

                                //10
                                object.ProductCatalogCategoryName(model.ProductCatalogCategoryName);

                                //11 
                                object.DowngradeAvailable(model.DowngradeAvailable);

                                //12
                                object.ProductCatalogTemplate(model.ProductCatalogTemplate);
                                // LocationRestrictions
                                object.IsRestrictionsLocationVisible(model.IsSoftwareLicenceSchemeHasLocationRestriction);
                                if (model.IsSoftwareLicenceSchemeHasLocationRestriction === true) {
                                    $('.resrtiction-location .messageContainer').css('height', '100%');
                                }

                            }
                        },
                        true,
                        {
                            TemplateClassID: 223,
                            HasLifeCycle: false,
                        },
                        getTextResource('SoftwareModelLinkHeader')
                    );
                });
            };

            self.IsModelChosen = ko.computed(function () {
                var object = vm.object();
                return object && object.SoftwareLicenceModelID();
            });

            //редактирование поля "Действительна с"
            self.EditBeginDate = function () {
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
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                });
            };

            //редактирование поля "Действительна по"
            self.EditEndDate = function () {
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
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                });
            };

            //редактирование вида лицензии
            self.EditLicenceType = function () {
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
                        },
                        nosave: true
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
                                object.SoftwareLicenceSchemeID(model.ID);
                                object.SoftwareLicenceSchemeName(model.Name);
                                object.IsRestrictionsLocationVisible(model.IsLocationRestrictionApplicable);
                                vm.raiseObjectModified();
                                return true;
                            }
                        },
                        true,
                        {
                            TemplateClassID: 745,
                            HasLifeCycle: false,
                        },
                        getTextResource('SoftwareLicenceScheme_Select')
                    );
                });
            };

            // показать Схему лицензирвоания
            self.OpenSoftwareLicenceSchemeForm = function () {
                require(['assetForms'], function (module) {
                    var object = vm.object();
                    var fh = new module.formHelper(true);
                    fh.ShowLicenceSchemeForm(object.SoftwareLicenceSchemeID());
                });

            };

            //редактирование поля "Возможность downgrade"
            self.EditDowngradeAvailable = function () {
                //
                var checkbox = vm.$region.find('#downgradeAvailableCheckbox');
                //
                if (!checkbox || !checkbox[0])
                    return;
                //
                var newValue = checkbox[0].checked;
                //
                var obj = vm.object();
                obj.DowngradeAvailable(newValue);
                checkbox[0].checked = !checkbox[0].checked;
            };

            //редактирование поля "Количество прав"
            self.EditCount = function () {

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
                        maxLength: 100000,
                        minValue: 1,
                        onSave: function (newCount) {
                            object.Count(newCount);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };

            //редактирование поля "Ограничение, дней"
            self.EditLimitInDays = function () {
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
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
            };

            //редактирование поля "Описание"
            self.EditNote = function () {
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
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };

            //редактирование местоположения TODO
            self.ajaxControl_location = new ajax.control();
            self.EditLocation = function () {
                showSpinner();
                var asset = vm.object();
                //
                require(['ui_forms/Asset/frmAssetLocation', 'sweetAlert'], function (module) {
                    //режим выбора местоположения
                    var locationType = module.LocationType.SoftwareLicenceLocation;

                    //когда новое местоположение будет выбрано
                    var saveLocation = function (objectInfo) {
                        if (!objectInfo)
                            return;

                        self.ajaxControl_location.Ajax(
                            null,
                            {
                                url: '/imApi/GetAssetLocationInfo',
                                method: 'POST',
                                data: {
                                    DeviceID: null,
                                    DeviceClassID: null,
                                    LocationID: objectInfo.ID,
                                    LocationClassID: objectInfo.ClassID
                                }
                            },
                            function (response) {
                                if (response && response.Result === 0) {
                                    var info = response.AssetLocationInfo;
                                    asset.RoomGuidID(objectInfo.ID);
                                    asset.OrganizationName(info.OrganizationName);
                                    asset.BuildingName(info.BuildingName);
                                    asset.RoomIntID(info.RoomIntID);
                                    asset.RoomWithFloorName(info.RoomName);
                                }
                            });
                    };
                    //
                    module.ShowDialog(locationType, null, saveLocation, true);
                });
            };


            //редактирование типа ограничения процессоров - диапазон
            self.EditCpuRestrictionsRadioRange = function () {
                var object = vm.object();
                if (object.RestrictionsCPUFrom() == null)
                    object.RestrictionsCPUFrom(1);
                if (object.RestrictionsCPUTill() == null)
                    object.RestrictionsCPUTill(1);
                return true;
            };

            // Редкатриования верхнего лимита ограничения числа процессоров
            self.EditRestrictionsCPUFrom = function () {
                showSpinner();
                var object = vm.object();
                var oldValue = object.RestrictionsCPUFrom();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'RestrictionsCPUFrom',
                        fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CPUCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_From'),
                        oldValue: object.RestrictionsCPUFrom() == null ? 1 : object.RestrictionsCPUFrom(),
                        maxLength: 255,
                        minValue: 1,
                        onSave: function (newValue) {
                            object.RestrictionsCPUFrom(newValue);
                            if (object.RestrictionsCPUTill() == null || object.RestrictionsCPUTill() < newValue)
                                object.RestrictionsCPUTill(newValue);
                            vm.raiseObjectModified();
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
                return true;
            };

            // Редактирвоание нижнего лимита ограничения числа процессоров
            self.EditRestrictionsCPUTill = function () {
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
                        fieldName: 'RestrictionsCPUTill',
                        fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CPUCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_Till'),
                        oldValue: object.RestrictionsCPUTill() == null ? 1 : object.RestrictionsCPUTill(),
                        maxLength: 255,
                        minValue: 1,
                        onSave: function (newValue) {
                            object.RestrictionsCPUTill(newValue);
                            if (object.RestrictionsCPUFrom() == null || object.RestrictionsCPUFrom() > newValue)
                                object.RestrictionsCPUFrom(newValue);
                            vm.raiseObjectModified();
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
                return true;

            };

            //редактирование типа ограничения ядер - диапазон
            self.EditCoreRestrictionsRadioRange = function () {
                var object = vm.object();
                if (object.RestrictionsCoreFrom() == null)
                    object.RestrictionsCoreFrom(1);
                if (object.RestrictionsCoreTill() == null)
                    object.RestrictionsCoreTill(1);
                return true;
            };

            // Редкатриования верхнего лимита ограничения числа ядер
            self.EditRestrictionsCoreFrom = function () {
                showSpinner();
                var object = vm.object();
                var oldValue = object.RestrictionsCoreFrom();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'RestrictionsCoreFrom',
                        fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CoreCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_From'),
                        oldValue: object.RestrictionsCoreFrom() == null ? 1 : object.RestrictionsCoreFrom(),
                        maxLength: 255,
                        minValue: 1,
                        onSave: function (newValue) {
                            object.RestrictionsCoreFrom(newValue);
                            if (object.RestrictionsCoreTill() == null || object.RestrictionsCoreTill() < newValue)
                                object.RestrictionsCoreTill(newValue);
                            vm.raiseObjectModified();
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
                return true;
            };

            // Редактирвоание нижнего лимита ограничения числа ядер
            self.EditRestrictionsCoreTill = function () {
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
                        fieldName: 'RestrictionsCoreTill',
                        fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_CoreCount') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_Till'),
                        oldValue: object.RestrictionsCoreTill() == null ? 1 : object.RestrictionsCoreTill(),
                        maxLength: 255,
                        minValue: 1,
                        onSave: function (newValue) {
                            object.RestrictionsCoreTill(newValue);
                            if (object.RestrictionsCoreFrom() == null || object.RestrictionsCoreFrom() > newValue)
                                object.RestrictionsCoreFrom(newValue);
                            vm.raiseObjectModified();
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
                return true;

            };

            //редактирование типа ограничения ядер - диапазон
            self.EditHzRestrictionsRadioRange = function () {
                var object = vm.object();
                if (object.RestrictionsHzFrom() == null)
                    object.RestrictionsHzFrom(1);
                if (object.RestrictionsHzTill() == null)
                    object.RestrictionsHzTill(1);
                return true;
            };

            // Редкатриования верхнего лимита ограничения числа ядер
            self.EditRestrictionsHzFrom = function () {
                showSpinner();
                var object = vm.object();
                var oldValue = object.RestrictionsHzFrom();
                require(['usualForms'], function (fhModule) {
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        ID: object.ID(),
                        objClassID: object.ClassID,
                        fieldName: 'RestrictionsHzFrom',
                        fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_Frequency') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_From'),
                        oldValue: object.RestrictionsHzFrom() == null ? 1 : object.RestrictionsHzFrom(),
                        maxLength: 255,
                        minValue: 1,
                        onSave: function (newValue) {
                            object.RestrictionsHzFrom(newValue);
                            if (object.RestrictionsHzTill() == null || object.RestrictionsHzTill() < newValue)
                                object.RestrictionsHzTill(newValue);
                            vm.raiseObjectModified();
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
                return true;
            };

            // Редактирвоание нижнего лимита ограничения числа ядер
            self.EditRestrictionsHzTill = function () {
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
                        fieldName: 'RestrictionsHzTill',
                        fieldFriendlyName: getTextResource('SoftwareLicence_Restrictions_Frequency') + ' ' + getTextResource('SoftwareLicence_Restrictions_Range_Till'),
                        oldValue: object.RestrictionsHzTill() == null ? 1 : object.RestrictionsHzTill(),
                        maxLength: 255,
                        minValue: 1,
                        onSave: function (newValue) {
                            object.RestrictionsHzTill(newValue);
                            if (object.RestrictionsHzFrom() == null || object.RestrictionsHzFrom() > newValue)
                                object.RestrictionsHzFrom(newValue);
                            vm.raiseObjectModified();
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                });
                return true;

            };


            ////Видимость блоков
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

            //----------------------------------------------------------------------------------------------------------------------------------------------

            self.dateTime_controls = [];
            self.searcher_controls = [];

            //when tab validating
            self.validate = function () {
                var object = vm.object();
                //
                if (object.isOEM() && !object.OEMDeviceSpecified()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('SoftwareLicence_OEMDevice_MustBeSet'));
                    });
                    return false;
                }
                if (object.IsRestrictionsLocationVisible() && !(object.RestrictionsLocations().length > 0)) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('Restriction_NeedOneLocation'));
                    });
                    return false;
                }
                return true;
            };

            self.AfterRender = function () {

                self.initDateControl('.beginDate', vm.object().BeginDateDT, vm.object().BeginDate);
                self.initDateControl('.endDate', vm.object().EndDateDT, vm.object().EndDate);

                for (var i = 0; i < self.dateTime_controls.length; i++)
                    self.dateTime_controls[i].datetimepicker('destroy');
                self.dateTime_controls.splice(0, self.dateTime_controls.length - 1);
            }

            self.initDateControl = function (selector, ko_value, ko_valueString) {
                var $frm = $('#' + vm.frm.GetRegionID()).find('.frmSoftwareLicence');
                var $div = $frm.find(selector);
                require(['dateTimePicker'], function () {
                    if (locale && locale.length > 0)
                        $.datetimepicker.setLocale(locale.substring(0, 2));
                    var control = $div.datetimepicker({
                        startDate: ko_value(),
                        closeOnDateSelect: true,
                        format: 'd.m.Y',
                        mask: '39.19.9999',
                        timepicker: false,
                        dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                        value: ko_value(),
                        validateOnBlur: true,
                        onSelectDate: function (current_time, $input) {
                            ko_valueString(dtLib.Date2String(current_time, true));
                        }
                    });
                    self.dateTime_controls.push(control);
                });
            };

            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
            };
            //editors
            {
                //редактирование поля "Без ограничений количества прав"
                self.EditSoftwareUsageNoLimits = function () {
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
                        newCountValue = 0;
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
                                    obj.Count(obj.SoftwareUsageNoLimits() ? null : 0);
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
                            object.HASPAdapterID(newHASPAdapter[0].ID);
                            object.HASPAdapterName(newHASPAdapter[0].Model);
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

                self.ClearHASPAdapter = function () {
                    var object = vm.object();
                    object.HASPAdapterID(null);
                    object.HASPAdapterName('');
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

                //Видимость поля "Выдано"
                self.IsInUseVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate == self.ProductCatalogTemplateIDRent
                        )
                        : true;
                });

                //Видимость поля "Свободно"
                self.IsBalanceVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent
                        )
                        : true;
                });

                //Видимость поля "Действительна с"
                self.IsBeginDateVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent)
                        : true;
                });

                //Видимость поля "Действительна по"
                self.IsEndDateVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent)
                        : true;
                });

                //Видимость поля "Ограничение, дней"
                self.IsLimitInDaysVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDApply)
                        : true;
                });

                //Видимость блока "Аппаратный ключ защиты"
                self.IsHASPAdapterVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate == self.ProductCatalogTemplateIDSingle
                            || object.ProductCatalogTemplate == self.ProductCatalogTemplateIDSubscription
                            || object.ProductCatalogTemplate == self.ProductCatalogTemplateIDRent
                        )
                        : true;
                });

                //Видимость блока "Cвязанная лицензия"
                self.IsParentSoftwareLicenceVisible = ko.computed(function () {
                    var object = vm.object();
                    return (object) ?
                        (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDApply
                            || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDUpgrade)
                        : true;
                });

                //Видимость блока "Связи"
                self.IsLinksDivVisible = ko.computed(function () {
                    return self.IsHASPAdapterVisible() || self.IsParentSoftwareLicenceVisible();
                });

                // OEM Device
                self.OpenSelectOEMDevice = function () {
                    var retvalD = $.Deferred();
                    //var ko_object = vm.object();
                    //
                    if (vm.object() != null) {
                        var fh = new fhModule.formHelper();
                        //    fh.ShowAssetForm(ko_object.OEMDeviceID(), null);
                        fh.ShowAssetLink({
                            ClassID: 189,
                            ID: vm.object().ID(),
                            SelectOnlyOne: true
                        }, function (newValues) {
                            if (!newValues || newValues.length != 1)
                                return;

                            showSpinner();
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
                                            retvalD.resolve(true);
                                        }
                                        else {
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                            });
                                        }
                                    }
                                });

                        });
                    }
                    else retvalD.resolve();
                    //
                    return retvalD;

                };
                self.CanOpenSelectOEMDevice = function () {
                    return true;
                };

                // Ограничения местоположения
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
                        $.when(self.locationListView.load()).done(function () {
                            $('.resrtiction-location .tableData').css('height', '100%');
                        });
                    };
                    // Получение списка именований мест расположений
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

                    // Получение списка местоположений
                    self.locationRetrieveItems = function () {
                        var retvalD = $.Deferred();
                        var idList = [];
                        var object = vm.object();
                        if (object.RestrictionsLocations()) {
                            ko.utils.arrayForEach(object.RestrictionsLocations(), function (el) {
                                idList.push(el.LocationID);
                            });
                        }
                        $.when(self.getLocationObjectList(idList, true)).done(function (objectList) {
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
                    //  Saving added locations
                    self.setRestrictionLocations = function (objectInfo, onSuccess) {//для сохранения нового местоположения
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
                                var item = { ID: ko.getNewID(), LocationID: el.ID, LocationType: el.ClassID, SoftwareLicenceID: object.ID() };
                            if (item)
                                newLocations.push(item);
                        });
                        object.RestrictionsLocations(newLocations);
                        if (onSuccess)
                            onSuccess();
                        $.when(self.locationListView.load()).done(function () {
                            $('.resrtiction-location .tableData').css('height', '100%');
                        });
                    }
                    // menu for locations restriction list
                    {
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
                        // add "ADD" menu item
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
                        // add "Delete" menu item
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
                        //  implement addining restriction location
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
                                module.ShowDialog(self.setRestrictionLocations, selectionInfo, true);
                            });
                        };
                        // implement remove restriction location
                        self.ViewSoftwareLicenceLocationDelete = function () {
                            var selectedItems = self.locationGetCheckedItems();
                            if (selectedItems.length <= 0)
                                return;
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
                                            if (selectedItems.length >= vm.object().RestrictionsLocations().length) {
                                                require(['sweetAlert'], function (swalErr) {
                                                    swalErr(getTextResource('SaveError'), getTextResource('Restriction_CantDeleteALlLocation'), 'error');
                                                });
                                                return false;
                                            }
                                            else {
                                                ko.utils.arrayForEach(selectedItems, function (todel) {
                                                    ko.utils.arrayForEach(vm.object().RestrictionsLocations(), function (loc) {
                                                        if (loc.LocationID == todel.LocationID)
                                                            vm.object().RestrictionsLocations().pop(loc);
                                                    });
                                                });
                                                $.when(self.locationListView.load()).done(function () {
                                                    $('.resrtiction-location .tableData').css('height', '100%');
                                                });
                                            }
                                        }
                                    });
                            });

                        };
                    }
                    // get checked items from location list
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

            self.CanHaveSublicences = ko.computed(function () {
                var object = vm.object();
                return (object) ?
                    (object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSingle
                        || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDSubscription
                        || object.ProductCatalogTemplate() == self.ProductCatalogTemplateIDRent
                    ) && (
                        object.SoftwareLicenceSchemeID() == 0
                        || object.SoftwareLicenceSchemeID() == 1
                        || object.SoftwareLicenceSchemeID() == 2
                        || object.SoftwareLicenceSchemeID() == 3
                        || object.SoftwareLicenceSchemeID() == 5
                    )
                    : false;
            });
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
        },
        SoftwareDistributionCentres: function () {
            var self = this;
            self.list = ko.observableArray([]);
            self.load = function () {
                var retD = $.Deferred();

                new ajax.control().Ajax(null, {
                    dataType: "json",
                    method: 'GET',
                    url: '/assetApi/SoftwareDistributionCentresByUser'
                }, function (newVal) {
                    if (newVal && newVal.Result === 0) {
                        var data = newVal.Data;
                        if (data) {
                            
                            self.list.removeAll();
                            ko.utils.arrayForEach(data.Objects, function (el) {                                
                                self.list.push(el.ObjectData);                                
                            });                            
                        }
                        retD.resolve();
                    }
                    else if (newVal && newVal.Result === 1)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                    else if (newVal && newVal.Result === 2)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                    else if (newVal && newVal.Result === 3)
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                        });
                    else
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmSublicenseTransfer.js, load]', 'error');
                        });
                });

                return retD;
            };
        }
    };
    return module;
});