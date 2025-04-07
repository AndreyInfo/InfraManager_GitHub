using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Rooms;

/// <summary>
/// бизнес логика для работы с сущностью Room(Комната)
/// </summary>
public interface IRoomBLL
{
    /// <summary>
    /// Получение комнаты по id
    /// </summary>
    /// <param name="id">идентификатор комнаты</param>
    /// <param name="cancellationToken"></param>
    Task<RoomDetails> DetailsAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка комнат, по фильтру
    /// </summary>
    /// <param name="filterBy">Условия выборки данных</param>\
    /// <param name="pageBy">Условия сортировки и постраничного вывода данных</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных комнат, удовлетворяющих условию выборки</returns>
    Task<RoomDetails[]> GetDetailsPageAsync(RoomListFilter filterBy, ClientPageFilter<Room> pageBy, CancellationToken cancellationToken);

    /// <summary>
    /// Получение выборки данных комнат
    /// </summary>
    /// <param name="filterBy">Условия выборки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных комнат, удовлетворяющих условиям выборки</returns>
    Task<RoomDetails[]> GetDetailsArrayAsync(RoomListFilter filterBy, CancellationToken cancellationToken);

    /// <summary>
    /// Создает новую комнату
    /// </summary>
    /// <param name="data">Данные новой комнаты</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные новой комнаты</returns>
    Task<RoomDetails> AddAsync(RoomData data, CancellationToken cancellationToken);

    /// <summary>
    /// Изменяет существующую комнату
    /// </summary>
    /// <param name="id">Идентификатор комнаты</param>
    /// <param name="data">Новые данные комнаты</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные новой комнаты</returns>
    Task<RoomDetails> UpdateAsync(int id, RoomData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление существующей комнаты
    /// </summary>
    /// <param name="id">Идентификатор комнаты</param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(int id, CancellationToken cancellationToken);
}
