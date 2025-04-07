using InfraManager.BLL.Import;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services;
using InfraManager.Services.ScheduleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Scheduler
{
    public interface ISchedulerBLL
    {
        /// <summary>
        /// Возвращает список задач
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SchedulerListDetail[]> GetScheduleTasksAsync(ScheduleFilterRequest filter, CancellationToken cancellationToken = default);
        /// <summary>
        /// Добавляет задачу
        /// </summary>
        /// <param name="task">задача</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> AddScheduleTaskAsync(ScheduleTask task, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновляет задачу
        /// </summary>
        /// <param name="task">задача</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateScheduleTaskAsync(ScheduleTask task, CancellationToken cancellationToken = default);
        /// <summary>
        /// Останавливает задачу
        /// </summary>
        /// <param name="task">задача</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> StopTaskAsync(TaskCallbackRequest task, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает задачу
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ScheduleTask> GetAsync(Guid id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет задачу
        /// </summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Запускает немедленное выполнение задачи
        /// </summary>
        /// <param name="runTaskRequest">запрос на запуск</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TaskState> RunScheduleTaskAsync(RunTaskRequest runTaskRequest, CancellationToken cancellationToken = default);
        /// <summary>
        /// Возвращает список протоколов
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SchedulerProtocolsDetail[]> GetScheduleProtocolsAsync(ScheduleFilterRequest filter, CancellationToken cancellationToken = default);
    }
}
