using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class ProductCatalogTemplateLookupQuery : ILookupQuery
{
    private readonly DbSet<ProductCatalogTemplate> _productCatalogTemplates;

    public ProductCatalogTemplateLookupQuery(DbSet<ProductCatalogTemplate> productCatalogTemplates)
    {
        _productCatalogTemplates = productCatalogTemplates;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Array.ConvertAll(
            await _productCatalogTemplates.AsNoTracking()
                .Select(template => new
                {
                    ID = template.ID,
                    ClassName = template.Name,
                }).ToArrayAsync(cancellationToken),
            item => new ValueData
            {
                ID = item.ID.ToString(),
                Info = item.ClassName,
            });
    }
}