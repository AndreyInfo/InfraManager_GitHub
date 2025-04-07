using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.ChangeRequest
{
    public class ChangeRequestListItemModel
    {
        public string Uri { get; init; }
        public Guid ID { get; set; }
        public Guid IMObjID => ID;

        public ObjectClass ClassID { get; set; }

        public int Number { get; set; }

        public Guid TypeID { get; set; }

        public string TypeFullName { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public int ManhoursInMinutes { get; set; }

        public int ManhoursNormInMinutes { get; set; }

        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }

        public string EntityStateName { get; set; }

        public string UrgencyName { get; set; }

        public string InfluenceName { get; set; }

        public int UnreadMessageCount { get; set; }

        public int DocumentCount { get; set; }

        public string OwnerFullName { get; set; }

        public Guid? OwnerID { get; set; }

        public string Target { get; set; }

        public int? FundingAmount { get; set; }

        public Guid PriorityID { get; set; }

        public string PriorityName { get; set; }

        public bool IsFinished { get; set; }

        public bool IsOverdue { get; set; }

        public string InitiatorFullName { get; set; }

        public Guid? InitiatorID { get; set; }

        public string CategoryFullName { get; set; }

        public Guid? CategoryID { get; set; }
        public int NoteCount { get; set; }
        public int MessageCount { get; set; }

        public int WorkOrderCount { get; set; }

        public DateTime UtcDateDetected { get; set; }

        public DateTime? UtcDatePromised { get; set; }

        public DateTime? UtcDateSolved { get; set; }

        public DateTime? UtcDateClosed { get; set; }

        public DateTime UtcDateModified { get; set; }

        public DateTime? UtcDateStarted { get; set; }

        public string ReasonObjectName { get; set; }
    }
}
