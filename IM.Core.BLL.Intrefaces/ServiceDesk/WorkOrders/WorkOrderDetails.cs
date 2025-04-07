using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderDetails : IUserFieldsModel, ISDEntityWithPriorityColorInt
    {
        public Guid ID { get; init; }
        public ObjectClass ClassID => ObjectClass.WorkOrder;
        public int Number { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string HTMLDescription { get; init; }
        public int ManhoursInMinutes { get; init; }
        public int ManhoursNormInMinutes { get; init; }
        public Guid? ReferencedObjectID { get; init; }
        public ObjectClass? ReferencedObjectClassID { get; init; }
        public string WorkOrderReferenceString { get; init; }
        public InframanagerObject? WorkOrderReference { get; init; }
        public DateTime UtcDateCreated { get; init; }
        public DateTime UtcDateModified { get; init; }
        public DateTime? UtcDateAssigned { get; init; }
        public DateTime? UtcDateAccepted { get; init; }
        public DateTime? UtcDateStarted { get; init; }
        public DateTime UtcDatePromised { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }
        public byte[] RowVersion { get; init; }
        public Guid TypeID { get; init; }
        public string TypeName { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PriorityColor { get; init; }
        public int PrioritySequence { get; init; }
        public Guid? AccomplisherID { get; init; }
        public string AccomplisherFullName { get; init; }
        public Guid? AssigneeID { get; init; }
        public string AssigneeFullName { get; init; }
        public Guid? InitiatorID { get; init; }
        public string InitiatorFullName { get; init; }
        public Guid? ExecutorID { get; init; }
        public string ExecutorFullName { get; init; }
        public Guid? QueueID { get; init; }
        public string QueueName { get; init; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public bool IsActive { get; init; }
        public Guid? BudgetUsageID { get; init; }
        public string BudgetUsageFullName { get; init; }
        public InframanagerObject? BudgetObject { get; init; }
        public Guid? BudgetUsageCauseID { get; init; }
        public string BudgetUsageCauseName { get; init; }
        public Guid? BudgetUsageCauseSlaID { get; init; }
        public int DocumentCount { get; init; }
        public byte WorkOrderTypeClass { get; init; }
        public long WorkOrderReferenceID { get; set; }
        public FormValuesDetailsModel FormValues { get; init; }
        public WorkOrderFinancePurchaseDetails FinancePurchase { get; init; }
        public int NegotiationCount { get;set; }
    }
}
