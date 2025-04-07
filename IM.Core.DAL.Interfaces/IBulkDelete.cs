using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL;

public interface IBulkDelete<TEntity> where TEntity : class, IImportEntity, new()
{
    Task DeleteExternalIDAsync(IEnumerable<string> keys, CancellationToken token);
}