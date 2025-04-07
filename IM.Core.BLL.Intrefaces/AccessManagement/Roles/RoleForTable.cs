using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.AccessManagement.ForTable;

[ListViewItem(ListView.RoleForTable)]
public class RoleForTable
{
    
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Role))]
    public string Name { get; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.Note))]
    public string Note { get; }
}
