define(['knockout', 'jquery', 'ajax', 'formControl', 'treeControl', 'usualForms'], function (ko, $, ajaxLib, formControl, treeLib, fhModule) {
    var module = {
        ViewModel: function (options) {
            var self = this;
            //       
            self.tvSearchText = ko.observable('');
            //
            self.locationSearcher = null;
            self.initLocationSearcherControl = function () {
                var $frm = $('#' + self.frm.GetRegionID()).find('.frmAssetLocation');
                var searcherControlD = $.Deferred();
                //
                var fh = new fhModule.formHelper();
                var searcherLoadD = fh.SetTextSearcherToField(
                    $frm.find('.locationSearcher'),
                    'AssetLocationSearcher',
                    null,
                    [false, false, true, false, null],
                    function (objectInfo) {//select
                        self.tvSearchText(objectInfo.FullName);
                        $.when(self.navigator.OpenToNode(objectInfo.ID, objectInfo.ClassID)).done(function (finalNode) {
                            if (finalNode && finalNode.ID == objectInfo.ID) {
                                self.navigator.SelectNode(finalNode);
                                self.navigator_nodeSelected(finalNode);
                            }
                        });
                    },
                    function () {//reset
                        self.tvSearchText('');
                    },
                    function (selectedItem) {//close
                        if (!selectedItem) {
                            self.tvSearchText('');
                        }
                    },
                    undefined,
                    true);
                $.when(searcherLoadD, userD).done(function (ctrl, user) {
                    var old = options.PlaceName() != 'Нет' && options.PlaceName() ? options.PlaceName() : '';
                    if (old != null) {
                        self.tvSearchText(old);
                    }
                    searcherControlD.resolve(ctrl);
                    ctrl.CurrentUserID = user.ID;
                    self.locationSearcher = ctrl;
                });
            };
            //
            self.navigatorObjectID = ko.observable(null);
            self.navigatorObjectClassID = ko.observable(null);
            //
            self.navigator = null;
            self.initNavigator = function () {
                var $div = $('#' + self.frm.GetRegionID()).find('.tvWrapper');
                //
                self.navigator = new treeLib.control();
                self.navigator.init($div, 1, {
                    onClick: self.navigator_nodeSelected,
                    UseAccessIsGranted: false,
                    ShowCheckboxes: false,
                    AvailableClassArray: [29, 101, 1, 2, 3, 22],
                    ClickableClassArray: [3, 22],
                    AllClickable: false,
                    FinishClassArray: [22],
                    Title: getTextResource('LocationCaption'),
                    WindowModeEnabled: false,
                    HasLifeCycle: false,
                    ExpandFirstNodes: true,
                });
                $.when(self.navigator.$isLoaded).done(function () {
                    $div.find('.treeControlWrapper .treeControlHeader').click();//открыть по местоположению
                    var PlaceInfoID = options.PlaceID();
                    var PlaceInfoClassID = options.PlaceClassID();
                    if (PlaceInfoID != null) {
                        $.when(self.navigator.OpenToNode(PlaceInfoID, PlaceInfoClassID)).done(function (finalNode) {
                            if (finalNode && finalNode.ID == PlaceInfoID) {
                                self.navigator.SelectNode(finalNode);
                            }
                        });
                    }
                });
            };
            self.navigator_nodeSelected = function (node) {
                self.navigatorObjectClassID(node.ClassID);
                self.navigatorObjectID(node.ID);
                return true;
            };
            //
            self.dispose = function () {
                //todo tv dispose
                self.locationSearcher.Remove();
            };
            self.afterRender = function (editor, elements) {
                self.initLocationSearcherControl();
                self.initNavigator();
            };
        },


        ShowDialog: function (options, onSelected, isSpinnerActive) {
            if (isSpinnerActive != true)
                showSpinner();
            //
            var frm = undefined;
            var vm = new module.ViewModel(options);
            var bindElement = null;
            var handleOfSelectedNode = null;
            //
            var buttons = [];
            var bSelect = {
                text: getTextResource('Select'),
                click: function () {
                    if (vm.navigatorObjectID() != null) {
                        var newLocationInfo = {
                            ID: vm.navigatorObjectID(),
                            ClassID: vm.navigatorObjectClassID(),
                        };
                        onSelected(newLocationInfo);
                        frm.Close();
                    }
                }
            }
            var bCancel = {
                text: getTextResource('Close'),
                click: function () { frm.Close(); }
            }
            buttons.push(bCancel);
            //
            frm = new formControl.control(
                'region_frmCallClientLocation',//form region prefix
                'setting_frmCallClientLocation',//location and size setting
                getTextResource('LocationCaption'),//caption
                true,//isModal
                true,//isDraggable
                true,//isResizable
                300, 350,//minSize
                buttons,//form buttons
                function () {
                    handleOfSelectedNode.dispose();
                    vm.dispose();
                    ko.cleanNode(bindElement);
                },//afterClose function
                'data-bind="template: {name: \'../UI/Forms/Asset/frmAssetLocation\', afterRender: afterRender}"'//attributes of form region
            );
            if (!frm.Initialized)
                return;//form with that region and settingsName was open
            frm.ExtendSize(400, 550);//normal size
            vm.frm = frm;
            handleOfSelectedNode = vm.navigatorObjectID.subscribe(function (newValue) {
                buttons = [];
                buttons.push(bCancel);
                if (newValue != null)
                    buttons.push(bSelect);
                //
                var scrollEl = $('#' + frm.GetRegionID()).find('.tvWrapper');
                var scrollTop = scrollEl.length > 0 ? scrollEl[0].scrollTop : 0;
                frm.UpdateButtons(buttons);
                if (scrollEl.length > 0)
                    scrollEl[0].scrollTop = scrollTop;
            });
            //
            bindElement = document.getElementById(frm.GetRegionID());
            ko.applyBindings(vm, bindElement);
            //
            $.when(frm.Show()).done(function (frmD) {
                hideSpinner();
            });
        },

    };
    return module;
});