using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;

internal sealed class LifeCycleNodeQuery : ILifeCycleNodeQuery
{
    private readonly DbSet<LifeCycle> _lifeCycles;

    public LifeCycleNodeQuery(DbSet<LifeCycle> lifeCycles)
    {
        _lifeCycles = lifeCycles;
    }

    public Task<LifeCycleTreeNode[]> ExecuteAsync(Guid? parentID, Guid? roleID, CancellationToken cancellationToken)
    {
        return _lifeCycles.AsNoTracking()
            .Select(c => new LifeCycleTreeNode()
            {
                ID = c.ID,
                Name = c.Name,
                ParentID = null,
                ClassID = ObjectClass.LifeCycle,
                HasChild = c.LifeCycleStates.Any(),
                FullSelect = roleID.HasValue
                             && c.LifeCycleStates.All(
                                 v => v.LifeCycleStateOperations.All(
                                     d => d.RoleLifeCycleStateOperations.All(
                                         x => x.RoleID == roleID))),
                PartSelect = roleID.HasValue
                             && c.LifeCycleStates.Any(
                                 v => v.LifeCycleStateOperations.Any(
                                     d => d.RoleLifeCycleStateOperations.Any(
                                         x => x.RoleID == roleID)))
            }).ToArrayAsync(cancellationToken);
    }
}
