using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.TableFilters
{
    public interface ITableFilterOperationsBLL
    {
        Task<LookupListItem<byte>[]> GetByElementTypeAsync(
            FilterTypes type, 
            CancellationToken cancellationToken = default);
    }
}
