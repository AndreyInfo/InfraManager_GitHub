using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class ServiceItemAndAttendanceSearchQuery :
        IServiceItemAndAttendanceSearchQuery,
        ISelfRegisteredService<IServiceItemAndAttendanceSearchQuery>
    {
        private readonly DbContext _db;

        public ServiceItemAndAttendanceSearchQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<ObjectSearchResult> Query(
            ServiceItemAndAttendanceSearchCriteria criteria,
            Guid userId)
        {
            var pattern =criteria.Text.ToLower();
            var availableIdsQuery =
                from sla in _db.Set<ServiceLevelAgreement>().AsNoTracking()
                join slaRef in _db.Set<SLAReference>().AsNoTracking()
                    on sla.ID equals slaRef.SLAID
                join orgItemGroup in _db.Set<OrganizationItemGroup>().AsNoTracking()
                    on sla.ID equals orgItemGroup.ID
                where (sla.UtcStartDate == null || sla.UtcStartDate < DateTime.UtcNow)
                    && (sla.UtcFinishDate == null || sla.UtcFinishDate > DateTime.UtcNow)
                    && Group.UserInOrganizationItem(orgItemGroup.ItemClassID, orgItemGroup.ItemID, criteria.ClientID ?? userId)
                select slaRef.ObjectID;

            // TODO: ServiceItem and ServiceAttendance have similar structure: fix copy - paste
            IQueryable<ServiceItem> serviceItemsQuery = _db.Set<ServiceItem>().AsNoTracking()
                .Where(x => x.Service.Type == ServiceType.External)
                .Where(x => x.State == CatalogItemState.Worked || x.State == CatalogItemState.Blocked || x.State == null)
                .Where(x => x.Service.State == CatalogItemState.Worked || x.Service.State == CatalogItemState.Blocked)
                .Where(x => availableIdsQuery.Any(t => t == x.ID || t == x.Service.ID));
            IQueryable<ServiceAttendance> serviceAttendancesQuery = _db.Set<ServiceAttendance>().AsNoTracking()
                .Where(x => x.Service.Type == ServiceType.External)
                .Where(x => x.State == CatalogItemState.Worked || x.State == CatalogItemState.Blocked || x.State == null)
                .Where(x => x.Service.State == CatalogItemState.Worked || x.Service.State == CatalogItemState.Blocked)
                .Where(x => x.Type == AttendanceType.User)
                .Where(x => availableIdsQuery.Any(t => t == x.ID || t == x.Service.ID));

            if (!string.IsNullOrWhiteSpace(pattern))
            {
                serviceItemsQuery = serviceItemsQuery
                    .Where(
                        x => x.Service.Category.Name.ToLower().Contains(pattern)
                            || x.Service.Name.ToLower().Contains(pattern)
                            || x.Name.ToLower().Contains(pattern)
                            || x.Parameter.ToLower().Contains(pattern)
                            || (x.Service.Category.Name + " \\ " + x.Service.Name + " \\ " + x.Name).ToLower().Contains(pattern));

                serviceAttendancesQuery = serviceAttendancesQuery
                    .Where(
                        x => x.Service.Category.Name.ToLower().Contains(pattern)
                            || x.Service.Name.ToLower().Contains(pattern)
                            || x.Name.ToLower().Contains(pattern)
                            || x.Parameter.ToLower().Contains(pattern)
                            || (x.Service.Category.Name + " \\ " + x.Service.Name + " \\ " + x.Name).ToLower().Contains(pattern));
            }

            if (criteria.CallTypeId.HasValue)
            {
                serviceItemsQuery = serviceItemsQuery
                    .Where(
                        x => CallType.GetRootId(criteria.CallTypeId.Value)
                            != CallType.ChangeRequestID);
                serviceAttendancesQuery = serviceAttendancesQuery
                    .Where(
                        x => CallType.GetRootId(criteria.CallTypeId.Value)
                            == CallType.ChangeRequestID);
            }
            var result = serviceItemsQuery
                .Select(
                    s => new ObjectSearchResult
                    {
                        ID = s.ID,
                        ClassID = ObjectClass.ServiceItem,
                        FullName = s.Service.Category.Name + " \\ " + s.Service.Name + " \\ " + s.Name,
                        Details = s.Parameter
                    });
            if (criteria.CallTypeId.HasValue && criteria.CallTypeId.Value == Guid.Empty)
            {
                return result;
            }
            return result.Union(serviceAttendancesQuery
                        .Select(
                            s => new ObjectSearchResult
                            {
                                ID = s.ID,
                                ClassID = ObjectClass.ServiceAttendance,
                                FullName = s.Service.Category.Name + " \\ " + s.Service.Name + " \\ " + s.Name,
                                Details = s.Parameter
                            }));
        }
    }
}
