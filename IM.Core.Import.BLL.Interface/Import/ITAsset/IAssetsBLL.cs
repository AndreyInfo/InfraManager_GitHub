using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace IM.Core.Import.BLL.Interface.Import.ITAsset;
public interface IAssetsBLL
{
    /// <summary>
    /// Метод получает ит-актив
    /// </summary>
    /// <param name="objectID">ид объекта модели</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>массив объектов имущества</returns>
    Task<AssetEntity[]> GetAsync(int objectID, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод создает ит-активы
    /// </summary>
    /// <param name="assets">ит-активы для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task CreateAsync(AssetEntity[] assets, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);
}
