using System.Linq;

namespace InfraManager.DAL.Search;

public interface IRFCResultSearchQuery
{
    IQueryable<ObjectSearchResult> Query(SearchCriteria searchBy);
}