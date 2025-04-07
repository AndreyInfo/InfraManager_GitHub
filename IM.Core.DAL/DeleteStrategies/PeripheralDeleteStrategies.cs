using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.DAL.DeleteStrategies;

internal class PeripheralDeleteStrategy : IDeleteStrategy<Peripheral>,
        ISelfRegisteredService<IDeleteStrategy<Peripheral>>
{
    private readonly IRepository<AssetEntity> _assetRepository;
    private readonly DbSet<Peripheral> _peripheralEntity;

    public PeripheralDeleteStrategy(
          IRepository<AssetEntity> assetRepository
        , DbSet<Peripheral> peripheralEntity
        )
    {
        _assetRepository = assetRepository;
        _peripheralEntity = peripheralEntity;
    }

    public void Delete(Peripheral entity)
    {
        _peripheralEntity.Remove(entity);
        DeleteAsset(entity);
    }

    private void DeleteAsset(Peripheral entity)
    {
        var asset = _assetRepository.FirstOrDefault(x => x.ID == entity.IMObjID);
        _assetRepository.Delete(asset);
    }
}