using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.Models;

/// <summary>
/// Основные операции с моделями продуктов
/// </summary>
public interface IProductCatalogModelBLL
{
    /// <summary>
    /// Получение данных модели каталога продуктов по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор модели каталога продуктов</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>модель каталога продуктов</returns>
    Task<ProductModelOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Вставка одной модели продуктов
    /// </summary>
    /// <param name="data">Данные модели продуктов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>добавленную модель каталога продуктов</returns>
    Task<ProductModelOutputDetails> AddAsync(
        ProductCatalogModelData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Изменение данных модели продуктов
    /// </summary>
    /// <param name="id">Идентификатор модели продуктов</param>
    /// <param name="data">Данные для изменения</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>обновленную модель каталога продуктов</returns>
    Task<ProductModelOutputDetails> UpdateAsync(Guid id, ProductCatalogModelData data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление модели продуктов
    /// </summary>
    /// <param name="id">Идентификатор модели продуктов</param>
    /// <param name="flags">Флаги удаления моделей</param>
    /// <param name="token">Токен отмены</param>
    Task DeleteByFlagsAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken token = default);


    /// <summary>
    /// Получение списка моделей продуктов без разделения на страницы
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Модели каталога продуктов.</returns>
    Task<ProductModelOutputDetails[]> GetByFilterAsync(ProductCatalogModelFilter filter,
        CancellationToken token);

    /// <summary>
    /// Получение моделей по фильтру
    /// </summary>
    /// <param name="filterBy">фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>модели каталога продуктов</returns>
    Task<ProductModelOutputDetails[]> GetDetailsArrayAsync(ProductCatalogTreeFilter filterBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление моделей по фильтру
    /// </summary>
    /// <param name="treeFilter">фильтр</param>
    /// <param name="withObjects">флаг удаления с объектами или без</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteByFilterAsync(ProductCatalogTreeFilter treeFilter, bool withObjects, CancellationToken cancellationToken);

    /// <summary>
    /// Получение моделей по фильтру без проверки прав пользователя.
    /// </summary>
    /// <param name="filterBy">Фильтр.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модели каталога продуктов.</returns>
    Task<ProductModelOutputDetails[]> GetDetailsArrayWithoutTTZAsync(ProductCatalogTreeFilter filterBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение данных модели каталога продуктов по идентификатору без проверки прав пользователя.
    /// </summary>
    /// <param name="id">Идентификатор модели каталога продуктов.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель каталога продуктов.</returns>
    Task<ProductModelOutputDetails> DetailsWithoutTTZAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Изменение данных модели продуктов без проверки прав пользователя.
    /// </summary>
    /// <param name="id">Идентификатор модели продуктов.</param>
    /// <param name="data">Данные для изменения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленную модель каталога продуктов.</returns>
    Task<ProductModelOutputDetails> UpdateWithoutTTZAsync(Guid id, ProductCatalogModelData data, CancellationToken cancellationToken = default);
}