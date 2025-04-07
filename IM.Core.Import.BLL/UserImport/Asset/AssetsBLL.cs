using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.ITAsset;
using InfraManager;
using InfraManager.DAL;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace IM.Core.Import.BLL.Import.Asset;
internal class AssetsBLL : IAssetsBLL, ISelfRegisteredService<IAssetsBLL>
{
    private readonly IRepository<AssetEntity> _repository;
    public AssetsBLL(IRepository<AssetEntity> repository)
    {
        _repository = repository;
    }
    public async Task CreateAsync(AssetEntity[] assets, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var asset in assets)
                _repository.Insert(asset);
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления ит-активов");
            protocolLogger.Error(e, $"Error Create IT assets");
            throw;
        }
    }

    public async Task<AssetEntity[]> GetAsync(int objectID, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        // TODO: добавить проверку прав
        return await _repository.ToArrayAsync(x => x.DeviceID == objectID, cancellationToken);
    }
}
