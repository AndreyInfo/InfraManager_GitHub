using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Workflow
{
    internal interface IFindWorkflowEntity
    {
        Task<IWorkflowEntity> FindAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
