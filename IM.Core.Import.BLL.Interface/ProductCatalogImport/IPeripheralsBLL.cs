using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace IM.Core.Import.BLL.Interface.Import.ProductCatalog;
public interface IPeripheralsBLL
{
    /// <summary>
    /// Метод создает объект каталога продуктов
    /// </summary>
    /// <param name="peripheral">объект для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>созданный объект</returns>
    Task<Peripheral> CreateAsync(Peripheral peripheral, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод создает модель объекта каталога продуктов
    /// </summary>
    /// <param name="peripheralType">модель объекта для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>созданную модель объекта</returns>
    Task<PeripheralType> CreateTypeAsync(PeripheralType peripheralType, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);
}
