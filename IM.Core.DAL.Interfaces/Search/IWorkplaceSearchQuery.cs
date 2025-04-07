using System;
using System.Linq;

namespace InfraManager.DAL.Search;

public interface IWorkplaceSearchQuery
{
    IQueryable<ObjectSearchResult> Query(WorkplaceSearchCriteria searchBy, Guid currentUserId);
}