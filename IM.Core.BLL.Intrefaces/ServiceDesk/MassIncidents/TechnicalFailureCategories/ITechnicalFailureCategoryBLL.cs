using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;

public interface ITechnicalFailureCategoryBLL
{
    /// <summary>
    ///     Весь список категорий технических сбоев
    /// </summary>
    /// <param name="filterBy">Сортировка списка</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список категорий технических сбоев</returns>
    Task<TechnicalFailureCategoryDetails[]> GetDetailsArrayAsync(
        TechnicalFailureCategoryFilter filterBy,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Возвращает категорий технических сбоев
    /// </summary>
    /// <param name="id">Идентификатор категории технических сбоев</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Категорий технических сбоев</returns>
    Task<TechnicalFailureCategoryDetails> DetailsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Создает новую категорию технических сбоев
    /// </summary>
    /// <param name="data">Данные новой категории технических сбоев</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Созданную категорию технических сбоев</returns>
    Task<TechnicalFailureCategoryDetails> AddAsync(
        TechnicalFailureCategoryData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет категорию технических сбоев
    /// </summary>
    /// <param name="id">Идентификатор категории технических сбоев</param>
    /// <param name="data">Данные категории технических сбоев</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Измененную категорию технических сбоев</returns>
    Task<TechnicalFailureCategoryDetails> UpdateAsync(
        int id, 
        TechnicalFailureCategoryData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет категорию технических сбоев
    /// </summary>
    /// <param name="id">Идентификатор категории технических сбоев</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет связь сервиса с категорией технического сбоя
    /// </summary>
    /// <param name="id">Идентификатор категории технического сбоя</param>
    /// <param name="data">Данные связи</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Данные связи после сохранения</returns>
    Task<ServiceReferenceDetails> AddServiceReferenceAsync(
        int id, 
        ServiceReferenceData data, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Изменяет данные связи сервиса и категории технических сбоев
    /// </summary>
    /// <param name="id">Идентификатор категории</param>
    /// <param name="serviceID">Идентификатор сервиса</param>
    /// <param name="data">Данные связки (изменяемые)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Ссылка на детальные данные связи после сохранения</returns>
    Task<ServiceReferenceDetails> UpdateServiceReferenceAsync(
        int id,
        Guid serviceID,
        ServiceReferenceUpdatableData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет связь сервиса и категории технического сбоя
    /// </summary>
    /// <param name="id">Идентификатор категории технического сбоя</param>
    /// <param name="serviceID">Идентификатор </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteServiceReferenceAsync(
        int id,
        Guid serviceID,
        CancellationToken cancellationToken = default);
}