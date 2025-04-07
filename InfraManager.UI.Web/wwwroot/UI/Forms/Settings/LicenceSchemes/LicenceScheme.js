define(['knockout', 'ajax', 'dateTimeControl', 'models/SDForms/SDForm.User', 'fingerPrintLib'], function (ko, ajax, dtLib, userLib, fingerPrint) {
    var module = {
        licenceScheme: function () {

            var self = this;
            //  Инитим отпечаток
            self.fingerprintJs = fingerPrint;
            self.fingerprintJs.init();

            self.ajaxControl = new ajax.control();

            self.ID = ko.observable('');

            self.ClassID = ko.observable(745);

            // возможность редактирования
            self.CanEdit = ko.observable(true);

            // название лицензии
            self.Name = ko.observable('');
            
            // =========== Блок "Общее" ===============
            // описание
            self.Description = ko.observable('');


            // =========== Блок "Настройки" =================

            // объект лицензирования (ID)
            self.LicensingObjectType = ko.observable(0);

            // объект лицензирования (название)
            self.LicensingObjectTypeLabel = ko.observable(null);
            
            // требуемое кол-во прав на объект
            self.LicenseCountPerObject = ko.observable('');

            // привязывать права к объектам
            self.IsLinkLicenseToObject = ko.observable(false);            

            self.IsLinkLicenseToObjectEnabled = ko.pureComputed({
                read: function () {
                    return self.IsLinkLicenseToObject();
                },
                write: function (value) {
                    self.IsLinkLicenseToObject(value == true);
                }
            });


            // лицензировать доступ пользователей
            self.IsLinkLicenseToUser = ko.observable(false);            

            self.IsLinkLicenseToUserEnabled = ko.pureComputed({
                read: function () {
                    return self.IsLinkLicenseToUser();
                },
                write: function (value) {
                    self.IsLinkLicenseToUser(value == true);
                }
            });

            // лицензировать все хосты кластера (для кластеров)
            self.IsLicenseAllHosts = ko.observable(false);            

            self.IsLicenseAllHostsEnabled = ko.pureComputed({
                read: function () {
                    return self.IsLicenseAllHosts();
                },
                write: function (value) {
                    self.IsLicenseAllHosts(value == true);
                }
            });

              

            // Тип схемы  лицензирования
            // 0 - Пользовательская
            // 1 - Системная
            self.SchemeType = ko.observable(0); 

            // Название схемы лицензирования
            self.SchemeTypeLabel = ko.observable('Пользовательская'); 

            // пользовательская схема лицензирования
            self.IsUserLicenceScheme = ko.pureComputed({
                read: function () {
                    return self.SchemeType() == 0;
                }
            });

            // допустимое число инсталляций на сервере бесконечно
            self.InstallationLimitUnlimit = ko.observable('0');     

            // кол-во  допустимых инсталляций на сервере
            self.InstallationLimits = ko.observable(1);         

            // число инсталляций на виртуальных машинах
            self.AllowInstallOnVM = ko.observable('0');           

            // число инсталяций на виртуальных машинах
            self.InstallationLimitPerVM = ko.observable(1);       

            // кол-во дополнительных прав
            self.AdditionalRights = ko.observable(' ');

            // pазмер увеличения числа инсталляций на виртуальных машинах
            self.IncreaseForVM = ko.observable(0);            

            // коэффициенты
            self.Coefficients = ko.observableArray([]);

            self.loadFields = function (obj) {
                self.ID(obj.ID);
                self.Name(obj.Name)
                self.Description(obj.Description);
                self.SchemeType(obj.SchemeType);
                self.SchemeTypeLabel(obj.SchemeTypeLabel);
                self.LicensingObjectType(obj.LicensingObjectType);
                self.LicensingObjectTypeLabel(obj.LicensingObjectTypeLabel);
                self.LicenseCountPerObject(obj.LicenseCountPerObject);
                self.IsLinkLicenseToObject(obj.IsLinkLicenseToObject);
                self.IsLicenseAllHosts(obj.IsLicenseAllHosts);
                self.IsLinkLicenseToUser(obj.IsLinkLicenseToUser);                
                self.InstallationLimitUnlimit(obj.InstallationLimits == null ? '1' : '0'); // 1 - Без ограничений, 0 - конкретное значение;
                self.InstallationLimits(obj.InstallationLimits == null ? 0 : obj.InstallationLimits);
                self.AllowInstallOnVM(String(obj.AllowInstallOnVM));
                self.InstallationLimitPerVM(obj.InstallationLimitPerVM);
                self.AdditionalRights(obj.AdditionalRights);
                self.IncreaseForVM(obj.IncreaseForVM);
                self.Coefficients(obj.SoftwareLicenceSchemeCoefficients);
                if (obj.SchemeType == 1)
                    self.CanEdit(false);
            };

            // id - идентификатор объекта для подгрузки
            self.load = function (id, urlLoadData) {
                var retval = $.Deferred();
                //
                self.ID(id);

                if (id) {

                    self.ajaxControl.Ajax(null,
                        {
                            url: urlLoadData + '/' + id,
                            method: 'GET',
                            headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                        },
                        function (response) {
                            if (response) {
                                if (!response.Success)
                                    require(['sweetAlert'], function () {
                                        swal(response.Fault);//some problem
                                    });
                                //
                                if (response.Success) {//ok 
                                    var obj = response.Result;
                                    self.loadFields(obj);
                                    retval.resolve(true);
                                }
                            }                         
                        },
                        function () {
                            hideSpinner();
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[LicenceScheme.js, Load]', 'error');
                            });
                            retval.resolve(null);
                        });
                }
                    else
                {
                    retval.resolve(false);
                }

                return retval;
            };

            self.validate = function () {

                if (!self.Name() || self.Name().trim().length == 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('LicenceScheme_Validate_NameEmpty'));
                    });
                    return false;
                }


                if (!self.LicensingObjectType() || self.LicensingObjectType() == 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('LicenceScheme_Validate_LicenceObjectEmpty'));
                    });
                    return false;
                }

                return true;
            };

            // добавление / обновление новой схемы лицензирования
            self.AddOrUpdate = function (openLicenceSchemeForm, urlSaveData) {

                if (!self.validate()) {
                    return false;
                }

                return self._addOrUpdate(openLicenceSchemeForm, urlSaveData);

            }

            // добавление/обновление новой схемы лицензирования
            self._addOrUpdate = function (openLicenceSchemeForm, urlSaveData) {

                var retval = $.Deferred();

                //             
                var data = {
                    // идентификатор схемы лицензирования
                    'ID': self.ID() === '' ? '00000000-0000-0000-0000-000000000000' : self.ID(),
                    // название схемы лицензирования
                    'Name': self.Name(),
                    // описание схемы лицензирования
                    'Description': self.Description(),
                    // Тип схемы лицензирования
                    'SchemeType': self.SchemeType(),
                    // объект лицензирования (ID)
                    'LicensingObjectType': self.LicensingObjectType(),
                    //// объект лицензирования, название
                    //'LicensingObjectTypeLabel': self.LicensingObjectTypeLabel(),                    
                    // требуемое кол-во прав на объект
                    'LicenseCountPerObject': self.LicenseCountPerObject(),
                     // привязывать права к объектам
                    'IsLinkLicenseToObject': self.IsLinkLicenseToObject(),
                    // лицензировать все хосты кластера (для кластеров)
                    'IsLicenseAllHosts': self.IsLicenseAllHosts(),
                    // лицензировать доступ пользователей
                    'IsLinkLicenseToUser': self.IsLinkLicenseToUser(),                                        
                    // кол-во  допустимых инсталляций на сервере
                    'InstallationLimits': self.InstallationLimitUnlimit() != 0 ? null : self.InstallationLimits(),
                    // число инстялляций на виртуальных машинах (тип)
                    'AllowInstallOnVM': self.AllowInstallOnVM(),
                    // число инстялляций на виртуальных машинах
                    'InstallationLimitPerVM': self.InstallationLimitPerVM(),
                    // кол-во дополнительных прав
                    'AdditionalRights': self.AdditionalRights(),
                    // pазмер увеличения числа инсталляций на виртуальных машинах
                    'IncreaseForVM': self.IncreaseForVM(),
                    //  коэффициенты
                    'SoftwareLicenceSchemeCoefficients': self.Coefficients()
                };
                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: urlSaveData,
                        method: self.ID()? 'PUT' : 'POST',
                        dataType: 'json',
                        contentType: "application/json",
                        data: JSON.stringify(data),
                        headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                    },
                    function (response) {//AddSoftwareLicenceOutModel
                        hideSpinner();

                        if (response) {
                            if (!response.Success)
                                require(['sweetAlert'], function () {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), response.Fault, 'error');
                                    });
                                });
                            //
                            if (response.Success) {//ok 
                                if (openLicenceSchemeForm) {
                                    require(['ui_forms/Settings/LicenceSchemes/frmLicenceScheme'], function (jsModule) {
                                        jsModule.ShowDialog(response.Result, false, true);
                                    });
                                }
                                retval.resolve(response.Result);
                                return;
                            }
                        }                    
                        retval.resolve(null);
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[LicenceScheme.js, add]', 'error');
                        });
                        retval.resolve(null);
                    });

                //
                return retval.promise();
            };
        }
    };
    return module;
});