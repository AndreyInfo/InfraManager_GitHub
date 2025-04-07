using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Operations;

internal class LifeCycleListOperationQuery : ILifeCycleListOperationQuery,
    ISelfRegisteredService<ILifeCycleListOperationQuery>
{
    private readonly DbContext _context;

    public LifeCycleListOperationQuery(CrossPlatformDbContext context)
    {
        _context = context;
    }

    public async Task<GroupedLifeCycleListItem[]> ExecuteAsync(Guid[] rolesID, Guid? lifeCycleStateID, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<LifeCycle>()
            .AsNoTracking()
            .Include(x => x.LifeCycleStates)
            .ThenInclude(x => x.LifeCycleStateOperations)
            .AsQueryable();

        if (lifeCycleStateID is not null)
            query = query.Where(x => x.LifeCycleStates
                .Any(state => state.ID == lifeCycleStateID.Value && state.LifeCycle.Type == x.Type));

        return await query.Select(x => new GroupedLifeCycleListItem
        {
            Name = x.Name,
            States = x.LifeCycleStates.Where(x => lifeCycleStateID.HasValue && x.ID == lifeCycleStateID).Select(x =>
                new LifeCycleStateListItem
                {
                    Name = x.Name,
                    ID = x.ID,
                    Operations = x.LifeCycleStateOperations.Select(x =>
                        new LifeCycleStateOperationListItem
                        {
                            Name = x.Name,
                            ID = x.ID,
                            CommandTypeID = (int)x.CommandType,
                            IsSelected = _context.Set<RoleLifeCycleStateOperation>().Any(q =>
                                rolesID.Contains(q.RoleID) && q.LifeCycleStateOperationID == x.ID)
                        }).ToArray()
                }).ToArray()
        }).ToArrayAsync(cancellationToken);
    }
}