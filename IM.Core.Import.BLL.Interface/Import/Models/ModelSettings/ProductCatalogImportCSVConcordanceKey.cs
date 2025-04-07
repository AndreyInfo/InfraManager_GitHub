namespace IM.Core.Import.BLL.Interface.Import.Models.ModelSettings
{
    public record ProductCatalogImportCSVConcordanceKey
    {
        public ProductCatalogImportCSVConcordanceKey(Guid id, string field)
        {
            ID = id;
            Field = field;
        }
        
        public ProductCatalogImportCSVConcordanceKey()
        {
        }
        
        public Guid ID { get; init; }

        public string Field { get; init; }

    }
}