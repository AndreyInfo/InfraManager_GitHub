using InfraManager.DAL.ConfigurationUnits;

namespace IM.Core.Import.BLL.Interface.Import.ConfigurationUnit;
public interface IConfigurationUnitsBLL
{
    /// <summary>
    /// Метод создает КЕ
    /// </summary>
    /// <param name="configurationUnits">массив КЕ для создания</param>
    /// <param name="protocolLogger">протокол импорта</param>
    /// <param name="cancellationToken">отмена задачи</param>
    Task CreateAsync(NetworkNode[] configurationUnits, IProtocolLogger protocolLogger, CancellationToken cancellationToken = default);
}
