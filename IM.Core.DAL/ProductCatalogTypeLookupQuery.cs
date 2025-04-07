using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class ProductCatalogTypeLookupQuery : ILookupQuery
{
    private readonly DbSet<ProductCatalogType> _productCatalogTypes;

    public ProductCatalogTypeLookupQuery(DbSet<ProductCatalogType> productCatalogTypes)
    {
        _productCatalogTypes = productCatalogTypes;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Array.ConvertAll(
            await _productCatalogTypes.AsNoTracking()
                .Select(type => new
                {
                    ID = type.IMObjID,
                    TypeName = DbFunctions.GetFullObjectName(ObjectClass.ProductCatalogType, type.IMObjID),
                }).ToArrayAsync(cancellationToken),
            item => new ValueData
            {
                ID = item.ID.ToString(),
                Info = item.TypeName,
            });
    }
}