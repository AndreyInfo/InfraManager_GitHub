using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Operations;

internal class OperationsGetQuery : IOperationGetQuery
    , ISelfRegisteredService<IOperationGetQuery>
{
    private readonly DbSet<Operation> _operations;

    public OperationsGetQuery(DbSet<Operation> operations)
    {
        _operations = operations;
    }
    
    
    public async Task<GroupedOperationListItem[]> ExecuteAsync(Guid roleID, bool onlySelectedForRole,
        CancellationToken cancellationToken = default)
    {
        var operationsQuery = _operations.Include(x => x.Class)
            .Include(x => x.RoleOperations)
            .AsNoTracking();

        if (onlySelectedForRole)
        {
            operationsQuery = operationsQuery.Where(x => x.RoleOperations.Any(c => c.RoleID == roleID));
        }
        
        return (await operationsQuery.ToArrayAsync(cancellationToken)).GroupBy(x => x.Class.Name).Select(x =>
            new GroupedOperationListItem
            {
                Name = x.Key,
                Operations = x.Select(x => new OperationListItem
                {
                    Description = x.Description,
                    Name = x.OperationName,
                    ObjectID = x.ID,
                    ObjectName = x.Class.Name,
                    ID = x.ID,
                    IsSelect = x.RoleOperations.Any(x => x.RoleID == roleID)
                })
            }).ToArray();
    }
}