using System;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    public class ProblemListQueryResultItem : ServiceDeskQueryResultItemBase
    {
        public string TypeFullName { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public string Cause { get; init; }
        public string ProblemCauseName { get; init; }
        public string Fix { get; init; }
        public string Solution { get; init; }
        public int Manhours { get; init; }
        public int ManhoursNorm { get; init; }

        public string BudgetString { get; init; }
        public string BudgetUsageCauseString { get; init; }

        public string UrgencyName { get; init; }
        public int? UrgencySequence { get; init; }
        public string InfluenceName { get; init; }
        public int? InfluenceSequence { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PriorityColor { get; init; }
        public int PrioritySequence { get; init; }

        public int UnreadMessageCount { get; init; }
        public int DocumentCount { get; init; }
        public int CallCount { get; init; }
        public int WorkOrderCount { get; init; }

        public string OwnerFullName { get; init; }

        public DateTime UtcDateDetected { get; init; }
        public DateTime UtcDatePromised { get; init; }
        public DateTime? UtcDateSolved { get; init; }
        public DateTime? UtcDateClosed { get; init; }
        public DateTime UtcDateModified { get; init; }

        public string UserField1 { get; init; }
        public string UserField2 { get; init; }
        public string UserField3 { get; init; }
        public string UserField4 { get; init; }
        public string UserField5 { get; init; }

        public bool IsOverdue { get; init; }
        public bool IsFinished { get; init; }

        public bool InControl { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public Guid? OwnerID { get; init; }
        public bool OnWorkOrderExecutorControl { get; init; }
        public Guid? InitiatorID { get; init; }
        public string InitiatorFullName { get; init; }
        public Guid? QueueID { get; init; }
        public string QueueName { get; init; }
        public Guid? ExecutorID { get; init; }
        public string ExecutorFullName { get; init; }
        public string ServiceName { get; init; }
    }
}
