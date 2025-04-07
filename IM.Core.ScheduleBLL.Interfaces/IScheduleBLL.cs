using InfraManager.BLL.Import;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services;
using InfraManager.Services.ScheduleService;

namespace IM.Core.ScheduleBLL.Interfaces
{
    public interface IScheduleBLL
    {
        /// <summary>
        /// Возвращает список заданий планировщика
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ScheduleTask[]> GetListAsync(ScheduleFilterRequest filter, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Добавляет задание планировщика
        /// </summary>
        /// <param name="task">Задание</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Guid> AddAsync(ScheduleTask task, CancellationToken cancellationToken = default);

        /// <summary>
        /// Останавливает выполнение задания
        /// </summary>
        /// <param name="stopJobRequest">Задание</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> StopTaskAsync(TaskCallbackRequest stopJobRequest, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Обновляет задание
        /// </summary>
        /// <param name="task">Задание</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateAsync(ScheduleTask task, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Возврящает задание
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">Идентификатор задания</param>
        /// <returns></returns>
        Task<ScheduleTask> GetAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Удаляет задание
        /// </summary>
        /// <param name="id">Идентификатор задания</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Запускает задание
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TaskState> RunTaskAsync(RunTaskRequest runTaskRequest, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Удаляет связанные с задачей задания планировщика кроме находящихся в статусе выполнения
        /// </summary>
        /// <param name="id">Идентификатор настроек импорта</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Флаг показывает произошло ли удаление</returns>
        Task<bool> DeleteTasksByUISettingIDAsync(Guid id, CancellationToken cancellationToken = default);
      }
}
