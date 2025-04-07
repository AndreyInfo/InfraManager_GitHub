using System;
using InfraManager.BLL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration
{
    public class ProductCatalogImportInsertCSVConfigurationDetails:ProductCatalogImportCSVConfigurationDetails
    {
        public Guid ID { get; init; }
    }
}