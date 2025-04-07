using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;

internal sealed class ProductModelLoader<TEntity> : ILoadEntity<Guid, TEntity, ProductModelOutputDetails>
    where TEntity : class, IProductModel
{
    private readonly IFindEntityByGlobalIdentifier<TEntity> _finder;
    public ProductModelLoader(IFindEntityByGlobalIdentifier<TEntity> finder)
    {
        _finder = finder;
    }

    public Task<TEntity> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return  _finder
            .With(x => x.ProductCatalogType)
            .ThenWith(x => x.ProductCatalogCategory)
            .FindOrRaiseErrorAsync(id, cancellationToken);
    }
}
