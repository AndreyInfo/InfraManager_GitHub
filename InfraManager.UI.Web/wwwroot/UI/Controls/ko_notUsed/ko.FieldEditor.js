define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    (function () {
        var settings = {
            template: '../UI/Controls/ko/ko.FieldEditor',   //template of control
            classID: ko.observable(null),                   //default ko_classID of object
            id: ko.observable(null),                        //default ko_ID of object
            identifier: ko.observable('type.property'),     //default ko_property of object
        };
        /*
            element - The DOM element involved in this binding
            valueAccessor - A JavaScript function that you can call to get the current model property that is involved in this binding. Call valueAccessor() to get the current model property value.
            allBindings -  A JavaScript object that you can use to access all the model values bound to this DOM element. Call allBindings.get('name') to retrieve the value of the name binding (returns undefined if the binding doesn’t exist);
            viewModel - This parameter is deprecated in Knockout 3.x. Use bindingContext.$data or bindingContext.$rawData to access the view model instead.
            bindingContext - An object that holds the binding context available to this element’s bindings. This object includes special properties including $parent, $parents, and $root that can be used to access data that is bound against ancestors of this context.
        */
        ko.bindingHandlers.koFieldEditor = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                if (element.id == '' || !ko.controls || !ko.controls[element.id])
                    throw 'koFieldEditor must be bind after ko<Control>';
                //
                $(element).css('position', 'relative');                         //for spinner position
                //
                var options = ko.utils.unwrapObservable(valueAccessor());
                for (var key in settings)
                    if (options[key] === undefined)
                        options[key] = ko.cloneVariable(settings[key]);
                    else if (ko.isObservable(settings[key]) && !ko.isObservable(options[key])) //make ko-value
                        options[key] = ko.observable(options[key]);
                //
                var id = 'fieldEditor_' + ko.getNewID();
                $('#controlsContainer').append('<div id="' + id + '" class="ko_FieldEditor" data-bind="template: {name: \'' + options.template + '\'}"></div>');
                var fieldEditorElement = document.getElementById(id);
                //
                var vm = new ko.bindingHandlers.koFieldEditor.ViewModel(options, fieldEditorElement, element);
                ko.applyBindings(vm, fieldEditorElement);
                //
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    ko.cleanNode(fieldEditorElement);
                    vm.Dispose();
                    $(fieldEditorElement).remove();
                });
            }
        };
        ko.bindingHandlers.koFieldEditor.ViewModel = function (options, fieldEditorElement, rootElement) {
            var self = this;
            self.options = options;
            //
            self.oldValue = ko.observable(null);                                    //previous value
            self.newValue = ko.observable(null);                                    //current value
            //
            self.ajaxExecuting = ko.observable(false);                              //save in process
            self.Visible = ko.computed({                                            //buttons visible
                read: function () {
                    return self.oldValue() != self.newValue() && self.ajaxExecuting() == false;
                },
                disposeWhenNodeIsRemoved: rootElement
            }).extend({ deferred: true });
            self.Visible_handle = self.Visible.subscribe(function (newValue) {      //set position of control
                var $control = $(fieldEditorElement);
                if (newValue == true) {
                    var $element = $(rootElement).first();                          //first, because template of editor injected as first element in rootElement
                    var offset = $element.offset();
                    $control.css('left', offset.left + $element.width() - $control.width() + 'px');
                    $control.css('top', offset.top + $element.height() + 'px');
                    $control.css('display', 'block');
                }
                else
                    $control.css('display', 'none');
            });
            //
            self.onSaved = null;                                                    //when value saved successfully ()
            self.onError = null;                                                    //when value saving unsuccess   (message)
            self.onCancel = null;                                                   //when user canceled changes    ()
            self.onValueChanged = null;                                             //when another user changed value before current user   (id, classID, value)
            //
            self.ajaxControl = new ajaxLib.control();
            self.ApplyClick = function (replaceAnyway) {                            //save click
                var data = {
                    ID: options.id(),
                    ObjClassID: options.classID(),
                    Field: options.identifier,
                    OldValue: self.oldValue(),
                    NewValue: self.newValue(),
                    ReplaceAnyway: replaceAnyway
                };
                //
                self.ajaxExecuting(true);
                self.ajaxControl.Ajax(
                   $(rootElement).first(),
                   {
                       dataType: "json",
                       method: 'POST',
                       url: '/sdApi/SetField',
                       data: data
                   },
                   function (retval) {
                       if (retval) {
                           var result = retModel.ResultWithMessage.Result;
                           //
                           if (result === 0)
                               self.onSaved();
                           else if (result === 1)
                               self.onError(getTextResource('SaveError') + ': ' + getTextResource('NullParamsError'));
                           else if (result === 2)
                               self.onError(getTextResource('SaveError') + ': ' + getTextResource('BadParamsError'));
                           else if (result === 3)
                               self.onError(getTextResource('SaveError') + ': ' + getTextResource('AccessError'));
                               // 4 - is global error
                           else if (result === 5 && data.ReplaceAnyway == false) {
                               require(['sweetAlert'], function () {
                                   swal({
                                       title: getTextResource('SaveError'),
                                       text: getTextResource('ConcurrencyError'),
                                       showCancelButton: true,
                                       closeOnConfirm: true,
                                       closeOnCancel: true,
                                       confirmButtonText: getTextResource('ButtonOK'),
                                       cancelButtonText: getTextResource('ButtonCancel')
                                   },
                                   function (value) {
                                       if (value == true)
                                           self.ApplyClick(true);
                                       else
                                           self.onValueChanged(retval.CurrentObjectID, retval.CurrentObjectClassID, retval.CurrentObjectValue);//TODO - в общем виде это массив упакованных значений
                                   });
                               });
                           }
                           else if (result === 6)
                               self.onError(getTextResource('SaveError') + ': ' + getTextResource('ObjectDeleted'));
                           else if (result === 7)
                               self.onError(getTextResource('SaveError') + ': ' + getTextResource('OperationError'));
                           else if (result === 8)
                               self.onError(getTextResource('SaveError') + ': ' + getTextResource('ValidationError'));
                           else
                               self.onError(getTextResource('SaveError') + ': ' + getTextResource('GlobalError'));
                       }
                       else
                           self.onError(getTextResource('SaveError') + ': ' + getTextResource('GlobalError'));
                   },
                   undefined,
                   function () {
                       self.ajaxExecuting(false);
                   });
            };
            //
            self.CancelClick = function () {                                        //cancel click
                self.ajaxControl.Abort();
                self.onCancel();
            };
            //
            self.ValueChanged = function (oldValue, newValue) {                     //when user changed value
                self.oldValue(oldValue);
                self.newValue(newValue);
            };
            //
            self.Dispose = function () {                                            //dispose
                self.ajaxControl.Abort();
                self.Visible_handle.dispose();
            };
            //
            //            
            var container = {
                valueChanged: self.ValueChanged,
                onValueChanged: null,
                onSaved: null,
                onError: null,
                onCancel: null
            };
            var editorVM = ko.controls[rootElement.id];
            editorVM.InitializeEditor(container);                                   //invoke editor of dield
            //
            self.onSaved = container.onSaved;
            self.onError = container.onError;
            self.onCancel = container.onCancel;
            self.onValueChanged = container.onValueChanged;
            //
            if (!self.onSaved || !self.onError || !self.onCancel || !self.onValueChanged)
                throw 'koFieldEditor initialization error';
        };
    }());
});