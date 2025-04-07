using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface IPagingQuery<T>
    {
        Task<T[]> PageAsync(int skip, int take, CancellationToken cancellationToken = default);
    }
}
