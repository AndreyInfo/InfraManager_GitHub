using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure
{
    [ListViewItem(ListView.QueueGroup)]
    public class GroupQueueForTable
    {
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get { return default; } }

        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.Note))]
        public string Note { get { return default; } }

        [ColumnSettings(3, 100)]
        [Label(nameof(Resources.QueueGroup_Performers))]
        public string ResponsibleName { get { return default; } }

        [ColumnSettings(4, 100)]
        [Label(nameof(Resources.QueueGroup_Uses))]
        public string Type { get { return default; } }
    }
}
