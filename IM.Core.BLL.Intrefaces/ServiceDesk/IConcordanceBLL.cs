using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IConcordanceBLL
    {
        Task<ConcordanceModel[]> ListAsync(CancellationToken cancellationToken = default);
    }
}
