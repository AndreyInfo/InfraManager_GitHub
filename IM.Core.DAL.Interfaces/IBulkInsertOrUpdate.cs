using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL;

public interface IBulkInsertOrUpdate<TEntity>
{
    Task ExecuteAsync(IEnumerable<TEntity> data,
        CancellationToken token);
}