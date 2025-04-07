using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ProductCatalogue.Tree;

internal sealed class ProductCatalogTreeNodeGetTypeQuery : 
    IProductCatalogTreeNodeGetTypeQuery
    , ISelfRegisteredService<IProductCatalogTreeNodeGetTypeQuery>
{
    private readonly DbSet<ProductCatalogCategory> _context;
    private readonly DbSet<ProductCatalogType> _types;

    public ProductCatalogTreeNodeGetTypeQuery(DbSet<ProductCatalogCategory> context
        , DbSet<ProductCatalogType> types)
    {
        _context = context;
        _types = types;
    }

    public async Task<ObjectClass> ExecuteAsync(Guid id, CancellationToken token)
    {
        var isCatalog = await _context.AnyAsync(x => x.ID == id, token);
        if (isCatalog)
            return ObjectClass.ProductCatalogCategory;

        var isType = await _types.AnyAsync(x => x.IMObjID == id, token);
        if (isType)
            return ObjectClass.ProductCatalogType;

        return ObjectClass.AbstractModel;
    }
}