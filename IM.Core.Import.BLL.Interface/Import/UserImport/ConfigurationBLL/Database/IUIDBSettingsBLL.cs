using InfraManager.ServiceBase.ImportService.DBService;

namespace IM.Core.Import.BLL.Interface.Database;

public interface IUIDBSettingsBLL
{
    /// <summary>
    /// Получение таблицы настроек импорта из базы данных.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица настроек импорта из базы данных</returns>
    Task<UIDBSettingsOutputDetails[]> GetDetailsArrayAsync(UIDBSettingsFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для настроек импорта из базы данных каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация настроек импорта из базы данных</returns>
    Task<UIDBSettingsOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет настройки импорта из базы данных в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBSettingsOutputDetails> AddAsync(UIDBSettingsData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление настроек импорта из базы данных ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор настроек импорта из базы данных</param>
    /// <param name="data">Новые данные для настроек импорта из базы данных с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<UIDBSettingsOutputDetails> UpdateAsync(Guid id,
        UIDBSettingsData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление настроек импорта из базы данных
    /// </summary>
    /// <param name="id">Идентификатор настроек импорта из базы данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}