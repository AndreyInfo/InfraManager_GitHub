using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public interface IAssetCheckerBLL
{
    /// <summary>
    /// Проверка завязанных типов объектов к типу каталога продуктов
    /// </summary>
    /// <param name="type">тип каталога продуктов</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>могут быть привязаны или нет</returns>
    Task<bool> HasAssetAsync(ProductCatalogType type, CancellationToken cancellationToken);
}