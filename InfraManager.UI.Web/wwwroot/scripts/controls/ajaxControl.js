define(['knockout', 'jquery'], function (ko, $) {
    var module = {
        control: function () {
            var self = this;
            //
            self.xhr = null;
            self.container = null;
            self.Ajax = function ($container, ajaxSettings, onSuccess, onError, onComplete, errorHandlers) {
                var defaultHandlers = {
                    onForbidden: function () {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('AccessError'));
                        })
                        hideSpinner();
                    },
                    onNotFound: function () {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ResourceNotFoundErrorText'));
                        })
                    },
                    onUnprocessableEntity: function (responseText) {
                        require(['sweetAlert'], function () {
                            swal(getTextResource(responseText) || responseText);
                        })
                    },
                    onConflict: function () {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('ConcurrencyErrorWithoutQuestion'));
                        })
                    },
                    onInternalServerError: function () {
                        require(['sweetAlert'], function () {
                            swal(getTextResource('UnhandledErrorServer'));
                        })
                    },
                    onMethodNotAllowed: function () {
                        require(['sweetAlert'], function () { // по умолчанию считаем это такой же ошибкой как и 500
                            swal(getTextResource('UnhandledErrorServer'));
                        })
                    }
                };

                var handlers = $.extend(defaultHandlers, errorHandlers || {});

                self.Abort();
                //
                var d = $.Deferred();
                //
                var ajaxConfig = {
                    dataType: "json",
                    method: '',
                    url: '/',
                    data: {}
                };
                $.extend(ajaxConfig, ajaxSettings);
                //
                if (onSuccess) {
                    ajaxConfig.success = onSuccess;
                }
                ajaxConfig.error = function (e) {
                    if (e.status === 403) {
                        handlers.onForbidden();
                    } else if (e.status === 404) {
                        handlers.onNotFound();
                    } else if (e.status === 422) {
                        handlers.onUnprocessableEntity(e.responseText);
                    } else if (e.status === 409) {
                        handlers.onConflict();
                    } else if (e.status === 500) {
                        handlers.onInternalServerError();
                    } else if (e.status === 405) {
                        handlers.onMethodNotAllowed();
                    } else if (e.status === 200 && e.responseText) {//all is ok, jquery didnt parse json and think there are error
                        let contentType = e.getResponseHeader('content-type');
                        if (contentType.match('^application/json')) {
                            let json = JSON.parse(e.responseText);
                            if (onSuccess && json) {
                                onSuccess(json);
                                return;
                            }
                        } else if (contentType.match('^text/html')) { // сессия "протухла" и произошел редирект на форму входа
                            window.location.reload();
                            return;
                        }
                    } else if (e.status === 401) {
                        window.location.reload();
                        return;
                    }
                    if (onError) {
                        onError(e);
                    }
                };
                if (onComplete) {
                    ajaxConfig.complete = onComplete;
                }
                self.container = $container;
                if ($container && $container[0]) {
                    showSpinner($container[0]);
                }
                //for slow test
                //setTimeout(function () {
                self.IsAcitve(true);
                self.xhr = $.ajax(ajaxConfig).done(function () {
                    d.resolve();
                }).fail(function () {
                    d.fail();
                }).always(function () {
                    if ($container && $container[0]) {
                        hideSpinner($container[0]);
                    }
                    self.xhr = null;
                    self.IsAcitve(false);
                });
                //}, 3000);
                //
                return d.promise();
            };
            //
            self.IsAcitve = ko.observable(false);
            //
            self.Abort = function () {
                var xhr = self.xhr;
                if (xhr) {
                    xhr.abort();
                    self.xhr = null;
                }
                //
                var container = self.container;
                if (container && container[0])
                    hideSpinner(container[0]);
                self.container = null;
            };
        }
    }
    return module;
});