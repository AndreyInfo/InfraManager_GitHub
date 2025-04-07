define(['knockout', 'ajax'], function (ko, ajax) {
    var module = {
        contactPerson: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            //            
            self.ID = ko.observable(null);
            self.ClassID = 384;//OBJ_SupplierContactPerson
            self.Name = ko.observable('');
            self.Surname = ko.observable('');
            self.Patronymic = ko.observable('');
            self.Phone = ko.observable('');
            self.SecondPhone = ko.observable('');
            self.Email = ko.observable('');
            self.PositionID = ko.observable('');
            self.PositionName = ko.observable('');
            self.SupplierID = ko.observable('');
            self.SupplierName = ko.observable('');
            self.Note = ko.observable('');
            //
            self.loadFields = function (obj) {
                if (obj.Name)
                    self.Name(obj.Name);
                if (obj.Surname)
                    self.Surname(obj.Surname);
                if (obj.Patronymic)
                    self.Patronymic(obj.Patronymic);
                if (obj.Phone)
                    self.Phone(obj.Phone);
                if (obj.SecondPhone)
                    self.SecondPhone(obj.SecondPhone);
                if (obj.Email)
                    self.Email(obj.Email);
                if (obj.PositionID)
                    self.PositionID(obj.PositionID);
                if (obj.PositionName)
                    self.PositionName(obj.PositionName);
                if (obj.SupplierID)
                    self.SupplierID(obj.SupplierID);
                if (obj.SupplierName)
                    self.SupplierName(obj.SupplierName);
                if (obj.Note)
                    self.Note(obj.Note);
            };
            //
            self.load = function (id, supplierID, supplierName) {
                var retval = $.Deferred();
                //
                if (!id)
                {
                    self.SupplierID(supplierID);
                    self.SupplierName(supplierName);
                    retval.resolve(true);
                    return retval;
                }
                //
                self.ID(id);
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/assetApi/GetSupplierContactPerson',
                    method: 'GET',
                    data: { ID: id }
                },
                function (response) {
                    if (response.Result === 0 && response.SupplierContactPerson) {
                        var obj = response.SupplierContactPerson;
                        //               
                        self.loadFields(obj);
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSupplierContactPerson.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
        },
    };
    return module;
});