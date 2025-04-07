using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public interface IProductCatalogTypeBLL
{
    /// <summary>
    /// Получение списка типов в указанной категории
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Модели типов каталога продуктов</returns>
    Task<ProductCatalogTypeDetails[]> GetDetailsArrayAsync(ProductCatalogTypeFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для типа каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор типа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Модель типа каталога продуктов</returns>
    Task<ProductCatalogTypeDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставка типа в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для вставки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленная модель типа каталога продуктов</returns>
    Task<ProductCatalogTypeDetails> AddAsync(ProductCatalogTypeData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление типа
    /// </summary>
    /// <param name="id">Идентификатор обновляемого типа</param>
    /// <param name="data">Новые данные для типа с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Обновленная модель типа каталога продуктов</returns>
    Task<ProductCatalogTypeDetails> UpdateAsync(Guid id,
        ProductCatalogTypeData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление типа
    /// </summary>
    /// <param name="id">Идентификатор типа</param>
    /// <param name="flags">Флаги удаления типа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка типов по указанному фильтру
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Узлы дерева каталога продуктов</returns>
    Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверка используется ли тип
    /// </summary>
    /// <param name="typeID">Идентификатор типа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Используется/нет</returns>
    Task<bool> IsUseAsync(Guid typeID, CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для типа без проверки прав пользователя на каталог продуктов.
    /// </summary>
    /// <param name="id">Идентификатор типа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель типа каталога продуктов.</returns>
    Task<ProductCatalogTypeDetails> DetailsWithoutTTZAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление типа без проверки прав пользователя на каталог продуктов.
    /// </summary>
    /// <param name="id">Идентификатор обновляемого типа.</param>
    /// <param name="data">Новые данные для типа с тем же идентификатором.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленная модель типа каталога продуктов.</returns>
    Task<ProductCatalogTypeDetails> UpdateWithoutTTZAsync(Guid id,
        ProductCatalogTypeData data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление типа без проверки прав пользователя на каталог продуктов.
    /// </summary>
    /// <param name="id">Идентификатор типа.</param>
    /// <param name="flags">Флаги удаления типа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteWithoutByFlagsTTZAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken cancellationToken);
}
