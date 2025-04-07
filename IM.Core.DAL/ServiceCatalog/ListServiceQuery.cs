using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalog;

internal sealed class ListServiceQuery : ServiceBaseQuery, IListServiceQuery, ISelfRegisteredService<IListServiceQuery>
{
    public ListServiceQuery(CrossPlatformDbContext db,
        IPagingQueryCreator pagging) : base(db, pagging)
    { }

    public async Task<ServiceModelItem[]> ExecuteAsync(PaggingFilter filter, Sort sortProperty, Guid? categoryID, CancellationToken cancellationToken = default)
    {
        var query = _db.Set<Service>().AsNoTracking();
        if (categoryID.HasValue)
            query = query.Where(c => c.CategoryID == categoryID);

        return await GetServiceModelsByQueryAsync(query, filter, sortProperty, cancellationToken);
    }
}

