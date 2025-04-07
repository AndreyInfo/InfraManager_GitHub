using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.Tree;

public interface IProductCatalogTreeParentBLL
{
    /// <summary>
    /// Получение списка идентификаторов родителей узла по идентификатору узла
    /// </summary>
    /// <param name="id">Идентификатор узла</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Упорядоченный список идентификаторов родителей узла</returns>
    Task<List<TreeParentsDetails>> GetProductCatalogCategoryParentsData(Guid id, CancellationToken token);
}