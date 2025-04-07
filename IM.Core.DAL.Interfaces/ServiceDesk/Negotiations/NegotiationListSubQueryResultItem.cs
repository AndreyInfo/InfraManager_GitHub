using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class NegotiationListSubQueryResultItem
    {
        public Guid ObjectID { get; init; }
        public int Number { get; init; }
        public string Name { get; init; }
        public ObjectClass ClassID { get; init; }
        public Issues CategorySortColumn { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PriorityColor { get; init; }
        public int PrioritySequence { get; init; }
        public string TypeFullName { get; init; }
        public Guid TypeID { get; init; }
        public Guid? OwnerID { get; init; }
        public Guid? ExecutorID { get; init; }
        public Guid? QueueID { get; init; }
        public DateTime? UtcDateRegistered { get; init; }
        public DateTime UtcDateModified { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public DateTime? UtcDatePromised { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public Guid? ClientID { get; init; }
        public Guid? ClientSubdivisionID { get; init; }
        public Guid? ClientOrganizationID { get; init; }
        public string ClientFullName { get; init; }
        public string ClientSubdivisionFullName { get; init; }
        public string ClientOrganizationName { get; init; }
        public bool CanBePicked { get; init; }
        public int MessagesCount { get; init; }
        public int NoteCount { get; init; }
        public bool IsFinished { get; init; }
    }
}
        