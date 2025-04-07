using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.DAL.DeleteStrategies;

internal class NetworkDeviceDeleteStrategy : IDeleteStrategy<NetworkDevice>
    , ISelfRegisteredService<IDeleteStrategy<NetworkDevice>>
{
    private readonly DbSet<NetworkDevice> _networkDeviceDbSet;
    private readonly IRepository<AssetEntity> _assetRepository;

    public NetworkDeviceDeleteStrategy(DbSet<NetworkDevice> networkDeviceDbSet
        , IRepository<AssetEntity> assetRepository)
    {
        _networkDeviceDbSet = networkDeviceDbSet;
        _assetRepository = assetRepository;
    }

    public void Delete(NetworkDevice entity)
    {
        var asset = _assetRepository.FirstOrDefault(x => x.ID == entity.IMObjID);

        _assetRepository.Delete(asset);
        _networkDeviceDbSet.Remove(entity);
    }
}
