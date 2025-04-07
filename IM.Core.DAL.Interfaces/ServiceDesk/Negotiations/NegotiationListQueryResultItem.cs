using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class NegotiationListQueryResultItem : ServiceDeskQueryResultItemBase
    {
        public string Name { get; init; }
        public ObjectClass ClassID { get; init; }
        public Issues CategorySortColumn { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PriorityColor { get; init; }
        public int PrioritySequence { get; init; }
        public string TypeFullName { get; init; }
        public string OwnerFullName { get; init; }
        public Guid? OwnerID { get; init; }
        public string ExecutorFullName { get; init; }
        public Guid? ExecutorID { get; init; }
        public string QueueName { get; init; }
        public Guid? QueueID { get; init; }
        public DateTime? UtcDateRegistered { get; init; }
        public DateTime UtcDateModified { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public DateTime? UtcDatePromised { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public string ClientFullName { get; init; }
        public string ClientSubdivisionFullName { get; init; }
        public string ClientOrganizationName { get; init; }
        public int DocumentCount { get; init; }
        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }
        public int UnreadMessageCount { get; init; }
        public Guid ObjectID { get; init; }
        public DateTime? UtcDateVote { get; init; }
        public VotingType UserVote { get; init; }
        public NegotiationStatus NegotiationStatus { get; init; }
        public string NegotiationName { get; init; }
        public NegotiationMode NegotiationMode { get; init; }
        public DateTime? UtcNegotiationDateVoteStart { get; init; }
        public DateTime? UtcNegotiationDateVoteEnd { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public bool InControl { get; init; }
        public bool CanBePicked { get; init; }

    }
}
