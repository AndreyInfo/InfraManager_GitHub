using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.Tree;

public interface IProductCatalogTreeNodeGetTypeQuery
{
    Task<ObjectClass> ExecuteAsync(Guid id, CancellationToken token);
}