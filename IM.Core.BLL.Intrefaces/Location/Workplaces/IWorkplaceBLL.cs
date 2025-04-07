using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Workplaces;

/// <summary>
/// бизнес логика для работы с сущностью Workplace(Рабочее место)
/// </summary>
public interface IWorkplaceBLL
{
    /// <summary>
    /// Получение рабочего места по id
    /// </summary>
    /// <param name="id">идентификатор рабочего места</param>
    /// <param name="cancellationToken"></param>
    Task<WorkplaceDetails> DetailsAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка рабочих мест, по фильтру
    /// </summary>
    /// <param name="filterBy">Условия выборки данных</param>\
    /// <param name="pageBy">Условия сортировки и постраничного вывода данных</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных рабочих мест, удовлетворяющих условию выборки</returns>
    Task<WorkplaceDetails[]> GetDetailsPageAsync(WorkplaceListFilter filterBy, ClientPageFilter<Workplace> pageBy, CancellationToken cancellationToken);

    /// <summary>
    /// Получение выборки данных рабочих мест
    /// </summary>
    /// <param name="filterBy">Условия выборки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных рабочих мест, удовлетворяющих условиям выборки</returns>
    Task<WorkplaceDetails[]> GetDetailsArrayAsync(WorkplaceListFilter filterBy, CancellationToken cancellationToken);

    /// <summary>
    /// Создает новое рабочее место
    /// </summary>
    /// <param name="data">Данные нового рабочего места</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные нового рабочего места</returns>
    Task<WorkplaceDetails> AddAsync(WorkplaceData data, CancellationToken cancellationToken);

    /// <summary>
    /// Изменяет существующее рабочее место
    /// </summary>
    /// <param name="id">Идентификатор рабочего места</param>
    /// <param name="data">Новые данные рабочего места</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные нового рабочего места</returns>
    Task<WorkplaceDetails> UpdateAsync(int id, WorkplaceData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление существующего рабочего места
    /// </summary>
    /// <param name="id">Идентификатор рабочего места</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
