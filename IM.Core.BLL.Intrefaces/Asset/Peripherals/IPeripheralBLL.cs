using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.Peripherals;

/// <summary>
/// Служба периферийных устройств
/// </summary>
public interface IPeripheralBLL
{
    /// <summary>
    /// Получение списка периферийных устройств для конкретного сетевого оборудования.
    /// </summary>
    /// <param name="networkDeviceID">Идентификатор сетевого оборудования.</param>
    /// <param name="filter">Базовый фильтр.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных периферийных устройств типа <see cref="PeripheralListItemDetails"/>.</returns>
    Task<PeripheralListItemDetails[]> GetPeripheralsForNetworkDeviceAsync(int networkDeviceID, BaseFilter filter, CancellationToken cancellationToken);
}