define(['knockout', 'jquery'], function (ko, $) {
    var module = {
        /*features:
            -расширяет модель списка набором сведений об изменных объектах для их дальнейшей подгрузки - modifiedObjectID, modifiedObjectCount, modifiedObjectCountString, loadModifiedObjects
            -расширяет модель списка функцией reloadObjectByID, накапливающей id в буффер и дергающей не чаще, чем раз в 5 сек функцию loadObjectListByIDs. Если часто дергать сервер по мелочам, СУБД перетрудится и учадет производительность!
            -расширяет модель списка функцией checkAvailabilityID, накапливающей id в буффер и дергающей раз в 30 сек функцию getObjectListByIDs, в результате чего накапливаются сведения об измененных объектах, для их дальнейшей подгрузки. Если часто дергать сервер по мелочам, СУБД перетрудится и учадет производительность!
        */
        init: function (tableModel) {
            /*Attention! 
              tableModel must constans:
                -isAjaxActive() - bool
                -loadObjectListByIDs(idArray, unshiftMode) - deferred of objectList
                -getObjectListByIDs(idArray) - deferred of objectList
                -getObjectID(object) - id of object
            */
            var self = tableModel;
            //
            {//mechanizm of notification
                self.modifiedObjectIDs = ko.observableArray([]);//количество измененных, но не отображенных элементов, которые могут быть интересны пользователю
                self.modifiedObjectCount = ko.pureComputed(function () {
                    return self.modifiedObjectIDs().length;
                });
                self.modifiedObjectCountString = ko.pureComputed(function () {
                    var retval = self.modifiedObjectIDs().length;
                    return retval > 9 ? '9+' : retval.toString();
                });
                //
                {//helper operations for array
                    self.addToModifiedObjectIDs = function (objectID) {
                        var index = self.modifiedObjectIDs().indexOf(objectID);
                        if (index == -1) {
                            self.modifiedObjectIDs().push(objectID);
                            self.modifiedObjectIDs.valueHasMutated();
                        }
                    };
                    self.removeFromModifiedObjectIDs = function (objectID) {
                        var index = self.modifiedObjectIDs().indexOf(objectID);
                        if (index > -1) {
                            self.modifiedObjectIDs().splice(index, 1);
                            self.modifiedObjectIDs.valueHasMutated();
                        }
                    };
                }
                //
                self.loadModifiedObjects = function () {//вызывается при клике на количество измененных объектов
                    var idList = JSON.parse(JSON.stringify(self.modifiedObjectIDs()));
                    self.loadObjectListByIDs(idList, true);
                };
            }
            //
            {//mechanizm of loading (or reloading) objects by id
                self.queueToReloadIDs = [];//очередь ожидания загрузки объектов
                {//helper operations for array
                    self.addToQueueToReloadIDs = function (objectID) {                        
                        var index = self.queueToReloadIDs.indexOf(objectID);
                        if (index === -1)
                            self.queueToReloadIDs.push(objectID);
                        return index === -1;
                    };
                    self.removeFromQueueToReloadIDs = function (objectID) {
                        var index = self.queueToReloadIDs.indexOf(objectID);
                        if (index > -1)
                            self.queueToReloadIDs.splice(index, 1);
                        return index > -1;
                    };
                }
                {//wait and check queue to load objects
                    self.reloadObjectsLastDate = new Date();//for block often refreshing
                    self.timer_queuetoReloadIDs = null;//for waiting refresh
                    self.timer_queuetoReloadIDs_milliseconds = 5000;//one time per 5 sec 
                    //
                    self.checkQueueToReloadIDs = function () {
                        if (self.timer_queuetoReloadIDs != null)
                            return;//wait for timeout
                        //
                        var dt = Math.abs((new Date()) - self.reloadObjectsLastDate);
                        if (dt >= self.timer_queuetoReloadIDs_milliseconds && self.isAjaxActive() === false) {
                            var cloneIDs = JSON.parse(JSON.stringify(self.queueToReloadIDs));
                            self.reloadObjectsLastDate = new Date();
                            //
                            $.when(self.loadObjectListByIDs(cloneIDs, true)).done(function (objectList) {
                                if (objectList) {
                                    for (var i = 0; i < cloneIDs.length; i++)//если какие-то объекты не вернулись, то и загружать их не имеет смысла
                                        self.removeFromQueueToReloadIDs(cloneIDs[i].toUpperCase());
                                }
                                else
                                    self.checkQueueToReloadIDs();
                            });
                        }
                        else
                            self.timer_queuetoReloadIDs = setTimeout(function () {
                                self.timer_queuetoReloadIDs = null;
                                self.checkQueueToReloadIDs();
                            }, Math.min(dt, self.timer_queuetoReloadIDs_milliseconds));
                    };
                }
                //
                self.reloadObjectByID = function (objectID) {
                    if (self.addToQueueToReloadIDs(objectID))
                        setTimeout(self.checkQueueToReloadIDs, 200);//wait before check for massive update                        
                };
            }
            //
            {//mechanizm of notifications about new objects
                self.queueToCheckAvailabilityIDs = [];//очередь ожидания загрузки доступности объектов, из-за наличия фильтра
                {//helper operations for array
                    self.addToQueueToCheckAvailabilityIDs = function (objectID) {
                        var index = self.queueToCheckAvailabilityIDs.indexOf(objectID);
                        if (index === -1)
                            self.queueToCheckAvailabilityIDs.push(objectID);
                    };
                    self.removeFromQueueToCheckAvailabilityIDs = function (objectID) {
                        var index = self.queueToCheckAvailabilityIDs.indexOf(objectID);
                        if (index > -1)
                            self.queueToCheckAvailabilityIDs.splice(index, 1);
                    };
                }
                {//wait and check queue to check availability
                    self.timer_queueToCheckAvailabilityIDs = null;
                    self.timer_queueToCheckAvailabilityIDs_milliseconds = 30000;//one time per 30 sec
                    //
                    self.checkAvailabilityIDs = function () {
                        if (self.isAjaxActive() === true) {
                            self.timer_queueToCheckAvailabilityIDs = setTimeout(self.checkAvailabilityIDs, self.timer_queueToCheckAvailabilityIDs_milliseconds);
                            return;
                        }
                        //
                        var cloneIDs = JSON.parse(JSON.stringify(self.queueToCheckAvailabilityIDs));
                        self.queueToCheckAvailabilityIDs = [];
                        //
                        $.when(self.getObjectListByIDs(cloneIDs)).done(function (objectList) {
                            var ids = [];
                            if (objectList)
                                for (var i = 0; i < objectList.length; i++)
                                    ids.push(self.getObjectID(objectList[i]));
                            //
                            for (var i = 0; i < cloneIDs.length; i++) {
                                var id = cloneIDs[i].toUpperCase();
                                var index = ids.indexOf(id);
                                if (index != -1)//is avaialble
                                    self.addToModifiedObjectIDs(id);
                                else
                                    self.clearInfoByObject(id);
                            }
                            //                
                            self.timer_queueToCheckAvailabilityIDs = setTimeout(self.checkAvailabilityIDs, self.timer_queueToCheckAvailabilityIDs_milliseconds);
                        });
                    };
                    self.checkAvailabilityIDs();//run timer
                }
                //
                self.checkAvailabilityID = function (objectID) {
                    self.addToQueueToCheckAvailabilityIDs(objectID);
                };
            }
            //
            self.clearAllInfos = function () {
                self.queueToCheckAvailabilityIDs = [];
                self.queueToReloadIDs = [];
                self.modifiedObjectIDs.removeAll();
            };
            self.clearInfoByObject = function (id) {
                id = id.toUpperCase();
                //
                self.removeFromModifiedObjectIDs(id);
                self.removeFromQueueToReloadIDs(id);
                self.removeFromQueueToCheckAvailabilityIDs(id);
            };
        }
    }
    return module;
});