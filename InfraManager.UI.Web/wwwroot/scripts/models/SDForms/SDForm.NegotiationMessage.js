define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        VoteMode: {
            For: 'for',
            Against: 'against'
        },
        ViewModel: function (negID, objID, comment, regionID, userID) {
            var self = this;
            //
            self.regionID = regionID;
            self.negotiationID = negID;
            self.objID = objID;
            self.UserID = userID ? userID : null;
            self.ForseClose = undefined;//задается в формхелпере
            //
            self.Comment = ko.observable(comment ? comment() : '');
            //
            self.AfterRender = function () {
                var textbox = $(document).find('.negotiation-message-form-comment');
                textbox.click();
                textbox.focus();
            };

            self.ajaxControl_message = new ajaxLib.control();
            self.PostMessage = function () {
                var PostD = $.Deferred();
                self.ajaxControl_message.Ajax($('#' + self.regionID),
                    {
                        dataType: 'json',
                        contentType: 'application/json',
                        method: 'PATCH',
                        data: JSON.stringify({ Comment: self.Comment() }),
                        url: '/api/negotiations/' + self.negotiationID + '/users/' + self.UserID
                    },
                    function () {
                        PostD.resolve({ Message: self.Comment() });
                        $(document).trigger('local_objectUpdated', [160, self.negotiationID, self.objID]);//OBJ_NEGOTIATION                  
                    }, function () {
                        PostD.resolve(null);
                    });
                return PostD.promise();
            };
            //
            self.Unload = function () {
                $(document).unbind('objectDeleted', self.onObjectModified);
            };
            self.onObjectModified = function (e, objectClassID, objectID, parentObjectID) {
                if (objectClassID == 160) {
                    if (self.negotiationID && objectID == self.negotiationID && parentObjectID == self.objID) {
                        if (e.type == 'objectDeleted') {
                            if (self.ForseClose != undefined)
                                self.ForseClose();
                        }
                    }
                }
            };
        }
    };
    return module;
});