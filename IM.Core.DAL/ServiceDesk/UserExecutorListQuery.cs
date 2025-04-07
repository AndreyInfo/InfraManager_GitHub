using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk;

internal class UserExecutorListQuery<TEntity> : IExecutorListQuery<UserExecutorListQueryResultItem, User>
    where TEntity : class
{
    private readonly DbContext _db;
    private readonly IBuildAvailableToExecutorViaToz<User, TEntity> _availableViaToz;
    private readonly IBuildAvailableToExecutorViaTtz<User, TEntity> _availableViaTtz;
    private readonly IBuildAvailableToExecutorViaSupportLine<User, TEntity> _availableViaSupportLine;

    public UserExecutorListQuery(
        CrossPlatformDbContext db,
        IBuildAvailableToExecutorViaToz<User, TEntity> availableViaToz,
        IBuildAvailableToExecutorViaTtz<User, TEntity> availableViaTtz,
        IBuildAvailableToExecutorViaSupportLine<User, TEntity> availableViaSupportLine)
    {
        _db = db;
        _availableViaToz = availableViaToz;
        _availableViaTtz = availableViaTtz;
        _availableViaSupportLine = availableViaSupportLine;
    }

    public async Task<UserExecutorListQueryResultItem[]> ExecuteAsync(ExecutorListQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var shouldNotParticipateAutoAssign = new Specification<User>(user => !filter.ShouldParticipateAutoAssign);

        var query = _db.Set<User>().AsNoTracking()
            .Where(User.ExceptRemovedUsers && User.ExceptSystemUsers && User.HasNonAdminRole
                   && (User.HasOperation(OperationID.SD_General_Executor) || User.HasOperation(OperationID.SD_General_Administrator))
                   && (shouldNotParticipateAutoAssign || User.HasOperation(OperationID.SD_General_ParticipateInAutoAssign)));

        if (filter.UserIDs is not null && filter.UserIDs.Any())
        {
            query = query.Where(u => filter.UserIDs.Contains(u.IMObjID));
        }

        if (filter.QueueIDs != null && filter.QueueIDs.Any())
        {
            query = query.Where(u => _db.Set<GroupUser>()
                .Any(x => x.UserID == u.IMObjID && filter.QueueIDs.Contains(x.GroupID)));
        }

        if (filter.TTZEnabled)
        {
            query = query.Where(_availableViaTtz.Build(await FindEntityAsync(filter.ObjectID, cancellationToken)));
        }

        if (filter.TOZEnabled)
        {
            query = query.Where(_availableViaToz.Build(await FindEntityAsync(filter.ObjectID, cancellationToken)));
        }

        if (filter.ServiceResponsibilityEnabled)
        {
            query = query.Where(_availableViaSupportLine.Build(await FindEntityAsync(filter.ObjectID, cancellationToken)));
        }

        return await query
            .Select(x => new UserExecutorListQueryResultItem
            {
                IMObjID = x.IMObjID,
                Name = User.GetFullName(x.IMObjID),
                SubdivisionID = x.SubdivisionID,
                Details = x.Details,
            })
            .ToArrayAsync(cancellationToken);
    }

    private async Task<TEntity> FindEntityAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await _db.Set<TEntity>().FindAsync(new object[] { entityID, }, cancellationToken);
    }
}