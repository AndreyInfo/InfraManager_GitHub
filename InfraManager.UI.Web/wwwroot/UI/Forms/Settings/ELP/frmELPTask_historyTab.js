define(['knockout', 'jquery', 'ajax'], function (ko, $, ajax) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('ELPTask_HistoryTab');
            self.Template = '../UI/Forms/Settings/ELP/frmELPTask_historyTab';
            self.IconCSS = 'historyTab';
            //
            self.IsVisible = ko.observable(true);
            //
            {//fields
                self.historyList = ko.observableArray([]);
                self.isLoaded = false;
            }
            //
            //when object changed
            self.Initialize = function (obj) {
                self.isLoaded = false;
                self.historyList([]);
            };
            //when tab selected
            self.load = function () {               

                if (self.isLoaded === true)
                    return;
                //
                self.isLoaded = true;
                require(['models/SDForms/SDForm.TapeRecord'], function (tapeLib) {
                    var element = $('#' + vm.frm.GetRegionID()).find('.historyList');
                    self.ajaxControl.Ajax(element,
                        {
                            method: 'GET',
                            url: `/api/${vm.object().ClassID}/${vm.object().ID()}/events`
                        },
                        function (list) {
                            if (list) {
                                self.historyList.removeAll();

                                var options = {
                                    entityID: vm.object().ID(),
                                    entityClassID: vm.object().ClassID,
                                    type: 'history'
                                };
                                ko.utils.arrayForEach(list, function (item) {
                                    self.historyList.push(new tapeLib.TapeRecord(item, options));
                                });
                                self.sortTapeRecord(self.historyList);
                                self.historyList.valueHasMutated();
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmELPTask_historyTab.js, load]', 'error');
                                });
                            }
                        });
                });
            };
            //when tab unload
            self.dispose = function () {
                self.isLoaded = false;
                self.historyList.removeAll();
                self.ajaxControl.Abort();
            };
            //
            self.sortTapeRecord = function (list_obj) {
                if (!list_obj)
                    return;
                //
                list_obj.sort(
                    function (left, right) {
                        if (left.DateObj() == null)
                            return -1;
                        //
                        if (right.DateObj() == null)
                            return 1;
                        //
                        return left.DateObj() == right.DateObj() ? 0 : (left.DateObj() < right.DateObj() ? -1 : 1);
                    }
                );
            };
        }
    };
    return module;
});