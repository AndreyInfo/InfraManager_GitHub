using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Operations;

public interface ILifeCycleListOperationQuery
{
    /// <summary>
    /// Запрос на получение объекта с массивом жизненных циклов
    /// </summary>
    /// <param name="rolesID">Массив идентификаторов ролей</param>
    /// <param name="lifeCycleStateID">Идентификатор состояния</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Объект с массивом жизненных циклов</returns>
    Task<GroupedLifeCycleListItem[]> ExecuteAsync(Guid[] rolesID, Guid? lifeCycleStateID, CancellationToken cancellationToken = default);
}