using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

[ListViewItem(ListView.OperationLevelAgreementColumns)]
public class OperationLevelAgreementListItem
{
    [ColumnSettings(0)]
    [Label(nameof(Resources.Number))]
    public string Number { get; set; }

    [ColumnSettings(1)]
    [Label(nameof(Resources.Name))]
    public string Name { get; set; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.Note))]
    public string Note { get; set; }

    [ColumnSettings(3)]
    [Label(nameof(Resources.Contract_StartDate))]
    public DateTime? UtcStartDate { get; set; }

    [ColumnSettings(4)]
    [Label(nameof(Resources.Contract_EndDate))]
    public DateTime? UtcFinishDate { get; set; }

    [ColumnSettings(5)]
    [Label(nameof(Resources.SLA_TimeZone))]
    public string TimeZoneName { get; set; }

    [ColumnSettings(6)]
    [Label(nameof(Resources.SLA_WorkSchedule))]
    public Guid? CalendarWorkSchedule { get; set; }

    public int ID => default;
    public Guid IMObjID => default;
}