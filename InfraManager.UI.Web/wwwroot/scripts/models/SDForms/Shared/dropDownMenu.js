define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    const module = {
        ViewModel: function ($regionParent, isReadOnly, nameTemplate, refreshItemMenu) {
            const self = this;

            self.LoadD = $.Deferred();

            self.divID = 'dropDownMenu_' + ko.getNewID();

            self.$region = $regionParent;

            self.ReadOnly = ko.observable(isReadOnly);

            self.IsLoaded = ko.observable(false);

            self.RefreshItemMenu = refreshItemMenu;

            self.Initialize = function () {
                self.$region.find('[drop-down-handler]').click(self.HeaderClick);
                self.$region.find('[drop-down-list]').append(`<div id="${self.divID}" data-bind="template: {name: ${nameTemplate}, afterRender: AfterRender}" ></div>`);

                try {
                    ko.applyBindings(self, document.getElementById(self.divID));
                }
                catch (err) {
                    if (document.getElementById(self.divID))
                        throw err;
                }
            };

            self.AfterRender = function () {
                self.LoadD.resolve();
            };
            
            self.BasicItemID = ko.observable(null);

            self.MenuItemsList = ko.observableArray([]);

            self.HeaderClick = function (e) {
                if (!self.ReadOnly()) {
                    openRegion(self.$region.find('[drop-down-list]'), e);
                }
                return true;
            };

            self.ClosePanel = function () {
                self.$region.find('[drop-down-list]').hide();
            };
            
            self.ClassID = null;
            self.ObjectID = null;
            self.ajaxControl_load = new ajaxLib.control();

            self.Load = function (objID, classID, basicItemID, url) {
                self.BasicItemID(basicItemID);
                
                self.ObjectID = objID;
                self.ClassID = classID;
                
                const loadD = $.Deferred();
                
                $.when(self.LoadD).done(function () {
                    self.ajaxControl_load.Ajax(null,
                        {
                            url: url,
                            method: 'GET',
                            dataType: "json"
                        },
                        function (response) {
                            self.MenuItemsList.removeAll();

                            if (!response) {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[dropDownMenu.js, Load]', 'error');
                                    loadD.resolve(false);
                                });

                                return;
                            };

                            ko.utils.arrayForEach(response, function (el) {
                                if (!el) {
                                    return;
                                };

                                self.MenuItemsList.push(new module.Item(el));
                            });

                            self.MenuItemsList.valueHasMutated();
                            self.IsLoaded(true);
                            loadD.resolve(true);
                        });
                });

                return loadD.promise();
            };
            
            self.CheckCurrentItem = function (item) {
                if (item == null) {
                    return false;
                };

                return item.ID == self.BasicItemID();
            };
            
            self.ajaxControl_edit = new ajaxLib.control();

            self.SelectItem = function (item) {
                if (self.ReadOnly() || item == null) {
                    return;
                };

                if (item.ID == self.BasicItemID()) {
                    return;
                };

                if (self.ObjectID == null) {
                    self.BasicItemID(item.ID);
                    self.ClosePanel();

                    self.RefreshItemMenu(item);
                    return;
                }

                $.when(userD).done(function (user) {
                    self.BasicItemID(item.ID);
                    self.ClosePanel();

                    self.RefreshItemMenu(item);
                });
            };
        },
        Item: function (obj) {
            const pself = this;

            pself.ID = obj.ID;
            pself.Name = obj.Name;
            pself.Sequence = obj.Sequence;
            pself.Color = obj.Color;
            pself.Default = obj.Default;
        }
    };
    return module;
});