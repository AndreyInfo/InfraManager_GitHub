using InfraManager.BLL.AccessManagement.Operations;
using InfraManager.DAL.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;

public interface ILifeCycleStateOperationBLL
{
    /// <summary>
    /// Добавление операции состояния жизненного цикла
    /// </summary>
    /// <param name="data">данные для добавления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>добавленная модель</returns>
    Task<LifeCycleStateOperationDetails> AddAsync(LifeCycleStateOperationData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновления операции состояния жизненного цикла
    /// </summary>
    /// <param name="id">идентификатор обновляемой сущности</param>
    /// <param name="data">данные для обновления</param>
    /// <param name="cancellationToken"></param>
    /// <returns>обновленная модель</returns>
    Task<LifeCycleStateOperationDetails> UpdateAsync(Guid id, LifeCycleStateOperationData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление операции состояния жизненного цикла
    /// </summary>
    /// <param name="id">идентификатор удаляемой сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение по идентификатору
    /// </summary>
    /// <param name="id">идентификатор операции</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>модель операции</returns>
    Task<LifeCycleStateOperationDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение операция для состояния с фильтрацией
    /// </summary>
    /// <param name="filter">фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>массив моделей операций</returns>
    Task<LifeCycleStateOperationDetails[]> GetDetailsArrayAsync(LifeCycleStateOperationFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение операций состояния жизненного цикла по фильтру
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Массив объектов</returns>
    Task<GroupedLifeCycleListItem[]> GetOperationsAsync(LifeCycleStateOperationFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранение операций состояния жизненного цикла доступных роли
    /// </summary>
    /// <param name="roleID">идентификатор роли</param>
    /// <param name="data">данные для сохранения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SaveOperationsAsync(Guid roleID, LifeCycleOperationsData[] data, CancellationToken cancellationToken = default);
}