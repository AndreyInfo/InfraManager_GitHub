namespace IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration
{
    public class ProductCatalogImportCSVConfigurationDetails
    {
        public string Name { get; init; }

        public string Note { get; init; }

        public char Delimeter { get; init; }

        public byte[] RowVersion { get; init; }
    }
}