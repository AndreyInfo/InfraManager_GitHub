define(['knockout', 'jquery'], function (ko, $) {
    (function () {
        ko.bindingHandlers.datetimepicker = {
            setDefaults: function (options) {
                ko.utils.extend(defaultOptions, options);
            },
            init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var options = ko.utils.unwrapObservable(valueAccessor());
                for (var index in defaultOptions)
                    if (options[index] === undefined) {
                        options[index] = defaultOptions[index];
                    }
                //
                var isObservableValue = ko.isObservable(allBindingsAccessor().datetimepicker.value);
                var ko_value = ko.computed({
                    read: function () {
                        return ko.utils.unwrapObservable(allBindingsAccessor().datetimepicker.value);
                    },
                    write: function (newValue) {
                        writeValueToProperty(allBindingsAccessor().datetimepicker.value, allBindingsAccessor, "value", newValue);
                        if (!isObservableValue)
                            ko_value.notifySubscribers(newValue);
                    },
                    disposeWhenNodeIsRemoved: element
                });
                //
                var model = new ko.bindingHandlers.datetimepicker.ViewModel(options, viewModel, ko_value, element);
                renderTemplate(element, options.template, model, bindingContext);
                //
                return { controlsDescendantBindings: true };
            }
        };

        ko.bindingHandlers.datetimepicker.ViewModel = function (options, viewModel, ko_value, element) {
            var self = this;
            self.options = options;
            //
            self.TempValue = ko.observable(ko_value());
            self.Value = ko_value;
            self.Value.subscribe(function (newValue) {
                self.TempValue(newValue);
            });
            self.Css = ko.computed(function () {
                return options.validate(self.TempValue()) ? '' : options.invalidClass;
            });
            self.IsLoaded = ko.observable(false);
            //
            showSpinner(element);
            setTimeout(function () {
                require(['dateTimePicker'], function () {
                    if (locale && locale.length > 0)
                        $.datetimepicker.setLocale(locale.substring(0, 2));
                    var allowTimes = []; for (var xh = 0; xh <= 23; xh++) for (var xm = 0; xm < 60; xm++) allowTimes.push(("0" + xh).slice(-2) + ':' + ("0" + xm).slice(-2));
                    var control = $(element).find(':input[type=text]').datetimepicker({
                        startDate: self.TempValue() == null ? new Date() : self.TempValue(),
                        closeOnDateSelect: false,
                        format: 'd.m.Y H:i',
                        mask: '39.19.9999 29:59',
                        allowTimes: allowTimes,
                        dayOfWeekStart: locale && locale.length > 0 && locale.substring(0, 2) == 'en' ? 0 : 1,
                        value: self.TempValue(),
                        validateOnBlur: true,
                        onSelectDate: function (current_time, $input) {
                            self.TempValue(current_time);
                            //editor.ValueDateTime(current_time);
                            //editor.Value(dtLib.Date2String(current_time));
                        },
                        onSelectTime: function (current_time, $input) {
                            self.TempValue(current_time);
                            //editor.ValueDateTime(current_time);
                            //editor.Value(dtLib.Date2String(current_time));
                        }
                    });
                    self.IsLoaded(true);
                    hideSpinner(element);
                    //
                    //var handler = null;
                    //handler = editor.Value.subscribe(function (newValue) {
                    //    if ($.contains(window.document, $parent[0])) {
                    //        var ctrl = editor.controls.length == 0 ? null : editor.controls[editor.controls.length - 1];//last control: typing
                    //        if (ctrl == null)
                    //            return;
                    //        var dt = ctrl.length > 0 ? ctrl.datetimepicker('getValue') : null;
                    //        //
                    //        if (!newValue || newValue.length == 0)
                    //            editor.ValueDateTime(null);//clear field => reset value
                    //        else if (dtLib.Date2String(dt) != newValue) {
                    //            editor.ValueDateTime(null);//value incorrect => reset value
                    //            editor.Value('');
                    //        }
                    //        else
                    //            editor.ValueDateTime(dt);
                    //    }
                    //    else {
                    //        handler.dispose();
                    //        var index = editor.handlers.indexOf(handler);
                    //        if (index != -1)
                    //            editor.handlers.splice(index, 1);
                    //        //
                    //        index = editor.controls.indexOf(control);
                    //        if (index != -1)
                    //            editor.controls.splice(index, 1);
                    //        control.datetimepicker('destroy');
                    //    }
                    //});
                    //editor.handlers.push(handler);
                });



                //var $input = $(element).find(':input[type=text]');
                //require(['jqueryStepper'], function () {
                //    $input.stepper({
                //        type: 'int',//isInteger ? 'int' : 'float',
                //        floatPrecission: 0, //isInteger ? 0 : 2,
                //        wheelStep: 1,
                //        //arrowStep: isInteger ? 1 : 0.01,
                //        limit: [1900, 9999],//[minValue, maxValue],
                //        onStep: function (val, up) {
                //            self.TempValue(val);
                //        }
                //    });
                //    self.IsLoaded(true);
                //    hideSpinner(element);
                //});
            }, 1000);
            //
            self.ApplyClick = function () {
                showSpinner(element);
                setTimeout(function () {
                    self.Value(self.TempValue());//closeEditor
                    hideSpinner(element);
                }, 3000);
            };
            self.CancelClick = function () {
                self.TempValue(self.Value());
            };
            self.OpenEditorClick = function () {
                showSpinner();
                require(['usualForms'], function (module) {
                    var fh = new module.formHelper(true);
                    var options = {
                        ID: self.options.objectID,
                        objClassID: self.options.objectClassID,
                        fieldName: self.options.fieldIdentifier,
                        fieldFriendlyName: self.options.fieldFriendlyName,
                        oldValue: self.TempValue(),
                        onSave: function (newText) {
                            self.Value(newText);//closeEditor
                        }
                    };
                    fh.ShowSDEditor(fh.SDEditorTemplateModes.textEdit, options);
                });
            };
        };

        //TODO: remove this function when writeValueToProperty is made public by KO team
        var writeValueToProperty = function (property, allBindingsAccessor, key, value, checkIfDifferent) {
            if (!property || !ko.isObservable(property)) {
                var propWriters = allBindingsAccessor()['_ko_property_writers'];
                if (propWriters && propWriters[key])
                    propWriters[key](value);
            } else if (ko.isWriteableObservable(property) && (!checkIfDifferent || property.peek() !== value)) {
                property(value);
            }
        };

        var engines = {};
        var renderTemplate = function (element, template, data, bindingContext) {
            var engine = engines[template];

            var success = false;
            do {
                try {
                    ko.renderTemplate(template, bindingContext.createChildContext(data), engine, element, "replaceChildren");
                    success = true;
                    engines[template] = engine;
                } catch (err) {
                    if (engine != null)
                        throw "Template engine not found";

                    engine = { templateEngine: stringTemplateEngine };
                }

            } while (!success)
        };

        //string template source engine
        var stringTemplateSource = function (template) {
            this.template = template;
        };
        stringTemplateSource.prototype.text = function () {
            return this.template;
        };
        var stringTemplateEngine = new ko.nativeTemplateEngine();
        stringTemplateEngine.makeTemplateSource = function (template) {
            return new stringTemplateSource(template);
        };

        var defaultOptions = {
            template: '../UI/Controls/ko/ko.DateTimePicker',
            maxLength: 250,
            invalidClass: 'invalid',
            validate: function (value) { return true; },
            fieldIdentifier: 'TypeName.PropertyName',
            fieldFriendlyName: 'date field',
            objectClassID: null,
            objectID: null
        };
    }());
});