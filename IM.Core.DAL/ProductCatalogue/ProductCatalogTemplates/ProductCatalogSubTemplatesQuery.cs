using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTemplates;

internal sealed class ProductCatalogSubTemplatesQuery : 
    IProductCatalogSubTemplatesQuery
    , ISelfRegisteredService<IProductCatalogSubTemplatesQuery>
{
    private readonly DbSet<ProductCatalogTemplate> _productCatalogTemplates;

    public ProductCatalogSubTemplatesQuery(DbSet<ProductCatalogTemplate> productCatalogTemplates)
    {
        _productCatalogTemplates = productCatalogTemplates;
    }

    public async Task<ProductTemplate[]> ExecuteAsync(ProductTemplate templateId, CancellationToken token)
    {
        return await _productCatalogTemplates.Where(x => x.ParentID == templateId)
            .Select(x => x.ID)
            .ToArrayAsync(token);
    }
}