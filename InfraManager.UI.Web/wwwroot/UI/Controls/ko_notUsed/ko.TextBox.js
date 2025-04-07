define(['knockout', 'jquery'], function (ko, $) {
    (function () {
        var settings = {
            template: '../UI/Controls/ko/ko.TextBox',   //template of control
            invalidClass: '_invalid',                   //css class when value is invalid and not empty
            requiredClass: '_required',                 //css class when value is required and empty
            focusedClass: '_focused',                   //css class when control is focused
            readOnlyClass: '_readOnly',                 //css class when control in readOnly mode
            readOnly: ko.observable(false),             //default ko_readOnly of control
            required: ko.observable(false),             //default ko_required of control
            maxLength: 250,                             //max symbols in control
            prompt: ko.observable(''),                  //default placeHolder, when value is empty
            validate: function (value) { return ''; },  //validator of value - returns message of problem
            value: ko.observable(''),                   //default ko_value of control
        };
        /*
            element - The DOM element involved in this binding
            valueAccessor - A JavaScript function that you can call to get the current model property that is involved in this binding. Call valueAccessor() to get the current model property value.
            allBindings -  A JavaScript object that you can use to access all the model values bound to this DOM element. Call allBindings.get('name') to retrieve the value of the name binding (returns undefined if the binding doesn’t exist);
            viewModel - This parameter is deprecated in Knockout 3.x. Use bindingContext.$data or bindingContext.$rawData to access the view model instead.
            bindingContext - An object that holds the binding context available to this element’s bindings. This object includes special properties including $parent, $parents, and $root that can be used to access data that is bound against ancestors of this context.
        */
        ko.bindingHandlers.koTextBox = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                if (element.id == '')
                    element.id = 'koTextBox_' + ko.getNewID();
                //
                var options = ko.utils.unwrapObservable(valueAccessor());
                for (var key in settings)
                    if (options[key] === undefined)
                        options[key] = ko.cloneVariable(settings[key]);
                    else if (ko.isObservable(settings[key]) && !ko.isObservable(options[key])) //make ko-value
                        options[key] = ko.observable(options[key]);
                //
                var vm = new ko.bindingHandlers.koTextBox.ViewModel(options, element);
                var vm_dispose = vm.Dispose;
                vm.dispose = function () {
                    vm_dispose();
                    ko.controls[element.id] = undefined;
                };
                //
                if (!ko.controls)
                    ko.controls = {};
                if (ko.controls[element.id] && ko.controls[element.id].Dispose)
                    ko.controls[element.id].Dispose();
                ko.controls[element.id] = vm;
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    vm.Dispose();
                });
                //
                ko.applyBindingsToNode(element, { template: options.template }, vm);
                //tell KO *not* to bind the descendants itself, otherwise they will be bound twice                
                return { controlsDescendantBindings: true };
            }
        };
        ko.bindingHandlers.koTextBox.ViewModel = function (options, element) {
            var self = this;
            self.options = options;
            //
            self.Value = options.value;                     //reference to real value, before editor initialized
            self.Value_handle = null;                       //when value changed handle
            self.Value_originalHandle = null;               //when original value changed handle
            //
            self.Focused = ko.observable(false);            //focus highlight
            //
            self.Prompt = ko.computed({                     //placeholder expression
                read: function () {
                    return options.readOnly() || self.Focused() || self.Value().trim() != '' ? '' : options.prompt();
                },
                disposeWhenNodeIsRemoved: element
            });
            //
            self.errorMessage = ko.observable('');          //error message
            self.ValidateMessage = ko.computed({            //validation message
                read: function () {
                    return self.errorMessage() != '' ? self.errorMessage() : options.validate(self.Value());
                },
                disposeWhenNodeIsRemoved: element
            });
            //
            self.Css = ko.computed({                        //style of control
                read: function () {
                    var retval = self.Focused() ? options.focusedClass : '';
                    if (options.readOnly())
                        return options.readOnlyClass;
                    else if (options.required() && self.Value().trim() == '')
                        return retval + ' ' + options.requiredClass;
                    else if (self.errorMessage() != '' || options.validate(self.Value()) != '')
                        return retval + ' ' + options.invalidClass;
                    else
                        return retval;
                },
                disposeWhenNodeIsRemoved: element
            });
            //
            self.EraserVisible = ko.computed({              //eraser available
                read: function () {
                    return options.readOnly() == false && self.Value() != '';
                },
                disposeWhenNodeIsRemoved: element
            });
            self.EraserClick = function () {                //clear value
                if (!options.readOnly()) {
                    self.Value('');
                    self.Focused(true);
                }
            };
            //
            self.Dispose = function () {                    //dispose
                if (self.Value_handle != null)
                    self.Value_handle.dispose();
                if (self.Value_originalHandle != null)
                    self.Value_originalHandle.dispose();
            };
            //
            //
            self.SetValue = function (id, classID, value) {//todo - в общем виде это упакованный список
                self.errorMessage('');
                options.value(value);
            };
            self.AcceptChanges = function () {              //set value to new value
                self.errorMessage('');
                options.value(self.Value());
            };
            self.SetError = function (message) {
                self.errorMessage(message);
            };
            self.DiscardChanged = function () {             //set value to previous value
                self.errorMessage('');
                self.Value(options.value());
            };
            //
            self.InitializeEditor = function (fieldEditor)                                  //fieldEditor connector (invoke from FieldEditor)
            {
                fieldEditor.onValueChanged = self.SetValue;
                fieldEditor.onSaved = self.AcceptChanges;
                fieldEditor.onError = self.SetError;
                fieldEditor.onCancel = self.DiscardChanged;
                //
                self.Value = ko.observable(options.value());                                //new object of value
                self.Value_handle = self.Value.subscribe(function (newValue) {              //when Value changed
                    if (options.validate(self.Value()) == '')
                        fieldEditor.valueChanged(options.value(), newValue);                //oldValue, newValue
                    else
                        fieldEditor.valueChanged(null, null);                               //when validation error - block saving
                });
                self.Value_originalHandle = options.value.subscribe(function (newValue) {   //when oldValue changed
                    self.Value(newValue);
                });
            };
        };
    }());
});