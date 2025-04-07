using System;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

public class ServiceDetailsModel
{
    public Guid ID { get; init; }
    public Guid ServiceCategoryID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public bool IsAvailable { get; set; }
    public ServiceItemDetailsModel[] ServiceItemAttendanceList { get; set; }
    public int? OrganizationItemClassID { get; set; }
    public Guid? OrganizationItemObjectID { get; set; }
    public bool IsAllItemsAvailableByDefault { get; set; }
}
