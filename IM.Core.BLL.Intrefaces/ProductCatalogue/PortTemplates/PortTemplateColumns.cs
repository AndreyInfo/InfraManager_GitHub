using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
namespace InfraManager.BLL.ProductCatalogue.PortTemplates;

[ListViewItem(ListView.PortTemplateColumns)]
public class PortTemplateColumns
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Number))]
    public int PortNumber { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.JackType))]
    public string JackTypeName { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.TechnologyType))]
    public string TechnologyTypeName { get; }
}