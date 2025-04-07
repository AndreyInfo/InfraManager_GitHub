using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

public class ServiceItemData : PortfolioServcieItemData
{
    public Guid ServiceID { get; init; }

    public byte[] RowVersion { get; init; }

    public string Parameter { get; init; }

    public string Note { get; init; }

    public string ExternalID { get; init; }

    public Guid CategoryID { get; init; }

    public Guid? FormID { get; init; }
}
