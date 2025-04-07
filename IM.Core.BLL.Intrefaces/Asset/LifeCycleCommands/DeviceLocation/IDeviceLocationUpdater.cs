using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;

/// <summary>
/// Интерфейс, обновляющий местоположение оборудования при выполнении команды.
/// </summary>
public interface IDeviceLocationUpdater
{
    /// <summary>
    /// Обновление местоположения оборудования.
    /// </summary>
    /// <param name="id">Идентификатор оборудования.</param>
    /// <param name="data">Данные нового местоположения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленное местоположение.</returns>
    public Task<DeviceLocationDetails> UpdateDeviceLocationAsync(Guid id, DeviceLocationData data, CancellationToken cancellationToken);
}
