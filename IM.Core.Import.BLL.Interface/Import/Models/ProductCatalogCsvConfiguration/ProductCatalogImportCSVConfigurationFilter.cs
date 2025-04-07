namespace IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration
{
    public class ProductCatalogImportCSVConfigurationFilter
    {
        public string Name { get; init; }
        
        public string Note { get; init; }
        
        public char? Delimeter { get; init; }
    }
}