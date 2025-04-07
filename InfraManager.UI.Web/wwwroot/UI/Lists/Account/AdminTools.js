define(['knockout', 'jquery', 'ajax', 'ui_controls/ListView/ko.ListView', 'ui_controls/ContextMenu/ko.ContextMenu', 'ui_lists/Account/AdminTools.SessionTable', 'ui_lists/Account/AdminTools.SessionTable.ContextMenu', 'ui_lists/Account/AdminTools.SessionHistoryTable', 'ui_lists/Account/AdminTools.PersonalLicenceTable', 'ui_lists/Account/AdminTools.PersonalLicenceTable.ContextMenu', 'ui_lists/Account/AdminTools.ActiveSessionsDashboard'], function (ko, $, ajaxLib, m_lv, m_cm, m_sessionTable, m_sessionTableContextMenu, m_sessionHistoryTable, m_personalLicenceTable, m_personalLicenceTableContextMenu, m_activeSessionsDashboard) {
    var module = {
        ViewModel: function () {
            var self = this;
            //
            {//info
                self.activeSessionCount = ko.observable('');
                self.activeConcurrentSessionCount = ko.observable('');
                self.activePersonalSessionCount = ko.observable('');
                self.availableConcurrentSessionCount = ko.observable('');
                self.availablePersonalSessionCount = ko.observable('');
            }
            //
            {//tabs
                self.modes = {
                    sessions: 0,
                    history: 1,
                    personalLicence: 2,
                    activeSessions: 3
                };
                self.mode = ko.observable(null);
                self.mode.subscribe(function (newValue) {
                    self.setSizeOfControls();
                });
                //
                self.sessionClick = function () {
                    self.mode(self.modes.sessions);
                };
                self.sessionHistoryClick = function () {
                    self.mode(self.modes.history);
                };
                self.personalLicenceClick = function () {
                    self.mode(self.modes.personalLicence);
                };
                self.activeSessionClick = function () {
                    self.mode(self.modes.activeSessions);
                    self.activeSessionsDashboard_renered();
                };
            }
            //
            {//listView session
                self.sessionTable_loaded = false;
                self.sessionTable_renered = function () {//fire after render
                    if (self.sessionTable_loaded === false) {
                        self.sessionTable_loaded = true;
                        self.sessionTable.load();
                    }
                };
                self.sessionTable = new m_sessionTable.ViewModel(self, self.sessionTable_renered);
                //
                self.sessionTable_ContextMenu = ko.observable(null);//context menu
                self.sessionTableContextMenu = new m_sessionTableContextMenu.ViewModel(self, self.sessionTable_ContextMenu);//view model of context menu                
            }
            //
            {//listView sessionHistory
                self.sessionHistoryTable_loaded = false;
                self.sessionHistoryTable_renered = function () {//fire after render
                    if (self.sessionHistoryTable_loaded === false) {
                        self.sessionHistoryTable_loaded = true;
                        self.sessionHistoryTable.load();
                    }
                };
                self.sessionHistoryTable = new m_sessionHistoryTable.ViewModel(self, self.sessionHistoryTable_renered);
            }
            //
            {//listView personalLicence
                self.personalLicenceTable_loaded = false;
                self.personalLicenceTable_renered = function () {//fire after render
                    if (self.personalLicenceTable_loaded === false) {
                        self.personalLicenceTable_loaded = true;
                        self.personalLicenceTable.load();
                    }
                };
                self.personalLicenceTable = new m_personalLicenceTable.ViewModel(self, self.personalLicenceTable_renered);
                //
                self.personalLicenceTable_ContextMenu = ko.observable(null);//context menu
                self.personalLicenceTableContextMenu = new m_personalLicenceTableContextMenu.ViewModel(self, self.personalLicenceTable_ContextMenu);//view model of context menu                
            }
            //
            {//listView activeSessions
                self.activeSessionsDashboard_loaded = false;
                self.activeSessionsDashboard_renered = function () {//fire after render
                    if (self.activeSessionsDashboard_loaded === false) {
                        self.activeSessionsDashboard_loaded = true;
                        self.activeSessionsDashboard.LoadDashboard();
                    }
                };
                self.activeSessionsDashboard = new m_activeSessionsDashboard.ViewModel(self, self.activeSessionsDashboard_renered);
            }
            //
            {//set size of elements
                self.setSizeOfControls = function () {
                    var pageHeight = getPageContentHeight();
                    var height = pageHeight - $(".connectionInfo").outerHeight() - $(".tabControl_header").outerHeight() - 10;//margin-top
                    if (self.mode() == self.modes.sessions)
                        $('.lvSession').css('height', height + "px");
                    else if (self.mode() == self.modes.history)
                        $('.lvSessionHistory').css('height', height + "px");
                    else if (self.mode() == self.modes.personalLicence)
                        $('.lvPersonalLicence').css('height', height + "px");
                };
                //
                $(window).resize(self.setSizeOfControls);
            }
            //
            self.ajaxControl = new ajaxLib.control();
            self.reloadSessionInfo_timeout = undefined;
            self.reloadSessionInfoNow = function () {
                self.ajaxControl.Ajax($('.connectionInfo'),
                    {
                        url: '/accountApi/GetSessionInfo',
                        method: 'GET'
                    },
                    function (response) {
                        if (response.Result === 0 && response.Data) {
                            var obj = response.Data;
                            //               
                            self.activeSessionCount(obj.ActiveSessionCount);
                            self.activeConcurrentSessionCount(obj.ActiveConcurrentSessionCount);
                            self.activePersonalSessionCount(obj.ActivePersonalSessionCount);                            
                            self.availableConcurrentSessionCount(obj.AvailableConcurrentSessionCount);
                            self.availablePersonalSessionCount(obj.AvailablePersonalSessionCount);
                            //
                            self.reloadSessionInfo_timeout = null;
                        }
                        else
                            setLocation('Account/AdminTools');
                    });

            };
            self.reloadSessionInfo = function () {
                if (self.reloadSessionInfo_timeout === undefined) {//first time
                    self.reloadSessionInfo_timeout = null;
                    self.reloadSessionInfoNow();
                }
                else if (self.reloadSessionInfo_timeout === null) {//not first, wait
                    self.reloadSessionInfo_timeout = setTimeout(self.reloadSessionInfoNow, 5000);
                }
                else {//now is waiting                    
                }
            };
            //
            self.load = function () {
                self.mode(self.modes.sessions);//default tab
                self.reloadSessionInfo();
            };
            //
            self.afterRender = function () {
                self.setSizeOfControls();
            };
            //            
            {//server and local(only this browser tab) events                                              
                self.onUserSessionChanged = function (e, userID, userAgent) {
                    self.reloadSessionInfo();
                };
                //
                //отписываться не будем
                $(document).bind('userSessionChanged', self.onUserSessionChanged);
            }
        }
    }
    return module;
});