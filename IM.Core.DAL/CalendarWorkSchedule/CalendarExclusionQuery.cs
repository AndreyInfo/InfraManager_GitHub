using InfraManager.DAL.CalendarWork;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.CalendarWorkSchedule
{
    internal class CalendarExclusionQuery : ICalendarExclusionQuery, ISelfRegisteredService<ICalendarExclusionQuery>
    {
        private readonly CrossPlatformDbContext _db;

        public CalendarExclusionQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<string> GetNameReferenceServiceAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Set<ServiceReference>().Where(x => x.ID == id)
                                                    .Select(x =>  DbFunctions.GetFullObjectName(x.ClassID, x.ObjectID))
                                                    .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
