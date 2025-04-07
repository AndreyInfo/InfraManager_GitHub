using InfraManager.DAL.ProductCatalogue.Tree;
using System;

namespace InfraManager.BLL.ProductCatalogue.Models;
public class ProductCatalogModelDeleteFilter
{
    public Guid? TypeID { get; set; }
    public Guid? CategoryID { get; set; }

    public ProductCatalogTreeFilter ToTreeFilter()
        => new ()
        { 
            ParentID = TypeID,
            AvailableCategoryID = CategoryID,
        };
}
