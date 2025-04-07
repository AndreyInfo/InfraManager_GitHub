using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    public class OrganizationSearchCriteria : SearchCriteria
    {
        public Guid? UserId { get; init; }
    }

    public interface IOrganizationSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(OrganizationSearchCriteria searchBy, Guid? currentUserId = default);
    }
}
