using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;
internal class LifeCycleStateNodeQuery : ILifeCycleNodeQuery
{
    private readonly DbSet<LifeCycleState> _lifeCycleStates;

    public LifeCycleStateNodeQuery(DbSet<LifeCycleState> lifeCycleStates)
    {
        _lifeCycleStates = lifeCycleStates;
    }

    public async Task<LifeCycleTreeNode[]> ExecuteAsync(Guid? parentID, Guid? roleID, CancellationToken cancellationToken)
    {
        return await _lifeCycleStates.AsNoTracking()
            .Where(c=> c.LifeCycleID == parentID)
            .Select(c => new LifeCycleTreeNode()
            {
                ID = c.ID,
                Name = c.Name,
                ParentID = c.LifeCycleID,
                ClassID = ObjectClass.LifeCycleState,
                HasChild = c.LifeCycleStateOperations.Any(),
                FullSelect = roleID.HasValue
                             && c.LifeCycleStateOperations.All(
                                 d => d.RoleLifeCycleStateOperations.All(
                                     x => x.RoleID == roleID)),
                PartSelect = roleID.HasValue
                             && c.LifeCycleStateOperations.Any(
                                 d => d.RoleLifeCycleStateOperations.Any(
                                     x => x.RoleID == roleID))
            }).ToArrayAsync(cancellationToken);
    }
}