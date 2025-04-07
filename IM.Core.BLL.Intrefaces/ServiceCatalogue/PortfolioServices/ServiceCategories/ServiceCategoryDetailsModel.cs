using System;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;

public class ServiceCategoryDetailsModel
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public bool HasImage { get; init; }
    public bool IsAvailable { get; set; }
    public ServiceDetailsModel[] ServiceList { get; set; }
}
