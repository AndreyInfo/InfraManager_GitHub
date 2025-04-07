using Inframanager.BLL;
using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders
{
    public class WorkOrderDataModel
    {
        public string Name { get; init; }
        public string UtcDateAssigned { get; init; }
        public string UtcDateAccepted { get; init; }
        public string UtcDateStarted { get; init; }
        public string UtcDatePromised { get; init; }
        public string UtcDateAccomplished { get; init; }
        public byte[] RowVersion { get; init; }
        public Guid? WorkOrderTypeID { get; init; }
        public Guid? PriorityID { get; init; }
        public Guid? InitiatorID { get; init; }
        public NullablePropertyWrapper<Guid> ExecutorID { get; init; }
        public NullablePropertyWrapper<Guid> AssigneeID { get; init; }
        public NullablePropertyWrapper<Guid> QueueID { get; init; }
        public string UserField1 { get; init; }
        public string UserField2 { get; init; }
        public string UserField3 { get; init; }
        public string UserField4 { get; init; }
        public string UserField5 { get; init; }
        public string EntityStateID { get; init; }
        public bool EntityStateIDIsNull { get; init; }
        public string EntityStateName { get; init; }
        public NullablePropertyWrapper<Guid> WorkflowSchemeID { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string Description { get; init; }
        public int? ManhoursInMinutes { get; init; }
        public int? ManhoursNormInMinutes { get; init; }
        public long? DatePromisedDeltaInMinutes { get; init; }
        public long? DateStartedDeltaInMinutes { get; init; }
        public InframanagerObject? ReferencedObject { get; init; }
        public FormValuesDataModel FormValuesData { get; init; }
        public WorkOrderFinancePurchaseDataModel FinancePurchase { get; init; }
    }
}
