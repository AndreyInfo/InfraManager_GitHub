using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL.ListView;

namespace InfraManager.BLL.AssetsManagement.Hardware;

/// <summary>
/// Взаимодействие с оборудованием.
/// </summary>
public interface IHardwareBLL
{
    /// <summary>
    /// Получить полный список оборудования асинхронно.
    /// </summary>
    /// <param name="filterBy">Фильтр.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список.</returns>
    Task<HardwareListItem[]> AllHardwareAsync(
        ListViewFilterData<AllHardwareListFilter> filterBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить список оборудования, удовлетворяющий заданному фильтру, асинхронно.
    /// </summary>
    /// <param name="filterBy">Фильтр списка.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список оборудования, удоавлетворяющий заданному фильтру.</returns>
    Task<AssetSearchListItem[]> GetReportAsync(
        ListViewFilterData<AssetSearchListFilter> filterBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить список оборудования, используемого клиентом.
    /// </summary>
    /// <param name="filterBy">Фильтр списка.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Массив элементов <see cref="HardwareListItem"/>.</returns>
    Task<ClientsHardwareListItem[]> GetClientsHardwareListAsync(
        ListViewFilterData<ClientsHardwareListFilter> filterBy,
        CancellationToken cancellationToken = default);
}