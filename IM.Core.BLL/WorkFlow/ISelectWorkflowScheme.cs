using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Workflow
{
    internal interface ISelectWorkflowScheme<T>
    {
        Task<string> SelectIdentifierAsync(T data, CancellationToken cancellationToken = default);
    }
}
