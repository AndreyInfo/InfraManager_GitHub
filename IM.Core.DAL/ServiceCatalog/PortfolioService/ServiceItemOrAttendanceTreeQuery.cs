using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalog.PortfolioService;

internal class ServiceItemOrAttendanceTreeQuery : IPortfolioServiceTreeQuery,
    ISelfRegisteredService<IPortfolioServiceTreeQuery>
{
    private readonly DbContext _dbSet;

    public ServiceItemOrAttendanceTreeQuery(CrossPlatformDbContext dbSet)
    {
        _dbSet = dbSet;
    }

    public async Task<PortfolioServicesItem[]> ExecuteAsync(Guid? slaID, Guid parentID,
        CancellationToken cancellationToken = default)
    {
        var queryService = _dbSet.Set<ServiceItem>().AsNoTracking().AsQueryable().Where(x => x.ServiceID == parentID)
            .Select(x => new PortfolioServicesItem
            {
                Name = DbFunctions.CastAsString(x.Name),
                HasChild = false,
                IsSelectFull = slaID.HasValue && _dbSet.Set<SLAReference>().Any(z => z.ObjectID == x.ID && z.SLAID == slaID),
                Note = DbFunctions.CastAsString(x.Note),
                ClassId = ObjectClass.ServiceItem,
                ID = x.ID,
                ParentId = x.ServiceID,
                RowVersion = x.RowVersion
            });


        var queryServiceAttendance = _dbSet.Set<ServiceAttendance>().AsQueryable().Where(x => x.ServiceID == parentID)
            .Select(x => new PortfolioServicesItem
            {
                Name = DbFunctions.CastAsString(x.Name),
                HasChild = false,
                IsSelectFull = slaID.HasValue && _dbSet.Set<SLAReference>().Any(z => z.ObjectID == x.ID && z.SLAID == slaID),
                Note = DbFunctions.CastAsString(x.Note),
                ClassId = ObjectClass.ServiceAttendance,
                ParentId = x.ServiceID,
                ID = x.ID,
                RowVersion = x.RowVersion
            });

        queryService = queryService.Union(queryServiceAttendance);

        return await queryService.ToArrayAsync(cancellationToken);
    }
}