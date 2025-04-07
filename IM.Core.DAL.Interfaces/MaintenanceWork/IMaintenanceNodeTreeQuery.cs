using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.MaintenanceWork;
/// <summary>
/// Получение узлов дерева регламентных работ
/// </summary>
public interface IMaintenanceNodeTreeQuery
{
    /// <summary>
    /// Получение узлов дерева регламентых работ по идентификатору родителя
    /// </summary>
    /// <param name="parentID">идентификатору родителя</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева регламентных работ</returns>
    Task<MaintenanceNodeTree[]> ExecuteAsync(Guid? parentID, CancellationToken cancellationToken);
}
