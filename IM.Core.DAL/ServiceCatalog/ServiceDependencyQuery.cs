using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalog;

internal sealed class ServiceDependencyQuery : ServiceBaseQuery, IServiceDependencyQuery, ISelfRegisteredService<IServiceDependencyQuery>
{
    public ServiceDependencyQuery(CrossPlatformDbContext db,
                                 IPagingQueryCreator pagging) : base(db, pagging)
    { }

    public async Task<ServiceModelItem[]> ExecuteQueryServiceDependencyAsync(PaggingFilter filter, Sort sortProperty, Guid? parentID, CancellationToken cancellationToken = default)
    {
        var query = _db.Set<ServiceDependency>().AsNoTracking()
                          .Where(c => c.ParentServiceID == parentID)
                          .Select(c => c.ChildService);

        return await GetServiceModelsByQueryAsync(query, filter, sortProperty, cancellationToken);
    }
}
