using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;

namespace InfraManager.BLL.ProductCatalogue.PortTemplates;

public interface IPortTemplatesBLL
{
    /// <summary>
    /// Возвращает страницу отфильтрованных отсортированных данных портов
    /// </summary>
    /// <param name="portTemplatesFilter">Фильтр портов</param>
    /// <param name="pageFilter">Данные страницы и сортировки</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Страница данных о портах</returns>
    Task<PortTemplatesDetails[]> GetListAsync(PortTemplatesFilter portTemplatesFilter, CancellationToken token);

    /// <summary>
    /// Возвращает данные порта
    /// </summary>
    /// <param name="id">Идентификатор порта</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные порта</returns>
    Task<PortTemplatesDetails> DetailsAsync(PortTemplatesKey id, CancellationToken token);

    /// <summary>
    /// Добавляет данные порта в базу
    /// </summary>
    /// <param name="data">Данные порта</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Записанные данные порта</returns>
    Task<PortTemplatesDetails> AddAsync(PortTemplatesData data, CancellationToken token);

    /// <summary>
    /// Обновляет данные порта
    /// </summary>
    /// <param name="id">Идентификатор порта</param>
    /// <param name="data">Новые данные порта</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Записанные данные порта</returns>
    Task<PortTemplatesDetails> UpdateAsync(PortTemplatesKey key, PortTemplatesData data, CancellationToken token);

    /// <summary>
    /// Удаляет данные порта из базы
    /// </summary>
    /// <param name="id">Идентификатор порта</param>
    /// <param name="token">Токен отмены</param>
    Task DeleteAsync(PortTemplatesKey id, CancellationToken token);
}