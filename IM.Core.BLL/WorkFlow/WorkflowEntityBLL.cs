using InfraManager.DAL;
using InfraManager.DAL.WorkFlow;
using we = InfraManager.Services.WorkflowService;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using InfraManager.ServiceBase.WorkflowService;

namespace InfraManager.BLL.Workflow
{
    /// <summary>
    /// TODO: Перенести функционал этого сервиса в тот, который обобщенный
    /// </summary>
    internal class WorkflowEntityBLL : IWorkflowEntityBLL, ISelfRegisteredService<IWorkflowEntityBLL>
    {
        private readonly IWorkflowServiceApi _api;
        private readonly IServiceMapper<ObjectClass, IFindWorkflowEntity> _finders;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<WorkflowEntityBLL> _logger;

        public WorkflowEntityBLL(
            IWorkflowServiceApi api,
            IServiceMapper<ObjectClass, IFindWorkflowEntity> finders,
            IUnitOfWork saveChangesCommand,
            ICurrentUser currentUser,
            ILogger<WorkflowEntityBLL> logger)
        {
            _api = api;
            _finders = finders;
            _saveChangesCommand = saveChangesCommand;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<WorkflowDetails> DetailsAsync(InframanagerObject workflowEntity, CancellationToken cancellationToken = default)
        {
            if (!_finders.HasKey(workflowEntity.ClassId))
            {
                throw new ObjectNotFoundException($"Objects (ClassID = {workflowEntity.ClassId}) do not support workflow.");
            }

            var finder = _finders.Map(workflowEntity.ClassId);
            var entity = await finder.FindAsync(workflowEntity.Id, cancellationToken);

            if (entity == null)
            {
                throw new ObjectNotFoundException<Guid>(workflowEntity.Id, workflowEntity.ClassId);
            }

            var hasExternalEvents = await _api.HasExternalEventsAsync(workflowEntity.Id, cancellationToken);
            var isReadonly = await _api.WorkflowIsReadonlyAsync(workflowEntity.Id, (int)workflowEntity.ClassId, _currentUser.UserId, cancellationToken);

            var transitions = isReadonly 
                ? Array.Empty<we.TransitionInfo>()
                : await _api.GetTransitionsAsync(workflowEntity.Id, (int)workflowEntity.ClassId, _currentUser.UserId, cancellationToken);
            var states = isReadonly
                ? Array.Empty<WorkflowStateModel>()
                : await _api.GetStatesAsync(workflowEntity.Id, cancellationToken);            
            
            
            var data = new WorkflowDetails
            {
                HasExternalEvents = hasExternalEvents,
                EntityStateName = entity.EntityStateName,
                ReadOnly = isReadonly,
                States = states
                    .Where(s => transitions.Any(t => t.TargetStateID == s.ID))
                    .Select(
                        s => new WorkflowStateDetails
                        {
                            StateID = s.ID,
                            Text = !string.IsNullOrWhiteSpace(transitions.FirstOrDefault(t => t.TargetStateID == s.ID)?.Action) 
                                ? transitions.FirstOrDefault(t => t.TargetStateID == s.ID).Action
                                : s.Name,
                            ImageBase64 = s.ImageBase64String
                        })
                    .ToArray()
            };
            return data;
        }

        public async Task<TransitionIsAllowedResult> TransitionIsAllowedAsync(Guid entityID, ObjectClass classID,
            string entityState, CancellationToken cancellationToken)
        => await _api.TransitionIsAllowedAsync(entityID, (int)classID, entityState, _currentUser.UserId, cancellationToken);

        public async Task EnqueueSetStateAsync(WorkflowEntityData data, CancellationToken cancellationToken = default)
        {
            var transitionValidationResult = await TransitionIsAllowedAsync(data.Id, data.ClassId, data.EntityState, cancellationToken);
            if (!transitionValidationResult.IsAllowed)
            {
                throw new InvalidObjectException(transitionValidationResult.Message);
            }

            var finder = _finders.Map(data.ClassId);
            var entity = await finder.FindAsync(data.Id, cancellationToken);

            if (entity.EntityStateID != data.EntityState)
            {
                entity.UtcDateModified = DateTime.UtcNow;
                entity.TargetEntityStateID = data.EntityState;

                await _saveChangesCommand.SaveAsync(cancellationToken);
            }
        }
    }

    internal class WorkflowEntityBLL<TEntity> : ICreateWorkflow<TEntity>
        where TEntity : class, IWorkflowEntity
    {
        private readonly IWorkflowServiceApi _api;
        private readonly IInsertWorkflowRequestCommand _insertRequestCommand;
        private readonly IDeleteWorkflowRequestCommand _deleteRequestCommand;
        private readonly ISelectWorkflowScheme<TEntity> _schemeProvider;

        public WorkflowEntityBLL(
            IWorkflowServiceApi api,
            IInsertWorkflowRequestCommand insertRequestCommand,
            IDeleteWorkflowRequestCommand deleteRequestCommand,
            ISelectWorkflowScheme<TEntity> schemeProvider)
        {
            _api = api;
            _insertRequestCommand = insertRequestCommand;
            _deleteRequestCommand = deleteRequestCommand;
            _schemeProvider = schemeProvider;
        }

        public async Task<bool> TryStartNewAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.WorkflowSchemeIdentifier = await _schemeProvider.SelectIdentifierAsync(entity, cancellationToken);

            if (string.IsNullOrWhiteSpace(entity.WorkflowSchemeIdentifier)) // Для объекта не найдена (не назначена) схема рабочей процедуры
            {
                return false; 
            }

            using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.RequiresNew))
            {
                await _insertRequestCommand.ExecuteAsync(entity.IMObjID); // TODO: Выпилить эту кривую блокировку

                transaction.Complete();
            }

            try
            {
                var response = await _api.CreateWorkflowAsync(entity.IMObjID, entity.WorkflowSchemeIdentifier, cancellationToken);
                entity.WorkflowSchemeVersion = response.Version;
                entity.WorkflowSchemeID = response.ID;

                var initialStates = await _api.GetInitialStatesAsync(entity.IMObjID); // после того, как воркфло уже создан сторонним сервисом, прерывать его уже нельзя
                entity.TargetEntityStateID = initialStates.FirstOrDefault()?.ID;

                return true;
            }
            finally
            {
                await _deleteRequestCommand.ExecuteAsync(entity.IMObjID); //Самое слабое место этой кривой блокировки (если мы не сможем удалить из-за дедлока например, то РП зависнет)
            }
        }
    }
}
