using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ConfigurationUnit;

[ListViewItem(ListView.ConfigurationUnitColumns)]
public class ConfigurationUnitColumns
{

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Number))]
    public string Number { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Type))]
    public string TypeName { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.Asset_CreateDate))]
    public string DateCreate { get; set; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.AssetNumber_AssetStateName))]
    public string StateName { get; set; }
}
