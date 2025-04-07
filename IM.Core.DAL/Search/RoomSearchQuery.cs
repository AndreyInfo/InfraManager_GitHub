using System;
using System.Linq;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Search;

internal class RoomSearchQuery : IRoomSearchQuery, ISelfRegisteredService<IRoomSearchQuery>
{
    private readonly DbSet<Room> _rooms;

    public RoomSearchQuery(DbSet<Room> rooms)
    {
        _rooms = rooms;
    }

    public IQueryable<ObjectSearchResult> Query(RoomSearchCriteria searchBy, Guid currentUserId)
    {
        var rooms = _rooms
            .Include(r => r.Floor)
            .ThenInclude(f => f.Building)
            .ThenInclude(b => b.Organization)
            .AsNoTracking()
            .Select(room => new ObjectSearchResult
            {
                ID = room.IMObjID,
                ClassID = ObjectClass.Room,
                FullName = room.Floor.Building.Organization.Name + " \\ " +
                           room.Floor.Building.Name + " \\ " +
                           room.Floor.Name + " \\ " +
                           room.Name,
            });

        if (string.IsNullOrWhiteSpace(searchBy.Text))
        {
            return rooms;
        }

        var searchPattern = searchBy.Text.Trim().ToContainsPattern();
        rooms = rooms.Where(o => EF.Functions.Like(o.FullName, searchPattern));

        return rooms;
    }
}