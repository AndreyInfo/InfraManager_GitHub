using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    [ListViewItem(ListView.ClientCallList)]
    public class CallFromMeListItem : PlugViewListItem, ISDEntityWithPriorityColorInt
    {
        public ObjectClass ClassID => ObjectClass.Call;
        public Guid? OwnerID { get; init; }
        public int MessageCount { get; init; }
        public int NoteCount { get; init; }       
        public bool InControl { get; init; }
        public bool CanBePicked { get; init; }
        public bool HasState { get; init; }
        public Guid TypeID { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(0, 50, false)]
        [Label(nameof(Resources.NewMessages))]
        public int UnreadMessageCount { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(1)]
        [Label(nameof(Resources.NumberSymbol))]
        public int Number { get; init; }

        [SimpleValueFilter("WebUserSearcherStrictNoTOZ", true)]
        [ColumnSettings(2)]
        [Label(nameof(Resources.Client))]
        public string ClientFullName { get; init; }

        [SimpleValueFilter("SubDivisionSearcherNoTOZ", false)]
        [ColumnSettings(3, false)]
        [Label(nameof(Resources.ClientSubdivision))]
        public string ClientSubdivisionFullName { get; init; }

        [SimpleValueFilter("OrganizationSearcherNoTOZ", false)]
        [ColumnSettings(4, false)]
        [Label(nameof(Resources.ClientOrganization))]
        public string ClientOrganizationName { get; init; }

        [SimpleValueFilter("WebUserSearcherStrictNoTOZ", true)]
        [ColumnSettings(5)]
        [Label(nameof(Resources.Initiator))]
        public string InitiatorFullName { get; init; }

        [MultiSelectFilter(LookupQueries.CallReceiptType)]
        [ColumnSettings(6, false)]
        [Label(nameof(Resources.CallReceiptType))]
        [ColumnSort("ReceiptType")]
        public string ReceiptTypeString { get; init; }

        [MultiSelectFilter(LookupQueries.CallType)]
        [ColumnSettings(7, false)]
        [Label(nameof(Resources.CallType))]
        public string TypeFullName { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(8, 50, false)]
        [Label(nameof(Resources.Attachments))]
        public int DocumentCount { get; init; }

        [LikeFilter]
        [ColumnSettings(9)]
        [Label(nameof(Resources.CallSummaryCaption))]
        public string Summary { get; init; }

        [ColumnSettings(10, false)]
        [Label(nameof(Resources.CallService))]
        public string ServiceName { get; init; }

        [ColumnSettings(11, false)]
        [Label(nameof(Resources.CallServiceItemOrAttendance))]
        public string ServiceItemOrAttendance { get; init; }

        [MultiSelectFilter(LookupQueries.CallStateName)]
        [ColumnSettings(12)]
        [Label(nameof(Resources.CallState))]
        public string EntityStateName { get; init; }

        [MultiSelectFilter(LookupQueries.CallUrgencies)]
        [ColumnSettings(13, false)]
        [ColumnSort("UrgencySequence")]
        [Label(nameof(Resources.Urgency))]
        public string UrgencyName { get; init; }

        [LikeFilter]
        [ColumnSettings(14, false)]
        [Label(nameof(Resources.Description))]
        public string Description { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(15, false)]
        [Label(nameof(Resources.Grade))]
        public byte? Grade { get; init; }

        [LikeFilter]
        [ColumnSettings(16, false)]
        [Label(nameof(Resources.Solution))]
        public string Solution { get; init; }

        [DatePickFilter]
        [ColumnSettings(17)]
        [Label(nameof(Resources.CallDateRegistered))]
        public DateTime? UtcDateRegistered { get; init; }

        [DatePickFilter]
        [ColumnSettings(18, false)]
        [Label(nameof(Resources.CallDatePromise))]
        public DateTime UtcDatePromised { get; init; }

        [DatePickFilter]
        [ColumnSettings(19)]
        [Label(nameof(Resources.CallDateAccomplished))]
        public DateTime? UtcDateAccomplished { get; init; }

        [DatePickFilter]
        [ColumnSettings(20, false)]
        [Label(nameof(Resources.CallDateClosed))]
        public DateTime? UtcDateClosed { get; init; }

        [DatePickFilter]
        [ColumnSettings(21, false)]
        [Label(nameof(Resources.CallDateModified))]
        public DateTime UtcDateModified { get; init; }

        [ColumnSettings(22, false)]
        [Label(nameof(Resources.SLA))]
        public string SLAName { get; init; }

        [RangeSliderFilter(true)]
        [ColumnSettings(23, false)]
        [Label(nameof(Resources.Price))]
        public decimal? Price { get; init; }

        [DatePickFilter]
        [ColumnSettings(29, false)]
        [Label(nameof(Resources.DateCreated))]
        public DateTime UtcDateCreated { get; init; }

        [ColumnSettings(30)]
        [Label(nameof(Resources.Owner))]
        public string OwnerFullName { get; init; }

        public int PriorityColor { get; init; }

        public Guid? ClientID { get; init; }
        public Guid? ExecutorID { get; init; }
    }
}
