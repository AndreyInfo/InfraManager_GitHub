using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.AccessManagement.ForTable
{
    [ListViewItem(ListView.AccessPermissionColumns)]
    public class AccessPermissionForTable
    {
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.IssueRights))]
        public string Name { get; }

        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.Rights))]
        public string Rights { get; }
    }
}
