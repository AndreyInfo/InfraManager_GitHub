using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders
{
    public class WorkOrderDetailsModel : IUserFieldsModel
    {
        public Guid ID { get; init; }
        public int ClassID { get; init; }
        public int Number { get; init; }
        public string NumberString => $"IM-TS-{Number}";
        public string Name { get; init; }
        public string FullName => $"Задание №{Number}"; // TODO: отсутствует локализация
        public string Description { get; init; }
        public string HTMLDescription { get; init; }
        public int ManhoursInMinutes { get; init; }
        public int ManhoursNormInMinutes { get; init; }
        public string ManhoursString { get; init; }
        public string ManhoursNormString { get; init; }
        public Guid? ReferencedObjectID { get; init; }
        public int? ReferencedObjectClassID { get; init; }
        public string WorkOrderReferenceString { get; init; }
        public string WorkOrderReference { get; init; }
        public string UtcDateCreated { get; init; }
        public string UtcDateModified { get; init; }
        public string UtcDateAssigned { get; init; }
        public string UtcDateAccepted { get; init; }
        public string UtcDateStarted { get; init; }
        public string UtcDatePromised { get; init; }
        public string UtcDateAccomplished { get; init; }
        public bool IsFinished { get; init; }
        public string FinishedString { get; init; }
        public bool IsOverdue { get; init; }
        public string OverdueString { get; init; }
        public byte[] RowVersion { get; init; }
        public Guid TypeID { get; init; }
        public string TypeName { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PrioritySequence { get; init; }
        public Guid? AccomplisherID { get; init; }
        public string AccomplisherFullName { get; init; }
        public Guid? AssigneeID { get; init; }
        public Guid? ExecutorID { get; init; }
        public string ExecutorFullName { get; init; }
        public string AssigneeFullName { get; init; }
        public Guid? InitiatorID { get; init; }
        public string InitiatorFullName { get; init; }
        public Guid? QueueID { get; init; }
        public string QueueName { get; init; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public bool IsActive { get; init; }
        public string IsActiveString { get; init; }
        public Guid? BudgetUsageID { get; init; }
        public string BudgetUsageFullName { get; init; }
        public string BudgetObject { get; init; }
        public Guid? BudgetUsageCauseID { get; init; }
        public string BudgetUsageCauseName { get; init; }
        public Guid? BudgetUsageCauseSlaID { get; init; }
        public int DocumentCount { get; init; }
        public string PriorityColor { get; init; }
        public int WorkOrderTypeClass { get; init; }
        public string WorkflowImageSource { get; init; }
        public long WorkOrderReferenceID { get; init; }
        public FormValuesDetailsModel FormValues { get; init; }
        public WorkOrderFinancePurchaseDetailsModel FinancePurchase { get; init; }
        public NoteListItemModel[] NoteList {get; set;}
        public int NoteCount { get { return NoteList == null ? 0 : NoteList.Length; } }
        public int NegotiationCount { get; init; }
    }
}
