using System.Threading.Tasks;
using System.Threading;
using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    public interface IServiceItemAttendanceQuery
    {
        Task<ServiceItemAttendanceItem[]> GetItemAttendacesAsync(Guid userId, CancellationToken cancellationToken);
    }
}
