using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface IAnyQuery
    {
        Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
