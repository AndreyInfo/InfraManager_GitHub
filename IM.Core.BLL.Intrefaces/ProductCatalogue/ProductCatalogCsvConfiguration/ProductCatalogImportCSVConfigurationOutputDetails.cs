using System;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration
{
    public class ProductCatalogImportCSVConfigurationOutputDetails
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Note { get; init; }

        public char Delimeter { get; init; }

        public byte[] RowVersion { get; init; }
    }
}