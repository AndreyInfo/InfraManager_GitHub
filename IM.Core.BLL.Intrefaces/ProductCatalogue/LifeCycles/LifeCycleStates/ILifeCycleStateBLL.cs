using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
public interface ILifeCycleStateBLL
{
    /// <summary>
    /// Получение состояния по идентификатору
    /// </summary>
    /// <param name="id">идентификатор получаемой сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>модель состояния</returns>
    Task<LifeCycleStateDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление состояния жизненного цикла
    /// </summary>
    /// <param name="data">модель обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>добавленная модель</returns>
    Task<LifeCycleStateDetails> AddAsync(LifeCycleStateData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление состояния жизненного цикла
    /// </summary>
    /// <param name="id">идентификатор обновляемой сущности</param>
    /// <param name="data">модель обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>обновленная модель</returns>
    Task<LifeCycleStateDetails> UpdateAsync(Guid id, LifeCycleStateData data, CancellationToken cancellationToken);


    /// <summary>
    /// Удаление состояния
    /// </summary>
    /// <param name="id">идентификатор удаляемой состояния</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение состояний по идентификатор жизненного цикла
    /// </summary>
    /// <param name="filter">фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>модели состояний</returns>
    Task<LifeCycleStateDetails[]> GetByLifeCycleIDAsync(LifeCycleStateFilter filter, CancellationToken cancellationToken);
}
