using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Synonyms
{
    [ListViewItem(ListView.GuidSynonymList)]
    public class SynonymOutputDetails
    {
        public Guid ID { get; init; }

        public int ClassID { get; init; }

        public Guid ModelID { get; init; }
        
        [ColumnSettings(0)]
        [Label(nameof(Resources.ProductCatalogueModel_TypeName))]
        public string ProductCatalogTypeName { get; set; }

        [ColumnSettings(1)]
        [Label(nameof(Resources.ProductCatalogueModel_Name))]
        public string ModelName { get; init; }

        [ColumnSettings(2)]
        [Label(nameof(Resources.ProductCatalogueModel_ManufacturerName))]
        public string ModelProducer { get; init; }
        
    }
}