define(['knockout', 'jquery'], function (ko, $) {
    (function () {
        ko.bindingHandlers.numericupdown = {
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
                var isObservableValue = ko.isObservable(allBindingsAccessor().numericupdown.value);
                var ko_value = ko.computed({
                    read: function () {
                        return ko.utils.unwrapObservable(allBindingsAccessor().numericupdown.value);
                    },
                    write: function (newValue) {
                        writeValueToProperty(allBindingsAccessor().numericupdown.value, allBindingsAccessor, "value", newValue);
                        if (!isObservableValue)
                            ko_value.notifySubscribers(newValue);
                    },
                    disposeWhenNodeIsRemoved: element
                });
                //
                var model = new ko.bindingHandlers.numericupdown.ViewModel(options, viewModel, ko_value, element);
                renderTemplate(element, options.template, model, bindingContext);
                //
                return { controlsDescendantBindings: true };
            }
        };

        ko.bindingHandlers.numericupdown.ViewModel = function (options, viewModel, ko_value, element) {
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
                var $input = $(element).find(':input[type=text]');
                require(['jqueryStepper'], function () {
                    $input.stepper({
                        type: 'int',//isInteger ? 'int' : 'float',
                        floatPrecission: 0, //isInteger ? 0 : 2,
                        wheelStep: 1,
                        //arrowStep: isInteger ? 1 : 0.01,
                        limit: [1900, 9999],//[minValue, maxValue],
                        onStep: function (val, up) {
                            self.TempValue(val);
                        }
                    });
                    self.IsLoaded(true);
                    hideSpinner(element);
                });
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
            template: '../UI/Controls/ko/ko.NumericUpDown',
            maxLength: 250,
            invalidClass: 'invalid',
            validate: function (value) { return true; },
            fieldIdentifier: 'TypeName.PropertyName',
            fieldFriendlyName: 'text field',
            objectClassID: null,
            objectID: null
        };
    }());
});