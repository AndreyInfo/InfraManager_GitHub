using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class CallService
    {
        public static Guid NullCallServiceID = Guid.Empty;

        protected CallService()
        {
        }

        private CallService(IServiceItem serviceItem)
        {
            ID = Guid.NewGuid();
            ServiceName = serviceItem.Service.Name;
            ServiceItemOrAttendanceName = serviceItem.Name;
            Service = serviceItem.Service;
        }

        public CallService(ServiceItem serviceItem) : this((IServiceItem)serviceItem)
        {
            ServiceItem = serviceItem;
            ServiceItemID = serviceItem.ID;
        }

        public CallService(ServiceAttendance serviceAttendance) : this((IServiceItem)serviceAttendance)
        {
            ServiceAttendance = serviceAttendance;
            ServiceAttendanceID = serviceAttendance.ID;
        }

        public Guid ID { get; private set; }
        public string ServiceName { get; private set; }
        public string ServiceItemOrAttendanceName { get; private set; }
        public Guid? ServiceID { get; private set; }
        public virtual Service Service { get; private set; }
        public Guid? ServiceItemID { get; private set; }
        public virtual ServiceItem ServiceItem { get; private set; }
        public Guid? ServiceAttendanceID { get; private set; }
        public virtual ServiceAttendance ServiceAttendance { get; private set; }
        public PortfolioServiceItemAbstract PortfolioService => 
            ServiceItem == null ? ServiceAttendance : ServiceItem;
        public bool IsNull => !ServiceItemID.HasValue && !ServiceAttendanceID.HasValue;
    }
}
