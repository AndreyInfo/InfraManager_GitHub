using System.Linq;

namespace InfraManager.DAL.Search;

public interface IIncidentResultSearchQuery
{
    IQueryable<ObjectSearchResult> Query(SearchCriteria searchBy);
}