using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Parameters
{

    [ListViewItem(ListView.ParameterEnumList)]
    public class ParameterEnumListItem
    {
        public Guid ID { get; init; }
        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }
        public byte[] RowVersion { get; init; }
        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.Tree))]
        public bool IsTree { get; init; }
    }
}
