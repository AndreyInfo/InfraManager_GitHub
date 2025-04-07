using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

internal sealed class ProductCatalogTypeQueryBuilder:
    IBuildEntityQuery<ProductCatalogType,ProductCatalogTypeDetails, ProductCatalogTypeFilter>
    , ISelfRegisteredService<IBuildEntityQuery<ProductCatalogType,ProductCatalogTypeDetails,ProductCatalogTypeFilter>>
{
    private readonly IReadonlyRepository<ProductCatalogType> _productCatalogTypes;

    public ProductCatalogTypeQueryBuilder(IReadonlyRepository<ProductCatalogType> productCatalogTypes)
    {
        _productCatalogTypes = productCatalogTypes;
    }

    public IExecutableQuery<ProductCatalogType> Query(ProductCatalogTypeFilter filter)
    {
        var query = _productCatalogTypes.With(x => x.ProductCatalogTemplate).Query();

        if (filter.HasParentProductCatalogCategoryID)
            query = query.Where(x => x.ProductCatalogCategoryID == filter.ParentProductCatalogCategoryID.Value);

        return query;
    }
}