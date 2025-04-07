define(['knockout', 'jquery'], function (ko, $) {
    var module = {
        contextMenuItem: function () {
            var self = this;
            //
            self.text = ko.observable('');
            self.restext = ko.pureComputed({
                read: function () {
                    return '';
                },
                write: function (value) {
                    self.text(getTextResource(value));
                }
            });
            self.children = ko.observableArray([]);
            //
            self.enabled = ko.observable(true);
            self.visible = ko.observable(true);
            self.isSeparator = ko.observable(false);
            //
            self.click = ko.observable(function () { console.log('click at ' + self.text); });
            //
            return self;
        },
        contextMenu: function (options, element) {
            var self = this;
            //
            {//items of menu
                self.items = ko.observableArray([]);
                self.visibleItems = ko.pureComputed(function () {
                    var list = self.items();
                    var retval = [];
                    var previousItemIsSeparator = false;
                    list.forEach(function (item) {
                        if (item.visible() === true) {
                            if (previousItemIsSeparator === false || previousItemIsSeparator === true && item.isSeparator() === false) {
                                retval.push(item);
                                previousItemIsSeparator = (item.isSeparator() === true);
                            }
                        }
                    });
                    if (retval.length > 0 && retval[0].isSeparator() === true)
                        retval.splice(0, 1);
                    if (retval.length > 0 && retval[retval.length - 1].isSeparator() === true)
                        retval.splice(retval.length - 1, 1);
                    return retval;
                });
                self.visibleSelectableItems = ko.pureComputed(function () {
                    var list = self.visibleItems();
                    var retval = [];
                    list.forEach(function (item) {
                        if (item.visible() === true && item.enabled() === true && item.isSeparator() === false)
                            retval.push(item);
                    });
                    return retval;
                });
                self.itemClick = function (item, e) {
                    if (item.enabled() === false)
                        return;
                    //
                    item.click()();
                    self.close();
                };
                self.focusedItem = ko.observable(null);
                //
                self.addContextMenuItem = function () {
                    var item = new module.contextMenuItem();
                    self.items().push(item);
                    self.items.valueHasMutated();
                    return item;
                };

                self.unshiftContextMenuItem = function () {
                    var item = new module.contextMenuItem();
                    self.items().unshift(item);
                    self.items.valueHasMutated();
                    return item;
                };

                self.addSeparator = function () {
                    var item = new module.contextMenuItem();
                    item.isSeparator(true);
                    item.enabled(false);
                    self.items().push(item);
                    self.items.valueHasMutated();
                    return item;
                };
                //
                self.removeItem = function (item) {
                    var index = self.items().indexOf(item);
                    if (index != -1) {
                        item.restext.dispose();
                        //
                        self.items().splice(index, 1);
                        self.items.valueHasMutated();
                    }
                };
                self.clearItems = function () {
                    self.items().forEach(function (item) {
                        item.restext.dispose();
                    });
                    self.items([]);
                };
            }
            self.isItemFocused = function(item) {
                if (self.focusedItem() == item) return true;
                for (var i = 0; i < item.children().length; i++) {
                    if (self.isItemFocused(item.children()[i])) return true;
                }
                return false;
            }
            //
            self.dispose = function () {
                if (self.isVisible === true)
                    closeRegions();
                //
                self.clearItems();
                self.visibleItems();//to recalc
                self.visibleItems.dispose();
                self.visibleSelectableItems();//to recalc
                self.visibleSelectableItems.dispose();
                //
                //var $control = $('#' + element.id);
                //$control.remove();
            };
            //
            {//open and close
                self.isVisible = false;
                self._previousActiveElement = null;
                self.show = function (e) {
                    closeRegions();
                    self.focusedItem(null);
                    //
                    options.opening()(self);//to prepare all items
                    if (self.visibleItems().length === 0)
                        return false;
                    //
                    var $control = $('#' + element.id).find('.ko_ContextMenu');
                    if ($control.length > 0) {
                        if ($control[0].id == '')//set id, one time
                            $control[0].id = '_' + element.id;
                        //
                        $('#controlsContainer').append($control);//move fake container
                        //
                        var menuHeight = $control.outerHeight(true);
                        var menuWidth = $control.outerWidth(true);
                        var windowWidth = $(window).width();
                        var windowHeight = $(window).height();
                        var x = e.clientX;
                        if (x + menuWidth > windowWidth)
                            x = windowWidth - menuWidth;
                        var y = e.clientY;
                        if (y + menuHeight > windowHeight)
                            y = windowHeight - menuHeight;
                        $control.css('left', x).css('top', y);
                        //
                        openRegion($control, e, self.close);
                        self._previousActiveElement = document.activeElement;
                        $control.focus();
                        self.isVisible = true;
                        return true;
                    }
                    else
                        return false;
                };
                self.close = function () {                    
                    var $control = $('#_' + element.id);
                    $control.hide();
                    //
                    $('#' + element.id).append($control);//move to parent
                    //
                    self.isVisible = false;
                    self.focusedItem(null);
                    //
                    if (self._previousActiveElement != null)
                        self._previousActiveElement.focus();
                };
            }
            //
            {//events               
                self.mouseOver = function (item, e) {
                    if (self.isVisible === false)
                        return true;
                    //
                    self.focusedItem(item);
                    return true;
                };
                //
                self.onKeyDown = function (vm, e) {
                    if (self.isVisible === false)
                        return true;
                    //
                    var k = e.which || e.keyCode;
                    switch (k) {
                        case 36: {//36 - home
                            if (self.visibleSelectableItems().length > 0)
                                self.focusedItem(self.visibleSelectableItems()[0]);
                            else
                                self.focusedItem(null);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 38: {//38 - up                            
                            if (self.visibleSelectableItems().length > 0) {
                                var index = self.visibleSelectableItems().indexOf(self.focusedItem());
                                if (index === -1)
                                    self.focusedItem(self.visibleSelectableItems()[0]);
                                else if (index === 0)
                                    self.focusedItem(self.visibleSelectableItems()[self.visibleSelectableItems().length - 1]);
                                else if (index > 0)
                                    self.focusedItem(self.visibleSelectableItems()[index - 1]);
                            }
                            else
                                self.focusedItem(null);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 33: {//38 - pageUp
                            if (self.visibleSelectableItems().length > 0)
                                self.focusedItem(self.visibleSelectableItems()[0]);
                            else
                                self.focusedItem(null);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 40: {//40 - down
                            if (self.visibleSelectableItems().length > 0) {
                                var index = self.visibleSelectableItems().indexOf(self.focusedItem());
                                if (index === -1)
                                    self.focusedItem(self.visibleSelectableItems()[0]);
                                else if (index === self.visibleSelectableItems().length - 1)
                                    self.focusedItem(self.visibleSelectableItems()[0]);
                                else if (index < self.visibleSelectableItems().length - 1)
                                    self.focusedItem(self.visibleSelectableItems()[index + 1]);
                            }
                            else
                                self.focusedItem(null);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 34: {//34 - pagedown
                            if (self.visibleSelectableItems().length > 0)
                                self.focusedItem(self.visibleSelectableItems()[self.visibleSelectableItems().length - 1]);
                            else
                                self.focusedItem(null);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 35: {//35 - end
                            if (self.visibleSelectableItems().length > 0)
                                self.focusedItem(self.visibleSelectableItems()[self.visibleSelectableItems().length - 1]);
                            else
                                self.focusedItem(null);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 13: {//13 - enter
                            var item = self.focusedItem();
                            if (item != null)
                                self.itemClick(item, e);
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 27: {//27 - esc
                            closeRegions();
                            //
                            e.stopPropagation();
                            break;
                        }
                        case 116: {//f5
                            window.location.reload(true);
                            //
                            e.stopPropagation();
                            break;
                        }
                    }
                    return false;
                };
            }
            //
            //return link to control
            options.controlInitialized()(self);
        }
    }
    return module;
});