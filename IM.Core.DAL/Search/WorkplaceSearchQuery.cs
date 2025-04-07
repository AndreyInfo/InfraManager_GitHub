using System;
using System.Linq;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Search;

internal class WorkplaceSearchQuery : IWorkplaceSearchQuery, ISelfRegisteredService<IWorkplaceSearchQuery>
{
    private readonly DbSet<Workplace> _workplaces;

    public WorkplaceSearchQuery(DbSet<Workplace> workplaces)
    {
        _workplaces = workplaces;
    }

    public IQueryable<ObjectSearchResult> Query(WorkplaceSearchCriteria searchBy, Guid currentUserId)
    {
        var workplaces = _workplaces
            .Include(wp => wp.Room)
            .ThenInclude(r => r.Floor)
            .ThenInclude(f => f.Building)
            .ThenInclude(b => b.Organization)
            .AsNoTracking()
            .Select(workplace => new ObjectSearchResult
            {
                ID = workplace.IMObjID,
                ClassID = ObjectClass.Workplace,
                FullName = workplace.Room.Floor.Building.Organization.Name + " \\ " +
                           workplace.Room.Floor.Building.Name + " \\ " +
                           workplace.Room.Floor.Name + " \\ " +
                           workplace.Room.Name + " \\ " +
                           workplace.Name,
            });

        if (string.IsNullOrWhiteSpace(searchBy.Text))
        {
            return workplaces;
        }

        var searchPattern = searchBy.Text.Trim().ToContainsPattern();
        workplaces = workplaces.Where(o => EF.Functions.Like(o.FullName, searchPattern));

        return workplaces;
    }
}