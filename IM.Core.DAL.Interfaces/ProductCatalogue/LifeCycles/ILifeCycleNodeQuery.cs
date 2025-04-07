using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;

/// <summary>
/// Получение узлов дерева жизненных циклов
/// </summary>
public interface ILifeCycleNodeQuery
{
    /// <summary>
    /// Получение узлов дерева жизненных циклов
    /// </summary>
    /// <param name="parentID">идентификатор родительского элемента</param>
    /// <param name="roleID">идентификатор роли для которой выбраны операции</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева</returns>
    Task<LifeCycleTreeNode[]> ExecuteAsync(Guid? parentID, Guid? roleID, CancellationToken cancellationToken);
}
