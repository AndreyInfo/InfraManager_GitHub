using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogImportSettings;

public interface IProductCatalogImportSettingBLL
{
    /// <summary>
    /// Получение nаблицы задач импорта каталога продуктов.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица задач импорта каталога продуктов</returns>
    Task<ProductCatalogImportSettingOutputDetails[]> GetDetailsArrayAsync(ProductCatalogImportSettingFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для задачи импорта каталога продуктов каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация задачи импорта каталога продуктов</returns>
    Task<ProductCatalogImportSettingOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет задачу импорта каталога продуктов в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<ProductCatalogImportSettingOutputDetails> AddAsync(ProductCatalogImportSettingDetails data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление задачи импорта каталога продуктов ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи импорта каталога продуктов</param>
    /// <param name="data">Новые данные для задачи импорта каталога продуктов с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<ProductCatalogImportSettingOutputDetails> UpdateAsync(Guid id,
        ProductCatalogImportSettingDetails data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление задачи импорта каталога продуктов
    /// </summary>
    /// <param name="id">Идентификатор задачи импорта каталога продуктов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}