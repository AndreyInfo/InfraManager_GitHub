using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.Import;

//TODO: Выпилить
public class ConcordanceFinder:
    IFinder<ProductCatalogImportCSVConfigurationConcordance>,ISelfRegisteredService<IFinder<ProductCatalogImportCSVConfigurationConcordance>>
{
    private readonly IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> _finder;
    
    public ConcordanceFinder(IReadonlyRepository<ProductCatalogImportCSVConfigurationConcordance> finder)
    {
        _finder = finder;
    } 
    
    public ProductCatalogImportCSVConfigurationConcordance Find(params object[] keys)
    {
        if (keys.Length == 1 && keys[0] is ProductCatalogImportCSVConcordanceKey)
        {
            var id = keys[0] as ProductCatalogImportCSVConcordanceKey;
            return _finder
                .SingleOrDefault(x => x.ID == id.ID && x.Field == id.Field);
        }

        throw new ObjectNotFoundException(nameof(ProductCatalogImportCSVConfigurationConcordance));
    }

    public async ValueTask<ProductCatalogImportCSVConfigurationConcordance> FindAsync(object[] keys, CancellationToken token = default)
    {
        if (keys.Length == 1 && keys[0] is ProductCatalogImportCSVConcordanceKey)
        {
            var id = keys[0] as ProductCatalogImportCSVConcordanceKey;
            return await _finder.FirstOrDefaultAsync(x => x.ID == id.ID && x.Field == id.Field, token);
        }

        throw new ObjectNotFoundException(nameof(ProductCatalogImportCSVConfigurationConcordance));
    }

    public IFinder<ProductCatalogImportCSVConfigurationConcordance> With<TProperty>(System.Linq.Expressions.Expression<System.Func<ProductCatalogImportCSVConfigurationConcordance, TProperty>> include)
    {
        throw new System.NotImplementedException();
    }

    public IFinder<ProductCatalogImportCSVConfigurationConcordance> WithMany<TProperty>(Expression<Func<ProductCatalogImportCSVConfigurationConcordance, IEnumerable<TProperty>>> include)
    {
        throw new NotImplementedException();
    }
}