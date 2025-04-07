using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.TableFilters
{
    public interface IAvailableTableFilterElementsBLL
    {
        Task<FilterElementBase[]> GetAllAsync(string view, CancellationToken cancellationToken = default);
        Task<FilterElementBase> GetByPropertyNameAsync(string view, string propertyName, CancellationToken cancellationToken = default);
    }
}
