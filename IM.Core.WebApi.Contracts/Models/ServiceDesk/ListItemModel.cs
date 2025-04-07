using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    public class ListItemModel
    {
        public Guid ID { get; set; }
        public Guid IMObjID { get; init; }
        public int ClassID { get; set; }
        public string Uri { get; set; }
        public Guid? OwnerID { get; set; }
        public bool HasState { get; set; }
        public int NoteCount { get; set; }
        public int MessageCount { get; set; }
        public bool InControl { get; set; }
        public bool CanBePicked { get; set; }
        public int UnreadMessageCount { get; set; }
        public string CategoryName { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public Guid PriorityID { get; set; }
        public string PriorityName { get; set; }
        public string PriorityColor { get; set; }
        public Guid TypeID { get; set; }
        public string TypeFullName { get; set; }
        public int DocumentCount { get; set; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; set; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public string OwnerFullName { get; set; }
        public string ExecutorFullName { get; set; }
        public Guid? QueueID { get; set; }
        public string ClientFullName { get; set; }
        public string ClientSubdivisionFullName { get; set; }
        public string ClientOrganizationName { get; set; }
        public DateTime? UtcDateRegistered { get; set; }
        public DateTime UtcDatePromised { get; set; }
        public DateTime? UtcDateAccomplished { get; set; }
        public DateTime? UtcDateClosed { get; set; }
        public DateTime UtcDateModified { get; set; }
        public bool IsFinished { get; set; }
        public bool IsOverdue { get; set; }
        public string QueueName { get; set; }
    }
}
