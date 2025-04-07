using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalog.PortfolioService;

internal sealed class ServiceCategoryTreeQuery : IPortfolioServiceTreeQuery, ISelfRegisteredService<IPortfolioServiceTreeQuery>
{
    private readonly CrossPlatformDbContext _dbSet;

    public ServiceCategoryTreeQuery(CrossPlatformDbContext dbSet)
    {
        _dbSet = dbSet;
    }

    public async Task<PortfolioServicesItem[]> ExecuteAsync(Guid? slaID, Guid parentID,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Set<ServiceCategory>().AsNoTracking();

        return await query.Select(x => new PortfolioServicesItem
        {
            Name = x.Name,
            HasChild = _dbSet.Set<Service>().AsNoTracking().Any(z => z.CategoryID == x.ID),

            IsSelectPart = !slaID.HasValue ? false : (_dbSet.Set<SLAReference>().AsNoTracking()
                                                  .Any(q => _dbSet.Set<Service>().AsNoTracking()
                                                      .Where(z => z.CategoryID == x.ID).Select(x => x.ID)
                                                      .Contains(q.ObjectID) && q.SLAID == slaID)
                                              ||
                                              _dbSet.Set<SLAReference>().AsNoTracking().Any(t => _dbSet
                                                  .Set<ServiceItem>().AsNoTracking().Where(
                                                      q => _dbSet.Set<Service>().AsNoTracking()
                                                              .Where(z => z.CategoryID == x.ID).Select(x => x.ID)
                                                              .Contains(q.ServiceID.Value)).Select(x => x.ID)
                                                  .Contains(t.ObjectID) && t.SLAID == slaID)
                                              ||
                                              _dbSet.Set<SLAReference>().AsNoTracking().Any(t => _dbSet
                                                  .Set<ServiceAttendance>().AsNoTracking().Where(
                                                      q => _dbSet.Set<Service>().AsNoTracking()
                                                          .Where(z => z.CategoryID == x.ID).Select(x => x.ID)
                                                          .Contains(q.ServiceID.Value)).Select(x => x.ID)
                                                  .Contains(t.ObjectID) && t.SLAID == slaID)),
            
            IsSelectFull = !slaID.HasValue
                ? false
                : _dbSet.Set<SLAReference>()
                    .AsNoTracking().Count(q => _dbSet.Set<Service>().AsNoTracking().Where(z => z.CategoryID == x.ID).Select(x => x.ID)
                        .Contains(q.ObjectID) && q.SLAID == slaID) == _dbSet.Set<Service>().AsNoTracking().Count(z => z.CategoryID == x.ID),
            
            Note = x.Note,
            ClassId = ObjectClass.ServiceCategory,
            IconName = x.IconName,
            ID = x.ID,
            RowVersion = x.RowVersion
        }).ToArrayAsync(cancellationToken);
    }
}