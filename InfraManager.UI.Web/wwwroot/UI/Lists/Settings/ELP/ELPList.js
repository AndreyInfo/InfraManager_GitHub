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
                self.baseUrl = "/api/elpsettings";               
                                
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
                        self.filterELPListName = ko.observable('');                      

                        // обратка нажатия кнопок в строке поиска
                        self.filterELPListNameKeyPressed = function (data, event) {
                            if (event.keyCode == 13)
                                self.onSearch();
                            else
                                return true;
                        };

                        // нажатие на кнопку поиска
                        self.onSearch = function () {
                            self.listView.load();
                        };

                    }

                    self.listView = null;

                    // привязать тот же ListView к другому экземпляру шаблона (при изменении вкладки), без перезагрузки списка
                    self.listViewID  = 'listView_' + ko.getNewID();

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
                        if (obj && obj.ID && (self.UserIsAdmin || self.operationIsGranted(self.OPERATION_ELP_Create))) {
                            self.editItem(obj.ID);
                        }
                    };
                }

                //
                {//identification      
                    self.getObjectID = function (obj) {
                        return obj.ID.toUpperCase();
                    };

                    self.isObjectClassVisible = function (objectClassID) {
                        return objectClassID == 820;
                    };
                }

                //
                self.editItem = function (id) {
                    showSpinner();

                    //вызов формы связи
                    require(['ui_forms/Settings/ELP/frmELPTask'], function (jsModule) {
                        jsModule.ShowDialog(id, false, true);
                    });

                };

                //
                {   // контекстное меню

                    {   // права пользователя

                        self.grantedOperations = [];

                        // права позволяющие создание схемы лицензирования
                        self.OPERATION_ELP_Create = 820002;

                        // права позволяющие просматривать карточку схемы лицензирования
                        self.OPERATION_ELP_Properties = 820001;

                        // права позволяющие возможность пометить схему лицензирования как удаленную
                        self.OPERATION_ELP_Delete = 820004;

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
                        self.createMenuItem(contextMenu, "ELPListMenuActionCreate");
                        // Свойства
                        self.propertiesMenuItem(contextMenu, "ELPListMenuActionProperties");
                        // Удалить (пометить как удалённая)
                        self.DeletedMenuItem(contextMenu, "ELPListMenuActionMarkAsDeleted");
                        // Снять пометку удаления
                        //self.unmarkAsDeletedMenuItem(contextMenu, "LicenceSchemesMenuActionUnmarkAsDeleted");
                    };

                    // создать
                    self.createMenuItem = function (contextMenu, restext) {
                        var isEnable = function () {
                            return true;
                        };
                        var isVisible = function () {
                            return (self.UserIsAdmin || self.operationIsGranted(self.OPERATION_ELP_Create)) ? true : false;
                        };
                        var action = function () {
                            showSpinner();
                            //вызов формы добавления лицензии ПО
                            require(['ui_forms/Settings/ELP/frmELPTask'], function (jsModule) {
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

                            if (!(self.UserIsAdmin || self.operationIsGranted(self.OPERATION_ELP_Properties)))
                                return false;

                            return result;
                        };

                        var action = function () {

                            let selectedItems = self.getSelectedItems();

                            if (selectedItems.length > 0) {
                                self.editItem(selectedItems[0].ID);
                            }
                        };
                        //
                        var cmd = contextMenu.addContextMenuItem();
                        cmd.restext(restext);
                        cmd.isEnable = isEnable;
                        cmd.isVisible = isVisible;
                        cmd.click(action);
                    };

                    // удалить
                    self.DeletedMenuItem = function (contextMenu, restext) {
                        var isEnable = function () {
                            return true;
                        };

                        var isVisible = function () {

                            let result = true;

                            let selectedItems = self.getSelectedItems();

                            // кол-во выбранных == 1
                            if (selectedItems.length != 1)
                                return false;

                            // админ или соответствующая роль
                            if (!(self.UserIsAdmin || self.operationIsGranted(self.OPERATION_ELP_Delete)))
                                return false;

                            return result;                           
                        };
                        var action = function () {                                                   

                            require(['sweetAlert'], function (swal) {
                                swal({
                                    title: getTextResource('ELPListsAlertTitleMarkAsDeleted'),
                                    text: getTextResource('ELPListAlertQuestionMarkAsDeleted'),
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
                                            self.ajaxControl.Ajax(null,
                                                {
                                                    url: self.baseUrl+'/'+self.getSelectedItems()[0].ID,
                                                    method: 'DELETE',
                                                    dataType: 'json',
                                                    headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                                                },
                                                null,
                                                function (){},
                                                function () {
                                                    self.listView.load();  
                                                });
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

                    
                    self.getIdObjectList = function (items) {
                        var retval = [];
                        items.forEach(function (item) {
                            retval.push(
                                self.getObjectID(item)
                            );
                        });
                        return retval;
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
                        var ELPListFilter = {                            
                            SearchString: self.filterELPListName()
                        };

                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                data: ELPListFilter,
                                url: self.baseUrl,
                                headers: { 'x-device-fingerprint': self.fingerprintJs.fHash }
                            },
                            function (list) {
                                retvalD.resolve(list);
                            },
                            function (XMLHttpRequest, textStatus, errorThrown) {
                                if (showErrors === true)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[Lists/Settings/ELP/ELPList.js getObjectList]', 'error');
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