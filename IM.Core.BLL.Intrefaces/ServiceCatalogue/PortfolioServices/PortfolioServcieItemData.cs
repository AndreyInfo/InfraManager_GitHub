using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public abstract class PortfolioServcieItemData
{
    public string Name { get; init; }

    public ObjectClass ClassID { get; init; }

    public CatalogItemState? State { get; init; }

    public SupportLineResponsibleDetails[] SupportLineResponsibles { get; set; }

    public ServiceTag[] Tags { get; set; }
}