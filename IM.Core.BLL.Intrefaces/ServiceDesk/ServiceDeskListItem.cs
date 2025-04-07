using Inframanager.BLL;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class ServiceDeskListItem : PlugViewListItem, ISDEntityWithPriorityColorInt
    {
        public Guid? OwnerID { get; init; }
        public bool HasState { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public bool InControl { get; init; }
        public bool CanBePicked { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(5, 50)]
        [Label(nameof(Resources.NewMessages))]
        public int UnreadMessageCount { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(7)]
        [Label(nameof(Resources.NumberSymbol))]
        public int Number { get; init; }

        [LikeFilter]
        [ColumnSettings(8)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }

        public Guid PriorityID { get; init; }

        [MultiSelectFilter(LookupQueries.TaskPriority)]
        [ColumnSettings(9)]
        [Label(nameof(Resources.CallPriority))]
        [ColumnSort(nameof(MyTasksListQueryResultItem.PrioritySequence))]
        public string PriorityName { get; init; }

        public int PriorityColor { get; init; }

        public Guid TypeID { get; init; }

        [MultiSelectFilter(LookupQueries.TaskType)]
        [ColumnSettings(10)]
        [Label(nameof(Resources.CallType))]
        public string TypeFullName { get; init; }

        [RangeSliderFilter(false)]
        [ColumnSettings(11, 50)]
        [Label(nameof(Resources.Attachments))]
        public int DocumentCount { get; init; }

        [MultiSelectFilter(LookupQueries.TaskStateName)]
        [ColumnSettings(12)]
        [Label(nameof(Resources.CallState))]
        public string EntityStateName { get; init; }

        [SimpleValueFilter("OwnerUserSearcher", true)]
        [ColumnSettings(13)]
        [Label(nameof(Resources.Owner))]
        public string OwnerFullName { get; init; }

        [SimpleValueFilter("ExecutorUserSearcher", true)]
        [ColumnSettings(14)]
        [Label(nameof(Resources.Executor))]
        public string ExecutorFullName { get; init; }

        public Guid? QueueID { get; init; }

        [SimpleValueFilter("WebUserSearcher", true)]
        [ColumnSettings(15)]
        [Label(nameof(Resources.Client))]
        public string ClientFullName { get; init; }

        [SimpleValueFilter("SubDivisionSearcher", false)]
        [ColumnSettings(16)]
        [Label(nameof(Resources.ClientSubdivision))]
        public string ClientSubdivisionFullName { get; init; }

        [SimpleValueFilter("OrganizationSearcher", false)]
        [ColumnSettings(17)]
        [Label(nameof(Resources.ClientOrganization))]
        public string ClientOrganizationName { get; init; }

        [DatePickFilter]
        [ColumnSettings(18)]
        [Label(nameof(Resources.CallDateRegistered))]
        public DateTime? UtcDateRegistered { get; init; }

        [DatePickFilter]
        [ColumnSettings(19)]
        [Label(nameof(Resources.CallDatePromise))]
        public DateTime UtcDatePromised { get; init; }

        [DatePickFilter]
        [ColumnSettings(20)]
        [Label(nameof(Resources.CallDateAccomplished))]
        public DateTime? UtcDateAccomplished { get; init; }

        [DatePickFilter]
        [ColumnSettings(20)]
        [Label(nameof(Resources.CallDateClosed))]
        public DateTime? UtcDateClosed { get; init; }

        [DatePickFilter]
        [ColumnSettings(21)]
        [Label(nameof(Resources.CallDateModified))]
        public DateTime UtcDateModified { get; init; }

        [ColumnSettings(22, 50)]
        [Label(nameof(Resources.CallIsFinished))]
        public bool IsFinished { get; init; }

        [ColumnSettings(23, 50)]
        [Label(nameof(Resources.CallIsOverdue))]
        public bool IsOverdue { get; init; }

        [SimpleValueFilter("QueueSearcher", true)]
        [ColumnSettings(24)]
        [Label(nameof(Resources.Queue))]
        public string QueueName { get; init; }
    }
}
