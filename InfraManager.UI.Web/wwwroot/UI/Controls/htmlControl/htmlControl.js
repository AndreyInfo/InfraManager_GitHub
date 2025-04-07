define(['knockout', 'jquery', 'comboBox'], function (ko, $) {
    //read more https://developer.mozilla.org/ru/docs/Web/API/Document/execCommand

    var module = {
        //объект jQuery, в который добавляется контрол, расширенный редактор
        control: function (targetDiv, extendedMode, readOnly) {
            var self = this;
            //
            self.id = 'htmlControl_' + ko.getNewID();
            self.frameID = self.id + '_frame';
            //
            self.firstLoadD = $.Deferred();//используется извне, не трогать
            self.html = '';
            self.readOnly = ko.observable(readOnly ? true : false);
            self.frameD = null;//используется извне, не трогать
            self.frame = null;//сам редактор    
            self.koElem = null;//div, в который грузится контрол
            //
            self.getFrameDiv = function () {
                return $('#' + self.frameID);
            };
            self.getDoc = function () {
                if (!self.frame || !self.frame.contentWindow)
                    return null;
                return self.frame.contentWindow.document;
            };
            //
            {//extended menu height
                self.css = ko.observable('');
                self.refreshSize = function () {
                    if (extendedMode != true || self.readOnly() == true) {
                        self.css('');
                        return;
                    }
                    var $ctrl = $('#' + self.id);
                    var width = $ctrl.width();
                    if (self.browser_ie11 == true) {
                        if (width > 523)
                            self.css('h1');
                        else
                            self.css('h2');
                    }
                    else if (self.browser_edgeOrFirefox == true) {
                        if (width > 805)
                            self.css('h1');
                        else if (width > 405)
                            self.css('h2');
                        else
                            self.css('h3');
                    }
                    else {
                        if (width > 916)
                            self.css('h1');
                        else if (width > 500)
                            self.css('h2');
                        else
                            self.css('h3');
                    }
                };
                self.waitAndRefreshSize_timeout = null;
                self.waitAndRefreshSize = function () {
                    clearTimeout(self.waitAndRefreshSize_timeout);
                    self.waitAndRefreshSize_timeout = null;
                    self.waitAndRefreshSize_timeout = setTimeout(self.refreshSize, 100);
                };
                //
                $(window).bind('resize', self.waitAndRefreshSize);
                $(document).bind('form_sizeChanged', self.waitAndRefreshSize);
            }
            self.browser_ie11 = getIEVersion() == 11 ? true : false;
            self.browser_edgeOrFirefox = window.navigator.userAgent.indexOf("Edge/") > -1 || window.navigator.userAgent.indexOf("Firefox") > -1;
            //команды меню

            function contains(r, s) {
                return r.indexOf(s) > -1;
            }
            function SaveFormatPastedText(arg) {
                var text = '';
                if (contains(arg, '\r') || contains(arg, '\r\n') || contains(arg, '\n')) {
                    var array = arg.split(/\r?\n/);
                    for (let i = 0; i < array.length; i++) {
                        if (!array[i] || array[i] == undefined || array[i] == "" || array[i].length == 0 || array[i].length == array[i].split(' ').length - 1) {
                            if (array[i].split(' ').length - 1 > 1) {
                                text = text + '<div>';
                                for (let b = 0; b < array[i].split(' ').length - 1; b++) {
                                    text = text + '&nbsp';
                                }
                                text = text + '</div>';
                            }
                            else
                                text = text + '<div>' + '&nbsp' + '</div>';
                        }
                        else {
                            var ArrayChar = array[i].split('');
                            text = text + '<div>';
                            for (let c = 0; c < ArrayChar.length; c++) {
                                if (ArrayChar[c] === ' ')
                                    text = text + '&nbsp';
                                else
                                    text = text + ArrayChar[c];
                            }
                            text = text + '</div>';
                        }
                    }
                }
                else {
                    var ArrayChar = arg.split('');
                    text = text + '<div>';
                    for (let c = 0; c < ArrayChar.length; c++) {
                        if (ArrayChar[c] === ' ')
                            text = text + '&nbsp';
                        else
                            text = text + ArrayChar[c];
                    }
                    text = text + '</div>';
                }

                return text;
            }

            self.cmdFrame = function (cmdName, arg) {
                if (cmdName == 'paste' || cmdName == 'insertHTML' || cmdName == 'insertText') {
                    if (arg) {
                        if (contains(arg, '</div>') == false || contains(arg, 'div') == false) {
                            arg = SaveFormatPastedText(arg);
                        }
                    }
                }
                var doc = self.getDoc();
                if (doc == null || !doc.body)
                    return;
                //
                //if (document.queryCommandSupported(cmdName) == false) {
                //    alert('browser command not supported');
                //    return;
                //}
                doc.execCommand(cmdName, false, arg);
                doc.body.focus();
            };
            self.cmdUndoClick = function () { self.cmdFrame('undo'); };
            self.cmdRedoClick = function () { self.cmdFrame('redo'); };
            self.cmdBoldClick = function () { self.cmdFrame('bold'); };
            self.cmdItalicClick = function () { self.cmdFrame('italic'); };
            self.cmdUnderlineClick = function () { self.cmdFrame('underline'); };
            self.cmdCutClick = function () { self.cmdFrame('cut'); };
            self.cmdCopyClick = function () { self.cmdFrame('copy'); };
            self.cmdPasteClick = function () { self.PasteFromClipboard(); };
            self.cmdNumbersClick = function () { self.cmdFrame('insertorderedlist'); };
            self.cmdBulletsClick = function () { self.cmdFrame('insertunorderedlist'); };
            self.cmdAlignLeftClick = function () { self.cmdFrame('justifyleft'); };
            self.cmdAlignCenterClick = function () { self.cmdFrame('justifycenter'); };
            self.cmdAlignJustifyClick = function () { self.cmdFrame('justifyfull'); };
            self.cmdAlignRightClick = function () { self.cmdFrame('justifyright'); };
            self.cmdRightClick = function () { self.cmdFrame('indent'); };
            self.cmdLeftClick = function () { self.cmdFrame('outdent'); };
            self.cmdSetBackgroundClick = function (m, e) { self.openPalette(m, e); };
            self.cmdSetColorClick = function (m, e) { self.openPalette(m, e); };
            //
            //контролы шрифтов
            self.fonts = [];
            {
                self.fonts.push({ ID: 0, Name: 'Arial', Style: 'font-family:arial, sans-serif;' });
                self.fonts.push({ ID: 1, Name: 'Arial Black', Style: 'font-family:"arial black", sans-serif;' });
                self.fonts.push({ ID: 2, Name: 'Arial Narrow', Style: 'font-family:"arial narrow", sans-serif;' });
                self.fonts.push({ ID: 3, Name: 'Comic Sans MS', Style: 'font-family:"comic sans ms", sans-serif;' });
                self.fonts.push({ ID: 4, Name: 'Garamond', Style: 'font-family:garamond, "times new roman", serif;' });
                self.fonts.push({ ID: 5, Name: 'Helvetica', Style: 'font-family:Helvetica, sans-serif;' });
                self.fonts.push({ ID: 6, Name: 'Monoscape', Style: 'font-family:monospace;' });
                self.fonts.push({ ID: 7, Name: 'Roboto', Style: 'font-family:Roboto, RobotoDraft, sans-serif;' });
                self.fonts.push({ ID: 8, Name: 'Tahoma', Style: 'font-family:tahoma, sans-serif;' });
                self.fonts.push({ ID: 9, Name: 'Times New Roman', Style: 'font-family:"times new roman", serif;' });
                self.fonts.push({ ID: 10, Name: 'Trebuchet MS', Style: 'font-family:"trebuchet ms", sans-serif' });
                self.fonts.push({ ID: 11, Name: 'Verdana', Style: 'font-family:verdana, sans-serif;' });
            }
            self.getFontFamilyList = function (options) {
                var data = self.fonts;
                options.callback({ data: data, total: data.length });
            };
            self.selectedFontFamily = ko.observable(self.fonts[0]);
            self.selectedFontFamily.subscribe(function (newValue) {
                self.cmdFrame('fontName', newValue.Name);
            });
            self.fontSizes = [];
            {//only 7 values - maximum ofr execCommand
                self.fontSizes.push({ ID: 1, Name: '8', Style: 'font-size:8px;' });
                self.fontSizes.push({ ID: 2, Name: '10', Style: 'font-size:10px;' });
                self.fontSizes.push({ ID: 3, Name: '12', Style: 'font-size:12px;' });
                self.fontSizes.push({ ID: 4, Name: '14', Style: 'font-size:14px;' });
                self.fontSizes.push({ ID: 5, Name: '16', Style: 'font-size:16px;' });
                self.fontSizes.push({ ID: 6, Name: '20', Style: 'font-size:20px;' });
                self.fontSizes.push({ ID: 7, Name: '24', Style: 'font-size:24px;' });
            }
            self.getFontSizeList = function (options) {
                var data = self.fontSizes;
                options.callback({ data: data, total: data.length });
            };
            self.selectedFontSize = ko.observable(self.fontSizes[4]);
            self.selectedFontSize.subscribe(function (newValue) {
                self.cmdFrame('fontSize', newValue.ID);
            });
            //
            //контролы палитры
            self.rgb2hex = function (rgb) {
                rgb = rgb.match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);
                return (rgb && rgb.length === 4) ? "#" +
                    ("0" + parseInt(rgb[1], 10).toString(16)).slice(-2) +
                    ("0" + parseInt(rgb[2], 10).toString(16)).slice(-2) +
                    ("0" + parseInt(rgb[3], 10).toString(16)).slice(-2) : '';
            };
            self.setColorCommandName = null;
            self.setColorClick = function (m, e) {
                var color = e.target.style.backgroundColor;
                self.cmdFrame('styleWithCSS', true);
                self.cmdFrame(self.setColorCommandName, self.rgb2hex(color));
                self.cmdFrame('styleWithCSS', false);
                closeRegions();
            };
            self.openPalette = function (m, e) {
                var $control = $(self.koElem);
                var $panel = $control.find('.htmlControl-palette')
                if (e.target.className == 'cmd-background')
                    self.setColorCommandName = 'backColor';
                else if (e.target.className == 'cmd-color')
                    self.setColorCommandName = 'foreColor';
                openRegion($panel, e);
                {
                    var controlOffset = $control.offset();
                    var $button = $(e.target);
                    var buttonOffset = $button.offset();
                    var buttonHeight = $button.height();
                    if ($control.length > 0 && $button.length > 0 && $.contains($control[0], $button[0])) {
                        var top = 2 * (buttonOffset.top - controlOffset.top) + buttonHeight;
                        var left = buttonOffset.left - controlOffset.left;
                        $panel.css('top', top + 'px');
                        $panel.css('left', left + 'px');
                    }
                }
            };
            //
            //отрисовка редактора
            self.afterRender = function () {
                var frameContainerDiv = targetDiv.find('.htmlEditor-full .htmlEditor-frameContainer');
                self.createFrame(frameContainerDiv);
                //
                $(self.koElem).css('display', '');
                self.firstLoadD.resolve();//не менять местами с предыдущей строчкой
                //
                self.waitAndRefreshSize();
            };
            self.createFrame = function (frameContainerDiv) {
                self.frame = document.createElement('iframe');
                self.frame.id = self.frameID;
                //
                self.frame.frameBorder = 0;
                self.frame.className = 'htmlEditor-frame';
                self.frame.onload = function (e) {
                    var doc = self.getDoc();
                    //
                    if (doc == null || !doc.body)
                        return;//destroy, invoke because moved in DOM
                    //
                    var newDoctype = document.implementation.createDocumentType('html', '', '');
                    doc.insertBefore(newDoctype, doc.childNodes[0]);
                    //
                    self.initializeFrame(doc);
                    //
                    if (self.frameD.state() == 'resolved') //moved in DOM (cause: jQuery dialog, in formControl)                       
                        self.SetHTML(self.html);
                    else
                        self.frameD.resolve();
                };
                frameContainerDiv.append(self.frame);
                self.SetHTML(self.html);
                //
                //fireFox height problem
                setTimeout(function () {
                    var tmp = $(frameContainerDiv.selector);
                    if (tmp.length > 0 && tmp.height() == 0) {
                        tmp.css('height', '1px');
                        setTimeout(function () {
                            tmp.css('height', '');
                        }, 500);
                    }
                }, 500);
            };
            self.initializeDocument = function (doc) {
                doc.designMode = self.readOnly() ? 'Off' : 'On';
                if (doc.body == null) // null in IE 10 after On designMode
                    doc.write('<head></head><body></body>');
                //                
                doc.body.contentEditable = !self.readOnly();
            };
            self.initializeFrame = function (doc) {
                var window = $(self.frame.contentWindow);
                if (window.length == 0)
                    return;
                //
                self.initializeDocument(doc);
                //
                var serverPath = $(document.head).find('link[href*="Styles/redesign.min.css"]')[0].href;
                serverPath = serverPath.substring(0, serverPath.indexOf('Styles/redesign.min.css'));
                //
                var headScripts = '<link type="text/css" rel="Stylesheet" href="' + serverPath + 'Styles/redesign.min.css" />';
                {//ie bug
                    var ieVer = getIEVersion();
                    if (ieVer == 10)
                        headScripts += '<style type="text/css">.htmlEditor-textArea { margin:0; }</style>';
                }
                //
                doc.head.innerHTML = headScripts;
                doc.body.className = 'htmlEditor-textArea';
                $(doc).bind('click', closeRegions);
                //
                self.setFrameSize();

                //edit events
                {
                    var storeHtml = function (e) {
                        self.setFrameSize();
                        //
                        var currentDoc = self.getDoc();
                        if (currentDoc == null || !currentDoc.body)
                            return;
                        //
                        var changed = (self.html != currentDoc.body.innerHTML);
                        self.html = currentDoc.body.innerHTML;
                        if (self.OnHTMLChanged && changed)
                            self.OnHTMLChanged(self.html);
                    };
                    var processImageInsertion = function (clipboardData){
                        if (clipboardData && clipboardData.items) {
                            var items = clipboardData.items;
                            var tmp = null;
                            for (var i = 0; i < items.length; i++) {
                                var item = items[i];
                                if (item.type.match('^image/')) {
                                    tmp = item.getAsFile();
                                    break;
                                }
                            }
                            if (tmp !== null) {
                                var reader = new FileReader();
                                reader.onload = function (event) {
                                    self.cmdFrame('insertHTML', '<div><img src="' + event.target.result + '"/></div>');
                                };
                                reader.readAsDataURL(tmp);
                            }
                        }
                    };
                    var onpaste = function (event) {
                        event.preventDefault();
                        self.PasteFromClipboard(event.clipboardData || event.originalEvent.clipboardData);
                        setTimeout(storeHtml, 1000);//wait insertHTML command  
                    }

                    //
                    window.keydown(function (e) {
                        var retval = true;
                        if (self.OnKeyDown)
                            retval = self.OnKeyDown(e);
                        //
                        return retval;
                    });
                    window.keyup(storeHtml);
                    window.bind('paste', null, onpaste);
                }
                //image resize events
                {
                    var imageResizeData = null;
                    window.bind('mouseenter', function (e) {
                        if (e.buttons == 0)
                            imageResizeData = null;
                    });
                    window.bind('mousedown', function (e) {
                        if (!e.target || e.target.localName != 'img')
                            return;
                        imageResizeData = {
                            x: e.clientX,
                            y: e.clientY,
                            realWidth: e.target.width,
                            realHeight: e.target.height,
                            target: e.target
                        };
                        e.preventDefault();
                    });
                    window.bind('mouseup', function () {
                        imageResizeData = null;
                    });
                    window.bind('mousemove', function (e) {
                        if (imageResizeData) {
                            var dx = e.clientX - imageResizeData.x;
                            var dy = e.clientY - imageResizeData.y;
                            imageResizeData.x = e.clientX;
                            imageResizeData.y = e.clientY;
                            //
                            var newWidth = 0;
                            var newHeight = 0;
                            if (dx != 0) {
                                newWidth = imageResizeData.realWidth + dx;
                                newHeight = (imageResizeData.realWidth + dx) * imageResizeData.realHeight / imageResizeData.realWidth;
                            }
                            else if (dy != 0) {
                                newHeight = imageResizeData.realHeight + dy;
                                newWidth = (imageResizeData.realHeight + dy) * imageResizeData.realWidth / imageResizeData.realHeight;
                            }
                            //
                            if (newWidth > 10 && newHeight > 10) {
                                imageResizeData.realWidth = newWidth;
                                imageResizeData.realHeight = newHeight;
                                //
                                imageResizeData.target.width = newWidth;
                                imageResizeData.target.height = newHeight;
                            }
                        }
                    });
                    window.bind('click', function (e) {
                        if (!e.target || e.target.localName != 'img' || !e.ctrlKey)
                            return;
                        self.openImageInNewWindow(e.target.src);
                    });
                }
            };
            //
            self.GetFrameChildMaxBottomY = function () {
                var currentDoc = self.getDoc();
                //
                if (self.frame == null || currentDoc == null || !currentDoc.body)
                    return null;
                //
                var childNodes = currentDoc.body.childNodes;
                if (!childNodes)
                    return;
                //
                var rect = null;
                //
                var maxHeight = 0;
                //
                for (var i = childNodes.length - 1; i >= 0; i--) {
                    var el = childNodes[i];
                    if (!el.tagName)//text
                    {
                        var range = document.createRange();
                        range.selectNode(el);
                        rect = range.getBoundingClientRect();
                        range.detach();
                    }
                    else if (el.getBoundingClientRect) {
                        rect = el.getBoundingClientRect();
                    }
                    //
                    var height = Math.max(rect.height, el.scrollHeight ? el.scrollHeight : 0);
                    var topY = 0;
                    //
                    if (self.browser_ie11) {
                        topY = rect.top;
                    }
                    else
                        topY = rect.y;
                    //
                    var padding = 0;
                    var margin = 0;
                    if (el.style) {
                        var paddingBottom = el.style.paddingBottom;
                        var marginBottom = el.style.marginBottom;
                        if (paddingBottom && marginBottom.indexOf("px") !== -1)
                            padding += parseInt(paddingBottom.replace('px', ''));
                        if (marginBottom && marginBottom.indexOf("px") !== -1)
                            margin += parseInt(marginBottom.replace('px', ''));
                    }
                    //
                    var bottomY = 0;
                    if ((topY || topY === 0) && (height || height === 0)) {
                        if (topY >= 0) {
                            bottomY = Math.ceil(currentDoc.body.scrollTop + topY + height + padding + margin);
                        }
                        else {
                            bottomY = Math.ceil(currentDoc.body.scrollTop - Math.abs(topY) + Math.abs(height) + padding + margin);
                        }
                    }
                    //
                    maxHeight = Math.max(maxHeight, bottomY);
                }
                return maxHeight;
            };
            //
            self.GetFrameScrollHeight = function () {
                var currentDoc = self.getDoc();
                if (self.frame == null || currentDoc == null || !currentDoc.body || !currentDoc.body.lastChild)
                    return;
                //
                return currentDoc.body.scrollHeight;
            };
            //
            self.fitHeight = null;
            //
            //у контрола есть три варианта поведения: 
            //1. размер контрола фиксирован (если содержимое не помещается, то появляется скролл)
            //2. высота контрола меняется в зависимости от объема содержимого, но при этом ограничена максимальной величиной 200px. (использовать htmlEditor-extensionContainer)
            //3. высота контрола определяется высотой содержимого (использовать htmlEditor-extensionContainerFull). в этом случае выставляем высоту контрола равной положению самого нижнего элемента в html.
            //   в общем случае этот элемент html может быть не последним в разметке, поэтому необходимо пробежать по всем элементам.
            //   выставлять высоту исходя из scrollHeight в этом случае не можем потому, что отрисовка html в iframe происходит в несколько этапов: сначала html отображается, а потом его размер
            //   автоматически масштабируется под ширину iframe. поэтому scrollHeight, установленный сразу после отрисовки, задает неправильную высоту. (события, которое указывало бы на окончание масштабирования, не существует) 
            self.setFrameSize = function () {
                if (!targetDiv.hasClass('htmlEditor-extensionContainer') && !targetDiv.hasClass('htmlEditor-extensionContainerFull'))
                    return;
                var currentDoc = self.getDoc();
                if (self.frame == null || currentDoc == null || !currentDoc.body || !currentDoc.body.lastChild)
                    return;
                //
                if (targetDiv.hasClass('htmlEditor-extensionContainer')) {
                    self.frame.style.height = '0px';//hack
                    self.frame.style.height = currentDoc.body.scrollHeight + 'px';
                }
                else if (targetDiv.hasClass('htmlEditor-extensionContainerFull')) {
                    var offset = 16;
                    var _realHeight = self.GetFrameChildMaxBottomY();
                    if (_realHeight)
                        self.frame.style.height = _realHeight + offset + 'px';
                    //
                    if (!self.fitHeight) {
                        self.fitHeight = setInterval(function () {
                            var realHeight = self.GetFrameChildMaxBottomY();
                            var scrollHeight = self.GetFrameScrollHeight();
                            //
                            if (scrollHeight !== realHeight + offset)
                                self.setFrameSize();
                        }, 100);
                    }
                }
            };
            self.openImageInNewWindow = function (src) {
                var wnd = window.open();
                if (wnd) {
                    wnd.document.write('<img src="' + src + '"/>');
                    //wnd.focus(); not work
                }
                return false;
            };
            //            
            //
            self.Load = function (newTargetDiv) {
                targetDiv = newTargetDiv;
                self.frameD = $.Deferred();
                //
                if (self.frame)
                    $(self.frame).remove();
                if (self.koElem) {
                    ko.cleanNode(self.koElem);
                    $(self.koElem).remove();
                }
                //
                targetDiv.append('<div id="' + self.id + '" class="htmlEditor-full" style="display:none;" data-bind="css: css, template: {name: \'../UI/Controls/htmlControl/htmlControl\', afterRender: afterRender}"></div>');
                self.koElem = document.getElementById(self.id);
                ko.applyBindings(self, self.koElem);
                //wait afterRender
            };
            self.SetHTML = function (htmlValue) {
                var htmlChanged = (self.html != htmlValue);
                self.html = htmlValue;
                //
                $.when(self.firstLoadD).done(function () {
                    $.when(self.frameD).done(function () {
                        var doc = self.getDoc();
                        if (doc == null || !doc.body || doc.body.innerHTML == self.html)
                            return;
                        //
                        doc.body.innerHTML = self.html;
                        doc.body.innerHTML = self.tagCleaner.process(doc.body.innerHTML);
                        self.setFrameSize();
                        //
                        if (self.OnHTMLChanged && htmlChanged)
                            self.OnHTMLChanged(self.html);
                    });
                });
            };
            self.GetHTML = function () {
                var div = self.getFrameDiv();
                if (div.contents().find('body').children().length == 0 && div.contents().find('body').innerHTML == '')
                    return self.html;//can't get, body not load
                //
                var doc = self.getDoc();
                if (doc == null || !doc.body)
                    return;
                doc.body.innerHTML = self.tagCleaner.process(doc.body.innerHTML);
                var retval = doc.body.innerHTML;
                return retval;
            };
            self.IsEmpty = function () {
                var html = self.GetHTML();
                if (!html || html == '') //chrome
                    return true;
                else if (html == '<br>') //firefox
                    return true;
                else if (html == '<p><br></p>') //ie
                    return true;
                //
                return false;
            };
            self.SetReadOnly = function (value) {
                self.readOnly(value);
                self.waitAndRefreshSize();
                //
                $.when(self.firstLoadD).done(function () {
                    $.when(self.frameD).done(function () {
                        var doc = self.getDoc();
                        if (doc == null || !doc.body)
                            return;
                        //
                        self.initializeDocument(doc);
                    });
                });
            };
            self.Focus = function () {
                $.when(self.firstLoadD).done(function () {
                    $.when(self.frameD).done(function () {
                        var doc = self.getDoc();
                        if (doc == null || !doc.body)
                            return;
                        setTimeout(function () { doc.body.focus(); }, 500);
                    });
                });
            };
            self.Dispose = function () {
                //TODO window unbind...
                //
                $(window).unbind('resize', self.waitAndRefreshSize);
                $(document).unbind('form_sizeChanged', self.waitAndRefreshSize);
            };
            //
            self.OnHTMLChanged = null;  
            self.OnKeyDown = null;
            //
            self.Load(targetDiv);

            self.PasteFromClipboard = function (clipboardData) {
                if (self.getDoc() == null) {
                    return;
                }

                const image = '^image/';
                const html = 'text/html';
                const plain = 'text/plain';

                let promise = $.Deferred();
                let reader = new FileReader();
                reader.onload = function (event) {
                    if (event && event.target && event.target.result) {
                        promise.resolve('<div><img src="' + event.target.result + '"/></div>');
                    }
                };

                $.when(promise).done(function (text) {
                    self.cmdFrame('insertHTML', self.tagCleaner.process(text) || "");
                });

                if (clipboardData && clipboardData.items) {
                    if (clipboardData.types.indexOf(html) > -1) {
                        promise.resolve(clipboardData.getData(html));
                    } else if (clipboardData.types.indexOf(plain) > -1) {
                        promise.resolve(clipboardData.getData(plain));
                    } else {
                        for (let i = 0; i < clipboardData.items.length; i++) {
                            let item = clipboardData.items[i];
                            if (item.type.match('^image/')) {
                                reader.readAsDataURL(item.getAsFile());
                                break;
                            }
                        }
                    }
                } else if (navigator.clipboard) {
                    let contentType = null;
                    navigator.clipboard.read().then(function (clipboardItems) {
                        let clipboardItem = clipboardItems[0];
                        if (clipboardItem.types.indexOf(html) > -1) { contentType = html; }
                        else if (clipboardItem.types.indexOf(plain) > -1) { contentType = plain; }
                        else {
                            for (let i = 0; i < clipboardItem.types.length; i++) {
                                if (clipboardItem.types[i].match(image)) {
                                    clipboardItem.getType(clipboardItem.types[i]).then(function (blob) {
                                        reader.readAsDataURL(blob);
                                    });
                                    break;
                                }
                            }
                        }

                        if (contentType !== null) {
                            clipboardItem.getType(contentType).then(function (blob) {
                                new Response(blob).text().then(function (text) {
                                    promise.resolve(text);
                                });
                            });
                        }
                    });
                } else if (window.clipboardData) {
                    promise.resolve(window.clipboardData.getData('Text'));
                }
            };

            /**@class
             * @name EventHandlerAttributesCleaner - cleaning event attributes from html tags, remove 'data-bind' attribute */
            self.tagCleaner = (function () {

                function EventHandlerAttributesCleaner() { }

                EventHandlerAttributesCleaner.prototype.process = function (text) {
                    if (checkIfContainsTag.call(this, text))
                        return cleanTags.call(this, text);
                    return text;
                }

                EventHandlerAttributesCleaner.prototype.processElseThrow = function (text) {
                    if (checkIfContainsTag.call(this, text))
                        return cleanTagsElseThrow.call(this, text);
                    return text;
                }

                function checkIfContainsTag(text) {
                    var isHTML = RegExp.prototype.test.bind(/(<([^>]+)>)/i);
                    return isHTML(text);
                }

                function cleanTags(text) {
                    let result = '';
                    try {
                        result = removeAllHtmlCommentsFrom(text)
                        result = removeAllDataBindFrom(result);
                        result = removeEventHandlerAttributesFrom(result);
                    }
                    catch (e) {
                        if (result === undefined || result === '') result = text;
                    }
                    return result;
                }

                function cleanTagsElseThrow(text) {
                    return removeEventHandlerAttributesFrom( removeAllDataBindFrom( removeAllHtmlCommentsFrom(text)));
                }

                function removeAllHtmlCommentsFrom(text) {
                    var result = '';
                    try {
                        result = text.replace(/(<!--.*?-->)|(<!--[\S\s]+?-->)|(<!--[\S\s]*?$)/g, '');
                    }
                    catch (e) { result = text; }
                    return result;
                }

                function removeAllDataBindFrom(htmlString) {
                    var stringArr = htmlString.split(/<|&lt;/);
                    for (var i = 0; i < stringArr.length; i++) {
                        if (stringArr[i].at(0) !== '/') {
                            stringArr[i] = removeDataBindFrom(stringArr[i]);
                        }
                    }
                    return stringArr.join('<');
                }

                function removeDataBindFrom(tagString) {
                    var result = tagString;
                    if (containsDataBind(tagString)) {
                        result = cleanDataBind(tagString);
                    }
                    return result;
                }

                function containsDataBind(tagString) {
                    return -1 !== tagString.search(/data-bind/);
                }

                function cleanDataBind(tagString) {
                    var split = tagString.split('data-bind');
                    split[1] = split[1].trim();
                    var split2 = split[1].split('"');
                    split[1] = split2[2];
                    for (var i = 3; i < split2.length; i++) {
                        split[1] = split[1].concat('"').concat(split2[i]);
                    }
                    return split.join('');
                }

                function removeEventHandlerAttributesFrom(htmlString) {
                    var docFragment = createDocumentFragmentFrom(htmlString);
                    var div = document.createElement('div');
                    div.appendChild(docFragment.cloneNode(true));
                    removeEventHandlerAttributes(
                        removeScriptElementsFrom( removeButtonElementsFrom(div))
                    );
                    return div.innerHTML;
                }

                function removeScriptElementsFrom(rootElement) {
                    return removeElementsByNameFrom(rootElement, 'script');
                }

                function removeButtonElementsFrom(rootElement) {
                    return removeElementsByNameFrom(rootElement, 'button');
                }

                function removeElementsByNameFrom(rootElement, elementName) {
                    if (elementName === undefined || elementName === '') return rootElement;
                    var scripts = rootElement.getElementsByTagName(elementName);
                    var idx = scripts.length;
                    if (scripts !== undefined && idx !== undefined) {
                        while (idx--) scripts[idx].parentNode.removeChild(scripts[idx]);
                    }
                    return rootElement;
                }

                function removeEventHandlerAttributes(root) {
                    if (root === undefined) {
                        return;
                    }
                    if (containsEventHandler(root.attributes)) {
                        removeEventHandler(root.attributes);
                    }
                    if (root.children === undefined) {
                        return;
                    }
                    while (traverseAndCheck(root.children)) {
                        traverseAndRemove(root.children);
                    }
                }

                function createDocumentFragmentFrom(htmlString) {
                    var t = document.createElement('template');
                    t.innerHTML = htmlString;
                    return t.content;
                }

                function traverseAndCheck(children) {
                    if (children === undefined) {
                        return false;
                    }
                    for (var i = 0; i < children.length; i++) {
                        if (containsEventHandler(children[i].attributes)) {
                            return true;
                        }
                        return traverseAndCheck(children[i].children);
                    }
                }

                function containsEventHandler(attributes) {
                    var result = false;
                    if (typeof attributes !== 'undefined' && attributes) {
                        for (var i = 0; i < attributes.length; i++) {
                            result = attributes[i].name.search("on") === 0;
                            if (result) break;
                        }
                    }
                    return result;
                }

                function traverseAndRemove(children) {
                    if (children === undefined) {
                        return;
                    }
                    for (var i = 0; i < children.length; i++) {
                        removeEventHandler(children[i].attributes);
                        traverseAndRemove(children[i].children);
                    }
                }

                function removeEventHandler(attributes) {
                    if (typeof attributes === undefined) {
                        return;
                    }
                    for (i = 0; i < attributes.length; i++) {
                        var name = attributes[i].name;
                        if (name.search("on") === 0) {
                            attributes.removeNamedItem(name);
                        }
                    }
                }

                return new EventHandlerAttributesCleaner();
            }());
        }
    }
    return module;
});