using System;
using System.Linq;

namespace InfraManager.DAL.Search;

public interface IUtilizerSearchQuery
{
    IQueryable<ObjectSearchResult> Query(UtilizerSearchCriteria searchBy, Guid currentUserId = default);
}