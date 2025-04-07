using System;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;

public class ProductCatalogCategoryDetails
{
    public Guid? ID { get; init; }

    public string Name { get; init; }

    public string IconName { get; init; }

    public Guid? ParentID { get; init; }
    
    public string RowVersion { get; init; }
}
