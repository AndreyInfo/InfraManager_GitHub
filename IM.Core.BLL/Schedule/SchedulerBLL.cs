using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using IM.Import.Services.Logger;
using InfraManager.BLL.Extensions;
using InfraManager.BLL.Scheduler;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services;
using InfraManager.Services.ScheduleService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Import;

namespace InfraManager.BLL.Schedule
{
    //TODO выпилить этот класс из веба
    public class ScheduleBLL : ISchedulerBLL, ISelfRegisteredService<ISchedulerBLL>
    {
        private readonly IScheduleServiceWebApi _api;
        private readonly IMapper _mapper;
        private readonly IImportLogger _logger;
        private readonly IFinder<UISetting> _settingFinder;
        private readonly ICurrentUser _currentUser;
        
        public ScheduleBLL(
            IScheduleServiceWebApi api,
            IMapper mapper,
            IImportLogger logger,
            IFinder<UISetting> settingFinder,
            
            ICurrentUser currentUser)
        {
            _api = api;
            _mapper = mapper;
            _logger = logger;
            _settingFinder = settingFinder;
            _currentUser = currentUser;
        }

        public async Task<SchedulerListDetail[]> GetScheduleTasksAsync(ScheduleFilterRequest filter,
            CancellationToken cancellationToken = default)
        {
            var scheduleTasks = await _api.GetScheduleTasksAsync(filter, _currentUser.UserId, cancellationToken);

            return _mapper.Map<SchedulerListDetail[]>(scheduleTasks);
        }

        public async Task<SchedulerProtocolsDetail[]> GetScheduleProtocolsAsync(ScheduleFilterRequest filter,
            CancellationToken cancellationToken = default)
        {
            var scheduleTasks = await _api.GetScheduleTasksAsync(filter, _currentUser.UserId, cancellationToken);
           
            return _mapper.Map<SchedulerProtocolsDetail[]>(scheduleTasks);
        }

        public async Task<Guid> AddScheduleTaskAsync(ScheduleTask task, CancellationToken cancellationToken = default)
        {
            return await _api.AddScheduleTaskAsync(task, _currentUser.UserId, cancellationToken);
        }

        public async Task<OperationResult> UpdateScheduleTaskAsync(ScheduleTask task, CancellationToken cancellationToken = default)
        {
            return await _api.UpdateScheduleTaskAsync(task, _currentUser.UserId, cancellationToken);
        }

        public async Task<bool> StopTaskAsync(TaskCallbackRequest task, CancellationToken cancellationToken = default)
        {
            return await _api.StopTaskAsync(task, _currentUser.UserId, cancellationToken);
        }
        
        public async Task<ScheduleTask> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _api.GetAsync(id, _currentUser.UserId, cancellationToken);
        }
        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _api.DeleteAsync(id, _currentUser.UserId, cancellationToken);
            }
            catch(Exception ex)
            {
                throw new SchedulerApiException(ex.Message);
            }
            
        }
        
        public async Task<TaskState> RunScheduleTaskAsync(RunTaskRequest runTaskRequest, CancellationToken cancellationToken = default)
        {
            return await _api.RunScheduleTaskAsync(runTaskRequest, _currentUser.UserId, cancellationToken);
        }
    }
}
