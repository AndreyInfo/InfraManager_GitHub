define(['knockout', 'jquery', 'ajax'], function (ko, $, ajax) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('Contract_HistoryTab');
            self.Template = '../UI/Forms/Asset/Contracts/frmContract_historyTab';
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
            self.init = function (obj) {
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
                            dataType: "json",
                            method: 'GET',
                            data: {
                                'ID': vm.object().ID(),
                                'EntityClassId': vm.object().ClassID,
                            },
                            url: '/sdApi/GetAssetHistory'
                        },
                        function (newVal) {
                            if (newVal && newVal.Result === 0) {
                                var list = newVal.List;
                                if (list) {
                                    self.historyList.removeAll();
                                    //

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
                                        swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContract_historyTab.js, load]', 'error');
                                    });
                                }
                            }
                            else if (newVal && newVal.Result === 1)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmContract_historyTab.js, load]', 'error');
                                });
                            else if (newVal && newVal.Result === 2)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmContract_historyTab.js, load]', 'error');
                                });
                            else if (newVal && newVal.Result === 3)
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                                });
                            else
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmContract_historyTab.js, load]', 'error');
                                });
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