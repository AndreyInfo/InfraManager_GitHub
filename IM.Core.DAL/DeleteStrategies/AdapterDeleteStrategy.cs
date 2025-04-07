using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.DAL.DeleteStrategies;

internal class AdapterDeleteStrategy : IDeleteStrategy<Adapter>,
        ISelfRegisteredService<IDeleteStrategy<Adapter>>
{
    private readonly IRepository<AssetEntity> _assetRepository;
    private readonly DbSet<Adapter> _adapterEntity;

    public AdapterDeleteStrategy(
          IRepository<AssetEntity> assetRepository
        , DbSet<Adapter> adapterEntity
        )
    {
        _assetRepository = assetRepository;
        _adapterEntity = adapterEntity;
    }

    public void Delete(Adapter entity)
    {
        _adapterEntity.Remove(entity);
        DeleteAsset(entity);
    }

    private void DeleteAsset(Adapter entity)
    {
        var asset = _assetRepository.FirstOrDefault(x => x.ID == entity.IMObjID);
        _assetRepository.Delete(asset);
    }
}