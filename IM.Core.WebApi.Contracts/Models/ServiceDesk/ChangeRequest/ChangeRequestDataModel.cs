using System;
using Inframanager.BLL;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.ChangeRequest
{
    public class ChangeRequestDataModel
    {
        public string EntityStateID { get; set; }
        public bool EntityStateIDIsNull { get; init; }
        public string EntityStateName { get; set; }
        public NullablePropertyWrapper<Guid> WorkflowSchemeID { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public Guid? TypeID { get; set; }
        public Guid? PriorityID { get; set; }
        public string Description { get; set; }
        public string FundingAmountNumber { get; set; }
        public string HTMLDescription { get; set; }
        public Guid? InfluenceID { get; set; }
        public Guid? InitiatorID { get; set; }
        public Guid? ChangeRequestTypeID { get; set; }
        public Guid? QueueID { get; set; }
        public Guid? CategoryID { get; set; }
        public int? ReasonObjectClassID { get; set; }
        public Guid? ReasonObjectID { get; set; }
        public Guid? ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int ManhoursInMinutes { get; set; }
        public int ManhoursNormInMinutes { get; set; }
        public bool InRealization { get; set; }
        public bool OnWorkOrderExecutorControl { get; set; }
        public string Summary { get; set; }
        public string Target { get; set; }
        public Guid? UrgencyID { get; set; }
        public Guid? OwnerID { get; set; }
        public string UtcDatePromised { get; set; }
        public string UtcDateStarted { get; set; }
        public string UtcDateClosed { get; set; }
        public string UtcDateDetected { get; set; }
        public string UtcDateSolved { get; set; }
        public Guid? RealizationDocumentID { get; set; }
        public Guid? RollbackDocumentID { get; set; }
        public bool? IsSuspended { get; set; }
        public FormValuesDataModel FormValuesData { get; init; }
    }
}
