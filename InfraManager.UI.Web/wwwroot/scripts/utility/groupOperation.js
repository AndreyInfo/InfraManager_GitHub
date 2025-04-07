define(['ajax', 'knockout'], function (ajax, ko) {
    var module = {
        ViewModelBase: function (items, options) {
            var self = this;

            self.total = items.length;
            self.completedQuantity = ko.observable(0);
            self.errors = ko.observableArray();
            self.completedItems = ko.observableArray();
            self.batchSize = options.batchSize || module.DefaultBatchSize;
            self.progress = ko.pureComputed(function () { return 100 * (self.completedQuantity() / self.total); });

            const copyItems = items.slice(0);
            self.remaining = ko.observableArray(copyItems);

            self._getUrl = function (item) {
                return item.Uri; // реализация по умолчанию, можно переопределить в потомке
            };
            self._getData = function (item) {
                return undefined; // реализация по умолчанию, можно переопределить в потомке
            };

            function tryStartNext() {
                if (self.remaining().length > 0) {
                    var nextItem = self.remaining.pop();
                    process(nextItem);
                    return true;
                }

                return false;
            };

            function onSpecificError(item, messageResourceKey) {
                return function (errorText) {
                    self.errors.push({ item: item, message: getTextResource(messageResourceKey || errorText)} );
                };
            };

            self._onComplete = function () { }; // по умолчанию ничего не делаем
            self.subscribeComplete = function (handler) {
                var baseOnComplete = self._onComplete;
                self._onComplete = function () {
                    baseOnComplete();
                    handler();
                };
            }
            var _complete = false;

            function onComplete(item) {
                self.completedItems.push(item);
                self.completedQuantity(self.completedQuantity() + 1);
                if (!tryStartNext() && !_complete) { // очередь опустела
                    _complete = true;
                    self._onComplete(item);                    
                }
            }

            self._onSuccess = function (item, response) { }; // по умолчанию ничего дополнительно не делаем
            self.subscribeSuccess = function (handler) {
                var baseOnSuccess = self._onSuccess;
                self._onSuccess = function (item, response) {
                    handler(item, response);
                    baseOnSuccess(item, response);                    
                };
            }
            function onSuccess(item, response) {                
                self._onSuccess(item, response);
                onComplete(item, response);
            }

            self._onError = function (item, e) { }; // по умолчанию ничего дополнительно не делаем
            function onError(item, e) {
                onComplete(item);
                self._onError(item, e);
            }

            function process(item) {
                var ajaxControl = new ajax.control();

                var ajaxConfig = options.ajax;
                ajaxConfig.url = self._getUrl(item);
                ajaxConfig.data = self._getData(item);
                ajaxConfig.onForbidden = onSpecificError(item, 'AccessError');
                ajaxConfig.onNotFound = onSpecificError(item, 'ResourceNotFoundErrorText');
                ajaxConfig.onUnprocessableEntity = onSpecificError(item); // тут выводим текст из ответа
                ajaxConfig.onConflict = onSpecificError(item, 'ConcurrencyErrorWithoutQuestion');
                ajaxConfig.onInternalServerError = onSpecificError(item, 'UnhandledErrorServer');

                ajaxControl.Ajax(
                    options.div, 
                    ajaxConfig,
                    function (response) {
                        onSuccess(item, response);
                    },
                    function (e) {
                        e.status === 200 
                            ? onSuccess(item) 
                            : onError(item, e);
                    }
                );
            };

            self.start = function () {
                for (var num = 0; num < Math.min(self.batchSize, items.length); num++) {
                    tryStartNext();
                }
            };
        },
        CustomControlViewModel: function (objects, selectedUsers, underControl, onSuccess, onComplete) {

            var items = [];
            ko.utils.arrayForEach(objects, function (obj) {
                items = items.concat(
                    selectedUsers.map(function (user) {
                        return {
                            UserID: user.ID,
                            UnderControl: underControl,
                            Uri: obj.Uri,
                            Object: obj
                        };
                }));
            });

            module.ViewModelBase.call(
                this,
                items, {
                ajax: { contentType: 'application/json', dataType: 'JSON', method: 'POST' },
                div: null
            });

            this._getData = function (item) {
                return JSON.stringify({
                    UserID: item.UserID,
                    UnderControl: item.UnderControl
                });
            }
            this._getUrl = function (item) {
                return item.Uri + '/customcontrols/'
            }

            if (typeof onComplete === 'function') {
                this._onComplete = onComplete;
            }

            if (typeof onSuccess === 'function') {
                this._onSuccess = onSuccess;
            }
        },
        MyCustomControlViewModel: function (objects, underControl, onSuccess, onComplete) {

            module.ViewModelBase.call(
                this,
                objects.map(function (obj) { return { UnderControl: underControl, Uri: obj.Uri, Object: obj }; }), {
                ajax: { contentType: 'application/json', dataType: 'JSON', method: 'PUT' },
                div: null
            });

            this._getData = function (item) {
                return JSON.stringify({
                    UnderControl: item.UnderControl
                });
            }
            this._getUrl = function (item) {
                return item.Uri + '/customcontrols/my'
            }

            if (typeof onComplete === 'function') {
                this._onComplete = onComplete;
            }

            if (typeof onSuccess === 'function') {
                this._onSuccess = onSuccess;
            }
        },

        Status: { 
            Success: 0,
            Error: 1
        },
        DefaultBatchSize: 10,
        DeleteAjaxOptions: {
            method: 'DELETE',
            dataType: 'text',
            contentType: 'application/json'
        },
        PostAjaxOptions: {
            method: 'POST',
            dataType: 'json',
            contentType: 'application/json'
        },
    };

    return module;
});