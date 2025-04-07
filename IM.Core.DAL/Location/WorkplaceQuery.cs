using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.DAL.Location
{
    internal class WorkplaceQuery : IWorkplaceQuery, ISelfRegisteredService<IWorkplaceQuery>
    {
        private readonly CrossPlatformDbContext _context;

        public WorkplaceQuery(CrossPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<WorkplaceItem> QueryAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = from wp in _context.Set<Workplace>().AsNoTracking()
                        join rm in _context.Set<Room>().AsNoTracking()
                        on wp.RoomID equals rm.ID
                        join fl in _context.Set<Floor>().AsNoTracking()
                        on rm.FloorID equals fl.ID
                        join bl in _context.Set<Building>().AsNoTracking()
                        on fl.BuildingID equals bl.ID
                        join org in _context.Set<Organization>().AsNoTracking()
                        on bl.OrganizationID equals org.ID
                        into jorg
                        from org in jorg.DefaultIfEmpty()
                        where wp.IMObjID == id
                        select new WorkplaceItem
                        {
                            ID = wp.IMObjID,
                            IntID = wp.ID,
                            Name = wp.Name,
                            OrganizationID = bl.OrganizationID,
                            OrganizationName = org.Name,
                            BuildingID = fl.BuildingID,
                            BuildingName = bl.Name,
                            FloorID = rm.FloorID,
                            FloorName = fl.Name,
                            RoomID = wp.RoomID,
                            RoomName = rm.Name
                        };
            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
