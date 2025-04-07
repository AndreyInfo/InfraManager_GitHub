using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Import;

public interface IProductCatalogImportSettingTypesBLL
{
    /// <summary>
    /// Получение nаблицы разрешенныйх типов для импорта каталога продуктов.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица разрешенныйх типов для импорта каталога продуктов</returns>
    Task<ProductCatalogImportSettingTypesOutputDetails[]> GetDetailsArrayAsync(
        ProductCatalogImportSettingTypesFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для разрешенного типа для импорта каталога продуктов каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация разрешенного типа для импорта каталога продуктов</returns>
    Task<ProductCatalogImportSettingTypesOutputDetails> DetailsAsync(ProductCatalogImportSettingsType id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет разрешенный тип для импорта каталога продуктов в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<ProductCatalogImportSettingTypesOutputDetails> AddAsync(ProductCatalogImportSettingsType data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление разрешенного типа для импорта каталога продуктов ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор разрешенного типа для импорта каталога продуктов</param>
    /// <param name="data">Новые данные для разрешенного типа для импорта каталога продуктов с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<ProductCatalogImportSettingTypesOutputDetails> UpdateAsync(ProductCatalogImportSettingsType id,
        ProductCatalogImportSettingTypesDetails data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление разрешенного типа для импорта каталога продуктов
    /// </summary>
    /// <param name="id">Идентификатор разрешенного типа для импорта каталога продуктов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(ProductCatalogImportSettingsType id, CancellationToken cancellationToken = default);
}