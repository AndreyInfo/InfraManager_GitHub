using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class AbstractFinder<TReference, TAbstract, TKey> : IAbstractFinder<TAbstract, TKey>
        where TReference : class, TAbstract
    {
        private readonly DbSet<TReference> _set;

        public AbstractFinder(DbSet<TReference> set)
        {
            _set = set;
        }

        public TAbstract Find(TKey id)
        {
            return _set.Find(id);
        }

        public async Task<TAbstract> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _set.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}
