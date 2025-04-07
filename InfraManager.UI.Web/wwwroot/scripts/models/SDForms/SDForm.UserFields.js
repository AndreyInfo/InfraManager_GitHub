define(['jquery', 'knockout', 'ajax'], function ($, ko, ajaxLib) {
    var module = {
        UserFields: function (userFieldType) {
            var mself = this;
            
            mself.ajaxControl_load = new ajaxLib.control();

            mself.UserFieldType = 1;
            mself.ReadOnly = ko.observable(false);
            mself.UserFieldType = userFieldType;
            mself.UserField1Name = ko.observable('');
            mself.UserField2Name = ko.observable('');
            mself.UserField3Name = ko.observable('');
            mself.UserField4Name = ko.observable('');
            mself.UserField5Name = ko.observable('');
            mself.UserField1 = ko.observable('');
            mself.UserField2 = ko.observable('');
            mself.UserField3 = ko.observable('');
            mself.UserField4 = ko.observable('');
            mself.UserField5 = ko.observable('');    

            mself.SetFields = ko.pureComputed({
                read: function () {
                    return mself;
                },
                write: function (parent) {
                    if (parent === undefined || parent.UserFieldNamesDictionary === undefined) return;

                    for (const key of Object.keys(parent.UserFieldNamesDictionary)) {
                        mself[key](parent.UserFieldNamesDictionary[key]);
                    };
                    var count = 0;
                    for (var idx = 1; idx <= 5; idx++) {
                        const fieldStr = 'UserField' + idx;
                        if (mself[fieldStr + 'Name']() !== '') {
                            mself['CanShowName' + idx](true);
                            mself[fieldStr](parent[fieldStr]());
                            mself[fieldStr].subscribe(function (newValue) { parent[fieldStr](newValue); });
                            if (mself[fieldStr]() !== '') count++;
                        }
                    }
                    mself.ItemsCount(count);
                },
                owner: mself
            });     
            
            mself.CanShowName1 = ko.observable(false);
            mself.CanShowName2 = ko.observable(false);
            mself.CanShowName3 = ko.observable(false);
            mself.CanShowName4 = ko.observable(false);
            mself.CanShowName5 = ko.observable(false);

            mself.Initialize = function () {
                var retval = $.Deferred();
                mself.ajaxControl_load.Ajax(null,
                    {
                        url: '/api/userfields/nondefault?userFieldType=' + mself.UserFieldType,
                        method: 'GET',
                        dataType: "json"
                    },
                    function (userFields) {
                        if (userFields === undefined) retval.resolve(null);                        
                        for (var idx = 0; idx < userFields.length; idx++) {
                            mself['UserField' + userFields[idx].Number + 'Name'](userFields[idx].Text);
                            mself['CanShowName' + userFields[idx].Number]
                                (userFields[idx].Text !== undefined && userFields[idx].Text !== '');
                        }                                                
                    },
                    function () { retval.resolve(null); });
                retval.resolve(mself);
                return retval.promise();
            };
            mself.ItemsCount = ko.observable(0);
        }
    };
    return module;
});