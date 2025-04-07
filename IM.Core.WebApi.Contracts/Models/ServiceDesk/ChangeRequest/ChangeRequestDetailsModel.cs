using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.ChangeRequest
{
    public class ChangeRequestDetailsModel
    {
        public Guid ID { get; set; }

        public int ClassID { get; set; }

        public int CallCount { get; set; }

        public Guid CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string DependencyKEObjectCount { get; set; }

        public string DependencyObjectCount { get; set; }

        public string Description { get; set; }

        public string EntityStateID { get; set; }

        public string EntityStateName { get; set; }

        public string FullName { get; set; }

        public int? FundingAmount { get; set; }

        public bool HaveUnvotedNegotiation { get; set; }

        public bool InRealization { get; set; }

        public Guid InfluenceID { get; set; }

        public string InfluenceName { get; set; }

        public Guid InitiatorID { get; set; }

        public int ManHours { get; set; }

        public int ManhoursNorm { get; set; }

        public string NegotiationCount { get; set; }

        public string NoteCount { get; set; }

        public int Number { get; set; }

        public bool OnWorkOrderExecutorControl { get; set; }

        public Guid? OwnerID { get; set; }

        public string PriorityColor { get; set; }

        public Guid PriorityID { get; set; }

        public string PriorityName { get; set; }

        public Guid? QueueId { get; set; }

        public string QueueName { get; set; }

        public string RealizationDocumentID { get; set; }

        public int? ReasonObjectClassID { get; set; }

        public Guid? ReasonObjectID { get; set; }

        public string RollbackDocumentID { get; set; }

        public Guid ServiceID { get; set; }

        public string ServiceName { get; set; }

        public string Summary { get; set; }

        public string Target { get; set; }

        public Guid TypeID { get; set; }

        public string TypeName { get; set; }

        public string UnreadNoteCount { get; set; }

        public Guid UrgencyID { get; set; }

        public string UrgencyName { get; set; }

        public string UtcDateDetected { get; set; }

        public string UtcDatePromised { get; set; }

        public string UtcDateClosed { get; set; }

        public string UtcDateSolved { get; set; }

        public string UtcDateModified { get; set; }

        public string UtcDateStarted { get; set; }

        public string WorkOrderCount { get; set; }

        public string WorkflowImageSource { get; set; }

        public string WorkflowSchemeID { get; set; }

        public string WorkflowSchemeIdentifier { get; set; }

        public string WorkflowSchemeVersion { get; set; }

        public string ManhoursString { get; set; }

        public string ManhoursNormString { get; set; }

        public FormValuesDetailsModel FormValues { get; init; }
    }
}
