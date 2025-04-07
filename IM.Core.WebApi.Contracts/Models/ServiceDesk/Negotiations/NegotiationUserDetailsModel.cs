using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Negotiations
{
    public class NegotiationUserDetailsModel
    {
        public Guid NegotiationID { get; init; }
        public string UserFullName { get; init; }
        public string OldUserName { get; init; }
        public string User { get; init; }
        public Guid UserID { get; init; }
        public string Message { get; init; }
        public byte VotingType { get; init; }
        public string VotingTypeString { get; init; }
        public string UtcDateVote { get; init; }
        public string UtcDateComment { get; init; }
        public string Details { get; init; }
        public string PositionName { get; init; }
        public string SubdivisionName { get; init; }
        public string Email { get; init; }
    }
}
