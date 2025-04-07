using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Workflow
{
    internal class WorkflowEntityFinder<T> : IFindWorkflowEntity
        where T : class, IWorkflowEntity
    {
        private readonly IFindEntityByGlobalIdentifier<T> _finder;

        public WorkflowEntityFinder(IFindEntityByGlobalIdentifier<T> finder)
        {
            _finder = finder;
        }

        public async Task<IWorkflowEntity> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _finder.FindAsync(id, cancellationToken);
        }
    }
}
