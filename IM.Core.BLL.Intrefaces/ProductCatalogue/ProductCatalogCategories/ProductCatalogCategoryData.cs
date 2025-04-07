using System;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;

public class ProductCatalogCategoryData
{
    public string Name { get; init; }

    public string IconName { get; init; }

    public Guid? ParentID { get; init; }
    
    public byte[] RowVersion { get; init; }
}