using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk
{
    internal class EntityWithManhoursFinder<T> : IFindEntityWithManhours
        where T : class, IHaveManhours
    {
        private readonly DbSet<T> _dbSet;

        public EntityWithManhoursFinder(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public async Task<IHaveManhours> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}