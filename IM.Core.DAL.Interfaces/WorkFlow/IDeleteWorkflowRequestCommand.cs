using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.WorkFlow
{
    /// <summary>
    /// Этот интерфейс описывает команду удаления WorkflowRequest
    /// (не нужно это копипастить и использовать в обычных задачах, этот сервис обслуживает костыль в WE)
    /// </summary>
    public interface IDeleteWorkflowRequestCommand
    {
        /// <summary>
        /// Добавляет новый WorkflowRequest в автономной транзакции
        /// </summary>
        /// <param name="workflowID">Идентификатор рабочей процедуры</param>
        /// <param name="cancellationToken"/>
        /// <returns></returns>
        Task<int> ExecuteAsync(Guid workflowID, CancellationToken cancellationToken = default);
    }
}
