using System;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementServiceDetails
{
    public Guid ID { get; init; }
    
    public string Name { get; init; }
    
    public string StateName { get; init; }
    
    public string OwnerName { get; init; }
    
    public string Category { get; init; }
}