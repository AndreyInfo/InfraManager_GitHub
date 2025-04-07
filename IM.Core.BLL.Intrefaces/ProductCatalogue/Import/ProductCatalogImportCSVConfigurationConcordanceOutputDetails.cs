using System;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.Import
{
    public class ProductCatalogImportCSVConfigurationConcordanceOutputDetails
    {
        public Guid ID { get; init; }

        //public ProductCatalogImportCSVConfiguration ProductCatalogImportCSVConfiguration { get; set; }

        public string Field { get; init; }

        public string Expression { get; init; }
    }
}