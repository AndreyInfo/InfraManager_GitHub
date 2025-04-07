using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager;
using InfraManager.DAL.ServiceDesk.Calls;

namespace InfraManager.DAL.ServiceCatalog
{
    internal class SLAQuery : ISLAQuery, ISelfRegisteredService<ISLAQuery>
    {
        private class TempOrgItem
        {
            public Guid Id { get; init; }

            public int Sequence { get; set; }
        }

        private class TempSubDivision
        {
            public Guid Id { get; init; }

            public bool InProcess { get; set; }

            public int Sequence { get; set; }
        }

        private readonly DbContext _db;

        public SLAQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<SLAItem[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            // User
            var orgItems = new List<TempOrgItem>()
            {
                new TempOrgItem {
                    Id = userId,
                    Sequence = 0
                }
            };

            // User subdivision
            var sequence = 1;
            var query = from dv in _db.Set<Subdivision>().AsNoTracking()
                        join ur in _db.Set<User>().AsNoTracking()
                                      .Include(x => x.Subdivision)
                        on dv.ID equals ur.Subdivision.ID
                        where ur.IMObjID == userId
                        select new TempSubDivision
                        {
                            Id = dv.ID,
                            InProcess = false,
                            Sequence = sequence
                        };
            var tempSubDivs = await query.ToListAsync(cancellationToken);

            // All parent user subdivisions
            var subDivision = tempSubDivs.FirstOrDefault(x => !x.InProcess);
            while (subDivision != null)
            {
                subDivision.InProcess = true;
                sequence++;

                var list = await _db.Set<Subdivision>().AsNoTracking()
                    .Where(x => x.ID == subDivision.Id && x.SubdivisionID != null)
                    .Select(x => new TempSubDivision
                    {
                        Id = x.SubdivisionID.Value,
                        InProcess = false,
                        Sequence = sequence
                    })
                    .ToArrayAsync(cancellationToken);

                tempSubDivs.AddRange(list);

                subDivision = tempSubDivs.FirstOrDefault(x => !x.InProcess);
            }

            orgItems.AddRange(tempSubDivs.Select(x => new TempOrgItem
            {
                Id = x.Id,
                Sequence = x.Sequence
            }));

            // User organization
            var queryOrg = from dv in _db.Set<Subdivision>().AsNoTracking()
                           join ur in _db.Set<User>().AsNoTracking()
                                         .Include(x => x.Subdivision)
                           on dv.ID equals ur.Subdivision.ID
                           where ur.IMObjID == userId
                           select new TempOrgItem
                           {
                               Id = dv.OrganizationID,
                               Sequence = sequence
                           };
            orgItems.AddRange(await queryOrg.ToArrayAsync(cancellationToken));

            var tmp = new List<TempOrgItem>();
            var tmpSLA = new List<TempOrgItem>();
            while (orgItems.Any())
            {
                var orgItem = orgItems.OrderBy(x => x.Sequence)
                                      .First();
                orgItems.Remove(orgItem);

                var slaQuery = from sla in _db.Set<ServiceLevelAgreement>().AsNoTracking()
                               join oig in _db.Set<OrganizationItemGroup>().AsNoTracking()
                               on sla.ID equals oig.ID
                               where oig.ItemID == orgItem.Id
                               orderby sla.UtcStartDate
                               select new TempOrgItem
                               {
                                   Id = sla.ID,
                                   Sequence = sequence
                               };
                tmp.AddRange(await slaQuery.ToArrayAsync(cancellationToken));

                while (tmp.Any())
                {
                    var tmpItem = tmp.First();
                    tmp.Remove(tmpItem);

                    if (!tmpSLA.Any(x => x.Id == tmpItem.Id))
                        tmpSLA.Add(tmpItem);
                }
            }

            var ids = tmpSLA.Select(x => x.Id).ToArray();
            var SLAs = await _db.Set<ServiceLevelAgreement>().AsNoTracking()
                          .Where(x => ids.Contains(x.ID))
                          .Select(x => new SLAItem
                          {
                              ID = x.ID,
                              Name = x.Name,
                              Note = x.Note,
                              Number = x.Number,
                              UtcStartDate = x.UtcStartDate,
                              UtcFinishDate = x.UtcFinishDate,
                              TimeZoneID = x.TimeZoneID,
                              CalendarWorkScheduleID = x.CalendarWorkScheduleID,
                              TimeZoneName = DbFunctions.GetFullTimeZoneName(x.TimeZoneID),
                              CalendarWorkScheduleName = DbFunctions.GetFullCalendarWorkScheduleName(x.CalendarWorkScheduleID)
                          })
                          .ToArrayAsync(cancellationToken);

            return SLAs.Select(x => Tuple.Create(x, tmpSLA.First(z => z.Id == x.ID).Sequence))
                       .OrderBy(x => x.Item2)
                       .Select(x => x.Item1)
                       .ToArray();
        }

        public async Task<SLAReferenceItem[]> GetReferencesAsync(CancellationToken cancellationToken)
        {
            return await _db.Set<SLAReference>().AsNoTracking()
               .Select(x => new SLAReferenceItem
               {
                   ID = x.SLAID,
                   ClassID = x.ClassID,
                   ObjectID = x.ObjectID,
                   CalendarWorkScheduleID = x.CalendarWorkScheduleID,
                   TimeZoneID = x.TimeZoneID,
                   TimeZoneName = DbFunctions.GetFullTimeZoneName(x.TimeZoneID),
                   CalendarWorkScheduleName = DbFunctions.GetFullCalendarWorkScheduleName(x.CalendarWorkScheduleID)
               })
               .ToArrayAsync(cancellationToken);
        }
    }
}
