using System;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

public class ServiceDetails : PortfolioServcieItemDetails
{
    public Guid ServiceCategoryID { get; set; }

    public ServiceType Type { get; set; }

    public string TypeName { get; set; }

    public ObjectClass? OrganizationItemClassID { get; set; }

    public Guid? OrganizationItemObjectID { get; set; }

    public byte[] RowVersion { get; set; }

    public string Note { get; set; }

    public Guid? CriticalityID { get; set; }

    public string ExternalID { get; set; }

    public ObjectClass? OrganizationItemClassIDCustomer { get; set; }

    public Guid? OrganizationItemObjectIDCustomer { get; set; }

    public string ServiceCategoryName { get; set; }

    public string OwnerName { get; set; }

    public string IconName { get; set; }
}