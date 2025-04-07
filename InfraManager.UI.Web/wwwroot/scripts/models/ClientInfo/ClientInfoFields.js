define(['knockout', 'ajax'], function (ko, ajax) {
    var module = {
        client: function (options) {
            var self = this;
            self.$region = null;
            self.applyTemplate = null;
            self.options = options;
            self.clientLocationNewInfo = ko.observable(null);
            self.clientSubDivisionNewInfo = ko.observable(null);
            self.clientlocationChanged = ko.observable(false);
            self.clientSubDivisionChanged = ko.observable(false);

            if (options.fieldFriendlyName) {
                self.fieldFriendlyName = ko.observable(options.fieldFriendlyName);
            }
            else {
                self.fieldFriendlyName = ko.observable('');
            }
            
            self.NameField = getTextResource('UserFullName');
            self.FamilyField = getTextResource('Surname');
            self.PatronymicField = getTextResource('Patronymic');
            self.PhoneField = getTextResource('UserPhone');
            self.InPhoneField = getTextResource('InternalPhone');
            self.EmailField = getTextResource('UserEmail'); 
            self.SecondPhoneField = getTextResource('UserFax');
            self.PositionField = getTextResource('UserPosition');
            self.SubDivisionField = getTextResource('UserSubdivision');
            self.LocationField = getTextResource('UserRoom');


            if (options.placeholderName) {
                self.placeholderName = ko.observable(options.placeholderName);
            }
            else {
                self.placeholderName = ko.observable('');
            }
            
            self.ajaxControl = new ajax.control();
            
            self.saveClick = function (retval) {
                var textFamily = self.$region.find('.FamilyEditor');
                var family = textFamily.val();
                var textName = self.$region.find('.NameEditor');
                var name = textName.val();
                if (name == "" || family == "") {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('EnterFIO'));
                    });
                    retval.resolve(null);
                    return false;
                };
                var textPatronymic = self.$region.find('.PatronymicEditor');
                var patronymic = textPatronymic.val();
                var textPhoneEditor = self.$region.find('.PhoneEditor');
                var phone = textPhoneEditor.val();
                var textInPhoneEditor = self.$region.find('.InPhoneEditor');
                var inPhone = textInPhoneEditor.val();
                var textEmailEditor = self.$region.find('.EmailEditor');
                var emailEditor = textEmailEditor.val();
                var textSecondPhoneEditor = self.$region.find('.SecondPhoneEditor');
                var secondPhone = textSecondPhoneEditor.val();
                var contact =
                {
                    ID: self.options.ID,
                    Surname: family,
                    Name: name,
                    Email: emailEditor,
                    Patronymic: patronymic,
                    Phone: phone,
                    PhoneInternal: inPhone,
                    Fax: secondPhone,
                    PositionID: self.comboBoxSelectedValue().ID,
                    RowVersion: options.RowVersion,
                    ExternalID: options.ExternalID,
                    OrganizationID: options.OrganizationID,
                    RoomID: options.RoomID,
                    SubdivisionID: options.SubdivisionID,
                    WorkplaceID: options.WorkplaceID
                };
                
                if (self.clientlocationChanged()) {
                    var locationInfo = self.clientLocationNewInfo();
                    contact = Object.assign(contact, locationInfo);
                }
                
                if (self.clientSubDivisionChanged()) {
                    var subdivisionInfo = self.clientSubDivisionNewInfo();
                    contact = Object.assign(contact, subdivisionInfo);
                }
                return contact;
            }
            self.TemplateLoadedD = $.Deferred();
            //
            self.onLocationChanged = function (selectedWorkspaceData) {//когда новое местоположение будет выбрано
                if (!selectedWorkspaceData) {
                    return;
                }
                
                self.ajaxControl.Ajax(
                    self.$region.find('.clientRoomInfo'),
                    {
                        url: '/api/workplaces/' + selectedWorkspaceData.ID,
                        method: 'GET'
                    },
                    function (workplace) {
                        if (workplace) {
                            var textFieldRoom = self.$region.find('.text-field-room');
                            textFieldRoom.val(workplace.Room.Name + ' / ' + workplace.Name);
                            options.WorkplaceID = workplace.ID;
                            options.WorkplaceName = workplace.Name;

                            self.clientLocationNewInfo({
                                // RoomID: workplace.Room.ID,
                                WorkplaceID: workplace.ID,
                                RowVersion: workplace.RowVersion
                            });
                            self.clientlocationChanged(true);
                        }
                    }
                );

            };
            self.EditLocation = function () {
                showSpinner();
                //
                require(['models/ClientInfo/frmClientEditLocation', 'sweetAlert'], function (module) {
                    var saveLocation = function (objectInfo) {
                        if (!objectInfo) {
                            return;
                        }
                        self.onLocationChanged(objectInfo);
                    };
                    //
                    module.ShowDialog(options, saveLocation, true);
                });
            };
            //
            self.onSubDivisionChanged = function (selectedSubdivisionData) {
                if (!selectedSubdivisionData) {
                    return;
                }
                
                self.ajaxControl.Ajax(
                    self.$region.find('.clientDivisionInfo'),
                    {
                        url: '/api/subdivisions/' + selectedSubdivisionData.ID,
                        method: 'GET'
                    },
                    function (subdivision) {
                        if (subdivision) {
                            self.ajaxControl.Ajax(
                                self.$region.find('.clientDivisionInfo'),
                                {
                                    url: '/api/organizations/' + subdivision.OrganizationID,
                                    method: 'GET'
                                },
                                function (organization) {
                                    if (organization) {
                                        var textFieldRoom = self.$region.find('.text-field-Organization');
                                        textFieldRoom.val(subdivision.Name + ' / ' + organization.Name);
                                        options.SubdivisionName = subdivision.Name;
                                        options.SubdivisionID = subdivision.ID;
                                        self.clientSubDivisionNewInfo({
                                            SubdivisionID: subdivision.ID,
                                            // OrganizationID: subdivision.OrganizationID,
                                        });
                                        self.clientSubDivisionChanged(true);
                                    }
                                });
                        }
                    });
            };
            self.EditSubDivision = function () {
                showSpinner();
                //
                require(['models/ClientInfo/frmClientEditDivision', 'sweetAlert'], function (module) {
                    //
                    var saveLocation = function (objectInfo) {
                        if (!objectInfo)
                            return;
                        self.onSubDivisionChanged(objectInfo);
                    };
                    //
                    module.ShowDialog(options, saveLocation, true);
                });
            };
            //
            self.comboBoxSelectedValue = ko.observable(null);
            self.comboBoxValueList = ko.observableArray([]);
            self.getComboBoxValueList = function (options) {
                var data = self.comboBoxValueList();
                options.callback({ data: data, total: data.length });
            };
            self.createComboBoxValue = function (enumValue) {
                var thisObj = this;
                //
                thisObj.ID = enumValue.ID;
                thisObj.Name = enumValue.Name;
            };
            self.comboBoxControlD = $.Deferred();
            self.CreateComboBoxEditor = function () {
                var valueListD = $.Deferred();
                self.ajaxControl.Ajax(self.$region.find('.comboboxPosition'),
                    {
                        url: '/api/JobTitles',
                        method: 'GET'
                    },
                    function (response) {
                        if (response) {
                            self.comboBoxValueList.removeAll();
                            //
                            $.each(response, function (index, simpleEnum) {
                                var item = new self.createComboBoxValue(simpleEnum);
                                if (options.ClientPosition) {
                                    if (options.ClientPosition == item.Name)
                                        self.comboBoxSelectedValue(item);
                                }
                                //
                                self.comboBoxValueList().push(item);
                            });
                            //
                            $.when(self.TemplateLoadedD).done(function () {
                                if (options.readOnly === false) {
                                    var textBox = self.$region.find('.comboboxPosition');
                                    textBox[0].readOnly = false;
                                    if (options.maxLength)
                                        textBox[0].maxLength = options.maxLength;
                                    //
                                    if (options.oldValue)
                                        textBox.val(options.oldValue.Name);
                                }
                            });
                            //
                            self.comboBoxValueList.valueHasMutated();
                        }
                        valueListD.resolve();
                    });
                 //                
                $.when(self.TemplateLoadedD, valueListD).done(function () {
                    self.comboBoxControlD.resolve();
                });
                return self.comboBoxControlD.promise();
            };
            //
            self.GetWorkPlaceInfo = function () {
                var valueListD = $.Deferred();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/api/users/' + options.ID,
                        method: 'GET'
                    },
                    function (response) {
                        if (response) {
                            options.RowVersion = response.RowVersion;
                            options.RoomID = response.RoomID;
                            options.WorkplaceID = response.WorkplaceID;
							options.ExternalID = response.ExternalID;
                        }
                        valueListD.resolve();
                    });
                return valueListD.promise();
            };
            self.CreateWorkPlaceField = function () {
                var textFieldRoom = self.$region.find('.text-field-room');
                textFieldRoom[0].maxLength = 100;
                textFieldRoom.val(self.CreateFullName(self.options.RoomName, self.options.WorkplaceName));
                var eraserRoom = 'eraser_' + ko.getNewID();
                var parent = textFieldRoom.parent();
                parent.css('position', 'relative');
                parent.append('<div id="' + eraserRoom + '" class="search-client-eraser cursor-pointer" data-bind="click: $root.EraseRoomTextClick"/>');
                var eElem = document.getElementById(eraserRoom);
                ko.applyBindings(self, eElem);
            };

            self.CreateSubdivisionField = function () {
                var textFieldOrganization = self.$region.find('.text-field-Organization');
                textFieldOrganization[0].maxLength = 100;
                textFieldOrganization.val(self.CreateFullName(self.options.SubdivisionName, self.options.Organization));
                var eraserOrg = 'eraser_' + ko.getNewID();
                var parent = textFieldOrganization.parent();
                parent.css('position', 'relative');
                parent.append('<div id="' + eraserOrg + '" class="search-client-eraser cursor-pointer" data-bind="click: $root.EraseOrganizationTextClick"/>');
                var eElemOrg = document.getElementById(eraserOrg);
                ko.applyBindings(self, eElemOrg);
            };
            //
            self.AfterRender = function (elements) {
                self.TemplateLoadedD.resolve();
            };
            //
            self.EraseRoomTextClick = function () {
                self.clientLocationNewInfo(null);
                options.WorkplaceID = null;
                options.WorkplaceName = null;
                var textFieldRoom = self.$region.find('.text-field-room');
                textFieldRoom.val('');
                self.clientlocationChanged(true);
            };
            self.EraseOrganizationTextClick = function () {
                self.clientSubDivisionNewInfo(null);
                options.SubdivisionName = null;
                options.SubdivisionID = null;
                var textFieldOrganization = self.$region.find('.text-field-Organization');
                textFieldOrganization.val('');
                self.clientSubDivisionChanged(true);
            };
            //
            self.CreateFullName = function (FirstName, SecondName) {
                var FullName = '';
                if (FirstName && FirstName != 'Нет' && SecondName && SecondName != 'Нет') {
                    return FullName = FirstName + ' / ' + SecondName;
                }
                return FullName;
            };
            //
            self.textareaControlD = $.Deferred();
            self.loadFields = function () {
                var retval = $.Deferred();
                    //
                $.when(self.TemplateLoadedD, self.GetWorkPlaceInfo()).done(function () {
                //textBox
                    {
                        var textFamily = self.$region.find('.FamilyEditor');
                        textFamily.attr('placeholder', getTextResource('UserFamily'));
                        textFamily[0].maxLength = self.options.maxLength ? self.options.maxLength : 100;
                        textFamily.val(self.options.Family);
                        //
                        var textName = self.$region.find('.NameEditor');
                        textName[0].maxLength = self.options.maxLength ? self.options.maxLength : 100;
                        textName.val(self.options.Name);
                        //
                        var textPatronymic = self.$region.find('.PatronymicEditor');
                        textPatronymic[0].maxLength = self.options.maxLength ? self.options.maxLength : 100;
                        textPatronymic.val(self.options.Patronymic);
                        //
                        var textPhoneEditor = self.$region.find('.PhoneEditor');
                        textPhoneEditor[0].maxLength = self.options.maxLength ? self.options.maxLength : 100;
                        textPhoneEditor.val(self.options.Phone);
                        //
                        var textEmailEditor = self.$region.find('.EmailEditor');
                        textEmailEditor[0].maxLength = self.options.maxLength ? self.options.maxLength : 100;
                        textEmailEditor.val(self.options.Email);
                        //
                        var textInPhoneEditor = self.$region.find('.InPhoneEditor');
                        textInPhoneEditor[0].maxLength = self.options.maxLength ? self.options.maxLength : 15;
                        textInPhoneEditor.val(self.options.InPhone);
                        //
                        var textSecondPhoneEditor = self.$region.find('.SecondPhoneEditor');
                        //textarea.text(self.options.oldValue); ie problem + placeHolder ie10
                        textSecondPhoneEditor[0].maxLength = self.options.maxLength ? self.options.maxLength : 100;
                        textSecondPhoneEditor.val(self.options.SecondPhone); 
                        //
                    }
                    //
                    
                    self.CreateSubdivisionField();
                    self.CreateWorkPlaceField();
                    //ComboBox
                    $.when(self.CreateComboBoxEditor()).done(function () {
                        retval.resolve();
                    });
                    self.textareaControlD.resolve();
                    //
                });
               
                self.applyTemplate(self);
                    return self.textareaControlD.promise();
            };
        },
    };
    return module;
});