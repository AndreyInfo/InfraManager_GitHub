using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Models
{
    [ListViewItem(ListView.GuidsSlotModelList)]
    public class SlotListItem
    {
        public Guid ID => default;

        [ColumnSettings(0)]
        [Label(nameof(Resources.Number))]
        public string Number => default;

        [ColumnSettings(1)]
        [Label(nameof(Resources.Type))]
        public string Type => default;
    }
}