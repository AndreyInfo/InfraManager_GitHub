using System.Linq;

namespace InfraManager.DAL.Search
{
    public interface IChangeRequestCategorySearcherQuery
    {
        IQueryable<ObjectSearchResult> Query(string text);
    }
}
