using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Location.StorageLocations;

[ListViewItem(ListView.StorageLocationForTable)]
public class StorageLocationForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.MOL))]
    [ColumnSort("UserID")]
    public string MOL { get; }
}