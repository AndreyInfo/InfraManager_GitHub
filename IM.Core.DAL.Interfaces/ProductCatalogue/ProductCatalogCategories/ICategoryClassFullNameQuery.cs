using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogCategories
{
    /// <summary>
    /// Запрос полного имени категории продуктов
    /// </summary>
    public interface ICategoryClassFullNameQuery
    {
        /// <summary>
        /// Получает полное имя категории продуктов, состоящее из категории и субкатегорий,
        /// разделенных слэшами
        /// </summary>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <param name="token">Токен отмены</param>
        /// <returns></returns>
        Task<string> QueryAsync(Guid categoryId, CancellationToken token = default);
    }
}