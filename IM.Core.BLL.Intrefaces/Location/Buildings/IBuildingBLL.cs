using Inframanager.BLL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Buildings;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Бизнес логика с сущностью Здание
/// </summary>
public interface IBuildingBLL
{
    /// <summary>
    /// Получение здания по id
    /// </summary>
    /// <param name="id">идентификатор здания</param>
    /// <param name="cancellationToken"></param>
    Task<BuildingDetails> DetailsAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка Зданий, по фильтру
    /// </summary>
    /// <param name="filterBy">Условия выборки данных</param>\
    /// <param name="pageBy">Условия сортировки и постраничного вывода данных</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных зданий, удовлетворяющих условию выборки</returns>
    Task<BuildingDetails[]> GetDetailsPageAsync(BuildingListFilter filterBy, ClientPageFilter<Building> pageBy, CancellationToken cancellationToken);

    /// <summary>
    /// Получение выборки данных зданий
    /// </summary>
    /// <param name="filterBy">Условия выборки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных зданий, удовлетворяющих условиям выборки</returns>
    Task<BuildingDetails[]> GetDetailsArrayAsync(BuildingListFilter filterBy, CancellationToken cancellationToken);

    /// <summary>
    /// Создает новое здание
    /// </summary>
    /// <param name="data">Данные нового здания</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные нового здания</returns>
    Task<BuildingDetails> AddAsync(BuildingData data, CancellationToken cancellationToken);

    /// <summary>
    /// Изменяет существующее здание
    /// </summary>
    /// <param name="id">Идентификатор задния</param>
    /// <param name="data">Новые данные здания</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные нового здания</returns>
    Task<BuildingDetails> UpdateAsync(int id, BuildingData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление существующего здания
    /// </summary>
    /// <param name="id">Идентификатор здания</param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(int id, CancellationToken cancellationToken);
}