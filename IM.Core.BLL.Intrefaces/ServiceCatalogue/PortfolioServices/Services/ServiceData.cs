using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

public class ServiceData : PortfolioServcieItemData
{
    public Guid ServiceCategoryID { get; init; }

    public ServiceType Type { get; init; }

    public ObjectClass? OrganizationItemClassID { get; init; }

    public Guid? OrganizationItemObjectID { get; init; }

    public byte[] RowVersion { get; init; }

    public string Note { get; init; }

    public Guid? CriticalityID { get; init; }

    public string ExternalID { get; init; }

    public ObjectClass? OrganizationItemClassIDCustomer { get; init; }

    public Guid? OrganizationItemObjectIDCustomer { get; init; }

    public string IconName { get; init; }
}
