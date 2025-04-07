using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.DAL.Location
{
    internal class RoomQuery : IRoomQuery, ISelfRegisteredService<IRoomQuery>
    {
        private readonly CrossPlatformDbContext _context;

        public RoomQuery(CrossPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<RoomItem> QueryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = from rm in _context.Set<Room>().AsNoTracking()
                        join fl in _context.Set<Floor>().AsNoTracking()
                        on rm.FloorID equals fl.ID
                        join bl in _context.Set<Building>().AsNoTracking()
                        on fl.BuildingID equals bl.ID
                        join org in _context.Set<Organization>().AsNoTracking()
                        on bl.OrganizationID equals org.ID
                        into jorg
                        from org in jorg.DefaultIfEmpty()
                        where rm.IMObjID == id
                        select new RoomItem
                        {
                            ID = rm.IMObjID,
                            IntID = rm.ID,
                            Name = rm.Name,
                            OrganizationID = bl.OrganizationID,
                            OrganizationName = org.Name,
                            BuildingID = fl.BuildingID,
                            BuildingName = bl.Name,
                            FloorID = rm.FloorID,
                            FloorName = fl.Name
                        };
            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
