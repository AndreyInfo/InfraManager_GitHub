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
                    self.killSession(contextMenu);
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
                    return item.UserFullName;
                };
                //
                self.getSelectedItems = function () {
                    var selectedItems = adminToolsVM.sessionTable.listView.rowViewModel.checkedItems();
                    return selectedItems;
                };
                self.clearSelection = function () {
                    adminToolsVM.sessionTable.listView.rowViewModel.checkedItems([]);
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
                        retval.push({
                            UserID: item.UserID,
                            UserAgent: item.UserAgent
                        });
                    });
                    return retval;
                };
            }
            //
            {//menu operations
                self.killSession = function (contextMenu) {
                    var isEnable = function () {
                        var retval = true;
                        var selItems = self.getSelectedItems();
                        if (selItems.length == 0)
                            return false;
                        for (var i = 0; i < selItems.length; i++)
                            if (selItems[i].UtcDateClosed != null)
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
                                title: getTextResource('AdminTools_KillSession') + ': ' + question,
                                text: getTextResource('AdminTools_KillSession_ConfirmQuestion'),
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
                                                data: { List: infos },
                                                url: '/accountApi/KillUserSession'
                                            },
                                            function (response) {
                                                hideSpinner();
                                                if (response) {
                                                    if (response == 0) {//ok                                                                                                   
                                                    }
                                                    else if (response.Result === 7) {
                                                        require(['sweetAlert'], function () {
                                                            swal(getTextResource('ErrorCaption'), getTextResource('OperationError'), 'error');
                                                        });
                                                    }
                                                }
                                            },
                                            function (response) {
                                                hideSpinner();
                                                require(['sweetAlert'], function () {
                                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[AdminTools.SessionTable.ContextMenu.js, killSession]', 'error');
                                                });
                                            });
                                    }
                                });
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('AdminTools_KillSession');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                };
            }
        }
    }
    return module;
});
