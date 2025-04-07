using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.Models;

namespace InfraManager.BLL.ProductCatalogue.Synonyms;

public interface ISynonymUpdateEntityListener<TEntity>
{

    Task EntityUpdatedAsync(Guid id, ProductModelOutputDetails data, CancellationToken token = default);
}