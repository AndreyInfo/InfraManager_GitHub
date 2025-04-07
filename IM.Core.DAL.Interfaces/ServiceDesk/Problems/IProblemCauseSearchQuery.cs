using System.Linq;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    public interface IProblemCauseSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(SearchCriteria searchCriteria);
    }
}
