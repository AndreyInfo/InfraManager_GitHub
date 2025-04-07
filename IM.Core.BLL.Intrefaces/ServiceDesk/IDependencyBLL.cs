using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    /// <summary>
    /// Бизнес логика работы с зависмостями для объектов СД
    /// </summary>
    /// <typeparam name="TDependency"></typeparam>
    public interface IDependencyBLL<TDependency>
        where TDependency : Dependency
    {
        /// <summary>
        /// Получение списка зависимостей объекта
        /// </summary>
        /// <param name="objectID">Идентификатор объекта СД, для которого нужны зависимые объекты</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns></returns>
        Task<DependencyDetails[]> GetDependenciesAsync(Guid objectID, CancellationToken cancellationToken);
    }
}