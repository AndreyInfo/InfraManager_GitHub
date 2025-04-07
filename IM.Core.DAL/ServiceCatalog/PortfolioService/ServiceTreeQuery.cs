using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalog.PortfolioService;

internal class ServiceTreeQuery : IPortfolioServiceTreeQuery, ISelfRegisteredService<IPortfolioServiceTreeQuery>
{ 
    private readonly DbContext _dbSet;

    public ServiceTreeQuery(CrossPlatformDbContext dbSet)
    {
        _dbSet = dbSet;
    }

    public async Task<PortfolioServicesItem[]> ExecuteAsync(Guid? slaID, Guid parentID,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Set<Service>().AsNoTracking().AsQueryable().Where(x=>x.CategoryID == parentID);

        return await query.Select(x => new PortfolioServicesItem
        {
            Name = x.Name,
            HasChild =
                _dbSet.Set<ServiceAttendance>().AsNoTracking().Any(z => z.ServiceID == x.ID) 
                ||
                _dbSet.Set<ServiceItem>().AsNoTracking().Any(z => z.ServiceID == x.ID),
            
            IsSelectPart = !slaID.HasValue ? false : _dbSet.Set<SLAReference>().AsNoTracking()
                .Any(q => (_dbSet.Set<ServiceAttendance>().AsNoTracking().Where(z => z.ServiceID == x.ID).Select(x => x.ID)
                    .Contains(q.ObjectID) || _dbSet.Set<ServiceItem>().AsNoTracking().Where(z => z.ServiceID == x.ID).Select(x => x.ID)
                    .Contains(q.ObjectID)) && q.SLAID == slaID),

            IsSelectFull = !slaID.HasValue ? false : _dbSet.Set<SLAReference>().Any(z => z.ObjectID == x.ID && z.SLAID == slaID),
            ParentId = x.CategoryID,
            Note = x.Note,
            ClassId = ObjectClass.Service,
            IconName = x.IconName,
            ID = x.ID,
            RowVersion = x.RowVersion
        }).ToArrayAsync(cancellationToken);
    }
}