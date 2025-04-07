using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location
{
    public interface IRoomQuery
    {
        Task<RoomItem> QueryAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
