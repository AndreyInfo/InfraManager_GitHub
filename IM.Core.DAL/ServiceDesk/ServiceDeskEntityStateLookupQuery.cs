using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    internal class ServiceDeskEntityStateLookupQuery<TEntity> : ITaskStateNameQuery
        where TEntity : class, IWorkflowEntity
    {
        private readonly DbSet<TEntity> _entities;

        public ServiceDeskEntityStateLookupQuery(DbSet<TEntity> entities)
        {
            _entities = entities;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _entities                
                .Where(p => p.EntityStateName != null)
                .Select(p => p.EntityStateName)                
                .Distinct()                
                .Select(stateName => new ValueData { ID = stateName, Info = stateName })
                .ToArrayAsync(cancellationToken);
        }
    }
}
