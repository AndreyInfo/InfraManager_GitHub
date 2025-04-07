using System;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceDesk.MassIncidents;

public class ProblemMassIncidentsReportItemBase
{
    public int ID { get; init; }
    public Guid IMObjID { get; init; }

    [ColumnSettings(0)]
    [Label(nameof(Resources.Number))]
    [ColumnSort(nameof(MassIncidentsReportQueryResultItem.ID))]
    public int Number => ID;

    [ColumnSettings(1)]
    [Label(nameof(Resources.Type))]
    public string Type { get; init; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.Summary))]
    public string Summary { get; init; }

    [ColumnSettings(3)]
    [Label(nameof(Resources.Service))]
    public string ServiceName { get; init; }

    [ColumnSettings(4)]
    [Label(nameof(Resources.Cause))]
    public string Cause { get; init; }

    [ColumnSettings(5)]
    [Label(nameof(Resources.Owner))]
    public string Owner { get; init; }

    [ColumnSettings(6)]
    [Label(nameof(Resources.AllMassIncidentsReport_PriorityColumnTitle))]
    public string Priority { get; init; }

    [ColumnSettings(7)]
    [Label(nameof(Resources.AllMassIncidentsReport_StateColumnTitle))]
    public string EntityStateName { get; init; }
}