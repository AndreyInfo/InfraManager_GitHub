using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Sessions;

internal class ActivePersonalSessionCountQuery : IActivePersonalSessionCountQuery,
    ISelfRegisteredService<IActivePersonalSessionCountQuery>
{
    private readonly DbContext _db;

    public ActivePersonalSessionCountQuery(CrossPlatformDbContext db)
    {
        _db = db;
    }

    public async Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await (_db.Set<Session>()
            .AsNoTracking()
            .AsQueryable()
            .Where(sessions => sessions.UtcDateClosed != null &&
                               _db.Set<UserPersonalLicence>().AsNoTracking().Any(x => x.UserID == sessions.UserID))
            .CountAsync(cancellationToken));
    }
}