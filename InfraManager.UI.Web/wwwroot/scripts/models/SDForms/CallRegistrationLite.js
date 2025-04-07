define(['knockout', 'jquery', 'usualForms', 'ajax', 'dynamicOptionsService', 'comboBox'], function (ko, $, fhModule, ajaxLib, dynamicOptionsService, comboBoxLib) {
    var module = {
        LocationInfoResources: {
            22: { URI: '/api/workplaces/', Method: 'GET', },
            3: { URI: '/api/rooms/', Method: 'GET', },
        },
        ViewModel: function (mainRegionID, objectClassID, objectID) {
            var self = this;
            //
            //outer actions
            self.InvalidateHelpPanel = undefined;
            self.frm = undefined;
            //
            //vars
            self.MainRegionID = mainRegionID;
            self.ID = null;//for registered call
            self.renderedD = $.Deferred();
            //
            self.ObjectClassID = objectClassID;//объект, с которым связана заявка
            self.ObjectID = objectID;//объект, с которым связана заявка
            self.formClassID = 701;//OBJ_CALL
            // urgency
            self.SelectedUrgency = ko.observable(null);
            self.ajaxControl_UrgencyList = new ajaxLib.control();
            self.UrgencyList = ko.observableArray([]);
            self.getUrgencyList = function (options) {
                var data = self.UrgencyList();
                options.callback({ data: data, total: data.length });
            };
            var createUrgency = function (simpleDictionary) {
                var thisObj = this;
                //
                thisObj.ID = simpleDictionary.ID;
                thisObj.Name = simpleDictionary.Name;
                //
                thisObj.UrgencyClick = function () {
                    self.SelectedUrgency(thisObj);
                };
            };
            self.UrgencyListD = $.Deferred();//для выбора значения из загруженного справочника
            self.LoadUrgencyList = function () {
                if (self.UrgencyList().length > 0)
                    return;
                //
                self.ajaxControl_UrgencyList.Ajax($('#' + self.MainRegionID).find('.urgency'),
                    {
                        url: '/api/Urgencies',
                        method: 'GET'
                    },
                    function (response) {
                        if (response) {
                            self.UrgencyList.removeAll();
                            //
                            $.each(response, function (index, simpleDictionary) {
                                var u = new createUrgency(simpleDictionary);
                                self.UrgencyList().push(u);
                            });
                            self.UrgencyList.valueHasMutated();
                            if (self.UrgencyList().length > 0)
                                self.SelectedUrgency(self.UrgencyList()[0]);
                        }
                        self.resizePage();
                        self.UrgencyListD.resolve();
                    });
            };

            self.BeforeSave = ko.observable(true);//была ли сделана попытка сохранения?           
            //  
            self.ShowPlaceOfServiceD = $.Deferred();
            self.ShowPlaceOfService = ko.observable(false);


            //callSummary
            {
                self.CallSummaryName = ko.observable('');
                self.callSummarySearcher = null;
                self.callSummarySearcherD = $.Deferred();
                self.ajaxControl_ServiceItemAttendance = new ajaxLib.control();
                self.InitializeCallSummarySearcher = function () {
                    var fh = new fhModule.formHelper();
                    var callSummaryD = fh.SetTextSearcherToField(
                        $('#' + self.MainRegionID).find('.callSummarySearcher'),
                        'CallSummarySearcher',
                        null,
                        [self.ServiceItemID ? self.ServiceItemID : (self.ServiceAttendanceID ? self.ServiceAttendanceID : undefined),
                        self.CallTypeID ? self.CallTypeID : undefined],
                        function (objectInfo) {//select
                            self.CallSummaryName(objectInfo.FullName);
                            //         
                            var clientID = self.ClientID();
                            self.ajaxControl_ServiceItemAttendance.Ajax($('#' + self.MainRegionID).find('.serviceItemAttendance'),
                                {
                                    url: '/api/serviceitems/callSummary/' + objectInfo.ID,
                                    method: 'GET',
                                },
                                function (response) {
                                    if (response)
                                        self.ServiceSet(response.ServiceID, response.ServiceItemID, response.ServiceAttendanceID, response.FullName, response.Parameter, null,response.Summary);
                                    self.resizePage();
                                });
                        },
                        function () {//reset
                            self.CallSummaryName('');
                        });
                    $.when(callSummaryD).done(function (ctrl) {
                        self.callSummarySearcher = ctrl;
                        self.callSummarySearcherD.resolve(ctrl);
                        ctrl.CurrentUserID = self.ClientID();
                        //
                        ctrl.LoadD.done(function () {
                            $('#' + self.MainRegionID).find('.callSummarySearcher').focus();
                        });
                    });
                };
                //
                self.EditCallSummaryName = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Call.CallSummary',
                        fieldFriendlyName: getTextResource('CallSummary'),
                        oldValue: self.CallSummaryName().length > 0 ? { ID: null, ClassID: 132, FullName: self.CallSummaryName() } : null, //OBJ_CallSummary = 132
                        searcherName: 'CallSummarySearcher',
                        searcherPlaceholder: getTextResource('PromptCallSummary'),
                        searcherParams: [
                            self.ServiceItemID ? self.ServiceItemID : (self.ServiceAttendanceID ? self.ServiceAttendanceID : undefined),
                            self.CallTypeID ? self.CallTypeID : undefined],
                        allowAnyText: true,
                        onSave: function (objectInfo) {
                            self.CallSummaryName(objectInfo ? objectInfo.FullName : '');
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }
            //
            //description
            {
                self.htmlDescriptionControlD = $.Deferred();
                self.IsDescriptionContainerVisible = ko.observable(true);
                self.ToggleDescriptionContainer = function () {
                    self.IsDescriptionContainerVisible(!self.IsDescriptionContainerVisible());
                };
                //
                self.isUserChangedDescription = false;
                self.htmlDescriptionControl = null;
                self.InitializeDescription = function () {
                    var htmlElement = $('#' + self.MainRegionID).find('.description');
                    if (self.htmlDescriptionControl == null)
                        require(['htmlControl'], function (htmlLib) {
                            self.htmlDescriptionControl = new htmlLib.control(htmlElement);
                            self.htmlDescriptionControl.SetHTML(self.Description());
                            self.htmlDescriptionControlD.resolve();
                            self.htmlDescriptionControl.OnHTMLChanged = function (htmlValue) {
                                if (self.Description() != htmlValue)
                                    self.isUserChangedDescription = true;
                                self.Description(htmlValue);
                            };
                        });
                    else
                        self.htmlDescriptionControl.Load(htmlElement);
                };
                //
                self.Description = ko.observable('');
                self.Description.subscribe(function (newValue) {
                    if (self.htmlDescriptionControl != null)
                        self.htmlDescriptionControl.SetHTML(newValue);
                });
                //
                self.EditDescription = function () {
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Call.Description',
                        fieldFriendlyName: getTextResource('Description'),
                        oldValue: self.Description(),
                        onSave: function (newHTML) {
                            self.Description(newHTML);
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.htmlEdit, options);
                    //
                };
            }
            //
            //attachments
            {
                self.attachmentsControl = null;
                self.LoadAttachmentsControl = function () {
                    require(['fileControl'], function (fcLib) {
                        if (self.attachmentsControl != null)
                            self.attachmentsControl.RemoveUploadedFiles();//previous object                   
                        //
                        if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                            var attachmentsElement = $('#' + self.MainRegionID).find('.documentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.call-registration-lite', '.icon.call-lite-icon-plus.active');
                        }
                        self.attachmentsControl.ReadOnly(false);
                    });
                };
            }
            //
            //solution
            {
                self.Solution = ko.observable('');
            }
            //
            //service
            {
                self.ServiceID = null;
                self.ServiceItemID = null;
                self.ServiceAttendanceID = null;
                self.ServiceItemAttendanceFullName = ko.observable('');
                self.SiaName = ko.observable('');
                self.SiaPath = ko.observable('');
                //
                self.ServiceItemAttendanceFullName.subscribe(function (newValue) {
                    var arr = newValue.split(' \\ ');
                    var arrStr = '';
                    var tempSiaPath = '';
                    for (var i = 0; i < arr.length; i++) {
                        if (i == arr.length - 1) {
                            self.SiaName(arr[i]);
                            self.SiaPath(tempSiaPath);
                            break;
                        }
                        else if (i != 0) {
                            tempSiaPath += ' > ';
                        }
                        tempSiaPath += arr[i] + '';
                        //
                        arrStr += arr[i] + '';
                    }
                    //
                    if (arrStr.length === 0) {
                        self.SiaName('[Затрудняюсь выбрать]');
                        self.SiaPath('');
                    }
                });
                //
                self.ServiceClear = function () {
                    self.ServiceItemAttendanceFullName('');
                    //
                    self.ServiceID = null;
                    self.ServiceItemID = null;
                    self.ServiceAttendanceID = null;
                    //
                    self.DynamicOptionsService.ResetData();
                    //
                    $.when(self.callSummarySearcherD).done(function () {
                        self.callSummarySearcher.SetSearchParameters([self.ServiceItemID ? self.ServiceItemID : (self.ServiceAttendanceID ? self.ServiceAttendanceID : undefined), self.CallTypeID ? self.CallTypeID : undefined]);
                    });
                };
                //
                self.ServiceSet = function (serviceID, serviceItemID, serviceAttendanceID, fullName, serviceParameter, setSearcher, summaryName) {
                    self.ServiceItemAttendanceFullName(fullName);
                    if (setSearcher != false) {
                        $.when(self.serviceItemAttendanceSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(serviceItemID == null ? serviceAttendanceID : serviceItemID, serviceItemID == null ? 407 : 406, fullName, '');
                        });
                    }
                    //
                    self.ServiceID = serviceID;
                    self.ServiceItemID = serviceItemID;
                    self.ServiceAttendanceID = serviceAttendanceID;
                    //
                    if (self.isUserChangedDescription === false && !!serviceParameter) {
                        var html = '<html><body><p style="white-space:pre-wrap">' + serviceParameter.replace(/</g, '&lt;').replace(/>/g, '&gt;') + '</p></body></html>';
                        self.Description(html);
                    }
                    //
                    self.DynamicOptionsServiceInit();
                    //
                    $.when(self.callSummarySearcherD).done(function () {
                        self.callSummarySearcher.SetSearchParameters( { "CallTypeID" : self.SelectedCallType().ID });
                    });

                    if (summaryName && summaryName != '' && !self.CallSummaryName().length > 0)
                        self.CallSummaryName(summaryName);
                    self.SummaryDefaultName(summaryName ? summaryName:'');
                };
                self.SummaryDefaultName = ko.observable('');
                //
                self.serviceItemAttendanceSearcher = null;
                self.serviceItemAttendanceSearcherD = $.Deferred();
                self.InitializeServiceSearcher = function () {
                    var fh = new fhModule.formHelper();
                    //
                    var serviceSearcherExtensionLoadedD = $.Deferred();
                    var serviceD = fh.SetTextSearcherToField(
                        $('#' + self.MainRegionID).find('.serviceItemAttendance'),
                        'ServiceItemAndAttendanceSearcher',
                        'SearchControl/SearchServiceItemAttendanceControl',
                        [self.CallTypeID ? self.CallTypeID : undefined],
                        function (objectInfo) {//select        
                            var clientID = self.ClientID();
                            //
                            self.ajaxControl_ServiceItemAttendance.Ajax($('#' + self.MainRegionID).find('.serviceItemAttendance'),
                                {
                                    url: '/api/serviceitems/' + objectInfo.ID,
                                    method: 'GET'
                                },
                                function (response) {
                                    if (response)
                                        self.ServiceSet(response.ServiceID, response.ServiceItemID, response.ServiceAttendanceID, response.FullName, response.Parameter, false, response.Summary);
                                    self.resizePage();
                                });
                        },
                        function () {//reset
                            self.ServiceClear();
                        },
                        function (selectedItem) {//close
                            if (!selectedItem)
                                self.ServiceClear();
                        },
                        serviceSearcherExtensionLoadedD);
                    $.when(serviceD).done(function (vm) {//after load searcher
                        self.serviceItemAttendanceSearcher = vm;
                        self.serviceItemAttendanceSearcherD.resolve(vm);
                        vm.CurrentUserID = self.ClientID();
                        //
                        vm.SelectFromServiceCatalogueClick = function () {
                            vm.Close();//close dropDownMenu
                            if (self.CallTypeID == null)
                                return;
                            var mode = fh.ServiceCatalogueBrowserMode.Default;
                            if (self.CallTypeID != '00000000-0000-0000-0000-000000000000') {//default callType
                                if (self.IsServiceAttendance == true)
                                    mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceAttendances;
                                else
                                    mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceItems;
                            }
                            var clientID = self.ClientID();
                            var result = fh.ShowServiceCatalogueBrowser(mode, clientID, self.ServiceID);
                            $.when(result).done(function (serviceItemAttendance) {
                                if (serviceItemAttendance)
                                    self.ServiceSet(serviceItemAttendance.Service.ID, serviceItemAttendance.ClassID == 406 ? serviceItemAttendance.ID : null, serviceItemAttendance.ClassID == 407 ? serviceItemAttendance.ID : null, serviceItemAttendance.FullName(), serviceItemAttendance.Parameter, null, response.Summary);
                            });
                        };
                        //
                        vm.HardToChooseClick = function () { };
                        vm.HardToChooseClickVisible = ko.observable(false);
                        //      
                        serviceSearcherExtensionLoadedD.resolve();
                        $.when(vm.LoadD).done(function () {
                            $('#' + vm.searcherDivID).find('.ui-dialog-buttonset').css({ opacity: 1 });
                        });
                    });
                };
                //
                self.EditServiceItemAttendance = function () {
                    var getObjectInfo = function () {
                        if (self.ServiceItemID)
                            return { ID: self.ServiceItemID, ClassID: 406, FullName: self.ServiceItemAttendanceFullName() };
                        else if (self.ServiceAttendanceID)
                            return { ID: self.ServiceAttendanceID, ClassID: 407, FullName: self.ServiceItemAttendanceFullName() };
                        else
                            return null;
                    };
                    var extensionLoadD = $.Deferred();
                    //
                    showSpinner();
                    var fh = new fhModule.formHelper(true);
                    var options = {
                        fieldName: 'Call.ServiceItemAttendance',//одновременно оба поля
                        fieldFriendlyName: getTextResource('CallServiceItemOrAttendance'),
                        oldValue: getObjectInfo(),
                        searcherName: 'ServiceItemAndAttendanceSearcher',
                        searcherTemplateName: 'SearchControl/SearchServiceItemAttendanceControl',//иной шаблон, дополненительные кнопки
                        searcherParams: [self.CallTypeID ? self.CallTypeID : undefined],//параметры искалки - тип заявки, для фильтрации - только элементы / только услуги
                        searcherLoadD: extensionLoadD,//ожидание дополнений для модели искалки
                        searcherCurrentUser: self.ClientID(),
                        searcherPlaceholder: getTextResource('PromptServiceItemAttendance'),
                        searcherLoad: function (vm, setObjectInfoFunc) {//дополнения искалки
                            vm.CurrentUserID = null;//для фильтрации данных по доступности их пользователю (клиенту)
                            vm.SelectFromServiceCatalogueClick = function () {//кнопка выбора из каталога сервисов
                                vm.Close();//close dropDownMenu
                                if (self.CallTypeID == null)
                                    return;
                                var mode = fh.ServiceCatalogueBrowserMode.Default;
                                if (self.CallTypeID != '00000000-0000-0000-0000-000000000000') {//default callType
                                    if (self.IsServiceAttendance == true)
                                        mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceAttendances;
                                    else
                                        mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceItems;
                                }
                                var clientID = self.ClientID();//показываем, что есть недоступное этому клиенту, но позволяем выбрать
                                showSpinner();
                                var result = fh.ShowServiceCatalogueBrowser(mode, clientID, self.ServiceID);
                                $.when(result).done(function (serviceItemAttendance) {
                                    if (serviceItemAttendance && setObjectInfoFunc)
                                        setObjectInfoFunc({
                                            ID: serviceItemAttendance.ID,
                                            ClassID: serviceItemAttendance.ClassID,
                                            FullName: serviceItemAttendance.FullName(),
                                            Details: null
                                        });
                                });
                            };
                            vm.HardToChooseClick = function () { };//кнопка затрудняюсь выбрать
                            vm.HardToChooseClickVisible = ko.observable(false);
                            //
                            extensionLoadD.resolve();//можно рендерить искалку, формирование модели окончено
                            $.when(vm.LoadD).done(function () {
                                $('#' + vm.searcherDivID).find('.ui-dialog-buttonset').css({ opacity: 1 });//show buttons                                
                            });
                        },
                        onSave: function (objectInfo) {
                            if (!objectInfo)
                                return;
                            var clientID = self.ClientID();
                            self.ajaxControl_ServiceItemAttendance.Ajax($('#' + self.MainRegionID).find('.serviceItemAttendance'),
                                {
                                    url: '/api/serviceitems/' + objectInfo.ID,
                                    method: 'GET'
                                },
                                function (response) {
                                    if (response)
                                        self.ServiceSet(response.ServiceID, response.ServiceItemID, response.ServiceAttendanceID, response.FullName, response.Parameter, false, response.Summary);
                                    self.resizePage();
                                });
                        },
                        nosave: true
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                };
            }
            //
            //for all user controls
            {
                self.CanEdit = ko.computed(function () {
                    return true;
                });
            }
            //
            //client
            {
                self.ClientLoaded = ko.observable(false);
                self.ClientID = ko.observable(null);
                self.ClientID.subscribe(function (newValue) {
                    var clientID = newValue;
                    //
                    if (self.parametersControl != null)
                        self.parametersControl.ClientID(clientID);
                    //
                    if (self.callSummarySearcher != null) {
                        self.callSummarySearcher.CurrentUserID = clientID;
                        self.callSummarySearcher.ClearValues();
                    }
                });
                self.Client = ko.observable(null);
                //
                self.InitializeClient = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        if (self.ClientLoaded() == false) {
                            if (self.ClientID()) {
                                const options = {
                                    UserID: self.ClientID(),
                                    UserType: userLib.UserTypes.client,
                                    UserName: null,
                                    EditAction: self.EditClient,
                                    RemoveAction: self.DeleteClient
                                };

                                const user = new userLib.User(self, options);
                                self.Client(user);

                                $.when(self.GetWorkPlaceInfo(options.UserID)).done(function () {
                                    self.ClientLoaded(true);
                                    self.DynamicOptionsServiceInit();
                                });
                            }
                            else {
                                self.Client(new userLib.EmptyUser(self, userLib.UserTypes.client, self.EditClient));
                                self.DynamicOptionsServiceInit();
                            }
                            //
                            setTimeout(self.resizePage, 1000);
                        }
                    });
                };
                self.EditClient = function () {
                    showSpinner();
                    $.when(userD).done(function (user) {
                        require(['models/SDForms/SDForm.User'], function (userLib) {
                            var fh = new fhModule.formHelper(true);
                            var options = {
                                fieldName: 'Call.Client',
                                fieldFriendlyName: getTextResource('Client'),
                                oldValue: self.ClientLoaded() ? { ID: self.Client().ID(), ClassID: 9, FullName: self.Client().FullName() } : null,
                                object: ko.toJS(self.Client()),
                                searcherName: 'WebUserSearcherNoTOZ',
                                searcherPlaceholder: getTextResource('EnterFIO'),
                                searcherCurrentUser: user.UserID, //чтобы видеть только относительно подразделений этого пользователя
                                onSave: function (objectInfo) {
                                    self.ClientLoaded(false);
                                    self.Client(new userLib.EmptyUser(self, userLib.UserTypes.client, self.EditClient));
                                    self.ClientID(objectInfo ? objectInfo.ID : null);
                                    //
                                    self.InitializeClient();
                                },
                                nosave: true
                            };
                            fh.ShowSDEditor(fh.SDEditorTemplateModes.searcherEdit, options);
                        });
                    });
                };
                self.DeleteClient = function () {
                    require(['models/SDForms/SDForm.User'], function (userLib) {
                        self.ClientLoaded(false);
                        self.ClientID(null);
                        self.Client(new userLib.EmptyUser(self, userLib.UserTypes.client, self.EditClient));
                    });
                };
            }
            //
            //parameters
            {
                self.DynamicOptionsService = new dynamicOptionsService.ViewModel(self.MainRegionID, {
                    typeForm: 'Create',
                    typesUserForm: 'User',
                    isClientMode: true
                });

                self.DynamicOptionsServiceInit = function () {
                    const serviceItemID = self.ServiceItemID;
                    const serviceAttendanceID = self.ServiceAttendanceID;

                    if (!serviceItemID && !serviceAttendanceID) {
                        return;
                    };

                    if (self.DynamicOptionsService.IsInit()) {
                        self.DynamicOptionsService.ResetData();
                    };

                    self.DynamicOptionsService.FormId = self.MainRegionID;
                    
                    const selectUser = self.Client();
                    const isSelectUserNotEmpty = selectUser && selectUser.hasOwnProperty('ID');

                    const dependences = {};
                    if (isSelectUserNotEmpty) {
                        dependences.User = {};
                        dependences.User.OrganizationID = selectUser.OrganizationID();
                        dependences.User.SubdivisionID = selectUser.SubdivisionID();
                    };

                    if (serviceItemID) {
                        self.DynamicOptionsService.GetTemplate(dynamicOptionsService.Classes.ServiceItem, serviceItemID, null, dependences);
                    } else {
                        self.DynamicOptionsService.GetTemplate(dynamicOptionsService.Classes.ServiceAttendance, serviceAttendanceID, null, dependences);
                    };
                };
            };

            //
            $.when(self.UrgencyListD).done(function () {
                $.each(self.UrgencyList(), function (index, urgency) {
                    if (urgency.ID == self.UrgencyID) //exists
                        self.SelectedUrgency(urgency);
                });
            });
            //
            self.Load = function () {
                $.when(self.renderedD).done(function () {
                    if (self.AddAs || self.AddFromServiceCatalogue)
                        self.Fill(self.callData);
                    //
                    //TODO: not for production self.InitializeCallSummarySearcher();
                    //
                    self.InitializeDescription();
                    self.LoadAttachmentsControl();
                    //                    
                    self.InitializeServiceSearcher();
                    self.InitializeClient();
                    self.LoadUrgencyList();
                    //
                    $.when(self.renderedD).done(function () {
                        var ajaxControl = new ajaxLib.control();
                        ajaxControl.Ajax(null,
                            {
                                url: '/api/settings/CallHidePlaceOfService',
                                method: 'GET'
                            },
                            function (response) {
                                var show = false;
                                if (response) show = !response.Value;                                
                                self.ShowPlaceOfService(show);
                            });
                    });
                });
            };
            //
            self.AfterRender = function () {
                self.renderedD.resolve();
                //
                self.resizePage();
                $(window).resize(self.resizePage);
            };
            //
            self.OnCloseAction = undefined;//set in fh in fullScreen
            self.forceClose = false;
            //
            self.BeforeClose = function () {
                if (self.forceClose) {
                    if (self.attachmentControl != null)
                        self.attachmentControl.RemoveUploadedFiles();
                    if (self.parametersControl != null)
                        self.parametersControl.DestroyControls();
                    return true;
                }
                //
                require(['sweetAlert'], function () {
                    swal({
                        title: getTextResource('FormClosing'),
                        text: getTextResource('FormClosingQuestion'),
                        showCancelButton: true,
                        closeOnConfirm: true,
                        closeOnCancel: true,
                        confirmButtonText: getTextResource('ButtonOK'),
                        cancelButtonText: getTextResource('ButtonCancel')
                    },
                        function (value) {
                            if (value == true) {
                                self.forceClose = true;
                                self.Close();
                            }
                        });
                });
                return false;
            };
            //
            self.Close = function () {//if change name - check fh
                if (!self.forceClose) {
                    self.BeforeClose();
                    return;
                }
                //
                var div = $('#' + self.MainRegionID);
                if (div.length == 1) {
                    ko.cleanNode(div[0]);
                    $('.call-registration-lite').remove();
                    //
                    if (self.OnCloseAction)
                        self.OnCloseAction();
                }
            };
            //  
            self.IsCallActive = ko.observable(true);
            //
            self.resizePage = function () {
                var new_height = getPageContentHeight();
                var left = $(".sc-views").width() + 1;
                var width = $("#mainMenu").width();
                $(".call-registration-lite").css("top", "0px");
                $(".call-registration-lite").css("left", left + "px");
                $(".call-registration-lite").css("height", new_height + "px");
                $(".call-registration-lite").css("width", width - left + "px");
                //
                var height = $('.call-registration-lite-header').outerHeight();
                $(".call-registration-lite-main").css("top", height + 1 + "px");
            };
            //                                         
            self.Validate = function () {
                self.ValidateAndRegisterCall(true);
            };
            //

            self.ajaxControl_RegisterCall = new ajaxLib.control();
            self.ValidateAndRegisterCall = function (showSuccessMessage, kbArticleID) {
                var retval = $.Deferred();
                //
                var data = {
                    'UserID': self.ClientID(),
                    'CallTypeID': self.CallTypeID,
                    'UrgencyID': self.SelectedUrgency().ID,
                    'CallSummaryName': self.CallSummaryName(),
                    'Description': self.Description(),
                    'ServiceItemAttendanceID': self.ServiceItemID != null ? self.ServiceItemID : self.ServiceAttendanceID,
                    'Files': self.attachmentsControl == null ? null : self.attachmentsControl.GetData(),
                    'KBArticleID': kbArticleID != null ? kbArticleID : null,
                    'FormValuesData': self.DynamicOptionsService.SendByServer(),
                    //
                    'PriorityID': self.PriorityID,
                    'InfluenceID': self.InfluenceID,
                    'HTMLSolution': self.Solution(),
                    'RFCResultID': null,
                    'IncidentResultID': null,
                    'ReceiptType': 2,
                    'ClientID': self.ClientID(),
                    'InitiatorID': self.InitiatorID,
                    'OwnerID': self.OwnerID,
                    'ExecutorID': self.ExecutorID,
                    'QueueID': self.QueueID,
                    'AccomplisherID': self.AccomplisherID,
                    'ObjectClassID': self.ObjectClassID,
                    'ObjectID': self.ObjectID,
                    'CreatedByClient': true,
                    //                                     
                    'ServicePlaceID': self.ServicePlaceCallIMObjID(),
                    'ServicePlaceClassID': self.ServicePlaceCallClassID()
                    //
                };
                //    

                var validationSummary = !data.CallSummaryName || data.CallSummaryName.trim().length == 0;
                if (validationSummary)
                    data.CallSummaryName = self.SummaryDefaultName();
                //
                var validationError = data.CallTypeID == null || (!data.CallSummaryName || data.CallSummaryName.trim().length == 0) || data.UserID == null;

                if (validationError) {
                    self.BeforeSave(false);
                    //
                    if (validationError)
                        require(['sweetAlert'], function (swal) {
                            swal('Заданы не все обязательные поля');//DIMA - resources!
                        });

                    retval.resolve(null);
                    return;
                };

                if (data.FormValuesData === null && self.DynamicOptionsService.IsInit()) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('ParametersNotLoaded'));
                    });
                    retval.resolve(null);
                    return;
                };

                if (data.FormValuesData && data.FormValuesData.hasOwnProperty('valid')) {
                    data.FormValuesData.callBack();
                    retval.resolve(null);
                    return;
                };
                //
                var registerAttachmentsCallback = function (response) {
                    var URL = "/api/DocumentReferences/" + self.formClassID + "/" + response.ID + "/documents";
                    self.ajaxControl_RegisterCall.Ajax(null,
                        {
                            url: URL,
                            method: "Post",
                            dataType: "json",
                            data: { 'docID': self.attachmentsControl.Items().map(function (item) { return item.ID; }) }
                        },
                        function () {
                            hideSpinner();
                            if (showSuccessMessage) {
                                require(['sweetAlert'], function () {
                                    swal({
                                        title: getTextResource('CallRegisteredMessage').replace('{0}', response.Number),
                                        showCancelButton: true,
                                        closeOnConfirm: true,
                                        closeOnCancel: true,
                                        confirmButtonText: getTextResource('NavigateToCallClientList'),
                                        cancelButtonText: getTextResource('Continue')
                                    },
                                        function (value) {
                                            if (value == true)
                                                self.NavigateToClientCallList();
                                            else {
                                                self.forceClose = true;
                                                if (self.attachmentControl != null)
                                                    self.attachmentControl.RemoveUploadedFiles();
                                                if (self.parametersControl != null)
                                                    self.parametersControl.DestroyControls();
                                                self.Close();
                                            }
                                        });
                                });
                            }
                            retval.resolve({ Id: response.ID, CallID: response.ID });
                        },
                        function () {
                            hideSpinner();
                            require(['sweetAlert'], function () {
                                swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[CallRegistrationLite.js, RegisterCall]', 'error');
                            });
                            retval.resolve(null);
                            self.IsRegisteringCall = false;
                        });
                };
                //
                showSpinner();
                self.ajaxControl_RegisterCall.Ajax(null,
                    {
                        url: '/api/calls',
                        method: 'POST',
                        dataType: 'json',
                        contentType: 'application/json',
                        data: JSON.stringify(data)
                    },
                    ((response) => registerAttachmentsCallback(response)),
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[CallRegistrationLite.js, RegisterCall]', 'error');
                        });
                        retval.resolve(null);
                    });
                //
                return retval.promise();
            };
            //
            self.NavigateToClientCallList = function () {
                $.when(userD).done(function () {
                    setLocation('SD/Table');
                });
            };
            //
            //
            self.ServicePlaceCallID = ko.observable(null);
            self.ServicePlaceCallIMObjID = ko.observable(null);
            self.ServicePlaceCallClassID = ko.observable(null);
            self.ServicePlaceCallName = ko.observable('');
            self.ajaxControl_ClientInfo = new ajaxLib.control();
            /**
             * @param {{ WorkplaceID: number, WorkplaceIMObjID: GUID, WorkplaceFullName: string }} userData user data response
             */
            self.onLocationChangedFromUserData = function (userData) {
                if (!userData) {
                    return;
                }

                self.ServicePlaceCallID(userData.WorkplaceID);
                self.ServicePlaceCallIMObjID(userData.WorkplaceIMObjID);
                self.ServicePlaceCallClassID(22); //Класс "Рабочее место" (Workplace)
                self.ServicePlaceCallName(userData.WorkplaceFullName);
                self.ServicePlaceCallName.valueHasMutated();
                self.resizePage();

            };
            /**
             * @param {{ ID: number, ClassID: GUID, SelectedTreePathNames: array<string> }} selectedWorkspaceData data returned from dialog
             */
            self.onLocationChangedFromDialog = function (selectedWorkspaceData) {
                if (!selectedWorkspaceData) {
                    return;
                }

                if(!module.LocationInfoResources[selectedWorkspaceData.ClassID]) {
                    return;
                }

                var servicePlaceCallName = selectedWorkspaceData.SelectedTreePathNames
                    .reverse()
                    .slice(1) // пропускаем Owner
                    .join(" \\ ");
                self.ServicePlaceCallID(selectedWorkspaceData.ID);
                self.ServicePlaceCallClassID(selectedWorkspaceData.ClassID);
                self.ServicePlaceCallName(servicePlaceCallName);

                self.ajaxControl.Ajax(
                    null,
                    {
                        url: module.LocationInfoResources[selectedWorkspaceData.ClassID].URI + selectedWorkspaceData.ID,
                        method: module.LocationInfoResources[selectedWorkspaceData.ClassID].Method,
                    },
                    function (response) {
                        if (response) {
                            self.ServicePlaceCallIMObjID(response.IMObjID);
                            self.ServicePlaceCallName.valueHasMutated();
                            self.resizePage();
                        }
                    }
                );
            };
            //
            self.EditLocation = function () {
                showSpinner();
                //
                require(['models/ClientInfo/frmClientEditLocation', 'sweetAlert'], function (module) {
                    let options = {
                        ServicePlaceID: self.ServicePlaceCallID(),
                        ServicePlaceClassID: self.ServicePlaceCallClassID(),
                    };

                    var saveLocation = function (objectInfo) {
                        if (!objectInfo) {
                            return;
                        }
                        self.onLocationChangedFromDialog(objectInfo);
                    };

                    module.ShowDialog(options, saveLocation, true);
                });
            };
            //
            self.ajaxControl = new ajaxLib.control();
            self.GetWorkPlaceInfo = function (id) {
                var valueListD = $.Deferred();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/api/users/' + id,
                        method: 'GET'
                    },
                    function (response) {
                        if (response) {
                            self.onLocationChangedFromUserData(response);
                        }
                        self.resizePage();
                        valueListD.resolve();
                    });
                return valueListD.promise();
            };
            self.AddAs = false;//заявка создана по аналогии
            self.CallData = null;//заявка, взятая за основу
            self.primaryCallID = null;//ID заявки, взятой за основу
            //
            self.AddFromServiceCatalogue = false;//заявка создана из каталога сервисов
            self.CallTypeID = null;
            self.CallTypeName = ko.observable('');
            self.ImageSource = ko.observable(null);
            self.IsServiceAttendance = false;
            //
            self.Fill = function (callData) {
                if (callData.ID()) {
                    self.primaryObjectID = callData.ID();
                }
                //
                self.CallData = callData;
                if (callData.InfluenceID())
                    self.InfluenceID = callData.InfluenceID();
                //
                if (callData.PriorityID())
                    self.PriorityID = callData.PriorityID();
                if (callData.PriorityColor())
                    self.PriorityColor = callData.PriorityColor();
                if (callData.PriorityName())
                    self.PriorityName = callData.PriorityName();
                //
                if (callData.CallSummaryName())
                    self.CallSummaryName(callData.CallSummaryName());
                if (callData.UrgencyID())
                    self.UrgencyID = callData.UrgencyID();
                //
                if (callData.CallTypeID())
                    self.CallTypeID = callData.CallTypeID();
                if (callData.CallType())
                    self.CallTypeName(callData.CallType());
                //
                if (callData.ClientID())
                    self.ClientID(callData.ClientID());
                if (callData.ExecutorID())
                    self.ExecutorID = callData.ExecutorID();
                if (callData.InitiatorID())
                    self.InitiatorID = callData.InitiatorID();
                if (callData.OwnerID())
                    self.OwnerID = callData.OwnerID();
                if (callData.QueueID())
                    self.QueueID = callData.QueueID();
                if (callData.QueueName())
                    self.QueueName = callData.QueueName();
                //if (callData.ServiceAttendanceID())
                //    self.ServiceAttendanceID = callData.ServiceAttendanceID();
                //if (callData.ServiceID())
                //    self.ServiceID = callData.ServiceID();
                //if (callData.ServiceItemID())
                //    self.ServiceItemID = callData.ServiceItemID();
                //self.ServiceItemAttendanceFullName(callData.ServiceItemAttendanceFullName());
                //
                var clientID = self.ClientID();
                var id = callData.ServiceItemID() == null ? callData.ServiceAttendanceID() : callData.ServiceItemID();
                if (id)
                    self.ajaxControl_ServiceItemAttendance.Ajax($('#' + self.MainRegionID).find('.serviceItemAttendance'),
                        {
                            url: '/api/serviceitems/' + id,
                            method: 'GET'
                        },
                        function (response) {
                            self.ServiceSet(callData.ServiceID(), callData.ServiceItemID(), callData.ServiceAttendanceID(), callData.ServiceItemAttendanceFullName(), response != null ? response.Parameter : null, false, response.Summary);
                        });

                if (callData.Description())
                    $.when(self.htmlDescriptionControlD).done(function () { self.Description(callData.Description()); });
                //
                if (callData.ImageSource)
                    self.ImageSource(callData.ImageSource);
                if (callData.IsServiceAttendance)
                    self.IsServiceAttendance = callData.IsServiceAttendance;
            };
        }
    }
    return module;
});
