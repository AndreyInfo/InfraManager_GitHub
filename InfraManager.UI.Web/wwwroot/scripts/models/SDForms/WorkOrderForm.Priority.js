define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        ViewModel: function ($regionPriority, parentSelf, isReadOnly) {
            var vself = this;
            var self = parentSelf;
            vself.LoadD = $.Deferred();
            //
            vself.divID = 'priorityControlWorkOrder_' + ko.getNewID();//main control div.ID
            vself.$region = $regionPriority;
            vself.ReadOnly = ko.observable(isReadOnly);
            vself.IsLoaded = ko.observable(false);
            vself.Initialize = function () {
                vself.$region.find('.priority-header').click(vself.HeaderClick);
                vself.$region.find('.priority-workorder-panel').append('<div id="' + vself.divID + '" data-bind="template: {name: \'Priority/WorkOrderPriority\', afterRender: AfterRender}" ></div>');
                //
                try {
                    ko.applyBindings(vself, document.getElementById(vself.divID));
                }
                catch (err) {
                    if (document.getElementById(vself.divID))
                        throw err;
                }
            };
            vself.AfterRender = function () {
                vself.LoadD.resolve();
            };
            //
            vself.BasicPriorityID = ko.observable(null);
            vself.PriorityList = ko.observableArray([]);
            vself.HeaderClick = function (e) {
                if (!vself.ReadOnly()) {
                    openRegion(vself.$region.find('.priority-workorder-panel'), e);
                }
                return true;
            };
            vself.ClosePanel = function () {
                vself.$region.find('.priority-workorder-panel').hide();
            };
            //
            vself.ClassID = null;
            vself.ObjectID = null;
            vself.ajaxControl_load = new ajaxLib.control();
            vself.Load = function (objID, classID, basicPriorityID) {
                vself.BasicPriorityID(basicPriorityID);
                //
                vself.ObjectID = objID;
                vself.ClassID = classID;
                //
                var loadD = $.Deferred();
                //
                $.when(userD, vself.LoadD).done(function (user) {
                    var param = {
                        'ID': vself.ObjectID,
                        'EntityClassId': vself.ClassID,
                    };
                    vself.ajaxControl_load.Ajax(vself.$region,
                        {
                            url: '/api/WorkOrderPriority',
                            method: 'GET',
                            dataType: "json"
                        },
                        function (response) {
                            vself.PriorityList.removeAll();

                            if (response) {
                                ko.utils.arrayForEach(response, function (el) {
                                    if (el)
                                        vself.PriorityList.push(new module.Priority(vself, el));
                                });
                                vself.PriorityList.valueHasMutated();
                                vself.IsLoaded(true);
                                loadD.resolve(true);
                            }
                            else {
                                require(['sweetAlert'], function () {
                                    swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[WorkOrderPriority.js, Load]', 'error');
                                    loadD.resolve(false);
                                });
                            }
                        });
                });
                return loadD.promise();
            };
            //
            vself.CheckCurrentPriority = function (priority) {
                if (priority != null)
                    return priority.ID == vself.BasicPriorityID();
                else return false;
            };
            //
            vself.ajaxControl_edit = new ajaxLib.control();
            vself.SelectPriority = function (priority) {
                if (vself.ReadOnly() || priority == null) {
                    return;
                }                    
                
                if (priority.ID == vself.BasicPriorityID()) {
                    return;
                }

                if (vself.ObjectID == null) {
                    vself.BasicPriorityID(priority.ID);
                    vself.ClosePanel();
                    self.RefreshPriority(priority);
                    return;
                }
                
                $.when(userD).done(function (user) {
                    vself.BasicPriorityID(priority.ID);                                
                    vself.ClosePanel();
                    self.RefreshPriority(priority);                    
                });
            };
        },
        Priority: function (parentSelf, obj) {
            var pself = this;
            var vself = parentSelf;
            //
            pself.ID = obj.ID;
            pself.Name = obj.Name;
            pself.Sequence = obj.Sequence;
            pself.Color = obj.Color;
            pself.Default = obj.Default;
        }
    };
    return module;
});