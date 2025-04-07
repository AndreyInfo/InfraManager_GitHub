using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public abstract class PortfolioServcieItemDetails : PortfolioServcieItemData
{
    public Guid ID { get; init; }

    public string StateName { get; init; }

    public string TagNames { get; set; }
}
