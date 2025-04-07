using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Operations;

public interface IOperationGetQuery
{
    Task<GroupedOperationListItem[]> ExecuteAsync(Guid roleID, bool onlySelectedForRole,
        CancellationToken cancellationToken = default);
}