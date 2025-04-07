using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.NetworkDevices;
internal sealed class NetworkDeviceLoader : ILoadEntity<int, NetworkDevice, NetworkDeviceDetails>
    , ISelfRegisteredService<ILoadEntity<int, NetworkDevice, NetworkDeviceDetails>>
{
    private readonly IReadonlyRepository<NetworkDevice> _networkDeviceRepository;

    public NetworkDeviceLoader(IReadonlyRepository<NetworkDevice> networkDeviceRepository)
    {
        _networkDeviceRepository = networkDeviceRepository;
    }

    public async Task<NetworkDevice> LoadAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _networkDeviceRepository.With(x => x.Room)
            .ThenWith(x => x.Floor)
            .ThenWith(x => x.Building)
            .ThenWith(x => x.Organization)
            .With(x => x.Model)
            .With(x => x.Rack)
            .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
    }
}
