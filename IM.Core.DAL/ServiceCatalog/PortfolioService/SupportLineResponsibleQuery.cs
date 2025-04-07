using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalog.PortfolioService;

internal class SupportLineResponsibleQuery : ISupportLineResponsibleQuery, ISelfRegisteredService<ISupportLineResponsibleQuery>
{
    private readonly DbContext _dbSet;

    public SupportLineResponsibleQuery(CrossPlatformDbContext dbSet) => _dbSet = dbSet;

    public async Task<IEnumerable<SupportLineResponsible>> ExecuteAsync(Guid objectID, ObjectClass objectClassID, CancellationToken cancellationToken = default)
    {
        var supportObjectLines = await QueryFromDirectService(objectID, objectClassID, cancellationToken);
        if (supportObjectLines.Length == 0)
        {
            supportObjectLines = await QueryFromParentService(objectID, objectClassID, cancellationToken);
        }
        
        return supportObjectLines;
    }

    private async Task<SupportLineResponsible[]> QueryFromDirectService(Guid objectID,
        ObjectClass objectClassID, CancellationToken cancellationToken)
        => await _dbSet.Set<SupportLineResponsible>()
            .AsNoTracking()
            .Where(slr => slr.ObjectID == objectID && slr.ObjectClassID == objectClassID)
            .OrderBy(x => x.LineNumber)
            .ToArrayAsync(cancellationToken);
    
    
    private async Task<SupportLineResponsible[]> QueryFromParentService(Guid objectID, ObjectClass objectClassID, CancellationToken cancellationToken)
    {
        var directServiceQuery = _dbSet.Set<SupportLineResponsible>()
            .AsNoTracking()
            .Where(slr => slr.ObjectClassID == objectClassID);

        var callServiceAttendanceQuery = _dbSet.Set<CallService>()
            .AsNoTracking()
            .Where(cs => cs.ServiceAttendanceID == objectID);

        var callServiceItemQuery = _dbSet.Set<CallService>()
            .AsNoTracking()
            .Where(cs => cs.ServiceItemID == objectID);
        
        var serviceAttendanceSupportLineResponsibles = directServiceQuery
            .Join(callServiceAttendanceQuery,
                slr => slr.ObjectID,
                cs => cs.ServiceID,
                (slr, cs) => slr);

        var serviceItemSupportLineResponsibles = directServiceQuery
            .Join(callServiceItemQuery,
                slr => slr.ObjectID,
                cs => cs.ServiceID,
                (slr, cs) => slr);
                
        return await serviceAttendanceSupportLineResponsibles
            .Union(serviceItemSupportLineResponsibles)
            .OrderBy(c=> c.LineNumber)
            .ToArrayAsync(cancellationToken);
    }
}