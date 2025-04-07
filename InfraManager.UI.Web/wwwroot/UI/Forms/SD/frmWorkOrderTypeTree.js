define(['knockout', 'jquery', 'ajax', 'formControl', 'workOrderTypesTree', 'UI/Controls/ko.TreeView.js'], function (ko, $, ajax, formControl, workOrderTypesTree) {
    var module = {
        ViewModel: function () {
            var self = this;
            {//ko.treeView
                self.selectedWorkOrderTypeID = ko.observable(null);//выделенный идентификатор типа задания

                self.WorkOrderTypesTree = new workOrderTypesTree.WorkOrderTypesTreeViewModel();
                self.WorkOrderTypesTree.selectedNode.subscribe(function (selected) {
                    if (selected && selected.classId === workOrderTypesTree.ObjectClass.WorkOrderType) {
                        self.selectedWorkOrderTypeID(selected.id);
                    }
                });
            }

            {//loading
                self.isLoading = ko.observable(false);
                self.load = function () {
                    var retval = $.Deferred();
                    self.isLoading(true);

                    $.when(self.WorkOrderTypesTree.Load('.frmWorkOrderTypeTree')).done(function () {
                        self.isLoading(false);
                        retval.resolve(true);
                    });

                    return retval;
                };
            }
        },

        ShowDialog: function (isSpinnerActive) {
            var retval = $.Deferred();
            //
            if (isSpinnerActive != true)
                showSpinner();
            //
            var frm = undefined;
            var bindElement = null;
            //
            var vm = new module.ViewModel();
            //отслеживаем изменения в дереве и меняем кнопки на форме (когда меняются кнопки, может потеряться прокрутка)
            var handle_selectedWorkOrderTypeID = vm.selectedWorkOrderTypeID.subscribe(function (id) {
                if (!frm.frm) return;
                buttons = id ? [bSelect, bCancel] : [bCancel];
                var scrollEl = $('#' + frm.GetRegionID()).find('.ko_treeView');
                var scrollTop = scrollEl.length > 0 ? scrollEl[0].scrollTop : 0;
                frm.UpdateButtons(buttons);
                if (scrollEl.length > 0)
                    scrollEl[0].scrollTop = scrollTop;
            });
            //
            var buttons = [];
            var bSelect = {
                text: getTextResource('Select'),
                click: function () {
                    retval.resolve(vm.selectedWorkOrderTypeID());
                    frm.Close();
                }
            }
            var bCancel = {
                text: getTextResource('Close'),
                click: function () {
                    retval.resolve(null);
                    frm.Close();
                }
            }
            buttons.push(bCancel);
            //
            frm = new formControl.control(
                    'region_frmWorkOrderTypeTree',//form region prefix
                    'setting_frmWorkOrderTypeTree',//location and size setting
                    getTextResource('WorkOrderTypeTree_Caption'),//caption
                    true,//isModal
                    true,//isDraggable
                    true,//isResizable
                    500, 400,//minSize
                    buttons,//form buttons
                    function () {
                        handle_selectedWorkOrderTypeID.dispose();
                        ko.cleanNode(bindElement);
                    },//afterClose function
                    'data-bind="template: {name: \'../UI/Forms/SD/frmWorkOrderTypeTree\'}"'//attributes of form region
                );
            if (!frm.Initialized) {
                retval.resolve(undefined);
                return retval.promise();//form with that region and settingsName was open
            }
            //
            bindElement = document.getElementById(frm.GetRegionID());
            ko.applyBindings(vm, bindElement);
            //
            $.when(frm.Show(), vm.load()).done(function (frmD, loadD) {
                hideSpinner();
            });
            //
            return retval.promise();
        }
    };
    return module;
});