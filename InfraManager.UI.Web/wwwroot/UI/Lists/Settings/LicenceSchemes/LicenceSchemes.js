define(['knockout', 'jquery', 'ajax', 'dateTimeControl',
    'ui_controls/ListView/ko.ListView.Cells', 'ui_controls/ListView/ko.ListView.Helpers', 'ui_controls/ListView/ko.ListView.LazyEvents',
    'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu', 'fingerPrintLib'],
    function (ko, $, ajaxLib, dtLib, m_cells, m_helpers, m_lazyEvents, lv, cm, fingerPrint) {
        var module = {
            ViewModel: function (classID, id, selectDivision) {
                var self = this;
                //  Инитим отпечаток
                self.fingerprintJs = fingerPrint;
                self.fingerprintJs.init();

                self.ajaxControl = new ajaxLib.control();

                // базовый URL
                self.baseUrl = "/licence-scheme";

                // когда объект изменился
                self.init = function (obj) {
                    console.log('init');
                };

                // при выборе вкладки
                self.load = function () {
                    console.log('load');
                };

                // когда вкладка выгружается
                self.dispose = function () {

                    console.log('dispose');

                    self.ajaxControl.Abort();
                };


                {  // основная модель

                    {   // панель фильтра

                        // поле для поиска
                        self.filterLicenceSchemeName = ko.observable('');

                        // обратка нажатия кнопок в строке поиска
                        self.filterLicenceSchemeNameKeyPressed = function (data, event) {
                            if (event.keyCode == 13)
                                self.onSearch();
                            else
                                return true;
                        };

                        // нажатие на кнопку поиска
                        self.onSearch = function () {
                            self.listView.load();
                        };

                        // показывать удаленные элементы
                        self.filterIsShowDeletedRecords = ko.observable(false);

                        self.filterIsShowDeletedRecordssubscribe = self.filterIsShowDeletedRecords.subscribe(function () {
                            self.listView.load();
                        });
                    }

                    self.listView = null;

                    // привязать тот же ListView к другому экземпляру шаблона (при изменении вкладки), без перезагрузки списка
                    self.listViewID = 'listView_' + ko.getNewID();

                    ////
                    self.listViewInit = function (listView) {

                        if (self.listView != null)
                            throw 'listView inited already';
                        //
                        self.listView = listView;

                        m_helpers.init(self, listView);//extend self        

                        //
                        self.listView.load();
                    };

                    self.listViewRetrieveItems = function () {

                        var retvalD = $.Deferred();

                        $.when(self.getObjectList(null, true)).done(function (objectList) {
                            if (objectList)
                                self.clearAllInfos();
                            //
                            retvalD.resolve(objectList);

                        });

                        return retvalD.promise();
                    };

                    self.listViewRowClick = function (obj) {
                        if (!(self.UserIsAdmin || self.operationIsGranted(self.OPERATION_LicenceScheme_Properties)))
                            return;
                        if (!obj)
                            return;

                        showSpinner();

                        //вызов формы добавления лицензии ПО
                        require(['ui_forms/Settings/LicenceSchemes/frmLicenceScheme'], function (jsModule) {
                            jsModule.ShowDialog(obj.ID, false, true);
                        });

                    };
                }

                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };

                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 750;
                    };
                }

                //
                {   // контекстное меню

                    {   // права пользователя

                        self.grantedOperations = [];

                        // права позволяющие создание схемы лицензирования
                        self.OPERATION_LicenceScheme_Create = 750002;

                        // права позволяющие просматривать карточку схемы лицензирования
                        self.OPERATION_LicenceScheme_Properties = 750001;

                        // права позволяющие возможность пометить схему лицензирования как удаленную
                        self.OPERATION_LicenceScheme_Delete = 750004;

                        //
                        self.operationIsGranted = function (operationID) {
                            for (var i = 0; i < self.grantedOperations.length; i++)
                                if (self.grantedOperations[i] === operationID)
                                    return true;
                            return false;
                        };
                        //
                        self.UserIsAdmin = false;

                        $.when(userD).done(function (user) {
                            self.UserIsAdmin = user.HasAdminRole;
                            self.grantedOperations = user.GrantedOperations;
                        });
                    }

                    //
                    self.getSelectedItems = function () {
                        var selectedItems = self.listView.rowViewModel.checkedItems();
                        return selectedItems;
                    };

                    //
                    self.contextMenu = ko.observable(null);

                    self.contextMenuInit = function (contextMenu) {

                        self.contextMenu(contextMenu);//bind contextMenu
                        //
                        contextMenu.clearItems();
                        // Создать
                        self.createMenuItem(contextMenu, "LicenceSchemesMenuActionCreate");
                        // Создать по аналогии
                        self.createByAnalogyMenuItem(contextMenu, "LicenceSchemesMenuActionCreateByAnalogy");
                        // Свойства
                        self.propertiesMenuItem(contextMenu, "LicenceSchemesMenuActionProperties");
                        // Удалить (пометить как удалённая)
                        self.markAsDeletedMenuItem(contextMenu, "LicenceSchemesMenuActionMarkAsDeleted");
                        // Снять пометку удаления
                        self.unmarkAsDeletedMenuItem(contextMenu, "LicenceSchemesMenuActionUnmarkAsDeleted");
                    };

                    // создать
                    self.createMenuItem = function (contextMenu, restext) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return (self.UserIsAdmin || self.operationIsGranted(self.OPERATION_LicenceScheme_Create)) ? true : false;
                        };
                        var action = function () {
                            showSpinner();
                            //вызов формы добавления лицензии ПО
                            require(['ui_forms/Settings/LicenceSchemes/frmLicenceScheme'], function (jsModule) {
                                jsModule.ShowDialog(null, true, true);
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext(restext);
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    // создать по аналогии
                    self.createByAnalogyMenuItem = function (contextMenu, restext) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            var result = true;

                            let selectedItems = self.getSelectedItems();

                            // кол-во выбранных = 1
                            if (selectedItems.length != 1)
                                return false;

                            if (!(self.UserIsAdmin || self.operationIsGranted(self.OPERATION_LicenceScheme_Create)))
                                return false;

                            // если в выбраных встречаются не только пользовательские схемы (0 - ползовательская схема)
                            if (selectedItems.some((item) => item.SchemeType != 0))
                                return false;

                            return result;

                        };
                        var action = function () {

                            let selectedItems = self.getSelectedItems();

                            showSpinner();

                            //вызов формы добавления лицензии ПО
                            require(['ui_forms/Settings/LicenceSchemes/frmLicenceScheme'], function (jsModule) {
                                jsModule.ShowDialog(selectedItems[0].ID, true, true);
                            });

                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext(restext);
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    // Свойства
                    self.propertiesMenuItem = function (contextMenu, restext) {
                        var isEnable = function () {
                            return true;
                        };

                        var isVisible = function () {

                            let result = true;

                            let selectedItems = self.getSelectedItems();

                            // кол-во выбранных = 1
                            if (selectedItems.length != 1)
                                return false;

                            if (!(self.UserIsAdmin || self.operationIsGranted(self.OPERATION_LicenceScheme_Properties)))
                                return false;

                            return result;
                        };

                        var action = function () {

                            let selectedItems = self.getSelectedItems();

                            showSpinner();

                            //вызов формы добавления лицензии ПО
                            require(['ui_forms/Settings/LicenceSchemes/frmLicenceScheme'], function (jsModule) {
                                jsModule.ShowDialog(selectedItems[0].ID, false, true);
                            });

                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext(restext);
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };


                    self.getIdObjectList = function (items) {
                        var retval = [];
                        items.forEach(function (item) {
                            retval.push(
                                self.getObjectID(item)
                            );
                        });
                        return retval;
                    };

                    // удалить (пометить как удалённая)
                    self.markAsDeletedMenuItem = function (contextMenu, restext) {
                        var isEnable = function () {
                            return true;
                        };

                        var isVisible = function () {

                            let result = true;

                            let selectedItems = self.getSelectedItems();

                            // кол-во выбранных > 0 
                            if (selectedItems.length == 0)
                                return false;

                            // админ или соответствующая роль
                            if (!(self.UserIsAdmin || self.operationIsGranted(self.OPERATION_LicenceScheme_Delete)))
                                return false;

                            // если в выбраных встречаются не только пользовательские схемы (0 - ползовательская схема)
                            if (selectedItems.some((item) => item.SchemeType != 0))
                                return false;

                            // если в выбранных элементах есть удаленные схемы
                            // TODO: возможно стоить удалить данное условие
                            if (selectedItems.some((item) => item.IsDeleted))
                                result = false;

                            return result;
                        };
                        var action = function () {

                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('LicenceSchemesAlertTitleMarkAsDeleted'),
                                    text: getTextResource('LicenceSchemesAlertQuestionMarkAsDeleted'),
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
                                            var data = {
                                                'Guids': self.getIdObjectList(self.getSelectedItems())
                                            };
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    dataType: "json",
                                                    method: 'POST',
                                                    contentType: 'application/json',
                                                    data: JSON.stringify(data),
                                                    url: self.baseUrl + '/mark-as-deleted',
                                                    headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                                                },
                                                function (responce) {
                                                    if (responce.Success) {
                                                        self.listView.load();
                                                    }
                                                    else {
                                                        if (showErrors === true) {
                                                            require(['sweetAlert'], function () {
                                                                swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError') + '\n[Lists/Settings/LicenceSchemes/LicenceSchemes.js GroupDeleteRequest]', 'error');
                                                            });
                                                        }
                                                    }
                                                },
                                                function (XMLHttpRequest, textStatus, errorThrown) {
                                                    if (showErrors === true)
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Settings/LicenceSchemes/LicenceSchemes.js GroupDeleteRequest]', 'error');
                                                        });
                                                    //
                                                    retvalD.resolve([]);
                                                },
                                                null);
                                        }
                                    });
                            });

                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext(restext);
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    // Снять пометку удаления
                    self.unmarkAsDeletedMenuItem = function (contextMenu, restext) {
                        var isEnable = function () {
                            return true;
                        };

                        var isVisible = function () {
                            let result = true;

                            let selectedItems = self.getSelectedItems();

                            // кол-во выбранных > 0 
                            if (selectedItems.length == 0)
                                return false;

                            // админ или соответствующая роль
                            if (!(self.UserIsAdmin || self.operationIsGranted(self.OPERATION_LicenceScheme_Delete)))
                                return false;

                            // если в выбраных встречаются не только пользовательские схемы (0 - ползовательская схема)
                            if (selectedItems.some((item) => item.SchemeType != 0))
                                return false;

                            // если в выбанном есть, не удаленные схемы 
                            // TODO: может вообще убрать данное условие
                            if (selectedItems.some((item) => !item.IsDeleted))
                                return false;

                            return result;
                        };
                        var action = function () {

                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('LicenceSchemesAlertTitleUnmarkAsDeleted'),
                                    text: getTextResource('LicenceSchemesAlertQuestionUnmarkAsDeleted'),
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
                                            var data = {
                                                'Guids': self.getIdObjectList(self.getSelectedItems())
                                            };
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    dataType: 'json',
                                                    method: 'POST',
                                                    contentType: 'application/json',
                                                    data: JSON.stringify(data),
                                                    url: self.baseUrl + '/unmark-as-deleted',
                                                    headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                                                },
                                                function (responce) {
                                                    if (responce.Success) {
                                                        self.listView.load();
                                                    }
                                                    else {
                                                        if (showErrors === true) {
                                                            require(['sweetAlert'], function () {
                                                                swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError') + '\n[Lists/Settings/LicenceSchemes/LicenceSchemes.js UndoGroupDeleteRequest]', 'error');
                                                            });
                                                        }
                                                    }
                                                },
                                                function (XMLHttpRequest, textStatus, errorThrown) {
                                                    if (showErrors === true)
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Settings/LicenceSchemes/LicenceSchemes.js UndoGroupDeleteRequest]', 'error');
                                                        });
                                                    //
                                                    retvalD.resolve([]);
                                                },
                                                null);
                                        }
                                    });
                            });
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext(restext);
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    //
                    self.contextMenuOpening = function (contextMenu) {
                        contextMenu.items().forEach(function (item) {
                            item.enabled(item.isEnable());
                            item.visible(item.isVisible());
                        });
                    };
                }

                {//geting data             

                    self.loadObjectListByIDs = function (idArray, unshiftMode) {
                        for (var i = 0; i < idArray.length; i++)
                            idArray[i] = idArray[i].toUpperCase();
                        //
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(idArray, false)).done(function (objectList) {
                                if (objectList) {
                                    var rows = self.appendObjectList(objectList, unshiftMode);
                                    rows.forEach(function (row) {
                                        self.setRowAsNewer(row);
                                        //
                                        var obj = row.object;
                                        var id = self.getObjectID(obj);
                                        self.clearInfoByObject(id);
                                        //
                                        var index = idArray.indexOf(id);
                                        if (index != -1)
                                            idArray.splice(index, 1);
                                    });
                                }
                                idArray.forEach(function (id) {
                                    self.removeRowByID(id);
                                    self.clearInfoByObject(id);
                                });
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };

                    self.getObjectListByIDs = function (idArray, unshift) {
                        var retvalD = $.Deferred();
                        if (idArray.length > 0) {
                            $.when(self.getObjectList(idArray, false)).done(function (objectList) {
                                retvalD.resolve(objectList);
                            });
                        }
                        else
                            retvalD.resolve([]);
                        return retvalD.promise();
                    };

                    //
                    self.ajaxControl = new ajaxLib.control();
                    self.isAjaxActive = function () {
                        return self.ajaxControl.IsAcitve() == true;
                    };

                    //
                    self.getObjectList = function (idArray, showErrors) {
                        var retvalD = $.Deferred();
                        //
                        var softwareLicenceSchemeListFilter = {
                            SearchText: self.filterLicenceSchemeName(),
                            ShowDeleted: self.filterIsShowDeletedRecords()
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                data: softwareLicenceSchemeListFilter,
                                url: self.baseUrl,
                                headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                            },
                            function (response) {

                                if (response && response.Success) {
                                    response.Result.forEach(item => {
                                        if (item.UpdatedDate)
                                            item.UpdatedDate = new Date(item.UpdatedDate + 'Z').toLocaleString();
                                        if (item.CreatedDate)
                                            item.CreatedDate = new Date(item.CreatedDate + 'Z').toLocaleString();
                                        
                                    });
                                    retvalD.resolve(response.Result);//can be null, if server canceled request, because it has a new request                               
                                }
                                else {
                                    if (showErrors === true) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('ErrorCaption'), getTextResource('FiltrationError') + '\n[Lists/Settings/LicenceSchemes/LicenceSchemes.js getObjectList]', 'error');
                                        });
                                    }
                                    retvalD.resolve([]);
                                }
                            },
                            function (XMLHttpRequest, textStatus, errorThrown) {
                                if (showErrors === true)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Settings/LicenceSchemes/LicenceSchemes.js getObjectList]', 'error');
                                    });
                                //
                                retvalD.resolve([]);
                            },
                            null
                        );
                        //
                        return retvalD.promise();
                    };
                }

                {//server and local(only this browser tab) events                               
                    self.onObjectInserted = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        self.listView.load();
                    };
                    //
                    self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке измененный объект присутствовать не может
                        self.listView.load();

                    };
                    //
                    self.onObjectDeleted = function (e, objectClassID, objectID, parentObjectID) {
                        if (!self.isObjectClassVisible(objectClassID))
                            return;//в текущем списке удаляемый объект присутствовать не может

                    };
                    //
                    $(document).bind('objectInserted', self.onObjectInserted);
                    $(document).bind('local_objectInserted', self.onObjectInserted);
                    $(document).bind('objectUpdated', self.onObjectModified);
                    $(document).bind('local_objectUpdated', self.onObjectModified);
                    $(document).bind('objectDeleted', self.onObjectDeleted);
                    $(document).bind('local_objectDeleted', self.onObjectDeleted);
                }

                m_lazyEvents.init(self);//extend self

                ////Переопределяем функцию, т.к. в этом списке нет информации о новых объектах
                //self.addToModifiedObjectIDs = function (objectID) {
                //    self.reloadObjectByID(objectID);
                //};
            }
        };
        return module;
    });