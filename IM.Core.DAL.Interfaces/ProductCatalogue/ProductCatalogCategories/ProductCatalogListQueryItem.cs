using System;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogCategories
{
    public class ProductCatalogListQueryItem
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public ObjectClass ClassID { get; init; }
        public Guid ProductCatalogTypeID { get; init; }
    }
}