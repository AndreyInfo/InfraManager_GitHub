using System.Linq;

namespace InfraManager.DAL.Search
{
    public interface IChangeRequestServiceSearcherQuery
    {
        IQueryable<ObjectSearchResult> Query(string text);
    }
}
