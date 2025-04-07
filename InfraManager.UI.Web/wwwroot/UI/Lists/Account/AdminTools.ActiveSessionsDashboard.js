define(['knockout', 'jquery', 'ajax', 'dateTimeControl'], function (ko, $, ajaxLib, dtLib) {
    var module = {
        ViewModel: function (adminToolsVM, onRendered) {
            var self = this;
            //
            {//dashboard
                self.dashboardIsLoaded = false;
                //
                self.Top = ko.observable(84); //connectionInfo + tabControl_header = 84px
                //
                self.LoadDashboard = function () {
                    if (self.dashboardIsLoaded)
                        return;
                    //
                    require(['models/Dashboard/DashboardSearch'], function (vm) {
                        vm.DashboardLoad($('.dashboardView__dashboard_activeSessions'), '.dashboardView__dashboard_activeSessions', '00000000-0000-0000-0000-000000000001', self.iframeOnLoad);
                        self.dashboardIsLoaded = true;
                    });
                };
                //
                self.iframeOnLoad = function () {
                    var $iframe = $('.dashboardView__dashboard_activeSessions').find('.dashboardViewerIframe');
                    //
                    $iframe.on('load', function () {
                        self.iframeResize();
                        hideSpinner($('.dashboardView__dashboard_activeSessions')[0]);
                    });
                };
                //
                self.iframeResize = function () {
                    var $iframe = $('.dashboardView__dashboard_activeSessions').find('.dashboardViewerIframe');
                    //
                    if (!$iframe || !$iframe[0])
                        return;
                    //
                    //TODO: убрать дублирование кода с DashboardSearch
                    var _document = $iframe[0].contentWindow.document;
                    //
                    if (!_document || !_document.body || !_document.forms || !_document.forms[0]) {
                        console.log('bad render order');
                        return;
                    }
                    //
                    $iframe[0].style.height = '100%';
                    _document.body.style.height = '100%';
                    _document.forms[0].style.height = '100%';
                    //
                    var container = _document.getElementsByClassName('dxWindowsPlatform')[0];//'dxWindowsPlatform' - на chrome, win10. проверить для других браузеров.
                    container.style.height = '100%';
                    container.style.overflow = 'hidden';
                };
            }
        }
    }
    return module;
});