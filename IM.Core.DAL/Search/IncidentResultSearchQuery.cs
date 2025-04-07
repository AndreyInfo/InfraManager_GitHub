using System.Linq;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Search;

public class IncidentResultSearchQuery : IIncidentResultSearchQuery, ISelfRegisteredService<IIncidentResultSearchQuery>
{
    private readonly DbSet<IncidentResult> _incidentResults;

    public IncidentResultSearchQuery(DbSet<IncidentResult> incidentResults) => _incidentResults = incidentResults;

    public IQueryable<ObjectSearchResult> Query(SearchCriteria searchBy)
    {
        var incidentQuery = _incidentResults.AsNoTracking()
            .Where(incidentResult => !incidentResult.Removed);

        if (!string.IsNullOrWhiteSpace(searchBy.Text))
        {
            var searchPattern = searchBy.Text.Trim().ToLower().ToContainsPattern();
            incidentQuery =
                incidentQuery.Where(incidentResult => EF.Functions.Like(incidentResult.Name.ToLower(), searchPattern));
        }

        return incidentQuery.Select(incidentResult => new ObjectSearchResult
        {
            ID = incidentResult.ID,
            FullName = incidentResult.Name,
            ClassID = ObjectClass.IncidentResult
        });
    }
}