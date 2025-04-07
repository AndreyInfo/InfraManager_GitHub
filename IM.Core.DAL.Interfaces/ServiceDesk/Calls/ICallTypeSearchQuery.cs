using System.Linq;

namespace InfraManager.DAL.Search
{
    public interface ICallTypeSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(SearchCriteria searchCriteria);
    }
}
