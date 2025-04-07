define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    'usualForms',
    'dateTimeControl',
    './../Asset/Controls/SoftwareLicenceList',
    'jqueryStepper'],
    function (ko, $, ajax, formControl, fhModule, dtLib, softwareLicenceList) {
    var module = {
        ViewModel: function () {
            var self = this;
            //            
            self.frm = null;
            self.saveFunc = null;//set in frmContract_generalTab.js
            self.list = new softwareLicenceList.List(self);
            //
            {//search
                self.SearchText = ko.observable('');
                self.SearchText.subscribe(function (newValue) {
                    self.WaitAndSearch(newValue);
                });
                self.IsSearchTextEmpty = ko.computed(function () {
                    var text = self.SearchText();
                    if (!text)
                        return true;
                    //
                    return false;
                });
                //
                self.SearchKeyPressed = function (data, event) {
                    if (event.keyCode == 13) {
                        if (!self.IsSearchTextEmpty())
                            self.Search();
                    }
                    else
                        return true;
                };
                self.EraseTextClick = function () {
                    self.SearchText('');
                };
                //
                self.searchTimeout = null;
                self.WaitAndSearch = function (text) {
                    clearTimeout(self.searchTimeout);
                    self.searchTimeout = setTimeout(function () {
                        if (text == self.SearchText())
                            self.Search();
                    }, 500);
                };
            }
            //
            self.ajaxControl_search = new ajax.control();
            self.searchPhraseObservable = ko.observable('');//set in ActivesLocatedLink.js
            self.Search = function () {
                self.searchPhraseObservable(self.SearchText());
                self.list.listView.load();
            };
            //
            self.list.SelectedItemsChanged = function (checkedItemsCount) {
                if (!self.frm)
                    return;
                //
                var buttons = [];
                var bSave = {
                    text: getTextResource('Select'),
                    click: function () {
                        self.Save();
                        self.frm.Close();
                    }
                }
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { self.frm.Close(); }
                }
                if (checkedItemsCount != 0)
                    buttons.push(bSave);
                buttons.push(bCancel);
                //
                self.frm.UpdateButtons(buttons);
            };
            //
            self.Save = function () {
                if (!self.saveFunc)
                    return;
                //
                var selected = self.list.listView.rowViewModel.checkedItems();
                if (selected.length == 0)
                    return;
                //
                self.saveFunc(selected);
            };
            //
            self.AfterRender = function (editor, elements) {
                var $frm = $('#' + self.frm.GetRegionID());
            };
        },

        ShowDialog: function (saveFunc, isSpinnerActive, isMultiSelect) {
            $.when(operationIsGrantedD(217)).done(function (can_properties) {
                //OPERATION_ADD_SUPPLIER = 218,
                //OPERATION_UPDATE_SUPPLIER = 285, 
                //OPERATION_PROPERTIES_SUPPLIER = 217,
                //OPERATION_DELETE_SUPPLIER = 219,
                if (can_properties == false) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('OperationError'));
                    });
                    return;
                }
                //
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var frm = undefined;
                var vm = new module.ViewModel();
                var bindElement = null;
                //
                var buttons = [];
                var bCancel = {
                    text: getTextResource('Close'),
                    click: function () { frm.Close(); }
                }
                buttons.push(bCancel);
                //
                frm = new formControl.control(
                        'region_frmSoftwareLicenceList',//form region prefix
                        'setting_frmSoftwareLicenceList',//location and size setting
                        getTextResource('SoftwareLicenceList'),//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        700, 400,//minSize
                        buttons,//form buttons
                        function () {
                            ko.cleanNode(bindElement);
                            vm.list.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/frmSoftwareLicenceLink\', afterRender: AfterRender}"'//attributes of form region
                    );
                if (!frm.Initialized)
                    return;//form with that region and settingsName was open
                //frm.ExtendSize(600, 700);//normal size
                vm.frm = frm;
                vm.saveFunc = saveFunc;
                vm.isMultiSelect = isMultiSelect;
                //
                bindElement = document.getElementById(frm.GetRegionID());
                ko.applyBindings(vm, bindElement);
                //
                $.when(frm.Show()).done(function (frmD) {
                    hideSpinner();
                });
            });
        },

    };
    return module;
});