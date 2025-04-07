using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    public class ServiceItemAndAttendanceSearchCriteria : SearchCriteria
    {
        public Guid? CallTypeId { get; init; }
        public Guid? ClientID { get; init; }
    }
    public interface IServiceItemAndAttendanceSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(
            ServiceItemAndAttendanceSearchCriteria searchCriteria,
            Guid userId);
    }
}
