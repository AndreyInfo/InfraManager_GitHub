using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue;

[ListViewItem(ListView.ServiceUnitForTable)]
public class ServiceUnitColumns
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get { return default; } }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.QueueGroup_Performers))]
    public string ResponsibleName { get { return default; } }
}
