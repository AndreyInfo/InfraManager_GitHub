using System;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;
public class ServiceCategoryData : LookupData
{
    public Guid ID { get; set; }

    public string Note { get; init; }

    public byte[] Icon { get; init; }

    public string IconName { get; init; }

    public string ExternalID { get; init; }

    public ServiceDetails[] Services { get; init; }
}