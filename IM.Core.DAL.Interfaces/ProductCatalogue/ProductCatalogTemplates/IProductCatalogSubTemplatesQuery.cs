using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTemplates;

public interface IProductCatalogSubTemplatesQuery
{
    Task<ProductTemplate[]> ExecuteAsync(ProductTemplate templateId, CancellationToken token = default);
}