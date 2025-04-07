define(['knockout',
    'jquery',
    'ajax',
    'formControl',
    'ui_lists/Asset/LicenceList',
    'ui_lists/Asset/Table.ContextMenu',
    'ui_controls/ContextMenu/ko.ContextMenu'
],
    function (ko,
        $,
        ajaxLib,
        fc,
        licenseList,
        cxMenuModule
    ) {
        var module = {
            CaptionComponentName: 'SoftwarePoolCaptionComponent',
            Modes: {
                structurePool: 1,
                references: 2
            },
            Classes: {
                Manufacturer: 89
            },
            StructurePoolView: "SoftwareSublicenseReferencesPool",
            ReferencesView: "SoftwareSublicenseReferences",
            ViewModelBase: function (pool, viewname, frm, $region) {
                var self = this;
                self.$region = $region;
                self.pool = pool;

                // model
                {
                    self.caption = ko.pureComputed(function () { return '' });
                    self.softwareDistributionCentre = ko.observable('');
                    self.softwareModel = ko.observable(pool.SoftwareModelName);
                    self.manufacturer = ko.observable('');
                    self.poolName = ko.pureComputed(function () {
                        return self.manufacturer()
                            + ' '
                            + self.softwareModel()
                            + ' / '
                            + getTextResource('SDC_Object')
                            + ' '
                            + self.softwareDistributionCentre();
                    });
                }

                // load 
                function loadSoftwareDistributionCentre(id) {
                    var retD = $.Deferred();

                    new ajaxLib.control().Ajax($region, {
                        dataType: "json",
                        method: 'GET',
                        url: '/assetApi/SoftwareDistributionCentres/' + self.pool.SoftwareDistributionCentreID
                    },
                    function (response) {
                        if (response && response.Result === 0 && response.Data) {
                            var data = response.Data;
                            retD.resolve(data.ObjectData);
                        } else if (response && response.Result === 1)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('NullParamsError') + '\n[frmStructurePool.js, getData]', 'error');
                            });
                        else if (response && response.Result === 2)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('BadParamsError') + '\n[frmStructurePool.js, getData]', 'error');
                            });
                        else if (response && response.Result === 3)
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AccessError'), 'error');
                            });
                        else if (response && response.Result === 7) {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('OperationError'), 'error');
                            });
                        } else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('GlobalError') + '\n[frmStructurePool.js, getData]', 'error');
                            });
                        }
                    });

                    return retD.promise();
                };

                function getName(objectId, classId) {
                    var retD = $.Deferred();

                    new ajaxLib.control().Ajax($region, {
                        dataType: "json",
                        method: 'GET',
                        url: '/searchApi/getObjectFullName?objectID=' + objectId + '&objectClassID=' + classId
                    },
                    function (response) {
                        retD.resolve(response.result);
                    });

                    return retD.promise();
                };

                self.Load = function () {
                    var retD = $.Deferred();
                    $.when(
                        loadSoftwareDistributionCentre(),
                        getName(pool.ManufacturerID, module.Classes.Manufacturer)
                    ).done(function (sdc, manufacturer) {
                        self.manufacturer(manufacturer);
                        self.softwareDistributionCentre(sdc.Name);

                        retD.resolve();
                    });

                    return retD.promise();
                };

                self.dispose = function () {
                    if (self.list) {
                        self.list.dispose();
                    }
                };

                self.AfterRender = function () {
                    self.onHeightChanged();
                }

                self.onHeightChanged = function () {
                    setTimeout(
                        function () {
                            $('#' + self.list.listViewID + ' .tableData')
                                .css('height', ($('.frmStructurePool-Content').height() - 175) + 'px')
                        }, 250);
                };

                // list of references
                {
                    self.viewName = ko.observable(viewname);
                    self.list = new licenseList.List(self);

                    self.list.softwarePoolSettings = pool;
                    self.list.hasFilter(false);
                    self.list.SelectedItemsChanged = function () {
                    };
                    self.list.listViewRowClick = function () {
                        self.list.showDetails();
                    };
                }

                // close
                {
                    self.close = function () {
                        frm.Close();
                    }
                }
            },
            PoolStructureViewModel: function (pool, frm, $region) {
                var self = this;
                module.ViewModelBase.call(this, pool, module.StructurePoolView, frm, $region);

                self.listTitle = getTextResource('StructurePool');
                self.caption = ko.pureComputed(function () {
                    return getTextResource('StructurePool')
                        + ' '
                        + self.poolName();
                });
            },
            ReferencesViewModel: function (pool, frm, $region) {
                var self = this;
                module.ViewModelBase.call(this, pool, module.ReferencesView, frm, $region);
                self.listTitle = getTextResource('SoftwareLicence_ReferencesTab_ListCaption');
                self.caption = ko.pureComputed(function () {
                    return getTextResource('SoftwareSublicencePool_GrantedRights_Title')
                        + ' '
                        + self.poolName();
                });                

                self.list.properties = function (contextMenu) { }
                self.list.issueRights = function (contextMenu) { }
                self.list.returnRight = function (contextMenu) {
                    var isEnable = function () {
                        return self.list.getSelectedItems().length == 1;
                    };
                    isVisible = function () {
                        var selected = self.list.getSelectedItems()[0];
                        return (self.list.viewName === 'SoftwareSublicenseReferencesPool'  || self.list.viewName === 'SoftwareSublicenseReferences')
                            && selected
                            && selected.AllowReturnRights;
                    };
                    var action = function () {                        
                        require(['assetForms'], function (module) {
                            var selected = self.list.getSelectedItems();
                            var fh = new module.formHelper(true);
                            fh.ShowLicenceReturning(selected, getTextResource('ReturnRight'), null, 1);
                        });
                    };
                    //
                    var cmd = contextMenu.addContextMenuItem();
                    cmd.restext('ReturnRight');
                    cmd.isEnable = isEnable;
                    cmd.isVisible = isVisible;
                    cmd.click(action);
                    
                }
                
            },
            ShowDialog: function (pool, mode, isSpinnerActive) {
                if (isSpinnerActive != true)
                    showSpinner();
                //
                var $retval = $.Deferred();
                var bindElement = null;
                //
                $.when(userD).done(function (user) {
                    var isReadOnly = false;
                    var forceClose = false;
                    //
                    if (user.HasRoles == false)
                        isReadOnly = true;
                    //
                    var frm = undefined;
                    var vm = undefined;
                    //
                    var buttons = [];

                    var minWidth = 914;
                    var minHeight = 546;
                    frm = new fc.control(
                        'frmStructurePool',//form region prefix
                        'frmStructurePool_setting',//location and size setting
                        'operationName',//caption
                        true,//isModal
                        true,//isDraggable
                        true,//isResizable
                        minWidth, minHeight,//minSize
                        buttons,//form buttons
                        function () {
                            vm.dispose();
                        },//afterClose function
                        'data-bind="template: {name: \'../UI/Forms/Asset/AssetOperations/frmStructurePool\', afterRender: AfterRender}"'//attributes of form region
                    );
                    //
                    if (!frm.Initialized)
                        return;//form with that region and settingsName was open
                    //
                    var $region = $('#' + frm.GetRegionID());
                    var vmConstructor = mode === module.Modes.structurePool ? module.PoolStructureViewModel : module.ReferencesViewModel;
                    vm = new vmConstructor(pool, frm, $region);                    
                    vm.Load();
                    //
                    vm.frm = frm;
                    frm.SizeChanged = function () {
                        var width = frm.GetInnerWidth();
                        var height = frm.GetInnerHeight();
                        
                        vm.$region.css('width', width + 'px').css('height', height + 'px');
                        vm.onHeightChanged();
                    };
                    //
                    ko.applyBindings(vm, document.getElementById(frm.GetRegionID()));
                    $.when(frm.Show(), vm.LoadD).done(function (frmD, loadD) {
                        if (loadD == false) {//force close
                            frm.Close();
                        } else {
                            if (!ko.components.isRegistered(module.CaptionComponentName))
                                ko.components.register(module.CaptionComponentName, {
                                    template: '<span data-bind="text: $str"/>'
                                });
                            frm.BindCaption(vm, "component: {name: '" + module.CaptionComponentName + "', params: { $str: caption } }");
                        }

                        hideSpinner();
                    });
                });
                //
                return $retval.promise();
            }
        };
        return module;
    });