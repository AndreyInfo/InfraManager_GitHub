using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;


namespace InfraManager.BLL.Dashboards.ForTable;

[ListViewItem(ListView.Dashboard)]
public class DashboardsForTable
{
    [ColumnSettings(0, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Folder))]
    public string StringFolder { get; }
}
