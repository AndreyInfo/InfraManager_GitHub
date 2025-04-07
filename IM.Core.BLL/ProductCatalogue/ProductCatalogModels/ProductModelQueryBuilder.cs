using Inframanager.BLL;
using InfraManager.DAL;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models;

internal sealed class ProductModelQueryBuilder<TEntity, TDetail, TFilter> :
    IBuildEntityQuery<TEntity, TDetail, TFilter>
    where TEntity : class, IProductModel
    where TFilter : ProductModelListFilter
{
    private readonly IReadonlyRepository<TEntity> _repository;

    public ProductModelQueryBuilder(IReadonlyRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<TEntity> Query(TFilter filterBy)
    {
        var query = _repository.Query();

        if (filterBy.TypeID.HasValue)
            query = query.Where(x => x.ProductCatalogTypeID == filterBy.TypeID);

        return query;
    }
}