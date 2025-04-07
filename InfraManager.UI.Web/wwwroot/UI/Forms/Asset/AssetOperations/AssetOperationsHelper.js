define(['jquery', 'ajax', 'knockout'], function ($, ajaxLib, ko) {
    var module = {
        executeLifeCycleOperation: function (contextMenuItem, selectedObjects) {
            var $retval = $.Deferred();
            //
            if (contextMenuItem.LifeCycleStateOperationID) {
                if (contextMenuItem.CommandType == 0) {//asset registration
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowAssetRegistration(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }
                else if (contextMenuItem.CommandType == 1 || contextMenuItem.CommandType == 7 || contextMenuItem.CommandType == 8) {//asset move; asset from storage; asset to storage
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowAssetMoveForm(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID, contextMenuItem.CommandType)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }
                else if (contextMenuItem.CommandType == 2) {//asset to repair
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowAssetToRepairForm(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }
                else if (contextMenuItem.CommandType == 3) {//asset from repair
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowAssetFromRepairForm(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }
                else if (contextMenuItem.CommandType == 4) {//asset write off
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowAssetOffForm(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }
                else if (contextMenuItem.CommandType == 12) { //give out rights
                    showSpinner();
                    require(['assetForms'], function (module) {
                        self.ajaxControlExecuteContextMenu = new ajaxLib.control();
                        self.ajaxControlExecuteContextMenu.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                data: { DeviceList: selectedObjects},
                                url: '/sdApi/IsDeviceReference'
                            },
                            function (newVal) {
                                if (newVal) {
                                    if (newVal.Result == 0) {
                                        if (newVal.Message) {
                                            var fh = new module.formHelper(true);
                                            $.when(fh.ShowLicenceConsumption(selectedObjects,
                                                contextMenuItem.Name,
                                                contextMenuItem.LifeCycleStateOperationID, newVal.IsResult, 0)).done(function(result) {
                                                $retval.resolve(result);
                                            });
                                            
                                        }
                                    }
                                    else if (newVal.Result != 0) {
                                        require(['sweetAlert'], function () {
                                            swal(contextMenuItem.Name, 'Операция не выполнена', 'error');
                                        });
                                        $retval.resolve(false);
                                    }
                                }
                            });                        
                        
                    });
                    hideSpinner();
                    return $retval.promise();
                }
                else if (contextMenuItem.CommandType == 13) { //return rights
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowLicenceReturning(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }
                else if (contextMenuItem.CommandType == 17) {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowLicenceActive(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }

                else if (contextMenuItem.CommandType == 18) {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowLicenceContractUpdate(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }                
                else if (contextMenuItem.CommandType == 22) {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        
                        let classID = selectedObjects[0].ClassID;
                        
                        if (classID == 223) {
                            $.when(fh.ShowLicenceUpgrade(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                                $retval.resolve(result);
                            });
                        }
                    });
                }
                else if (contextMenuItem.CommandType == 19) {
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(fh.ShowLicenceApplying(selectedObjects, contextMenuItem.Name, contextMenuItem.LifeCycleStateOperationID)).done(function (result) {
                            $retval.resolve(result);
                        });
                    });
                }
                else if (contextMenuItem.CommandType == 11 || contextMenuItem.CommandType == 10 || contextMenuItem.CommandType == 14 || contextMenuItem.CommandType == 20|| contextMenuItem.CommandType == 21) {//workOrder or setState or createLicences
                    self.ajaxControlExecuteContextMenu = new ajaxLib.control();
                    var cmd = {
                        Enabled: contextMenuItem.Enabled,
                        Name: contextMenuItem.Name,
                        CommandType: contextMenuItem.CommandType,
                        LifeCycleStateOperationID: contextMenuItem.LifeCycleStateOperationID
                    };
                    //
                    var retval = [];
                    let url = '/sdApi/ExecuteContextMenu';
                    selectedObjects.forEach(function (item) {                        
                        if (item.hasOwnProperty('ContractSoftwareLicenceID') && (contextMenuItem.CommandType == 14 || contextMenuItem.CommandType == 21)) {
                            retval.push({
                                ClassID: item.ClassID,
                                ID: item.ID,
                                SoftwareLicenceID : item.ContractSoftwareLicenceID
                            });
                            url = '/sdApi/CreateSoftwareLicenceFromContextMenu'; 
                        }
                        else {
                            retval.push({
                                ClassID: item.ClassID,
                                ID: item.ID
                            });
                        }
                    });
                    var data = {
                        DeviceList: retval,
                        Command: cmd
                    };
                    self.ajaxControlExecuteContextMenu.Ajax(null,
                        {
                            dataType: "json",
                            method: 'POST',
                            data: data,
                            url: url
                        },
                        function (newVal) {
                            if (newVal) {
                                if (newVal.Result == 0) {
                                    if (newVal.Message)
                                        require(['sweetAlert'], function () {
                                            swal(contextMenuItem.Name, newVal.Message, 'info');
                                        });
                                    ko.utils.arrayForEach(selectedObjects, function (el) {
                                        $(document).trigger('local_objectUpdated', [el.ClassID, el.ID]);
                                    });
                                    $retval.resolve(true);
                                }
                                else if (newVal.Result != 0) {
                                    require(['sweetAlert'], function () {
                                        swal(contextMenuItem.Name, 'Операция не выполнена', 'error');
                                    });
                                    $retval.resolve(false);
                                }
                            }
                        });
                }
            }
            else {//serviceContractAgreement command
                self.ajaxControlExecuteContextMenu = new ajaxLib.control();
                var cmd = {
                    Enabled: contextMenuItem.Enabled,
                    Name: contextMenuItem.Name,
                    CommandType: contextMenuItem.CommandType,
                    LifeCycleStateOperationID: contextMenuItem.LifeCycleStateOperationID
                };
                //
                self.ajaxControlExecuteContextMenu.Ajax(null,
                    {
                        dataType: "json",
                        method: 'GET',
                        data: { DeviceList: selectedObjects, Command: cmd },
                        url: '/sdApi/ExecuteContextMenu'
                    },
                    function (newVal) {
                        if (newVal) {
                            if (newVal.Result == 0) {
                                if (newVal.Message)
                                    require(['sweetAlert'], function () {
                                        swal(contextMenuItem.Name, newVal.Message, 'info');
                                    });
                                ko.utils.arrayForEach(selectedObjects, function (el) {
                                    $(document).trigger('local_objectUpdated', [el.ClassID, el.ID]);
                                });
                                $retval.resolve(true);
                            }
                            else if (newVal.Result == 8) {//validation
                                require(['sweetAlert'], function () {
                                    swal(contextMenuItem.Name, newVal.Message, 'warning');
                                });
                                $retval.resolve(false);
                            }
                            else if (newVal.Result != 0) {
                                require(['sweetAlert'], function () {
                                    swal(contextMenuItem.Name, 'Операция не выполнена', 'error');
                                });
                                $retval.resolve(false);
                            }
                        }
                    });
            }
            return $retval.promise();
        }
    }
    return module;
});