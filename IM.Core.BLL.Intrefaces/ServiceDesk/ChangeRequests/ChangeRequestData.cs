using System;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.FormDataValue;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestData : IServiceDeskEntityData
    {
        public Guid? PriorityID { get; init; }
        public string Description { get; init; }
        public string FundingAmountNumber { get; init; }
        public string HTMLDescription { get; init; }
        public Guid? TypeID { get; init; }
        public NullablePropertyWrapper<Guid> InfluenceID { get; init; }
        public NullablePropertyWrapper<Guid> InitiatorID { get; init; }
        public Guid? ChangeRequestTypeID { get; init; }
        public NullablePropertyWrapper<Guid> QueueID { get; init; }
        public Guid? CategoryID { get; init; }
        public int? ReasonObjectClassID { get; init; }
        public Guid? ReasonObjectID { get; init; }
        public Guid? ServiceID { get; init; }
        public string ServiceName { get; init; }
        public int ManhoursInMinutes { get; init; }
        public int ManhoursNormInMinutes { get; init; }
        public bool InRealization { get; init; }
        public bool OnWorkOrderExecutorControl { get; init; }
        public string Summary { get; init; }
        public string Target { get; init; }
        public NullablePropertyWrapper<Guid> UrgencyID { get; init; }
        public NullablePropertyWrapper<Guid> OwnerID { get; init; }
        public string UtcDatePromised { get; init; }
        public string UtcDateStarted { get; init; }
        public string UtcDateClosed { get; init; }
        public string UtcDateDetected { get; init; }
        public string UtcDateSolved { get; init; }
        public NullablePropertyWrapper<Guid> RealizationDocumentID { get; init; }
        public NullablePropertyWrapper<Guid> RollbackDocumentID { get; init; }
        public string EntityStateID { get; init; }
        public bool EntityStateIDIsNull { get; init; }
        public string EntityStateName { get; init; }
        public NullablePropertyWrapper<Guid> WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public FormValuesData FormValuesData { get; init; }
    }
}
