using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Calls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class CallSummarySearchQuery : ICallSummarySearchQuery, ISelfRegisteredService<ICallSummarySearchQuery>
    {
        private readonly CrossPlatformDbContext _db;
        private readonly IRepository<CallType> _callTypeRepository;

        public CallSummarySearchQuery(
                    IRepository<CallType> callTypeRepository,
                    CrossPlatformDbContext db)
        {
            _db = db;
            _callTypeRepository = callTypeRepository;
        }

        public IQueryable<ObjectSearchResult> Query(CallSummarySearchCriteria searchCriteria, Guid userId)
        {
            var pattern = string.IsNullOrEmpty(searchCriteria.Text) ? null : searchCriteria.Text.ToContainsPattern();

            var callType = _callTypeRepository
                                .With(x => x.Parent)
                                .FirstOrDefault(x => x.ID == searchCriteria.CallTypeID);

            var saValue = callType.IsChangeRequest ? 1 : 0;

            var availableIdsQuery =
                from sla in _db.Set<ServiceLevelAgreement>().AsNoTracking()
                join slaRef in _db.Set<SLAReference>().AsNoTracking()
                    on sla.ID equals slaRef.SLAID
                join orgItemGroup in _db.Set<OrganizationItemGroup>().AsNoTracking()
                    on sla.ID equals orgItemGroup.ID
                where (sla.UtcStartDate == null || sla.UtcStartDate < DateTime.UtcNow)
                    && (sla.UtcFinishDate == null || sla.UtcFinishDate > DateTime.UtcNow)
                    && Group.UserInOrganizationItem(orgItemGroup.ItemClassID, orgItemGroup.ItemID, userId)
                select slaRef.ObjectID;

            var catalogStates = new CatalogItemState[] {
                CatalogItemState.Worked,
                CatalogItemState.Blocked
            };

            return from callSummary in _db.Set<CallSummary>().AsNoTracking()
                   from serviceItem in _db.Set<ServiceItem>().AsNoTracking()
                                          .Where(x => x.ID == callSummary.ServiceItemID)
                                          .DefaultIfEmpty()
                   from serviceAttendance in _db.Set<ServiceAttendance>().AsNoTracking()
                                                .Where(x => x.ID == callSummary.ServiceAttendanceID)
                                                .DefaultIfEmpty()
                   from service in _db.Set<Service>().AsNoTracking()
                                      .Where(x => x.ID == serviceItem.ServiceID || x.ID == serviceAttendance.ServiceID)
                                      .DefaultIfEmpty()
                   join serviceCategory in _db.Set<ServiceCategory>().AsNoTracking()
                   on service.CategoryID equals serviceCategory.ID
                   into jServiceCategory
                   from ljServiceCategory in jServiceCategory.DefaultIfEmpty()
                   where callSummary.Visible == true && ((pattern == null) || EF.Functions.Like(callSummary.Name, pattern)) &&
                         ((saValue == 0 && callSummary.ServiceItemID != null) || (searchCriteria.CallTypeID == null) || (saValue == 1)) &&
                         ((callSummary.ServiceItemID == null) || 
                          ((callSummary.ServiceItemID != null) && service.Type == ServiceType.External &&
                           catalogStates.Contains(service.State) && (serviceItem.State == null || catalogStates.Contains(serviceItem.State.Value)))) &&
                         ((saValue == 1 && callSummary.ServiceAttendanceID != null) || (searchCriteria.CallTypeID == null) || (saValue == 0)) &&
                         ((callSummary.ServiceAttendanceID == null) || ((callSummary.ServiceAttendanceID != null) &&
                          service.Type == ServiceType.External && catalogStates.Contains(service.State) &&
                          (serviceAttendance.State == null || catalogStates.Contains(serviceAttendance.State.Value)) && serviceAttendance.Type == AttendanceType.User)) &&
                         (/* (@UserID IS NULL) OR */ // TODO: У нас всегда есть пользователь
                          (callSummary.ServiceItemID != null && availableIdsQuery.Any(x => x == callSummary.ServiceItemID || x == service.ID)) ||
                          (callSummary.ServiceAttendanceID != null && availableIdsQuery.Any(x => x == callSummary.ServiceAttendanceID || x == service.ID)))
                   orderby callSummary.Name
                   select new ObjectSearchResult
                   {
                       ID = callSummary.ID,
                       ClassID = ObjectClass.CallSummary,
                       FullName = callSummary.Name,
                       Details = $"{ljServiceCategory.Name} \\ {service.Name}  \\ {serviceItem.Name ?? serviceAttendance.Name}"
                   };
        }
    }
}
