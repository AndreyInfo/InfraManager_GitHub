using System;
using System.Linq;

namespace InfraManager.DAL.Search;

public interface IRoomSearchQuery
{
    IQueryable<ObjectSearchResult> Query(RoomSearchCriteria searchBy, Guid currentUserId);
}