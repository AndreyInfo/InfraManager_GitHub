define(['knockout', 'jquery', 'formControl', 'ui_controls/ContextMenu/ko.ContextMenu'],
function (ko, $, formControl) {
    var module = {
        FormatError: function (text, args) {
            for (var idx = 0; idx < args.length; idx++) {
                text = text.replace(`{${idx}}`, args[idx]);
            }

            return text;
        },
        OperatorIcons: function (operator) {
            switch (operator) {
                case "+": return "plus";
                case "-": return "minus";
                case "*": return "multiplier";
                case "/": return "division";
                case "%": return "division-remainder";
                default: return null;
            }
        },
        TextAreaProperties: [
            'boxSizing',
            'width',  // on Chrome and IE, exclude the scrollbar, so the mirror div wraps exactly as the textarea does
            'height',
            'overflowX',
            'overflowY',  // copy the scrollbar for IE

            'borderTopWidth',
            'borderRightWidth',
            'borderBottomWidth',
            'borderLeftWidth',

            'paddingTop',
            'paddingRight',
            'paddingBottom',
            'paddingLeft',

            // https://developer.mozilla.org/en-US/docs/Web/CSS/font
            'fontStyle',
            'fontVariant',
            'fontWeight',
            'fontStretch',
            'fontSize',
            'lineHeight',
            'fontFamily',

            'textAlign',
            'textTransform',
            'textIndent',
            'textDecoration',  // might not make a difference, but better be safe

            'letterSpacing',
            'wordSpacing'
        ],
        ViewModel: function (options, form) {
            var self = this;

            self.caption = ko.observable(options.caption);
            self.editorLegendText = ko.observable(options.legend);

            function convertStatements(list) {
                return list.map(function (item) { return { value: item.Name, title: item.Description } });
            }

            function convertOperators(list) {
                return list.map(
                    function (item) {
                        return {
                            title: item.Description,
                            icon: module.OperatorIcons(item.Name),
                            value: item.Name
                        }
                    });
            }

            self.variables = ko.observableArray(convertStatements(options.statements.Variables));
            self.functions = ko.observableArray(convertStatements(options.statements.Functions));
            self.operators = ko.observableArray(convertOperators(options.statements.Operators));

            self.validationError = ko.observable(null);
            self.hasError = ko.pureComputed(function () { return self.validationError() });

            self.expression = ko.observable(options.expression || '');
            self.textarea = null;
            self.add = function (statement) {              
                self.expression(
                    self.textarea.selectionStart && self.textarea.selectionEnd
                        ? self.expression().substr(0, self.textarea.selectionStart)
                            + statement.value
                            + self.expression().substr(self.textarea.selectionEnd)
                        : self.expression() + statement.value);
            };

            function getCaretCoordinates(element, position) {
                // mirrored div
                mirrorDiv = document.getElementById(element.nodeName + '--mirror-div');
                if (!mirrorDiv) {
                    mirrorDiv = document.createElement('div');
                    mirrorDiv.id = element.nodeName + '--mirror-div';
                    document.body.appendChild(mirrorDiv);
                }

                style = mirrorDiv.style;
                computed = getComputedStyle(element);

                // default textarea styles
                style.whiteSpace = 'pre-wrap';
                if (element.nodeName !== 'INPUT')
                    style.wordWrap = 'break-word';  // only for textarea-s

                // position off-screen
                style.position = 'absolute';  // required to return coordinates properly
                style.top = element.offsetTop + parseInt(computed.borderTopWidth) + 'px';
                style.left = "400px";
                style.visibility = 'hidden';  // not 'display: none' because we want rendering

                // transfer the element's properties to the div
                module.TextAreaProperties.forEach(function (prop) {
                    style[prop] = computed[prop];
                });

                var isFirefox = !(window.mozInnerScreenX == null);
                var mirrorDiv, computed, style;

                if (isFirefox) {
                    style.width = parseInt(computed.width) - 2 + 'px'  // Firefox adds 2 pixels to the padding - https://bugzilla.mozilla.org/show_bug.cgi?id=753662
                    // Firefox lies about the overflow property for textareas: https://bugzilla.mozilla.org/show_bug.cgi?id=984275
                    if (element.scrollHeight > parseInt(computed.height))
                        style.overflowY = 'scroll';
                } else {
                    style.overflow = 'hidden';  // for Chrome to not render a scrollbar; IE keeps overflowY = 'scroll'
                }

                mirrorDiv.textContent = element.value.substring(0, position);
                // the second special handling for input type="text" vs textarea: spaces need to be replaced with non-breaking spaces - http://stackoverflow.com/a/13402035/1269037
                if (element.nodeName === 'INPUT')
                    mirrorDiv.textContent = mirrorDiv.textContent.replace(/\s/g, "\u00a0");

                var span = document.createElement('span');
                // Wrapping must be replicated *exactly*, including when a long word gets
                // onto the next line, with whitespace at the end of the line before (#7).
                // The  *only* reliable way to do that is to copy the *entire* rest of the
                // textarea's content into the <span> created at the caret position.
                // for inputs, just '.' would be enough, but why bother?
                span.textContent = element.value.substring(position) || '.';  // || because a completely empty faux span doesn't render at all
                span.style.backgroundColor = "lightgrey";
                mirrorDiv.appendChild(span);

                var coordinates = {
                    top: span.offsetTop + parseInt(computed['borderTopWidth']),
                    left: span.offsetLeft + parseInt(computed['borderLeftWidth'])
                };

                return coordinates;
            }

            var wordBreaks = self.operators().map(function (operator) { return operator.value; }).concat([')', ' ']);
            function getCurrentWord() {
                var leftExpressionPart = self.textarea.value.substr(0, self.textarea.selectionStart);                
                var wordStartIndex = Math.max.apply(null, wordBreaks.map(function (ch) { return leftExpressionPart.lastIndexOf(ch); }));
                return leftExpressionPart.substr(wordStartIndex + 1);
            }

            var contextMenuOffset = {
                left: 15,
                top: 15
            };
            self.searchText = '';

            self.dispose = function () {
            };

            self.save = function () {
                $.when(options.save(self.expression()))
                    .done(function (result) {
                        if (result.IsSuccess) {
                            options.complete(self.expression());
                            form.Close();
                        } else {
                            self.validationError(module.FormatError(getTextResource(result.MessageKey), result.MessageArguments));
                        }
                    });
            };

            self.cancel = function () {
                form.Close();
            };

            self.contextMenu = ko.observable(null);

            self.contextMenuInit = function (contextMenu) {
                self.contextMenu(contextMenu);

                ko.utils.arrayForEach(self.variables().concat(self.functions()), function (item) {
                    var command = contextMenu.addContextMenuItem();
                    command.text(item.value);
                    command.isVisible = function () {
                        return command.text().toLowerCase().startsWith(self.searchText.toLowerCase());
                    };
                    command.click(function () {
                        self.expression(
                            self.expression().slice(0, self.textarea.selectionStart - self.searchText.length)
                            + self.expression().slice(self.textarea.selectionStart));
                        self.add(item);
                    });
                });
            };

            self.contextMenuOpening = function (contextMenu) {
            };

            self.afterRender = function () {
                self.textarea = form.GetDialogDIV().find('.expression-textarea')[0];
                self.textarea.addEventListener('keypress', function (e) {
                    if (wordBreaks.some(function (ch) { return ch == e.Key; })) {
                        self.contextMenu().close();
                        return;
                    }

                    self.searchText = getCurrentWord() + e.key;
                    var anyVisible = false;
                    self.contextMenu().items().forEach(function (item) {
                        item.visible(item.isVisible());
                        anyVisible = item.visible() || anyVisible;
                    });

                    if (anyVisible) {
                        var coordinates = getCaretCoordinates(self.textarea, self.textarea.selectionStart);
                        self.contextMenu().show({
                            clientX: coordinates.left
                                + form.realX()
                                + self.textarea.offsetLeft
                                + contextMenuOffset.left,
                            clientY: coordinates.top
                                + form.realY()
                                + form.GetDialogDIV().find('.ui-dialog-title').height()
                                + self.textarea.offsetTop
                                + contextMenuOffset.top
                        });
                        self.expression.valueHasMutated();
                    } else {
                        self.contextMenu().close();
                    }

                }, false);
            };

            self.captionRendered = function () {
                form
                    .GetDialogDIV()
                    .find('.ui-dialog-titlebar')[0]
                    .addEventListener(
                        "mousedown",
                        function () {
                            self.contextMenu().close();
                        });
            };
        },
        CaptionComponentName: 'ExpressionEditorComponentName',
        ShowDialog: function (options) {
            var form;
            var bindElement = null;
            var retD = $.Deferred();
            var viewModel;

            form = new formControl.control(
                'region_ExpressionEditor', //form region prefix
                'setting_ExpressionEditor',//location and size setting
                options.caption,//caption
                true,//isModal
                true,//isDraggable
                true,//isResizable
                800, 400,//minSize
                [],//form buttons
                function () {
                    ko.cleanNode(bindElement);
                    viewModel.dispose();
                },//afterClose function
                'data-bind="template: {name: \'../UI/Controls/ExpressionEditor/ExpressionEditor\', afterRender: afterRender}"'
            );

            if (!form.Initialized) {//form with that region and settingsName was open
                require(['sweetAlert'], function () {
                    swal(getTextResource('OpenError'), getTextResource('CantDuplicateForm'), 'warning');
                });
                retD.resolve();
            }

            form.ExtendSize(options.width, options.height);//normal size

            bindElement = document.getElementById(form.GetRegionID());
            viewModel = new module.ViewModel(options, form);
            ko.applyBindings(viewModel, bindElement);

            $.when(form.Show()).done(function () {
                if (!ko.components.isRegistered(module.CaptionComponentName))
                    ko.components.register(module.CaptionComponentName, {
                        template: '<span data-bind="text: $str"/>'
                    });
                form.BindCaption(viewModel, "component: {name: '" + module.CaptionComponentName + "', params: { $str: caption } }");
                viewModel.captionRendered();
                retD.resolve();
            });

            return retD;
        }
    };

    return module;
});