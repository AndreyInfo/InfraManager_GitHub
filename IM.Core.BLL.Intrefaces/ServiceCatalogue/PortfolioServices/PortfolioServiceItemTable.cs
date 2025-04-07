using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public class PortfolioServiceItemTable
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public Guid? ServiceCategoryID { get; init; }

    public string ServiceCategoryName { get; init; }

    public string OwnerName { get; init; }

    public Guid OwnerID { get; init; }

    public ObjectClass OwnerClassID { get; init; }

    public ServiceType Type { get; init; }

    public string TypeName { get; init; }

    public CatalogItemState State { get; init; }

    public string StateName { get; init; }

    public string ExternalID { get; init; }
}
