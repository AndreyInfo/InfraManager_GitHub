using System.Linq;

namespace InfraManager.DAL.Search
{
    public interface IChangeRequestTypeSearcherQuery
    {
        IQueryable<ObjectSearchResult> Query(string text);
    }
}
