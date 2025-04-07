using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue;

[ListViewItem(ListView.SLAInfrastructure)]
public class SLAInfrastructureItem
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.LocateAsset))]
    public string Location { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Asset_Category))]
    public string Category { get; }
}