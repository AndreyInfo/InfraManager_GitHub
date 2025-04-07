define([
    'knockout',
    'jquery',
    'formControl',
    'models/SDForms/References/ReferencesTable',
    'models/SDForms/References/AddReference',    
], function (
    ko,
    $,
    fc,
    tableModule,
    appendReferenceModule) {
    var module = {
        ViewModel: function (
            tableViewModel,
            appendViewModelCreator,
            addReferencesViewModelCreator,
            options) {
            var self = this;

            self.title = 'Links';
            self.IsReadOnly = options.isReadOnly;
            self.CanEdit = options.canEdit;

            // helpers
            self.createModalConfirm = function (titlePrefix, onConfirm) {
                const question = self.Table.getCheckedItems().map(function (item) {
                    return `№ ${item.Number}`;
                }).join(', ');

                require(['sweetAlert'], function (swal) {
                    swal({
                        title: `${titlePrefix}: ${question}`,
                        text: getTextResource('ConfirmRemoveQuestion'),
                        showCancelButton: true,
                        closeOnConfirm: false,
                        closeOnCancel: true,
                        confirmButtonText: getTextResource('ButtonOK'),
                        cancelButtonText: getTextResource('ButtonCancel')
                    },
                        function (value) {
                            swal.close();
                            //
                            if (!value) {
                                return;
                            };

                            onConfirm();
                        });
                });
            };

            function appendReferences(selectedObjects) {
                var groupOperationViewModel = addReferencesViewModelCreator(selectedObjects);
                var baseSuccess = groupOperationViewModel._onSuccess;
                groupOperationViewModel._onSuccess = function (obj, response) {
                    baseSuccess(obj, response);
                    self.Table.appendRow(obj);
                }
                groupOperationViewModel.start();
            }

            // methods
            self.addReferenceWithCreate = function () {
                if (!self.CanEdit()) {
                    return;
                }
                require(['registrationForms'], function (lib) {
                    var fh = new lib.formHelper(true);
                    $.when(self.createNew(fh)).done(function (ids) {
                        self.Table.addReferencesByID(ids);
                    });
                });
            };

            self.addReference = function () {
                if (!self.CanEdit()) {
                    return;
                }

                var cancelButton = {
                    text: getTextResource('ButtonCancel'),
                    click: function () {
                        ctrl.Close();
                    }
                };

                var selectButton = {
                    text: getTextResource('ELPVendor_ReadyButton'),
                    'Class': 'btnVisibility',
                    click: function () {
                        appendReferences(self.AddRefencesTable.getCheckedItems());
                        ctrl.Close();
                    }
                };

                var buttons = [cancelButton, selectButton];

                var ctrl = new fc.control(
                    self.appendTitle,
                    null,
                    getTextResource(self.appendTitle),
                    true, true, true, 1000, 500, buttons, null,
                    'data-bind="template: {name: \'' + options.appendTemplate + '\'}"'
                );

                var ctrlD = ctrl.Show();

                $.when(ctrlD).done(function () {
                    var elem = $(`#${ctrl.GetRegionID()}`);
                    var model = appendViewModelCreator(elem);
                    ko.applyBindings(model, elem.get(0));
                    self.AddRefencesTable = model.Table;

                    var form = $(`[aria-describedby="${ctrl.FormID}"]`);
                    var buttonsContainer = form.find('.ui-dialog-buttonpane');
                    buttonsContainer.addClass('ui-dialog-buttonpane_center');
                });
            };

            self.deleteSelectedReferences = function () {
                if (!self.CanEdit()) {
                    return;
                }

                self.createModalConfirm(getTextResource('ReferenceRemoving'), function () {
                    $.when(self.deleteReferences(self.Table.getCheckedItems())).done(function (ids) {
                        self.Table.removeReferencesByID(ids);
                    });
                });
            };

            self.removeObjectTitle = 'ReferenceRemoving'
            self.deleteSelectedObjects = function () {
                if (!self.CanEdit()) {
                    return;
                }

                self.createModalConfirm(getTextResource(self.removeObjectTitle), function () {
                    $.when(self.deleteObjects(self.Table.getCheckedItems())).done(function (ids) {
                        self.Table.removeReferencesByID(ids);
                    });
                });
            };

            self.createNewTitle = 'MassIncident_CreateNew';
            self.addReferenceTitle = 'MassIncident_AddReference'
            self.removeTitle = 'MassIncident_Delete';
            self.Table = tableViewModel;
            self.Table.listViewRowClick = function (obj) {
                showSpinner();
                require(['sdForms'], function (module) {
                    const fh = new module.formHelper(true);
                    self.viewDetails(obj, fh);
                });
            }

            self.Table.addContextMenuAction(options.menu.addNew, self.addReferenceWithCreate, self.CanEdit);
            self.Table.addContextMenuAction(options.menu.addReference, self.addReference, self.CanEdit);
            self.Table.addContextMenuAction('RemoveReference', self.deleteSelectedReferences, self.CanEdit);
            self.Table.addContextMenuAction(options.menu.remove, self.deleteSelectedObjects, self.CanEdit);

            self.AddRefencesTable = null;
        },
        TableViewModel: function (
            form,
            addReferencesViewModelCreator,
            options) {
            var self = this;
            module.ViewModel.call(
                this,
                new tableModule.ViewModel(form, options.list.view, options.list.ajax),
                function (elem) {
                    return new appendReferenceModule.ViewModel(
                        new tableModule.ViewModel(elem, options.append.view, options.append.ajax),
                        self);
                },
                addReferencesViewModelCreator,
                options.tab);
        },
        DeleteMany: function (groupOperation) {
            var retD = $.Deferred();
            var deletedRows = [];

            groupOperation.subscribeSuccess(function (obj) {
                deletedRows.push(obj.ID);
            });
            groupOperation.subscribeComplete(function () {
                retD.resolve(deletedRows);
            });
            groupOperation.start();

            return retD.promise();
        },
        SetupMenuOptions: function (options, defaultOptions) {
            options.tab.menu = Object.assign(options.tab.menu || {}, defaultOptions);
        }
    };

    return module;
});