using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.Adapters;

internal sealed class AdapterLoader : ILoadEntity<Guid, Adapter, AdapterDetails>,
    ISelfRegisteredService<ILoadEntity<Guid, Adapter, AdapterDetails>>
{
    private readonly IReadonlyRepository<Adapter> _adapterRepository;

    public AdapterLoader(IReadonlyRepository<Adapter> adapterRepository)
    {
        _adapterRepository = adapterRepository;
    }

    public async Task<Adapter> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _adapterRepository.With(x => x.Room)
            .ThenWith(x => x.Floor)
            .ThenWith(x => x.Building)
            .ThenWith(x => x.Organization)
            .With(x => x.Model)
            .FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);
    }
}