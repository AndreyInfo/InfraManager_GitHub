define(['jquery', 'models/SDForms/References/ReferencesTab', 'groupOperation'], function ($, referencesTabModule, groupOperation) {
    const module = {
        ViewModel: function (form, referencedObject, options) {
            referencesTabModule.SetupMenuOptions(options, {
                addNew: 'AddNewWorkOrder',
                addReference: 'AddWorkOrderReference',
                remove: 'RemoveWorkOrder'
            });
            referencesTabModule.TableViewModel.call(this, form, function (selectedWorkOrders) {
                return new module.AddViewModel(referencedObject(), selectedWorkOrders);
            }, options);
            this.title = 'LinkWorkorders';
            this.appendTitle = 'AddWorkOrderReferenceDialogTitle';
            this.removeObjectTitle = 'WorkOrderRemoving';
            this.viewDetails = function (obj, fh) {
                fh.ShowWorkOrder(obj.IMObjID, fh.Mode.Default);
            };
            this.createNew = function (fh) {
                var retD = $.Deferred();
                var refObject = referencedObject();
                $.when(fh.ShowWorkOrderRegistration(refObject.ClassId, refObject.Id)).done(function (workOrderID) {
                    retD.resolve([workOrderID]);
                });

                return retD.promise();
            };
            this.deleteReferences = function (selectedReferences) {
                var groupOperation = new module.RemoveReferenceViewModel(selectedReferences);
                return referencesTabModule.DeleteMany(groupOperation);
            };
            this.deleteObjects = function (selectedReferences) {
                return referencesTabModule.DeleteMany(new module.RemoveWorkOrderViewModel(selectedReferences));
            };
        },
        AddViewModel: function (referencedObject, selectedWorkOrders) {
            groupOperation.ViewModelBase.call(this, selectedWorkOrders, {
                batchSize: 1,
                ajax: {
                    method: 'PATCH',
                    dataType: 'json',
                    contentType: 'application/json'
                }
            });
            this._getUrl = function (wo) {
                return '/api/workorders/' + wo.IMObjID;
            };
            this._getData = function () {
                return JSON.stringify({ ReferencedObject: referencedObject });
            }
        },
        RemoveReferenceViewModel: function (selectedWorkOrders) {
            groupOperation.ViewModelBase.call(this, selectedWorkOrders, {
                batchSize: 1,
                ajax: {
                    method: 'PATCH',
                    dataType: 'json',
                    contentType: 'application/json'
                }
            });
            this._getUrl = function (wo) {
                return '/api/workorders/' + wo.IMObjID;
            };
            this._getData = function () {
                return JSON.stringify({ ReferencedObject: {} });
            }
        },
        RemoveWorkOrderViewModel: function (selectedWorkOrders) {
            groupOperation.ViewModelBase.call(this, selectedWorkOrders, {
                batchSize: 1,
                ajax: groupOperation.DeleteAjaxOptions
            });
            this._getUrl = function (wo) { return '/api/workorders/' + wo.IMObjID; };
            this._getData = function () { return null; };
        }
    };

    return module;
});

