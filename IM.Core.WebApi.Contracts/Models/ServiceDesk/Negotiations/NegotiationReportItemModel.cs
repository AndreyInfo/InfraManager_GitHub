using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Negotiations
{
    public class NegotiationReportItemModel : ListItemModel
    {
        public Guid ObjectID { get; init; }
        public string NegotiationName { get; init; }
        public DateTime? UtcNegotiationDateVoteStart { get; init; }
        public DateTime? UtcNegotiationDateVoteEnd { get; init; }
        public string NegotiationStatusString { get; init; }
        public string NegotiationModeString { get; init; }
    }
}
