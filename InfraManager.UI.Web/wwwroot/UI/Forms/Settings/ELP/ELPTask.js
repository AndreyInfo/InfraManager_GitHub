define(['knockout', 'ajax', 'dateTimeControl', 'models/SDForms/SDForm.User', 'fingerPrintLib'], function (ko, ajax, dtLib, userLib, fingerPrint) {
    var module = {
        ELPTask: function () {

            var self = this;
            //  Инитим отпечаток
            self.fingerprintJs = fingerPrint;
            self.fingerprintJs.init();

            self.ajaxControl = new ajax.control();

            self.ClassID = 820;
            // возможность редактирования
            self.CanEdit = ko.observable(true);

            self.ID = ko.observable('');

            // название связи   
            self.Name = ko.observable('');
            
            // описание
            self.Description = ko.observable('');

            // Производитель
            self.VendorID = ko.observable(null);
            self.VendorLabel = ko.observable('');
            self.ELPVendorAny = ko.observable('1');

            self.loadFields = function (obj) {
                self.ID(obj.ID);
                self.Name(obj.Name)
                self.Description(obj.Description);
                self.VendorID(obj.VendorID);
                self.VendorLabel(obj.VendorName);
                self.ELPVendorAny(obj.VendorID ? '0': '1');
            };

            // id - идентификатор объекта для подгрузки
            self.load = function (id, urlLoadData) {
                var retval = $.Deferred();
                //
                self.ID(id);

                if (id) {

                    self.ajaxControl.Ajax(null,
                        {
                            url: `${urlLoadData}/${id}`,
                            method: 'GET',
                            headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                        },
                        function (response) {
                            if (self.loadFields(response));
                            retval.resolve(true);
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
                        swal(getTextResource('ELPTask_Validate_NameEmpty'), '', 'error');
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
                    // название схемы лицензирования
                    'Name': self.Name(),
                    // описание схемы лицензирования
                    'Description': self.Description(),
                    // Производитель
                    'VendorID': self.VendorID()
                };

                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: self.ID() ? `${urlSaveData}/${self.ID()}` : urlSaveData,
                        method: self.ID() ? 'PUT' : 'POST',
                        dataType: 'json',
                        data: data,
                        headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                    },
                    function (response) {
                        hideSpinner();
                        if (response) {
                            retval.resolve(response);
                            return;
                        } else {
                            retval.resolve(null);
                        }
                        
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[ELPTask.js, add]', 'error');
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