using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Users
{
    [ListViewItem(ListView.Positions)]
    public class PositionForTable
    {
        [ColumnSettings(1)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }
    }
}