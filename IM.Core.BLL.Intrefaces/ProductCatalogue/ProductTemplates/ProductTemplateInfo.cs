using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.Classes;

public class ProductTemplateInfo
{
    public ProductTemplate ID { get; init; }
    
    public string Name { get; init; }

    public ProductTemplate? ParentID { get; init; }
    
    public ObjectClass ClassID { get; set; }
}