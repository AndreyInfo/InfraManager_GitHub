using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location;

internal sealed class LocationNodeRoomQuery : LocationNodeQuery<Room>
{
    private readonly DbSet<Workplace> _workplaces;
    private readonly DbSet<Rack> _racks;

    public LocationNodeRoomQuery(DbSet<Room> rooms
                                         , DbSet<Workplace> workplaces
                                         , DbSet<Rack> racks
                                         , DbSet<ClassIcon> classIcons)
        : base(classIcons, rooms)
    {
        _racks = racks;
        _workplaces = workplaces;
    }

    protected override ObjectClass ClassID => ObjectClass.Room;

    public override async Task<LocationTreeNode[]> GetNodesAsync(int parentID, ObjectClass? childClassID = null, CancellationToken cancellationToken = default)
    {
        return await _entities.AsNoTracking().Where(c => c.FloorID == parentID)
            .Select(c => new LocationTreeNode()
            {
                ID = c.ID,
                UID = c.IMObjID,
                Name = c.Name,
                ClassID = ClassID,
                ParentID = c.FloorID,
                ParentUID = c.Floor.IMObjID,
                HasChild = childClassID.HasValue
                           && (childClassID == ObjectClass.Workplace
                                ? _workplaces.Any(v => v.RoomID == c.ID)
                                : _racks.Any(v => v.RoomID == c.ID)),
                IconName = _classIcons.Where(c => c.ClassID == ClassID)
                                      .Select(c => c.IconName)
                                      .FirstOrDefault()
            })
            .ToArrayAsync(cancellationToken);
    }
}
