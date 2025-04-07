using System;
using System.Linq;

namespace InfraManager.DAL.Search;

public interface IRackSearchQuery
{
    IQueryable<ObjectSearchResult> Query(RackSearchCriteria searchBy, Guid currentUserId);
}