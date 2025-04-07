namespace InfraManager.BLL.ProductCatalogue.ProductClass;

public class AdditionalFields
{
    public bool HasParameters { get; set; }
    
    public bool HasSlotType { get; set; }
    
    public bool CanBuy { get; set; }
    
    public bool HasPort { get; set; }
    public bool HasTechnology { get; set; }
    public bool HasUnits { get; set; }
    public bool HasGost { get; set; }
    public bool HasPrice { get; set; }
    public bool CanUpdate { get; set; }
    public bool IsRackMount { get; set; }
    public bool HasHeight { get; set; }
    public bool HasWidthHeightDepth { get; set; }
}