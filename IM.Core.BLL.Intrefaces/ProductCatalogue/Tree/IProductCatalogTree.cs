using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.Tree
{
    /// <summary>
    /// Работа с узлами дерева каталогов
    /// </summary>
    public interface IProductCatalogTree
    {
        /// <summary>
        /// Получает узлы дерева каталогов по идентификатору узла родителя
        /// Если нужен корень, то передаем null
        /// </summary>
        /// <param name="id">Идентификатор родителя</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Узлы, содержащиеся в родительском узле или корне</returns>
        public Task<ProductCatalogNode[]> GetTreeNodeByParentAsync(Guid? id, CancellationToken token);

        /// <summary>
        /// Получение спика узлов дерева каталога продуктов по фильтру
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public Task<ProductCatalogNode[]> ExecuteAsync(ProductCatalogTreeFilter filter, CancellationToken cancellationToken = default);
    }
}