define(['knockout'], function (ko) {
    const module = {
        InitContainer: function () {
            const container = {
                IsVisible: ko.observable(true)
            };

            container.OnToggle = function () {
                container.IsVisible(!container.IsVisible());
            };

            return container;
        }
    };

    return module;
});
