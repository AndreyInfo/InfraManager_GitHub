define(['knockout', 'jquery', './ko.ContextMenu.ViewModel'], function (ko, $, m_contextMenuViewModel) {
    (function () {
        var settings = {
            controlInitialized: ko.observable(                          //raise after control initialized, to get control viewModel
               function (contextMenu) {
               }),
            opening: ko.observable(                                     //raise when contextMenu should show
               function (contextMenu) {
               })
        };
        /*
         element - The DOM element involved in this binding
         valueAccessor - A JavaScript function that you can call to get the current model property that is involved in this binding. Call valueAccessor() to get the current model property value.
         allBindings -  A JavaScript object that you can use to access all the model values bound to this DOM element. Call allBindings.get('name') to retrieve the value of the name binding (returns undefined if the binding doesn’t exist);
         viewModel - This parameter is deprecated in Knockout 3.x. Use bindingContext.$data or bindingContext.$rawData to access the view model instead.
         bindingContext - An object that holds the binding context available to this element’s bindings. This object includes special properties including $parent, $parents, and $root that can be used to access data that is bound against ancestors of this context.
        */
        ko.bindingHandlers.koContextMenu = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                if (element.id == '')
                    element.id = 'koContextMenu_' + ko.getNewID();
                //
                element.style.visibility = 'hidden';//hide context menu
                //
                var options = ko.utils.unwrapObservable(valueAccessor());
                for (var key in settings)
                    if (options[key] === undefined)
                        options[key] = ko.cloneVariable(settings[key]);
                    else if (ko.isObservable(settings[key]) && !ko.isObservable(options[key])) //make ko-value
                        options[key] = ko.observable(options[key]);
                //
                var vm = new m_contextMenuViewModel.contextMenu(options, element);
                var vm_dispose = vm.dispose;
                vm.dispose = function () {
                    vm_dispose();
                    ko.controls[element.id] = undefined;
                };
                if (!ko.controls)
                    ko.controls = {};
                if (ko.controls[element.id] && ko.controls[element.id].dispose)
                    ko.controls[element.id].dispose();
                ko.controls[element.id] = vm;
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    vm.dispose();
                });
                //
                ko.applyBindingsToNode(element, { template: { name: '../UI/Controls/ContextMenu/ko.ContextMenu', afterRender: vm.afterRender } }, vm);
                //tell KO *not* to bind the descendants itself, otherwise they will be bound twice                
                return { controlsDescendantBindings: true };
            }
        };
    }());
});