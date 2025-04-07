using InfraManager.BLL.ProductCatalogue.Models;
using Inframanager.BLL;
using InfraManager.DAL.ProductCatalogue.ProductCatalogCategories;
using InfraManager.DAL.ProductCatalogue.Tree;
using InfraManager.DAL;
using System;
using System.Collections.Generic;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;

internal sealed class ProductModelByTreeFilterQueryBuilder<TEntity> 
    : IBuildEntityQuery<TEntity, ProductModelOutputDetails, ProductCatalogTreeFilter>
    where TEntity : class, IProductModel
{
    private readonly IReadonlyRepository<TEntity> _repository;
    private readonly IProductCatalogCategorySubcategoriesIDQuery _subcategories;

    public ProductModelByTreeFilterQueryBuilder(IReadonlyRepository<TEntity> repository,
        IProductCatalogCategorySubcategoriesIDQuery subcategories)
    {
        _repository = repository;
        _subcategories = subcategories;
    }

    public IExecutableQuery<TEntity> Query(ProductCatalogTreeFilter filterBy)
    {
        var query = _repository.With(x => x.ProductCatalogType)
            .ThenWith(x => x.ProductCatalogCategory)
            .Query();

        if (filterBy.AvailableCategoryID.HasValue)
        {
            HashSet<Guid> categoryIDs = new()
            {
                filterBy.AvailableCategoryID.Value
            };
            categoryIDs.UnionWith(_subcategories.Execute(filterBy.AvailableCategoryID.Value));
            query = query.Where(x => categoryIDs.Contains(x.ProductCatalogType.ProductCatalogCategoryID));
            
        }

        if (filterBy.ParentID.HasValue)
            query = query.Where(x => x.ProductCatalogType.IMObjID == filterBy.ParentID);

        return query;
    }
}
