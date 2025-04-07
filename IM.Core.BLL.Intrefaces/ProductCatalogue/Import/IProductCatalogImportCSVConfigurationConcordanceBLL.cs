using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Import;

public interface IProductCatalogImportCSVConfigurationConcordanceBLL
{
    /// <summary>
    /// Получение nаблицы конфигураций поля задачи импорта каталога продуктов.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица конфигураций поля задачи импорта каталога продуктов</returns>
    Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails[]> GetDetailsArrayAsync(
        ProductCatalogImportCSVConfigurationConcordanceFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для конфигурации поля задачи импорта каталога продуктов каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация конфигурации поля задачи импорта каталога продуктов</returns>
    Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails> DetailsAsync(
        ProductCatalogImportCSVConcordanceKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет конфигурацию поля задачи импорта каталога продуктов в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails> AddAsync(
        ProductCatalogImportCSVConfigurationConcordanceDetails data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление конфигурации поля задачи импорта каталога продуктов ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор конфигурации поля задачи импорта каталога продуктов</param>
    /// <param name="data">Новые данные для конфигурации поля задачи импорта каталога продуктов с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails> UpdateAsync(
        ProductCatalogImportCSVConcordanceKey id,
        ProductCatalogImportCSVConfigurationConcordanceDetails data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление конфигурации поля задачи импорта каталога продуктов
    /// </summary>
    /// <param name="id">Идентификатор конфигурации поля задачи импорта каталога продуктов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(ProductCatalogImportCSVConcordanceKey id, CancellationToken cancellationToken = default);
}