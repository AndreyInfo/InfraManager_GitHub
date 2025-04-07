using System.Linq;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;

internal sealed class ProductCatalogCategoryQueryBuilder:
    IBuildEntityQuery<ProductCatalogCategory, ProductCatalogCategoryDetails, ProductCatalogCategoryFilter>,
    ISelfRegisteredService<IBuildEntityQuery<ProductCatalogCategory, ProductCatalogCategoryDetails, ProductCatalogCategoryFilter>>
{
    private readonly IRepository<ProductCatalogCategory> _repository;

    public ProductCatalogCategoryQueryBuilder(IRepository<ProductCatalogCategory> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<ProductCatalogCategory> Query(ProductCatalogCategoryFilter filterBy)
    {
        var query = _repository.Query()
            .Where(x => x.ParentProductCatalogCategoryID == filterBy.ParentCatalogCategoryID);

        if (filterBy.ExcludeIDs is not null && filterBy.ExcludeIDs.Any())
            query = query.Where(x => !filterBy.ExcludeIDs.Contains(x.ID));

        return query;
    }
}