using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Linq;
using System.Text;

namespace InfraManager.DAL.ServiceCatalog;

public class ServiceModelItem
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public CatalogItemState? State { get; init; }

    public Guid ServiceCategoryID { get; set; }

    public ServiceType Type { get; init; }

    public ObjectClass? OrganizationItemClassID { get; init; }

    public Guid? OrganizationItemObjectID { get; init; }

    public byte[] RowVersion { get; init; }

    public string Note { get; init; }

    public Guid? CriticalityID { get; init; }

    public string ExternalID { get; init; }

    public ObjectClass? OrganizationItemClassIDCustomer { get; init; }

    public Guid? OrganizationItemObjectIDCustomer { get; init; }

    public string ServiceCategoryName { get; init; }

    public string OwnerName { get; init; }

    public string IconName { get; init; }

    public SupportLineResponsibleModelItem[] SupportLineResponsibles { get; init; }

    public  ServiceTag[] Tags { get; init; }

    public string TagNames  { get; init; } 
}
