using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogModel;

internal class ProductCatalogModelFinder<TEntity>:IFinder<TEntity>
    where TEntity:class,IProductModel
{
    private readonly IReadonlyRepository<TEntity> _entities;

    public ProductCatalogModelFinder(IReadonlyRepository<TEntity> entities)
    {
        _entities = entities;
    }

    public TEntity Find(params object[] keys)
    {
        if (IsKeysNotValid(keys))
            return null;
        return _entities.Query().First(x => x.IMObjID == (Guid) keys[0]);
    }

    private static bool IsKeysNotValid(object[] keys)
    {
        return keys.Length != 1 || keys[0] is not Guid;
    }

    public async ValueTask<TEntity> FindAsync(object[] keys, CancellationToken token = default)
    {
        if (IsKeysNotValid(keys))
            return null;
        return await _entities.Query().FirstOrDefaultAsync(x => x.IMObjID == (Guid) keys[0], token);
    }

    public IFinder<TEntity> With<TProperty>(Expression<Func<TEntity, TProperty>> include)
    {
        throw new NotImplementedException();
    }

    public IFinder<TEntity> WithMany<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> include)
    {
        throw new NotImplementedException();
    }
}