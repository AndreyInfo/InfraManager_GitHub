using System;

namespace InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementServiceListItem
{
    public Guid ID { get; init; }
    
    public string Name { get; init; }
    
    public CatalogItemState State { get; init; }
    
    public string OwnerName { get; init; }
    
    public string Category { get; init; }
}