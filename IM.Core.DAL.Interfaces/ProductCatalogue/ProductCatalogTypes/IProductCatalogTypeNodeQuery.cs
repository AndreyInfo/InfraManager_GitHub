using InfraManager.DAL.ProductCatalogue.Tree;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;

public interface IProductCatalogTypeNodeQuery
{
    Task<ProductCatalogNode[]> ExecuteAsync(ProductCatalogTreeFilter filter, CancellationToken cancellationToken);
}