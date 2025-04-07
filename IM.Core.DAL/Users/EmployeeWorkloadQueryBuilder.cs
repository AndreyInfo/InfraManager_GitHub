using System;
using System.Linq;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Users;

internal class EmployeeWorkloadQueryBuilder : IEmployeeWorkloadQueryBuilder, ISelfRegisteredService<IEmployeeWorkloadQueryBuilder>
{
    private static readonly OperationID[] Operations =
    {
        OperationID.SD_General_Executor,
        OperationID.SD_General_Administrator,
    };

    private readonly DbContext _db;

    public EmployeeWorkloadQueryBuilder(CrossPlatformDbContext db)
    {
        _db = db;
    }

    public IExecutableQuery<EmployeeWorkloadQueryResultItem> BuildQuery(DateTime utcDateTime, DateTime utcLastActivityTime)
    {
        var resultQuery =
            from user in _db.Set<User>().AsNoTracking()
            join userRole in _db.Set<UserRole>().Include(u => u.Role).ThenInclude(r => r.Operations).AsNoTracking()
                on user.IMObjID equals userRole.UserID
            where !user.Removed && !User.SystemUserIds.Contains(user.ID)
                && userRole.Role.Operations.Any(o => Operations.Contains(o.OperationID))
                  && userRole.RoleID != Role.AdminRoleId

            let callCount = _db.Set<Call>().AsNoTracking()
                .Count(c => !c.Removed
                            && c.WorkflowSchemeID.HasValue
                            && (c.ExecutorID == user.IMObjID || c.OwnerID == user.IMObjID)
                            && ((!c.UtcDateClosed.HasValue || c.UtcDateClosed.Value > utcDateTime)
                                || (!c.UtcDateAccomplished.HasValue || c.UtcDateAccomplished.Value > utcDateTime)))

            let workOrderCount = _db.Set<WorkOrder>().AsNoTracking()
                .Count(wo => wo.WorkflowSchemeID.HasValue
                             && wo.ExecutorID == user.IMObjID
                             && (!wo.UtcDateAccomplished.HasValue || wo.UtcDateAccomplished.Value > utcDateTime))
            
            let sessionCount = _db.Set<Session>().AsNoTracking()
                .Count(session => session.UserID == user.IMObjID
                            && session.UtcDateClosed == null
                            && session.UtcDateLastActivity >= utcLastActivityTime)

            select new EmployeeWorkloadQueryResultItem
            {
                ID = user.IMObjID,
                FullName = User.GetFullName(user.IMObjID),
                IsOnWorkplace = sessionCount > 0,
                CallCount = callCount, 
                WorkOrderCount = workOrderCount, 
            };

        return new ExecutableQuery<EmployeeWorkloadQueryResultItem>(resultQuery);
    }
}