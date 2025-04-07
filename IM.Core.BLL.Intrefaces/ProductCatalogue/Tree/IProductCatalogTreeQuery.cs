using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.Tree
{
    /// <summary>
    /// Интерфейс посредника для работы с деревом
    /// </summary>
    public interface IProductCatalogTreeQuery
    {
        /// <summary>
        /// Получение вложенных узлов по родительскому
        /// </summary>
        /// <param name="id">Идентификатор родительского узла</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Вложенные узлы</returns>
        public Task<ProductCatalogNode[]> GetTreeNodesByParentIdAsync(Guid id,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Реализация получения узла заданного типа
        /// </summary>
        public Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter,
            IEnumerable<ObjectClass> objectClasses, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение списка идентификаторов родителей узла по идентификатору узла
        /// </summary>
        /// <param name="id">Идентификатор узла</param>
        /// <param name="classId"></param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Упорядоченный список идентификаторов родителей узла</returns>
        public Task<TreeParentsDetails[]> GetTreeParentsDetailsAsync(Guid id, ObjectClass classId,
            CancellationToken token);
    }
}