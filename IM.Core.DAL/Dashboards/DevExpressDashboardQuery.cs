using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Dashboards;

internal sealed class DevExpressDashboardQuery : IDevExpressDashboardQuery, ISelfRegisteredService<IDevExpressDashboardQuery>
{
    private readonly DbContext _db;

    public DevExpressDashboardQuery(
        CrossPlatformDbContext db)
    {
        _db = db;
    }

    public async Task<string> ExecuteAsync(Guid dashboardID, CancellationToken cancellationToken = default)
    {
        var result = await
            (from d in _db.Set<DashboardDevEx>().AsNoTracking()
            where d.DashboardID == dashboardID
            select d.Data).FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}