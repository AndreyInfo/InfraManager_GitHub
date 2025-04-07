using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    public interface IUserSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(UserSearchCriteria searchCriteria, Guid currentUserId);
    }
}
