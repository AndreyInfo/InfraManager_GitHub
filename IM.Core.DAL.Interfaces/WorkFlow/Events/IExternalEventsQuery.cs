using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.WorkFlow.Events;

public interface IExternalEventsQuery
{
    Task<BaseEventItem[]> QueryAsync(CancellationToken cancellationToken = default);
}