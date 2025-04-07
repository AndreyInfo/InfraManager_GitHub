define(['jquery', 'signalRHubs'], function ($, signalR) {
    function init() {
        var self = this;
        //
        var connection = new signalR.HubConnectionBuilder().withUrl('/events').withAutomaticReconnect().build();

        if (connection) {

            connection.on('objectInserted', function (objectClassID, objectID, parentObjectID) {
                $(document).trigger('objectInserted', [objectClassID, objectID, parentObjectID]);
            });
            connection.on('objectUpdated', function (objectClassID, objectID, parentObjectID) {
                $(document).trigger('objectUpdated', [objectClassID, objectID, parentObjectID]);
            });
            connection.on('objectDeleted', function (objectClassID, objectID, parentObjectID) {
                $(document).trigger('objectDeleted', [objectClassID, objectID, parentObjectID]);
            });
            //
            connection.on('workflowOnSaveError', function (objectClassID, objectID, response) {
                $(document).trigger('workflowOnSaveError', [objectClassID, objectID, response]);
            });
            //
            connection.on('externalEventCreated', function (objectID) {
                $(document).trigger('externalEventCreated', [objectID]);
            });
            connection.on('externalEventProcessed', function (objectID) {
                $(document).trigger('externalEventProcessed', [objectID]);
            });
            //
            connection.on('tsMessageInserted', function (messageID, timesheetID, ownerTimesheetID, authorID) {
                $(document).trigger('tsMessageInserted', [messageID, timesheetID, ownerTimesheetID, authorID]);
            });
            //
            connection.on('progressBarProcessed', function (objectClassID, objectID, parentObjectID, percentage) {
                $(document).trigger('progressBarProcessed', [objectClassID, objectID, parentObjectID, percentage]);
            });
            //
            connection.on('userSessionChanged', function (userID, userAgent) {
                $(document).trigger('userSessionChanged', [userID, userAgent]);
                //
                $.when(userD).done(function (user) {
                    if (user.UserID == userID && user.UserAgent == userAgent)
                        setLocation('Errors/Message?msg=' + escape(getTextResource('AdminTools_YourConnectionKilled')));
                });
            });
            //
            connection.on('callAnswered', function (fromNumber) {
                $.when(userD).done(function (user) {
                    if (user.IncomingCallProcessing) {
                        showSpinner();
                        require(['usualForms'], function (lib) {
                            var fh = new lib.formHelper(true);
                            fh.ShowClientSearcher(fromNumber);
                        });
                    }
                });
            });
            //
            self.signalRhubInterval = null;//для автоматического переподключения
            self.stopReconnectInterval = function () {
                clearInterval(self.signalRhubInterval);
                self.signalRhubInterval = null;
            };
            //
            self.allNotificationsStopped = false;//флаг отключения от сервера после бездействия
            self.stopRefreshTimeout = null;//таймер бездействия страницы
            self.stopRefreshTimer = function () {
                clearTimeout(self.stopRefreshTimeout);
                self.stopRefreshTimeout = null;
            };
            //
            //close page
            if (getIEVersion() == -1) {
                window.onunload = function (event) {
                    self.allNotificationsStopped = true;
                    self.stopReconnectInterval();
                    self.stopRefreshTimer();
                    connection.stop();
                };
            }
            connection.start();

            connection.onclose(function () {
                $('.b-connection-error').text(getTextResource('NoConnectionWithServer'));
                $('.b-connection-error').css("display", "block");
                setInterval(function () { connection.start(); }, 2000);
            });
            //
            function onVisibilityChange(callback) {
                var visible = true;
                function focused() {
                    if (!visible)
                        callback(visible = true);
                }
                function unfocused() {
                    if (visible)
                        callback(visible = false);
                }
                //
                if ('hidden' in document) {
                    document.addEventListener('visibilitychange',
                        function () { (document.hidden ? unfocused : focused)() });
                }
                if ('mozHidden' in document) {
                    document.addEventListener('mozvisibilitychange',
                        function () { (document.mozHidden ? unfocused : focused)() });
                }
                if ('webkitHidden' in document) {
                    document.addEventListener('webkitvisibilitychange',
                        function () { (document.webkitHidden ? unfocused : focused)() });
                }
                if ('msHidden' in document) {
                    document.addEventListener('msvisibilitychange',
                        function () { (document.msHidden ? unfocused : focused)() });
                }
                //<=IE9:
                if ('onfocusin' in document) {
                    document.onfocusin = focused;
                    document.onfocusout = unfocused;
                }
                //others:
                window.onpageshow = window.onfocus = focused;
                window.onpagehide = window.onblur = unfocused;
                //hack:
                document.addEventListener('mousemove', focused);
            };
            function setVisible(visible) {
                if (visible == true) {
                    self.stopRefreshTimer();
                    self.allNotificationsStopped = false;//reconnect in interval                        
                }
                else if (document.canDisconnect != false)
                    self.stopRefreshTimeout = setTimeout(function () {
                        self.stopRefreshTimer();
                        self.allNotificationsStopped = true;
                        connection.stop();
                    }, 1000 * 60 * 3);
            };
            //page hide/show, not active/active
            onVisibilityChange(setVisible);
        }
        else
            require(['sweetAlert'], function () {
                swal(getTextResource('UnhandledErrorClient'), getTextResource('AjaxError') + '\n[eventManager.js, init]', 'error');
            });
    }
    //
    return {
        init: init
    };
});