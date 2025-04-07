using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

public class LifeCycleFilter
{
    public string SearchName { get; init; }
    public LifeCycleType[] Types { get; init; }
}