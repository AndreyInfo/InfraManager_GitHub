using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Operations;

namespace InfraManager.BLL.AccessManagement.Operations;

public interface IOperationsBLL
{
    Task<GroupedOperationListItem[]> GetOperationsListAsync(OperationFilter filter,
        CancellationToken cancellationToken = default);

    Task UpdateOperationsRolesAsync(Guid roleID, OperationData[] data, CancellationToken cancellationToken);
}
