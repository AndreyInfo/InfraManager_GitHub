using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Sessions;

internal class SessionListQuery : ISessionListQuery, ISelfRegisteredService<ISessionListQuery>
{
    public async Task<SessionDetailsListItem[]> ExecuteAsync(IOrderedQueryable<Session> orderedQuery, int take,
        int skip, CancellationToken cancellationToken = default)
    {
        var selectQuery = orderedQuery.Select(x => new SessionDetailsListItem
        {
            LicenceType = x.LicenceType,
            Location = x.Location,
            SecurityStamp = x.SecurityStamp,
            UserAgent = x.UserAgent,
            UserLogin = x.User.LoginName,
            UserName = x.User.FullName,
            UserID = x.UserID,
            UtcDateClosed = x.UtcDateClosed,
            UtcDateOpened = x.UtcDateOpened,
            UserSubdivisionFullName = x.User.SubdivisionID.HasValue ? DbFunctions.GetFullObjectLocation(ObjectClass.Division, x.User.SubdivisionID) +
                                      " \\ " + Subdivision.GetFullSubdivisionName(x.User.SubdivisionID) : "",
            UtcDateLastActivity = x.UtcDateLastActivity
        });

        if (take > 0)
        {
            selectQuery = selectQuery.Take(take);
        }

        return await selectQuery.Skip(skip).ToArrayAsync(cancellationToken);
    }
}