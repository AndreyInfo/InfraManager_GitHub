define(['knockout', 'ajax', 'dateTimeControl', 'models/SDForms/SDForm.User', 'fingerPrintLib'], function (ko, ajax, dtLib, userLib, fingerPrint) {
    var module = {
        SoftwareInstallation: function () {

            var self = this;
            //  Инитим отпечаток
            self.fingerprintJs = fingerPrint;
            self.fingerprintJs.init();

            self.ajaxControl = new ajax.control();

            self.ClassID = function () { return 71; };
            // возможность редактирования
            self.CanEdit = ko.observable(true);

            self.ID = ko.observable('');

            // название связи   
            self.Name = ko.observable('');
            
            // Модель ПО
            // ИД
            self.ModelID = ko.observable('');
            // Наименование
            self.ModelName = ko.observable('');
            // Наименование коммерческой 
            self.ProductName = ko.observable('');
            // Место установки
            self.DeviceClass = ko.observable(0);
            self.DeviceID = ko.observable('');
            self.DeviceName = ko.observable('');
            // Путь установки
            self.InstallationPath = ko.observable('');
            // Дата установки
            self.InstallationDate = ko.observable('');
            // Дата понего обнаружения
            self.LastDiscoverDate = ko.observable('');
            // Дата создания
            self.CreateDate = ko.observable('');
            // Зависимые инсталяции
            self.DependantInstallations = ko.observableArray([]);
            // Статус архивности
            self.Status = ko.observable(0);
            self.LifeCycleStateName = ko.observable('');
            self.ProductCatalogTemplate = ko.observable(421);

            self.loadFields = function (obj) {
                self.ID(obj.ID);
                if (obj.SoftwareInstallationName) {
                    self.Name(obj.SoftwareInstallationName);
                } else {
                    self.Name('');
                }
                self.ModelID(obj.SoftwareModelID);
                self.ModelName(obj.SoftwareModelName);
                self.ProductName(obj.CommercialModelName);
                self.DeviceClass(obj.DeviceClassID);
                self.DeviceID(obj.DeviceID);
                self.DeviceName(obj.DeviceName);
                self.InstallationPath(obj.InstallPath);
                self.InstallationDate(obj.InstallDate);
                self.LastDiscoverDate(obj.DateLastSurvey);
                self.CreateDate(obj.CreateDate);
                self.Status(obj.Status);
                self.LifeCycleStateName(obj.Status > 0 ? getTextResource('software_installation_status_archive') : getTextResource('software_installation_status_active'));
            };

            // id - идентификатор объекта для подгрузки
            self.load = function (id, urlLoadData) {
                var retval = $.Deferred();
                //
                self.ID(id);

                if (id) {

                    self.ajaxControl.Ajax(null,
                        {
                            url: urlLoadData+'/'+id,
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
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareInstallation.js, Load]', 'error');
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
                        swal(getTextResource('SoftwareInstallation_Validate_NameEmpty'));
                    });
                    return false;
                }
                if (!self.DeviceID() || self.DeviceID().trim().length == 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('SoftwareInstallation_Validate_DeviceEmpty'));
                    });
                    return false;
                }
                if (!self.ModelID() || self.ModelID().trim().length == 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('SoftwareInstallation_Validate_ModelEmpty'));
                    });
                    return false;
                }

                return true;
            };

            // добавление / обновление новой связи 
            self.AddOrUpdate = function (urlSaveData) {

                if (!self.validate()) {
                    return false;
                }

                return self._addOrUpdate(urlSaveData);

            }

            // добавление/обновление новой связи..
            self._addOrUpdate = function (urlSaveData) {

                var retval = $.Deferred();

                //             
                var data = {
                    'ID': self.ID(),
                    'SoftwareInstallationName': self.Name(),
                    'SoftwareModelID': self.ModelID(),
                    'DeviceClassID': self.DeviceClass(),
                    'DeviceID': self.DeviceID(),
                    'InstallPath': self.InstallationPath(),
                    'InstallDate': self.InstallationDate(),
                };

                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: urlSaveData,
                        method: 'PUT',
                        dataType: 'json',
                        data: data,
                        headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                    },
                    function (response) {//AddSoftwareLicenceOutModel
                        hideSpinner();

                        if (response) {
                            if (!response.Success)
                                require(['sweetAlert'], function () {
                                    swal(response.Fault);//some problem
                                });
                            //
                            if (response.Success) {//ok 
                                retval.resolve(response.Result);
                                return;
                            }
                        }                    
                        retval.resolve(null);
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareInstallation.js, add]', 'error');
                        });
                        retval.resolve(null);
                    });

                //
                return retval.promise();
            };

            self.IsSoftwareModelExists = function () {
                //  TODO: uncomment when form for Model will be implemented
                //if (self.ModelID())
                //    return true;
                return false;
            };
            self.IsDeviceExists = function () {
                if (self.DeviceID())
                    return tru;
                return false;
            }
        }
    };
    return module;
});