using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Floors;

/// <summary>
/// бизнес логика для работы с сущностью Floor(Этаж)
/// </summary>
public interface IFloorBLL
{
    /// <summary>
    /// Получение этажа по id
    /// </summary>
    /// <param name="id">идентификатор этажа</param>
    /// <param name="cancellationToken"></param>
    Task<FloorDetails> DetailsAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка Этажей, по фильтру
    /// </summary>
    /// <param name="filterBy">Условия выборки данных</param>\
    /// <param name="pageBy">Условия сортировки и постраничного вывода данных</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных этажей, удовлетворяющих условию выборки</returns>
    Task<FloorDetails[]> GetDetailsPageAsync(FloorListFilter filterBy, ClientPageFilter<Floor> pageBy, CancellationToken cancellationToken);

    /// <summary>
    /// Получение выборки данных этажей
    /// </summary>
    /// <param name="filterBy">Условия выборки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных этажей, удовлетворяющих условиям выборки</returns>
    Task<FloorDetails[]> GetDetailsArrayAsync(FloorListFilter filterBy, CancellationToken cancellationToken);

    /// <summary>
    /// Создает новый этаж
    /// </summary>
    /// <param name="data">Данные нового этажа</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные нового этажа</returns>
    Task<FloorDetails> AddAsync(FloorData data, CancellationToken cancellationToken);

    /// <summary>
    /// Изменяет существующий этаж
    /// </summary>
    /// <param name="id">Идентификатор этажа</param>
    /// <param name="data">Новые данные этажа</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные нового этажа</returns>
    Task<FloorDetails> UpdateAsync(int id, FloorData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление существующего этажа
    /// </summary>
    /// <param name="id">Идентификатор этажа</param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(int id, CancellationToken cancellationToken);
}
