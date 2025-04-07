using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk;

internal class GroupExecutorListQuery<TEntity> : IExecutorListQuery<GroupExecutorListQueryResultItem, Group>
    where TEntity : class
{
    private readonly IReadOnlyDictionary<Type, GroupType> _entityTypeToGroupTypeMap =
        new Dictionary<Type, GroupType>
        {
            { typeof(Call), GroupType.Call },
            { typeof(WorkOrder), GroupType.WorkOrder },
            { typeof(MassIncident), GroupType.MassiveIncident },
            { typeof(Problem), GroupType.Problem },
            { typeof(ChangeRequest), GroupType.ChangeRequest },
        };

    private readonly DbContext _db;
    private readonly IBuildAvailableToExecutorViaToz<Group, TEntity> _availableViaToz;
    private readonly IBuildAvailableToExecutorViaTtz<Group, TEntity> _availableViaTtz;
    private readonly IBuildAvailableToExecutorViaSupportLine<Group, TEntity> _availableViaSupportLine;

    public GroupExecutorListQuery(
        CrossPlatformDbContext db,
        IBuildAvailableToExecutorViaToz<Group, TEntity> availableViaToz,
        IBuildAvailableToExecutorViaTtz<Group, TEntity> availableViaTtz,
        IBuildAvailableToExecutorViaSupportLine<Group, TEntity> availableViaSupportLine)
    {
        _db = db;
        _availableViaToz = availableViaToz;
        _availableViaTtz = availableViaTtz;
        _availableViaSupportLine = availableViaSupportLine;
    }

    public async Task<GroupExecutorListQueryResultItem[]> ExecuteAsync(ExecutorListQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var groupType = _entityTypeToGroupTypeMap[typeof(TEntity)];

        var query = _db.Set<Group>()
            .Include(x => x.QueueUsers)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.Workplace)
            .AsNoTracking()
            .Where(q => q.IMObjID != Guid.Empty && (q.Type & groupType) == groupType);

        if (filter.QueueIDs is not null && filter.QueueIDs.Any())
        {
            query = query.Where(q => filter.QueueIDs.Contains(q.IMObjID));
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
            .Select(queue => new GroupExecutorListQueryResultItem
            {
                ID = queue.IMObjID,
                Name = queue.Name,
                ResponsibleName = User.GetFullName(queue.ResponsibleUser.IMObjID),
                Note = queue.Note,
                Type = queue.Type,
                ResponsibleUserID = queue.ResponsibleUser.IMObjID,
                QueueUserList = queue.QueueUsers.Select(x => x.User),
            })
            .ToArrayAsync(cancellationToken);
    }

    private async Task<TEntity> FindEntityAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await _db.Set<TEntity>().FindAsync(new object[] { entityID, }, cancellationToken);
    }
}