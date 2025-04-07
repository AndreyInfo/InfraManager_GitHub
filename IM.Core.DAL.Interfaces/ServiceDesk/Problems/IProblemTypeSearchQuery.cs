using System.Linq;

namespace InfraManager.DAL.Search
{
    public interface IProblemTypeSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(string text);
    }
}
