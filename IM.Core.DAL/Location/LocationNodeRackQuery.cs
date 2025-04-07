using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location;

internal sealed class LocationNodeRackQuery : LocationNodeQuery<Rack>
{
    public LocationNodeRackQuery(DbSet<Rack> racks
                                 , DbSet<ClassIcon> classIcons)
        : base(classIcons, racks)
    { }

    protected override ObjectClass ClassID => ObjectClass.Rack;

    public override async Task<LocationTreeNode[]> GetNodesAsync(int parentID, ObjectClass? childClassID = null, CancellationToken cancellationToken = default)
    {
        return await _entities.AsNoTracking().Where(c => c.RoomID == parentID)
            .Select(c => new LocationTreeNode()
            {
                ID = c.ID,
                UID = c.IMObjID,
                Name = c.Name,
                ClassID = ClassID,
                ParentID = c.RoomID,
                ParentUID = c.Room.IMObjID,
                IconName = _classIcons.Where(c => c.ClassID == ClassID)
                                      .Select(c => c.IconName)
                                      .FirstOrDefault()
            })
            .ToArrayAsync(cancellationToken);
    }
}