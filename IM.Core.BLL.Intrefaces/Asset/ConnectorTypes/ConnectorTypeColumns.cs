using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Asset.ConnectorTypes;

[ListViewItem(ListView.ConnectorTypeColumns)]
public class ConnectorTypeColumns
{
    [ColumnSettings(0)]
    [Label(nameof(Resources.Name))]
    public string Name { get; init; }

    [ColumnSettings(1)]
    [Label(nameof(Resources.PairsCount))]
    public int PairCount { get; init; }

    [ColumnSettings(2)]
    [Label(nameof(Resources.ConnectorType_Medium))]
    public string MediumName { get; init; }
}
