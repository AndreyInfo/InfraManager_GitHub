using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public class PortfoliServiceItemSaveModel
{
    public Guid ID { get; set; }

    public Guid? ServiceID { get; init; }

    public ObjectClass ClassID { get; init; }

    public CatalogItemState? State { get; init; }

    public ServiceTag[] Tags { get; init; }

    public SupportLineResponsibleDetails[] SupportLineResponsibles { get; init; }
}
