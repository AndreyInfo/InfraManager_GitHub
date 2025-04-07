using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface ILookupQuery
    {
        Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
