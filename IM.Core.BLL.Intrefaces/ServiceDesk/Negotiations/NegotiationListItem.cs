using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    [ListViewItem(ListView.NegotiationList)]
    public class NegotiationListItem : ServiceDeskListItem
    {
        public Guid ObjectID { get; init; }

        [MultiSelectFilter(LookupQueries.IssueCategory)]
        [ColumnSettings(6)]
        [Label(nameof(Resources.LinkCategory))]
        [ColumnSort(nameof(MyTasksListQueryResultItem.CategorySortColumn))]
        public string CategoryName { get; init; }

        [LikeFilter]
        [ColumnSettings(0)]
        [Label(nameof(Resources.Property_Negotiation_Name))]
        public string NegotiationName { get; init; }

        [DatePickFilter]
        [ColumnSettings(1)]
        [Label(nameof(Resources.NegotiationDateVoteStart))]
        public DateTime? UtcNegotiationDateVoteStart { get; init; }

        [DatePickFilter]
        [ColumnSettings(2)]
        [Label(nameof(Resources.NegotiationDateVoteEnd))]
        public DateTime? UtcNegotiationDateVoteEnd { get; init; }

        [MultiSelectFilter(LookupQueries.NegotiationStatus)]
        [ColumnSettings(3)]
        [ColumnSort(nameof(NegotiationListQueryResultItem.NegotiationStatus))]
        [Label(nameof(Resources.NegotiationStatus))]
        public string NegotiationStatusString { get; init; }

        [MultiSelectFilter(LookupQueries.NegotiationMode)]
        [ColumnSettings(4)]
        [ColumnSort(nameof(NegotiationListQueryResultItem.NegotiationMode))]
        [Label(nameof(Resources.NegotiationMode))]
        public string NegotiationModeString { get; init; }

    }
}
