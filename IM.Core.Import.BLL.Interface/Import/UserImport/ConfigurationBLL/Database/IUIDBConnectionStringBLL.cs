using InfraManager.ServiceBase.ImportService.DBService;

namespace IM.Core.Import.BLL.Interface.Database;

public interface IUIDBConnectionStringBLL
{
    /// <summary>
    /// Получение таблицы строк подключения для импорта из базы данных.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица строк подключения для импорта из базы данных</returns>
    Task<UIDBConnectionStringOutputDetails[]> GetDetailsArrayAsync(UIDBConnectionStringFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для строки подключения для импорта из базы данных каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация строки подключения для импорта из базы данных</returns>
    Task<UIDBConnectionStringOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет строку подключения для импорта из базы данных в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBConnectionStringOutputDetails> AddAsync(UIDBConnectionStringData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление строки подключения для импорта из базы данных ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор строки подключения для импорта из базы данных</param>
    /// <param name="data">Новые данные для строки подключения для импорта из базы данных с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBConnectionStringOutputDetails> UpdateAsync(Guid id,
        UIDBConnectionStringData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление строки подключения для импорта из базы данных
    /// </summary>
    /// <param name="id">Идентификатор строки подключения для импорта из базы данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}