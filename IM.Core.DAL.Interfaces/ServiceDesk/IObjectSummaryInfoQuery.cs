using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    public interface IObjectSummaryInfoQuery<TEntity> where TEntity : class
    {
        Task<ObjectSummaryInfo> ExecuteAsync(
            Guid objectID, 
            Guid userID,
            CancellationToken cancellationToken = default);
    }
}
