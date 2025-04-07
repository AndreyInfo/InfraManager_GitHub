define(['knockout', 'ajax'], function (ko, ajax) {
    var module = {
        supplier: function () {
            var self = this;
            self.ajaxControl = new ajax.control();
            //            
            self.ID = ko.observable(null);
            self.ClassID = 116//OBJ_SUPPLIER
            self.Name = ko.observable('');
            self.Phone = ko.observable('');
            self.Email = ko.observable('');
            self.Address = ko.observable('');
            self.RegisteredAddress = ko.observable('');
            self.Notice = ko.observable('');
            self.INN = ko.observable('');
            self.KPP = ko.observable('');
            self.TypeList = ko.observableArray([]);
            //
            self.loadFields = function (obj) {
                if (obj.Name)
                    self.Name(obj.Name);
                if (obj.Phone)
                    self.Phone(obj.Phone);
                if (obj.Email)
                    self.Email(obj.Email);
                if (obj.Address)
                    self.Address(obj.Address);
                if (obj.RegisteredAddress)
                    self.RegisteredAddress(obj.RegisteredAddress);
                if (obj.Notice)
                    self.Notice(obj.Notice);
                if (obj.INN)
                    self.INN(obj.INN);
                if (obj.KPP)
                    self.KPP(obj.KPP);
                if (obj.TypeList)
                    self.TypeList(obj.TypeList);
            };
            //
            self.load = function (id) {
                var retval = $.Deferred();
                //
                if (!id) {
                    self.ajaxControl.Ajax(null,
                    {
                        url: '/assetApi/GetSupplierTypeList',
                        method: 'GET',
                    },
                    function (response) {
                        if (response.Result === 0 && response.TypeList) {
                            self.TypeList(response.TypeList);
                            retval.resolve(true);
                        }
                        else {
                            retval.resolve(false);
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSupplier.js, Load]', 'error');
                            });
                        }
                    });
                    //
                    return retval;
                }
                //
                self.ID(id);
                //
                self.ajaxControl.Ajax(null,
                {
                    url: '/assetApi/GetSupplier',
                    method: 'GET',
                    data: { ID: id }
                },
                function (response) {
                    if (response.Result === 0 && response.Supplier) {
                        var obj = response.Supplier;
                        //               
                        self.loadFields(obj);
                        //
                        retval.resolve(true);
                    }
                    else {
                        retval.resolve(false);
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[frmSupplier.js, Load]', 'error');
                        });
                    }
                });
                return retval;
            };
        },
    };
    return module;
});