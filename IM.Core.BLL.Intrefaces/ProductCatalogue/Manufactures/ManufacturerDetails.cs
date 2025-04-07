using System;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

public class ManufacturerDetails
{
    public int ID { get; init; }

    public Guid ImObjID { get; init; }
    
    public string Name { get; set; }
    
    public bool IsCable { get; init; }

    public bool IsRack { get; init; }

    public bool IsPanel { get; init; }

    public bool IsNetworkDevice { get; init; }
        
    public bool IsComputer { get; init; }

    public bool IsOutlet { get; init; }
        
    public bool IsCableCanal { get; init; }
        
    public bool IsSoftware { get; init; }

    public bool IsMaterials { get; init; }
    
    public string ExternalID { get; init; }
}