namespace IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration;

public interface IProductCatalogImportCSVConfigurationBLL
    :IBaseBLL<Guid,ProductCatalogImportCSVConfigurationFilter,ProductCatalogImportCSVConfigurationDetails,ProductCatalogImportCSVConfigurationOutputDetails>
{
    // /// <summary>
    // /// Получение nаблицы конфигураций csv задачи импорта каталога продуктов.
    // /// </summary>
    // /// <param name="filter">Фильтр таблицы</param>
    // /// <param name="cancellationToken">Токен отмены</param>
    // /// <returns>Таблица конфигураций csv задачи импорта каталога продуктов</returns>
    // Task<ProductCatalogImportCSVConfigurationOutputDetails[]> GetDetailsArrayAsync(
    //     ProductCatalogImportCSVConfigurationFilter filter,
    //     CancellationToken cancellationToken);
    //
    // /// <summary>
    // /// Получение данных для конфигурации csv задачи импорта каталога продуктов каталога продуктов 
    // /// </summary>
    // /// <param name="id">Идентификатор</param>
    // /// <param name="cancellationToken"></param>
    // /// <returns>Детализация конфигурации csv задачи импорта каталога продуктов</returns>
    // Task<ProductCatalogImportCSVConfigurationOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);
    //
    // /// <summary>
    // /// Вставляет конфигурацию csv задачи импорта каталога продуктов в указанную категорию 
    // /// </summary>
    // /// <param name="data">Данные для встаки</param>
    // /// <param name="cancellationToken">Токен отмены</param>
    // /// <returns></returns>
    // Task<ProductCatalogImportCSVConfigurationOutputDetails> AddAsync(ProductCatalogImportCSVConfigurationDetails data,
    //     CancellationToken cancellationToken = default);
    //
    // /// <summary>
    // /// Обновление конфигурации csv задачи импорта каталога продуктов ссответствующего идентификатору
    // /// </summary>
    // /// <param name="id">Идентификатор конфигурации csv задачи импорта каталога продуктов</param>
    // /// <param name="data">Новые данные для конфигурации csv задачи импорта каталога продуктов с тем же идентификатором</param>
    // /// <param name="cancellationToken">Токен отмены</param>
    // /// <returns></returns>
    // Task<ProductCatalogImportCSVConfigurationOutputDetails> UpdateAsync(Guid id,
    //     ProductCatalogImportCSVConfigurationDetails data,
    //     CancellationToken cancellationToken = default);
    //
    // /// <summary>
    // /// Удаление конфигурации csv задачи импорта каталога продуктов
    // /// </summary>
    // /// <param name="id">Идентификатор конфигурации csv задачи импорта каталога продуктов</param>
    // /// <param name="cancellationToken">Токен отмены</param>
    // /// <returns></returns>
    // Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}