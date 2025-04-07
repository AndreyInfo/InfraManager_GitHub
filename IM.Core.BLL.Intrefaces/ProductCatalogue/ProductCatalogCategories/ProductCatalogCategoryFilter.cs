using System;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;

public class ProductCatalogCategoryFilter
{
    public bool HasParentCatalogCategoryID { get; init; }
    public Guid? ParentCatalogCategoryID { get; init; }
    public Guid[] ExcludeIDs { get; init; }
}