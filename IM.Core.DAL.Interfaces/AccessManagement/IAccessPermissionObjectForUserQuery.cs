using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.AccessManagement;
public interface IAccessPermissionObjectForUserQuery
{
    /// <summary>
    /// Получение объектов к которым пользователь имеет доступ
    /// </summary>
    /// <param name="userID">идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>модели доступа к объектам</returns>
    Task<AccessPermissionObject[]> ExecuteAsync(Guid userID, ObjectClass? objectClassID, CancellationToken cancellationToken);
}
