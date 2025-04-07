using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestDetails
    {
        public Guid ID { get; init; }

        public ObjectClass ClassID => ObjectClass.ChangeRequest;

        public int CallCount { get; init; }

        public Guid CategoryID { get; init; }

        public string CategoryName { get; init; }

        public string DependencyKEObjectCount { get; init; }

        public string DependencyObjectCount { get; init; }

        public string Description { get; init; }

        public string EntityStateID { get; init; }

        public string EntityStateName { get; init; }

        public string FullName { get; init; }

        public int? FundingAmount { get; init; }

        public bool HaveUnvotedNegotiation { get; init; }

        public bool InRealization { get; init; }

        public Guid InfluenceID { get; init; }

        public string InfluenceName { get; init; }

        public Guid InitiatorID { get; init; }

        public int ManhoursInMinutes { get; init; }

        public int ManhoursNormInMinutes { get; init; }

        public string NegotiationCount { get; init; }

        public string NoteCount { get; init; }

        public int Number { get; init; }

        public bool OnWorkOrderExecutorControl { get; init; }

        public string OwnerID { get; init; }

        public string PriorityColor { get; init; }

        public string PriorityID { get; init; }

        public string PriorityName { get; init; }

        public Guid? QueueId { get; init; }

        public string QueueName { get; init; }

        public string RealizationDocumentID { get; init; }

        public int? ReasonObjectClassID { get; init; }

        public string ReasonObjectID { get; init; }

        public string RollbackDocumentID { get; init; }

        public Guid ServiceID { get; init; }

        public string ServiceName { get; init; }

        public string Summary { get; init; }

        public string Target { get; init; }

        public Guid TypeID { get; init; }

        public string TypeName { get; init; }

        public string UnreadNoteCount { get; init; }

        public Guid UrgencyID { get; init; }

        public string UrgencyName { get; init; }

        public string UtcDateDetected { get; init; }

        public string UtcDatePromised { get; init; }

        public string UtcDateClosed { get; init; }

        public string UtcDateSolved { get; init; }

        public string UtcDateModified { get; init; }

        public string UtcDateStarted { get; init; }

        public string WorkOrderCount { get; init; }

        public string WorkflowImageSource { get; init; }

        public string WorkflowSchemeID { get; init; }

        public string WorkflowSchemeIdentifier { get; init; }

        public string WorkflowSchemeVersion { get; init; }

        public FormValuesDetailsModel FormValues { get; init; }
    }
}
