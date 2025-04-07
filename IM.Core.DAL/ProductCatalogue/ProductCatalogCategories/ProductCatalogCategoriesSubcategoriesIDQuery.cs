using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogCategories;

internal sealed class ProductCatalogCategoriesSubcategoriesIDQuery :
    IProductCatalogCategorySubcategoriesIDQuery
    ,ISelfRegisteredService<IProductCatalogCategorySubcategoriesIDQuery>
{
    private readonly DbSet<ProductCatalogCategory> _productCatalogCategories;

    public ProductCatalogCategoriesSubcategoriesIDQuery(DbSet<ProductCatalogCategory> productCatalogCategories)
    {
        _productCatalogCategories = productCatalogCategories;
    }

    public Guid[] Execute(Guid productCatalogCategoryID)
    {
        var rootCategory = _productCatalogCategories.Include(c => c.SubCategories)
            .FirstOrDefault(c => c.ID == productCatalogCategoryID);

        return GetTreeFromCategories(rootCategory).Select(c=> c.ID).ToArray();
    }

    private ProductCatalogCategory[] GetTreeFromCategories(ProductCatalogCategory rootCategory)
    {
        var result = new Queue<ProductCatalogCategory>();

        
        var subCategories = new Queue<ProductCatalogCategory>();
        do
        {
            result.Enqueue(rootCategory);
            foreach(var subCategory in rootCategory.SubCategories)
                subCategories.Enqueue(subCategory);

        } while(subCategories.TryDequeue(out rootCategory));

        return result.ToArray();
    }
}