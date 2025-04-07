define(['knockout', 'jquery', 'usualForms', 'sdForms', 'ajax', 'dynamicOptionsService'], function (ko, $, fhModule, sfhModule, ajaxLib, dynamicOptionsService) {
    var module = {
        ViewModel: function () {
            var self = this;
            //
            //обратит внимание на selected.sedt = observable
            //outer actions
            self.MainRegionID = undefined;
            self.ChangeTemplate = undefined;
            self.InvalidateHelpPanel = undefined;
            self.frm = undefined;

            self.CallTypeDefaultID = '00000000-0000-0000-0000-000000000000';

            self.Client = ko.observable(null);

            self.DynamicOptionsService = new dynamicOptionsService.ViewModel(self.MainRegionID, {
                typeForm: 'Create',
                typesUserForm: 'User',
                changeTabCb: function (_invalidValue, tab) {
                    const currentPage = self.TabPages().find(function (page) {
                        return page.Name === tab.Tab.Name
                    });
                    currentPage.TabPageClick();
                },
                isClientMode: true
            });

            self.TabElements = ko.observableArray();

            self.DynamicOptionsServiceInit = function () {
                const serviceItemID = self.ServiceItemID;
                const serviceAttendanceID = self.ServiceAttendanceID;

                if (!serviceItemID && !serviceAttendanceID) {
                    return;
                };

                if (self.DynamicOptionsService.IsInit()) {
                    self.DestroyDynamicServiceData();
                };

                self.DynamicOptionsService.FormId = self.MainRegionID;

                const dependences = {};
                const selectUser = self.Client();

                if (selectUser) {
                    dependences.User = {};
                    dependences.User.OrganizationID = selectUser.OrganizationID;
                    dependences.User.SubdivisionID = selectUser.SubdivisionID;
                };

                if (serviceItemID) {
                    self.DynamicOptionsService.GetTemplate(dynamicOptionsService.Classes.ServiceItem, serviceItemID, null, dependences);
                } else {
                    self.DynamicOptionsService.GetTemplate(dynamicOptionsService.Classes.ServiceAttendance, serviceAttendanceID, null, dependences);
                };

                self.DynamicOptionsService.LoadTabsElementD.then(function () {
                    self.DynamicOptionsService.FormTabs().forEach(function (tab, i) {
                        const tabIndex = self.TabPages().length + 1;
                        const tabName = tab.Tab.Name;
                        const tabId = tab.Tab.ID;

                        const tabPage = new createTabPage(
                            tabIndex,
                            tabName,
                            '',
                            false,
                            '../Templates/Shared/DynamicOptionsService/Templates/CallRegistration',
                            '',
                            self.tpMain.MinWidth,
                            self.tpMain.MinHeight,
                            self.tpMain.ExWidth,
                            self.tpMain.ExHeight,
                            self.tpMain.FormButtons,
                            function () {
                                return true;
                            },
                            tabId);
                        self.TabPages().push(tabPage);
                    });

                    self.TabPages.valueHasMutated();
                });
            };

            self.DestroyDynamicServiceData = function () {
                self.DynamicOptionsService.ResetData();
                self.TabPages.remove(function (tabPage) {
                    return tabPage.Id != null;
                });

                self.TabElements.removeAll();
            };

            //
            //vars
            self.ID = null;//for registered call
            self.CurrentUserD = $.Deferred();
            self.IsHardToChooseButtonVisibleD = $.Deferred();
            self.ShowPlaceOfServiceD = $.Deferred();
            self.ShowPlaceOfService = ko.observable(true);
            self.formClassID = 701;
            //
            self.SelectedClient = ko.observable(null);
            self.SelectedClient.subscribe(function (newValue) {
                self.ServiceClear();//меняем клиента - я инициатор, и доступны будут другие сервисы
                //
                self.LoadTipicalCalls();
                if (self.TipicalCalls().length > 0)
                    self.TipicalCallsVisible(true);
                //
                self.LoadTipicalItemAttendanceList();
                if (self.TipicalItemAttendanceList().length > 0)
                    self.TipicalItemAttendanceListVisible(true);
                //
                var clientID = newValue == null ? null : newValue.ID;
                //
                if (self.callSummarySearcher != null) {
                    self.callSummarySearcher.CurrentUserID = clientID;
                    self.callSummarySearcher.ClearValues();
                }
                if (self.serviceItemAttendanceSearcher != null) {
                    self.serviceItemAttendanceSearcher.CurrentUserID = clientID;
                    self.serviceItemAttendanceSearcher.ClearValues();
                }
                //
                if (self.SelectedTabPage == self.tpMain && newValue != null)
                    self.SelectedCallType().CallTypeClick();//for show message of sla

                if (clientID != null) {
                    $.when(self.GetUserInfo(clientID)).done(function () {
                        self.DynamicOptionsServiceInit();
                    });
                } else {
                    self.DestroyDynamicServiceData();
                };
            });
            self.SelectedCallType = ko.observable(null);
            self.SelectedCallType.subscribe(function (newValue) {
                self.InitializeTabPage();//меняем тип заявки - меняем полностью зависимые данные на карточке
                //
                self.ServiceClear();
            });
            self.SelectedUrgency = ko.observable(null);
            //
            self.CallSummaryName = ko.observable('');
            self.CallSummaryName.subscribe(function (newValue) {
                self.LoadKBArticleList();//при смене краткого описания - поиск похожих статей БЗ
            });
            //
            self.HTMLDescription = ko.observable('');
            //
            self.ServiceID = null;
            self.ServiceItemID = null;
            self.ServiceAttendanceID = null;
            self.ServiceItemAttendanceFullName = ko.observable('');
            self.ServiceHardToChooseMode = false;
            self.SetCallSummarySearcherParameters = function () {
                $.when(self.callSummarySearcherD).done(function () {
                    self.callSummarySearcher.SetSearchParameters( { "CallTypeID" : self.SelectedCallType().ID });
                });
            };
            self.ServiceClear = function () {
                self.ServiceItemAttendanceFullName('');
                //
                self.ServiceID = null;
                self.ServiceItemID = null;
                self.ServiceAttendanceID = null;
                self.ServiceHardToChooseMode = false;
                //
                self.LoadKBArticleList();
                self.DestroyDynamicServiceData();
                //
                self.SetCallSummarySearcherParameters();
            };
            self.ServiceHardToChoose = function () {
                self.ServiceItemAttendanceFullName('[' + getTextResource('ButtonHardToChooseFromServiceCatalogue') + ']');
                $.when(self.serviceItemAttendanceSearcherD).done(function (ctrl) {
                    ctrl.SetSelectedItem();
                });
                //
                self.ServiceID = null;
                self.ServiceItemID = null;
                self.ServiceAttendanceID = null;
                self.ServiceHardToChooseMode = true;
                //
                self.LoadKBArticleList();
                //
                self.SetCallSummarySearcherParameters();
                self.DestroyDynamicServiceData();
            };
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
                self.ServiceHardToChooseMode = false;
                //
                if (serviceParameter && self.HTMLDescription().length == 0) {
                    var html = '<html><body><p style="white-space:pre-wrap">' + serviceParameter.replace(/</g, '&lt;').replace(/>/g, '&gt;') + '</p></body></html>';
                    self.HTMLDescription(html);
                }
                //
                self.LoadKBArticleList();
                // Получаем шаблон по типу сервиса
                self.DynamicOptionsServiceInit();

                //
                self.SetCallSummarySearcherParameters();
                if (summaryName && summaryName != '' && !self.CallSummaryName().length > 0)
                    self.CallSummaryName(summaryName);
                self.SummaryDefaultName(summaryName ? summaryName:'');
            };
            self.SummaryDefaultName = ko.observable('');

            //
            self.htmlDescriptionControl = null;
            self.attachmentsControl = null;
            //
            self.callSummarySearcher = null;
            self.callSummarySearcherD = null;
            self.serviceItemAttendanceSearcher = null;
            self.serviceItemAttendanceSearcherD = null;
            self.clientSearcher = null;
            self.clientSearcherD = null;
            //
            self.ajaxControl_ServiceItemAttendance = new ajaxLib.control();
            self.ajaxControl_Client = new ajaxLib.control();
            self.ajaxControl_RegisterCall = new ajaxLib.control();
            self.ajaxControl_CheckCallType = new ajaxLib.control();
            self.ajaxControl_CallTypeList = new ajaxLib.control();
            self.ajaxControl_UrgencyList = new ajaxLib.control();
            self.ajaxControl_KBArticleList = new ajaxLib.control();
            self.ajaxControl_CallInfoList = new ajaxLib.control();
            self.ajaxControl_ServiceItemAttendanceInfoList = new ajaxLib.control();
            self.ajaxControl = new ajaxLib.control();
            //
            self.SelectedTabPage = undefined;
            self.SelectedView = ko.observable('');//function in ko, template    
            self.tpCallType = undefined;
            self.tpMain = undefined;
            //
            //
            self.Load = function () {
                self.LoadCallTypeList();
                self.LoadGeneralInfo();
                //
                //init tabPages
                var buttons = {};
                buttons[getTextResource('CancelButtonText')] = function () { self.forceClose = false; self.frm.Close(); }
                var checkCallType = function () {
                    if (!self.SelectedCallType()) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('TabPage_StepNotCompleted'), getTextResource('RequireToSelectCallType'), 'info');
                        });
                        return false;
                    } else
                        return true;
                };
                self.tpCallType = new createTabPage(1, getTextResource('TabPage_CallType'), '', true, 'SDForms/CallTypeSelector', 'clientCallRegistration_callTypeSelector', 250, 250, 300, 300, buttons, checkCallType);
                self.TabPages().push(self.tpCallType);
                //
                buttons = {};
                buttons[getTextResource('ButtonCreateCall')] = function () {
                    var d = self.ValidateAndRegisterCall(true);
                    self.forceClose = true;
                    $.when(d).done(function (result) {
                        if (result == null)
                            return;
                        //
                        self.ID = result.ID;
                        self.frm.Close();
                    });
                };
                buttons[getTextResource('ButtonCreateCallAndOpen')] = function () {
                    var d = self.ValidateAndRegisterCall(false);
                    self.forceClose = true;
                    $.when(d).done(function (result) {
                        if (result == null)
                            return;
                        //
                        self.ID = result.Id;
                        self.frm.Close();
                        //                        
                        var fh = new sfhModule.formHelper();
                        fh.ShowCallByContext(self.ID, { useView: false });
                    });
                };
                buttons[getTextResource('CancelButtonText')] = function () {
                    self.forceClose = false;
                    self.frm.Close();
                }
                self.tpMain = new createTabPage(2, getTextResource('TabPage_Service'), '', false, 'SDForms/CallRegistration', 'clientCallRegistration_main', 490, 250, 600, 700, buttons, function () { return true; });
                self.TabPages().push(self.tpMain);
                //
                self.TabPages.valueHasMutated();
            };
            self.InitializeTabPage = function () {
                if (self.SelectedTabPage == self.tpCallType) {
                    self.TipicalCallsVisible(false);
                    self.TipicalItemAttendanceListVisible(false);
                    self.KBArticlesVisible(false);
                    //
                    self.callSummarySearcherD = $.Deferred();
                    self.serviceItemAttendanceSearcherD = $.Deferred();
                    self.clientSearcherD = $.Deferred();
                }
                else if (self.SelectedTabPage == self.tpMain) {
                    var fh = new fhModule.formHelper();
                    //
                    if (self.callSummarySearcher != null)
                        self.callSummarySearcher.Remove();
                    var callSummaryD = fh.SetTextSearcherToField(
                        $('#' + self.MainRegionID).find('.callSummaryName .text-input'),
                        'CallSummarySearcher',
                        null,
                        [undefined, self.SelectedCallType().ID],
                        function (objectInfo) {//select
                            self.CallSummaryName(objectInfo.FullName);
                            //
                            self.DestroyDynamicServiceData();
                            var clientID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                            self.ajaxControl_ServiceItemAttendance.Ajax($('#' + self.MainRegionID).find('.serviceItemAttendance'),
                                {
                                    url: '/api/serviceitems/callSummary/' + objectInfo.ID,
                                    method: 'GET',
                                },
                                function (response) {
                                    if (response) {
                                        self.ServiceSet(response.ServiceID, response.ServiceItemID, response.ServiceAttendanceID, response.FullName, response.Parameter, null, response.Summary);
                                    }
                                });
                        },
                        function () {//reset
                            self.CallSummaryName('');
                        });
                    $.when(callSummaryD).done(function (ctrl) {
                        self.callSummarySearcher = ctrl;
                        self.callSummarySearcherD.resolve(ctrl);
                        ctrl.CurrentUserID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                        //
                        ctrl.LoadD.done(function () {
                            $('#' + self.MainRegionID).find('.callSummaryName .text-input').focus();
                            ctrl.Close();
                            setTimeout(ctrl.Close, 500);
                        });
                    });
                    //
                    if (self.serviceItemAttendanceSearcher != null)
                        self.serviceItemAttendanceSearcher.Remove();
                    var serviceSearcherExtensionLoadedD = $.Deferred();
                    var serviceD = fh.SetTextSearcherToField(
                        $('#' + self.MainRegionID).find('.serviceItemAttendance .text-input'),
                        'ServiceItemAndAttendanceSearcher',
                        'SearchControl/SearchServiceItemAttendanceControl',
                        [self.SelectedCallType().ID],
                        function (objectInfo) {//select        
                            var clientID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                            //
                            self.ajaxControl_ServiceItemAttendance.Ajax($('#' + self.MainRegionID).find('.serviceItemAttendance'),
                                {
                                    url: '/api/serviceitems/' + objectInfo.ID,
                                    method: 'GET'
                                },
                                function (response) {
                                    if (response) {
                                        self.ServiceSet(response.ServiceID, response.ServiceItemID, response.ServiceAttendanceID, response.FullName, response.Parameter, false, response.Summary);
                                    }
                                });
                        },
                        function () {//reset
                            self.ServiceClear();
                        },
                        function (selectedItem) {//close
                            if (!selectedItem && !self.ServiceHardToChooseMode)
                                self.ServiceClear();
                            else if (!selectedItem && self.ServiceHardToChooseMode)
                                self.serviceItemAttendanceSearcher.HardToChooseClick();
                        },
                        serviceSearcherExtensionLoadedD);
                    $.when(serviceD).done(function (vm) {//after load searcher
                        self.serviceItemAttendanceSearcher = vm;
                        self.serviceItemAttendanceSearcherD.resolve(vm);
                        vm.CurrentUserID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                        //
                        vm.SelectFromServiceCatalogueClick = function () {
                            vm.Close();//close dropDownMenu
                            var callType = self.SelectedCallType();
                            if (callType == null)
                                return;
                            var mode = fh.ServiceCatalogueBrowserMode.Default;
                            if (callType.ID != self.CallTypeDefaultID) {//default callType
                                if (callType.IsRFC == true)
                                    mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceAttendances;
                                else
                                    mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceItems;
                            }
                            if (callType.ID === self.CallTypeDefaultID) {
                                mode = fh.ServiceCatalogueBrowserMode.ShowOnlyServiceItems;
                            }
                            mode |= fh.ServiceCatalogueBrowserMode.FilterBySLA | fh.ServiceCatalogueBrowserMode.ShowHardToChoose;
                            var clientID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                            var result = fh.ShowServiceCatalogueBrowser(mode, clientID, self.ServiceID);
                            $.when(result).done(function (serviceItemAttendance) {
                                if (serviceItemAttendance != undefined && serviceItemAttendance != null)
                                    self.ServiceSet(serviceItemAttendance.Service.ID, serviceItemAttendance.ClassID == 406 ? serviceItemAttendance.ID : null, serviceItemAttendance.ClassID == 407 ? serviceItemAttendance.ID : null, serviceItemAttendance.FullName(), serviceItemAttendance.Parameter, null, serviceItemAttendance.Summary);
                                else if (serviceItemAttendance === null)
                                    self.ServiceHardToChoose();
                            });
                        };
                        //
                        vm.HardToChooseClick = function () {
                            vm.Close();//close dropDownMenu
                            self.ServiceHardToChoose();
                        };
                        vm.HardToChooseClickVisible = ko.observable(false);
                        //      
                        serviceSearcherExtensionLoadedD.resolve();
                        //
                        $.when(self.IsHardToChooseButtonVisibleD).done(function (isHardToChooseButtonVisible) {
                            if (isHardToChooseButtonVisible)
                                vm.HardToChooseClickVisible(isHardToChooseButtonVisible);
                        });
                        //
                        $.when(self.ShowPlaceOfServiceD).done(function (isShowHidePlaceOfService) {
                            self.ShowPlaceOfService(isShowHidePlaceOfService);
                        });
                        //
                        $.when(vm.LoadD).done(function () {
                            $('#' + vm.searcherDivID).find('.ui-dialog-buttonset').css({ opacity: 1 });
                        });
                    });
                    //
                    if (self.clientSearcher != null)
                        self.clientSearcher.Remove();
                    var clientD = fh.SetTextSearcherToField(
                        $('#' + self.MainRegionID).find('.clientField .text-input'),
                        'WebUserSearcherNoTOZ',
                        null,
                        null,
                        function (objectInfo) {//select
                            var param = { userID: objectInfo.ID };
                            self.ajaxControl_Client.Ajax($('#' + self.MainRegionID).find('.clientField'),
                               {
                                   dataType: "json",
                                   method: 'GET',
                                   url: '/api/users/' + objectInfo.ID
                               },
                               function (response) {
                                   if (response)
                                       self.SelectedClient(response);
                                   else
                                       self.SelectedClient(null);
                               });
                        },
                        function () {//reset
                            self.SelectedClient(null);
                        },
                        function (selectedItem) {//close
                            if (!selectedItem)
                                self.SelectedClient(null);
                        });
                    $.when(clientD, userD).done(function (ctrl, user) {
                        ctrl.CurrentUserID = user.UserID;//чтобы видеть только относительно подразделений этого пользователя
                        //
                        self.clientSearcher = ctrl;
                        self.clientSearcherD.resolve(ctrl);
                    });
                    //
                    self.LoadUrgencyList();
                    self.LoadCurrentUser();
                    self.LoadServiceItemAttendance();
                    //
                    //if element of elements loaded - just open, overwise - load -> open
                    self.LoadTipicalCalls();
                    if (self.TipicalCalls().length > 0)
                        self.TipicalCallsVisible(true);
                    //
                    self.LoadTipicalItemAttendanceList();
                    if (self.TipicalItemAttendanceList().length > 0)
                        self.TipicalItemAttendanceListVisible(true);
                    //
                    self.LoadKBArticleList();
                    if (self.KBArticles().length > 0)
                        self.KBArticlesVisible(true);
                    //     
                    var htmlElement = $('#' + self.MainRegionID).find('.description');
                    if (self.htmlDescriptionControl == null)
                        require(['htmlControl'], function (htmlLib) {
                            self.htmlDescriptionControl = new htmlLib.control(htmlElement);
                            self.htmlDescriptionControl.OnHTMLChanged = function (htmlValue) {
                                self.HTMLDescription(htmlValue);
                            };
                            self.HTMLDescription.subscribe(function (newValue) {
                                if (self.htmlDescriptionControl != null)
                                    self.htmlDescriptionControl.SetHTML(newValue);
                            });
                        });
                    else
                        self.htmlDescriptionControl.Load(htmlElement);//ko template changed

                    //

                    if (self.attachmentsControl == null || self.attachmentsControl.IsLoaded() == false) {
                        require(['fileControl'], function (fcLib) {
                            var attachmentsElement = $('#' + self.MainRegionID).find('.documentList');
                            self.attachmentsControl = new fcLib.control(attachmentsElement, '.ui-dialog', '.b-requestDetail__files-addBtn');
                            self.attachmentsControl.ReadOnly(false);
                        });
                    }
                    else {
                        self.attachmentsControl.RemoveUploadedFiles();//previous object                   
                        self.attachmentsControl.Load(attachmentsElement);//ko template changed
                    }
                }
                else {//другие вкладки это параметры
                    const currentTab = self.DynamicOptionsService.FormTabs().find(function (tab) {
                        return tab.Tab.ID === self.SelectedTabPage.Id;
                    });

                    if (!currentTab) {
                        self.DestroyDynamicServiceData();
                        return;
                    };

                    self.TabElements(currentTab.TabElements);
                };
            };
            //
            self.IsRegisteringCall = false;
            //
            self.ValidateAndRegisterCall = function (showSuccessMessage, kbArticleID) {
                var retval = $.Deferred();
                //     
                if (self.IsRegisteringCall)
                    return;
                //
                const data = {
                    'UserID': self.SelectedClient() == null ? null : self.SelectedClient().ID,
                    'ClientID': self.Client() == null ? null : self.Client().ID,
                    'CallTypeID': self.SelectedCallType() == null ? null : self.SelectedCallType().ID,
                    'UrgencyID': self.SelectedUrgency() == null ? null : self.SelectedUrgency().ID,
                    'CallSummaryName': self.CallSummaryName(),
                    'HTMLDescription': self.HTMLDescription(),
                    'Description': self.HTMLDescription(),
                    'ServiceItemAttendanceID': self.ServiceHardToChooseMode === true
                        ? null
                        : self.ServiceItemID != null
                            ? self.ServiceItemID
                            : self.ServiceAttendanceID,
                    'Files': self.attachmentsControl == null ? null : self.attachmentsControl.GetData(),
                    'KBArticleID': kbArticleID != null ? kbArticleID : null,
                    //                  
                    'ServicePlaceID': self.ServicePlaceCallID(),
                    'ServicePlaceClassID': self.ServicePlaceCallClassID(),
                    'CreatedByClient': true,
                    //
                    'FormValuesData': self.DynamicOptionsService.SendByServer(),
                };
                if (!data.CallSummaryName || data.CallSummaryName.trim().length == 0) {
                    data.CallSummaryName = self.SummaryDefaultName();
                }
                if (!data.CallSummaryName || data.CallSummaryName.trim().length == 0) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('PromptCallSummary'));
                    });
                    retval.resolve(null);
                    return;
                }
                if (data.ServiceItemAttendanceID == null && !self.ServiceHardToChooseMode) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('PromptServiceItemAttendance'));
                    });
                    retval.resolve(null);
                    return;
                }
                if (self.CallTypeDefaultID === data.CallTypeID && self.ServiceAttendanceID !== null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('RegisterCallForAttendanceForThisTypeUnavailable'));
                    });
                    retval.resolve(null);
                    return;
                }
                if (data.UserID == null) {
                    require(['sweetAlert'], function () {
                        swal(getTextResource('PromptClient'));
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

                data.ReceiptType = 'web';
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
                                    if (showSuccessMessage) {
                                        require(['sweetAlert'], function () {
                                            swal(getTextResource('CallRegisteredMessage').replace('{0}', response.Number));
                                        });
                                    }
                                    retval.resolve({ Id: response.ID, CallID: response.ID });
                                    self.IsRegisteringCall = false;

                                    swal({
                                        title: getTextResource('CallRegisteredMessage').replace('{0}', response.Number)
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
                self.IsRegisteringCall = true;
                showSpinner();
                self.ajaxControl_RegisterCall.Ajax(null,
                    {
                        url: '/api/calls',
                        method: 'POST',
                        contentType: 'application/json',
                        dataType: 'json',
                        data: JSON.stringify(data)
                    },
                    ((response) => registerAttachmentsCallback(response)),
                    function (response) {
                        hideSpinner();
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ErrorCaption'), getTextResource('AjaxError') + '\n[CallRegistration.js, RegisterCall]', 'error');
                        });
                        retval.resolve(null);
                        self.IsRegisteringCall = false;
                    });
                //
                return retval.promise();
            };
            //
            //
            self.LoadGeneralInfo = function () {
                var d = $.Deferred();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/api/Accounts/my',
                        method: 'GET'
                    },
                    function (response) {
                        self.ajaxControl.Ajax(null,
                            {
                                dataType: "json",
                                method: 'GET',
                                url: `/api/users/${response.UserID}`
                            },
                            function (user) {
                                self.CurrentUserD.resolve(user);
                                d.resolve()
                            });
                    });
                $.when(d).done(function () {
                    self.ajaxControl.Ajax(null,
                        {
                            url: '/configApi/IsHardToChooseButtonVisibile',
                            method: 'GET'
                        },
                        function (response) {
                            self.IsHardToChooseButtonVisibleD.resolve(response);
                        });
                });
                $.when(d).done(function () {
                    var ajaxControl = new ajaxLib.control();
                    ajaxControl.Ajax(null,
                        {
                            url: '/configApi/IsHidePlaceOfServiceVisible',
                            method: 'GET'
                        },
                        function (response) {
                            self.ShowPlaceOfServiceD.resolve(!response);
                        });
                });
            };
            //
            //
            self.LoadCurrentUser = function () {
                var selectedClient = self.SelectedClient();
                if (selectedClient != null) {
                    $.when(self.clientSearcherD).done(function (ctrl) {
                        ctrl.SetSelectedItem(selectedClient.UserID, 9, selectedClient.UserFullName, '');
                    });
                    return;
                }
                //
                $.when(self.CurrentUserD).done(function (currentUser) {
                    self.SelectedClient(currentUser);
                    if (currentUser)
                        $.when(self.clientSearcherD).done(function (ctrl) {
                            ctrl.SetSelectedItem(currentUser.UserID, 9, currentUser.UserFullName, '');
                        });
                });
            };
            self.LoadServiceItemAttendance = function () {
                var id = self.ServiceItemID == null ? self.ServiceAttendanceID : self.ServiceItemID;
                var classID = self.ServiceItemID == null ? 407 : 406;
                var fullName = self.ServiceItemAttendanceFullName();
                if (id)
                    $.when(self.serviceItemAttendanceSearcherD).done(function (ctrl) {
                        ctrl.SetSelectedItem(id, classID, fullName, '');
                    });
            };
            //
            //
            self.CallTypeList = ko.observableArray([]);
            var createCallType = function (callType) {
                var thisObj = this;
                //
                thisObj.ID = callType.ID;
                thisObj.Name = callType.Name;
                thisObj.IsRFC = callType.IsRFC;
                thisObj.IsIncident = callType.IsIncident;
                //
                thisObj.CallTypeClick = function () {
                    var clientID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                    self.ajaxControl_CheckCallType.Ajax($('#' + self.MainRegionID),
                        {
                            url: '/api/' + (thisObj.IsRFC ? 'ServiceAttendance' : 'ServiceItem') + 'SlaApplicabilities',
                            data: { userID: clientID, page: 0, pageSize: 1 }, 
                        },
                        function (response) {
                            if (response) {
                                if (response.ItemIDs.length > 0)
                                    require(['sweetAlert'], function () {
                                        swal(getTextResource(thisObj.IsRFC ? 'SLANotFoundForRFC' : 'SLANotFound')); //some problem
                                    });
                                //
                                if (self.SelectedTabPage == self.tpCallType) {
                                    self.SelectedCallType(thisObj);
                                    self.tpCallType.Value(self.SelectedCallType().Name);//caption in tabControl
                                    self.tpMain.TabPageClick();//next step of registration
                                }
                            }
                        });
                };
            };
            self.LoadCallTypeList = function () {
                if (self.CallTypeList().length > 0)
                    return;
                //
                self.ajaxControl_CallTypeList.Ajax($('#' + self.MainRegionID),
                    {
                        url: '/api/callTypes',
                        method: 'GET',
                        data: { visibleInWeb: true }
                    },
                    function (response) {
                        if (response) {
                            self.CallTypeList.removeAll();
                            //
                            $.each(response, function (index, callType) {
                                var c = new createCallType(callType);
                                self.CallTypeList().push(c);
                            });
                            self.CallTypeList.valueHasMutated();
                        }
                    });
            };
            //
            //
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
                        self.UrgencyListD.resolve();
                    });
            };
            //
            //
            self.TabPages = ko.observableArray([]);
            var createTabPage = function (index, name, value, isSelected, templateName, formSettingsName, minWidth, minHeight, width, height, formButtons, checkStepCompleted, Id) {
                var thisObj = this;
                //
                thisObj.Index = index;
                thisObj.Name = name;
                thisObj.Id = Id;
                thisObj.Value = ko.observable(value);
                thisObj.IsSelected = ko.observable(isSelected);
                //
                thisObj.TemplateName = templateName;
                thisObj.FormSettingsName = formSettingsName;
                thisObj.FormButtons = formButtons;
                thisObj.MinWidth = minWidth;
                thisObj.MinHeight = minHeight;
                thisObj.ExWidth = width;
                thisObj.ExHeight = height;
                thisObj.CheckStepCompleted = checkStepCompleted;
                //
                thisObj.Caption = ko.computed(function () {
                    return thisObj.Value() ? thisObj.Name : '';
                });
                thisObj.Text = ko.computed(function () {
                    return thisObj.Value() ? thisObj.Value() : thisObj.Name;
                });
                //
                thisObj.FuncIsValid = ko.observable(function () { return true; });
                thisObj.IsValid = ko.computed(function () {
                    return thisObj.FuncIsValid()();
                });
                //
                thisObj.TabPageClick = function () {
                    if (thisObj === self.SelectedTabPage)
                        return;
                    for (var i = 1; i < thisObj.Index; i++)
                        if (!self.TabPages()[i - 1].CheckStepCompleted())
                            return;
                    //
                    self.SelectedTabPage = thisObj;
                    if (self.SelectedView() != thisObj.TemplateName)
                        self.ChangeTemplate();
                    else
                        self.InitializeTabPage();
                    //
                    $.each(self.TabPages(), function (index, tabPage) {
                        tabPage.IsSelected(tabPage === self.SelectedTabPage);
                    });
                };
                //
                if (isSelected) {
                    self.SelectedTabPage = thisObj;
                    self.ChangeTemplate();
                }
            };
            //
            //
            self.TipicalCalls = ko.observableArray([]);
            self.TipicalCallsVisible = ko.observable(false);
            self.TipicalCalls_paramCallTypeID = null;
            self.TipicalCalls_paramUserID = null;
            self.TipicalCalls_timeout = null;
            self.TipicalCallsVisible.subscribe(function (newValue) {
                self.InvalidateHelpPanel();
            });
            var createTipicalCall = function (callInfo) {
                var thisObj = this;
                //
                thisObj.ID = callInfo.ID;
                thisObj.Number = callInfo.Number;
                thisObj.CallSummaryName = callInfo.CallSummaryName;
                thisObj.ServiceItemAttendanceFullName = callInfo.ServiceItemAttendanceFullName;
                thisObj.ServiceItemID = callInfo.ServiceItemID;
                thisObj.ServiceAttendanceID = callInfo.ServiceAttendanceID;
                thisObj.ServiceID = callInfo.ServiceID;
                thisObj.UrgencyName = callInfo.UrgencyName;
                thisObj.UrgencyID = callInfo.UrgencyID;
                thisObj.Description = callInfo.Description;
                thisObj.HTMLDescription = callInfo.HTMLDescription;
                //
                thisObj.UseCallClick = function () {
                    self.CallSummaryName(thisObj.CallSummaryName);
                    //
                    self.ServiceSet(thisObj.ServiceID, thisObj.ServiceItemID, thisObj.ServiceAttendanceID, thisObj.ServiceItemAttendanceFullName)
                    //
                    $.when(self.UrgencyListD).done(function () {
                        $.each(self.UrgencyList(), function (index, urgency) {
                            if (urgency.ID == thisObj.UrgencyID) //exists
                                self.SelectedUrgency(urgency);
                        });
                    });
                    //
                    self.HTMLDescription(thisObj.HTMLDescription);
                };
                thisObj.ShowCallClick = function () {
                    var fh = new sfhModule.formHelper();
                    fh.ShowCall(thisObj.ID, fh.Mode.ReadOnly | fh.Mode.ClientMode);
                };
            };
            self.LoadTipicalCalls = function () {
                var selectedCallTypeID = self.SelectedCallType() == null ? null : self.SelectedCallType().ID;
                var selectedUserID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                //
                if (self.TipicalCalls_paramCallTypeID == selectedCallTypeID && self.TipicalCalls_paramUserID == selectedUserID)
                    return;
                //
                self.TipicalCallsVisible(false);
                self.TipicalCalls.removeAll();
                //
                if (selectedCallTypeID == null || selectedUserID == null)
                    return;
                //
                self.TipicalCalls_paramCallTypeID = selectedCallTypeID;
                self.TipicalCalls_paramUserID = selectedUserID;
                //
                if (self.TipicalCalls_timeout != null)
                    clearTimeout(self.TipicalCalls_timeout);
                self.TipicalCalls_timeout = setTimeout(function () {
                    self.ajaxControl_CallInfoList.Ajax(null,
                    {
                        url: '/sdApi/GetCallInfoList',
                        method: 'GET',
                        data: { callTypeID: self.TipicalCalls_paramCallTypeID, userID: self.TipicalCalls_paramUserID }
                    },
                    function (response) {
                        if (response) {
                            self.TipicalCalls.removeAll();
                            //
                            $.each(response, function (index, callInfo) {
                                var u = new createTipicalCall(callInfo);
                                self.TipicalCalls().push(u);
                            });
                            self.TipicalCalls.valueHasMutated();
                            if (self.TipicalCalls().length > 0 && self.SelectedTabPage == self.tpMain)
                                self.TipicalCallsVisible(true);
                            else
                                self.TipicalCallsVisible(false);
                            self.InvalidateHelpPanel();
                        }
                    });
                }, 1000);
            };
            //
            //
            self.TipicalItemAttendanceList = ko.observableArray([]);
            self.TipicalItemAttendanceListVisible = ko.observable(false);
            self.TipicalItemAttendanceListVisible.subscribe(function (newValue) {
                self.InvalidateHelpPanel();
            });
            self.TipicalItemAttendanceList_paramCallTypeID = null;
            self.TipicalItemAttendanceList_paramUserID = null;
            self.TipicalItemAttendanceList_timeout = null;
            var createTipicalItemAttendance = function (info) {
                var thisObj = this;
                //
                thisObj.ServiceID = info.ServiceID;
                thisObj.ServiceName = info.ServiceName;
                thisObj.Name = info.Name;
                thisObj.FullName = info.FullName;
                thisObj.ServiceItemID = info.ServiceItemID;
                thisObj.ServiceAttendanceID = info.ServiceAttendanceID;
                thisObj.Parameter = info.Parameter;
                thisObj.Summary = info.Summary;
                //
                thisObj.UseItemAttendanceClick = function () {
                    self.ServiceSet(thisObj.ServiceID, thisObj.ServiceItemID, thisObj.ServiceAttendanceID, thisObj.FullName, thisObj.Parameter, null, thisObj.Summary)
                };
            };
            self.LoadTipicalItemAttendanceList = function () {
                var selectedCallTypeID = self.SelectedCallType() == null ? null : self.SelectedCallType().ID;
                var selectedUserID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                //
                if (self.TipicalItemAttendanceList_paramCallTypeID == selectedCallTypeID && self.TipicalItemAttendanceList_paramUserID == selectedUserID)
                    return;
                //
                self.TipicalItemAttendanceListVisible(false);
                self.TipicalItemAttendanceList.removeAll();
                //
                if (selectedCallTypeID == null || selectedUserID == null)
                    return;
                ///sdApi/GetServiceItemAttendanceInfoList
                self.TipicalItemAttendanceList_paramCallTypeID = selectedCallTypeID;
                self.TipicalItemAttendanceList_paramUserID = selectedUserID;
                //
                if (self.TipicalItemAttendanceList_timeout != null)
                    clearTimeout(self.TipicalItemAttendanceList_timeout);
                self.TipicalItemAttendanceList_timeout = setTimeout(function () {
                    self.ajaxControl_ServiceItemAttendanceInfoList.Ajax(null,
                        {
                            url: '/sdApi/GetServiceItemAttendanceInfoList',
                            method: 'GET',
                            data: { callTypeID: self.TipicalItemAttendanceList_paramCallTypeID, userID: self.TipicalItemAttendanceList_paramUserID }
                        },
                        function (response) {
                            if (response) {
                                self.TipicalItemAttendanceList.removeAll();
                                //
                                $.each(response, function (index, info) {
                                    var s = new createTipicalItemAttendance(info);
                                    self.TipicalItemAttendanceList().push(s);
                                });
                                self.TipicalItemAttendanceList.valueHasMutated();
                                if (self.TipicalItemAttendanceList().length > 0 && self.SelectedTabPage == self.tpMain)
                                    self.TipicalItemAttendanceListVisible(true);
                                else
                                    self.TipicalItemAttendanceListVisible(false);
                                self.InvalidateHelpPanel();
                            }
                        });
                }, 100);
            };
            //
            self.ServicePlaceCallID = ko.observable(null);
            self.ServicePlaceCallClassID = ko.observable(null);
            self.ServicePlaceCallName = ko.observable('');
            self.ajaxControl_ClientInfo = new ajaxLib.control();
            self.onLocationChanged = function (objectInfo) { // когда новое местоположение будет выбрано
                self.ServicePlaceCallID(objectInfo.ID);
                self.ServicePlaceCallClassID(objectInfo.ClassID);
                self.ServicePlaceCallName(objectInfo.PlaceFullName);
                self.ServicePlaceCallName.valueHasMutated();
            };
            //
            self.EditLocation = function () {
                showSpinner();
                //
                require(['../Templates/ClientSearch/frmCallClientEditLocation', 'sweetAlert'], function (module) {
                    //
                    var options = {
                        PlaceID: self.ServicePlaceCallID,
                        PlaceClassID: self.ServicePlaceCallClassID,
                        PlaceName: self.ServicePlaceCallName

                    };

                    var saveLocation = function (objectInfo) {
                        if (!objectInfo)
                            return;
                        self.onLocationChanged(objectInfo);
                    };
                    //
                    module.ShowDialog(options, saveLocation, true);
                });
            };
            //
            self.ajaxControl = new ajaxLib.control();

            self.GetUserInfo = function (id) {
                var valueListD = $.Deferred();
                self.ajaxControl.Ajax(null,
                    {
                        url: '/api/users/' + id,
                        method: 'GET'
                    },
                    function (response) {
                        if (response) {
                            var locInfo = {
                                ID: response.WorkplaceIMObjID,
                                ClassID: 22,
                                PlaceFullName: response.WorkplaceFullName
                            }
                            self.onLocationChanged(locInfo);
                            self.Client(response);
                        }
                        valueListD.resolve();
                    });
                return valueListD.promise();
            };
            //
            self.KBArticles = ko.observableArray([]);
            self.KBArticlesVisible = ko.observable(false);
            self.KBArticlesVisible.subscribe(function (newValue) {
                self.InvalidateHelpPanel();
            });
            self.KBArticles_paramCallSummaryName = null;
            self.KBArticles_paramServiceItemAttendanceID = null;
            self.KBArticles_timeout = null;
            var createKBArticle = function (kbInfo) {
                var thisObj = this;
                //
                thisObj.ID = kbInfo.ID;
                thisObj.Name = kbInfo.Name;
                thisObj.TagString = kbInfo.TagString;
                thisObj.Description = kbInfo.Description;
                thisObj.HTMLDescription = kbInfo.HTMLDescription;
                //
                thisObj.UseKBArticleClick = function () {
                    self.CallSummaryName(thisObj.Name);
                    self.HTMLDescription(thisObj.HTMLDescription);
                };
                thisObj.ShowKBArticleClick = function () {
                    var fh = new fhModule.formHelper();
                    fh.ShowKBAView(thisObj.ID);
                };
                thisObj.RegisterCallByKBArticleClick = function () {
                    require(['sweetAlert'], function () {
                        swal({
                            title: getTextResource('UseKBArticleQuestion'),
                            text: thisObj.Name,
                            showCancelButton: true,
                            closeOnConfirm: true,
                            closeOnCancel: true,
                            confirmButtonText: getTextResource('ButtonOK'),
                            cancelButtonText: getTextResource('ButtonCancel')
                        },
                        function (value) {
                            if (value == true)
                                setTimeout(function () {
                                    if (self.CallSummaryName().trim() == '')
                                        self.CallSummaryName(thisObj.Name);
                                    if (self.HTMLDescription().trim() == '')
                                        self.HTMLDescription(thisObj.HTMLDescription);
                                    //
                                    var d = self.ValidateAndRegisterCall(false, thisObj.ID);
                                    $.when(d).done(function (result) {
                                        if (result == null)
                                            return;
                                        //
                                        self.ID = result.Id;
                                        self.frm.Close();
                                        //
                                        var fh = new sfhModule.formHelper();
                                        fh.ShowCallByContext(self.ID);
                                    });
                                }, 1000);//swal close timeout
                        });
                    });
                };
            };
            self.LoadKBArticleList = function () {
                var selectedCallSummaryName = self.CallSummaryName();
                var selectedUserID = self.SelectedClient() == null ? null : self.SelectedClient().ID;
                var selectedServiceItemAttendanceID = self.ServiceAttendanceID == null ? self.ServiceItemID : self.ServiceAttendanceID;
                //
                if (selectedServiceItemAttendanceID == self.KBArticles_paramServiceItemAttendanceID && selectedCallSummaryName == self.KBArticles_paramCallSummaryName)
                    return;
                //
                self.KBArticlesVisible(false);
                self.KBArticles.removeAll();
                //
                self.KBArticles_paramCallSummaryName = selectedCallSummaryName;
                self.KBArticles_paramServiceItemAttendanceID = selectedServiceItemAttendanceID;
                //
                if (selectedCallSummaryName.length == 0 || selectedUserID == null || selectedServiceItemAttendanceID == null)
                    return;
                //
                if (self.KBArticles_timeout != null)
                    clearTimeout(self.KBArticles_timeout);
                self.KBArticles_timeout = setTimeout(function () {
                    self.ajaxControl_KBArticleList.Ajax(null,
                        {
                            url: '/api/kb/searchInfo',
                            method: 'GET',
                            data: {
                                text: self.KBArticles_paramCallSummaryName,
                                clientRegistration: true,
                                serviceItemAttendanceID: self.KBArticles_paramServiceItemAttendanceID
                            }
                        },
                        function (response) {
                            self.KBArticles.removeAll();
                            //
                            if (response) {
                                $.each(response, function (index, kbInfo) {
                                    var kba = new createKBArticle(kbInfo);
                                    self.KBArticles().push(kba);
                                });
                            }
                            self.KBArticles.valueHasMutated();
                            if (self.KBArticles().length > 0 && self.SelectedTabPage == self.tpMain)
                                self.KBArticlesVisible(true);
                            else
                                self.KBArticlesVisible(false);
                            self.InvalidateHelpPanel();
                        });
                }, 1000);
            };
        }
    }
    return module;
});
