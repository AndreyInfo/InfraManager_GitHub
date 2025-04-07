namespace IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration
{
    public class ProductCatalogImportCSVConfigurationOutputDetails
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Note { get; init; }

        public string Delimeter { get; init; }

        public byte[] RowVersion { get; init; }
    }
}