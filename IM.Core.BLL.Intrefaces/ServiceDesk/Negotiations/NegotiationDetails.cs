using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationDetails
    {
        public Guid ID { get; init; }
        public Guid ObjectID { get; init; }
        public ObjectClass ObjectClassID { get; init; }
        public string Name { get; init; }
        public NegotiationMode Mode { get; init; }
        public NegotiationStatus Status { get; init; }
        public DateTime? UtcDateVoteStart { get; init; }
        public DateTime? UtcDateVoteEnd { get; init; }
        public Guid[] UserIDs { get; init; }
        public byte[] RowVersion { get; init; }
        public NegotiationUserDetails[] NegotiationUsers { get; init; }
        public string StatusName { get; init; }
        public string ModeName { get; init; }
        public bool IsFinished { get; init; }
        public bool IsNotStarted { get; init; }
        public string FullName => Name;
        public string Theme => Name;
        public bool SettingCommentPlacet { get; init; }
        public bool SettingCommentNonPlacet { get; init; }
    }
}
