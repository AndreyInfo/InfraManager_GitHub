using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

public class ManufacturersFilter : BaseFilter
{
    public string Name { get; init; }
    
    public bool? IsCable { get; init; }

    public bool? IsRack { get; init; }

    public bool? IsPanel { get; init; }

    public bool? IsNetworkDevice { get; init; }
        
    public bool? IsComputer { get; init; }

    public bool? IsOutlet { get; init; }
        
    public bool? IsCableCanal { get; init; }
        
    public bool? IsSoftware { get; init; }

    public bool? IsMaterials { get; init; }
}