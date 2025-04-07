using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Problems;
using InfraManager.DAL.Settings.UserFields;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    [ListViewItem(ListView.ProblemList, OperationID.SD_General_Problems_View)]
    public class ProblemListItem : PlugViewListItem, ISDEntityWithPriorityColorInt
    {
        public ObjectClass ClassID => ObjectClass.Problem;

        [ColumnSettings(1)]
        [Label(nameof(Resources.NumberSymbol))]
        [RangeSliderFilter]
        public int Number { get; init; }

        public Guid TypeID { get; init; }

        [ColumnSettings(3)]
        [Label(nameof(Resources.ProblemType))]
        [MultiSelectFilter(LookupQueries.ReferencedProblemTypes)]
        public string TypeFullName { get; init; }

        [ColumnSettings(5)]
        [Label(nameof(Resources.Summary))]
        [LikeFilter]
        public string Summary { get; init; }

        [ColumnSettings(13)]
        [Label(nameof(Resources.Description))]
        [LikeFilter]
        public string Description { get; init; }

        [ColumnSettings(14)]
        [Label(nameof(Resources.Cause))]
        [LikeFilter]
        public string Cause { get; init; }

        [ColumnSettings(6)]
        [Label(nameof(Resources.ShortCause))]
        [LikeFilter]
        public string ProblemCauseName { get; init; }

        [ColumnSettings(15)]
        [Label(nameof(Resources.Fix))]
        [LikeFilter]
        public string Fix { get; init; }

        [ColumnSettings(16)]
        [Label(nameof(Resources.Solution))]
        [LikeFilter]
        public string Solution { get; init; }

        [ColumnSettings(24)]
        [Label(nameof(Resources.ManhoursListCaption))]
        [ColumnSort("Manhours")]
        public string ManhoursInMinutes { get; init; }

        [ColumnSettings(25)]
        [Label(nameof(Resources.ManhoursNorm))]
        [ColumnSort("ManhoursNorm")]
        public string ManhoursNormInMinutes { get; init; }

        [ColumnSettings(26)]
        [Label(nameof(Resources.BudgetName))]
        [MultiSelectFilter(LookupQueries.ProblemBudget)]
        public string BudgetString { get; init; }

        [ColumnSettings(27)]
        [Label(nameof(Resources.BudgetGround))]
        [MultiSelectFilter(LookupQueries.ProblemBudgetUsageCause)]
        public string BudgetUsageCauseString { get; init; }

        [ColumnSettings(7)]
        [Label(nameof(Resources.ProblemState))]
        [MultiSelectFilter(LookupQueries.ProblemStateNames)]
        public string EntityStateName { get; init; }

        [ColumnSettings(8)]
        [ColumnSort(nameof(ProblemListQueryResultItem.UrgencySequence))]
        [Label(nameof(Resources.Urgency))]
        [MultiSelectFilter(LookupQueries.ProblemUrgencies)]
        public string UrgencyName { get; init; }

        [ColumnSettings(9)]
        [ColumnSort(nameof(ProblemListQueryResultItem.InfluenceSequence))]
        [Label(nameof(Resources.Influence))]
        [MultiSelectFilter(LookupQueries.ProblemInfluences)]
        public string InfluenceName { get; init; }
        public Guid PriorityID { get; init; }

        [ColumnSettings(10)]
        [ColumnSort(nameof(ProblemListQueryResultItem.PrioritySequence))]
        [Label(nameof(Resources.ProblemPriority))]
        [MultiSelectFilter(LookupQueries.ProblemPriorities)]
        public string PriorityName { get; init; }
        public int PriorityColor { get; init; }

        [ColumnSettings(0, 50)]
        [Label(nameof(Resources.NewMessages))]
        [RangeSliderFilter]
        public int UnreadMessageCount { get; init; }

        [ColumnSettings(4, 50)]
        [Label(nameof(Resources.Attachments))]
        [RangeSliderFilter]
        public int DocumentCount { get; init; }

        [ColumnSettings(11)]
        [Label(nameof(Resources.Calls))]
        [RangeSliderFilter]
        public int CallCount { get; init; }

        [ColumnSettings(12)]
        [Label(nameof(Resources.WorkOrders))]
        [RangeSliderFilter]
        public int WorkOrderCount { get; init; }

        [ColumnSettings(2)]
        [Label(nameof(Resources.Owner))]
        [SimpleValueFilter("OwnerUserSearcher", useSelf: true)]
        public string OwnerFullName { get; init; }

        [ColumnSettings(17)]
        [Label(nameof(Resources.ProblemDateDetected))]
        [DatePickFilter]
        public DateTime UtcDateDetected { get; init; }

        [ColumnSettings(18)]
        [Label(nameof(Resources.ProblemDatePromise))]
        [DatePickFilter]
        public DateTime UtcDatePromised { get; init; }

        [ColumnSettings(21)]
        [Label(nameof(Resources.ProblemDateSolved))]
        [DatePickFilter]
        public DateTime? UtcDateSolved { get; init; }

        [ColumnSettings(22)]
        [Label(nameof(Resources.ProblemDateClosed))]
        [DatePickFilter]
        public DateTime? UtcDateClosed { get; init; }

        [ColumnSettings(23)]
        [Label(nameof(Resources.ProblemDateModified))]
        [DatePickFilter]
        public DateTime UtcDateModified { get; init; }

        [ColumnSettings(28)]
        [UserFieldDisplay(UserFieldType.Problem, FieldNumber.UserField1)]
        [LikeFilter]
        public string UserField1 { get; init; }

        [ColumnSettings(29)]
        [UserFieldDisplay(UserFieldType.Problem, FieldNumber.UserField2)]
        [LikeFilter]
        public string UserField2 { get; init; }

        [ColumnSettings(30)]
        [UserFieldDisplay(UserFieldType.Problem, FieldNumber.UserField3)]
        [LikeFilter]
        public string UserField3 { get; init; }

        [ColumnSettings(31)]
        [UserFieldDisplay(UserFieldType.Problem, FieldNumber.UserField4)]
        [LikeFilter]
        public string UserField4 { get; init; }

        [ColumnSettings(32)]
        [UserFieldDisplay(UserFieldType.Problem, FieldNumber.UserField5)]
        [LikeFilter]
        public string UserField5 { get; init; }

        [ColumnSettings(20, 50)]
        [Label(nameof(Resources.ProblemIsOverdue))]
        public bool IsOverdue { get; init; }

        [ColumnSettings(19, 50)]
        [Label(nameof(Resources.ProblemIsFinished))]
        public bool IsFinished { get; init; }

        [ColumnSettings(33)]
        [Label(nameof(Resources.Queue))]
        [SimpleValueFilter("QueueSearcher", true)]
        public string QueueName { get; init; }

        [ColumnSettings(34)]
        [Label(nameof(Resources.InitiatorReally))]
        [SimpleValueFilter("WebUserSearcher", true)]
        public string InitiatorFullName { get; init; }

        [ColumnSettings(35)]
        [Label(nameof(Resources.Executor))]
        [SimpleValueFilter("ExecutorUserSearcher", true)]
        public string ExecutorFullName { get; init; }

        [ColumnSettings(36)]
        [Label(nameof(Resources.MainService))]
        [MultiSelectFilter(LookupQueries.ProblemServiceName)]
        public string ServiceName { get; init; }

        public bool InControl { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public Guid? OwnerID { get; init; }
        public Guid? InitiatorID { get; init; }
        public Guid? QueueID { get; init; }
        public Guid? ExecutorID { get; init; }
        public Guid? ServiceID { get; init; }
    }
}
