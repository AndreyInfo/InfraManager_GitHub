using InfraManager.DAL.ServiceDesk.Calls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using InfraManager.DAL.ServiceCatalog;

namespace InfraManager.BLL.ServiceCatalogue
{
    internal class SlaApplicabilityBLL : ISlaApplicabilityBLL, ISelfRegisteredService<ISlaApplicabilityBLL>
    {
        private readonly ISLAQuery _slaQuery;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceItemAttendanceQuery _serviceItemAttendanceQuery;

        public SlaApplicabilityBLL(
                    ISLAQuery slaQuery, 
                    ICurrentUser currentUser,
                    IServiceItemAttendanceQuery serviceItemAttendanceQuery)
        {
            _slaQuery = slaQuery;
            _currentUser = currentUser;
            _serviceItemAttendanceQuery = serviceItemAttendanceQuery;
        }

        public async Task<Guid[]> AttendanceItemsAsync(Guid? userId, ObjectClass objectClass, int skip, int take, CancellationToken cancellationToken)
        {
            var slaItems = await _slaQuery.GetByUserAsync(userId ?? _currentUser.UserId, cancellationToken);
            slaItems = slaItems.Where(x => x.UtcStartDate < DateTime.UtcNow && x.UtcFinishDate > DateTime.UtcNow)
                               .ToArray();
            var slaRefList = await _slaQuery.GetReferencesAsync(cancellationToken);

            var refsList = new List<SLAReferenceItem>();
            foreach (var slaItem in slaItems)
            {
                foreach (var slaRefItem in slaRefList)
                    if (slaItem.ID == slaRefItem.ID)
                        refsList.Add(slaRefItem);
            }
            var availableIDs = refsList.Select(x => x.ObjectID)
                                       .ToHashSet();

            var attendaces = await _serviceItemAttendanceQuery.GetItemAttendacesAsync(userId ?? _currentUser.UserId, cancellationToken);

            return attendaces.Where(x => x.ObjectClass == objectClass &&
                                         ((x.ServiceID.HasValue && availableIDs.Contains(x.ServiceID.Value)) || availableIDs.Contains(x.ID)))
                             .Select(x => x.ID)
                             .Skip(skip)
                             .Take(take)
                             .ToArray();
        }
    }
}
