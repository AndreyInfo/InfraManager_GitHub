using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.FormDataValue;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderData : IServiceDeskEntityData
    {
        public string Name { get; init; }
        public DateTime? UtcDateAssigned { get; init; }
        public DateTime? UtcDateAccepted { get; init; }
        public DateTime? UtcDateStarted { get; init; }
        public DateTime? UtcDatePromised { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public Guid? TypeID { get; init; }
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
        public byte[] RowVersion { get; set; }
        public InframanagerObject? ReferencedObject { get; set; }
        public long? DatePromisedDeltaInMinutes { get; init; }
        public long? DateStartedDeltaInMinutes { get; init; }
        public FormValuesData FormValuesData { get; init; }
        public WorkOrderFinancePurchaseData FinancePurchase { get; init; }
    }
}
