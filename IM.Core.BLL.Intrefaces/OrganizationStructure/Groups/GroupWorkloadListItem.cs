using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure.Groups;

[ListViewItem(ListView.ExecutorSelectorQueueList)]
public class GroupWorkloadListItem
{
    public Guid ID { get; init; }

    [ColumnSettings(0)]
    [Label(nameof(Resources.AdminTools_PersonalLicence_UserFullName))]
    public string FullName { get; init; }

    [ColumnSettings(1)]
    [Label(nameof(Resources.ExecutorObject))]
    public int Size { get; init; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.ExecutorPresenceTimeInHours))]
    public decimal? AvailableHours { get; set; }

    [ColumnSettings(3)]
    [Label(nameof(Resources.ExecutorPresenceTimeInPercents))]
    public decimal? AvailablePercent { get; set; }
}