using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;
public interface ILifeCycleStateOperationTransitionBLL
{
    /// <summary>
    /// Получение фильтрованного массива переходов
    /// </summary>
    /// <param name="filterBy">фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>массив переходов</returns>
    Task<LifeCycleStateOperationTransitionDetails[]> GetDetailsArrayAsync(LifeCycleStateOperationTransitionFilter filterBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение перехода по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<LifeCycleStateOperationTransitionDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление перехода
    /// </summary>
    /// <param name="data">данные добавления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>добавленная модель</returns>
    Task<LifeCycleStateOperationTransitionDetails> AddAsync(LifeCycleStateOperationTransitionData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление перехода
    /// </summary>
    /// <param name="id">идентификатор обновляемого перехода</param>
    /// <param name="data">данные обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>обновленная модель</returns>
    Task<LifeCycleStateOperationTransitionDetails> UpdateAsync(Guid id, LifeCycleStateOperationTransitionData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление перехода
    /// </summary>
    /// <param name="id">идентификатор удаляемого перехода</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
