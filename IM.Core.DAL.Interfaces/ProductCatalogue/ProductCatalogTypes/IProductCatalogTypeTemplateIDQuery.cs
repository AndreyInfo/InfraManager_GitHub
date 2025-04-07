using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;

public interface IProductCatalogTypeTemplateIDQuery
{
    Task<ProductTemplate?> ExecuteAsync(Guid id, CancellationToken token = default);
}