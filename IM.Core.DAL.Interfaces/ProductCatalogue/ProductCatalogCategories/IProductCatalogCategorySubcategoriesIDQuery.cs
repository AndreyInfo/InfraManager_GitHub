using System;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogCategories;

public interface IProductCatalogCategorySubcategoriesIDQuery
{
    Guid[] Execute(Guid productCatalogCategoryID);
}