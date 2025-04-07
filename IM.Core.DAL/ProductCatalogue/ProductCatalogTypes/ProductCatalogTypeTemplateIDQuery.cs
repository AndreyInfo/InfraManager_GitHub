using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;

internal sealed class ProductCatalogTypeTemplateIDQuery :
    IProductCatalogTypeTemplateIDQuery
    , ISelfRegisteredService<IProductCatalogTypeTemplateIDQuery>
{
    private readonly DbSet<ProductCatalogType> _productCatalogTypes;

    public ProductCatalogTypeTemplateIDQuery(DbSet<ProductCatalogType> productCatalogTypes)
    {
        _productCatalogTypes = productCatalogTypes;
    }

    public async Task<ProductTemplate?> ExecuteAsync(Guid id, CancellationToken token = default)
    {
        return await _productCatalogTypes.AsNoTracking()
            .Where(x=>x.IMObjID == id)
            .Select(x=>x.ProductCatalogTemplateID)
            .FirstOrDefaultAsync(token);
    }
}