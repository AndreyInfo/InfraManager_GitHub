using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    [ListViewItem(ListView.AllMassIncidents, OperationID.MassIncident_ViewAllMassiveIncidentsReport)]
    public class AllMassIncidentsReportItem : PlugViewListItem, ISDEntityWithPriorityColorInt
    {
        public int ID { get; init; }
        public Guid IMObjID { get; init; }
        public ObjectClass ClassID => ObjectClass.MassIncident;

        [Label(nameof(Resources.AllMassIncidentsReport_NumberColumnTitle))]
        [ColumnSettings(1)]
        [RangeSliderFilter]
        [ColumnSort(nameof(AllMassIncidentsReportQueryResultItem.ID))]
        public int Number => ID;

        public Guid InitiatorID { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_CreatedByColumnTitle))]
        [ColumnSettings(2)]
        [SimpleValueFilter("WebUserSearcher", true)]
        public string CreatedByUserName { get; init; }
        public Guid? OwnerID { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_OwnedByColumnTitle))]
        [ColumnSettings(3)]
        [SimpleValueFilter("OwnerUserSearcher", true)]
        public string OwnedByUserName { get; init; }


        [Label(nameof(Resources.AllMassIncidentsReport_InformationChannelColumnTitle))]
        [ColumnSettings(4)]
        [MultiSelectFilter(LookupQueries.MassIncidentInformationChannels)]
        [ColumnSort(nameof(AllMassIncidentsReportQueryResultItem.InformationChannelID))]
        public string InformationChannel { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_TypeColumnTitle))]
        [ColumnSettings(5)]
        [MultiSelectFilter(LookupQueries.MassIncidentTypes)]
        public string Type { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_SummaryColumnTitle))]
        [ColumnSettings(6)]
        [LikeFilter]
        public string Name { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_ServiceColumnTitle))]
        [ColumnSettings(7)]
        [MultiSelectFilter(LookupQueries.MassIncidentServices)]
        public string ServiceName { get; init; }

        public string WorkflowSchemeIdentifier { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_StateColumnTitle))]
        [ColumnSettings(9)]
        [MultiSelectFilter(LookupQueries.MassIncidentStates)]
        public string EntityStateName { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_PriorityColumnTitle))]
        [MultiSelectFilter(LookupQueries.MassIncidentPriorities)]
        [ColumnSettings(10)]
        [ColumnSort(nameof(AllMassIncidentsReportQueryResultItem.PrioritySequence))]
        public string Priority { get; init; }
        public int PriorityColor { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_CriticalityColumnTitle))]
        [MultiSelectFilter(LookupQueries.MassIncidentCriticalities)]
        [ColumnSettings(11)]        
        public string Criticality { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_CauseColumnTitle))]
        [MultiSelectFilter(LookupQueries.MassIncidentCauses)]
        [ColumnSettings(12)]
        public string Cause { get; init; }

        [Label(nameof(Resources.Attachments))]
        [RangeSliderFilter]
        [ColumnSettings(13)]
        public int DocumentsQuantity { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_ShortDescriptionColumnTitle))]
        [ColumnSettings(14)]
        [LikeFilter]
        public string ShortDescription { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_FullDescriptionColumnTitle))]
        [ColumnSettings(15)]
        [LikeFilter]
        public string FullDescription { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_SolutionColumnTitle))]
        [ColumnSettings(16)]
        [LikeFilter]
        public string Solution { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_CreatedColumnTitle))]
        [ColumnSettings(17)]
        [DatePickFilter]
        public DateTime UtcCreatedAt { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_ChangedColumnTitle))]
        [ColumnSettings(18)]
        [DatePickFilter]
        public DateTime UtcLastModifiedAt { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_OpenedColumnTitle))]
        [ColumnSettings(19)]
        [DatePickFilter]
        public DateTime? UtcOpenedAt { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_RegisteredColumnTitle))]
        [ColumnSettings(20)]
        [DatePickFilter]
        public DateTime? UtcRegisteredAt { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_CloseUntilColumnTitle))]
        [ColumnSettings(21)]
        [DatePickFilter]
        public DateTime? UtcCloseUntil { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_GroupColumnTitle))]
        [ColumnSettings(22)]
        [SimpleValueFilter("QueueSearcher", true)]
        public string GroupName { get; init; }

        [Label(nameof(Resources.AllMassIncidentsReport_SlaColumnTitle))]
        [ColumnSettings(23)]
        [MultiSelectFilter(LookupQueries.MassIncidentSLAs)]
        public string OperationalLevelAgreement { get; init; }

        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }
    }
}
