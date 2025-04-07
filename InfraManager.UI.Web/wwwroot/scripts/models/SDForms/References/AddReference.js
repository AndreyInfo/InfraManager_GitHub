define([], function () {
    const module = {
        ViewModel: function (tableViewModel, parentViewModel) {
            const self = this;

            self.Table = tableViewModel;

            self.Table.listViewRowClick = function (obj) {
                showSpinner();
                require(['sdForms'], function (module) {
                    const fh = new module.formHelper(true);
                    parentViewModel.viewDetails(obj, fh);
                });
            }
        }
    };

    return module;
});

