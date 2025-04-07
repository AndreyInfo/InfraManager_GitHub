using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Models
{
    
    [ListViewItem(ListView.ExpendableMaterials)]
    public class ExpendableMaterialsListItem
    {
        public Guid ID => default;

        [ColumnSettings(0)]
        [Label(nameof(Resources.Name))]
        public string Name => default;

        [ColumnSettings(1)]
        [Label(nameof(Resources.Norm))]
        public int Norm => default;
    }
}