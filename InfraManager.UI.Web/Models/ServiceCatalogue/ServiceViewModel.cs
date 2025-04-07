using InfraManager.BLL.ServiceCatalogue;
using System;

namespace InfraManager.UI.Web.Models.ServiceCatalogue
{
    public class ServiceViewModel
    {
        public Guid ID { get; init; }
        public Guid ServiceCategoryID { get; init; }
        public string Name { get; init; }
        public string Note { get; set; }
        public bool IsAvailable { get; set; }
        public ServiceItemViewModel[] ServiceItemAttendanceList { get; init; }
    }
}
