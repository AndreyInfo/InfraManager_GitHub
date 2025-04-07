using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders
{
    public class WorkOrderListItemModelBase
    {
        public Guid ID { get; init; }
        public Guid IMObjID => ID;
        public int ClassID { get; init; }
        public int Number { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string UserField1 { get; init; }
        public string UserField2 { get; init; }
        public string UserField3 { get; init; }
        public string UserField4 { get; init; }
        public string UserField5 { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public Guid TypeID { get; init; }
        public string TypeName { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityColor { get; init; }
        public string PriorityName { get; init; }
        public string InitiatorFullName { get; init; }
        public string ExecutorFullName { get; init; }
        public Guid? ExecutorID { get; init; }
        public Guid? InitiatorID { get; init; }
        public Guid? AssigneeID { get; init; }
        public string QueueName { get; init; }
        public Guid? QueueID { get; init; }
        public string AssignorFullName { get; init; }
        public DateTime UtcDateCreated { get; init; }
        public DateTime? UtcDateAssigned { get; init; }
        public DateTime? UtcDateAccepted { get; init; }
        public DateTime UtcDatePromised { get; init; }
        public DateTime? UtcDateStarted { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public DateTime UtcDateModified { get; init; }
        public string BudgetUsageCauseString { get; init; }
        public string BudgetString { get; init; }
        public string ReferencedObjectNumberString { get; init; }
        public int DocumentCount { get; init; }
        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }
        public int UnreadMessageCount { get; init; }
        public string ManhoursInMinutes { get; init; }
        public string ManhoursNormInMinutes { get; init; }
    }
}
