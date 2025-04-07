using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Location;

internal sealed class LocationNodeFloorQuery : LocationNodeQuery<Floor>
{
    private readonly DbSet<Room> _rooms;

    public LocationNodeFloorQuery(DbSet<Floor> floors
                                         , DbSet<Room> rooms
                                         , DbSet<ClassIcon> classIcons)
        : base(classIcons, floors)
    {
        _rooms = rooms;
    }

    protected override ObjectClass ClassID => ObjectClass.Floor;

    public override async Task<LocationTreeNode[]> GetNodesAsync(int parentID, ObjectClass? childClassID = null, CancellationToken cancellationToken = default)
    {
        return await _entities.AsNoTracking().Where(c => c.BuildingID == parentID)
            .Select(c => new LocationTreeNode()
            {
                ID = c.ID,
                UID = c.IMObjID,
                Name = c.Name,
                ClassID = ClassID,
                ParentID = c.BuildingID,
                ParentUID = c.Building.IMObjID,
                HasChild = _rooms.Any(v => v.FloorID == v.ID),
                IconName = _classIcons.Where(c => c.ClassID == ClassID)
                                      .Select(c => c.IconName)
                                      .FirstOrDefault()
            })
            .ToArrayAsync(cancellationToken);
    }
}
