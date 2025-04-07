using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public interface IQueryCallClientLocationInfo
    {
        Task<CallClientLocationInfoItem> GetCallClientLocationInfoAsync(Guid locationId, CancellationToken cancallationToken);
    }
}
