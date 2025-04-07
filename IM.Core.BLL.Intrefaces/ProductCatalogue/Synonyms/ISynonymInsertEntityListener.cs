using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.Models;

namespace InfraManager.BLL.ProductCatalogue.Synonyms;

public interface ISynonymInsertEntityListener<TEntity>
{
    Task EntityAddedAsync(ProductModelOutputDetails entity, CancellationToken token = default);
}