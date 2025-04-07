using Inframanager.BLL;
using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;
using PortAdapterEntity = InfraManager.DAL.Asset.PortAdapter;

namespace InfraManager.BLL.Asset.PortAdapter;

internal sealed class PortAdapterLoader : ILoadEntity<Guid, PortAdapterEntity, PortAdapterDetails>
    , ISelfRegisteredService<ILoadEntity<Guid, PortAdapterEntity, PortAdapterDetails>>
{
    private readonly IReadonlyRepository<PortAdapterEntity> _repository;

    public PortAdapterLoader(IReadonlyRepository<PortAdapterEntity> repository)
    {
        _repository = repository;
    }

    public Task<PortAdapterEntity> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        return _repository
            .With(x => x.JackType)
            .With(x => x.TechnologyType)
            .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
    }
}