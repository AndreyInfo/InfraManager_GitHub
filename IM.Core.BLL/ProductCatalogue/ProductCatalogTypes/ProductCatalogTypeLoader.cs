using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;
internal sealed class ProductCatalogTypeLoader :
    ILoadEntity<Guid, ProductCatalogType, ProductCatalogTypeDetails>
    , ISelfRegisteredService<ILoadEntity<Guid, ProductCatalogType, ProductCatalogTypeDetails>>
{
    private readonly IReadonlyRepository<ProductCatalogType> _repository;

    public ProductCatalogTypeLoader(IReadonlyRepository<ProductCatalogType> repository)
    {
        _repository = repository;
    }

    public async Task<ProductCatalogType> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.With(c=> c.LifeCycle)
            .With(c=> c.LifeCycle)
            .With(c=> c.ServiceContractTypeAgreement)
                .ThenWith(c=> c.LifeCycle)
            .With(c=> c.ProductCatalogTemplate)
            .WithMany(c=> c.ServiceContractFeatures)
            .FirstOrDefaultAsync(c=> c.IMObjID == id, cancellationToken);
    }
}
