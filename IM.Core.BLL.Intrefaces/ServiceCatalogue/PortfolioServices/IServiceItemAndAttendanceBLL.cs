using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

/// <summary>
/// Бизнес логика с элементами и услугами сервисов
/// Объединино ибо логика очень схожа
/// </summary>
/// <typeparam name="TEntity">Сущность хранимая в БД</typeparam>
/// <typeparam name="TDetails">модель для получения</typeparam>
/// <typeparam name="TData">модель для сохранения</typeparam>
/// <typeparam name="TTable">модель колонок для сущности</typeparam>
public interface IServiceItemAndAttendanceBLL<TEntity, TDetails, TData, TTable>
    where TEntity : PortfolioServiceItemAbstract, new()
    where TDetails : PortfolioServcieItemDetails
    where TData : PortfolioServcieItemData
    where TTable : class
{
    /// <summary>
    /// Получение данных у конкретного сервиса по id
    /// поддерживает скролинг
    /// </summary>
    /// <param name="serviceId"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TDetails[]> GetByServiceIdAsync(Guid serviceId, BaseFilter filter, CancellationToken cancellationToken);


    /// <summary>
    /// Получение всех данных об объекте по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление 
    /// поддерживает сохранение тегов и линий поддержки
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> AddAsync(TData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление
    /// поддерживает сохранение тегов и линий поддержки
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> UpdateAsync(Guid id, TData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление сущности
    /// </summary>
    /// <param name="id">идентификатор удаляемой сущности</param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}
