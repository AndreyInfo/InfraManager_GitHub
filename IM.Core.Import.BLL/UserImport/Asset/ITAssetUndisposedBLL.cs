using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.ITAsset;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ITAsset;

namespace IM.Core.Import.BLL.Import.Asset;
internal class ITAssetUndisposedBLL : IITAssetUndisposedBLL, ISelfRegisteredService<IITAssetUndisposedBLL>
{
    private readonly IRepository<ITAssetUndisposed> _repository;
    public ITAssetUndisposedBLL(IRepository<ITAssetUndisposed> repository)
    {
        _repository = repository;
    }
    public async Task CreateAsync(ITAssetUndisposed[] undisposeds, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var undisposed in undisposeds)
                _repository.Insert(undisposed);
        }
        catch (Exception e)
        {
            protocolLogger.Information($"ERR Ошибка добавления необработанной строки");
            protocolLogger.Error(e, $"Error Create undisposed it asset");
            throw;
        }
    }
}
