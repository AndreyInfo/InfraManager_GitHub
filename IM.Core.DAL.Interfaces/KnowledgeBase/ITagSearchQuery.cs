using System.Linq;

namespace InfraManager.DAL.KnowledgeBase
{
    public interface ITagSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(SearchCriteria searchCriteria);
    }
}
