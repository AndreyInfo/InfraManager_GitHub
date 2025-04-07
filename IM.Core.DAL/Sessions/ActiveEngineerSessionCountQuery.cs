using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Sessions;

internal class ActiveEngineerSessionCountQuery : IActiveEngineerSessionCountQuery,
    ISelfRegisteredService<IActiveEngineerSessionCountQuery>
{
    private readonly DbContext _db;

    public ActiveEngineerSessionCountQuery(CrossPlatformDbContext db)
    {
        _db = db;
    }

    public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await (_db.Set<Session>()
            .AsNoTracking()
            .AsQueryable()
            .Where(sessions => sessions.UtcDateClosed != null &&
                               !_db.Set<UserPersonalLicence>().AsNoTracking().Any(x => x.UserID == sessions.UserID) &&
                               _db.Set<UserRole>().AsNoTracking().Any(x => x.UserID == sessions.UserID))
            .CountAsync(cancellationToken));
    }
}