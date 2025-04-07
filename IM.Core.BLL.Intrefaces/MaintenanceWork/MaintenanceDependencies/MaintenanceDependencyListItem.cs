using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;

[ListViewItem(ListView.MaintenanceDependencyList)]
public sealed class MaintenanceDependencyListItem
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string ObjectName { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.ObjectClass))]
    public string ClassName { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.LocateAsset))]
    public string ObjectLocation { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.LinkNote))]
    public string Note { get; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.Blocked))]
    public bool Locked { get; }
}
