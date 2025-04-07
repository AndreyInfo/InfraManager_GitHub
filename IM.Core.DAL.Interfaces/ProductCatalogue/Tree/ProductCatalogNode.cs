using System;

namespace InfraManager.DAL.ProductCatalogue.Tree;

public class ProductCatalogNode
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public ObjectClass ClassID { get; init; }
    public string IconName { get; init; }
    public string Location { get; init; }
    public Guid? ParentID { get; init; }
    public bool CanContainsSubNodes { get; init; }
    public ProductTemplate? TemplateID { get; init; }
    public ObjectClass? TemplateClassID { get; init; }
    public bool IsSelectFull { get; set; }
    public bool IsSelectPart { get; set; }

}