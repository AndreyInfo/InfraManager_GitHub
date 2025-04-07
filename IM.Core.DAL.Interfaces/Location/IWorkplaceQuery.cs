using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location
{
    public interface IWorkplaceQuery
    {
        Task<WorkplaceItem> QueryAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
