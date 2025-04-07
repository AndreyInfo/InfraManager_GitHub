using InfraManager.DAL.Asset;

namespace IM.Core.Import.BLL.Interface.Import.ProductCatalog;
public interface INetworkDevicesBLL
{
    /// <summary>
    /// Метод создает объект каталога продуктов
    /// </summary>
    /// <param name="networkDevice">объект для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>созданный объект</returns>
    Task<NetworkDevice> CreateAsync(NetworkDevice networkDevice, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);
}
