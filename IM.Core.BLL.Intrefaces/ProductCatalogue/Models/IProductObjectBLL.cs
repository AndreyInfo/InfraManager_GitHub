using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Models;
public interface IProductObjectBLL
{
    /// <summary>
    /// Удаление объектов по идентификатору модели
    /// </summary>
    /// <param name="modelID">идентификатор модели</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task DeleteByModelIDAsync(Guid modelID, CancellationToken cancellationToken);

    /// <summary>
    /// Проверка наличия объектов по идентификатору модели
    /// </summary>
    /// <param name="modelID">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><c>True</c> если объекты найдены, иначе <c>False</c></returns>
    Task<bool> HasObjectsInModelAsync(Guid modelID, CancellationToken cancellationToken);
}

