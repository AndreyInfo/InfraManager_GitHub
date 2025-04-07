namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration
{
    public class ProductCatalogImportCSVConfigurationDetails
    {
        public string Name { get; init; }

        public string Note { get; init; }

        public char Delimeter { get; init; }

        public byte[] RowVersion { get; init; }
    }
}