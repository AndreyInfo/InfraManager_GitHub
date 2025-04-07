using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    [ListViewItem(ListView.UserActivityTypeForGroupColumns)]
    public class UserActivityTypeForGroupColumns
    {
        [ColumnSettings(1, 200)]
        [Label(nameof(Resources.ActivityType))]
        public string Path { get; }
    }
}
