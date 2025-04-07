using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Interface.ServiceDesk.ChangeRequests.RFCGantt;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests.RFCGantt;

internal class RFCGanttCrossDbQuery : IRFCGanttQuery, ISelfRegisteredService<IRFCGanttQuery>
{
    private readonly DbContext _db;

    public RFCGanttCrossDbQuery(
        CrossPlatformDbContext db)
    {
        _db = db;
    }

    public async Task<RFCGanttResultItem[]> ExecuteAsync(DateTime firstDay, DateTime secondDay, CancellationToken cancellationToken = default)
    {
        var subQuery1 = await
            (from rfc in _db.Set<ChangeRequest>().AsNoTracking()
             join priority in _db.Set<Priority>().AsNoTracking()
                 on rfc.PriorityID equals priority.ID

             join rfcCategory in _db.Set<ChangeRequestCategory>().AsNoTracking()
                 on rfc.CategoryID equals rfcCategory.ID
                 into rfcCategoryLeftJoin
             from rfcCategory in rfcCategoryLeftJoin.DefaultIfEmpty()

             join queue in _db.Set<Group>().AsNoTracking()
                 on rfc.QueueID equals queue.IMObjID
                 into queueLeftJoin
             from queue in queueLeftJoin.DefaultIfEmpty()

             join userOwner in _db.Set<User>().AsNoTracking()
                 on rfc.OwnerID equals userOwner.IMObjID
                 into userOwnerLeftJoin
             from userOwner in userOwnerLeftJoin.DefaultIfEmpty()
             join userOwnerPos in _db.Set<JobTitle>().AsNoTracking()
                 on userOwner.PositionID equals userOwnerPos.ID
                 into userOwnerPosLeftJoin
             from userOwnerPos in userOwnerPosLeftJoin.DefaultIfEmpty()

             join userInit in _db.Set<User>().AsNoTracking()
                 on rfc.InitiatorID equals userInit.IMObjID
                 into userInitLeftJoin
             from userInit in userInitLeftJoin.DefaultIfEmpty()
             join userInitPos in _db.Set<JobTitle>().AsNoTracking()
                 on userInit.PositionID equals userInitPos.ID
                 into userInitPosLeftJoin
             from userInitPos in userInitPosLeftJoin.DefaultIfEmpty()

             where rfc.UtcDateStarted != null
             select new RFCGanttResultItem()
             {
                 ID = rfc.IMObjID,
                 TypeClass = ObjectClass.ChangeRequest,
                 Number = rfc.Number,
                 Summary = rfc.Summary,
                 PriorityName = priority.Name,
                 PriorityColor = priority.Color,
                 UtcDateDetected = rfc.UtcDateDetected,
                 UtcDatePromised = rfc.UtcDatePromised,
                 UtcDateSolved = rfc.UtcDateSolved,
                 UtcDateClosed = rfc.UtcDateClosed,
                 UtcDateStarted = rfc.UtcDateStarted,
                 UtcDateModified = rfc.UtcDateModified,
                 OwnerFullName = User.GetFullName(rfc.OwnerID),
                 OwnerID = rfc.OwnerID,
                 Owner = new RFCGanttUserResultItem
                 {
                     ID = userOwner.IMObjID,
                     PositionName = userOwner.PositionName,
                     Phone = userOwner.Phone,
                     PhoneInternal = userOwner.Phone1,
                     SubdivisionFullName = Subdivision.GetFullSubdivisionName(userOwner.SubdivisionID),
                     Email = userOwner.Email,
                 },
                 QueueID = queue.IMObjID,
                 QueueName = queue.Name,
                 InitiatorID = rfc.InitiatorID,
                 Initiator = new RFCGanttUserResultItem
                 {
                     ID = userInit.IMObjID,
                     FullName = User.GetFullName(userInit.IMObjID),
                     PositionName = userInit.PositionName,
                     Phone = userInit.Phone,
                     PhoneInternal = userInit.Phone1,
                     SubdivisionFullName = Subdivision.GetFullSubdivisionName(userInit.SubdivisionID),
                     Email = userInit.Email,
                 },
                 EntityStateName = rfc.EntityStateName,
                 Description = rfc.Description,
                 Target = rfc.Target,
                 ServiceName = rfc.ServiceName,
                 ParentID = null,
                 CategoryName = rfcCategory.Name
             })
            .Where(f => (f.UtcDatePromised >= firstDay || f.UtcDatePromised == null)
                        && (f.UtcDateStarted >= secondDay || f.UtcDateStarted == null))
            .ToArrayAsync(cancellationToken);

        var subQuery2 = await
            (from r in _db.Set<WorkOrder>().AsNoTracking()
             join pr in _db.Set<WorkOrderPriority>().AsNoTracking()
                 on r.PriorityID equals pr.ID
             join wor in _db.Set<WorkOrderReference>().AsNoTracking()
                 on r.WorkOrderReferenceID equals wor.ID
             join rfc in _db.Set<ChangeRequest>().AsNoTracking()
                 on wor.ObjectID equals rfc.IMObjID

             join queue in _db.Set<Group>().AsNoTracking()
                 on rfc.QueueID equals queue.IMObjID
                 into queueLeftJoin
             from queue in queueLeftJoin.DefaultIfEmpty()

             join userOwner in _db.Set<User>().AsNoTracking()
                 on rfc.OwnerID equals userOwner.IMObjID
                 into userOwnerLeftJoin
             from userOwner in userOwnerLeftJoin.DefaultIfEmpty()
             join userOwnerPos in _db.Set<JobTitle>().AsNoTracking()
                 on userOwner.PositionID equals userOwnerPos.ID
                 into userOwnerPosLeftJoin
             from userOwnerPos in userOwnerPosLeftJoin.DefaultIfEmpty()

             join userInit in _db.Set<User>().AsNoTracking()
                 on rfc.InitiatorID equals userInit.IMObjID
                 into userInitLeftJoin
             from userInit in userInitLeftJoin.DefaultIfEmpty()
             join userInitPos in _db.Set<JobTitle>().AsNoTracking()
                 on userInit.PositionID equals userInitPos.ID
                 into userInitPosLeftJoin
             from userInitPos in userInitPosLeftJoin.DefaultIfEmpty()

             where rfc.UtcDateStarted != null
             select new RFCGanttResultItem()
             {
                 ID = r.IMObjID,
                 TypeClass = ObjectClass.WorkOrder,
                 Number = r.Number,
                 Summary = r.Name,
                 PriorityName = pr.Name,
                 PriorityColor = pr.Color,
                 UtcDateDetected = r.UtcDateCreated,
                 UtcDatePromised = r.UtcDatePromised,
                 UtcDateSolved = r.UtcDateAccomplished,
                 UtcDateClosed = r.UtcDateAccomplished,
                 UtcDateStarted = r.UtcDateStarted,
                 UtcDateModified = r.UtcDateModified,
                 OwnerFullName = User.GetFullName(r.ExecutorID),
                 OwnerID = r.ExecutorID,
                 Owner = new RFCGanttUserResultItem
                 {
                     ID = userOwner.IMObjID,
                     PositionName = userOwner.PositionName,
                     Phone = userOwner.Phone,
                     PhoneInternal = userOwner.Phone1,
                     SubdivisionFullName = Subdivision.GetFullSubdivisionName(userOwner.SubdivisionID),
                     Email = userOwner.Email,
                 },
                 QueueID = queue.IMObjID,
                 QueueName = queue.Name,
                 InitiatorID = r.InitiatorID,
                 Initiator = new RFCGanttUserResultItem
                 {
                     ID = userInit.IMObjID,
                     FullName = User.GetFullName(userInit.IMObjID),
                     PositionName = userInit.PositionName,
                     Phone = userInit.Phone,
                     PhoneInternal = userInit.Phone1,
                     SubdivisionFullName = Subdivision.GetFullSubdivisionName(userInit.SubdivisionID),
                     Email = userInit.Email,
                 },
                 EntityStateName = r.EntityStateName,
                 Description = r.Description,
                 Target = string.Empty,
                 ServiceName = string.Empty,
                 ParentID = wor.ObjectID,
                 CategoryName = null
             })
            .Where(f => (f.UtcDatePromised >= firstDay || f.UtcDatePromised == null)
                          && (f.UtcDateStarted >= secondDay || f.UtcDateStarted == null))
            .ToArrayAsync(cancellationToken);

        return subQuery1.Concat(subQuery2).ToArray();
    }
}