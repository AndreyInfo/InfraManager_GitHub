define(['knockout', 'jquery', 'ttControl', 'ajax', 'jqueryMouseWheel'], function (ko, $, tclib, ajaxLib) {
    var module = {
        ViewModel: function () {
            var self = this;
            self.ajaxControl = new ajaxLib.control();
            self.availability = functionsAvailability;
            //
            self.filterLocationVisible = null;//set in Table.cshtml
            self.filterOrgstructureVisible = null;//set in Table.cshtml
            self.filterSoftCatalogueVisible = null;
            self.filterProductCatalogueVisible = null;
            self.changeAssetMonitor = null;
            //
            self.viewName = ko.observable();
            self.viewName.subscribe(function (newValue) {
                $.when(userD).done(function (user) {
                    var oldNameAsset = user.ViewNameAsset;
                    if (user.ViewNameAsset == newValue)
                        return;
                    //            
                    showSpinner($('#regionListMode')[0]);
                    self.ajaxControl.Ajax(null,
                        {
                            contentType: "application/json",
                            url: '/api/UserSettings',
                            method: 'POST',
                            data: JSON.stringify({ ViewNameAsset: newValue }),
                            dataType: "text"
                        },
                        function () {
                            hideSpinner($('#regionListMode')[0]);
                            //
                                user.ViewNameAsset = newValue;
                                //
                                if (self.tableModel && self.filtersModel) {
                                    self.filterLocationVisible(newValue != 'Contracts' && newValue != 'Inventories' && newValue != 'SoftwareLicenseDistribution' &&
                                        newValue != 'ConfigurationUnits' && newValue != 'Clusters' && newValue != 'LogicObjects' && newValue != 'DataEntities');
                                    self.filterOrgstructureVisible(newValue != 'Contracts' && newValue != 'Inventories' && newValue != 'SoftwareLicenseDistribution' &&
                                        newValue != 'ConfigurationUnits' && newValue != 'Clusters' && newValue != 'LogicObjects' && newValue != 'DataEntities');
                                    self.filterProductCatalogueVisible(newValue != 'Inventories' && newValue != 'SoftwareLicenseDistribution' && newValue != 'SoftwareInstallation');
                                    self.filterSoftCatalogueVisible(newValue == 'SoftwareLicense' || newValue == 'SoftwareLicenseDistribution' || newValue == 'SubSoftwareLicense');
                                    self.changeAssetMonitor(user, oldNameAsset);
                                    //
                                    $.when(self.filtersModel.Load()).done(function () {
                                        self.tableModel.reload();
                                    });
                                }       
                        });
                });
            });
            //
            self.HardwareClass = ko.computed(function () {
                return self.viewName() === 'Hardware' ? 'b-content-table__hardware_active' : 'b-content-table__hardware';
            });
            self.MyObjectsClass = ko.computed(function () {
                return self.viewName() === 'MyObjects' ? 'b-content-table__myObjects_active' : 'b-content-table__myObjects';
            });          
            self.DiscardedClass = ko.computed(function () {
                return self.viewName() === 'Discarded' ? 'b-content-table__discarded_active' : 'b-content-table__discarded';
            });
            self.AssetRepairClass = ko.computed(function () {
                return self.viewName() === 'AssetRepair' ? 'b-content-table__assetRepair_active' : 'b-content-table__assetRepair';
            });          
            self.ConsumablesClass = ko.computed(function () {
                return self.viewName() === 'Consumables' ? 'b-content-table__consumables_active' : 'b-content-table__consumables';
            });
            self.ShopCallsClass = ko.computed(function () {
                return self.viewName() === 'ShopCalls' ? 'b-content-table__shopCalls_active' : 'b-content-table__shopCalls';
            });
            self.SoftwareLicenseClass = ko.computed(function () {
                return self.viewName() === 'SoftwareLicense' ? 'b-content-table__softwareLicense_active' : 'b-content-table__softwareLicense';
            });
            self.SoftwareDistributionClass = ko.computed(function () {
                return self.viewName() === 'SoftwareLicenseDistribution' || self.viewName() === 'SubSoftwareLicense' ? 'b-content-table__softwareDistribution_active' : 'b-content-table__softwareDistribution';
            });
            self.UtilizerClass = ko.computed(function () {
                return self.viewName() === 'UtilizerClient' ? 'b-content-table__utilizer_active' : 'b-content-table__utilizer';
            });
            self.UtilizerCompleteClass = ko.computed(function () {
                return self.viewName() === 'UtilizerComplete' ? 'b-content-table__utilizercomplete_active' : 'b-content-table__utilizercomplete';
            });
            self.ContractsClass = ko.computed(function () {
                return self.viewName() === 'Contracts' ? 'b-content-table__contracts_active' : 'b-content-table__contracts';
            });
            self.InventoriesClass = ko.computed(function () {
                return self.viewName() === 'Inventories' ? 'b-content-table__inventory_active' : 'b-content-table__inventory';
            });
            self.ConfigurationUnitsClass = ko.computed(function () {
                return self.viewName() === 'ConfigurationUnits' ? 'b-content-table__configurationUnit_active' : 'b-content-table__configurationUnit';
            });
            self.ClustersClass = ko.computed(function () {
                return self.viewName() === 'Clusters' ? 'b-content-table__cluster_active' : 'b-content-table__cluster';
            });
            self.LogicObjectsClass = ko.computed(function () {
                return self.viewName() === 'LogicObjects' ? 'b-content-table__logicObject_active' : 'b-content-table__logicObject';
            });
            self.DataEntitiesClass = ko.computed(function () {
                return self.viewName() === 'DataEntities' ? 'b-content-table__dataEntity_active' : 'b-content-table__dataEntity';
            });
            self.SoftwareInstallationClass = ko.computed(function () {
                return self.viewName() === 'SoftwareInstallation' ? 'b-content-table__softwareInstallation_active' : 'b-content-table__softwareInstallation';
            });
            //
            self.tableModel = null; //модель таблицы, которой управляем
            self.tableModelExists = ko.observable(false);
            self.filtersModel = null; //модель управления фильтрами
            self.ready = ko.observable(false);
            //
            self.ShowHardware = function () {
                self.viewName('Hardware');
            };
            self.ShowMyObjects = function () {
                self.viewName('MyObjects');
            };          
            self.ShowDiscarded = function () {
                self.viewName('Discarded');
            };
            self.ShowAssetRepair = function () {
                self.viewName('AssetRepair');
            };
            self.ShowConsumables = function () {
                self.viewName('Consumables');
            };
            self.ShowShopCalls = function () {
                self.viewName('ShopCalls');
            };
            self.ShowSoftwareLicense = function () {
                self.viewName('SoftwareLicense');
            };
            self.ShowSoftwareLicenseDistribution = function () {
                self.viewName('SoftwareLicenseDistribution');
            }            
            self.ShowUtilizer = function () {
                self.viewName('UtilizerClient');
            };
            self.ShowUtilizerComplete = function () {
                self.viewName('UtilizerComplete');
            };
            self.ShowContracts = function () {
                self.viewName('Contracts');
            };
            self.ShowInventories = function () {
                self.viewName('Inventories');
            };
            self.ShowConfigurationUnits = function () {
                self.viewName('ConfigurationUnits');
            };
            self.ShowClusters = function () {
                self.viewName('Clusters');
            };
            self.ShowLogicObject = function () {
                self.viewName('LogicObjects');
            };
            self.ShowDataEntity = function () {
                self.viewName('DataEntities');
            };
            self.ShowSoftwareInstallation = function () {
                self.viewName('SoftwareInstallation');
            };
            //
            self.ScrollButtonsVisible = ko.observable(true);
            self.OnScrollDownClick = function () {
                var $scrollContainer = $('.b-content-table__left');
                //
                if ($scrollContainer.length > 0) {
                    var oldvalue = $scrollContainer.scrollTop();
                    if ((oldvalue + $scrollContainer.height()) < $scrollContainer[0].scrollHeight) {
                        var newvalue = oldvalue + 400;
                        $scrollContainer.animate({ scrollTop: newvalue }, 800);
                    }
                }
            };
            self.OnScrollUpClick = function () {
                var $scrollContainer = $('.b-content-table__left');
                //
                if ($scrollContainer.length > 0) {
                    var oldvalue = $scrollContainer.scrollTop();
                    if (oldvalue > 0) {
                        var newvalue = oldvalue - 400 >= 0 ? oldvalue - 400 : 0;
                        $scrollContainer.animate({ scrollTop: newvalue }, 800);
                    }
                }
            };
            self.InitScroll = function () {
                var $scrollcontainer = $('.b-content-table__left');
                $scrollcontainer.mousewheel(function (event, delta) {
                    this.scrollTop -= (delta * 30);
                    event.preventDefault();
                });
            };
            self.UpdateScrollButtonsVisibility = function () {
                var $region = $('.b-content-table__left');
                if ($region.length > 0) {
                    if ($region[0].scrollHeight > $region.height())
                        self.ScrollButtonsVisible(true);
                    else self.ScrollButtonsVisible(false);
                }
                else {
                    self.ScrollButtonsVisible(true);
                    setTimeout(self.UpdateScrollButtonsVisibility, 200);//try again later
                };
            };
            //
            self.AfterRender = function () {
                //return;
                $.when(userD).done(function (user) {
                    if (user.HasRoles) {
                        $('#listModeHardware').css('display', 'block');
                        $('#listModeHardwareText').css('display', 'block');
                        //$('#listModeMyObjects').css('display', 'block');
                        //$('#listModeMyObjectsText').css('display', 'block');
                        //$('#listModeConsumables').css('display', 'block');
                        //$('#listModeConsumablesText').css('display', 'block');
                        //$('#listModeShopCalls').css('display', 'block');
                        //$('#listModeShopCallsText').css('display', 'block');
                        $('#listModeSoftwareLicense').css('display', 'block');
                        $('#listModeSoftwareLicenseText').css('display', 'block');
                        $('#listModeSoftwareDistribution').css('display', 'block');
                        $('#listModeSoftwareDistributionText').css('display', 'block');
                        //$('#listModeUtilizer').css('display', 'block');
                        //$('#listModeUtilizerText').css('display', 'block');
                        $('#listModeUtilizerComplete').css('display', 'block');
                        $('#listModeUtilizerCompleteText').css('display', 'block');
                    }
                    else {
                        $('#listModeHardware').remove();
                        $('#listModeHardwareText').remove();
                        //$('#listModeMyObjects').remove();
                        //$('#listModeMyObjectsText').remove();
                        //$('#listModeConsumables').remove();
                        //$('#listModeConsumablesText').remove();
                        //$('#listModeShopCalls').remove();
                        //$('#listModeShopCallsText').remove();
                        $('#listModeSoftwareLicense').remove();
                        $('#listModeSoftwareLicenseText').remove();
                        //$('#listModeUtilizer').remove();
                        //$('#listModeUtilizerText').remove();
                        $('#listModeUtilizerComplete').remove();
                        $('#listModeUtilizerCompleteText').remove();
                    }
                    //
                    if (user.GrantedOperations.indexOf(885) != -1) {//Asset.WriteOffList
                        $('#listModeDiscarded').css('display', 'block');
                        $('#listModeDiscardedText').css('display', 'block');
                    }
                    else {
                        $('#listModeDiscarded').remove();
                        $('#listModeDiscardedText').remove();
                    }
                    if (user.GrantedOperations.indexOf(884) != -1) {//Asset.RepairList
                        $('#listModeAssetRepair').css('display', 'block');
                        $('#listModeAssetRepairText').css('display', 'block');
                    }
                    else {
                        $('#listModeAssetRepair').remove();
                        $('#listModeAssetRepairText').remove();

                    }
                    if (user.GrantedOperations.indexOf(211) != -1) {//OPERATION_PROPERTIES_SERVICECONTRACT
                        $('#listModeContracts').css('display', 'block');
                        $('#listModeContractsText').css('display', 'block');
                    }
                    else {
                        $('#listModeContracts').remove();
                        $('#listModeContractsText').remove();
                    }
                    if (user.GrantedOperations.indexOf(895) != -1) {//OPERATION_Asset_Inventory
                        $('#listModeInventories').css('display', 'block');
                        $('#listModeInventoriesText').css('display', 'block');
                    }
                    else {
                        $('#listModeInventories').remove();
                        $('#listModeInventoriesText').remove();
                    }
                    if (user.GrantedOperations.indexOf(653) != -1) {//OPERATION_ApplicationModule_SoftwareManagment_View
                        $('#listModeSoftwareLicense').css('display', 'block');
                        $('#listModeSoftwareLicenseText').css('display', 'block');
                    }
                    else {
                        $('#listModeSoftwareLicense').remove();
                        $('#listModeSoftwareLicenseText').remove();
                    }
                    if (user.GrantedOperations.indexOf(955) != -1) {//OPERATION_ConfigurationUnit_Properties
                        $('#listModeConfigurationUnits').css('display', 'block');
                        $('#listModeConfigurationUnitsText').css('display', 'block');
                    }
                    else {
                        $('#listModeConfigurationUnits').remove();
                        $('#listModeConfigurationUnitsText').remove();
                    }
                    if (user.GrantedOperations.indexOf(614) != -1) {//OPERATION_DataEntity_Properties
                        $('#listModeDataEntity').css('display', 'block');
                        $('#listModeDataEntityText').css('display', 'block');
                    }
                    else {
                        $('#listModeDataEntity').remove();
                        $('#listModeDataEntityText').remove();
                    }         
                    if (user.GrantedOperations.indexOf(959) != -1) {//OPERATION_Cluster_Properties
                        $('#listModeClusters').css('display', 'block');
                        $('#listModeClustersText').css('display', 'block');
                        $('#listModeLogicObject').css('display', 'block');
                        $('#listModeLogicObjectText').css('display', 'block');
                        $('#listModeDataEntity').css('display', 'block');
                        $('#listModeDataEntityText').css('display', 'block');
                    }                    
                    else {
                        $('#listModeClusters').remove();
                        $('#listModeClustersText').remove();
                        $('#listModeLogicObject').remove();
                        $('#listModeLogicObjectText').remove();
                        $('#listModeDataEntity').remove();
                        $('#listModeDataEntityText').remove();                        
                    }
                    if (user.GrantedOperations.indexOf(87) != -1) {//OPERATION_INSTALLATIONSOFTWARE
                        $('#listModeSoftwareInstallation').css('display', 'block');
                        $('#listModeSoftwareInstallationText').css('display', 'block');
                    }
                    else {
                        $('#listModeSoftwareInstallation').remove();
                        $('#listModeSoftwareInstallationText').remove();
                    }
                    //
                    self.UpdateScrollButtonsVisibility();
                    self.InitScroll();
                    $(window).resize(self.UpdateScrollButtonsVisibility);
                    //
                    self.viewName(user.ViewNameAsset);
                });
            };
        }
    }
    return module;
});