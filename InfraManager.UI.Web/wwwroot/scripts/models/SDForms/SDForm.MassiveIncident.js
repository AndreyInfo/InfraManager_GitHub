define(['knockout', 'jquery', 'ttControl'], function (ko, $, tclib) {
    var module = {
        MassiveIncident: function (imList, obj) {
            var nself = this;
            //
            nself.ID = obj.Number;
            nself.Name = '№ ' + obj.Number + ' ' + obj.Name;
        }
    };
    return module;
});