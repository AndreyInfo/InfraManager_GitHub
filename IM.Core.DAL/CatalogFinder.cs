using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class CatalogFinder<TCatalog, TKey> : ICatalogFinder<TCatalog, TKey>
        where TKey : struct
        where TCatalog : class, ICatalog<TKey>
    {
        private readonly DbSet<TCatalog> _db;

        public CatalogFinder(DbSet<TCatalog> db)
        {
            _db = db;
        }

        public Task<TCatalog> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(TCatalog));
            var predicate = Expression.Lambda<Func<TCatalog, bool>>(
                Expression.Equal(
                    Expression.Property(parameter, nameof(ICatalog<TKey>.ID)),
                    Expression.Constant(id)),
                parameter);
            return _db.SingleOrDefaultAsync(predicate);
        }
    }
}
