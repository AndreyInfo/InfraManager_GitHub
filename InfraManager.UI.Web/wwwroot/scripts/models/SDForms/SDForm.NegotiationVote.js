define(['knockout', 'jquery', 'ajax'], function (ko, $, ajaxLib) {
    var module = {
        VoteMode: {
            For: 'for',
            Against: 'against'
        },
        ViewModel: function (negID, isFinance, objID, objClassID, vote, regionID, user) {
            var self = this;
            self.IsFinanceMode = ko.observable(isFinance === true ? true : false);
            //
            self.regionID = regionID;
            self.negotiationID = negID;
            self.objID = objID;
            self.objClassID = objClassID;
            self.ForseClose = undefined;//задается в формхелпере
            //
            self.Vote = ko.observable(0); //НЕ ГОЛОСОВАЛ
            self.Comment = ko.observable(user ? user.Message() : '');
            self.UserID = ko.observable(user ? user.UserID : null);
            //
            self.SettingCommentPlacet = ko.observable(false);
            self.SettingCommentNonPlacet = ko.observable(false);
            //
            self.SetFor = function () {
                self.Vote(1);//ЗА
            };
            self.SetAgainst = function () {
                self.Vote(2);//ПРОТИВ
            };
            //
            self.IconFor = ko.computed(function () {
                if (self.Vote() == 1)//ЗА
                    return 'negotiation-user-icon_for';
                else return 'negotiation-user-icon_for-gray';
            });
            self.IconAgainst = ko.computed(function () {
                if (self.Vote() == 2)//ПРОТИВ
                    return 'negotiation-user-icon_against';
                else return 'negotiation-user-icon_against-gray';
            });
            self.IconMustComment = ko.computed(function () {
                if (self.SettingCommentPlacet() && self.Vote() == 1)
                    return 'required-negotiation';
                else if (self.SettingCommentNonPlacet() && self.Vote() == 2)
                    return 'required-negotiation';
                else return '';
            });            
            //
            if (vote == module.VoteMode.For)
                self.SetFor();
            else if (vote == module.VoteMode.Against)
                self.SetAgainst();
            //
            self.ajaxControl_vote = new ajaxLib.control

            function patch(data) {
                var PostD = $.Deferred();
                self.ajaxControl_vote.Ajax(self.regionID == null ? null : $('#' + self.regionID),
                    {
                        dataType: 'json',
                        contentType: 'application/json',
                        method: 'PATCH',
                        data: JSON.stringify(data),
                        url: '/api/negotiations/' + self.negotiationID + '/users/' + self.UserID()
                    },
                    function (model) {
                        PostD.resolve({
                            UtcDateTimeVote: parseDate(model.UtcVoteDate),
                            Vote: self.Vote(),
                            Message: self.Comment()
                        });
                        //
                        $(document).trigger('local_objectUpdated', [160, self.negotiationID, self.objID]);//OBJ_NEGOTIATION
                    },
                    function () {
                        PostD.resolve(null);
                    });
                return PostD.promise();
            }

            self.PostVote = function () {
                return patch({ Vote: self.Vote(), Comment: self.Comment() });               
            };
            //
            self.AfterRender = function () {
                var textbox = $(document).find('.negotiation-vote-form-comment');
                textbox.click();
                textbox.focus();
        
            };

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