using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories
{
    public class ServiceCategoryFilter
    {
        public Guid? CategoryID { get; set; }
        public Guid? ServiceID { get; set; }
        public Guid? ServiceItemAttendanceID { get; set; }
        public Guid? UserID { get; set; }
        public bool AvailableOnly { get; set; }
        public ServiceType[] ServiceTypes { get; set; }
    }
}
