using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Negotiations
{
    public class NegotiationDetailsModel
    {
        public Guid ID { get; set; }
        public Guid ObjectID { get; set; }
        public string Object { get; set; }
        public int ObjectClassID { get; set; }
        public NegotiationUserDetailsModel[] NegotiationUsers { get; set; }
        public string Name { get; set; }
        public byte Mode { get; set; }
        public string ModeName { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public string UtcDateVoteStart { get; set; }
        public string UtcDateVoteEnd { get; set; }
        public string RowVersion { get; set; }
        public bool IsFinished { get; set; }
        public bool IsNotStarted { get; set; }
        public string FullName { get; set; }
        public string Theme { get; set; }
        public bool SettingCommentPlacet { get; set; }
        public bool SettingCommentNonPlacet { get; set; }
    }
}
