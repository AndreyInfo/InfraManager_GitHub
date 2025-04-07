using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.DAL.WorkFlow
{
    public interface IWorkFlowShemeDataProvider
    {
        Task<bool> IsExistByIdentifierAsync(string identifier);
        Task<WorkFlowScheme> GetActualVersionByIdentifierAsync(string identifier, CancellationToken cancellationToken);
    }
}
