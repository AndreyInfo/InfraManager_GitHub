using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.Settings.UserFields;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    [ListViewItem(ListView.AllCallsList, OperationID.SD_General_Calls_View)]
    public class CallListItem : PlugViewListItem, ISDEntityWithPriorityColorInt
    {
        public ObjectClass ClassID => ObjectClass.Call;
        public Guid? OwnerID { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(1)]
        [Label(nameof(Resources.NumberSymbol))]  
        public int Number { get; init; }

        [MultiSelectFilter(LookupQueries.CallReceiptType)]
        [ColumnSettings(9)]
        [Label(nameof(Resources.CallReceiptType))]
        [ColumnSort("ReceiptType")]
        public string ReceiptTypeString { get; init; }

        [LikeFilter]
        [ColumnSettings(12)]
        [Label(nameof(Resources.CallSummary))]
        public string Summary { get; init; }

        [LikeFilter]
        [ColumnSettings(21)]
        [Label(nameof(Resources.Description))]
        public string Description { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(22)]
        [Label(nameof(Resources.Grade))]
        public byte? Grade { get; init; }

        [LikeFilter]
        [ColumnSettings(23)]
        [Label(nameof(Resources.Solution))]
        public string Solution { get; init; }

        [MultiSelectFilter(LookupQueries.CallSla)]
        [ColumnSettings(32)]
        [Label(nameof(Resources.SLA))]
        public string SLAName { get; init; }

        [RangeSliderFilter(true)]
        [ColumnSettings(33)]
        [Label(nameof(Resources.Price))]
        public decimal? Price { get; init; }

        [ColumnSettings(34)]
        [Label(nameof(Resources.ManhoursListCaption))]
        [ColumnSort("Manhours")]
        public string ManhoursInMinutes { get; init; }

        [ColumnSettings(35)]
        [Label(nameof(Resources.ManhoursNorm))]
        [ColumnSort("ManhoursNorm")]
        public string ManhoursNormInMinutes { get; init; }

        [LikeFilter]
        [ColumnSettings(38)]
        [UserFieldDisplay(UserFieldType.Call, FieldNumber.UserField1)]
        public string UserField1 { get; init; }

        [LikeFilter]
        [ColumnSettings(39)]
        [UserFieldDisplay(UserFieldType.Call, FieldNumber.UserField2)]
        public string UserField2 { get; init; }

        [LikeFilter]
        [ColumnSettings(40)]
        [UserFieldDisplay(UserFieldType.Call, FieldNumber.UserField3)]
        public string UserField3 { get; init; }

        [LikeFilter]
        [ColumnSettings(41)]
        [UserFieldDisplay(UserFieldType.Call, FieldNumber.UserField4)]
        public string UserField4 { get; init; }

        [LikeFilter]
        [ColumnSettings(42)]
        [UserFieldDisplay(UserFieldType.Call, FieldNumber.UserField5)]
        public string UserField5 { get; init; }

        [MultiSelectFilter(LookupQueries.CallStateName)]
        [ColumnSettings(15)]
        [Label(nameof(Resources.CallState))]
        public string EntityStateName { get; init; }

        [SimpleValueFilter("SubDivisionSearcher", false)]
        [ColumnSettings(3)]
        [Label(nameof(Resources.ClientSubdivision))]
        public string ClientSubdivisionFullName { get; init; }

        [SimpleValueFilter("OrganizationSearcher", false)]
        [ColumnSettings(4)]
        [Label(nameof(Resources.ClientOrganization))]
        public string ClientOrganizationName { get; init; }

        [MultiSelectFilter(LookupQueries.CallServiceName)]
        [ColumnSettings(13)]
        [Label(nameof(Resources.CallService))]
        public string ServiceName { get; init; }

        [ColumnSettings(14)]
        [Label(nameof(Resources.CallServiceItemOrAttendance))]
        public string ServiceItemOrAttendance { get; init; }

        public Guid TypeID { get; init; }

        public Guid? ExecutorID { get; init; }

        [SimpleValueFilter("QueueSearcher", true)]
        [ColumnSettings(44)]
        [Label(nameof(Resources.Queue))]
        public string QueueName { get; init; }

        public Guid? QueueID { get; init; }

        [MultiSelectFilter(LookupQueries.CallUrgencies)]
        [ColumnSettings(16)]
        [Label(nameof(Resources.Urgency))]
        public string UrgencyName { get; init; }

        [MultiSelectFilter(LookupQueries.CallInfluences)]
        [ColumnSettings(17)]
        [Label(nameof(Resources.Influence))]
        public string InfluenceName { get; init; }

        [MultiSelectFilter(LookupQueries.CallType)]
        [ColumnSettings(10)]
        [Label(nameof(Resources.CallType))]
        public string TypeFullName { get; init; }

        public Guid PriorityID { get; init; }

        [MultiSelectFilter(LookupQueries.CallPriorities)]
        [ColumnSettings(18)]
        [ColumnSort("PrioritySequence")]
        [Label(nameof(Resources.CallPriority))]
        public string PriorityName { get; init; }

        public int PriorityColor { get; init; }

        [DatePickFilter]
        [ColumnSettings(31)]
        [Label(nameof(Resources.CallDateModified))]
        public DateTime UtcDateModified { get; init; }

        [DatePickFilter]
        [ColumnSettings(24)]
        [Label(nameof(Resources.CallDateRegistered))]
        public DateTime? UtcDateRegistered { get; init; }

        [DatePickFilter]
        [ColumnSettings(25)]
        [Label(nameof(Resources.CallDateOpened))]
        public DateTime? UtcDateOpened { get; init; }

        [DatePickFilter]
        [ColumnSettings(26)]
        [Label(nameof(Resources.CallDatePromise))]
        public DateTime UtcDatePromised { get; init; }

        [DatePickFilter]
        [ColumnSettings(29)]
        [Label(nameof(Resources.CallDateAccomplished))]
        public DateTime? UtcDateAccomplished { get; init; }

        [DatePickFilter]
        [ColumnSettings(30)]
        [Label(nameof(Resources.CallDateClosed))]
        public DateTime? UtcDateClosed { get; init; }

        [DatePickFilter]
        [ColumnSettings(43, false)]
        [Label(nameof(Resources.DateCreated))]
        public DateTime UtcDateCreated { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(11, 50)]
        [Label(nameof(Resources.Attachments))]
        public int DocumentCount { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(20)]
        [Label(nameof(Resources.WorkOrders))]
        public int WorkOrderCount { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(19)]
        [Label(nameof(Resources.Problems))]
        public int ProblemCount { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(0, 50)]
        [Label(nameof(Resources.NewMessages))]
        public int UnreadMessageCount { get; init; }

        [MultiSelectFilter(LookupQueries.CallBudgetUsageCause)]
        [ColumnSettings(37)]
        [Label(nameof(Resources.BudgetGround))]
        public string BudgetUsageCauseString { get; init; }

        [MultiSelectFilter(LookupQueries.CallBudget)]
        [ColumnSettings(36)]
        [Label(nameof(Resources.BudgetName))]
        public string BudgetString { get; init; }

        [ColumnSettings(27, 50)]
        [Label(nameof(Resources.CallIsFinished))]
        public bool IsFinished { get; init; }

        [ColumnSettings(28, 50)]
        [Label(nameof(Resources.CallIsOverdue))]
        public bool IsOverdue { get; init; }

        [SimpleValueFilter("WebUserSearcher", true)]
        [ColumnSettings(2)]
        [Label(nameof(Resources.Client))]
        public string ClientFullName { get; init; }

        [SimpleValueFilter("WebUserSearcher", true)]
        [ColumnSettings(5)]
        [Label(nameof(Resources.Initiator))]
        public string InitiatorFullName { get; init; }

        [SimpleValueFilter("ExecutorUserSearcher", true)]
        [ColumnSettings(8)]
        [Label(nameof(Resources.Executor))]
        public string ExecutorFullName { get; init; }

        [SimpleValueFilter("OwnerUserSearcher", true)]
        [ColumnSettings(6)]
        [Label(nameof(Resources.Owner))]
        public string OwnerFullName { get; init; }

        [SimpleValueFilter("AccomplisherUserSearcher", true)]
        [ColumnSettings(7)]
        [Label(nameof(Resources.Accomplisher))]
        public string AccomplisherFullName { get; init; }

        public Guid? ClientID { get; init; }
        public string ClientEmail { get; init; }
        public Guid? InitiatorID { get; init; }
        public Guid? AccomplisherID { get; init; }
    }
}
