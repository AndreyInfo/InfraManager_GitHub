using Inframanager.BLL;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.WorkFlow
{
    internal class WorkflowEntityStateValidator<TEntity> : IValidateObjectState<TEntity>
        where TEntity : IWorkflowEntity
    {
        private readonly IWorkflowServiceApi _api;

        public WorkflowEntityStateValidator(IWorkflowServiceApi api)
        {
            _api = api;
        }

        public async Task<bool> IsReadOnlyAsync(Guid userID, TEntity entity, CancellationToken cancellationToken = default)
        {        
            return !string.IsNullOrWhiteSpace(entity.WorkflowSchemeIdentifier)
                && await _api.WorkflowIsReadonlyAsync(
                entity.IMObjID,
                (int)typeof(TEntity).GetObjectClassOrRaiseError(),
                userID,
                cancellationToken);
        }
    }
}
