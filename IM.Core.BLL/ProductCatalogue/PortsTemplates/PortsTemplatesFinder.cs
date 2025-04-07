using Inframanager.DAL.ProductCatalogue.Import;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.PortsTemplates;

internal class PortsTemplatesFinder :
    IFinder<PortTemplate>,
    ISelfRegisteredService<IFinder<PortTemplate>>
{
    private readonly IReadonlyRepository<PortTemplate> _finder;

    public PortsTemplatesFinder(IReadonlyRepository<PortTemplate> finder)
    {
        _finder = finder;
    }

    public PortTemplate Find(params object[] keys)
    {
        if (keys.Length == 1 && keys[0] is PortTemplatesKey)
        {
            var id = keys[0] as PortTemplatesKey;
            return _finder
                .SingleOrDefault(x => x.ObjectID == id.ObjectID && x.PortNumber == id.PortNumber);
        }

        throw new ObjectNotFoundException(nameof(ProductCatalogImportCSVConfigurationConcordance));
    }

    public async ValueTask<PortTemplate> FindAsync(object[] keys, CancellationToken token = default)
    {
        if (keys.Length == 1 && keys[0] is PortTemplatesKey)
        {
            var id = keys[0] as PortTemplatesKey;
            return await _finder.FirstOrDefaultAsync(x => x.ObjectID == id.ObjectID && x.PortNumber == id.PortNumber, token);
        }

        throw new ObjectNotFoundException(nameof(ProductCatalogImportCSVConfigurationConcordance));
    }

    public IFinder<PortTemplate> With<TProperty>(System.Linq.Expressions.Expression<System.Func<PortTemplate, TProperty>> include)
    {
        throw new System.NotImplementedException();
    }

    public IFinder<PortTemplate> WithMany<TProperty>(Expression<Func<PortTemplate, IEnumerable<TProperty>>> include)
    {
        throw new NotImplementedException();
    }

}
