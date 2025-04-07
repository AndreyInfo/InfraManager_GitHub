using InfraManager.BLL.Import;
using InfraManager.BLL.Scheduler;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services;
using InfraManager.Services.ScheduleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Scheduler
{
    [Authorize]
    [ApiController]
    [Route("bff/[controller]")]
    public class SchedulerServiceController : ControllerBase
    {
        private ISchedulerBLL _service;

        public SchedulerServiceController(ISchedulerBLL service)
        {
            _service = service;
        }

        /// <summary>
        /// Возвращает список заданий планировщика
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SchedulerListDetail[]> GetScheduleTasksAsync([FromQuery] ScheduleFilterRequest filter, CancellationToken cancellationToken = default)
        {
            return await _service.GetScheduleTasksAsync(filter, cancellationToken);
        }
        /// <summary>
        /// Возвращает список протоколов планировщика
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("protocol")]
        public async Task<SchedulerProtocolsDetail[]> GetScheduleProtocolsAsync([FromQuery] ScheduleFilterRequest filter, CancellationToken cancellationToken = default)
        {
            return await _service.GetScheduleProtocolsAsync(filter, cancellationToken);
        }
        /// <summary>
        /// Создает задание планировшика
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> AddScheduleTaskAsync(ScheduleTask task, CancellationToken cancellationToken = default)
        {
            return await _service.AddScheduleTaskAsync(task, cancellationToken);
        }
        /// <summary>
        /// Редактирует задание планировщика
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<OperationResult> UpdateScheduleTaskAsync(ScheduleTask task, CancellationToken cancellationToken = default)
        {
            return await _service.UpdateScheduleTaskAsync(task, cancellationToken);
        }
        /// <summary>
        /// Останавливает задание планировщика
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("stop")]
        public async Task<bool> StopTaskAsync(TaskCallbackRequest task, CancellationToken cancellationToken = default)
        {
            return await _service.StopTaskAsync(task, cancellationToken);
        }
        /// <summary>
        /// Возвращает задание планировщика
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScheduleTask> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _service.GetAsync(id, cancellationToken);
        }
        /// <summary>
        /// Удаляет задание планировщика
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task  DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _service.DeleteAsync(id, cancellationToken);
        }

        [HttpPost("run")]
        public async Task<TaskState> RunScheduleTaskAsync(RunTaskRequest runTaskRequest, CancellationToken cancellationToken = default)
        {
            return await _service.RunScheduleTaskAsync(runTaskRequest, cancellationToken);
        }
    }
}
