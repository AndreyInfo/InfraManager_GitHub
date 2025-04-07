using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class NegotiationUser
    {
        #region .ctor

        protected NegotiationUser()
        {
        }

        internal NegotiationUser(Guid negotiationID, User user)
        {
            NegotiationID = negotiationID;
            UserID = user.IMObjID;
            User = user;
            VotingType = VotingType.None;
            Message = string.Empty;
        }

        #endregion

        #region Properties

        public Guid NegotiationID { get; init; }
        public Guid UserID { get; set; }
        public virtual User User { get; }
        public string OldUserName { get; set; }

        #endregion

        #region Comment

        public string Message { get; private set; }
        public DateTime? UtcDateComment { get; private set; }
        public void Comment(string text)
        {
            Message = text;
            UtcDateComment = DateTime.UtcNow;
        }

        #endregion

        #region Vote
        public VotingType VotingType { get; private set; }
        public DateTime? UtcVoteDate { get; private set; }
        public void Vote(VotingType vote)
        {
            VotingType = vote;
            UtcVoteDate = DateTime.UtcNow;
        }

        #endregion

        #region Copy

        public NegotiationUser Copy()
        {
            return new NegotiationUser
            {
                NegotiationID = NegotiationID,
                UserID = UserID,
                OldUserName = OldUserName,
                VotingType = VotingType,
                UtcVoteDate = UtcVoteDate,
                UtcDateComment = UtcDateComment,
                Message = Message
            };
        }

        #endregion
    }
}
