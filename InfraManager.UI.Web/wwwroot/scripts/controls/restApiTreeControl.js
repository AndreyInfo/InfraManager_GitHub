define(
    [ 'knockout', 'restApiTree' ],
    function (ko, tree) {
        var module = {
            Control: function (caption, theTree, options) {
                var self = this;

                self.Caption = ko.observable(caption);
                self.HeaderExpanded = ko.observable((options && options.HeaderExpanded) || false);

                self.CollapseExpandHeader = function () {
                    self.HeaderExpanded(!self.HeaderExpanded());
                };

                self.IsShowTree = ko.computed(function () {
                    return self.HeaderExpanded();
                });

                self.Tree = theTree;

                self.FilterType = null;

                // utils
                {
                    self.deselectAll = function () {
                        self.Tree.deselectAll.call(self);
                    }
                }
            }
        };
        return module;
    }
)