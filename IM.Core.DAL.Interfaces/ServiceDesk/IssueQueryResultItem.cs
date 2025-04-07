using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class IssueQueryResultItem : ServiceDeskQueryResultItemBase
    {
        public string Name { get; init; }
        public ObjectClass ClassID { get; init; }
        public Issues CategorySortColumn { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PrioritySequence { get; init; }
        public int PriorityColor { get; init; }
        public string TypeFullName { get; init; }
        public Guid? OwnerID { get; init; }
        public string OwnerFullName { get; init; }
        public Guid? ExecutorID { get; init; }
        public string ExecutorFullName { get; init; }
        public Guid? QueueID { get; init; }
        public string QueueName { get; init; }
        public Guid? ClientID { get; init; }
        public string ClientFullName { get; init; }
        public string ClientSubdivisionFullName { get; init; }
        public string ClientOrganizationName { get; init; }
        public DateTime? UtcDateRegistered { get; init; }
        public DateTime UtcDateModified { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public DateTime? UtcDatePromised { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }
        public int DocumentCount { get; init; }
        public int UnreadMessageCount { get; init; }
    }
}
