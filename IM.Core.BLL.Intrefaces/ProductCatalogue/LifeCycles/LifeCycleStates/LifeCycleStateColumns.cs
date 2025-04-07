using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;

[ListViewItem(ListView.LifeCycleStateColumns)]
public sealed class LifeCycleStateColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.StateOptions))]
    public string Options { get; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.UseByDefault))]
    public string IsDefault { get; }
}
