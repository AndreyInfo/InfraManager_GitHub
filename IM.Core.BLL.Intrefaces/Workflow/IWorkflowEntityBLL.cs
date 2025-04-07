using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.ServiceBase.WorkflowService;

namespace InfraManager.BLL.Workflow
{
    public interface IWorkflowEntityBLL
    {
        Task<WorkflowDetails> DetailsAsync(
            InframanagerObject objectID, 
            CancellationToken cancellationToken = default);

        Task EnqueueSetStateAsync(
            WorkflowEntityData data, 
            CancellationToken cancellationToken = default);

        Task<TransitionIsAllowedResult> TransitionIsAllowedAsync(Guid entityID, ObjectClass classID, string entityState, CancellationToken cancellationToken = default);
    }
}
