using System.Transactions;
using AutoMapper;
using IM.Core.ScheduleBLL.Interfaces;
using Inframanager;
using Inframanager.BLL;
using InfraManager;
using InfraManager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services;
using InfraManager.Services.ScheduleService;
using Microsoft.Extensions.Logging;
using InfraManager.ResourcesArea;

namespace IM.Core.Schedule.BLL
{
    public class ScheduleBLL : IScheduleBLL, ISelfRegisteredService<IScheduleBLL>
    {
        private readonly ILogger<ScheduleBLL> _logger;
        private readonly IRepository<ScheduleTaskEntity> _repository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IMapper _mapper;
        private readonly IFinder<ScheduleTaskEntity> _finder;
        private readonly IValidatePermissions<ScheduleTaskEntity> _validatePermissions;
        private readonly ICurrentUser _currentUser;
        private readonly IPagingQueryCreator _paging;
        private readonly IJobExecutor _executor;
        private readonly IScheduleCalculator _scheduleCalculator;
        private readonly IAfterExecuteJobProcessor _afterExecuteJobProcessor;

        public ScheduleBLL(ILogger<ScheduleBLL> logger,
            IRepository<ScheduleTaskEntity> repository,
            IUnitOfWork saveChangesCommand,
            IMapper mapper,
            IFinder<ScheduleTaskEntity> finder,
            IValidatePermissions<ScheduleTaskEntity> validatePermissions,
            IPagingQueryCreator paging,
            ICurrentUser currentUser,
            IJobExecutor executor,
            IScheduleCalculator scheduleCalculator,
            IAfterExecuteJobProcessor afterExecuteJobProcessor)
        {
            _logger = logger;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _mapper = mapper;
            _finder = finder;
            _validatePermissions = validatePermissions;
            _paging = paging;
            _currentUser = currentUser;
            _executor = executor;
            _scheduleCalculator = scheduleCalculator;
            _afterExecuteJobProcessor = afterExecuteJobProcessor;
        }

        public async Task<ScheduleTask[]> GetListAsync(ScheduleFilterRequest filter,
            CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
                ObjectAction.ViewDetailsArray, cancellationToken);

            var query = _repository.With(x => x.Schedules).DisableTrackingForQuery().Query();

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(p => p.Name.ToLower().Contains(filter.SearchString.ToLower()));
            }

            var paging = _paging.Create(query.OrderBy(x => x.Name));

            var tasks = await paging.PageAsync(filter.StartRecordIndex, filter.CountRecords, cancellationToken);

            return _mapper.Map<ScheduleTask[]>(tasks);
        }

        public async Task<ScheduleTask> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
                ObjectAction.ViewDetails, cancellationToken);

            var taskEntity = await _finder.With(x => x.Schedules).FindAsync(id, cancellationToken) ??
                             throw new ObjectNotFoundException<Guid>(id, nameof(ScheduleTask));
            
            var task = _mapper.Map<ScheduleTask>(taskEntity);

            task.Schedules = task.Schedules?.Where(p => p.ScheduleType != ScheduleType.Immediately).ToArray();

            return task;
        }

        //TODO ScheduleTask -> ScheduleTaskData
        public async Task<Guid> AddAsync(ScheduleTask task, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
                ObjectAction.Insert, cancellationToken);
            
            var entity = _mapper.Map<ScheduleTaskEntity>(task);

            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {
                _repository.Insert(entity);
                task.TaskState = TaskState.Inactive;
                await _saveChangesCommand.SaveAsync(cancellationToken);
                
                SetNewSchedule(entity);
                
                await _saveChangesCommand.SaveAsync(cancellationToken);
                transaction.Complete();
            }

            _logger.LogInformation("Задача {EntityID} сохранена", entity.ID);
            return entity.ID;
        }

       
        public async Task<OperationResult> UpdateAsync(ScheduleTask task, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
                ObjectAction.Update, cancellationToken);

            var foundEntity = await _finder.FindAsync(task.ID, cancellationToken)
                              ?? throw new ObjectNotFoundException<Guid>(task.ID, nameof(ScheduleTask));

            using (var transaction =
                   TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {     
                _mapper.Map(task, foundEntity);
                await _saveChangesCommand.SaveAsync(cancellationToken);
                
                SetNewSchedule(foundEntity);
                await _saveChangesCommand.SaveAsync(cancellationToken);
                
                transaction.Complete();
            }

            return OperationResult.Success;
        }

        private void SetNewSchedule(ScheduleTaskEntity job)
        {
            var currentSchedule = _scheduleCalculator.CalculateNextSchedule(job);
            if (currentSchedule != null)
            {
                job.CurrentExecutingScheduleID = currentSchedule.ID;
                job.NextRunAt = currentSchedule.NextAt;
                job.TaskState = TaskStateEnum.Waiting;
            }
        }

        public async Task<bool> StopTaskAsync(TaskCallbackRequest stopJobRequest,
            CancellationToken cancellationToken = default)
        {
            var task = await _repository.With(x => x.CurrentSchedule).WithMany(x => x.Schedules)
                .FirstOrDefaultAsync(x => x.ID == stopJobRequest.ID, cancellationToken);

            if (task != null)
            {
                _afterExecuteJobProcessor.ProcessJobAfterExecute(task);
                await _saveChangesCommand.SaveAsync(cancellationToken);
            }

            return task != null;
        }
        
        //TODO если не получилось удалить статус ответа != 200
        public async Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(
                _logger,
                _currentUser.UserId,
                ObjectAction.Delete,
                cancellationToken);

            var foundEntity = await _finder.FindAsync(id, cancellationToken) ??
                              throw new ObjectNotFoundException<Guid>(id, nameof(ScheduleTask));

            if (foundEntity.TaskState == TaskStateEnum.Running)
            {
                throw new SchedulerApiException(Resources.CantDeleteTaskImport);
            }
            
            _repository.Delete(foundEntity);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<bool> DeleteTasksByUISettingIDAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(
                _logger,
                _currentUser.UserId,
                ObjectAction.Delete,
                cancellationToken);
            
            var tasks = await _repository.ToArrayAsync(p => p.TaskSettingID == id, cancellationToken);
            foreach (var task in tasks)
            {
                if (task.TaskState != TaskStateEnum.Running)
                {
                    _repository.Delete(task);
                }
                else
                {
                    return false;
                }
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return true;
        }

        public async Task<TaskState> RunTaskAsync(RunTaskRequest runTaskRequest,
            CancellationToken cancellationToken = default)
        {
            var task = await _repository.FirstOrDefaultAsync(x => x.ID == runTaskRequest.TaskID,
                           cancellationToken) ??
                       throw new ObjectNotFoundException<Guid>(runTaskRequest.TaskID, nameof(ScheduleTask));
            
            task.CredentialID = runTaskRequest.CredentialID;
            
            try
            {
                await _executor.ExecuteAsync(task, cancellationToken);
                return TaskState.Running;
            }
            catch
            {
                return TaskState.Error;
            }
        }
    }
}
