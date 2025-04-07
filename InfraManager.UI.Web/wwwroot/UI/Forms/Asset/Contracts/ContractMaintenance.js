define(['knockout', 'ajax'], function (ko, ajax) {
    var module = {
        contractMaintenance: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            //            
            self.ID = ko.observable(null);
            self.ServiceContractID = ko.observable(null);
            self.ObjectClassID = ko.observable(null);
            self.ObjectIdentifier = ko.observable(null);
            self.ObjectType = ko.observable(null);
            self.ObjectSoftwareModel = ko.observable(null);
            self.ObjectModel = ko.observable(null);
            self.ObjectSoftwareType = ko.observable(null);
            self.UtcDateMaintenanceStart = ko.observable('');
            self.UtcDateMaintenanceStartDT = ko.observable(null);
            self.UtcDateMaintenanceEnd = ko.observable('');
            self.UtcDateMaintenanceEndDT = ko.observable(null);
            self.OnMaintenance = ko.observable(null);
            //
            self.IdentifierCaption = ko.observable(''); 
            self.TypeCaption = ko.observable('');
            //
            self.ModelCaption = ko.observable('');
            //
            self.ObjectModelVisible = ko.observable(true);
            self.TypeModelCaptionVisible = ko.observable(false);
            //
            self.TypeModelCaption = ko.observable('');
            //
            self.loadFields = function (obj) {
                if (obj.ID)
                    self.ID(obj.ID);
                if (obj.ServiceContractID)
                    self.ServiceContractID(obj.ServiceContractID);
                if (obj.ObjectClassID)
                    self.ObjectClassID(obj.ObjectClassID);
                if (obj.ObjectIdentifier)
                    self.ObjectIdentifier(obj.ObjectIdentifier);
                if (obj.ObjectType || obj.ObjectSoftwareModel)
                    self.ObjectType(obj.ObjectClassID == 223 /*OBJ_SOFTWARE_LICENSE*/ ? obj.ObjectSoftwareModel : obj.ObjectType);
                if (obj.ObjectModel)
                    self.ObjectModel(obj.ObjectModel);
                //
                var test = self.ObjectModel();
                if (self.ObjectModel())
                    self.ObjectModelVisible(true);
                else
                    self.ObjectModelVisible(false);
                //
                if (obj.ObjectClassID == 223 && obj.ObjectSoftwareType)
                    self.ObjectSoftwareType(obj.ObjectSoftwareType);
                //
                if (obj.UtcDateMaintenanceStart) {
                    self.UtcDateMaintenanceStart(parseDate(obj.UtcDateMaintenanceStart, true));//show only date
                    self.UtcDateMaintenanceStartDT(new Date(parseInt(obj.UtcDateMaintenanceStart)));
                }
                if (obj.UtcDateMaintenanceEnd) {
                    self.UtcDateMaintenanceEnd(parseDate(obj.UtcDateMaintenanceEnd, true));//show only date
                    self.UtcDateMaintenanceEndDT(new Date(parseInt(obj.UtcDateMaintenanceEnd)));
                }
                else
                {
                    self.UtcDateMaintenanceEnd(parseDate(Date.now(), true));//show only date
                    self.UtcDateMaintenanceEndDT(new Date(parseInt(Date.now())));
                }
                //
                if (obj.OnMaintenance)
                    self.OnMaintenance(obj.OnMaintenance);
                //
                self.IdentifierCaption(obj.ObjectClassID == 223 /*OBJ_SOFTWARE_LICENSE*/ ? getTextResource('Maintenance_Name') : getTextResource('Identifier'));
                self.TypeCaption(obj.ObjectClassID == 223 /*OBJ_SOFTWARE_LICENSE*/ ? getTextResource('SoftwareModel') : getTextResource('ContractMaintenance_ObjectType'));
                self.ModelCaption(obj.ObjectClassID == 223 /*OBJ_SOFTWARE_LICENSE*/ ? getTextResource('Maintenance_LicenceModel') : getTextResource('Maintenance_EquipmentModel'));
                self.TypeModelCaption(obj.ObjectClassID == 223 /*OBJ_SOFTWARE_LICENSE*/ ? getTextResource('Maintenance_LicenceType') : '');
                self.TypeModelCaptionVisible(obj.ObjectClassID == 223? true:false);
            };
            //
            self.load = function (serviceContractID, objectID, objectClassID) {
                var retval = $.Deferred();
                //
                self.ID(objectID);
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/assetApi/GetContractMaintenance',
                    method: 'GET',
                    data: {
                        ServiceContractID: serviceContractID,
                        ObjectID: objectID,
                        ObjectClassID: objectClassID,
                    }
                },
                function (response) {
                    if (response.Result === 0 && response.ContractMaintenance) {
                        var obj = response.ContractMaintenance;
                        //               
                        self.loadFields(obj);
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmContractMaintenance.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
        }
    };
    return module;
});