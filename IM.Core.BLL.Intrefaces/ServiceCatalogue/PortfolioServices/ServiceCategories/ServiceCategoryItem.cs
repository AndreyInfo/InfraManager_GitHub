using System;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;

public class ServiceCategoryItem : LookupListItem<Guid>
{
    public string Note { get; init; }

    public byte[] RowVersion { get; init; }

    public byte[] Icon { get; init; }

    public string IconName { get; init; }

    public string ExternalID { get; init; }

    public ServiceDetails[] Services { get; init; }
}
