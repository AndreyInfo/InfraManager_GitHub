using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationUserDetails
    {
        public Guid NegotiationID { get; init; }
        public Guid UserID { get; init; }
        public string Message { get; init; }
        public VotingType VotingType { get; init; }
        public DateTime? UtcVoteDate { get; init; }
        public DateTime? UtcDateComment { get; init; }
        public string UserFullName { get; init; }
        public string OldUserName { get; init; }
        public string VotingTypeName { get; init; }
        public string UserDetails { get; init; }
        public string UserPositionName { get; init; }
        public string UserSubdivisionName { get; init; }
        public string UserEmail { get; init; }
    }
}
