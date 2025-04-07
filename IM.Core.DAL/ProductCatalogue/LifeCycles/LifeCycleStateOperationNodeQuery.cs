using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;
internal class LifeCycleStateOperationNodeQuery : ILifeCycleNodeQuery
{
    private readonly DbSet<LifeCycleStateOperation> _lifeCycleStateOperations;

    public LifeCycleStateOperationNodeQuery(DbSet<LifeCycleStateOperation> lifeCycleStateOperations)
    {
        _lifeCycleStateOperations = lifeCycleStateOperations;
    }

    public async Task<LifeCycleTreeNode[]> ExecuteAsync(Guid? parentID, Guid? roleID, CancellationToken cancellationToken)
    {
        return await _lifeCycleStateOperations.AsNoTracking()
            .Where(c => c.LifeCycleStateID == parentID)
            .Select(c => new LifeCycleTreeNode()
            {
                ID = c.ID,
                Name = c.Name,
                ParentID = c.LifeCycleStateID,
                ClassID = ObjectClass.Unknown,
                HasChild = false,
                FullSelect = roleID.HasValue
                             && c.RoleLifeCycleStateOperations.All(x => x.RoleID == roleID),
                PartSelect = roleID.HasValue
                             && c.RoleLifeCycleStateOperations.Any(x => x.RoleID == roleID),
            }).ToArrayAsync(cancellationToken);
    }
}
