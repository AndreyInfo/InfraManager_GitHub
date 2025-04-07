define(['knockout', 'jquery', 'ajax'], function (ko, $, ajax) {
    var module = {
        Tab: function (vm) {
            var self = this;
            self.ajaxControl = new ajax.control();
            //
            self.Name = getTextResource('Contract_GeneralTab');
            self.Template = '../UI/Forms/Asset/Contracts/frmContract_generalTab';
            self.IconCSS = 'generalTab';
            //
            self.IsVisible = ko.observable(true);
            //
            self.CanUpdate = vm.CanUpdate;
            self.CanEdit = vm.CanUpdate;//for userlib
            //
            //when object changed
            self.init = function (obj) {
            };
            //when tab selected
            self.load = function () {
            };
            //when tab unload
            self.dispose = function () {
                self.ajaxControl.Abort();
            };
            {//editors
                self.editFinanceCenter = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var obj = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(userD).done(function (user) {
                            var options = {
                                ID: obj.ID(),
                                objClassID: obj.ClassID,
                                fieldName: 'FinanceCenter',
                                fieldFriendlyName: getTextResource('Contract_FinanceCenter'),
                                oldValue: obj.FinanceCenterID() ? { ID: obj.FinanceCenterID(), ClassID: 181, FullName: obj.FinanceCenterName() } : null,
                                searcherName: 'FinanceCenterSearcher',
                                searcherPlaceholder: getTextResource('Contract_FinanceCenter'),
                                searcherParams: [],
                                onSave: function (objectInfo) {
                                    obj.FinanceCenterID(objectInfo ? objectInfo.ID : '');
                                    obj.FinanceCenterName(objectInfo ? objectInfo.FullName : '');
                                    vm.raiseObjectModified();
                                }
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    });
                };


                self.editManager = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var obj = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(userD).done(function (user) {
                            var options = {
                                ID: obj.ID(),
                                objClassID: obj.ClassID,
                                fieldName: 'Manager',
                                fieldFriendlyName: getTextResource('Contract_Manager'),
                                oldValue: obj.ManagerLoaded() ? { ID: obj.ManagerID(), ClassID: obj.ManagerClassID(), FullName: obj.ManagerName() } : null,
                                searcherName: 'SDExecutorSearcher',
                                searcherPlaceholder: getTextResource('EnterFIO'),
                                searcherParams: ['3', obj.ProductCatalogTypeID(),obj.ModelID()],
                                onSave: function (objectInfo) {
                                    obj.ManagerLoaded(true);
                                    obj.ManagerID(objectInfo ? objectInfo.ID : null);
                                    obj.ManagerClassID(objectInfo ? objectInfo.ClassID : null);
                                    obj.ManagerName(objectInfo ? objectInfo.FullName : '')
                                    obj.ManagerPositionName('');
                                    obj.ManagerDivisionFullName('');
                                    //
                                    if (objectInfo && objectInfo.ClassID == 9) {
                                        var ajaxControl = new ajax.control();
                                        
                                        ajaxControl.Ajax(null,
                                            {
                                                dataType: "json",
                                                method: 'GET',
                                                url: '/api/users/' + objectInfo.ID
                                            },
                                            function (userData) {
                                                if (userData) {
                                                    obj.ManagerPositionName(userData.PositionName);
                                                    obj.ManagerDivisionFullName(userData.SubdivisionFullName);
                                                }
                                            }
                                        );
                                    }
                                    //
                                    vm.raiseObjectModified();
                                }
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    });
                };

                self.editInitiator = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var obj = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        $.when(userD).done(function (user) {
                            var options = {
                                ID: obj.ID(),
                                objClassID: obj.ClassID,
                                fieldName: 'Initiator',
                                fieldFriendlyName: getTextResource('ContractInitiator'),
                                oldValue: obj.InitiatorLoaded() ? { ID: obj.InitiatorID(), ClassID: obj.InitiatorClassID(), FullName: obj.InitiatorName() } : null,
                                searcherName: 'UtilizerSearcher',
                                searcherPlaceholder: getTextResource('EnterFIO'),
                                searcherParams: [user.UserID],
                                onSave: function (objectInfo) {
                                    obj.InitiatorLoaded(true);
                                    obj.InitiatorID(objectInfo ? objectInfo.ID : null);
                                    obj.InitiatorClassID(objectInfo ? objectInfo.ClassID : null);
                                    obj.InitiatorName(objectInfo ? objectInfo.FullName : '')
                                    obj.InitiatorPositionName('');
                                    obj.InitiatorDivisionFullName('');
                                    //
                                    if (objectInfo && objectInfo.ClassID == 9) {
                                        var ajaxControl = new ajax.control();
                                        
                                        ajaxControl.Ajax(null,
                                            {
                                                dataType: "json",
                                                method: 'GET',
                                                url: '/api/users/' + objectInfo.ID
                                            },
                                            function (userData) {
                                                if (userData) {
                                                    obj.InitiatorPositionName(userData.PositionName);
                                                    obj.InitiatorDivisionFullName(userData.SubdivisionFullName);
                                                }
                                            }
                                        );
                                    }
                                }
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    });
                };

                self.editUpdateType = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    var checkbox = vm.$region.prevObject.find('.slider-checkbox .pronongation-input');
                    //
                    if (!checkbox || !checkbox[0])
                        return;
                    //
                    var oldValue = !checkbox[0].checked;
                    //
                    showSpinner();
                    var obj = vm.object();
                    obj.UpdateType(checkbox[0].checked ? 0 : 1);

                    var data = {
                        ID: obj.ID(),
                        ObjClassID: obj.ClassID,
                        Field: 'UpdateType',
                        OldValue: JSON.stringify({ 'val': oldValue }),
                        NewValue: JSON.stringify({ 'val': checkbox[0].checked }),
                        ReplaceAnyway: false
                    };

                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                //
                                hideSpinner();
                                if (result === 0) {
                                    checkbox[0].checked = obj.UpdateType() === 0;
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });
                };

                self.costClick = function () {
                    var obj = vm.object();
                    if (obj.IsReInit())
                        return;
                    showSpinner();
                    require(['assetForms'], function (module) {
                        var fh = new module.formHelper(true);
                        //
                        var oldCost = obj.Cost();
                        var oldNDSType = obj.NDSType();
                        var oldNDSPercent = obj.NDSPercent();
                        var oldNDSCustomValue = obj.NDSCustomValue();
                        var oldTimePeriod = obj.TimePeriod();
                        //
                        var editOnStore = function () {
                            var retvalD = $.Deferred();
                            if (!vm.CanUpdate()) {
                                retvalD.resolve(false);
                                return retvalD;
                            }
                            //
                            if (obj.Cost() === oldCost && obj.NDSType() === oldNDSType && obj.NDSPercent() === oldNDSPercent && obj.NDSCustomValue() === oldNDSCustomValue && obj.TimePeriod() === oldTimePeriod) {
                                retvalD.resolve(true);
                                return retvalD;
                            }
                            //
                            showSpinner();
                            var object = vm.object();
                            var newValue = JSON.stringify({ 'Cost': obj.Cost(), 'NDSType': obj.NDSType(), 'NDSPercent': obj.NDSPercent(), 'NDSCustomValue': obj.NDSCustomValue(), 'TimePeriod': obj.TimePeriod() });

                            var data = {
                                ID: object.ID(),
                                ObjClassID: object.ClassID,
                                Field: 'Cost',
                                //OldValue: JSON.stringify({ 'val': oldCost }),
                                NewValue: newValue,
                                ReplaceAnyway: true
                            };

                            self.ajaxControl.Ajax(
                                null,//self.$region, two spinner problem
                                {
                                    dataType: "json",
                                    method: 'POST',
                                    url: '/sdApi/SetField',
                                    data: data
                                },
                                function (retModel) {
                                    if (retModel) {
                                        var result = retModel.ResultWithMessage.Result;
                                        //
                                        hideSpinner();
                                        if (result === 0) {
                                            retvalD.resolve(true);
                                            vm.raiseObjectModified();                                            
                                        }
                                        else if (result === 8) {
                                            retvalD.resolve(false);
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('ErrorCaption'), result.Message && result.Message.length > 0 ? result.Message : getTextResource('ValidationError'), 'error');
                                            });
                                        }
                                        else {
                                            retvalD.resolve(false);
                                            require(['sweetAlert'], function () {
                                                swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                            });
                                        }
                                    }
                                });
                            return retvalD;
                        };
                        //
                        var CanChange = ko.observable(self.CanUpdate() && vm.CanEdit());
                        fh.ShowCostNDS(obj.Cost, obj.NDSType, obj.NDSPercent, obj.NDSCustomValue, obj.TimePeriod, CanChange, editOnStore);
                    });
                };
                //
                self.editDateRegistered = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            ClassID: object.ClassID,
                            fieldName: 'DateRegistered',
                            fieldFriendlyName: getTextResource('Contact_RegistrationDate'),
                            oldValue: object.UtcDateRegisteredDT(),
                            allowNull: false,
                            OnlyDate: true,
                            onSave: function (newDate) {
                                object.UtcDateRegistered(parseDate(newDate, true));
                                object.UtcDateRegisteredDT(newDate ? new Date(parseInt(newDate)) : null);
                                vm.raiseObjectModified();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };

                self.editExternalNumber = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'ExternalNumber',
                            fieldFriendlyName: getTextResource('Contract_ExternalNumber'),
                            oldValue: object.ExternalNumber(),
                            allowNull: true,
                            maxLength: 250,
                            onSave: function (newText) {
                                object.ExternalNumber(newText);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };

                self.editNote = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'Note',
                            fieldFriendlyName: getTextResource('Contract_Note'),
                            oldValue: object.Notice(),
                            allowNull: true,
                            maxLength: 255,
                            onSave: function (newText) {
                                object.Notice(newText);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                    });
                };

                self.editDateStarted = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            ClassID: object.ClassID,
                            fieldName: 'DateStarted',
                            fieldFriendlyName: getTextResource('Contract_StartDate'),
                            oldValue: object.UtcDateStartedDT(),
                            allowNull: false,
                            OnlyDate: true,
                            onSave: function (newDate) {
                                object.UtcDateStarted(parseDate(newDate, true));
                                object.UtcDateStartedDT(newDate ? new Date(parseInt(newDate)) : null);
                                vm.raiseObjectModified();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };

                self.editDateFinished = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    var object = vm.object();
                    require(['usualForms'], function (module) {
                        var fh = new module.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            ClassID: object.ClassID,
                            fieldName: 'DateFinished',
                            fieldFriendlyName: getTextResource('Contract_EndDate'),
                            oldValue: object.UtcDateFinishedDT(),
                            allowNull: false,
                            OnlyDate: true,
                            onSave: function (newDate) {
                                object.UtcDateFinished(parseDate(newDate, true));
                                object.UtcDateFinishedDT(newDate ? new Date(parseInt(newDate)) : null);
                                vm.raiseObjectModified();
                            }
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.dateEdit, options);
                    });
                };

                self.editUpdatePeriod = function () {
                    if (!vm.CanUpdate())
                        return;
                    //
                    var object = vm.object();
                    require(['usualForms'], function (fhModule) {
                        var fh = new fhModule.formHelper(true);
                        var options = {
                            ID: object.ID(),
                            objClassID: object.ClassID,
                            fieldName: 'UpdatePeriod',
                            fieldFriendlyName: getTextResource('Contract_ProlongationEventMonthCount'),
                            oldValue: object.UpdatePeriod(),
                            maxValue: 255,
                            onSave: function (newVal) {
                                var newValStr = newVal.toString();
                                object.UpdatePeriod(newVal);
                                object.UpdatePeriod(newValStr);
                                vm.raiseObjectModified();
                            },
                        };
                        fh.ShowSDEditor(fh.SDEditorTemplateModes.numberEdit, options);
                    });
                };
            }
            //
            self.ShowSupplierList = function () {
                if (!vm.CanUpdate())
                    return;
                    //
                showSpinner();
                //
                var editSupplier = function (newSupplier) {
                    if (!vm.CanUpdate())
                        return;
                    //
                    showSpinner();
                    //
                    var obj = vm.object();
                    var oldValue = JSON.stringify({ 'id': obj.SupplierID() });
                    var newValue = JSON.stringify({ 'id': newSupplier.ID });

                    var data = {
                        ID: obj.ID(),
                        ObjClassID: obj.ClassID,
                        Field: 'Supplier',
                        OldValue: oldValue,
                        NewValue: newValue,
                        ReplaceAnyway: false
                    };

                    self.ajaxControl.Ajax(
                        null,//self.$region, two spinner problem
                        {
                            dataType: "json",
                            method: 'POST',
                            url: '/sdApi/SetField',
                            data: data
                        },
                        function (retModel) {
                            if (retModel) {
                                var result = retModel.ResultWithMessage.Result;
                                //
                                hideSpinner();
                                if (result === 0) {
                                    obj.SupplierID(newSupplier.ID);
                                    obj.SupplierName(newSupplier.Name);
                                    //
                                    var tab_contacts = vm.tabList()[1];
                                    tab_contacts.listLoaded = false;
                                    //
                                    vm.raiseObjectModified();
                                }
                                else {
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource('SaveError'), getTextResource('GlobalError'), 'error');
                                    });
                                }
                            }
                        });
                };
                //
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSupplierList(editSupplier);
                });
            };
            //
            self.saveWorkOrder = function (workorder) {
                var obj = vm.object();
                if (!obj)
                    return;
                //
                var data = {
                    ID: obj.ID(),
                    ObjClassID: obj.ClassID,
                    Field: 'WorkOrder',
                    OldValue: JSON.stringify({ 'id': obj.WorkOrderID() }),
                    NewValue: JSON.stringify({ 'id': workorder ? workorder.ID : null }),
                    Params: null,
                    ReplaceAnyway: false
                };
                //
                self.ajaxControl.Ajax(
                    self.$region,
                    {
                        dataType: "json",
                        method: 'POST',
                        url: '/sdApi/SetField',
                        data: data
                    },
                    function (retModel) {
                        if (retModel) {
                            obj.WorkOrderID(workorder ? workorder.ID : null);
                            obj.WorkOrderName(workorder ? workorder.NumberName() : '');
                            vm.raiseObjectModified();
                        }
                        else {
                            require(['sweetAlert'], function () {
                                swal(getTextResource('SaveError'), getTextResource('GlobalError') + '\n[frmContract.js saveWorkOrder]', 'error');
                            });
                        }
                    });
            };
            self.editWorkorder = function () {
                if (!vm.CanUpdate())
                    return;
                //
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowSearcherLite([119], null, null, null, null, self.saveWorkOrder);
                });
            };
            self.removeWorkorder = function () {
                if (!vm.CanUpdate())
                    return;
                //
                self.saveWorkOrder(null);
            };
            self.showWorkOrder = function () {
                var obj = vm.object();
                if (!obj || obj.WorkOrderID() == null)
                    return;
                //
                showSpinner();
                require(['sdForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowWorkOrder(obj.WorkOrderID(), fh.Mode.Default);
                });
            };
            //
            self.showLastAgreement = function () {
                var obj = vm.object();
                if (!obj || obj.LastAgreementID() == null)
                    return;
                //
                showSpinner();
                require(['assetForms'], function (module) {
                    var fh = new module.formHelper(true);
                    fh.ShowServiceContractAgreement(obj.LastAgreementID());
                });
            };
        }
    };
    return module;
});