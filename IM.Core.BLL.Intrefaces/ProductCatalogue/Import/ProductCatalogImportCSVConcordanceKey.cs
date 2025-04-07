using System;
using InfraManager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.Import
{
    public class ProductCatalogImportCSVConcordanceKey
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