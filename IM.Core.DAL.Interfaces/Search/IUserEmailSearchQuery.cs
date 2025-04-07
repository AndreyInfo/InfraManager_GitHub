using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    public class UserEmailSearchCriteria : SearchCriteria
    {
    }

    public interface IUserEmailSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(UserEmailSearchCriteria searchCriteria, Guid currentUserId);
    }
}
