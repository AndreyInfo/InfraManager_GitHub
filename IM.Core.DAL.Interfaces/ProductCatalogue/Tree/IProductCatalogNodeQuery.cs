using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.Tree;
public interface IProductCatalogNodeQuery
{
    Task<ProductCatalogNode[]> ExecuteAsync(ProductCatalogTreeFilter filter, CancellationToken cancellationToken);
}
