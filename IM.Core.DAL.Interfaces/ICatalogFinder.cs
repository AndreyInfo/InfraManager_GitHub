using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface ICatalogFinder<TCatalog, TKey>
        where TKey : struct
        where TCatalog : class, ICatalog<TKey>
    {
        Task<TCatalog> FindAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
