using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogCategories;

public interface IProductCatalogCategoryParentIDQuery
{
    /// <summary>
    /// Возвращает идентификатор родительской категории
    /// null для корня
    /// </summary>
    /// <param name="id">Идентификатор категории</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Идентификатор родительской категории</returns>
    Task<Guid?> ExecuteAsync(Guid id, CancellationToken token);
}