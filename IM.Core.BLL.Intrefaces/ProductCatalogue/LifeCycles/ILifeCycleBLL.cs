using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

/// <summary>
/// бизнес Логика для работы с Жизненными циклами
/// </summary>
public interface ILifeCycleBLL
{
    /// <summary>
    /// Получение страницы результатов поиска по имени жизненного цикла 
    /// </summary>
    /// <param name="filter">фильтр для поиска и пагинации</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>массив моделей</returns>
    Task<LifeCycleDetails[]> GetDetailsArrayAsync(LifeCycleFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных жизненного цикла по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор жизненного цикла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>модель жизненного цикла</returns>
    Task<LifeCycleDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление жизненного цикла
    /// </summary>
    /// <param name="data">Данные жизненного цикла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>добавленная модель</returns>
    Task<LifeCycleDetails> AddAsync(LifeCycleData data, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление жизненного цикла по аналогии
    /// </summary>
    /// <param name="data">данные для добавления</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>добавленная модель</returns>
    Task<LifeCycleDetails> InsertAsAsync(LifeCycleData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление данных жизненного цикла
    /// </summary>
    /// <param name="id">идентификатор обновляемой сущности</param>
    /// <param name="data">Данные жизненного цикла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>обновленная модель</returns>
    Task<LifeCycleDetails> UpdateAsync(Guid id, LifeCycleData data, CancellationToken cancellationToken);


    /// <summary>
    /// Удаление жизненного цикла
    /// </summary>
    /// <param name="id">идентификатор удаляемого жизненного цикла</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
