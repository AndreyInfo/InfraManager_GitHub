using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Units;
[ListViewItem(ListView.UnitColumns)]
public class UnitColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; init; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Code))]
    public string Code { get; init; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; init; }
}
