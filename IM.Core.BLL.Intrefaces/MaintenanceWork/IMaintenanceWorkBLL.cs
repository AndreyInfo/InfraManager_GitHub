using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.MaintenanceWork;

/// <summary>
/// Бизнес логика для работы с регламентными работами и их папками
/// </summary>
public interface IMaintenanceWorkBLL
{
    /// <summary>
    /// Возвращает дерево
    /// </summary>
    /// <param name="parentID">идентификатор родительского узла</param>
    /// <returns>дочерние узлы дерева</returns>
    Task<MaintenanceNodeTreeDetails[]> GetFolderTreeAsync(Guid? parentID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает путь до указанного объекта
    /// </summary>
    /// <param name="id">идентификатор объекта</param>
    /// <param name="classID">класс объекта</param>
    /// <returns>родительские узлы</returns>
    Task<MaintenanceNodeTreeDetails[]> GetPathToElementAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken = default);
}