using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.WorkFlow;

public interface IRemoveWorkflowByRemovedObjectsCommand
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}