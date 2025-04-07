using System;
using System.Linq;
using Inframanager;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk.Calls;

internal class CallIsAvailableToExecutorViaSupportLine :
    IBuildAvailableToExecutorViaSupportLine<User, Call>,
    IBuildAvailableToExecutorViaSupportLine<Group, Call>,
    ISelfRegisteredService<CallIsAvailableToExecutorViaSupportLine>
{
    private readonly DbContext _db;

    public CallIsAvailableToExecutorViaSupportLine(CrossPlatformDbContext db)
    {
        _db = db;
    }

    internal Specification<TExecutor> BuildSpecification<TExecutor>(Guid callID, ObjectClass itemClassID)
        where TExecutor : IGloballyIdentifiedEntity
    {
        var callServices = _db.Set<Call>().Include(x => x.CallService).AsNoTracking()
            .Where(c => c.IMObjID == callID
                        && c.CallService.ServiceID.HasValue
                        && (c.CallService.ServiceItemID.HasValue || c.CallService.ServiceAttendanceID.HasValue))
            .Select(c => c.CallService);

        var supportLines = _db.Set<SupportLineResponsible>().AsNoTracking();

        var linesOfService = callServices
            .Join(supportLines,
                service => new { ObjectID = service.ServiceID, ClassID = ObjectClass.Service, },
                line => new { ObjectID = (Guid?) line.ObjectID, ClassID = line.ObjectClassID, },
                (service, line) => line);

        var linesOfServiceItem = callServices
            .Join(supportLines,
                service => new { ObjectID = service.ServiceItemID, ClassID = ObjectClass.ServiceItem, },
                line => new { ObjectID = (Guid?) line.ObjectID, ClassID = line.ObjectClassID, },
                (service, line) => line);

        var linesOfServiceAttendance = callServices
            .Join(supportLines,
                service => new { ObjectID = service.ServiceItemID, ClassID = ObjectClass.ServiceAttendance, },
                line => new { ObjectID = (Guid?) line.ObjectID, ClassID = line.ObjectClassID, },
                (service, line) => line);

        var allSupportLines= linesOfService
            .Concat(linesOfServiceItem)
            .Concat(linesOfServiceAttendance);
        
        return new Specification<TExecutor>(executor =>
            !allSupportLines.Any()
            || allSupportLines.Any(line =>
                DbFunctions.ItemInOrganizationItem(
                    line.OrganizationItemClassID,
                    line.OrganizationItemID,
                    itemClassID,
                    executor.IMObjID
                )));
    }

    Specification<User> IBuildSpecification<User, Call>.Build(Call filterBy)
    {
        return BuildSpecification<User>(filterBy.IMObjID, ObjectClass.User);
    }

    Specification<Group> IBuildSpecification<Group, Call>.Build(Call filterBy)
    {
        return BuildSpecification<Group>(filterBy.IMObjID, ObjectClass.Group);
    }
}