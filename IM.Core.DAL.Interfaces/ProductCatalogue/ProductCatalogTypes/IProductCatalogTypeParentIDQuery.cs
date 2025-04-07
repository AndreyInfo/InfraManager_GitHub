using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;

public interface IProductCatalogTypeParentIDQuery
{
    /// <summary>
    ///Возвращает идентификатор родительской категории
    /// Возвращает Guid.Empty, если тип не найден 
    /// </summary>
    /// <param name="id">Идентификатор типа</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Идентификатор родительской категории</returns>
    Task<Guid> ExecuteAsync(Guid id, CancellationToken token);
}