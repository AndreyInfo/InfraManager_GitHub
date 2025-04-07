using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Location.StorageLocations;

[ListViewItem(ListView.StorageLocationColumns)]
internal class LocationStorageColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.LocationCaption))]
    public string Location { get; }
}
