using InfraManager.ServiceBase.ImportService.DBService;

namespace IM.Core.Import.BLL.Interface.Database;

public interface IUIDBFieldsBLL
{
    /// <summary>
    /// Получение таблицы полей таблицы импорта из базы данных.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица полей таблицы импорта из базы данных</returns>
    Task<UIDBFieldsOutputDetails[]> GetDetailsArrayAsync(UIDBFieldsFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для полей таблицы импорта из базы данных каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация полей таблицы импорта из базы данных</returns>
    Task<UIDBFieldsOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет поле таблицы импорта из базы данных в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBFieldsOutputDetails> AddAsync(UIDBFieldsData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление полей таблицы импорта из базы данных ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор полей таблицы импорта из базы данных</param>
    /// <param name="data">Новые данные для полей таблицы импорта из базы данных с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBFieldsOutputDetails> UpdateAsync(Guid id,
        UIDBFieldsData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление полей таблицы импорта из базы данных
    /// </summary>
    /// <param name="id">Идентификатор полей таблицы импорта из базы данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}