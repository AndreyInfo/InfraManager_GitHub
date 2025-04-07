using InfraManager.DAL.Asset;

namespace IM.Core.Import.BLL.Interface.Import.Port;
public interface IPortBLL
{
    /// <summary>
    /// Метод создает порты оборудования
    /// </summary>
    /// <param name="ports">объекты для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task CreateAsync(ActivePort[] ports, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод получает массив портов оборудования из шаблона
    /// </summary>
    /// <param name="modelID">идентификатор модели оборудования</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns>массив портов оборудования из шаблона</returns>
    Task<PortTemplate[]> GetPortTemplateAsync(Guid modelID, CancellationToken cancellationToken = default);
}