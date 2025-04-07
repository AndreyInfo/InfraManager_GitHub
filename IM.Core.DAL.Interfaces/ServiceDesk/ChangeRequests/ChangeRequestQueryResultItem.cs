using System;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestQueryResultItem : ServiceDeskQueryResultItemBase
    {
        public string TypeFullName { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public int Manhours { get; init; }
        public int ManhoursNorm { get; init; }

        public string UrgencyName { get; init; }
        public int? UrgencySequence { get; init; }
        public string InfluenceName { get; init; }
        public int? InfluenceSequence { get; init; }

        public int UnreadMessageCount { get; init; }
        public int DocumentCount { get; init; }

        public string OwnerFullName { get; init; }
        public Guid? OwnerID { get; init; }

        public string Target { get; init; }
        public int? FundingAmount { get; init; }

        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PrioritySequence { get; init; }

        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }

        public string InitiatorFullName { get; init; }
        public Guid? InitiatorID { get; init; }

        public string CategoryName { get; init; }
        public Guid? CategoryID { get; init; }

        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public int WorkOrderCount { get; init; }

        public DateTime UtcDateDetected { get; init; }
        public DateTime? UtcDatePromised { get; init; }
        public DateTime? UtcDateSolved { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public DateTime UtcDateModified { get; init; }
        public DateTime? UtcDateStarted { get; init; }

        public string ReasonObjectName { get; init; }

        public bool OnWorkOrderExecutorControl { get; init; }
    }
}
