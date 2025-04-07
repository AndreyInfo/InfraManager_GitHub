using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class AllCallsQueryResultItem : ServiceDeskQueryResultItemBase
    {
        public CallReceiptType ReceiptType { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public byte? Grade { get; init; }
        public string Solution { get; init; }
        public string SLAName { get; init; }
        public decimal? Price { get; init; }

        public int Manhours { get; init; }
        public int ManhoursNorm { get; init; }

        public string UserField1 { get; init; }
        public string UserField2 { get; init; }
        public string UserField3 { get; init; }
        public string UserField4 { get; init; }
        public string UserField5 { get; init; }

        public Guid? OwnerID { get; init; }
        public string ClientSubdivisionFullName { get; init; }
        public string ClientOrganizationName { get; init; }

        public string ServiceName { get; init; }
        public string ServiceItemOrAttendance { get; init; }

        public Guid? ExecutorID { get; init; }

        public Guid? InitiatorID { get; init; }
        public string QueueName { get; init; }
        public Guid? QueueID { get; init; }

        public string UrgencyName { get; init; }
        public string InfluenceName { get; init; }
        public string TypeFullName { get; init; }

        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PrioritySequence { get; init; }
        public int PriorityColor { get; init; }

        public DateTime UtcDateModified { get; init; }
        public DateTime? UtcDateRegistered { get; init; }
        public DateTime? UtcDateOpened { get; init; }
        public DateTime UtcDatePromised { get; init; }
        public DateTime? UtcDateAccomplished { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public DateTime UtcDateCreated { get; init; }

        public int DocumentCount { get; init; }
        public int WorkOrderCount { get; init; }
        public int ProblemCount { get; init; }
        public int UnreadMessageCount { get; init; }

        public string BudgetUsageCauseString { get; init; }
        public string BudgetString { get; init; }

        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }
        public string ClientFullName { get; init; }
        public string InitiatorFullName { get; init; }
        public string ExecutorFullName { get; init; }
        public string OwnerFullName { get; init; }
        public string AccomplisherFullName { get; init; }

        public Guid ClientID { get; init; }
        public string ClientEmail { get; init; }

        public Guid? AccomplisherID { get; init; }

    }
}
