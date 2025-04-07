using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Slots;

[ListViewItem(ListView.SlotColumns)]
public class SlotColumns
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Number))]
    public int Number { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.SlotType))]
    public int SlotTypeName { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Adapter))]
    public int AdapterID { get; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.Description))]
    public int Note { get; }
}
