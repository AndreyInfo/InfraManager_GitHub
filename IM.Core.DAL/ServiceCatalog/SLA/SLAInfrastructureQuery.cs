using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceCatalog.SLA;

internal class SLAInfrastructureQuery : ISLAInfrastructureQuery, ISelfRegisteredService<ISLAInfrastructureQuery>
{
    private readonly DbContext _dbSet;

    public SLAInfrastructureQuery(CrossPlatformDbContext dbSet)
    {
        _dbSet = dbSet;
    }

    public async Task<PortfolioServiceInfrastructureItem[]> ExecuteAsync(Guid slaID, Guid serviceID, int? skip = null, int? take = null,
        string searchString = null, CancellationToken cancellationToken = default)
    {
        var querySlaServiceReference = _dbSet.Set<SLAServiceReference>().AsQueryable();
        var queryServiceReference = _dbSet.Set<ServiceReference>().AsQueryable();

        var slaServiceIDs = querySlaServiceReference.Where(x => x.SLAID == slaID).AsNoTracking();

        if (skip.HasValue)
        {
            queryServiceReference = queryServiceReference.Skip(skip.Value);
        }
        
        if (take.HasValue)
        {
            queryServiceReference = queryServiceReference.Take(take.Value);
        }

        return await queryServiceReference
            .Where(x => slaServiceIDs.Any(z => x.ID == z.ServiceReferenceID) && x.ServiceID == serviceID).Select(x =>
                new PortfolioServiceInfrastructureItem
                {
                    ID = x.ID,
                    ServiceID = x.ServiceID,
                    ClassID = x.ClassID,
                    ObjectID = x.ObjectID,
                    Name = DbFunctions.GetFullObjectName(x.ClassID, x.ObjectID),
                    Location = DbFunctions.GetFullObjectLocation(x.ClassID, x.ObjectID),
                    Category = _dbSet.Set<InframanagerObjectClass>().FirstOrDefault(z=>z.ID == x.ClassID).Name
                }).ToArrayAsync(cancellationToken);
    }
}