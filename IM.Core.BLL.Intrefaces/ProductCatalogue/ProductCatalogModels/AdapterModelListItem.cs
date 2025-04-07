using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels
{
    [ListViewItem(ListView.GuidsAdapterModelList)]
    public class AdapterModelListItem
    {
        public Guid ID => default;

        [ColumnSettings(0)]
        [Label(nameof(Resources.Name))]
        public string Name => default;

        [ColumnSettings(1)]
        [Label(nameof(Resources.ELPTask_Form_Vendor))]
        public string Vendor => default;
    }
}