using System;
using System.Linq;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Search;

internal class RackSearchQuery : IRackSearchQuery, ISelfRegisteredService<IRackSearchQuery>
{
    private readonly DbSet<Rack> _racks;

    public RackSearchQuery(DbSet<Rack> racks)
    {
        _racks = racks;
    }

    public IQueryable<ObjectSearchResult> Query(RackSearchCriteria searchBy, Guid currentUserId)
    {
        var racks = _racks
            .Include(wp => wp.Room)
            .ThenInclude(r => r.Floor)
            .ThenInclude(f => f.Building)
            .ThenInclude(b => b.Organization)
            .AsNoTracking()
            .Select(rack => new ObjectSearchResult
            {
                ID = rack.IMObjID,
                ClassID = ObjectClass.Rack,
                FullName = rack.Room.Floor.Building.Organization.Name + " \\ " +
                           rack.Room.Floor.Building.Name + " \\ " +
                           rack.Room.Floor.Name + " \\ " +
                           rack.Room.Name + " \\ " +
                           rack.Name,
            });

        if (string.IsNullOrWhiteSpace(searchBy.Text))
        {
            return racks;
        }

        var searchPattern = searchBy.Text.Trim().ToContainsPattern();
        racks = racks.Where(o => EF.Functions.Like(o.FullName, searchPattern));

        return racks;
    }
}