using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL
{
    public interface IObjectSearcher
    {
        Task<ObjectSearchResult[]> SearchAsync(string queryString, CancellationToken cancellationToken = default);
    }
}
