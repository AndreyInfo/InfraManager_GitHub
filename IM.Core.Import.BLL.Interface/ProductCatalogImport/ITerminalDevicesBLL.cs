using InfraManager.DAL.Asset;

namespace IM.Core.Import.BLL.Interface.Import.ProductCatalog;
public interface ITerminalDevicesBLL
{
    /// <summary>
    /// Метод создает объект каталога продуктов
    /// </summary>
    /// <param name="terminalDevice">объект для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>созданный объект</returns>
    Task<TerminalDevice> CreateAsync(TerminalDevice terminalDevice, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод создает модель объекта каталога продуктов
    /// </summary>
    /// <param name="terminalDeviceType">модель объекта для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>созданную модель объекта</returns>
    Task<TerminalDeviceModel> CreateTypeAsync(TerminalDeviceModel terminalDeviceType, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);
}
