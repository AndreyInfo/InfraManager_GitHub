using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.DAL;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    [ListViewItem(ListView.ChangeRequestList, OperationID.SD_General_ChangeRequests_View)]
    public class ChangeRequestListItem: PlugViewListItem
    {
        public ObjectClass ClassID => ObjectClass.ChangeRequest;

        [RangeSliderFilter(false)]
        [ColumnSettings(1)]
        [Label(nameof(Resources.NumberSymbol))]
        public int Number { get; init; }

        public Guid TypeID { get; init; }

        [MultiSelectFilter(LookupQueries.ChangeRequestType)]
        [ColumnSettings(6)]
        [Label(nameof(Resources.Type))]
        public string TypeFullName { get; init; }

        [LikeFilter]
        [ColumnSettings(2)]
        [Label(nameof(Resources.Summary))]
        public string Summary { get; init; }

        [LikeFilter]
        [ColumnSettings(20)]
        [Label(nameof(Resources.Description))]
        public string Description { get; init; }

        [ColumnSort("Manhours")]
        [ColumnSettings(22)]
        [Label(nameof(Resources.ManhoursListCaption))]
        public int ManhoursInMinutes { get; init; }

        [ColumnSort("ManhoursNorm")]
        [ColumnSettings(23)]
        [Label(nameof(Resources.ManhoursNorm))]
        public int ManhoursNormInMinutes { get; init; }

        [MultiSelectFilter(LookupQueries.ChangeRequestStateName)]
        [ColumnSettings(8)]
        [Label(nameof(Resources.ProblemState))]
        public string EntityStateName { get; init; }

        [MultiSelectFilter(LookupQueries.ChangeRequestUrgency)]
        [ColumnSettings(3)]
        [ColumnSort("UrgencySequence")]
        [Label(nameof(Resources.Urgency))]
        public string UrgencyName { get; init; }

        [MultiSelectFilter(LookupQueries.ChangeRequestInfluence)]
        [ColumnSettings(4)]
        [ColumnSort("InfluenceSequence")]
        [Label(nameof(Resources.Influence))]
        public string InfluenceName { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(0, 50)]
        [Label(nameof(Resources.NewMessages))]
        public int UnreadMessageCount { get; init; }

        [ColumnSettings(50)]
        [Label(nameof(Resources.Attachments))]
        public int DocumentCount { get; init; }

        [SimpleValueFilter("OwnerUserSearcher", true)]
        [ColumnSettings(9)]
        [Label(nameof(Resources.Owner))]
        public string OwnerFullName { get; init; }

        public Guid? OwnerID { get; init; }

        [LikeFilter]
        [ColumnSettings(19)]
        [Label(nameof(Resources.Target))]
        public string Target { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(21)]
        [Label(nameof(Resources.FundingAmount))]
        public int? FundingAmount { get; init; }

        public Guid PriorityID { get; init; }

        [MultiSelectFilter(LookupQueries.ChangeRequestPriority)]
        [ColumnSettings(5)]
        [ColumnSort("PrioritySequence")]
        [Label(nameof(Resources.ProblemPriority))]
        public string PriorityName { get; init; }

        [ColumnSettings(13, 50)]
        [Label(nameof(Resources.RFCIsFinished))]
        public bool IsFinished { get; init; }

        [ColumnSettings(14, 50)]
        [Label(nameof(Resources.RFCIsOverdue))]
        public bool IsOverdue { get; init; }

        [SimpleValueFilter("WebUserSearcher", true)]
        [ColumnSettings(10)]
        [Label(nameof(Resources.Initiator))]
        public string InitiatorFullName { get; init; }

        public Guid? InitiatorID { get; init; }

        [MultiSelectFilter(LookupQueries.ChangeRequestCategory)]
        [ColumnSettings(7)]
        [ColumnSort("CategoryName")]
        [Label(nameof(Resources.RFCCategory))]
        public string CategoryFullName { get; init; }

        public Guid? CategoryID { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(24)]
        [Label(nameof(Resources.WorkOrders))]
        public int WorkOrderCount { get; init; }

        [DatePickFilter]
        [ColumnSettings(11)]
        [Label(nameof(Resources.RFCDateDetected))]
        public DateTime UtcDateDetected { get; init; }

        [DatePickFilter]
        [ColumnSettings(12)]
        [Label(nameof(Resources.ProblemDatePromise))]
        public DateTime? UtcDatePromised { get; init; }

        [DatePickFilter]
        [ColumnSettings(15)]
        [Label(nameof(Resources.RFCDateSolved))]
        public DateTime? UtcDateSolved { get; init; }

        [DatePickFilter]
        [ColumnSettings(16)]
        [Label(nameof(Resources.RFCDateClosed))]
        public DateTime? UtcDateClosed { get; init; }

        [DatePickFilter]
        [ColumnSettings(17)]
        [Label(nameof(Resources.RFCDateModified))]
        public DateTime UtcDateModified { get; init; }

        [DatePickFilter]
        [ColumnSettings(18)]
        [Label(nameof(Resources.BeginWith))]
        public DateTime? UtcDateStarted { get; init; }

        [LikeFilter]
        [ColumnSettings(25)]
        [Label(nameof(Resources.ReasonForChange))]
        public string ReasonObjectName { get; init; }
    }
}
