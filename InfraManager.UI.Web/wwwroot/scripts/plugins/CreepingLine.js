define(['jquery'], function ($) {
    return {
        init: function () {
            //example: get creepingLine
            String.prototype.escape = function () {
                var tagsToReplace = {
                    '&': '&amp;',
                    '<': '&lt;',
                    '>': '&gt;'
                };
                return this.replace(/[&<>]/g, function (tag) {
                    return tagsToReplace[tag] || tag;
                });
            };
            //
            $.ajax({
                dataType: "json",
                method: 'GET',
                url: '/api/creepinglines?Visible=true',
                success: function (str) {
                    if (str && str.length > 0)
                    {
                        let joinedString = str.map(x => x.Name).join(' ')
                        $('.b-footer').append('<marquee scrollamount="2" behavior="scroll" direction="left" bgcolor="#ffcc00">' + joinedString.escape() + '</marquee>');
                    }
                }
            });
        }
    }
});