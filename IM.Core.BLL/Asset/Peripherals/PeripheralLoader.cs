using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.Peripherals;

internal sealed class PeripheralLoader : ILoadEntity<Guid, Peripheral, PeripheralDetails>,
    ISelfRegisteredService<ILoadEntity<Guid, Peripheral, PeripheralDetails>>
{
    private readonly IReadonlyRepository<Peripheral> _peripheralRepository;

    public PeripheralLoader(IReadonlyRepository<Peripheral> peripheralRepository)
    {
        _peripheralRepository = peripheralRepository;
    }

    public async Task<Peripheral> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _peripheralRepository.With(x => x.Room)
            .ThenWith(x => x.Floor)
            .ThenWith(x => x.Building)
            .ThenWith(x => x.Organization)
            .With(x => x.Model)
            .FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);
    }
}