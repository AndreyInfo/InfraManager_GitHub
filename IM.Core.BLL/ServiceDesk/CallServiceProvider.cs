using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal class CallServiceProvider : ICallServiceProvider, ISelfRegisteredService<ICallServiceProvider>
    {
        private readonly IRepository<CallService> _repository;
        private readonly IFinder<ServiceItem> _serviceItemsFinder;
        private readonly IFinder<ServiceAttendance> _serviceAttendancesFinder;

        public CallServiceProvider(
            IRepository<CallService> repository, 
            IFinder<ServiceItem> serviceItemsFinder, 
            IFinder<ServiceAttendance> serviceAttendancesFinder)
        {
            _repository = repository;
            _serviceItemsFinder = serviceItemsFinder;
            _serviceAttendancesFinder = serviceAttendancesFinder;
        }

        public async Task<CallService> GetOrCreateAsync(Guid? serviceItemOrAttendanceID, CancellationToken cancellationToken = default)
        {
            var serviceItem = await _serviceItemsFinder
                .With(x => x.Service)
                .FindAsync(serviceItemOrAttendanceID, cancellationToken);
            var serviceAttendance = await _serviceAttendancesFinder
                .With(x => x.Service)
                .FindAsync(serviceItemOrAttendanceID, cancellationToken);

            if (serviceItemOrAttendanceID.HasValue && serviceItem == null && serviceAttendance == null)
            {
                throw new InvalidObjectException(nameof(Resources.Call_ServiceItemOrAttendanceNotFound));
            }

            var itemID = serviceItem?.ID;
            var attendanceID = serviceAttendance?.ID;
            var callServiceSearchResult = await _repository
                .ToArrayAsync(
                    x => x.ServiceAttendanceID == attendanceID
                        && x.ServiceItemID == itemID,
                    cancellationToken);
            return callServiceSearchResult.FirstOrDefault()
                ?? (serviceItem == null
                    ? new CallService(serviceAttendance)
                    : new CallService(serviceItem));
        }
    }
}
