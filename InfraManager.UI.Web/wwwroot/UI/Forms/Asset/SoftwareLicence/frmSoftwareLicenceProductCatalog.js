define(['knockout', 'jquery', 'ajax', 'formControl', 'treeControl', 'usualForms'], function (ko, $, ajaxLib, formControl, treeLib, fhModule) {
    var module = {
        ViewModel: function () {
            var self = this;
            //       
            self.tvSearchText = ko.observable('');
            //
            self.locationSearcher = null;

            self.productCatalogType = ko.observableArray([true, false, false, false, false, false, false, false, 223]);
            self.typeOrModelSearcher = null;
            self.InitializeTypeOrModelSearcher = function () {
                var fh = new fhModule.formHelper();
                var $frm = $('#' + self.frm.GetRegionID()).find('.frmSoftwareLicenceProductCatalog');
                var productD = fh.SetTextSearcherToField(
                    $frm.find('.typeOrModel'),
                    'ProductCatalogTypeAndModelSearcher',
                    null,
                    self.productCatalogType(),
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
                    });
                $.when(productD).done(function (ctrl) {
                    self.typeOrModelSearcher = ctrl;
                    ctrl.CurrentUserID = null;
                });
            };
            //
            self.navigatorObjectID = ko.observable(null);
            self.navigatorObjectClassID = ko.observable(null);
            self.Name = ko.observable(null);
            self.ProductCatalogTemplateID = ko.observable(null);
            //
            self.navigator = null;
            self.initNavigator = function () {
                var $div = $('#' + self.frm.GetRegionID()).find('.tvWrapper');
                //
                self.navigator = new treeLib.control();
                let productCatalogType = 2;
                self.navigator.init($div, productCatalogType,
                    {
                        onClick: function (node) {
                            self.navigatorObjectClassID(node ? node.ClassID : null);
                            self.navigatorObjectID(node ? node.ID : null);
                            self.Name(node ? node.Name : null);
                            self.ProductCatalogTemplateID(node ? node.TemplateID : null);
                            return true;
                        },
                        ShowCheckboxes: false,
                        AvailableClassArray: [374, 378],
                        ClickableClassArray: [378],
                        FinishClassArray: [378],
                        AllClickable: false,
                        HasLifeCycle: false,
                        ExpandFirstNodes: true,
                        AvailableTemplateClassArray: [183, 184, 185, 186, 187, 223, 189]
                    }
                );

                $.when(self.navigator.$isLoaded).done(function () {
                    $div.find('.treeControlWrapper .treeControlHeader').click();
                });
            };
            self.navigator_nodeSelected = function (node) {
                self.navigatorObjectClassID(node.ClassID);
                self.navigatorObjectID(node.ID);
                self.Name(node.Name);
                self.ProductCatalogTemplateID(node.TemplateID);

                return true;
            };
            //
            self.dispose = function () {
                //todo tv dispose
                self.typeOrModelSearcher.Remove();
            };
            self.afterRender = function (editor, elements) {
                self.InitializeTypeOrModelSearcher();
                self.initNavigator();
            };
        },

        ShowDialog: function (onSelectedCallback) {
            showSpinner();
            //
            var frm = undefined;
            var vm = new module.ViewModel();
            var bindElement = null;
            var handleOfSelectedNode = null;
            //
            var buttons = [];
            var bSelect = {
                text: getTextResource('Select'),
                click: function () {
                    if (vm.navigatorObjectID() != null) {
                        var newObjectInfo = {
                            ID: vm.navigatorObjectID(),
                            ClassID: vm.navigatorObjectClassID(),
                            Name: vm.Name(),
                            ProductCatalogTemplateID: vm.ProductCatalogTemplateID()
                        };
                        onSelectedCallback(newObjectInfo);
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
                'region_frmSoftwareLicenceProductCatalog',//form region prefix
                'setting_frmSoftwareLicenceProductCatalog',//location and size setting
                getTextResource('SoftCatalogueTreeCaption'),//caption
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
                'data-bind="template: {name: \'../UI/Forms/Asset/SoftwareLicence/frmSoftwareLicenceProductCatalog\', afterRender: afterRender}"' //attributes of form region
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