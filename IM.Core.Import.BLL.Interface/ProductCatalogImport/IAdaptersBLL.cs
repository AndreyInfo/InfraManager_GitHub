using InfraManager.DAL.Asset;

namespace IM.Core.Import.BLL.Interface.Import.ProductCatalog;
public interface IAdaptersBLL
{
    /// <summary>
    /// Метод создает объект каталога продуктов
    /// </summary>
    /// <param name="adapter">объект для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>созданный объект</returns>
    Task<Adapter> CreateAsync(Adapter adapter, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод создает модель объекта каталога продуктов
    /// </summary>
    /// <param name="adapterType">модель объекта для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>созданную модель объекта</returns>
    Task<AdapterType> CreateTypeAsync(AdapterType adapterType, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);
}
