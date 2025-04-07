using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess;
public interface ISaveAccess
{
    /// <summary>
    /// Обновление доступа к объектам и его дочерним элементами
    /// Удаляет или добавляет, зависит от значения в data
    /// </summary>
    /// <param name="data">данные доступа</param>
    /// <param name="id">идентификатор доступа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task SaveAccessAsync(AccessElementsData data, Guid? id = null, CancellationToken cancellationToken = default);
}
