using System;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public class ProductCatalogTypeFilter
{
    public bool HasParentProductCatalogCategoryID { get; init; }

    public Guid? ParentProductCatalogCategoryID { get; init; }
}