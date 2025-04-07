namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration
{
    public class ProductCatalogImportCSVConfigurationFilter
    {
        public string Name { get; init; }

        public string Note { get; init; }

        public char? Delimeter { get; init; }

        public byte[] RowVersion { get; init; }
    }
}