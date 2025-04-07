using InfraManager.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Workflow
{
    /// <summary>
    /// Это интерфейс описывает сервис рабочих процедур
    /// </summary>
    /// <typeparam name="TEntity">Тип объекта рабочей процедуры</typeparam>
    public interface ICreateWorkflow<TEntity> where TEntity : class, IWorkflowEntity
    {
        /// <summary>
        /// Пытается создать и запустить новую рабочую процедуру для объекта
        /// </summary>
        /// <param name="entity">Ссылка на объект рабочую процедуру которого необходимо создать и запустить</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Признак того, была ли создана / запущена рабочая процедура</returns>
        Task<bool> TryStartNewAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
