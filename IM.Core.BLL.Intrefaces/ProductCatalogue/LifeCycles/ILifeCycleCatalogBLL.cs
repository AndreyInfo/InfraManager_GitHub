using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;
public interface ILifeCycleCatalogBLL
{
    /// <summary>
    /// получение дерева операций для редактора ролей
    /// Иерархия дерева
    /// LifeCycle -> LifeCycleState -> LifeCycleStateOperation
    /// так же отмечается в каком узле есть выделенные операции
    /// </summary>
    /// <param name="filter">фильтр для получения дерева</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>узлы дерева</returns>
    Task<LifeCycleTreeNode[]> GetNodesAsync(LifeCycleTreeFilter filter, CancellationToken cancellationToken);
}
