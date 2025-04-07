using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories
{
    public interface IProductCatalogCategoryBLL
    {
        /// <summary>
        /// Получение списка категорий по идентификатору базовой категории или корень, если идентификатор пуст
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>модели категорий каталога продуктов</returns>
        Task<ProductCatalogCategoryDetails[]> GetDetailsArrayAsync(ProductCatalogCategoryFilter filter,
            CancellationToken cancellationToken);

        /// <summary>
        /// получение свойств категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>модель категории каталога продуктов</returns>
        Task<ProductCatalogCategoryDetails> DetailsAsync(Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// Вставка категории в указанную категорию 
        /// </summary>
        /// <param name="data">Данные для вставки</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>добавленная модель категории каталога продуктов</returns>
        Task<ProductCatalogCategoryDetails> AddAsync(ProductCatalogCategoryData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление категории соответствующей идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data">Новые данные для типа с тем же идентификатором</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>обновленная модель категории каталога продуктов</returns>
        Task<ProductCatalogCategoryDetails> UpdateAsync(Guid id, ProductCatalogCategoryData data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="flags">флаги настройки удаления</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task DeleteAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken cancellationToken);

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получение списка типов по указанному фильтру
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>получение узлов дерева каталога продуктов</returns>
        Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter, CancellationToken token);
    }
}