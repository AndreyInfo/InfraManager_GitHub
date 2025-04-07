using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Synonyms;

public interface ISynonymRemoveEntityListener<TEntity>
{
    Task EntityDeletedAsync(Guid id, CancellationToken token = default);
}