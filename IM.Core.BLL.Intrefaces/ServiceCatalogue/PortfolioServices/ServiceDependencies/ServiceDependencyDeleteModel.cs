using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceDependencies;

public class ServiceDependencyModel
{
    public Guid ParentId { get; init; }

    public Guid ChildId { get; init; }
}
