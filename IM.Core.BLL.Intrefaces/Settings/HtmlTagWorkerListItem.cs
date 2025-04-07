using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Settings
{
    [ListViewItem(ListView.HtmlTagWorkerList)]
    public class HtmlTagWorkerListItem
    {
        public Guid ID { get { return default; } }

        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get { return default; } }
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.Tag))]
        public string TagName { get { return default; } }
    }
}
