define(['knockout', 'ajax', 'dateTimeControl', 'models/SDForms/SDForm.User', 'iconHelper'], function (ko, ajax, dtLib, userLib, ihLib) {
    var module = {
        softwareLicence: function () {
            var self = this;

            self.ajaxControl = new ajax.control();
            self.ID = ko.observable(null);
            self.ClassID = ko.observable(223); //OBJ_SoftwareLicence = 223

            self.LifeCycleStateName = ko.observable(null);

            //
            self.CanEdit = ko.observable(true);

            //Название лицензии
            self.Name = ko.observable(null);

            //Инвентарный номер
            self.InventoryNumber = ko.observable('');

            //==Блок "Лимиты"==
            //Количество прав
            self.Count = ko.observable(null);

            //Количество прав для отображения на форме
            self.CountForForm = ko.computed(function () {
                return self.Count() != null ? self.Count() : '-';
            });

            //Количество прав: без ограничений
            self.SoftwareUsageNoLimits = ko.observable(false);
            self.SoftwareUsageNoLimitsForForm = ko.pureComputed({
                read: function () {
                    return self.SoftwareUsageNoLimits() === true;
                },
                write: function (value) {
                    self.SoftwareUsageNoLimits(value);
                    if (value) {
                        self.Count(0);
                    } else {
                        self.Count(1);
                    }
                }
            });

            //Выдано
            self.InUseReferenceCount = ko.observable(null);

            //Свободно
            self.Balance = ko.observable(null);
            self.BalanceComputed = ko.computed(function () {
                return self.SoftwareUsageNoLimits()
                    ? '∞'
                    : self.Count() - self.InUseReferenceCount();
            });

            //Действительна с
            self.BeginDate = ko.observable(null);
            self.BeginDateDT = ko.observable(null);

            //Действительна по
            self.EndDate = ko.observable(null);
            self.EndDateDT = ko.observable(null);

            //Ограничение, дней
            self.LimitInDays = ko.observable(null);

            //==Блок "Местоположение"==
            //Организация
            self.OrganizationName = ko.observable('-');

            //Здание
            self.BuildingName = ko.observable('-');

            //Этаж
            self.FloorName = ko.observable('-');

            //Комната
            self.RoomName = ko.observable('-');

            //Id комнаты (int)
            self.RoomIntID = ko.observable(null);

            //Id комнаты (Guid)
            self.RoomGuidID = ko.observable(null);

            //Комната на этаже
            self.RoomWithFloorName = ko.observable('-');

            //==Блок "Информация"==
            //Описание
            self.Note = ko.observable(' ');

            //==Блок "Характеристики"==
            //Вид лицензии (ID)
            self.LicenceTypeID = ko.observable(null);
            //Вид лицензии (название)
            self.LicenceTypeName = ko.observable(null);

            //Схема лицензирования (ID)
            self.SoftwareLicenceSchemeID = ko.observable(null);
            //Схема лицензирования (название)
            self.SoftwareLicenceSchemeName = ko.observable(null);
            // Задана ли Лицензия
            self.IsLicenceSchemeExists = ko.computed(function () {
                return self.SoftwareLicenceSchemeID() != null;
            })

            //ID аппаратного ключа
            self.HASPAdapterID = ko.observable(null);
            //Название аппаратного ключа
            self.HASPAdapterName = ko.observable('');

            //Проставлен ли аппаратный ключ
            self.IsHASPAdapterExists = ko.computed(function () {
                return self.HASPAdapterID() != null;
            });

            //ID связанной лицензии
            self.ParentSoftwareLicenceID = ko.observable(null);
            //Название связанной лицензии
            self.ParentSoftwareLicenceName = ko.observable('');

            //Проставлена ли связанная лицензия
            self.IsParentSoftwareLicenceExists = ko.computed(function () {
                return self.ParentSoftwareLicenceID() != null;
            });

            //Разрешён downgrade
            self.DowngradeAvailable = ko.observable(false);
            self.DowngradeAvailableForForm = ko.pureComputed({
                read: function () {
                    return self.DowngradeAvailable() === true;
                },
                write: function (value) {
                    self.DowngradeAvailable(value);
                }
            });

            //Характер лицензии (ID)
            self.SoftwareModelTemplateID = ko.observable(null);
            //Характер лицензии (название)
            self.SoftwareModelTemplateName = ko.observable('');


            //==Блок "Классификатор"==
            //Категория
            self.ProductCatalogCategoryName = ko.observable(null);

            //Тип
            self.ProductCatalogTypeName = ko.observable(null);

            //Тип (ID)
            self.ProductCatalogTypeID = ko.observable(null);

            //Класс категории
            self.ProductCatalogTemplate = ko.observable(null);

            //Модель продукта (SoftwareModel) (ID)
            self.SoftwareModelID = ko.observable(null);

            //Модель продукта
            self.SoftwareModelName = ko.observable(null);

            //Модель ПО (SoftwareLicenceModel) (ID)
            self.SoftwareLicenceModelID = ko.observable(null);

            //Модель ПО
            self.SoftwareLicenceModelName = ko.observable(null);

            //Модель (ID)
            self.SoftwareTypeID = ko.observable(null);

            //Модель
            self.SoftwareTypeName = ko.observable(null);

            //Производитель (ID)
            self.ManufacturerID = ko.observable(null);

            //Производитель ПО
            self.ManufacturerName = ko.observable(null);

            //Описание
            self.Description = ko.observable(null);

            //Полный путь до объекта
            self.CategoryFullName = ko.observable();

            //==Блок Ограничения
            //
            self.RestrictionsCPUFrom = ko.observable(null);
            self.RestrictionsCPUTill = ko.observable(null);
            self.RestrictionsCoreFrom = ko.observable(null);
            self.RestrictionsCoreTill = ko.observable(null);
            self.RestrictionsHzFrom = ko.observable(null);
            self.RestrictionsHzTill = ko.observable(null);

            self.RestrictionsCPUUnlimit = ko.observable('1');
            self.RestrictionsCoreUnlimit = ko.observable('1');
            self.RestrictionsHzUnlimit = ko.observable('1');

            //  Ограничения метоположения
            self.IsRestrictionsLocationVisible = ko.observable(false);
            self.RestrictionsLocations = ko.observableArray([]);

            //  ==  OEM DEvice
            self.OEMDeviceID = ko.observable(null);
            self.OEMDeviceClassID = ko.observable(null);
            self.OEMDeviceLocation = ko.observable('');
            self.OEMDeviceFullName = ko.observable('');
            self.OEMDeviceType = ko.observable('');
            self.OEMDeviceIconClass = ko.computed(function () {
                if (self.OEMDeviceClassID())
                    return ihLib.getIconByClassID(self.OEMDeviceClassID());
                return '';
            });
            self.OEMDeviceSpecified = ko.computed(function () {
                if (self.OEMDeviceClassID())
                    return true;
                return false;
            });
            //
            self.isOEM = ko.computed(function () {
                if (self.ProductCatalogTemplate() == 189)
                    return true;
                return false;
            });
            self.isNotOEM = ko.computed(function () {
                return !self.isOEM();
            });
            self.OEMDeviceSetClass = function (classID) {

            };

            //
            self.IsFull = ko.observable(true);

            //
            self.ShowUtilization = ko.computed(function () {
                return true;
            });
            //
            self.ShowUtilizer = ko.computed(function () {
                return self.ShowUtilization();
            });
            //
            self.UtilizerID = ko.observable('');
            //
            self.UtilizerClassID = ko.observable('');
            //
            self.UtilizerLoaded = ko.observable(false);
            self.Utilizer = ko.observable(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditUtilizer, false, false));
            //
            self.CreateFullRoomName = ko.computed(function () {
                return self.FloorName() != 'Нет' ? 'Этаж ' + self.FloorName() + ' / ' + self.RoomName() : 'Нет';
            });
            //
            self.CanHaveSublicenses = false;
            self.targetSoftwareDistributionCentre = ko.observable(null);

            //файлы для добавления
            self.files = ko.observableArray([]);

            self.loadFields = function (obj, OEMDevice, IsSiteLicenceScheme) {
                self.ID(obj != null ? obj.ID : null);               
                self.Name(obj != null ? obj.Name : null);
                self.InventoryNumber(obj != null ? obj.InventoryNumber : null);
                self.Description(obj != null ? obj.Description : null);
                self.Count(obj != null ? obj.Count : null);
                self.InUseReferenceCount(obj != null ? obj.InUseReferenceCount : null);
                self.Balance(obj != null ? obj.Balance : null);

                self.BeginDate(obj != null ? parseDate(obj.BeginDate, true) : null); //show only date
                self.BeginDateDT(obj != null ? new Date(parseInt(obj.BeginDate)) : null);

                self.EndDate(obj != null ? parseDate(obj.EndDate, true) : null); //show only date
                self.EndDateDT(obj != null ? new Date(parseInt(obj.EndDate)) : null);

                self.OrganizationName(obj != null ? obj.OrganizationName : null);
                self.BuildingName(obj != null ? obj.BuildingName : null);
                self.FloorName(obj != null ? obj.FloorName : null);
                self.RoomName(obj != null ? obj.RoomName : null);
                self.RoomIntID(obj != null ? obj.RoomIntID : null);
                self.RoomGuidID(obj != null ? obj.RoomGuidID : null);
                self.RoomWithFloorName(self.CreateFullRoomName());
                self.LimitInDays(obj != null ? obj.LimitInDays : null);
                self.Note(obj != null ? obj.Note : '');
                self.SoftwareLicenceSchemeID(obj != null ? obj.SoftwareLicenceSchemeID : null);
                self.SoftwareLicenceSchemeName(obj != null ? obj.SoftwareLicenceSchemeName : null);
                self.DowngradeAvailable(obj != null ? obj.DowngradeAvailable : null);
                self.LicenceTypeID(obj != null ? obj.LicenceType : null);
                self.LicenceTypeName(obj != null ? obj.LicenceTypeName : null);
                self.SoftwareModelID(obj != null ? obj.SoftwareModelID : null);
                self.SoftwareModelName(obj != null ? obj.SoftwareModelName : null);
                self.SoftwareLicenceModelName(obj != null ? obj.SoftwareLicenceModelName : null);
                self.SoftwareTypeName(obj != null ? obj.SoftwareTypeName : null);
                self.SoftwareTypeID(obj != null ? obj.SoftwareTypeID : null);
                self.ManufacturerName(obj != null ? obj.ManufacturerName : null);
                self.ManufacturerID(obj != null ? obj.ManufacturerID : null);
                self.LifeCycleStateName(obj != null ? obj.LifeCycleStateName : null);
                self.SoftwareModelTemplateID(obj != null ? obj.SoftwareModelTemplateID : null);
                self.SoftwareModelTemplateName(obj != null ? obj.SoftwareModelTemplateName : null);
                self.ProductCatalogTemplate(obj != null ? obj.ProductCatalogTemplate : null);
                self.ProductCatalogTypeName(obj != null ? obj.ProductCatalogTypeName : null);
                self.ProductCatalogTypeID(obj != null ? obj.ProductCatalogTypeID : null);
                self.ProductCatalogCategoryName(obj != null ? obj.ProductCatalogCategoryName : null);
                self.HASPAdapterID(obj != null ? obj.HASPAdapterID : null);
                self.HASPAdapterName(obj != null ? obj.HASPAdapterName : null);
                self.ParentSoftwareLicenceID(obj != null ? obj.ParentSoftwareLicenceID : null);
                self.ParentSoftwareLicenceName(obj != null ? obj.ParentSoftwareLicenceName : null);

                //
                if (obj.UtilizerID) {
                    self.UtilizerID = ko.observable(obj.UtilizerID);
                } else {
                    self.UtilizerID = ko.observable('');
                }
                //
                if (obj.UtilizerClassID) {
                    self.UtilizerClassID = ko.observable(obj.UtilizerClassID);
                }
                else {
                    self.UtilizerClassID = ko.observable('');
                }
                //
                self.UtilizerLoaded = ko.observable(false);
                self.Utilizer = ko.observable(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditUtilizer, false, false));
                self.InitializeUtilizer();

                if (self.SoftwareLicenceModelName()) {
                    self.CategoryFullName(self.ProductCatalogCategoryName() + ' > ' + self.ProductCatalogTypeName() + ' > ' + self.SoftwareLicenceModelName());
                } else {
                    self.CategoryFullName(self.ProductCatalogCategoryName() + ' > ' + self.ProductCatalogTypeName());
                }

                if (obj && obj.Count !== null) {
                    self.Count(obj.Count);
                } else {
                    self.SoftwareUsageNoLimits(true);
                }

                self.CanHaveSublicenses = obj.CanHaveSublicenses;

                self.OEMDeviceClassID(obj != null ? obj.OEMDeviceClassID : null);
                self.OEMDeviceID(obj != null ? obj.OEMDeviceID : null);
                if (OEMDevice) {
                    self.OEMDeviceFullName(OEMDevice.FullName);
                    self.OEMDeviceLocation(OEMDevice.Location);
                    self.OEMDeviceType(OEMDevice.TypeName);
                }

                if (obj && obj.RestrictionsCPUFrom !== null && obj.RestrictionsCPUTill !== null) {
                    self.RestrictionsCPUFrom(obj.RestrictionsCPUFrom);
                    self.RestrictionsCPUTill(obj.RestrictionsCPUTill);
                    self.RestrictionsCPUUnlimit('0');
                } else {
                    self.RestrictionsCPUUnlimit('1');
                }
                if (obj && obj.RestrictionsCoreFrom !== null && obj.RestrictionsCoreTill !== null) {
                    self.RestrictionsCoreFrom(obj.RestrictionsCoreFrom);
                    self.RestrictionsCoreTill(obj.RestrictionsCoreTill);
                    self.RestrictionsCoreUnlimit('0');
                } else {
                    self.RestrictionsCoreUnlimit('1');
                }
                if (obj && obj.RestrictionsHzFrom !== null && obj.RestrictionsHzTill !== null) {
                    self.RestrictionsHzFrom(obj.RestrictionsHzFrom);
                    self.RestrictionsHzTill(obj.RestrictionsHzTill);
                    self.RestrictionsHzUnlimit('0');
                } else {
                    self.RestrictionsHzUnlimit('1');
                }

                self.IsRestrictionsLocationVisible(IsSiteLicenceScheme);
                if (obj.SoftwareLicenceLocations)
                    self.RestrictionsLocations(obj.SoftwareLicenceLocations);

            };

            self.load = function (id, vm) {
                var retval = $.Deferred();
                //
                self.ID(id);

                self.ajaxControl.Ajax(null,
                    {
                        url: '/sdApi/GetSoftwareLicence',
                        method: 'GET',
                        data: { ID: id }
                    },
                    function (response) {
                        if (response.Result === 0 && response.SoftwareLicence) {
                            var obj = response.SoftwareLicence;
                            
                            if (vm !== undefined)
                                vm.tab_parameters.Initialize();
                            
                            var oemDevice = response.OEMDevice;
                            self.loadFields(obj, oemDevice, response.IsSiteLicenceScheme);
                            retval.resolve(true);
                        }
                        else {
                            retval.resolve(false);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareLicence.js, Load]', 'error');
                            });
                        }
                    });

                return retval;
            };

            //
            self.EditUtilizer = function () {
                if (!self.CanEdit())
                    return;

                showSpinner();
                require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                    var fh = new module.formHelper(true);
                    $.when(userD).done(function (user) {
                        var options = {
                            ID: self.ID(),
                            objClassID: self.ClassID(),
                            fieldName: 'SoftwareLicence.Utilizer',
                            fieldFriendlyName: getTextResource('AssetNumber_UtilizerName'),
                            oldValue: self.UtilizerLoaded() ? { ID: self.Utilizer().ID(), ClassID: self.Utilizer().ClassID(), FullName: self.Utilizer().FullName() } : null,
                            object: ko.toJS(self.Utilizer()),
                            searcherName: 'UtilizerSearcher',
                            searcherPlaceholder: getTextResource('EnterFIO'),
                            searcherParams: [user.UserID],
                            onSave: function (objectInfo) {
                                self.UtilizerLoaded(false);
                                self.Utilizer(new userLib.EmptyUser(self, userLib.UserTypes.utilizer, self.EditUtilizer, false, false));
                                //
                                self.UtilizerID(objectInfo ? objectInfo.ID : '');
                                self.UtilizerClassID(objectInfo ? objectInfo.ClassID : '');
                                self.InitializeUtilizer();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                    });
                });
            };
            //
            self.InitializeUtilizer = function () {
                require(['models/SDForms/SDForm.User'], function (userLib) {
                    if (self.UtilizerLoaded() == false && self.UtilizerID()) {
                        var type = null;
                        if (self.UtilizerClassID() == 9) {//IMSystem.Global.OBJ_USER
                            type = userLib.UserTypes.utilizer;
                        }
                        else if (self.UtilizerClassID() == 722) {//IMSystem.Global.OBJ_QUEUE
                            type = userLib.UserTypes.queueExecutor;
                        }
                        else if (self.UtilizerClassID() == 101) {//IMSystem.Global.OBJ_ORGANIZATION
                            type = userLib.UserTypes.organization;
                        }
                        else if (self.UtilizerClassID() == 102) {//IMSystem.Global.OBJ_DIVISION
                            type = userLib.UserTypes.subdivision;
                        }

                        var options = {
                            UserID: self.UtilizerID(),
                            UserType: type,
                            UserName: null,
                            EditAction: self.EditUtilizer,
                            RemoveAction: null,
                            ShowTypeName: false
                        };
                        var user = new userLib.User(self, options);
                        self.Utilizer(user);
                        self.UtilizerLoaded(true);
                    }
                });
            };

            //Добавление новой лицензии
            self.Add = function (openSoftwareLicenceForm) {
                var retval = $.Deferred();
                //             
                var data = {
                    'Licence':{
                        'ID': self.ID(),
                        'Name': self.Name(),
                        'Note': self.Note(),
                        'Count': self.Count(),
                        'BeginDateJS': dtLib.Date2String(self.BeginDateDT()),
                        'EndDateJS': dtLib.Date2String(self.EndDateDT()),
                        'HASPAdapterID': self.HASPAdapterID(),
                        'RoomIntID': self.RoomIntID(),
                        'InventoryNumber': self.InventoryNumber(),
                        'SoftwareLicenceModelID': self.SoftwareLicenceModelID(),
                        'ProductCatalogTypeID': self.ProductCatalogTypeID(),
                        'SoftwareModelID': self.SoftwareModelID(),
                        'Type': self.LicenceTypeID(),
                        'SoftwareLicenceSchemeId': self.SoftwareLicenceSchemeID(),
                        'SoftwareLicenceSchemeName': self.SoftwareLicenceSchemeName(),
                        'LimitInHours': self.LimitInDays() * 24,
                        'DowngradeAvailable': self.DowngradeAvailable(),
                        'IsFull': self.IsFull(),
                        'SoftwareUsageNoLimits': self.SoftwareUsageNoLimits(),
                        'Files': self.files(),
                        'OEMDeviceClassID': self.OEMDeviceClassID,
                        'OEMDeviceID': self.OEMDeviceID,
                        'RestrictionsCoreFrom': self.RestrictionsCoreFrom(),
                        'RestrictionsCoreTill': self.RestrictionsCoreTill(),
                        'RestrictionsCoreUnlimit': self.RestrictionsCoreUnlimit(),
                        'RestrictionsCPUFrom': self.RestrictionsCPUFrom(),
                        'RestrictionsCPUTill': self.RestrictionsCPUTill(),
                        'RestrictionsCPUUnlimit': self.RestrictionsCPUUnlimit(),
                        'RestrictionsHzFrom': self.RestrictionsHzFrom(),
                        'RestrictionsHzTill': self.RestrictionsHzTill(),
                        'RestrictionsHzUnlimit': self.RestrictionsHzUnlimit(),
                        'SoftwareDistributionCentreID': self.targetSoftwareDistributionCentre() == null ? null : self.targetSoftwareDistributionCentre().ID
                    },
                    'LocationRestrictions': self.RestrictionsLocations()
                };

                //
                showSpinner();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/AddSoftwareLicence',
                        method: 'POST',
                        dataType: 'json',
                        data: data
                    },
                    function (response) {//AddSoftwareLicenceOutModel
                        hideSpinner();
                        if (response) {
                            if (response.Message && response.Message.length > 0 && (response.Type != 0))
                                require(['sweetAlert'], function () {
                                    swal(response.Message);//some problem
                                });

                            if (openSoftwareLicenceForm) {
                                require(['assetForms'], function (module) {

                                    var fh = new module.formHelper(true);
                                    fh.ShowSoftwareLicenceForm(response.SoftwareLicenceID);
                                });
                            }

                            if (response.Type == 0) {//ok 
                                retval.resolve(response);
                                return;
                            }
                        }
                        retval.resolve(null);
                    },
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[SoftwareLicence.js, add]', 'error');
                        });
                        retval.resolve(null);
                    });
                //
                return retval.promise();
            };

            //  права пользователя
            self.grantedOperations = [];
            $.when(userD).done(function (user) {
                self.grantedOperations = user.GrantedOperations;
            });
            self.operationIsGranted = function (operationID) {
                for (var i = 0; i < self.grantedOperations.length; i++)
                    if (self.grantedOperations[i] === operationID)
                        return true;
                return false;
            };

        }
    };
    return module;
});
