define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        ViewModel: function (adminToolsVM, ko_cms) {
            var self = this;
            //
            self.ajaxControl = new ajaxLib.control();
            //
            {//ko.contextMenu
                self.cm_Init = function (contextMenu) {
                    ko_cms(contextMenu);//bind contextMenu
                    //                   
                    self.addPersonalLicence(contextMenu);
                    self.deletePersonalLicence(contextMenu);
                };
                self.cm_Opening = function (contextMenu) {
                    contextMenu.items().forEach(function (item) {
                        if (item.isEnable && item.isVisible) {
                            item.enabled(item.isEnable());
                            item.visible(item.isVisible());
                        }
                    });
                };
            }
            //
            {//helper methods                               
                self.getItemName = function (item) {
                    return item.FullName;
                };
                //
                self.getSelectedItems = function () {
                    var selectedItems = adminToolsVM.personalLicenceTable.listView.rowViewModel.checkedItems();
                    return selectedItems;
                };
                self.getConcatedItemNames = function (items) {
                    var retval = '';
                    items.forEach(function (item) {
                        if (retval.length < 200) {
                            retval += (retval.length > 0 ? ', ' : '') + self.getItemName(item);
                            if (retval.length >= 200)
                                retval += '...';
                        }
                    });
                    return retval;
                };
                self.getItemInfos = function (items) {
                    var retval = [];
                    items.forEach(function (item) {
                        retval.push(item.UserID);
                    });
                    return retval;
                };
            }
            //
            {//menu operations
                self.addPersonalLicence = function (contextMenu) {
                    var isEnable = function () {
                        return true;
                    };
                    var isVisible = function () {
                        return true;
                    };
                    var action = function () {
                        showSpinner();
                        require(['usualForms', 'models/SDForms/SDForm.User'], function (module, userLib) {
                            var fh = new module.formHelper(true);
                            $.when(userD).done(function (user) {
                                var options = {                                    
                                    fieldFriendlyName: getTextResource('User'),
                                    oldValue: null,
                                    searcherName: 'UserSearcherLoginName',
                                    searcherPlaceholder: getTextResource('EnterFIO'),
                                    searcherParams: [],
                                    nosave: true,
                                    onSave: function (objectInfo) {
                                        var row = adminToolsVM.personalLicenceTable.getRowByID(objectInfo.ID);
                                        if (row != null)
                                            return;
                                        //
                                        showSpinner();
                                        self.ajaxControl.Ajax(null,
                                            {
                                                dataType: "json",
                                                method: 'POST',
                                                url: '/accountApi/insertPersonalLicence?UserID=' + objectInfo.ID
                                            },
                                            function (response) {
                                                hideSpinner();
                                                if (response) {
                                                    if (response.Result == 0 && response.Data) {//ok 
                                                    }
                                                    else if (response.Result === 7) {
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('ErrorCaption'), getTextResource('AdminTools_PersonalLicence_CantAdd'), 'error');
                                                        });
                                                    }
                                                    else if (response.Result === 3) {
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                                        });
                                                    }
                                                }
                                            },
                                            function (response) {
                                                hideSpinner();
                                                require(['sweetAlert'], function () {
                                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[AdminTools.PersonalLicence.ContextMenu.js, add]', 'error');
                                                });
                                            });                                        
                                    }
                                };
                                fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                            });
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('AdminTools_PersonalLicence_Add');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
                self.deletePersonalLicence = function (contextMenu) {
                    var isEnable = function () {
                        var selItems = self.getSelectedItems();
                        if (selItems.length == 0)
                            return false;
                        //
                        return true;
                    };
                    var isVisible = function () {
                        return true;
                    };
                    var action = function () {
                        var list = self.getSelectedItems();
                        if (list.length == 0)
                            return;
                        //     
                        var question = self.getConcatedItemNames(list);
                        require(['sweetAlert'], function (swal) {
                            swal({
                                title: getTextResource('AdminTools_DeletePersonalLicence') + ': ' + question,
                                text: getTextResource('AdminTools_DeletePersonalLicence_ConfirmQuestion'),
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
                                        var infos = self.getItemInfos(list);
                                        showSpinner();
                                        self.ajaxControl.Ajax(null,
                                            {
                                                dataType: "json",
                                                method: 'POST',
                                                data: { IDList: infos },
                                                url: '/accountApi/deletePersonalLicence'
                                            },
                                            function (response) {
                                                hideSpinner();
                                                if (response == 0) {//ok 
                                                }
                                                else if (response === 5) {
                                                    require(['sweetAlert'], function () {
                                                        swal(getTextResource('ErrorCaption'), getTextResource('ConcurrencyError'), 'error');
                                                    });
                                                }
                                                else if (response === 3) {
                                                    require(['sweetAlert'], function () {
                                                        swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                                    });
                                                }
                                            },
                                            function (response) {
                                                hideSpinner();
                                                require(['sweetAlert'], function () {
                                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[AdminTools.PersonalLicence.ContextMenu.js, delete]', 'error');
                                                });
                                            });
                                    }
                                });
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('AdminTools_PersonalLicence_Delete');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
            }
        }
    }
    return module;
});
