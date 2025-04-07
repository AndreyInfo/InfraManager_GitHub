using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    public class SubdivisionSearchCriteria : SearchCriteria
    {
        public Guid? UserId { get; init; }
    }

    public interface ISubdivisionSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(SubdivisionSearchCriteria searchBy, Guid? currentUserId = default);
    }
}
