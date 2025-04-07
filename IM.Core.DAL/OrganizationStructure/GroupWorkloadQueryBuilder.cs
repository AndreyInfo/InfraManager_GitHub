using System;
using System.Linq;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.OrganizationStructure;

internal class GroupWorkloadQueryBuilder : IGroupWorkloadQueryBuilder, ISelfRegisteredService<IGroupWorkloadQueryBuilder>
{
    private readonly DbContext _db;

    public GroupWorkloadQueryBuilder(CrossPlatformDbContext db)
    {
        _db = db;
    }

    public IExecutableQuery<GroupWorkloadListQueryResultItem> BuildQuery(DateTime utcDateTime, Guid currentUserId, GroupType type)
    {
        var resultQuery =
            from @group in _db.Set<Group>().Include(g => g.QueueUsers).AsNoTracking()
            where @group.IMObjID != Guid.Empty
                  && ((byte) @group.Type & (byte) type) == (byte) type
                  && @group.QueueUsers.Any(gu =>
                      DbFunctions.AccessIsGranted(ObjectClass.User, gu.UserID, currentUserId, ObjectClass.User, AccessManagement.AccessTypes.TOZ_sks, false)
                      || DbFunctions.AccessIsGranted(ObjectClass.User, gu.UserID, currentUserId, ObjectClass.User, AccessManagement.AccessTypes.TOZ_org, false))

            let callCount = _db.Set<Call>().AsNoTracking()
                .Count(c => !c.Removed
                            && c.WorkflowSchemeID.HasValue
                            && c.QueueID == @group.IMObjID
                            && ((!c.UtcDateClosed.HasValue || c.UtcDateClosed.Value > utcDateTime)
                                || (!c.UtcDateAccomplished.HasValue || c.UtcDateAccomplished.Value > utcDateTime)))

            let workOrderCount = _db.Set<WorkOrder>().AsNoTracking()
                .Count(wo => wo.WorkflowSchemeID.HasValue
                             && wo.QueueID == @group.IMObjID
                             && (!wo.UtcDateAccomplished.HasValue || wo.UtcDateAccomplished.Value > utcDateTime))

            select new GroupWorkloadListQueryResultItem
            {
                ID = @group.IMObjID,
                FullName = @group.Name,
                Size = callCount + workOrderCount,
            };
        
        return new ExecutableQuery<GroupWorkloadListQueryResultItem>(resultQuery);
    }
}