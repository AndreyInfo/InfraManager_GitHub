using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.MaintenanceWork.Maintenances;

[ListViewItem(ListView.MaintenanceList)]
public sealed class MaintenanceColumns
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Folder))]
    public string MaintenanceFolderName { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Description))]
    public string Note { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.TemplateName))]
    public string WorkOrderTemplateName { get; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.CallState))]
    public string StateName { get; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.MaintenanceMultiplicity))]
    public string MultiplicityName { get; }
}
