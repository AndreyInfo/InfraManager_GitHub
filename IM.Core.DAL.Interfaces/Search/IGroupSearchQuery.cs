using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    public class GroupSearchCriteria : SearchCriteria
    {
        public byte Type { get; init; }
    }

    public interface IGroupSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(GroupSearchCriteria criteria, Guid? userId = default);
    }
}
