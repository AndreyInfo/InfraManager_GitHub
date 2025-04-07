using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.Adapters
{
    /// <summary>
    /// Служба адаптеров
    /// </summary>
    public interface IAdapterBLL
    {
        /// <summary>
        /// Получение списка адаптеров для конкретного сетевого оборудования.
        /// </summary>
        /// <param name="networkDeviceID">Идентификатор сетевого оборудования.</param>
        /// <param name="filter">Базовый фильтр.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Массив данных адаптеров типа <see cref="AdapterListItemDetails"/>.</returns>
        Task<AdapterListItemDetails[]> GetAdaptersForNetworkDeviceAsync(int networkDeviceID, BaseFilter filter, CancellationToken cancellationToken);
    }
}