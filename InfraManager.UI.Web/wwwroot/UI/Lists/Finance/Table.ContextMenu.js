define(['knockout', 'jquery', 'ajax', 'sdTableContextMenuHelper'], function (ko, $, ajaxLib, sdTableContextMenuHelper) {
    var module = {
        ViewModel: function (tableViewModel) {
            var self = this;
            //
            self.ajaxControl = new ajaxLib.control();
            self.sdContextMenuHelper = new sdTableContextMenuHelper.helper(tableViewModel);
            //
            self.contextMenuInit = function (contextMenu) {
                self.sdContextMenuHelper.contextMenuInit(contextMenu);
            }
            self.contextMenuOpening = function (contextMenu) {
                self.sdContextMenuHelper.contextMenuOpening(contextMenu);
            };
        }
    }
    return module;
});
