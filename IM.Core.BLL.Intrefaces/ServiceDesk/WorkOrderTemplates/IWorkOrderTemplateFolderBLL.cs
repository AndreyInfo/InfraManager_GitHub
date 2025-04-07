using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

/// <summary>
/// Папки шаблонов заданий
/// </summary>
public interface IWorkOrderTemplateFolderBLL
{
    /// <summary>
    /// Удаление папки и всех вложенных папок с их шаблонами
    /// </summary>
    /// <param name="id">идентификатор удаляемой сущности</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
