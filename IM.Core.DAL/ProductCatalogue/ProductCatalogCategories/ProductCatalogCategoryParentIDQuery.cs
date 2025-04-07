using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogCategories;

internal sealed class ProductCatalogCategoryParentIDQuery:IProductCatalogCategoryParentIDQuery,
    ISelfRegisteredService<IProductCatalogCategoryParentIDQuery>
{
    private readonly IReadonlyRepository<ProductCatalogCategory> _repository;

    public ProductCatalogCategoryParentIDQuery(IReadonlyRepository<ProductCatalogCategory> repository)
    {
        _repository = repository;
    }

    public Task<Guid?> ExecuteAsync(Guid id, CancellationToken token)
    {
       return _repository.Query().Where(x => x.ID == id)
           .Select(x => x.ParentProductCatalogCategoryID)
            .SingleOrDefaultAsync(token);
    }
}