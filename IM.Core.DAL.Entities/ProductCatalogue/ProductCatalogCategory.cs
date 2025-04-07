using System;
using System.Collections.Generic;
using Inframanager;

namespace InfraManager.DAL.ProductCatalogue;

[ObjectClassMapping(ObjectClass.ProductCatalogCategory)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ProductCatalogCategory_Insert)]
[OperationIdMapping(ObjectAction.Update, OperationID.ProductCatalogCategory_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ProductCatalogCategory_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ProductCatalogCategory_Details)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ProductCatalogCategory_Details)]
public class ProductCatalogCategory : IMarkableForDelete
{
    protected ProductCatalogCategory()
    {

    }
    public ProductCatalogCategory(string name)
    {
        ID = Guid.NewGuid();
        Name = name;
    }
    public Guid ID { get; init; }
    public string Name { get; set; }
    public Guid? ParentProductCatalogCategoryID { get; set; }
    public byte[] Icon { get; set; }
    public string IconName { get; set; }
    public bool Removed { get; private set; }
    public byte[] RowVersion { get; init; }


    public virtual ProductCatalogCategory ParentProductCatalogCategory { get; set; }
    public virtual ICollection<ProductCatalogCategory> SubCategories { get; init; }
    public virtual ICollection<ProductCatalogType> ProductCatalogTypes { get; init; }
    public void MarkForDelete()
    {
        Removed = true;
    }
}