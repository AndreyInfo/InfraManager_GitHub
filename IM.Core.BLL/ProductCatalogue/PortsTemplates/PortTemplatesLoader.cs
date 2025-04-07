using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.PortTemplatess;

public class PortTemplatesLoader:ILoadEntity<PortTemplatesKey,PortTemplate, PortTemplatesDetails>,
    ISelfRegisteredService<ILoadEntity<PortTemplatesKey,PortTemplate, PortTemplatesDetails>>
{
    private readonly IRepository<PortTemplate> _repository;

    public PortTemplatesLoader(IRepository<PortTemplate> repository)
    {
        _repository = repository;
    }

    public Task<PortTemplate> LoadAsync(PortTemplatesKey id, CancellationToken cancellationToken = default)
    {
        return _repository.With(x=>x.JackType).With(x=>x.TechnologyType)
            .FirstOrDefaultAsync(x => x.ObjectID == id.ObjectID && x.PortNumber == id.PortNumber, cancellationToken);
    }
}