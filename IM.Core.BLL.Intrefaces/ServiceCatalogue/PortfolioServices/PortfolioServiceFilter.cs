using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public class PortfolioServiceFilter
{
    public Guid? ParentID { get; init; }

    public ObjectClass? ClassID { get; init; }

    public Guid? SLAID { get; init; }
}
