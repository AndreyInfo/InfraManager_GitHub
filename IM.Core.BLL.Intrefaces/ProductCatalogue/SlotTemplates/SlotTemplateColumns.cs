using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
namespace InfraManager.BLL.ProductCatalogue.SlotTemplates;

[ListViewItem(ListView.SlotTemplateColumns)]
public class SlotTemplateColumns
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Number))]
    public int Number { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.SlotType))]
    public int SlotTypeName { get; }
}
