using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Threading;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    internal class ServiceItemAttendanceQuery : IServiceItemAttendanceQuery, ISelfRegisteredService<IServiceItemAttendanceQuery>
    {
        private readonly DbContext _db;

        public ServiceItemAttendanceQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceItemAttendanceItem[]> GetItemAttendacesAsync(Guid userId, CancellationToken cancellationToken)
        {
            var query = (from srvitm in _db.Set<ServiceItem>().AsNoTracking()
                         join cntrl in _db.Set<CustomControl>().AsNoTracking()
                                          .Where(x => x.UserId == userId)
                         on srvitm.ID equals cntrl.ObjectId into ccjoin
                         from cntrl in ccjoin.DefaultIfEmpty()
                         where srvitm.State == null || srvitm.State == CatalogItemState.Worked || srvitm.State == CatalogItemState.Blocked
                         select new ServiceItemAttendanceItem {
                             ID = srvitm.ID,
                             Name = srvitm.Name,
                             Note = DbFunctions.CastAsString(srvitm.Note),
                             Parameter = srvitm.Parameter,
                             Summary = "",
                             ServiceID = srvitm.ServiceID,
                             ObjectClass = ObjectClass.ServiceItem,
                             IsInFavorite = cntrl != null
                         })
                  .Union(from sa in _db.Set<ServiceAttendance>().AsNoTracking()
                         join cntrl in _db.Set<CustomControl>().AsNoTracking()
                                          .Where(x => x.UserId == userId)
                         on sa.ID equals cntrl.ObjectId into ccjoin
                         from cntrl in ccjoin.DefaultIfEmpty()
                         where (sa.State == null || sa.State == CatalogItemState.Worked || sa.State == CatalogItemState.Blocked)
                               && sa.Type == AttendanceType.User
                         select new ServiceItemAttendanceItem {
                             ID = sa.ID,
                             Name = sa.Name,
                             Note = DbFunctions.CastAsString(sa.Note),
                             Parameter = sa.Parameter,
                             Summary = "",
                             ServiceID = sa.ServiceID,
                             ObjectClass = ObjectClass.ServiceAttendance,
                             IsInFavorite = cntrl != null
                         });

            return await query.ToArrayAsync(cancellationToken);
        }
    }
}
