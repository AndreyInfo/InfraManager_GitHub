using InfraManager.DAL.ITAsset;

namespace IM.Core.Import.BLL.Interface.Import.ITAsset;
public interface IITAssetUndisposedBLL
{
    /// <summary>
    /// Метод создает необработанные строки импорта ит-активов
    /// </summary>
    /// <param name="undisposeds">необработанные строки для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task CreateAsync(ITAssetUndisposed[] undisposeds, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);
}
