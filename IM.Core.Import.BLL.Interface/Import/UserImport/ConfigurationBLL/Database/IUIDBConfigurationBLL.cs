using InfraManager.ServiceBase.ImportService.DBService;

namespace IM.Core.Import.BLL.Interface.Database;

public interface IUIDBConfigurationBLL
{
    /// <summary>
    /// Получение таблицы конфигураций импорта из базы данных.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица конфигураций импорта из базы данных</returns>
    Task<UIDBConfigurationOutputDetails[]> GetDetailsArrayAsync(UIDBConfigurationFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для конфигурации импорта из базы данных каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация конфигурации импорта из базы данных</returns>
    Task<UIDBConfigurationOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет конфугурацию импорта из базы данных в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBConfigurationOutputDetails> AddAsync(UIDBConfigurationData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление конфигурации импорта из базы данных ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор конфигурации импорта из базы данных</param>
    /// <param name="data">Новые данные для конфигурации импорта из базы данных с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBConfigurationOutputDetails> UpdateAsync(Guid id,
        UIDBConfigurationData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление конфигурации импорта из базы данных
    /// </summary>
    /// <param name="id">Идентификатор конфигурации импорта из базы данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}