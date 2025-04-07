using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;
public class ServiceCategoryDetails : LookupDetails<Guid>
{
    public string Note { get; init; }

    public byte[] Icon { get; init; }

    public string IconName { get; init; }

    public string ExternalID { get; init; }

    public ServiceDetails[] Services { get; init; }
}