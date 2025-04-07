define(['knockout', 'jquery', 'models/SDForms/SDForm.User', 'dateTimeControl'], function (ko, $, userLib, dtLib) {
    var module = {
        AssetFieldsRegistration: function () {
            var self = this;
            //
            self.ID = ko.observable('');
            self.ClassID = ko.observable('');
            //
            self.DateReceived = ko.observable(new Date());
            self.DateReceivedString = ko.pureComputed({
                read: function () {
                    return self.DateReceived() ? dtLib.Date2String(self.DateReceived(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.DateReceived(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.DateReceived(null);//value incorrect => reset value                    
                    else
                        self.DateReceived(dtLib.StringToDate(value));
                }
            });
            //
            self.Warranty = ko.observable(null);
            self.WarrantyString = ko.pureComputed({
                read: function () {
                    return self.Warranty() ? dtLib.Date2String(self.Warranty(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.Warranty(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.Warranty(null);//value incorrect => reset value                    
                    else
                        self.Warranty(dtLib.StringToDate(value));
                }
            });

            self.AppointmentDate = ko.observable(null);
            self.AppointmentDateString = ko.pureComputed({
                read: function () {
                    return self.AppointmentDate() ? dtLib.Date2String(self.AppointmentDate(), true) : '';
                },
                write: function (value) {
                    if (!value || value.length == 0)
                        self.AppointmentDate(null);//clear field => reset value
                    else if (dtLib.StringIsDate(value) != true)
                        self.AppointmentDate(null);//value incorrect => reset value                    
                    else
                        self.AppointmentDate(dtLib.StringToDate(value));
                }
            });
            //
            self.SupplierID = ko.observable('');
            self.SupplierName = ko.observable('');
            self.Cost = ko.observable(0);
            //
            self.Document = ko.observable('');
            //
            self.OwnerID = ko.observable('');
            self.OwnerClassID = ko.observable('');
            self.OwnerFullName = ko.observable('');
            //
            self.ServiceCenterID = ko.observable('');
            self.ServiceCenterName = ko.observable('');
            //
            self.ServiceContractID = ko.observable('');
            self.ServiceContractNumber = ko.observable('');
            //
            self.UserID = ko.observable('');
            self.UserFullName = ko.observable('');
            //
            self.Founding = ko.observable('');
            //
            self.UserFieldList = ko.observableArray([]);
            /*if (obj.UserFieldList) {
                ko.utils.arrayForEach(obj.UserFieldList, function (item) {
                    self.UserFieldList.push(
                    {
                        FieldNumber: item.FieldNumber,
                        FieldName: ko.observable(item.FieldName),
                        Value: ko.observable(item.Value),
                    });
                });
            }*/
            //
            //TODO: использовать parametersControl?
            /*self.UserField1 = ko.observable(obj.UserField1 ? obj.UserField1 : '');
            self.UserField2 = ko.observable(obj.UserField2 ? obj.UserField2 : '');
            self.UserField3 = ko.observable(obj.UserField3 ? obj.UserField3 : '');
            self.UserField4 = ko.observable(obj.UserField4 ? obj.UserField4 : '');
            self.UserField5 = ko.observable(obj.UserField5 ? obj.UserField5 : '');
            //
            self.UserField1Name = obj.UserField1Name;
            self.UserField2Name = obj.UserField2Name;
            self.UserField3Name = obj.UserField3Name;
            self.UserField4Name = obj.UserField4Name;
            self.UserField5Name = obj.UserField5Name;*/
        }
    };
    return module;
});
