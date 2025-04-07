using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IObjectSummaryBLL
    {
        Task<ObjectSummaryInfo> GetObjectSummaryAsync(Guid objectID, CancellationToken cancellationToken = default);
    }
}
