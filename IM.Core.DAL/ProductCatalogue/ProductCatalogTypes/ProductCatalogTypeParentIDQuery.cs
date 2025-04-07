using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;

public class ProductCatalogTypeParentIDQuery:IProductCatalogTypeParentIDQuery,
    ISelfRegisteredService<IProductCatalogTypeParentIDQuery>
{
    private readonly IReadonlyRepository<ProductCatalogType> _repository;

    public ProductCatalogTypeParentIDQuery(IReadonlyRepository<ProductCatalogType> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> ExecuteAsync(Guid id, CancellationToken token)
    {
        return  await _repository.Query().Where(x => x.IMObjID == id).Select(x => x.ProductCatalogCategoryID)
            .SingleOrDefaultAsync(token);
    }
}